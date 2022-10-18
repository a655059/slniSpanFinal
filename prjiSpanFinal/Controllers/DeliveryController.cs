using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using prjiSpanFinal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjiSpanFinal.Controllers
{
    public class DeliveryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AddItemToCart(int productDetailID, int quantity)
        {
            iSpanProjectContext dbContext = new iSpanProjectContext();
            var sellerID = dbContext.ProductDetails.Where(i => i.ProductDetailId == productDetailID).Select(i => i.Product.MemberId).FirstOrDefault();
            decimal unitPrice = dbContext.ProductDetails.Where(i => i.ProductDetailId == productDetailID).Select(i => i.UnitPrice).FirstOrDefault();
            var existingOrderDetails = dbContext.OrderDetails.Where(i => i.Order.MemberId == 1 && i.ProductDetail.Product.MemberId == sellerID && i.Order.StatusId == 1).Select(i => i);
            if (existingOrderDetails.Count() == 0)
            {
                Order order = new Order
                {
                    MemberId = 1,
                    OrderDatetime = DateTime.Now,
                    RecieveAdr = "",                     //等session
                    FinishDate = DateTime.Now,
                    CouponId = 1,
                    StatusId = 1,
                    ShipperId = 1,
                    PaymentDate = DateTime.Now,
                    ShippingDate = DateTime.Now,
                    ReceiveDate = DateTime.Now,
                    PaymentId = 1,
                    OrderMessage = "",
                };
                dbContext.Orders.Add(order);
                dbContext.SaveChanges();
                var newOrderID = dbContext.Orders.Where(i => i.MemberId == 1).OrderByDescending(i => i.OrderId).Select(i => i.OrderId).FirstOrDefault();
                OrderDetail orderDetail = new OrderDetail
                {
                    OrderId = newOrderID,
                    ProductDetailId = productDetailID,
                    Quantity = quantity,
                    ReceiveDate = DateTime.Now,
                    ShippingStatusId = 1,
                    Unitprice = unitPrice,
                };
                dbContext.OrderDetails.Add(orderDetail);
                dbContext.SaveChanges();
            }
            else
            {
                var existingOrderDetail = existingOrderDetails.Where(i => i.ProductDetailId == productDetailID).Select(i => i);
                if (existingOrderDetail.Count() != 0)
                {
                    OrderDetail x = existingOrderDetail.FirstOrDefault();
                    x.Quantity += quantity;
                    dbContext.SaveChanges();
                }
                else
                {
                    int existingOrderID = existingOrderDetails.Select(i => i.OrderId).FirstOrDefault();
                    
                    OrderDetail orderDetail = new OrderDetail
                    {
                        OrderId = existingOrderID,
                        ProductDetailId = productDetailID,
                        Quantity = quantity,
                        ReceiveDate = DateTime.Now,
                        ShippingStatusId = 1,
                        Unitprice = unitPrice,
                    };
                    dbContext.OrderDetails.Add(orderDetail);
                    dbContext.SaveChanges();
                }
            }
            string remainingQty = GetRemainingQtyForSpecificMember(productDetailID, 1);
            return Content(remainingQty);
        }
        public IActionResult ShowQtyToSpecificMember(int productDetailID)
        {
            string remainingQty = GetRemainingQtyForSpecificMember(productDetailID, 1);
            return Content(remainingQty);
        }

        public string GetRemainingQtyForSpecificMember(int productDetailID, int memberID)
        {
            iSpanProjectContext dbContext = new iSpanProjectContext();
            var qty = dbContext.ProductDetails.Where(i => i.ProductDetailId == productDetailID).Select(i => i.Quantity).FirstOrDefault();
            var qtyInCart = dbContext.OrderDetails.Where(i => i.ProductDetailId == productDetailID && i.Order.MemberId == memberID && i.Order.StatusId == 1).Select(i => i.Quantity);
            int remainingQty = qty;
            if (qtyInCart.Count() != 0)
            {
                remainingQty = qty - qtyInCart.FirstOrDefault();
            }
            return remainingQty.ToString();
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
        public IActionResult CheckoutConfirm()
        {
            return View();
        }
        public IActionResult OrderSuccess()
        {
            MimeMessage message = new MimeMessage();
            message.From.Add(new MailboxAddress("Jacob", "ShopDaoBao@outlook.com"));
            message.To.Add(new MailboxAddress("Jacob", "maimaisatt@gmail.com"));
            message.Subject = "測試一下";
            BodyBuilder builder = new BodyBuilder();
            builder.HtmlBody = System.IO.File.ReadAllText("./Views/Delivery/MailContent.cshtml");
            message.Body = builder.ToMessageBody();
            using (SmtpClient client = new SmtpClient())
            {
                client.Connect("smtp.outlook.com", 25, false);
                client.Authenticate("ShopDaoBao@outlook.com", "SDB20221013");
                client.Send(message);
                client.Disconnect(true);
            }
            return View();
        }
    }
}
