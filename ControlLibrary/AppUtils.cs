using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlLibrary
{
    public class AppUtils
    {
        public static bool IsLightTheme()
        {
            const string keyPath = @"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize";
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(keyPath))
            {
                if (key != null)
                {
                    object value = key.GetValue("AppsUseLightTheme");
                    if (value is int intValue)
                    {
                        return intValue != 0; // true = светлая тема
                    }
                }
            }
            // Если ключ не найден — считаем, что светлая (по умолчанию)
            return true;
        }

        public static Image InvertColors(Image image)
        {
            var colorMatrix = new System.Drawing.Imaging.ColorMatrix(new float[][]
            {
                new float[] {-1, 0, 0, 0, 0},
                new float[] {0, -1, 0, 0, 0},
                new float[] {0, 0, -1, 0, 0},
                new float[] {0, 0, 0, 1, 0},
                new float[] {1, 1, 1, 0, 1}
            });

            var attributes = new System.Drawing.Imaging.ImageAttributes();
            attributes.SetColorMatrix(colorMatrix);

            var result = new Bitmap(image.Width, image.Height);
            using (var g = Graphics.FromImage(result))
            {
                g.DrawImage(image, new Rectangle(0, 0, image.Width, image.Height),
                    0, 0, image.Width, image.Height, GraphicsUnit.Pixel, attributes);
            }

            return result;
        }
    }
}
