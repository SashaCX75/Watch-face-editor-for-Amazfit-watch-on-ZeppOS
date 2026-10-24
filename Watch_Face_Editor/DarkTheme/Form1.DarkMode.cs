using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace Watch_Face_Editor
{
    // Experimental dark mode (local branch experimento/dark-mode).
    // Kept in its own partial file so Form1.cs only gains two calls.
    public partial class Form1
    {
        private CheckBox checkBox_DarkMode;

        /// <summary>Called right after InitializeComponent.</summary>
        private void DarkMode_Init()
        {
            try
            {
                DarkMode_CreateCheckBox();
            }
            catch (Exception ex)
            {
                Logger.WriteLine("DarkMode_CreateCheckBox error: " + ex);
            }
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

        private void DarkMode_CreateCheckBox()
        {
            CheckBox anchor = checkBox_Show_Buttons; // "Destacar áreas dos botões"
            if (anchor == null || anchor.Parent == null) return;

            checkBox_DarkMode = new CheckBox();
            checkBox_DarkMode.Name = "checkBox_DarkMode";
            checkBox_DarkMode.AutoSize = true;
            checkBox_DarkMode.Font = anchor.Font;
            checkBox_DarkMode.Anchor = anchor.Anchor;
            checkBox_DarkMode.Location = new Point(anchor.Left, anchor.Bottom + (int)Math.Round(4 * currentDPI));
            checkBox_DarkMode.Text = DarkMode_Caption();
            checkBox_DarkMode.UseVisualStyleBackColor = true;
            checkBox_DarkMode.Checked = ProgramSettings != null && ProgramSettings.DarkMode;
            checkBox_DarkMode.CheckedChanged += checkBox_DarkMode_CheckedChanged;

            anchor.Parent.Controls.Add(checkBox_DarkMode);
            checkBox_DarkMode.BringToFront();
            DarkMode_CreateAccentSwatches();
        }

        // ---------------------------------------------------------------- accent selector
        // Two small color dots to the right of "Dark mode": blue (Zepp app) and orange (Zepp OS).
        private AccentSwatch swatchBlue, swatchOrange;

        private void DarkMode_CreateAccentSwatches()
        {
            Control parent = checkBox_DarkMode.Parent;
            int d = (int)Math.Round(14 * currentDPI), gap = (int)Math.Round(5 * currentDPI);
            int x = checkBox_DarkMode.Right + (int)Math.Round(1 * currentDPI); // the checkbox already has right padding
            int y = checkBox_DarkMode.Top + (checkBox_DarkMode.Height - d) / 2;

            string current = ProgramSettings != null && "orange".Equals(ProgramSettings.DarkAccent, StringComparison.OrdinalIgnoreCase)
                ? "orange" : "blue";
            swatchBlue = new AccentSwatch("blue", Color.FromArgb(0x3E, 0x5F, 0x86), Color.FromArgb(0x5A, 0xA0, 0xE6));
            swatchOrange = new AccentSwatch("orange", Color.FromArgb(0xFF, 0x6C, 0x36), Color.FromArgb(0xF3, 0x59, 0x30));
            ToolTip tip = new ToolTip();
            AccentSwatch[] all = { swatchBlue, swatchOrange };
            for (int i = 0; i < all.Length; i++)
            {
                AccentSwatch sw = all[i];
                sw.Bounds = new Rectangle(x + i * (d + gap), y, d, d);
                sw.Anchor = checkBox_DarkMode.Anchor;
                sw.Selected = sw.AccentName == current;
                sw.Click += AccentSwatch_Click;
                tip.SetToolTip(sw, DarkMode_AccentCaption(sw.AccentName));
                parent.Controls.Add(sw);
                sw.BringToFront();
            }
        }

        private static string DarkMode_AccentCaption(string accent)
        {
            bool orange = accent == "orange";
            switch (CultureInfo.CurrentUICulture.TwoLetterISOLanguageName)
            {
                case "pt": return orange ? "Laranja (identidade Zepp OS)" : "Azul (identidade app Zepp)";
                case "ru": return orange ? "Оранжевый (Zepp OS)" : "Синий (приложение Zepp)";
                case "uk": return orange ? "Помаранчевий (Zepp OS)" : "Синій (застосунок Zepp)";
                default: return orange ? "Orange (Zepp OS identity)" : "Blue (Zepp app identity)";
            }
        }

        private void AccentSwatch_Click(object sender, EventArgs e)
        {
            AccentSwatch sw = (AccentSwatch)sender;
            if (ProgramSettings == null || sw.Selected) return;
            swatchBlue.Selected = sw == swatchBlue;
            swatchOrange.Selected = sw == swatchOrange;
            ProgramSettings.DarkAccent = sw.AccentName;
            Save_Settings();
            if (!ProgramSettings.DarkMode) return; // only matters in dark mode; nothing to restart

            if (MessageBox.Show(Properties.FormStrings.Message_Restart_Text1 + Environment.NewLine +
                                Properties.FormStrings.Message_Restart_Text2, Properties.FormStrings.Message_Restart_Caption,
                                MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                Application.Restart();
            }
        }

        /// <summary>Round color dot; the selected one gets a light ring.</summary>
        private class AccentSwatch : Control
        {
            public readonly string AccentName;
            private readonly Color fill, rim;
            private bool selected, hot;

            public AccentSwatch(string name, Color fillColor, Color rimColor)
            {
                AccentName = name; fill = fillColor; rim = rimColor;
                Tag = "no-theme"; // the theme must not recolor it
                Cursor = Cursors.Hand;
                SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer |
                         ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor, true);
                BackColor = Color.Transparent;
            }

            public bool Selected { get { return selected; } set { selected = value; Invalidate(); } }

            protected override void OnMouseEnter(EventArgs e) { hot = true; Invalidate(); base.OnMouseEnter(e); }
            protected override void OnMouseLeave(EventArgs e) { hot = false; Invalidate(); base.OnMouseLeave(e); }

            protected override void OnPaint(PaintEventArgs e)
            {
                Graphics g = e.Graphics;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                Rectangle r = new Rectangle(0, 0, Width - 1, Height - 1);
                if (selected)
                {
                    using (Pen ring = new Pen(Color.FromArgb(0xE0, 0xE0, 0xE0), 1.5f)) g.DrawEllipse(ring, r);
                    r.Inflate(-3, -3);
                }
                else if (hot) r.Inflate(-1, -1);
                else r.Inflate(-2, -2);
                using (SolidBrush b = new SolidBrush(fill)) g.FillEllipse(b, r);
                using (Pen p = new Pen(rim, 1f)) g.DrawEllipse(p, r);
            }
        }

        // Text set in code on purpose: no .resx is touched, so the pt-BR translation stays intact.
        private static string DarkMode_Caption()
        {
            switch (CultureInfo.CurrentUICulture.TwoLetterISOLanguageName)
            {
                case "pt": return "Modo escuro";
                case "ru": return "Тёмная тема";
                case "uk": return "Темна тема";
                default: return "Dark mode";
            }
        }

        private void checkBox_DarkMode_CheckedChanged(object sender, EventArgs e)
        {
            if (Settings_Load || ProgramSettings == null) return;
            ProgramSettings.DarkMode = checkBox_DarkMode.Checked;
            Save_Settings();

            // Same restart prompt used by the language selector (already translated in every language).
            if (MessageBox.Show(Properties.FormStrings.Message_Restart_Text1 + Environment.NewLine +
                                Properties.FormStrings.Message_Restart_Text2, Properties.FormStrings.Message_Restart_Caption,
                                MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                Application.Restart();
            }
        }
    }
}
