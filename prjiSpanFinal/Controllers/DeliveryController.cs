﻿using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using prjiSpanFinal.Models;
using prjiSpanFinal.ViewModels;
using prjiSpanFinal.ViewModels.Delivery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
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
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER))
            {
                iSpanProjectContext dbContext = new iSpanProjectContext();
                string memberJsonString = HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER);
                MemberAccount member = JsonSerializer.Deserialize<MemberAccount>(memberJsonString);
                int memberID = member.MemberId;
                var sellerID = dbContext.ProductDetails.Where(i => i.ProductDetailId == productDetailID).Select(i => i.Product.MemberId).FirstOrDefault();
                decimal unitPrice = dbContext.ProductDetails.Where(i => i.ProductDetailId == productDetailID).Select(i => i.UnitPrice).FirstOrDefault();
                var existingOrderDetails = dbContext.OrderDetails.Where(i => i.Order.MemberId == memberID && i.ProductDetail.Product.MemberId == sellerID && i.Order.StatusId == 1).Select(i => i);
                if (existingOrderDetails.Count() == 0)
                {
                    Order order = new Order
                    {
                        MemberId = memberID,
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
                    var newOrderID = dbContext.Orders.Where(i => i.MemberId == memberID).OrderByDescending(i => i.OrderId).Select(i => i.OrderId).FirstOrDefault();
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
                string orderDetailCount = dbContext.OrderDetails.Where(i => i.Order.MemberId == memberID).Select(i => i).Count().ToString();
                string remainingQty = GetRemainingQtyForSpecificMember(productDetailID, memberID);
                
                string[] arr = { orderDetailCount, remainingQty };
                string data = JsonSerializer.Serialize(arr);
                return Content(data);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public IActionResult ShowQtyToSpecificMember(int productDetailID)
        {
            iSpanProjectContext dbContext = new iSpanProjectContext();
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER))
            {
                string memberJsonString = HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER);
                MemberAccount member = JsonSerializer.Deserialize<MemberAccount>(memberJsonString);
                int memberID = member.MemberId;
                string remainingQty = GetRemainingQtyForSpecificMember(productDetailID, memberID);
                return Content(remainingQty);
            }
            else
            {
                var remainingQty = dbContext.ProductDetails.Where(i => i.ProductDetailId == productDetailID).Select(i => i.Quantity).FirstOrDefault();
                return Content(remainingQty.ToString());
            }
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
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER))
            {
                string memberJson = HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER);
                MemberAccount memberAccount = JsonSerializer.Deserialize<MemberAccount>(memberJson);
                int memberID = memberAccount.MemberId;
                iSpanProjectContext dbContext = new iSpanProjectContext();
                var allOrderDetail = dbContext.OrderDetails.Where(i => i.Order.MemberId == memberID && i.Order.StatusId == 1).Select(i => i);

                if (allOrderDetail.Count() > 0)
                {
                    List<CDeliveryOrderViewModel> cDeliveryOrderList = new List<CDeliveryOrderViewModel>();
                    //var orders = dbContext.Orders.Where(i => i.MemberId == memberID && i.StatusId == 1).Select(i => i).ToList();
                    foreach (var orderDetail in allOrderDetail)
                    {
                        int sellerID = orderDetail.ProductDetail.Product.Member.MemberId;
                        string sellerName = orderDetail.ProductDetail.Product.Member.Name;
                        byte[] productDetailPic = orderDetail.ProductDetail.Pic;
                        string productName = orderDetail.ProductDetail.Product.ProductName;
                        int purchaseQuantity = orderDetail.Quantity;
                        int productQuantity = orderDetail.ProductDetail.Quantity;
                        string style = orderDetail.ProductDetail.Style;
                        int orderDetailID = orderDetail.OrderDetailId;
                        decimal unitPrice = orderDetail.ProductDetail.UnitPrice;
                        CDeliveryOrderViewModel cDeliveryOrder = new CDeliveryOrderViewModel
                        {
                            sellerID = sellerID,
                            sellerName = sellerName,
                            productDetailPic = productDetailPic,
                            productName = productName,
                            purchaseQuantity = purchaseQuantity,
                            productQuantity = productQuantity,
                            style = style,
                            orderDetailID = orderDetailID,
                            unitPrice = unitPrice
                        };
                        cDeliveryOrderList.Add(cDeliveryOrder);
                        
                    }
                    CDeliveryShowCartViewModel cDeliveryShowCart = new CDeliveryShowCartViewModel
                    {
                        cart = cDeliveryOrderList
                    };

                    return View(cDeliveryShowCart);
                }
                else
                {
                    return RedirectToAction("Index","Home");
                }
            }
            else
            {
                return RedirectToAction("Login", "Member");
            }
        }
        public IActionResult ShowNoItemInCart()
        {
            iSpanProjectContext dbContext = new iSpanProjectContext();
            

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
        [HttpPost]
        public IActionResult AddComment(CSubmitCommentViewModel cSubmitComment)
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
