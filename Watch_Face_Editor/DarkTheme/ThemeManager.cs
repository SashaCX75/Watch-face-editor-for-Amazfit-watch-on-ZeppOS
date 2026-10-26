using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using CheckBoxState = System.Windows.Forms.VisualStyles.CheckBoxState;
using RadioButtonState = System.Windows.Forms.VisualStyles.RadioButtonState;
// The project has its own Watch_Face_Editor.Button (watch-face buttons), so the WinForms one needs an alias.
using WinButton = System.Windows.Forms.Button;

namespace Watch_Face_Editor.DarkTheme
{
    /// <summary>
    /// Experimental dark mode (local branch experimento/dark-mode).
    /// With DarkMode off, Install/Apply return immediately: the editor behaves exactly as before.
    /// Never touches the watch face being edited: only UI colors and UI icons.
    /// </summary>
    internal static class ThemeManager
    {
        // ===================== PALETTE (edit here) =====================
        public static readonly Color Panel = Hex(0x383838);   // F0F0F0 SystemColors.Control (window background)
        public static readonly Color Raised = Hex(0x484848);  // FDFDFD buttons, tab headers, grid headers
        public static readonly Color RaisedDim = Hex(0x3E3E3E); // ControlLight on buttons: hidden elements in the list
        public static readonly Color Surface = Hex(0x2C2C2C); // FFFFFF SystemColors.Window (lists, text boxes)
        public static readonly Color Workspace = Hex(0x262626); // ABABAB AppWorkspace (empty grid area)
        public static Color Selected = Hex(0x3E5F86); // ActiveCaption (selected element)      [accent]
        public static readonly Color Light = Hex(0x444444);   // ControlLight
        public static readonly Color Text = Hex(0xE0E0E0);    // 000000 ControlText / WindowText
        public static readonly Color TextDim = Hex(0x9A9A9A); // GrayText
        public static readonly Color Border = Hex(0x5E5E5E);
        public static readonly Color Hover = Hex(0x555555);
        public static readonly Color Pressed = Hex(0x606060);
        public static Color SelectedHover = Hex(0x4A6E99); // hover over a selected element row  [accent]
        public static readonly Color Deep = Hex(0x252525);    // check/radio boxes, combo arrow button
        public static readonly Color FormBack = Hex(0x2B2B2B); // window background (left panel); tab pages stay Panel
        public static readonly Color TabStrip = Hex(0x383838); // tab strip and the gaps around tab pages
        public static readonly Color PreviewBack = Hex(0x1D1D1D); // behind the watch-face preview
        public static Color Accent = Hex(0x5AA0E6);  // check mark / radio dot                  [accent]
        public static readonly Color ImageCell = Hex(0x202020); // background behind image previews in lists
        public static readonly Color ImageDark = Hex(0x000000);  // image tables: dark sample column (watch screen)
        public static readonly Color ImageLight = Hex(0xB6B6B6); // image tables: light sample column (no glare)
        public static Color Link = Hex(0x009BB7);     // links (was 0000FF)                     [accent]
        public static readonly Color LinkActive = Hex(0xFFFFFF); // link while clicking
        public static readonly Color TipsBack = Hex(0x383838);  // "Dicas de uso" page
        public static readonly Color TipsText = Hex(0xFFFFFF);
        public static readonly Color ScrollTrack = Hex(0x252525);      // custom scrollbars
        public static readonly Color ScrollThumb = Hex(0x4A4A4A);
        public static readonly Color ScrollThumbHot = Hex(0x5A5A5A);
        public static readonly Color ScrollThumbDrag = Hex(0x6A6A6A);
        // ===============================================================

        public static bool Enabled { get; private set; }

        private static readonly Dictionary<int, Color> backMap = new Dictionary<int, Color>();
        private static readonly Dictionary<int, Color> foreMap = new Dictionary<int, Color>();

        // original-icon pixel hash -> dark icon
        private static readonly Dictionary<string, Bitmap> darkByPixels = new Dictionary<string, Bitmap>();

        private static readonly ConditionalWeakTable<Control, object> hooked = new ConditionalWeakTable<Control, object>();
        private static readonly ConditionalWeakTable<Image, object> darkImages = new ConditionalWeakTable<Image, object>();
        private static readonly object Marker = new object();
        private static readonly object iconLock = new object(); // dark masters are shared: LockBits is not concurrent
        private static bool busy;

        private static Color Hex(int rgb) { return Color.FromArgb(255, Color.FromArgb(rgb)); }
        private static int Key(Color c) { return c.ToArgb(); }

        /// <summary>Accent identity: "blue" (Zepp app, default) or "orange" (Zepp OS).</summary>
        public static string AccentName { get; private set; } = "blue";

        /// <summary>Call once, BEFORE Form1.InitializeComponent. accent: "blue" (default) or "orange".</summary>
        public static void Install(bool enabled, string accent)
        {
            if (string.Equals(accent, "orange", StringComparison.OrdinalIgnoreCase))
            {
                AccentName = "orange";
                Selected = Hex(0xFF6C36);
                SelectedHover = Hex(0xFF6C36);
                Accent = Hex(0xF35930);
                Link = Hex(0xFF823B);
            }
            Install(enabled);
        }

        /// <summary>Call once, BEFORE Form1.InitializeComponent.</summary>
        public static void Install(bool enabled)
        {
            Enabled = enabled;
            if (!enabled) return;
            try
            {
                backMap[Key(SystemColors.Control)] = Panel;
                backMap[Key(Hex(0xFDFDFD))] = Raised;
                backMap[Key(SystemColors.Window)] = Surface;
                backMap[Key(SystemColors.AppWorkspace)] = Workspace;
                backMap[Key(SystemColors.ActiveCaption)] = Selected;
                backMap[Key(SystemColors.ControlLight)] = Light;
                backMap[Key(SystemColors.ButtonFace)] = Panel;

                foreMap[Key(SystemColors.ControlText)] = Text;
                foreMap[Key(SystemColors.WindowText)] = Text;
                foreMap[Key(SystemColors.GrayText)] = TextDim;

                EnableNativeDarkMode();
                LoadIcons();
                WrapResourceManager(typeof(ThemeManager).Assembly, "Watch_Face_Editor.Properties.Resources");
                WrapResourceManager(typeof(ControlLibrary.AppUtils).Assembly, "ControlLibrary.Properties.Resources");
                Application.Idle += Application_Idle;
            }
            catch (Exception ex)
            {
                Enabled = false; // any failure = classic look, never a broken editor
                Logger.WriteLine("DarkTheme.Install error: " + ex);
            }
        }

        /// <summary>Applies the theme to a form/control tree and keeps watching it.</summary>
        public static void Apply(Control root)
        {
            if (!Enabled || root == null) return;
            try { ApplyTree(root); }
            catch (Exception ex) { Logger.WriteLine("DarkTheme.Apply error: " + ex); }
        }

        // Forms opened later (dialogs, preview window) are themed on the next idle tick.
        private static void Application_Idle(object sender, EventArgs e)
        {
            try
            {
                foreach (Form f in Application.OpenForms)
                {
                    object o;
                    if (!hooked.TryGetValue(f, out o)) Apply(f);
                }
            }
            catch (InvalidOperationException) { /* collection changed (form closed); retry next tick */ }
        }

        // ---------------------------------------------------------------- tree

        private static bool IsExcluded(Control c)
        {
            // Only the tips page is themed: its RTF has no color table (all text uses the auto color).
            // Other RichTextBoxes may carry their own colors (e.g. syntax highlighting) and are left alone.
            if (c is RichTextBox) return !IsTips(c);
            if ("no-theme".Equals(c.Tag as string)) return true;
            // User-chosen color swatches are combo boxes (31 of them) and the per-cell color grid.
            // Other controls with "color" in the name (groups "Cor", radios, a NumericUpDown...) are normal UI.
            if (!(c is ComboBox) && !(c is DataGridView)) return false;
            string n = (c.Name ?? "").ToLowerInvariant();
            return n.Contains("color") || n.Contains("colour");
        }

        private static bool IsTips(Control c)
        {
            return (c.Name ?? "").ToLowerInvariant().Contains("tips");
        }

        private static void ApplyTree(Control c)
        {
            bool excluded = IsExcluded(c);
            if (excluded && c is ComboBox) ThemeSwatchCombo((ComboBox)c); // keep the user's color, theme the frame
            if (!excluded)
            {
                try { ThemeControl(c); }
                catch (Exception ex) { Debug("ThemeControl failed on " + Describe(c) + ": " + ex.Message); } // keep going
            }

            object o;
            if (!hooked.TryGetValue(c, out o))
            {
                hooked.Add(c, Marker);
                c.ControlAdded += (s, e) => Apply(e.Control);
                if (!excluded)
                {
                    c.BackColorChanged += (s, e) => Remap((Control)s);
                    c.ForeColorChanged += (s, e) => Remap((Control)s);
                    if (c is WinButton)
                    {
                        c.EnabledChanged += (s, e) => StyleButton((WinButton)s);
                        c.HandleCreated += (s, e) => StyleButton((WinButton)s);
                    }
                }
                Form form = c as Form;
                if (form != null) form.Shown += Form_Shown;
                if (c is Form || c is ScrollBar || c is ListBox || c is TreeView || c is DataGridView ||
                    (c is ScrollableControl && ((ScrollableControl)c).AutoScroll) ||
                    (c is TextBoxBase && ((TextBoxBase)c).Multiline))
                {
                    if (c.IsHandleCreated) NativeDark(c);
                    c.HandleCreated += (s, e) => NativeDark((Control)s);
                }
                if (c.ContextMenuStrip != null) ThemeToolStrip(c.ContextMenuStrip);
                if (!excluded) ScrollOverlay.Register(c);
            }

            foreach (Control child in c.Controls) ApplyTree(child);
        }

        // Second pass after Load/Shown: catches anything the editor changed during startup.
        private static void Form_Shown(object sender, EventArgs e)
        {
            Form f = (Form)sender;
            try
            {
                ApplyTree(f);
                // diagnostics only on demand: create an empty "DarkTheme.debug" file next to the exe
                if (f is Form1 && File.Exists(Path.Combine(Application.StartupPath, "DarkTheme.debug"))) WriteDebugDump(f);
            }
            catch (Exception ex) { Debug("Form_Shown: " + ex); }
        }

        private static void Remap(Control c)
        {
            if (busy) return;
            busy = true;
            try
            {
                Color nb;
                if (c.BackColor.A == 255 && backMap.TryGetValue(Key(c.BackColor), out nb)) c.BackColor = nb;
                Color nf;
                if (foreMap.TryGetValue(Key(c.ForeColor), out nf)) c.ForeColor = nf;
            }
            finally { busy = false; }
            SpecialBack(c);
            WinButton b = c as WinButton;
            if (b != null) StyleButton(b); // editor reset it to Control -> back to button color
        }

        private static bool IsPreview(Control c)
        {
            return c is PictureBox && (c.Name ?? "").ToLowerInvariant().Contains("preview");
        }

        // Light/default backgrounds that the special areas below may replace.
        private static bool IsDefaultBack(Color col)
        {
            if (col.A != 255) return false;
            int k = Key(col);
            return k == Key(SystemColors.Control) || k == Key(SystemColors.Window) || k == Key(Panel) ||
                   k == Key(Surface) || k == Key(FormBack) || k == Key(Hex(0xFDFDFD));
        }

        private static void SetBack(Control c, Color col)
        {
            if (Key(c.BackColor) == Key(col)) return;
            busy = true;
            try { c.BackColor = col; } finally { busy = false; }
        }

        // Areas with their own shade: window background, tab pages, preview canvas.
        private static void SpecialBack(Control c)
        {
            if (c is Form) { if (IsDefaultBack(c.BackColor)) SetBack(c, FormBack); }
            else if (c is TabPage)
            {
                // A TabPage with visual styles reports Transparent and then shows whatever is behind it.
                TabPage tp = (TabPage)c;
                if (tp.UseVisualStyleBackColor) tp.UseVisualStyleBackColor = false;
                if (tp.BackColor.A < 255 || IsDefaultBack(tp.BackColor)) SetBack(tp, Panel);
            }
            else if (IsPreview(c)) { if (IsDefaultBack(c.BackColor)) SetBack(c, PreviewBack); }
            // Inside tab pages nothing should show the window background (2B2B2B): containers that inherited
            // it (e.g. "Adicionar itens ao mostrador", the main screen/AOD bar) get the page color.
            else if ((c is GroupBox || c is Panel || c is UserControl) && !(c.Parent is Form) &&
                     c.BackColor.A == 255 && Key(c.BackColor) == Key(FormBack))
                SetBack(c, Panel);
        }

        private static void ThemeControl(Control c)
        {
            Remap(c);
            SpecialBack(c);

            if (c is ButtonBase)
            {
                ButtonBase b = (ButtonBase)c;
                if (b is WinButton) StyleButton((WinButton)b);
                if ((b is CheckBox && ((CheckBox)b).Appearance == Appearance.Normal) ||
                    (b is RadioButton && ((RadioButton)b).Appearance == Appearance.Normal))
                    if (!HasPaintHandler(b))
                    {
                        DoubleBuffer(b); // system glyph + our glyph composed off-screen: no white flash on hover
                        AttachOnce(b, "check", () => b.Paint += Check_Paint);
                    }
                { Image sw = Swap(b.Image); if (!ReferenceEquals(sw, b.Image)) b.Image = sw; }
                { Image sw = Swap(b.BackgroundImage); if (!ReferenceEquals(sw, b.BackgroundImage)) b.BackgroundImage = sw; }
            }
            else if (c is PictureBox)
            {
                PictureBox p = (PictureBox)c;
                { Image sw = Swap(p.Image); if (!ReferenceEquals(sw, p.Image)) p.Image = sw; }
                { Image sw = Swap(p.BackgroundImage); if (!ReferenceEquals(sw, p.BackgroundImage)) p.BackgroundImage = sw; }
            }
            else if (c is RichTextBox)
            {
                RichTextBox rtb = (RichTextBox)c;
                busy = true;
                try { rtb.BackColor = TipsBack; } finally { busy = false; }
                AttachOnce(rtb, "rtf", () => rtb.TextChanged += (s, e) => RecolorRich((RichTextBox)s));
                RecolorRich(rtb);
            }
            else if (c is LinkLabel)
            {
                LinkLabel ll = (LinkLabel)c;
                ll.LinkColor = Link;
                ll.VisitedLinkColor = Link;
                ll.ActiveLinkColor = LinkActive;
                { Image sw = Swap(ll.Image); if (!ReferenceEquals(sw, ll.Image)) ll.Image = sw; }
            }
            else if (c is Label)
            {
                Label l = (Label)c;
                { Image sw = Swap(l.Image); if (!ReferenceEquals(sw, l.Image)) l.Image = sw; }
                if (!HasPaintHandler(l)) { DoubleBuffer(l); AttachOnce(l, "label", () => l.Paint += Label_Paint); }
            }
            else if (c is ComboBox)
            {
                ComboBox cb = (ComboBox)c;
                // Flat also for owner-drawn combos (image pickers): the Standard style draws a light
                // themed frame and ignores BackColor. The editor's DrawItem already uses e.BackColor.
                if (cb.FlatStyle != FlatStyle.Flat) cb.FlatStyle = FlatStyle.Flat;
                ForceColors(cb, Surface, Text);
                AttachOnce(cb, "list", () =>
                {
                    cb.HandleCreated += (s, e) => DarkComboList((ComboBox)s);
                    cb.DropDown += (s, e) => DarkComboList((ComboBox)s);
                });
                DarkComboList(cb);
                // Image pickers: the editor draws item text in hard-coded black. Repaint just the text, after it.
                if (cb.DrawMode != DrawMode.Normal && HasHandlerNamed(cb, "EVENT_DRAWITEM", "comboBox_DrawItem"))
                    AttachOnce(cb, "imgtext", () => cb.DrawItem += ImageCombo_DrawItem);
                // Plain lists: Windows highlights the item under the mouse with the system blue.
                if (cb.DrawMode == DrawMode.Normal && !IsExcluded(cb))
                {
                    int h = cb.ItemHeight;
                    cb.DrawMode = DrawMode.OwnerDrawFixed;
                    cb.ItemHeight = h;
                    AttachOnce(cb, "darkitems", () => cb.DrawItem += DarkCombo_DrawItem);
                }
                OverPainter.Attach(cb);
            }
            else if (c is UpDownBase)
            {
                UpDownBase ud = (UpDownBase)c;
                ForceColors(ud, Surface, Text);
                OverPainter.Attach(ud);
                if (ud.BorderStyle == BorderStyle.Fixed3D) ud.BorderStyle = BorderStyle.FixedSingle;
                foreach (Control part in ud.Controls)
                {
                    if (part is TextBoxBase) ForceColors(part, Surface, Text);   // the inner edit box
                    else OverPainter.Attach(part);                              // the ▲▼ buttons
                }
            }
            else if (c is TextBoxBase && !(c is RichTextBox) && c.Parent is UpDownBase)
            {
                ForceColors(c, Surface, Text); // inner edit of a NumericUpDown: no own border
                OverPainter.Attach(c);         // disabled text is drawn by Windows in its own gray
            }
            else if (c is TextBoxBase && !(c is RichTextBox))
            {
                OverPainter.Attach(c);
            }
            else if (c is DateTimePicker)
            {
                OverPainter.Attach(c);
            }
            else if (c is GroupBox)
            {
                // Some group boxes already paint themselves (e.g. collapsible groups in "Dados para prévia",
                // which offset the title past the ▼ button). Those already use the themed colors: leave them.
                if (!HasPaintHandler(c)) { DoubleBuffer(c); AttachOnce(c, "group", () => c.Paint += GroupBox_Paint); }
                else if (PaintHandlerTargetIsForm(c))
                {
                    // Form1.groupBox_Paint clears with the form's color (2B2B2B): repaint it right after,
                    // same layout, with the group's own background.
                    DoubleBuffer(c);
                    AttachOnce(c, "formgroup", () => c.Paint += FormGroupBox_Paint);
                }
            }
            else if (c is TabControl)
            {
                ThemeTabControl((TabControl)c);
            }
            else if (c is TabPage)
            {
                ((TabPage)c).UseVisualStyleBackColor = false;
                SpecialBack(c);
            }
            else if (c is DataGridView)
            {
                ThemeGrid((DataGridView)c);
            }
            else if (c is ToolStrip)
            {
                ThemeToolStrip((ToolStrip)c);
            }
            else
            {
                { Image sw = Swap(c.BackgroundImage); if (!ReferenceEquals(sw, c.BackgroundImage)) c.BackgroundImage = sw; }
            }
        }

        // Sets colors directly (light defaults only), bypassing the map: used where the default is Window.
        private static void ForceColors(Control c, Color back, Color fore)
        {
            int k = Key(c.BackColor);
            busy = true;
            try
            {
                if (c.BackColor.A == 255 && (k == Key(SystemColors.Window) || k == Key(SystemColors.Control) ||
                    k == Key(Panel) || k == Key(Hex(0xFDFDFD)) || k == Key(Surface)))
                    c.BackColor = back;
                int f = Key(c.ForeColor);
                if (f == Key(SystemColors.WindowText) || f == Key(SystemColors.ControlText) || f == Key(Text))
                    c.ForeColor = fore;
            }
            finally { busy = false; }
        }

        private static void ThemeSwatchCombo(ComboBox cb)
        {
            try
            {
                if (cb.FlatStyle != FlatStyle.Flat) cb.FlatStyle = FlatStyle.Flat; // shows BackColor (the chosen color)
                OverPainter.Attach(cb);
                AttachOnce(cb, "swatch-en", () => cb.EnabledChanged += (s, e) => ((Control)s).Invalidate());
            }
            catch (Exception ex) { Debug("swatch " + Describe(cb) + ": " + ex.Message); }
        }

        private static Color MapRowHover(Color c, WinButton b)
        {
            int k = Key(c);
            if (k == Key(SystemColors.ActiveCaption) || k == Key(Selected) || k == Key(SelectedHover)) return Selected;
            Color row = b.Parent != null ? EffectiveBack(b.Parent) : Panel;
            if (k == Key(SystemColors.Control) || k == Key(SystemColors.Window) || k == Key(Hex(0xFDFDFD)) ||
                k == Key(Panel) || k == Key(Raised) || k == Key(Surface) || k == Key(FormBack) || k == Key(Hover) ||
                k == Key(Pressed) || k == Key(SystemColors.ControlLight)) return row;
            return c;
        }

        private static Color MapHover(Color c, Color normal)
        {
            int k = Key(c);
            if (k == Key(SystemColors.ActiveCaption) || k == Key(Selected)) return SelectedHover;
            if (k == Key(SystemColors.Control) || k == Key(SystemColors.Window) || k == Key(Hex(0xFDFDFD)) ||
                k == Key(Panel) || k == Key(Raised) || k == Key(Surface) || k == Key(FormBack) ||
                k == Key(SystemColors.ControlLight)) return normal;
            return c; // any other (custom) color is left as the editor set it
        }

        private static void FixHover(WinButton b)
        {
            if (!Enabled || IsExcluded(b)) return;
            FlatButtonAppearance fa = b.FlatAppearance;
            Color over, down;
            if (fa.BorderSize == 0)
            {
                // Borderless rows (sub-items of an element): the editor sets hover = row color on purpose,
                // so nothing lights up behind the text. Keep it that way in dark mode.
                over = MapRowHover(fa.MouseOverBackColor, b);
                down = MapRowHover(fa.MouseDownBackColor, b);
            }
            else
            {
                over = MapHover(fa.MouseOverBackColor, Hover);
                down = MapHover(fa.MouseDownBackColor, Pressed);
            }
            if (Key(over) != Key(fa.MouseOverBackColor)) fa.MouseOverBackColor = over;
            if (Key(down) != Key(fa.MouseDownBackColor)) fa.MouseDownBackColor = down;
        }

        private static void StyleButton(WinButton b)
        {
            if (!Enabled || IsExcluded(b)) return;
            if (b.FlatStyle != FlatStyle.Flat) b.FlatStyle = FlatStyle.Flat;
            if (b.UseVisualStyleBackColor) b.UseVisualStyleBackColor = false;
            b.FlatAppearance.BorderColor = Border;
            FixHover(b);
            // The editor swaps these at runtime (ActiveCaption when an element is selected, Control when not),
            // and FlatAppearance has no change event: fix them right before they are used.
            AttachOnce(b, "hover", () =>
            {
                b.MouseEnter += (s, e) => FixHover((WinButton)s);
                b.MouseDown += (s, e) => FixHover((WinButton)s);
                // The editor sets ActiveCaption as hover color in its Click handler (runs before ours):
                // fix it again right after, otherwise the light-theme blue shows while the mouse is still over it.
                b.Click += (s, e) => { FixHover((WinButton)s); ((WinButton)s).Invalidate(); };
                b.MouseUp += (s, e) => { FixHover((WinButton)s); ((WinButton)s).Invalidate(); };
            });
            int k = Key(b.BackColor);
            // Borderless flat buttons are list rows (e.g. "Imagens", "Valores numéricos" inside an element):
            // in the light theme they blended with the row, so they stay transparent instead of button-colored.
            bool row = b.FlatAppearance.BorderSize == 0;
            // transparent icon buttons stay transparent; selected/custom colors are left alone
            // The element list shows hidden elements on ControlLight (E3E3E3) instead of Control (F0F0F0).
            if (!row && b.BackColor.A == 255 && (k == Key(SystemColors.ControlLight) || k == Key(Light)))
            {
                busy = true;
                try { b.BackColor = RaisedDim; } finally { busy = false; }
            }
            else if (b.BackColor.A == 255 && (k == Key(Panel) || k == Key(Surface) || k == Key(SystemColors.Control) ||
                k == Key(SystemColors.Window) || k == Key(Hex(0xFDFDFD)) || k == Key(FormBack) || (row && k == Key(Raised))))
            {
                busy = true;
                try { b.BackColor = row ? Color.Transparent : Raised; } finally { busy = false; }
            }
            Color nf;
            if (foreMap.TryGetValue(Key(b.ForeColor), out nf)) b.ForeColor = nf;
            if (!row && b.BackgroundImage == null)
                AttachOnce(b, "disabledText", () => { if (!HasPaintHandler(b)) b.Paint += DisabledButton_Paint; });
        }

        // Flat buttons draw disabled text with ControlPaint.Dark(BackColor): on 484848 that is almost black.
        // Repaint the content of disabled buttons with TextDim (and a disabled image, if any).
        private static void DisabledButton_Paint(object sender, PaintEventArgs e)
        {
            WinButton b = (WinButton)sender;
            if (!Enabled || b.Enabled || IsExcluded(b) || b.BackgroundImage != null) return;
            if (string.IsNullOrEmpty(b.Text) && b.Image == null) return;
            Graphics g = e.Graphics;
            int bw = Math.Max(1, b.FlatAppearance.BorderSize);
            Rectangle inner = Rectangle.Inflate(b.ClientRectangle, -bw, -bw);
            Color back = b.BackColor.A == 255 ? b.BackColor : EffectiveBack(b);
            using (SolidBrush bg = new SolidBrush(back)) g.FillRectangle(bg, inner);
            Rectangle content = new Rectangle(inner.Left + b.Padding.Left + 1, inner.Top + b.Padding.Top,
                Math.Max(0, inner.Width - b.Padding.Horizontal - 2), Math.Max(0, inner.Height - b.Padding.Vertical));
            TextFormatFlags f = AlignFlags(b.TextAlign) | TextFormatFlags.WordBreak;
            if (b.UseMnemonic) f &= ~TextFormatFlags.NoPrefix;
            Image img = b.Image;
            if (img == null) { TextRenderer.DrawText(g, b.Text, b.Font, content, TextDim, f); return; }

            Size ts = string.IsNullOrEmpty(b.Text) ? Size.Empty : TextRenderer.MeasureText(g, b.Text, b.Font);
            if (b.TextImageRelation == TextImageRelation.ImageBeforeText || b.TextImageRelation == TextImageRelation.TextBeforeImage)
            {
                int gap = ts.Width > 0 ? 2 : 0;
                int x = content.Left + Math.Max(0, (content.Width - (img.Width + gap + ts.Width)) / 2);
                bool imageFirst = b.TextImageRelation == TextImageRelation.ImageBeforeText;
                int ix = imageFirst ? x : x + ts.Width + gap;
                int tx = imageFirst ? x + img.Width + gap : x;
                ControlPaint.DrawImageDisabled(g, img, ix, content.Top + (content.Height - img.Height) / 2, back);
                if (ts.Width > 0)
                    TextRenderer.DrawText(g, b.Text, b.Font, new Rectangle(tx, content.Top, ts.Width + 2, content.Height), TextDim,
                        TextFormatFlags.VerticalCenter | TextFormatFlags.Left | TextFormatFlags.NoPadding | (f & TextFormatFlags.NoPrefix));
            }
            else
            {
                Point ip = AlignPoint(img.Size, content, b.ImageAlign);
                ControlPaint.DrawImageDisabled(g, img, ip.X, ip.Y, back);
                if (ts.Width > 0) TextRenderer.DrawText(g, b.Text, b.Font, content, TextDim, f);
            }
        }

        private static Point AlignPoint(Size s, Rectangle r, ContentAlignment a)
        {
            int x = r.Left, y = r.Top;
            switch (a)
            {
                case ContentAlignment.TopCenter: case ContentAlignment.MiddleCenter: case ContentAlignment.BottomCenter:
                    x = r.Left + (r.Width - s.Width) / 2; break;
                case ContentAlignment.TopRight: case ContentAlignment.MiddleRight: case ContentAlignment.BottomRight:
                    x = r.Right - s.Width; break;
            }
            switch (a)
            {
                case ContentAlignment.MiddleLeft: case ContentAlignment.MiddleCenter: case ContentAlignment.MiddleRight:
                    y = r.Top + (r.Height - s.Height) / 2; break;
                case ContentAlignment.BottomLeft: case ContentAlignment.BottomCenter: case ContentAlignment.BottomRight:
                    y = r.Bottom - s.Height; break;
            }
            return new Point(x, y);
        }

        // ---------------------------------------------------------------- painted-over parts
        // Windows draws tab strips, check/radio glyphs, combo arrow buttons and group borders
        // itself, ignoring BackColor. We let it paint, then paint our version on top.

        private static readonly ConditionalWeakTable<Control, HashSet<string>> attached =
            new ConditionalWeakTable<Control, HashSet<string>>();

        private static void AttachOnce(Control c, string what, Action attach)
        {
            HashSet<string> set = attached.GetOrCreateValue(c);
            if (set.Add(what)) attach();
        }

        private static readonly object paintEventKey =
            typeof(Control).GetField("EventPaint", BindingFlags.NonPublic | BindingFlags.Static) != null
                ? typeof(Control).GetField("EventPaint", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null)
                : null;
        private static readonly PropertyInfo eventsProperty =
            typeof(System.ComponentModel.Component).GetProperty("Events", BindingFlags.NonPublic | BindingFlags.Instance);

        /// <summary>True if the control has a handler for the given event whose method has this name.</summary>
        private static bool HasHandlerNamed(Control c, string keyField, string method)
        {
            try
            {
                Type t = c.GetType();
                FieldInfo kf = null;
                while (t != null && kf == null) { kf = t.GetField(keyField, BindingFlags.NonPublic | BindingFlags.Static); t = t.BaseType; }
                if (kf == null || eventsProperty == null) return false;
                System.ComponentModel.EventHandlerList list =
                    (System.ComponentModel.EventHandlerList)eventsProperty.GetValue(c, null);
                Delegate d = list != null ? list[kf.GetValue(null)] : null;
                if (d == null) return false;
                foreach (Delegate x in d.GetInvocationList()) if (x.Method.Name == method) return true;
                return false;
            }
            catch (Exception) { return false; }
        }

        private static void DarkCombo_DrawItem(object sender, DrawItemEventArgs e)
        {
            ComboBox cb = (ComboBox)sender;
            if (!Enabled || e.Index < 0 || e.Index >= cb.Items.Count) return;
            bool sel = (e.State & DrawItemState.Selected) != 0;
            bool edit = (e.State & DrawItemState.ComboBoxEdit) != 0;
            Color back = sel ? Selected : (edit ? e.BackColor : Surface);
            using (SolidBrush bg = new SolidBrush(back)) e.Graphics.FillRectangle(bg, e.Bounds);
            Rectangle tr = new Rectangle(e.Bounds.X + 2, e.Bounds.Y, Math.Max(0, e.Bounds.Width - 2), e.Bounds.Height);
            // back color passed to GDI: ClearType then blends the letter edges with it (not with black)
            TextRenderer.DrawText(e.Graphics, cb.GetItemText(cb.Items[e.Index]), cb.Font, tr,
                !cb.Enabled ? TextDim : (sel ? Color.White : Text), back,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine |
                TextFormatFlags.NoPrefix | TextFormatFlags.EndEllipsis);
        }

        private static void RedrawThumbnail(ComboBox cb, DrawItemEventArgs e, int thumb)
        {
            try
            {
                Rectangle sq = new Rectangle(e.Bounds.X, e.Bounds.Y, thumb, e.Bounds.Height);
                using (SolidBrush b = new SolidBrush(Selected)) e.Graphics.FillRectangle(b, sq);
                string path = ImagePathFor(cb, e.Index);
                if (path == null || !File.Exists(path)) return;
                int itemWidth = e.Bounds.Height, itemHeight = e.Bounds.Height - 4;
                using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read))
                using (Image image = Image.FromStream(stream))
                {
                    float scale = (float)itemWidth / image.Width;
                    if ((float)itemHeight / image.Height < scale) scale = (float)itemHeight / image.Height;
                    float w = image.Width * scale, h = image.Height * scale;
                    Rectangle r = new Rectangle((int)(itemWidth - w) / 2 + 2,
                        e.Bounds.Top + (int)(itemHeight - h) / 2 + 2, (int)w, (int)h);
                    e.Graphics.DrawImage(image, r);
                }
            }
            catch (Exception) { /* cosmetic only */ }
        }

        // The image pickers keep their file list in a private field "ListImagesFullName" of the control that
        // owns the comboBox_DrawItem handler.
        private static string ImagePathFor(ComboBox cb, int index)
        {
            try
            {
                FieldInfo kf = typeof(ComboBox).GetField("EVENT_DRAWITEM", BindingFlags.NonPublic | BindingFlags.Static);
                if (kf == null || eventsProperty == null) return null;
                System.ComponentModel.EventHandlerList list =
                    (System.ComponentModel.EventHandlerList)eventsProperty.GetValue(cb, null);
                Delegate d = list != null ? list[kf.GetValue(null)] : null;
                if (d == null) return null;
                foreach (Delegate x in d.GetInvocationList())
                {
                    if (x.Method.Name != "comboBox_DrawItem" || x.Target == null) continue;
                    FieldInfo f = x.Target.GetType().GetField("ListImagesFullName",
                        BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
                    System.Collections.IList paths = f != null ? f.GetValue(x.Target) as System.Collections.IList : null;
                    if (paths != null && index >= 0 && index < paths.Count) return paths[index] as string;
                }
            }
            catch (Exception) { }
            return null;
        }

        private static void ImageCombo_DrawItem(object sender, DrawItemEventArgs e)
        {
            ComboBox cb = (ComboBox)sender;
            if (!Enabled || e.Index < 0 || e.Index >= cb.Items.Count) return;
            int thumb = e.Bounds.Height; // same layout as the editor: thumbnail square on the left
            Rectangle tr = new Rectangle(e.Bounds.X + thumb, e.Bounds.Y, Math.Max(0, e.Bounds.Width - thumb), e.Bounds.Height);
            bool sel = (e.State & DrawItemState.Selected) != 0;
            using (SolidBrush bg = new SolidBrush(sel ? Selected : e.BackColor)) e.Graphics.FillRectangle(bg, tr);
            // The editor filled the thumbnail square with the system blue (e.DrawBackground): repaint it in the
            // accent color and draw the thumbnail again, with the editor's own layout.
            if (sel) RedrawThumbnail(cb, e, thumb);
            TextRenderer.DrawText(e.Graphics, cb.Items[e.Index].ToString(), cb.Font, tr,
                sel ? Color.White : (cb.Enabled ? Text : TextDim), sel ? Selected : e.BackColor,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine |
                TextFormatFlags.NoPrefix | TextFormatFlags.EndEllipsis);
            if ((e.State & DrawItemState.Focus) != 0 && (e.State & DrawItemState.NoFocusRect) == 0) e.DrawFocusRectangle();
        }

        private static bool PaintHandlerTargetIsForm(Control c)
        {
            try
            {
                if (paintEventKey == null || eventsProperty == null) return false;
                System.ComponentModel.EventHandlerList list =
                    (System.ComponentModel.EventHandlerList)eventsProperty.GetValue(c, null);
                Delegate d = list != null ? list[paintEventKey] : null;
                if (d == null) return false;
                foreach (Delegate x in d.GetInvocationList()) if (x.Target is Form) return true;
                return false;
            }
            catch (Exception) { return false; }
        }

        // Mirror of Form1.DrawGroupBox (same text position, font and border), using the group's background.
        private static void FormGroupBox_Paint(object sender, PaintEventArgs e)
        {
            GroupBox gb = (GroupBox)sender;
            if (!Enabled) return;
            Graphics g = e.Graphics;
            SizeF str = g.MeasureString(gb.Text, gb.Font);
            Rectangle rect = new Rectangle(gb.ClientRectangle.X, gb.ClientRectangle.Y + (int)(str.Height / 2),
                gb.ClientRectangle.Width - 1, gb.ClientRectangle.Height - (int)(str.Height / 2) - 5);
            g.Clear(EffectiveBack(gb));
            using (SolidBrush tb = new SolidBrush(gb.Enabled ? Text : TextDim))
                g.DrawString(gb.Text, gb.Font, tb, gb.Padding.Left, 0);
            using (Pen p = new Pen(Color.DarkGray))
            {
                g.DrawLine(p, rect.Location, new Point(rect.X, rect.Y + rect.Height));
                g.DrawLine(p, new Point(rect.X + rect.Width, rect.Y), new Point(rect.X + rect.Width, rect.Y + rect.Height));
                g.DrawLine(p, new Point(rect.X, rect.Y + rect.Height), new Point(rect.X + rect.Width, rect.Y + rect.Height));
                g.DrawLine(p, new Point(rect.X, rect.Y), new Point(rect.X + gb.Padding.Left, rect.Y));
                g.DrawLine(p, new Point(rect.X + gb.Padding.Left + (int)str.Width, rect.Y), new Point(rect.X + rect.Width, rect.Y));
            }
        }

        /// <summary>True if the editor's own code already subscribed to this control's Paint event.</summary>
        private static bool HasPaintHandler(Control c)
        {
            try
            {
                if (paintEventKey == null || eventsProperty == null) return false;
                System.ComponentModel.EventHandlerList list =
                    (System.ComponentModel.EventHandlerList)eventsProperty.GetValue(c, null);
                return list != null && list[paintEventKey] != null;
            }
            catch (Exception) { return false; }
        }

        private static readonly MethodInfo setStyle =
            typeof(Control).GetMethod("SetStyle", BindingFlags.NonPublic | BindingFlags.Instance);

        // Paints into an off-screen buffer first, so the system drawing we paint over never reaches the screen.
        private static void DoubleBuffer(Control c)
        {
            try
            {
                if (setStyle != null)
                    setStyle.Invoke(c, new object[] { ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true });
            }
            catch (Exception) { }
        }

        private static ContentAlignment[] leftAligned =
            { ContentAlignment.TopLeft, ContentAlignment.MiddleLeft, ContentAlignment.BottomLeft };

        private static void Check_Paint(object sender, PaintEventArgs e)
        {
            ButtonBase b = (ButtonBase)sender;
            if (!Enabled) return;
            bool radio = b is RadioButton;
            ContentAlignment align = radio ? ((RadioButton)b).CheckAlign : ((CheckBox)b).CheckAlign;
            if (Array.IndexOf(leftAligned, align) < 0 || b.RightToLeft == RightToLeft.Yes) return;

            Size gs = radio ? RadioButtonRenderer.GetGlyphSize(e.Graphics, RadioButtonState.UncheckedNormal)
                            : CheckBoxRenderer.GetGlyphSize(e.Graphics, CheckBoxState.UncheckedNormal);
            int y;
            if (align == ContentAlignment.TopLeft) y = b.Padding.Top + 1;
            else if (align == ContentAlignment.BottomLeft) y = b.Height - gs.Height - b.Padding.Bottom - 1;
            else y = (b.Height - gs.Height) / 2;
            Rectangle box = new Rectangle(b.Padding.Left, y, gs.Width, gs.Height);

            bool on = radio ? ((RadioButton)b).Checked : ((CheckBox)b).CheckState != CheckState.Unchecked;
            bool indeterminate = !radio && ((CheckBox)b).CheckState == CheckState.Indeterminate;
            Graphics g = e.Graphics;
            System.Drawing.Drawing2D.SmoothingMode old = g.SmoothingMode;

            // erase the system glyph (2px margin: its exact position can vary by 1-2px)
            using (SolidBrush bg = new SolidBrush(b.BackColor.A == 255 ? b.BackColor : Panel))
                g.FillRectangle(bg, new Rectangle(Math.Max(0, box.X - 2), box.Y - 2, box.Width + 4, box.Height + 4));

            Color edge = b.Enabled ? (on ? Accent : Border) : Light;
            using (SolidBrush fill = new SolidBrush(Deep))
            using (Pen pen = new Pen(edge))
            {
                if (radio)
                {
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    Rectangle r = new Rectangle(box.X, box.Y, box.Width - 1, box.Height - 1);
                    g.FillEllipse(fill, r);
                    g.DrawEllipse(pen, r);
                    if (on)
                    {
                        Rectangle dot = Rectangle.Inflate(r, -(r.Width / 4 + 1), -(r.Height / 4 + 1));
                        using (SolidBrush ab = new SolidBrush(b.Enabled ? Accent : TextDim)) g.FillEllipse(ab, dot);
                    }
                }
                else
                {
                    Rectangle r = new Rectangle(box.X, box.Y, box.Width - 1, box.Height - 1);
                    g.FillRectangle(fill, r);
                    g.DrawRectangle(pen, r);
                    if (indeterminate)
                    {
                        using (SolidBrush ab = new SolidBrush(b.Enabled ? Accent : TextDim))
                            g.FillRectangle(ab, Rectangle.Inflate(r, -3, -3));
                    }
                    else if (on)
                    {
                        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                        float w = r.Width, h = r.Height;
                        PointF[] tick =
                        {
                            new PointF(r.X + w * 0.22f, r.Y + h * 0.52f),
                            new PointF(r.X + w * 0.42f, r.Y + h * 0.72f),
                            new PointF(r.X + w * 0.78f, r.Y + h * 0.30f)
                        };
                        using (Pen tp = new Pen(b.Enabled ? Accent : TextDim, Math.Max(1.6f, w / 7f)))
                        {
                            tp.StartCap = System.Drawing.Drawing2D.LineCap.Round;
                            tp.EndCap = System.Drawing.Drawing2D.LineCap.Round;
                            tp.LineJoin = System.Drawing.Drawing2D.LineJoin.Round;
                            g.DrawLines(tp, tick);
                        }
                    }
                }
            }
            g.SmoothingMode = old;

            if (!b.Enabled && !string.IsNullOrEmpty(b.Text))
            {
                int tx = box.Right + 3;
                Rectangle tr = new Rectangle(tx, 0, Math.Max(0, b.Width - tx), b.Height);
                using (SolidBrush bg = new SolidBrush(EffectiveBack(b))) g.FillRectangle(bg, tr);
                TextFormatFlags f = AlignFlags(b.TextAlign) | TextFormatFlags.WordBreak;
                if (b.UseMnemonic) f &= ~TextFormatFlags.NoPrefix;
                TextRenderer.DrawText(g, b.Text, b.Font, tr, TextDim, f);
            }
        }

        // WinForms draws disabled text "etched", derived from BackColor: on dark backgrounds it vanishes.
        private static Color EffectiveBack(Control c)
        {
            for (Control p = c; p != null; p = p.Parent) if (p.BackColor.A == 255) return p.BackColor;
            return Panel;
        }

        private static TextFormatFlags AlignFlags(ContentAlignment a)
        {
            TextFormatFlags f = TextFormatFlags.NoPrefix;
            switch (a)
            {
                case ContentAlignment.TopCenter: case ContentAlignment.MiddleCenter: case ContentAlignment.BottomCenter:
                    f |= TextFormatFlags.HorizontalCenter; break;
                case ContentAlignment.TopRight: case ContentAlignment.MiddleRight: case ContentAlignment.BottomRight:
                    f |= TextFormatFlags.Right; break;
            }
            switch (a)
            {
                case ContentAlignment.MiddleLeft: case ContentAlignment.MiddleCenter: case ContentAlignment.MiddleRight:
                    f |= TextFormatFlags.VerticalCenter; break;
                case ContentAlignment.BottomLeft: case ContentAlignment.BottomCenter: case ContentAlignment.BottomRight:
                    f |= TextFormatFlags.Bottom; break;
            }
            return f;
        }

        private static void Label_Paint(object sender, PaintEventArgs e)
        {
            Label l = (Label)sender;
            if (!Enabled || l.Enabled || l.Image != null || string.IsNullOrEmpty(l.Text)) return;
            Rectangle r = new Rectangle(l.Padding.Left, l.Padding.Top,
                l.ClientSize.Width - l.Padding.Horizontal, l.ClientSize.Height - l.Padding.Vertical);
            using (SolidBrush bg = new SolidBrush(EffectiveBack(l))) e.Graphics.FillRectangle(bg, l.ClientRectangle);
            TextFormatFlags f = AlignFlags(l.TextAlign);
            if (!l.AutoSize) f |= TextFormatFlags.WordBreak;
            if (l.UseMnemonic) f &= ~TextFormatFlags.NoPrefix;
            TextRenderer.DrawText(e.Graphics, l.Text, l.Font, r, TextDim, f);
        }

        private static void GroupBox_Paint(object sender, PaintEventArgs e)
        {
            GroupBox gb = (GroupBox)sender;
            if (!Enabled) return;
            Graphics g = e.Graphics;
            Size ts = TextRenderer.MeasureText(g, gb.Text, gb.Font);
            int top = ts.Height / 2;
            using (SolidBrush bg = new SolidBrush(gb.BackColor.A == 255 ? gb.BackColor : Panel))
                g.FillRectangle(bg, gb.ClientRectangle);
            using (Pen pen = new Pen(Border))
                g.DrawRectangle(pen, 0, top, gb.Width - 1, gb.Height - top - 1);
            if (!string.IsNullOrEmpty(gb.Text))
            {
                Rectangle tr = new Rectangle(6, 0, ts.Width, ts.Height);
                using (SolidBrush bg = new SolidBrush(gb.BackColor.A == 255 ? gb.BackColor : Panel))
                    g.FillRectangle(bg, tr);
                TextRenderer.DrawText(g, gb.Text, gb.Font, tr, gb.Enabled ? gb.ForeColor : TextDim,
                    TextFormatFlags.Left | TextFormatFlags.Top | TextFormatFlags.SingleLine);
            }
        }

        // Subclasses the native window: after Windows' own WM_PAINT, paints our version on top.
        private class OverPainter : NativeWindow
        {
            private const int WM_PAINT = 0x000F;
            private const int WM_NCPAINT = 0x0085;
            private readonly Control owner;

            private OverPainter(Control c)
            {
                owner = c;
                if (c.IsHandleCreated) AssignHandle(c.Handle);
                c.HandleCreated += (s, e) =>
                {
                    if (Handle != IntPtr.Zero) ReleaseHandle(); // recreated without a destroy notice
                    AssignHandle(((Control)s).Handle);
                };
                c.HandleDestroyed += (s, e) =>
                {
                    noPrintClient.Remove(((Control)s).Handle); // hwnd values are reused by Windows
                    ReleaseHandle();
                };
            }

            public static void Attach(Control c)
            {
                AttachOnce(c, "over", () => new OverPainter(c));
            }

            protected override void WndProc(ref Message m)
            {
                // Drop-down-list combos: we paint everything into one off-screen buffer and show it once.
                // Letting Windows + WinForms paint first (light frame, F0F0F0 arrow) caused a flash on hover.
                if (m.Msg == WM_PAINT && ThemeManager.Enabled && owner is DateTimePicker)
                {
                    if (PaintDateTimeBuffered((DateTimePicker)owner, Handle)) { m.Result = IntPtr.Zero; return; }
                }
                if (m.Msg == WM_NCPAINT && ThemeManager.Enabled && owner is DateTimePicker)
                {
                    base.WndProc(ref m);
                    PaintTextBorder(owner, Handle);
                    return;
                }
                ComboBox full = owner as ComboBox;
                if (m.Msg == WM_PAINT && ThemeManager.Enabled && full != null &&
                    full.DropDownStyle != ComboBoxStyle.Simple)
                {
                    if (PaintComboBuffered(full, Handle)) { m.Result = IntPtr.Zero; return; }
                }
                TextBoxBase tbb = owner as TextBoxBase;
                if (tbb != null && ThemeManager.Enabled)
                {
                    // Hover: the themed edit repaints its light "hot" frame, then we painted ours on top: flicker.
                    // Without scrollbars the frame is all ours, so Windows does not paint it first.
                    if (m.Msg == WM_NCPAINT && !HasScrollBars(tbb))
                    {
                        PaintTextBorder(owner, Handle);
                        m.Result = IntPtr.Zero;
                        return;
                    }
                    // Disabled single-line fields: Windows always uses its own gray (6D6D6D) and ignores ours.
                    if (m.Msg == WM_PAINT && !tbb.Enabled && !tbb.Multiline && PaintDisabledEdit(tbb, Handle))
                    {
                        m.Result = IntPtr.Zero;
                        return;
                    }
                }
                base.WndProc(ref m);
                if (!ThemeManager.Enabled) return;
                if (m.Msg == WM_NCPAINT && owner is TextBoxBase) { PaintTextBorder(owner, Handle); return; }
                if (m.Msg != WM_PAINT) return;
                try
                {
                    using (Graphics g = Graphics.FromHwnd(Handle))
                    {
                        TabControl t = owner as TabControl;
                        if (t != null) { PaintTabs(t, g); return; }
                        ComboBox cb = owner as ComboBox;
                        if (cb != null) { PaintComboButton(cb, g); return; }
                        UpDownBase ud = owner as UpDownBase;
                        if (ud != null) { PaintUpDownBorder(ud, g); return; }
                        // only the ▲▼ part: the inner edit box of a NumericUpDown is also a child of it
                        if (owner.Parent is UpDownBase && !(owner is TextBoxBase)) PaintUpDownButtons(owner, g);
                    }
                }
                catch (Exception) { /* cosmetic only */ }
            }
        }

        private static void PaintTabs(TabControl t, Graphics g)
        {
            if (t.Alignment != TabAlignment.Top) return;
            Rectangle client = t.ClientRectangle;
            Rectangle page = t.DisplayRectangle;
            using (Region r = new Region(client))
            {
                r.Exclude(page); // tab pages are child windows; never paint over them
                using (SolidBrush deep = new SolidBrush(TabStrip)) g.FillRegion(deep, r);
            }
            using (Pen frame = new Pen(Border))
                g.DrawRectangle(frame, page.X - 1, page.Y - 1, page.Width + 1, page.Height + 1);
            for (int i = 0; i < t.TabCount; i++) DrawTab(t, g, i);
        }

        private static void DrawTab(TabControl t, Graphics g, int i)
        {
            bool sel = i == t.SelectedIndex;
            Rectangle r = t.GetTabRect(i);
            if (sel) r = new Rectangle(r.X - 2, r.Y - 2, r.Width + 4, r.Height + 3); // selected tab is raised
            using (SolidBrush br = new SolidBrush(sel ? Panel : Raised)) g.FillRectangle(br, r);
            using (Pen p = new Pen(Border))
            {
                g.DrawLine(p, r.Left, r.Bottom - (sel ? 0 : 1), r.Left, r.Top);
                g.DrawLine(p, r.Left, r.Top, r.Right - 1, r.Top);
                g.DrawLine(p, r.Right - 1, r.Top, r.Right - 1, r.Bottom - (sel ? 0 : 1));
            }
            if (sel) using (SolidBrush ac = new SolidBrush(Accent)) g.FillRectangle(ac, r.Left + 1, r.Top + 1, r.Width - 2, 2);
            TextRenderer.DrawText(g, t.TabPages[i].Text, t.Font, r, sel ? Color.White : Text,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine);
        }

        [DllImport("user32.dll")] private static extern IntPtr GetWindowDC(IntPtr hWnd);
        [DllImport("user32.dll")] private static extern int GetWindowLong(IntPtr hWnd, int index);
        private const int GWL_STYLE = -16, GWL_EXSTYLE = -20, WS_BORDER = 0x00800000, WS_EX_CLIENTEDGE = 0x200;
        [DllImport("user32.dll")] private static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

        // TextBox borders live in the non-client area: draw ours after Windows draws its light one.
        private static bool HasScrollBars(TextBoxBase t)
        {
            TextBox tb = t as TextBox;
            if (tb != null) return tb.Multiline && tb.ScrollBars != ScrollBars.None;
            return t.Multiline;
        }

        private static bool PaintDisabledEdit(TextBoxBase t, IntPtr hwnd)
        {
            TextBox tb = t as TextBox;
            if (tb != null && (tb.PasswordChar != '\0' || tb.UseSystemPasswordChar)) return false;
            PAINTSTRUCT ps;
            IntPtr dc = BeginPaint(hwnd, out ps);
            try
            {
                using (Graphics g = Graphics.FromHdc(dc))
                {
                    Rectangle r = t.ClientRectangle;
                    Color back = t.BackColor.A == 255 ? t.BackColor : Surface;
                    using (SolidBrush b = new SolidBrush(back)) g.FillRectangle(b, r);
                    TextFormatFlags f = TextFormatFlags.TextBoxControl | TextFormatFlags.SingleLine | TextFormatFlags.NoPrefix;
                    HorizontalAlignment a = tb != null ? tb.TextAlign : HorizontalAlignment.Left;
                    if (a == HorizontalAlignment.Center) f |= TextFormatFlags.HorizontalCenter;
                    else if (a == HorizontalAlignment.Right) f |= TextFormatFlags.Right;
                    Rectangle tr = new Rectangle(r.X + 1, r.Y, Math.Max(0, r.Width - 2), r.Height);
                    TextRenderer.DrawText(g, t.Text, t.Font, tr, TextDim, back, f);
                }
            }
            catch (Exception) { }
            finally { EndPaint(hwnd, ref ps); }
            return true;
        }

        private static void PaintTextBorder(Control c, IntPtr hwnd)
        {
            TextBox tb = c as TextBox;
            if (tb != null && tb.BorderStyle == BorderStyle.None) return;
            if (c is DateTimePicker && (GetWindowLong(c.Handle, GWL_EXSTYLE) & WS_EX_CLIENTEDGE) == 0 &&
                (GetWindowLong(c.Handle, GWL_STYLE) & WS_BORDER) == 0) return;
            IntPtr dc = GetWindowDC(hwnd);
            if (dc == IntPtr.Zero) return;
            try
            {
                using (Graphics g = Graphics.FromHdc(dc))
                using (Pen p = new Pen(Border))
                using (Pen inner = new Pen(c.BackColor.A == 255 ? c.BackColor : Surface))
                {
                    g.DrawRectangle(p, 0, 0, c.Width - 1, c.Height - 1);
                    g.DrawRectangle(inner, 1, 1, c.Width - 3, c.Height - 3); // covers the 3D inner edge
                }
            }
            finally { ReleaseDC(hwnd, dc); }
        }

        private static void PaintUpDownBorder(UpDownBase ud, Graphics g)
        {
            if (ud.BorderStyle == BorderStyle.None) return;
            using (Pen p = new Pen(Border)) g.DrawRectangle(p, 0, 0, ud.Width - 1, ud.Height - 1);
        }

        private static void PaintUpDownButtons(Control buttons, Graphics g)
        {
            int w = buttons.Width, h = buttons.Height, half = h / 2;
            bool en = buttons.Parent == null || buttons.Parent.Enabled;
            using (SolidBrush b = new SolidBrush(Deep)) g.FillRectangle(b, 0, 0, w, h);
            using (Pen p = new Pen(Border))
            {
                g.DrawLine(p, 0, 0, 0, h);          // separator from the edit box
                g.DrawLine(p, 1, half, w, half);    // between ▲ and ▼
            }
            int cx = w / 2, a = Math.Max(2, Math.Min(w, half) / 4);
            System.Drawing.Drawing2D.SmoothingMode old = g.SmoothingMode;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            using (SolidBrush ab = new SolidBrush(en ? Text : TextDim))
            {
                int uy = half / 2, dy = half + (h - half) / 2;
                g.FillPolygon(ab, new[] { new Point(cx - a - 1, uy + a / 2 + 1), new Point(cx + a + 1, uy + a / 2 + 1), new Point(cx, uy - a / 2 - 1) });
                g.FillPolygon(ab, new[] { new Point(cx - a - 1, dy - a / 2 - 1), new Point(cx + a + 1, dy - a / 2 - 1), new Point(cx, dy + a / 2 + 1) });
            }
            g.SmoothingMode = old;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct PAINTSTRUCT
        {
            public IntPtr hdc; public bool fErase;
            public int left, top, right, bottom;
            public bool fRestore, fIncUpdate;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)] public byte[] rgbReserved;
        }
        [DllImport("user32.dll")] private static extern IntPtr BeginPaint(IntPtr hwnd, out PAINTSTRUCT ps);
        [DllImport("user32.dll")] private static extern bool EndPaint(IntPtr hwnd, ref PAINTSTRUCT ps);

        private static readonly MethodInfo onDrawItem =
            typeof(ComboBox).GetMethod("OnDrawItem", BindingFlags.NonPublic | BindingFlags.Instance);

        /// <summary>Whole drop-down-list combo painted off-screen, then shown in one blit. False = fall back.</summary>
        private static bool PaintComboBuffered(ComboBox cb, IntPtr hwnd)
        {
            int w = cb.Width, h = cb.Height;
            if (w <= 0 || h <= 0) return false;
            // Editable combos (DropDown) host a child Edit window that paints itself: leave its area alone.
            Rectangle edit = Rectangle.Empty;
            if (cb.DropDownStyle == ComboBoxStyle.DropDown)
            {
                COMBOBOXINFO info = new COMBOBOXINFO();
                info.cbSize = Marshal.SizeOf(typeof(COMBOBOXINFO));
                if (GetComboBoxInfo(hwnd, ref info))
                    edit = Rectangle.FromLTRB(info.itemL, info.itemT, info.itemR, info.itemB);
            }
            PAINTSTRUCT ps;
            IntPtr hdc = BeginPaint(hwnd, out ps);
            try
            {
                // Opaque buffer: on a 32bpp ARGB bitmap GDI ClearType blends the letter edges with black
                // (dark outline around white text on the orange/blue focus fill).
                using (Bitmap buf = new Bitmap(w, h, System.Drawing.Imaging.PixelFormat.Format24bppRgb))
                {
                    using (Graphics g = Graphics.FromImage(buf)) PaintComboContent(cb, g, edit.IsEmpty);
                    using (Graphics screen = Graphics.FromHdc(hdc))
                    {
                        if (!edit.IsEmpty) screen.SetClip(edit, System.Drawing.Drawing2D.CombineMode.Exclude);
                        screen.DrawImageUnscaled(buf, 0, 0);
                    }
                }
            }
            catch (Exception ex) { Debug("combo paint " + Describe(cb) + ": " + ex.Message); }
            finally { EndPaint(hwnd, ref ps); }
            return true;
        }

        private const int WM_PRINTCLIENT = 0x0318, PRF_CLIENT = 0x4, PRF_ERASEBKGND = 0x8;
        private static readonly HashSet<IntPtr> noPrintClient = new HashSet<IntPtr>();

        /// <summary>
        /// DateTimePicker has no dark theme and ignores BackColor. Ask it to paint itself into a buffer
        /// (WM_PRINTCLIENT), turn its grays dark (white -> Surface, black -> Text), keep real colors
        /// (selection blue), then show the result once. False = control did not cooperate: paint normally.
        /// </summary>
        private static bool PaintDateTimeBuffered(DateTimePicker dtp, IntPtr hwnd)
        {
            HookDateTimeSpin(dtp);
            if (noPrintClient.Contains(hwnd)) return false;
            Size cs = dtp.ClientSize;
            if (cs.Width <= 0 || cs.Height <= 0) return false;
            using (Bitmap buf = new Bitmap(cs.Width, cs.Height, PixelFormat.Format32bppArgb))
            {
                Color sentinel = Color.FromArgb(255, 1, 2, 3);
                using (Graphics g = Graphics.FromImage(buf))
                {
                    g.Clear(sentinel);
                    IntPtr hdc = g.GetHdc();
                    try { SendMessage(hwnd, WM_PRINTCLIENT, hdc, (IntPtr)(PRF_CLIENT | PRF_ERASEBKGND)); }
                    finally { g.ReleaseHdc(hdc); }
                }
                if (!RemapGrays(buf, sentinel))
                {
                    noPrintClient.Add(hwnd); // nothing was drawn: this control does not support it
                    return false;
                }
                PAINTSTRUCT ps;
                IntPtr dc = BeginPaint(hwnd, out ps);
                try { using (Graphics screen = Graphics.FromHdc(dc)) screen.DrawImageUnscaled(buf, 0, 0); }
                finally { EndPaint(hwnd, ref ps); }
            }
            return true;
        }

        // Everything is turned into an inverted gray by luminance (white -> Surface, black -> Text), which also
        // removes ClearType's colored fringes. Only the selection highlight (and a few px around it, so the
        // white digits on it stay white) keeps its original pixels.
        private static bool RemapGrays(Bitmap b, Color sentinel)
        {
            Rectangle r = new Rectangle(0, 0, b.Width, b.Height);
            BitmapData d = b.LockBits(r, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            bool drawn = false;
            try
            {
                int w = b.Width, h = b.Height, stride = d.Stride, n = stride * h;
                byte[] px = new byte[n];
                Marshal.Copy(d.Scan0, px, 0, n);
                // Selection highlight = solid runs (>= 4 px) of the exact highlight color. ClearType fringes are
                // isolated bluish pixels and never form such runs. Keep only the bounding box of those runs.
                Color hl = SystemColors.Highlight;
                int bx0 = int.MaxValue, by0 = int.MaxValue, bx1 = -1, by1 = -1;
                for (int y = 0; y < h; y++)
                {
                    int run = 0;
                    for (int x = 0; x < w; x++)
                    {
                        int i = y * stride + x * 4;
                        bool isHl = Math.Abs(px[i + 2] - hl.R) + Math.Abs(px[i + 1] - hl.G) + Math.Abs(px[i] - hl.B) < 24;
                        run = isHl ? run + 1 : 0;
                        if (run >= 4)
                        {
                            if (x - run + 1 < bx0) bx0 = x - run + 1;
                            if (x > bx1) bx1 = x;
                            if (y < by0) by0 = y;
                            if (y > by1) by1 = y;
                        }
                    }
                }
                bool hasHl = bx1 >= 0;
                int lo = Surface.R, hi = Text.R;
                for (int y = 0; y < h; y++)
                    for (int x = 0; x < w; x++)
                    {
                        int i = y * stride + x * 4;
                        int bl = px[i], gr = px[i + 1], rd = px[i + 2];
                        px[i + 3] = 255;
                        if (rd == sentinel.R && gr == sentinel.G && bl == sentinel.B)
                        { px[i] = (byte)Surface.B; px[i + 1] = (byte)Surface.G; px[i + 2] = (byte)Surface.R; continue; }
                        drawn = true;
                        if (hasHl && x >= bx0 && x <= bx1 && y >= by0 && y <= by1) continue; // selected field
                        int lum = (rd * 299 + gr * 587 + bl * 114) / 1000;
                        int nv = lo + (255 - lum) * (hi - lo) / 255;
                        px[i] = px[i + 1] = px[i + 2] = (byte)nv;
                    }
                Marshal.Copy(px, 0, d.Scan0, n);
            }
            finally { b.UnlockBits(d); }
            return drawn;
        }

        // The ▲▼ of a DateTimePicker (ShowUpDown) is a native child window (msctls_updown32) that paints itself.
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern IntPtr FindWindowEx(IntPtr parent, IntPtr after, string cls, string title);
        [DllImport("user32.dll")] private static extern bool GetClientRect(IntPtr h, out RECT r);
        [StructLayout(LayoutKind.Sequential)] private struct RECT { public int Left, Top, Right, Bottom; }
        private static readonly Dictionary<IntPtr, NativeWindow> spinPainters = new Dictionary<IntPtr, NativeWindow>();

        private static void HookDateTimeSpin(DateTimePicker dtp)
        {
            if (!dtp.IsHandleCreated) return;
            IntPtr spin = FindWindowEx(dtp.Handle, IntPtr.Zero, "msctls_updown32", null);
            if (spin == IntPtr.Zero || spinPainters.ContainsKey(spin)) return;
            SpinPainter sp = new SpinPainter(dtp);
            sp.AssignHandle(spin);
            spinPainters[spin] = sp;
            dtp.HandleDestroyed += (s, e) => { sp.ReleaseHandle(); spinPainters.Remove(spin); };
        }

        private class SpinPainter : NativeWindow
        {
            private const int WM_PAINT = 0x000F, WM_ERASEBKGND = 0x0014;
            private readonly Control owner;
            private readonly bool horizontal;
            public SpinPainter(Control o, bool horz = false) { owner = o; horizontal = horz; }

            protected override void WndProc(ref Message m)
            {
                if (ThemeManager.Enabled && m.Msg == WM_ERASEBKGND) { m.Result = (IntPtr)1; return; }
                if (ThemeManager.Enabled && m.Msg == WM_PAINT)
                {
                    RECT rc;
                    GetClientRect(Handle, out rc);
                    int w = rc.Right - rc.Left, h = rc.Bottom - rc.Top;
                    PAINTSTRUCT ps;
                    IntPtr dc = BeginPaint(Handle, out ps);
                    try
                    {
                        if (w > 0 && h > 0)
                            using (Bitmap buf = new Bitmap(w, h))
                            {
                                using (Graphics g = Graphics.FromImage(buf))
                                {
                                    if (horizontal) DrawSpinHorizontal(g, w, h, owner.Enabled);
                                    else DrawSpin(g, w, h, owner.Enabled);
                                }
                                using (Graphics screen = Graphics.FromHdc(dc)) screen.DrawImageUnscaled(buf, 0, 0);
                            }
                    }
                    catch (Exception) { }
                    finally { EndPaint(Handle, ref ps); }
                    m.Result = IntPtr.Zero;
                    return;
                }
                base.WndProc(ref m);
            }
        }

        private static void DrawSpin(Graphics g, int w, int h, bool en)
        {
            int half = h / 2;
            using (SolidBrush b = new SolidBrush(Deep)) g.FillRectangle(b, 0, 0, w, h);
            using (Pen p = new Pen(Border))
            {
                g.DrawLine(p, 0, 0, 0, h);
                g.DrawLine(p, 1, half, w, half);
            }
            int cx = w / 2, a = Math.Max(2, Math.Min(w, half) / 4);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            using (SolidBrush ab = new SolidBrush(en ? Text : TextDim))
            {
                int uy = half / 2, dy = half + (h - half) / 2;
                g.FillPolygon(ab, new[] { new Point(cx - a - 1, uy + a / 2 + 1), new Point(cx + a + 1, uy + a / 2 + 1), new Point(cx, uy - a / 2 - 1) });
                g.FillPolygon(ab, new[] { new Point(cx - a - 1, dy - a / 2 - 1), new Point(cx + a + 1, dy - a / 2 - 1), new Point(cx, dy + a / 2 + 1) });
            }
        }

        // ◄ ► scroller a TabControl creates (msctls_updown32, horizontal) when its tabs do not fit.
        private static void HookTabSpin(TabControl t)
        {
            if (!t.IsHandleCreated || t.Multiline) return;
            IntPtr spin = FindWindowEx(t.Handle, IntPtr.Zero, "msctls_updown32", null);
            if (spin == IntPtr.Zero || spinPainters.ContainsKey(spin)) return;
            SpinPainter sp = new SpinPainter(t, true);
            sp.AssignHandle(spin);
            spinPainters[spin] = sp;
            t.HandleDestroyed += (s, e) => { sp.ReleaseHandle(); spinPainters.Remove(spin); };
            InvalidateRect(spin, IntPtr.Zero, true);
        }
        [DllImport("user32.dll")] private static extern bool InvalidateRect(IntPtr h, IntPtr r, bool erase);

        private static void DrawSpinHorizontal(Graphics g, int w, int h, bool en)
        {
            int half = w / 2;
            using (SolidBrush b = new SolidBrush(Raised)) g.FillRectangle(b, 0, 0, w, h);
            using (Pen p = new Pen(Border))
            {
                g.DrawRectangle(p, 0, 0, w - 1, h - 1);
                g.DrawLine(p, half, 0, half, h);
            }
            int cy = h / 2, a = Math.Max(2, Math.Min(half, h) / 4);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            using (SolidBrush ab = new SolidBrush(en ? Text : TextDim))
            {
                int lx = half / 2, rx = half + (w - half) / 2;
                g.FillPolygon(ab, new[] { new Point(lx + a / 2 + 1, cy - a - 1), new Point(lx + a / 2 + 1, cy + a + 1), new Point(lx - a / 2 - 1, cy) });
                g.FillPolygon(ab, new[] { new Point(rx - a / 2 - 1, cy - a - 1), new Point(rx - a / 2 - 1, cy + a + 1), new Point(rx + a / 2 + 1, cy) });
            }
        }

        private static void PaintComboContent(ComboBox cb, Graphics g, bool drawItem)
        {
            int bw = SystemInformation.VerticalScrollBarWidth;
            Rectangle client = new Rectangle(0, 0, cb.Width, cb.Height);
            Rectangle item = new Rectangle(2, 2, Math.Max(0, cb.Width - bw - 5), Math.Max(0, cb.Height - 4));
            bool swatch = IsExcluded(cb);              // color picker: shows the chosen color
            bool focused = cb.Focused && !cb.DroppedDown;
            Color back = !cb.Enabled ? (swatch ? Deep : Surface) : cb.BackColor;

            using (SolidBrush b = new SolidBrush(back)) g.FillRectangle(b, client);

            if (drawItem && !(swatch && !cb.Enabled))
            {
                if (cb.DrawMode != DrawMode.Normal && onDrawItem != null)
                {
                    // Let the editor draw the selected item (thumbnail + our text fix) into the buffer.
                    DrawItemState st = DrawItemState.ComboBoxEdit;
                    if (focused) st |= DrawItemState.Focus | DrawItemState.Selected;
                    if (!cb.Enabled) st |= DrawItemState.Disabled;
                    Color ib = focused ? Selected : back;
                    using (SolidBrush b = new SolidBrush(ib)) g.FillRectangle(b, item);
                    DrawItemEventArgs e = new DrawItemEventArgs(g, cb.Font, item, cb.SelectedIndex, st,
                        focused ? Color.White : cb.ForeColor, ib);
                    try { onDrawItem.Invoke(cb, new object[] { e }); } catch (Exception) { }
                }
                else if (!swatch)
                {
                    if (focused) using (SolidBrush sb = new SolidBrush(Selected)) g.FillRectangle(sb, item);
                    TextRenderer.DrawText(g, cb.Text, cb.Font, new Rectangle(item.X + 1, item.Y, item.Width - 1, item.Height),
                        !cb.Enabled ? TextDim : (focused ? Color.White : cb.ForeColor), focused ? Selected : back,
                        TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine |
                        TextFormatFlags.NoPrefix | TextFormatFlags.EndEllipsis);
                }
            }
            PaintComboButton(cb, g, true);
        }

        private static void PaintComboButton(ComboBox cb, Graphics g)
        {
            PaintComboButton(cb, g, false);
        }

        private static void PaintComboButton(ComboBox cb, Graphics g, bool buffered)
        {
            if (cb.DropDownStyle == ComboBoxStyle.Simple) return;
            // A disabled color swatch shows a color that is not in use: hide it.
            if (!buffered && !cb.Enabled && IsExcluded(cb))
                using (SolidBrush d = new SolidBrush(Deep)) g.FillRectangle(d, cb.ClientRectangle);
            int w = SystemInformation.VerticalScrollBarWidth;
            Rectangle btn = new Rectangle(cb.Width - w - 1, 1, w, cb.Height - 2);
            using (SolidBrush b = new SolidBrush(Deep)) g.FillRectangle(b, btn);
            using (Pen p = new Pen(Border))
            {
                g.DrawRectangle(p, 0, 0, cb.Width - 1, cb.Height - 1);
                g.DrawLine(p, btn.Left - 1, 1, btn.Left - 1, cb.Height - 2);
            }
            int cx = btn.Left + btn.Width / 2, cy = btn.Top + btn.Height / 2;
            int a = Math.Max(3, w / 5);
            Point[] arrow = { new Point(cx - a, cy - a / 2), new Point(cx + a + 1, cy - a / 2), new Point(cx, cy + a / 2 + 1) };
            System.Drawing.Drawing2D.SmoothingMode old = g.SmoothingMode;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            using (SolidBrush ab = new SolidBrush(cb.Enabled ? Text : TextDim)) g.FillPolygon(ab, arrow);
            g.SmoothingMode = old;
        }

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);
        private const int WM_SETREDRAW = 0x000B;
        private static bool recoloring;

        // New RTF (language change) arrives with the auto color: paint all text white again.
        private static void RecolorRich(RichTextBox r)
        {
            if (!Enabled || recoloring || r.TextLength == 0 || !r.IsHandleCreated && r.Parent == null) return;
            recoloring = true;
            int start = r.SelectionStart, len = r.SelectionLength;
            bool redraw = r.IsHandleCreated;
            try
            {
                if (redraw) SendMessage(r.Handle, WM_SETREDRAW, IntPtr.Zero, IntPtr.Zero);
                r.SelectAll();
                r.SelectionColor = TipsText;
                r.Select(start, len);
            }
            finally
            {
                if (redraw)
                {
                    SendMessage(r.Handle, WM_SETREDRAW, (IntPtr)1, IntPtr.Zero);
                    r.Invalidate();
                }
                recoloring = false;
            }
        }

        // ---------------------------------------------------------------- tabs

        private static void ThemeTabControl(TabControl t)
        {
            OverPainter.Attach(t);
            AttachOnce(t, "pages", () => t.SelectedIndexChanged += (s, e) =>
            {
                foreach (TabPage tp in ((TabControl)s).TabPages) SpecialBack(tp);
            });
            foreach (TabPage tp in t.TabPages) SpecialBack(tp);
            // The scroller only exists once the tabs overflow, so look for it again after layout changes.
            AttachOnce(t, "tabspin", () =>
            {
                t.Layout += (s, e) => HookTabSpin((TabControl)s);
                t.ControlAdded += (s, e) => HookTabSpin((TabControl)s);
            });
            HookTabSpin(t);
            if (t.DrawMode == TabDrawMode.OwnerDrawFixed) return; // already custom
            t.DrawMode = TabDrawMode.OwnerDrawFixed;
            t.DrawItem += (s, e) =>
            {
                TabControl tc = (TabControl)s;
                if (e.Index < 0 || e.Index >= tc.TabPages.Count) return;
                bool sel = e.Index == tc.SelectedIndex;
                Rectangle r = e.Bounds;
                // same colors as DrawTab (painted right after), so nothing changes between the two passes
                using (SolidBrush br = new SolidBrush(sel ? Panel : Raised)) e.Graphics.FillRectangle(br, r);
                TextRenderer.DrawText(e.Graphics, tc.TabPages[e.Index].Text, tc.Font, r, sel ? Color.White : Text,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine);
            };
        }

        // ---------------------------------------------------------------- grid

        private static void ThemeGrid(DataGridView g)
        {
            g.EnableHeadersVisualStyles = false;
            // Flat 1px header separators (GridColor) instead of the thick 3D "Raised" bevel.
            g.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            g.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            g.BackgroundColor = Workspace;
            g.GridColor = Border;
            g.DefaultCellStyle.BackColor = Surface;
            g.DefaultCellStyle.ForeColor = Text;
            g.DefaultCellStyle.SelectionBackColor = Selected;
            g.DefaultCellStyle.SelectionForeColor = Color.White;
            g.ColumnHeadersDefaultCellStyle.BackColor = Raised;
            g.ColumnHeadersDefaultCellStyle.ForeColor = Text;
            g.ColumnHeadersDefaultCellStyle.SelectionBackColor = Selected; // was the system blue (Highlight)
            g.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.White;
            g.RowHeadersDefaultCellStyle.SelectionBackColor = Selected;
            g.RowHeadersDefaultCellStyle.SelectionForeColor = Color.White;
            AttachOnce(g, "checkcells", () => g.CellPainting += Grid_CellPainting);
            g.RowHeadersDefaultCellStyle.BackColor = Raised;
            g.RowHeadersDefaultCellStyle.ForeColor = Text;
            // The author colors one preview column on purpose (black in the light theme) so each image is seen
            // on a dark and on a light background. In dark mode: that column stays black (the watch screen) and its
            // partner becomes a light grey instead of white, so the check works without a big bright block.
            bool pair = false;
            foreach (DataGridViewColumn c0 in g.Columns)
                if (c0 is DataGridViewImageColumn && IsSampleColor(c0.DefaultCellStyle.BackColor)) pair = true;
            foreach (DataGridViewColumn col in g.Columns)
            {
                if (!(col is DataGridViewImageColumn)) continue; // the images themselves are never touched
                Color own = col.DefaultCellStyle.BackColor;
                if (IsSampleColor(own))
                {
                    col.DefaultCellStyle.BackColor = ImageDark;
                    col.DefaultCellStyle.ForeColor = Color.White;
                }
                else if (Key(own) == Key(ImageLight) || Key(own) == Key(ImageCell))
                {
                    // already themed in an earlier pass: keep it
                }
                else
                {
                    col.DefaultCellStyle.BackColor = pair ? ImageLight : ImageCell;
                    if (pair) col.DefaultCellStyle.ForeColor = Color.Black;
                }
                col.DefaultCellStyle.SelectionBackColor = Selected;
            }
        }

        // Check box cells are drawn by Windows with the system accent (blue): draw them like our check boxes.
        private static void Grid_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (!Enabled || e.RowIndex < 0 || e.ColumnIndex < 0) return;
            DataGridView g = (DataGridView)sender;
            if (!(g.Columns[e.ColumnIndex] is DataGridViewCheckBoxColumn)) return;
            try
            {
                e.PaintBackground(e.ClipBounds, true);
                object v = e.FormattedValue;
                bool on = false, mixed = false;
                if (v is bool) on = (bool)v;
                else if (v is CheckState) { on = (CheckState)v == CheckState.Checked; mixed = (CheckState)v == CheckState.Indeterminate; }
                bool en = g.Enabled;
                int size = Math.Min(13, Math.Max(8, e.CellBounds.Height - 6));
                Rectangle r = new Rectangle(e.CellBounds.X + (e.CellBounds.Width - size) / 2,
                                            e.CellBounds.Y + (e.CellBounds.Height - size) / 2, size - 1, size - 1);
                Graphics gr = e.Graphics;
                using (SolidBrush fill = new SolidBrush(Deep)) gr.FillRectangle(fill, r);
                using (Pen pen = new Pen(Border)) gr.DrawRectangle(pen, r);
                if (mixed)
                {
                    using (SolidBrush ab = new SolidBrush(en ? Accent : TextDim)) gr.FillRectangle(ab, Rectangle.Inflate(r, -3, -3));
                }
                else if (on)
                {
                    var old = gr.SmoothingMode;
                    gr.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    float w = r.Width, h = r.Height;
                    PointF[] tick =
                    {
                        new PointF(r.X + w * 0.22f, r.Y + h * 0.52f),
                        new PointF(r.X + w * 0.42f, r.Y + h * 0.72f),
                        new PointF(r.X + w * 0.78f, r.Y + h * 0.30f)
                    };
                    using (Pen tp = new Pen(en ? Accent : TextDim, Math.Max(1.6f, w / 7f)))
                    {
                        tp.StartCap = System.Drawing.Drawing2D.LineCap.Round;
                        tp.EndCap = System.Drawing.Drawing2D.LineCap.Round;
                        tp.LineJoin = System.Drawing.Drawing2D.LineJoin.Round;
                        gr.DrawLines(tp, tick);
                    }
                    gr.SmoothingMode = old;
                }
                e.Handled = true;
            }
            catch (Exception) { /* fall back to the default drawing */ }
        }

        // black (author's light theme) or white (author's own code when Windows itself is dark)
        private static bool IsSampleColor(Color c)
        {
            if (c.IsEmpty) return false;
            int k = c.ToArgb();
            return k == Color.Black.ToArgb() || k == Color.White.ToArgb() || k == ImageDark.ToArgb();
        }

        // ---------------------------------------------------------------- menus

        private static void ThemeToolStrip(ToolStrip ts)
        {
            object o;
            if (hooked.TryGetValue(ts, out o)) return;
            hooked.Add(ts, Marker);
            ts.Renderer = new ToolStripProfessionalRenderer(new DarkColorTable());
            ts.BackColor = Panel;
            ts.ForeColor = Text;
            ThemeItems(ts.Items);
            ts.ItemAdded += (s, e) => ThemeItem(e.Item);
            ToolStripDropDown menu = ts as ToolStripDropDown;
            if (menu != null) menu.Opening += (s, e) => ThemeItems(((ToolStrip)s).Items); // items rebuilt dynamically
        }

        private static void ThemeItems(ToolStripItemCollection items)
        {
            foreach (ToolStripItem it in items) ThemeItem(it);
        }

        private static void ThemeItem(ToolStripItem it)
        {
            it.ForeColor = Text;
            { Image sw = Swap(it.Image); if (!ReferenceEquals(sw, it.Image)) it.Image = sw; }
            ToolStripDropDownItem dd = it as ToolStripDropDownItem;
            if (dd != null && dd.HasDropDownItems) ThemeToolStrip(dd.DropDown);
        }

        private class DarkColorTable : ProfessionalColorTable
        {
            public override Color ToolStripDropDownBackground { get { return Panel; } }
            public override Color MenuBorder { get { return Border; } }
            public override Color MenuItemBorder { get { return Border; } }
            public override Color MenuItemSelected { get { return Hover; } }
            public override Color MenuItemSelectedGradientBegin { get { return Hover; } }
            public override Color MenuItemSelectedGradientEnd { get { return Hover; } }
            public override Color MenuItemPressedGradientBegin { get { return Pressed; } }
            public override Color MenuItemPressedGradientEnd { get { return Pressed; } }
            public override Color ImageMarginGradientBegin { get { return Panel; } }
            public override Color ImageMarginGradientMiddle { get { return Panel; } }
            public override Color ImageMarginGradientEnd { get { return Panel; } }
            public override Color SeparatorDark { get { return Border; } }
            public override Color SeparatorLight { get { return Panel; } }
            public override Color ToolStripBorder { get { return Border; } }
            public override Color ToolStripGradientBegin { get { return Panel; } }
            public override Color ToolStripGradientMiddle { get { return Panel; } }
            public override Color ToolStripGradientEnd { get { return Panel; } }
        }

        // ---------------------------------------------------------------- custom scrollbars
        // Native scrollbars only come in the system palette. For the editor's main lists we cover the
        // native vertical bar with our own (a sibling control on top of it) that reads and drives the real
        // scroll position. Targets inside ControlLibrary controls are skipped: the overlay is added to the
        // target's parent, and only Form1 containers are known not to be iterated by the editor's code.
        private class ScrollOverlay : Control
        {
            private enum Kind { Panel, Grid, RichText }

            [StructLayout(LayoutKind.Sequential)]
            private struct SCROLLINFO { public int cbSize; public uint fMask; public int nMin, nMax; public uint nPage; public int nPos, nTrackPos; }
            [StructLayout(LayoutKind.Sequential)]
            private struct SCROLLBARINFO
            {
                public int cbSize, left, top, right, bottom, dxyLineButton, xyThumbTop, xyThumbBottom, reserved;
                [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)] public int[] rgstate;
            }
            [DllImport("user32.dll")] private static extern bool GetScrollInfo(IntPtr h, int bar, ref SCROLLINFO si);
            [DllImport("user32.dll")] private static extern bool GetScrollBarInfo(IntPtr h, int idObject, ref SCROLLBARINFO sbi);
            private const int SB_VERT = 1, SIF_ALL = 0x17, OBJID_VSCROLL = unchecked((int)0xFFFFFFFB);
            private const int STATE_INVISIBLE = 0x8000, STATE_OFFSCREEN = 0x10000;
            private const int WM_VSCROLL = 0x0115, SB_THUMBPOSITION = 4, SB_ENDSCROLL = 8, WM_MOUSEWHEEL = 0x020A;

            private static readonly List<ScrollOverlay> all = new List<ScrollOverlay>();
            private static Timer timer;

            private readonly Control target;
            private readonly Kind kind;
            private int min, max, page, pos;      // Win32 semantics: last position = max - page + 1
            private bool hot, dragging;
            private int dragY, dragPos;

            public static void Register(Control c)
            {
                if (!ThemeManager.Enabled || c is Form || c is ScrollOverlay) return;
                Kind k;
                if (c is DataGridView) k = Kind.Grid;
                else if (c is RichTextBox && IsTips(c)) k = Kind.RichText;
                else if (c is ScrollableControl && ((ScrollableControl)c).AutoScroll && !(c is UpDownBase)) k = Kind.Panel;
                else return;
                for (Control p = c; p != null; p = p.Parent)
                    if (p.GetType().Assembly == typeof(ControlLibrary.AppUtils).Assembly) return;
                AttachOnce(c, "vscroll", () => all.Add(new ScrollOverlay(c, k)));
                if (timer == null)
                {
                    timer = new Timer();
                    timer.Interval = 40;
                    timer.Tick += (s, e) => SyncAll();
                    timer.Start();
                }
            }

            private ScrollOverlay(Control t, Kind k)
            {
                target = t; kind = k;
                Tag = "no-theme";
                Name = "darkScroll_" + t.Name;
                Visible = false;
                TabStop = false;
                SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint |
                         ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
                SetStyle(ControlStyles.Selectable, false);
            }

            private static void SyncAll()
            {
                for (int i = all.Count - 1; i >= 0; i--)
                {
                    ScrollOverlay o = all[i];
                    try
                    {
                        if (o.target.IsDisposed) { all.RemoveAt(i); o.Dispose(); continue; }
                        o.Sync();
                    }
                    catch (Exception ex) { Debug("scroll overlay " + o.Name + ": " + ex.Message); all.RemoveAt(i); o.Dispose(); }
                }
            }

            // Reads the real scrollbar: visible?, where (screen), and range/page/position.
            private bool Read(out Rectangle screen, out int mn, out int mx, out int pg, out int ps)
            {
                screen = Rectangle.Empty; mn = mx = pg = ps = 0;
                if (!target.IsHandleCreated || !target.Visible) return false;
                if (kind == Kind.Grid)
                {
                    VScrollBar vsb = null;
                    foreach (Control ch in target.Controls) if (ch is VScrollBar) { vsb = (VScrollBar)ch; break; }
                    if (vsb == null || !vsb.Visible || vsb.Width <= 0) return false;
                    screen = vsb.RectangleToScreen(vsb.ClientRectangle);
                    mn = vsb.Minimum; mx = vsb.Maximum; pg = Math.Max(1, vsb.LargeChange); ps = vsb.Value;
                    return true;
                }
                SCROLLBARINFO sbi = new SCROLLBARINFO();
                sbi.cbSize = Marshal.SizeOf(typeof(SCROLLBARINFO));
                sbi.rgstate = new int[6];
                if (!GetScrollBarInfo(target.Handle, OBJID_VSCROLL, ref sbi)) return false;
                if ((sbi.rgstate[0] & (STATE_INVISIBLE | STATE_OFFSCREEN)) != 0) return false;
                screen = Rectangle.FromLTRB(sbi.left, sbi.top, sbi.right, sbi.bottom);
                if (screen.Width <= 0 || screen.Height <= 0) return false;
                SCROLLINFO si = new SCROLLINFO();
                si.cbSize = Marshal.SizeOf(typeof(SCROLLINFO));
                si.fMask = SIF_ALL;
                if (!GetScrollInfo(target.Handle, SB_VERT, ref si)) return false;
                mn = si.nMin; mx = si.nMax; pg = Math.Max(1, (int)si.nPage); ps = si.nPos;
                return true;
            }

            private void Sync()
            {
                Control parent = target.Parent;
                Rectangle screen = Rectangle.Empty; int mn = 0, mx = 0, pg = 0, ps = 0;
                bool show = parent != null && Read(out screen, out mn, out mx, out pg, out ps);
                if (!show) { if (Visible) Visible = false; return; }
                if (Parent != parent) { parent.Controls.Add(this); } // only from the timer: never during a tree walk
                Rectangle r = parent.RectangleToClient(screen);
                if (Bounds != r) Bounds = r;
                if (parent.Controls.GetChildIndex(this) != 0) BringToFront();
                bool changed = mn != min || mx != max || pg != page || (!dragging && ps != pos);
                min = mn; max = mx; page = pg;
                if (!dragging) pos = ps;
                if (!Visible) Visible = true;
                if (changed) Invalidate();
            }

            private int LastPos { get { return Math.Max(min, max - page + 1); } }

            private Rectangle ThumbRect()
            {
                int track = Height - 4;
                int range = max - min + 1;
                if (range <= 0 || page >= range) return new Rectangle(2, 2, Width - 4, track);
                int len = Math.Max(24, (int)((long)track * page / range));
                len = Math.Min(len, track);
                int span = LastPos - min;
                int top = span <= 0 ? 0 : (int)((long)(track - len) * (pos - min) / span);
                return new Rectangle(2, 2 + top, Width - 4, len);
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                Graphics g = e.Graphics;
                using (SolidBrush tr = new SolidBrush(ScrollTrack)) g.FillRectangle(tr, ClientRectangle);
                Rectangle t = ThumbRect();
                if (t.Width <= 0 || t.Height <= 0) return;
                Color c = dragging ? ScrollThumbDrag : (hot ? ScrollThumbHot : ScrollThumb);
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                int rad = Math.Min(t.Width, 8);
                using (System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath())
                using (SolidBrush b = new SolidBrush(c))
                {
                    path.AddArc(t.X, t.Y, rad, rad, 180, 90);
                    path.AddArc(t.Right - rad, t.Y, rad, rad, 270, 90);
                    path.AddArc(t.Right - rad, t.Bottom - rad, rad, rad, 0, 90);
                    path.AddArc(t.X, t.Bottom - rad, rad, rad, 90, 90);
                    path.CloseFigure();
                    g.FillPath(b, path);
                }
            }

            private void ScrollTo(int p)
            {
                p = Math.Max(min, Math.Min(LastPos, p));
                if (p == pos && !dragging) return;
                pos = p;
                if (kind == Kind.Panel)
                {
                    ScrollableControl sc = (ScrollableControl)target;
                    sc.AutoScrollPosition = new Point(-sc.AutoScrollPosition.X, p);
                }
                else if (kind == Kind.RichText)
                {
                    int p16 = Math.Min(p, 0xFFFF); // WM_VSCROLL carries a 16-bit position
                    SendMessage(target.Handle, WM_VSCROLL, (IntPtr)((p16 << 16) | SB_THUMBPOSITION), IntPtr.Zero);
                    SendMessage(target.Handle, WM_VSCROLL, (IntPtr)SB_ENDSCROLL, IntPtr.Zero);
                }
                else
                {
                    DataGridView g = (DataGridView)target;
                    int acc = 0, idx = -1;
                    foreach (DataGridViewRow row in g.Rows)
                    {
                        if (!row.Visible || row.Frozen) continue;
                        idx = row.Index;
                        if (acc + row.Height > p) break;
                        acc += row.Height;
                    }
                    if (idx >= 0 && g.FirstDisplayedScrollingRowIndex != idx) g.FirstDisplayedScrollingRowIndex = idx;
                }
                Invalidate();
            }

            protected override void OnMouseDown(MouseEventArgs e)
            {
                base.OnMouseDown(e);
                if (e.Button != MouseButtons.Left) return;
                Rectangle t = ThumbRect();
                if (t.Contains(e.Location)) { dragging = true; dragY = e.Y; dragPos = pos; Capture = true; Invalidate(); }
                else if (e.Y < t.Top) ScrollTo(pos - page);
                else ScrollTo(pos + page);
            }

            protected override void OnMouseMove(MouseEventArgs e)
            {
                base.OnMouseMove(e);
                bool h = ThumbRect().Contains(e.Location);
                if (h != hot) { hot = h; Invalidate(); }
                if (!dragging) return;
                Rectangle t = ThumbRect();
                int free = (Height - 4) - t.Height;
                if (free <= 0) return;
                ScrollTo(dragPos + (int)((long)(e.Y - dragY) * (LastPos - min) / free));
            }

            protected override void OnMouseUp(MouseEventArgs e)
            {
                base.OnMouseUp(e);
                if (dragging) { dragging = false; Capture = false; Invalidate(); }
            }

            protected override void OnMouseLeave(EventArgs e)
            {
                base.OnMouseLeave(e);
                if (hot) { hot = false; Invalidate(); }
            }

            protected override void WndProc(ref Message m)
            {
                // the wheel over our bar scrolls the real control
                if (m.Msg == WM_MOUSEWHEEL && target.IsHandleCreated)
                {
                    SendMessage(target.Handle, WM_MOUSEWHEEL, m.WParam, m.LParam);
                    return;
                }
                base.WndProc(ref m);
            }
        }

        // ---------------------------------------------------------------- diagnostics

        private static readonly System.Text.StringBuilder debugLog = new System.Text.StringBuilder();

        private static int debugLines;

        private static void Debug(string line)
        {
            if (++debugLines > 300) return; // a paint-time error must not grow memory forever
            debugLog.AppendLine(line);
            Logger.WriteLine("DarkTheme: " + line);
        }

        private static string Describe(Control c)
        {
            return c.GetType().Name + " '" + c.Name + "'";
        }

        private static string HexOf(Color c)
        {
            return (c.IsNamedColor ? c.Name + "=" : "") + (c.ToArgb() & 0xFFFFFF).ToString("X6") + (c.A < 255 ? " a" + c.A : "");
        }

        private static void WriteDebugDump(Control root)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendLine("DarkTheme debug " + DateTime.Now);
            sb.AppendLine("icons loaded: " + darkByPixels.Count);
            sb.AppendLine("--- errors/notes ---");
            sb.Append(debugLog);
            sb.AppendLine("--- buttons / tabs / grids ---");
            DumpTree(root, sb, 0);
            try { File.WriteAllText(Path.Combine(Application.StartupPath, "DarkTheme_debug.txt"), sb.ToString()); }
            catch (Exception) { }
        }

        private static void DumpTree(Control c, System.Text.StringBuilder sb, int depth)
        {
            object o;
            bool hk = hooked.TryGetValue(c, out o);
            if (c is ButtonBase || c is TabControl || c is TabPage || c is GroupBox || c is Panel ||
                c is DataGridView || c is ComboBox || c is UpDownBase || !hk)
            {
                sb.Append(new string(' ', depth * 2)).Append(Describe(c))
                  .Append(" hooked=").Append(hk)
                  .Append(" back=").Append(HexOf(c.BackColor))
                  .Append(" fore=").Append(HexOf(c.ForeColor));
                ButtonBase b = c as ButtonBase;
                if (b != null) sb.Append(" flat=").Append(b.FlatStyle).Append(" uvs=").Append(b.UseVisualStyleBackColor).Append(" en=").Append(b.Enabled);
                TabControl t = c as TabControl;
                if (t != null) sb.Append(" draw=").Append(t.DrawMode).Append(" appearance=").Append(t.Appearance);
                ComboBox cbx = c as ComboBox;
                if (cbx != null) sb.Append(" flat=").Append(cbx.FlatStyle).Append(" draw=").Append(cbx.DrawMode).Append(" style=").Append(cbx.DropDownStyle);
                if (c is UpDownBase) foreach (Control part in c.Controls) sb.Append(" [").Append(part.GetType().Name).Append(" back=").Append(HexOf(part.BackColor)).Append("]");
                if (IsExcluded(c)) sb.Append(" EXCLUDED");
                sb.AppendLine();
            }
            foreach (Control ch in c.Controls) DumpTree(ch, sb, depth + 1);
        }

        // ---------------------------------------------------------------- native (title bar, scrollbars)

        [DllImport("dwmapi.dll")]
        private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int value, int size);

        [DllImport("uxtheme.dll", CharSet = CharSet.Unicode)]
        private static extern int SetWindowTheme(IntPtr hwnd, string appName, string idList);

        // Undocumented uxtheme ordinals (used by Explorer, Notepad++...). Only on Windows 10 1809+ (build 17763),
        // where they exist with these signatures; on older systems the ordinals mean something else.
        [DllImport("uxtheme.dll", EntryPoint = "#135")] private static extern int SetPreferredAppMode(int mode);
        [DllImport("uxtheme.dll", EntryPoint = "#133")] private static extern bool AllowDarkModeForWindow(IntPtr hwnd, bool allow);
        [DllImport("uxtheme.dll", EntryPoint = "#104")] private static extern void RefreshImmersiveColorPolicyState();
        [DllImport("ntdll.dll")] private static extern int RtlGetVersion(ref OSVERSIONINFOEX info);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        private struct OSVERSIONINFOEX
        {
            public int dwOSVersionInfoSize, dwMajorVersion, dwMinorVersion, dwBuildNumber, dwPlatformId;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)] public string szCSDVersion;
            public short wServicePackMajor, wServicePackMinor, wSuiteMask;
            public byte wProductType, wReserved;
        }

        private static bool nativeDark;

        private static void EnableNativeDarkMode()
        {
            try
            {
                // Environment.OSVersion lies without an app manifest; RtlGetVersion tells the real build.
                OSVERSIONINFOEX v = new OSVERSIONINFOEX();
                v.dwOSVersionInfoSize = Marshal.SizeOf(typeof(OSVERSIONINFOEX));
                if (RtlGetVersion(ref v) != 0 || v.dwMajorVersion < 10 || v.dwBuildNumber < 17763) return;
                SetPreferredAppMode(2); // 1903+: ForceDark. 1809: AllowDarkModeForApp(true)
                RefreshImmersiveColorPolicyState();
                nativeDark = true;
                Debug("native dark mode on (build " + v.dwBuildNumber + ")");
            }
            catch (Exception ex) { Debug("native dark mode unavailable: " + ex.Message); }
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct COMBOBOXINFO
        {
            public int cbSize;
            public int itemL, itemT, itemR, itemB, btnL, btnT, btnR, btnB; // two RECTs
            public int stateButton;
            public IntPtr hwndCombo, hwndItem, hwndList;
        }
        [DllImport("user32.dll")] private static extern bool GetComboBoxInfo(IntPtr hwnd, ref COMBOBOXINFO info);

        private static void DarkHwnd(IntPtr h)
        {
            if (h == IntPtr.Zero) return;
            try
            {
                if (nativeDark) AllowDarkModeForWindow(h, true);
                SetWindowTheme(h, "DarkMode_Explorer", null);
            }
            catch (Exception) { }
        }

        // The drop-down list of a combo is a separate native window (ComboLBox) with its own scrollbar.
        private static void DarkComboList(ComboBox cb)
        {
            if (!cb.IsHandleCreated) return;
            COMBOBOXINFO info = new COMBOBOXINFO();
            info.cbSize = Marshal.SizeOf(typeof(COMBOBOXINFO));
            if (!GetComboBoxInfo(cb.Handle, ref info)) return;
            DarkHwnd(info.hwndList);
            ListBorder.Attach(info.hwndList);
        }

        // The open list's 1px frame is drawn by Windows in the system accent (blue): repaint it in our accent.
        [DllImport("user32.dll")] private static extern bool GetWindowRect(IntPtr h, out RECT r);
        private class ListBorder : NativeWindow
        {
            private const int WM_NCPAINT = 0x0085, WM_PAINT = 0x000F, WM_NCDESTROY = 0x0082;
            private static readonly Dictionary<IntPtr, ListBorder> all = new Dictionary<IntPtr, ListBorder>();

            public static void Attach(IntPtr h)
            {
                if (h == IntPtr.Zero || all.ContainsKey(h)) return;
                ListBorder lb = new ListBorder();
                lb.AssignHandle(h);
                all[h] = lb;
            }

            protected override void WndProc(ref Message m)
            {
                if (m.Msg == WM_NCDESTROY)
                {
                    IntPtr h = Handle;
                    base.WndProc(ref m);
                    ReleaseHandle();
                    all.Remove(h);
                    return;
                }
                base.WndProc(ref m);
                if (ThemeManager.Enabled && (m.Msg == WM_NCPAINT || m.Msg == WM_PAINT)) PaintFrame(Handle);
            }

            private static void PaintFrame(IntPtr h)
            {
                RECT r;
                if (!GetWindowRect(h, out r)) return;
                int w = r.Right - r.Left, ht = r.Bottom - r.Top;
                IntPtr dc = GetWindowDC(h);
                if (dc == IntPtr.Zero) return;
                try
                {
                    using (Graphics g = Graphics.FromHdc(dc))
                    using (Pen p = new Pen(Selected)) g.DrawRectangle(p, 0, 0, w - 1, ht - 1);
                }
                catch (Exception) { }
                finally { ReleaseDC(h, dc); }
            }
        }

        private static void NativeDark(Control c)
        {
            try
            {
                if (nativeDark) AllowDarkModeForWindow(c.Handle, true);
                if (c is Form)
                {
                    int on = 1;
                    // 20 = DWMWA_USE_IMMERSIVE_DARK_MODE (Win10 20H1+ / Win11); 19 = older Win10 builds
                    if (DwmSetWindowAttribute(c.Handle, 20, ref on, 4) != 0)
                        DwmSetWindowAttribute(c.Handle, 19, ref on, 4);
                    // The window's own scrollbars (AutoScroll when the window is smaller than its content,
                    // e.g. Form1 resized or the preview zoomed): our overlay bars need a parent, a top-level
                    // window has none, so use the native dark scrollbars instead.
                    SetWindowTheme(c.Handle, "DarkMode_Explorer", null);
                }
                else
                {
                    SetWindowTheme(c.Handle, "DarkMode_Explorer", null); // undocumented; ignored if unsupported
                }
            }
            catch (Exception) { /* cosmetic only */ }
        }

        // ---------------------------------------------------------------- icons

        private const string IconPrefix = "DarkIcons/";

        private static void LoadIcons()
        {
            Assembly asm = typeof(ThemeManager).Assembly;
            // logical name -> real manifest name (MSBuild may keep '\\' from %(RecursiveDir))
            Dictionary<string, string> real = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            foreach (string n in asm.GetManifestResourceNames()) real[n.Replace('\\', '/')] = n;
            ICollection<string> names = real.Keys;

            foreach (string n in names)
            {
                if (!n.StartsWith(IconPrefix + "orig/", StringComparison.OrdinalIgnoreCase)) continue;
                string darkName = IconPrefix + "dark/" + n.Substring((IconPrefix + "orig/").Length);
                if (!real.ContainsKey(darkName)) continue; // no dark version yet: keep original

                Bitmap orig = LoadBitmap(asm, real[n]);
                Bitmap dark = LoadBitmap(asm, real[darkName]);
                if (orig == null || dark == null) continue;
                darkImages.Add(dark, Marker);

                string px = PixelHash(orig);
                if (px != null && !darkByPixels.ContainsKey(px)) darkByPixels[px] = dark;

                orig.Dispose();
            }

            Logger.WriteLine("DarkTheme: " + darkByPixels.Count + " dark icons loaded");
        }

        /// <summary>
        /// Loads a PNG embedded under DarkTheme\Icons (e.g. "about/gtr_4b.png"). Works with dark mode on or off.
        /// </summary>
        internal static Bitmap LoadEmbedded(string relative)
        {
            try
            {
                Assembly asm = typeof(ThemeManager).Assembly;
                string want = IconPrefix + relative.Replace('\\', '/');
                foreach (string n in asm.GetManifestResourceNames())
                    if (string.Equals(n.Replace('\\', '/'), want, StringComparison.OrdinalIgnoreCase))
                        return LoadBitmap(asm, n);
            }
            catch (Exception ex) { Logger.WriteLine("DarkTheme.LoadEmbedded " + relative + ": " + ex.Message); }
            return null;
        }

        private static Bitmap LoadBitmap(Assembly asm, string name)
        {
            using (Stream s = asm.GetManifestResourceStream(name))
            {
                if (s == null) return null;
                using (Bitmap tmp = new Bitmap(s)) return CloneExact(tmp); // detach from stream
            }
        }

        // Bit-exact 32bpp copy. new Bitmap(img) redraws the image and can change
        // semi-transparent pixels (premultiplied alpha rounding), which breaks the pixel hash.
        private static Bitmap CloneExact(Image img)
        {
            Bitmap src = (Bitmap)img;
            Rectangle r = new Rectangle(0, 0, src.Width, src.Height);
            Bitmap dst = new Bitmap(src.Width, src.Height, PixelFormat.Format32bppArgb);
            dst.SetResolution(src.HorizontalResolution, src.VerticalResolution);
            BitmapData a = src.LockBits(r, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            try
            {
                BitmapData b = dst.LockBits(r, ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
                try
                {
                    byte[] row = new byte[src.Width * 4];
                    for (int y = 0; y < src.Height; y++)
                    {
                        Marshal.Copy(IntPtr.Add(a.Scan0, y * a.Stride), row, 0, row.Length);
                        Marshal.Copy(row, 0, IntPtr.Add(b.Scan0, y * b.Stride), row.Length);
                    }
                }
                finally { dst.UnlockBits(b); }
            }
            finally { src.UnlockBits(a); }
            return dst;
        }

        /// <summary>Returns the dark master for an original UI icon, or null.</summary>
        internal static Bitmap FindDark(Image img)
        {
            if (!Enabled || img == null) return null;
            object o;
            if (darkImages.TryGetValue(img, out o)) return null; // already dark
            Bitmap d;
            string px = PixelHash(img);
            if (px != null && darkByPixels.TryGetValue(px, out d)) return d;
            return null;
        }

        private static Image Swap(Image img)
        {
            Bitmap d = FindDark(img);
            if (d == null) return img;
            Bitmap copy;
            lock (iconLock) copy = CloneExact(d);
            darkImages.Add(copy, Marker);
            return copy;
        }

        // FNV-1a 64 over exact ARGB pixels. Only small UI icons (<= 64 px) are hashed,
        // so watch-face images and previews can never be matched.
        private static string PixelHash(Image img)
        {
            Bitmap b = img as Bitmap;
            if (b == null || b.Width > 64 || b.Height > 64) return null;
            if (ImageAnimator.CanAnimate(b)) return null;
            Rectangle r = new Rectangle(0, 0, b.Width, b.Height);
            BitmapData data;
            try { data = b.LockBits(r, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb); }
            catch (Exception) { return null; }
            try
            {
                int row = b.Width * 4;
                byte[] buf = new byte[row];
                ulong h = 14695981039346656037UL;
                for (int y = 0; y < b.Height; y++)
                {
                    Marshal.Copy(IntPtr.Add(data.Scan0, y * data.Stride), buf, 0, row);
                    for (int i = 0; i < row; i++)
                    {
                        h ^= buf[i];
                        h *= 1099511628211UL;
                    }
                }
                return b.Width + "x" + b.Height + ":" + h.ToString("X16");
            }
            finally { b.UnlockBits(data); }
        }

        // ---------------------------------------------------------------- Properties.Resources

        // Icons swapped at runtime (eye on/off, arrows, delete) come from Properties.Resources.X.
        // We replace the generated class's private ResourceManager so every access returns the dark icon.
        private static void WrapResourceManager(Assembly asm, string typeName)
        {
            Type t = asm.GetType(typeName);
            if (t == null) return;
            FieldInfo f = t.GetField("resourceMan", BindingFlags.NonPublic | BindingFlags.Static);
            if (f == null) return;
            f.SetValue(null, new DarkResourceManager(typeName, asm));
        }

        private class DarkResourceManager : ResourceManager
        {
            private readonly Dictionary<string, Bitmap> cache = new Dictionary<string, Bitmap>();
            private readonly HashSet<string> plain = new HashSet<string>();

            public DarkResourceManager(string baseName, Assembly asm) : base(baseName, asm) { }

            private readonly object sync = new object();

            public override object GetObject(string name, CultureInfo culture)
            {
                lock (sync) lock (iconLock) return GetObjectCore(name, culture);
            }

            private object GetObjectCore(string name, CultureInfo culture)
            {
                if (plain.Contains(name)) return base.GetObject(name, culture);
                Bitmap master;
                if (cache.TryGetValue(name, out master))
                {
                    Bitmap c = CloneExact(master);
                    darkImages.Add(c, Marker);
                    return c;
                }

                object o = base.GetObject(name, culture);
                Image img = o as Image;
                Bitmap d = img != null ? FindDark(img) : null;
                if (d == null) { plain.Add(name); return o; }

                cache[name] = d;
                img.Dispose();
                Bitmap copy = CloneExact(d);
                darkImages.Add(copy, Marker);
                return copy;
            }
        }
    }
}
