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
            var memacc = dbcontext.MemberAccounts.Where(p => p.MemberAcc.Contains(keyword) && keyword != p.MemberAcc).Select(p => p.MemberAcc);
            return Json(memacc);
        }
    }
}
