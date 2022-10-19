using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.Runtime.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.EntityFrameworkCore;
using prjiSpanFinal.Models;
using prjiSpanFinal.ViewModels;
using prjiSpanFinal.ViewModels.Popup;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace prjiSpanFinal.ViewComponents
{
    public static class CusDistinctBy
    {
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>
        (this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }
    }
    public class MessagePopupViewComponent : ViewComponent
    {

        public IViewComponentResult Invoke()
        {
            if (HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER) == null)
                return View(new PopupViewModel());
            MemberAccount a = JsonSerializer.Deserialize<MemberAccount>(HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER));
            PopupViewModel b = new PopupViewModel();
            b.MemID = a.MemberId;
            b.MemAcc = a.MemberAcc;
            if (a.MemPic != null)
            {
                b.MemPic = a.MemPic;
            }
            else
            {
                b.MemPic = File.ReadAllBytes("~/img/Member/nopicmem.jpg");
            }

            iSpanProjectContext dbcontext = new iSpanProjectContext();
            var q = dbcontext.ChatLogs.Where(c => c.SendTo == b.MemID).Select(c => new { c.SendFrom, c.Msg, c.SendFromNavigation.MemberAcc, c.SendFromNavigation.MemPic, }).DistinctBy(c => c.SendFrom).ToList();
            var q1 = dbcontext.ChatLogs.AsEnumerable().Where(c => c.SendTo == b.MemID).GroupBy(c => c.SendFrom).ToList();
            List<int> idlist = new List<int>();
            List<string> acclist = new List<string>();
            List<string> msglist = new List<string>();
            List<byte[]> bslist = new List<byte[]>();
            List<int> hrlist = new List<int>();
            for (int i = 0; i < q.Count ; i++)
            {
                idlist.Add(q[i].SendFrom);
                acclist.Add(q[i].MemberAcc);
                msglist.Add(q[i].Msg);
                hrlist.Add(q1[i].Count(c => c.HaveRead == false));
                if (q[i].MemPic != null)
                {
                    bslist.Add(q[i].MemPic);
                }
                else
                {
                    bslist.Add(File.ReadAllBytes("~/img/Member/nopicmem.jpg"));
                }
            }
            b.CMemPic = bslist;
            b.CMemAcc = acclist;
            b.COneMsg = msglist;
            b.CMemID = idlist;
            b.CHaveread = hrlist;
            return View(b);
        }
    }
}
