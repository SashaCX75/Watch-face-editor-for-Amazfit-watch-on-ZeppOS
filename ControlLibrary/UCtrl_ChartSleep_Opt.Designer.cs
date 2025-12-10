namespace ControlLibrary
{
    partial class UCtrl_ChartSleep_Opt
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
            this.comboBox_deepSleep_colour = new System.Windows.Forms.ComboBox();
            this.label16 = new System.Windows.Forms.Label();
            this.comboBox_lightSleep_colour = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBox_wakeup_colour = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBox_REM_colour = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // comboBox_deepSleep_colour
            // 
            this.comboBox_deepSleep_colour.BackColor = System.Drawing.Color.RoyalBlue;
            this.comboBox_deepSleep_colour.DropDownHeight = 1;
            this.comboBox_deepSleep_colour.FormattingEnabled = true;
            this.comboBox_deepSleep_colour.IntegralHeight = false;
            this.comboBox_deepSleep_colour.Location = new System.Drawing.Point(6, 23);
            this.comboBox_deepSleep_colour.Name = "comboBox_deepSleep_colour";
            this.comboBox_deepSleep_colour.Size = new System.Drawing.Size(75, 21);
            this.comboBox_deepSleep_colour.TabIndex = 173;
            this.comboBox_deepSleep_colour.Click += new System.EventHandler(this.comboBox_color_Click);
            this.comboBox_deepSleep_colour.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.comboBox_KeyPress);
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label16.Location = new System.Drawing.Point(6, 7);
            this.label16.Margin = new System.Windows.Forms.Padding(3);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(75, 13);
            this.label16.TabIndex = 174;
            this.label16.Text = "Глубокий сон";
            // 
            // comboBox_lightSleep_colour
            // 
            this.comboBox_lightSleep_colour.BackColor = System.Drawing.Color.SkyBlue;
            this.comboBox_lightSleep_colour.DropDownHeight = 1;
            this.comboBox_lightSleep_colour.FormattingEnabled = true;
            this.comboBox_lightSleep_colour.IntegralHeight = false;
            this.comboBox_lightSleep_colour.Location = new System.Drawing.Point(130, 23);
            this.comboBox_lightSleep_colour.Name = "comboBox_lightSleep_colour";
            this.comboBox_lightSleep_colour.Size = new System.Drawing.Size(75, 21);
            this.comboBox_lightSleep_colour.TabIndex = 175;
            this.comboBox_lightSleep_colour.Click += new System.EventHandler(this.comboBox_color_Click);
            this.comboBox_lightSleep_colour.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.comboBox_KeyPress);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label1.Location = new System.Drawing.Point(130, 7);
            this.label1.Margin = new System.Windows.Forms.Padding(3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 13);
            this.label1.TabIndex = 176;
            this.label1.Text = "Легкий сон";
            // 
            // comboBox_wakeup_colour
            // 
            this.comboBox_wakeup_colour.BackColor = System.Drawing.Color.Orange;
            this.comboBox_wakeup_colour.DropDownHeight = 1;
            this.comboBox_wakeup_colour.FormattingEnabled = true;
            this.comboBox_wakeup_colour.IntegralHeight = false;
            this.comboBox_wakeup_colour.Location = new System.Drawing.Point(130, 85);
            this.comboBox_wakeup_colour.Name = "comboBox_wakeup_colour";
            this.comboBox_wakeup_colour.Size = new System.Drawing.Size(75, 21);
            this.comboBox_wakeup_colour.TabIndex = 179;
            this.comboBox_wakeup_colour.Click += new System.EventHandler(this.comboBox_color_Click);
            this.comboBox_wakeup_colour.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.comboBox_KeyPress);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label2.Location = new System.Drawing.Point(130, 69);
            this.label2.Margin = new System.Windows.Forms.Padding(3);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 13);
            this.label2.TabIndex = 180;
            this.label2.Text = "Пробуждение";
            // 
            // comboBox_REM_colour
            // 
            this.comboBox_REM_colour.BackColor = System.Drawing.Color.LimeGreen;
            this.comboBox_REM_colour.DropDownHeight = 1;
            this.comboBox_REM_colour.FormattingEnabled = true;
            this.comboBox_REM_colour.IntegralHeight = false;
            this.comboBox_REM_colour.Location = new System.Drawing.Point(6, 85);
            this.comboBox_REM_colour.Name = "comboBox_REM_colour";
            this.comboBox_REM_colour.Size = new System.Drawing.Size(75, 21);
            this.comboBox_REM_colour.TabIndex = 177;
            this.comboBox_REM_colour.Click += new System.EventHandler(this.comboBox_color_Click);
            this.comboBox_REM_colour.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.comboBox_KeyPress);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label3.Location = new System.Drawing.Point(6, 69);
            this.label3.Margin = new System.Windows.Forms.Padding(3);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(31, 13);
            this.label3.TabIndex = 178;
            this.label3.Text = "REM";
            // 
            // UCtrl_ChartSleep_Opt
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.comboBox_wakeup_colour);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.comboBox_REM_colour);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.comboBox_lightSleep_colour);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBox_deepSleep_colour);
            this.Controls.Add(this.label16);
            this.Name = "UCtrl_ChartSleep_Opt";
            this.Size = new System.Drawing.Size(255, 220);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ComboBox comboBox_deepSleep_colour;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.ComboBox comboBox_lightSleep_colour;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBox_wakeup_colour;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBox_REM_colour;
        private System.Windows.Forms.Label label3;
    }
}
