using Microsoft.AspNetCore.Mvc;
using prjiSpanFinal.Models;
using System.Collections.Generic;
using System.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace prjiSpanFinal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MsgApiController : ControllerBase
    {
        // GET: api/<MsgApiController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<MsgApiController>/5
        [HttpGet("{id}")]
        public string Get(int cid, int id)
        {
            iSpanProjectContext dbcontext = new iSpanProjectContext();
            var cl = dbcontext.ChatLogs.Where(i => i.SendFrom == cid && i.SendTo == id).ToList();
            return "";/*Json(cl);*/
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
