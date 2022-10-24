using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using prjiSpanFinal.Models;
using prjiSpanFinal.ViewModels;
using prjiSpanFinal.ViewModels.Delivery;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace prjiSpanFinal.Controllers
{
    public class DeliveryController : Controller
    {
        private IWebHostEnvironment _enviro;
        public DeliveryController(IWebHostEnvironment enviro)
        {
            _enviro = enviro;
        }
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
                        product = i.ProductDetail.Product,
                        orderDetail = i,
                        productDetail = i.ProductDetail,
                    });
                    var shippers = dbContext.Shippers.Select(i => i).ToList();
                    var shipperToSellers = dbContext.ShipperToSellers.Select(i => i).ToList();
                    foreach (var info in allInfo)
                    {
                        int sellerID = info.sellerID;
                        string sellerAcc = info.sellerAcc;
                        Product product = info.product;
                        OrderDetail orderDetail = info.orderDetail;
                        ProductDetail productDetail = info.productDetail;
                        CDeliveryOrderViewModel cDeliveryOrder = new CDeliveryOrderViewModel
                        {
                            sellerID = sellerID,
                            sellerAcc = sellerAcc,
                            product = product,
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
        public IActionResult SetItemToCheckoutSession(string purchaseItemInfo)
        {
            try
            {
                HttpContext.Session.SetString(CDictionary.SK_PURCHASEITEMINFO, purchaseItemInfo);
                return Content("1");
            }
            catch
            {
                return Content("0");
            }
        }
        public IActionResult Checkout()
        {
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_PURCHASEITEMINFO))
            {
                iSpanProjectContext dbContext = new iSpanProjectContext();
                string buyerString = HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER);
                MemberAccount buyer = JsonSerializer.Deserialize<MemberAccount>(buyerString);
                string purchaseItemInfo = HttpContext.Session.GetString(CDictionary.SK_PURCHASEITEMINFO);
                List<CPurchaseItemToSession> purchaseItems = JsonSerializer.Deserialize<List<CPurchaseItemToSession>>(purchaseItemInfo);
                var productDetails = dbContext.ProductDetails.Select(i => i);
                var members = dbContext.MemberAccounts.Select(i => i);
                var shipperToSellers = dbContext.ShipperToSellers.Select(i => i);
                var paymentToSellers = dbContext.PaymentToSellers.Select(i => i);
                var shippers = dbContext.Shippers.Select(i => i);
                var payments = dbContext.Payments.Select(i => i);
                List<CPurchaseItemInfo> cPurchaseItemList = new List<CPurchaseItemInfo>();
                foreach (var a in purchaseItems)
                {
                    byte[] productDetailPic = productDetails.Where(i => i.ProductDetailId == a.productDetailID).Select(i => i.Pic).FirstOrDefault();
                    CPurchaseItemInfo cPurchaseItemInfo = new CPurchaseItemInfo
                    {
                        orderDetailID = a.orderDetailID,
                        productName = a.productName,
                        productDetailPic = productDetailPic,
                        unitPrice = a.unitPrice,
                        sellerAcc = a.sellerAcc,
                        sellerID = a.sellerID,
                        purchaseCount = a.purchaseCount,
                        productStyle = a.productStyle,
                    };
                    cPurchaseItemList.Add(cPurchaseItemInfo);
                }
                List<int> sellerIDList = new List<int>();
                foreach(var a in cPurchaseItemList)
                {
                    int sellerID = members.Where(i => i.MemberAcc == a.sellerAcc).Select(i => i.MemberId).FirstOrDefault();
                    if (sellerIDList.Contains(sellerID))
                    {
                        continue;
                    }
                    else
                    {
                        sellerIDList.Add(sellerID);
                    }
                }
                List<CDeliverySellerShipperPayment> cDeliverySellerShipperPaymentList = new List<CDeliverySellerShipperPayment>();
                foreach (var a in sellerIDList)
                {
                    MemberAccount seller = members.Where(i => i.MemberId == a).Select(i => i).FirstOrDefault();
                    var shipperIDs = shipperToSellers.Where(i => i.MemberId == a).Select(i => i.ShipperId).ToList();
                    var paymentIDs = paymentToSellers.Where(i => i.MemberId == a).Select(i => i.PaymentId).ToList();
                    List<Shipper> shipperList = new List<Shipper>();
                    foreach (var b in shipperIDs)
                    {
                        var shipper = shippers.Where(i => i.ShipperId == b).Select(i => i).FirstOrDefault();
                        shipperList.Add(shipper);
                    }
                    List<Payment> paymentList = new List<Payment>();
                    foreach (var b in paymentIDs)
                    {
                        var payment = payments.Where(i => i.PaymentId == b).Select(i => i).FirstOrDefault();
                        paymentList.Add(payment);
                    }
                    CDeliverySellerShipperPayment cDeliverySellerShipperPayment = new CDeliverySellerShipperPayment
                    { 
                        seller = seller,
                        shippers = shipperList,
                        payments = paymentList
                    };
                    cDeliverySellerShipperPaymentList.Add(cDeliverySellerShipperPayment);
                }
                CDeliveryCheckoutViewModel cDeliveryCheckout = new CDeliveryCheckoutViewModel
                {
                    buyer = buyer,
                    purchaseItemInfo = cPurchaseItemList,
                    sellerShipperPayments = cDeliverySellerShipperPaymentList
                };
                string jsonString = JsonSerializer.Serialize(cDeliveryCheckout);
                HttpContext.Session.SetString(CDictionary.SK_ALL_INFO_TO_SHOW_CHECKOUT, jsonString);

                return View(cDeliveryCheckout);
            }
            else
            {
                return View();
            }
        }

        public IActionResult GetShipperLocation(string shipperName)
        {
            try
            {
                string[] files = Directory.GetFiles(_enviro.WebRootPath + "/ShipperLocation/" + shipperName, "*.json");
                string store = "[";
                foreach (var i in files)
                {

                string jsonString = System.IO.File.ReadAllText(i).Split('[')[1].Split(']')[0] + ",";
                store += jsonString;
                }
                store = store.Substring(0, store.Length - 1) + "]";

                return Json(store);
            }
            catch
            {
                return Content("0");
            }
        }
        public IActionResult SaveShipperPaymentCoupon(string x)
        {
            CSaveShipperPaymentCoupon newPurchaseInfo = JsonSerializer.Deserialize<CSaveShipperPaymentCoupon>(x);
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_ALL_INFO_TO_SHOW_CHECKOUT))
            {
                string jsonString = HttpContext.Session.GetString(CDictionary.SK_ALL_INFO_TO_SHOW_CHECKOUT);
                CDeliveryCheckoutViewModel cDeliveryCheckout = JsonSerializer.Deserialize<CDeliveryCheckoutViewModel>(jsonString);
                foreach (var a in cDeliveryCheckout.sellerShipperPayments)
                {
                    if (a.seller.MemberId == newPurchaseInfo.sellerID)
                    {
                        a.savedShipperPaymentCoupon = newPurchaseInfo;
                    }
                }
                string newJsonString = JsonSerializer.Serialize(cDeliveryCheckout);
                HttpContext.Session.SetString(CDictionary.SK_ALL_INFO_TO_SHOW_CHECKOUT, newJsonString);
                return Content("1");
            }
            else
            {
                return Content("0");
            }
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
        public IActionResult CheckoutForm(int sellerIDIndex)
        {
            return ViewComponent("DeliveryFillCheckoutForm", sellerIDIndex);
        }
        public IActionResult CheckoutConfirm()
        {
            string jsonString = HttpContext.Session.GetString(CDictionary.SK_ALL_INFO_TO_SHOW_CHECKOUT);
            CDeliveryCheckoutViewModel cDeliveryCheckout = JsonSerializer.Deserialize<CDeliveryCheckoutViewModel>(jsonString);
            return View(cDeliveryCheckout);
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
