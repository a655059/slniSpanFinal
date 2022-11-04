using prjiSpanFinal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjiSpanFinal.ViewModels.seller
{
    public class CADeffectViewmodel
    {
        //public Ad ADeffect { get; set; }

        public int ADID { get; set; }
        public decimal ADFee { get; set; }
        public int ADPeriod { get; set; }
        public int TotalCost
        {
            get
            {
                return Convert.ToInt32(ADFee) * ADPeriod;
            }
        }
        public string TypeName
        {
            get;
            set;
        }
        public string TypeNameDescription
        {
            get;
            set;
        }
    }
}
