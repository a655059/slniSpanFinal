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
        public Ad AD { 
            get {
                return ADtoProd.Ad; ;
            }  
        }
        public Product Product{
            get 
            {
                return ADtoProd.Product;
            }
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
    }
}
