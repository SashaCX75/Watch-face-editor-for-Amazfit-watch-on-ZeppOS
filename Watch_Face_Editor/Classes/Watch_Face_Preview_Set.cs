using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Watch_Face_Editor
{
    public class Watch_Face_Preview_Set
    {
        public DateTimeSet DateTime { get; set; }
        public ActivitySet Activity { get; set; }
        public WeatherSet Air { get; set; }
        public SystemSet System { get; set; }
        //public StatusSet Status { get; set; }
        //public int Battery { get; set; }
        public int SetNumber { get; set; }
    }

    public class DateTimeSet
    {
        public DateSet Date { get; set; }
        public TimeSet Time { get; set; }
        public TimeSet Sunrise { get; set; }
        public TimeSet Sunset { get; set; }
        public TimeSet Moonrise { get; set; }
        public TimeSet Moonset { get; set; }
        public TimeSet AlarmClock { get; set; }
        public TimeSet SleepStart { get; set; }
        public TimeSet SleepEnd { get; set; }
    }

    public class DateSet
    {
        public int Day { get; set; }
        public int Month { get; set; }
        public int WeekDay { get; set; }
        public int Year { get; set; }
    }

    public class TimeSet
    {
        public int Hour { get; set; }
        public int Minute { get; set; }
        public int Second { get; set; }
    }

    public class ActivitySet
    {
        public int Steps { get; set; } = 5678;
        public int StepsGoal { get; set; } = 8000;
        public int Calories { get; set; } = 150;
        public int HeartRate { get; set; } = 87;
        public int PAI { get; set; } = 70;
        public int Distance { get; set; } = 4567;
        public int StandUp { get; set; } = 5;
        public int Stress { get; set; } = 7;
        public int FatBurning { get; set; } = 7;
        public int SpO2 { get; set; } = 97;
        public int TrainingLoad { get; set; } = 280;
        public int TrainingLoadGoal { get; set; } = 560; // 300-600
        public int VO2Max { get; set; } = 47;
        public int Floor { get; set; } = 7;
        public int Readiness { get; set; } = 87;
        public int HRV { get; set; } = 37;
        public int BioCharge { get; set; } = 73;
    }

    public class WeatherSet
    {
        public int Temperature { get; set; } = 3;
        public int TemperatureMin { get; set; } = -15;
        public int TemperatureMax { get; set; } = 5;
        public int WeatherIcon { get; set; }
        //public bool TemperatureNoData { get; set; }
        //public bool TemperatureMinMaxNoData { get; set; }
        public int UVindex { get; set; } = 3;
        public int AirQuality { get; set; } = 73;
        public int Humidity { get; set; } = 55;
        public int WindForce { get; set; } = 5;
        public int WindDirection { get; set; } = 3;
        public int CompassDirection { get; set; } = 30;
        public int Altitude { get; set; } = 130;
        public int AirPressure { get; set; } = 1010;
        public bool showTemperature { get; set; }
        //public bool showTemperatureMaxMin { get; set; }
        public List<ForecastData> forecastData { get; set; }
    }

    public class ForecastData
    {
        public int high { get; set; }
        public int low { get; set; }
        public int index { get; set; }
    }

    public class SystemSet
    {
        public StatusSet Status { get; set; }
        public int Battery { get; set; } = 97;
        public int BodyTemp { get; set; } = 327;
    }

    public class StatusSet
    {
        public bool Bluetooth { get; set; }
        public bool Alarm { get; set; }
        public bool Lock { get; set; }
        public bool DoNotDisturb { get; set; }
    }
}
