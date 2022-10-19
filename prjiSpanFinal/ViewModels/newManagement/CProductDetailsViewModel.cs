using prjiSpanFinal.Models;
using System.Linq;

namespace prjiSpanFinal.ViewModels
{
    public class CProductDetailsViewModel
    {
        public Product Product { get; set; }
        public IQueryable<ProductDetail> ProductDetail { get; set; }
    }
}
