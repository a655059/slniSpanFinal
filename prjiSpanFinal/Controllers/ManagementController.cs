using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using prjiSpanFinal.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace prjiSpanFinal.Controllers
{
    public class ManagementController : Controller
    {
        public static List<CMemberHu> members = new();
        public static List<CProductHu> Products = new();
        static List<COrderHu> Orders = new();
        static List<CProductDetailHu> ProductDetails = new();
        static List<CReportHu> Reports = new();
        public ManagementController()
        {
            if (members.Count <= 0 || Orders.Count <= 0 || Products.Count <= 0)
            {
                ADDDATAS();
            }

        }
        #region DATASRegion
        public void ADDDATAS()
        {
            for (int i = 0; i <= 20; i++)
            {
                CMemberHu newm = new()
                {
                    Id = i,
                    Address = "台北市",
                    Email = "ShopDaoBao@sdbmail.com",
                    MemName = "蘋果" + i,
                    Password = "Apple",
                    Phone = "7414",
                    MemberStatus = "註冊會員",
                };
                members.Add(newm);
                Random rnd = new();
                COrderHu cOrder = new()
                {
                    OrderID = i,
                    Quantity = rnd.Next(1, 100),
                    MemberName = "蘋果" + i,
                    ProductName = "蘋果",
                    Date = DateTime.Now.ToString("yyyy/mm/dd"),
                    UnitPrice = 10,
                    OrderStatus = "未結帳",
                };
                Orders.Add(cOrder);
                CProductHu cProduct = new()
                {
                    ProductName = "蘋果",
                    SmallType = "食品",
                    ProductId = i,
                    ProductStatus = "可以購買",
                    SellerName = "蘋果" + i,
                    Stock = rnd.Next(1, 999),
                    UnitPrice = 10,
                };
                Products.Add(cProduct);
                foreach (var prods in Products)
                {
                    CProductDetailHu cProductDetail = new()
                    {
                        CProductHu = prods,
                        ProductDetailId = i,
                        ProductId = prods.ProductId,
                        Quantity = rnd.Next(1, 999),
                        Style = "樣式" + i,
                        UnitPrice = 10,
                    };
                    ProductDetails.Add(cProductDetail);
                }
            }

            CReportHu cReport = new()
            {
                CMemberHu = members.FirstOrDefault(m => m.Id == 1),
                CProductHu = Products.FirstOrDefault(i => i.ProductId == 3),
                Reason = "品名寫香蕉圖片卻是蘋果，點進去發現是耳機還有品名根本不是食物，這一切都一團亂",
                ReporterID = 1,
                ProductName = "好大一根香蕉",
                ReportId = 1,
                ReportType = "廣告不實",
                ReportStatus = "未處理",
            };
            Reports.Add(cReport);
        }
        #endregion
        #region ReportRegion
        public IActionResult ReportList()
        {
            return View(Reports);
        }
        public IActionResult ReportDetail(int? id)
        {
            var Q = Reports.FirstOrDefault(i => i.ReportId == id);
            return View(Q);
        }
        public IActionResult ReportApprove(int? id)
        {
            var Q = Reports.FirstOrDefault(i => i.ReportId == id);
            Q.ReportStatus = "已結案";
            return RedirectToAction("ReportList");
        }
        public IActionResult ReportDelete(int? id)
        {
            var Q = Reports.FirstOrDefault(i => i.ReportId == id);
            Q.ReportStatus = "不成立";
            return RedirectToAction("ReportList");
        }
        public IActionResult ReportUndo(int? id)
        {
            var Q = Reports.FirstOrDefault(i => i.ReportId == id);
            Q.ReportStatus = "未處理";
            return RedirectToAction("ReportList");
        }
        public IActionResult ReportProcess(int? id)
        {
            var Q = Reports.FirstOrDefault(i => i.ReportId == id);
            Q.ReportStatus = "審核中";
            return RedirectToAction("ReportList");
        }
        #endregion
        #region PowerBiRegion
        public IActionResult PowerBi()
        {
            return View();
        }
        #endregion
        #region ProductRegion
        public IActionResult ProductList()
        {
            return View(Products);
        }
        public IActionResult ProductEdit(int? id)
        {
            var product = from i in Products
                          where i.ProductId == id
                          select i;
            return View(product.First());
        }
        [HttpPost]
        public IActionResult ProductEdit(CProductHu prod)
        {
            if (prod != null)
            {
                var EditProd = from i in Products where i.ProductId == prod.ProductId select i;
                EditProd.First().ProductName = prod.ProductName;
                EditProd.First().ProductStatus = prod.ProductStatus;
                EditProd.First().Stock = prod.Stock;
                EditProd.First().UnitPrice = prod.UnitPrice;
                EditProd.First().SmallType = prod.SmallType;
                EditProd.First().SellerName = prod.SellerName;

                return RedirectToAction("ProductList");
            }
            return RedirectToAction("ProductList");
        }
        public IActionResult ProductDelete(int? id)
        {
            if (id != null)
            {
                foreach (var a in Products)
                {
                    if (a.ProductId == id)
                    {
                        a.ProductStatus = "已刪除";
                        break;
                    }
                }
            }
            return RedirectToAction("ProductList");
        }
        public IActionResult ProductRecover(int? id)
        {
            if (id != null)
            {
                foreach (var a in Products)
                {
                    if (a.ProductId == id && a.ProductStatus == "已刪除")
                    {
                        a.ProductStatus = "可以購買";
                        break;
                    }
                    else if (a.ProductId == id && a.ProductStatus != "已刪除")
                    {
                        break;
                    }
                }
            }
            return RedirectToAction("ProductList");
        }
        #endregion
        #region ProductDetailRegion
        public IActionResult ProductDetailList(int? id)
        {
            var product = from i in ProductDetails
                          where i.ProductId == id
                          select i;

            return View(product);
        }
        public IActionResult ProductDetailEdit(int? id)
        {
            var productdetai = from i in ProductDetails
                               where i.ProductDetailId == id
                               select i;
            return View(productdetai.First());
        }
        [HttpPost]
        public IActionResult ProductDetailEdit(CProductDetailHu prod, IFormFile img)
        {

            if (prod != null)
            {
                var EditProd = from i in ProductDetails where i.ProductDetailId == prod.ProductDetailId select i;
                var Proddet = EditProd.First();
                Proddet.Style = prod.Style;
                Proddet.Quantity = prod.Quantity;
                Proddet.UnitPrice = prod.UnitPrice;

                if (img != null)
                {
                    using var ms = new MemoryStream();
                    img.CopyTo(ms);
                    Proddet.Pic = ms.ToArray();
                }
                return RedirectToAction("ProductList");
            }
            return RedirectToAction("ProductList");
        }
        #endregion
        #region MemberRegion
        public IActionResult MemberList()
        {
            return View(members);
        }
        public IActionResult MemberCreate()
        {
            return View();
        }
        [HttpPost]
        public IActionResult MemberCreate(CMemberHu mem)
        {
            var EditMember = new CMemberHu()
            {
                Id = members.Last().Id + 1,
                MemName = mem.MemName,
                Phone = mem.Phone,
                Address = mem.Address,
                Email = mem.Email,
                Password = mem.Password,
                MemberStatus = mem.MemberStatus,
            };
            members.Add(EditMember);
            return RedirectToAction("MemberList");
        }

        public IActionResult MemberEdit(int? id)
        {
            var member = from i in members
                         where i.Id == id
                         select i;
            return View(members.First());
        }
        [HttpPost]
        public IActionResult MemberEdit(CMemberHu mem)
        {
            if (mem != null)
            {
                var EditMember = from i in members where i.Id == mem.Id select i;
                EditMember.First().MemName = mem.MemName;
                EditMember.First().Phone = mem.Phone;
                EditMember.First().Address = mem.Address;
                EditMember.First().Email = mem.Email;
                EditMember.First().Password = mem.Password;
                EditMember.First().MemberStatus = mem.MemberStatus;

                return RedirectToAction("MemberList");
            }
            return RedirectToAction("MemberList");
        }
        public IActionResult MemberDelete(int? id)
        {
            if (id != null)
            {
                foreach (var a in members)
                {
                    if (a.Id == id)
                    {
                        a.MemberStatus = "已刪除";
                        break;
                    }
                }
            }
            return RedirectToAction("MemberList");
        }
        public IActionResult MemberRecover(int? id)
        {
            if (id != null)
            {
                foreach (var a in members)
                {
                    if (a.Id == id && a.MemberStatus == "已刪除")
                    {
                        a.MemberStatus = "註冊會員";
                        break;
                    }
                    else if (a.Id == id && a.MemberStatus != "已刪除")
                    {
                        break;
                    }
                }
            }
            return RedirectToAction("MemberList");
        }
        #endregion
        #region OrderRegion
        public IActionResult OrderList()
        {
            return View(Orders);
        }
        public IActionResult OrderEdit(int? id)
        {
            var order = from i in Orders
                        where i.OrderID == id
                        select i;
            return View(order.First());
        }

        [HttpPost]
        public IActionResult OrderEdit(COrderHu order)
        {
            if (order != null)
            {
                Orders[order.OrderID].OrderStatus = order.OrderStatus;
                return RedirectToAction("OrderList");
            }
            return RedirectToAction("OrderList");
        }
        public IActionResult OrderDelete(int? id)
        {
            if (id != null)
            {
                foreach (var a in Orders)
                {
                    if (a.OrderID == id && a.OrderStatus != "已刪除")
                    {
                        a.OrderStatus = "已刪除";
                        break;
                    }
                    else if (a.OrderID == id && a.OrderStatus == "已刪除")
                    {
                        break;
                    }
                }
            }
            return RedirectToAction("OrderList");
        }
        public IActionResult OrderRecover(int? id)
        {
            if (id != null)
            {
                foreach (var a in Orders)
                {
                    if (a.OrderID == id && a.OrderStatus == "已刪除")
                    {
                        a.OrderStatus = "未結帳";
                        break;
                    }
                    else if (a.OrderID == id && a.OrderStatus != "已刪除")
                    {
                        break;
                    }
                }
            }
            return RedirectToAction("OrderList");
        }
        #endregion

    }
}
