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
                        RecieveAdr = "", 
                        CouponId = 1,
                        StatusId = 1,
                        ShipperId = 1,
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
                string orderDetailCount = dbContext.OrderDetails.Where(i => i.Order.MemberId == memberID && i.Order.StatusId == 1).Select(i => i).Count().ToString();
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

        public IActionResult AddComment(int id)
        {
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER))
            {
                iSpanProjectContext dbContext = new iSpanProjectContext();
                string jsonString = HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER);
                MemberAccount member = JsonSerializer.Deserialize<MemberAccount>(jsonString);
                int memberID = member.MemberId;
                var orderDetails = dbContext.OrderDetails.Where(i => i.OrderId == id).Select(i=>new { 
                    orderDetailID = i.OrderDetailId,
                    productDetailPic = i.ProductDetail.Pic,
                    style = i.ProductDetail.Style,
                    productName = i.ProductDetail.Product.ProductName
                }).ToList();
                var comments = dbContext.Comments.Select(i => i).ToList();
                List<CAddCommentViewModel> cAddCommentList = new List<CAddCommentViewModel>();
                foreach (var a in orderDetails)
                {
                    var comment = comments.Where(i => i.OrderDetailId == a.orderDetailID).Select(i => i).FirstOrDefault();
                    if (comment == null)
                    {
                        CAddCommentViewModel cAddComment = new CAddCommentViewModel
                        {
                            orderDetailID = a.orderDetailID,
                            productDetailPic = a.productDetailPic,
                            style = a.style,
                            productName = a.productName
                        };
                        cAddCommentList.Add(cAddComment);
                    }
                }
                if (cAddCommentList.Count > 0)
                {
                    CAddCommentViewModel addCommentViewModel = new CAddCommentViewModel();
                    addCommentViewModel.cAddComments = cAddCommentList;
                    return View(addCommentViewModel);
                }
                else
                {
                    return RedirectToAction("Order", "Member");
                }
            }
            else
            {
                return RedirectToAction("Login", "Member");
            }
        }
        
        public IActionResult SubmitComment(CSubmitCommentViewModel cSubmitComment, List<IFormFile> photos)
        {
            iSpanProjectContext dbContext = new iSpanProjectContext();
            string memberString = HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER);
            MemberAccount member = JsonSerializer.Deserialize<MemberAccount>(memberString);
            int memberID = member.MemberId;
            int orderDetailID = cSubmitComment.orderDetailID;
            var comments = dbContext.Comments.Where(i => i.OrderDetailId == orderDetailID).Select(i => i).ToList();
            if (comments.Count > 0)
            {
                return Content("0");
            }
            else
            {
                string commentString = $"品質: {cSubmitComment.quality}, 色差: {cSubmitComment.colorDifference}, 和圖片相符: {cSubmitComment.picMatch}, 更多評論: {cSubmitComment.other}";
                Comment comment = new Comment
                {
                    OrderDetailId = cSubmitComment.orderDetailID,
                    Comment1 = commentString,
                    CommentStar = (byte)cSubmitComment.commentStar,
                    CommentTime = DateTime.Now,
                    ShipperStar = cSubmitComment.shipperStar
                };
                dbContext.Comments.Add(comment);
                dbContext.SaveChanges();
                int commentID = dbContext.Comments.Where(i => i.OrderDetailId == orderDetailID && i.OrderDetail.Order.MemberId == memberID).Select(i => i.CommentId).FirstOrDefault();
                if (photos.Count > 0)
                {
                    foreach(var i in photos)
                    {
                        if (i.Length > 0)
                        {
                            byte[] fileBytes;
                            using (MemoryStream ms = new MemoryStream())
                            {
                                i.CopyTo(ms);
                                fileBytes = ms.GetBuffer();
                            }
                            CommentPic commentPic = new CommentPic
                            {
                                CommentId = commentID,
                                CommentPic1 = fileBytes,
                            };
                            dbContext.CommentPics.Add(commentPic);
                        }
                    }
                    dbContext.SaveChanges();
                }

                int orderID = dbContext.OrderDetails.Where(i => i.OrderDetailId == orderDetailID).Select(i => i.OrderId).FirstOrDefault();
                List<int> orderDetailIDList = dbContext.OrderDetails.Where(i => i.OrderId == orderID).Select(i => i.OrderDetailId).ToList();
                List<Comment> commentList = new List<Comment>();
                var newComments = dbContext.Comments.Select(i => i).ToList();
                foreach (var a in orderDetailIDList)
                {
                    var comment1 = newComments.Where(i => i.OrderDetailId == a).Select(i => i).FirstOrDefault();
                    if (comment1 != null)
                    {
                        commentList.Add(comment1);
                    }
                }
                var commentForCustomer = dbContext.CommentForCustomers.Where(i => i.OrderId == orderID).Select(i => i).FirstOrDefault();
                if (commentList.Count >= orderDetailIDList.Count && commentForCustomer != null)
                {
                    var q = dbContext.Orders.Where(i => i.OrderId == orderID).Select(i => i).FirstOrDefault();
                    q.StatusId = 7;
                    dbContext.SaveChanges();
                }
                return Content("1");
            }
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
            iSpanProjectContext dbContext = new iSpanProjectContext();
            string jsonString = HttpContext.Session.GetString(CDictionary.SK_ALL_INFO_TO_SHOW_CHECKOUT);
            CDeliveryCheckoutViewModel cDeliveryCheckout = JsonSerializer.Deserialize<CDeliveryCheckoutViewModel>(jsonString);
            int buyerID = cDeliveryCheckout.buyer.MemberId;
            var allOrderOrderDetail = dbContext.OrderDetails.Select(i => new {
                orders = i.Order,
                orderDetails = i,
                sellerID = i.ProductDetail.Product.MemberId,
            }).ToList();
            List<int> orderIDs = new List<int>();
            foreach (var a in cDeliveryCheckout.purchaseItemInfo)
            {
                var orderID = allOrderOrderDetail.Where(i => i.orderDetails.OrderDetailId == a.orderDetailID).Select(i => i.orderDetails.OrderId).FirstOrDefault();
                if (orderIDs.Contains(orderID))
                {
                    continue;
                }
                else
                {
                    orderIDs.Add(orderID);
                }
            }
            foreach (var a in orderIDs)
            {
                var orderDetailIDsInOrder = allOrderOrderDetail.Where(i => i.orders.OrderId == a).Select(i => i.orderDetails.OrderDetailId).ToList();
                int orderDetailCountInOrder = orderDetailIDsInOrder.Count;
                var purchaseOrderDetailID = cDeliveryCheckout.purchaseItemInfo.Where(i => orderDetailIDsInOrder.Contains(i.orderDetailID)).Select(i => i.orderDetailID).ToList();
                int purchaseOrderDetailCount = purchaseOrderDetailID.Count;
                int sellerID = allOrderOrderDetail.Where(i => i.orderDetails.OrderDetailId == purchaseOrderDetailID[0]).Select(i => i.sellerID).FirstOrDefault();
                string address = cDeliveryCheckout.sellerShipperPayments.Where(i => i.seller.MemberId == sellerID).Select(i => i.savedShipperPaymentCoupon.address).FirstOrDefault();
                int shipperID = cDeliveryCheckout.sellerShipperPayments.Where(i => i.seller.MemberId == sellerID).Select(i => i.savedShipperPaymentCoupon.shipperID).FirstOrDefault();
                int paymentID = cDeliveryCheckout.sellerShipperPayments.Where(i => i.seller.MemberId == sellerID).Select(i => i.savedShipperPaymentCoupon.paymentID).FirstOrDefault();
                string orderMessage = cDeliveryCheckout.sellerShipperPayments.Where(i => i.seller.MemberId == sellerID).Select(i => i.savedShipperPaymentCoupon.wordToSeller).FirstOrDefault();
                if (purchaseOrderDetailCount < orderDetailCountInOrder)
                {
                    var oldOrder = dbContext.Orders.Where(i => i.OrderId == a).Select(i => i).FirstOrDefault();
                    Order newOrder = new Order
                    {
                        MemberId = oldOrder.MemberId,
                        RecieveAdr = address,
                        CouponId = 1,
                        StatusId = 2,
                        ShipperId = shipperID,
                        PaymentId = paymentID,
                        OrderMessage = orderMessage
                    };
                    dbContext.Orders.Add(newOrder);
                    dbContext.SaveChanges();
                    int newOrderID = dbContext.Orders.Where(i => i.MemberId == oldOrder.MemberId && i.StatusId == 2).OrderByDescending(i => i.OrderId).Select(i => i.OrderId).FirstOrDefault();
                    foreach (var b in purchaseOrderDetailID)
                    {
                        int productDetailID = allOrderOrderDetail.Where(i => i.orderDetails.OrderDetailId == b).Select(i => i.orderDetails.ProductDetailId).FirstOrDefault();
                        int quantity = cDeliveryCheckout.purchaseItemInfo.Where(i => i.orderDetailID == b).Select(i => i.purchaseCount).FirstOrDefault();
                        decimal unitPrice = Convert.ToDecimal(cDeliveryCheckout.purchaseItemInfo.Where(i => i.orderDetailID == b).Select(i => i.unitPrice).FirstOrDefault());
                        OrderDetail newOrderDetail = new OrderDetail
                        {
                            OrderId = newOrderID,
                            ProductDetailId = productDetailID,
                            Quantity = quantity,
                            ShippingStatusId = 1,
                            Unitprice = unitPrice
                        };
                        dbContext.OrderDetails.Add(newOrderDetail);
                        var oldOrderDetail =  dbContext.OrderDetails.Where(i => i.OrderDetailId == b).Select(i => i).FirstOrDefault();
                        dbContext.OrderDetails.Remove(oldOrderDetail);
                    }
                    dbContext.SaveChanges();
                }
                else
                {
                    var order = dbContext.Orders.Where(i => i.OrderId == a).Select(i => i).FirstOrDefault();
                    order.RecieveAdr = address;
                    order.CouponId = 1;
                    order.StatusId = 2;
                    order.ShipperId = shipperID;
                    order.PaymentId = paymentID;
                    order.OrderMessage = orderMessage;
                    foreach (var b in purchaseOrderDetailID)
                    {
                        int productDetailID = allOrderOrderDetail.Where(i => i.orderDetails.OrderDetailId == b).Select(i => i.orderDetails.ProductDetailId).FirstOrDefault();
                        int quantity = cDeliveryCheckout.purchaseItemInfo.Where(i => i.orderDetailID == b).Select(i => i.purchaseCount).FirstOrDefault();
                        decimal unitPrice = Convert.ToDecimal(cDeliveryCheckout.purchaseItemInfo.Where(i => i.orderDetailID == b).Select(i => i.unitPrice).FirstOrDefault());
                        var orderDetail = dbContext.OrderDetails.Where(i => i.OrderDetailId == b).Select(i => i).FirstOrDefault();
                        orderDetail.Quantity = quantity;
                        orderDetail.Unitprice = unitPrice;
                    }
                    dbContext.SaveChanges();
                }
            }
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
