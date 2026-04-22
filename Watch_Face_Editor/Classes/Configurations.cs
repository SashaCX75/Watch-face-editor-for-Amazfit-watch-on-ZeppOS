using System;
using Newtonsoft.Json;
using System.IO;
using System.Collections.Generic;
using static Newtonsoft.Json.JsonConvert;
//using Newtonsoft.Json.Linq;

namespace Watch_Face_Editor.Classes
{
    public struct PlatformBackground
    {
        public int w;
        public int h;

        public PlatformBackground(int iWidth, int iHeight)
        {
            w = iWidth;
            h = iHeight;
        }

        public override string ToString() => $"(Width:{w}, Height: {h})";

    }
    public struct Scaling
    {
        public PlatformBackground scaling_0_5;

        // Размер pictureBox при масштабе 1.0
        public PlatformBackground scaling_1_0;

        // Размер pictureBox при масштабе 1.5
        public PlatformBackground scaling_1_5;

        // Размер pictureBox при масштабе 2.0
        public PlatformBackground scaling_2_0;

        // Размер pictureBox при масштабе 2.5
        public PlatformBackground scaling_2_5;

    }

    public class AmazfitPlatform
    {
        // Название
        public string name;
        // Нужное
        public int designWidth;
        public int[] deviceSource_ids;

        // тип процесора
        public string cpuPlatform = "MHS";
        // формат экрана
        public string screenType = "round";

        // Размеры фона
        public PlatformBackground background;

        // настройки для масштабирования предпросмотра
        public Scaling scaling;

        // Маска
        public string maskImage;

        // Размер preview
        public int previewHeight;

        // путь к скину часов
        public string watchSkin;

        // тип кодирования изображений
        public int colorScheme;

        // необходимость корректировки ширины изображения
        public bool fixSize;

        public float versionOS;

        public override string ToString() =>
        $"Name: {name}; designWidth: {designWidth}; Background: w: {background.w}, h: {background.h}; versionOS: {versionOS};";

    }


    public class Configurations
    {
        //public Dictionary<string, AmazfitPlatform> AvaialblePlatforms { get; set; }

        public static Dictionary<string, AmazfitPlatform> LoadFromFile(string Filename = @"\model_config\configurations.json")
        {
            Dictionary<string, AmazfitPlatform> result;
            try
            {

                result = new Dictionary<string, AmazfitPlatform>();

                if (File.Exists(Filename))
                {
                    result = DeserializeObject<Dictionary<string, AmazfitPlatform>>
                    (File.ReadAllText(Filename), new JsonSerializerSettings
                    {
                        DefaultValueHandling = DefaultValueHandling.Ignore,
                        NullValueHandling = NullValueHandling.Ignore
                    });
                    return result;
                }
                else
                {
                    Logger.WriteLine("No file for device config:" + Filename);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLine("Error reading model configurations:" + ex);
            }
            return null;
        }
    }
}
