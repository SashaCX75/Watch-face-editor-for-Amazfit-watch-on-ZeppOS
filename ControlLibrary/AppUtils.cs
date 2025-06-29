using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ControlLibrary
{
    public class AppUtils
    {
        //[DllImport("user32.dll")]
        //static extern int GetSysColor(int nIndex);

        public static bool IsLightTheme()
        {
            bool result_1 = true;
            bool result_2 = true;
            //const string keyPath = @"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize";
            //using (RegistryKey key = Registry.CurrentUser.OpenSubKey(keyPath))
            //{
            //    if (key != null)
            //    {
            //        object value = key.GetValue("AppsUseLightTheme");
            //        if (value is int intValue)
            //        {
            //            return intValue != 0; // true = светлая тема
            //        }
            //    }
            //}
            //// Если ключ не найден — считаем, что светлая (по умолчанию)
            //return true;


            //const int COLOR_WINDOW = 5;
            //int colorRef = GetSysColor(COLOR_WINDOW);
            //int r = colorRef & 0xFF;
            //int g = (colorRef >> 8) & 0xFF;
            //int b = (colorRef >> 16) & 0xFF;
            //int brightness = (r + g + b) / 3;

            //return brightness > 128; // Примерная оценка

            const string keyPath = @"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize";
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(keyPath))
            {
                if (key != null)
                {
                    object value = key.GetValue("AppsUseLightTheme");
                    if (value is int intValue)
                    {
                        result_1 = intValue != 0; // true = светлая тема
                    }
                }
            }

            Color controlColor = SystemColors.Control;
            int brightness = (controlColor.R + controlColor.G + controlColor.B) / 3;
            result_2 = brightness > 128;

            return result_1 || result_2;
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
