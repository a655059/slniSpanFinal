using System;
using System.Collections.Generic;

#nullable disable

namespace prjiSpanFinal.Models
{
    public partial class Payment
    {
        public Payment()
        {
            Orders = new HashSet<Order>();
            PaymentToProducts = new HashSet<PaymentToProduct>();
            PaymentToSellers = new HashSet<PaymentToSeller>();
        }

        public int PaymentId { get; set; }
        public string PaymentName { get; set; }
        public int Fee { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<PaymentToProduct> PaymentToProducts { get; set; }
        public virtual ICollection<PaymentToSeller> PaymentToSellers { get; set; }
    }
}
