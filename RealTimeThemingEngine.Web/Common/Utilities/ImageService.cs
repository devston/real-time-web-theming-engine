using RealTimeThemingEngine.Web.Common.Interfaces;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace RealTimeThemingEngine.Web.Common.Utilities
{
    public class ImageService : IImageService
    {
        public ImageService()
        {

        }

        // Check image extension.
        public bool IsImageExtension(string extension)
        {
            switch (extension.ToLower())
            {
                case ".jpg":
                case ".jpeg":
                case ".png":
                case ".bmp":
                    return true;
            }

            return false;
        }

        // Resize image.
        public bool ResizeImage(string sourceFile, string targetFile, int maxWidth, int maxHeight, int quality)
        {
            Image sourceImage;

            try
            {
                sourceImage = Image.FromFile(sourceFile);
            }
            catch (OutOfMemoryException)
            {
                // This is not a valid image. Do nothing.
                return false;
            }

            // If 0 is passed in any of the max sizes it means that that size must be ignored,
            // so the original image size is used.
            maxWidth = maxWidth == 0 ? sourceImage.Width : maxWidth;
            maxHeight = maxHeight == 0 ? sourceImage.Height : maxHeight;

            if (sourceImage.Width <= maxWidth && sourceImage.Height <= maxHeight)
            {
                sourceImage.Dispose();

                if (sourceFile != targetFile)
                {
                    File.Copy(sourceFile, targetFile);
                }

                return true;
            }

            // Gets the best size for aspect ratio resampling
            Size oSize = GetAspectRatioSize(maxWidth, maxHeight, sourceImage.Width, sourceImage.Height);

            Image oResampled;

            if (sourceImage.PixelFormat == PixelFormat.Indexed || sourceImage.PixelFormat == PixelFormat.Format1bppIndexed || sourceImage.PixelFormat == PixelFormat.Format4bppIndexed || sourceImage.PixelFormat == PixelFormat.Format8bppIndexed)
            {
                oResampled = new Bitmap(oSize.Width, oSize.Height, PixelFormat.Format24bppRgb);
            }
            else
            {
                oResampled = new Bitmap(oSize.Width, oSize.Height, sourceImage.PixelFormat);
            }

            // Creates a Graphics for the oResampled image
            var oGraphics = Graphics.FromImage(oResampled);

            // The Rectangle that holds the Resampled image size
            Rectangle oRectangle;

            // High quality resizing
            if (quality > 80)
            {
                oGraphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

                // If HighQualityBicubic is used, bigger Rectangle is required to remove the white border
                oRectangle = new Rectangle(-1, -1, oSize.Width + 1, oSize.Height + 1);
            }
            else
            {
                oRectangle = new Rectangle(0, 0, oSize.Width, oSize.Height);
            }

            // Draws over the oResampled image the resampled Image
            oGraphics.DrawImage(sourceImage, oRectangle);

            sourceImage.Dispose();

            string extension = Path.GetExtension(targetFile).ToLower();

            if (extension == ".jpg" || extension == ".jpeg")
            {
                ImageCodecInfo oCodec = GetJpgCodec();

                if (oCodec != null)
                {
                    var aCodecParams = new EncoderParameters(1);
                    aCodecParams.Param[0] = new EncoderParameter(Encoder.Quality, quality);

                    oResampled.Save(targetFile, oCodec, aCodecParams);
                }
                else
                {
                    oResampled.Save(targetFile);
                }
            }
            else
            {
                switch (extension)
                {
                    case ".png":
                        oResampled.Save(targetFile, ImageFormat.Png);
                        break;

                    case ".bmp":
                        oResampled.Save(targetFile, ImageFormat.Bmp);
                        break;
                }
            }
            oGraphics.Dispose();
            oResampled.Dispose();

            return true;
        }

        // Get image aspect ratio.
        private Size GetAspectRatioSize(int maxWidth, int maxHeight, int actualWidth, int actualHeight)
        {
            // Creates the Size object to be returned
            var oSize = new Size(maxWidth, maxHeight);

            // Calculates the X and Y resize factors
            float iFactorX = (float)maxWidth / (float)actualWidth;
            float iFactorY = (float)maxHeight / (float)actualHeight;

            // If some dimension have to be scaled
            if (iFactorX != 1 || iFactorY != 1)
            {
                // Uses the lower Factor to scale the opposite size
                if (iFactorX < iFactorY)
                {
                    oSize.Height = (int)Math.Round((float)actualHeight * iFactorX);
                }
                else if (iFactorX > iFactorY)
                {
                    oSize.Width = (int)Math.Round((float)actualWidth * iFactorY);
                }
            }

            if (oSize.Height <= 0)
            {
                oSize.Height = 1;
            }

            if (oSize.Width <= 0)
            {
                oSize.Width = 1;
            }

            // Returns the Size
            return oSize;
        }

        // Get jpg codec.
        private ImageCodecInfo GetJpgCodec()
        {
            var aCodecs = ImageCodecInfo.GetImageEncoders();
            ImageCodecInfo oCodec = null;

            for (int i = 0; i < aCodecs.Length; i++)
            {
                if (aCodecs[i].MimeType.Equals("image/jpeg"))
                {
                    oCodec = aCodecs[i];
                    break;
                }
            }

            return oCodec;
        }
    }
}