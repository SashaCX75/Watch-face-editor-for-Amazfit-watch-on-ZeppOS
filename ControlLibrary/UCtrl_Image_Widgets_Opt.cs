using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace ControlLibrary
{
    /// <summary>Настройки элемента с набором изображений: список изображений и параметры выбранного</summary>
    public partial class UCtrl_Image_Widgets_Opt : UserControl
    {
        private bool setValue; // режим задания параметров
        public Object _ImageWidgets; // Общий элемент с изображениями

        /// <summary>Максимальное количество изображений в элементе</summary>
        public const int MaxImagesCount = 30;

        public UCtrl_Image_Widgets_Opt()
        {
            InitializeComponent();
            setValue = false;
            dataGridView_images.Columns[0].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }

        [Browsable(true)]
        [Description("Происходит при изменении параметров выбранного изображения")]
        public event ValueChangedHandler ValueChanged;
        public delegate void ValueChangedHandler(object sender, EventArgs eventArgs, int rowIndex);

        [Browsable(true)]
        [Description("Добавление изображения")]
        public event AddImageHandler AddImage;
        public delegate void AddImageHandler(int rowIndex);

        [Browsable(true)]
        [Description("Удаление изображения")]
        public event DelImageHandler DelImage;
        public delegate void DelImageHandler(int rowIndex);

        [Browsable(true)]
        [Description("Выбрали другое изображение")]
        public event SelectImageWidgetHandler SelectImageWidget;
        public delegate void SelectImageWidgetHandler(int rowIndex);

        [Browsable(true)]
        [Description("Происходит при изменении видимости выбранного изображения")]
        public event VisibleImageChangedHandler VisibleImageChanged;
        public delegate void VisibleImageChangedHandler(int rowIndex, bool visible);

        /// <summary>Доступность настройки прозрачности</summary>
        [Description("Доступность настройки прозрачности")]
        public virtual bool Alpha
        {
            get { return uCtrl_Icon_Opt.Alpha; }
            set { uCtrl_Icon_Opt.Alpha = value; }
        }

        /// <summary>Добавляет ссылки на картинки в выпадающий список</summary>
        public void ComboBoxAddItems(List<string> ListImages, List<string> _ListImagesFullName)
        {
            uCtrl_Icon_Opt.ComboBoxAddItems(ListImages, _ListImagesFullName);
        }

        /// <summary>Устанавливает выбранную картинку</summary>
        public void SetImage(string value)
        {
            uCtrl_Icon_Opt.SetIcon(value);
        }

        /// <summary>Возвращает название выбранной картинки</summary>
        public string GetImage()
        {
            return uCtrl_Icon_Opt.GetIcon();
        }

        public NumericUpDown numericUpDown_X { get { return uCtrl_Icon_Opt.numericUpDown_iconX; } }
        public NumericUpDown numericUpDown_Y { get { return uCtrl_Icon_Opt.numericUpDown_iconY; } }
        public NumericUpDown numericUpDown_Alpha { get { return uCtrl_Icon_Opt.numericUpDown_Alpha; } }

        /// <summary>Индекс выбранной строки или -1</summary>
        public int GetSelectedRowIndex()
        {
            try
            {
                if (dataGridView_images.SelectedCells.Count > 0) return dataGridView_images.SelectedCells[0].RowIndex;
            }
            catch (Exception)
            {
            }
            return -1;
        }

        /// <summary>Обновляет список изображений и выбирает строку rowIndex</summary>
        public void UpdateImageList(List<string> imagesList, List<bool> imagesVisibleList, int rowIndex = 0)
        {
            setValue = true;

            dataGridView_images.Rows.Clear();
            for (int index = 0; index < imagesList.Count; index++)
            {
                DataGridViewRow RowNew = new DataGridViewRow();
                RowNew.Cells.Add(new DataGridViewTextBoxCell() { Value = (index + 1).ToString() });
                RowNew.Cells.Add(new DataGridViewTextBoxCell() { Value = imagesList[index] });
                RowNew.Cells.Add(new DataGridViewCheckBoxCell() { Value = imagesVisibleList[index] });
                RowNew.Cells[0].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                RowNew.Cells[1].Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
                RowNew.Cells[2].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridView_images.Rows.Add(RowNew);
            }

            button_del.Enabled = imagesList.Count > 0;
            button_add.Enabled = imagesList.Count < MaxImagesCount;
            uCtrl_Icon_Opt.Enabled = imagesList.Count > 0;

            if (rowIndex >= 0 && rowIndex < dataGridView_images.Rows.Count)
            {
                dataGridView_images.Rows[rowIndex].Selected = true;
                dataGridView_images.CurrentCell = dataGridView_images.Rows[rowIndex].Cells[0];
                if (SelectImageWidget != null) SelectImageWidget(rowIndex);
            }
            setValue = false;
        }

        /// <summary>Сбрасывает данные на значения по умолчанию</summary>
        public void SettingsClear()
        {
            setValue = true;

            uCtrl_Icon_Opt.SettingsClear();
            dataGridView_images.Rows.Clear();
            button_add.Enabled = true;
            button_del.Enabled = false;
            uCtrl_Icon_Opt.Enabled = false;

            setValue = false;
        }

        private void uCtrl_Icon_Opt_ValueChanged(object sender, EventArgs eventArgs)
        {
            int rowIndex = GetSelectedRowIndex();
            if (rowIndex < 0 || setValue) return;

            // имя картинки в списке
            string imageName = GetImage();
            if (rowIndex < dataGridView_images.Rows.Count)
                dataGridView_images.Rows[rowIndex].Cells[1].Value = imageName;

            if (ValueChanged != null) ValueChanged(this, eventArgs, rowIndex);
        }

        private void button_add_Click(object sender, EventArgs e)
        {
            if (AddImage != null && !setValue) AddImage(GetSelectedRowIndex());
        }

        private void button_del_Click(object sender, EventArgs e)
        {
            int rowIndex = GetSelectedRowIndex();
            if (rowIndex >= 0 && DelImage != null && !setValue) DelImage(rowIndex);
        }

        private void dataGridView_images_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (SelectImageWidget != null && !setValue) SelectImageWidget(e.RowIndex);
        }

        private void dataGridView_images_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back)
            {
                int rowIndex = GetSelectedRowIndex();
                if (rowIndex >= 0 && DelImage != null && !setValue) DelImage(rowIndex);
            }
        }

        private void dataGridView_images_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView senderGrid = (DataGridView)sender;
            senderGrid.EndEdit();
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewCheckBoxColumn)
            {
                DataGridViewCheckBoxCell cbxCell = (DataGridViewCheckBoxCell)senderGrid.Rows[e.RowIndex].Cells[e.ColumnIndex];
                bool value = !(cbxCell.Value is bool && (bool)cbxCell.Value);
                cbxCell.Value = value;

                if (VisibleImageChanged != null && !setValue) VisibleImageChanged(e.RowIndex, value);
            }
        }
    }
}
