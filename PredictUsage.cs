using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cw2
{
    public class PredictUsage
    {
        public double[] TotalCalculations(List<Event> eventsData)
        {
            //chart2.Series["Total"].Points.Clear();
            double[] totalMean = new double[4]; //Array of calculated final mean duration

            //var day1 = DateTime.Today.Date; //today date
            var day11 = GetMonday();

            for (int i = 0; i < 4; i++)
            {
                double totalW1 = 0, totalW2 = 0; //total durations of last week & 5 weeks before to cal mean

                // 2 week starts & ends to compare them and to get mean
                var mondayOfLastWeek = day11;
                var sundayOfLastWeek = day11.AddDays(6);
                var mondayOfLastmonth = day11.AddDays(-28);
                var sundayOfLastmonth = mondayOfLastmonth.AddDays(6);
                Console.WriteLine($"{mondayOfLastWeek.ToString()} **** {sundayOfLastWeek.ToString()}  **** {mondayOfLastmonth.ToString()}  **** {sundayOfLastmonth.ToString()}");

                for (int u = 0; u < eventsData.Count; u++)
                {
                    if ((eventsData[u].DateTime.Date >= mondayOfLastWeek) && (eventsData[u].DateTime.Date <= sundayOfLastWeek))
                    {
                        totalW1 += eventsData[u].Duration;
                    }

                    if ((eventsData[u].DateTime.Date >= mondayOfLastmonth) && (eventsData[u].DateTime.Date <= sundayOfLastmonth))
                    {
                        totalW2 += eventsData[u].Duration;
                    }
                }

                day11 = day11.Date.AddDays(-7);
                double mean = ((totalW1 + totalW2) / 2); //mean calculating
                totalMean[i] = mean;
            }

            Array.Reverse(totalMean);

            return totalMean;
        }




        public Tuple<double[], double[]> ExtraCalculations(List<Event> eventsData)
        {
            //chart2.Series["Total"].Points.Clear();
            double[] appointMean = new double[4]; //Array of calculated appointment mean duration
            double[] tasksMean = new double[4]; //Array of calculated tasks mean duration

            var day = GetMonday();

            for (int i = 0; i < 4; i++)
            {
                var day2 = day.Date.AddDays(-28); //getting 4 weeks before date

                double appoints1 = 0, appoints2 = 0;
                double tasks1 = 0, tasks2 = 0;

                // 2 week starts & ends to compare them and to get mean
                var mondayOfLastWeek = day;
                var sundayOfLastWeek = day.AddDays(6);
                var mondayOfLastmonth = day.AddDays(-28);
                var sundayOfLastmonth = mondayOfLastmonth.AddDays(6);


                for (int u = 0; u < eventsData.Count; u++)
                {
                    if ((eventsData[u].DateTime.Date >= mondayOfLastWeek) && (eventsData[u].DateTime.Date <= sundayOfLastWeek))
                    {
                        //if statement to calculate appointments seperately
                        if (eventsData[u].EventType == "Appointments")
                        {
                            appoints1 += eventsData[u].Duration;
                        }
                        //if statement to calculate appointments seperately
                        if (eventsData[u].EventType == "Tasks")
                        {
                            tasks1 += eventsData[u].Duration;
                        }
                    }

                    if ((eventsData[u].DateTime.Date >= mondayOfLastmonth) && (eventsData[u].DateTime.Date <= sundayOfLastmonth))
                    {
                        //if statement to calculate appointments seperately
                        if (eventsData[u].EventType == "Appointments")
                        {
                            appoints2 += eventsData[u].Duration;
                        }
                        //if statement to calculate appointments seperately
                        if (eventsData[u].EventType == "Tasks")
                        {
                            tasks2 += eventsData[u].Duration;
                        }
                    }
                }

                day = day.Date.AddDays(-7);

                double mean1 = ((appoints1 + appoints2) / 2);
                double mean2 = ((tasks1 + tasks2) / 2);

                appointMean[i] = mean1;
                tasksMean[i] = mean2;
            }

            Array.Reverse(appointMean);
            Array.Reverse(tasksMean);

            return Tuple.Create(appointMean, tasksMean);
        }


        public DateTime GetMonday()
        {
            var lastweek = DateTime.Now.Date.AddDays(-7);
            var mondayOfLastWeek = DateTime.Now;

            Boolean monday = false;
            while (monday == false)
            {
                if (lastweek.Date.DayOfWeek == DayOfWeek.Monday)
                {
                    //Console.WriteLine($"Monday found - {lastweek.Date}");
                    mondayOfLastWeek = lastweek.Date;
                    monday = true;
                }
                else
                {
                    //Console.WriteLine($"Monday not found - {lastweek.Date}");
                    lastweek = lastweek.AddDays(-1);
                }
            }
            return mondayOfLastWeek;
        }
    }
}
