﻿using prjiSpanFinal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjiSpanFinal.ViewModels.Delivery
{
    public class COrderedOrderViewModel
    {
        public prjiSpanFinal.Models.Order order { get; set; }
        public MemberAccount seller { get; set; }
        public OrderDetail orderDetail { get; set; }
        public ProductDetail productDetail { get; set; }
        public string productName { get; set; }

    }
}