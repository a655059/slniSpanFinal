using prjiSpanFinal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjiSpanFinal.ViewModels
{
    public class CProductPicViewModel
    {
        private ProductPic _productPic;
        public ProductPic productPic
        {
            get { return _productPic; }
            set { _productPic = value; }
        }
        public CProductPicViewModel()
        {
            _productPic = new ProductPic();
        }
        public int ProductId 
        {
            get { return _productPic.ProductId; }
            set { _productPic.ProductId = value; }
        }
        public int PicId
        {
            get { return _productPic.PicId; }
            set { _productPic.PicId = value; }
        }
        public byte[] Picture
        {
            get { return _productPic.Picture; }
            set { _productPic.Picture = value; }
        }
    }
}
