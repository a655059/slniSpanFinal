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
            var q1 = dbcontext.ChatLogs.Where(c => c.SendFrom == b.MemID).Select(c => new { c.ChatLogId,c.SendTo,c.Msg,c.HaveRead,c.SendToNavigation.MemberAcc,c.SendToNavigation.MemPic}).OrderByDescending(c=>c.ChatLogId).DistinctBy(c => c.SendTo).ToList();
            var q2 = dbcontext.ChatLogs.Where(c => c.SendTo == b.MemID).Select(c => new { c.ChatLogId, c.SendFrom, c.Msg, c.HaveRead, c.SendFromNavigation.MemberAcc, c.SendFromNavigation.MemPic }).OrderByDescending(c => c.ChatLogId).DistinctBy(c => c.SendFrom).ToList();
            List<int> idlist = new List<int>();
            List<string> acclist = new List<string>();
            List<string> msglist = new List<string>();
            List<byte[]> bslist = new List<byte[]>();
            List<int> hrlist = new List<int>();
            List<int> SendList = new List<int>();
            int count = q1.Count + q2.Count;
            int count1 = 0, count2 = 0;
            q1.Add(new { ChatLogId = 0, SendTo = 0, Msg = "000", HaveRead = false, MemberAcc = "aaa", MemPic = new byte[5] });
            q2.Add(new { ChatLogId = 0, SendFrom = 0, Msg = "000", HaveRead = false, MemberAcc = "aaa", MemPic = new byte[5] });
            for (; count1+count2 < count ;)
            {
                if(q1[count1].ChatLogId > q2[count2].ChatLogId)
                {
                    if(SendList.Contains(q1[count1].SendTo))
                    {
                        count1++;
                        continue;
                    }
                    if (q1[count1].MemPic != null)
                    {
                        bslist.Add(q1[count1].MemPic);
                    }
                    else
                    {
                        string pName = "/img/Member/nopicmem.jpg";
                        string path = _enviro.WebRootPath + pName;
                        bslist.Add(File.ReadAllBytes(path));
                    }
                    acclist.Add(q1[count1].MemberAcc);
                    int id = q1[count1].SendTo;
                    idlist.Add(id);
                    msglist.Add(q1[count1].Msg);
                    hrlist.Add(0);
                    SendList.Add(q1[count1].SendTo);
                    count1++;
                }
                else
                {
                    if (SendList.Contains(q2[count2].SendFrom))
                    {
                        count2++;
                        continue;
                    }
                    if (q2[count2].MemPic != null)
                    {
                        bslist.Add(q2[count2].MemPic);
                    }
                    else
                    {
                        string pName = "/img/Member/nopicmem.jpg";
                        string path = _enviro.WebRootPath + pName;
                        bslist.Add(File.ReadAllBytes(path));
                    }
                    acclist.Add(q2[count2].MemberAcc);
                    int id = q2[count2].SendFrom;
                    idlist.Add(id);
                    msglist.Add(q2[count2].Msg);
                    hrlist.Add(q2.Where(c => c.HaveRead == false && c.SendFrom == q2[count2].SendFrom).Count());
                    SendList.Add(q2[count2].SendFrom);
                    count2++;
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
