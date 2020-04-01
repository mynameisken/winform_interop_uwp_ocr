using System;
using System.Collections.Generic;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using Windows.Media.Ocr;
using Windows.Graphics.Imaging;
using Windows.Storage.Streams;
using Windows.ApplicationModel.DataTransfer;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using Windows.Globalization;
using Windows.System.UserProfile;

namespace Winform_Interop_UWP_OCR
{
    // Made by mynameisken(高剑(Gao Jian) in Chinese)
    // Inter-Operation between Winfrom and UWP is not an easy job.
    // This program bridges them together, so Winfrom can share UWP power as well.
    public partial class FormMain : Form
    {
        //Ocr engine from UWP sdk packages
        private OcrEngine ocr;
        public FormMain()
        {
            InitializeComponent();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            SetLayout();

            // Init OCR engine
            var topUserLanguage = GlobalizationPreferences.Languages[0];
            var language = new Language(topUserLanguage);
            var displayName = language.DisplayName;
            ocr = OcrEngine.TryCreateFromLanguage(language);

            // Regiter Clipboard event handler
            Windows.ApplicationModel.DataTransfer.Clipboard.Clear();
            Windows.ApplicationModel.DataTransfer.Clipboard.ContentChanged += Clipboard_ContentChanged;
        }

        // Get image data in Clipboard
        private async void Clipboard_ContentChanged(object sender, object e)
        {
            DataPackageView package = Windows.ApplicationModel.DataTransfer.Clipboard.GetContent();
            if (package.Contains(StandardDataFormats.Bitmap))
            {
                RandomAccessStreamReference img = await package.GetBitmapAsync();
                var imgstream = await img.OpenReadAsync();
                BitmapDecoder decoder = await BitmapDecoder.CreateAsync(imgstream);
                SoftwareBitmap soft = await decoder.GetSoftwareBitmapAsync();
                OcrResult result = await ocr.RecognizeAsync(soft);

                Bitmap clip = Soft2Bitmap(soft);
                boxClip.Image = clip;

                Bitmap bmp = new Bitmap(clip.Width, clip.Height);
                Graphics g = Graphics.FromImage(bmp);
                g.SmoothingMode = SmoothingMode.HighQuality;
                foreach (var line in result.Lines)
                {
                    foreach (var word in line.Words)
                    {
                        Windows.Foundation.Rect r = word.BoundingRect;
                        g.DrawRectangle(Pens.LightSkyBlue, (float)r.X, (float)r.Y, (float)r.Width, (float)r.Height);
                        g.DrawString(word.Text, new Font("宋体", 9f), Brushes.Violet, (float)r.X, (float)r.Y);
                    }
                }
                g.Dispose();

                boxOCR.Image = bmp;
            }
        }
        
        // Convert UWP SoftwareBitmap to Bitmap
        private unsafe Bitmap Soft2Bitmap(SoftwareBitmap soft)
        {
            int w = soft.PixelWidth;
            int h = soft.PixelHeight;

            //int stride = (w + 4 - (w % 4)) * 4;
            int stride = w * 4;
            int size = stride * h;
            Windows.Storage.Streams.Buffer buf = new Windows.Storage.Streams.Buffer((uint)size);
            soft.CopyToBuffer(buf);
            byte[] data = new byte[size];
            buf.CopyTo(data);

            Bitmap bmp = new Bitmap(w, h);
            BitmapData bdata = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.WriteOnly, bmp.PixelFormat);
            IntPtr ptr = bdata.Scan0;
            Marshal.Copy(data, 0, ptr, size);
            bmp.UnlockBits(bdata);

            return bmp;
        }

        // Convert Bitmap to UWP SoftwareBitmap
        private unsafe SoftwareBitmap Bitmap2Soft(Bitmap bmp)
        {
            int w = bmp.Width;
            int h = bmp.Height;
            
            BitmapData bdata = bmp.LockBits(new Rectangle(0, 0, w, h), ImageLockMode.ReadOnly, bmp.PixelFormat);
            IntPtr ptr = bdata.Scan0;
            int size = bdata.Stride * bdata.Height;
            byte[] data = new byte[size];
            Marshal.Copy(ptr, data, 0, size);
            bmp.UnlockBits(bdata);

            IBuffer buf = data.AsBuffer();

            SoftwareBitmap soft = new SoftwareBitmap(BitmapPixelFormat.Bgra8, w, h);
            soft.CopyFromBuffer(buf);

            return soft;
        }

        [ComImport]
        [Guid("5B0D3235-4DBA-4D44-865E-8F1D0E4FD04D")]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        unsafe interface IMemoryBufferByteAccess
        {
            void GetBuffer(out byte* buffer, out uint capacity);
        }

        private unsafe void AccessPixel(SoftwareBitmap softwareBitmap)
        {
            using (BitmapBuffer buffer = softwareBitmap.LockBuffer(BitmapBufferAccessMode.Write))
            {
                using (var reference = buffer.CreateReference())
                {
                    byte* dataInBytes;
                    uint capacity;
                    ((IMemoryBufferByteAccess)reference).GetBuffer(out dataInBytes, out capacity);

                    // Fill-in the BGRA plane
                    BitmapPlaneDescription bufferLayout = buffer.GetPlaneDescription(0);
                    for (int i = 0; i < bufferLayout.Height; i++)
                    {
                        for (int j = 0; j < bufferLayout.Width; j++)
                        {

                            byte value = (byte)((float)j / bufferLayout.Width * 255);
                            dataInBytes[bufferLayout.StartIndex + bufferLayout.Stride * i + 4 * j + 0] = value;
                            dataInBytes[bufferLayout.StartIndex + bufferLayout.Stride * i + 4 * j + 1] = value;
                            dataInBytes[bufferLayout.StartIndex + bufferLayout.Stride * i + 4 * j + 2] = value;
                            dataInBytes[bufferLayout.StartIndex + bufferLayout.Stride * i + 4 * j + 3] = (byte)255;
                        }
                    }
                }
            }
        }

        // Get a resized Bitmap
        private Bitmap ResizeBitmap(Bitmap bmp, int w, int h)
        {
            Bitmap result = new Bitmap(w, h);
            Graphics g = Graphics.FromImage(result);
            g.DrawImage(bmp, 0, 0, w, h);
            g.Dispose();
            g = null;
            return result;
        }

        private void FormMain_SizeChanged(object sender, EventArgs e)
        {
            SetLayout();
        }

        // Refresh Layout
        private void SetLayout()
        {
            Rectangle rect = this.DisplayRectangle;

            boxClip.Width = rect.Width / 2 - 5;
        }

        // Change PictureBox.SizeMode
        private void boxClip_Click(object sender, EventArgs e)
        {
            if (boxClip.SizeMode == PictureBoxSizeMode.CenterImage)
            {
                boxClip.SizeMode = PictureBoxSizeMode.Zoom;
                boxOCR.SizeMode = PictureBoxSizeMode.Zoom;
                return;
            }

            if (boxClip.SizeMode == PictureBoxSizeMode.Zoom)
            {
                boxClip.SizeMode = PictureBoxSizeMode.CenterImage;
                boxOCR.SizeMode = PictureBoxSizeMode.CenterImage;
                return;
            }
        }

        // Change PictureBox.SizeMode
        private void boxOCR_Click(object sender, EventArgs e)
        {
            if (boxOCR.SizeMode == PictureBoxSizeMode.CenterImage)
            {
                boxClip.SizeMode = PictureBoxSizeMode.Zoom;
                boxOCR.SizeMode = PictureBoxSizeMode.Zoom;
                return;
            }

            if (boxOCR.SizeMode == PictureBoxSizeMode.Zoom)
            {
                boxClip.SizeMode = PictureBoxSizeMode.CenterImage;
                boxOCR.SizeMode = PictureBoxSizeMode.CenterImage;
                return;
            }
        }
    }
}