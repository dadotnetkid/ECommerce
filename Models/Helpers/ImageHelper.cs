using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Helpers
{
    public class ImageHelper
    {
        public static Image ResizeImage(Image imgPhoto, int width, int height)
        {
            int sourceWidth = imgPhoto.Width;
            int sourceHeight = imgPhoto.Height;
            int sourceX = 0;
            int sourceY = 0;
            int destX = 0;
            int destY = 0;

            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;

            nPercentW = ((float) width / (float) sourceWidth);
            nPercentH = ((float) height / (float) sourceHeight);
            if (nPercentH < nPercentW)
            {
                nPercent = nPercentH;
                destX = System.Convert.ToInt16((width -
                                                (sourceWidth * nPercent)) / 2);
            }
            else
            {
                nPercent = nPercentW;
                destY = System.Convert.ToInt16((height -
                                                (sourceHeight * nPercent)) / 2);
            }

            int destWidth = (int) (sourceWidth * nPercent);
            int destHeight = (int) (sourceHeight * nPercent);

            Bitmap bmPhoto = new Bitmap(width, height,
                PixelFormat.Format24bppRgb);
            bmPhoto.SetResolution(imgPhoto.HorizontalResolution,
                imgPhoto.VerticalResolution);

            Graphics grPhoto = Graphics.FromImage(bmPhoto);
            grPhoto.Clear(Color.Red);
            grPhoto.InterpolationMode =
                InterpolationMode.HighQualityBicubic;

            grPhoto.DrawImage(imgPhoto,
                new Rectangle(destX, destY, destWidth, destHeight),
                new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
                GraphicsUnit.Pixel);

            grPhoto.Dispose();
            return bmPhoto;
        }

        public static string ImageToBase64(Image imgPhoto, int width, int height)
        {
            var img = ResizeImage(imgPhoto, width, height);
            MemoryStream ms = new MemoryStream();
            img.Save(ms, ImageFormat.Png);
            return $"data:image/png;base64,{Convert.ToBase64String(ms.ToArray())}";
        }

        public static string ImageToBase64(string path, int width, int height)
        {
            
            if (!File.Exists(path))
            {
                path = HttpContext.Current.Server.MapPath(
                    $"~/content/images/User-Images/default-img.png");
            }
            var imgPhoto = Image.FromFile(path);
            var img = ResizeImage(imgPhoto, width, height);
            MemoryStream ms = new MemoryStream();
            img.Save(ms, ImageFormat.Bmp);
            return $"data:image/jpeg;base64,{Convert.ToBase64String(ms.ToArray())}";
        }
    }
}