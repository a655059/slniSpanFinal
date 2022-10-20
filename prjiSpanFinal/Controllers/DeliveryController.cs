using MailKit.Net.Smtp;
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
                    var allInfo = dbContext.OrderDetails.Where(i => i.Order.MemberId == memberID && i.Order.StatusId == 1).Select(i => new {
                        sellerID = i.ProductDetail.Product.MemberId,
                        sellerAcc = i.ProductDetail.Product.Member.MemberAcc,
                        productName = i.ProductDetail.Product.ProductName,
                        orderDetail = i,
                        productDetail = i.ProductDetail,
                    });
                    var shippers = dbContext.Shippers.Select(i => i).ToList();
                    var shipperToSellers = dbContext.ShipperToSellers.Select(i => i).ToList();
                    foreach (var info in allInfo)
                    {
                        int sellerID = info.sellerID;
                        string sellerAcc = info.sellerAcc;
                        string productName = info.productName;
                        OrderDetail orderDetail = info.orderDetail;
                        ProductDetail productDetail = info.productDetail;
                        CDeliveryOrderViewModel cDeliveryOrder = new CDeliveryOrderViewModel
                        {
                            sellerID = sellerID,
                            sellerAcc = sellerAcc,
                            productName = productName,
                            orderDetail = orderDetail,
                            productDetail = productDetail
                        };
                        cDeliveryOrderList.Add(cDeliveryOrder);
                    }
                    List<int> sellerIDList = new List<int>();
                    foreach (var a in cDeliveryOrderList)
                    {
                        if (sellerIDList.Contains(a.sellerID))
                        {
                            continue;
                        }
                        else
                        {
                            sellerIDList.Add(a.sellerID);
                        }
                    }
                    List<CShipperToSellerViewModel> cShipperToSellerList = new List<CShipperToSellerViewModel>();
                    foreach (var sellerID in sellerIDList)
                    {
                        var shipperIDs = shipperToSellers.Where(i => i.MemberId == sellerID).Select(i=>i.ShipperId);
                        List<Shipper> shipperList = new List<Shipper>();
                        foreach (var shipperID in shipperIDs)
                        {
                            var shipper = shippers.Where(i => i.ShipperId == shipperID).Select(i => i).FirstOrDefault();
                            shipperList.Add(shipper);
                        }
                       
                        CShipperToSellerViewModel cShipperToSeller = new CShipperToSellerViewModel
                        {
                            sellerID = sellerID,
                            shippers = shipperList,
                        };
                        cShipperToSellerList.Add(cShipperToSeller);

                    }
                    CDeliveryShowCartViewModel cDeliveryShowCart = new CDeliveryShowCartViewModel
                    {
                        cart = cDeliveryOrderList,
                        shipperToSeller= cShipperToSellerList
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

        public IActionResult DeleteOrderDetail(int orderDetailID)
        {
            iSpanProjectContext dbContext = new iSpanProjectContext();
            var orderID = dbContext.OrderDetails.Where(i => i.OrderDetailId == orderDetailID).Select(i => i.OrderId).FirstOrDefault();
            int orderDetailCount = dbContext.OrderDetails.Where(i => i.OrderId == orderID).Select(i => i).Count();
            if (orderDetailCount == 1)
            {
                var orderDetail = dbContext.OrderDetails.Where(i => i.OrderDetailId == orderDetailID).Select(i => i).FirstOrDefault();
                dbContext.OrderDetails.Remove(orderDetail);
                dbContext.SaveChanges();
                var order = dbContext.Orders.Where(i => i.OrderId == orderID).Select(i => i).FirstOrDefault();
                dbContext.Orders.Remove(order);
                dbContext.SaveChanges();
                return Content("0");
            }
            else
            {
                var orderDetail = dbContext.OrderDetails.Where(i => i.OrderDetailId == orderDetailID).Select(i => i).FirstOrDefault();
                dbContext.OrderDetails.Remove(orderDetail);
                dbContext.SaveChanges();
                return Content("1");
            }
            
            
        }
        public IActionResult ShowNoItemInCart()
        {
            return ViewComponent("ShowNoItemInCart");
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
