using prjiSpanFinal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjiSpanFinal.ViewModels.Member
{
    public class BalanceRecordViewModel
    {
        public BalanceRecord Balance { get; set; }

        public int BalanceCount { get; set; }
        public int RecordMoney
        {
            get { return Balance.Amount; }
        }
        public string RecordDate
        {
            get
            {
                return Balance.Record.ToString("yyyy/MM/dd HH:mm:ss");
            }
        }
        public int RecordNumber
        {
            get { return Balance.BalanceRecordId; }
        }
        public string RecordReason
        {
            get { return Balance.Reason; }
        }
    }
}
