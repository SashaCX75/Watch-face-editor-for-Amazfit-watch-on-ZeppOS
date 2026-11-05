namespace ControlLibrary
{
    partial class UCtrl_Image_Widgets_Opt
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UCtrl_Image_Widgets_Opt));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dataGridView_images = new System.Windows.Forms.DataGridView();
            this.Column_Index = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_Image = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_Visible = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.tableLayoutPanel_buttons = new System.Windows.Forms.TableLayoutPanel();
            this.button_add = new System.Windows.Forms.Button();
            this.button_del = new System.Windows.Forms.Button();
            this.uCtrl_Icon_Opt = new ControlLibrary.UCtrl_Icon_Opt();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_images)).BeginInit();
            this.tableLayoutPanel_buttons.SuspendLayout();
            this.SuspendLayout();
            //
            // dataGridView_images
            //
            this.dataGridView_images.AllowUserToAddRows = false;
            this.dataGridView_images.AllowUserToDeleteRows = false;
            this.dataGridView_images.AllowUserToResizeRows = false;
            this.dataGridView_images.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_images.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column_Index,
            this.Column_Image,
            this.Column_Visible});
            resources.ApplyResources(this.dataGridView_images, "dataGridView_images");
            this.dataGridView_images.MultiSelect = false;
            this.dataGridView_images.Name = "dataGridView_images";
            this.dataGridView_images.ReadOnly = true;
            this.dataGridView_images.RowHeadersVisible = false;
            this.dataGridView_images.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView_images.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView_images_CellContentClick);
            this.dataGridView_images.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView_images_RowEnter);
            this.dataGridView_images.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dataGridView_images_KeyDown);
            //
            // Column_Index
            //
            this.Column_Index.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.Column_Index.DefaultCellStyle = dataGridViewCellStyle1;
            this.Column_Index.FillWeight = 13F;
            resources.ApplyResources(this.Column_Index, "Column_Index");
            this.Column_Index.Name = "Column_Index";
            this.Column_Index.ReadOnly = true;
            this.Column_Index.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            //
            // Column_Image
            //
            this.Column_Image.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.Column_Image.DefaultCellStyle = dataGridViewCellStyle2;
            this.Column_Image.FillWeight = 120F;
            resources.ApplyResources(this.Column_Image, "Column_Image");
            this.Column_Image.Name = "Column_Image";
            this.Column_Image.ReadOnly = true;
            this.Column_Image.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            //
            // Column_Visible
            //
            this.Column_Visible.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Column_Visible.FillWeight = 10F;
            resources.ApplyResources(this.Column_Visible, "Column_Visible");
            this.Column_Visible.Name = "Column_Visible";
            this.Column_Visible.ReadOnly = true;
            this.Column_Visible.TrueValue = "true";
            //
            // tableLayoutPanel_buttons
            //
            resources.ApplyResources(this.tableLayoutPanel_buttons, "tableLayoutPanel_buttons");
            this.tableLayoutPanel_buttons.Controls.Add(this.button_add, 0, 0);
            this.tableLayoutPanel_buttons.Controls.Add(this.button_del, 1, 0);
            this.tableLayoutPanel_buttons.Name = "tableLayoutPanel_buttons";
            //
            // button_add
            //
            resources.ApplyResources(this.button_add, "button_add");
            this.button_add.Name = "button_add";
            this.button_add.UseVisualStyleBackColor = true;
            this.button_add.Click += new System.EventHandler(this.button_add_Click);
            //
            // button_del
            //
            resources.ApplyResources(this.button_del, "button_del");
            this.button_del.Name = "button_del";
            this.button_del.UseVisualStyleBackColor = true;
            this.button_del.Click += new System.EventHandler(this.button_del_Click);
            //
            // uCtrl_Icon_Opt
            //
            resources.ApplyResources(this.uCtrl_Icon_Opt, "uCtrl_Icon_Opt");
            this.uCtrl_Icon_Opt.Name = "uCtrl_Icon_Opt";
            this.uCtrl_Icon_Opt.ValueChanged += new ControlLibrary.UCtrl_Icon_Opt.ValueChangedHandler(this.uCtrl_Icon_Opt_ValueChanged);
            //
            // UCtrl_Image_Widgets_Opt
            //
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.uCtrl_Icon_Opt);
            this.Controls.Add(this.tableLayoutPanel_buttons);
            this.Controls.Add(this.dataGridView_images);
            this.Name = "UCtrl_Image_Widgets_Opt";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_images)).EndInit();
            this.tableLayoutPanel_buttons.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView_images;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_Index;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_Image;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Column_Visible;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel_buttons;
        private System.Windows.Forms.Button button_add;
        private System.Windows.Forms.Button button_del;
        private UCtrl_Icon_Opt uCtrl_Icon_Opt;
    }
}
