using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode_Day04
{
    class LogEntry
    {
    public int year;
    public int month;
    public int day;
    public int hour;
    public int minute;
    public string eventlog;

    public LogEntry(string rowdata)
    {
        try { 
            year = Int32.Parse(rowdata.Substring(1, 4));
            month = Int32.Parse(rowdata.Substring(6, 2));
            day = Int32.Parse(rowdata.Substring(9, 2));
            hour = Int32.Parse(rowdata.Substring(12, 2));
            minute = Int32.Parse(rowdata.Substring(15, 2));
            eventlog = rowdata.Substring(19);
        }
        catch (Exception)
        {
            Console.WriteLine("Unable to parse: {0}", rowdata);
        }
    }   

    public override string ToString()
    {
        return ( $"[{year}-{month}-{day} {hour}:{minute}] {eventlog}");
    }

}
}
