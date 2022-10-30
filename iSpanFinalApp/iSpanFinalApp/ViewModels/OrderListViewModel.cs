using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace iSpanFinalApp.ViewModels
{
    public class OrderListViewModel
    {
        public int OrderId { get; set; }
        public DateTime OrderDatetime { get; set; }
        public string OrderDatetimeCool 
        { 
            get 
            {
                return OrderDatetime.ToString("yyyy-MM-dd HH:mm:ss");
            } 
        }
        public string OrderStatusName { get; set; }
        public int Quantity { get; set; }
        public decimal Unitprice { get; set; }
        public string Style { get; set; }
        public byte[] Pic { get; set; }
        public ImageSource SourcePic
        {
            get
            {
                return ImageSource.FromStream(() => new MemoryStream(Pic));
            }
        }
        public string ProductName { get; set; }

    }
}
