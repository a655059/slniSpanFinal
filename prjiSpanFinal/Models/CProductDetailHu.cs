namespace prjiSpanFinal.Models
{
    public class CProductDetailHu
    {
       public virtual CProductHu CProductHu { get; set; }
        public int ProductDetailId { get; set; }
        public int ProductId { get; set; }
        public string Style { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public byte[] Pic { get; set; }

    }
}
