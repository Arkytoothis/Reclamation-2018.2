using UnityEngine;
using System.Collections.Generic;

public class TimeData
{
    public int Year;
    public int Month;
    public int Week;
    public int Day;
    public int Hour;
    public int Minute;
    public int Second;

    public static bool operator ==(TimeData a, TimeData b) { return a.Equals(b); }
    public static bool operator !=(TimeData a, TimeData b) { return !a.Equals(b); }

    public static TimeData operator +(TimeData a, TimeData b) { return new TimeData(a.Year + b.Year, a.Month + b.Month, a.Week + b.Week, a.Day + b.Day, a.Hour + b.Hour, a.Minute + b.Minute, a.Second + b.Second); }
    public static TimeData operator -(TimeData a, TimeData b) { return new TimeData(a.Year - b.Year, a.Month - b.Month, a.Week - b.Week, a.Day - b.Day, a.Hour - b.Hour, a.Minute - b.Minute, a.Second + b.Second); }
    public static TimeData operator *(TimeData a, int b) { return new TimeData(a.Year *= b, a.Month *= b, a.Week *= b, a.Day *= b, a.Hour *= b, a.Minute *= b, a.Second *= b); }

    public static bool operator <=(TimeData a, TimeData b) { return a.Year <= b.Year && a.Month <= b.Month && a.Week <= b.Week && a.Day <= b.Day && a.Hour <= b.Hour && a.Minute <= b.Minute && a.Second <= b.Second; }
    public static bool operator >=(TimeData a, TimeData b) { return a.Year >= b.Year && a.Month >= b.Month && a.Week >= b.Week && a.Day >= b.Day && a.Hour >= b.Hour && a.Minute >= b.Minute && a.Second <= b.Second; }

    public static bool operator <(TimeData a, TimeData b) { return a.Year < b.Year && a.Month < b.Month && a.Week < b.Week && a.Day < b.Day && a.Hour < b.Hour && a.Minute < b.Minute && a.Second < b.Second; }
    public static bool operator >(TimeData a, TimeData b) { return a.Year > b.Year && a.Month > b.Month && a.Week > b.Week && a.Day > b.Day && a.Hour > b.Hour && a.Minute > b.Minute && a.Second < b.Second; }

    public static TimeData Zero = new TimeData(0, 0, 0, 0, 0, 0, 0);
    public static TimeData OneYear = new TimeData(1, 0, 0, 0, 0, 0, 0);
    public static TimeData OneMonth = new TimeData(0, 1, 0, 0, 0, 0, 0);
    public static TimeData OneDay = new TimeData(0, 0, 0, 1, 0, 0, 0);
    public static TimeData OneHour = new TimeData(0, 0, 0, 0, 1, 0, 0);
    public static TimeData OneMinute = new TimeData(0, 0, 0, 0, 0, 1, 0);
    public static TimeData OneSecond = new TimeData(0, 0, 0, 0, 0, 0, 1);

    public static TimeData Infinity = new TimeData(-1, -1, -1, -1, -1, -1, -1);

    public TimeData()
    {
        Year = 0;
        Month = 0;
        Week = 0;
        Day = 0;
        Hour = 0;
        Minute = 0;
        Second = 0;
    }

    public TimeData(int year, int month, int week, int day, int hour, int minute, int second)
    {
        Year = year;
        Month = month;
        Week = week;
        Day = day;
        Hour = hour;
        Minute = minute;
        Second = second;
    }

    public TimeData(TimeData data)
    {
        Year = data.Year;
        Month = data.Month;
        Week = data.Week;
        Day = data.Day;
        Hour = data.Hour;
        Minute = data.Minute;
        Second = data.Second;
    }

    public override bool Equals(System.Object obj)
    {
        if (obj == null || GetType() != obj.GetType())
            return false;

        TimeData td = obj as TimeData;
        if ((System.Object)td == null)
            return false;

        return Year == td.Year && Month == td.Month && Week == td.Week && Day == td.Day && Hour == td.Hour && Minute == td.Minute && Second == td.Second;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public string ToString(bool full)
    {
        if (full == false)
        {
            return Month + "/" + Day + "/" + Year + ", " + Hour + ":" + Minute + ":" + Second;
        }
        else
        {
            return "Year " + Year + ", Month " + Month + ", Week " + Week + ", Day " + Day + ", Hour " + Hour;
        }
    }
}