using Microsoft.AspNetCore.Mvc;
using prjiSpanFinal.ViewModels.Delivery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjiSpanFinal.ViewComponents
{
    public class DeliveryFillCheckoutFormViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(CDeliveryCheckoutViewModel cDeliveryCheckout, int sellerID)
        {
            List<CDeliverySellerShipperPayment> cDeliverySellerShipperPaymentList = new List<CDeliverySellerShipperPayment>();
            string sellerAcc = ""; 
            foreach (var i in cDeliveryCheckout.sellerShipperPayments)
            {
                if (i.seller.MemberId == sellerID)
                {
                    sellerAcc = i.seller.MemberAcc;
                    cDeliverySellerShipperPaymentList.Add(i);
                }   
            }
            List<CPurchaseItemInfo> cPurchaseItemInfoList = new List<CPurchaseItemInfo>();
            foreach (var i in cDeliveryCheckout.purchaseItemInfo)
            {
                if (i.sellerAcc == sellerAcc)
                {
                    cPurchaseItemInfoList.Add(i);
                }
            }
            CDeliveryCheckoutViewModel x = new CDeliveryCheckoutViewModel();
            x.purchaseItemInfo = cPurchaseItemInfoList;
            x.sellerShipperPayments = cDeliverySellerShipperPaymentList;
            return View(x);
        }
    }
}
