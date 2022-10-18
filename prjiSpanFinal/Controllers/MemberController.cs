using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using prjiSpanFinal.Models;
using prjiSpanFinal.ViewComponents;
using prjiSpanFinal.ViewModels.Member;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MimeKit;
namespace prjiSpanFinal.Controllers
{
    public class MemberController : Controller
    {
        private readonly IWebHostEnvironment _host;
        private readonly iSpanProjectContext _context;
        public MemberController(iSpanProjectContext context, IWebHostEnvironment host) 
        {
            _host = host;
            _context = context;
        }

        public IActionResult List()
        {
            return View();
        }
        //public IActionResult productShow()
        //{
        //    return PartialView("cshtml");
        //}
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            MemberAccount ACC = (new iSpanProjectContext()).MemberAccounts.FirstOrDefault(i => i.MemberAcc.Equals(model.txtAccount));
            if (ACC != null)
            {
                if (ACC.MemberPw.Equals(model.txtPassword))
                {
                    string jsonUser = JsonSerializer.Serialize(ACC);
                    HttpContext.Session.SetString(Dictionary.sk_登入key_User, jsonUser);

                    return RedirectToAction("Index","Home");
                }
            }
            return View();
        }
        //public IActionResult Edit1()
        //{
        //   return View();    
        //}
        public IActionResult Edit(int? id)
        {
            if (id != null)
            {
                iSpanProjectContext db = new iSpanProjectContext();
                MemberAccount prod = db.MemberAccounts.FirstOrDefault(p => p.MemberId == id);
                if (prod != null)
                {
                    return View(prod);
                }
            }
            return RedirectToAction("Login");
        }
        [HttpPost]
        public IActionResult Edit(MemberAccViewModel mem)
        {
            if (mem != null)
            {
                iSpanProjectContext db = new iSpanProjectContext();
                MemberAccount acc = db.MemberAccounts.FirstOrDefault(p => p.MemberId == mem.MemberId);
                if (acc != null)
                {
                    acc.Name = mem.Name;
                    acc.MemberAcc = mem.MemberAcc;
                    acc.Address = mem.Address;
                    acc.Phone = mem.Phone;
                    acc.RegionId = mem.RegionId;
                    acc.BackUpEmail = mem.BackUpEmail;
                    acc.Birthday = mem.Birthday;
                    acc.Email = mem.Email;
                    acc.Gender = mem.Gender;
                    acc.IsTw = mem.IsTw;
                    acc.MemPic = mem.MemPic;
                    acc.NickName = mem.NickName;
                    
                }
            }
            return View();
    }
        //public IActionResult Create()
        //{
        //    return View();
        //}


        public IActionResult Create1()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create1(MemberAccViewModel mem, IFormFile File1)
        {

            iSpanProjectContext db = new iSpanProjectContext();
            MemberAccount memberac = new MemberAccount();

            byte[] imgByte = null;
            using (var memoryStream = new MemoryStream())
            {
                //File1.CopyTo(memoryStream);
                imgByte = memoryStream.ToArray();
            }
            mem.MemPic = imgByte;
            memberac = mem.memACC;
            memberac.RegionId = db.RegionLists.FirstOrDefault(p => p.RegionName == mem.regionName).RegionId;
            if (mem.gender == "female")
            {
                memberac.Gender = 2;
            }
            else if (mem.gender == "male")
            {
                memberac.Gender = 1;
            }
            else
            {
                memberac.Gender = 0;
            }
            if (mem.TW == "tw")
            {
                memberac.IsTw = true;
            }
            else
            {
                memberac.IsTw = false;
            }
            db.MemberAccounts.Add(memberac);
            db.SaveChanges();
            return RedirectToAction("Login");
        }
        public IActionResult Like()
        {
            return View();
        }
        public IActionResult Coupon()
        {
            return View();
        }
        public IActionResult Order()
        {
            return View();
        }
        public IActionResult OrderDetail()
        {
            return View();
        }
        public IActionResult forgetPw() 
        {
            return View();
        }
        [HttpPost]
        public IActionResult forgetPw(MemberAccViewModel mem)
        {
            iSpanProjectContext db = new iSpanProjectContext();
            MemberAccount memberac = new MemberAccount();
            memberac = mem.memACC;
            var mempass = "";
           mempass = db.MemberAccounts.FirstOrDefault(p => p.Email == mem.PhoneMail || p.Phone == mem.PhoneMail).MemberPw;
            if (!String.IsNullOrEmpty(mempass)) { 
            MimeMessage message = new MimeMessage();
            BodyBuilder builder = new BodyBuilder();
            var image = builder.LinkedResources.Add(@"C:\Users\Student\source\repos\slniSpanFinal\prjiSpanFinal\wwwroot\img\蝦到爆.png");
            //==>這裡可以放入圖片路徑

            builder.HtmlBody = System.IO.File.ReadAllText("./Views/Member/ChangePwMail.cshtml");
            //builder.HtmlBody = "您的密碼為:"+mempass+"\n"+ $"當前時間:{DateTime.Now:yyyy-MM-dd HH:mm:ss}";
            //=>內容

            message.From.Add(new MailboxAddress("蝦到爆商城", "ShopDaoBao@outlook.com"));
            message.To.Add(new MailboxAddress("詹勳棋", "harry80011@gmail.com"));
            message.Subject = "忘記密碼"; //==>標題
            message.Body = builder.ToMessageBody();


            using (SmtpClient client = new SmtpClient())
            {
                client.Connect("smtp.outlook.com", 25, MailKit.Security.SecureSocketOptions.StartTls);
                //第二個參數是port
                //outlook.com smtp.outlook.com port:25
                //yahoo smtp.mail.yahoo.com.tw port:465
                //gmail smtp.gmail.com port:587
                client.Authenticate("ShopDaoBao@outlook.com", "SDB20221013");
                client.Send(message);
                client.Disconnect(true);
            }
                return RedirectToAction("Login");
            }
            
            return RedirectToAction("forgetPw");
        }
        public IActionResult ChangePw()
        {
            return View();
        }
        public IActionResult City()
        {
            var cities = _context.CountryLists.Select(a => a.CountryName).Distinct();
            return Json(cities);
        }
        public IActionResult getCityID(string city)
        {
            var sites = _context.CountryLists.Where(a => a.CountryName == city).Select(a => a.CountryId).Distinct();
            return Json(sites);
        }
        public IActionResult Site(int site)
        {
            var sites = _context.RegionLists.Where(a => a.CountryId == site).Select(a => a.RegionName).Distinct();
            return Json(sites);
        }
    }
}
