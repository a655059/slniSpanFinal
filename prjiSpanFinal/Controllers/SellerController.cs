using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using prjiSpanFinal.Models;
using prjiSpanFinal.ViewModels.seller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjiSpanFinal.Controllers
{
    public class SellerController : Controller
    {
        private readonly IWebHostEnvironment _enviro;
        private readonly iSpanProjectContext _db;

        public SellerController(IWebHostEnvironment p, iSpanProjectContext db)
        {
            _enviro = p;
            _db = db;
        }

        public IActionResult Order()
        {
            return View();
        }
        public IActionResult Create()
        {
            var bigType = _db.BigTypes.Select(i => i.BigTypeName).ToList();
            var smallType = _db.SmallTypes.Select(i => i.SmallTypeName).ToList();
            CSellerCreateToViewViewModel x = new CSellerCreateToViewViewModel
            {
                bigType = bigType,
                smallType=smallType,
            };
            return View(x);
        }
        //[HttpPost]
        //public IActionResult create(CSellerCreateViewModel product)
        //{
        //    _db.productdetails.add(product);
        //    _db.savechanges();
        //    var q = 
        //}


        //連結小類別選項

        public IActionResult SmallType(string bigtype)
        {
            var smalltype = _db.SmallTypes.Where(a => a.BigType.BigTypeName == bigtype).Select(a => a.SmallTypeName).Distinct();
            return Json(smalltype);
        }




        public IActionResult OrderDetail()
        {
            return View();
        }
        public IActionResult Shipper()
        {
            return View();
        }

        public IActionResult Center()
        {
            return View();
        }

        public IActionResult NewIndex(int id)
        {
            id = 2;//先把ID寫死
            var myproductlist = _db.Products.Where(n => n.MemberId == id).Select(n => n.ProductId).ToList(); //賣家所有商品ID
            var q1 = _db.Products.Where(n => n.MemberId == id).Select(n => n).ToList();//賣家所有商品
            var q2 = _db.ProductDetails.Where(n => myproductlist.Contains(n.ProductId)).Select(n => n).ToList(); //Contains是只把賣家所有商品ID全部挑出來
            List<string> listName = new List<string>();

            List<List<string>> listStyle = new List<List<string>>();
            //  List<一個商品有兩個Style>  =   <一個商品有兩個Style>  <一個商品有兩個Style>  <一個商品有兩個Style>

            List<List<int>> listQty = new List<List<int>>();
            List<List<decimal>> listPrice = new List<List<decimal>>();
            List<List<byte[]>> listPic = new List<List<byte[]>>();
            
            for (int i = 0; i < myproductlist.Count; i++)  //外迴圈把所有商品列出來存進List
            {
                List<string> sublistStyle = new List<string>();
                List<int> sublistQty = new List<int>();
                List<decimal> sublistPrice = new List<decimal>();
                List<byte[]> sublistPic = new List<byte[]>();

                listName.Add(q1[i].ProductName);//把商品名稱存進去
                var detail = q2.Where(p => p.ProductId == q1[i].ProductId).ToList();//找出所有同ID商品的資料

                for(int j = 0;j<detail.Count;j++)//內迴圈把同ID商品所有Style和相關資料列出來存進List
                {
                    sublistStyle.Add(detail[j].Style);
                    sublistQty.Add(detail[j].Quantity);
                    sublistPrice.Add(detail[j].UnitPrice);
                    sublistPic.Add(detail[j].Pic);
                }
                listQty.Add(sublistQty);  //把所有商品的數量存進去
                listPrice.Add(sublistPrice);
                listPic.Add(sublistPic);
                listStyle.Add(sublistStyle);
            }

            CSellerNewIndexToViewViewModel x = new CSellerNewIndexToViewViewModel
            {
                productName = listName,
                Style = listStyle,
                Quantity = listQty,
                UnitPrice = listPrice,
                Pic = listPic
            };

            return View(x);
        }


        public IActionResult Coupon()
        {
            return View();
        }

        public IActionResult AD()
        {
            return View();
        }

        public IActionResult seller跑條(int page)
        {
            return ViewComponent("seller跑條",page);
        }

        public IActionResult Event()
        {
            return View();
        }
    }
}
