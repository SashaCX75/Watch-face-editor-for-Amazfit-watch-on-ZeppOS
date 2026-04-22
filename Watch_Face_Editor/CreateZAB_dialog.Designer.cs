namespace Watch_Face_Editor
{
    partial class CreateZAB_dialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CreateZAB_dialog));
            this.label_name = new System.Windows.Forms.Label();
            this.textBox_name = new System.Windows.Forms.TextBox();
            this.textBox_id = new System.Windows.Forms.TextBox();
            this.label_id = new System.Windows.Forms.Label();
            this.label_version = new System.Windows.Forms.Label();
            this.numeric_version = new System.Windows.Forms.NumericUpDown();
            this.button_save = new System.Windows.Forms.Button();
            this.button_cancel = new System.Windows.Forms.Button();
            this.button_savePNG = new System.Windows.Forms.Button();
            this.button_saveGIF = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numeric_version)).BeginInit();
            this.SuspendLayout();
            // 
            // label_name
            // 
            resources.ApplyResources(this.label_name, "label_name");
            this.label_name.Name = "label_name";
            // 
            // textBox_name
            // 
            resources.ApplyResources(this.textBox_name, "textBox_name");
            this.textBox_name.Name = "textBox_name";
            this.textBox_name.TextChanged += new System.EventHandler(this.textBox_name_TextChanged);
            // 
            // textBox_id
            // 
            resources.ApplyResources(this.textBox_id, "textBox_id");
            this.textBox_id.Name = "textBox_id";
            this.textBox_id.TextChanged += new System.EventHandler(this.textBox_id_TextChanged);
            // 
            // label_id
            // 
            resources.ApplyResources(this.label_id, "label_id");
            this.label_id.Name = "label_id";
            // 
            // label_version
            // 
            resources.ApplyResources(this.label_version, "label_version");
            this.label_version.Name = "label_version";
            // 
            // numeric_version
            // 
            resources.ApplyResources(this.numeric_version, "numeric_version");
            this.numeric_version.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.numeric_version.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numeric_version.Name = "numeric_version";
            this.numeric_version.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numeric_version.ValueChanged += new System.EventHandler(this.numeric_version_ValueChanged);
            // 
            // button_save
            // 
            resources.ApplyResources(this.button_save, "button_save");
            this.button_save.Name = "button_save";
            this.button_save.UseVisualStyleBackColor = true;
            this.button_save.Click += new System.EventHandler(this.button_save_Click);
            // 
            // button_cancel
            // 
            resources.ApplyResources(this.button_cancel, "button_cancel");
            this.button_cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button_cancel.Name = "button_cancel";
            this.button_cancel.UseVisualStyleBackColor = true;
            this.button_cancel.Click += new System.EventHandler(this.button_cancel_Click);
            // 
            // button_savePNG
            // 
            resources.ApplyResources(this.button_savePNG, "button_savePNG");
            this.button_savePNG.Name = "button_savePNG";
            this.button_savePNG.UseVisualStyleBackColor = true;
            this.button_savePNG.Click += new System.EventHandler(this.button_savePNG_Click);
            // 
            // button_saveGIF
            // 
            resources.ApplyResources(this.button_saveGIF, "button_saveGIF");
            this.button_saveGIF.Name = "button_saveGIF";
            this.button_saveGIF.UseVisualStyleBackColor = true;
            this.button_saveGIF.Click += new System.EventHandler(this.button_saveGIF_Click);
            // 
            // CreateZAB_dialog
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.button_cancel;
            this.Controls.Add(this.button_saveGIF);
            this.Controls.Add(this.button_savePNG);
            this.Controls.Add(this.button_cancel);
            this.Controls.Add(this.button_save);
            this.Controls.Add(this.numeric_version);
            this.Controls.Add(this.label_version);
            this.Controls.Add(this.textBox_id);
            this.Controls.Add(this.label_id);
            this.Controls.Add(this.textBox_name);
            this.Controls.Add(this.label_name);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CreateZAB_dialog";
            ((System.ComponentModel.ISupportInitialize)(this.numeric_version)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label_name;
        private System.Windows.Forms.TextBox textBox_name;
        private System.Windows.Forms.TextBox textBox_id;
        private System.Windows.Forms.Label label_id;
        private System.Windows.Forms.Label label_version;
        private System.Windows.Forms.NumericUpDown numeric_version;
        private System.Windows.Forms.Button button_save;
        private System.Windows.Forms.Button button_cancel;
        private System.Windows.Forms.Button button_savePNG;
        private System.Windows.Forms.Button button_saveGIF;
    }
}