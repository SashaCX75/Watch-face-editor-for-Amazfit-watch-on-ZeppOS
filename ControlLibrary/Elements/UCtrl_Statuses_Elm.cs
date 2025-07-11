﻿using System;
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
    public partial class UCtrl_Statuses_Elm : UserControl
    {
        private bool setValue; // режим задания параметров
        bool highlight_lock = false;
        bool highlight_DND = false;
        bool highlight_bluetooth = false;
        bool highlight_alarm = false;

        bool visibility_elements = false; // развернут список с элементами
        bool visibilityElement = true; // элемент оторажается на предпросмотре

        public int position = -1; // позиция в наборе элеменетов
        public string selectedElement; // название выбраного элемента

        Point cursorPos = new Point(0, 0);

        public UCtrl_Statuses_Elm()
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

        public bool GetHighlightState()
        {
            bool highlight = highlight_DND || highlight_bluetooth || highlight_alarm || highlight_lock;
            return highlight;
        }

        public void ResetHighlightState()
        {
            selectedElement = "";

            highlight_lock = false;
            highlight_DND = false;
            highlight_bluetooth = false;
            highlight_alarm = false;

            if (highlight_DND)
            {
                panel_DND.BackColor = SystemColors.ActiveCaption;
                button_DND.FlatAppearance.MouseOverBackColor = SystemColors.ActiveCaption;
                button_DND.FlatAppearance.MouseDownBackColor = SystemColors.ActiveCaption;
            }
            else
            {
                panel_DND.BackColor = SystemColors.Control;
                button_DND.FlatAppearance.MouseOverBackColor = SystemColors.Control;
                button_DND.FlatAppearance.MouseDownBackColor = SystemColors.Control;
            }

            if (highlight_bluetooth)
            {
                panel_Bluetooth.BackColor = SystemColors.ActiveCaption;
                button_Bluetooth.FlatAppearance.MouseOverBackColor = SystemColors.ActiveCaption;
                button_Bluetooth.FlatAppearance.MouseDownBackColor = SystemColors.ActiveCaption;
            }
            else
            {
                panel_Bluetooth.BackColor = SystemColors.Control;
                button_Bluetooth.FlatAppearance.MouseOverBackColor = SystemColors.Control;
                button_Bluetooth.FlatAppearance.MouseDownBackColor = SystemColors.Control;
            }

            if (highlight_alarm)
            {
                panel_Alarm.BackColor = SystemColors.ActiveCaption;
                button_Alarm.FlatAppearance.MouseOverBackColor = SystemColors.ActiveCaption;
                button_Alarm.FlatAppearance.MouseDownBackColor = SystemColors.ActiveCaption;
            }
            else
            {
                panel_Alarm.BackColor = SystemColors.Control;
                button_Alarm.FlatAppearance.MouseOverBackColor = SystemColors.Control;
                button_Alarm.FlatAppearance.MouseDownBackColor = SystemColors.Control;
            }

            if (highlight_lock)
            {
                panel_Lock.BackColor = SystemColors.ActiveCaption;
                button_Lock.FlatAppearance.MouseOverBackColor = SystemColors.ActiveCaption;
                button_Lock.FlatAppearance.MouseDownBackColor = SystemColors.ActiveCaption;
            }
            else
            {
                panel_Lock.BackColor = SystemColors.Control;
                button_Lock.FlatAppearance.MouseOverBackColor = SystemColors.Control;
                button_Lock.FlatAppearance.MouseDownBackColor = SystemColors.Control;
            }
        }

        private void panel_DND_Click(object sender, EventArgs e)
        {
            selectedElement = "DND";

            highlight_DND = true;
            highlight_bluetooth = false;
            highlight_alarm = false;

            highlight_lock = false;

            if (highlight_DND)
            {
                panel_DND.BackColor = SystemColors.ActiveCaption;
                button_DND.FlatAppearance.MouseOverBackColor = SystemColors.ActiveCaption;
                button_DND.FlatAppearance.MouseDownBackColor = SystemColors.ActiveCaption;
            }
            else
            {
                panel_DND.BackColor = SystemColors.Control;
                button_DND.FlatAppearance.MouseOverBackColor = SystemColors.Control;
                button_DND.FlatAppearance.MouseDownBackColor = SystemColors.Control;
            }

            if (highlight_bluetooth)
            {
                panel_Bluetooth.BackColor = SystemColors.ActiveCaption;
                button_Bluetooth.FlatAppearance.MouseOverBackColor = SystemColors.ActiveCaption;
                button_Bluetooth.FlatAppearance.MouseDownBackColor = SystemColors.ActiveCaption;
            }
            else
            {
                panel_Bluetooth.BackColor = SystemColors.Control;
                button_Bluetooth.FlatAppearance.MouseOverBackColor = SystemColors.Control;
                button_Bluetooth.FlatAppearance.MouseDownBackColor = SystemColors.Control;
            }

            if (highlight_alarm)
            {
                panel_Alarm.BackColor = SystemColors.ActiveCaption;
                button_Alarm.FlatAppearance.MouseOverBackColor = SystemColors.ActiveCaption;
                button_Alarm.FlatAppearance.MouseDownBackColor = SystemColors.ActiveCaption;
            }
            else
            {
                panel_Alarm.BackColor = SystemColors.Control;
                button_Alarm.FlatAppearance.MouseOverBackColor = SystemColors.Control;
                button_Alarm.FlatAppearance.MouseDownBackColor = SystemColors.Control;
            }

            if (highlight_lock)
            {
                panel_Lock.BackColor = SystemColors.ActiveCaption;
                button_Lock.FlatAppearance.MouseOverBackColor = SystemColors.ActiveCaption;
                button_Lock.FlatAppearance.MouseDownBackColor = SystemColors.ActiveCaption;
            }
            else
            {
                panel_Lock.BackColor = SystemColors.Control;
                button_Lock.FlatAppearance.MouseOverBackColor = SystemColors.Control;
                button_Lock.FlatAppearance.MouseDownBackColor = SystemColors.Control;
            }

            if (SelectChanged != null)
            {
                EventArgs eventArgs = new EventArgs();
                SelectChanged(this, eventArgs);
            }
        }

        private void panel_Bluetooth_Click(object sender, EventArgs e)
        {
            selectedElement = "Bluetooth";

            highlight_DND = false;
            highlight_bluetooth = true;
            highlight_alarm = false;

            highlight_lock = false;

            if (highlight_DND)
            {
                panel_DND.BackColor = SystemColors.ActiveCaption;
                button_DND.FlatAppearance.MouseOverBackColor = SystemColors.ActiveCaption;
                button_DND.FlatAppearance.MouseDownBackColor = SystemColors.ActiveCaption;
            }
            else
            {
                panel_DND.BackColor = SystemColors.Control;
                button_DND.FlatAppearance.MouseOverBackColor = SystemColors.Control;
                button_DND.FlatAppearance.MouseDownBackColor = SystemColors.Control;
            }

            if (highlight_bluetooth)
            {
                panel_Bluetooth.BackColor = SystemColors.ActiveCaption;
                button_Bluetooth.FlatAppearance.MouseOverBackColor = SystemColors.ActiveCaption;
                button_Bluetooth.FlatAppearance.MouseDownBackColor = SystemColors.ActiveCaption;
            }
            else
            {
                panel_Bluetooth.BackColor = SystemColors.Control;
                button_Bluetooth.FlatAppearance.MouseOverBackColor = SystemColors.Control;
                button_Bluetooth.FlatAppearance.MouseDownBackColor = SystemColors.Control;
            }

            if (highlight_alarm)
            {
                panel_Alarm.BackColor = SystemColors.ActiveCaption;
                button_Alarm.FlatAppearance.MouseOverBackColor = SystemColors.ActiveCaption;
                button_Alarm.FlatAppearance.MouseDownBackColor = SystemColors.ActiveCaption;
            }
            else
            {
                panel_Alarm.BackColor = SystemColors.Control;
                button_Alarm.FlatAppearance.MouseOverBackColor = SystemColors.Control;
                button_Alarm.FlatAppearance.MouseDownBackColor = SystemColors.Control;
            }

            if (highlight_lock)
            {
                panel_Lock.BackColor = SystemColors.ActiveCaption;
                button_Lock.FlatAppearance.MouseOverBackColor = SystemColors.ActiveCaption;
                button_Lock.FlatAppearance.MouseDownBackColor = SystemColors.ActiveCaption;
            }
            else
            {
                panel_Lock.BackColor = SystemColors.Control;
                button_Lock.FlatAppearance.MouseOverBackColor = SystemColors.Control;
                button_Lock.FlatAppearance.MouseDownBackColor = SystemColors.Control;
            }

            if (SelectChanged != null)
            {
                EventArgs eventArgs = new EventArgs();
                SelectChanged(this, eventArgs);
            }
        }

        private void panel_Alarm_Click(object sender, EventArgs e)
        {
            selectedElement = "Alarm";

            highlight_DND = false;
            highlight_bluetooth = false;
            highlight_alarm = true;

            highlight_lock = false;

            if (highlight_DND)
            {
                panel_DND.BackColor = SystemColors.ActiveCaption;
                button_DND.FlatAppearance.MouseOverBackColor = SystemColors.ActiveCaption;
                button_DND.FlatAppearance.MouseDownBackColor = SystemColors.ActiveCaption;
            }
            else
            {
                panel_DND.BackColor = SystemColors.Control;
                button_DND.FlatAppearance.MouseOverBackColor = SystemColors.Control;
                button_DND.FlatAppearance.MouseDownBackColor = SystemColors.Control;
            }

            if (highlight_bluetooth)
            {
                panel_Bluetooth.BackColor = SystemColors.ActiveCaption;
                button_Bluetooth.FlatAppearance.MouseOverBackColor = SystemColors.ActiveCaption;
                button_Bluetooth.FlatAppearance.MouseDownBackColor = SystemColors.ActiveCaption;
            }
            else
            {
                panel_Bluetooth.BackColor = SystemColors.Control;
                button_Bluetooth.FlatAppearance.MouseOverBackColor = SystemColors.Control;
                button_Bluetooth.FlatAppearance.MouseDownBackColor = SystemColors.Control;
            }

            if (highlight_alarm)
            {
                panel_Alarm.BackColor = SystemColors.ActiveCaption;
                button_Alarm.FlatAppearance.MouseOverBackColor = SystemColors.ActiveCaption;
                button_Alarm.FlatAppearance.MouseDownBackColor = SystemColors.ActiveCaption;
            }
            else
            {
                panel_Alarm.BackColor = SystemColors.Control;
                button_Alarm.FlatAppearance.MouseOverBackColor = SystemColors.Control;
                button_Alarm.FlatAppearance.MouseDownBackColor = SystemColors.Control;
            }

            if (highlight_lock)
            {
                panel_Lock.BackColor = SystemColors.ActiveCaption;
                button_Lock.FlatAppearance.MouseOverBackColor = SystemColors.ActiveCaption;
                button_Lock.FlatAppearance.MouseDownBackColor = SystemColors.ActiveCaption;
            }
            else
            {
                panel_Lock.BackColor = SystemColors.Control;
                button_Lock.FlatAppearance.MouseOverBackColor = SystemColors.Control;
                button_Lock.FlatAppearance.MouseDownBackColor = SystemColors.Control;
            }

            if (SelectChanged != null)
            {
                EventArgs eventArgs = new EventArgs();
                SelectChanged(this, eventArgs);
            }
        }

        private void panel_Lock_Click(object sender, EventArgs e)
        {
            selectedElement = "Lock";

            highlight_DND = false;
            highlight_bluetooth = false;
            highlight_alarm = false;

            highlight_lock = true;

            if (highlight_DND)
            {
                panel_DND.BackColor = SystemColors.ActiveCaption;
                button_DND.FlatAppearance.MouseOverBackColor = SystemColors.ActiveCaption;
                button_DND.FlatAppearance.MouseDownBackColor = SystemColors.ActiveCaption;
            }
            else
            {
                panel_DND.BackColor = SystemColors.Control;
                button_DND.FlatAppearance.MouseOverBackColor = SystemColors.Control;
                button_DND.FlatAppearance.MouseDownBackColor = SystemColors.Control;
            }

            if (highlight_bluetooth)
            {
                panel_Bluetooth.BackColor = SystemColors.ActiveCaption;
                button_Bluetooth.FlatAppearance.MouseOverBackColor = SystemColors.ActiveCaption;
                button_Bluetooth.FlatAppearance.MouseDownBackColor = SystemColors.ActiveCaption;
            }
            else
            {
                panel_Bluetooth.BackColor = SystemColors.Control;
                button_Bluetooth.FlatAppearance.MouseOverBackColor = SystemColors.Control;
                button_Bluetooth.FlatAppearance.MouseDownBackColor = SystemColors.Control;
            }

            if (highlight_alarm)
            {
                panel_Alarm.BackColor = SystemColors.ActiveCaption;
                button_Alarm.FlatAppearance.MouseOverBackColor = SystemColors.ActiveCaption;
                button_Alarm.FlatAppearance.MouseDownBackColor = SystemColors.ActiveCaption;
            }
            else
            {
                panel_Alarm.BackColor = SystemColors.Control;
                button_Alarm.FlatAppearance.MouseOverBackColor = SystemColors.Control;
                button_Alarm.FlatAppearance.MouseDownBackColor = SystemColors.Control;
            }

            if (highlight_lock)
            {
                panel_Lock.BackColor = SystemColors.ActiveCaption;
                button_Lock.FlatAppearance.MouseOverBackColor = SystemColors.ActiveCaption;
                button_Lock.FlatAppearance.MouseDownBackColor = SystemColors.ActiveCaption;
            }
            else
            {
                panel_Lock.BackColor = SystemColors.Control;
                button_Lock.FlatAppearance.MouseOverBackColor = SystemColors.Control;
                button_Lock.FlatAppearance.MouseDownBackColor = SystemColors.Control;
            }

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

            //((Control)sender).Tag = new object();
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

            //Control control = (Control)sender;
            //if (control.Tag != null)
            //    control.DoDragDrop(sender, DragDropEffects.Move);
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


            if (control != null)
            {
                var pos = tableLayoutPanel1.GetPositionFromControl(control);
                var posOld = tableLayoutPanel1.GetPositionFromControl(draggedPanel);
                //if (pos.Row == 6) return;
                //tableLayoutPanel1.Controls.Add(draggedButton, pos.Column, pos.Row);

                //if (pos != posOld)
                //{
                //    if (pt.Y < control.Location.Y + draggedPanel.Height * 0.9)
                //    {
                //        tableLayoutPanel1.SetRow(draggedPanel, pos.Row);
                //        if (pos.Row < posOld.Row) tableLayoutPanel1.SetRow(control, pos.Row + 1);
                //        else tableLayoutPanel1.SetRow(control, pos.Row - 1);
                //    }
                //}

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

            if (tableLayoutPanel1.Height > 130)
            {
                float currentDPI = tableLayoutPanel1.Height / 101f;
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
            pictureBox_Show.Visible = visibilityElement;
            pictureBox_NotShow.Visible = !visibilityElement;
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
            pictureBox_Show.Visible = visibilityElement;
            pictureBox_NotShow.Visible = !visibilityElement;
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

        /// <summary>Устанавливаем статус видимости для всего элемента</summary>
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
                case "DND":
                    checkBox_DND.Checked = status;
                    break;
                case "Bluetooth":
                    checkBox_Bluetooth.Checked = status;
                    break;
                case "Alarm":
                    checkBox_Alarm.Checked = status;
                    break;
                case "Lock":
                    checkBox_Lock.Checked = status;
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
                        case "DND":
                            panel = panel_DND;
                            break;
                        case "Bluetooth":
                            panel = panel_Bluetooth;
                            break;
                        case "Alarm":
                            panel = panel_Alarm;
                            break;
                        case "Lock":
                            panel = panel_Lock;
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
                    case "panel_Alarm":
                        elementOptions.Add("Alarm", count - i);
                        break;
                    case "panel_Bluetooth":
                        elementOptions.Add("Bluetooth", count - i);
                        break;
                    case "panel_DND":
                        elementOptions.Add("DND", count - i);
                        break;
                    case "panel_Lock":
                        elementOptions.Add("Lock", count - i);
                        break;
                }
            }
            return elementOptions;
        }

        public void SettingsClear()
        {
            setValue = true;

            Dictionary<int, string> elementOptions = new Dictionary<int, string>();
            elementOptions.Add(4, "Alarm");
            elementOptions.Add(3, "Bluetooth");
            elementOptions.Add(2, "DND");
            elementOptions.Add(1, "Lock");
            SetOptionsPosition(elementOptions);

            checkBox_DND.Checked = false;
            checkBox_Bluetooth.Checked = false;
            checkBox_Alarm.Checked = false;
            checkBox_Lock.Checked = false;

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
