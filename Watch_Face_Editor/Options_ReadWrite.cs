﻿using ControlLibrary;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrayNotify;

namespace Watch_Face_Editor
{
    public partial class Form1 : Form
    {
        private WATCH_FACE TextToJson(string text)
        {
            WATCH_FACE Watch_Face_return = null;
            WATCH_FACE Watch_Face_temp = null;
            try
            {
                Watch_Face_temp = JsonConvert.DeserializeObject<WATCH_FACE>(text, new JsonSerializerSettings
                {
                    //DefaultValueHandling = DefaultValueHandling.Ignore,
                    NullValueHandling = NullValueHandling.Ignore
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(Properties.FormStrings.Message_JsonError_Text + Environment.NewLine + ex,
                    Properties.FormStrings.Message_Error_Caption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            if (Watch_Face_temp == null) return Watch_Face_return;
            
            Watch_Face_return = new WATCH_FACE();
            if (Watch_Face_temp.WatchFace_Info != null) Watch_Face_return.WatchFace_Info = Watch_Face_temp.WatchFace_Info;
            if (Watch_Face_temp.ScreenNormal != null)
            {
                Watch_Face_return.ScreenNormal = new ScreenNormal();
                if (Watch_Face_temp.ScreenNormal.Background != null)
                    Watch_Face_return.ScreenNormal.Background = Watch_Face_temp.ScreenNormal.Background;
                if (Watch_Face_temp.ScreenNormal.Elements != null)
                {
                    Watch_Face_return.ScreenNormal.Elements = new List<object>();
                    List<object> NewElements = ObjectsToElements(Watch_Face_temp.ScreenNormal.Elements);
                    Watch_Face_return.ScreenNormal.Elements = NewElements;

                    ElementShortcuts shortcuts = (ElementShortcuts)NewElements.Find(e1 => e1.GetType().Name == "ElementShortcuts");
                    if (shortcuts != null) 
                    {
                        Watch_Face_return.Shortcuts = (ElementShortcuts)shortcuts.Clone();
                        int index = NewElements.FindIndex(e => e.GetType().Name == "ElementShortcuts");
                        NewElements.RemoveAt(index);
                    }
                }
            }

            if (Watch_Face_temp.ScreenAOD != null)
            {
                Watch_Face_return.ScreenAOD = new ScreenAOD();
                if (Watch_Face_temp.ScreenAOD.Background != null)
                    Watch_Face_return.ScreenAOD.Background = Watch_Face_temp.ScreenAOD.Background;
                if (Watch_Face_temp.ScreenAOD.Elements != null)
                {
                    Watch_Face_return.ScreenAOD.Elements = new List<object>();
                    List<object> NewElements = ObjectsToElements(Watch_Face_temp.ScreenAOD.Elements);
                    Watch_Face_return.ScreenAOD.Elements = NewElements;
                }
            }

            if (Watch_Face_temp.ElementEditablePointers != null) 
                Watch_Face_return.ElementEditablePointers = Watch_Face_temp.ElementEditablePointers;

            if (Watch_Face_temp.Editable_Elements != null)
                Watch_Face_return.Editable_Elements = Watch_Face_temp.Editable_Elements;
            if(Watch_Face_temp.Editable_Elements != null && Watch_Face_temp.Editable_Elements.Watchface_edit_group != null &&
                Watch_Face_temp.Editable_Elements.Watchface_edit_group.Count > 0)
            {
                for (int i = 0; i < Watch_Face_temp.Editable_Elements.Watchface_edit_group.Count; i++)
                {
                    List<object> NewElements = ObjectsToElements(Watch_Face_temp.Editable_Elements.Watchface_edit_group[i].Elements, true);
                    Watch_Face_return.Editable_Elements.Watchface_edit_group[i].Elements = NewElements;
                }
            }

            if (Watch_Face_temp.Shortcuts != null)
                Watch_Face_return.Shortcuts = Watch_Face_temp.Shortcuts;

            if (Watch_Face_temp.DisconnectAlert != null)
                Watch_Face_return.DisconnectAlert = Watch_Face_temp.DisconnectAlert;

            if (Watch_Face_temp.RepeatAlert != null)
                Watch_Face_return.RepeatAlert = Watch_Face_temp.RepeatAlert;

            if (Watch_Face_temp.TopImage != null)
                Watch_Face_return.TopImage = Watch_Face_temp.TopImage;

            if (Watch_Face_temp.Buttons != null)
                Watch_Face_return.Buttons = Watch_Face_temp.Buttons;

            if (Watch_Face_temp.SwitchBackground != null)
                Watch_Face_return.SwitchBackground = Watch_Face_temp.SwitchBackground;

            if (Watch_Face_temp.SwitchBG_Color != null)
                Watch_Face_return.SwitchBG_Color = Watch_Face_temp.SwitchBG_Color;

            return Watch_Face_return;
        }

        /// <summary>Распознаем конкретный тип объекта</summary>
        private List<object> ObjectsToElements(List<object> elements, bool editableElement = false)
        {
            List<object> NewElements = new List<object>();
            if(elements == null || elements.Count == 0) return NewElements;
            foreach (object element in elements)
            {
                string elementStr = element.ToString();
                string type = GetTypeFromSring(elementStr);
                switch (type)
                {
                    /*#region DigitalTime
                    case "DigitalTime":
                        ElementDigitalTime DigitalTime = null;
                        try
                        {
                            DigitalTime = JsonConvert.DeserializeObject<ElementDigitalTime>(elementStr, new JsonSerializerSettings
                            {
                                //DefaultValueHandling = DefaultValueHandling.Ignore,
                                NullValueHandling = NullValueHandling.Ignore
                            });
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(Properties.FormStrings.Message_JsonError_Text + Environment.NewLine + ex,
                                Properties.FormStrings.Message_Error_Caption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        if (DigitalTime != null) NewElements.Add(DigitalTime);
                        break;
                    #endregion*/

                    #region DigitalTime
                    case "DigitalTime":
                        ElementDigitalTime DigitalTime = null;
                        try
                        {
                            DigitalTime = JsonConvert.DeserializeObject<ElementDigitalTime>(elementStr, new JsonSerializerSettings
                            {
                                //DefaultValueHandling = DefaultValueHandling.Ignore,
                                NullValueHandling = NullValueHandling.Ignore
                            });
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(Properties.FormStrings.Message_JsonError_Text + Environment.NewLine + ex,
                                Properties.FormStrings.Message_Error_Caption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        if (DigitalTime != null) {
                            ElementDigitalTime_v2 DigitalTime_v2_temp = new ElementDigitalTime_v2();

                            if (DigitalTime.Second != null)
                            {
                                if (DigitalTime_v2_temp.Group_Second == null) DigitalTime_v2_temp.Group_Second = new DigitalTimeGroup();
                                DigitalTime_v2_temp.Group_Second.Number = DigitalTime.Second;
                                DigitalTime_v2_temp.Group_Second.position = DigitalTime.Second.position;
                            }
                            if (DigitalTime.Second_Font != null)
                            {
                                if (DigitalTime_v2_temp.Group_Second == null) DigitalTime_v2_temp.Group_Second = new DigitalTimeGroup();
                                DigitalTime_v2_temp.Group_Second.Number_Font = DigitalTime.Second_Font;
                                DigitalTime_v2_temp.Group_Second.position = DigitalTime.Second_Font.position;
                            }
                            if (DigitalTime.Second_rotation != null)
                            {
                                if (DigitalTime_v2_temp.Group_Second == null) DigitalTime_v2_temp.Group_Second = new DigitalTimeGroup();
                                DigitalTime_v2_temp.Group_Second.Text_rotation = DigitalTime.Second_rotation;
                                DigitalTime_v2_temp.Group_Second.position = DigitalTime.Second_rotation.position;
                            }
                            if (DigitalTime.Second_circle != null)
                            {
                                if (DigitalTime_v2_temp.Group_Second == null) DigitalTime_v2_temp.Group_Second = new DigitalTimeGroup();
                                DigitalTime_v2_temp.Group_Second.Text_circle = DigitalTime.Second_circle;
                                DigitalTime_v2_temp.Group_Second.position = DigitalTime.Second_circle.position;
                            }

                            if (DigitalTime.Minute != null)
                            {
                                if (DigitalTime_v2_temp.Group_Minute == null) DigitalTime_v2_temp.Group_Minute = new DigitalTimeGroup();
                                DigitalTime_v2_temp.Group_Minute.Number = DigitalTime.Minute;
                                DigitalTime_v2_temp.Group_Minute.position = DigitalTime.Minute.position;
                            }
                            if (DigitalTime.Minute_Font != null)
                            {
                                if (DigitalTime_v2_temp.Group_Minute == null) DigitalTime_v2_temp.Group_Minute = new DigitalTimeGroup();
                                DigitalTime_v2_temp.Group_Minute.Number_Font = DigitalTime.Minute_Font;
                                DigitalTime_v2_temp.Group_Minute.position = DigitalTime.Minute_Font.position;
                            }
                            if (DigitalTime.Minute_rotation != null)
                            {
                                if (DigitalTime_v2_temp.Group_Minute == null) DigitalTime_v2_temp.Group_Minute = new DigitalTimeGroup();
                                DigitalTime_v2_temp.Group_Minute.Text_rotation = DigitalTime.Minute_rotation;
                                DigitalTime_v2_temp.Group_Minute.position = DigitalTime.Minute_rotation.position;
                            }
                            if (DigitalTime.Minute_circle != null)
                            {
                                if (DigitalTime_v2_temp.Group_Minute == null) DigitalTime_v2_temp.Group_Minute = new DigitalTimeGroup();
                                DigitalTime_v2_temp.Group_Minute.Text_circle = DigitalTime.Minute_circle;
                                DigitalTime_v2_temp.Group_Minute.position = DigitalTime.Minute_circle.position;
                            }

                            if (DigitalTime.Hour != null)
                            {
                                if (DigitalTime_v2_temp.Group_Hour == null) DigitalTime_v2_temp.Group_Hour = new DigitalTimeGroup();
                                DigitalTime_v2_temp.Group_Hour.Number = DigitalTime.Hour;
                                DigitalTime_v2_temp.Group_Hour.position = DigitalTime.Hour.position;
                            }
                            if (DigitalTime.Hour_Font != null)
                            {
                                if (DigitalTime_v2_temp.Group_Hour == null) DigitalTime_v2_temp.Group_Hour = new DigitalTimeGroup();
                                DigitalTime_v2_temp.Group_Hour.Number_Font = DigitalTime.Hour_Font;
                                DigitalTime_v2_temp.Group_Hour.position = DigitalTime.Hour_Font.position;
                            }
                            if (DigitalTime.Hour_rotation != null)
                            {
                                if (DigitalTime_v2_temp.Group_Hour == null) DigitalTime_v2_temp.Group_Hour = new DigitalTimeGroup();
                                DigitalTime_v2_temp.Group_Hour.Text_rotation = DigitalTime.Hour_rotation;
                                DigitalTime_v2_temp.Group_Hour.position = DigitalTime.Hour_rotation.position;
                            }
                            if (DigitalTime.Hour_circle != null)
                            {
                                if (DigitalTime_v2_temp.Group_Hour == null) DigitalTime_v2_temp.Group_Hour = new DigitalTimeGroup();
                                DigitalTime_v2_temp.Group_Hour.Text_circle = DigitalTime.Hour_circle;
                                DigitalTime_v2_temp.Group_Hour.position = DigitalTime.Hour_circle.position;
                            }

                            if (DigitalTime.Hour_min_Font != null) DigitalTime_v2_temp.Hour_Min_Font = DigitalTime.Hour_min_Font;
                            if (DigitalTime.Hour_min_sec_Font != null) DigitalTime_v2_temp.Hour_Min_Sec_Font = DigitalTime.Hour_min_sec_Font;

                            if (DigitalTime.AmPm != null) DigitalTime_v2_temp.AmPm = DigitalTime.AmPm;

                            int index = 1;
                            for (int i = 0; i < 25; i++)
                            {
                                if (DigitalTime_v2_temp.Group_Second != null && DigitalTime_v2_temp.Group_Second.position == i) DigitalTime_v2_temp.Group_Second.position = index++;
                                if (DigitalTime_v2_temp.Group_Minute != null && DigitalTime_v2_temp.Group_Minute.position == i) DigitalTime_v2_temp.Group_Minute.position = index++;
                                if (DigitalTime_v2_temp.Group_Hour != null && DigitalTime_v2_temp.Group_Hour.position == i) DigitalTime_v2_temp.Group_Hour.position = index++;

                                if (DigitalTime_v2_temp.Hour_Min_Font != null && DigitalTime_v2_temp.Hour_Min_Font.position == i) DigitalTime_v2_temp.Hour_Min_Font.position = index++;
                                if (DigitalTime_v2_temp.Hour_Min_Sec_Font != null && DigitalTime_v2_temp.Hour_Min_Sec_Font.position == i) DigitalTime_v2_temp.Hour_Min_Sec_Font.position = index++;
                                if (DigitalTime_v2_temp.AmPm != null && DigitalTime_v2_temp.AmPm.position == i) DigitalTime_v2_temp.AmPm.position = index++;
                            }

                            NewElements.Add(DigitalTime_v2_temp);
                        }
                        break;
                    #endregion

                    #region DigitalTime_v2
                    case "DigitalTime_v2":
                        ElementDigitalTime_v2 DigitalTime_v2 = null;
                        try
                        {
                            DigitalTime_v2 = JsonConvert.DeserializeObject<ElementDigitalTime_v2>(elementStr, new JsonSerializerSettings
                            {
                                //DefaultValueHandling = DefaultValueHandling.Ignore,
                                NullValueHandling = NullValueHandling.Ignore
                            });

                            //if (DigitalTime_v2.Group_Hour == null) DigitalTime_v2.Group_Hour = new DigitalTimeGroup();
                            //if (DigitalTime_v2.Group_Minute == null) DigitalTime_v2.Group_Minute = new DigitalTimeGroup();
                            //if (DigitalTime_v2.Group_Second == null) DigitalTime_v2.Group_Second = new DigitalTimeGroup();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(Properties.FormStrings.Message_JsonError_Text + Environment.NewLine + ex,
                                Properties.FormStrings.Message_Error_Caption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        if (DigitalTime_v2 != null) NewElements.Add(DigitalTime_v2);
                        break;
                    #endregion

                    #region AnalogTime
                    case "AnalogTime":
                        ElementAnalogTime AnalogTime = null;
                        try
                        {
                            AnalogTime = JsonConvert.DeserializeObject<ElementAnalogTime>(elementStr, new JsonSerializerSettings
                            {
                                //DefaultValueHandling = DefaultValueHandling.Ignore,
                                NullValueHandling = NullValueHandling.Ignore
                            });
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(Properties.FormStrings.Message_JsonError_Text + Environment.NewLine + ex,
                                Properties.FormStrings.Message_Error_Caption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        if (AnalogTime != null) NewElements.Add(AnalogTime);
                        break;
                    #endregion

                    #region AnalogTimePro
                    case "AnalogTimePro":
                        ElementAnalogTimePro AnalogTimePro = null;
                        try
                        {
                            AnalogTimePro = JsonConvert.DeserializeObject<ElementAnalogTimePro>(elementStr, new JsonSerializerSettings
                            {
                                //DefaultValueHandling = DefaultValueHandling.Ignore,
                                NullValueHandling = NullValueHandling.Ignore
                            });
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(Properties.FormStrings.Message_JsonError_Text + Environment.NewLine + ex,
                                Properties.FormStrings.Message_Error_Caption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        if (AnalogTimePro != null) NewElements.Add(AnalogTimePro);
                        break;
                    #endregion

                    /*#region ElementEditablePointers
                    case "ElementEditablePointers":
                        ElementEditablePointers EditablePointers = null;
                        try
                        {
                            EditablePointers = JsonConvert.DeserializeObject<ElementEditablePointers>(elementStr, new JsonSerializerSettings
                            {
                                //DefaultValueHandling = DefaultValueHandling.Ignore,
                                NullValueHandling = NullValueHandling.Ignore
                            });
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(Properties.FormStrings.Message_JsonError_Text + Environment.NewLine + ex,
                                Properties.FormStrings.Message_Error_Caption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        if (EditablePointers != null) NewElements.Add(EditablePointers);
                        break;
                    #endregion*/

                    #region DateDay
                    case "DateDay":
                        ElementDateDay DateDay = null;
                        try
                        {
                            DateDay = JsonConvert.DeserializeObject<ElementDateDay>(elementStr, new JsonSerializerSettings
                            {
                                //DefaultValueHandling = DefaultValueHandling.Ignore,
                                NullValueHandling = NullValueHandling.Ignore
                            });
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(Properties.FormStrings.Message_JsonError_Text + Environment.NewLine + ex,
                                Properties.FormStrings.Message_Error_Caption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        if (DateDay != null) NewElements.Add(DateDay);
                        break;
                    #endregion

                    #region DateMonth
                    case "DateMonth":
                        ElementDateMonth DateMonth = null;
                        try
                        {
                            DateMonth = JsonConvert.DeserializeObject<ElementDateMonth>(elementStr, new JsonSerializerSettings
                            {
                                //DefaultValueHandling = DefaultValueHandling.Ignore,
                                NullValueHandling = NullValueHandling.Ignore
                            });
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(Properties.FormStrings.Message_JsonError_Text + Environment.NewLine + ex,
                                Properties.FormStrings.Message_Error_Caption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        if (DateMonth != null) NewElements.Add(DateMonth);
                        break;
                    #endregion

                    #region DateYear
                    case "DateYear":
                        ElementDateYear DateYear = null;
                        try
                        {
                            DateYear = JsonConvert.DeserializeObject<ElementDateYear>(elementStr, new JsonSerializerSettings
                            {
                                //DefaultValueHandling = DefaultValueHandling.Ignore,
                                NullValueHandling = NullValueHandling.Ignore
                            });
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(Properties.FormStrings.Message_JsonError_Text + Environment.NewLine + ex,
                                Properties.FormStrings.Message_Error_Caption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        if (DateYear != null) NewElements.Add(DateYear);
                        break;
                    #endregion

                    #region DateWeek
                    case "DateWeek":
                        ElementDateWeek DateWeek = null;
                        try
                        {
                            DateWeek = JsonConvert.DeserializeObject<ElementDateWeek>(elementStr, new JsonSerializerSettings
                            {
                                //DefaultValueHandling = DefaultValueHandling.Ignore,
                                NullValueHandling = NullValueHandling.Ignore
                            });
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(Properties.FormStrings.Message_JsonError_Text + Environment.NewLine + ex,
                                Properties.FormStrings.Message_Error_Caption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        if (DateWeek != null) NewElements.Add(DateWeek);
                        break;
                    #endregion

                    #region ElementStatuses
                    case "ElementStatuses":
                        ElementStatuses Statuses = null;
                        try
                        {
                            Statuses = JsonConvert.DeserializeObject<ElementStatuses>(elementStr, new JsonSerializerSettings
                            {
                                //DefaultValueHandling = DefaultValueHandling.Ignore,
                                NullValueHandling = NullValueHandling.Ignore
                            });
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(Properties.FormStrings.Message_JsonError_Text + Environment.NewLine + ex,
                                Properties.FormStrings.Message_Error_Caption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        if (Statuses != null) NewElements.Add(Statuses);
                        break;
                    #endregion

                    #region ElementShortcuts
                    case "ElementShortcuts":
                        ElementShortcuts Shortcuts = null;
                        try
                        {
                            Shortcuts = JsonConvert.DeserializeObject<ElementShortcuts>(elementStr, new JsonSerializerSettings
                            {
                                //DefaultValueHandling = DefaultValueHandling.Ignore,
                                NullValueHandling = NullValueHandling.Ignore
                            });
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(Properties.FormStrings.Message_JsonError_Text + Environment.NewLine + ex,
                                Properties.FormStrings.Message_Error_Caption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        if (Shortcuts != null) NewElements.Add(Shortcuts);
                        break;
                    #endregion

                    #region ElementSteps
                    case "ElementSteps":
                        ElementSteps Steps = null;
                        try
                        {
                            Steps = JsonConvert.DeserializeObject<ElementSteps>(elementStr, new JsonSerializerSettings
                            {
                                //DefaultValueHandling = DefaultValueHandling.Ignore,
                                NullValueHandling = NullValueHandling.Ignore
                            });
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(Properties.FormStrings.Message_JsonError_Text + Environment.NewLine + ex,
                                Properties.FormStrings.Message_Error_Caption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        if (Steps != null) NewElements.Add(Steps);
                        break;
                    #endregion

                    #region ElementBattery
                    case "ElementBattery":
                        ElementBattery Battery = null;
                        try
                        {
                            Battery = JsonConvert.DeserializeObject<ElementBattery>(elementStr, new JsonSerializerSettings
                            {
                                //DefaultValueHandling = DefaultValueHandling.Ignore,
                                NullValueHandling = NullValueHandling.Ignore
                            });
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(Properties.FormStrings.Message_JsonError_Text + Environment.NewLine + ex,
                                Properties.FormStrings.Message_Error_Caption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        if (Battery != null) NewElements.Add(Battery);
                        break;
                    #endregion

                    #region ElementCalories
                    case "ElementCalories":
                        ElementCalories Calories = null;
                        try
                        {
                            Calories = JsonConvert.DeserializeObject<ElementCalories>(elementStr, new JsonSerializerSettings
                            {
                                //DefaultValueHandling = DefaultValueHandling.Ignore,
                                NullValueHandling = NullValueHandling.Ignore
                            });
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(Properties.FormStrings.Message_JsonError_Text + Environment.NewLine + ex,
                                Properties.FormStrings.Message_Error_Caption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        if (Calories != null) NewElements.Add(Calories);
                        break;
                    #endregion

                    #region ElementHeart
                    case "ElementHeart":
                        ElementHeart Heart = null;
                        try
                        {
                            Heart = JsonConvert.DeserializeObject<ElementHeart>(elementStr, new JsonSerializerSettings
                            {
                                //DefaultValueHandling = DefaultValueHandling.Ignore,
                                NullValueHandling = NullValueHandling.Ignore
                            });
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(Properties.FormStrings.Message_JsonError_Text + Environment.NewLine + ex,
                                Properties.FormStrings.Message_Error_Caption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        if (Heart != null) NewElements.Add(Heart);
                        break;
                    #endregion

                    #region ElementPAI
                    case "ElementPAI":
                        ElementPAI PAI = null;
                        try
                        {
                            PAI = JsonConvert.DeserializeObject<ElementPAI>(elementStr, new JsonSerializerSettings
                            {
                                //DefaultValueHandling = DefaultValueHandling.Ignore,
                                NullValueHandling = NullValueHandling.Ignore
                            });
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(Properties.FormStrings.Message_JsonError_Text + Environment.NewLine + ex,
                                Properties.FormStrings.Message_Error_Caption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        if (PAI != null) NewElements.Add(PAI);
                        break;
                    #endregion

                    #region ElementDistance
                    case "ElementDistance":
                        ElementDistance Distance = null;
                        try
                        {
                            Distance = JsonConvert.DeserializeObject<ElementDistance>(elementStr, new JsonSerializerSettings
                            {
                                //DefaultValueHandling = DefaultValueHandling.Ignore,
                                NullValueHandling = NullValueHandling.Ignore
                            });
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(Properties.FormStrings.Message_JsonError_Text + Environment.NewLine + ex,
                                Properties.FormStrings.Message_Error_Caption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        if (Distance != null) NewElements.Add(Distance);
                        break;
                    #endregion

                    #region ElementStand
                    case "ElementStand":
                        ElementStand Stand = null;
                        try
                        {
                            Stand = JsonConvert.DeserializeObject<ElementStand>(elementStr, new JsonSerializerSettings
                            {
                                //DefaultValueHandling = DefaultValueHandling.Ignore,
                                NullValueHandling = NullValueHandling.Ignore
                            });
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(Properties.FormStrings.Message_JsonError_Text + Environment.NewLine + ex,
                                Properties.FormStrings.Message_Error_Caption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        if (Stand != null) NewElements.Add(Stand);
                        break;
                    #endregion

                    #region ElementActivity
                    case "ElementActivity":
                        ElementActivity Activity = null;
                        try
                        {
                            Activity = JsonConvert.DeserializeObject<ElementActivity>(elementStr, new JsonSerializerSettings
                            {
                                //DefaultValueHandling = DefaultValueHandling.Ignore,
                                NullValueHandling = NullValueHandling.Ignore
                            });
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(Properties.FormStrings.Message_JsonError_Text + Environment.NewLine + ex,
                                Properties.FormStrings.Message_Error_Caption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        if (Activity != null) NewElements.Add(Activity);
                        break;
                    #endregion

                    #region ElementSpO2
                    case "ElementSpO2":
                        ElementSpO2 SpO2 = null;
                        try
                        {
                            SpO2 = JsonConvert.DeserializeObject<ElementSpO2>(elementStr, new JsonSerializerSettings
                            {
                                //DefaultValueHandling = DefaultValueHandling.Ignore,
                                NullValueHandling = NullValueHandling.Ignore
                            });
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(Properties.FormStrings.Message_JsonError_Text + Environment.NewLine + ex,
                                Properties.FormStrings.Message_Error_Caption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        if (SpO2 != null) NewElements.Add(SpO2);
                        break;
                    #endregion

                    #region ElementStress
                    case "ElementStress":
                        ElementStress Stress = null;
                        try
                        {
                            Stress = JsonConvert.DeserializeObject<ElementStress>(elementStr, new JsonSerializerSettings
                            {
                                //DefaultValueHandling = DefaultValueHandling.Ignore,
                                NullValueHandling = NullValueHandling.Ignore
                            });
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(Properties.FormStrings.Message_JsonError_Text + Environment.NewLine + ex,
                                Properties.FormStrings.Message_Error_Caption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        if (Stress != null) NewElements.Add(Stress);
                        break;
                    #endregion

                    #region ElementFatBurning
                    case "ElementFatBurning":
                        ElementFatBurning FatBurning = null;
                        try
                        {
                            FatBurning = JsonConvert.DeserializeObject<ElementFatBurning>(elementStr, new JsonSerializerSettings
                            {
                                //DefaultValueHandling = DefaultValueHandling.Ignore,
                                NullValueHandling = NullValueHandling.Ignore
                            });
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(Properties.FormStrings.Message_JsonError_Text + Environment.NewLine + ex,
                                Properties.FormStrings.Message_Error_Caption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        if (FatBurning != null) NewElements.Add(FatBurning);
                        break;
                    #endregion


                    //#region ElementWeather
                    //case "ElementWeather":
                    //    ElementWeather Weather = null;
                    //    try
                    //    {
                    //        Weather = JsonConvert.DeserializeObject<ElementWeather>(elementStr, new JsonSerializerSettings
                    //        {
                    //            //DefaultValueHandling = DefaultValueHandling.Ignore,
                    //            NullValueHandling = NullValueHandling.Ignore
                    //        });
                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        MessageBox.Show(Properties.FormStrings.Message_JsonError_Text + Environment.NewLine + ex,
                    //            Properties.FormStrings.Message_Error_Caption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    //    }
                    //    if (Weather != null) NewElements.Add(Weather);
                    //    break;
                    //#endregion

                    #region ElementWeather
                    case "ElementWeather":
                        ElementWeather Weather = null;
                        try
                        {
                            Weather = JsonConvert.DeserializeObject<ElementWeather>(elementStr, new JsonSerializerSettings
                            {
                                //DefaultValueHandling = DefaultValueHandling.Ignore,
                                NullValueHandling = NullValueHandling.Ignore
                            });
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(Properties.FormStrings.Message_JsonError_Text + Environment.NewLine + ex,
                                Properties.FormStrings.Message_Error_Caption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        if (Weather != null) 
                        {
                            if (!editableElement)
                            {
                                ElementWeather_v2 Weather_v2_temp = new ElementWeather_v2();

                                if (Weather.Number != null)
                                {
                                    if (Weather_v2_temp.Group_Current == null) Weather_v2_temp.Group_Current = new WeatherGroup();
                                    Weather_v2_temp.Group_Current.Number = Weather.Number;
                                    Weather_v2_temp.Group_Current.position = Weather.Number.position;
                                }
                                if (Weather.Number_Font != null)
                                {
                                    if (Weather_v2_temp.Group_Current == null) Weather_v2_temp.Group_Current = new WeatherGroup();
                                    Weather_v2_temp.Group_Current.Number_Font = Weather.Number_Font;
                                    Weather_v2_temp.Group_Current.position = Weather.Number_Font.position;
                                }

                                if (Weather.Number_Min != null)
                                {
                                    if (Weather_v2_temp.Group_Min == null) Weather_v2_temp.Group_Min = new WeatherGroup();
                                    Weather_v2_temp.Group_Min.Number = Weather.Number_Min;
                                    Weather_v2_temp.Group_Min.position = Weather.Number_Min.position;
                                }
                                if (Weather.Number_Min_Font != null)
                                {
                                    if (Weather_v2_temp.Group_Min == null) Weather_v2_temp.Group_Min = new WeatherGroup();
                                    Weather_v2_temp.Group_Min.Number_Font = Weather.Number_Min_Font;
                                    Weather_v2_temp.Group_Min.position = Weather.Number_Min_Font.position;
                                }
                                if (Weather.Text_Min_rotation != null)
                                {
                                    if (Weather_v2_temp.Group_Min == null) Weather_v2_temp.Group_Min = new WeatherGroup();
                                    Weather_v2_temp.Group_Min.Text_rotation = Weather.Text_Min_rotation;
                                    Weather_v2_temp.Group_Min.position = Weather.Text_Min_rotation.position;
                                }
                                if (Weather.Text_Min_circle != null)
                                {
                                    if (Weather_v2_temp.Group_Min == null) Weather_v2_temp.Group_Min = new WeatherGroup();
                                    Weather_v2_temp.Group_Min.Text_circle = Weather.Text_Min_circle;
                                    Weather_v2_temp.Group_Min.position = Weather.Text_Min_circle.position;
                                }

                                if (Weather.Number_Max != null)
                                {
                                    if (Weather_v2_temp.Group_Max == null) Weather_v2_temp.Group_Max = new WeatherGroup();
                                    Weather_v2_temp.Group_Max.Number = Weather.Number_Max;
                                    Weather_v2_temp.Group_Max.position = Weather.Number_Max.position;
                                }
                                if (Weather.Number_Max_Font != null)
                                {
                                    if (Weather_v2_temp.Group_Max == null) Weather_v2_temp.Group_Max = new WeatherGroup();
                                    Weather_v2_temp.Group_Max.Number_Font = Weather.Number_Max_Font;
                                    Weather_v2_temp.Group_Max.position = Weather.Number_Max_Font.position;
                                }
                                if (Weather.Text_Max_rotation != null)
                                {
                                    if (Weather_v2_temp.Group_Max == null) Weather_v2_temp.Group_Max = new WeatherGroup();
                                    Weather_v2_temp.Group_Max.Text_rotation = Weather.Text_Max_rotation;
                                    Weather_v2_temp.Group_Max.position = Weather.Text_Max_rotation.position;
                                }
                                if (Weather.Text_Max_circle != null)
                                {
                                    if (Weather_v2_temp.Group_Max == null) Weather_v2_temp.Group_Max = new WeatherGroup();
                                    Weather_v2_temp.Group_Max.Text_circle = Weather.Text_Max_circle;
                                    Weather_v2_temp.Group_Max.position = Weather.Text_Max_circle.position;
                                }


                                if (Weather.Number_Min_Max_Font != null)
                                {
                                    if (Weather_v2_temp.Group_Max_Min == null) Weather_v2_temp.Group_Max_Min = new WeatherGroup();
                                    Weather_v2_temp.Group_Max_Min.Number_Font = Weather.Number_Min_Max_Font;
                                    Weather_v2_temp.Group_Max_Min.position = Weather.Number_Min_Max_Font.position;
                                }

                                if (Weather.Images != null) Weather_v2_temp.Images = Weather.Images;
                                if (Weather.City_Name != null) Weather_v2_temp.City_Name = Weather.City_Name;
                                if (Weather.Icon != null) Weather_v2_temp.Icon = Weather.Icon;

                                int index = 1;
                                for (int i = 0; i < 25; i++)
                                {
                                    if (Weather_v2_temp.Group_Current != null && Weather_v2_temp.Group_Current.position == i) Weather_v2_temp.Group_Current.position = index++;
                                    if (Weather_v2_temp.Group_Min != null && Weather_v2_temp.Group_Min.position == i) Weather_v2_temp.Group_Min.position = index++;
                                    if (Weather_v2_temp.Group_Max != null && Weather_v2_temp.Group_Max.position == i) Weather_v2_temp.Group_Max.position = index++;
                                    if (Weather_v2_temp.Group_Max_Min != null && Weather_v2_temp.Group_Max_Min.position == i) Weather_v2_temp.Group_Max_Min.position = index++;
                                    if (Weather_v2_temp.Images != null && Weather_v2_temp.Images.position == i) Weather_v2_temp.Images.position = index++;
                                    if (Weather_v2_temp.City_Name != null && Weather_v2_temp.City_Name.position == i) Weather_v2_temp.City_Name.position = index++;
                                    if (Weather_v2_temp.Icon != null && Weather_v2_temp.Icon.position == i) Weather_v2_temp.Icon.position = index++;
                                }

                                NewElements.Add(Weather_v2_temp); 
                            }
                            else NewElements.Add(Weather);
                        }
                        break;
                    #endregion

                    #region ElementWeather_v2
                    case "ElementWeather_v2":
                        ElementWeather_v2 Weather_v2 = null;
                        try
                        {
                            Weather_v2 = JsonConvert.DeserializeObject<ElementWeather_v2>(elementStr, new JsonSerializerSettings
                            {
                                //DefaultValueHandling = DefaultValueHandling.Ignore,
                                NullValueHandling = NullValueHandling.Ignore
                            });
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(Properties.FormStrings.Message_JsonError_Text + Environment.NewLine + ex,
                                Properties.FormStrings.Message_Error_Caption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        if (Weather_v2 != null) NewElements.Add(Weather_v2);
                        break;
                    #endregion

                    #region Element_Weather_FewDays
                    case "Element_Weather_FewDays":
                        Element_Weather_FewDays Weather_FewDays = null;
                        try
                        {
                            Weather_FewDays = JsonConvert.DeserializeObject<Element_Weather_FewDays>(elementStr, new JsonSerializerSettings
                            {
                                //DefaultValueHandling = DefaultValueHandling.Ignore,
                                NullValueHandling = NullValueHandling.Ignore
                            });
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(Properties.FormStrings.Message_JsonError_Text + Environment.NewLine + ex,
                                Properties.FormStrings.Message_Error_Caption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        if (Weather_FewDays != null) NewElements.Add(Weather_FewDays);
                        break;
                    #endregion

                    #region ElementUVIndex
                    case "ElementUVIndex":
                        ElementUVIndex UVIndex = null;
                        try
                        {
                            UVIndex = JsonConvert.DeserializeObject<ElementUVIndex>(elementStr, new JsonSerializerSettings
                            {
                                //DefaultValueHandling = DefaultValueHandling.Ignore,
                                NullValueHandling = NullValueHandling.Ignore
                            });
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(Properties.FormStrings.Message_JsonError_Text + Environment.NewLine + ex,
                                Properties.FormStrings.Message_Error_Caption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        if (UVIndex != null) NewElements.Add(UVIndex);
                        break;
                    #endregion

                    #region ElementHumidity
                    case "ElementHumidity":
                        ElementHumidity Humidity = null;
                        try
                        {
                            Humidity = JsonConvert.DeserializeObject<ElementHumidity>(elementStr, new JsonSerializerSettings
                            {
                                //DefaultValueHandling = DefaultValueHandling.Ignore,
                                NullValueHandling = NullValueHandling.Ignore
                            });
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(Properties.FormStrings.Message_JsonError_Text + Environment.NewLine + ex,
                                Properties.FormStrings.Message_Error_Caption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        if (Humidity != null) NewElements.Add(Humidity);
                        break;
                    #endregion

                    #region ElementAltimeter
                    case "ElementAltimeter":
                        ElementAltimeter Altimeter = null;
                        try
                        {
                            Altimeter = JsonConvert.DeserializeObject<ElementAltimeter>(elementStr, new JsonSerializerSettings
                            {
                                //DefaultValueHandling = DefaultValueHandling.Ignore,
                                NullValueHandling = NullValueHandling.Ignore
                            });
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(Properties.FormStrings.Message_JsonError_Text + Environment.NewLine + ex,
                                Properties.FormStrings.Message_Error_Caption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        if (Altimeter != null) NewElements.Add(Altimeter);
                        break;
                    #endregion

                    #region ElementSunrise
                    case "ElementSunrise":
                        ElementSunrise Sunrise = null;
                        try
                        {
                            Sunrise = JsonConvert.DeserializeObject<ElementSunrise>(elementStr, new JsonSerializerSettings
                            {
                                //DefaultValueHandling = DefaultValueHandling.Ignore,
                                NullValueHandling = NullValueHandling.Ignore
                            });
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(Properties.FormStrings.Message_JsonError_Text + Environment.NewLine + ex,
                                Properties.FormStrings.Message_Error_Caption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        if (Sunrise != null) NewElements.Add(Sunrise);
                        break;
                    #endregion

                    #region ElementWind
                    case "ElementWind":
                        ElementWind Wind = null;
                        try
                        {
                            Wind = JsonConvert.DeserializeObject<ElementWind>(elementStr, new JsonSerializerSettings
                            {
                                //DefaultValueHandling = DefaultValueHandling.Ignore,
                                NullValueHandling = NullValueHandling.Ignore
                            });
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(Properties.FormStrings.Message_JsonError_Text + Environment.NewLine + ex,
                                Properties.FormStrings.Message_Error_Caption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        if (Wind != null) NewElements.Add(Wind);
                        break;
                    #endregion

                    #region ElementMoon
                    case "ElementMoon":
                        ElementMoon Moon = null;
                        try
                        {
                            Moon = JsonConvert.DeserializeObject<ElementMoon>(elementStr, new JsonSerializerSettings
                            {
                                //DefaultValueHandling = DefaultValueHandling.Ignore,
                                NullValueHandling = NullValueHandling.Ignore
                            });
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(Properties.FormStrings.Message_JsonError_Text + Environment.NewLine + ex,
                                Properties.FormStrings.Message_Error_Caption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        if (Moon != null) NewElements.Add(Moon);
                        break;
                    #endregion

                    #region ElementImage
                    case "ElementImage":
                        ElementImage Image = null;
                        try
                        {
                            Image = JsonConvert.DeserializeObject<ElementImage>(elementStr, new JsonSerializerSettings
                            {
                                //DefaultValueHandling = DefaultValueHandling.Ignore,
                                NullValueHandling = NullValueHandling.Ignore
                            });
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(Properties.FormStrings.Message_JsonError_Text + Environment.NewLine + ex,
                                Properties.FormStrings.Message_Error_Caption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        if (Image != null) NewElements.Add(Image);
                        break;
                    #endregion

                    #region ElementCompass
                    case "ElementCompass":
                        ElementCompass Compass = null;
                        try
                        {
                            Compass = JsonConvert.DeserializeObject<ElementCompass>(elementStr, new JsonSerializerSettings
                            {
                                //DefaultValueHandling = DefaultValueHandling.Ignore,
                                NullValueHandling = NullValueHandling.Ignore
                            });
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(Properties.FormStrings.Message_JsonError_Text + Environment.NewLine + ex,
                                Properties.FormStrings.Message_Error_Caption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        if (Compass != null) NewElements.Add(Compass);
                        break;
                    #endregion


                    #region ElementAlarmClock
                    case "ElementAlarmClock":
                        ElementAlarmClock AlarmClock = null;
                        try
                        {
                            AlarmClock = JsonConvert.DeserializeObject<ElementAlarmClock>(elementStr, new JsonSerializerSettings
                            {
                                //DefaultValueHandling = DefaultValueHandling.Ignore,
                                NullValueHandling = NullValueHandling.Ignore
                            });
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(Properties.FormStrings.Message_JsonError_Text + Environment.NewLine + ex,
                                Properties.FormStrings.Message_Error_Caption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        if (AlarmClock != null) NewElements.Add(AlarmClock);
                        break;
                    #endregion

                    #region ElementTrainingLoad
                    case "ElementTrainingLoad":
                        ElementTrainingLoad TrainingLoad = null;
                        try
                        {
                            TrainingLoad = JsonConvert.DeserializeObject<ElementTrainingLoad>(elementStr, new JsonSerializerSettings
                            {
                                //DefaultValueHandling = DefaultValueHandling.Ignore,
                                NullValueHandling = NullValueHandling.Ignore
                            });
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(Properties.FormStrings.Message_JsonError_Text + Environment.NewLine + ex,
                                Properties.FormStrings.Message_Error_Caption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        if (TrainingLoad != null) NewElements.Add(TrainingLoad);
                        break;
                    #endregion

                    #region ElementVO2Max
                    case "ElementVO2Max":
                        ElementVO2Max VO2Max = null;
                        try
                        {
                            VO2Max = JsonConvert.DeserializeObject<ElementVO2Max>(elementStr, new JsonSerializerSettings
                            {
                                //DefaultValueHandling = DefaultValueHandling.Ignore,
                                NullValueHandling = NullValueHandling.Ignore
                            });
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(Properties.FormStrings.Message_JsonError_Text + Environment.NewLine + ex,
                                Properties.FormStrings.Message_Error_Caption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        if (VO2Max != null) NewElements.Add(VO2Max);
                        break;
                    #endregion

                    #region ElementAQI
                    case "ElementAQI":
                        ElementAQI AQI = null;
                        try
                        {
                            AQI = JsonConvert.DeserializeObject<ElementAQI>(elementStr, new JsonSerializerSettings
                            {
                                //DefaultValueHandling = DefaultValueHandling.Ignore,
                                NullValueHandling = NullValueHandling.Ignore
                            });
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(Properties.FormStrings.Message_JsonError_Text + Environment.NewLine + ex,
                                Properties.FormStrings.Message_Error_Caption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        if (AQI != null) NewElements.Add(AQI);
                        break;
                    #endregion

                    #region ElementBodyTemp
                    case "ElementBodyTemp":
                        ElementBodyTemp BodyTemp = null;
                        try
                        {
                            BodyTemp = JsonConvert.DeserializeObject<ElementBodyTemp>(elementStr, new JsonSerializerSettings
                            {
                                //DefaultValueHandling = DefaultValueHandling.Ignore,
                                NullValueHandling = NullValueHandling.Ignore
                            });
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(Properties.FormStrings.Message_JsonError_Text + Environment.NewLine + ex,
                                Properties.FormStrings.Message_Error_Caption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        if (BodyTemp != null) NewElements.Add(BodyTemp);
                        break;
                    #endregion

                    #region ElementFloor
                    case "ElementFloor":
                        ElementFloor Floor = null;
                        try
                        {
                            Floor = JsonConvert.DeserializeObject<ElementFloor>(elementStr, new JsonSerializerSettings
                            {
                                //DefaultValueHandling = DefaultValueHandling.Ignore,
                                NullValueHandling = NullValueHandling.Ignore
                            });
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(Properties.FormStrings.Message_JsonError_Text + Environment.NewLine + ex,
                                Properties.FormStrings.Message_Error_Caption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        if (Floor != null) NewElements.Add(Floor);
                        break;
                    #endregion

                    #region ElementReadiness
                    case "ElementReadiness":
                        ElementReadiness Readiness = null;
                        try
                        {
                            Readiness = JsonConvert.DeserializeObject<ElementReadiness>(elementStr, new JsonSerializerSettings
                            {
                                //DefaultValueHandling = DefaultValueHandling.Ignore,
                                NullValueHandling = NullValueHandling.Ignore
                            });
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(Properties.FormStrings.Message_JsonError_Text + Environment.NewLine + ex,
                                Properties.FormStrings.Message_Error_Caption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        if (Readiness != null) NewElements.Add(Readiness);
                        break;
                    #endregion

                    #region ElementHRV
                    case "ElementHRV":
                        ElementHRV HRV = null;
                        try
                        {
                            HRV = JsonConvert.DeserializeObject<ElementHRV>(elementStr, new JsonSerializerSettings
                            {
                                //DefaultValueHandling = DefaultValueHandling.Ignore,
                                NullValueHandling = NullValueHandling.Ignore
                            });
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(Properties.FormStrings.Message_JsonError_Text + Environment.NewLine + ex,
                                Properties.FormStrings.Message_Error_Caption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        if (HRV != null) NewElements.Add(HRV);
                        break;
                    #endregion


                    #region ElementScript
                    case "ElementScript":
                        ElementScript Script = null;
                        try
                        {
                            Script = JsonConvert.DeserializeObject<ElementScript>(elementStr, new JsonSerializerSettings
                            {
                                //DefaultValueHandling = DefaultValueHandling.Ignore,
                                NullValueHandling = NullValueHandling.Ignore
                            });
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(Properties.FormStrings.Message_JsonError_Text + Environment.NewLine + ex,
                                Properties.FormStrings.Message_Error_Caption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        if (Script != null) NewElements.Add(Script);
                        break;
                    #endregion


                    #region ElementAnimation
                    case "ElementAnimation":
                        ElementAnimation Animation = null;
                        try
                        {
                            Animation = JsonConvert.DeserializeObject<ElementAnimation>(elementStr, new JsonSerializerSettings
                            {
                                //DefaultValueHandling = DefaultValueHandling.Ignore,
                                NullValueHandling = NullValueHandling.Ignore
                            });
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(Properties.FormStrings.Message_JsonError_Text + Environment.NewLine + ex,
                                Properties.FormStrings.Message_Error_Caption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        if (Animation != null) NewElements.Add(Animation);
                        break;
                        #endregion
                }
            }
            return NewElements;
        }

        /// <summary>Выделяем тип элемента из строки с параметрами</summary>
        private string GetTypeFromSring(string str)
        {
            string name = "elementName";
            string returnStr = "";
            int indexOf = str.IndexOf("elementName");
            if (indexOf >= 0)
            {
                str = str.Substring(indexOf + name.Length + 2);
                indexOf = str.IndexOf("\"");
                str = str.Substring(indexOf + 1);
                indexOf = str.IndexOf("\"");
                str = str.Substring(0, indexOf);
                returnStr = str;
            }
            return returnStr;
        }

        /// <summary>Читаем настройки для фона</summary>
        private void Read_Background_Options(Background background, bool Editable_background, bool Editable_background_ShowOnAOD, 
            string preview = "", int id = 0)
        {
            PreviewView = false;
            userCtrl_Background_Options.SettingsClear();
            userCtrl_Background_Options.Editable_background = Editable_background;

            if (preview != null && preview.Length > 0) userCtrl_Background_Options.SetPreview(preview);
            if (id > 999 && id < 10000000) userCtrl_Background_Options.SetID(id);
            if (background == null) 
            {
                PreviewView = true;
                return;
            }
            userCtrl_Background_Options.Visible = true;
            if (background == null)
            {
                PreviewView = true;
                return;
            }
            if (background.BackgroundImage != null)
            {
                userCtrl_Background_Options.SetBackground(background.BackgroundImage.src);
                userCtrl_Background_Options.Switch_ImageType(0);
            }
            if (background.BackgroundColor != null)
            {
                userCtrl_Background_Options.SetColorBackground(StringToColor(background.BackgroundColor.color));
                userCtrl_Background_Options.Switch_ImageType(1);
            }
            if (background.Editable_Background != null && background.Editable_Background.enable_edit_bg)
            {
                userCtrl_Background_Options.Switch_ImageType(2);

                Read_EditableBackground_Options(background.Editable_Background);
                uCtrl_EditableBackground_Opt.Visible = true;
            }
            if (Editable_background_ShowOnAOD) userCtrl_Background_Options.Switch_ImageType(2);
            userCtrl_Background_Options._Background = background;

            PreviewView = true;
        }

        /// <summary>Читаем настройки для отображения числа картинками</summary>
        /// <param name="img_number">Элемент hmUI_widget_IMG_NUMBER для отображения настроек</param>
        /// <param name="_dastance">Режим отображения для дистанции</param>
        /// <param name="_follow">Отображение параметра "Следовать за ..."</param>
        /// <param name="_followText">Текст для параметра параметра "Следовать за ..."</param>
        /// <param name="_imageError">Отображение настроек изображения при ошибке</param>
        /// <param name="_optionalSymbol">Отображение настроек дополнительного символа</param>
        /// <param name="_padingZero">Отображение настроек ведущих нулей</param>
        /// <param name="_sunrise">Режим отображения для восхода/заката</param>
        /// <param name="_altitude">Режим отображения для высоты</param>
        /// <param name="_compass">Режим отображения для компаса</param>
        /// <param name="_bodyTemp">Режим отображения для тмпературы тела</param>
        /// <param name="_angleVisible">Видимость настроек угла</param>
        private void Read_ImgNumber_Options(hmUI_widget_IMG_NUMBER img_number, bool _dastance, bool _follow, string _followText,
            bool _imageError, bool _optionalSymbol, bool _padingZero, bool _angleVisible, bool _sunrise = false, 
            bool _altitude = false, bool _compass = false, bool _bodyTemp = false)
        {
            PreviewView = false;

            uCtrl_Text_Opt.SettingsClear();

            uCtrl_Text_Opt.Distance = _dastance;
            uCtrl_Text_Opt.Sunrise = _sunrise;
            uCtrl_Text_Opt.Altitude = _altitude;
            uCtrl_Text_Opt._Compass = _compass;
            uCtrl_Text_Opt.BodyTemp = _bodyTemp;
            uCtrl_Text_Opt.Follow = _follow;
            uCtrl_Text_Opt.FollowText = _followText;
            uCtrl_Text_Opt.ImageError = _imageError;
            uCtrl_Text_Opt.OptionalSymbol = _optionalSymbol;
            uCtrl_Text_Opt.PaddingZero = _padingZero;
            uCtrl_Text_Opt.AngleVisible = _angleVisible;
            uCtrl_Text_Opt.Visible = true;
            uCtrl_Text_Opt.Alpha = SelectedModel.versionOS >= 2.1;
            if (!_optionalSymbol)
            {
                if (img_number.dot_image != null)
                {
                    JSON_Modified = true;
                    img_number.dot_image = null;
                }
            }
            uCtrl_Text_Opt.WidgetProperty = WidgetProperty;

            if (SelectedModel.versionOS >= 2) uCtrl_Text_Opt.Angle = true;

            uCtrl_Text_Opt._ElementWithText = img_number;

            if (img_number == null)
            {
                PreviewView = true; 
                return; 
            }
            if (img_number.img_First != null) 
                uCtrl_Text_Opt.SetImage(img_number.img_First);
            uCtrl_Text_Opt.numericUpDown_imageX.Value = img_number.imageX;
            uCtrl_Text_Opt.numericUpDown_imageY.Value = img_number.imageY;

            uCtrl_Text_Opt.SetIcon(img_number.icon);
            uCtrl_Text_Opt.numericUpDown_iconX.Value = img_number.iconPosX;
            uCtrl_Text_Opt.numericUpDown_iconY.Value = img_number.iconPosY;

            uCtrl_Text_Opt.SetUnit(img_number.unit);
            uCtrl_Text_Opt.SetUnitMile(img_number.imperial_unit);
            if (!_compass) uCtrl_Text_Opt.SetImageError(img_number.invalid_image);
            else uCtrl_Text_Opt.SetImageError(img_number.negative_image);
            if (!_altitude) uCtrl_Text_Opt.SetImageDecimalPoint(img_number.dot_image);
            else uCtrl_Text_Opt.SetImageDecimalPoint(img_number.negative_image);
            //uCtrl_Text_Opt.SetImageDecimalPointOrMinus
            uCtrl_Text_Opt.numericUpDown_spacing.Value = img_number.space;
            uCtrl_Text_Opt.numericUpDown_angle.Value = img_number.angle;
            uCtrl_Text_Opt.numericUpDown_Alpha.Value = img_number.alpha;
            uCtrl_Text_Opt.numericUpDown_iconAlpha.Value = img_number.icon_alpha;

            uCtrl_Text_Opt.SetAlignment(img_number.align);

            uCtrl_Text_Opt.checkBox_addZero.Checked = img_number.zero;
            uCtrl_Text_Opt.checkBox_follow.Checked = img_number.follow;

            //uCtrl_Text_Opt.SetUnitMile

            PreviewView = true;
        }

        /// <summary>Читаем настройки для отображения числа картинками</summary>
        /// <param name="img_number">Элемент hmUI_widget_IMG_NUMBER для отображения настроек</param>
        /// <param name="_dastance">Режим отображения для дистанции</param>
        /// <param name="_optionalSymbol">Отображение настроек дополнительного символа</param>
        /// <param name="_padingZero">Отображение настроек ведущих нулей</param>
        /// <param name="_sunrise">Режим отображения для восхода/заката</param>
        /// <param name="_weather">Режим отображения для погоды</param>
        /// <param name="_year">Режим отображения для года</param>
        private void Read_ImgNumber_Rotate_Options(hmUI_widget_IMG_NUMBER img_number, bool _dastance, bool _weather, bool _sunrise, bool _year,
            bool _optionalSymbol, bool _padingZero)
        {
            PreviewView = false;

            uCtrl_Text_Rotate_Opt.SettingsClear();

            uCtrl_Text_Rotate_Opt.Distance = _dastance;
            uCtrl_Text_Rotate_Opt.Sunrise = _sunrise;
            uCtrl_Text_Rotate_Opt.Weather = _weather;
            uCtrl_Text_Rotate_Opt.Imperial_unit = SelectedModel.versionOS > 2;
            uCtrl_Text_Rotate_Opt.Year = _year;
            uCtrl_Text_Rotate_Opt.ImageError = true;
            uCtrl_Text_Rotate_Opt.OptionalSymbol = _optionalSymbol;
            uCtrl_Text_Rotate_Opt.PaddingZero = _padingZero;
            uCtrl_Text_Rotate_Opt.Visible = true;

            uCtrl_Text_Rotate_Opt._ElementWithText = img_number;

            if (img_number == null)
            {
                PreviewView = true;
                return;
            }
            if (img_number.img_First != null)
                uCtrl_Text_Rotate_Opt.SetImage(img_number.img_First);
            uCtrl_Text_Rotate_Opt.numericUpDown_imageX.Value = img_number.imageX;
            uCtrl_Text_Rotate_Opt.numericUpDown_imageY.Value = img_number.imageY;

            uCtrl_Text_Rotate_Opt.SetUnit(img_number.unit);
            uCtrl_Text_Rotate_Opt.SetUnitMile(img_number.imperial_unit);
            uCtrl_Text_Rotate_Opt.SetImageError(img_number.invalid_image);
            uCtrl_Text_Rotate_Opt.SetImageDecimalPointOrMinus(img_number.dot_image);
            if(_weather) uCtrl_Text_Rotate_Opt.SetImageDecimalPointOrMinus(img_number.negative_image);
            uCtrl_Text_Rotate_Opt.SetSeparator(img_number.separator_image); // разделитель для макс/мин температуры
            uCtrl_Text_Rotate_Opt.numericUpDown_spacing.Value = img_number.space;
            uCtrl_Text_Rotate_Opt.numericUpDown_angle.Value = img_number.angle;

            uCtrl_Text_Rotate_Opt.SetAlignment(img_number.align);

            uCtrl_Text_Rotate_Opt.checkBox_addZero.Checked = img_number.zero;
            uCtrl_Text_Rotate_Opt.checkBox_unit_in_alignment.Checked = img_number.unit_in_alignment;

            //uCtrl_Text_Rotate_Opt.SetUnitMile

            PreviewView = true;
        }

        /// <summary>Читаем настройки для отображения числа картинками</summary>
        /// <param name="text_circle">Элемент Text_Circle для отображения настроек</param>
        /// <param name="_dastance">Режим отображения для дистанции</param>
        /// <param name="_optionalSymbol">Отображение настроек дополнительного символа</param>
        /// <param name="_padingZero">Отображение настроек ведущих нулей</param>
        /// <param name="_sunrise">Режим отображения для восхода/заката</param>
        /// <param name="_angleVisible">Видимость настроек угла</param>
        private void Read_TextCircle_Options(Text_Circle text_circle, bool _dastance, bool _weather, bool _sunrise, bool _year, bool _optionalSymbol, bool _padingZero)
        {
            PreviewView = false;

            uCtrl_Text_Circle_Opt.SettingsClear();

            uCtrl_Text_Circle_Opt.Distance = _dastance;
            uCtrl_Text_Circle_Opt.Weather = _weather;
            uCtrl_Text_Circle_Opt.Sunrise = _sunrise;
            uCtrl_Text_Circle_Opt.Imperial_unit = SelectedModel.versionOS > 2;
            //uCtrl_Text_Circle_Opt.ImageError = _imageError;
            uCtrl_Text_Circle_Opt.Year = _year;
            uCtrl_Text_Circle_Opt.OptionalSymbol = _optionalSymbol;
            uCtrl_Text_Circle_Opt.PaddingZero = _padingZero;
            uCtrl_Text_Circle_Opt.Visible = true;
            //if (!_optionalSymbol)
            //{
            //    if (text_circle.dot_image != null)
            //    {
            //        JSON_Modified = true;
            //        text_circle.dot_image = null;
            //    }
            //}


            uCtrl_Text_Circle_Opt._ElementWithText = text_circle;

            if (text_circle == null)
            {
                PreviewView = true;
                return;
            }
            if (text_circle.img_First != null)
                uCtrl_Text_Circle_Opt.SetImage(text_circle.img_First);
            uCtrl_Text_Circle_Opt.numericUpDown_centr_X.Value = text_circle.circle_center_X;
            uCtrl_Text_Circle_Opt.numericUpDown_centr_Y.Value = text_circle.circle_center_Y;

            uCtrl_Text_Circle_Opt.numericUpDown_angle.Value = text_circle.angle;
            uCtrl_Text_Circle_Opt.numericUpDown_radius.Value = text_circle.radius;
            uCtrl_Text_Circle_Opt.numericUpDown_spacing.Value = text_circle.char_space_angle;

            uCtrl_Text_Circle_Opt.SetUnit(text_circle.unit);
            uCtrl_Text_Circle_Opt.SetUnitImperial(text_circle.imperial_unit);
            uCtrl_Text_Circle_Opt.SetImageError(text_circle.error_image);
            uCtrl_Text_Circle_Opt.SetImageDecimalPoint(text_circle.dot_image);
            uCtrl_Text_Circle_Opt.SetSeparator(text_circle.separator_image);

            uCtrl_Text_Circle_Opt.SetHorizontalAlignment(text_circle.horizontal_alignment);
            uCtrl_Text_Circle_Opt.SetVerticalAlignment(text_circle.vertical_alignment);

            uCtrl_Text_Circle_Opt.checkBox_addZero.Checked = text_circle.zero;
            uCtrl_Text_Circle_Opt.checkBox_reverse_direction.Checked = text_circle.reverse_direction;
            uCtrl_Text_Circle_Opt.checkBox_unit_in_alignment.Checked = text_circle.unit_in_alignment;


            PreviewView = true;
        }

        /// <summary>Читаем настройки для отображения температуры</summary>
        private void Read_ImgNumberWeather_Options(hmUI_widget_IMG_NUMBER img_number)
        {
            PreviewView = false;

            uCtrl_Text_Weather_Opt.SettingsClear();

            uCtrl_Text_Weather_Opt.Visible = true;

            //if (ProgramSettings.Watch_Model == "GTR 4" || ProgramSettings.Watch_Model == "GTS 4" || ProgramSettings.Watch_Model == "T-Rex 2")
            if (SelectedModel.versionOS >= 2) uCtrl_Text_Weather_Opt.Angle = true;

            uCtrl_Text_Weather_Opt._ElementWithText = img_number;

            if (img_number == null)
            {
                PreviewView = true;
                return;
            }
            if (img_number.img_First != null)
                uCtrl_Text_Weather_Opt.SetImage(img_number.img_First);
            uCtrl_Text_Weather_Opt.numericUpDown_imageX.Value = img_number.imageX;
            uCtrl_Text_Weather_Opt.numericUpDown_imageY.Value = img_number.imageY;

            uCtrl_Text_Weather_Opt.SetIcon(img_number.icon);
            uCtrl_Text_Weather_Opt.numericUpDown_iconX.Value = img_number.iconPosX;
            uCtrl_Text_Weather_Opt.numericUpDown_iconY.Value = img_number.iconPosY;

            uCtrl_Text_Weather_Opt.SetUnit_C(img_number.unit);
            uCtrl_Text_Weather_Opt.SetUnit_F(img_number.imperial_unit);
            uCtrl_Text_Weather_Opt.SetImageError(img_number.invalid_image);
            uCtrl_Text_Weather_Opt.SetImageMinus(img_number.negative_image);
            uCtrl_Text_Weather_Opt.SetSeparator(img_number.dot_image);
            uCtrl_Text_Weather_Opt.numericUpDown_spacing.Value = img_number.space;
            uCtrl_Text_Weather_Opt.numericUpDown_angle.Value = img_number.angle;

            uCtrl_Text_Weather_Opt.SetAlignment(img_number.align);

            PreviewView = true;
        }

        /// <summary>Читаем настройки для отображения иконки</summary>
        private void Read_Icon_Options(hmUI_widget_IMG img)
        {
            PreviewView = false;

            uCtrl_Icon_Opt.SettingsClear();
            uCtrl_Icon_Opt.Visible = true;
            uCtrl_Icon_Opt.Alpha = SelectedModel.versionOS >= 2.1;

            uCtrl_Icon_Opt._Icon = img;

            //userCtrl_Background_Options.SettingsClear();

            if (img == null)
            {
                PreviewView = true;
                return;
            }

            if (img.src != null) uCtrl_Icon_Opt.SetIcon(img.src);
            uCtrl_Icon_Opt.numericUpDown_iconX.Value = img.x;
            uCtrl_Icon_Opt.numericUpDown_iconY.Value = img.y;
            uCtrl_Icon_Opt.numericUpDown_Alpha.Value = img.alpha;

            PreviewView = true;
        }

        /// <summary>Читаем настройки для отображения статусов</summary>
        private void Read_Statuses_Options(hmUI_widget_IMG_STATUS img_status)
        {
            PreviewView = false;

            uCtrl_Icon_Opt.SettingsClear();

            uCtrl_Icon_Opt._Icon = img_status;
            uCtrl_Icon_Opt.Alpha = SelectedModel.versionOS >= 2.1;

            //userCtrl_Background_Options.SettingsClear();

            if (img_status == null)
            {
                PreviewView = true;
                return;
            }

            if (img_status.src != null) uCtrl_Icon_Opt.SetIcon(img_status.src);
            uCtrl_Icon_Opt.numericUpDown_iconX.Value = img_status.x;
            uCtrl_Icon_Opt.numericUpDown_iconY.Value = img_status.y;
            uCtrl_Icon_Opt.numericUpDown_Alpha.Value = img_status.alpha;

            PreviewView = true;
        }

        /// <summary>Читаем настройки для отображения статусов</summary>
        private void Read_Shortcuts_Options(hmUI_widget_IMG_CLICK img_click)
        {
            PreviewView = false;

            uCtrl_Shortcut_Opt.SettingsClear();

            uCtrl_Shortcut_Opt._Shortcut = img_click;

            //userCtrl_Background_Options.SettingsClear();

            if (img_click == null)
            {
                PreviewView = true;
                return;
            }

            if (img_click.src != null)
                uCtrl_Shortcut_Opt.SetImage(img_click.src);
            uCtrl_Shortcut_Opt.numericUpDown_imageX.Value = img_click.x;
            uCtrl_Shortcut_Opt.numericUpDown_imageY.Value = img_click.y;
            uCtrl_Shortcut_Opt.numericUpDown_width.Value = img_click.w;
            uCtrl_Shortcut_Opt.numericUpDown_height.Value = img_click.h;

            PreviewView = true;
        }

        /// <summary>Читаем настройки для отображения покадровой анимации</summary>
        private void Read_FrameAnimation_Options(hmUI_widget_IMG_ANIM_List frame_animation_list)
        {
            PreviewView = false;

            uCtrl_Animation_Frame_Opt.SettingsClear();

            uCtrl_Animation_Frame_Opt._AnimationFrame = frame_animation_list;

            //userCtrl_Background_Options.SettingsClear();

            if (frame_animation_list == null || frame_animation_list.Frame_Animation == null)
            {
                uCtrl_Animation_Frame_Opt.SetAnimationCount(0);
                PreviewView = true;
                return;
            }

            uCtrl_Animation_Frame_Opt.SetAnimationCount(frame_animation_list.Frame_Animation.Count);
            int selected_animation = frame_animation_list.selected_animation;
            if (selected_animation < 0 || selected_animation > frame_animation_list.Frame_Animation.Count-1)
            {
                PreviewView = true;
                return;
            }

            hmUI_widget_IMG_ANIM frame_animation = frame_animation_list.Frame_Animation[selected_animation];
            //uCtrl_Animation_Frame_Opt.SetAnimationCount(frame_animation_list.Frame_Animation.Count);
            uCtrl_Animation_Frame_Opt.SetAnimationIndex(selected_animation);
            if (frame_animation.anim_src != null)
                uCtrl_Animation_Frame_Opt.SetImage(frame_animation.anim_src);
            uCtrl_Animation_Frame_Opt.numericUpDown_imageX.Value = frame_animation.x;
            uCtrl_Animation_Frame_Opt.numericUpDown_imageY.Value = frame_animation.y;
            uCtrl_Animation_Frame_Opt.numericUpDown_images_count.Value = frame_animation.anim_size;
            uCtrl_Animation_Frame_Opt.numericUpDown_preview_frame.Maximum = frame_animation.anim_size;
            if (frame_animation.preview_frame <= frame_animation.anim_size)
            {
                uCtrl_Animation_Frame_Opt.numericUpDown_preview_frame.Value = frame_animation.preview_frame; 
            }
            else
            {
                uCtrl_Animation_Frame_Opt.numericUpDown_preview_frame.Value = 1;
            }

            uCtrl_Animation_Frame_Opt.numericUpDown_fps.Value = frame_animation.anim_fps;
            uCtrl_Animation_Frame_Opt.checkBox_anim_repeat.Checked = frame_animation.anim_repeat;
            //uCtrl_Animation_Frame_Opt.checkBox_anim_restart.Checked = frame_animation.display_on_restart;

            uCtrl_Animation_Frame_Opt.label_prefix.Text = frame_animation.anim_prefix;
            uCtrl_Animation_Frame_Opt.checkBox_visible.Checked = frame_animation.visible;
            PreviewView = true;
        }

        /// <summary>Читаем настройки для отображения анимации движения</summary>
        private void Read_MotionAnimation_Options(Motion_Animation_List motion_animation_list)
        {
            PreviewView = false;

            uCtrl_Animation_Motion_Opt.SettingsClear();

            uCtrl_Animation_Motion_Opt._AnimationMotion = motion_animation_list;

            //userCtrl_Background_Options.SettingsClear();

            if (motion_animation_list == null || motion_animation_list.Motion_Animation == null)
            {
                uCtrl_Animation_Motion_Opt.SetAnimationCount(0);
                PreviewView = true;
                return;
            }

            uCtrl_Animation_Motion_Opt.SetAnimationCount(motion_animation_list.Motion_Animation.Count);
            int selected_animation = motion_animation_list.selected_animation;
            if (selected_animation < 0 || selected_animation > motion_animation_list.Motion_Animation.Count - 1)
            {
                PreviewView = true;
                return;
            }

            Motion_Animation motion_animation = motion_animation_list.Motion_Animation[selected_animation];
            uCtrl_Animation_Motion_Opt.SetAnimationIndex(selected_animation);
            if (motion_animation.src != null)
                uCtrl_Animation_Motion_Opt.SetImage(motion_animation.src);
            uCtrl_Animation_Motion_Opt.numericUpDown_start_x.Value = motion_animation.x_start;
            uCtrl_Animation_Motion_Opt.numericUpDown_start_y.Value = motion_animation.y_start;
            uCtrl_Animation_Motion_Opt.numericUpDown_end_x.Value = motion_animation.x_end;
            uCtrl_Animation_Motion_Opt.numericUpDown_end_y.Value = motion_animation.y_end;

            uCtrl_Animation_Motion_Opt.numericUpDown_fps.Value = motion_animation.anim_fps;
            uCtrl_Animation_Motion_Opt.numericUpDown_anim_duration.Value = (decimal)(motion_animation.anim_duration/1000f);
            uCtrl_Animation_Motion_Opt.numericUpDown_repeat_count.Value = motion_animation.repeat_count;

            uCtrl_Animation_Motion_Opt.checkBox_anim_two_sides.Checked = motion_animation.anim_two_sides;
            uCtrl_Animation_Motion_Opt.checkBox_show_in_startPos.Checked = motion_animation.show_in_start;
            uCtrl_Animation_Motion_Opt.checkBox_visible.Checked = motion_animation.visible;

            PreviewView = true;
        }

        /// <summary>Читаем настройки для отображения анимации движения </summary>
        private void Read_RotateAnimation_Options(Rotate_Animation_List rotate_animation_list)
        {
            PreviewView = false;

            uCtrl_Animation_Rotate_Opt.SettingsClear();

            uCtrl_Animation_Rotate_Opt._AnimationRotate = rotate_animation_list;

            //userCtrl_Background_Options.SettingsClear();

            if (rotate_animation_list == null || rotate_animation_list.Rotate_Animation == null)
            {
                uCtrl_Animation_Rotate_Opt.SetAnimationCount(0);
                PreviewView = true;
                return;
            }

            uCtrl_Animation_Rotate_Opt.SetAnimationCount(rotate_animation_list.Rotate_Animation.Count);
            int selected_animation = rotate_animation_list.selected_animation;
            if (selected_animation < 0 || selected_animation > rotate_animation_list.Rotate_Animation.Count - 1)
            {
                PreviewView = true;
                return;
            }

            Rotate_Animation rotate_animation = rotate_animation_list.Rotate_Animation[selected_animation];
            uCtrl_Animation_Rotate_Opt.SetAnimationIndex(selected_animation);
            if (rotate_animation.src != null)
                uCtrl_Animation_Rotate_Opt.SetImage(rotate_animation.src);
            uCtrl_Animation_Rotate_Opt.numericUpDown_pos_x.Value = rotate_animation.pos_x;
            uCtrl_Animation_Rotate_Opt.numericUpDown_pos_y.Value = rotate_animation.pos_y;
            uCtrl_Animation_Rotate_Opt.numericUpDown_center_x.Value = rotate_animation.center_x;
            uCtrl_Animation_Rotate_Opt.numericUpDown_center_y.Value = rotate_animation.center_y;
            uCtrl_Animation_Rotate_Opt.numericUpDown_start_angle.Value = rotate_animation.start_angle;
            uCtrl_Animation_Rotate_Opt.numericUpDown_end_angle.Value = rotate_animation.end_angle;

            uCtrl_Animation_Rotate_Opt.numericUpDown_fps.Value = rotate_animation.anim_fps;
            uCtrl_Animation_Rotate_Opt.numericUpDown_anim_duration.Value = (decimal)(rotate_animation.anim_duration/1000f);
            uCtrl_Animation_Rotate_Opt.numericUpDown_repeat_count.Value = rotate_animation.repeat_count;

            uCtrl_Animation_Rotate_Opt.checkBox_anim_two_sides.Checked = rotate_animation.anim_two_sides;
            uCtrl_Animation_Rotate_Opt.checkBox_show_in_startPos.Checked = rotate_animation.show_in_start;
            uCtrl_Animation_Rotate_Opt.checkBox_visible.Checked = rotate_animation.visible;

            PreviewView = true;
        }

        private void Read_EditableBackground_Options(Editable_Background editable_background)
        {
            PreviewView = false;
            uCtrl_EditableBackground_Opt.SettingsClear();

            uCtrl_EditableBackground_Opt._EditableBackground = editable_background;
            int index = -1;
            if (editable_background.BackgroundList != null && editable_background.BackgroundList.Count > 0)
            {
                index = editable_background.selected_background;
                uCtrl_EditableBackground_Opt.SetBackgroundCount(editable_background.BackgroundList.Count);
                uCtrl_EditableBackground_Opt.SetBackgroundIndex(index);

                if (index >= 0 && index < editable_background.BackgroundList.Count)
                {
                    uCtrl_EditableBackground_Opt.SetImage(editable_background.BackgroundList[index].path);
                    uCtrl_EditableBackground_Opt.SetPreview(editable_background.BackgroundList[index].preview); 
                }
            }
            uCtrl_EditableBackground_Opt.SetBackgroundIndex(index);
            uCtrl_EditableBackground_Opt.SetForeground(editable_background.fg);

            uCtrl_EditableBackground_Opt.SetTip(editable_background.tips_bg);
            uCtrl_EditableBackground_Opt.numericUpDown_tipX.Value = editable_background.tips_x;
            uCtrl_EditableBackground_Opt.numericUpDown_tipY.Value = editable_background.tips_y;

            uCtrl_EditableBackground_Opt.checkBox_edit_mode.Checked = editable_background.showEeditMode;

            PreviewView = true;
        }

        /// <summary>Читаем настройки для отображения стрелочного указателя</summary>
        private void Read_ImgPointer_Options(hmUI_widget_IMG_POINTER img_pointer, bool _showBackground)
        {
            PreviewView = false;

            uCtrl_Pointer_Opt.SettingsClear();

            uCtrl_Pointer_Opt.ShowBackground = _showBackground;
            uCtrl_Pointer_Opt.Visible = true;

            uCtrl_Pointer_Opt._ElementWithPointer = img_pointer;

            //userCtrl_Background_Options.SettingsClear();

            if (img_pointer == null)
            {
                PreviewView = true;
                return;
            }
            if (img_pointer.src != null)
                uCtrl_Pointer_Opt.SetPointerImage(img_pointer.src);
            uCtrl_Pointer_Opt.numericUpDown_pointer_X.Value = img_pointer.center_x;
            uCtrl_Pointer_Opt.numericUpDown_pointer_Y.Value = img_pointer.center_y;
            uCtrl_Pointer_Opt.numericUpDown_pointer_offset_X.Value = img_pointer.pos_x;
            uCtrl_Pointer_Opt.numericUpDown_pointer_offset_Y.Value = img_pointer.pos_y;

            if (img_pointer.cover_path != null)
                uCtrl_Pointer_Opt.SetPointerImageCentr(img_pointer.cover_path);
            uCtrl_Pointer_Opt.numericUpDown_pointer_centr_X.Value = img_pointer.cover_x;
            uCtrl_Pointer_Opt.numericUpDown_pointer_centr_Y.Value = img_pointer.cover_y;

            if (img_pointer.scale != null)
                uCtrl_Pointer_Opt.SetPointerImageBackground(img_pointer.scale);
            uCtrl_Pointer_Opt.numericUpDown_pointer_background_X.Value = img_pointer.scale_x;
            uCtrl_Pointer_Opt.numericUpDown_pointer_background_Y.Value = img_pointer.scale_y;

            uCtrl_Pointer_Opt.numericUpDown_pointer_startAngle.Value = img_pointer.start_angle;
            uCtrl_Pointer_Opt.numericUpDown_pointer_endAngle.Value = img_pointer.end_angle;

            PreviewView = true;
        }

        /// <summary>Читаем настройки для отображения редактируемых стрелок</summary>
        private void Read_EditablePointers_Options(ElementEditablePointers editablePointers)
        {
            PreviewView = false;

            uCtrl_EditableTimePointer_Opt.SettingsClear();

            uCtrl_EditableTimePointer_Opt.Visible = true;

            uCtrl_EditableTimePointer_Opt._EditableTimePointer = editablePointers;

            //userCtrl_Background_Options.SettingsClear();

            if (editablePointers == null)
            {
                PreviewView = true;
                return;
            }
            if (editablePointers.config == null || editablePointers.config.Count == 0 ||
                editablePointers.selected_pointers >= editablePointers.config.Count)
            {
                PreviewView = true;
                return;
            }
            uCtrl_EditableTimePointer_Opt.SetPointerSetCount(editablePointers.config.Count);
            uCtrl_EditableTimePointer_Opt.SetPointerSetIndex(editablePointers.selected_pointers);
            PointersList pointersList = editablePointers.config[editablePointers.selected_pointers];

            if (pointersList.hour != null) 
            {
                uCtrl_EditableTimePointer_Opt.SetHourPointerImage(pointersList.hour.path);
                uCtrl_EditableTimePointer_Opt.numericUpDown_hourPointer_X.Value = pointersList.hour.centerX;
                uCtrl_EditableTimePointer_Opt.numericUpDown_hourPointer_Y.Value = pointersList.hour.centerY;
                uCtrl_EditableTimePointer_Opt.numericUpDown_hourPointer_offset_X.Value = pointersList.hour.posX;
                uCtrl_EditableTimePointer_Opt.numericUpDown_hourPointer_offset_Y.Value = pointersList.hour.posY;
            }

            if (pointersList.minute != null)
            {
                uCtrl_EditableTimePointer_Opt.SetMinutePointerImage(pointersList.minute.path);
                uCtrl_EditableTimePointer_Opt.numericUpDown_minutePointer_X.Value = pointersList.minute.centerX;
                uCtrl_EditableTimePointer_Opt.numericUpDown_minutePointer_Y.Value = pointersList.minute.centerY;
                uCtrl_EditableTimePointer_Opt.numericUpDown_minutePointer_offset_X.Value = pointersList.minute.posX;
                uCtrl_EditableTimePointer_Opt.numericUpDown_minutePointer_offset_Y.Value = pointersList.minute.posY;
            }

            if (pointersList.second != null)
            {
                uCtrl_EditableTimePointer_Opt.SetSecondPointerImage(pointersList.second.path);
                uCtrl_EditableTimePointer_Opt.numericUpDown_secondPointer_X.Value = pointersList.second.centerX;
                uCtrl_EditableTimePointer_Opt.numericUpDown_secondPointer_Y.Value = pointersList.second.centerY;
                uCtrl_EditableTimePointer_Opt.numericUpDown_secondPointer_offset_X.Value = pointersList.second.posX;
                uCtrl_EditableTimePointer_Opt.numericUpDown_secondPointer_offset_Y.Value = pointersList.second.posY;
            }

            uCtrl_EditableTimePointer_Opt.SetPreview(pointersList.preview);

            uCtrl_EditableTimePointer_Opt.SetForeground(editablePointers.fg);

            uCtrl_EditableTimePointer_Opt.SetTip(editablePointers.tips_bg);
            uCtrl_EditableTimePointer_Opt.numericUpDown_tipX.Value = editablePointers.tips_x;
            uCtrl_EditableTimePointer_Opt.numericUpDown_tipY.Value = editablePointers.tips_y;

            if (editablePointers.cover != null && editablePointers.cover.src != null && editablePointers.cover.src.Length > 0)
            {
                uCtrl_EditableTimePointer_Opt.SetImageCentr(editablePointers.cover.src);
                uCtrl_EditableTimePointer_Opt.numericUpDown_pointer_centr_X.Value = editablePointers.cover.x;
                uCtrl_EditableTimePointer_Opt.numericUpDown_pointer_centr_Y.Value = editablePointers.cover.y; 
            }

            uCtrl_EditableTimePointer_Opt.checkBox_secondInAOD.Checked = editablePointers.AOD_show;
            uCtrl_EditableTimePointer_Opt.checkBox_edit_mode.Checked = editablePointers.showEeditMode;

            PreviewView = true;
        }

        /// <summary>Читаем настройки для отображения редактируемых зон</summary>
        private void Read_EditableElements_Options(EditableElements editableElements)
        {
            PreviewView = false;

            uCtrl_EditableElements_Opt.SettingsClear();
            uCtrl_EditableElements_Opt.Visible = true;
            uCtrl_EditableElements_Opt._EditableElemets = editableElements;


            if (editableElements == null)
            {
                PreviewView = true;
                return;
            }
            if (editableElements.Watchface_edit_group == null || editableElements.Watchface_edit_group.Count == 0 ||
                editableElements.selected_zone >= editableElements.Watchface_edit_group.Count)
            {
                PreviewView = true;
                return;
            }
            uCtrl_EditableElements_Opt.SetZoneCount(editableElements.Watchface_edit_group.Count);
            uCtrl_EditableElements_Opt.SetZoneIndex(editableElements.selected_zone);

            uCtrl_EditableElements_Opt.checkBox_showInAOD.Checked = editableElements.AOD_show;
            uCtrl_EditableElements_Opt.checkBox_edit_mode.Checked = editableElements.showEeditMode;

            uCtrl_EditableElements_Opt.SetMask(editableElements.mask);
            uCtrl_EditableElements_Opt.SetFgMask(editableElements.fg_mask);

            int index = editableElements.selected_zone;
            if (editableElements.Watchface_edit_group != null && index >= 0 && editableElements.Watchface_edit_group.Count > index)
            {
                WATCHFACE_EDIT_GROUP zonesList = editableElements.Watchface_edit_group[index];

                uCtrl_EditableElements_Opt.numericUpDown_zone_X.Value = zonesList.x;
                uCtrl_EditableElements_Opt.numericUpDown_zone_Y.Value = zonesList.y;
                uCtrl_EditableElements_Opt.numericUpDown_zone_H.Value = zonesList.h;
                uCtrl_EditableElements_Opt.numericUpDown_zone_W.Value = zonesList.w;

                uCtrl_EditableElements_Opt.SetTip(zonesList.tips_BG);
                uCtrl_EditableElements_Opt.numericUpDown_tipX.Value = zonesList.tips_x;
                uCtrl_EditableElements_Opt.numericUpDown_tipY.Value = zonesList.tips_y;
                uCtrl_EditableElements_Opt.numericUpDown_tips_width.Value = zonesList.tips_width;
                uCtrl_EditableElements_Opt.numericUpDown_tips_margin.Value = zonesList.tips_margin;

                uCtrl_EditableElements_Opt.SetSelectImage(zonesList.select_image);
                uCtrl_EditableElements_Opt.SetUnSelectImage(zonesList.un_select_image);
            }

            Read_EditableElements_ElementOptions(editableElements);
            PreviewView = true;
        }

        /// <summary>Читаем настройки выбраной зоны для отображения редактируемых зон</summary>
        private void Read_EditableElements_ElementOptions(EditableElements editableElements)
        {
            PreviewView = false;

            if (editableElements == null)
            {
                PreviewView = true;
                return;
            }
            if (editableElements.Watchface_edit_group == null || editableElements.Watchface_edit_group.Count == 0 ||
                editableElements.selected_zone >= editableElements.Watchface_edit_group.Count)
            {
                PreviewView = true;
                return;
            }

            int index = editableElements.selected_zone;
            if (editableElements.Watchface_edit_group != null && index >= 0 && editableElements.Watchface_edit_group.Count > index)
            {
                WATCHFACE_EDIT_GROUP zonesList = editableElements.Watchface_edit_group[index];

                if (zonesList != null && zonesList.Elements != null && zonesList.Elements.Count > 0)
                {
                    List<string> elementName = GetElementsNameList(zonesList.Elements, zonesList.optional_types_list);
                    uCtrl_EditableElements_Opt.SetElementsCount(elementName);
                    uCtrl_EditableElements_Opt.SetElementsIndex(zonesList.selected_element);

                    uCtrl_EditableElements_Opt.numericUpDown_zone_X.Value = zonesList.x;
                    uCtrl_EditableElements_Opt.numericUpDown_zone_Y.Value = zonesList.y;
                    uCtrl_EditableElements_Opt.numericUpDown_zone_H.Value = zonesList.h;
                    uCtrl_EditableElements_Opt.numericUpDown_zone_W.Value = zonesList.w;

                    uCtrl_EditableElements_Opt.SetTip(zonesList.tips_BG);
                    uCtrl_EditableElements_Opt.numericUpDown_tipX.Value = zonesList.tips_x;
                    uCtrl_EditableElements_Opt.numericUpDown_tipY.Value = zonesList.tips_y;
                    uCtrl_EditableElements_Opt.numericUpDown_tips_width.Value = zonesList.tips_width;
                    uCtrl_EditableElements_Opt.numericUpDown_tips_margin.Value = zonesList.tips_margin;

                    uCtrl_EditableElements_Opt.SetSelectImage(zonesList.select_image);
                    uCtrl_EditableElements_Opt.SetUnSelectImage(zonesList.un_select_image);
                }
                else {
                    uCtrl_EditableElements_Opt.SetElementsCount(0);
                    uCtrl_EditableElements_Opt.SetVisibilityOptions(null);
                }
            }

            PreviewView = true;
        }

        /// <summary>Читаем настройки для отображения круговой шкалы</summary>
        private void Read_CircleScale_Options(Circle_Scale circle_scale, bool inverseMode = true)
        {
            PreviewView = false;

            uCtrl_Circle_Scale_Opt.SettingsClear();
            uCtrl_Circle_Scale_Opt.LineCap = SelectedModel.versionOS >= 2;
            uCtrl_Circle_Scale_Opt.Alpha = SelectedModel.versionOS >= 2.1;
            if (!inverseMode) uCtrl_Circle_Scale_Opt.Inverse = false;

            uCtrl_Circle_Scale_Opt.Visible = true;

            uCtrl_Circle_Scale_Opt._CircleScale = circle_scale;

            if (circle_scale == null)
            {
                PreviewView = true;
                return;
            }
            uCtrl_Circle_Scale_Opt.numericUpDown_scaleCircleX.Value = circle_scale.center_x;
            uCtrl_Circle_Scale_Opt.numericUpDown_scaleCircleY.Value = circle_scale.center_y;
            uCtrl_Circle_Scale_Opt.numericUpDown_scaleCircle_radius.Value = circle_scale.radius;
            uCtrl_Circle_Scale_Opt.numericUpDown_scaleCircle_width.Value = circle_scale.line_width;

            uCtrl_Circle_Scale_Opt.numericUpDown_scaleCircle_startAngle.Value = circle_scale.start_angle;
            uCtrl_Circle_Scale_Opt.numericUpDown_scaleCircle_endAngle.Value = circle_scale.end_angle;

            uCtrl_Circle_Scale_Opt.numericUpDown_Alpha.Value = circle_scale.alpha;

            uCtrl_Circle_Scale_Opt.SetLineCap(circle_scale.line_cap);

            uCtrl_Circle_Scale_Opt.SetColorScale(StringToColor(circle_scale.color));

            uCtrl_Circle_Scale_Opt.checkBox_mirror.Checked = circle_scale.mirror;
            uCtrl_Circle_Scale_Opt.checkBox_inversion.Checked = circle_scale.inversion;

            PreviewView = true;
        }

        /// <summary>Читаем настройки для отображения линейной шкалы</summary>
        private void Read_LinearScale_Options(Linear_Scale linear_scale)
        {
            PreviewView = false;

            uCtrl_Linear_Scale_Opt.SettingsClear();

            uCtrl_Linear_Scale_Opt.Visible = true;

            uCtrl_Linear_Scale_Opt._LinearScale = linear_scale;
            uCtrl_Linear_Scale_Opt.Alpha = SelectedModel.versionOS >= 2.1;

            if (linear_scale == null)
            {
                PreviewView = true;
                return;
            }

            if (linear_scale.color != null)
            {
                uCtrl_Linear_Scale_Opt.SetColorScale(StringToColor(linear_scale.color));
            }
            if (linear_scale.pointer != null)
            {
                uCtrl_Linear_Scale_Opt.SetImagePointer(linear_scale.pointer);
            }

            uCtrl_Linear_Scale_Opt.numericUpDown_scaleLinearX.Value = linear_scale.start_x;
            uCtrl_Linear_Scale_Opt.numericUpDown_scaleLinearY.Value = linear_scale.start_y;

            uCtrl_Linear_Scale_Opt.numericUpDown_scaleLinear_length.Value = linear_scale.lenght;
            uCtrl_Linear_Scale_Opt.numericUpDown_scaleLinear_width.Value = linear_scale.line_width;
            uCtrl_Linear_Scale_Opt.numericUpDown_Alpha.Value = linear_scale.alpha;
            uCtrl_Linear_Scale_Opt.SetLineCap(linear_scale.line_cap);

            uCtrl_Linear_Scale_Opt.SetColorScale(StringToColor(linear_scale.color));

            uCtrl_Linear_Scale_Opt.checkBox_mirror.Checked = linear_scale.mirror;
            uCtrl_Linear_Scale_Opt.checkBox_inversion.Checked = linear_scale.inversion;

            uCtrl_Linear_Scale_Opt.radioButton_horizontal.Checked = !linear_scale.vertical;
            uCtrl_Linear_Scale_Opt.radioButton_vertical.Checked = linear_scale.vertical;

            PreviewView = true;
        }

        /// <summary>Читаем настройки для отображения набора картинок</summary>
        private void Read_ImgLevel_Options(hmUI_widget_IMG_LEVEL img_level, int imagesCount, bool imagesCountEnable, bool shortcut = false, bool error_mode = false)
        {
            PreviewView = false;

            uCtrl_Images_Opt.SettingsClear();

            uCtrl_Images_Opt.ImagesCount = imagesCount;
            uCtrl_Images_Opt.ImagesCountEnable = imagesCountEnable;
            uCtrl_Images_Opt.Shortcut = shortcut;
            uCtrl_Images_Opt.ErrorMode = error_mode;
            uCtrl_Images_Opt.Alpha = SelectedModel.versionOS >= 2.1;

            uCtrl_Images_Opt._ElementWithImages = img_level;

            uCtrl_Images_Opt.Visible = true;

            //userCtrl_Background_Options.SettingsClear();

            if (img_level == null)
            {
                PreviewView = true;
                return;
            }
            if (img_level.img_First != null)
                uCtrl_Images_Opt.SetImage(img_level.img_First);
            uCtrl_Images_Opt.numericUpDown_imageX.Value = img_level.X;
            uCtrl_Images_Opt.numericUpDown_imageY.Value = img_level.Y;
            if (img_level.image_length > 0)
                uCtrl_Images_Opt.numericUpDown_pictures_count.Value = img_level.image_length;
            if (!imagesCountEnable) uCtrl_Images_Opt.numericUpDown_pictures_count.Value = imagesCount;
            uCtrl_Images_Opt.numericUpDown_Alpha.Value = img_level.alpha;

            if (error_mode && img_level.img_error != null)
                uCtrl_Images_Opt.SetImageError(img_level.img_error);

            uCtrl_Images_Opt.checkBox_shortcut.Checked = img_level.shortcut;

            PreviewView = true;
        }

        /// <summary>Читаем настройки для отображения картинок сегментами</summary>
        private void Read_ImgProrgess_Options(hmUI_widget_IMG_PROGRESS img_prorgess, int imagesCount, bool fixedRowsCount)
        {
            PreviewView = false;

            uCtrl_Segments_Opt.SettingsClear();

            uCtrl_Segments_Opt.ImagesCount = imagesCount;
            uCtrl_Segments_Opt.FixedRowsCount = fixedRowsCount;
            uCtrl_Segments_Opt.UpdateTable();

            uCtrl_Segments_Opt._ElementWithSegments = img_prorgess;

            uCtrl_Segments_Opt.Visible = true;

            //userCtrl_Background_Options.SettingsClear();

            if (img_prorgess == null)
            {
                PreviewView = true;
                return;
            }
            if (img_prorgess.img_First != null)
                uCtrl_Segments_Opt.SetImage(img_prorgess.img_First);

            uCtrl_Segments_Opt.SetCoordinates(img_prorgess.X, img_prorgess.Y);

            PreviewView = true;
        }

        /// <summary>Читаем настройки для отображения AM PM</summary>
        private void Read_AM_PM_Options(hmUI_widget_IMG_TIME_am_pm am_pm)
        {
            PreviewView = false;

            uCtrl_AmPm_Opt.SettingsClear();

            uCtrl_AmPm_Opt._AmPm = am_pm;

            if (am_pm == null)
            {
                PreviewView = true;
                return;
            }
            if (am_pm.am_img != null)
                uCtrl_AmPm_Opt.Set_AM_Image(am_pm.am_img);
            uCtrl_AmPm_Opt.numericUpDown_AM_X.Value = am_pm.am_x;
            uCtrl_AmPm_Opt.numericUpDown_AM_Y.Value = am_pm.am_y;

            if (am_pm.pm_img != null)
                uCtrl_AmPm_Opt.Set_PM_Image(am_pm.pm_img);
            uCtrl_AmPm_Opt.numericUpDown_PM_X.Value = am_pm.pm_x;
            uCtrl_AmPm_Opt.numericUpDown_PM_Y.Value = am_pm.pm_y;

            PreviewView = true;
        }

        private void Read_Text_Options(hmUI_widget_TEXT system_font, bool unitMode, bool zeroMode, bool unitStrMode = false, bool AmPmMode = false, bool DOWMode = false, bool MonthMode = false)
        {
            PreviewView = false;

            uCtrl_Text_SystemFont_Opt.SettingsClear();
            uCtrl_Text_SystemFont_Opt.Visible = true;
            if (SelectedModel.versionOS >= 2) uCtrl_Text_SystemFont_Opt.UserFont = true;
            //uCtrl_Text_SystemFont_Opt.NumberValue = numberValue;
            uCtrl_Text_SystemFont_Opt.UnitMode = unitMode;
            uCtrl_Text_SystemFont_Opt.ZeroMode = zeroMode;
            uCtrl_Text_SystemFont_Opt.UnitStrMode = unitStrMode;
            uCtrl_Text_SystemFont_Opt.AmPm = AmPmMode;
            uCtrl_Text_SystemFont_Opt.DOWMode = DOWMode;
            uCtrl_Text_SystemFont_Opt.MonthMode = MonthMode;
            uCtrl_Text_SystemFont_Opt.Alpha = SelectedModel.versionOS >= 3.5;

            uCtrl_Text_SystemFont_Opt._ElementWithSystemFont = system_font;
            //uCtrl_Text_SystemFont_Opt.fonts_path = ProjectDir + @"\assets\fonts\";
            //uCtrl_Text_SystemFont_Opt.AddFonts(ProjectDir + @"\assets\fonts\");

            uCtrl_Text_SystemFont_Opt.numericUpDown_X.Value = system_font.x;
            uCtrl_Text_SystemFont_Opt.numericUpDown_Y.Value = system_font.y;
            uCtrl_Text_SystemFont_Opt.numericUpDown_Width.Value = system_font.w;
            uCtrl_Text_SystemFont_Opt.numericUpDown_Height.Value = system_font.h;

            uCtrl_Text_SystemFont_Opt.numericUpDown_Size.Value = system_font.text_size;
            uCtrl_Text_SystemFont_Opt.numericUpDown_Spacing.Value = system_font.char_space;
            uCtrl_Text_SystemFont_Opt.numericUpDown_LineSpace.Value = system_font.line_space;
            uCtrl_Text_SystemFont_Opt.numericUpDown_Alpha.Value = system_font.alpha;

            uCtrl_Text_SystemFont_Opt.SetColorText(StringToColor(system_font.color));
            uCtrl_Text_SystemFont_Opt.SetColor2Text(StringToColor(system_font.color_2));

            uCtrl_Text_SystemFont_Opt.SetHorizontalAlignment(system_font.align_h);
            uCtrl_Text_SystemFont_Opt.SetVerticalAlignment(system_font.align_v);
            uCtrl_Text_SystemFont_Opt.SetTextStyle(system_font.text_style);

            uCtrl_Text_SystemFont_Opt.checkBox_CentreHorizontally.Checked = system_font.centreHorizontally;
            uCtrl_Text_SystemFont_Opt.checkBox_CentreVertically.Checked = system_font.centreVertically;
            uCtrl_Text_SystemFont_Opt.checkBox_Color2.Checked = system_font.use_color_2;

            uCtrl_Text_SystemFont_Opt.SetFont(system_font.font);

            uCtrl_Text_SystemFont_Opt.SetUnitText(system_font.unit_string);

            uCtrl_Text_SystemFont_Opt.checkBox_addZero.Checked = system_font.padding;
            uCtrl_Text_SystemFont_Opt.SetUnitType(system_font.unit_type);
            uCtrl_Text_SystemFont_Opt.SetUnitEnd(system_font.unit_end);

            PreviewView = true;
        }

        /// <summary>Читаем настройки для отображения иконки</summary>
        private void Read_Script_Options(ElementScript script)
        {
            PreviewView = false;

            uCtrl_JS_script_Opt.SettingsClear(ProjectDir);
            uCtrl_JS_script_Opt.AOD = !radioButton_ScreenNormal.Checked;
            uCtrl_JS_script_Opt.Visible = true;
            uCtrl_JS_script_Opt._ElementScript = script;

            uCtrl_JS_script_Opt.checkBox_user_functions.Checked = script.user_functions;
            uCtrl_JS_script_Opt.checkBox_user_script_start.Checked = script.user_script_start;
            uCtrl_JS_script_Opt.checkBox_user_script.Checked = script.user_script;
            uCtrl_JS_script_Opt.checkBox_user_script_beforeShortcuts.Checked = script.user_script_beforeShortcuts;
            uCtrl_JS_script_Opt.checkBox_user_script_end.Checked = script.user_script_end;
            uCtrl_JS_script_Opt.checkBox_resume_call.Checked = script.resume_call;
            uCtrl_JS_script_Opt.checkBox_pause_call.Checked = script.pause_call;

            PreviewView = true;
        }

        /// <summary>Читаем настройки для отображения оповещения об обрыве связи</summary>
        private void Read_DisconnectAlert_Options(DisconnectAlert disconnectAlert)
        {
            PreviewView = false;

            uCtrl_DisconnectAlert_Opt.SettingsClear();

            uCtrl_DisconnectAlert_Opt.Visible = true;

            //userCtrl_Background_Options.SettingsClear();

            if (disconnectAlert == null)
            {
                PreviewView = true;
                return;
            }

            if (disconnectAlert.BluetoothOff != null)
            {
                uCtrl_DisconnectAlert_Opt.checkBox_disconneсt_vibrate.Checked = disconnectAlert.BluetoothOff.vibrate;
                uCtrl_DisconnectAlert_Opt.checkBox_disconneсt_toast.Checked = disconnectAlert.BluetoothOff.toastShow;
                uCtrl_DisconnectAlert_Opt.SetVibratetDisconneсnt(disconnectAlert.BluetoothOff.vibrateType);
                uCtrl_DisconnectAlert_Opt.textBox_disconneсt_toast_text.Text = disconnectAlert.BluetoothOff.toastText; 
            }

            if (disconnectAlert.BluetoothOn != null)
            {
                uCtrl_DisconnectAlert_Opt.checkBox_conneсt_vibrate.Checked = disconnectAlert.BluetoothOn.vibrate;
                uCtrl_DisconnectAlert_Opt.checkBox_conneсt_toast.Checked = disconnectAlert.BluetoothOn.toastShow;
                uCtrl_DisconnectAlert_Opt.SetVibratetConneсnt(disconnectAlert.BluetoothOn.vibrateType);
                uCtrl_DisconnectAlert_Opt.textBox_conneсt_toast_text.Text = disconnectAlert.BluetoothOn.toastText; 
            }

            PreviewView = true;
        }

        /// <summary>Читаем настройки для отображения повторяющихся сигналов</summary>
        private void Read_RepeatingAlert_Options(RepeatAlert repeatingAlert)
        {
            PreviewView = false;

            uCtrl_RepeatingAlert_Opt.SettingsClear();

            uCtrl_RepeatingAlert_Opt.Visible = true;

            //userCtrl_Background_Options.SettingsClear();

            if (repeatingAlert == null)
            {
                PreviewView = true;
                return;
            }

            if (repeatingAlert.EveryHour != null)
            {
                uCtrl_RepeatingAlert_Opt.checkBox_hour_vibrate.Checked = repeatingAlert.EveryHour.vibrate;
                uCtrl_RepeatingAlert_Opt.checkBox_hour_toast.Checked = repeatingAlert.EveryHour.toastShow;
                uCtrl_RepeatingAlert_Opt.SetVibratetHour(repeatingAlert.EveryHour.vibrateType);
                uCtrl_RepeatingAlert_Opt.textBox_hour_toast_text.Text = repeatingAlert.EveryHour.toastText;
            }

            if (repeatingAlert.RepeatingAlert != null)
            {
                uCtrl_RepeatingAlert_Opt.checkBox_repeat_period_vibrate.Checked = repeatingAlert.RepeatingAlert.vibrate;
                uCtrl_RepeatingAlert_Opt.checkBox_repeat_period_toast.Checked = repeatingAlert.RepeatingAlert.toastShow;
                uCtrl_RepeatingAlert_Opt.SetVibratetRepeatPeriod(repeatingAlert.RepeatingAlert.vibrateType);
                uCtrl_RepeatingAlert_Opt.SetVibratetRepeatPeriodTime(repeatingAlert.RepeatingAlert.repeatPeriod);
                uCtrl_RepeatingAlert_Opt.textBox_repeat_period_toast_text.Text = repeatingAlert.RepeatingAlert.toastText;
            }

            PreviewView = true;
        }

        /// <summary>Читаем настройки для отображения плавной секундной стрелки</summary>
        private void Read_Smooth_Second_Options(Smooth_Second smoothSecond)
        {
            PreviewView = false;

            uCtrl_SmoothSeconds_Opt.SettingsClear();
            uCtrl_SmoothSeconds_Opt.Visible = true;

            uCtrl_SmoothSeconds_Opt._SmoothSeconds = smoothSecond;
            if (smoothSecond == null)
            {
                PreviewView = true;
                return;
            }
            switch(smoothSecond.type)
            {
                case 1:
                    uCtrl_SmoothSeconds_Opt.radioButton_type1.Checked = true;
                    break;
                case 2:
                    uCtrl_SmoothSeconds_Opt.radioButton_type2.Checked = true;
                    break;
                case 3:
                    uCtrl_SmoothSeconds_Opt.radioButton_type3.Checked = true;
                    break;
                case 4:
                    uCtrl_SmoothSeconds_Opt.radioButton_type4.Checked = true;
                    break;
            }
            uCtrl_SmoothSeconds_Opt.numericUpDown_fps.Value = smoothSecond.fps;

            PreviewView = true;
        }

        /// <summary>Читаем перечень кнопок</summary>
        private void Read_Button_Options(List<Button> buttonsList)
        {
            PreviewView = false;

            uCtrl_Button_Opt.SettingsClear(SelectedModel.versionOS);
            uCtrl_Button_Opt.Visible = true;

            //List<Button> buttonsList = Watch_Face.Buttons.Button;
            List<String> scriptClickList = ButtonClickScriptToString(buttonsList);
            List<String> scriptLongPressList = ButtonLongPressScriptToString(buttonsList);
            List<bool> visibleList = ButtonVisibleList(buttonsList);
            uCtrl_Button_Opt.UpdateButtonsList(scriptClickList, scriptLongPressList, visibleList);

            PreviewView = true;
        }

        /// <summary>Читаем настройки переключаемого фона</summary>
        private void Read_SwitchBG_Options(ElementSwitchBackground switchBG)
        {
            PreviewView = false;

            uCtrl_Switch_Background_Opt.SettingsClear();
            uCtrl_Switch_Background_Opt.Visible = true;
            uCtrl_Switch_Background_Opt.AddBackgroundImages(switchBG.bg_list, switchBG.toast_list, switchBG.select_index);

            if (switchBG.Button != null)
            {
                uCtrl_Switch_Background_Opt.numericUpDown_buttonX.Value = switchBG.Button.x;
                uCtrl_Switch_Background_Opt.numericUpDown_buttonY.Value = switchBG.Button.y;
                uCtrl_Switch_Background_Opt.numericUpDown_width.Value = switchBG.Button.w;
                uCtrl_Switch_Background_Opt.numericUpDown_height.Value = switchBG.Button.h;
                uCtrl_Switch_Background_Opt.numericUpDown_radius.Value = switchBG.Button.radius;
                uCtrl_Switch_Background_Opt.numericUpDown_textSize.Value = switchBG.Button.text_size;
                uCtrl_Switch_Background_Opt.SetColorText(StringToColor(switchBG.Button.color));
                uCtrl_Switch_Background_Opt.SetText(switchBG.Button.text);
                uCtrl_Switch_Background_Opt.SetPressImage(switchBG.Button.press_src);
                uCtrl_Switch_Background_Opt.SetNormalImage(switchBG.Button.normal_src);
                uCtrl_Switch_Background_Opt.SetColorNormal(StringToColor(switchBG.Button.normal_color));
                uCtrl_Switch_Background_Opt.SetColorPress(StringToColor(switchBG.Button.press_color));

                uCtrl_Switch_Background_Opt.checkBox_use_crown.Checked = switchBG.use_crown;
                uCtrl_Switch_Background_Opt.checkBox_use_in_AOD.Checked = switchBG.use_in_AOD;
                uCtrl_Switch_Background_Opt.checkBox_vibro.Checked = switchBG.vibro;
            }


            PreviewView = true;
        }

        /// <summary>Читаем настройки переключаемого цвета</summary>
        private void Read_SwitchBG_Color_Options(ElementSwitchBG_Color switchBG_Color)
        {
            PreviewView = false;

            uCtrl_Switch_BG_Color_Opt.SettingsClear();
            uCtrl_Switch_BG_Color_Opt.Visible = true;
            List<Color> listColors = switchBG_Color.color_list.Select(StringToColor).ToList();
            uCtrl_Switch_BG_Color_Opt.AddBackgroundColors(listColors, switchBG_Color.toast_list, switchBG_Color.select_index);

            if (switchBG_Color.Button != null)
            {
                uCtrl_Switch_BG_Color_Opt.numericUpDown_buttonX.Value = switchBG_Color.Button.x;
                uCtrl_Switch_BG_Color_Opt.numericUpDown_buttonY.Value = switchBG_Color.Button.y;
                uCtrl_Switch_BG_Color_Opt.numericUpDown_width.Value = switchBG_Color.Button.w;
                uCtrl_Switch_BG_Color_Opt.numericUpDown_height.Value = switchBG_Color.Button.h;
                uCtrl_Switch_BG_Color_Opt.numericUpDown_radius.Value = switchBG_Color.Button.radius;
                uCtrl_Switch_BG_Color_Opt.numericUpDown_textSize.Value = switchBG_Color.Button.text_size;
                uCtrl_Switch_BG_Color_Opt.SetColorText(StringToColor(switchBG_Color.Button.color));
                uCtrl_Switch_BG_Color_Opt.SetText(switchBG_Color.Button.text);
                uCtrl_Switch_BG_Color_Opt.SetPressImage(switchBG_Color.Button.press_src);
                uCtrl_Switch_BG_Color_Opt.SetNormalImage(switchBG_Color.Button.normal_src);
                uCtrl_Switch_BG_Color_Opt.SetColorNormal(StringToColor(switchBG_Color.Button.normal_color));
                uCtrl_Switch_BG_Color_Opt.SetColorPress(StringToColor(switchBG_Color.Button.press_color));

                uCtrl_Switch_BG_Color_Opt.checkBox_use_crown.Checked = switchBG_Color.use_crown;
                uCtrl_Switch_BG_Color_Opt.checkBox_use_in_AOD.Checked = switchBG_Color.use_in_AOD;
                uCtrl_Switch_BG_Color_Opt.checkBox_vibro.Checked = switchBG_Color.vibro;
            }


            PreviewView = true;
        }

        /// <summary>Читаем настройки прогноза погоды</summary>
        private void Read_WeatherFewDay_Options(FewDays fewDays)
        {
            PreviewView = false;

            uCtrl_Weather_FewDays_Opt.SettingsClear();
            uCtrl_Weather_FewDays_Opt.Visible = true;

            uCtrl_Weather_FewDays_Opt._FewDays = fewDays;

            if (fewDays == null)
            {
                PreviewView = true;
                return;
            }

            if (fewDays.Background != null) uCtrl_Weather_FewDays_Opt.SetBackground(fewDays.Background);
            uCtrl_Weather_FewDays_Opt.numericUpDown_posX.Value = fewDays.X;
            uCtrl_Weather_FewDays_Opt.numericUpDown_posY.Value = fewDays.Y;

            uCtrl_Weather_FewDays_Opt.numericUpDown_daysCount.Value = fewDays.DaysCount;
            uCtrl_Weather_FewDays_Opt.numericUpDown_columnWidth.Value = fewDays.ColumnWidth;

            //uCtrl_Weather_FewDays_Opt.checkBox_position_on_graph.Checked = fewDays.PositionOnGraph;

            PreviewView = true;
        }

        /// <summary>Читаем настройки графика прогноза погоды</summary>
        private void Read_WeatherDiagram_Options(Weather_Diagram diagram)
        {
            PreviewView = false;

            uCtrl_TemperatureGraph_Opt.SettingsClear();
            uCtrl_TemperatureGraph_Opt.Visible = true;

            uCtrl_TemperatureGraph_Opt._Diagram = diagram;

            if (diagram == null)
            {
                PreviewView = true;
                return;
            }

            uCtrl_TemperatureGraph_Opt.numericUpDown_posY.Value = diagram.Y; 
            uCtrl_TemperatureGraph_Opt.numericUpDown_height.Value = diagram.Height;
            uCtrl_TemperatureGraph_Opt.checkBox_position_on_graph.Checked = diagram.PositionOnGraph;
            uCtrl_TemperatureGraph_Opt.checkBox_graph_fullScreen.Checked = diagram.GraphFullScreen;

            uCtrl_TemperatureGraph_Opt.SetMaxColor(StringToColor(diagram.Max_color));
            uCtrl_TemperatureGraph_Opt.numericUpDown_max_lineWidth.Value = diagram.Max_lineWidth;
            uCtrl_TemperatureGraph_Opt.SetMaxPointType(diagram.Max_pointType);
            uCtrl_TemperatureGraph_Opt.numericUpDown_max_pointSize.Value = diagram.Max_pointSize;
            uCtrl_TemperatureGraph_Opt.numericUpDown_max_offsetX.Value = diagram.Max_offsetX;

            uCtrl_TemperatureGraph_Opt.SetMinColor(StringToColor(diagram.Min_color));
            uCtrl_TemperatureGraph_Opt.numericUpDown_min_lineWidth.Value = diagram.Min_lineWidth;
            uCtrl_TemperatureGraph_Opt.SetMinPointType(diagram.Min_pointType);
            uCtrl_TemperatureGraph_Opt.numericUpDown_min_pointSize.Value = diagram.Min_pointSize;
            uCtrl_TemperatureGraph_Opt.numericUpDown_min_offsetX.Value = diagram.Min_offsetX;

            uCtrl_TemperatureGraph_Opt.SetAverageColor(StringToColor(diagram.Average_color));
            uCtrl_TemperatureGraph_Opt.numericUpDown_average_lineWidth.Value = diagram.Average_lineWidth;
            uCtrl_TemperatureGraph_Opt.SetAveragePointType(diagram.Average_pointType);
            uCtrl_TemperatureGraph_Opt.numericUpDown_average_pointSize.Value = diagram.Average_pointSize;
            uCtrl_TemperatureGraph_Opt.numericUpDown_average_offsetX.Value = diagram.Average_offsetX;

            uCtrl_TemperatureGraph_Opt.checkBox_use_max.Checked = diagram.Use_max_diagram;
            uCtrl_TemperatureGraph_Opt.checkBox_use_min.Checked = diagram.Use_min_diagram;
            uCtrl_TemperatureGraph_Opt.checkBox_use_average.Checked = diagram.Use_average_diagram;

            PreviewView = true;
        }



        /// <summary>Меняем настройки фона</summary>
        private void userCtrl_Background_Options_ValueChanged(object sender, EventArgs eventArgs)
        {
            if (!PreviewView) return;
            if (Watch_Face == null) return;
            Background background = (Background)userCtrl_Background_Options._Background;
            string backgroundImg = userCtrl_Background_Options.GetBackground();
            string preview = userCtrl_Background_Options.GetPreview();
            if (Watch_Face.WatchFace_Info == null) Watch_Face.WatchFace_Info = new WatchFace_Info();
            if (preview.Length > 0) Watch_Face.WatchFace_Info.Preview = preview;
            else Watch_Face.WatchFace_Info.Preview = null;
            Watch_Face.WatchFace_Info.WatchFaceId = userCtrl_Background_Options.GetID();

            // картинка для фона
            if (userCtrl_Background_Options.radioButton_Background_image.Checked)
            {
                if (background == null)
                {
                    if (radioButton_ScreenNormal.Checked)
                    {
                        if (Watch_Face.ScreenNormal == null) Watch_Face.ScreenNormal = new ScreenNormal();
                        if (Watch_Face.ScreenNormal.Background == null)
                            Watch_Face.ScreenNormal.Background = new Background();
                        background = Watch_Face.ScreenNormal.Background;
                    }
                    else
                    {
                        if (Watch_Face.ScreenAOD == null) Watch_Face.ScreenAOD = new ScreenAOD();
                        if (Watch_Face.ScreenAOD.Background == null)
                            Watch_Face.ScreenAOD.Background = new Background();
                        background = Watch_Face.ScreenAOD.Background;
                    }
                }

                //background = new Background();
                if (background.BackgroundImage == null)
                    background.BackgroundImage = new hmUI_widget_IMG();
                if(backgroundImg.Length > 0) background.BackgroundImage.src = backgroundImg;
                background.BackgroundImage.x = 0;
                background.BackgroundImage.y = 0;
                background.BackgroundImage.h = SelectedModel.background.h;
                background.BackgroundImage.w = SelectedModel.background.w;
                background.BackgroundColor = null;
                uCtrl_Switch_BG_Color_Elm.EnableElement = false;
                if (background.Editable_Background != null) background.Editable_Background.enable_edit_bg = false;

                //uCtrl_Switch_BG_Color_Elm.Visible = false;
                if (radioButton_ScreenIdle.Checked)
                {
                    if (Watch_Face.ScreenNormal.Background != null && Watch_Face.ScreenNormal.Background.Editable_Background != null)
                    {
                        Watch_Face.ScreenNormal.Background.Editable_Background.AOD_show = false;
                    }
                }
                else { 
                    uCtrl_Switch_Background_Elm.EnableElement = true;
                    if (Watch_Face.SwitchBackground != null && Watch_Face.SwitchBackground.bg_list != null &&
                        Watch_Face.SwitchBackground.bg_list.Count > 0) Watch_Face.SwitchBackground.bg_list[0] = backgroundImg;
                }
                uCtrl_EditableBackground_Opt.Visible = false;
            }
            // цвет для фона
            else if (userCtrl_Background_Options.radioButton_Background_color.Checked)
            {
                if (background == null)
                {
                    if (radioButton_ScreenNormal.Checked)
                    {
                        if (Watch_Face.ScreenNormal == null) Watch_Face.ScreenNormal = new ScreenNormal();
                        if (Watch_Face.ScreenNormal.Background == null)
                            Watch_Face.ScreenNormal.Background = new Background();
                        background = Watch_Face.ScreenNormal.Background;
                    }
                    else
                    {
                        if (Watch_Face.ScreenAOD == null) Watch_Face.ScreenAOD = new ScreenAOD();
                        if (Watch_Face.ScreenAOD.Background == null)
                            Watch_Face.ScreenAOD.Background = new Background();
                        background = Watch_Face.ScreenAOD.Background;
                    }
                }

                if (background.BackgroundColor == null) background.BackgroundColor = new hmUI_widget_FILL_RECT();
                background.BackgroundColor.color = ColorToString(userCtrl_Background_Options.GetColorBackground());
                background.BackgroundColor.x = 0;
                background.BackgroundColor.y = 0;
                background.BackgroundColor.h = SelectedModel.background.h;
                background.BackgroundColor.w = SelectedModel.background.w;
                background.BackgroundImage = null;
                if (background.Editable_Background != null) background.Editable_Background.enable_edit_bg = false;
                uCtrl_EditableBackground_Opt.Visible = false;
                uCtrl_Switch_Background_Elm.EnableElement = false;

                //uCtrl_Switch_Background_Elm.Visible = false;
                if (radioButton_ScreenIdle.Checked)
                {
                    if (Watch_Face.ScreenNormal.Background != null && Watch_Face.ScreenNormal.Background.Editable_Background != null)
                    {
                        Watch_Face.ScreenNormal.Background.Editable_Background.AOD_show = false;
                    }
                }
                else
                {
                    uCtrl_Switch_BG_Color_Elm.EnableElement = true;
                    if (Watch_Face.SwitchBG_Color != null && Watch_Face.SwitchBG_Color.color_list != null && 
                        Watch_Face.SwitchBG_Color.color_list.Count > 0) 
                        Watch_Face.SwitchBG_Color.color_list[0] = background.BackgroundColor.color;
                }
            }
            // настраиваемый фон
            else if (userCtrl_Background_Options.radioButton_EditableBackground.Checked)
            {
                if (background == null)
                {
                    if (radioButton_ScreenNormal.Checked)
                    {
                        if (Watch_Face.ScreenNormal == null) Watch_Face.ScreenNormal = new ScreenNormal();
                        if (Watch_Face.ScreenNormal.Background == null)
                            Watch_Face.ScreenNormal.Background = new Background();
                        background = Watch_Face.ScreenNormal.Background;
                    }
                    else
                    {
                        if (Watch_Face.ScreenAOD == null) Watch_Face.ScreenAOD = new ScreenAOD();
                        if (Watch_Face.ScreenAOD.Background == null)
                            Watch_Face.ScreenAOD.Background = new Background();
                        background = Watch_Face.ScreenAOD.Background;
                    }
                }

                if (radioButton_ScreenNormal.Checked)
                {
                    if (background.Editable_Background == null)
                        background.Editable_Background = new Editable_Background();
                    background.Editable_Background.h = SelectedModel.background.h;
                    background.Editable_Background.w = SelectedModel.background.w;

                    background.Editable_Background.enable_edit_bg = true;
                    Read_EditableBackground_Options(background.Editable_Background);
                    uCtrl_EditableBackground_Opt.Visible = true;
                    background.BackgroundImage = null;
                    background.BackgroundColor = null;
                    uCtrl_Switch_Background_Elm.EnableElement = false;
                    uCtrl_Switch_BG_Color_Elm.EnableElement = false;
                    uCtrl_Switch_Background_Elm.Visible = false;
                }
                else
                {
                    if (Watch_Face.ScreenNormal.Background != null)
                    {
                        Watch_Face.ScreenNormal.Background.Editable_Background.AOD_show=true;
                        background.BackgroundImage = null;
                        background.BackgroundColor = null;
                    }
                }
            }
            //background.enable = userCtrl_Background_Options.Visible;

            JSON_Modified = true;
            PreviewImage();
            FormText();

            // отображение кнопок создания картинки предпросмотра
            if (Watch_Face != null && Watch_Face.WatchFace_Info != null && Watch_Face.WatchFace_Info.Preview != null)
            {
                button_RefreshPreview.Visible = true;
                button_CreatePreview.Visible = false;
            }
            else
            {
                button_RefreshPreview.Visible = false;
                if (FileName != null && ProjectDir != null)
                {
                    button_CreatePreview.Visible = true;
                }
                else
                {
                    button_CreatePreview.Visible = false;
                }
            }
        }

        private void uCtrl_Text_Opt_ValueChanged(object sender, EventArgs eventArgs)
        {
            if (!PreviewView) return;
            if (Watch_Face == null) return;
            hmUI_widget_IMG_NUMBER img_number = (hmUI_widget_IMG_NUMBER)uCtrl_Text_Opt._ElementWithText;
            if (img_number == null) return;

            img_number.align = uCtrl_Text_Opt.GetAlignment();
            img_number.follow = uCtrl_Text_Opt.checkBox_follow.Checked;
            img_number.icon = uCtrl_Text_Opt.GetIcon();
            img_number.iconPosX = (int)uCtrl_Text_Opt.numericUpDown_iconX.Value;
            img_number.iconPosY = (int)uCtrl_Text_Opt.numericUpDown_iconY.Value;
            img_number.img_First = uCtrl_Text_Opt.GetImage();
            img_number.imageX = (int)uCtrl_Text_Opt.numericUpDown_imageX.Value;
            img_number.imageY = (int)uCtrl_Text_Opt.numericUpDown_imageY.Value;
            img_number.space = (int)uCtrl_Text_Opt.numericUpDown_spacing.Value;
            img_number.angle = (int)uCtrl_Text_Opt.numericUpDown_angle.Value;
            img_number.alpha = (int)uCtrl_Text_Opt.numericUpDown_Alpha.Value;
            img_number.icon_alpha = (int)uCtrl_Text_Opt.numericUpDown_iconAlpha.Value;
            if (!uCtrl_Text_Opt.Altitude) img_number.dot_image = uCtrl_Text_Opt.GetImageDecimalPoint();
            else img_number.negative_image = uCtrl_Text_Opt.GetImageDecimalPoint();
            img_number.unit = uCtrl_Text_Opt.GetUnit();
            img_number.imperial_unit = uCtrl_Text_Opt.GetUnitMile();
            if (!uCtrl_Text_Opt._Compass) img_number.invalid_image = uCtrl_Text_Opt.GetImageError();
            else img_number.negative_image = uCtrl_Text_Opt.GetImageError();
            img_number.zero = uCtrl_Text_Opt.checkBox_addZero.Checked;


            JSON_Modified = true;
            PreviewImage();
            FormText();
        }

        private void uCtrl_Text_Weather_Opt_ValueChanged(object sender, EventArgs eventArgs)
        {
            if (!PreviewView) return;
            if (Watch_Face == null) return;
            hmUI_widget_IMG_NUMBER img_number = (hmUI_widget_IMG_NUMBER)uCtrl_Text_Weather_Opt._ElementWithText;
            if (img_number == null) return;

            img_number.align = uCtrl_Text_Weather_Opt.GetAlignment();
            img_number.icon = uCtrl_Text_Weather_Opt.GetIcon();
            img_number.iconPosX = (int)uCtrl_Text_Weather_Opt.numericUpDown_iconX.Value;
            img_number.iconPosY = (int)uCtrl_Text_Weather_Opt.numericUpDown_iconY.Value;
            img_number.img_First = uCtrl_Text_Weather_Opt.GetImage();
            img_number.imageX = (int)uCtrl_Text_Weather_Opt.numericUpDown_imageX.Value;
            img_number.imageY = (int)uCtrl_Text_Weather_Opt.numericUpDown_imageY.Value;
            img_number.space = (int)uCtrl_Text_Weather_Opt.numericUpDown_spacing.Value;
            img_number.angle = (int)uCtrl_Text_Weather_Opt.numericUpDown_angle.Value;
            img_number.unit = uCtrl_Text_Weather_Opt.GetUnit_C();
            img_number.imperial_unit = uCtrl_Text_Weather_Opt.GetUnit_F();
            img_number.negative_image = uCtrl_Text_Weather_Opt.GetImageMinus();
            img_number.invalid_image = uCtrl_Text_Weather_Opt.GetImageError();
            img_number.dot_image = uCtrl_Text_Weather_Opt.GetSeparator();

            JSON_Modified = true;
            PreviewImage();
            FormText();
        }

        private void uCtrl_AmPm_Opt_ValueChanged(object sender, EventArgs eventArgs)
        {
            if (!PreviewView) return;
            if (Watch_Face == null) return;
            hmUI_widget_IMG_TIME_am_pm am_pm = (hmUI_widget_IMG_TIME_am_pm)uCtrl_AmPm_Opt._AmPm;
            if (am_pm == null) return;

            am_pm.am_img = uCtrl_AmPm_Opt.Get_AM_Image();
            am_pm.am_x = (int)uCtrl_AmPm_Opt.numericUpDown_AM_X.Value;
            am_pm.am_y = (int)uCtrl_AmPm_Opt.numericUpDown_AM_Y.Value;

            am_pm.pm_img = uCtrl_AmPm_Opt.Get_PM_Image();
            am_pm.pm_x = (int)uCtrl_AmPm_Opt.numericUpDown_PM_X.Value;
            am_pm.pm_y = (int)uCtrl_AmPm_Opt.numericUpDown_PM_Y.Value;

            JSON_Modified = true;
            PreviewImage();
            FormText();
        }

        private void uCtrl_Pointer_Opt_ValueChanged(object sender, EventArgs eventArgs)
        {
            if (!PreviewView) return;
            if (Watch_Face == null) return;
            hmUI_widget_IMG_POINTER img_pointer = (hmUI_widget_IMG_POINTER)uCtrl_Pointer_Opt._ElementWithPointer;
            if (img_pointer == null) return;

            img_pointer.src = uCtrl_Pointer_Opt.GetPointerImage();
            img_pointer.center_x = (int)uCtrl_Pointer_Opt.numericUpDown_pointer_X.Value;
            img_pointer.center_y = (int)uCtrl_Pointer_Opt.numericUpDown_pointer_Y.Value;

            img_pointer.pos_x = (int)uCtrl_Pointer_Opt.numericUpDown_pointer_offset_X.Value;
            img_pointer.pos_y = (int)uCtrl_Pointer_Opt.numericUpDown_pointer_offset_Y.Value;

            img_pointer.start_angle = (int)uCtrl_Pointer_Opt.numericUpDown_pointer_startAngle.Value;
            img_pointer.end_angle = (int)uCtrl_Pointer_Opt.numericUpDown_pointer_endAngle.Value;

            img_pointer.cover_path = uCtrl_Pointer_Opt.GetPointerImageCentr();
            img_pointer.cover_x = (int)uCtrl_Pointer_Opt.numericUpDown_pointer_centr_X.Value;
            img_pointer.cover_y = (int)uCtrl_Pointer_Opt.numericUpDown_pointer_centr_Y.Value;

            img_pointer.scale = uCtrl_Pointer_Opt.GetPointerImageBackground();
            img_pointer.scale_x = (int)uCtrl_Pointer_Opt.numericUpDown_pointer_background_X.Value;
            img_pointer.scale_y = (int)uCtrl_Pointer_Opt.numericUpDown_pointer_background_Y.Value;


            JSON_Modified = true;
            PreviewImage();
            FormText();
        }

        private void uCtrl_Images_Opt_ValueChanged(object sender, EventArgs eventArgs)
        {
            if (!PreviewView) return;
            if (Watch_Face == null) return;
            hmUI_widget_IMG_LEVEL img_level = (hmUI_widget_IMG_LEVEL)uCtrl_Images_Opt._ElementWithImages;
            if (img_level == null) return;

            img_level.img_First = uCtrl_Images_Opt.GetImage();
            img_level.X = (int)uCtrl_Images_Opt.numericUpDown_imageX.Value;
            img_level.Y = (int)uCtrl_Images_Opt.numericUpDown_imageY.Value;
            img_level.alpha = (int)uCtrl_Images_Opt.numericUpDown_Alpha.Value;
            img_level.image_length = (int)uCtrl_Images_Opt.numericUpDown_pictures_count.Value;
            img_level.shortcut = uCtrl_Images_Opt.checkBox_shortcut.Checked;
            img_level.img_error = uCtrl_Images_Opt.GetImageError();

            JSON_Modified = true;
            PreviewImage();
            FormText();
        }

        private void uCtrl_Segments_Opt_ValueChanged(object sender, EventArgs eventArgs)
        {
            if (!PreviewView) return;
            if (Watch_Face == null) return;
            hmUI_widget_IMG_PROGRESS img_progress = (hmUI_widget_IMG_PROGRESS)uCtrl_Segments_Opt._ElementWithSegments;
            if (img_progress == null) return;

            img_progress.img_First = uCtrl_Segments_Opt.GetImage();

            List<int> coordinatesX = new List<int>();
            List<int> coordinatesY = new List<int>();
            uCtrl_Segments_Opt.GetCoordinates(out coordinatesX, out coordinatesY);
            img_progress.X = coordinatesX;
            img_progress.Y = coordinatesY;
            int image_length = 0;
            if (coordinatesX != null) image_length = coordinatesX.Count;
            img_progress.image_length = image_length;

            JSON_Modified = true;
            PreviewImage();
            FormText();
        }

        private void uCtrl_Circle_Scale_Opt_ValueChanged(object sender, EventArgs eventArgs)
        {
            if (!PreviewView) return;
            if (Watch_Face == null) return;
            Circle_Scale circle_scale = (Circle_Scale)uCtrl_Circle_Scale_Opt._CircleScale;
            if (circle_scale == null) return;

            circle_scale.center_x = (int)uCtrl_Circle_Scale_Opt.numericUpDown_scaleCircleX.Value;
            circle_scale.center_y = (int)uCtrl_Circle_Scale_Opt.numericUpDown_scaleCircleY.Value;

            circle_scale.radius = (int)(int)Math.Abs(uCtrl_Circle_Scale_Opt.numericUpDown_scaleCircle_radius.Value);
            circle_scale.line_width = (int)(int)Math.Abs(uCtrl_Circle_Scale_Opt.numericUpDown_scaleCircle_width.Value);

            circle_scale.start_angle = (int)uCtrl_Circle_Scale_Opt.numericUpDown_scaleCircle_startAngle.Value;
            circle_scale.end_angle = (int)uCtrl_Circle_Scale_Opt.numericUpDown_scaleCircle_endAngle.Value;

            circle_scale.alpha = (int)uCtrl_Circle_Scale_Opt.numericUpDown_Alpha.Value;

            circle_scale.line_cap = uCtrl_Circle_Scale_Opt.GetLineCap();

            circle_scale.color = ColorToString(uCtrl_Circle_Scale_Opt.GetColorScale());

            circle_scale.mirror = uCtrl_Circle_Scale_Opt.checkBox_mirror.Checked;
            circle_scale.inversion = uCtrl_Circle_Scale_Opt.checkBox_inversion.Checked;

            JSON_Modified = true;
            PreviewImage();
            FormText();
        }

        private void uCtrl_Linear_Scale_Opt_ValueChanged(object sender, EventArgs eventArgs)
        {
            if (!PreviewView) return;
            if (Watch_Face == null) return;
            Linear_Scale linear_scale = (Linear_Scale)uCtrl_Linear_Scale_Opt._LinearScale;
            if (linear_scale == null) return;

            linear_scale.pointer = uCtrl_Linear_Scale_Opt.GetImagePointer();
            linear_scale.color = ColorToString(uCtrl_Linear_Scale_Opt.GetColorScale());

            linear_scale.start_x = (int)uCtrl_Linear_Scale_Opt.numericUpDown_scaleLinearX.Value;
            linear_scale.start_y = (int)uCtrl_Linear_Scale_Opt.numericUpDown_scaleLinearY.Value;

            linear_scale.lenght = (int)uCtrl_Linear_Scale_Opt.numericUpDown_scaleLinear_length.Value;
            linear_scale.line_width = (int)Math.Abs(uCtrl_Linear_Scale_Opt.numericUpDown_scaleLinear_width.Value);
            linear_scale.alpha = (int)uCtrl_Linear_Scale_Opt.numericUpDown_Alpha.Value;
            linear_scale.line_cap = uCtrl_Linear_Scale_Opt.GetLineCap();

            linear_scale.vertical = uCtrl_Linear_Scale_Opt.radioButton_vertical.Checked;

            linear_scale.mirror = uCtrl_Linear_Scale_Opt.checkBox_mirror.Checked;
            linear_scale.inversion = uCtrl_Linear_Scale_Opt.checkBox_inversion.Checked;

            JSON_Modified = true;
            PreviewImage();
            FormText();
        }

        private void uCtrl_Icon_Opt_ValueChanged(object sender, EventArgs eventArgs)
        {
            if (!PreviewView) return;
            if (Watch_Face == null) return;
            string type = uCtrl_Icon_Opt._Icon.GetType().Name;
            if (type == "hmUI_widget_IMG")
            {
                hmUI_widget_IMG icon = (hmUI_widget_IMG)uCtrl_Icon_Opt._Icon;
                if (icon == null) return;

                icon.src = uCtrl_Icon_Opt.GetIcon();
                icon.x = (int)uCtrl_Icon_Opt.numericUpDown_iconX.Value;
                icon.y = (int)uCtrl_Icon_Opt.numericUpDown_iconY.Value;
                icon.alpha = (int)uCtrl_Icon_Opt.numericUpDown_Alpha.Value;
            }
            else if (type == "hmUI_widget_IMG_STATUS")
            {
                hmUI_widget_IMG_STATUS status = (hmUI_widget_IMG_STATUS)uCtrl_Icon_Opt._Icon;
                if (status == null) return;

                status.src = uCtrl_Icon_Opt.GetIcon();
                status.x = (int)uCtrl_Icon_Opt.numericUpDown_iconX.Value;
                status.y = (int)uCtrl_Icon_Opt.numericUpDown_iconY.Value;
                status.alpha = (int)uCtrl_Icon_Opt.numericUpDown_Alpha.Value;
            }

            JSON_Modified = true;
            PreviewImage();
            FormText();
        }

        private void uCtrl_Shortcut_Opt_ValueChanged(object sender, EventArgs eventArgs)
        {
            if (!PreviewView) return;
            if (Watch_Face == null) return;
            hmUI_widget_IMG_CLICK shortcut = (hmUI_widget_IMG_CLICK)uCtrl_Shortcut_Opt._Shortcut;
            if (shortcut == null) return;

            shortcut.src = uCtrl_Shortcut_Opt.GetImage();
            shortcut.x = (int)uCtrl_Shortcut_Opt.numericUpDown_imageX.Value;
            shortcut.y = (int)uCtrl_Shortcut_Opt.numericUpDown_imageY.Value;
            shortcut.h = (int)uCtrl_Shortcut_Opt.numericUpDown_height.Value;
            shortcut.w = (int)uCtrl_Shortcut_Opt.numericUpDown_width.Value;

            JSON_Modified = true;
            PreviewImage();
            FormText();
        }

        private void uCtrl_Text_SystemFont_Opt_ValueChanged(object sender, EventArgs eventArgs)
        {
            if (!PreviewView) return;
            if (Watch_Face == null) return;
            hmUI_widget_TEXT systemFont = (hmUI_widget_TEXT)uCtrl_Text_SystemFont_Opt._ElementWithSystemFont;
            if (systemFont == null) return;

            systemFont.x = (int)uCtrl_Text_SystemFont_Opt.numericUpDown_X.Value;
            systemFont.y = (int)uCtrl_Text_SystemFont_Opt.numericUpDown_Y.Value;
            systemFont.h = (int)uCtrl_Text_SystemFont_Opt.numericUpDown_Height.Value;
            systemFont.w = (int)uCtrl_Text_SystemFont_Opt.numericUpDown_Width.Value;

            systemFont.text_size = (int)uCtrl_Text_SystemFont_Opt.numericUpDown_Size.Value;
            systemFont.char_space = (int)uCtrl_Text_SystemFont_Opt.numericUpDown_Spacing.Value;
            systemFont.line_space = (int)uCtrl_Text_SystemFont_Opt.numericUpDown_LineSpace.Value;
            systemFont.alpha = (int)uCtrl_Text_SystemFont_Opt.numericUpDown_Alpha.Value;

            systemFont.color = ColorToString(uCtrl_Text_SystemFont_Opt.GetColorText());
            systemFont.color_2 = ColorToString(uCtrl_Text_SystemFont_Opt.GetColor2Text());

            systemFont.align_h = uCtrl_Text_SystemFont_Opt.GetHorizontalAlignment();
            systemFont.align_v = uCtrl_Text_SystemFont_Opt.GetVerticalAlignment();
            systemFont.text_style = uCtrl_Text_SystemFont_Opt.GetTextStyle();

            systemFont.centreHorizontally = uCtrl_Text_SystemFont_Opt.checkBox_CentreHorizontally.Checked;
            systemFont.centreVertically = uCtrl_Text_SystemFont_Opt.checkBox_CentreVertically.Checked;
            systemFont.use_color_2 = uCtrl_Text_SystemFont_Opt.checkBox_Color2.Checked;

            systemFont.font = uCtrl_Text_SystemFont_Opt.GetFont();
            systemFont.unit_string = uCtrl_Text_SystemFont_Opt.GetUnitText();

            systemFont.padding = uCtrl_Text_SystemFont_Opt.checkBox_addZero.Checked;
            systemFont.unit_type = uCtrl_Text_SystemFont_Opt.GetUnitType();
            systemFont.unit_end = uCtrl_Text_SystemFont_Opt.GetUnitEnd();

            JSON_Modified = true;
            PreviewImage();
            FormText();
        }

        private void uCtrl_Animation_Frame_Opt_ValueChanged(object sender, EventArgs eventArgs, int index)
        {
            if (!PreviewView) return;
            if (Watch_Face == null) return;
            if (index < 0) return;
            hmUI_widget_IMG_ANIM_List animation_frame = (hmUI_widget_IMG_ANIM_List)uCtrl_Animation_Frame_Opt._AnimationFrame;
            if (animation_frame == null) return;

            if (animation_frame.Frame_Animation == null) animation_frame.Frame_Animation = new List<hmUI_widget_IMG_ANIM>();
            List<hmUI_widget_IMG_ANIM> frameAnimation = animation_frame.Frame_Animation;
            if (frameAnimation == null) return;
            if (frameAnimation.Count < index + 1) return;

            hmUI_widget_IMG_ANIM img_anim = new hmUI_widget_IMG_ANIM();

            img_anim.visible = uCtrl_Animation_Frame_Opt.checkBox_visible.Checked;
            img_anim.x = (int)uCtrl_Animation_Frame_Opt.numericUpDown_imageX.Value;
            img_anim.y = (int)uCtrl_Animation_Frame_Opt.numericUpDown_imageY.Value;

            img_anim.anim_src = uCtrl_Animation_Frame_Opt.GetImage();
            img_anim.anim_prefix = uCtrl_Animation_Frame_Opt.GetPrefix();
            img_anim.anim_fps = (int)uCtrl_Animation_Frame_Opt.numericUpDown_fps.Value;
            img_anim.anim_size = (int)uCtrl_Animation_Frame_Opt.numericUpDown_images_count.Value;
            img_anim.preview_frame = (int)uCtrl_Animation_Frame_Opt.numericUpDown_preview_frame.Value;
            img_anim.anim_repeat = uCtrl_Animation_Frame_Opt.checkBox_anim_repeat.Checked;

            //rotate_anim.display_on_restart = uCtrl_Animation_Frame_Opt.checkBox_anim_restart.Checked;

            if (index + 1 <= frameAnimation.Count) frameAnimation[index] = img_anim;

            JSON_Modified = true;
            PreviewImage();
            FormText();
        }

        private void uCtrl_Animation_Motion_Opt_ValueChanged(object sender, EventArgs eventArgs, int index)
        {
            if (!PreviewView) return;
            if (Watch_Face == null) return;
            if (index < 0) return;
            Motion_Animation_List animation_motion = (Motion_Animation_List)uCtrl_Animation_Motion_Opt._AnimationMotion;
            if (animation_motion == null) return;

            if (animation_motion.Motion_Animation == null) animation_motion.Motion_Animation = new List<Motion_Animation>();
            List<Motion_Animation> motionAnimation = animation_motion.Motion_Animation;
            if (motionAnimation == null) return;
            if (motionAnimation.Count < index + 1) return;

            Motion_Animation motion_anim = new Motion_Animation();

            motion_anim.visible = uCtrl_Animation_Motion_Opt.checkBox_visible.Checked;
            motion_anim.x_start = (int)uCtrl_Animation_Motion_Opt.numericUpDown_start_x.Value;
            motion_anim.y_start = (int)uCtrl_Animation_Motion_Opt.numericUpDown_start_y.Value;
            motion_anim.x_end = (int)uCtrl_Animation_Motion_Opt.numericUpDown_end_x.Value;
            motion_anim.y_end = (int)uCtrl_Animation_Motion_Opt.numericUpDown_end_y.Value;

            motion_anim.src = uCtrl_Animation_Motion_Opt.GetImage();
            motion_anim.anim_fps = (int)uCtrl_Animation_Motion_Opt.numericUpDown_fps.Value;
            motion_anim.anim_duration = (int)(uCtrl_Animation_Motion_Opt.numericUpDown_anim_duration.Value * 1000);
            motion_anim.repeat_count = (int)uCtrl_Animation_Motion_Opt.numericUpDown_repeat_count.Value;

            motion_anim.anim_two_sides = uCtrl_Animation_Motion_Opt.checkBox_anim_two_sides.Checked;
            motion_anim.show_in_start = uCtrl_Animation_Motion_Opt.checkBox_show_in_startPos.Checked;

            if (index + 1 <= motionAnimation.Count) motionAnimation[index] = motion_anim;

            JSON_Modified = true;
            PreviewImage();
            FormText();
        }

        private void uCtrl_Animation_Rotate_Opt_ValueChanged(object sender, EventArgs eventArgs, int index)
        {
            if (!PreviewView) return;
            if (Watch_Face == null) return;
            if (index < 0) return;
            Rotate_Animation_List animation_rotate = (Rotate_Animation_List)uCtrl_Animation_Rotate_Opt._AnimationRotate;
            if (animation_rotate == null) return;

            if (animation_rotate.Rotate_Animation == null) animation_rotate.Rotate_Animation = new List<Rotate_Animation>();
            List<Rotate_Animation> motionAnimation = animation_rotate.Rotate_Animation;
            if (motionAnimation == null) return;
            if (motionAnimation.Count < index + 1) return;

            Rotate_Animation rotate_anim = new Rotate_Animation();

            rotate_anim.visible = uCtrl_Animation_Rotate_Opt.checkBox_visible.Checked;
            rotate_anim.center_x = (int)uCtrl_Animation_Rotate_Opt.numericUpDown_center_x.Value;
            rotate_anim.center_y = (int)uCtrl_Animation_Rotate_Opt.numericUpDown_center_y.Value;
            rotate_anim.pos_x = (int)uCtrl_Animation_Rotate_Opt.numericUpDown_pos_x.Value;
            rotate_anim.pos_y = (int)uCtrl_Animation_Rotate_Opt.numericUpDown_pos_y.Value;

            rotate_anim.start_angle = (int)uCtrl_Animation_Rotate_Opt.numericUpDown_start_angle.Value;
            rotate_anim.end_angle = (int)uCtrl_Animation_Rotate_Opt.numericUpDown_end_angle.Value;

            rotate_anim.src = uCtrl_Animation_Rotate_Opt.GetImage();
            rotate_anim.anim_fps = (int)uCtrl_Animation_Rotate_Opt.numericUpDown_fps.Value;
            rotate_anim.anim_duration = (int)(uCtrl_Animation_Rotate_Opt.numericUpDown_anim_duration.Value*1000);
            rotate_anim.repeat_count = (int)uCtrl_Animation_Rotate_Opt.numericUpDown_repeat_count.Value;

            rotate_anim.anim_two_sides = uCtrl_Animation_Rotate_Opt.checkBox_anim_two_sides.Checked;
            rotate_anim.show_in_start = uCtrl_Animation_Rotate_Opt.checkBox_show_in_startPos.Checked;

            if (index + 1 <= motionAnimation.Count) motionAnimation[index] = rotate_anim;

            JSON_Modified = true;
            PreviewImage();
            FormText();
        }

        private void uCtrl_Animation_Frame_Opt_AnimationAdd(object sender, EventArgs eventArgs, int index)
        {
            if (!PreviewView) return;
            if (Watch_Face == null) return;
            //if (index < 0) return;
            hmUI_widget_IMG_ANIM_List animation_frame = (hmUI_widget_IMG_ANIM_List)uCtrl_Animation_Frame_Opt._AnimationFrame;
            if (animation_frame == null) return;

            if (animation_frame.Frame_Animation == null) animation_frame.Frame_Animation = new List<hmUI_widget_IMG_ANIM>();
            List<hmUI_widget_IMG_ANIM> frameAnimation = animation_frame.Frame_Animation;

            hmUI_widget_IMG_ANIM img_anim = new hmUI_widget_IMG_ANIM();
            img_anim.x = (int)uCtrl_Animation_Frame_Opt.numericUpDown_imageX.Value;
            img_anim.y = (int)uCtrl_Animation_Frame_Opt.numericUpDown_imageY.Value;

            img_anim.anim_src = uCtrl_Animation_Frame_Opt.GetImage();
            img_anim.anim_prefix = uCtrl_Animation_Frame_Opt.GetPrefix();
            img_anim.anim_fps = (int)uCtrl_Animation_Frame_Opt.numericUpDown_fps.Value;
            img_anim.anim_size = (int)uCtrl_Animation_Frame_Opt.numericUpDown_images_count.Value;
            img_anim.anim_repeat = uCtrl_Animation_Frame_Opt.checkBox_anim_repeat.Checked;
            img_anim.visible = true;

            //rotate_anim.display_on_restart = uCtrl_Animation_Frame_Opt.checkBox_anim_restart.Checked;

            if (frameAnimation.Count > index) frameAnimation.Add(img_anim);
            else frameAnimation.Insert(index, img_anim);
            animation_frame.selected_animation = ++index;
            //uCtrl_Animation_Frame_Opt.SetAnimationIndex(index);
            uCtrl_Animation_Elm_SelectChanged(sender, eventArgs);

            JSON_Modified = true;
            PreviewImage();
            FormText();
        }

        private void uCtrl_Animation_Motion_Opt_AnimationAdd(object sender, EventArgs eventArgs, int index)
        {
            if (!PreviewView) return;
            if (Watch_Face == null) return;
            //if (index < 0) return;
            Motion_Animation_List animation_motion = (Motion_Animation_List)uCtrl_Animation_Motion_Opt._AnimationMotion;
            if (animation_motion == null) return;

            if (animation_motion.Motion_Animation == null) animation_motion.Motion_Animation = new List<Motion_Animation>();
            List<Motion_Animation> motionAnimation = animation_motion.Motion_Animation;

            Motion_Animation motion_anim = new Motion_Animation();
            motion_anim.x_start = (int)uCtrl_Animation_Motion_Opt.numericUpDown_start_x.Value;
            motion_anim.x_end = (int)uCtrl_Animation_Motion_Opt.numericUpDown_end_x.Value;
            motion_anim.y_start = (int)uCtrl_Animation_Motion_Opt.numericUpDown_start_y.Value;
            motion_anim.y_end = (int)uCtrl_Animation_Motion_Opt.numericUpDown_end_y.Value;


            motion_anim.src = uCtrl_Animation_Motion_Opt.GetImage();
            motion_anim.anim_fps = (int)uCtrl_Animation_Motion_Opt.numericUpDown_fps.Value;
            motion_anim.anim_duration = (int)(uCtrl_Animation_Motion_Opt.numericUpDown_anim_duration.Value*1000);
            motion_anim.repeat_count = (int)uCtrl_Animation_Motion_Opt.numericUpDown_repeat_count.Value;

            motion_anim.anim_two_sides = uCtrl_Animation_Motion_Opt.checkBox_anim_two_sides.Checked;
            motion_anim.show_in_start = uCtrl_Animation_Motion_Opt.checkBox_show_in_startPos.Checked;
            motion_anim.visible = true;

            if (motionAnimation.Count > index) motionAnimation.Add(motion_anim);
            else motionAnimation.Insert(index, motion_anim);
            animation_motion.selected_animation = ++index;
            //uCtrl_Animation_Motion_Opt.SetAnimationIndex(index);
            uCtrl_Animation_Elm_SelectChanged(sender, eventArgs);

            JSON_Modified = true;
            PreviewImage();
            FormText();
        }

        private void uCtrl_Animation_Rotate_Opt_AnimationAdd(object sender, EventArgs eventArgs, int index)
        {
            if (!PreviewView) return;
            if (Watch_Face == null) return;
            //if (index < 0) return;
            Rotate_Animation_List animation_rotate = (Rotate_Animation_List)uCtrl_Animation_Rotate_Opt._AnimationRotate;
            if (animation_rotate == null) return;

            if (animation_rotate.Rotate_Animation == null) animation_rotate.Rotate_Animation = new List<Rotate_Animation>();
            List<Rotate_Animation> frameAnimation = animation_rotate.Rotate_Animation;

            Rotate_Animation rotate_anim = new Rotate_Animation();
            rotate_anim.center_x = (int)uCtrl_Animation_Rotate_Opt.numericUpDown_center_x.Value;
            rotate_anim.center_y = (int)uCtrl_Animation_Rotate_Opt.numericUpDown_center_y.Value;
            rotate_anim.pos_x = (int)uCtrl_Animation_Rotate_Opt.numericUpDown_pos_x.Value;
            rotate_anim.pos_y = (int)uCtrl_Animation_Rotate_Opt.numericUpDown_pos_y.Value;

            rotate_anim.src = uCtrl_Animation_Rotate_Opt.GetImage();

            rotate_anim.start_angle = (int)uCtrl_Animation_Rotate_Opt.numericUpDown_start_angle.Value;
            rotate_anim.end_angle = (int)uCtrl_Animation_Rotate_Opt.numericUpDown_end_angle.Value;

            rotate_anim.anim_fps = (int)uCtrl_Animation_Rotate_Opt.numericUpDown_fps.Value;
            rotate_anim.anim_duration = (int)(uCtrl_Animation_Rotate_Opt.numericUpDown_anim_duration.Value * 1000);
            rotate_anim.repeat_count = (int)uCtrl_Animation_Rotate_Opt.numericUpDown_repeat_count.Value;

            rotate_anim.anim_two_sides = uCtrl_Animation_Rotate_Opt.checkBox_anim_two_sides.Checked;
            rotate_anim.show_in_start = uCtrl_Animation_Rotate_Opt.checkBox_show_in_startPos.Checked;
            rotate_anim.visible = true;

            if (frameAnimation.Count > index) frameAnimation.Add(rotate_anim);
            else frameAnimation.Insert(index, rotate_anim);
            animation_rotate.selected_animation = ++index;
            //uCtrl_Animation_Rotate_Opt.SetAnimationIndex(index);
            uCtrl_Animation_Elm_SelectChanged(sender, eventArgs);

            JSON_Modified = true;
            PreviewImage();
            FormText();
        }

        private void uCtrl_Animation_Frame_Opt_AnimationDel(object sender, EventArgs eventArgs, int index)
        {
            if (!PreviewView) return;
            if (Watch_Face == null) return;
            //if (index < 0) return;
            hmUI_widget_IMG_ANIM_List animation_frame = (hmUI_widget_IMG_ANIM_List)uCtrl_Animation_Frame_Opt._AnimationFrame;
            if (animation_frame == null) return;

            if (animation_frame.Frame_Animation == null) animation_frame.Frame_Animation = new List<hmUI_widget_IMG_ANIM>();
            List<hmUI_widget_IMG_ANIM> frameAnimation = animation_frame.Frame_Animation;

            if (frameAnimation.Count > index) frameAnimation.RemoveAt(index);
            animation_frame.selected_animation = --index;
            if (index < 0 && animation_frame.Frame_Animation != null && animation_frame.Frame_Animation.Count > 0)
                animation_frame.selected_animation = 0;
            uCtrl_Animation_Elm_SelectChanged(sender, eventArgs);

            JSON_Modified = true;
            PreviewImage();
            FormText();
        }

        private void uCtrl_Animation_Motion_Opt_AnimationDel(object sender, EventArgs eventArgs, int index)
        {
            if (!PreviewView) return;
            if (Watch_Face == null) return;
            //if (index < 0) return;
            Motion_Animation_List animation_motion = (Motion_Animation_List)uCtrl_Animation_Motion_Opt._AnimationMotion;
            if (animation_motion == null) return;

            if (animation_motion.Motion_Animation == null) animation_motion.Motion_Animation = new List<Motion_Animation>();
            List<Motion_Animation> motionAnimation = animation_motion.Motion_Animation;

            if (motionAnimation.Count > index) motionAnimation.RemoveAt(index);
            animation_motion.selected_animation = --index;
            if (index < 0 && animation_motion.Motion_Animation != null && animation_motion.Motion_Animation.Count > 0)
                animation_motion.selected_animation = 0;
            uCtrl_Animation_Elm_SelectChanged(sender, eventArgs);

            JSON_Modified = true;
            PreviewImage();
            FormText();
        }

        private void uCtrl_Animation_Rotate_Opt_AnimationDel(object sender, EventArgs eventArgs, int index)
        {
            if (!PreviewView) return;
            if (Watch_Face == null) return;
            //if (index < 0) return;
            Rotate_Animation_List animation_rotate = (Rotate_Animation_List)uCtrl_Animation_Rotate_Opt._AnimationRotate;
            if (animation_rotate == null) return;

            if (animation_rotate.Rotate_Animation == null) animation_rotate.Rotate_Animation = new List<Rotate_Animation>();
            List<Rotate_Animation> rotateAnimation = animation_rotate.Rotate_Animation;

            if (rotateAnimation.Count > index) rotateAnimation.RemoveAt(index);
            animation_rotate.selected_animation = --index;
            if (index < 0 && animation_rotate.Rotate_Animation != null && animation_rotate.Rotate_Animation.Count > 0)
                animation_rotate.selected_animation = 0;
            uCtrl_Animation_Elm_SelectChanged(sender, eventArgs);

            JSON_Modified = true;
            PreviewImage();
            FormText();
        }

        private void uCtrl_Animation_Frame_Opt_AnimIndexChanged(object sender, EventArgs eventArgs, int index)
        {
            if (!PreviewView) return;
            if (Watch_Face == null) return;
            //if (index < 0) return;
            hmUI_widget_IMG_ANIM_List animation_frame = (hmUI_widget_IMG_ANIM_List)uCtrl_Animation_Frame_Opt._AnimationFrame;
            if (animation_frame == null) return;
            animation_frame.selected_animation = index;

            uCtrl_Animation_Elm_SelectChanged(sender, eventArgs);
        }

        private void uCtrl_Animation_Motion_Opt_AnimIndexChanged(object sender, EventArgs eventArgs, int index)
        {
            if (!PreviewView) return;
            if (Watch_Face == null) return;
            //if (index < 0) return;
            Motion_Animation_List animation_motion = (Motion_Animation_List)uCtrl_Animation_Motion_Opt._AnimationMotion;
            if (animation_motion == null) return;
            animation_motion.selected_animation = index;

            uCtrl_Animation_Elm_SelectChanged(sender, eventArgs);
        }

        private void uCtrl_Animation_Rotate_Opt_AnimIndexChanged(object sender, EventArgs eventArgs, int index)
        {
            if (!PreviewView) return;
            if (Watch_Face == null) return;
            //if (index < 0) return;
            Rotate_Animation_List animation_rotate = (Rotate_Animation_List)uCtrl_Animation_Rotate_Opt._AnimationRotate;
            if (animation_rotate == null) return;
            animation_rotate.selected_animation = index;

            uCtrl_Animation_Elm_SelectChanged(sender, eventArgs);
        }


        private void uCtrl_EditableBackground_Opt_BackgroundAdd(object sender, EventArgs eventArgs, int index)
        {
            if (!PreviewView) return;
            if (Watch_Face == null) return;
            Editable_Background background = (Editable_Background)uCtrl_EditableBackground_Opt._EditableBackground;
            if (background == null) return;

            if (background.BackgroundList == null) background.BackgroundList = new List<BackgroundList>();
            BackgroundList background_list = new BackgroundList();
            background_list.path = uCtrl_EditableBackground_Opt.GetImage();
            background_list.preview = uCtrl_EditableBackground_Opt.GetPreview();
            if ((background.BackgroundList.Count <= index || index < 0) || (background.BackgroundList.Count == index + 1 && index >= 0))
            {
                background.BackgroundList.Add(background_list);
                ++index;
            }
            else
            {
                background.BackgroundList.Insert(++index, background_list);
            }
            background.selected_background = index;
            Read_EditableBackground_Options(background);

            JSON_Modified = true;
            PreviewImage();
            FormText();
        }

        private void uCtrl_EditableBackground_Opt_BackgroundDel(object sender, EventArgs eventArgs, int index)
        {
            if (!PreviewView) return;
            if (Watch_Face == null) return;
            if (index < 0) return;
            Editable_Background background = (Editable_Background)uCtrl_EditableBackground_Opt._EditableBackground;
            if (background == null) return;

            if (background.BackgroundList == null) background.BackgroundList = new List<BackgroundList>();
            List<BackgroundList> background_list = background.BackgroundList;

            if (background_list.Count > index) background_list.RemoveAt(index);
            background.selected_background = --index;
            if (index < 0 && background_list != null && background_list.Count > 0)
                background.selected_background = 0;
            Read_EditableBackground_Options(background);

            JSON_Modified = true;
            PreviewImage();
            FormText();
        }

        private void uCtrl_EditableBackground_Opt_BackgroundIndexChanged(object sender, EventArgs eventArgs, int index)
        {
            if (!PreviewView) return;
            if (Watch_Face == null) return;
            if (index < 0) return;
            Editable_Background background = (Editable_Background)uCtrl_EditableBackground_Opt._EditableBackground;
            if (background == null) return;
            background.selected_background = index;

            Read_EditableBackground_Options(background);
            PreviewImage();
        }

        private void uCtrl_EditableBackground_Opt_PreviewAdd(object sender, EventArgs eventArgs, int index)
        {
            if (index < 0) return;
            if (Watch_Face == null || Watch_Face.ScreenNormal == null || Watch_Face.ScreenNormal.Background == null ||
                Watch_Face.ScreenNormal.Background.Editable_Background == null ||
                Watch_Face.ScreenNormal.Background.Editable_Background.BackgroundList == null ||
                index >= Watch_Face.ScreenNormal.Background.Editable_Background.BackgroundList.Count)
            {
                return;
            }
            if (FileName != null && ProjectDir != null) // проект уже сохранен
            {
                // формируем картинку для предпросмотра
                /* Bitmap bitmap = new Bitmap(Convert.ToInt32(454), Convert.ToInt32(454), PixelFormat.Format32bppArgb);
                 Bitmap mask = new Bitmap(Application.StartupPath + @"\Mask\mask_gtr_3.png");
                 switch (ProgramSettings.Watch_Model)
                 {
                     case "GTR 3 Pro":
                         bitmap = new Bitmap(Convert.ToInt32(480), Convert.ToInt32(480), PixelFormat.Format32bppArgb);
                         mask = new Bitmap(Application.StartupPath + @"\Mask\mask_gtr_3_pro.png");
                         break;
                     case "GTS 3":
                     case "GTS 4":
                         bitmap = new Bitmap(Convert.ToInt32(390), Convert.ToInt32(450), PixelFormat.Format32bppArgb);
                         mask = new Bitmap(Application.StartupPath + @"\Mask\mask_gts_3.png");
                         break;
                     case "GTR 4":
                         bitmap = new Bitmap(Convert.ToInt32(466), Convert.ToInt32(466), PixelFormat.Format32bppArgb);
                         mask = new Bitmap(Application.StartupPath + @"\Mask\mask_gtr_4.png");
                         break;
                     case "Amazfit Band 7":
                         bitmap = new Bitmap(Convert.ToInt32(194), Convert.ToInt32(368), PixelFormat.Format32bppArgb);
                         mask = new Bitmap(Application.StartupPath + @"\Mask\mask_band_7.png");
                         break;
                     case "GTS 4 mini":
                         bitmap = new Bitmap(Convert.ToInt32(336), Convert.ToInt32(384), PixelFormat.Format32bppArgb);
                         mask = new Bitmap(Application.StartupPath + @"\Mask\mask_gts_4_mini.png");
                         break;
                     case "Falcon":
                     case "GTR mini":
                         bitmap = new Bitmap(Convert.ToInt32(416), Convert.ToInt32(416), PixelFormat.Format32bppArgb);
                         mask = new Bitmap(Application.StartupPath + @"\Mask\mask_falcon.png");
                         break;
                 }*/
                Bitmap bitmap = new Bitmap(SelectedModel.background.w, SelectedModel.background.h, PixelFormat.Format32bppArgb);
                Bitmap mask = new Bitmap(Application.StartupPath + @"\Mask\" + SelectedModel.maskImage);
                Graphics gPanel = Graphics.FromImage(bitmap);
                int link = radioButton_ScreenNormal.Checked ? 0 : 1;
                Preview_screen(gPanel, 1.0f, false, false, false, false, false, false, false, false, false, false, false, true, false,
                    false, false, link, false, false, -1, false, 0);
                if (checkBox_crop.Checked) bitmap = ApplyMask(bitmap, mask);

                // определяем имя файла для сохранения и сохраняем файл
                int i = 1;
                string tempName = "bg_edit_" + (index + 1).ToString() + "_preview";
                string NamePreview = tempName + ".png";
                string PathPreview = ProjectDir + @"\assets\" + NamePreview;
                while (File.Exists(PathPreview) && i < 10)
                {
                    NamePreview = tempName + i.ToString() + ".png";
                    PathPreview = ProjectDir + @"\assets\" + NamePreview;
                    i++;
                    if (i > 9)
                    {
                        MessageBox.Show(Properties.FormStrings.Message_Wrong_Preview_Exists,
                            Properties.FormStrings.Message_Warning_Caption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
                bitmap.Save(PathPreview, ImageFormat.Png);
                string fileNameOnly = Path.GetFileNameWithoutExtension(PathPreview);

                PreviewView = false;

                LoadImage(Path.GetDirectoryName(PathPreview) + @"\");

                Watch_Face.ScreenNormal.Background.Editable_Background.BackgroundList[index].preview = fileNameOnly;
                PreviewView = true;
                JSON_Modified = true;
                FormText();
                Read_EditableBackground_Options(Watch_Face.ScreenNormal.Background.Editable_Background);
                //HideAllElemenrOptions();
                //ResetHighlightState("");

                bitmap.Dispose();

                PreviewImage();
            }
        }

        private void uCtrl_EditableBackground_Opt_PreviewRefresh(object sender, EventArgs eventArgs, int index)
        {
            if (FileName == null || ProjectDir == null) return;
            if (index < 0) return;
            if (Watch_Face == null || Watch_Face.ScreenNormal == null || Watch_Face.ScreenNormal.Background == null ||
                Watch_Face.ScreenNormal.Background.Editable_Background == null ||
                Watch_Face.ScreenNormal.Background.Editable_Background.BackgroundList == null) return;
            if (index >= Watch_Face.ScreenNormal.Background.Editable_Background.BackgroundList.Count) return;
            if ( Watch_Face.ScreenNormal.Background.Editable_Background.BackgroundList[index].preview == null ||
                 Watch_Face.ScreenNormal.Background.Editable_Background.BackgroundList[index].preview.Length < 1)
            {
                uCtrl_EditableBackground_Opt_PreviewAdd(null, null, index);
                return;
            }
            if (index < Watch_Face.ScreenNormal.Background.Editable_Background.BackgroundList.Count &&
                Watch_Face.ScreenNormal.Background.Editable_Background.BackgroundList[index].preview.Length > 0)
            {
                string preview = ProjectDir + @"\assets\" + 
                    Watch_Face.ScreenNormal.Background.Editable_Background.BackgroundList[index].preview + ".png";
                if (!File.Exists(preview))
                {
                    Watch_Face.ScreenNormal.Background.Editable_Background.BackgroundList[index].preview = "";
                    uCtrl_EditableBackground_Opt_PreviewAdd(null, null, index);
                    return;
                }
                /*Bitmap bitmap = new Bitmap(Convert.ToInt32(454), Convert.ToInt32(454), PixelFormat.Format32bppArgb);
                Bitmap mask = new Bitmap(Application.StartupPath + @"\Mask\mask_gtr_3.png");
                switch (ProgramSettings.Watch_Model)
                {
                    case "GTR 3 Pro":
                        bitmap = new Bitmap(Convert.ToInt32(480), Convert.ToInt32(480), PixelFormat.Format32bppArgb);
                        mask = new Bitmap(Application.StartupPath + @"\Mask\mask_gtr_3_pro.png");
                        break;
                    case "GTS 3":
                    case "GTS 4":
                        bitmap = new Bitmap(Convert.ToInt32(390), Convert.ToInt32(450), PixelFormat.Format32bppArgb);
                        mask = new Bitmap(Application.StartupPath + @"\Mask\mask_gts_3.png");
                        break;
                    case "GTR 4":
                        bitmap = new Bitmap(Convert.ToInt32(466), Convert.ToInt32(466), PixelFormat.Format32bppArgb);
                        mask = new Bitmap(Application.StartupPath + @"\Mask\mask_gtr_4.png");
                        break;
                    case "Amazfit Band 7":
                        bitmap = new Bitmap(Convert.ToInt32(194), Convert.ToInt32(368), PixelFormat.Format32bppArgb);
                        mask = new Bitmap(Application.StartupPath + @"\Mask\mask_band_7.png");
                        break;
                    case "GTS 4 mini":
                        bitmap = new Bitmap(Convert.ToInt32(336), Convert.ToInt32(384), PixelFormat.Format32bppArgb);
                        mask = new Bitmap(Application.StartupPath + @"\Mask\mask_gts_4_mini.png");
                        break;
                    case "Falcon":
                    case "GTR mini":
                        bitmap = new Bitmap(Convert.ToInt32(416), Convert.ToInt32(416), PixelFormat.Format32bppArgb);
                        mask = new Bitmap(Application.StartupPath + @"\Mask\mask_falcon.png");
                        break;
                }*/
                Bitmap bitmap = new Bitmap(SelectedModel.background.w, SelectedModel.background.h, PixelFormat.Format32bppArgb);
                Bitmap mask = new Bitmap(Application.StartupPath + @"\Mask\" + SelectedModel.maskImage);
                Graphics gPanel = Graphics.FromImage(bitmap);
                int link = radioButton_ScreenNormal.Checked ? 0 : 1;
                Preview_screen(gPanel, 1.0f, false, false, false, false, false, false, false, false, false, false, false, true, false,
                    false, false, link, false, false, -1, false, 0);
                if (checkBox_crop.Checked) bitmap = ApplyMask(bitmap, mask);

                //Image loadedImage = null;
                //using (FileStream stream = new FileStream(preview, FileMode.Open, FileAccess.Read))
                //{
                //    loadedImage = Image.FromStream(stream);
                //}
                bitmap.Save(preview, ImageFormat.Png);

                bitmap.Dispose();
                //loadedImage.Dispose();

                LoadImage(Path.GetDirectoryName(preview) + @"\");
                //HideAllElemenrOptions();
                //ResetHighlightState("");
                Read_EditableBackground_Options(Watch_Face.ScreenNormal.Background.Editable_Background);

                PreviewImage();
            }
        }

        private void uCtrl_EditableBackground_Opt_ValueChanged(object sender, EventArgs eventArgs, int index)
        {
            if (!PreviewView) return;
            if (Watch_Face == null) return;
            if (index < 0) return;
            Editable_Background background = (Editable_Background)uCtrl_EditableBackground_Opt._EditableBackground;
            if (background == null) return;

            if (background.BackgroundList == null) background.BackgroundList = new List<BackgroundList>();
            List<BackgroundList> background_list = background.BackgroundList;
            if (background_list == null) return;
            if (background_list.Count < index + 1) return;

            background.fg = uCtrl_EditableBackground_Opt.GetForeground();

            background.tips_bg = uCtrl_EditableBackground_Opt.GetTip();
            background.tips_x = (int)uCtrl_EditableBackground_Opt.numericUpDown_tipX.Value;
            background.tips_y = (int)uCtrl_EditableBackground_Opt.numericUpDown_tipY.Value;

            background_list[index].path = uCtrl_EditableBackground_Opt.GetImage();
            background_list[index].preview = uCtrl_EditableBackground_Opt.GetPreview();
            background.showEeditMode = uCtrl_EditableBackground_Opt.checkBox_edit_mode.Checked;

            JSON_Modified = true;
            PreviewImage();
            FormText();
        }

        private void uCtrl_EditableTimePointer_Opt_PointersAdd(object sender, EventArgs eventArgs, int index)
        {
            if (!PreviewView) return;
            if (Watch_Face == null) return;
            ElementEditablePointers editablePointers = (ElementEditablePointers)uCtrl_EditableTimePointer_Opt._EditableTimePointer;
            if (editablePointers == null) return;

            if (editablePointers.config == null) editablePointers.config = new List<PointersList>();
            PointersList pointers_list = new PointersList();
            pointers_list.hour = new EDITABLE_POINTER();
            pointers_list.minute = new EDITABLE_POINTER();
            pointers_list.second = new EDITABLE_POINTER();

            pointers_list.hour.path = uCtrl_EditableTimePointer_Opt.GetHourPointerImage();
            pointers_list.hour.centerX = (int)uCtrl_EditableTimePointer_Opt.numericUpDown_hourPointer_X.Value;
            pointers_list.hour.centerY = (int)uCtrl_EditableTimePointer_Opt.numericUpDown_hourPointer_Y.Value;
            pointers_list.hour.posX = (int)uCtrl_EditableTimePointer_Opt.numericUpDown_hourPointer_offset_X.Value;
            pointers_list.hour.posY = (int)uCtrl_EditableTimePointer_Opt.numericUpDown_hourPointer_offset_Y.Value;

            pointers_list.minute.path = uCtrl_EditableTimePointer_Opt.GetMinutePointerImage();
            pointers_list.minute.centerX = (int)uCtrl_EditableTimePointer_Opt.numericUpDown_minutePointer_X.Value;
            pointers_list.minute.centerY = (int)uCtrl_EditableTimePointer_Opt.numericUpDown_minutePointer_Y.Value;
            pointers_list.minute.posX = (int)uCtrl_EditableTimePointer_Opt.numericUpDown_minutePointer_offset_X.Value;
            pointers_list.minute.posY = (int)uCtrl_EditableTimePointer_Opt.numericUpDown_minutePointer_offset_Y.Value;

            pointers_list.second.path = uCtrl_EditableTimePointer_Opt.GetSecondPointerImage();
            pointers_list.second.centerX = (int)uCtrl_EditableTimePointer_Opt.numericUpDown_secondPointer_X.Value;
            pointers_list.second.centerY = (int)uCtrl_EditableTimePointer_Opt.numericUpDown_secondPointer_Y.Value;
            pointers_list.second.posX = (int)uCtrl_EditableTimePointer_Opt.numericUpDown_secondPointer_offset_X.Value;
            pointers_list.second.posY = (int)uCtrl_EditableTimePointer_Opt.numericUpDown_secondPointer_offset_Y.Value;

            pointers_list.preview = uCtrl_EditableTimePointer_Opt.GetPreview();

            if ((editablePointers.config.Count <= index || index < 0) || (editablePointers.config.Count == index + 1 && index >= 0))
            {
                editablePointers.config.Add(pointers_list);
                ++index;
            }
            else
            {
                editablePointers.config.Insert(++index, pointers_list);
            }
            editablePointers.selected_pointers = index;
            Read_EditablePointers_Options(editablePointers);

            JSON_Modified = true;
            PreviewImage();
            FormText();
        }

        private void uCtrl_EditableTimePointer_Opt_PointersDel(object sender, EventArgs eventArgs, int index)
        {
            if (!PreviewView) return;
            if (Watch_Face == null) return;
            if (index < 0) return;
            ElementEditablePointers editablePointers = (ElementEditablePointers)uCtrl_EditableTimePointer_Opt._EditableTimePointer;
            if (editablePointers == null) return;

            if (editablePointers.config == null) editablePointers.config = new List<PointersList>();
            List<PointersList> pointers_list = editablePointers.config;

            if (pointers_list.Count > index) pointers_list.RemoveAt(index);
            editablePointers.selected_pointers = --index;
            if (index < 0 && pointers_list != null && pointers_list.Count > 0)
                editablePointers.selected_pointers = 0;
            Read_EditablePointers_Options(editablePointers);

            JSON_Modified = true;
            PreviewImage();
            FormText();
        }

        private void uCtrl_EditableTimePointer_Opt_PointersIndexChanged(object sender, EventArgs eventArgs, int index)
        {
            if (!PreviewView) return;
            if (Watch_Face == null) return;
            if (index < 0) return;
            ElementEditablePointers editablePointers = (ElementEditablePointers)uCtrl_EditableTimePointer_Opt._EditableTimePointer;
            if (editablePointers == null) return;
            editablePointers.selected_pointers = index;

            Read_EditablePointers_Options(editablePointers);
            PreviewImage();
        }

        private void uCtrl_EditableTimePointer_Opt_PreviewAdd(object sender, EventArgs eventArgs, int index)
        {
            if (index < 0) return;
            if (Watch_Face == null || Watch_Face.ElementEditablePointers == null ||
                Watch_Face.ElementEditablePointers.config == null ||
                index >= Watch_Face.ElementEditablePointers.config.Count)
            {
                return;
            }
            if (FileName != null && ProjectDir != null) // проект уже сохранен
            {
                Bitmap bitmap = new Bitmap(SelectedModel.background.w, SelectedModel.background.h, PixelFormat.Format32bppArgb);
                Bitmap mask = new Bitmap(Application.StartupPath + @"\Mask\" + SelectedModel.maskImage);
                Graphics gPanel = Graphics.FromImage(bitmap);
                Creat_preview_editable_pointers(gPanel, 1.0f, false, radioButton_ScreenNormal.Checked ? 0 : 1);
                if (checkBox_crop.Checked) bitmap = ApplyMask(bitmap, mask);

                // определяем имя файла для сохранения и сохраняем файл
                int i = 1;
                string tempName = "pointer_edit_" + (index + 1).ToString() + "_preview";
                string NamePreview = tempName + ".png";
                string PathPreview = ProjectDir + @"\assets\" + NamePreview;
                while (File.Exists(PathPreview) && i < 10)
                {
                    NamePreview = tempName + i.ToString() + ".png";
                    PathPreview = ProjectDir + @"\assets\" + NamePreview;
                    i++;
                    if (i > 9)
                    {
                        MessageBox.Show(Properties.FormStrings.Message_Wrong_Preview_Exists,
                            Properties.FormStrings.Message_Warning_Caption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
                bitmap.Save(PathPreview, ImageFormat.Png);
                string fileNameOnly = Path.GetFileNameWithoutExtension(PathPreview);

                PreviewView = false;

                LoadImage(Path.GetDirectoryName(PathPreview) + @"\");

                Watch_Face.ElementEditablePointers.config[index].preview = fileNameOnly;
                PreviewView = true;
                JSON_Modified = true;
                FormText();
                Read_EditablePointers_Options(Watch_Face.ElementEditablePointers);
                //HideAllElemenrOptions();
                //ResetHighlightState("");

                bitmap.Dispose();

                PreviewImage();
            }
        }

        private void uCtrl_EditableTimePointer_Opt_PreviewRefresh(object sender, EventArgs eventArgs, int index)
        {
            if (FileName == null || ProjectDir == null) return;
            if (index < 0) return;
            if (Watch_Face == null || Watch_Face.ElementEditablePointers == null ||
                Watch_Face.ElementEditablePointers.config == null) return;
            if (index >= Watch_Face.ElementEditablePointers.config.Count) return;
            if (Watch_Face.ElementEditablePointers.config[index].preview == null ||
                 Watch_Face.ElementEditablePointers.config[index].preview.Length < 1)
            {
                uCtrl_EditableBackground_Opt_PreviewAdd(null, null, index);
                return;
            }
            if (index < Watch_Face.ElementEditablePointers.config.Count &&
                Watch_Face.ElementEditablePointers.config[index].preview.Length > 0)
            {
                string preview = ProjectDir + @"\assets\" +
                    Watch_Face.ElementEditablePointers.config[index].preview + ".png";
                if (!File.Exists(preview))
                {
                    Watch_Face.ElementEditablePointers.config[index].preview = "";
                    uCtrl_EditableBackground_Opt_PreviewAdd(null, null, index);
                    return;
                }
                Bitmap bitmap = new Bitmap(SelectedModel.background.w, SelectedModel.background.h, PixelFormat.Format32bppArgb);
                Bitmap mask = new Bitmap(Application.StartupPath + @"\Mask\" + SelectedModel.maskImage);
                Graphics gPanel = Graphics.FromImage(bitmap);
                Creat_preview_editable_pointers(gPanel, 1.0f, false, radioButton_ScreenNormal.Checked ? 0 : 1);
                if (checkBox_crop.Checked) bitmap = ApplyMask(bitmap, mask);

                //Image loadedImage = null;
                //using (FileStream stream = new FileStream(preview, FileMode.Open, FileAccess.Read))
                //{
                //    loadedImage = Image.FromStream(stream);
                //}
                bitmap.Save(preview, ImageFormat.Png);

                bitmap.Dispose();
                //loadedImage.Dispose();

                LoadImage(Path.GetDirectoryName(preview) + @"\");
                //HideAllElemenrOptions();
                //ResetHighlightState("");
                Read_EditablePointers_Options(Watch_Face.ElementEditablePointers);

                PreviewImage();
            }
        }

        private void uCtrl_EditableTimePointer_Opt_ValueChanged(object sender, EventArgs eventArgs, int index)
        {
            if (!PreviewView) return;
            if (Watch_Face == null) return;
            if (index < 0) return;
            ElementEditablePointers editablePointers = (ElementEditablePointers)uCtrl_EditableTimePointer_Opt._EditableTimePointer;
            if (editablePointers == null) return;

            if (editablePointers.config == null) editablePointers.config = new List<PointersList>();
            if (editablePointers.cover == null) editablePointers.cover = new hmUI_widget_IMG();
            List<PointersList> pointers_list = editablePointers.config;
            if (pointers_list == null) return;
            if (pointers_list.Count < index + 1) return;

            editablePointers.fg = uCtrl_EditableTimePointer_Opt.GetForeground();

            editablePointers.tips_bg = uCtrl_EditableTimePointer_Opt.GetTip();
            editablePointers.tips_x = (int)uCtrl_EditableTimePointer_Opt.numericUpDown_tipX.Value;
            editablePointers.tips_y = (int)uCtrl_EditableTimePointer_Opt.numericUpDown_tipY.Value;
            
            pointers_list[index].hour.path = uCtrl_EditableTimePointer_Opt.GetHourPointerImage();
            pointers_list[index].hour.centerX = (int)uCtrl_EditableTimePointer_Opt.numericUpDown_hourPointer_X.Value;
            pointers_list[index].hour.centerY = (int)uCtrl_EditableTimePointer_Opt.numericUpDown_hourPointer_Y.Value;
            pointers_list[index].hour.posX = (int)uCtrl_EditableTimePointer_Opt.numericUpDown_hourPointer_offset_X.Value;
            pointers_list[index].hour.posY = (int)uCtrl_EditableTimePointer_Opt.numericUpDown_hourPointer_offset_Y.Value;

            pointers_list[index].minute.path = uCtrl_EditableTimePointer_Opt.GetMinutePointerImage();
            pointers_list[index].minute.centerX = (int)uCtrl_EditableTimePointer_Opt.numericUpDown_minutePointer_X.Value;
            pointers_list[index].minute.centerY = (int)uCtrl_EditableTimePointer_Opt.numericUpDown_minutePointer_Y.Value;
            pointers_list[index].minute.posX = (int)uCtrl_EditableTimePointer_Opt.numericUpDown_minutePointer_offset_X.Value;
            pointers_list[index].minute.posY = (int)uCtrl_EditableTimePointer_Opt.numericUpDown_minutePointer_offset_Y.Value;

            pointers_list[index].second.path = uCtrl_EditableTimePointer_Opt.GetSecondPointerImage();
            pointers_list[index].second.centerX = (int)uCtrl_EditableTimePointer_Opt.numericUpDown_secondPointer_X.Value;
            pointers_list[index].second.centerY = (int)uCtrl_EditableTimePointer_Opt.numericUpDown_secondPointer_Y.Value;
            pointers_list[index].second.posX = (int)uCtrl_EditableTimePointer_Opt.numericUpDown_secondPointer_offset_X.Value;
            pointers_list[index].second.posY = (int)uCtrl_EditableTimePointer_Opt.numericUpDown_secondPointer_offset_Y.Value;

            editablePointers.cover.src = uCtrl_EditableTimePointer_Opt.GetImageCentr();
            editablePointers.cover.x = (int)uCtrl_EditableTimePointer_Opt.numericUpDown_pointer_centr_X.Value;
            editablePointers.cover.y = (int)uCtrl_EditableTimePointer_Opt.numericUpDown_pointer_centr_Y.Value;

            pointers_list[index].preview = uCtrl_EditableTimePointer_Opt.GetPreview();

            editablePointers.AOD_show = uCtrl_EditableTimePointer_Opt.checkBox_secondInAOD.Checked;
            editablePointers.showEeditMode = uCtrl_EditableTimePointer_Opt.checkBox_edit_mode.Checked;

            JSON_Modified = true;
            PreviewImage();
            FormText();
        }

        private void uCtrl_DisconnectAlert_Opt_ValueChanged(object sender, EventArgs eventArgs)
        {
            if (!PreviewView) return;
            if (Watch_Face == null) return;
            DisconnectAlert disconnectAlert = Watch_Face.DisconnectAlert;
            if (disconnectAlert == null) return;

            if (disconnectAlert.BluetoothOff == null) disconnectAlert.BluetoothOff = new BluetoothStateAlert();
            if (disconnectAlert.BluetoothOn == null) disconnectAlert.BluetoothOn = new BluetoothStateAlert();

            disconnectAlert.BluetoothOff.vibrate = uCtrl_DisconnectAlert_Opt.checkBox_disconneсt_vibrate.Checked;
            disconnectAlert.BluetoothOff.vibrateType = uCtrl_DisconnectAlert_Opt.GetVibratetDisconneсnt();
            disconnectAlert.BluetoothOff.toastShow = uCtrl_DisconnectAlert_Opt.checkBox_disconneсt_toast.Checked;
            disconnectAlert.BluetoothOff.toastText = uCtrl_DisconnectAlert_Opt.textBox_disconneсt_toast_text.Text;

            disconnectAlert.BluetoothOn.vibrate = uCtrl_DisconnectAlert_Opt.checkBox_conneсt_vibrate.Checked;
            disconnectAlert.BluetoothOn.vibrateType = uCtrl_DisconnectAlert_Opt.GetVibratetConneсnt();
            disconnectAlert.BluetoothOn.toastShow = uCtrl_DisconnectAlert_Opt.checkBox_conneсt_toast.Checked;
            disconnectAlert.BluetoothOn.toastText = uCtrl_DisconnectAlert_Opt.textBox_conneсt_toast_text.Text;

            JSON_Modified = true;
            //PreviewImage();
            FormText();
        }

        private void uCtrl_RepeatingAlert_Opt_ValueChanged(object sender, EventArgs eventArgs)
        {
            if (!PreviewView) return;
            if (Watch_Face == null) return;
            RepeatAlert repeatAlert = Watch_Face.RepeatAlert;
            if (repeatAlert == null) return;

            if (repeatAlert.EveryHour == null) repeatAlert.EveryHour = new PeriodicAlert();
            if (repeatAlert.RepeatingAlert == null) repeatAlert.RepeatingAlert = new PeriodicAlert();

            repeatAlert.EveryHour.vibrate = uCtrl_RepeatingAlert_Opt.checkBox_hour_vibrate.Checked;
            repeatAlert.EveryHour.vibrateType = uCtrl_RepeatingAlert_Opt.GetVibratetHour();
            repeatAlert.EveryHour.toastShow = uCtrl_RepeatingAlert_Opt.checkBox_hour_toast.Checked;
            repeatAlert.EveryHour.toastText = uCtrl_RepeatingAlert_Opt.textBox_hour_toast_text.Text;

            repeatAlert.RepeatingAlert.vibrate = uCtrl_RepeatingAlert_Opt.checkBox_repeat_period_vibrate.Checked;
            repeatAlert.RepeatingAlert.vibrateType = uCtrl_RepeatingAlert_Opt.GetVibratetRepeatPeriod();
            repeatAlert.RepeatingAlert.repeatPeriod = uCtrl_RepeatingAlert_Opt.GetVibratetRepeatPeriodTime();
            repeatAlert.RepeatingAlert.toastShow = uCtrl_RepeatingAlert_Opt.checkBox_repeat_period_toast.Checked;
            repeatAlert.RepeatingAlert.toastText = uCtrl_RepeatingAlert_Opt.textBox_repeat_period_toast_text.Text;

            JSON_Modified = true;
            //PreviewImage();
            FormText();
        }

        private void uCtrl_SmoothSeconds_Opt_ValueChanged(object sender, EventArgs eventArgs)
        {
            if (!PreviewView) return;
            if (Watch_Face == null) return;
            Smooth_Second smoothSecon = (Smooth_Second)uCtrl_SmoothSeconds_Opt._SmoothSeconds;
            if (smoothSecon == null) return;

            smoothSecon.type = uCtrl_SmoothSeconds_Opt.GetSmothType();
            smoothSecon.fps = (int)uCtrl_SmoothSeconds_Opt.numericUpDown_fps.Value;

            JSON_Modified = true;
            PreviewImage();
            FormText();
        }

        private void uCtrl_Text_Circle_Opt_ValueChanged(object sender, EventArgs eventArgs)
        {
            if (!PreviewView) return;
            if (Watch_Face == null) return;
            Text_Circle text_circle = (Text_Circle)uCtrl_Text_Circle_Opt._ElementWithText;
            if (text_circle == null) return;

            text_circle.img_First = uCtrl_Text_Circle_Opt.GetImage();
            text_circle.unit = uCtrl_Text_Circle_Opt.GetUnit();
            text_circle.imperial_unit = uCtrl_Text_Circle_Opt.GetUnitImperial();
            text_circle.dot_image = uCtrl_Text_Circle_Opt.GetImageDecimalPoint();
            text_circle.error_image = uCtrl_Text_Circle_Opt.GetImageError();
            text_circle.separator_image = uCtrl_Text_Circle_Opt.GetSeparator();

            text_circle.circle_center_X = (int)uCtrl_Text_Circle_Opt.numericUpDown_centr_X.Value;
            text_circle.circle_center_Y = (int)uCtrl_Text_Circle_Opt.numericUpDown_centr_Y.Value;
            text_circle.radius = (int)uCtrl_Text_Circle_Opt.numericUpDown_radius.Value;
            text_circle.angle = (int)uCtrl_Text_Circle_Opt.numericUpDown_angle.Value;
            text_circle.char_space_angle = (int)uCtrl_Text_Circle_Opt.numericUpDown_spacing.Value;

            text_circle.zero = uCtrl_Text_Circle_Opt.checkBox_addZero.Checked;
            text_circle.reverse_direction = uCtrl_Text_Circle_Opt.checkBox_reverse_direction.Checked;
            text_circle.unit_in_alignment = uCtrl_Text_Circle_Opt.checkBox_unit_in_alignment.Checked;

            text_circle.vertical_alignment = uCtrl_Text_Circle_Opt.GetVerticalAlignment();
            text_circle.horizontal_alignment = uCtrl_Text_Circle_Opt.GetHorizontalAlignment();

            //text_circle.image_width = uCtrl_Text_Circle_Opt.image_width;
            //text_circle.image_height = uCtrl_Text_Circle_Opt.image_height;
            //text_circle.unit_width = uCtrl_Text_Circle_Opt.unit_width;
            //text_circle.error_width = uCtrl_Text_Circle_Opt.error_width;

            JSON_Modified = true;
            PreviewImage();
            FormText();
        }

        private void uCtrl_Text_Rotate_Opt_ValueChanged(object sender, EventArgs eventArgs)
        {
            if (!PreviewView) return;
            if (Watch_Face == null) return;
            hmUI_widget_IMG_NUMBER img_number = (hmUI_widget_IMG_NUMBER)uCtrl_Text_Rotate_Opt._ElementWithText;
            if (img_number == null) return;

            img_number.align = uCtrl_Text_Rotate_Opt.GetAlignment();
            img_number.unit_in_alignment = uCtrl_Text_Rotate_Opt.checkBox_unit_in_alignment.Checked;
            img_number.img_First = uCtrl_Text_Rotate_Opt.GetImage();
            img_number.imageX = (int)uCtrl_Text_Rotate_Opt.numericUpDown_imageX.Value;
            img_number.imageY = (int)uCtrl_Text_Rotate_Opt.numericUpDown_imageY.Value;
            img_number.space = (int)uCtrl_Text_Rotate_Opt.numericUpDown_spacing.Value;
            img_number.angle = (int)uCtrl_Text_Rotate_Opt.numericUpDown_angle.Value;
            img_number.unit = uCtrl_Text_Rotate_Opt.GetUnit();
            img_number.imperial_unit = uCtrl_Text_Rotate_Opt.GetUnitMile();
            img_number.dot_image = uCtrl_Text_Rotate_Opt.GetImageDecimalPointOrMinus();
            img_number.negative_image = uCtrl_Text_Rotate_Opt.GetImageDecimalPointOrMinus();
            img_number.invalid_image = uCtrl_Text_Rotate_Opt.GetImageError();
            img_number.separator_image = uCtrl_Text_Rotate_Opt.GetSeparator(); // разделитель для макс/мин температуры
            img_number.zero = uCtrl_Text_Rotate_Opt.checkBox_addZero.Checked;


            JSON_Modified = true;
            PreviewImage();
            FormText();
        }

        private void uCtrl_Weather_FewDays_Opt_ValueChanged(object sender, EventArgs eventArgs)
        {
            if (!PreviewView) return;
            if (Watch_Face == null) return;
            FewDays fewDays = (FewDays)uCtrl_Weather_FewDays_Opt._FewDays;
            if (fewDays == null) return;


            uCtrl_Weather_FewDays_Opt._FewDays = fewDays;

            if (fewDays == null)
            {
                PreviewView = true;
                return;
            }

            fewDays.Background = uCtrl_Weather_FewDays_Opt.GetBackground();
            fewDays.X = (int)uCtrl_Weather_FewDays_Opt.numericUpDown_posX.Value;
            fewDays.Y = (int)uCtrl_Weather_FewDays_Opt.numericUpDown_posY.Value;

            fewDays.DaysCount = (int)uCtrl_Weather_FewDays_Opt.numericUpDown_daysCount.Value;
            fewDays.ColumnWidth = (int)uCtrl_Weather_FewDays_Opt.numericUpDown_columnWidth.Value;

            //fewDays.PositionOnGraph = uCtrl_Weather_FewDays_Opt.checkBox_position_on_graph.Checked;

            JSON_Modified = true;
            PreviewImage();
            FormText();
        }

        private void uCtrl_TemperatureGraph_Opt_ValueChanged(object sender, EventArgs eventArgs)
        {
            if (!PreviewView) return;
            if (Watch_Face == null) return;
            Weather_Diagram diagram = (Weather_Diagram)uCtrl_TemperatureGraph_Opt._Diagram;
            if (diagram == null) return;

            diagram.Y = (int)uCtrl_TemperatureGraph_Opt.numericUpDown_posY.Value;
            diagram.Height = (int)uCtrl_TemperatureGraph_Opt.numericUpDown_height.Value;
            diagram.PositionOnGraph = uCtrl_TemperatureGraph_Opt.checkBox_position_on_graph.Checked;
            diagram.GraphFullScreen = uCtrl_TemperatureGraph_Opt.checkBox_graph_fullScreen.Checked;

            diagram.Max_color = ColorToString(uCtrl_TemperatureGraph_Opt.GetMaxColor());
            diagram.Max_lineWidth = (int)uCtrl_TemperatureGraph_Opt.numericUpDown_max_lineWidth.Value;
            diagram.Max_pointType = uCtrl_TemperatureGraph_Opt.GetMaxPointType();
            diagram.Max_pointSize = (int)uCtrl_TemperatureGraph_Opt.numericUpDown_max_pointSize.Value;
            diagram.Max_offsetX = (int)uCtrl_TemperatureGraph_Opt.numericUpDown_max_offsetX.Value;

            diagram.Min_color = ColorToString(uCtrl_TemperatureGraph_Opt.GetMinColor());
            diagram.Min_lineWidth = (int)uCtrl_TemperatureGraph_Opt.numericUpDown_min_lineWidth.Value;
            diagram.Min_pointType = uCtrl_TemperatureGraph_Opt.GetMinPointType();
            diagram.Min_pointSize = (int)uCtrl_TemperatureGraph_Opt.numericUpDown_min_pointSize.Value;
            diagram.Min_offsetX = (int)uCtrl_TemperatureGraph_Opt.numericUpDown_min_offsetX.Value;

            diagram.Average_color = ColorToString(uCtrl_TemperatureGraph_Opt.GetAverageColor());
            diagram.Average_lineWidth = (int)uCtrl_TemperatureGraph_Opt.numericUpDown_average_lineWidth.Value;
            diagram.Average_pointType = uCtrl_TemperatureGraph_Opt.GetAveragePointType();
            diagram.Average_pointSize = (int)uCtrl_TemperatureGraph_Opt.numericUpDown_average_pointSize.Value;
            diagram.Average_offsetX = (int)uCtrl_TemperatureGraph_Opt.numericUpDown_average_offsetX.Value;

            diagram.Use_max_diagram = uCtrl_TemperatureGraph_Opt.checkBox_use_max.Checked;
            diagram.Use_min_diagram = uCtrl_TemperatureGraph_Opt.checkBox_use_min.Checked;
            diagram.Use_average_diagram = uCtrl_TemperatureGraph_Opt.checkBox_use_average.Checked;

            JSON_Modified = true;
            PreviewImage();
            FormText();
        }

        private void uCtrl_JS_script_Opt_ValueChanged(object sender, EventArgs eventArgs)
        {
            if (!PreviewView) return;
            if (Watch_Face == null) return;
            ElementScript script = (ElementScript)uCtrl_JS_script_Opt._ElementScript;
            if (script == null) return;

            script.user_functions = uCtrl_JS_script_Opt.checkBox_user_functions.Checked;
            script.user_script_start = uCtrl_JS_script_Opt.checkBox_user_script_start.Checked;
            script.user_script = uCtrl_JS_script_Opt.checkBox_user_script.Checked;
            script.user_script_beforeShortcuts = uCtrl_JS_script_Opt.checkBox_user_script_beforeShortcuts.Checked;
            script.user_script_end = uCtrl_JS_script_Opt.checkBox_user_script_end.Checked;
            script.resume_call = uCtrl_JS_script_Opt.checkBox_resume_call.Checked;
            script.pause_call = uCtrl_JS_script_Opt.checkBox_pause_call.Checked;


            JSON_Modified = true;
            PreviewImage();
            FormText();
        }

        private void uCtrl_Button_Opt_ValueChanged(object sender, EventArgs eventArgs, int rowIndex)
        {
            if (!PreviewView) return;
            if (Watch_Face == null) return;

            List<Button> buttonsList = Watch_Face.Buttons.Button;
            if (rowIndex >= 0 && rowIndex < buttonsList.Count)
            {
                Button button = buttonsList[rowIndex];

                button.x = (int)uCtrl_Button_Opt.numericUpDown_buttonX.Value;
                button.y = (int)uCtrl_Button_Opt.numericUpDown_buttonY.Value;
                button.w = (int)uCtrl_Button_Opt.numericUpDown_width.Value;
                button.h = (int)uCtrl_Button_Opt.numericUpDown_height.Value;
                button.radius = (int)uCtrl_Button_Opt.numericUpDown_radius.Value;
                button.text_size = (int)uCtrl_Button_Opt.numericUpDown_textSize.Value;
                button.color = ColorToString(uCtrl_Button_Opt.GetColorText());
                button.text = uCtrl_Button_Opt.GetText();
                button.press_src = uCtrl_Button_Opt.GetPressImage();
                button.normal_src = uCtrl_Button_Opt.GetNormalImage();
                button.normal_color = ColorToString(uCtrl_Button_Opt.GetColorNormal());
                button.press_color = ColorToString(uCtrl_Button_Opt.GetColorPress());

                JSON_Modified = true;
                PreviewImage();
                FormText();
            }
        }

        private void uCtrl_Button_Opt_ScriptChanged(int rowIndex, string clickFunc, string longPressFunc)
        {
            if (!PreviewView) return;
            if (Watch_Face == null) return;

            List<Button> buttonsList = Watch_Face.Buttons.Button;
            if (rowIndex >= 0 && rowIndex < buttonsList.Count)
            {
                Button button = buttonsList[rowIndex];

                button.click_func = clickFunc;
                button.longpress_func = longPressFunc;

                List<String> scriptClickList = ButtonClickScriptToString(buttonsList);
                List<String> scriptLongPressList = ButtonLongPressScriptToString(buttonsList);
                List<bool> visibleList = ButtonVisibleList(buttonsList);
                uCtrl_Button_Opt.UpdateButtonsList(scriptClickList, scriptLongPressList, visibleList, rowIndex);

                JSON_Modified = true;
                PreviewImage();
                FormText();
            }
        }

        private void uCtrl_Button_Opt_VisibleButtonChanged(int rowIndex, bool visible)
        {
            if (!PreviewView) return;
            if (Watch_Face == null) return;

            List<Button> buttonsList = Watch_Face.Buttons.Button;
            if (rowIndex >= 0 && rowIndex < buttonsList.Count)
            {
                buttonsList[rowIndex].visible = visible;

                JSON_Modified = true;
                PreviewImage();
                FormText();
            }
        }

        /// <summary>Читаем настройки для отображения выбранной кнопки</summary>
        private void uCtrl_Button_Opt_SelectButton(int rowIndex)
        {
            PreviewView = false;
            List<Button> buttonsList = Watch_Face.Buttons.Button;
            if (rowIndex >= 0 && rowIndex < buttonsList.Count)
            {
                Button button = buttonsList[rowIndex];

                uCtrl_Button_Opt.numericUpDown_buttonX.Value = button.x;
                uCtrl_Button_Opt.numericUpDown_buttonY.Value = button.y;
                uCtrl_Button_Opt.numericUpDown_width.Value = button.w;
                uCtrl_Button_Opt.numericUpDown_height.Value = button.h;
                uCtrl_Button_Opt.numericUpDown_radius.Value = button.radius;
                uCtrl_Button_Opt.numericUpDown_textSize.Value = button.text_size;
                uCtrl_Button_Opt.SetColorText(StringToColor(button.color));
                uCtrl_Button_Opt.SetText(button.text);
                uCtrl_Button_Opt.SetPressImage(button.press_src);
                uCtrl_Button_Opt.SetNormalImage(button.normal_src);
                uCtrl_Button_Opt.SetColorNormal(StringToColor(button.normal_color));
                uCtrl_Button_Opt.SetColorPress(StringToColor(button.press_color));
            }
            PreviewView = true;
        }

        //////
        private void uCtrl_Switch_Background_Opt_ValueChanged(object sender, EventArgs eventArgs)
        {
            if (!PreviewView) return;
            if (Watch_Face == null) return;
            ElementSwitchBackground switchBG = Watch_Face.SwitchBackground;
            if (switchBG == null) switchBG = new ElementSwitchBackground();
            Button button = switchBG.Button;

            button.x = (int)uCtrl_Switch_Background_Opt.numericUpDown_buttonX.Value;
            button.y = (int)uCtrl_Switch_Background_Opt.numericUpDown_buttonY.Value;
            button.w = (int)uCtrl_Switch_Background_Opt.numericUpDown_width.Value;
            button.h = (int)uCtrl_Switch_Background_Opt.numericUpDown_height.Value;
            button.radius = (int)uCtrl_Switch_Background_Opt.numericUpDown_radius.Value;
            button.text_size = (int)uCtrl_Switch_Background_Opt.numericUpDown_textSize.Value;
            button.color = ColorToString(uCtrl_Switch_Background_Opt.GetColorText());
            button.text = uCtrl_Switch_Background_Opt.GetText();
            button.press_src = uCtrl_Switch_Background_Opt.GetPressImage();
            button.normal_src = uCtrl_Switch_Background_Opt.GetNormalImage();
            button.normal_color = ColorToString(uCtrl_Switch_Background_Opt.GetColorNormal());
            button.press_color = ColorToString(uCtrl_Switch_Background_Opt.GetColorPress());

            switchBG.use_crown = uCtrl_Switch_Background_Opt.checkBox_use_crown.Checked;
            switchBG.use_in_AOD = uCtrl_Switch_Background_Opt.checkBox_use_in_AOD.Checked;
            switchBG.vibro = uCtrl_Switch_Background_Opt.checkBox_vibro.Checked;

            JSON_Modified = true;
            PreviewImage();
            FormText();
        }

        private void uCtrl_Switch_Background_Opt_SelectImage(int rowIndex)
        {
            if (!PreviewView) return;
            if (Watch_Face == null) return;
            ElementSwitchBackground switchBG = Watch_Face.SwitchBackground;
            if (switchBG == null) switchBG = new ElementSwitchBackground();

            switchBG.select_index = rowIndex;

            JSON_Modified = true;
            PreviewImage();
            //FormText();
        }

        private void uCtrl_Switch_Background_Opt_AddImage(List<String> BG_images, List<String> Toast_list, int rowIndex)
        {
            if (!PreviewView) return;
            if (Watch_Face == null) return;
            ElementSwitchBackground switchBG = Watch_Face.SwitchBackground;
            if (switchBG == null) switchBG = new ElementSwitchBackground();

            switchBG.bg_list = BG_images;
            Toast_list[rowIndex] = Properties.FormStrings.ToastBG;
            switchBG.toast_list = Toast_list;
            switchBG.select_index = rowIndex;
            uCtrl_Switch_Background_Opt.AddBackgroundImages(BG_images, Toast_list, rowIndex);

            JSON_Modified = true;
            PreviewImage();
            FormText();
        }

        private void uCtrl_Switch_Background_Opt_DelImage(List<String> BG_images, List<String> Toast_list, int rowIndex)
        {
            if (!PreviewView) return;
            if (Watch_Face == null) return;
            ElementSwitchBackground switchBG = Watch_Face.SwitchBackground;
            if (switchBG == null) switchBG = new ElementSwitchBackground();

            switchBG.bg_list = BG_images;
            switchBG.toast_list = Toast_list;
            switchBG.select_index = rowIndex;
            uCtrl_Switch_Background_Opt.AddBackgroundImages(BG_images, Toast_list, rowIndex);

            JSON_Modified = true;
            PreviewImage();
            FormText();
        }

        private void uCtrl_Switch_Background_Opt_СhangeImage(List<String> BG_images, List<String> Toast_list, int rowIndex)
        {
            if (!PreviewView) return;
            if (Watch_Face == null) return;
            ElementSwitchBackground switchBG = Watch_Face.SwitchBackground;
            if (switchBG == null) switchBG = new ElementSwitchBackground();

            switchBG.bg_list = BG_images;
            switchBG.toast_list = Toast_list;
            switchBG.select_index = rowIndex;
            uCtrl_Switch_Background_Opt.AddBackgroundImages(BG_images, Toast_list, rowIndex);

            JSON_Modified = true;
            PreviewImage();
            FormText();
        }

        private void uCtrl_Switch_Background_Opt_EditToast(string ToastText, int rowIndex)
        {
            if (!PreviewView) return;
            if (Watch_Face == null) return;
            ElementSwitchBackground switchBG = Watch_Face.SwitchBackground;
            if (switchBG == null) switchBG = new ElementSwitchBackground();

            if (rowIndex < switchBG.toast_list.Count) switchBG.toast_list[rowIndex] = ToastText;
            uCtrl_Switch_Background_Opt.AddBackgroundImages(switchBG.bg_list, switchBG.toast_list, rowIndex);

            JSON_Modified = true;
            //PreviewImage();
            FormText();
        }

        //////

        private void uCtrl_Switch_BG_Color_Opt_ValueChanged(object sender, EventArgs eventArgs)
        {
            if (!PreviewView) return;
            if (Watch_Face == null) return;
            ElementSwitchBG_Color switchBG_Color = Watch_Face.SwitchBG_Color;
            if (switchBG_Color == null) switchBG_Color = new ElementSwitchBG_Color();
            Button button = switchBG_Color.Button;

            button.x = (int)uCtrl_Switch_BG_Color_Opt.numericUpDown_buttonX.Value;
            button.y = (int)uCtrl_Switch_BG_Color_Opt.numericUpDown_buttonY.Value;
            button.w = (int)uCtrl_Switch_BG_Color_Opt.numericUpDown_width.Value;
            button.h = (int)uCtrl_Switch_BG_Color_Opt.numericUpDown_height.Value;
            button.radius = (int)uCtrl_Switch_BG_Color_Opt.numericUpDown_radius.Value;
            button.text_size = (int)uCtrl_Switch_BG_Color_Opt.numericUpDown_textSize.Value;
            button.color = ColorToString(uCtrl_Switch_BG_Color_Opt.GetColorText());
            button.text = uCtrl_Switch_BG_Color_Opt.GetText();
            button.press_src = uCtrl_Switch_BG_Color_Opt.GetPressImage();
            button.normal_src = uCtrl_Switch_BG_Color_Opt.GetNormalImage();
            button.normal_color = ColorToString(uCtrl_Switch_BG_Color_Opt.GetColorNormal());
            button.press_color = ColorToString(uCtrl_Switch_BG_Color_Opt.GetColorPress());

            switchBG_Color.use_crown = uCtrl_Switch_BG_Color_Opt.checkBox_use_crown.Checked;
            switchBG_Color.use_in_AOD = uCtrl_Switch_BG_Color_Opt.checkBox_use_in_AOD.Checked;
            switchBG_Color.vibro = uCtrl_Switch_BG_Color_Opt.checkBox_vibro.Checked;

            JSON_Modified = true;
            PreviewImage();
            FormText();
        }
       
        private void uCtrl_Switch_BG_Color_Opt_SelectColor(int rowIndex)
        {
            if (!PreviewView) return;
            if (Watch_Face == null) return;
            ElementSwitchBG_Color switchBG_Color = Watch_Face.SwitchBG_Color;
            if (switchBG_Color == null) switchBG_Color = new ElementSwitchBG_Color();

            switchBG_Color.select_index = rowIndex;

            JSON_Modified = true;
            PreviewImage();
            //FormText();
        }

        private void uCtrl_Switch_BG_Color_Opt_AddColor(List<Color> BG_colors, List<String> Toast_list, int rowIndex)
        {
            if (!PreviewView) return;
            if (Watch_Face == null) return;
            ElementSwitchBG_Color switchBG_Color = Watch_Face.SwitchBG_Color;
            if (switchBG_Color == null) switchBG_Color = new ElementSwitchBG_Color();

            List<string> listColors = BG_colors.Select(ColorToString).ToList();
            switchBG_Color.color_list = listColors;
            Toast_list[rowIndex] = Properties.FormStrings.ToastBG;
            switchBG_Color.toast_list = Toast_list;
            switchBG_Color.select_index = rowIndex;
            uCtrl_Switch_BG_Color_Opt.AddBackgroundColors(BG_colors, Toast_list, rowIndex);

            JSON_Modified = true;
            PreviewImage();
            FormText();
        }

        private void uCtrl_Switch_BG_Color_Opt_DelColor(List<Color> BG_colors, List<String> Toast_list, int rowIndex)
        {
            if (!PreviewView) return;
            if (Watch_Face == null) return;
            ElementSwitchBG_Color switchBG_Color = Watch_Face.SwitchBG_Color;
            if (switchBG_Color == null) switchBG_Color = new ElementSwitchBG_Color();

            List<string> listColors = BG_colors.Select(ColorToString).ToList();
            switchBG_Color.color_list = listColors;
            switchBG_Color.toast_list = Toast_list;
            switchBG_Color.select_index = rowIndex;
            uCtrl_Switch_BG_Color_Opt.AddBackgroundColors(BG_colors, Toast_list, rowIndex);

            JSON_Modified = true;
            PreviewImage();
            FormText();
        }

        private void uCtrl_Switch_BG_Color_Opt_СhangeColor(List<Color> BG_colors, List<String> Toast_list, int rowIndex)
        {
            if (!PreviewView) return;
            if (Watch_Face == null) return;
            ElementSwitchBG_Color switchBG_Color = Watch_Face.SwitchBG_Color;
            if (switchBG_Color == null) switchBG_Color = new ElementSwitchBG_Color();

            List<string> listColors = BG_colors.Select(ColorToString).ToList();
            switchBG_Color.color_list = listColors;
            switchBG_Color.toast_list = Toast_list;
            switchBG_Color.select_index = rowIndex;
            uCtrl_Switch_BG_Color_Opt.AddBackgroundColors(BG_colors, Toast_list, rowIndex);

            JSON_Modified = true;
            PreviewImage();
            FormText();
        }

        private void uCtrl_Switch_BG_Color_Opt_EditToast(string ToastText, int rowIndex)
        {
            if (!PreviewView) return;
            if (Watch_Face == null) return;
            ElementSwitchBG_Color switchBG_Color = Watch_Face.SwitchBG_Color;
            if (switchBG_Color == null) switchBG_Color = new ElementSwitchBG_Color();

            if (rowIndex < switchBG_Color.toast_list.Count) switchBG_Color.toast_list[rowIndex] = ToastText;
            List<Color> listColors = switchBG_Color.color_list.Select(StringToColor).ToList();
            uCtrl_Switch_BG_Color_Opt.AddBackgroundColors(listColors, switchBG_Color.toast_list, rowIndex);

            JSON_Modified = true;
            //PreviewImage();
            FormText();
        }
        //////

        private Color StringToColor(string color)
        {
            Color new_color = Color.Black;
            if (color != null)
            {
                if (color.Length == 18) color = color.Remove(2, 8);
                Color old_color = ColorTranslator.FromHtml(color);
                new_color = Color.FromArgb(255, old_color.R, old_color.G, old_color.B); 
            }
            return new_color;
        }

        private string ColorToString(Color color)
        {
            Color new_color = Color.FromArgb(0, color.R, color.G, color.B);
            string colorStr = ColorTranslator.ToHtml(new_color);
            colorStr = colorStr.Replace("#", "0xFF");
            return colorStr;
        }

        private List<string> GetElementsNameList(List<Object> elements, List<Optional_Types_List> types_list)
        {
            List<string> elementName = new List<string>();

            for (int i = 0; i < elements.Count; i++)
            //foreach (Object element in elements)
            {
                Object element = elements[i];
                //string elementStr = element.ToString();
                //string type = GetTypeFromSring(elementStr);
                string type = element.GetType().Name;
                switch (type)
                {
                    #region DateDay
                    case "ElementDateDay":
                        elementName.Add(Properties.ElementsString.TypeDay);
                        break;
                    #endregion

                    #region DateMonth
                    case "ElementDateMonth":
                        elementName.Add(Properties.ElementsString.TypeMonth);
                        break;
                    #endregion

                    #region DateYear
                    case "ElementDateYear":
                        elementName.Add(Properties.ElementsString.TypeYear);
                        break;
                    #endregion

                    #region DateWeek
                    case "ElementDateWeek":
                        elementName.Add(Properties.ElementsString.TypeWeek);
                        break;
                    #endregion

                    #region ElementBattery
                    case "ElementBattery":
                        elementName.Add(Properties.ElementsString.TypeBattery);
                        break;
                    #endregion

                    #region ElementSteps
                    case "ElementSteps":
                        elementName.Add(Properties.ElementsString.TypeSteps);
                        break;
                    #endregion

                    #region ElementCalories
                    case "ElementCalories":
                        elementName.Add(Properties.ElementsString.TypeCalories);
                        break;
                    #endregion

                    #region ElementHeart
                    case "ElementHeart":
                        elementName.Add(Properties.ElementsString.TypeHeart);
                        break;
                    #endregion

                    #region ElementPAI
                    case "ElementPAI":
                        elementName.Add(Properties.ElementsString.TypePAI);
                        break;
                    #endregion

                    #region ElementDistance
                    case "ElementDistance":
                        elementName.Add(Properties.ElementsString.TypeDistance);
                        break;
                    #endregion

                    #region ElementStand
                    case "ElementStand":
                        elementName.Add(Properties.ElementsString.TypeStand);
                        break;
                    #endregion

                    #region ElementActivity
                    //case "ElementActivity":
                    //    elementName.Add(Properties.ElementsString.TypeActivity);
                    //    break;
                    #endregion

                    #region ElementSpO2
                    case "ElementSpO2":
                        elementName.Add(Properties.ElementsString.TypeSpO2);
                        break;
                    #endregion

                    #region ElementStress
                    case "ElementStress":
                        elementName.Add(Properties.ElementsString.TypeStress);
                        break;
                    #endregion

                    #region ElementFatBurning
                    case "ElementFatBurning":
                        elementName.Add(Properties.ElementsString.TypeFatBurning);
                        break;
                    #endregion


                    #region ElementWeather
                    case "ElementWeather":
                        elementName.Add(Properties.ElementsString.TypeWeather);
                        break;
                    #endregion

                    #region ElementUVIndex
                    case "ElementUVIndex":
                        elementName.Add(Properties.ElementsString.TypeUVIndex);
                        break;
                    #endregion

                    #region ElementHumidity
                    case "ElementHumidity":
                        elementName.Add(Properties.ElementsString.TypeHumidity);
                        break;
                    #endregion

                    #region ElementAltimeter
                    case "ElementAltimeter":
                        elementName.Add(Properties.ElementsString.TypeAltimeter);
                        break;
                    #endregion

                    #region ElementSunrise
                    case "ElementSunrise":
                        elementName.Add(Properties.ElementsString.TypeSunrise);
                        break;
                    #endregion

                    #region ElementWind
                    case "ElementWind":
                        elementName.Add(Properties.ElementsString.TypeWind);
                        break;
                    #endregion

                    #region ElementMoon
                    case "ElementMoon":
                        elementName.Add(Properties.ElementsString.TypeMoon);
                        break;
                    #endregion

                    #region ElementImage
                    case "ElementImage":
                        if (types_list != null && types_list.Count > 0 && types_list.Count > i)
                        {
                            if (types_list[i].type == "APPLIST") elementName.Add(Properties.ElementsString.TypeAppsList);
                            if (types_list[i].type == "SPORTSLIST") elementName.Add(Properties.ElementsString.TypeSportList);
                        }
                        break;
                    #endregion

                    //#region ElementAppsList
                    //case "ElementAppsList":
                    //    elementName.Add(Properties.ElementsString.TypeAppsList);
                    //    break;
                    //#endregion

                    //#region ElementSportList
                    //case "ElementSportList":
                    //    elementName.Add(Properties.ElementsString.TypeSportList);
                    //    break;
                    //#endregion
                }
            }
            return elementName;
        }

        private void uCtrl_Button_Opt_AddButton(int rowIndex)
        {
            if (Watch_Face.Buttons.Button == null) Watch_Face.Buttons.Button = new List<Button>();
            List<Button> buttonsList = Watch_Face.Buttons.Button;
            Button button = new Button();

            button.x = (int)uCtrl_Button_Opt.numericUpDown_buttonX.Value;
            button.y = (int)uCtrl_Button_Opt.numericUpDown_buttonY.Value;
            button.w = (int)uCtrl_Button_Opt.numericUpDown_width.Value;
            button.h = (int)uCtrl_Button_Opt.numericUpDown_height.Value;
            button.radius = (int)uCtrl_Button_Opt.numericUpDown_radius.Value;
            button.text_size = (int)uCtrl_Button_Opt.numericUpDown_textSize.Value;
            button.color = ColorToString(uCtrl_Button_Opt.GetColorText());
            button.text = uCtrl_Button_Opt.GetText();
            button.press_src = uCtrl_Button_Opt.GetPressImage();
            button.normal_src = uCtrl_Button_Opt.GetNormalImage();
            button.normal_color = ColorToString(uCtrl_Button_Opt.GetColorNormal());
            button.press_color = ColorToString(uCtrl_Button_Opt.GetColorPress());
            button.visible = true;

            //if (rowIndex < 0 || rowIndex >= buttonsList.Count) buttonsList.Add(button);
            //else buttonsList.Insert(rowIndex, button);
            //buttonsList.Add(button);

            //if (rowIndex < 0 || rowIndex == buttonsList.Count - 1) buttonsList.Add(button);
            //else buttonsList.Insert(rowIndex, button);
            //rowIndex++;

            if (rowIndex < 0 || rowIndex >= buttonsList.Count - 1) { 
                buttonsList.Add(button);
                rowIndex = buttonsList.Count - 1;
            }
            else buttonsList.Insert(++rowIndex, button);

            List<String> scriptClickList = ButtonClickScriptToString(buttonsList);
            List<String> scriptLongPressList = ButtonLongPressScriptToString(buttonsList);
            List<bool> visibleList = ButtonVisibleList(buttonsList);
            uCtrl_Button_Opt.UpdateButtonsList(scriptClickList, scriptLongPressList, visibleList, rowIndex);

            JSON_Modified = true;
            PreviewImage();
            FormText();
        }

        private void uCtrl_Button_Opt_DelButton(int rowIndex)
        {
            if (Watch_Face.Buttons.Button == null) Watch_Face.Buttons.Button = new List<Button>();
            List<Button> buttonsList = Watch_Face.Buttons.Button;

            if (rowIndex >= 0 || rowIndex < buttonsList.Count) buttonsList.RemoveAt(rowIndex);
            if (rowIndex >= buttonsList.Count) rowIndex = buttonsList.Count - 1;

            List<String> scriptClickList = ButtonClickScriptToString(buttonsList);
            List<String> scriptLongPressList = ButtonLongPressScriptToString(buttonsList);
            List<bool> visibleList = ButtonVisibleList(buttonsList);
            uCtrl_Button_Opt.UpdateButtonsList(scriptClickList, scriptLongPressList, visibleList, rowIndex);

            JSON_Modified = true;
            PreviewImage();
            FormText();
        }

        private List<String> ButtonClickScriptToString(List<Button> buttonsList)
        {
            List<String> scriptList = new List<String>();
            if (buttonsList == null || buttonsList.Count == 0) return scriptList;
            foreach (Button button in buttonsList)
            {
                scriptList.Add(button.click_func);
            }
            return scriptList;
        }

        private List<String> ButtonLongPressScriptToString(List<Button> buttonsList)
        {
            List<String> scriptList = new List<String>();
            if (buttonsList == null || buttonsList.Count == 0) return scriptList;
            foreach (Button button in buttonsList)
            {
                scriptList.Add(button.longpress_func);
            }
            return scriptList;
        }

        private List<bool> ButtonVisibleList(List<Button> buttonsList)
        {
            List<bool> visibleList = new List<bool>();
            if (buttonsList == null || buttonsList.Count == 0) return visibleList;
            foreach (Button button in buttonsList)
            {
                visibleList.Add(button.visible);
            }
            return visibleList;
        }

        ///////////
        private void uCtrl_Text_Opt_WidgetProperty_Copy(object sender, EventArgs eventArgs)
        {
            if (WidgetProperty.ContainsKey("hmUI_widget_IMG_NUMBER")) WidgetProperty.Remove("hmUI_widget_IMG_NUMBER");
            hmUI_widget_IMG_NUMBER img_number = new hmUI_widget_IMG_NUMBER();

            img_number.align = uCtrl_Text_Opt.GetAlignment();
            img_number.follow = uCtrl_Text_Opt.checkBox_follow.Checked;
            img_number.icon = uCtrl_Text_Opt.GetIcon();
            img_number.iconPosX = (int)uCtrl_Text_Opt.numericUpDown_iconX.Value;
            img_number.iconPosY = (int)uCtrl_Text_Opt.numericUpDown_iconY.Value;
            img_number.img_First = uCtrl_Text_Opt.GetImage();
            img_number.imageX = (int)uCtrl_Text_Opt.numericUpDown_imageX.Value;
            img_number.imageY = (int)uCtrl_Text_Opt.numericUpDown_imageY.Value;
            img_number.space = (int)uCtrl_Text_Opt.numericUpDown_spacing.Value;
            img_number.angle = (int)uCtrl_Text_Opt.numericUpDown_angle.Value;
            img_number.alpha = (int)uCtrl_Text_Opt.numericUpDown_Alpha.Value;
            img_number.icon_alpha = (int)uCtrl_Text_Opt.numericUpDown_iconAlpha.Value;
            if (!uCtrl_Text_Opt.Altitude) img_number.dot_image = uCtrl_Text_Opt.GetImageDecimalPoint();
            else img_number.negative_image = uCtrl_Text_Opt.GetImageDecimalPoint();
            img_number.unit = uCtrl_Text_Opt.GetUnit();
            img_number.imperial_unit = uCtrl_Text_Opt.GetUnitMile();
            img_number.invalid_image = uCtrl_Text_Opt.GetImageError();
            img_number.zero = uCtrl_Text_Opt.checkBox_addZero.Checked;

            WidgetProperty.Add("hmUI_widget_IMG_NUMBER", img_number);
            uCtrl_Text_Opt.WidgetProperty = WidgetProperty;
        }

        private void uCtrl_Text_Opt_WidgetProperty_Paste(object sender, EventArgs eventArgs)
        {
            if (!WidgetProperty.ContainsKey("hmUI_widget_IMG_NUMBER")) return;
            Object obj = null;
            WidgetProperty.TryGetValue("hmUI_widget_IMG_NUMBER", out obj);
            if (obj == null) return;
            hmUI_widget_IMG_NUMBER img_number = (hmUI_widget_IMG_NUMBER)obj;

            PreviewView = false;

            //bool _dastance = uCtrl_Text_Opt.Distance;
            //bool sunrise = uCtrl_Text_Opt.Sunrise;
            bool _altitude = uCtrl_Text_Opt.Altitude;
            bool _follow = uCtrl_Text_Opt.Follow;
            bool _imageError = uCtrl_Text_Opt.ImageError;
            bool _optionalSymbol = uCtrl_Text_Opt.OptionalSymbol;
            bool _padingZero = uCtrl_Text_Opt.PaddingZero;
            bool _angleVisible = uCtrl_Text_Opt.AngleVisible;

            if (img_number.img_First != null) uCtrl_Text_Opt.SetImage(img_number.img_First);
            uCtrl_Text_Opt.numericUpDown_imageX.Value = img_number.imageX;
            uCtrl_Text_Opt.numericUpDown_imageY.Value = img_number.imageY;

            uCtrl_Text_Opt.SetIcon(img_number.icon);
            uCtrl_Text_Opt.numericUpDown_iconX.Value = img_number.iconPosX;
            uCtrl_Text_Opt.numericUpDown_iconY.Value = img_number.iconPosY;

            uCtrl_Text_Opt.SetUnit(img_number.unit);
            uCtrl_Text_Opt.SetUnitMile(img_number.imperial_unit);
            if (_imageError) uCtrl_Text_Opt.SetImageError(img_number.invalid_image);
            if (_optionalSymbol)
            {
                if (!_altitude) uCtrl_Text_Opt.SetImageDecimalPoint(img_number.dot_image);
                else uCtrl_Text_Opt.SetImageDecimalPoint(img_number.negative_image); 
            }
            uCtrl_Text_Opt.numericUpDown_spacing.Value = img_number.space;
            if (_angleVisible && SelectedModel.versionOS >= 2) uCtrl_Text_Opt.numericUpDown_angle.Value = img_number.angle;
            uCtrl_Text_Opt.numericUpDown_Alpha.Value = img_number.alpha;
            uCtrl_Text_Opt.numericUpDown_iconAlpha.Value = img_number.icon_alpha;

            uCtrl_Text_Opt.SetAlignment(img_number.align);

            if (_padingZero) uCtrl_Text_Opt.checkBox_addZero.Checked = img_number.zero;
            if (_follow) uCtrl_Text_Opt.checkBox_follow.Checked = img_number.follow;

            PreviewView = true;
            uCtrl_Text_Opt_ValueChanged(sender, eventArgs);
        }

        private void uCtrl_Pointer_Opt_WidgetProperty_Copy(object sender, EventArgs eventArgs)
        {
            if (WidgetProperty.ContainsKey("hmUI_widget_IMG_POINTER")) WidgetProperty.Remove("hmUI_widget_IMG_POINTER");
            hmUI_widget_IMG_POINTER img_pointer = new hmUI_widget_IMG_POINTER();

            img_pointer.src = uCtrl_Pointer_Opt.GetPointerImage();
            img_pointer.center_x = (int)uCtrl_Pointer_Opt.numericUpDown_pointer_X.Value;
            img_pointer.center_y = (int)uCtrl_Pointer_Opt.numericUpDown_pointer_Y.Value;

            img_pointer.pos_x = (int)uCtrl_Pointer_Opt.numericUpDown_pointer_offset_X.Value;
            img_pointer.pos_y = (int)uCtrl_Pointer_Opt.numericUpDown_pointer_offset_Y.Value;

            img_pointer.start_angle = (int)uCtrl_Pointer_Opt.numericUpDown_pointer_startAngle.Value;
            img_pointer.end_angle = (int)uCtrl_Pointer_Opt.numericUpDown_pointer_endAngle.Value;

            img_pointer.cover_path = uCtrl_Pointer_Opt.GetPointerImageCentr();
            img_pointer.cover_x = (int)uCtrl_Pointer_Opt.numericUpDown_pointer_centr_X.Value;
            img_pointer.cover_y = (int)uCtrl_Pointer_Opt.numericUpDown_pointer_centr_Y.Value;

            img_pointer.scale = uCtrl_Pointer_Opt.GetPointerImageBackground();
            img_pointer.scale_x = (int)uCtrl_Pointer_Opt.numericUpDown_pointer_background_X.Value;
            img_pointer.scale_y = (int)uCtrl_Pointer_Opt.numericUpDown_pointer_background_Y.Value;

            WidgetProperty.Add("hmUI_widget_IMG_POINTER", img_pointer);
            uCtrl_Pointer_Opt.WidgetProperty = WidgetProperty;
        }

        private void uCtrl_Pointer_Opt_WidgetProperty_Paste(object sender, EventArgs eventArgs)
        {
            if (!WidgetProperty.ContainsKey("hmUI_widget_IMG_POINTER")) return;
            Object obj = null;
            WidgetProperty.TryGetValue("hmUI_widget_IMG_POINTER", out obj);
            if (obj == null) return;
            hmUI_widget_IMG_POINTER img_pointer = (hmUI_widget_IMG_POINTER)obj;

            PreviewView = false;

            if (img_pointer == null)
            {
                PreviewView = true;
                return;
            }
            if (img_pointer.src != null)
                uCtrl_Pointer_Opt.SetPointerImage(img_pointer.src);
            uCtrl_Pointer_Opt.numericUpDown_pointer_X.Value = img_pointer.center_x;
            uCtrl_Pointer_Opt.numericUpDown_pointer_Y.Value = img_pointer.center_y;
            uCtrl_Pointer_Opt.numericUpDown_pointer_offset_X.Value = img_pointer.pos_x;
            uCtrl_Pointer_Opt.numericUpDown_pointer_offset_Y.Value = img_pointer.pos_y;

            if (img_pointer.cover_path != null)
                uCtrl_Pointer_Opt.SetPointerImageCentr(img_pointer.cover_path);
            uCtrl_Pointer_Opt.numericUpDown_pointer_centr_X.Value = img_pointer.cover_x;
            uCtrl_Pointer_Opt.numericUpDown_pointer_centr_Y.Value = img_pointer.cover_y;

            if (img_pointer.scale != null)
                uCtrl_Pointer_Opt.SetPointerImageBackground(img_pointer.scale);
            uCtrl_Pointer_Opt.numericUpDown_pointer_background_X.Value = img_pointer.scale_x;
            uCtrl_Pointer_Opt.numericUpDown_pointer_background_Y.Value = img_pointer.scale_y;

            uCtrl_Pointer_Opt.numericUpDown_pointer_startAngle.Value = img_pointer.start_angle;
            uCtrl_Pointer_Opt.numericUpDown_pointer_endAngle.Value = img_pointer.end_angle;

            PreviewView = true;
            uCtrl_Pointer_Opt_ValueChanged(sender, eventArgs);
        }

        private void uCtrl_Images_Opt_WidgetProperty_Copy(object sender, EventArgs eventArgs)
        {
            if (WidgetProperty.ContainsKey("hmUI_widget_IMG_LEVEL")) WidgetProperty.Remove("hmUI_widget_IMG_LEVEL");
            hmUI_widget_IMG_LEVEL img_level = new hmUI_widget_IMG_LEVEL();

            img_level.img_First = uCtrl_Images_Opt.GetImage();
            img_level.X = (int)uCtrl_Images_Opt.numericUpDown_imageX.Value;
            img_level.Y = (int)uCtrl_Images_Opt.numericUpDown_imageY.Value;
            img_level.image_length = (int)uCtrl_Images_Opt.numericUpDown_pictures_count.Value;
            img_level.alpha = (int)uCtrl_Images_Opt.numericUpDown_Alpha.Value;
            img_level.shortcut = uCtrl_Images_Opt.checkBox_shortcut.Checked;

            WidgetProperty.Add("hmUI_widget_IMG_LEVEL", img_level);
            uCtrl_Images_Opt.WidgetProperty = WidgetProperty;
        }

        private void uCtrl_Images_Opt_WidgetProperty_Paste(object sender, EventArgs eventArgs)
        {
            if (!WidgetProperty.ContainsKey("hmUI_widget_IMG_LEVEL")) return;
            Object obj = null;
            WidgetProperty.TryGetValue("hmUI_widget_IMG_LEVEL", out obj);
            if (obj == null) return;
            hmUI_widget_IMG_LEVEL img_level = (hmUI_widget_IMG_LEVEL)obj;

            PreviewView = false;

            if (img_level == null)
            {
                PreviewView = true;
                return;
            }
            if (img_level.img_First != null)
                uCtrl_Images_Opt.SetImage(img_level.img_First);
            uCtrl_Images_Opt.numericUpDown_imageX.Value = img_level.X;
            uCtrl_Images_Opt.numericUpDown_imageY.Value = img_level.Y;
            if (img_level.image_length > 0)
                uCtrl_Images_Opt.numericUpDown_pictures_count.Value = img_level.image_length;
            //if (!imagesCountEnable) uCtrl_Images_Opt.numericUpDown_pictures_count.Value = imagesCount;
            uCtrl_Images_Opt.numericUpDown_Alpha.Value = img_level.alpha;

            uCtrl_Images_Opt.checkBox_shortcut.Checked = img_level.shortcut;

            PreviewView = true;
            uCtrl_Images_Opt_ValueChanged(sender, eventArgs);
        }

        private void uCtrl_Circle_Scale_Opt_WidgetProperty_Copy(object sender, EventArgs eventArgs)
        {
            if (WidgetProperty.ContainsKey("Circle_Scale")) WidgetProperty.Remove("Circle_Scale");
            Circle_Scale circle_scale = new Circle_Scale();

            circle_scale.center_x = (int)uCtrl_Circle_Scale_Opt.numericUpDown_scaleCircleX.Value;
            circle_scale.center_y = (int)uCtrl_Circle_Scale_Opt.numericUpDown_scaleCircleY.Value;

            circle_scale.radius = (int)(int)Math.Abs(uCtrl_Circle_Scale_Opt.numericUpDown_scaleCircle_radius.Value);
            circle_scale.line_width = (int)(int)Math.Abs(uCtrl_Circle_Scale_Opt.numericUpDown_scaleCircle_width.Value);

            circle_scale.start_angle = (int)uCtrl_Circle_Scale_Opt.numericUpDown_scaleCircle_startAngle.Value;
            circle_scale.end_angle = (int)uCtrl_Circle_Scale_Opt.numericUpDown_scaleCircle_endAngle.Value;
            circle_scale.alpha = (int)uCtrl_Circle_Scale_Opt.numericUpDown_Alpha.Value;

            circle_scale.line_cap = uCtrl_Circle_Scale_Opt.GetLineCap();

            circle_scale.color = ColorToString(uCtrl_Circle_Scale_Opt.GetColorScale());

            circle_scale.mirror = uCtrl_Circle_Scale_Opt.checkBox_mirror.Checked;
            circle_scale.inversion = uCtrl_Circle_Scale_Opt.checkBox_inversion.Checked;

            WidgetProperty.Add("Circle_Scale", circle_scale);
            uCtrl_Circle_Scale_Opt.WidgetProperty = WidgetProperty;
        }

        private void uCtrl_Circle_Scale_Opt_WidgetProperty_Paste(object sender, EventArgs eventArgs)
        {
            if (!WidgetProperty.ContainsKey("Circle_Scale")) return;
            Object obj = null;
            WidgetProperty.TryGetValue("Circle_Scale", out obj);
            if (obj == null) return;
            Circle_Scale circle_scale = (Circle_Scale)obj;

            PreviewView = false;

            if (circle_scale == null)
            {
                PreviewView = true;
                return;
            }
            uCtrl_Circle_Scale_Opt.numericUpDown_scaleCircleX.Value = circle_scale.center_x;
            uCtrl_Circle_Scale_Opt.numericUpDown_scaleCircleY.Value = circle_scale.center_y;
            uCtrl_Circle_Scale_Opt.numericUpDown_scaleCircle_radius.Value = circle_scale.radius;
            uCtrl_Circle_Scale_Opt.numericUpDown_scaleCircle_width.Value = circle_scale.line_width;

            uCtrl_Circle_Scale_Opt.numericUpDown_scaleCircle_startAngle.Value = circle_scale.start_angle;
            uCtrl_Circle_Scale_Opt.numericUpDown_scaleCircle_endAngle.Value = circle_scale.end_angle;
            uCtrl_Circle_Scale_Opt.numericUpDown_Alpha.Value = circle_scale.alpha;

            uCtrl_Circle_Scale_Opt.SetLineCap(circle_scale.line_cap);

            uCtrl_Circle_Scale_Opt.SetColorScale(StringToColor(circle_scale.color));

            uCtrl_Circle_Scale_Opt.checkBox_mirror.Checked = circle_scale.mirror;
            uCtrl_Circle_Scale_Opt.checkBox_inversion.Checked = circle_scale.inversion;

            PreviewView = true;
            uCtrl_Circle_Scale_Opt_ValueChanged(sender, eventArgs);
        }

        private void uCtrl_Linear_Scale_Opt_WidgetProperty_Copy(object sender, EventArgs eventArgs)
        {
            if (WidgetProperty.ContainsKey("Linear_Scale")) WidgetProperty.Remove("Linear_Scale");
            Linear_Scale linear_scale = new Linear_Scale();

            linear_scale.pointer = uCtrl_Linear_Scale_Opt.GetImagePointer();
            linear_scale.color = ColorToString(uCtrl_Linear_Scale_Opt.GetColorScale());

            linear_scale.start_x = (int)uCtrl_Linear_Scale_Opt.numericUpDown_scaleLinearX.Value;
            linear_scale.start_y = (int)uCtrl_Linear_Scale_Opt.numericUpDown_scaleLinearY.Value;

            linear_scale.lenght = (int)uCtrl_Linear_Scale_Opt.numericUpDown_scaleLinear_length.Value;
            linear_scale.line_width = (int)Math.Abs(uCtrl_Linear_Scale_Opt.numericUpDown_scaleLinear_width.Value);
            linear_scale.alpha = (int)uCtrl_Linear_Scale_Opt.numericUpDown_Alpha.Value;
            linear_scale.line_cap = uCtrl_Linear_Scale_Opt.GetLineCap();

            linear_scale.vertical = uCtrl_Linear_Scale_Opt.radioButton_vertical.Checked;

            linear_scale.mirror = uCtrl_Linear_Scale_Opt.checkBox_mirror.Checked;
            linear_scale.inversion = uCtrl_Linear_Scale_Opt.checkBox_inversion.Checked;

            WidgetProperty.Add("Linear_Scale", linear_scale);
            uCtrl_Linear_Scale_Opt.WidgetProperty = WidgetProperty;
        }

        private void uCtrl_Linear_Scale_Opt_WidgetProperty_Paste(object sender, EventArgs eventArgs)
        {
            if (!WidgetProperty.ContainsKey("Linear_Scale")) return;
            Object obj = null;
            WidgetProperty.TryGetValue("Linear_Scale", out obj);
            if (obj == null) return;
            Linear_Scale linear_scale = (Linear_Scale)obj;

            PreviewView = false;

            if (linear_scale == null)
            {
                PreviewView = true;
                return;
            }

            if (linear_scale.color != null)
            {
                uCtrl_Linear_Scale_Opt.SetColorScale(StringToColor(linear_scale.color));
            }
            if (linear_scale.pointer != null)
            {
                uCtrl_Linear_Scale_Opt.SetImagePointer(linear_scale.pointer);
            }

            uCtrl_Linear_Scale_Opt.numericUpDown_scaleLinearX.Value = linear_scale.start_x;
            uCtrl_Linear_Scale_Opt.numericUpDown_scaleLinearY.Value = linear_scale.start_y;

            uCtrl_Linear_Scale_Opt.numericUpDown_scaleLinear_length.Value = linear_scale.lenght;
            uCtrl_Linear_Scale_Opt.numericUpDown_scaleLinear_width.Value = linear_scale.line_width;
            uCtrl_Linear_Scale_Opt.numericUpDown_Alpha.Value = linear_scale.alpha;
            uCtrl_Linear_Scale_Opt.SetLineCap(linear_scale.line_cap);

            uCtrl_Linear_Scale_Opt.SetColorScale(StringToColor(linear_scale.color));

            uCtrl_Linear_Scale_Opt.checkBox_mirror.Checked = linear_scale.mirror;
            uCtrl_Linear_Scale_Opt.checkBox_inversion.Checked = linear_scale.inversion;

            uCtrl_Linear_Scale_Opt.radioButton_horizontal.Checked = !linear_scale.vertical;
            uCtrl_Linear_Scale_Opt.radioButton_vertical.Checked = linear_scale.vertical;

            PreviewView = true;
            uCtrl_Linear_Scale_Opt_ValueChanged(sender, eventArgs);
        }

        private void uCtrl_Shortcut_Opt_WidgetProperty_Copy(object sender, EventArgs eventArgs)
        {
            if (WidgetProperty.ContainsKey("hmUI_widget_IMG_CLICK")) WidgetProperty.Remove("hmUI_widget_IMG_CLICK");
            hmUI_widget_IMG_CLICK shortcut = new hmUI_widget_IMG_CLICK();

            shortcut.src = uCtrl_Shortcut_Opt.GetImage();
            shortcut.x = (int)uCtrl_Shortcut_Opt.numericUpDown_imageX.Value;
            shortcut.y = (int)uCtrl_Shortcut_Opt.numericUpDown_imageY.Value;
            shortcut.h = (int)uCtrl_Shortcut_Opt.numericUpDown_height.Value;
            shortcut.w = (int)uCtrl_Shortcut_Opt.numericUpDown_width.Value;

            WidgetProperty.Add("hmUI_widget_IMG_CLICK", shortcut);
            uCtrl_Shortcut_Opt.WidgetProperty = WidgetProperty;
        }

        private void uCtrl_Shortcut_Opt_WidgetProperty_Paste(object sender, EventArgs eventArgs)
        {
            if (!WidgetProperty.ContainsKey("hmUI_widget_IMG_CLICK")) return;
            Object obj = null;
            WidgetProperty.TryGetValue("hmUI_widget_IMG_CLICK", out obj);
            if (obj == null) return;
            hmUI_widget_IMG_CLICK img_click = (hmUI_widget_IMG_CLICK)obj;

            PreviewView = false;

            if (img_click == null)
            {
                PreviewView = true;
                return;
            }

            if (img_click.src != null)
                uCtrl_Shortcut_Opt.SetImage(img_click.src);
            uCtrl_Shortcut_Opt.numericUpDown_imageX.Value = img_click.x;
            uCtrl_Shortcut_Opt.numericUpDown_imageY.Value = img_click.y;
            uCtrl_Shortcut_Opt.numericUpDown_width.Value = img_click.w;
            uCtrl_Shortcut_Opt.numericUpDown_height.Value = img_click.h;

            PreviewView = true;
            uCtrl_Shortcut_Opt_ValueChanged(sender, eventArgs);
        }

        private void uCtrl_Text_Weather_Opt_WidgetProperty_Copy(object sender, EventArgs eventArgs)
        {
            if (WidgetProperty.ContainsKey("hmUI_widget_IMG_NUMBER")) WidgetProperty.Remove("hmUI_widget_IMG_NUMBER");
            hmUI_widget_IMG_NUMBER img_number = new hmUI_widget_IMG_NUMBER();

            img_number.align = uCtrl_Text_Weather_Opt.GetAlignment();
            img_number.icon = uCtrl_Text_Weather_Opt.GetIcon();
            img_number.iconPosX = (int)uCtrl_Text_Weather_Opt.numericUpDown_iconX.Value;
            img_number.iconPosY = (int)uCtrl_Text_Weather_Opt.numericUpDown_iconY.Value;
            img_number.img_First = uCtrl_Text_Weather_Opt.GetImage();
            img_number.imageX = (int)uCtrl_Text_Weather_Opt.numericUpDown_imageX.Value;
            img_number.imageY = (int)uCtrl_Text_Weather_Opt.numericUpDown_imageY.Value;
            img_number.space = (int)uCtrl_Text_Weather_Opt.numericUpDown_spacing.Value;
            img_number.angle = (int)uCtrl_Text_Weather_Opt.numericUpDown_angle.Value;
            img_number.alpha = (int)uCtrl_Text_Weather_Opt.numericUpDown_Alpha.Value;
            img_number.icon_alpha = (int)uCtrl_Text_Weather_Opt.numericUpDown_iconAlpha.Value;
            img_number.unit = uCtrl_Text_Weather_Opt.GetUnit_C();
            img_number.imperial_unit = uCtrl_Text_Weather_Opt.GetUnit_F();
            img_number.negative_image = uCtrl_Text_Weather_Opt.GetImageMinus();
            img_number.invalid_image = uCtrl_Text_Weather_Opt.GetImageError();
            img_number.dot_image = uCtrl_Text_Weather_Opt.GetSeparator();

            WidgetProperty.Add("hmUI_widget_IMG_NUMBER", img_number);
            uCtrl_Text_Weather_Opt.WidgetProperty = WidgetProperty;
        }

        private void uCtrl_Text_Weather_Opt_WidgetProperty_Paste(object sender, EventArgs eventArgs)
        {
            if (!WidgetProperty.ContainsKey("hmUI_widget_IMG_NUMBER")) return;
            Object obj = null;
            WidgetProperty.TryGetValue("hmUI_widget_IMG_NUMBER", out obj);
            if (obj == null) return;
            hmUI_widget_IMG_NUMBER img_number = (hmUI_widget_IMG_NUMBER)obj;

            PreviewView = false;

            if (img_number == null)
            {
                PreviewView = true;
                return;
            }
            if (img_number.img_First != null)
                uCtrl_Text_Weather_Opt.SetImage(img_number.img_First);
            uCtrl_Text_Weather_Opt.numericUpDown_imageX.Value = img_number.imageX;
            uCtrl_Text_Weather_Opt.numericUpDown_imageY.Value = img_number.imageY;

            uCtrl_Text_Weather_Opt.SetIcon(img_number.icon);
            uCtrl_Text_Weather_Opt.numericUpDown_iconX.Value = img_number.iconPosX;
            uCtrl_Text_Weather_Opt.numericUpDown_iconY.Value = img_number.iconPosY;

            uCtrl_Text_Weather_Opt.SetUnit_C(img_number.unit);
            uCtrl_Text_Weather_Opt.SetUnit_F(img_number.imperial_unit);
            uCtrl_Text_Weather_Opt.SetImageError(img_number.invalid_image);
            uCtrl_Text_Weather_Opt.SetImageMinus(img_number.negative_image);
            uCtrl_Text_Weather_Opt.SetSeparator(img_number.dot_image);
            uCtrl_Text_Weather_Opt.numericUpDown_spacing.Value = img_number.space;
            uCtrl_Text_Weather_Opt.numericUpDown_angle.Value = img_number.angle;
            uCtrl_Text_Weather_Opt.numericUpDown_Alpha.Value = img_number.alpha;
            uCtrl_Text_Weather_Opt.numericUpDown_iconAlpha.Value = img_number.icon_alpha;

            uCtrl_Text_Weather_Opt.SetAlignment(img_number.align);

            PreviewView = true;
            uCtrl_Text_Weather_Opt_ValueChanged(sender, eventArgs);
        }

        private void uCtrl_Text_SystemFont_Opt_WidgetProperty_Copy(object sender, EventArgs eventArgs)
        {
            if (WidgetProperty.ContainsKey("hmUI_widget_TEXT")) WidgetProperty.Remove("hmUI_widget_TEXT");
            hmUI_widget_TEXT systemFont = new hmUI_widget_TEXT();

            systemFont.x = (int)uCtrl_Text_SystemFont_Opt.numericUpDown_X.Value;
            systemFont.y = (int)uCtrl_Text_SystemFont_Opt.numericUpDown_Y.Value;
            systemFont.h = (int)uCtrl_Text_SystemFont_Opt.numericUpDown_Height.Value;
            systemFont.w = (int)uCtrl_Text_SystemFont_Opt.numericUpDown_Width.Value;

            systemFont.text_size = (int)uCtrl_Text_SystemFont_Opt.numericUpDown_Size.Value;
            systemFont.char_space = (int)uCtrl_Text_SystemFont_Opt.numericUpDown_Spacing.Value;
            systemFont.line_space = (int)uCtrl_Text_SystemFont_Opt.numericUpDown_LineSpace.Value;
            systemFont.alpha = (int)uCtrl_Text_SystemFont_Opt.numericUpDown_Alpha.Value;

            systemFont.color = ColorToString(uCtrl_Text_SystemFont_Opt.GetColorText());

            systemFont.align_h = uCtrl_Text_SystemFont_Opt.GetHorizontalAlignment();
            systemFont.align_v = uCtrl_Text_SystemFont_Opt.GetVerticalAlignment();
            systemFont.text_style = uCtrl_Text_SystemFont_Opt.GetTextStyle();

            systemFont.centreHorizontally = uCtrl_Text_SystemFont_Opt.checkBox_CentreHorizontally.Checked;
            systemFont.centreVertically = uCtrl_Text_SystemFont_Opt.checkBox_CentreVertically.Checked;

            systemFont.font = uCtrl_Text_SystemFont_Opt.GetFont();
            systemFont.unit_string = uCtrl_Text_SystemFont_Opt.GetUnitText();

            systemFont.padding = uCtrl_Text_SystemFont_Opt.checkBox_addZero.Checked;
            systemFont.unit_type = uCtrl_Text_SystemFont_Opt.GetUnitType();
            systemFont.unit_end = uCtrl_Text_SystemFont_Opt.GetUnitEnd();

            WidgetProperty.Add("hmUI_widget_TEXT", systemFont);
            uCtrl_Text_SystemFont_Opt.WidgetProperty = WidgetProperty;
        }

        private void uCtrl_Text_SystemFont_Opt_WidgetProperty_Paste(object sender, EventArgs eventArgs)
        {
            if (!WidgetProperty.ContainsKey("hmUI_widget_TEXT")) return;
            Object obj = null;
            WidgetProperty.TryGetValue("hmUI_widget_TEXT", out obj);
            if (obj == null) return;
            hmUI_widget_TEXT system_font = (hmUI_widget_TEXT)obj;

            PreviewView = false;

            uCtrl_Text_SystemFont_Opt.numericUpDown_X.Value = system_font.x;
            uCtrl_Text_SystemFont_Opt.numericUpDown_Y.Value = system_font.y;
            uCtrl_Text_SystemFont_Opt.numericUpDown_Width.Value = system_font.w;
            uCtrl_Text_SystemFont_Opt.numericUpDown_Height.Value = system_font.h;

            uCtrl_Text_SystemFont_Opt.numericUpDown_Size.Value = system_font.text_size;
            uCtrl_Text_SystemFont_Opt.numericUpDown_Spacing.Value = system_font.char_space;
            uCtrl_Text_SystemFont_Opt.numericUpDown_LineSpace.Value = system_font.line_space;
            uCtrl_Text_SystemFont_Opt.numericUpDown_Alpha.Value = system_font.alpha;

            uCtrl_Text_SystemFont_Opt.SetColorText(StringToColor(system_font.color));

            uCtrl_Text_SystemFont_Opt.SetHorizontalAlignment(system_font.align_h);
            uCtrl_Text_SystemFont_Opt.SetVerticalAlignment(system_font.align_v);
            uCtrl_Text_SystemFont_Opt.SetTextStyle(system_font.text_style);

            uCtrl_Text_SystemFont_Opt.checkBox_CentreHorizontally.Checked = system_font.centreHorizontally;
            uCtrl_Text_SystemFont_Opt.checkBox_CentreVertically.Checked = system_font.centreVertically;

            uCtrl_Text_SystemFont_Opt.SetFont(system_font.font);
            uCtrl_Text_SystemFont_Opt.SetUnitText(system_font.unit_string);

            uCtrl_Text_SystemFont_Opt.checkBox_addZero.Checked = system_font.padding;
            uCtrl_Text_SystemFont_Opt.SetUnitType(system_font.unit_type);
            uCtrl_Text_SystemFont_Opt.SetUnitEnd(system_font.unit_end);

            PreviewView = true;
            uCtrl_Text_SystemFont_Opt_ValueChanged(sender, eventArgs);
        }

        private void uCtrl_Text_Circle_Opt_WidgetProperty_Copy(object sender, EventArgs eventArgs)
        {
            if (WidgetProperty.ContainsKey("Text_Circle")) WidgetProperty.Remove("Text_Circle");
            Text_Circle text_circle = new Text_Circle();

            text_circle.img_First = uCtrl_Text_Circle_Opt.GetImage();
            text_circle.unit = uCtrl_Text_Circle_Opt.GetUnit();
            text_circle.imperial_unit = uCtrl_Text_Circle_Opt.GetUnitImperial();
            text_circle.dot_image = uCtrl_Text_Circle_Opt.GetImageDecimalPoint();
            text_circle.error_image = uCtrl_Text_Circle_Opt.GetImageError();
            text_circle.separator_image = uCtrl_Text_Circle_Opt.GetSeparator();

            text_circle.circle_center_X = (int)uCtrl_Text_Circle_Opt.numericUpDown_centr_X.Value;
            text_circle.circle_center_Y = (int)uCtrl_Text_Circle_Opt.numericUpDown_centr_Y.Value;
            text_circle.radius = (int)uCtrl_Text_Circle_Opt.numericUpDown_radius.Value;
            text_circle.angle = (int)uCtrl_Text_Circle_Opt.numericUpDown_angle.Value;
            text_circle.char_space_angle = (int)uCtrl_Text_Circle_Opt.numericUpDown_spacing.Value;

            text_circle.zero = uCtrl_Text_Circle_Opt.checkBox_addZero.Checked;
            text_circle.reverse_direction = uCtrl_Text_Circle_Opt.checkBox_reverse_direction.Checked;
            text_circle.unit_in_alignment = uCtrl_Text_Circle_Opt.checkBox_unit_in_alignment.Checked;

            text_circle.vertical_alignment = uCtrl_Text_Circle_Opt.GetVerticalAlignment();
            text_circle.horizontal_alignment = uCtrl_Text_Circle_Opt.GetHorizontalAlignment();

            //text_circle.image_width = uCtrl_Text_Circle_Opt.image_width;
            //text_circle.image_height = uCtrl_Text_Circle_Opt.image_height;
            //text_circle.unit_width = uCtrl_Text_Circle_Opt.unit_width;
            //text_circle.error_width = uCtrl_Text_Circle_Opt.error_width;

            WidgetProperty.Add("Text_Circle", text_circle);
            uCtrl_Text_Circle_Opt.WidgetProperty = WidgetProperty;
        }

        private void uCtrl_Text_Circle_Opt_WidgetProperty_Paste(object sender, EventArgs eventArgs)
        {
            if (!WidgetProperty.ContainsKey("Text_Circle")) return;
            Object obj = null;
            WidgetProperty.TryGetValue("Text_Circle", out obj);
            if (obj == null) return;
            Text_Circle text_circle = (Text_Circle)obj;

            PreviewView = false;

            if (text_circle == null)
            {
                PreviewView = true;
                return;
            }
            if (text_circle.img_First != null)
                uCtrl_Text_Circle_Opt.SetImage(text_circle.img_First);
            uCtrl_Text_Circle_Opt.numericUpDown_centr_X.Value = text_circle.circle_center_X;
            uCtrl_Text_Circle_Opt.numericUpDown_centr_Y.Value = text_circle.circle_center_Y;

            uCtrl_Text_Circle_Opt.numericUpDown_angle.Value = text_circle.angle;
            uCtrl_Text_Circle_Opt.numericUpDown_radius.Value = text_circle.radius;
            uCtrl_Text_Circle_Opt.numericUpDown_spacing.Value = text_circle.char_space_angle;

            uCtrl_Text_Circle_Opt.SetUnit(text_circle.unit);
            uCtrl_Text_Circle_Opt.SetUnitImperial(text_circle.imperial_unit);
            uCtrl_Text_Circle_Opt.SetImageError(text_circle.error_image);
            uCtrl_Text_Circle_Opt.SetImageDecimalPoint(text_circle.dot_image);
            uCtrl_Text_Circle_Opt.SetSeparator(text_circle.separator_image);

            uCtrl_Text_Circle_Opt.SetHorizontalAlignment(text_circle.horizontal_alignment);
            uCtrl_Text_Circle_Opt.SetVerticalAlignment(text_circle.vertical_alignment);

            uCtrl_Text_Circle_Opt.checkBox_addZero.Checked = text_circle.zero;
            uCtrl_Text_Circle_Opt.checkBox_reverse_direction.Checked = text_circle.reverse_direction;
            uCtrl_Text_Circle_Opt.checkBox_unit_in_alignment.Checked = text_circle.unit_in_alignment;


            PreviewView = true;
            uCtrl_Text_Circle_Opt_ValueChanged(sender, eventArgs);
        }

        private void uCtrl_Text_Rotate_Opt_WidgetProperty_Copy(object sender, EventArgs eventArgs)
        {
            if (WidgetProperty.ContainsKey("hmUI_widget_IMG_NUMBER")) WidgetProperty.Remove("hmUI_widget_IMG_NUMBER");
            hmUI_widget_IMG_NUMBER img_number = new hmUI_widget_IMG_NUMBER();

            img_number.align = uCtrl_Text_Rotate_Opt.GetAlignment();
            img_number.unit_in_alignment = uCtrl_Text_Rotate_Opt.checkBox_unit_in_alignment.Checked;
            img_number.img_First = uCtrl_Text_Rotate_Opt.GetImage();
            img_number.imageX = (int)uCtrl_Text_Rotate_Opt.numericUpDown_imageX.Value;
            img_number.imageY = (int)uCtrl_Text_Rotate_Opt.numericUpDown_imageY.Value;
            img_number.space = (int)uCtrl_Text_Rotate_Opt.numericUpDown_spacing.Value;
            img_number.angle = (int)uCtrl_Text_Rotate_Opt.numericUpDown_angle.Value;
            img_number.unit = uCtrl_Text_Rotate_Opt.GetUnit();
            img_number.imperial_unit = uCtrl_Text_Rotate_Opt.GetUnitMile();
            img_number.dot_image = uCtrl_Text_Rotate_Opt.GetImageDecimalPointOrMinus();
            img_number.invalid_image = uCtrl_Text_Rotate_Opt.GetImageError();
            img_number.separator_image = uCtrl_Text_Rotate_Opt.GetSeparator();
            img_number.zero = uCtrl_Text_Rotate_Opt.checkBox_addZero.Checked;

            WidgetProperty.Add("hmUI_widget_IMG_NUMBER", img_number);
            uCtrl_Text_Rotate_Opt.WidgetProperty = WidgetProperty;
        }

        private void uCtrl_Text_Rotate_Opt_WidgetProperty_Paste(object sender, EventArgs eventArgs)
        {
            if (!WidgetProperty.ContainsKey("hmUI_widget_IMG_NUMBER")) return;
            Object obj = null;
            WidgetProperty.TryGetValue("hmUI_widget_IMG_NUMBER", out obj);
            if (obj == null) return;
            hmUI_widget_IMG_NUMBER img_number = (hmUI_widget_IMG_NUMBER)obj;
            PreviewView = false;

            if (img_number == null)
            {
                PreviewView = true;
                return;
            }
            if (img_number.img_First != null)
                uCtrl_Text_Rotate_Opt.SetImage(img_number.img_First);
            uCtrl_Text_Rotate_Opt.numericUpDown_imageX.Value = img_number.imageX;
            uCtrl_Text_Rotate_Opt.numericUpDown_imageY.Value = img_number.imageY;

            uCtrl_Text_Rotate_Opt.SetUnit(img_number.unit);
            uCtrl_Text_Rotate_Opt.SetUnitMile(img_number.imperial_unit);
            uCtrl_Text_Rotate_Opt.SetImageError(img_number.invalid_image);
            uCtrl_Text_Rotate_Opt.SetImageDecimalPointOrMinus(img_number.dot_image);
            uCtrl_Text_Rotate_Opt.SetSeparator(img_number.separator_image);
            //uCtrl_Text_Rotate_Opt.SetImageDecimalPointOrMinus
            uCtrl_Text_Rotate_Opt.numericUpDown_spacing.Value = img_number.space;
            uCtrl_Text_Rotate_Opt.numericUpDown_angle.Value = img_number.angle;

            uCtrl_Text_Rotate_Opt.SetAlignment(img_number.align);

            uCtrl_Text_Rotate_Opt.checkBox_addZero.Checked = img_number.zero;
            uCtrl_Text_Rotate_Opt.checkBox_unit_in_alignment.Checked = img_number.unit_in_alignment;

            PreviewView = true;
            uCtrl_Text_Rotate_Opt_ValueChanged(sender, eventArgs);
        }

    }
}
