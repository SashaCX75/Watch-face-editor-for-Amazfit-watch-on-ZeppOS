namespace ControlLibrary
{
    partial class UCtrl_AnalogTimeCircle_Elm
    {
        /// <summary> 
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором компонентов

        /// <summary> 
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UCtrl_AnalogTimeCircle_Elm));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel_Format_24hour = new System.Windows.Forms.Panel();
            this.button_Format_24hour = new System.Windows.Forms.Button();
            this.checkBox_Format_24hour = new System.Windows.Forms.CheckBox();
            this.panel_SmoothSeconds = new System.Windows.Forms.Panel();
            this.button_SmoothSeconds = new System.Windows.Forms.Button();
            this.checkBox_SmoothSeconds = new System.Windows.Forms.CheckBox();
            this.panel_Minute = new System.Windows.Forms.Panel();
            this.button_Minute = new System.Windows.Forms.Button();
            this.checkBox_Minute = new System.Windows.Forms.CheckBox();
            this.panel_Hour = new System.Windows.Forms.Panel();
            this.checkBox_Hour = new System.Windows.Forms.CheckBox();
            this.button_Hour = new System.Windows.Forms.Button();
            this.panel_Second = new System.Windows.Forms.Panel();
            this.button_Second = new System.Windows.Forms.Button();
            this.checkBox_Second = new System.Windows.Forms.CheckBox();
            this.pictureBox_Arrow_Down = new System.Windows.Forms.PictureBox();
            this.pictureBox_NotShow = new System.Windows.Forms.PictureBox();
            this.pictureBox_Arrow_Right = new System.Windows.Forms.PictureBox();
            this.pictureBox_Show = new System.Windows.Forms.PictureBox();
            this.pictureBox_Del = new System.Windows.Forms.PictureBox();
            this.button_ElementName = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel_Format_24hour.SuspendLayout();
            this.panel_SmoothSeconds.SuspendLayout();
            this.panel_Minute.SuspendLayout();
            this.panel_Hour.SuspendLayout();
            this.panel_Second.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Arrow_Down)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_NotShow)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Arrow_Right)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Show)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Del)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.AllowDrop = true;
            this.tableLayoutPanel1.Controls.Add(this.panel_Format_24hour, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.panel_SmoothSeconds, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel_Minute, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.panel_Hour, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.panel_Second, 0, 1);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.DragDrop += new System.Windows.Forms.DragEventHandler(this.tableLayoutPanel1_DragDrop);
            this.tableLayoutPanel1.DragOver += new System.Windows.Forms.DragEventHandler(this.tableLayoutPanel1_DragOver);
            // 
            // panel_Format_24hour
            // 
            resources.ApplyResources(this.panel_Format_24hour, "panel_Format_24hour");
            this.panel_Format_24hour.BackColor = System.Drawing.SystemColors.Control;
            this.panel_Format_24hour.Controls.Add(this.button_Format_24hour);
            this.panel_Format_24hour.Controls.Add(this.checkBox_Format_24hour);
            this.panel_Format_24hour.Name = "panel_Format_24hour";
            this.panel_Format_24hour.Click += new System.EventHandler(this.panel_Format_24hour_Click);
            this.panel_Format_24hour.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Control_MouseDown);
            this.panel_Format_24hour.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Control_MouseMove);
            this.panel_Format_24hour.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Control_MouseUp);
            // 
            // button_Format_24hour
            // 
            resources.ApplyResources(this.button_Format_24hour, "button_Format_24hour");
            this.button_Format_24hour.FlatAppearance.BorderSize = 0;
            this.button_Format_24hour.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.Control;
            this.button_Format_24hour.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.Control;
            this.button_Format_24hour.Image = global::ControlLibrary.Properties.Resources.toggle_on;
            this.button_Format_24hour.Name = "button_Format_24hour";
            this.button_Format_24hour.UseVisualStyleBackColor = true;
            this.button_Format_24hour.Click += new System.EventHandler(this.panel_Format_24hour_Click);
            this.button_Format_24hour.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Control_MouseDown);
            this.button_Format_24hour.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Control_MouseMove);
            this.button_Format_24hour.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Control_MouseUp);
            // 
            // checkBox_Format_24hour
            // 
            resources.ApplyResources(this.checkBox_Format_24hour, "checkBox_Format_24hour");
            this.checkBox_Format_24hour.Name = "checkBox_Format_24hour";
            this.checkBox_Format_24hour.UseVisualStyleBackColor = true;
            this.checkBox_Format_24hour.CheckedChanged += new System.EventHandler(this.checkBox_Elements_CheckedChanged);
            // 
            // panel_SmoothSeconds
            // 
            resources.ApplyResources(this.panel_SmoothSeconds, "panel_SmoothSeconds");
            this.panel_SmoothSeconds.BackColor = System.Drawing.SystemColors.Control;
            this.panel_SmoothSeconds.Controls.Add(this.button_SmoothSeconds);
            this.panel_SmoothSeconds.Controls.Add(this.checkBox_SmoothSeconds);
            this.panel_SmoothSeconds.Name = "panel_SmoothSeconds";
            this.panel_SmoothSeconds.Click += new System.EventHandler(this.panel_SmoothSeconds_Click);
            this.panel_SmoothSeconds.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Control_MouseDown);
            this.panel_SmoothSeconds.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Control_MouseMove);
            this.panel_SmoothSeconds.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Control_MouseUp);
            // 
            // button_SmoothSeconds
            // 
            resources.ApplyResources(this.button_SmoothSeconds, "button_SmoothSeconds");
            this.button_SmoothSeconds.FlatAppearance.BorderSize = 0;
            this.button_SmoothSeconds.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.Control;
            this.button_SmoothSeconds.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.Control;
            this.button_SmoothSeconds.Image = global::ControlLibrary.Properties.Resources.toggle_on;
            this.button_SmoothSeconds.Name = "button_SmoothSeconds";
            this.button_SmoothSeconds.UseVisualStyleBackColor = true;
            this.button_SmoothSeconds.Click += new System.EventHandler(this.panel_SmoothSeconds_Click);
            this.button_SmoothSeconds.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Control_MouseDown);
            this.button_SmoothSeconds.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Control_MouseMove);
            this.button_SmoothSeconds.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Control_MouseUp);
            // 
            // checkBox_SmoothSeconds
            // 
            resources.ApplyResources(this.checkBox_SmoothSeconds, "checkBox_SmoothSeconds");
            this.checkBox_SmoothSeconds.Name = "checkBox_SmoothSeconds";
            this.checkBox_SmoothSeconds.UseVisualStyleBackColor = true;
            this.checkBox_SmoothSeconds.CheckedChanged += new System.EventHandler(this.checkBox_Elements_CheckedChanged);
            // 
            // panel_Minute
            // 
            resources.ApplyResources(this.panel_Minute, "panel_Minute");
            this.panel_Minute.BackColor = System.Drawing.SystemColors.Control;
            this.panel_Minute.Controls.Add(this.button_Minute);
            this.panel_Minute.Controls.Add(this.checkBox_Minute);
            this.panel_Minute.Name = "panel_Minute";
            this.panel_Minute.Click += new System.EventHandler(this.panel_Minutes_Click);
            this.panel_Minute.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Control_MouseDown);
            this.panel_Minute.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Control_MouseMove);
            this.panel_Minute.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Control_MouseUp);
            // 
            // button_Minute
            // 
            resources.ApplyResources(this.button_Minute, "button_Minute");
            this.button_Minute.FlatAppearance.BorderSize = 0;
            this.button_Minute.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.Control;
            this.button_Minute.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.Control;
            this.button_Minute.Image = global::ControlLibrary.Properties.Resources.circle_scale;
            this.button_Minute.Name = "button_Minute";
            this.button_Minute.UseVisualStyleBackColor = true;
            this.button_Minute.Click += new System.EventHandler(this.panel_Minutes_Click);
            this.button_Minute.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Control_MouseDown);
            this.button_Minute.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Control_MouseMove);
            this.button_Minute.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Control_MouseUp);
            // 
            // checkBox_Minute
            // 
            resources.ApplyResources(this.checkBox_Minute, "checkBox_Minute");
            this.checkBox_Minute.Name = "checkBox_Minute";
            this.checkBox_Minute.UseVisualStyleBackColor = true;
            this.checkBox_Minute.CheckedChanged += new System.EventHandler(this.checkBox_Elements_CheckedChanged);
            // 
            // panel_Hour
            // 
            resources.ApplyResources(this.panel_Hour, "panel_Hour");
            this.panel_Hour.BackColor = System.Drawing.SystemColors.Control;
            this.panel_Hour.Controls.Add(this.checkBox_Hour);
            this.panel_Hour.Controls.Add(this.button_Hour);
            this.panel_Hour.Name = "panel_Hour";
            this.panel_Hour.Click += new System.EventHandler(this.panel_Hours_Click);
            this.panel_Hour.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Control_MouseDown);
            this.panel_Hour.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Control_MouseMove);
            this.panel_Hour.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Control_MouseUp);
            // 
            // checkBox_Hour
            // 
            resources.ApplyResources(this.checkBox_Hour, "checkBox_Hour");
            this.checkBox_Hour.Name = "checkBox_Hour";
            this.checkBox_Hour.UseVisualStyleBackColor = true;
            this.checkBox_Hour.CheckedChanged += new System.EventHandler(this.checkBox_Elements_CheckedChanged);
            // 
            // button_Hour
            // 
            resources.ApplyResources(this.button_Hour, "button_Hour");
            this.button_Hour.FlatAppearance.BorderSize = 0;
            this.button_Hour.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.Control;
            this.button_Hour.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.Control;
            this.button_Hour.Image = global::ControlLibrary.Properties.Resources.circle_scale;
            this.button_Hour.Name = "button_Hour";
            this.button_Hour.UseVisualStyleBackColor = true;
            this.button_Hour.Click += new System.EventHandler(this.panel_Hours_Click);
            this.button_Hour.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Control_MouseDown);
            this.button_Hour.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Control_MouseMove);
            this.button_Hour.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Control_MouseUp);
            // 
            // panel_Second
            // 
            resources.ApplyResources(this.panel_Second, "panel_Second");
            this.panel_Second.BackColor = System.Drawing.SystemColors.Control;
            this.panel_Second.Controls.Add(this.button_Second);
            this.panel_Second.Controls.Add(this.checkBox_Second);
            this.panel_Second.Name = "panel_Second";
            this.panel_Second.Click += new System.EventHandler(this.panel_Seconds_Click);
            this.panel_Second.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Control_MouseDown);
            this.panel_Second.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Control_MouseMove);
            this.panel_Second.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Control_MouseUp);
            // 
            // button_Second
            // 
            resources.ApplyResources(this.button_Second, "button_Second");
            this.button_Second.FlatAppearance.BorderSize = 0;
            this.button_Second.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.Control;
            this.button_Second.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.Control;
            this.button_Second.Image = global::ControlLibrary.Properties.Resources.circle_scale;
            this.button_Second.Name = "button_Second";
            this.button_Second.UseVisualStyleBackColor = true;
            this.button_Second.Click += new System.EventHandler(this.panel_Seconds_Click);
            this.button_Second.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Control_MouseDown);
            this.button_Second.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Control_MouseMove);
            this.button_Second.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Control_MouseUp);
            // 
            // checkBox_Second
            // 
            resources.ApplyResources(this.checkBox_Second, "checkBox_Second");
            this.checkBox_Second.Name = "checkBox_Second";
            this.checkBox_Second.UseVisualStyleBackColor = true;
            this.checkBox_Second.CheckedChanged += new System.EventHandler(this.checkBox_Elements_CheckedChanged);
            // 
            // pictureBox_Arrow_Down
            // 
            resources.ApplyResources(this.pictureBox_Arrow_Down, "pictureBox_Arrow_Down");
            this.pictureBox_Arrow_Down.BackgroundImage = global::ControlLibrary.Properties.Resources.arrow_down;
            this.pictureBox_Arrow_Down.Name = "pictureBox_Arrow_Down";
            this.pictureBox_Arrow_Down.TabStop = false;
            this.pictureBox_Arrow_Down.Click += new System.EventHandler(this.button_ElementName_Click);
            // 
            // pictureBox_NotShow
            // 
            resources.ApplyResources(this.pictureBox_NotShow, "pictureBox_NotShow");
            this.pictureBox_NotShow.BackgroundImage = global::ControlLibrary.Properties.Resources.outline_visibility_off_black_24;
            this.pictureBox_NotShow.Name = "pictureBox_NotShow";
            this.pictureBox_NotShow.TabStop = false;
            this.pictureBox_NotShow.Click += new System.EventHandler(this.pictureBox_NotShow_Click);
            // 
            // pictureBox_Arrow_Right
            // 
            resources.ApplyResources(this.pictureBox_Arrow_Right, "pictureBox_Arrow_Right");
            this.pictureBox_Arrow_Right.BackgroundImage = global::ControlLibrary.Properties.Resources.arrow_right;
            this.pictureBox_Arrow_Right.Name = "pictureBox_Arrow_Right";
            this.pictureBox_Arrow_Right.TabStop = false;
            this.pictureBox_Arrow_Right.Click += new System.EventHandler(this.button_ElementName_Click);
            // 
            // pictureBox_Show
            // 
            resources.ApplyResources(this.pictureBox_Show, "pictureBox_Show");
            this.pictureBox_Show.BackgroundImage = global::ControlLibrary.Properties.Resources.outline_visibility_black_24;
            this.pictureBox_Show.Name = "pictureBox_Show";
            this.pictureBox_Show.TabStop = false;
            this.pictureBox_Show.Click += new System.EventHandler(this.pictureBox_Show_Click);
            // 
            // pictureBox_Del
            // 
            resources.ApplyResources(this.pictureBox_Del, "pictureBox_Del");
            this.pictureBox_Del.BackgroundImage = global::ControlLibrary.Properties.Resources.outline_delete_forever_black_24;
            this.pictureBox_Del.Name = "pictureBox_Del";
            this.pictureBox_Del.TabStop = false;
            this.pictureBox_Del.Click += new System.EventHandler(this.pictureBox_Del_Click);
            // 
            // button_ElementName
            // 
            resources.ApplyResources(this.button_ElementName, "button_ElementName");
            this.button_ElementName.BackColor = System.Drawing.SystemColors.Control;
            this.button_ElementName.Image = global::ControlLibrary.Properties.Resources.circle_time;
            this.button_ElementName.Name = "button_ElementName";
            this.button_ElementName.UseVisualStyleBackColor = false;
            this.button_ElementName.SizeChanged += new System.EventHandler(this.button_ElementName_SizeChanged);
            this.button_ElementName.Click += new System.EventHandler(this.button_ElementName_Click);
            this.button_ElementName.MouseDown += new System.Windows.Forms.MouseEventHandler(this.button_ElementName_MouseDown);
            this.button_ElementName.MouseMove += new System.Windows.Forms.MouseEventHandler(this.button_ElementName_MouseMove);
            this.button_ElementName.MouseUp += new System.Windows.Forms.MouseEventHandler(this.button_ElementName_MouseUp);
            // 
            // UCtrl_AnalogTimeCircle_Elm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pictureBox_Arrow_Down);
            this.Controls.Add(this.pictureBox_NotShow);
            this.Controls.Add(this.pictureBox_Arrow_Right);
            this.Controls.Add(this.pictureBox_Show);
            this.Controls.Add(this.pictureBox_Del);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.button_ElementName);
            this.Name = "UCtrl_AnalogTimeCircle_Elm";
            this.Load += new System.EventHandler(this.UCtrl_AnalogTimeCircle_Elm_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel_Format_24hour.ResumeLayout(false);
            this.panel_Format_24hour.PerformLayout();
            this.panel_SmoothSeconds.ResumeLayout(false);
            this.panel_SmoothSeconds.PerformLayout();
            this.panel_Minute.ResumeLayout(false);
            this.panel_Minute.PerformLayout();
            this.panel_Hour.ResumeLayout(false);
            this.panel_Hour.PerformLayout();
            this.panel_Second.ResumeLayout(false);
            this.panel_Second.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Arrow_Down)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_NotShow)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Arrow_Right)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Show)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Del)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox_Arrow_Down;
        private System.Windows.Forms.PictureBox pictureBox_NotShow;
        private System.Windows.Forms.PictureBox pictureBox_Arrow_Right;
        private System.Windows.Forms.PictureBox pictureBox_Show;
        private System.Windows.Forms.PictureBox pictureBox_Del;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel_Format_24hour;
        private System.Windows.Forms.Button button_Format_24hour;
        public System.Windows.Forms.CheckBox checkBox_Format_24hour;
        private System.Windows.Forms.Panel panel_SmoothSeconds;
        private System.Windows.Forms.Button button_SmoothSeconds;
        public System.Windows.Forms.CheckBox checkBox_SmoothSeconds;
        private System.Windows.Forms.Panel panel_Minute;
        private System.Windows.Forms.Button button_Minute;
        public System.Windows.Forms.CheckBox checkBox_Minute;
        private System.Windows.Forms.Panel panel_Hour;
        public System.Windows.Forms.CheckBox checkBox_Hour;
        private System.Windows.Forms.Button button_Hour;
        private System.Windows.Forms.Panel panel_Second;
        private System.Windows.Forms.Button button_Second;
        public System.Windows.Forms.CheckBox checkBox_Second;
        private System.Windows.Forms.Button button_ElementName;
    }
}
