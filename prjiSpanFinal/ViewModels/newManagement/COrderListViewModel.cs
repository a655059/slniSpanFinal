using prjiSpanFinal.Models;

namespace prjiSpanFinal.ViewModels.newManagement
{
    public class COrderListViewModel
    {
        public Order Order { get; set; }
        public string OrderStatusName { get; set; }
        public string CouponName { get; set; }
        public string ShipperName { get; set; }
        public string PaymentName { get; set; }
    }
}