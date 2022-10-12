﻿using Microsoft.AspNetCore.Mvc;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace prjiSpanFinal.Controllers
{
    public class DeliveryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ShowCart()
        {
            return View();
        }
        public IActionResult Checkout()
        {
            return View();
        }
        public IActionResult AddComment()
        {
            return View();
        }
        public IActionResult CheckoutForm(int? count)
        {
            return ViewComponent("DeliveryFillCheckoutForm", count);
        }
        public IActionResult OrderSuccess()
        {
            MimeMessage message = new MimeMessage();
            message.From.Add(new MailboxAddress("Jacob", "maimaisatt@gmail.com"));
            message.To.Add(new MailboxAddress("Jacob", "maimaisatt@gmail.com"));
            message.Subject = "測試一下";
            BodyBuilder builder = new BodyBuilder();
            builder.TextBody = "你好，這是測試";
            message.Body = builder.ToMessageBody();
            using(SmtpClient client = new SmtpClient())
            {
                
            }
            return View();
        }
    }
}
