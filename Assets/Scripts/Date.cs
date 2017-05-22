/// <summary>
/// Simple class for managing the in-game date.
/// </summary>
public class Date {

    int day;
    int week;
    int month;

    /// <summary>
    /// Constructor for specific date
    /// </summary>
    /// <param name="day">The day number</param>
    /// <param name="week">The week number</param>
    /// <param name="month">The month number</param>
    public Date(int day, int week, int month)
    {
        Day = day;
        Week = week;
        Month = month;
    }

    /// <summary>
    /// Default constructor
    /// </summary>
    public Date()
    {
        Day = 1;
        Week = 1;
        Month = 1;
    }

    /// <summary>
    /// Checks for a new week if the given day is the first of the week
    /// </summary>
    /// <returns>True if the first of the week, false if not</returns>
    public bool isNewWeek()
    {
        return day == 1;
    }

    /// <summary>
    /// Function to increment teh date
    /// </summary>
    /// <returns>Returns a string of the new date</returns>
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
