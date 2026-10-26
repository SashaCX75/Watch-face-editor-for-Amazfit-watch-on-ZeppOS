using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace SplashScreen
{
    // Experimental dark mode (local branch experimento/dark-mode).
    // The splash starts before the editor reads its settings, so it checks Settings.json itself.
    public partial class SplashScreen
    {
        private Image[] darkFrames;
        private int darkIndex;

        private void DarkSplash_Apply()
        {
            try
            {
                string json = DarkSplash_Settings();
                if (json == null || !Regex.IsMatch(json, "\"DarkMode\"\\s*:\\s*true", RegexOptions.IgnoreCase)) return;
                // One dark background for both accents (the Zepp OS logo keeps its original colors);
                // the spinning ring follows the accent: blue (default, as in the light theme) or orange.
                bool orange = Regex.IsMatch(json, "\"DarkAccent\"\\s*:\\s*\"orange\"", RegexOptions.IgnoreCase);
                string ring = orange ? "" : "blue/";

                Image background = DarkSplash_Load("fundo_649x449.png");
                if (background == null) return;

                List<Image> frames = new List<Image>();
                for (int i = 1; i <= 24; i++)
                {
                    Image f = DarkSplash_Load(ring + "loader_flash_" + i.ToString("00") + ".png");
                    if (f == null) { frames = null; break; }
                    frames.Add(f);
                }

                pictureBox_SplashScreen.BackgroundImage = background;
                if (frames != null && frames.Count > 0)
                {
                    darkFrames = frames.ToArray();
                    darkIndex = 0;
                    timer1.Tick -= timer1_Tick;
                    timer1.Tick += DarkSplash_Tick;
                    pictureBox_SplashScreen.Image = darkFrames[0];
                }
            }
            catch { /* the splash must never fail: keep the original images */ }
        }

        private void DarkSplash_Tick(object sender, EventArgs e)
        {
            darkIndex++;
            if (darkIndex >= darkFrames.Length) darkIndex = 0;
            pictureBox_SplashScreen.Image = darkFrames[darkIndex];
        }

        // SplashScreen.exe lives in Libs\; Settings.json is next to the editor exe (one level up).
        private static string DarkSplash_Settings()
        {
            string dir = Path.GetDirectoryName(Application.ExecutablePath);
            string[] candidates =
            {
                Path.Combine(Path.GetDirectoryName(dir) ?? dir, "Settings.json"),
                Path.Combine(dir, "Settings.json")
            };
            foreach (string path in candidates)
            {
                if (!File.Exists(path)) continue;
                return File.ReadAllText(path);
            }
            return null;
        }

        private static Image DarkSplash_Load(string name)
        {
            Assembly asm = typeof(SplashScreen).Assembly;
            string want = "DarkSplash/" + name;
            string found = null;
            foreach (string n in asm.GetManifestResourceNames())   // RecursiveDir may use '\\'
                if (string.Equals(n.Replace('\\', '/'), want, StringComparison.OrdinalIgnoreCase)) { found = n; break; }
            if (found == null) return null;
            using (Stream s = asm.GetManifestResourceStream(found))
            {
                if (s == null) return null;
                using (Image tmp = Image.FromStream(s))
                    return new Bitmap(tmp); // detached copy: the stream can be closed
            }
        }
    }
}
