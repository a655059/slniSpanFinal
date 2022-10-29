using System;
using System.Collections.Generic;

#nullable disable

namespace prjiSpanFinal.Models
{
    public partial class WebAdimageType
    {
        public WebAdimageType()
        {
            WebAds = new HashSet<WebAd>();
        }

        public int WebAdimageTypeId { get; set; }
        public string ImageType { get; set; }

        public virtual ICollection<WebAd> WebAds { get; set; }
    }
}
