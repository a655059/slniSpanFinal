using prjiSpanFinal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjiSpanFinal.ViewModels.Member
{
    public class MylikeViewModel
    {
        public Product _product;
        public ProductDetail _productDetail;
        public Product product
        {
            get { return _product; }
            set { _product = value; }
        }
        public ProductDetail productDetail
        {
            get { return _productDetail; }
            set { _productDetail = value; }
        }
        public MylikeViewModel()
        {
            _product = new Product();
            _productDetail = new ProductDetail();
        }
        public int MylikeID { get; set; }
        public int memberID { get; set; }
        public int ProductID { get; set; }
        public int ProductDetailID { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public List<decimal> Unitprice { get; set; }
        public List<string> Style { get; set; }
        public byte[] Pic { get; set; }

    }
}
