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
    public class MsgApiController : Controller
    {
        // GET: api/<MsgApiController>
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        // GET api/<MsgApiController>/5
        [HttpGet]
        public IActionResult Get(string scid, string sid)
        {
            var cid = Convert.ToInt32(scid);
            var id = Convert.ToInt32(sid);
            iSpanProjectContext dbcontext = new iSpanProjectContext();
            var cl = dbcontext.ChatLogs.Where(i => (i.SendFrom == cid && i.SendTo == id) || (i.SendFrom == id && i.SendTo == cid)).ToList();
            return Json(cl);
        }

        // POST api/<MsgApiController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<MsgApiController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<MsgApiController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
