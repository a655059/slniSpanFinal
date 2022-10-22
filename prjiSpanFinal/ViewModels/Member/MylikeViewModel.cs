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
        public Product product
        {
            get { return _product; }
            set { _product = value; }
        }
        public MylikeViewModel()
        {
            _product = new Product();
        }
        public int Mylike { get; set; }
        public int memberID { get; set; }
        public int ProductID { get; set; }
        public int ProductDetail { get; set; }
        public string ProductName { get; set; }

    }
}
