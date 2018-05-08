using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeThemingEngine.Web.Common.Interfaces
{
    public interface IImageService
    {
        /// <summary>
        /// Check if the string is an image extension. (Include the . at the beginning of the extension e.g. .jpg)
        /// </summary>
        /// <param name="extension"></param>
        /// <returns></returns>
        bool IsImageExtension(string extension);
        /// <summary>
        /// Reencode and resize an image.
        /// </summary>
        /// <param name="sourceFile">Path of the image to resize</param>
        /// <param name="targetFile">Path of where to put the new image, including the image name</param>
        /// <param name="maxWidth">Maximum width to resize the image</param>
        /// <param name="maxHeight">Maximum height to resize the image</param>
        /// <param name="quality">Quality of the image to encode, 0 = worst and 100 = best</param>
        /// <returns></returns>
        bool ResizeImage(string sourceFile, string targetFile, int maxWidth, int maxHeight, int quality);
    }
}
