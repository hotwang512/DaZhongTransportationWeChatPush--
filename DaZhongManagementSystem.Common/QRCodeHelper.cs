using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Text;
using System.Web;
using ZXing;
using ZXing.Common;
using ZXing.QrCode;
using ZXing.QrCode.Internal;


namespace DaZhongManagementSystem.Common
{
    public class QRCodeHelper
    {
        /// <summary>
        /// 生成二维码
        /// </summary>
        /// <param name="content">二维码内容</param>
        /// <param name="logoPath">中间logo的路径</param>
        /// <param name="fileName">生成的二维码保存路径</param>
        /// <returns>生成的二维码</returns>
        public static Bitmap GenerateQRCode(string content, string logoPath, string folder, string fileName)
        {
            BarcodeWriter writer = new BarcodeWriter();
            writer.Format = BarcodeFormat.QR_CODE;
            QrCodeEncodingOptions options = new QrCodeEncodingOptions()
            {
                CharacterSet = "UTF-8",
                ErrorCorrection = ErrorCorrectionLevel.H,
                Margin = 0,
                DisableECI = true,
                Width = 0x120,
                Height = 0x120
            };
            writer.Options = options;
            //var matrix = writer.Encode(content);//黑白二维码

            //var  matrix1 = deleteWhite(matrix);//删除白边
            Bitmap image = writer.Write(content);
            Bitmap bitmap2 = new Bitmap(image.Width, image.Height, PixelFormat.Format32bppArgb);
            Graphics graphics = Graphics.FromImage(bitmap2);
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            graphics.DrawImage(image, 0, 0);
            image.Dispose();
            AddLogo(logoPath, bitmap2);

            Bitmap bitmap3 = QrCodeVertical(bitmap2.Width, bitmap2.Height);//彩色渐变二维码
            SetPixels(content, bitmap2, bitmap3);
            bitmap3.Dispose();
            try
            {
                string path = HttpContext.Current.Server.MapPath(folder);
                if (!System.IO.Directory.Exists(path))
                {
                    System.IO.Directory.CreateDirectory(path);
                }
                fileName = System.IO.Path.Combine(path, fileName);
                bitmap2.Save(fileName);
            }
            catch (Exception ex)
            { }
            return bitmap2;
        }
        private static BitMatrix deleteWhite(BitMatrix matrix)
        {
            int[] rec = matrix.getEnclosingRectangle();
            int resWidth = rec[2] + 1;
            int resHeight = rec[3] + 1;
            BitMatrix resMatrix = new BitMatrix(resWidth, resHeight);
            resMatrix.clear();
            for (int i = 0; i < resWidth; i++)
            {
                for (int j = 0; j < resHeight; j++)
                {
                    if (matrix[i + rec[0], j + rec[1]])
                        resMatrix.flip(i + 1, j + 1);
                }
            }
            return resMatrix;
        }
        /// <summary>
        /// 添加Logo
        /// </summary>
        /// <param name="logoPath"></param>
        /// <param name="originBitmap"></param>
        private static void AddLogo(string logoPath, Bitmap originBitmap)
        {
            var logo = Image.FromFile(HttpContext.Current.Server.MapPath(logoPath));
            Graphics g = Graphics.FromImage(originBitmap);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle logoRec = new Rectangle();
            logoRec.Width = originBitmap.Width / 6;
            logoRec.Height = originBitmap.Height / 6;
            logoRec.X = originBitmap.Width / 2 - logoRec.Width / 2;
            logoRec.Y = originBitmap.Height / 2 - logoRec.Height / 2;
            g.DrawImage(logo, logoRec);
            logo.Dispose();
            g.Dispose();
        }

        /// <summary>
        /// 设置二维码图片颜色
        /// </summary>
        /// <param name="width"></param>
        /// <param name="heigth"></param>
        /// <returns></returns>
        private static Bitmap QrCodeVertical(int width, int heigth)
        {
            var image = new Bitmap(width, heigth, PixelFormat.Format32bppArgb);
            var rect = new Rectangle(0, 0, width, heigth);
            var brush = new LinearGradientBrush(rect, Color.FromArgb(230, 0x23, 0xa9, 160), Color.FromArgb(0xff, 8, 60, 0x63), LinearGradientMode.Vertical);
            Graphics graphics = Graphics.FromImage(image);
            graphics.FillRectangle(brush, rect);
            graphics.Dispose();
            return image;
        }

        /// <summary>
        /// 设置二维码四个角的颜色
        /// </summary>
        /// <param name="content"></param>
        /// <param name="originBitmap"></param>
        /// <param name="destImage"></param>
        private static void SetPixels(string content, Bitmap originBitmap, Bitmap destImage)
        {
            Color color = Color.FromArgb(200, 224, 114, 1);
            int num = 140;
            if (Encoding.UTF8.GetBytes(content).Length >= 20)
            {
                num -= (Encoding.UTF8.GetBytes(content).Length - 20) / 2;
            }
            int num2 = num;
            int num3 = num2;
            for (int i = 0; i < originBitmap.Width; i++)
            {
                for (int j = 0; j < originBitmap.Height; j++)
                {
                    Color color3;
                    Color pixel = originBitmap.GetPixel(i, j);
                    if ((i < num2) && (j < num3))
                    {
                        color3 = ((pixel.A == 0xff) && (pixel.B == 0)) ? color : pixel;
                    }
                    else
                    {
                        color3 = ((pixel.A == 0xff) && (pixel.B == 0)) ? destImage.GetPixel(i, j) : pixel;
                    }
                    originBitmap.SetPixel(i, j, color3);
                }
            }
        }
    }
}