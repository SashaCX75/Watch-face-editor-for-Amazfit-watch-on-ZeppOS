namespace ControlLibrary
{
    partial class UCtrl_SleepChart_Opt
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UCtrl_SleepChart_Opt));
            this.numericUpDown_radius = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.numericUpDown_height = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown_width = new System.Windows.Forms.NumericUpDown();
            this.label06 = new System.Windows.Forms.Label();
            this.label07 = new System.Windows.Forms.Label();
            this.comboBox_image = new System.Windows.Forms.ComboBox();
            this.numericUpDown_posX = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown_posY = new System.Windows.Forms.NumericUpDown();
            this.label01 = new System.Windows.Forms.Label();
            this.label02 = new System.Windows.Forms.Label();
            this.label04 = new System.Windows.Forms.Label();
            this.label05 = new System.Windows.Forms.Label();
            this.panel_settings = new System.Windows.Forms.Panel();
            this.groupBox_chartSleep = new System.Windows.Forms.GroupBox();
            this.comboBox_wakeup_colour = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBox_REM_colour = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.comboBox_lightSleep_colour = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.comboBox_deepSleep_colour = new System.Windows.Forms.ComboBox();
            this.label16 = new System.Windows.Forms.Label();
            this.checkBox_use_chartSleep = new System.Windows.Forms.CheckBox();
            this.groupBox_chartHR = new System.Windows.Forms.GroupBox();
            this.numericUpDown_hr_lineWidth = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.comboBox_hr_colour = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.checkBox_use_chartHR = new System.Windows.Forms.CheckBox();
            this.contextMenuStrip_Y = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.вставитьКоординатуYToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.копироватьToolStripMenuItemY = new System.Windows.Forms.ToolStripMenuItem();
            this.вставитьToolStripMenuItemY = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip_X = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.вставитьКоординатуХToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.копироватьToolStripMenuItemX = new System.Windows.Forms.ToolStripMenuItem();
            this.вставитьToolStripMenuItemX = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_radius)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_height)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_width)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_posX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_posY)).BeginInit();
            this.panel_settings.SuspendLayout();
            this.groupBox_chartSleep.SuspendLayout();
            this.groupBox_chartHR.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_hr_lineWidth)).BeginInit();
            this.contextMenuStrip_Y.SuspendLayout();
            this.contextMenuStrip_X.SuspendLayout();
            this.SuspendLayout();
            // 
            // numericUpDown_radius
            // 
            resources.ApplyResources(this.numericUpDown_radius, "numericUpDown_radius");
            this.numericUpDown_radius.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.numericUpDown_radius.Name = "numericUpDown_radius";
            this.numericUpDown_radius.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numericUpDown_radius.ValueChanged += new System.EventHandler(this.numericUpDown_ValueChanged);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // numericUpDown_height
            // 
            resources.ApplyResources(this.numericUpDown_height, "numericUpDown_height");
            this.numericUpDown_height.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.numericUpDown_height.Minimum = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.numericUpDown_height.Name = "numericUpDown_height";
            this.numericUpDown_height.Value = new decimal(new int[] {
            80,
            0,
            0,
            0});
            this.numericUpDown_height.ValueChanged += new System.EventHandler(this.numericUpDown_ValueChanged);
            this.numericUpDown_height.KeyDown += new System.Windows.Forms.KeyEventHandler(this.numericUpDown_size_KeyDown);
            this.numericUpDown_height.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.numericUpDown_height_MouseDoubleClick);
            // 
            // numericUpDown_width
            // 
            resources.ApplyResources(this.numericUpDown_width, "numericUpDown_width");
            this.numericUpDown_width.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.numericUpDown_width.Minimum = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.numericUpDown_width.Name = "numericUpDown_width";
            this.numericUpDown_width.Value = new decimal(new int[] {
            300,
            0,
            0,
            0});
            this.numericUpDown_width.ValueChanged += new System.EventHandler(this.numericUpDown_ValueChanged);
            this.numericUpDown_width.KeyDown += new System.Windows.Forms.KeyEventHandler(this.numericUpDown_size_KeyDown);
            this.numericUpDown_width.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.numericUpDown_width_MouseDoubleClick);
            // 
            // label06
            // 
            resources.ApplyResources(this.label06, "label06");
            this.label06.Name = "label06";
            // 
            // label07
            // 
            resources.ApplyResources(this.label07, "label07");
            this.label07.Name = "label07";
            // 
            // comboBox_image
            // 
            resources.ApplyResources(this.comboBox_image, "comboBox_image");
            this.comboBox_image.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.comboBox_image.DropDownWidth = 135;
            this.comboBox_image.FormattingEnabled = true;
            this.comboBox_image.Name = "comboBox_image";
            this.comboBox_image.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.comboBox_DrawItem);
            this.comboBox_image.MeasureItem += new System.Windows.Forms.MeasureItemEventHandler(this.comboBox_MeasureItem);
            this.comboBox_image.SelectedIndexChanged += new System.EventHandler(this.comboBox_SelectedIndexChanged);
            this.comboBox_image.KeyDown += new System.Windows.Forms.KeyEventHandler(this.comboBox_KeyDown);
            this.comboBox_image.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.comboBox_KeyPress);
            // 
            // numericUpDown_posX
            // 
            resources.ApplyResources(this.numericUpDown_posX, "numericUpDown_posX");
            this.numericUpDown_posX.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.numericUpDown_posX.Minimum = new decimal(new int[] {
            999,
            0,
            0,
            -2147483648});
            this.numericUpDown_posX.Name = "numericUpDown_posX";
            this.numericUpDown_posX.ValueChanged += new System.EventHandler(this.numericUpDown_ValueChanged);
            this.numericUpDown_posX.KeyDown += new System.Windows.Forms.KeyEventHandler(this.numericUpDown_pos_KeyDown);
            this.numericUpDown_posX.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.numericUpDown_picturesX_MouseDoubleClick);
            // 
            // numericUpDown_posY
            // 
            resources.ApplyResources(this.numericUpDown_posY, "numericUpDown_posY");
            this.numericUpDown_posY.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.numericUpDown_posY.Minimum = new decimal(new int[] {
            999,
            0,
            0,
            -2147483648});
            this.numericUpDown_posY.Name = "numericUpDown_posY";
            this.numericUpDown_posY.ValueChanged += new System.EventHandler(this.numericUpDown_ValueChanged);
            this.numericUpDown_posY.KeyDown += new System.Windows.Forms.KeyEventHandler(this.numericUpDown_pos_KeyDown);
            this.numericUpDown_posY.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.numericUpDown_picturesY_MouseDoubleClick);
            // 
            // label01
            // 
            resources.ApplyResources(this.label01, "label01");
            this.label01.Name = "label01";
            // 
            // label02
            // 
            resources.ApplyResources(this.label02, "label02");
            this.label02.Name = "label02";
            // 
            // label04
            // 
            resources.ApplyResources(this.label04, "label04");
            this.label04.Name = "label04";
            // 
            // label05
            // 
            resources.ApplyResources(this.label05, "label05");
            this.label05.Name = "label05";
            // 
            // panel_settings
            // 
            resources.ApplyResources(this.panel_settings, "panel_settings");
            this.panel_settings.Controls.Add(this.label02);
            this.panel_settings.Controls.Add(this.label05);
            this.panel_settings.Controls.Add(this.label04);
            this.panel_settings.Controls.Add(this.label01);
            this.panel_settings.Controls.Add(this.numericUpDown_posY);
            this.panel_settings.Controls.Add(this.numericUpDown_height);
            this.panel_settings.Controls.Add(this.numericUpDown_posX);
            this.panel_settings.Controls.Add(this.numericUpDown_width);
            this.panel_settings.Controls.Add(this.comboBox_image);
            this.panel_settings.Controls.Add(this.label06);
            this.panel_settings.Controls.Add(this.label07);
            this.panel_settings.Name = "panel_settings";
            // 
            // groupBox_chartSleep
            // 
            resources.ApplyResources(this.groupBox_chartSleep, "groupBox_chartSleep");
            this.groupBox_chartSleep.Controls.Add(this.comboBox_wakeup_colour);
            this.groupBox_chartSleep.Controls.Add(this.numericUpDown_radius);
            this.groupBox_chartSleep.Controls.Add(this.label2);
            this.groupBox_chartSleep.Controls.Add(this.label3);
            this.groupBox_chartSleep.Controls.Add(this.comboBox_REM_colour);
            this.groupBox_chartSleep.Controls.Add(this.label4);
            this.groupBox_chartSleep.Controls.Add(this.comboBox_lightSleep_colour);
            this.groupBox_chartSleep.Controls.Add(this.label5);
            this.groupBox_chartSleep.Controls.Add(this.comboBox_deepSleep_colour);
            this.groupBox_chartSleep.Controls.Add(this.label16);
            this.groupBox_chartSleep.Controls.Add(this.checkBox_use_chartSleep);
            this.groupBox_chartSleep.Name = "groupBox_chartSleep";
            this.groupBox_chartSleep.TabStop = false;
            this.groupBox_chartSleep.Paint += new System.Windows.Forms.PaintEventHandler(this.groupBox_Paint);
            // 
            // comboBox_wakeup_colour
            // 
            resources.ApplyResources(this.comboBox_wakeup_colour, "comboBox_wakeup_colour");
            this.comboBox_wakeup_colour.BackColor = System.Drawing.Color.Orange;
            this.comboBox_wakeup_colour.DropDownHeight = 1;
            this.comboBox_wakeup_colour.FormattingEnabled = true;
            this.comboBox_wakeup_colour.Name = "comboBox_wakeup_colour";
            this.comboBox_wakeup_colour.Click += new System.EventHandler(this.comboBox_color_Click);
            this.comboBox_wakeup_colour.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.comboBox_KeyPress);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // comboBox_REM_colour
            // 
            resources.ApplyResources(this.comboBox_REM_colour, "comboBox_REM_colour");
            this.comboBox_REM_colour.BackColor = System.Drawing.Color.LimeGreen;
            this.comboBox_REM_colour.DropDownHeight = 1;
            this.comboBox_REM_colour.FormattingEnabled = true;
            this.comboBox_REM_colour.Name = "comboBox_REM_colour";
            this.comboBox_REM_colour.Click += new System.EventHandler(this.comboBox_color_Click);
            this.comboBox_REM_colour.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.comboBox_KeyPress);
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // comboBox_lightSleep_colour
            // 
            resources.ApplyResources(this.comboBox_lightSleep_colour, "comboBox_lightSleep_colour");
            this.comboBox_lightSleep_colour.BackColor = System.Drawing.Color.SkyBlue;
            this.comboBox_lightSleep_colour.DropDownHeight = 1;
            this.comboBox_lightSleep_colour.FormattingEnabled = true;
            this.comboBox_lightSleep_colour.Name = "comboBox_lightSleep_colour";
            this.comboBox_lightSleep_colour.Click += new System.EventHandler(this.comboBox_color_Click);
            this.comboBox_lightSleep_colour.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.comboBox_KeyPress);
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // comboBox_deepSleep_colour
            // 
            resources.ApplyResources(this.comboBox_deepSleep_colour, "comboBox_deepSleep_colour");
            this.comboBox_deepSleep_colour.BackColor = System.Drawing.Color.RoyalBlue;
            this.comboBox_deepSleep_colour.DropDownHeight = 1;
            this.comboBox_deepSleep_colour.FormattingEnabled = true;
            this.comboBox_deepSleep_colour.Name = "comboBox_deepSleep_colour";
            this.comboBox_deepSleep_colour.Click += new System.EventHandler(this.comboBox_color_Click);
            this.comboBox_deepSleep_colour.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.comboBox_KeyPress);
            // 
            // label16
            // 
            resources.ApplyResources(this.label16, "label16");
            this.label16.Name = "label16";
            // 
            // checkBox_use_chartSleep
            // 
            resources.ApplyResources(this.checkBox_use_chartSleep, "checkBox_use_chartSleep");
            this.checkBox_use_chartSleep.Checked = true;
            this.checkBox_use_chartSleep.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_use_chartSleep.Name = "checkBox_use_chartSleep";
            this.checkBox_use_chartSleep.UseVisualStyleBackColor = true;
            this.checkBox_use_chartSleep.CheckedChanged += new System.EventHandler(this.checkBox_use_chartSleep_CheckedChanged);
            // 
            // groupBox_chartHR
            // 
            resources.ApplyResources(this.groupBox_chartHR, "groupBox_chartHR");
            this.groupBox_chartHR.Controls.Add(this.numericUpDown_hr_lineWidth);
            this.groupBox_chartHR.Controls.Add(this.label7);
            this.groupBox_chartHR.Controls.Add(this.comboBox_hr_colour);
            this.groupBox_chartHR.Controls.Add(this.label6);
            this.groupBox_chartHR.Controls.Add(this.checkBox_use_chartHR);
            this.groupBox_chartHR.Name = "groupBox_chartHR";
            this.groupBox_chartHR.TabStop = false;
            this.groupBox_chartHR.Paint += new System.Windows.Forms.PaintEventHandler(this.groupBox_Paint);
            // 
            // numericUpDown_hr_lineWidth
            // 
            resources.ApplyResources(this.numericUpDown_hr_lineWidth, "numericUpDown_hr_lineWidth");
            this.numericUpDown_hr_lineWidth.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.numericUpDown_hr_lineWidth.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown_hr_lineWidth.Name = "numericUpDown_hr_lineWidth";
            this.numericUpDown_hr_lineWidth.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.numericUpDown_hr_lineWidth.ValueChanged += new System.EventHandler(this.numericUpDown_ValueChanged);
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // comboBox_hr_colour
            // 
            resources.ApplyResources(this.comboBox_hr_colour, "comboBox_hr_colour");
            this.comboBox_hr_colour.BackColor = System.Drawing.Color.Red;
            this.comboBox_hr_colour.DropDownHeight = 1;
            this.comboBox_hr_colour.FormattingEnabled = true;
            this.comboBox_hr_colour.Name = "comboBox_hr_colour";
            this.comboBox_hr_colour.Click += new System.EventHandler(this.comboBox_color_Click);
            this.comboBox_hr_colour.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.comboBox_KeyPress);
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // checkBox_use_chartHR
            // 
            resources.ApplyResources(this.checkBox_use_chartHR, "checkBox_use_chartHR");
            this.checkBox_use_chartHR.Checked = true;
            this.checkBox_use_chartHR.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_use_chartHR.Name = "checkBox_use_chartHR";
            this.checkBox_use_chartHR.UseVisualStyleBackColor = true;
            this.checkBox_use_chartHR.CheckedChanged += new System.EventHandler(this.checkBox_use_chartHR_CheckedChanged);
            // 
            // contextMenuStrip_Y
            // 
            resources.ApplyResources(this.contextMenuStrip_Y, "contextMenuStrip_Y");
            this.contextMenuStrip_Y.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip_Y.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.вставитьКоординатуYToolStripMenuItem,
            this.копироватьToolStripMenuItemY,
            this.вставитьToolStripMenuItemY});
            this.contextMenuStrip_Y.Name = "contextMenuStrip_X";
            this.contextMenuStrip_Y.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip_Y_Opening);
            // 
            // вставитьКоординатуYToolStripMenuItem
            // 
            resources.ApplyResources(this.вставитьКоординатуYToolStripMenuItem, "вставитьКоординатуYToolStripMenuItem");
            this.вставитьКоординатуYToolStripMenuItem.Name = "вставитьКоординатуYToolStripMenuItem";
            this.вставитьКоординатуYToolStripMenuItem.Click += new System.EventHandler(this.вставитьКоординатуYToolStripMenuItem_Click);
            // 
            // копироватьToolStripMenuItemY
            // 
            resources.ApplyResources(this.копироватьToolStripMenuItemY, "копироватьToolStripMenuItemY");
            this.копироватьToolStripMenuItemY.Name = "копироватьToolStripMenuItemY";
            this.копироватьToolStripMenuItemY.Click += new System.EventHandler(this.копироватьToolStripMenuItem_Click);
            // 
            // вставитьToolStripMenuItemY
            // 
            resources.ApplyResources(this.вставитьToolStripMenuItemY, "вставитьToolStripMenuItemY");
            this.вставитьToolStripMenuItemY.Name = "вставитьToolStripMenuItemY";
            this.вставитьToolStripMenuItemY.Click += new System.EventHandler(this.вставитьToolStripMenuItem_Click);
            // 
            // contextMenuStrip_X
            // 
            resources.ApplyResources(this.contextMenuStrip_X, "contextMenuStrip_X");
            this.contextMenuStrip_X.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip_X.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.вставитьКоординатуХToolStripMenuItem,
            this.копироватьToolStripMenuItemX,
            this.вставитьToolStripMenuItemX});
            this.contextMenuStrip_X.Name = "contextMenuStrip_X";
            this.contextMenuStrip_X.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip_X_Opening);
            // 
            // вставитьКоординатуХToolStripMenuItem
            // 
            resources.ApplyResources(this.вставитьКоординатуХToolStripMenuItem, "вставитьКоординатуХToolStripMenuItem");
            this.вставитьКоординатуХToolStripMenuItem.Name = "вставитьКоординатуХToolStripMenuItem";
            this.вставитьКоординатуХToolStripMenuItem.Click += new System.EventHandler(this.вставитьКоординатуХToolStripMenuItem_Click);
            // 
            // копироватьToolStripMenuItemX
            // 
            resources.ApplyResources(this.копироватьToolStripMenuItemX, "копироватьToolStripMenuItemX");
            this.копироватьToolStripMenuItemX.Name = "копироватьToolStripMenuItemX";
            this.копироватьToolStripMenuItemX.Click += new System.EventHandler(this.копироватьToolStripMenuItem_Click);
            // 
            // вставитьToolStripMenuItemX
            // 
            resources.ApplyResources(this.вставитьToolStripMenuItemX, "вставитьToolStripMenuItemX");
            this.вставитьToolStripMenuItemX.Name = "вставитьToolStripMenuItemX";
            this.вставитьToolStripMenuItemX.Click += new System.EventHandler(this.вставитьToolStripMenuItem_Click);
            // 
            // UCtrl_SleepChart_Opt
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox_chartHR);
            this.Controls.Add(this.groupBox_chartSleep);
            this.Controls.Add(this.panel_settings);
            this.Name = "UCtrl_SleepChart_Opt";
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_radius)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_height)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_width)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_posX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_posY)).EndInit();
            this.panel_settings.ResumeLayout(false);
            this.panel_settings.PerformLayout();
            this.groupBox_chartSleep.ResumeLayout(false);
            this.groupBox_chartSleep.PerformLayout();
            this.groupBox_chartHR.ResumeLayout(false);
            this.groupBox_chartHR.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_hr_lineWidth)).EndInit();
            this.contextMenuStrip_Y.ResumeLayout(false);
            this.contextMenuStrip_X.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.NumericUpDown numericUpDown_radius;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.NumericUpDown numericUpDown_height;
        public System.Windows.Forms.NumericUpDown numericUpDown_width;
        private System.Windows.Forms.Label label06;
        private System.Windows.Forms.Label label07;
        public System.Windows.Forms.ComboBox comboBox_image;
        public System.Windows.Forms.NumericUpDown numericUpDown_posX;
        public System.Windows.Forms.NumericUpDown numericUpDown_posY;
        private System.Windows.Forms.Label label01;
        private System.Windows.Forms.Label label02;
        private System.Windows.Forms.Label label04;
        private System.Windows.Forms.Label label05;
        private System.Windows.Forms.Panel panel_settings;
        private System.Windows.Forms.GroupBox groupBox_chartSleep;
        public System.Windows.Forms.CheckBox checkBox_use_chartSleep;
        private System.Windows.Forms.ComboBox comboBox_wakeup_colour;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboBox_REM_colour;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox comboBox_lightSleep_colour;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox comboBox_deepSleep_colour;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.GroupBox groupBox_chartHR;
        public System.Windows.Forms.CheckBox checkBox_use_chartHR;
        public System.Windows.Forms.NumericUpDown numericUpDown_hr_lineWidth;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox comboBox_hr_colour;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip_Y;
        private System.Windows.Forms.ToolStripMenuItem вставитьКоординатуYToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem копироватьToolStripMenuItemY;
        private System.Windows.Forms.ToolStripMenuItem вставитьToolStripMenuItemY;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip_X;
        private System.Windows.Forms.ToolStripMenuItem вставитьКоординатуХToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem копироватьToolStripMenuItemX;
        private System.Windows.Forms.ToolStripMenuItem вставитьToolStripMenuItemX;
    }
}
