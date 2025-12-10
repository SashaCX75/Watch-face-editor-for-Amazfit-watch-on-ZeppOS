using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ControlLibrary
{
    public partial class UCtrl_Sleep_Elm : UserControl
    {
        private bool setValue; // режим задания параметров
        bool highlight_chart = false;
        bool highlight_start_sleep = false;
        bool highlight_end_sleep = false;
        bool highlight_duration_sleep_total = false;
        bool highlight_duration_sleep = false;
        bool highlight_wakeup = false;
        bool highlight_wakeup_count = false;
        bool highlight_score = false;
        bool highlight_icon = false;

        bool visibility_elements = false; // развернут список с элементами
        bool visibilityElement = true; // элемент оторажается на предпросмотре

        public int position = -1; // позиция в наборе элеменетов
        public string selectedElement; // название выбраного элемента

        Point cursorPos = new Point(0, 0);

        public UCtrl_Sleep_Elm()
        {
            InitializeComponent();
            setValue = false;

            button_ElementName.Controls.Add(pictureBox_Arrow_Right);
            button_ElementName.Controls.Add(pictureBox_Arrow_Down);
            button_ElementName.Controls.Add(pictureBox_NotShow);
            button_ElementName.Controls.Add(pictureBox_Show);
            button_ElementName.Controls.Add(pictureBox_Del);

            pictureBox_Arrow_Right.Location = new Point(1, 2);
            pictureBox_Arrow_Right.BackColor = Color.Transparent;

            pictureBox_Arrow_Down.Location = new Point(1, 2);
            pictureBox_Arrow_Down.BackColor = Color.Transparent;

            //pictureBox_Show.Location = new Point(button_ElementName.Width - pictureBox_Show.Width * 2 - 6 , 2);
            pictureBox_NotShow.BackColor = Color.Transparent;
            pictureBox_Show.BackColor = Color.Transparent;

            //pictureBox_Del.Location = new Point(button_ElementName.Width - pictureBox_Del.Width - 8, 2);
            pictureBox_Del.BackColor = Color.Transparent;

            if (!AppUtils.IsLightTheme())
            {
                foreach (Control ctrl in this.Controls)
                {
                    if (ctrl is PictureBox pb)
                    {
                        if (pb.Image != null) pb.Image = AppUtils.InvertColors(pb.Image);
                        if (pb.BackgroundImage != null) pb.BackgroundImage = AppUtils.InvertColors(pb.BackgroundImage);
                    }
                    if (ctrl is Button btn)
                    {
                        if (btn.Image != null) btn.Image = AppUtils.InvertColors(btn.Image);
                        foreach (Control ctrl_btn in btn.Controls)
                        {
                            if (ctrl_btn is PictureBox pb_btn)
                            {
                                if (pb_btn.Image != null) pb_btn.Image = AppUtils.InvertColors(pb_btn.Image);
                                if (pb_btn.BackgroundImage != null) pb_btn.BackgroundImage = AppUtils.InvertColors(pb_btn.BackgroundImage);
                            }
                        }
                    }
                }
            }
        }

        [Browsable(true)]
        [Description("Происходит при изменении видимости элемента")]
        public event VisibleElementChangedHandler VisibleElementChanged;
        public delegate void VisibleElementChangedHandler(object sender, EventArgs eventArgs, bool visible);

        [Browsable(true)]
        [Description("Происходит при изменении видимости отдельного параметра в элементе")]
        public event VisibleOptionsChangedHandler VisibleOptionsChanged;
        public delegate void VisibleOptionsChangedHandler(object sender, EventArgs eventArgs);

        [Browsable(true)]
        [Description("Происходит при изменении положения параметров в элементе")]
        public event OptionsMovedHandler OptionsMoved;
        public delegate void OptionsMovedHandler(object sender, EventArgs eventArgs, Dictionary<string, int> elementOptions);

        [Browsable(true)]
        [Description("Происходит при изменении выбора элемента")]
        public event SelectChangedHandler SelectChanged;
        public delegate void SelectChangedHandler(object sender, EventArgs eventArgs);

        [Browsable(true)]
        [Description("Происходит при удалении элемента")]
        public event DelElementHandler DelElement;
        public delegate void DelElementHandler(object sender, EventArgs eventArgs);

        private void button_ElementName_Click(object sender, EventArgs e)
        {
            visibility_elements = !visibility_elements;
            tableLayoutPanel1.Visible = visibility_elements;
            pictureBox_Arrow_Down.Visible = visibility_elements;
            pictureBox_Arrow_Right.Visible = !visibility_elements;
        }

        public void ResetHighlightState()
        {
            selectedElement = "";

            highlight_chart = false;
            highlight_start_sleep = false;
            highlight_end_sleep = false;
            highlight_duration_sleep_total = false;
            highlight_duration_sleep = false;
            highlight_wakeup = false;
            highlight_wakeup_count = false;
            highlight_score = false;
            highlight_icon = false;

            SelectElement();
        }

        private void SelectElement()
        {
            if (highlight_chart)
            {
                panel_Chart.BackColor = SystemColors.ActiveCaption;
                button_Chart.FlatAppearance.MouseOverBackColor = SystemColors.ActiveCaption;
                button_Chart.FlatAppearance.MouseDownBackColor = SystemColors.ActiveCaption;
            }
            else
            {
                panel_Chart.BackColor = SystemColors.Control;
                button_Chart.FlatAppearance.MouseOverBackColor = SystemColors.Control;
                button_Chart.FlatAppearance.MouseDownBackColor = SystemColors.Control;
            }

            if (highlight_start_sleep)
            {
                panel_StartSleep.BackColor = SystemColors.ActiveCaption;
                button_StartSleep.FlatAppearance.MouseOverBackColor = SystemColors.ActiveCaption;
                button_StartSleep.FlatAppearance.MouseDownBackColor = SystemColors.ActiveCaption;
            }
            else
            {
                panel_StartSleep.BackColor = SystemColors.Control;
                button_StartSleep.FlatAppearance.MouseOverBackColor = SystemColors.Control;
                button_StartSleep.FlatAppearance.MouseDownBackColor = SystemColors.Control;
            }

            if (highlight_end_sleep)
            {
                panel_EndSleep.BackColor = SystemColors.ActiveCaption;
                button_EndSleep.FlatAppearance.MouseOverBackColor = SystemColors.ActiveCaption;
                button_EndSleep.FlatAppearance.MouseDownBackColor = SystemColors.ActiveCaption;
            }
            else
            {
                panel_EndSleep.BackColor = SystemColors.Control;
                button_EndSleep.FlatAppearance.MouseOverBackColor = SystemColors.Control;
                button_EndSleep.FlatAppearance.MouseDownBackColor = SystemColors.Control;
            }

            if (highlight_duration_sleep_total)
            {
                panel_DurationSleep_total.BackColor = SystemColors.ActiveCaption;
                button_DurationSleep_total.FlatAppearance.MouseOverBackColor = SystemColors.ActiveCaption;
                button_DurationSleep_total.FlatAppearance.MouseDownBackColor = SystemColors.ActiveCaption;
            }
            else
            {
                panel_DurationSleep_total.BackColor = SystemColors.Control;
                button_DurationSleep_total.FlatAppearance.MouseOverBackColor = SystemColors.Control;
                button_DurationSleep_total.FlatAppearance.MouseDownBackColor = SystemColors.Control;
            }

            if (highlight_duration_sleep)
            {
                panel_DurationSleep.BackColor = SystemColors.ActiveCaption;
                button_DurationSleep.FlatAppearance.MouseOverBackColor = SystemColors.ActiveCaption;
                button_DurationSleep.FlatAppearance.MouseDownBackColor = SystemColors.ActiveCaption;
            }
            else
            {
                panel_DurationSleep.BackColor = SystemColors.Control;
                button_DurationSleep.FlatAppearance.MouseOverBackColor = SystemColors.Control;
                button_DurationSleep.FlatAppearance.MouseDownBackColor = SystemColors.Control;
            }

            if (highlight_wakeup)
            {
                panel_WakeUp.BackColor = SystemColors.ActiveCaption;
                button_WakeUp.FlatAppearance.MouseOverBackColor = SystemColors.ActiveCaption;
                button_WakeUp.FlatAppearance.MouseDownBackColor = SystemColors.ActiveCaption;
            }
            else
            {
                panel_WakeUp.BackColor = SystemColors.Control;
                button_WakeUp.FlatAppearance.MouseOverBackColor = SystemColors.Control;
                button_WakeUp.FlatAppearance.MouseDownBackColor = SystemColors.Control;
            }

            if (highlight_wakeup_count)
            {
                panel_WakeUpCount.BackColor = SystemColors.ActiveCaption;
                button_WakeUpCount.FlatAppearance.MouseOverBackColor = SystemColors.ActiveCaption;
                button_WakeUpCount.FlatAppearance.MouseDownBackColor = SystemColors.ActiveCaption;
            }
            else
            {
                panel_WakeUpCount.BackColor = SystemColors.Control;
                button_WakeUpCount.FlatAppearance.MouseOverBackColor = SystemColors.Control;
                button_WakeUpCount.FlatAppearance.MouseDownBackColor = SystemColors.Control;
            }

            if (highlight_score)
            {
                panel_Score.BackColor = SystemColors.ActiveCaption;
                button_Score.FlatAppearance.MouseOverBackColor = SystemColors.ActiveCaption;
                button_Score.FlatAppearance.MouseDownBackColor = SystemColors.ActiveCaption;
            }
            else
            {
                panel_Score.BackColor = SystemColors.Control;
                button_Score.FlatAppearance.MouseOverBackColor = SystemColors.Control;
                button_Score.FlatAppearance.MouseDownBackColor = SystemColors.Control;
            }

            if (highlight_icon)
            {
                panel_Icon.BackColor = SystemColors.ActiveCaption;
                button_Icon.FlatAppearance.MouseOverBackColor = SystemColors.ActiveCaption;
                button_Icon.FlatAppearance.MouseDownBackColor = SystemColors.ActiveCaption;
            }
            else
            {
                panel_Icon.BackColor = SystemColors.Control;
                button_Icon.FlatAppearance.MouseOverBackColor = SystemColors.Control;
                button_Icon.FlatAppearance.MouseDownBackColor = SystemColors.Control;
            }
        }

        private void panel_Chart_Click(object sender, EventArgs e)
        {
            selectedElement = "Chart";

            highlight_chart = true;
            highlight_start_sleep = false;
            highlight_end_sleep = false;
            highlight_duration_sleep_total = false;
            highlight_duration_sleep = false;
            highlight_wakeup = false;
            highlight_wakeup_count = false;
            highlight_score = false;
            highlight_icon = false;

            SelectElement();

            if (SelectChanged != null)
            {
                EventArgs eventArgs = new EventArgs();
                SelectChanged(this, eventArgs);
            }
        }

        private void panel_StartSleep_Click(object sender, EventArgs e)
        {
            selectedElement = "StartSleep";

            highlight_chart = false;
            highlight_start_sleep = true;
            highlight_end_sleep = false;
            highlight_duration_sleep_total = false;
            highlight_duration_sleep = false;
            highlight_wakeup = false;
            highlight_wakeup_count = false;
            highlight_score = false;
            highlight_icon = false;

            SelectElement();

            if (SelectChanged != null)
            {
                EventArgs eventArgs = new EventArgs();
                SelectChanged(this, eventArgs);
            }
        }

        private void panel_EndSleep_Click(object sender, EventArgs e)
        {
            selectedElement = "EndSleep";

            highlight_chart = false;
            highlight_start_sleep = false;
            highlight_end_sleep = true;
            highlight_duration_sleep_total = false;
            highlight_duration_sleep = false;
            highlight_wakeup = false;
            highlight_wakeup_count = false;
            highlight_score = false;
            highlight_icon = false;

            SelectElement();

            if (SelectChanged != null)
            {
                EventArgs eventArgs = new EventArgs();
                SelectChanged(this, eventArgs);
            }
        }

        private void panel_DurationSleep_total_Click(object sender, EventArgs e)
        {
            selectedElement = "DurationSleep_total";

            highlight_chart = false;
            highlight_start_sleep = false;
            highlight_end_sleep = false;
            highlight_duration_sleep_total = true;
            highlight_duration_sleep = false;
            highlight_wakeup = false;
            highlight_wakeup_count = false;
            highlight_score = false;
            highlight_icon = false;

            SelectElement();

            if (SelectChanged != null)
            {
                EventArgs eventArgs = new EventArgs();
                SelectChanged(this, eventArgs);
            }
        }

        private void panel_DurationSleep_Click(object sender, EventArgs e)
        {
            selectedElement = "DurationSleep";

            highlight_chart = false;
            highlight_start_sleep = false;
            highlight_end_sleep = false;
            highlight_duration_sleep_total = false;
            highlight_duration_sleep = true;
            highlight_wakeup = false;
            highlight_wakeup_count = false;
            highlight_score = false;
            highlight_icon = false;

            SelectElement();

            if (SelectChanged != null)
            {
                EventArgs eventArgs = new EventArgs();
                SelectChanged(this, eventArgs);
            }
        }

        private void panel_WakeUp_Click(object sender, EventArgs e)
        {
            selectedElement = "WakeUp";

            highlight_chart = false;
            highlight_start_sleep = false;
            highlight_end_sleep = false;
            highlight_duration_sleep_total = false;
            highlight_duration_sleep = false;
            highlight_wakeup = true;
            highlight_wakeup_count = false;
            highlight_score = false;
            highlight_icon = false;

            SelectElement();

            if (SelectChanged != null)
            {
                EventArgs eventArgs = new EventArgs();
                SelectChanged(this, eventArgs);
            }
        }

        private void panel_WakeUpCount_Click(object sender, EventArgs e)
        {
            selectedElement = "WakeUpCount";

            highlight_chart = false;
            highlight_start_sleep = false;
            highlight_end_sleep = false;
            highlight_duration_sleep_total = false;
            highlight_duration_sleep = false;
            highlight_wakeup = false;
            highlight_wakeup_count = true;
            highlight_score = false;
            highlight_icon = false;

            SelectElement();

            if (SelectChanged != null)
            {
                EventArgs eventArgs = new EventArgs();
                SelectChanged(this, eventArgs);
            }
        }

        private void panel_Score_Click(object sender, EventArgs e)
        {
            selectedElement = "Score";

            highlight_chart = false;
            highlight_start_sleep = false;
            highlight_end_sleep = false;
            highlight_duration_sleep_total = false;
            highlight_duration_sleep = false;
            highlight_wakeup = false;
            highlight_wakeup_count = false;
            highlight_score = true;
            highlight_icon = false;

            SelectElement();

            if (SelectChanged != null)
            {
                EventArgs eventArgs = new EventArgs();
                SelectChanged(this, eventArgs);
            }
        }

        private void panel_Icon_Click(object sender, EventArgs e)
        {
            selectedElement = "Icon";

            highlight_chart = false;
            highlight_start_sleep = false;
            highlight_end_sleep = false;
            highlight_duration_sleep_total = false;
            highlight_duration_sleep = false;
            highlight_wakeup = false;
            highlight_wakeup_count = false;
            highlight_score = false;
            highlight_icon = true;

            SelectElement();

            if (SelectChanged != null)
            {
                EventArgs eventArgs = new EventArgs();
                SelectChanged(this, eventArgs);
            }
        }


        #region перетягиваем элементы
        private void Control_MouseDown(object sender, MouseEventArgs e)
        {
            Control control = (Control)sender;
            Panel panel;
            if (control.GetType().Name == "Panel") panel = (Panel)sender;
            else panel = (Panel)control.Parent;
            if (panel != null)
            {
                panel.Tag = new object();
                cursorPos = Cursor.Position;
            }
        }

        private void Control_MouseMove(object sender, MouseEventArgs e)
        {
            Control control = (Control)sender;
            Panel panel;
            if (control.GetType().Name == "Panel") panel = (Panel)sender;
            else panel = (Panel)control.Parent;
            if (panel != null && panel.Tag != null)
            {
                int cursorX = Cursor.Position.X;
                int cursorY = Cursor.Position.Y;
                int dX = Math.Abs(cursorX - cursorPos.X);
                int dY = Math.Abs(cursorY - cursorPos.Y);
                if (dX > 5 || dY > 5)
                    panel.DoDragDrop(sender, DragDropEffects.Move);
                //panel.DoDragDrop(sender, DragDropEffects.Move);
            }
        }

        private void Control_MouseUp(object sender, MouseEventArgs e)
        {
            Control control = (Control)sender;
            Panel panel;
            if (control.GetType().Name == "Panel") panel = (Panel)sender;
            else panel = (Panel)control.Parent;
            if (panel != null && panel.Tag != null) panel.Tag = null;

            //((Control)sender).Tag = null;
        }

        private void tableLayoutPanel1_DragDrop(object sender, DragEventArgs e)
        {
            int cursorX = Cursor.Position.X;
            int cursorY = Cursor.Position.Y;
            int dX = Math.Abs(cursorX - cursorPos.X);
            int dY = Math.Abs(cursorY - cursorPos.Y);
            if (dX > 5 || dY > 5)
            {
                if (OptionsMoved != null)
                {
                    EventArgs eventArgs = new EventArgs();
                    OptionsMoved(this, eventArgs, GetOptionsPosition());
                }
            }
        }

        private void tableLayoutPanel1_DragOver(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(typeof(Button)) && !e.Data.GetDataPresent(typeof(Panel)))
                return;

            e.Effect = e.AllowedEffect;
            Panel draggedPanel = (Panel)e.Data.GetData(typeof(Panel));
            if (draggedPanel == null)
            {
                Button draggedButton = (Button)e.Data.GetData(typeof(Button));
                if (draggedButton != null) draggedPanel = (Panel)draggedButton.Parent;
            }
            if (draggedPanel == null) return;

            Point pt = tableLayoutPanel1.PointToClient(new Point(e.X, e.Y));
            Control control = tableLayoutPanel1.GetChildAtPoint(pt);

            if (control != null /*&& control.Name != "panel_Settings"*/)
            {
                /*if (control.Name == "panel_Settings")
                {
                    e.Effect = DragDropEffects.None;
                    return;
                }
                else */e.Effect = DragDropEffects.Move;

                var pos = tableLayoutPanel1.GetPositionFromControl(control);
                var posOld = tableLayoutPanel1.GetPositionFromControl(draggedPanel);
                if (pos != posOld && pos.Row < posOld.Row)
                {
                    if (pt.Y < control.Location.Y + draggedPanel.Height * 0.4)
                    {
                        tableLayoutPanel1.SetRow(draggedPanel, pos.Row);
                        tableLayoutPanel1.SetRow(control, pos.Row + 1);
                        //if (pos.Row < posOld.Row) tableLayoutPanel1.SetRow(control, pos.Row + 1);
                        //else tableLayoutPanel1.SetRow(control, pos.Row - 1);
                    }
                }
                if (pos != posOld && pos.Row > posOld.Row)
                {
                    if (pt.Y > control.Location.Y + control.Height * 0.6)
                    {
                        tableLayoutPanel1.SetRow(control, pos.Row - 1);
                        tableLayoutPanel1.SetRow(draggedPanel, pos.Row);
                        //if (pos.Row < posOld.Row) tableLayoutPanel1.SetRow(control, pos.Row + 1);
                        //else tableLayoutPanel1.SetRow(control, pos.Row - 1);
                    }
                }
                draggedPanel.Tag = null;
            }
        }

        private void button_ElementName_MouseDown(object sender, MouseEventArgs e)
        {
            base.OnMouseDown(e);
        }

        private void button_ElementName_MouseMove(object sender, MouseEventArgs e)
        {
            base.OnMouseMove(e);
        }

        private void button_ElementName_MouseUp(object sender, MouseEventArgs e)
        {
            base.OnMouseUp(e);
        }

        #endregion

        private void button_ElementName_SizeChanged(object sender, EventArgs e)
        {
            pictureBox_NotShow.Location = new Point(button_ElementName.Width - pictureBox_Show.Width * 2 - 6, 2);
            pictureBox_Show.Location = new Point(button_ElementName.Width - pictureBox_Show.Width * 2 - 6, 2);

            pictureBox_Del.Location = new Point(button_ElementName.Width - pictureBox_Del.Width - 4, 2);

            if (tableLayoutPanel1.Height > 285)
            {
                float currentDPI = tableLayoutPanel1.Height / 226f;
                button_ElementName.Image = (Image)(new Bitmap(button_ElementName.Image,
                    new Size((int)(16 * currentDPI), (int)(16 * currentDPI))));

                Control.ControlCollection controlCollection = tableLayoutPanel1.Controls;
                for (int i = 0; i < controlCollection.Count; i++)
                {
                    string name = controlCollection[i].GetType().Name;
                    if (name == "Panel")
                    {
                        Control.ControlCollection panelCollection = controlCollection[i].Controls;
                        for (int j = 0; j < panelCollection.Count; j++)
                        {
                            string nameButton = panelCollection[j].GetType().Name;
                            if (nameButton == "Button")
                            {
                                Button btn = (Button)panelCollection[j];
                                btn.Image = (Image)(new Bitmap(btn.Image,
                                    new Size((int)(16 * currentDPI), (int)(16 * currentDPI))));
                            }
                        }
                    }
                }

                controlCollection = button_ElementName.Controls;
                for (int i = 0; i < controlCollection.Count; i++)
                {
                    string name = controlCollection[i].GetType().Name;
                    if (name == "PictureBox")
                    {
                        PictureBox pb = (PictureBox)controlCollection[i];
                        pb.BackgroundImageLayout = ImageLayout.Zoom;
                    }
                }

            }
        }

        private void pictureBox_Show_Click(object sender, EventArgs e)
        {
            visibilityElement = false;
            pictureBox_Show.Visible = false;
            pictureBox_NotShow.Visible = true;
            SetColorActive();

            if (VisibleElementChanged != null)
            {
                EventArgs eventArgs = new EventArgs();
                VisibleElementChanged(this, eventArgs, visibilityElement);
            }
        }

        private void pictureBox_NotShow_Click(object sender, EventArgs e)
        {
            visibilityElement = true;
            pictureBox_Show.Visible = true;
            pictureBox_NotShow.Visible = false;
            SetColorActive();

            if (VisibleElementChanged != null)
            {
                EventArgs eventArgs = new EventArgs();
                VisibleElementChanged(this, eventArgs, visibilityElement);
            }
        }

        private void pictureBox_Del_Click(object sender, EventArgs e)
        {
            if (DelElement != null)
            {
                EventArgs eventArgs = new EventArgs();
                DelElement(this, eventArgs);
            }
        }

        private void checkBox_Elements_CheckedChanged(object sender, EventArgs e)
        {
            if (VisibleOptionsChanged != null && !setValue)
            {
                EventArgs eventArgs = new EventArgs();
                VisibleOptionsChanged(sender, eventArgs);
            }
        }

        /// <summary>Устанавливаем статус видимости для всего элемента</summary>DayOfWeek_Images
        public void SetVisibilityElementStatus(bool status)
        {
            visibilityElement = status;
            pictureBox_NotShow.Visible = !visibilityElement;
            pictureBox_Show.Visible = visibilityElement;
            SetColorActive();

        }

        /// <summary>Устанавливаем статус вкл/выкл для отдельной опции в элементе</summary>
        public void SetVisibilityOptionsStatus(string name, bool status)
        {
            setValue = true;
            switch (name)
            {
                case "Chart":
                    checkBox_Chart.Checked = status;
                    break;
                case "StartSleep":
                    checkBox_StartSleep.Checked = status;
                    break;
                case "EndSleep":
                    checkBox_EndSleep.Checked = status;
                    break;
                case "DurationSleep_total":
                    checkBox_DurationSleep_total.Checked = status;
                    break;
                case "DurationSleep":
                    checkBox_DurationSleep.Checked = status;
                    break;
                case "Score":
                    checkBox_Score.Checked = status;
                    break;
                case "Icon":
                    checkBox_Icon.Checked = status;
                    break;
            }
            setValue = false;
        }

        /// <summary>Устанавливаем порядок опций в элементе</summary>
        public void SetOptionsPosition(Dictionary<int, string> elementOptions)
        {
            int elementCount = tableLayoutPanel1.RowCount;
            for (int key = elementCount - 1; key >= 0; key--)
            {
                Control panel = null;
                if (elementOptions.ContainsKey(elementCount - key))
                {
                    string name = elementOptions[elementCount - key];
                    switch (name)
                    {
                        case "Chart":
                            panel = panel_Chart;
                            break;
                        case "StartSleep":
                            panel = panel_StartSleep;
                            break;
                        case "EndSleep":
                            panel = panel_EndSleep;
                            break;
                        case "DurationSleep_total":
                            panel = panel_DurationSleep_total;
                            break;
                        case "DurationSleep":
                            panel = panel_DurationSleep;
                            break;
                        case "WakeUp":
                            panel = panel_WakeUp;
                            break;
                        case "WakeUpCount":
                            panel = panel_WakeUpCount;
                            break;
                        case "Score":
                            panel = panel_Score;
                            break;
                        case "Icon":
                            panel = panel_Icon;
                            break;
                    }
                }
                position = key;
                if (panel == null)
                    continue;
                int realPos = tableLayoutPanel1.GetRow(panel);
                if (realPos == position)
                    continue;
                if (realPos < position)
                {
                    for (int i = realPos; i < position; i++)
                    {
                        Control panel2 = tableLayoutPanel1.GetControlFromPosition(0, i + 1);
                        if (panel2 == null) return;
                        tableLayoutPanel1.SetRow(panel2, i);
                        tableLayoutPanel1.SetRow(panel, i + 1);
                    }
                }
                else
                {
                    for (int i = realPos; i > position; i--)
                    {
                        Control panel2 = tableLayoutPanel1.GetControlFromPosition(0, i - 1);
                        if (panel2 == null)
                            return;
                        tableLayoutPanel1.SetRow(panel, i - 1);
                        tableLayoutPanel1.SetRow(panel2, i);
                    }
                }
            }
        }

        /// <summary>Получаем порядок опций в элементе</summary>
        public Dictionary<string, int> GetOptionsPosition()
        {
            Dictionary<string, int> elementOptions = new Dictionary<string, int>();
            int count = tableLayoutPanel1.RowCount;
            for (int i = 0; i < tableLayoutPanel1.RowCount; i++)
            {
                Control panel = tableLayoutPanel1.GetControlFromPosition(0, i);
                switch (panel.Name)
                {
                    case "panel_Chart":
                        elementOptions.Add("Chart", count - i);
                        break;
                    case "panel_StartSleep":
                        elementOptions.Add("StartSleep", count - i);
                        break;
                    case "panel_EndSleep":
                        elementOptions.Add("EndSleep", count - i);
                        break;
                    case "panel_DurationSleep_total":
                        elementOptions.Add("DurationSleep_total", count - i);
                        break;
                    case "panel_DurationSleep":
                        elementOptions.Add("DurationSleep", count - i);
                        break;
                    case "panel_WakeUp":
                        elementOptions.Add("WakeUp", count - i);
                        break;
                    case "panel_WakeUpCount":
                        elementOptions.Add("WakeUpCount", count - i);
                        break;
                    case "panel_Score":
                        elementOptions.Add("Score", count - i);
                        break;
                    case "panel_Icon":
                        elementOptions.Add("Icon", count - i);
                        break;
                }
            }
            return elementOptions;
        }

        public void SettingsClear()
        {
            setValue = true;

            Dictionary<int, string> elementOptions = new Dictionary<int, string>();
            int index = 1;
            elementOptions.Add(index++, "Icon");
            elementOptions.Add(index++, "Score");
            elementOptions.Add(index++, "WakeUpCount");
            elementOptions.Add(index++, "WakeUp");
            elementOptions.Add(index++, "DurationSleep");
            elementOptions.Add(index++, "DurationSleep_total");
            elementOptions.Add(index++, "EndSleep");
            elementOptions.Add(index++, "StartSleep");
            elementOptions.Add(index++, "Chart");
            SetOptionsPosition(elementOptions);

            checkBox_Chart.Checked = false;
            checkBox_StartSleep.Checked = false;
            checkBox_EndSleep.Checked = false;
            checkBox_DurationSleep_total.Checked = false;
            checkBox_DurationSleep.Checked = false;
            checkBox_WakeUp.Checked = false;
            checkBox_WakeUpCount.Checked = false;
            checkBox_Score.Checked = false;
            checkBox_Icon.Checked = false;

            visibility_elements = false;
            tableLayoutPanel1.Visible = visibility_elements;
            pictureBox_Arrow_Down.Visible = visibility_elements;
            pictureBox_Arrow_Right.Visible = !visibility_elements;

            visibilityElement = true;
            pictureBox_Show.Visible = visibilityElement;
            pictureBox_NotShow.Visible = !visibilityElement;
            SetColorActive();

            setValue = false;
        }

        private void SetColorActive()
        {
            if (visibilityElement)
            {
                button_ElementName.ForeColor = SystemColors.ControlText;
                button_ElementName.BackColor = SystemColors.Control;
            }
            else
            {
                button_ElementName.ForeColor = SystemColors.GrayText;
                button_ElementName.BackColor = SystemColors.ControlLight;
            }
        }
    }
}
