using Microsoft.AspNetCore.Hosting;
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
        private IWebHostEnvironment _enviro;
        public MessagePopupViewComponent(IWebHostEnvironment p)
        {
            _enviro = p;
        }

        public string compareTime(string msg1,string msg2)
        {
            string time1 = msg1.Substring(4, 17);
            string time2 = msg2.Substring(4, 17);
            DateTime dtime1 = new DateTime(Int32.Parse(time1.Substring(9, 4)), Int32.Parse(time1.Substring(13, 2)), Int32.Parse(time1.Substring(15, 2)), Int32.Parse(time1.Substring(0, 2)), Int32.Parse(time1.Substring(2, 2)), Int32.Parse(time1.Substring(4, 2)), Int32.Parse(time1.Substring(6, 3)));
            DateTime dtime2 = new DateTime(Int32.Parse(time2.Substring(9, 4)), Int32.Parse(time2.Substring(13, 2)), Int32.Parse(time2.Substring(15, 2)), Int32.Parse(time2.Substring(0, 2)), Int32.Parse(time2.Substring(2, 2)), Int32.Parse(time2.Substring(4, 2)), Int32.Parse(time2.Substring(6, 3)));
            return dtime1 > dtime2 ? msg1 : msg2;
        }

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
                string pName = "/img/Member/nopicmem.jpg";
                string path = _enviro.WebRootPath + pName;
                b.MemPic = File.ReadAllBytes(path);
            }
            iSpanProjectContext dbcontext = new iSpanProjectContext();
            var q = dbcontext.ChatLogs.Where(c => c.SendTo == b.MemID || c.SendFrom == b.MemID).Select(c => c).OrderByDescending(c=>c.ChatLogId).ToList();
            var q1 = dbcontext.ChatLogs.AsEnumerable().Where(c => c.SendTo == b.MemID).GroupBy(c => c.SendFrom).ToList();
            List<int> idlist = new List<int>();
            List<string> acclist = new List<string>();
            List<string> msglist = new List<string>();
            List<byte[]> bslist = new List<byte[]>();
            List<int> hrlist = new List<int>();
            List<int> ProSendFrom = new List<int>();
            List<int> ProSendTo = new List<int>();
            for (int i = 0; i < q.Count ; i++)
            {
                if((ProSendFrom.Contains(q[i].SendFrom)||ProSendTo.Contains(q[i].SendTo)) && (q[i].SendFrom != b.MemID && q[i].SendTo != b.MemID))
                {

                }

                if (q[i].MemPic != null)
                {
                    bslist.Add(q[i].MemPic);
                }
                else
                {
                    string pName = "/img/Member/nopicmem.jpg";
                    string path = _enviro.WebRootPath  + pName;
                    bslist.Add(File.ReadAllBytes(path));
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
