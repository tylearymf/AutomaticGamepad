using System.Drawing;
using System.Drawing.Imaging;

namespace AutomaticGamepad
{
    public static class ImageExtensions
    {
        static ColorMatrix grayMatrix = new ColorMatrix(new float[][]
        {
            new float[] { 0.299F, 0.299F, 0.299F, 0, 0 }, // red
            new float[] { 0.587F, 0.587F, 0.587F, 0, 0 }, // green
            new float[] { 0.114F, 0.114F, 0.114F, 0, 0 }, // blue
            new float[] { 0, 0, 0, 0.5F, 0 }, // alpha
            new float[] { 0, 0, 0, 0, 1 } // three translations
        });


        public static Bitmap ToGrayScale(this Image source)
        {
            if (source == null)
                return null;

            var grayImage = new Bitmap(source.Width, source.Height, source.PixelFormat);
            grayImage.SetResolution(source.HorizontalResolution, source.VerticalResolution);

            using (var g = Graphics.FromImage(grayImage))
            using (var attributes = new ImageAttributes())
            {
                attributes.SetColorMatrix(grayMatrix);
                g.DrawImage(source, new Rectangle(0, 0, source.Width, source.Height),
                            0, 0, source.Width, source.Height, GraphicsUnit.Pixel, attributes);
            }

            return grayImage;
        }
    }
}
