using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ControlLibrary
{
    public partial class UCtrl_Set: UserControl
    {
        private int setNumber;
        private bool setValue;

        // Скрытые значения, не меняются через интерфейс
        Dictionary<string, List<int>> ForecastData = new Dictionary<string, List<int>>();
        private int SpO2 = 97;
        private int TrainingLoad = 280;
        private int TrainingLoadGoal = 560;
        private int VO2Max = 47;
        private int Floor = 7;
        private int Readiness = 87;
        private int BodyTemp = 327;
        private int HRV = 37;

        public UCtrl_Set()
        {
            InitializeComponent();
            comboBox_WeatherSet_Icon.SelectedIndex = 0;
            comboBox_WindDirection.SelectedIndex = 0;
            setValue = false;
        }

        // меняем цвет текста и рамки для groupBox
        private void groupBox_Paint(object sender, PaintEventArgs e)
        {
            GroupBox box = sender as GroupBox;
            DrawGroupBox(box, e.Graphics, Color.Black, Color.DarkGray);
        }
        private void DrawGroupBox(GroupBox box, Graphics g, Color textColor, Color borderColor)
        {
            if (box != null)
            {
                Brush textBrush = new SolidBrush(textColor);
                Brush borderBrush = new SolidBrush(borderColor);
                Pen borderPen = new Pen(borderBrush);
                SizeF strSize = g.MeasureString(box.Text, box.Font);
                Rectangle rect = new Rectangle(box.ClientRectangle.X,
                                               box.ClientRectangle.Y + (int)(strSize.Height / 2),
                                               box.ClientRectangle.Width - 1,
                                               box.ClientRectangle.Height - (int)(strSize.Height / 2) - 5);

                // Clear text and border
                g.Clear(this.BackColor);

                // Draw text
                g.DrawString(box.Text, box.Font, textBrush, box.Padding.Left, 0);

                // Drawing Border
                //Left
                g.DrawLine(borderPen, rect.Location, new Point(rect.X, rect.Y + rect.Height));
                //Right
                g.DrawLine(borderPen, new Point(rect.X + rect.Width, rect.Y), new Point(rect.X + rect.Width, rect.Y + rect.Height));
                //Bottom
                g.DrawLine(borderPen, new Point(rect.X, rect.Y + rect.Height), new Point(rect.X + rect.Width, rect.Y + rect.Height));
                //Top1
                g.DrawLine(borderPen, new Point(rect.X, rect.Y), new Point(rect.X + box.Padding.Left, rect.Y));
                //Top2
                g.DrawLine(borderPen, new Point(rect.X + box.Padding.Left + (int)(strSize.Width), rect.Y), new Point(rect.X + rect.Width, rect.Y));
            }
        }


        [Browsable(true)]
        public event CollapseHandler Collapse;
        public delegate void CollapseHandler(object sender, EventArgs eventArgs, int setNumber);

        [Browsable(true)]
        public event ValueChangedHandler ValueChanged;
        public delegate void ValueChangedHandler(object sender, EventArgs eventArgs, int setNumber);

        private void button_Set_Click(object sender, EventArgs e)
        {
            bool v = groupBox_Activity.Visible;
            groupBox_Air.Visible = !v;
            groupBox_Activity.Visible = !v;

            if (Collapse != null)
            {
                EventArgs eventArgs = new EventArgs();
                Collapse(this, eventArgs, setNumber);
            }

        }

        /// <summary>Возвращает true если панель свернута</summary>
        public bool Collapsed
        {
            get
            {
                return !groupBox_Activity.Visible;
            }
            set
            {
                groupBox_Air.Visible = !value;
                groupBox_Activity.Visible = !value;
            }
        }

        /// <summary>Устанавливает номер панели</summary>
        public int SetNumber
        {
            get
            {
                return setNumber;
            }
            set
            {
                setNumber = value;
            }
        }

        /// <summary>Устанавливает надпись на кнопке</summary>
        [Localizable(true)]
        public string ButtonText
        {
            get
            {
                return button_Set.Text;
            }
            set
            {
                button_Set.Text = value;
            }
        }

        private void numericUpDown_Set_ValueChanged(object sender, EventArgs e)
        {
            if (ValueChanged != null && !setValue)
            {
                EventArgs eventArgs = new EventArgs();
                ValueChanged(this, eventArgs, setNumber);
            }
        }

        private void dateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            if (ValueChanged != null && !setValue)
            {
                EventArgs eventArgs = new EventArgs();
                ValueChanged(this, eventArgs, setNumber);
            }
        }

        private void comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ValueChanged != null && !setValue)
            {
                EventArgs eventArgs = new EventArgs();
                ValueChanged(this, eventArgs, setNumber);
            }
        }

        private void checkBox_Click(object sender, EventArgs e)
        {
            if (ValueChanged != null && !setValue)
            {
                EventArgs eventArgs = new EventArgs();
                ValueChanged(this, eventArgs, setNumber);
            }
        }

        /// <summary>Возвращает данные для предпросмотра из выбранного набора параметров</summary>
        /// <param name="DateTime">Дата и время, время различных событий</param>
        /// <param name="Activity">Данные активностей (калории, ЧСС,  путь, шаги, цель шагов, PAI, StandUp, стресс, жиросжигание)</param>
        /// <param name="Air">Атмосферные данные (иконка погоды, текущая температура, максимальная температура, 
        /// минимальная температура, УФ индекс, качество воздуха, влажность, сила ветра, направление ветра, высота, давление)</param>
        /// <param name="checkValue">Значение переключателей (Bluetooth, будильник, блокировка, DND, показ температуры)</param>
        /// <param name="System">Системные показатели (Заряд, температураы)</param>
        public void GetValue(out Dictionary<string, int> DateTime, out Dictionary<string, int> Activity, 
            out Dictionary<string, int> Air, out Dictionary<string, List<int>> ForecastData,
            out Dictionary<string, bool> checkValue, out Dictionary<string, int> System)
        {
            DateTime = new Dictionary<string, int>();
            Activity = new Dictionary<string, int>();
            Air = new Dictionary<string, int>();
            ForecastData = this.ForecastData;
            checkValue = new Dictionary<string, bool>();
            System = new Dictionary<string, int>();

            DateTime.Add("Year", dateTimePicker_Date_Set.Value.Year);
            DateTime.Add("Month", dateTimePicker_Date_Set.Value.Month);
            DateTime.Add("Day", dateTimePicker_Date_Set.Value.Day);
            int WeekDay = (int)dateTimePicker_Date_Set.Value.DayOfWeek;
            if (WeekDay == 0) WeekDay = 7;
            DateTime.Add("WeekDay", WeekDay);

            DateTime.Add("Hour", dateTimePicker_Time_Set.Value.Hour);
            DateTime.Add("Minute", dateTimePicker_Time_Set.Value.Minute);
            DateTime.Add("Second", dateTimePicker_Time_Set.Value.Second);

            //DateTime.Add("Hour_Sunrise", dateTimePicker_Time_Set.Value.Hour);
            //DateTime.Add("Minute_Sunrise", dateTimePicker_Time_Set.Value.Minute);
            DateTime.Add("Hour_Sunrise", 3);
            DateTime.Add("Minute_Sunrise", 30);
            //DateTime.Add("Hour_Sunset", dateTimePicker_Time_Set.Value.Hour);
            //DateTime.Add("Minute_Sunset", dateTimePicker_Time_Set.Value.Minute);
            DateTime.Add("Hour_Sunset", 20);
            DateTime.Add("Minute_Sunset", 30);

            //DateTime.Add("Hour_Moonrise", dateTimePicker_Time_Set.Value.Hour);
            //DateTime.Add("Minute_Moonrise", dateTimePicker_Time_Set.Value.Minute);
            DateTime.Add("Hour_Moonrise", 18);
            DateTime.Add("Minute_Moonrise", 30);
            //DateTime.Add("Hour_Moonset", dateTimePicker_Time_Set.Value.Hour);
            //DateTime.Add("Minute_Moonset", dateTimePicker_Time_Set.Value.Minute);
            DateTime.Add("Hour_Moonset", 5);
            DateTime.Add("Minute_Moonset", 30);

            DateTime.Add("Hour_AlarmClock", 9);
            DateTime.Add("Minute_AlarmClock", 43);

            //DateTime.Add("Hour_SleepStart", dateTimePicker_Time_Set.Value.Hour);
            //DateTime.Add("Minute_SleepStart", dateTimePicker_Time_Set.Value.Minute);
            DateTime.Add("Hour_SleepStart", 21);
            DateTime.Add("Minute_SleepStart", 7);
            //DateTime.Add("Hour_SleepEnd", dateTimePicker_Time_Set.Value.Hour);
            //DateTime.Add("Minute_SleepEnd", dateTimePicker_Time_Set.Value.Minute);
            DateTime.Add("Hour_SleepEnd", 7);
            DateTime.Add("Minute_SleepEnd", 3);

            Activity.Add("Calories", (int)numericUpDown_Calories_Set.Value);
            Activity.Add("HeartRate", (int)numericUpDown_HeartRate_Set.Value);
            Activity.Add("Distance", (int)numericUpDown_Distance_Set.Value);
            Activity.Add("Steps", (int)numericUpDown_Steps_Set.Value);
            Activity.Add("StepsGoal", (int)numericUpDown_Goal_Set.Value);

            Activity.Add("PAI", (int)numericUpDown_PAI_Set.Value);
            Activity.Add("StandUp", (int)numericUpDown_StandUp_Set.Value);
            Activity.Add("Stress", (int)numericUpDown_Stress_Set.Value);
            Activity.Add("FatBurning", (int)numericUpDown_FatBurning_Set.Value);

            Activity.Add("SpO2", SpO2);
            Activity.Add("TrainingLoad", TrainingLoad);
            Activity.Add("TrainingLoadGoal", TrainingLoadGoal);
            Activity.Add("VO2Max", VO2Max);
            Activity.Add("Floor", Floor);
            Activity.Add("Readiness", Readiness);


            Air.Add("Weather_Icon", comboBox_WeatherSet_Icon.SelectedIndex);
            Air.Add("Temperature", (int)numericUpDown_WeatherSet_Temp.Value);
            Air.Add("TemperatureMax", (int)numericUpDown_WeatherSet_MaxTemp.Value);
            Air.Add("TemperatureMin", (int)numericUpDown_WeatherSet_MinTemp.Value);

            Air.Add("UVindex", (int)numericUp_UVindex_Set.Value);
            Air.Add("AirQuality", (int)numericUpDown_AirQuality_Set.Value);
            Air.Add("Humidity", (int)numericUpDown_Humidity_Set.Value);
            Air.Add("WindForce", (int)numericUpDown_WindForce.Value);
            Air.Add("WindDirection", comboBox_WindDirection.SelectedIndex);
            Air.Add("CompassDirection", (int)numericUpDown_Compass_Set.Value);
            Air.Add("Altitude", (int)numericUpDown_Altitude_Set.Value);
            Air.Add("AirPressure", (int)numericUpDown_AirPressure_Set.Value);


            checkValue.Add("Bluetooth", checkBox_Bluetooth_Set.Checked);
            checkValue.Add("Alarm", checkBox_Alarm_Set.Checked);
            checkValue.Add("Lock", checkBox_Lock_Set.Checked);
            checkValue.Add("DND", checkBox_DND_Set.Checked);

            checkValue.Add("ShowTemperature", checkBox_WeatherSet_Temp.Checked);

            System.Add("Battery", (int)numericUpDown_Battery_Set.Value);
            System.Add("BodyTemp", BodyTemp);

        }

        public string GetValue()
        {
            int WeekDay = (int)dateTimePicker_Date_Set.Value.DayOfWeek;
            if (WeekDay == 0) WeekDay = 7;

            JObject forecastDataObject = new JObject();
            foreach (var kvp in ForecastData)
            {
                forecastDataObject[kvp.Key] = new JArray(kvp.Value);
            }

            JObject DateTime = new JObject
            {
                ["Date"] = new JObject
                {
                    ["Year"] = dateTimePicker_Date_Set.Value.Year,
                    ["Month"] = dateTimePicker_Date_Set.Value.Month,
                    ["Day"] = dateTimePicker_Date_Set.Value.Day,
                    ["WeekDay"] = WeekDay
                },
                ["Time"] = new JObject
                {
                    ["Hour"] = dateTimePicker_Time_Set.Value.Hour,
                    ["Minute"] = dateTimePicker_Time_Set.Value.Minute,
                    ["Second"] = dateTimePicker_Time_Set.Value.Second
                },

                ["Sunrise"] = new JObject
                {
                    ["Hour"] = 3,
                    ["Minute"] = 30
                    //["Hour"] = dateTimePicker_Time_Set.Value.Hour,
                    //["Minute"] = dateTimePicker_Time_Set.Value.Minute
                },
                ["Sunset"] = new JObject
                {
                    ["Hour"] = 20,
                    ["Minute"] = 30
                    //["Hour"] = dateTimePicker_Time_Set.Value.Hour,
                    //["Minute"] = dateTimePicker_Time_Set.Value.Minute
                },

                ["Moonrise"] = new JObject
                {
                    ["Hour"] = 18,
                    ["Minute"] = 30
                    //["Hour"] = dateTimePicker_Time_Set.Value.Hour,
                    //["Minute"] = dateTimePicker_Time_Set.Value.Minute
                },
                ["Moonset"] = new JObject
                {
                    ["Hour"] = 5,
                    ["Minute"] = 30
                    //["Hour"] = dateTimePicker_Time_Set.Value.Hour,
                    //["Minute"] = dateTimePicker_Time_Set.Value.Minute
                },

                ["SleepStart"] = new JObject
                {
                    ["Hour"] = 21,
                    ["Minute"] = 7
                    //["Hour"] = dateTimePicker_Time_Set.Value.Hour,
                    //["Minute"] = dateTimePicker_Time_Set.Value.Minute
                },
                ["SleepEnd"] = new JObject
                {
                    ["Hour"] = 7,
                    ["Minute"] = 3
                    //["Hour"] = dateTimePicker_Time_Set.Value.Hour,
                    //["Minute"] = dateTimePicker_Time_Set.Value.Minute
                },

                ["AlarmClock"] = new JObject
                {
                    ["Hour"] = 9,
                    ["Minute"] = 43
                    //["Hour"] = dateTimePicker_Time_Set.Value.Hour,
                    //["Minute"] = dateTimePicker_Time_Set.Value.Minute
                },

            };

            JObject Activity = new JObject
            {
                ["Steps"] = (int)numericUpDown_Steps_Set.Value,
                ["StepsGoal"] = (int)numericUpDown_Goal_Set.Value,
                ["Calories"] = (int)numericUpDown_Calories_Set.Value,
                ["HeartRate"] = (int)numericUpDown_HeartRate_Set.Value,
                ["PAI"] = (int)numericUpDown_PAI_Set.Value,
                ["Distance"] = (int)numericUpDown_Distance_Set.Value,
                ["StandUp"] = (int)numericUpDown_StandUp_Set.Value,
                ["Stress"] = (int)numericUpDown_Stress_Set.Value,
                ["FatBurning"] = (int)numericUpDown_FatBurning_Set.Value,

                ["SpO2"] = SpO2,
                ["TrainingLoad"] = TrainingLoad,
                ["TrainingLoadGoal"] = TrainingLoadGoal,
                ["VO2Max"] = VO2Max,
                ["Floor"] = Floor,
                ["Readiness"] = Readiness,
                ["HRV"] = HRV,

            };

            JObject Air = new JObject
            {
                ["Temperature"] = (int)numericUpDown_WeatherSet_Temp.Value,
                ["TemperatureMin"] = (int)numericUpDown_WeatherSet_MinTemp.Value,
                ["TemperatureMax"] = (int)numericUpDown_WeatherSet_MaxTemp.Value,
                ["WeatherIcon"] = comboBox_WeatherSet_Icon.SelectedIndex,

                ["UVindex"] = comboBox_WeatherSet_Icon.SelectedIndex,
                ["UVindex"] = (int)numericUp_UVindex_Set.Value,
                ["AirQuality"] = (int)numericUpDown_AirQuality_Set.Value,
                ["Humidity"] = (int)numericUpDown_Humidity_Set.Value,
                ["WindForce"] = (int)numericUpDown_WindForce.Value,
                ["WindDirection"] = comboBox_WindDirection.SelectedIndex,
                ["CompassDirection"] = (int)numericUpDown_Compass_Set.Value,
                ["Altitude"] = (int)numericUpDown_Altitude_Set.Value,
                ["AirPressure"] = (int)numericUpDown_AirPressure_Set.Value,
                ["showTemperature"] = checkBox_WeatherSet_Temp.Checked,

                ["forecastData"] = forecastDataObject,

            };

            JObject System = new JObject
            {
                ["Status"] = new JObject
                {
                    ["Bluetooth"] = checkBox_Bluetooth_Set.Checked,
                    ["Alarm"] = checkBox_Alarm_Set.Checked,
                    ["Lock"] = checkBox_Lock_Set.Checked,
                    ["DoNotDisturb"] = checkBox_DND_Set.Checked
                },
                ["Battery"] = (int)numericUpDown_Battery_Set.Value,
                ["BodyTemp"] = BodyTemp,
            };

            JObject json = new JObject
            {
                ["DateTime"] = DateTime,
                ["Activity"] = Activity,
                ["Air"] = Air,
                ["System"] = System,
            };

            string jsonStr = json.ToString();
            return jsonStr;

        }

        /// <summary>Устанавливает данные для предпросмотра из выбранного набора параметров</summary>
        /// <param name="Activity">Данные активностей (год, число, день, часы, минуты, секунды,
        /// заряд, калории, ЧСС,  путь, шаги, цель шагов, PAI, StandUp, стресс, жиросжигание)</param>
        /// <param name="Air">Атмосферные данные (иконка погоды, текущая температура, максимальная температура, 
        /// минимальная температура, УФ индекс, качество воздуха, влажность, сила ветра,
        /// высота, давление)</param>
        /// <param name="checkValue">Дначение переключателей (Bluetooth, будильник, блокировка, DND, показ температуры)</param>
        public void SetValue(Dictionary<string, int> Activity, Dictionary<string, int> Air, Dictionary<string, List<int>> ForecastData,
            Dictionary<string, bool> checkValue)
        {
            int year;
            Activity.TryGetValue("Year", out year);
            int month;
            Activity.TryGetValue("Month", out month);
            int day;
            Activity.TryGetValue("Day", out day);
            int hour;
            Activity.TryGetValue("Hour", out hour);
            int min;
            Activity.TryGetValue("Minute", out min);
            int sec;
            Activity.TryGetValue("Second", out sec);

            int battery;
            Activity.TryGetValue("Battery", out battery);
            int calories;
            Activity.TryGetValue("Calories", out calories);
            int heartRate;
            Activity.TryGetValue("HeartRate", out heartRate);
            int distance;
            Activity.TryGetValue("Distance", out distance);
            int steps;
            Activity.TryGetValue("Steps", out steps);
            int stepsGoal;
            Activity.TryGetValue("StepsGoal", out stepsGoal);

            int PAI;
            Activity.TryGetValue("PAI", out PAI);
            int standUp;
            Activity.TryGetValue("StandUp", out standUp);
            int stress;
            Activity.TryGetValue("Stress", out stress);
            //int activityGoal;
            //Activity.TryGetValue("ActivityGoal", out activityGoal);
            int fatBurning;
            Activity.TryGetValue("FatBurning", out fatBurning);

            int weather_Icon;
            Air.TryGetValue("Weather_Icon", out weather_Icon);
            int temperature;
            Air.TryGetValue("Temperature", out temperature);
            int temperatureMax;
            Air.TryGetValue("TemperatureMax", out temperatureMax);
            int temperatureMin;
            Air.TryGetValue("TemperatureMin", out temperatureMin);

            int UVindex;
            Air.TryGetValue("UVindex", out UVindex);
            int airQuality;
            Air.TryGetValue("AirQuality", out airQuality);
            int humidity;
            Air.TryGetValue("Humidity", out humidity);
            int windForce;
            Air.TryGetValue("WindForce", out windForce);
            int windDirection;
            Air.TryGetValue("WindDirection", out windDirection);
            int compassDirection;
            Air.TryGetValue("CompassDirection", out compassDirection);
            int altitude;
            Air.TryGetValue("Altitude", out altitude);
            int airPressure;
            Air.TryGetValue("AirPressure", out airPressure);

            bool bluetooth;
            checkValue.TryGetValue("Bluetooth", out bluetooth);
            bool alarm;
            checkValue.TryGetValue("Alarm", out alarm);
            bool Lock;
            checkValue.TryGetValue("Lock", out Lock);
            bool DND;
            checkValue.TryGetValue("DND", out DND);

            bool showTemperature;
            checkValue.TryGetValue("ShowTemperature", out showTemperature);

            Activity.TryGetValue("SpO2", out SpO2);
            Activity.TryGetValue("TrainingLoad", out TrainingLoad);
            Activity.TryGetValue("TrainingLoadGoal", out TrainingLoadGoal);
            Activity.TryGetValue("VO2Max", out VO2Max);
            Activity.TryGetValue("BodyTemp", out BodyTemp);
            Activity.TryGetValue("Floor", out Floor);
            Activity.TryGetValue("Readiness", out Readiness);

            try
            {
                setValue = true;

                dateTimePicker_Date_Set.Value = new DateTime(year, month, day, hour, min, sec);
                dateTimePicker_Time_Set.Value = new DateTime(year, month, day, hour, min, sec);

                numericUpDown_Battery_Set.Value = battery;
                numericUpDown_Calories_Set.Value = calories;
                numericUpDown_HeartRate_Set.Value = heartRate;
                numericUpDown_Distance_Set.Value = distance;
                numericUpDown_Steps_Set.Value = steps;
                numericUpDown_Goal_Set.Value = stepsGoal;

                numericUpDown_PAI_Set.Value = PAI;
                numericUpDown_StandUp_Set.Value = standUp;
                numericUpDown_Stress_Set.Value = stress;
                //numericUpDown_ActivityGoal_Set.Value = activityGoal;
                numericUpDown_FatBurning_Set.Value = fatBurning;


                comboBox_WeatherSet_Icon.SelectedIndex = weather_Icon;
                numericUpDown_WeatherSet_Temp.Value = temperature;
                numericUpDown_WeatherSet_MaxTemp.Value = temperatureMax;
                numericUpDown_WeatherSet_MinTemp.Value = temperatureMin;


                numericUp_UVindex_Set.Value = UVindex;
                numericUpDown_AirQuality_Set.Value = airQuality;
                numericUpDown_Humidity_Set.Value = humidity;
                numericUpDown_WindForce.Value = windForce;
                comboBox_WindDirection.SelectedIndex = windDirection;
                numericUpDown_Compass_Set.Value = compassDirection;
                numericUpDown_Altitude_Set.Value = altitude;
                numericUpDown_AirPressure_Set.Value = airPressure;

                checkBox_Bluetooth_Set.Checked = bluetooth;
                checkBox_Alarm_Set.Checked = alarm;
                checkBox_Lock_Set.Checked = Lock;
                checkBox_DND_Set.Checked = DND;

                checkBox_WeatherSet_Temp.Checked = showTemperature;

                if (ForecastData != null && ForecastData.Count > 0) this.ForecastData = ForecastData;
                else
                {
                    Dictionary<string, List<int>> ForecastDataTemp = new Dictionary<string, List<int>>();
                    List<int> high = new List<int>();
                    List<int> low = new List<int>();
                    List<int> index = new List<int>();
                    Random rnd = new Random();
                    int tempOffset = rnd.Next(-25, 25);
                    for (int i = 0; i < 9; i++)
                    {
                        int maxTemp = rnd.Next(-5, 5) + tempOffset;
                        int minTemp = maxTemp - rnd.Next(3, 7);
                        int iconIndex = rnd.Next(0, 25);

                        high.Add(maxTemp);
                        low.Add(minTemp);
                        index.Add(iconIndex);
                    }
                    ForecastDataTemp.Add("high", high);
                    ForecastDataTemp.Add("low", low);
                    ForecastDataTemp.Add("index", index);
                    this.ForecastData = ForecastDataTemp;
                }
            }
            finally
            {
                setValue = false;
            }

        }

        public void SetValue(string jsonStr)
        {
            JObject json = null;
            setValue = true;
            try
            {
                json = JObject.Parse(jsonStr);
            }
            catch { }

            if (json != null)
            {
                try
                {
                    setValue = true;
                    if (json["DateTime"] != null)
                    {
                        int year = 2025;
                        int month = 12;
                        int day = 25;
                        int hour = 10;
                        int min = 50;
                        int sec = 30;

                        if(json["DateTime"]["Date"] != null)
                        {
                            if (json["DateTime"]["Date"]["Year"] != null) year = json["DateTime"]["Date"]["Year"].Value<int>();
                            if (json["DateTime"]["Date"]["Month"] != null) month = json["DateTime"]["Date"]["Month"].Value<int>();
                            if (json["DateTime"]["Date"]["Day"] != null) day = json["DateTime"]["Date"]["Day"].Value<int>();
                        }

                        if (json["DateTime"]["Time"] != null)
                        {
                            if (json["DateTime"]["Time"]["Hour"] != null) hour = json["DateTime"]["Time"]["Hour"].Value<int>();
                            if (json["DateTime"]["Time"]["Minute"] != null) min = json["DateTime"]["Time"]["Minute"].Value<int>();
                            if (json["DateTime"]["Time"]["Second"] != null) sec = json["DateTime"]["Time"]["Second"].Value<int>();
                        }

                        dateTimePicker_Date_Set.Value = new DateTime(year, month, day, hour, min, sec);
                        dateTimePicker_Time_Set.Value = new DateTime(year, month, day, hour, min, sec);
                    }

                    if (json["Activity"] != null)
                    {
                        if (json["Activity"]["Steps"] != null) numericUpDown_Steps_Set.Value = json["Activity"]["Steps"].Value<int>();
                        if (json["Activity"]["StepsGoal"] != null) numericUpDown_Goal_Set.Value = json["Activity"]["StepsGoal"].Value<int>();
                        if (json["Activity"]["Calories"] != null) numericUpDown_Calories_Set.Value = json["Activity"]["Calories"].Value<int>();
                        if (json["Activity"]["HeartRate"] != null) numericUpDown_HeartRate_Set.Value = json["Activity"]["HeartRate"].Value<int>();
                        if (json["Activity"]["Distance"] != null) numericUpDown_Distance_Set.Value = json["Activity"]["Distance"].Value<int>();
                        if (json["Activity"]["PAI"] != null) numericUpDown_PAI_Set.Value = json["Activity"]["PAI"].Value<int>();
                        if (json["Activity"]["StandUp"] != null) numericUpDown_StandUp_Set.Value = json["Activity"]["StandUp"].Value<int>();
                        if (json["Activity"]["Stress"] != null) numericUpDown_Stress_Set.Value = json["Activity"]["Stress"].Value<int>();
                        if (json["Activity"]["FatBurning"] != null) numericUpDown_FatBurning_Set.Value = json["Activity"]["FatBurning"].Value<int>();

                        if (json["Activity"]["SpO2"] != null) SpO2 = json["Activity"]["SpO2"].Value<int>();
                        if (json["Activity"]["TrainingLoad"] != null) TrainingLoad = json["Activity"]["TrainingLoad"].Value<int>();
                        if (json["Activity"]["TrainingLoadGoal"] != null) TrainingLoadGoal = json["Activity"]["TrainingLoadGoal"].Value<int>();
                        if (json["Activity"]["VO2Max"] != null) VO2Max = json["Activity"]["VO2Max"].Value<int>();
                        if (json["Activity"]["Floor"] != null) Floor = json["Activity"]["Floor"].Value<int>();
                        if (json["Activity"]["Readiness"] != null) Readiness = json["Activity"]["Readiness"].Value<int>();
                        if (json["Activity"]["HRV"] != null) HRV = json["Activity"]["HRV"].Value<int>();
                    }

                    if (json["Air"] != null)
                    {
                        if (json["Air"]["Temperature"] != null) numericUpDown_WeatherSet_Temp.Value = json["Air"]["Temperature"].Value<int>();
                        if (json["Air"]["TemperatureMax"] != null) numericUpDown_WeatherSet_MaxTemp.Value = json["Air"]["TemperatureMax"].Value<int>();
                        if (json["Air"]["TemperatureMin"] != null) numericUpDown_WeatherSet_MinTemp.Value = json["Air"]["TemperatureMin"].Value<int>();
                        if (json["Air"]["WeatherIcon"] != null) comboBox_WeatherSet_Icon.SelectedIndex = json["Air"]["WeatherIcon"].Value<int>();
                        if (json["Air"]["showTemperature"] != null) checkBox_WeatherSet_Temp.Checked = json["Air"]["showTemperature"].Value<bool>();

                        if (json["Air"]["UVindex"] != null) numericUp_UVindex_Set.Value = json["Air"]["UVindex"].Value<int>();
                        if (json["Air"]["AirQuality"] != null) numericUpDown_AirQuality_Set.Value = json["Air"]["AirQuality"].Value<int>();
                        if (json["Air"]["Humidity"] != null) numericUpDown_Humidity_Set.Value = json["Air"]["Humidity"].Value<int>();
                        if (json["Air"]["WindForce"] != null) numericUpDown_WindForce.Value = json["Air"]["WindForce"].Value<int>();
                        if (json["Air"]["WindDirection"] != null) comboBox_WindDirection.SelectedIndex = json["Air"]["WindDirection"].Value<int>();
                        if (json["Air"]["CompassDirection"] != null) numericUpDown_Compass_Set.Value = json["Air"]["CompassDirection"].Value<int>();
                        if (json["Air"]["Altitude"] != null) numericUpDown_Altitude_Set.Value = json["Air"]["Altitude"].Value<int>();
                        if (json["Air"]["AirPressure"] != null) numericUpDown_AirPressure_Set.Value = json["Air"]["AirPressure"].Value<int>();
                        
                        Dictionary<string, List<int>> forecastData = new Dictionary<string, List<int>>();
                        if (json["Air"]["forecastData"] != null) forecastData = json["Air"]["forecastData"].ToObject<Dictionary<string, List<int>>>();
                        if (forecastData != null && forecastData.Count > 0) ForecastData = forecastData;
                        else
                        {
                            Dictionary<string, List<int>> ForecastDataTemp = new Dictionary<string, List<int>>();
                            List<int> high = new List<int>();
                            List<int> low = new List<int>();
                            List<int> index = new List<int>();
                            Random rnd = new Random();
                            int tempOffset = rnd.Next(-25, 25);
                            for (int i = 0; i < 9; i++)
                            {
                                int maxTemp = rnd.Next(-5, 5) + tempOffset;
                                int minTemp = maxTemp - rnd.Next(3, 7);
                                int iconIndex = rnd.Next(0, 25);

                                high.Add(maxTemp);
                                low.Add(minTemp);
                                index.Add(iconIndex);
                            }
                            ForecastDataTemp.Add("high", high);
                            ForecastDataTemp.Add("low", low);
                            ForecastDataTemp.Add("index", index);
                            ForecastData = ForecastDataTemp;
                        }
                    }

                    if (json["System"] != null)
                    {
                        if (json["System"]["Battery"] != null) numericUpDown_Battery_Set.Value = json["System"]["Battery"].Value<int>();
                        if (json["System"]["BodyTemp"] != null) BodyTemp = json["System"]["BodyTemp"].Value<int>();
                        if (json["System"]["Status"] != null)
                        {
                            if (json["System"]["Status"]["Bluetooth"] != null) checkBox_Bluetooth_Set.Checked = json["System"]["Status"]["Bluetooth"].Value<bool>();
                            if (json["System"]["Status"]["Alarm"] != null) checkBox_Alarm_Set.Checked = json["System"]["Status"]["Alarm"].Value<bool>();
                            if (json["System"]["Status"]["Lock"] != null) checkBox_Lock_Set.Checked = json["System"]["Status"]["Lock"].Value<bool>();
                            if (json["System"]["Status"]["DoNotDisturb"] != null) checkBox_DND_Set.Checked = json["System"]["Status"]["DoNotDisturb"].Value<bool>();
                        }
                    }

                }
                finally
                {
                    setValue = false;
                }
            }
        }

        /// <summary>Устанавливает случайные данные для значений</summary>
        public void RandomValue(Random rnd)
        {
            setValue = true;

            DateTime now = DateTime.Now;
            //Random rnd = new Random();

            int year = now.Year;
            int month = rnd.Next(0, 12) + 1;
            int day = rnd.Next(0, 28) + 1;
            int hour = rnd.Next(0, 24);
            int min = rnd.Next(0, 60);
            int sec = rnd.Next(0, 60);
            dateTimePicker_Date_Set.Value = new DateTime(year, month, day, hour, min, sec);
            dateTimePicker_Time_Set.Value = new DateTime(year, month, day, hour, min, sec);

            numericUpDown_Battery_Set.Value = rnd.Next(0, 101);
            numericUpDown_Calories_Set.Value = rnd.Next(0, 500);
            numericUpDown_HeartRate_Set.Value = rnd.Next(45, 180);
            numericUpDown_Distance_Set.Value = rnd.Next(0, 15000);
            numericUpDown_Steps_Set.Value = rnd.Next(0, 15000);
            numericUpDown_Goal_Set.Value = rnd.Next(0, 15000);

            numericUpDown_PAI_Set.Value = rnd.Next(0, 150);
            numericUpDown_StandUp_Set.Value = rnd.Next(0, 13);
            numericUpDown_Stress_Set.Value = rnd.Next(0, 101);
            //numericUpDown_ActivityGoal_Set.Value = rnd.Next(0, 13);
            numericUpDown_FatBurning_Set.Value = rnd.Next(0, 35);


            comboBox_WeatherSet_Icon.SelectedIndex = rnd.Next(0, 29);
            numericUpDown_WeatherSet_Temp.Value = rnd.Next(-25, 35) + 1;
            numericUpDown_WeatherSet_MaxTemp.Value = rnd.Next(-25, 35) + 1;
            numericUpDown_WeatherSet_MinTemp.Value = numericUpDown_WeatherSet_MaxTemp.Value - rnd.Next(3, 10);


            numericUp_UVindex_Set.Value = rnd.Next(0, 13);
            numericUpDown_AirQuality_Set.Value = rnd.Next(0, 350);
            numericUpDown_Humidity_Set.Value = rnd.Next(30, 100);
            numericUpDown_WindForce.Value = rnd.Next(0, 13);
            comboBox_WindDirection.SelectedIndex = rnd.Next(0, 8);
            numericUpDown_Compass_Set.Value = rnd.Next(0, 360);
            numericUpDown_Altitude_Set.Value = rnd.Next(-100, 300);
            numericUpDown_AirPressure_Set.Value = rnd.Next(800, 1200);

            checkBox_Bluetooth_Set.Checked = rnd.Next(2) == 0 ? false : true;
            checkBox_Alarm_Set.Checked = rnd.Next(2) == 0 ? false : true;
            checkBox_Lock_Set.Checked = rnd.Next(2) == 0 ? false : true;
            checkBox_DND_Set.Checked = rnd.Next(2) == 0 ? false : true;

            checkBox_WeatherSet_Temp.Checked = rnd.Next(7) == 0 ? false : true;

            Dictionary<string, List<int>> ForecastData = new Dictionary<string, List<int>>();
            List<int> high = new List<int>();
            List<int> low = new List<int>();
            List<int> index = new List<int>(); 
            int tempOffset = rnd.Next(-25, 25);
            for (int i = 0; i < 9; i++)
            {
                int maxTemp = rnd.Next(-5, 5) + tempOffset;
                int minTemp = maxTemp - rnd.Next(3, 7);
                int iconIndex = rnd.Next(0, 25);

                high.Add(maxTemp);
                low.Add(minTemp);
                index.Add(iconIndex);
            }
            ForecastData.Add("high", high);
            ForecastData.Add("low", low);
            ForecastData.Add("index", index);
            this.ForecastData = ForecastData;

            SpO2 = rnd.Next(80, 101);
            TrainingLoad = rnd.Next(280, 600);
            TrainingLoadGoal = rnd.Next(300, 600);
            VO2Max = rnd.Next(30, 70);
            BodyTemp = rnd.Next(300, 380);
            Floor = rnd.Next(0, 30);
            Readiness = rnd.Next(50, 101);
            HRV = rnd.Next(0, 80);

            setValue = false;
        }

        private void checkBox_WeatherSet_Temp_CheckedChanged(object sender, EventArgs e)
        {
            bool b = checkBox_WeatherSet_Temp.Checked;
            numericUpDown_WeatherSet_Temp.Enabled = b;
            numericUpDown_WeatherSet_MaxTemp.Enabled = b;
            numericUpDown_WeatherSet_MinTemp.Enabled = b;
        }
    }
}
