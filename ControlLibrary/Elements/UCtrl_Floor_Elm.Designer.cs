﻿namespace ControlLibrary
{
    partial class UCtrl_Floor_Elm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UCtrl_Floor_Elm));
            this.SuspendLayout();
            // 
            // checkBox_Number_Font
            // 
            resources.ApplyResources(this.checkBox_Number_Font, "checkBox_Number_Font");
            // 
            // checkBox_Number
            // 
            resources.ApplyResources(this.checkBox_Number, "checkBox_Number");
            // 
            // checkBox_Icon
            // 
            resources.ApplyResources(this.checkBox_Icon, "checkBox_Icon");
            // 
            // button_ElementName
            // 
            resources.ApplyResources(this.button_ElementName, "button_ElementName");
            this.button_ElementName.Image = global::ControlLibrary.Properties.Resources.floor;
            // 
            // UCtrl_Floor_Elm
            // 
            resources.ApplyResources(this, "$this");
            this.Name = "UCtrl_Floor_Elm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
    }
}
