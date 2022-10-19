using Microsoft.AspNetCore.Mvc;
using prjiSpanFinal.Models;
using System;
using System.Collections.Generic;
using System.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace prjiSpanFinal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MsgAccAutoApiController : Controller
    {
        [HttpGet]
        public IActionResult Get(string keyword)
        {
            iSpanProjectContext dbcontext = new iSpanProjectContext();
            var memacc = dbcontext.MemberAccounts.Where(p => p.MemberAcc.Contains(keyword)).Select(p => p.MemberAcc);
            return Json(memacc);
        }
    }
}
