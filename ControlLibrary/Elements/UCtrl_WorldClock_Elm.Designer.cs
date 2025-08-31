namespace ControlLibrary
{
    partial class UCtrl_WorldClock_Elm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UCtrl_WorldClock_Elm));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel_CityName = new System.Windows.Forms.Panel();
            this.button_CityName = new System.Windows.Forms.Button();
            this.checkBox_CityName = new System.Windows.Forms.CheckBox();
            this.panel_ButtonPrev = new System.Windows.Forms.Panel();
            this.button_ButtonPrev = new System.Windows.Forms.Button();
            this.checkBox_ButtonPrev = new System.Windows.Forms.CheckBox();
            this.panel_TimeDiff = new System.Windows.Forms.Panel();
            this.button_TimeDiff = new System.Windows.Forms.Button();
            this.checkBox_TimeDiff = new System.Windows.Forms.CheckBox();
            this.panel_Time = new System.Windows.Forms.Panel();
            this.checkBox_Time = new System.Windows.Forms.CheckBox();
            this.button_Time = new System.Windows.Forms.Button();
            this.panel_TimeZone = new System.Windows.Forms.Panel();
            this.button_TimeZone = new System.Windows.Forms.Button();
            this.checkBox_TimeZone = new System.Windows.Forms.CheckBox();
            this.panel_ButtonNext = new System.Windows.Forms.Panel();
            this.button_ButtonNext = new System.Windows.Forms.Button();
            this.checkBox_ButtonNext = new System.Windows.Forms.CheckBox();
            this.panel_Icon = new System.Windows.Forms.Panel();
            this.button_Icon = new System.Windows.Forms.Button();
            this.checkBox_Icon = new System.Windows.Forms.CheckBox();
            this.pictureBox_Arrow_Down = new System.Windows.Forms.PictureBox();
            this.pictureBox_NotShow = new System.Windows.Forms.PictureBox();
            this.pictureBox_Arrow_Right = new System.Windows.Forms.PictureBox();
            this.pictureBox_Show = new System.Windows.Forms.PictureBox();
            this.pictureBox_Del = new System.Windows.Forms.PictureBox();
            this.button_ElementName = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel_CityName.SuspendLayout();
            this.panel_ButtonPrev.SuspendLayout();
            this.panel_TimeDiff.SuspendLayout();
            this.panel_Time.SuspendLayout();
            this.panel_TimeZone.SuspendLayout();
            this.panel_ButtonNext.SuspendLayout();
            this.panel_Icon.SuspendLayout();
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
            this.tableLayoutPanel1.Controls.Add(this.panel_CityName, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.panel_ButtonPrev, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.panel_TimeDiff, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.panel_Time, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel_TimeZone, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel_ButtonNext, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.panel_Icon, 0, 6);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.DragDrop += new System.Windows.Forms.DragEventHandler(this.tableLayoutPanel1_DragDrop);
            this.tableLayoutPanel1.DragOver += new System.Windows.Forms.DragEventHandler(this.tableLayoutPanel1_DragOver);
            // 
            // panel_CityName
            // 
            resources.ApplyResources(this.panel_CityName, "panel_CityName");
            this.panel_CityName.BackColor = System.Drawing.SystemColors.Control;
            this.panel_CityName.Controls.Add(this.button_CityName);
            this.panel_CityName.Controls.Add(this.checkBox_CityName);
            this.panel_CityName.Name = "panel_CityName";
            this.panel_CityName.Click += new System.EventHandler(this.panel_CityName_Click);
            this.panel_CityName.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Control_MouseDown);
            this.panel_CityName.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Control_MouseMove);
            this.panel_CityName.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Control_MouseUp);
            // 
            // button_CityName
            // 
            resources.ApplyResources(this.button_CityName, "button_CityName");
            this.button_CityName.FlatAppearance.BorderSize = 0;
            this.button_CityName.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.Control;
            this.button_CityName.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.Control;
            this.button_CityName.Image = global::ControlLibrary.Properties.Resources.text_fields;
            this.button_CityName.Name = "button_CityName";
            this.button_CityName.UseVisualStyleBackColor = true;
            this.button_CityName.Click += new System.EventHandler(this.panel_CityName_Click);
            this.button_CityName.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Control_MouseDown);
            this.button_CityName.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Control_MouseMove);
            this.button_CityName.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Control_MouseUp);
            // 
            // checkBox_CityName
            // 
            resources.ApplyResources(this.checkBox_CityName, "checkBox_CityName");
            this.checkBox_CityName.Name = "checkBox_CityName";
            this.checkBox_CityName.UseVisualStyleBackColor = true;
            this.checkBox_CityName.CheckedChanged += new System.EventHandler(this.checkBox_Elements_CheckedChanged);
            // 
            // panel_ButtonPrev
            // 
            resources.ApplyResources(this.panel_ButtonPrev, "panel_ButtonPrev");
            this.panel_ButtonPrev.BackColor = System.Drawing.SystemColors.Control;
            this.panel_ButtonPrev.Controls.Add(this.button_ButtonPrev);
            this.panel_ButtonPrev.Controls.Add(this.checkBox_ButtonPrev);
            this.panel_ButtonPrev.Name = "panel_ButtonPrev";
            this.panel_ButtonPrev.Click += new System.EventHandler(this.panel_ButtonPrev_Click);
            this.panel_ButtonPrev.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Control_MouseDown);
            this.panel_ButtonPrev.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Control_MouseMove);
            this.panel_ButtonPrev.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Control_MouseUp);
            // 
            // button_ButtonPrev
            // 
            resources.ApplyResources(this.button_ButtonPrev, "button_ButtonPrev");
            this.button_ButtonPrev.FlatAppearance.BorderSize = 0;
            this.button_ButtonPrev.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.Control;
            this.button_ButtonPrev.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.Control;
            this.button_ButtonPrev.Image = global::ControlLibrary.Properties.Resources.button;
            this.button_ButtonPrev.Name = "button_ButtonPrev";
            this.button_ButtonPrev.UseVisualStyleBackColor = true;
            this.button_ButtonPrev.Click += new System.EventHandler(this.panel_ButtonPrev_Click);
            this.button_ButtonPrev.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Control_MouseDown);
            this.button_ButtonPrev.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Control_MouseMove);
            this.button_ButtonPrev.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Control_MouseUp);
            // 
            // checkBox_ButtonPrev
            // 
            resources.ApplyResources(this.checkBox_ButtonPrev, "checkBox_ButtonPrev");
            this.checkBox_ButtonPrev.Name = "checkBox_ButtonPrev";
            this.checkBox_ButtonPrev.UseVisualStyleBackColor = true;
            this.checkBox_ButtonPrev.CheckedChanged += new System.EventHandler(this.checkBox_Elements_CheckedChanged);
            // 
            // panel_TimeDiff
            // 
            resources.ApplyResources(this.panel_TimeDiff, "panel_TimeDiff");
            this.panel_TimeDiff.BackColor = System.Drawing.SystemColors.Control;
            this.panel_TimeDiff.Controls.Add(this.button_TimeDiff);
            this.panel_TimeDiff.Controls.Add(this.checkBox_TimeDiff);
            this.panel_TimeDiff.Name = "panel_TimeDiff";
            this.panel_TimeDiff.Click += new System.EventHandler(this.panel_TimeDiff_Click);
            this.panel_TimeDiff.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Control_MouseDown);
            this.panel_TimeDiff.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Control_MouseMove);
            this.panel_TimeDiff.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Control_MouseUp);
            // 
            // button_TimeDiff
            // 
            resources.ApplyResources(this.button_TimeDiff, "button_TimeDiff");
            this.button_TimeDiff.FlatAppearance.BorderSize = 0;
            this.button_TimeDiff.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.Control;
            this.button_TimeDiff.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.Control;
            this.button_TimeDiff.Image = global::ControlLibrary.Properties.Resources.text_fields;
            this.button_TimeDiff.Name = "button_TimeDiff";
            this.button_TimeDiff.UseVisualStyleBackColor = true;
            this.button_TimeDiff.Click += new System.EventHandler(this.panel_TimeDiff_Click);
            this.button_TimeDiff.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Control_MouseDown);
            this.button_TimeDiff.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Control_MouseMove);
            this.button_TimeDiff.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Control_MouseUp);
            // 
            // checkBox_TimeDiff
            // 
            resources.ApplyResources(this.checkBox_TimeDiff, "checkBox_TimeDiff");
            this.checkBox_TimeDiff.Name = "checkBox_TimeDiff";
            this.checkBox_TimeDiff.UseVisualStyleBackColor = true;
            this.checkBox_TimeDiff.CheckedChanged += new System.EventHandler(this.checkBox_Elements_CheckedChanged);
            // 
            // panel_Time
            // 
            resources.ApplyResources(this.panel_Time, "panel_Time");
            this.panel_Time.BackColor = System.Drawing.SystemColors.Control;
            this.panel_Time.Controls.Add(this.checkBox_Time);
            this.panel_Time.Controls.Add(this.button_Time);
            this.panel_Time.Name = "panel_Time";
            this.panel_Time.Click += new System.EventHandler(this.panel_Time_Click);
            this.panel_Time.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Control_MouseDown);
            this.panel_Time.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Control_MouseMove);
            this.panel_Time.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Control_MouseUp);
            // 
            // checkBox_Time
            // 
            resources.ApplyResources(this.checkBox_Time, "checkBox_Time");
            this.checkBox_Time.Name = "checkBox_Time";
            this.checkBox_Time.UseVisualStyleBackColor = true;
            this.checkBox_Time.CheckedChanged += new System.EventHandler(this.checkBox_Elements_CheckedChanged);
            // 
            // button_Time
            // 
            resources.ApplyResources(this.button_Time, "button_Time");
            this.button_Time.FlatAppearance.BorderSize = 0;
            this.button_Time.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.Control;
            this.button_Time.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.Control;
            this.button_Time.Image = global::ControlLibrary.Properties.Resources.hr_min_icon;
            this.button_Time.Name = "button_Time";
            this.button_Time.UseVisualStyleBackColor = true;
            this.button_Time.Click += new System.EventHandler(this.panel_Time_Click);
            this.button_Time.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Control_MouseDown);
            this.button_Time.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Control_MouseMove);
            this.button_Time.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Control_MouseUp);
            // 
            // panel_TimeZone
            // 
            resources.ApplyResources(this.panel_TimeZone, "panel_TimeZone");
            this.panel_TimeZone.BackColor = System.Drawing.SystemColors.Control;
            this.panel_TimeZone.Controls.Add(this.button_TimeZone);
            this.panel_TimeZone.Controls.Add(this.checkBox_TimeZone);
            this.panel_TimeZone.Name = "panel_TimeZone";
            this.panel_TimeZone.Click += new System.EventHandler(this.panel_TimeZone_Click);
            this.panel_TimeZone.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Control_MouseDown);
            this.panel_TimeZone.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Control_MouseMove);
            this.panel_TimeZone.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Control_MouseUp);
            // 
            // button_TimeZone
            // 
            resources.ApplyResources(this.button_TimeZone, "button_TimeZone");
            this.button_TimeZone.FlatAppearance.BorderSize = 0;
            this.button_TimeZone.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.Control;
            this.button_TimeZone.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.Control;
            this.button_TimeZone.Image = global::ControlLibrary.Properties.Resources.text_fields;
            this.button_TimeZone.Name = "button_TimeZone";
            this.button_TimeZone.UseVisualStyleBackColor = true;
            this.button_TimeZone.Click += new System.EventHandler(this.panel_TimeZone_Click);
            this.button_TimeZone.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Control_MouseDown);
            this.button_TimeZone.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Control_MouseMove);
            this.button_TimeZone.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Control_MouseUp);
            // 
            // checkBox_TimeZone
            // 
            resources.ApplyResources(this.checkBox_TimeZone, "checkBox_TimeZone");
            this.checkBox_TimeZone.Name = "checkBox_TimeZone";
            this.checkBox_TimeZone.UseVisualStyleBackColor = true;
            this.checkBox_TimeZone.CheckedChanged += new System.EventHandler(this.checkBox_Elements_CheckedChanged);
            // 
            // panel_ButtonNext
            // 
            resources.ApplyResources(this.panel_ButtonNext, "panel_ButtonNext");
            this.panel_ButtonNext.BackColor = System.Drawing.SystemColors.Control;
            this.panel_ButtonNext.Controls.Add(this.button_ButtonNext);
            this.panel_ButtonNext.Controls.Add(this.checkBox_ButtonNext);
            this.panel_ButtonNext.Name = "panel_ButtonNext";
            this.panel_ButtonNext.Click += new System.EventHandler(this.panel_ButtonNext_Click);
            this.panel_ButtonNext.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Control_MouseDown);
            this.panel_ButtonNext.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Control_MouseMove);
            this.panel_ButtonNext.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Control_MouseUp);
            // 
            // button_ButtonNext
            // 
            resources.ApplyResources(this.button_ButtonNext, "button_ButtonNext");
            this.button_ButtonNext.FlatAppearance.BorderSize = 0;
            this.button_ButtonNext.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.Control;
            this.button_ButtonNext.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.Control;
            this.button_ButtonNext.Image = global::ControlLibrary.Properties.Resources.button;
            this.button_ButtonNext.Name = "button_ButtonNext";
            this.button_ButtonNext.UseVisualStyleBackColor = true;
            this.button_ButtonNext.Click += new System.EventHandler(this.panel_ButtonNext_Click);
            this.button_ButtonNext.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Control_MouseDown);
            this.button_ButtonNext.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Control_MouseMove);
            this.button_ButtonNext.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Control_MouseUp);
            // 
            // checkBox_ButtonNext
            // 
            resources.ApplyResources(this.checkBox_ButtonNext, "checkBox_ButtonNext");
            this.checkBox_ButtonNext.Name = "checkBox_ButtonNext";
            this.checkBox_ButtonNext.UseVisualStyleBackColor = true;
            this.checkBox_ButtonNext.CheckedChanged += new System.EventHandler(this.checkBox_Elements_CheckedChanged);
            // 
            // panel_Icon
            // 
            resources.ApplyResources(this.panel_Icon, "panel_Icon");
            this.panel_Icon.BackColor = System.Drawing.SystemColors.Control;
            this.panel_Icon.Controls.Add(this.button_Icon);
            this.panel_Icon.Controls.Add(this.checkBox_Icon);
            this.panel_Icon.Name = "panel_Icon";
            this.panel_Icon.Click += new System.EventHandler(this.panel_Icon_Click);
            this.panel_Icon.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Control_MouseDown);
            this.panel_Icon.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Control_MouseMove);
            this.panel_Icon.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Control_MouseUp);
            // 
            // button_Icon
            // 
            resources.ApplyResources(this.button_Icon, "button_Icon");
            this.button_Icon.FlatAppearance.BorderSize = 0;
            this.button_Icon.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.Control;
            this.button_Icon.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.Control;
            this.button_Icon.Image = global::ControlLibrary.Properties.Resources.wallpaper_18;
            this.button_Icon.Name = "button_Icon";
            this.button_Icon.UseVisualStyleBackColor = true;
            this.button_Icon.Click += new System.EventHandler(this.panel_Icon_Click);
            this.button_Icon.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Control_MouseDown);
            this.button_Icon.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Control_MouseMove);
            this.button_Icon.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Control_MouseUp);
            // 
            // checkBox_Icon
            // 
            resources.ApplyResources(this.checkBox_Icon, "checkBox_Icon");
            this.checkBox_Icon.Name = "checkBox_Icon";
            this.checkBox_Icon.UseVisualStyleBackColor = true;
            this.checkBox_Icon.CheckedChanged += new System.EventHandler(this.checkBox_Elements_CheckedChanged);
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
            this.button_ElementName.Image = global::ControlLibrary.Properties.Resources.world_clock;
            this.button_ElementName.Name = "button_ElementName";
            this.button_ElementName.UseVisualStyleBackColor = false;
            this.button_ElementName.SizeChanged += new System.EventHandler(this.button_ElementName_SizeChanged);
            this.button_ElementName.Click += new System.EventHandler(this.button_ElementName_Click);
            this.button_ElementName.MouseDown += new System.Windows.Forms.MouseEventHandler(this.button_ElementName_MouseDown);
            this.button_ElementName.MouseMove += new System.Windows.Forms.MouseEventHandler(this.button_ElementName_MouseMove);
            this.button_ElementName.MouseUp += new System.Windows.Forms.MouseEventHandler(this.button_ElementName_MouseUp);
            // 
            // UCtrl_WorldClock_Elm
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
            this.Name = "UCtrl_WorldClock_Elm";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel_CityName.ResumeLayout(false);
            this.panel_CityName.PerformLayout();
            this.panel_ButtonPrev.ResumeLayout(false);
            this.panel_ButtonPrev.PerformLayout();
            this.panel_TimeDiff.ResumeLayout(false);
            this.panel_TimeDiff.PerformLayout();
            this.panel_Time.ResumeLayout(false);
            this.panel_Time.PerformLayout();
            this.panel_TimeZone.ResumeLayout(false);
            this.panel_TimeZone.PerformLayout();
            this.panel_ButtonNext.ResumeLayout(false);
            this.panel_ButtonNext.PerformLayout();
            this.panel_Icon.ResumeLayout(false);
            this.panel_Icon.PerformLayout();
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
        private System.Windows.Forms.Panel panel_CityName;
        private System.Windows.Forms.Button button_CityName;
        public System.Windows.Forms.CheckBox checkBox_CityName;
        private System.Windows.Forms.Panel panel_ButtonPrev;
        private System.Windows.Forms.Button button_ButtonPrev;
        public System.Windows.Forms.CheckBox checkBox_ButtonPrev;
        private System.Windows.Forms.Panel panel_TimeDiff;
        private System.Windows.Forms.Button button_TimeDiff;
        public System.Windows.Forms.CheckBox checkBox_TimeDiff;
        private System.Windows.Forms.Panel panel_Time;
        public System.Windows.Forms.CheckBox checkBox_Time;
        private System.Windows.Forms.Button button_Time;
        private System.Windows.Forms.Panel panel_TimeZone;
        private System.Windows.Forms.Button button_TimeZone;
        public System.Windows.Forms.CheckBox checkBox_TimeZone;
        private System.Windows.Forms.Panel panel_ButtonNext;
        private System.Windows.Forms.Button button_ButtonNext;
        public System.Windows.Forms.CheckBox checkBox_ButtonNext;
        private System.Windows.Forms.Panel panel_Icon;
        private System.Windows.Forms.Button button_Icon;
        public System.Windows.Forms.CheckBox checkBox_Icon;
        protected System.Windows.Forms.Button button_ElementName;
    }
}
