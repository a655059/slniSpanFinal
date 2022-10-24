using System;
using System.Collections.Generic;

#nullable disable

namespace prjiSpanFinal.Models
{
    public partial class Order
    {
        public Order()
        {
            Arguments = new HashSet<Argument>();
            CommentForCustomers = new HashSet<CommentForCustomer>();
            OrderDetails = new HashSet<OrderDetail>();
        }

        public int OrderId { get; set; }
        public int MemberId { get; set; }
        public DateTime OrderDatetime { get; set; }
        public string RecieveAdr { get; set; }
        public DateTime FinishDate { get; set; }
        public int CouponId { get; set; }
        public int StatusId { get; set; }
        public int ShipperId { get; set; }
        public DateTime PaymentDate { get; set; }
        public DateTime ShippingDate { get; set; }
        public DateTime ReceiveDate { get; set; }
        public int PaymentId { get; set; }
        public string OrderMessage { get; set; }

        public virtual Coupon Coupon { get; set; }
        public virtual MemberAccount Member { get; set; }
        public virtual Payment Payment { get; set; }
        public virtual Shipper Shipper { get; set; }
        public virtual OrderStatus Status { get; set; }
        public virtual ICollection<Argument> Arguments { get; set; }
        public virtual ICollection<CommentForCustomer> CommentForCustomers { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
