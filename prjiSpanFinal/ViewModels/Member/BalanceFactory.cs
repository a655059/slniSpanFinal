using prjiSpanFinal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjiSpanFinal.ViewModels.Member
{
    public class BalanceFactory
    {
        public List<BalanceRecordViewModel> fBalanceRecordFilter( int id, int status)
        {
            iSpanProjectContext db = new iSpanProjectContext();
            List<BalanceRecord> record = new List<BalanceRecord>();
            switch (status)
            {
                case 1:
                    record = db.BalanceRecords.Where(r => r.MemberId == id).Where(r => r.Amount >= 0).ToList();
                    break;
                case 2:
                    record = db.BalanceRecords.Where(r => r.MemberId == id).Where(r => r.Amount < 0).ToList();
                    break;
                default:
                    record = db.BalanceRecords.Where(r => r.MemberId == id).ToList();
                    break;
            }
            
            List<BalanceRecordViewModel> res = new List<BalanceRecordViewModel>();
            if (record.Any())
            {
                foreach (var item in record)
                {
                    BalanceRecordViewModel vm = new BalanceRecordViewModel
                    {
                        Balance = item,
                    };
                    res.Add(vm);
                }
            }
            return res;
        }
    }
}
