using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using prjiSpanFinal.ViewModels;
using prjiSpanFinal.ViewModels.Delivery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace prjiSpanFinal.ViewComponents
{
    public class DeliveryFillCheckoutFormViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(int sellerIDIndex)
        {
            string jsonString = HttpContext.Session.GetString(CDictionary.SK_ALL_INFO_TO_SHOW_CHECKOUT);
            CDeliveryCheckoutViewModel cDeliveryCheckout = JsonSerializer.Deserialize<CDeliveryCheckoutViewModel>(jsonString);
            List<CDeliverySellerShipperPayment> cDeliverySellerShipperPaymentList = new List<CDeliverySellerShipperPayment>();
            string sellerAcc = cDeliveryCheckout.sellerShipperPayments[sellerIDIndex].seller.MemberAcc;
            cDeliverySellerShipperPaymentList.Add(cDeliveryCheckout.sellerShipperPayments[sellerIDIndex]);
            List<CPurchaseItemInfo> cPurchaseItemInfoList = new List<CPurchaseItemInfo>();
            foreach (var i in cDeliveryCheckout.purchaseItemInfo)
            {
                if (i.sellerAcc == sellerAcc)
                {
                    cPurchaseItemInfoList.Add(i);
                }
            }
            CDeliveryCheckoutViewModel x = new CDeliveryCheckoutViewModel();
            x.buyer = cDeliveryCheckout.buyer;
            x.purchaseItemInfo = cPurchaseItemInfoList;
            x.sellerShipperPayments = cDeliverySellerShipperPaymentList;
            return View(x);
        }
    }
}
