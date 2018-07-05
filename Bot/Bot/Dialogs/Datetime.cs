using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bot.Dialogs
{
    [Serializable]
    public class Datatime
    {
        public int Year;
        public int Month;
        public int Day;
        public int Hour;
        public int Minute;
        public int Second;
        public Datatime()
        {
            DateTimeOffset thistime = DateTimeOffset.Now;
            Year = thistime.Year;
            Month = thistime.Month;
            Day = thistime.Day;
            Hour = thistime.Hour;
            Minute = thistime.Minute;
            Second = thistime.Second;
        }
        public Datatime(string time)
        {
            DateTimeOffset thistime = DateTimeOffset.Now;
            string hours = "";
            string minutes = "";
            bool good = true;
            for (int i = 0; i < time.Length; i++)
            {
                if (time[i] == ':') good = false;
                if (good) hours += time[i];
                else if (time[i] != ':')
                {
                    minutes += time[i];
                }
            }
            int hour = 0;
            int minute = 0;
            int.TryParse(hours, out Hour);
            int.TryParse(minutes, out Minute);
            Year = thistime.Year;
            Month = thistime.Month;
            Day = thistime.Day;
            Second = 0;
        }
    }
}