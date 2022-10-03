using prjiSpanFinal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjiSpanFinal.ViewModels
{
    public class CProductDetailViewModel
    {
        private ProductDetail _productDetail;
        public ProductDetail productDetail
        {
            get { return _productDetail; }
            set { _productDetail = value; }
        }
        public CProductDetailViewModel()
        {
            _productDetail = new ProductDetail();
        }
        public int ProductDetailId 
        {
            get { return _productDetail.ProductDetailId; }
            set { _productDetail.ProductDetailId = value; }
        }
        public int ProductId
        {
            get { return _productDetail.ProductId; }
            set { _productDetail.ProductId = value; }
        }
        public string Style
        {
            get { return _productDetail.Style; }
            set { _productDetail.Style = value; }
        }
        public int Quantity
        {
            get { return _productDetail.Quantity; }
            set { _productDetail.Quantity = value; }
        }
        public decimal UnitPrice
        {
            get { return _productDetail.UnitPrice; }
            set { _productDetail.UnitPrice = value; }
        }
        public byte[] Pic
        {
            get { return _productDetail.Pic; }
            set { _productDetail.Pic = value; }
        }
    }
}
