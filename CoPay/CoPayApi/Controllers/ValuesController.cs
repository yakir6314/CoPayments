using CoPayApi.Data;
using CoPayApi.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace CoPayApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]


    public class ValuesController : ControllerBase
    {
        private readonly CoDbContext _dbContext;


        public ValuesController(
            CoDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        // GET api/values
        [HttpGet]
        
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("Get")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }
        [HttpGet("test")]
        public void test()
        {
            List <User> u= this._dbContext.Users.ToList();
            Company c = new Company()
            {
                Name = "mishloha",
                PrivateCompanyID = 5155,
                UsersInTheCompany = u,
                customerCode = "605"
            };
            this._dbContext.companies.Add(c);
            this._dbContext.SaveChanges();
        }
    }
}
