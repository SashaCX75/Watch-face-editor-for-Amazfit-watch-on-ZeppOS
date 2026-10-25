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
        private AccentOption[] themeOptions;
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

            // The version label and the toggle sit on the form, over the tab strip: in dark mode give them
            // the tab strip color, otherwise they show the (darker) form background behind them.
            if (DarkTheme.ThemeManager.Enabled)
            {
                if (label_version != null) label_version.BackColor = DarkTheme.ThemeManager.TabStrip;
                if (button_ThemeToggle != null) button_ThemeToggle.BackColor = DarkTheme.ThemeManager.TabStrip;
            }
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
            button_ThemeToggle.Size = new Size((int)Math.Round(34 * currentDPI), (int)Math.Round(16 * currentDPI));
            button_ThemeToggle.Anchor = anchor.Anchor;
            PlaceToggle();
            anchor.LocationChanged += (s, e) => PlaceToggle();

            new ToolTip().SetToolTip(button_ThemeToggle, DarkMode_Caption());
            button_ThemeToggle.Toggled += button_ThemeToggle_Click; // after the slide animation
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
            page.Controls.Add(label_DarkTheme);

            string current = ProgramSettings != null && "orange".Equals(ProgramSettings.DarkAccent, StringComparison.OrdinalIgnoreCase)
                ? "orange" : "blue";
            themeOptions = new AccentOption[]
            {
                new AccentOption("blue", DarkMode_ThemeName("blue"), Color.FromArgb(0x3E, 0x5F, 0x86), Color.FromArgb(0x5A, 0xA0, 0xE6), langLabel.Font, currentDPI),
                new AccentOption("orange", DarkMode_ThemeName("orange"), Color.FromArgb(0xFF, 0x6C, 0x36), Color.FromArgb(0xF3, 0x59, 0x30), langLabel.Font, currentDPI)
            };
            // first option lines up with the Language list; the rows are centred on the label
            int x = Math.Max(langCombo.Left, label_DarkTheme.Right + (int)Math.Round(4 * currentDPI));
            int gap = (int)Math.Round(14 * currentDPI);
            foreach (AccentOption o in themeOptions)
            {
                o.Selected = o.AccentName == current;
                o.Location = new Point(x, label_DarkTheme.Top + label_DarkTheme.Height / 2 - o.Height / 2);
                o.Click += AccentOption_Click;
                page.Controls.Add(o);
                x = o.Right + gap;
            }

            // Make room: the rows that were right below "Language" move down one step.
            int below = langCombo.Bottom;
            foreach (Control c in page.Controls)
            {
                if (DarkMode_IsNew(c)) continue;
                if (c.Top >= below && c.Left >= langLabel.Left - (int)Math.Round(10 * currentDPI) &&
                    c.Bottom + step <= page.ClientSize.Height + (int)Math.Round(4 * currentDPI) &&
                    !DarkMode_Overlaps(page, c, step))
                    c.Top += step;
            }
        }

        private bool DarkMode_IsNew(Control c)
        {
            if (c == label_DarkTheme) return true;
            if (themeOptions != null) foreach (AccentOption o in themeOptions) if (c == o) return true;
            return false;
        }

        // Only move a control if its new place does not cover another control (e.g. the "Reset" button).
        private bool DarkMode_Overlaps(Control page, Control moving, int step)
        {
            Rectangle target = new Rectangle(moving.Left, moving.Top + step, moving.Width, moving.Height);
            foreach (Control other in page.Controls)
            {
                if (other == moving || DarkMode_IsNew(other)) continue;
                if (other.Top >= moving.Top && other.Top < moving.Top + step + moving.Height &&
                    other.Left >= moving.Left - (int)Math.Round(10 * currentDPI)) continue; // moves too
                if (target.IntersectsWith(other.Bounds)) return true;
            }
            return false;
        }

        private void AccentOption_Click(object sender, EventArgs e)
        {
            AccentOption picked = (AccentOption)sender;
            if (ProgramSettings == null || picked.Selected) return;
            foreach (AccentOption o in themeOptions) o.Selected = o == picked;
            ProgramSettings.DarkAccent = picked.AccentName;
            Save_Settings();
            if (ProgramSettings.DarkMode) DarkMode_AskRestart(); // in light mode it is only remembered
        }

        private static string DarkMode_ThemeLabel()
        {
            switch (CultureInfo.CurrentUICulture.TwoLetterISOLanguageName)
            {
                case "pt": return "Estilo do tema escuro:";
                case "ru": return "Стиль тёмной темы:";
                case "uk": return "Стиль темної теми:";
                default: return "Style for dark theme:";
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

        /// <summary>Color dot + name; the selected one gets a light ring around the dot.</summary>
        private class AccentOption : Control
        {
            public readonly string AccentName;
            private readonly Color fill, rim;
            private readonly int dot, pad;
            private bool selected, hot;

            public AccentOption(string id, string caption, Color fillColor, Color rimColor, Font font, float dpi)
            {
                AccentName = id; fill = fillColor; rim = rimColor;
                Text = caption; Font = font;
                Tag = "no-theme"; // the theme must not recolor it
                Cursor = Cursors.Hand;
                SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer |
                         ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor, true);
                BackColor = Color.Transparent;
                dot = (int)Math.Round(14 * dpi);
                pad = (int)Math.Round(5 * dpi);
                Size textSize = TextRenderer.MeasureText(caption, font);
                Size = new Size(dot + pad + textSize.Width, Math.Max(dot, textSize.Height) + 2);
            }

            public bool Selected { get { return selected; } set { selected = value; Invalidate(); } }

            protected override void OnMouseEnter(EventArgs e) { hot = true; Invalidate(); base.OnMouseEnter(e); }
            protected override void OnMouseLeave(EventArgs e) { hot = false; Invalidate(); base.OnMouseLeave(e); }

            protected override void OnPaint(PaintEventArgs e)
            {
                Graphics g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                bool dark = DarkTheme.ThemeManager.Enabled;
                Rectangle r = new Rectangle(0, (Height - dot) / 2, dot - 1, dot - 1);
                if (selected)
                {
                    Color ringColor = dark ? Color.FromArgb(0xE0, 0xE0, 0xE0) : Color.FromArgb(0x40, 0x40, 0x40);
                    using (Pen ring = new Pen(ringColor, 1.5f)) g.DrawEllipse(ring, r);
                    r.Inflate(-3, -3);
                }
                else if (hot) r.Inflate(-1, -1);
                else r.Inflate(-2, -2);
                using (SolidBrush b = new SolidBrush(fill)) g.FillEllipse(b, r);
                using (Pen p = new Pen(rim, 1f)) g.DrawEllipse(p, r);

                Color text = dark ? (selected || hot ? Color.White : Color.FromArgb(0xC0, 0xC0, 0xC0))
                                  : (selected || hot ? Color.Black : Color.FromArgb(0x40, 0x40, 0x40));
                Rectangle tr = new Rectangle(dot + pad, 0, Width - dot - pad, Height);
                TextRenderer.DrawText(g, Text, Font, tr, text,
                    TextFormatFlags.VerticalCenter | TextFormatFlags.Left | TextFormatFlags.NoPadding);
            }
        }

        // ---------------------------------------------------------------- toggle button

        /// <summary>
        /// Pill switch: sun on the left (light), moon on the right (dark). The knob slides to the active
        /// side; in dark mode it takes the accent color. Toggled fires after the short slide animation.
        /// </summary>
        private class ThemeToggle : Control
        {
            public event EventHandler Toggled;
            private bool on;          // true = dark
            private float pos;        // 0 = left (light) .. 1 = right (dark)
            private bool hot;
            private readonly Timer anim = new Timer { Interval = 15 };

            public ThemeToggle(bool darkNow)
            {
                on = darkNow; pos = darkNow ? 1f : 0f;
                Tag = "no-theme"; // the theme must not recolor it
                Cursor = Cursors.Hand;
                SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer |
                         ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor, true);
                BackColor = Color.Transparent;
                anim.Tick += (s, e) =>
                {
                    float target = on ? 1f : 0f;
                    pos += (target - pos) * 0.45f;
                    if (Math.Abs(target - pos) < 0.02f)
                    {
                        pos = target; anim.Stop(); Invalidate(); Update();
                        if (Toggled != null) Toggled(this, EventArgs.Empty);
                        return;
                    }
                    Invalidate();
                };
            }

            protected override void Dispose(bool disposing) { if (disposing) anim.Dispose(); base.Dispose(disposing); }
            protected override void OnMouseEnter(EventArgs e) { hot = true; Invalidate(); base.OnMouseEnter(e); }
            protected override void OnMouseLeave(EventArgs e) { hot = false; Invalidate(); base.OnMouseLeave(e); }

            protected override void OnClick(EventArgs e)
            {
                base.OnClick(e);
                if (anim.Enabled) return;
                on = !on;
                anim.Start();
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                Graphics g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                bool dark = DarkTheme.ThemeManager.Enabled; // colors follow the theme that is running now
                float h = Height - 1f, w = Width - 1f, r = h / 2f;

                Color track = dark ? Color.FromArgb(0x25, 0x25, 0x25) : Color.FromArgb(0xD6, 0xD6, 0xD6);
                Color border = dark ? (hot ? Color.FromArgb(0x8A, 0x8A, 0x8A) : Color.FromArgb(0x5E, 0x5E, 0x5E))
                                    : (hot ? Color.FromArgb(0x70, 0x70, 0x70) : Color.FromArgb(0xA0, 0xA0, 0xA0));
                Color faint = dark ? Color.FromArgb(0x80, 0x80, 0x80) : Color.FromArgb(0x90, 0x90, 0x90);
                Color knob = on ? (dark ? DarkTheme.ThemeManager.Accent : Color.FromArgb(0x40, 0x40, 0x40))
                                : Color.White;
                Color glyph = on ? Color.White : Color.FromArgb(0xF0, 0x9A, 0x00);

                using (GraphicsPath pill = Pill(0.5f, 0.5f, w, h))
                {
                    using (SolidBrush b = new SolidBrush(track)) g.FillPath(b, pill);
                    using (Pen p = new Pen(border)) g.DrawPath(p, pill);
                }

                // the two small marks on the track (the side the knob is not covering)
                float m = h * 0.5f;
                DrawSun(g, r + 0.5f, r + 0.5f, m * 0.62f, faint);
                DrawMoon(g, w - r + 0.5f, r + 0.5f, m * 0.62f, faint);

                // knob
                float kd = h - 3f, kx = 2f + pos * (w - kd - 3f), ky = 2f;
                using (SolidBrush kb = new SolidBrush(knob)) g.FillEllipse(kb, kx, ky, kd, kd);
                float kcx = kx + kd / 2f, kcy = ky + kd / 2f;
                if (pos >= 0.5f) DrawMoon(g, kcx, kcy, kd * 0.33f, glyph);
                else DrawSun(g, kcx, kcy, kd * 0.33f, glyph);
            }

            private static GraphicsPath Pill(float x, float y, float w, float h)
            {
                GraphicsPath p = new GraphicsPath();
                float d = h;
                p.AddArc(x, y, d, d, 90, 180);
                p.AddArc(x + w - d, y, d, d, 270, 180);
                p.CloseFigure();
                return p;
            }

            private static void DrawSun(Graphics g, float cx, float cy, float size, Color c)
            {
                using (SolidBrush b = new SolidBrush(c))
                using (Pen p = new Pen(c, Math.Max(1f, size / 4f)))
                {
                    float r = size * 0.45f;
                    g.FillEllipse(b, cx - r, cy - r, 2 * r, 2 * r);
                    p.StartCap = p.EndCap = LineCap.Round;
                    for (int i = 0; i < 8; i++)
                    {
                        double a = i * Math.PI / 4;
                        float r1 = size * 0.75f, r2 = size * 1.05f;
                        g.DrawLine(p, cx + (float)Math.Cos(a) * r1, cy + (float)Math.Sin(a) * r1,
                                      cx + (float)Math.Cos(a) * r2, cy + (float)Math.Sin(a) * r2);
                    }
                }
            }

            private static void DrawMoon(Graphics g, float cx, float cy, float size, Color c)
            {
                float r = size;
                using (SolidBrush b = new SolidBrush(c))
                using (GraphicsPath full = new GraphicsPath())
                using (GraphicsPath cut = new GraphicsPath())
                {
                    full.AddEllipse(cx - r, cy - r, 2 * r, 2 * r);
                    cut.AddEllipse(cx - r * 0.35f, cy - r * 1.2f, 2 * r, 2 * r);
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
