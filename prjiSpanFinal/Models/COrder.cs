using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace prjiSpanFinal.Models
{
    public class COrder
    {
        public int OrderIndex { get; set; }

        public int OrderID { get ; set; }
        public string MemberName { get; set; }
        public string Date { get; set; }
        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get { return this.UnitPrice * this.Quantity; } }
        public string OrderStatus { get; set; }

    }
}
