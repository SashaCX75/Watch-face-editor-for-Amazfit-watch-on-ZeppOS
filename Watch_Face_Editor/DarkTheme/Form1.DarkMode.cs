using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Windows.Forms;

namespace Watch_Face_Editor
{
    // Optional dark mode. Kept in its own partial file so Form1.cs only gains two calls.
    //  - light/dark toggle: small sun/moon button next to the version number (top right);
    //  - dark theme choice (blue / orange): Settings tab, right below "Language".
    public partial class Form1
    {
        private ThemeToggle button_ThemeToggle;
        private ComboBox comboBox_DarkTheme;
        private Label label_DarkTheme;

        /// <summary>Called right after InitializeComponent.</summary>
        private void DarkMode_Init()
        {
            try { DarkMode_CreateToggle(); }
            catch (Exception ex) { Logger.WriteLine("DarkMode_CreateToggle error: " + ex); }
            try { DarkMode_CreateThemeChoice(); }
            catch (Exception ex) { Logger.WriteLine("DarkMode_CreateThemeChoice error: " + ex); }
            DarkMode_AboutImage();
            DarkTheme.ThemeManager.Apply(this);
        }

        // "Sobre" tab: dark QR icon, set in code so Form1.resx (translations) stays untouched.
        // Normal mode keeps the original picture from Form1.resx.
        private void DarkMode_AboutImage()
        {
            if (!DarkTheme.ThemeManager.Enabled) return;
            try
            {
                // Small QR icon next to "BuyMeaCoffee": same dark icon as button_DownloadQR.
                Image qr = DarkTheme.ThemeManager.LoadEmbedded("dark/form1/button_DownloadQR.png");
                if (qr != null && pictureBox_qr != null) pictureBox_qr.BackgroundImage = qr;
            }
            catch (Exception ex) { Logger.WriteLine("DarkMode_AboutImage error: " + ex); }
        }

        // ---------------------------------------------------------------- light / dark toggle

        private void DarkMode_CreateToggle()
        {
            Label anchor = label_version; // "v 18.2", top right
            if (anchor == null || anchor.Parent == null) return;

            button_ThemeToggle = new ThemeToggle(DarkTheme.ThemeManager.Enabled);
            button_ThemeToggle.Name = "button_ThemeToggle";
            int size = (int)Math.Round(16 * currentDPI);
            button_ThemeToggle.Size = new Size(size, size);
            button_ThemeToggle.Anchor = anchor.Anchor;
            PlaceToggle();
            anchor.LocationChanged += (s, e) => PlaceToggle();

            new ToolTip().SetToolTip(button_ThemeToggle, DarkMode_Caption());
            button_ThemeToggle.Click += button_ThemeToggle_Click;
            anchor.Parent.Controls.Add(button_ThemeToggle);
            button_ThemeToggle.BringToFront();
        }

        private void PlaceToggle()
        {
            if (button_ThemeToggle == null) return;
            int gap = (int)Math.Round(3 * currentDPI);
            int x = label_version.Left - button_ThemeToggle.Width - gap;
            int y = label_version.Top + label_version.Height / 2 - button_ThemeToggle.Height / 2;
            button_ThemeToggle.Location = new Point(x, Math.Max(0, y));
        }

        private void button_ThemeToggle_Click(object sender, EventArgs e)
        {
            if (ProgramSettings == null) return;
            ProgramSettings.DarkMode = !ProgramSettings.DarkMode;
            Save_Settings();
            DarkMode_AskRestart();
        }

        // Text set in code on purpose: no .resx is touched, so the translations stay intact.
        private static string DarkMode_Caption()
        {
            switch (CultureInfo.CurrentUICulture.TwoLetterISOLanguageName)
            {
                case "pt": return "Modo escuro / claro";
                case "ru": return "Тёмная / светлая тема";
                case "uk": return "Темна / світла тема";
                default: return "Dark / light mode";
            }
        }

        // Same restart prompt used by the language selector (already translated in every language).
        private void DarkMode_AskRestart()
        {
            if (MessageBox.Show(Properties.FormStrings.Message_Restart_Text1 + Environment.NewLine +
                                Properties.FormStrings.Message_Restart_Text2, Properties.FormStrings.Message_Restart_Caption,
                                MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                Application.Restart();
            }
        }

        // ---------------------------------------------------------------- dark theme choice (Settings tab)

        private static readonly string[] DarkThemeIds = { "blue", "orange" };

        private void DarkMode_CreateThemeChoice()
        {
            Label langLabel = label356;          // "Language:"
            ComboBox langCombo = comboBox_Language;
            if (langLabel == null || langCombo == null || langCombo.Parent == null) return;
            Control page = langCombo.Parent;
            int step = (int)Math.Round(32 * currentDPI); // same spacing as the rows below

            label_DarkTheme = new Label();
            label_DarkTheme.Name = "label_DarkTheme";
            label_DarkTheme.AutoSize = true;
            label_DarkTheme.Font = langLabel.Font;
            label_DarkTheme.Text = DarkMode_ThemeLabel();
            label_DarkTheme.Location = new Point(langLabel.Left, langLabel.Top + step);

            comboBox_DarkTheme = new ComboBox();
            comboBox_DarkTheme.Name = "comboBox_DarkTheme";
            comboBox_DarkTheme.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox_DarkTheme.Font = langCombo.Font;
            foreach (string id in DarkThemeIds) comboBox_DarkTheme.Items.Add(DarkMode_ThemeName(id));
            string current = ProgramSettings != null && "orange".Equals(ProgramSettings.DarkAccent, StringComparison.OrdinalIgnoreCase)
                ? "orange" : "blue";
            comboBox_DarkTheme.SelectedIndex = Array.IndexOf(DarkThemeIds, current);
            comboBox_DarkTheme.SelectedIndexChanged += comboBox_DarkTheme_SelectedIndexChanged;

            page.Controls.Add(label_DarkTheme);
            page.Controls.Add(comboBox_DarkTheme);

            // Sizes are known only once the label has measured its text.
            int left = Math.Max(langCombo.Left, label_DarkTheme.Right + (int)Math.Round(4 * currentDPI));
            comboBox_DarkTheme.Bounds = new Rectangle(left, langCombo.Top + step,
                langCombo.Width, langCombo.Height);

            // Make room: the rows that were right below "Language" move down one step.
            int below = langCombo.Bottom;
            foreach (Control c in page.Controls)
            {
                if (c == label_DarkTheme || c == comboBox_DarkTheme) continue;
                if (c.Top >= below && c.Left >= langLabel.Left - (int)Math.Round(10 * currentDPI) &&
                    c.Bottom + step <= page.ClientSize.Height + (int)Math.Round(4 * currentDPI) &&
                    !DarkMode_Overlaps(page, c, step))
                    c.Top += step;
            }
        }

        // Only move a control if its new place does not cover another control (e.g. the "Reset" button).
        private bool DarkMode_Overlaps(Control page, Control moving, int step)
        {
            Rectangle target = new Rectangle(moving.Left, moving.Top + step, moving.Width, moving.Height);
            foreach (Control other in page.Controls)
            {
                if (other == moving || other == label_DarkTheme || other == comboBox_DarkTheme) continue;
                if (other.Top >= moving.Top && other.Top < moving.Top + step + moving.Height &&
                    other.Left >= moving.Left - (int)Math.Round(10 * currentDPI)) continue; // moves too
                if (target.IntersectsWith(other.Bounds)) return true;
            }
            return false;
        }

        private void comboBox_DarkTheme_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ProgramSettings == null || comboBox_DarkTheme.SelectedIndex < 0) return;
            string id = DarkThemeIds[comboBox_DarkTheme.SelectedIndex];
            if (id.Equals(ProgramSettings.DarkAccent, StringComparison.OrdinalIgnoreCase)) return;
            ProgramSettings.DarkAccent = id;
            Save_Settings();
            if (ProgramSettings.DarkMode) DarkMode_AskRestart(); // in light mode it is only remembered
        }

        private static string DarkMode_ThemeLabel()
        {
            switch (CultureInfo.CurrentUICulture.TwoLetterISOLanguageName)
            {
                case "pt": return "Tema escuro:";
                case "ru": return "Тёмная тема:";
                case "uk": return "Темна тема:";
                default: return "Dark theme:";
            }
        }

        private static string DarkMode_ThemeName(string id)
        {
            bool orange = id == "orange";
            switch (CultureInfo.CurrentUICulture.TwoLetterISOLanguageName)
            {
                case "pt": return orange ? "Laranja" : "Azul";
                case "ru": return orange ? "Оранжевая" : "Синяя";
                case "uk": return orange ? "Помаранчева" : "Синя";
                default: return orange ? "Orange" : "Blue";
            }
        }

        // ---------------------------------------------------------------- toggle button

        /// <summary>Sun (in dark mode: switch to light) or moon (in light mode: switch to dark).</summary>
        private class ThemeToggle : Control
        {
            private readonly bool dark;
            private bool hot;

            public ThemeToggle(bool darkNow)
            {
                dark = darkNow;
                Tag = "no-theme"; // the theme must not recolor it
                Cursor = Cursors.Hand;
                SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer |
                         ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor, true);
                BackColor = Color.Transparent;
            }

            protected override void OnMouseEnter(EventArgs e) { hot = true; Invalidate(); base.OnMouseEnter(e); }
            protected override void OnMouseLeave(EventArgs e) { hot = false; Invalidate(); base.OnMouseLeave(e); }

            protected override void OnPaint(PaintEventArgs e)
            {
                Graphics g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                float w = Width, h = Height, cx = w / 2f, cy = h / 2f;
                Color ink = dark ? (hot ? Color.White : Color.FromArgb(0xC8, 0xC8, 0xC8))
                                 : (hot ? Color.Black : Color.FromArgb(0x50, 0x50, 0x50));
                using (SolidBrush b = new SolidBrush(ink))
                using (Pen p = new Pen(ink, Math.Max(1f, w / 12f)))
                {
                    if (dark)
                    {
                        // sun: disc + 8 rays
                        float r = w * 0.2f;
                        g.FillEllipse(b, cx - r, cy - r, 2 * r, 2 * r);
                        p.StartCap = p.EndCap = LineCap.Round;
                        for (int i = 0; i < 8; i++)
                        {
                            double a = i * Math.PI / 4;
                            float r1 = w * 0.32f, r2 = w * 0.45f;
                            g.DrawLine(p, cx + (float)Math.Cos(a) * r1, cy + (float)Math.Sin(a) * r1,
                                          cx + (float)Math.Cos(a) * r2, cy + (float)Math.Sin(a) * r2);
                        }
                    }
                    else
                    {
                        // moon: crescent = disc minus a shifted disc
                        float r = w * 0.38f;
                        using (GraphicsPath full = new GraphicsPath())
                        using (GraphicsPath cut = new GraphicsPath())
                        {
                            full.AddEllipse(cx - r, cy - r, 2 * r, 2 * r);
                            cut.AddEllipse(cx - r * 0.35f, cy - r * 1.15f, 2 * r, 2 * r);
                            using (Region moon = new Region(full))
                            {
                                moon.Exclude(cut);
                                g.FillRegion(b, moon);
                            }
                        }
                    }
                }
            }
        }
    }
}
