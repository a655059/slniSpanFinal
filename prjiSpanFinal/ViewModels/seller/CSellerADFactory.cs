using prjiSpanFinal.Models;
using prjiSpanFinal.ViewModels.Home;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjiSpanFinal.ViewModels.seller
{
    public class CSellerADFactory
    {

        public List<CShowItem> fgetShowITem(List<Product> list, int nowpages)
        {
            List<CShowItem> res = (new CHomeFactory()).toShowItem(list);
            return res.Skip((nowpages - 1) * 15).Take(15).ToList();
        }
    }
}
