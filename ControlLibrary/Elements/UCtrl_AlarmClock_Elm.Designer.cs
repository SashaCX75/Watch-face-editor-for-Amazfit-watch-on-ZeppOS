namespace ControlLibrary
{
    partial class UCtrl_AlarmClock_Elm
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel_Number_Font = new System.Windows.Forms.Panel();
            this.checkBox_Number_Font = new System.Windows.Forms.CheckBox();
            this.panel_Number = new System.Windows.Forms.Panel();
            this.checkBox_Number = new System.Windows.Forms.CheckBox();
            this.panel_Icon = new System.Windows.Forms.Panel();
            this.checkBox_Icon = new System.Windows.Forms.CheckBox();
            this.pictureBox_Arrow_Down = new System.Windows.Forms.PictureBox();
            this.pictureBox_NotShow = new System.Windows.Forms.PictureBox();
            this.pictureBox_Arrow_Right = new System.Windows.Forms.PictureBox();
            this.pictureBox_Show = new System.Windows.Forms.PictureBox();
            this.pictureBox_Del = new System.Windows.Forms.PictureBox();
            this.button_Number_Font = new System.Windows.Forms.Button();
            this.button_Number = new System.Windows.Forms.Button();
            this.button_Icon = new System.Windows.Forms.Button();
            this.button_ElementName = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel_Number_Font.SuspendLayout();
            this.panel_Number.SuspendLayout();
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
            this.tableLayoutPanel1.AllowDrop = true;
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.panel_Number_Font, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel_Number, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel_Icon, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 28);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(220, 76);
            this.tableLayoutPanel1.TabIndex = 45;
            this.tableLayoutPanel1.Visible = false;
            this.tableLayoutPanel1.DragDrop += new System.Windows.Forms.DragEventHandler(this.tableLayoutPanel1_DragDrop);
            this.tableLayoutPanel1.DragOver += new System.Windows.Forms.DragEventHandler(this.tableLayoutPanel1_DragOver);
            // 
            // panel_Number_Font
            // 
            this.panel_Number_Font.BackColor = System.Drawing.SystemColors.Control;
            this.panel_Number_Font.Controls.Add(this.button_Number_Font);
            this.panel_Number_Font.Controls.Add(this.checkBox_Number_Font);
            this.panel_Number_Font.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel_Number_Font.Location = new System.Drawing.Point(1, 26);
            this.panel_Number_Font.Margin = new System.Windows.Forms.Padding(0);
            this.panel_Number_Font.Name = "panel_Number_Font";
            this.panel_Number_Font.Size = new System.Drawing.Size(218, 24);
            this.panel_Number_Font.TabIndex = 43;
            this.panel_Number_Font.Click += new System.EventHandler(this.panel_Number_Font_Click);
            this.panel_Number_Font.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Control_MouseDown);
            this.panel_Number_Font.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Control_MouseMove);
            this.panel_Number_Font.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Control_MouseUp);
            // 
            // checkBox_Number_Font
            // 
            this.checkBox_Number_Font.AutoSize = true;
            this.checkBox_Number_Font.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.checkBox_Number_Font.Location = new System.Drawing.Point(20, 5);
            this.checkBox_Number_Font.Name = "checkBox_Number_Font";
            this.checkBox_Number_Font.Size = new System.Drawing.Size(15, 14);
            this.checkBox_Number_Font.TabIndex = 0;
            this.checkBox_Number_Font.UseVisualStyleBackColor = true;
            this.checkBox_Number_Font.CheckedChanged += new System.EventHandler(this.checkBox_Elements_CheckedChanged);
            // 
            // panel_Number
            // 
            this.panel_Number.BackColor = System.Drawing.SystemColors.Control;
            this.panel_Number.Controls.Add(this.button_Number);
            this.panel_Number.Controls.Add(this.checkBox_Number);
            this.panel_Number.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel_Number.Location = new System.Drawing.Point(1, 1);
            this.panel_Number.Margin = new System.Windows.Forms.Padding(0);
            this.panel_Number.Name = "panel_Number";
            this.panel_Number.Size = new System.Drawing.Size(218, 24);
            this.panel_Number.TabIndex = 1;
            this.panel_Number.Click += new System.EventHandler(this.panel_Number_Click);
            this.panel_Number.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Control_MouseDown);
            this.panel_Number.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Control_MouseMove);
            this.panel_Number.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Control_MouseUp);
            // 
            // checkBox_Number
            // 
            this.checkBox_Number.AutoSize = true;
            this.checkBox_Number.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.checkBox_Number.Location = new System.Drawing.Point(20, 5);
            this.checkBox_Number.Name = "checkBox_Number";
            this.checkBox_Number.Size = new System.Drawing.Size(15, 14);
            this.checkBox_Number.TabIndex = 0;
            this.checkBox_Number.UseVisualStyleBackColor = true;
            this.checkBox_Number.CheckedChanged += new System.EventHandler(this.checkBox_Elements_CheckedChanged);
            // 
            // panel_Icon
            // 
            this.panel_Icon.BackColor = System.Drawing.SystemColors.Control;
            this.panel_Icon.Controls.Add(this.button_Icon);
            this.panel_Icon.Controls.Add(this.checkBox_Icon);
            this.panel_Icon.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel_Icon.Location = new System.Drawing.Point(1, 51);
            this.panel_Icon.Margin = new System.Windows.Forms.Padding(0);
            this.panel_Icon.Name = "panel_Icon";
            this.panel_Icon.Size = new System.Drawing.Size(218, 24);
            this.panel_Icon.TabIndex = 6;
            this.panel_Icon.Click += new System.EventHandler(this.panel_Icon_Click);
            this.panel_Icon.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Control_MouseDown);
            this.panel_Icon.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Control_MouseMove);
            this.panel_Icon.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Control_MouseUp);
            // 
            // checkBox_Icon
            // 
            this.checkBox_Icon.AutoSize = true;
            this.checkBox_Icon.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.checkBox_Icon.Location = new System.Drawing.Point(20, 5);
            this.checkBox_Icon.Name = "checkBox_Icon";
            this.checkBox_Icon.Size = new System.Drawing.Size(15, 14);
            this.checkBox_Icon.TabIndex = 0;
            this.checkBox_Icon.UseVisualStyleBackColor = true;
            this.checkBox_Icon.CheckedChanged += new System.EventHandler(this.checkBox_Elements_CheckedChanged);
            // 
            // pictureBox_Arrow_Down
            // 
            this.pictureBox_Arrow_Down.BackgroundImage = global::ControlLibrary.Properties.Resources.arrow_down;
            this.pictureBox_Arrow_Down.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pictureBox_Arrow_Down.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.pictureBox_Arrow_Down.Location = new System.Drawing.Point(131, 2);
            this.pictureBox_Arrow_Down.Name = "pictureBox_Arrow_Down";
            this.pictureBox_Arrow_Down.Size = new System.Drawing.Size(24, 24);
            this.pictureBox_Arrow_Down.TabIndex = 44;
            this.pictureBox_Arrow_Down.TabStop = false;
            this.pictureBox_Arrow_Down.Visible = false;
            this.pictureBox_Arrow_Down.Click += new System.EventHandler(this.button_ElementName_Click);
            // 
            // pictureBox_NotShow
            // 
            this.pictureBox_NotShow.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox_NotShow.BackgroundImage = global::ControlLibrary.Properties.Resources.outline_visibility_off_black_24;
            this.pictureBox_NotShow.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pictureBox_NotShow.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.pictureBox_NotShow.Location = new System.Drawing.Point(101, 2);
            this.pictureBox_NotShow.Name = "pictureBox_NotShow";
            this.pictureBox_NotShow.Size = new System.Drawing.Size(24, 24);
            this.pictureBox_NotShow.TabIndex = 49;
            this.pictureBox_NotShow.TabStop = false;
            this.pictureBox_NotShow.Visible = false;
            this.pictureBox_NotShow.Click += new System.EventHandler(this.pictureBox_NotShow_Click);
            // 
            // pictureBox_Arrow_Right
            // 
            this.pictureBox_Arrow_Right.BackgroundImage = global::ControlLibrary.Properties.Resources.arrow_right;
            this.pictureBox_Arrow_Right.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pictureBox_Arrow_Right.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.pictureBox_Arrow_Right.Location = new System.Drawing.Point(1, 2);
            this.pictureBox_Arrow_Right.Name = "pictureBox_Arrow_Right";
            this.pictureBox_Arrow_Right.Size = new System.Drawing.Size(24, 24);
            this.pictureBox_Arrow_Right.TabIndex = 48;
            this.pictureBox_Arrow_Right.TabStop = false;
            this.pictureBox_Arrow_Right.Click += new System.EventHandler(this.button_ElementName_Click);
            // 
            // pictureBox_Show
            // 
            this.pictureBox_Show.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox_Show.BackgroundImage = global::ControlLibrary.Properties.Resources.outline_visibility_black_24;
            this.pictureBox_Show.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pictureBox_Show.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.pictureBox_Show.Location = new System.Drawing.Point(163, 2);
            this.pictureBox_Show.Name = "pictureBox_Show";
            this.pictureBox_Show.Size = new System.Drawing.Size(24, 24);
            this.pictureBox_Show.TabIndex = 47;
            this.pictureBox_Show.TabStop = false;
            this.pictureBox_Show.Click += new System.EventHandler(this.pictureBox_Show_Click);
            // 
            // pictureBox_Del
            // 
            this.pictureBox_Del.BackgroundImage = global::ControlLibrary.Properties.Resources.outline_delete_forever_black_24;
            this.pictureBox_Del.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pictureBox_Del.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.pictureBox_Del.Location = new System.Drawing.Point(192, 2);
            this.pictureBox_Del.Name = "pictureBox_Del";
            this.pictureBox_Del.Size = new System.Drawing.Size(24, 24);
            this.pictureBox_Del.TabIndex = 46;
            this.pictureBox_Del.TabStop = false;
            this.pictureBox_Del.Click += new System.EventHandler(this.pictureBox_Del_Click);
            // 
            // button_Number_Font
            // 
            this.button_Number_Font.FlatAppearance.BorderSize = 0;
            this.button_Number_Font.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.Control;
            this.button_Number_Font.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.Control;
            this.button_Number_Font.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_Number_Font.Image = global::ControlLibrary.Properties.Resources.text_fields;
            this.button_Number_Font.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button_Number_Font.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.button_Number_Font.Location = new System.Drawing.Point(33, 0);
            this.button_Number_Font.Name = "button_Number_Font";
            this.button_Number_Font.Size = new System.Drawing.Size(160, 24);
            this.button_Number_Font.TabIndex = 4;
            this.button_Number_Font.Text = "Данные шрифтом";
            this.button_Number_Font.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.button_Number_Font.UseVisualStyleBackColor = true;
            this.button_Number_Font.Click += new System.EventHandler(this.panel_Number_Font_Click);
            this.button_Number_Font.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Control_MouseDown);
            this.button_Number_Font.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Control_MouseMove);
            this.button_Number_Font.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Control_MouseUp);
            // 
            // button_Number
            // 
            this.button_Number.FlatAppearance.BorderSize = 0;
            this.button_Number.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.Control;
            this.button_Number.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.Control;
            this.button_Number.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_Number.Image = global::ControlLibrary.Properties.Resources.text_icon;
            this.button_Number.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button_Number.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.button_Number.Location = new System.Drawing.Point(33, 0);
            this.button_Number.Name = "button_Number";
            this.button_Number.Size = new System.Drawing.Size(145, 24);
            this.button_Number.TabIndex = 3;
            this.button_Number.Text = "Цифровые значения";
            this.button_Number.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.button_Number.UseVisualStyleBackColor = true;
            this.button_Number.Click += new System.EventHandler(this.panel_Number_Click);
            this.button_Number.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Control_MouseDown);
            this.button_Number.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Control_MouseMove);
            this.button_Number.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Control_MouseUp);
            // 
            // button_Icon
            // 
            this.button_Icon.FlatAppearance.BorderSize = 0;
            this.button_Icon.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.Control;
            this.button_Icon.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.Control;
            this.button_Icon.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_Icon.Image = global::ControlLibrary.Properties.Resources.wallpaper_18;
            this.button_Icon.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button_Icon.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.button_Icon.Location = new System.Drawing.Point(33, 0);
            this.button_Icon.Name = "button_Icon";
            this.button_Icon.Size = new System.Drawing.Size(160, 24);
            this.button_Icon.TabIndex = 4;
            this.button_Icon.Text = "Иконка";
            this.button_Icon.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.button_Icon.UseVisualStyleBackColor = true;
            this.button_Icon.Click += new System.EventHandler(this.panel_Icon_Click);
            this.button_Icon.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Control_MouseDown);
            this.button_Icon.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Control_MouseMove);
            this.button_Icon.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Control_MouseUp);
            // 
            // button_ElementName
            // 
            this.button_ElementName.BackColor = System.Drawing.SystemColors.Control;
            this.button_ElementName.Dock = System.Windows.Forms.DockStyle.Top;
            this.button_ElementName.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_ElementName.Image = global::ControlLibrary.Properties.Resources.alarm_clock;
            this.button_ElementName.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button_ElementName.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.button_ElementName.Location = new System.Drawing.Point(0, 0);
            this.button_ElementName.Name = "button_ElementName";
            this.button_ElementName.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.button_ElementName.Size = new System.Drawing.Size(220, 28);
            this.button_ElementName.TabIndex = 43;
            this.button_ElementName.Text = "Будильник";
            this.button_ElementName.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.button_ElementName.UseVisualStyleBackColor = false;
            this.button_ElementName.SizeChanged += new System.EventHandler(this.button_ElementName_SizeChanged);
            this.button_ElementName.Click += new System.EventHandler(this.button_ElementName_Click);
            this.button_ElementName.MouseDown += new System.Windows.Forms.MouseEventHandler(this.button_ElementName_MouseDown);
            this.button_ElementName.MouseMove += new System.Windows.Forms.MouseEventHandler(this.button_ElementName_MouseMove);
            this.button_ElementName.MouseUp += new System.Windows.Forms.MouseEventHandler(this.button_ElementName_MouseUp);
            // 
            // UCtrl_AlarmClock_Elm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pictureBox_Arrow_Down);
            this.Controls.Add(this.pictureBox_NotShow);
            this.Controls.Add(this.pictureBox_Arrow_Right);
            this.Controls.Add(this.pictureBox_Show);
            this.Controls.Add(this.pictureBox_Del);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.button_ElementName);
            this.Name = "UCtrl_AlarmClock_Elm";
            this.Size = new System.Drawing.Size(220, 250);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel_Number_Font.ResumeLayout(false);
            this.panel_Number_Font.PerformLayout();
            this.panel_Number.ResumeLayout(false);
            this.panel_Number.PerformLayout();
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
        private System.Windows.Forms.Panel panel_Number_Font;
        private System.Windows.Forms.Button button_Number_Font;
        public System.Windows.Forms.CheckBox checkBox_Number_Font;
        private System.Windows.Forms.Panel panel_Number;
        private System.Windows.Forms.Button button_Number;
        public System.Windows.Forms.CheckBox checkBox_Number;
        private System.Windows.Forms.Panel panel_Icon;
        private System.Windows.Forms.Button button_Icon;
        public System.Windows.Forms.CheckBox checkBox_Icon;
        protected System.Windows.Forms.Button button_ElementName;
    }
}
