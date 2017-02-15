using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Date {

    int day;
    int month;
    int year;

    public Date(int day, int month, int year)
    {
        Day = day;
        Month = month;
        Year = year;
    }

    public Date()
    {
        Day = 1;
        Month = 1;
        Year = 0;
    }

    public string incrementDay()
    {
        Day++;
        if (Day > 30)
        {
            Day = 1;
            Month++;
            if (Month > 12)
            {
                Month = 1;
                Year++;
            }
        }

        return toString();
    }

    public string toString()
    {
        return "Day: " + Day + " Month: " + Month + "Year: " + Year;
    }

    public int Day
    {
        get
        {
            return day;
        }

        set
        {
            day = value;
        }
    }

    public int Month
    {
        get
        {
            return month;
        }

        set
        {
            month = value;
        }
    }

    public int Year
    {
        get
        {
            return year;
        }

        set
        {
            year = value;
        }
    }
}
