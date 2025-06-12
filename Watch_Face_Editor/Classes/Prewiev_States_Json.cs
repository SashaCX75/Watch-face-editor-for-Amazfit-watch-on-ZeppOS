using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Watch_Face_Editor
{
    class Prewiev_States_Json
    {
        public TimePreview Time { get; set; }
        public int Steps { get; set; } = 5678;
        public int Goal { get; set; } = 8000;
        public int Pulse { get; set; } = 87;
        public int BatteryLevel { get; set; } = 97;
        public int Distance { get; set; } = 4567;
        public int Calories { get; set; } = 150;

        public int PAI { get; set; } = 70;
        public int Stand { get; set; } = 5;
        public int Stress = 7;
        //public int ActivityGoal = 7;
        public int FatBurning = 7;

        public int CurrentWeather { get; set; }
        public int CurrentTemperature { get; set; }
        public int TemperatureMax = 5;
        public int TemperatureMin = -15;
        public int UVindex { get; set; } = 3;
        public int AirQuality { get; set; } = 73;
        public int Humidity { get; set; } = 55;
        public int WindForce = 7;
        public int WindDirection = 3;
        public int CompassDirection { get; set; } = 70;
        public int Altitude = 130;
        public int AirPressure = 1000;

        public bool Bluetooth { get; set; }
        public bool Unlocked { get; set; }
        public bool Alarm { get; set; }
        public bool DoNotDisturb { get; set; }
        public bool ShowTemperature = true;

        public List<ForecastData> forecastData { get; set; }

        public int SpO2 = 97;
        public int TrainingLoad = 280;
        public int TrainingLoadGoal = 560;
        public int VO2Max = 47;
        public int BodyTemp = 320;
        public int Floor = 7;
        public int Readiness = 87;

    }

    public class TimePreview
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
        public int Hour { get; set; }
        public int Minute { get; set; }
        public int Second { get; set; }
    }
}
