using ImageMagick;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Watch_Face_Editor;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Watch_Face_Editor
{
    public partial class CreateZAB_dialog : Form
    {
        private string name;
        private long id;
        private int version;
        private bool save = false;

        public bool Result
        {
            get
            {
                return save;
            }
        }

        public string WatchFaceName
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
                textBox_name.Text = name;
            }
        }

        public long WatchFaceId
        {
            get
            {
                return id;
            }
            set
            {
                if (value > 999 && value < 10000000)
                {
                    id = value;
                    textBox_id.Text = id.ToString();
                }
            }
        }

        public int Version
        {
            get
            {
                return version;
            }
            set
            {
                version = value;
                numeric_version.Value = version;
            }
        }

        public CreateZAB_dialog(string name, long id, int version)
        {
            InitializeComponent();
            WatchFaceName = name;
            WatchFaceId = id;
            Version = version;
        }

        private void button_save_Click(object sender, EventArgs e)
        {
            save = true;
            this.Close();
        }

        private void button_cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBox_name_TextChanged(object sender, EventArgs e)
        {
            name = textBox_name.Text;
        }

        private void textBox_id_TextChanged(object sender, EventArgs e)
        {
            int pos = textBox_id.SelectionStart;

            // Только цифры
            string digitsOnly = new string(textBox_id.Text.Where(char.IsDigit).ToArray());

            // Ограничение по значению
            if (long.TryParse(digitsOnly, out long value) && value >= 10000000)
            {
                digitsOnly = "9999999";
            }

            textBox_id.Text = digitsOnly;
            textBox_id.SelectionStart = Math.Min(pos, textBox_id.Text.Length);

            id = long.TryParse(digitsOnly, out value) ? value : 0;
        }

        private void numeric_version_ValueChanged(object sender, EventArgs e)
        {
            version = (int)numeric_version.Value;
        }

        private void button_savePNG_Click(object sender, EventArgs e)
        {
            Logger.WriteLine("* ZAP SavePNG");
            Form1 form1 = this.Owner as Form1;//Получаем ссылку на первую форму
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = form1.ProjectDir;
            saveFileDialog.Filter = Properties.FormStrings.FilterPng;
            saveFileDialog.FileName = "ZAP Preview.png";
            //openFileDialog.Filter = "PNG Files: (*.png)|*.png";
            saveFileDialog.RestoreDirectory = true;
            saveFileDialog.Title = Properties.FormStrings.Dialog_Title_SavePNG;
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                Bitmap bitmap = new Bitmap(form1.SelectedModel.background.w, form1.SelectedModel.background.h, PixelFormat.Format32bppArgb);
                Bitmap mask = new Bitmap(Application.StartupPath + @"\Mask\" + form1.SelectedModel.maskImage);

                Graphics gPanel = Graphics.FromImage(bitmap);
                int link = form1.radioButton_ScreenNormal.Checked ? 0 : 1;
                form1.Preview_screen(gPanel, 1.0f, false, false, false, false, false, false, false, false, false, false, false, true, false,
                    false, false, link, false, false, -1, false, 0);
                bitmap = form1.ApplyMask(bitmap, mask);
                //float scale = 360f / bitmap.Height;
                //bitmap = Form1.ResizeImage(bitmap, scale);
                bitmap = ResizeImage(bitmap);

                bitmap.Save(saveFileDialog.FileName, ImageFormat.Png);
            }
            Logger.WriteLine("* ZAP SavePNG(end)");
        }

        private void button_saveGIF_Click(object sender, EventArgs e)
        {
            Logger.WriteLine("* ZAP SaveGIF");
            Form1 form1 = this.Owner as Form1;//Получаем ссылку на первую форму
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = form1.ProjectDir;
            saveFileDialog.Filter = Properties.FormStrings.FilterGif;
            saveFileDialog.FileName = "ZAP Preview.gif";
            //openFileDialog.Filter = "GIF Files: (*.gif)|*.gif";
            saveFileDialog.RestoreDirectory = true;
            saveFileDialog.Title = Properties.FormStrings.Dialog_Title_SaveGIF;
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                form1.progressBar1.Maximum = 13;
                form1.progressBar1.Value = 0;
                form1.progressBar1.Visible = true;

                Bitmap bitmap = new Bitmap(form1.SelectedModel.background.w, form1.SelectedModel.background.h, PixelFormat.Format32bppArgb);
                Bitmap mask = new Bitmap(Application.StartupPath + @"\Mask\" + form1.SelectedModel.maskImage);

                Bitmap bitmapTemp = new Bitmap(bitmap.Width, bitmap.Height, PixelFormat.Format32bppArgb);
                Graphics gPanel = Graphics.FromImage(bitmap);
                bool save = false;
                Random rnd = new Random();
                form1.PreviewView = false;
                int SetNumber = form1.WatchFacePreviewSet.SetNumber;

                using (MagickImageCollection collection = new MagickImageCollection())
                {
                    // основной экран
                    //WidgetDrawIndex = 0;
                    for (int i = 0; i < 13; i++)
                    {
                        save = false;
                        switch (i)
                        {
                            case 0:
                                //button_Set1.PerformClick();
                                form1.SetPreferences(form1.userCtrl_Set1);
                                save = true;
                                break;
                            case 1:
                                if (form1.userCtrl_Set2.numericUpDown_Calories_Set.Value != 1234)
                                {
                                    form1.SetPreferences(form1.userCtrl_Set2);
                                    save = true;
                                }
                                break;
                            case 2:
                                if (form1.userCtrl_Set3.numericUpDown_Calories_Set.Value != 1234)
                                {
                                    form1.SetPreferences(form1.userCtrl_Set3);
                                    save = true;
                                }
                                break;
                            case 3:
                                if (form1.userCtrl_Set4.numericUpDown_Calories_Set.Value != 1234)
                                {
                                    form1.SetPreferences(form1.userCtrl_Set4);
                                    save = true;
                                }
                                break;
                            case 4:
                                if (form1.userCtrl_Set5.numericUpDown_Calories_Set.Value != 1234)
                                {
                                    form1.SetPreferences(form1.userCtrl_Set5);
                                    save = true;
                                }
                                break;
                            case 5:
                                if (form1.userCtrl_Set6.numericUpDown_Calories_Set.Value != 1234)
                                {
                                    form1.SetPreferences(form1.userCtrl_Set6);
                                    save = true;
                                }
                                break;
                            case 6:
                                if (form1.userCtrl_Set7.numericUpDown_Calories_Set.Value != 1234)
                                {
                                    form1.SetPreferences(form1.userCtrl_Set7);
                                    save = true;
                                }
                                break;
                            case 7:
                                if (form1.userCtrl_Set8.numericUpDown_Calories_Set.Value != 1234)
                                {
                                    form1.SetPreferences(form1.userCtrl_Set8);
                                    save = true;
                                }
                                break;
                            case 8:
                                if (form1.userCtrl_Set9.numericUpDown_Calories_Set.Value != 1234)
                                {
                                    form1.SetPreferences(form1.userCtrl_Set9);
                                    save = true;
                                }
                                break;
                            case 9:
                                if (form1.userCtrl_Set10.numericUpDown_Calories_Set.Value != 1234)
                                {
                                    form1.SetPreferences(form1.userCtrl_Set10);
                                    save = true;
                                }
                                break;
                            case 10:
                                if (form1.userCtrl_Set11.numericUpDown_Calories_Set.Value != 1234)
                                {
                                    form1.SetPreferences(form1.userCtrl_Set11);
                                    save = true;
                                }
                                break;
                            case 11:
                                if (form1.userCtrl_Set12.numericUpDown_Calories_Set.Value != 1234)
                                {
                                    form1.SetPreferences(form1.userCtrl_Set12);
                                    save = true;
                                }
                                break;
                        }

                        if (save)
                        {
                            bitmap = bitmapTemp;
                            gPanel = Graphics.FromImage(bitmap);
                            Logger.WriteLine("SaveGIF SetPreferences(" + i.ToString() + ")");

                            //int link = radioButton_ScreenNormal.Checked ? 0 : 1;
                            int link = 0;
                            form1.Preview_screen(gPanel, 1.0f, false, false, false, false, false, false, false, false, false, false, false, true,
                                false, false, false, link, false, false, -1, false, 0);
                            //if (checkBox_crop.Checked) {
                            //    bitmap = ApplyMask(bitmap, mask);
                            //    gPanel = Graphics.FromImage(bitmap);
                            //}
                            bitmap = form1.ApplyMask(bitmap, mask);
                            bitmap = ResizeImage(bitmap);
                            // Add first image and set the animation delay to 100ms
                            MagickImage item = new MagickImage(ImgConvert.CopyImageToByteArray(bitmap));
                            //ExifProfile profile = item.GetExifProfile();
                            collection.Add(item);
                            //collection[collection.Count - 1].AnimationDelay = 100;
                            collection[collection.Count - 1].AnimationDelay = (int)(100 * form1.numericUpDown_Gif_Speed.Value);
                            //WidgetDrawIndex++;
                        }
                    }

                    Logger.WriteLine("SaveGIF_Shortcuts");
                    bool Shortcuts_In_Gif = form1.checkBox_Shortcuts_In_Gif.Checked;
                    bool Buttons_In_Gif = form1.checkBox_Buttons_In_Gif.Checked;
                    // Shortcuts
                    if (Shortcuts_In_Gif && form1.Watch_Face.Shortcuts != null && form1.Watch_Face.Shortcuts.visible)
                    {
                        bitmap = bitmapTemp;
                        gPanel = Graphics.FromImage(bitmap);
                        //int link = radioButton_ScreenNormal.Checked ? 0 : 1;
                        int link_AOD = 0;
                        form1.Preview_screen(gPanel, 1.0f, false, false, false, false, false, false, false, false, false, false, false, true,
                            false, false, false, link_AOD, Shortcuts_In_Gif, Buttons_In_Gif, -1, false, 0);

                        bitmap = form1.ApplyMask(bitmap, mask);
                        bitmap = ResizeImage(bitmap);
                        // Add first image and set the animation delay to 100ms
                        MagickImage item_AOD = new MagickImage(ImgConvert.CopyImageToByteArray(bitmap));
                        //ExifProfile profile = item.GetExifProfile();
                        collection.Add(item_AOD);
                        //collection[collection.Count - 1].AnimationDelay = 100;
                        collection[collection.Count - 1].AnimationDelay = (int)(100 * form1.numericUpDown_Gif_Speed.Value);
                    }

                    // Buttons
                    Logger.WriteLine("SaveGIF Buttons");
                    if (Buttons_In_Gif && form1.Watch_Face.Buttons != null && form1.Watch_Face.Buttons.enable &&
                        form1.Watch_Face.Buttons.Button != null && form1.Watch_Face.Buttons.Button.Count > 0)
                    {
                        bitmap = bitmapTemp;
                        gPanel = Graphics.FromImage(bitmap);
                        //int link = radioButton_ScreenNormal.Checked ? 0 : 1;
                        int link_AOD = 0;
                        form1.Preview_screen(gPanel, 1.0f, false, false, false, false, false, false, false, false, false, false, false, true,
                            false, false, false, link_AOD, Shortcuts_In_Gif, Buttons_In_Gif, -1, false, 0);

                        bitmap = form1.ApplyMask(bitmap, mask);
                        bitmap = ResizeImage(bitmap);
                        // Add first image and set the animation delay to 100ms
                        MagickImage item_AOD = new MagickImage(ImgConvert.CopyImageToByteArray(bitmap));
                        //ExifProfile profile = item.GetExifProfile();
                        collection.Add(item_AOD);
                        //collection[collection.Count - 1].AnimationDelay = 100;
                        collection[collection.Count - 1].AnimationDelay = (int)(100 * form1.numericUpDown_Gif_Speed.Value);
                    }


                    Logger.WriteLine("SaveGIF_AOD");
                    // AOD
                    if (form1.Watch_Face.ScreenAOD != null &&
                        (form1.Watch_Face.ScreenAOD.Background != null || (form1.Watch_Face.ScreenAOD.Elements != null && form1.Watch_Face.ScreenAOD.Elements.Count > 0)))
                    {

                        bitmap = bitmapTemp;
                        gPanel = Graphics.FromImage(bitmap);
                        //int link = radioButton_ScreenNormal.Checked ? 0 : 1;
                        int link_AOD = 1;
                        form1.Preview_screen(gPanel, 1.0f, false, false, false, false, false, false, false, false, false, false, false, true,
                            false, false, false, link_AOD, false, false, -1, false, 0);
                        
                        bitmap = form1.ApplyMask(bitmap, mask);
                        bitmap = ResizeImage(bitmap);
                        // Add first image and set the animation delay to 100ms
                        MagickImage item_AOD = new MagickImage(ImgConvert.CopyImageToByteArray(bitmap));
                        //ExifProfile profile = item.GetExifProfile();
                        collection.Add(item_AOD);
                        //collection[collection.Count - 1].AnimationDelay = 100;
                        collection[collection.Count - 1].AnimationDelay = (int)(100 * form1.numericUpDown_Gif_Speed.Value);


                        form1.SetPreferences(form1.userCtrl_Set1);
                        bitmap = bitmapTemp;
                        gPanel = Graphics.FromImage(bitmap);
                        form1.Preview_screen(gPanel, 1.0f, false, false, false, false, false, false, false, false, false, false, false, true,
                            false, false, false, link_AOD, false, false, -1, false, 0);
                        
                        bitmap = form1.ApplyMask(bitmap, mask);
                        bitmap = ResizeImage(bitmap);
                        item_AOD = new MagickImage(ImgConvert.CopyImageToByteArray(bitmap));
                        //ExifProfile profile = item.GetExifProfile();
                        collection.Add(item_AOD);
                        //collection[collection.Count - 1].AnimationDelay = 100;
                        collection[collection.Count - 1].AnimationDelay = (int)(100 * form1.numericUpDown_Gif_Speed.Value);
                    }

                    // Переключаемый фон
                    Background background = null;
                    if (form1.Watch_Face != null && form1.Watch_Face.ScreenNormal != null && form1.Watch_Face.ScreenNormal.Background != null)
                        background = form1.Watch_Face.ScreenNormal.Background;
                    if (background != null)
                    {
                        // фон
                        if (background.BackgroundImage != null && background.BackgroundImage.src != null &&
                            background.BackgroundImage.src.Length > 0 && background.visible && form1.Watch_Face.SwitchBackground != null &&
                            form1.Watch_Face.SwitchBackground.bg_list != null && form1.Watch_Face.SwitchBackground.bg_list.Count > 0 &&
                            form1.Watch_Face.SwitchBackground.enable)
                        {
                            ElementSwitchBackground switchBG = form1.Watch_Face.SwitchBackground;
                            int switchBGIndex = switchBG.select_index;
                            for (int i = 0; i < switchBG.bg_list.Count; i++)
                            {
                                bitmap = bitmapTemp;
                                gPanel = Graphics.FromImage(bitmap);
                                switchBG.select_index = i;
                                form1.Preview_screen(gPanel, 1.0f, false, false, false, false, false, false, false, false, false, false, false, true,
                                    false, false, false, 0, false, false, -1, false, 0);
                                bitmap = form1.ApplyMask(bitmap, mask);
                                bitmap = ResizeImage(bitmap);
                                MagickImage item_bg_edit = new MagickImage(ImgConvert.CopyImageToByteArray(bitmap));
                                collection.Add(item_bg_edit);
                                collection[collection.Count - 1].AnimationDelay = (int)(100 * form1.numericUpDown_Gif_Speed.Value);
                            }
                            switchBG.select_index = switchBGIndex;
                        }

                        // цвет
                        if (background.BackgroundColor != null && background.visible && form1.Watch_Face.SwitchBG_Color != null &&
                        form1.Watch_Face.SwitchBG_Color.color_list != null && form1.Watch_Face.SwitchBG_Color.color_list.Count > 0 &&
                        form1.Watch_Face.SwitchBG_Color.enable)
                        {
                            ElementSwitchBG_Color switchBG_Color = form1.Watch_Face.SwitchBG_Color;
                            int switchBGIndex = switchBG_Color.select_index;
                            for (int i = 0; i < switchBG_Color.color_list.Count; i++)
                            {
                                bitmap = bitmapTemp;
                                gPanel = Graphics.FromImage(bitmap);
                                switchBG_Color.select_index = i;
                                form1.Preview_screen(gPanel, 1.0f, false, false, false, false, false, false, false, false, false, false, false, true,
                                    false, false, false, 0, false, false, -1, false, 0);
                                bitmap = form1.ApplyMask(bitmap, mask);
                                bitmap = ResizeImage(bitmap);
                                MagickImage item_bg_edit = new MagickImage(ImgConvert.CopyImageToByteArray(bitmap));
                                collection.Add(item_bg_edit);
                                collection[collection.Count - 1].AnimationDelay = (int)(100 * form1.numericUpDown_Gif_Speed.Value);
                            }
                            switchBG_Color.select_index = switchBGIndex;
                        }

                    }


                    Logger.WriteLine("SaveGIF_Editable_Background");
                    // Editable_Background
                    if (form1.Watch_Face.ScreenNormal != null && form1.Watch_Face.ScreenNormal.Background != null &&
                        (form1.Watch_Face.ScreenNormal.Background.Editable_Background != null &&
                        form1.Watch_Face.ScreenNormal.Background.Editable_Background.enable_edit_bg &&
                        form1.Watch_Face.ScreenNormal.Background.Editable_Background.BackgroundList.Count > 0))
                    {
                        int bg_index = form1.Watch_Face.ScreenNormal.Background.Editable_Background.selected_background;
                        //int link = radioButton_ScreenNormal.Checked ? 0 : 1;
                        int link_AOD = 0;
                        for (int index = 0; index < form1.Watch_Face.ScreenNormal.Background.Editable_Background.BackgroundList.Count; index++)
                        {
                            bitmap = bitmapTemp;
                            gPanel = Graphics.FromImage(bitmap);
                            form1.Watch_Face.ScreenNormal.Background.Editable_Background.selected_background = index;
                            form1.Preview_screen(gPanel, 1.0f, false, false, false, false, false, false, false, false, false, false, false, true,
                                            false, false, false, link_AOD, false, false, -1, true, 1);
                            bitmap = form1.ApplyMask(bitmap, mask);
                            bitmap = ResizeImage(bitmap);
                            // Add first image and set the animation delay to 100ms
                            MagickImage item_bg_edit = new MagickImage(ImgConvert.CopyImageToByteArray(bitmap));
                            //ExifProfile profile = item.GetExifProfile();
                            collection.Add(item_bg_edit);
                            //collection[collection.Count - 1].AnimationDelay = 100;
                            collection[collection.Count - 1].AnimationDelay = (int)(100 * form1.numericUpDown_Gif_Speed.Value);
                        }
                        form1.Watch_Face.ScreenNormal.Background.Editable_Background.selected_background = bg_index;
                    }


                    Logger.WriteLine("SaveGIF_Editable_Pointers");
                    // Editable_Pointers
                    if (form1.Watch_Face.ElementEditablePointers != null &&
                        form1.Watch_Face.ElementEditablePointers.visible &&
                        form1.Watch_Face.ElementEditablePointers.config != null &&
                        form1.Watch_Face.ElementEditablePointers.config.Count > 0)
                    {
                        int p_index = form1.Watch_Face.ElementEditablePointers.selected_pointers;
                        //int link = radioButton_ScreenNormal.Checked ? 0 : 1;
                        int link_AOD = 0;
                        for (int index = 0; index < form1.Watch_Face.ElementEditablePointers.config.Count; index++)
                        {
                            bitmap = bitmapTemp;
                            gPanel = Graphics.FromImage(bitmap);
                            form1.Watch_Face.ElementEditablePointers.selected_pointers = index;
                            form1.Preview_screen(gPanel, 1.0f, false, false, false, false, false, false, false, false, false, false, false, true,
                                            false, false, false, link_AOD, false, false, -1, true, 3);
                            bitmap = form1.ApplyMask(bitmap, mask);
                            bitmap = ResizeImage(bitmap);
                            // Add first image and set the animation delay to 100ms
                            MagickImage item_bg_edit = new MagickImage(ImgConvert.CopyImageToByteArray(bitmap));
                            //ExifProfile profile = item.GetExifProfile();
                            collection.Add(item_bg_edit);
                            //collection[collection.Count - 1].AnimationDelay = 100;
                            collection[collection.Count - 1].AnimationDelay = (int)(100 * form1.numericUpDown_Gif_Speed.Value);
                        }
                        form1.Watch_Face.ElementEditablePointers.selected_pointers = p_index;
                    }

                    Logger.WriteLine("SaveGIF_Editable_Elements edit mode");
                    // Editable_Elements edit mode
                    if (form1.Watch_Face.Editable_Elements != null &&
                        form1.Watch_Face.Editable_Elements.visible &&
                        form1.Watch_Face.Editable_Elements.Watchface_edit_group != null &&
                        form1.Watch_Face.Editable_Elements.Watchface_edit_group.Count > 0)
                    {
                        int zone_index = form1.Watch_Face.Editable_Elements.selected_zone;
                        int[] e_index = new int[form1.Watch_Face.Editable_Elements.Watchface_edit_group.Count];
                        for (int index = 0; index < form1.Watch_Face.Editable_Elements.Watchface_edit_group.Count; index++)
                        {
                            e_index[index] = form1.Watch_Face.Editable_Elements.Watchface_edit_group[index].selected_element;
                        }
                        //int link = radioButton_ScreenNormal.Checked ? 0 : 1;
                        int link_AOD = 0;
                        for (int index = 0; index < form1.Watch_Face.Editable_Elements.Watchface_edit_group.Count; index++) // поочередно выбираем все зоны
                        {
                            bitmap = bitmapTemp;
                            gPanel = Graphics.FromImage(bitmap);
                            form1.Watch_Face.Editable_Elements.selected_zone = index;
                            WATCHFACE_EDIT_GROUP e_g = form1.Watch_Face.Editable_Elements.Watchface_edit_group[index];
                            if (e_g.Elements != null && e_g.Elements.Count > 0)
                            {
                                // перебираем все зоны и меняем в них выбраные элементы
                                for (int group_index = 0; group_index < form1.Watch_Face.Editable_Elements.Watchface_edit_group.Count; group_index++)
                                {
                                    WATCHFACE_EDIT_GROUP edit_group = form1.Watch_Face.Editable_Elements.Watchface_edit_group[group_index];
                                    int element_index = index;
                                    while (element_index >= edit_group.Elements.Count)
                                    {
                                        element_index = element_index - edit_group.Elements.Count;
                                    }
                                    edit_group.selected_element = element_index;
                                }
                            }
                            form1.Preview_screen(gPanel, 1.0f, false, false, false, false, false, false, false, false, false, false, false, true,
                                            false, false, false, link_AOD, false, false, -1, true, 2);
                            bitmap = form1.ApplyMask(bitmap, mask);
                            bitmap = ResizeImage(bitmap);
                            // Add first image and set the animation delay to 100ms
                            MagickImage item_bg_edit = new MagickImage(ImgConvert.CopyImageToByteArray(bitmap));
                            //ExifProfile profile = item.GetExifProfile();
                            collection.Add(item_bg_edit);
                            //collection[collection.Count - 1].AnimationDelay = 100;
                            collection[collection.Count - 1].AnimationDelay = (int)(100 * form1.numericUpDown_Gif_Speed.Value);
                        }
                        for (int index = 0; index < form1.Watch_Face.Editable_Elements.Watchface_edit_group.Count; index++)
                        {
                            form1.Watch_Face.Editable_Elements.Watchface_edit_group[index].selected_element = e_index[index];
                        }
                        form1.Watch_Face.Editable_Elements.selected_zone = zone_index;
                    }


                    // Optionally reduce colors
                    QuantizeSettings settings = new QuantizeSettings();
                    //settings.Colors = 256;
                    //collection.Quantize(settings);

                    // Optionally optimize the images (images should have the same size).
                    collection.OptimizeTransparency();
                    //collection.Optimize();

                    // Save gif
                    collection.Write(saveFileDialog.FileName);
                }
                switch (SetNumber)
                {
                    case 1:
                        form1.SetPreferences(form1.userCtrl_Set1);
                        break;
                    case 2:
                        form1.SetPreferences(form1.userCtrl_Set2);
                        break;
                    case 3:
                        form1.SetPreferences(form1.userCtrl_Set3);
                        break;
                    case 4:
                        form1.SetPreferences(form1.userCtrl_Set4);
                        break;
                    case 5:
                        form1.SetPreferences(form1.userCtrl_Set5);
                        break;
                    case 6:
                        form1.SetPreferences(form1.userCtrl_Set6);
                        break;
                    case 7:
                        form1.SetPreferences(form1.userCtrl_Set7);
                        break;
                    case 8:
                        form1.SetPreferences(form1.userCtrl_Set8);
                        break;
                    case 9:
                        form1.SetPreferences(form1.userCtrl_Set9);
                        break;
                    case 10:
                        form1.SetPreferences(form1.userCtrl_Set10);
                        break;
                    case 11:
                        form1.SetPreferences(form1.userCtrl_Set11);
                        break;
                    case 12:
                        form1.SetPreferences(form1.userCtrl_Set12);
                        break;
                    default:
                        form1.SetPreferences(form1.userCtrl_Set12);
                        break;
                }
                form1.PreviewView = true;
                mask.Dispose();
                bitmapTemp.Dispose();
                bitmap.Dispose();
                form1.progressBar1.Visible = false;
            }
            Logger.WriteLine("* ZAP SaveGIF (end)");
        }

        public static Bitmap ResizeImage(Image image)
        {
            float scale = 360f / image.Height;
            int width = (int)Math.Round(image.Width * scale);
            int height = (int)Math.Round(image.Height * scale);
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            Bitmap returnBitmap = new Bitmap(360, 360, PixelFormat.Format32bppArgb);
            Graphics gSavePanel = Graphics.FromImage(returnBitmap);
            if (destImage.Height != destImage.Width)
            {
                int posX = (int)(360 / 2f - destImage.Width / 2f);
                int posY = (int)(360 / 2f - destImage.Height / 2f);
                gSavePanel.DrawImage(destImage, posX, posY, destImage.Width, destImage.Height);
            }
            else returnBitmap = destImage;

            return returnBitmap;
        }
    }
}
