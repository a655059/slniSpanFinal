using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjiSpanFinal.ViewModels.seller
{
    public class CSellerSessionViewModel
    {
        //HttpContext.Session.SetString(CSellerSessionViewModel.CREATE_PRODUCT, json(要塞的資料)) 儲存字串格式到Session
        //HttpContext.Session.GetString(CSellerSessionViewModel.CREATE_PRODUCT) 從Session取得字串格式的資料

        /// <summary>
        /// 新增商品的SessionKey
        /// </summary>
        public static readonly string CREATE_PRODUCT = "CREATE_PRODUCT";

        public static readonly string PRODUCT_ALL_DATA = "PRODUCT_ALL_DATA";

    }
}
