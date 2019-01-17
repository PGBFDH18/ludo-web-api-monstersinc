using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace LudoWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LudoController : ControllerBase
    {
        // GET api/ludo/newgame
        [HttpGet("newgame")]
        public ActionResult<IEnumerable<string>> NewGame()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/ludo/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/ludo
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/ludo/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/ludo/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
