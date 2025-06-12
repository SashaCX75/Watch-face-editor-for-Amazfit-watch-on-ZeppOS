namespace ControlLibrary
{
    partial class UCtrl_BodyTemp_Elm
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

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // button_ElementName
            // 
            this.button_ElementName.Image = global::ControlLibrary.Properties.Resources.body_temp;
            this.button_ElementName.Text = "Температура тела";
            // 
            // UCtrl_BodyTemp_Elm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.Name = "UCtrl_BodyTemp_Elm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
    }
}
