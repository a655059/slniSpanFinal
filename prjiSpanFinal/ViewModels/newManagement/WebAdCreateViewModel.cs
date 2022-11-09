using Microsoft.AspNetCore.Http;
using prjiSpanFinal.Models;

namespace prjiSpanFinal.ViewModels.newManagement
{
    public class WebAdCreateViewModel
    {
        public int WebAdid { get; set; }
        public IFormFile WebAdimage { get; set; }
        public int MemberId { get; set; }
        public int WebAdimageTypeId { get; set; }
        public bool IsPublishing { get; set; }
        public string Path { get; set; }
    }
}
