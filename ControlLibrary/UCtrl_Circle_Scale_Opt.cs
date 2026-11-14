using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ControlLibrary
{
    public partial class UCtrl_Circle_Scale_Opt : UserControl
    {
        private bool setValue; // режим задания параметров
        private List<string> ListImagesFullName = new List<string>(); // перечень путей к файлам с картинками
        public Object _CircleScale;
        public Dictionary<string, Object> WidgetProperty = new Dictionary<string, Object>();
        private int[] CustomColors = { }; // пользовательские цвета

        private bool LineCap_mode = false;
        private bool Alpha_mode = false;
        private bool Inverse_mode = true;

        public UCtrl_Circle_Scale_Opt()
        {
            InitializeComponent();
            setValue = false;
            comboBox_scaleCircle_lineCap.SelectedIndex = 0;
            comboBox_arcType.SelectedIndex = 1;
        }

        private void comboBox_scaleCircle_color_Click(object sender, EventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();
            ComboBox comboBox_color = sender as ComboBox;
            colorDialog.Color = comboBox_color.BackColor;
            colorDialog.FullOpen = true;

            colorDialog.CustomColors = CustomColors;


            if (colorDialog.ShowDialog() == DialogResult.Cancel)
                return;
            // установка цвета формы
            comboBox_color.BackColor = colorDialog.Color;
            LastColor.last_color = colorDialog.Color;
            if (CustomColors != colorDialog.CustomColors)
            {
                CustomColors = colorDialog.CustomColors;

                if (CustomColorsChanged != null && !setValue)
                {
                    CustomColorsChanged(CustomColors);
                }
            }

            if (ValueChanged != null && !setValue)
            {
                EventArgs eventArgs = new EventArgs();
                ValueChanged(this, eventArgs);
            }
        }

        private void comboBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        public void SetColorScale(Color color)
        {
            comboBox_scaleCircle_color.BackColor = color;
        }

        public Color GetColorScale()
        {
            return comboBox_scaleCircle_color.BackColor;
        }

        /// <summary>Устанавливает тип окончания линии "Rounded", "Flat"</summary>
        public void SetLineCap(string alignment)
        {
            int result;
            switch (alignment)
            {
                case "Rounded":
                    result = 0;
                    break;
                case "Flat":
                    result = 1;
                    break;

                default:
                    result = 0;
                    break;

            }
            comboBox_scaleCircle_lineCap.SelectedIndex = result;
        }

        /// <summary>Возвращает тип окончания линии строкой "Rounded", "Flat"</summary>
        public string GetLineCap()
        {
            string result;
            switch (comboBox_scaleCircle_lineCap.SelectedIndex)
            {
                case 0:
                    result = "Rounded";
                    break;
                case 1:
                    result = "Flat";
                    break;

                default:
                    result = "Rounded";
                    break;

            }
            return result;
        }
        /// <summary>Возвращает SelectedIndex выпадающего списка</summary>
        public int GetSelectedIndexLineCap()
        {
            return comboBox_scaleCircle_lineCap.SelectedIndex;
        }

        /// <summary>Доступность выбора окончания линии</summary>
        [Description("Доступность выбора окончания линии")]
        public virtual bool LineCap
        {
            get
            {
                return LineCap_mode;
            }
            set
            {
                LineCap_mode = value;
                comboBox_scaleCircle_lineCap.Enabled = value;
                label2.Enabled = value;
            }
        }

        /// <summary>Доступность инверсии шкалы</summary>
        [Description("Доступность инверсии шкалы")]
        public virtual bool Inverse
        {
            get
            {
                return Inverse_mode;
            }
            set
            {
                Inverse_mode = value;
                UpdateBackgroundState();
            }
        }

        #region Background
        /// <summary>Выбрана ли картинка фона (шкала рисуется картинкой через IMG_ARC_PROGRESS)</summary>
        public bool BackgroundSelected
        {
            get { return comboBox_background.SelectedIndex >= 0; }
        }

        /// <summary>С фоном зеркальное отображение и инверсия недоступны,
        /// а вместо окончания линии выбирается тип шкалы (сектор или дуга)</summary>
        private void UpdateBackgroundState()
        {
            bool noBackground = !BackgroundSelected;
            checkBox_mirror.Enabled = noBackground;
            checkBox_inversion.Enabled = Inverse_mode && noBackground;

            label2.Visible = noBackground;
            comboBox_scaleCircle_lineCap.Visible = noBackground;
            label_arcType.Visible = !noBackground;
            comboBox_arcType.Visible = !noBackground;
        }

        /// <summary>Тип шкалы с фоном: 0 - сектор (hmUI.arc_type.FILL), 1 - дуга (hmUI.arc_type.LINE)</summary>
        public void SetArcType(int arcType)
        {
            comboBox_arcType.SelectedIndex = arcType == 0 ? 0 : 1;
        }

        /// <summary>Тип шкалы с фоном: 0 - сектор (hmUI.arc_type.FILL), 1 - дуга (hmUI.arc_type.LINE)</summary>
        public int GetArcType()
        {
            return comboBox_arcType.SelectedIndex == 0 ? 0 : 1;
        }

        public void SetBackground(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                comboBox_background.SelectedIndex = -1;
                comboBox_background.Text = "";
            }
            else
            {
                comboBox_background.Text = value;
                if (comboBox_background.SelectedIndex < 0) comboBox_background.Text = "";
            }
            UpdateBackgroundState();
        }

        /// <summary>Возвращает название выбранной картинки фона</summary>
        public string GetBackground()
        {
            if (comboBox_background.SelectedIndex < 0) return "";
            return comboBox_background.Text;
        }

        /// <summary>Добавляет ссылки на картинки в выпадающий список фона</summary>
        public void ComboBoxAddItems(List<string> ListImages, List<string> _ListImagesFullName)
        {
            comboBox_background.Items.Clear();
            comboBox_background.Items.AddRange(ListImages.ToArray());
            ListImagesFullName = _ListImagesFullName;

            int count = ListImages.Count;
            if (count == 0) comboBox_background.DropDownHeight = 1;
            else if (count < 5) comboBox_background.DropDownHeight = 35 * count + 1;
            else comboBox_background.DropDownHeight = 106;
        }

        private void comboBox_background_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateBackgroundState();
            if (ValueChanged != null && !setValue)
            {
                EventArgs eventArgs = new EventArgs();
                ValueChanged(this, eventArgs);
            }
        }

        private void comboBox_background_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.Delete) || (e.KeyCode == Keys.Back))
            {
                comboBox_background.Text = "";
                comboBox_background.SelectedIndex = -1;
                UpdateBackgroundState();
                if (ValueChanged != null && !setValue)
                {
                    EventArgs eventArgs = new EventArgs();
                    ValueChanged(this, eventArgs);
                }
            }
        }

        private void comboBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            float size = comboBox.Font.Size;
            FontFamily family = comboBox.Font.FontFamily;
            e.DrawBackground();
            int itemWidth = e.Bounds.Height;
            int itemHeight = e.Bounds.Height - 4;

            if (e.Index >= 0 && e.Index < ListImagesFullName.Count)
            {
                try
                {
                    using (FileStream stream = new FileStream(ListImagesFullName[e.Index], FileMode.Open, FileAccess.Read))
                    {
                        Image image = Image.FromStream(stream);
                        float scale = (float)itemWidth / image.Width;
                        if ((float)itemHeight / image.Height < scale) scale = (float)itemHeight / image.Height;
                        float itemWidthRec = image.Width * scale;
                        float itemHeightRec = image.Height * scale;
                        Rectangle rectangle = new Rectangle((int)(itemWidth - itemWidthRec) / 2 + 2,
                            e.Bounds.Top + (int)(itemHeight - itemHeightRec) / 2 + 2, (int)itemWidthRec, (int)itemHeightRec);
                        e.Graphics.DrawImage(image, rectangle);
                    }
                }
                catch { }
            }
            using (Font myFont = new Font(family, size))
            {
                StringFormat lineAlignment = new StringFormat();
                lineAlignment.LineAlignment = StringAlignment.Center;
                if (e.Index >= 0)
                    e.Graphics.DrawString(comboBox.Items[e.Index].ToString(), myFont, System.Drawing.Brushes.Black,
                        new RectangleF(e.Bounds.X + itemWidth, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height), lineAlignment);
            }
            e.DrawFocusRectangle();
        }

        private void comboBox_MeasureItem(object sender, MeasureItemEventArgs e)
        {
            e.ItemHeight = 35;
        }
        #endregion

        /// <summary>Режим доступности прозрачности</summary>
        [Description("Режим доступности прозрачности")]
        public virtual bool Alpha
        {
            get
            {
                return Alpha_mode;
            }
            set
            {
                Alpha_mode = value;
                label_alpha.Enabled = Alpha_mode;
                numericUpDown_Alpha.Enabled = Alpha_mode;
            }
        }

        [Browsable(true)]
        [Description("Происходит при изменении выбора элемента")]
        public event ValueChangedHandler ValueChanged;
        public delegate void ValueChangedHandler(object sender, EventArgs eventArgs);

        [Browsable(true)]
        [Description("Происходит при копировании свойст виджета")]
        public event WidgetProperty_Copy_Handler WidgetProperty_Copy;
        public delegate void WidgetProperty_Copy_Handler(object sender, EventArgs eventArgs);

        [Browsable(true)]
        [Description("Происходит при вставке свойст виджета")]
        public event WidgetProperty_Paste_Handler WidgetProperty_Paste;
        public delegate void WidgetProperty_Paste_Handler(object sender, EventArgs eventArgs);

        [Browsable(true)]
        [Description("Происходит при изменении пользовательских цветов")]
        public event CustomColorsChangedHandler CustomColorsChanged;
        public delegate void CustomColorsChangedHandler(int[] customColors);

        #region Standard events

        private void checkBox_CheckedChanged(object sender, EventArgs e)
        {
            if (ValueChanged != null && !setValue)
            {
                EventArgs eventArgs = new EventArgs();
                ValueChanged(this, eventArgs);
            }
        }

        private void comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ValueChanged != null && !setValue)
            {
                EventArgs eventArgs = new EventArgs();
                ValueChanged(this, eventArgs);
            }
        }
        #endregion

        #region contextMenu
        private void contextMenuStrip_X_Opening(object sender, CancelEventArgs e)
        {
            if ((MouseСoordinates.X < 0) || (MouseСoordinates.Y < 0))
            {
                contextMenuStrip_X.Items[0].Enabled = false;
            }
            else
            {
                contextMenuStrip_X.Items[0].Enabled = true;
            }
            decimal i = 0;
            if ((Clipboard.ContainsText() == true) && (decimal.TryParse(Clipboard.GetText(), out i)))
            {
                contextMenuStrip_X.Items[2].Enabled = true;
            }
            else
            {
                contextMenuStrip_X.Items[2].Enabled = false;
            }
        }

        private void contextMenuStrip_Y_Opening(object sender, CancelEventArgs e)
        {
            if ((MouseСoordinates.X < 0) || (MouseСoordinates.Y < 0))
            {
                contextMenuStrip_Y.Items[0].Enabled = false;
            }
            else
            {
                contextMenuStrip_Y.Items[0].Enabled = true;
            }
            decimal i = 0;
            if ((Clipboard.ContainsText() == true) && (decimal.TryParse(Clipboard.GetText(), out i)))
            {
                contextMenuStrip_Y.Items[2].Enabled = true;
            }
            else
            {
                contextMenuStrip_Y.Items[2].Enabled = false;
            }
        }

        private void вставитьКоординатуХToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Try to cast the sender to a ToolStripItem
            ToolStripItem menuItem = sender as ToolStripItem;
            if (menuItem != null)
            {
                // Retrieve the ContextMenuStrip that owns this ToolStripItem
                ContextMenuStrip owner = menuItem.Owner as ContextMenuStrip;
                if (owner != null)
                {
                    // Get the control that is displaying this context menu
                    Control sourceControl = owner.SourceControl;
                    NumericUpDown numericUpDown = sourceControl as NumericUpDown;
                    numericUpDown.Value = MouseСoordinates.X;
                }
            }
        }

        private void вставитьКоординатуYToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Try to cast the sender to a ToolStripItem
            ToolStripItem menuItem = sender as ToolStripItem;
            if (menuItem != null)
            {
                // Retrieve the ContextMenuStrip that owns this ToolStripItem
                ContextMenuStrip owner = menuItem.Owner as ContextMenuStrip;
                if (owner != null)
                {
                    // Get the control that is displaying this context menu
                    Control sourceControl = owner.SourceControl;
                    NumericUpDown numericUpDown = sourceControl as NumericUpDown;
                    numericUpDown.Value = MouseСoordinates.Y;
                }
            }
        }

        private void копироватьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Try to cast the sender to a ToolStripItem
            ToolStripItem menuItem = sender as ToolStripItem;
            if (menuItem != null)
            {
                // Retrieve the ContextMenuStrip that owns this ToolStripItem
                ContextMenuStrip owner = menuItem.Owner as ContextMenuStrip;
                if (owner != null)
                {
                    // Get the control that is displaying this context menu
                    Control sourceControl = owner.SourceControl;
                    NumericUpDown numericUpDown = sourceControl as NumericUpDown;
                    Clipboard.SetText(numericUpDown.Value.ToString());
                }
            }
        }

        private void вставитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripItem menuItem = sender as ToolStripItem;
            if (menuItem != null)
            {
                // Retrieve the ContextMenuStrip that owns this ToolStripItem
                ContextMenuStrip owner = menuItem.Owner as ContextMenuStrip;
                if (owner != null)
                {
                    // Get the control that is displaying this context menu
                    Control sourceControl = owner.SourceControl;
                    NumericUpDown numericUpDown = sourceControl as NumericUpDown;
                    //Если в буфере обмен содержится текст
                    if (Clipboard.ContainsText() == true)
                    {
                        //Извлекаем (точнее копируем) его и сохраняем в переменную
                        decimal i = 0;
                        if (decimal.TryParse(Clipboard.GetText(), out i))
                        {
                            if (i > numericUpDown.Maximum) i = numericUpDown.Maximum;
                            if (i < numericUpDown.Minimum) i = numericUpDown.Minimum;
                            numericUpDown.Value = i;
                        }
                    }

                }
            }
        }
        #endregion

        #region numericUpDown
        private void numericUpDown_picturesX_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (MouseСoordinates.X < 0) return;
            NumericUpDown numericUpDown = sender as NumericUpDown;
            if (e.X <= numericUpDown.Controls[1].Width + 1)
            {
                // Click is in text area
                numericUpDown.Value = MouseСoordinates.X;
            }
        }

        private void numericUpDown_picturesY_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (MouseСoordinates.Y < 0) return;
            NumericUpDown numericUpDown = sender as NumericUpDown;
            if (e.X <= numericUpDown.Controls[1].Width + 1)
            {
                // Click is in text area
                numericUpDown.Value = MouseСoordinates.Y;
            }
        }

        private void numericUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (ValueChanged != null && !setValue)
            {
                EventArgs eventArgs = new EventArgs();
                ValueChanged(this, eventArgs);
            }
        }

        #endregion

        #region Settings Set/Clear

        /// <summary>Очищает выпадающие списки с картинками, сбрасывает данные на значения по умолчанию</summary>
        public void SettingsClear(int[] customColors)
        {
            setValue = true;
            CustomColors = customColors;

            numericUpDown_scaleCircleX.Value = 0;
            numericUpDown_scaleCircleY.Value = 0;

            numericUpDown_scaleCircle_radius.Value = 100;
            numericUpDown_scaleCircle_width.Value = 5;

            numericUpDown_scaleCircle_startAngle.Value = 0;
            numericUpDown_scaleCircle_endAngle.Value = 180;

            checkBox_mirror.Checked = false;
            checkBox_inversion.Checked = false;
            comboBox_scaleCircle_lineCap.SelectedIndex = 0;

            comboBox_background.Text = null;
            comboBox_background.SelectedIndex = -1;
            comboBox_arcType.SelectedIndex = 1;

            LineCap = false;
            Alpha = false;
            Inverse = true;

            setValue = false;
        }
        #endregion

        private void numericUpDown_scaleCircle_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && (e.KeyCode == Keys.Left || e.KeyCode == Keys.Right || e.KeyCode == Keys.Up || e.KeyCode == Keys.Down))
            {
                NumericUpDown numericUpDown = sender as NumericUpDown;
                if (e.KeyCode == Keys.Up && numericUpDown.Name == "numericUpDown_scaleCircleX")
                {
                    e.SuppressKeyPress = false;
                    numericUpDown_scaleCircleY.DownButton();
                }
                if (e.KeyCode == Keys.Down && numericUpDown.Name == "numericUpDown_scaleCircleX")
                {
                    e.SuppressKeyPress = false;
                    numericUpDown_scaleCircleY.UpButton();
                }

                if (e.KeyCode == Keys.Up && numericUpDown.Name == "numericUpDown_scaleCircleY")
                {
                    e.SuppressKeyPress = false;
                    numericUpDown_scaleCircleY.DownButton();
                }
                if (e.KeyCode == Keys.Down && numericUpDown.Name == "numericUpDown_scaleCircleY")
                {
                    e.SuppressKeyPress = false;
                    numericUpDown_scaleCircleY.UpButton();
                }

                if (e.KeyCode == Keys.Left && (numericUpDown.Name == "numericUpDown_scaleCircleX" || numericUpDown.Name == "numericUpDown_scaleCircleY"))
                    numericUpDown_scaleCircleX.DownButton();
                if (e.KeyCode == Keys.Right && (numericUpDown.Name == "numericUpDown_scaleCircleX" || numericUpDown.Name == "numericUpDown_scaleCircleY"))
                    numericUpDown_scaleCircleX.UpButton();

                e.Handled = true;
            }
        }

        private void numericUpDown_scaleCircle_radius_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (MouseСoordinates.X < 0 && MouseСoordinates.Y < 0) return;
            NumericUpDown numericUpDown = sender as NumericUpDown;
            if (e.X <= numericUpDown.Controls[1].Width + 1)
            {
                // Click is in text area
                int x = (int)(MouseСoordinates.X - numericUpDown_scaleCircleX.Value);
                int y = (int)(MouseСoordinates.Y - numericUpDown_scaleCircleY.Value);
                int r = (int)Math.Round( Math.Sqrt(x * x + y * y), MidpointRounding.AwayFromZero);
                numericUpDown_scaleCircle_radius.Value = r;
            }
        }

        private void context_WidgetProperty_Opening(object sender, CancelEventArgs e)
        {
            if (WidgetProperty.ContainsKey("Circle_Scale")) context_WidgetProperty.Items[1].Enabled = true;
            else context_WidgetProperty.Items[1].Enabled = false;
        }

        private void копироватьСвойстваToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (WidgetProperty_Copy != null && !setValue)
            {
                EventArgs eventArgs = new EventArgs();
                WidgetProperty_Copy(this, eventArgs);
            }
        }

        private void вставитьСвойстваToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Focus();
            if (WidgetProperty_Paste != null && !setValue)
            {
                EventArgs eventArgs = new EventArgs();
                WidgetProperty_Paste(this, eventArgs);
            }
        }
    }
}
