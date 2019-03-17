using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode_Day04
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = @"C:\Users\computer\source\repos\AdventOfCode-Day04\Advent_Calendar_-_day_4_input.txt";
            string readText = File.ReadAllText(path);
            string[] allFileLines = readText.Split('\n');

            List<LogEntry> guardLogs = new List<LogEntry>();
            foreach (string datarow in allFileLines)
            {
                if (datarow.Length > 15)
                {
                    guardLogs.Add(new LogEntry(datarow));
                }
            } 
            guardLogs = guardLogs.OrderBy(p => p.year).ThenBy(p => p.month).ThenBy(p => p.day).ThenBy(p => p.hour).ThenBy(p => p.minute).ToList();
            // List of logs has now been sorted by date & time.
            
            List<Guard> guardians = new List<Guard>();
            string currGuard = "";   int gSleep =0; int tempIndex = 0;  
            foreach (LogEntry jab in guardLogs)
            {
                if (jab.eventlog.Contains("Guard"))
                {
                    if (!guardians.Exists(x => x.IDnum == jab.eventlog))
                    {
                        guardians.Add(new Guard(jab.eventlog));
                    }
                    currGuard = jab.eventlog; 
                }
                else if (jab.eventlog.Contains("asleep"))
                {
                    // begin sleep count
                    gSleep = jab.minute;
                }
                else if (jab.eventlog.Contains("wakes"))
                { 
                    // calculate sleep & add to running total for that guard#
                    tempIndex = guardians.FindIndex(x => x.IDnum == currGuard);
                    guardians[tempIndex].totalMin += (jab.minute - gSleep);
                }
                else
                {
                    Console.WriteLine("invalid data line");
                    Console.WriteLine(jab);
                }
            }   // Done calculation of guard sleep times
            
            // Locate guard# who sleeps most
            tempIndex = 0;
            for(int y = 1; y < guardians.Count; y++)
            {
                if(guardians[tempIndex].totalMin < guardians[y].totalMin)
                {
                    tempIndex = y;
                }
            }

            int myMin = 0;  // The minute a guard sleeps the most
            int myQty = 0;  // The # of times the guard slept that particular minute.
            sleepyMinute(ref myMin, ref myQty, guardLogs, guardians[tempIndex].IDnum);

            // Final calculation.  Output.
            int myStringIndex = guardians[tempIndex].IDnum.IndexOf(" begins shift") - 7;
            int guardIDnum = Int32.Parse(guardians[tempIndex].IDnum.Substring(7, myStringIndex));
            Console.WriteLine("Part 1: ");
            Console.WriteLine(guardIDnum*myMin);

            // ==========================  BEGIN PART 2  ==========================
            string high_GuardID = "";
            int high_Minute = 0;
            int high_Qty = 0;

            for (int m = 0; m < guardians.Count; m++)
            {
                sleepyMinute(ref myMin, ref myQty, guardLogs, guardians[m].IDnum);
                if(myQty > high_Qty)
                {   // found a guard who sleeps more.
                    high_GuardID = guardians[m].IDnum;
                    high_Minute = myMin;
                    high_Qty = myQty;
                }
            }
            myStringIndex = high_GuardID.IndexOf(" begins shift") - 7;
            guardIDnum = Int32.Parse(high_GuardID.Substring(7, myStringIndex));
            Console.WriteLine("\nPart2: ");
            Console.WriteLine(guardIDnum * high_Minute);

            Console.WriteLine("\nPress ENTER to quit.");
            Console.ReadLine();
        }   // END of MAIN

        static void sleepyMinute(ref int mostMinute, ref int minuteQty, List<LogEntry> dataLogs, string GuardID)
        {   // mostMinute is the minute a guard sleeps the most.  minuteQty is the number of times.

            int[] minAsleep = new int[60];
            int secondCount = 0;  int timeAsleep = 0;
            // Fill array +1 each time they sleep that minute
            bool foundGuard = false;
            for (int y = 0; y < dataLogs.Count; y++)
            {
                if (!foundGuard)
                {
                    if (GuardID == dataLogs[y].eventlog)
                    {
                        foundGuard = true;
                    }
                }
                else if (dataLogs[y].eventlog.Contains("asleep"))
                {
                    timeAsleep = dataLogs[y].minute;
                }
                else if (dataLogs[y].eventlog.Contains("wakes"))
                {
                    for (int x = timeAsleep; x < dataLogs[y].minute; x++)
                    {   
                        minAsleep[x]++;
                        secondCount++;
                    }
                }
                else if (dataLogs[y].eventlog != GuardID)
                {
                    foundGuard = false;
                }
            }
            minuteQty = minAsleep.Max();  // number of times the most used minute.... was used.
            mostMinute = Array.IndexOf(minAsleep, minuteQty);   // the minute that was most used.
        }   // end of function sleepyMinute

        class Guard
        {
            public string IDnum;
            public int totalMin;

            public Guard(string _id)
            {
                IDnum = _id;
                totalMin = 0;
            }
        }
    }
}
