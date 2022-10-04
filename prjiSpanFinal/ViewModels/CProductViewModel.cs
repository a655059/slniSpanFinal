using prjiSpanFinal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjiSpanFinal.ViewModels
{
    public class CProductViewModel
    {
        private Product _product;
        public Product product
        {
            get { return _product; }
            set { _product = value; }
        }
        public CProductViewModel()
        {
            _product = new Product();
        }
        public int ProductId 
        {
            get { return _product.ProductId; }
            set { _product.ProductId = value; }
        }
        public string ProductName
        {
            get { return _product.ProductName; }
            set { _product.ProductName = value; }
        }
        public int SmallTypeId
        {
            get { return _product.SmallTypeId; }
            set { _product.SmallTypeId = value; }
        }
        public int MemberId
        {
            get { return _product.MemberId; }
            set { _product.MemberId = value; }
        }
        public int RegionId
        {
            get { return _product.RegionId; }
            set { _product.RegionId = value; }
        }
        public decimal AdFee
        {
            get { return _product.AdFee; }
            set { _product.AdFee = value; }
        }
        public string Description
        {
            get { return _product.Description; }
            set { _product.Description = value; }
        }
        public int ProductStatusId
        {
            get { return _product.ProductStatusId; }
            set { _product.ProductStatusId = value; }
        }
    }
}
