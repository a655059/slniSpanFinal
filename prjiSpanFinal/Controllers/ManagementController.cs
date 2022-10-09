using Microsoft.AspNetCore.Mvc;
using prjiSpanFinal.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace prjiSpanFinal.Controllers
{
    public class ManagementController : Controller
    {
        static List<CMemberHu> members = new();
        static List<CProductHu> Products = new();
        static List<COrderHu> Orders = new();
        public ManagementController()
        {
            if (members.Count <= 0 || Orders.Count <= 0 || Products.Count <= 0)
            {
                ADDDATAS();
            }

        }
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
            }
        }
        public IActionResult Home()
        {
            return View();
        }
        public IActionResult Test()
        {
            return PartialView();
        }
        public IActionResult PowerBi()
        {
            return View();
        }
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
                        a.OrderStatus = "已結帳";
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

    }
}
