public class Date {

    int day;
    int week;
    int month;

    public Date(int day, int week, int month)
    {
        Day = day;
        Week = week;
        Month = month;
    }

    public Date()
    {
        Day = 1;
        Week = 1;
        Month = 1;
    }

    public string incrementDay()
    {
        Day++;
        if (Day > 7)
        {
            Day = 1;
            Week++;
            if (Week > 4)
            {
                Month++;
                Week = 1;
            }
        }

        return ToString();
    }

    public override string ToString()
    {
        return "Day: " + Day + ", Week: " + Week + ", Month: " + Month;
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

    public int Week
    {
        get
        {
            return week;
        }

        set
        {
            week = value;
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
}
