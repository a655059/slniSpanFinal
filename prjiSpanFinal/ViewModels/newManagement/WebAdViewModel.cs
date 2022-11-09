using Microsoft.AspNetCore.Http;
using prjiSpanFinal.Models;

namespace prjiSpanFinal.ViewModels.newManagement
{
    public class WebAdViewModel
    {
        public WebAd WebAd { get; set; }
        public string ImageType { get; set; }
        public IFormFile WebADImage { get; set; }
    }
}
