using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using prjiSpanFinal.Models;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace prjiSpanFinal.Controllers
{
    
    public class MsgApiController : Controller
    {
        private IWebHostEnvironment _enviro;
        public MsgApiController(IWebHostEnvironment p)
        {
            _enviro = p;
        }
        [HttpGet]
        public IActionResult Getchat(string scid, string sid)
        {
            var cid = Convert.ToInt32(scid);
            var id = Convert.ToInt32(sid);
            iSpanProjectContext dbcontext = new iSpanProjectContext();
            var cl = dbcontext.ChatLogs.Where(i => (i.SendFrom == cid && i.SendTo == id) || (i.SendFrom == id && i.SendTo == cid)).ToList();

            var q = dbcontext.ChatLogs.Where(i => (i.SendFrom == cid && i.SendTo == id)).ToList();
            foreach (var item in q)
            {
                item.HaveRead = true;
            }
            dbcontext.SaveChanges();
            return Json(cl);
        }

        [HttpGet]
        public IActionResult GetmembyAcc(string acc)
        {
            iSpanProjectContext dbcontext = new iSpanProjectContext();
            var mem = dbcontext.MemberAccounts.Where(m => m.MemberAcc == acc).Select(m => new { m.MemberId, m.MemberAcc, m.MemPic }).FirstOrDefault();
            if(mem != null)
            {
                if (mem.MemPic == null)
                {
                    string pName = "/img/Member/nopicmem.jpg";
                    string path = _enviro.WebRootPath + pName;
                    return Json(new { mem.MemberId, mem.MemberAcc, MemPic = System.IO.File.ReadAllBytes(path) });
                }
                return Json(mem);
            }
            return Json(null);
        }
        public IActionResult GetmembyId(int id)
        {
            iSpanProjectContext dbcontext = new iSpanProjectContext();
            var mem = dbcontext.MemberAccounts.Where(m => m.MemberId == id).Select(m => new { m.MemberId, m.MemberAcc, m.MemPic }).FirstOrDefault();
            if (mem != null)
            {
                if (mem.MemPic == null)
                {
                    string pName = "/img/Member/nopicmem.jpg";
                    string path = _enviro.WebRootPath + pName;
                    return Json(new { mem.MemberId, mem.MemberAcc, MemPic = System.IO.File.ReadAllBytes(path) });
                }
                return Json(mem);
            }
            return Json(null);
        }
        [HttpGet]
        public IActionResult AutoComplete(string keyword)
        {
            iSpanProjectContext dbcontext = new iSpanProjectContext();
            var memacc = dbcontext.MemberAccounts.Where(p => p.MemberAcc.Contains(keyword) && keyword != p.MemberAcc).Select(p => p.MemberAcc).Take(5);
            return Json(memacc);
        }
        [HttpGet]
        public void HaveRead(int msgid)
        {
            iSpanProjectContext dbcontext = new iSpanProjectContext();
            var chat = dbcontext.ChatLogs.Where(p => p.ChatLogId == msgid).Select(p => p).FirstOrDefault();
            chat.HaveRead = true;
            dbcontext.SaveChanges();
        }
        public IActionResult GetProductbyId(int id)
        {
            iSpanProjectContext dbcontext = new iSpanProjectContext();
            var prod = dbcontext.Products.Where(p => p.ProductId == id).Select(p => new { p.ProductPics.FirstOrDefault().Pic,p.ProductName, p.ProductDetails.FirstOrDefault().Style, p.ProductDetails.FirstOrDefault().Quantity, p.ProductDetails.FirstOrDefault().UnitPrice }).FirstOrDefault();
            if (prod != null)
            {
                if (prod.Pic == null)
                {
                    string pName = "/img/imageNotFound.png";
                    string path = _enviro.WebRootPath + pName;
                    return Json(new { prod.ProductName, prod.UnitPrice, Pic = System.IO.File.ReadAllBytes(path),prod.Quantity,prod.Style });
                }
                return Json(prod);
            }
            return Json(null);
        }
        public IActionResult GetNotificationbyID(int id)
        {
            iSpanProjectContext dbcontext = new iSpanProjectContext();
            return Json(dbcontext.Notifications.Where(n=>n.MemberId == id).Select(n=> new {n.HaveRead,n.IconType.IconPic,n.Link,n.Text,n.Time }).Take(5)); 
        }
        public void HaveReadAllNoti(int id)
        {
            iSpanProjectContext dbcontext = new iSpanProjectContext();
            var a = dbcontext.Notifications.Where(n => n.MemberId == id).Select(n => n).ToList();
            foreach(var item in a)
            {
                item.HaveRead = true;
            }
            dbcontext.SaveChanges();
        }
    }
}
