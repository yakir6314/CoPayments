using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace CoPayApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]


    public class ValuesController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }
    }
}
