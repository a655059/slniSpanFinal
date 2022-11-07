using prjiSpanFinal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjiSpanFinal.ViewModels.seller
{
    public class CADSubviewmodel
    {
        public AdtoProduct ADtoProd { get; set; }

        public string adTypeName
        {
            get; set;
        }
        public Product prod{
            get;set;
        }        
        public bool isSubActive{
            get
            {
                return ADtoProd.IsSubActive;
            }        
        }

        public string Startdate{
            get
            {
                return ADtoProd.StartDate.ToString("yyyy/MM/dd HH:mm:ss");
            }
        }
        public string Enddate
        {
            get
            {
                return ADtoProd.EndDate.ToString("yyyy/MM/dd HH:mm:ss");
            }
        }
        public int ExpoTimes
        {
            get { return ADtoProd.ExpoTimes; }
        }
        public int ClickTimes
        {
            get { return ADtoProd.ClickTimes; }
        }
        public string Remaining
        {
            get
            {
                if (DateTime.Compare(ADtoProd.EndDate, DateTime.Now) >= 0)
                {
                    TimeSpan ts = (ADtoProd.EndDate).Subtract(DateTime.Now);
                    string hh = ts.Hours.ToString();
                    if (ts.Hours < 10)
                    {
                        hh = "0" + ts.Hours;
                    }
                    string mm = ts.Minutes.ToString();
                    if (ts.Minutes < 10)
                    {
                        mm = "0" + ts.Minutes;
                    }
                    string ss = ts.Seconds.ToString();
                    if (ts.Seconds < 10)
                    {
                        ss = "0" + ts.Seconds;
                    }
                    return string.Format("{0}天 {1}:{2}:{3}", ts.Days, hh, mm, ss);
                }
                else
                {
                    return "0天 00:00:00";
                }
            }
        }
        public double RemainingForSort
        {
            get
            {
                if (DateTime.Compare(ADtoProd.EndDate, DateTime.Now) >= 0)
                {
                    TimeSpan ts = (ADtoProd.EndDate).Subtract(DateTime.Now);

                    return ts.TotalSeconds;
                }
                else
                {
                    return 0;
                }

            }
        }
        public int dataCount { get; set; }
    }
}
