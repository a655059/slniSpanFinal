using Microsoft.AspNetCore.Mvc;
using prjiSpanFinal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjiSpanFinal.Controllers
{
    public class ManagementController : Controller
    {
        static List<CMember> members = new();
        static List<CProductHu> Products = new();
        static List<COrder> Orders = new();
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
                CMember newm = new()
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
                COrder cOrder = new()
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
        public IActionResult MemberList()
        {
            return View(members);
        }
        public IActionResult MemberCreate()
        {
            return View();
        }
        [HttpPost]
        public IActionResult MemberCreate(CMember mem)
        {
            var EditMember = new CMember()
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
        public IActionResult MemberEdit(CMember mem)
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
        public IActionResult OrderEdit(COrder order)
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
                        a.OrderStatus = "正常";
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
