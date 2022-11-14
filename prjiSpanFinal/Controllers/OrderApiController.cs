using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using prjiSpanFinal.Models;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using prjiSpanFinal.ViewModels;
using Microsoft.EntityFrameworkCore;
using Google.Apis.Sheets.v4.Data;
using static System.Net.Mime.MediaTypeNames;
using System.Reflection.Metadata;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace prjiSpanFinal.Controllers
{
    public class OrderApiController : Controller
    {
        iSpanProjectContext dbcontext = new iSpanProjectContext();
        private IWebHostEnvironment _enviro;
        public OrderApiController(IWebHostEnvironment p)
        {
            _enviro = p;
        }
        public void SettoShipping(int id)
        {
            iSpanProjectContext dbcontext = new iSpanProjectContext();
            OrderDetail a = dbcontext.OrderDetails.FirstOrDefault(o => o.OrderDetailId == id);
            a.ShippingStatusId = 2;
            int thisorderid = dbcontext.OrderDetails.FirstOrDefault(o => o.OrderDetailId == id).OrderId;
            int countofods = dbcontext.OrderDetails.Where(o => o.OrderId == thisorderid).Count();
            int countof45ods = dbcontext.OrderDetails.Where(o => o.OrderId == thisorderid && (o.ShippingStatusId == 4 || o.ShippingStatusId == 5)).Count();
            if (countof45ods != countofods)
            {
                Order b = dbcontext.Orders.FirstOrDefault(o => o.OrderId == thisorderid);
                b.StatusId = 4;
                b.ShippingDate = DateTime.Now;
                SendNoti(2, b.MemberId, "賣家已經出貨囉，", "/Member/Order?sort=0&tab=4", "可以在購買清單中查看訂單狀況。");
            }
            dbcontext.SaveChanges();
        }
        public void SendNoti(int type, int id, string text, string link, string content)
        {
            if (text == null)
            {
                text = "";
            }
            iSpanProjectContext dbcontext = new iSpanProjectContext();
            Notification a = new Notification() { MemberId = id, IconTypeId = type, Text = text, Link = link, HaveRead = false, Time = DateTime.Now, TextContent = content };
            dbcontext.Notifications.Add(a);
            dbcontext.SaveChanges();
        }
        public void SettoArrived(int id)
        {
            iSpanProjectContext dbcontext = new iSpanProjectContext();
            OrderDetail a = dbcontext.OrderDetails.FirstOrDefault(o => o.OrderDetailId == id);
            a.ShippingStatusId = 4;
            int thisorderid = dbcontext.OrderDetails.FirstOrDefault(o => o.OrderDetailId == id).OrderId;
            int countofods = dbcontext.OrderDetails.Where(o => o.OrderId == thisorderid).Count();
            int countof45ods = dbcontext.OrderDetails.Where(o => o.OrderId == thisorderid && (o.ShippingStatusId == 4 || o.ShippingStatusId == 5)).Count();
            if(countof45ods == countofods)
            {
                Order b = dbcontext.Orders.FirstOrDefault(o => o.OrderId == thisorderid);
                b.StatusId = 5;
            }
            dbcontext.SaveChanges();
        }

        public IActionResult RefreshOD(int id)
        {
            iSpanProjectContext dbcontext = new iSpanProjectContext();
            OrderDetail a = dbcontext.OrderDetails.FirstOrDefault(o => o.OrderDetailId == id);
            Order b = dbcontext.Orders.FirstOrDefault(o => o.OrderId == a.OrderId);
            return Json(new { a.ShippingStatusId, b.ShippingDate, b.StatusId });
        }
    }
}
