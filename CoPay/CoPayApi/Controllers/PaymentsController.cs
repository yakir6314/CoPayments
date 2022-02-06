using CoPayApi.Data;
using CoPayApi.Data.DTO;
using CoPayApi.Data.Entities;
using CoPayApi.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoPayApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PaymentsController : ControllerBase
    {
        private readonly CoDbContext _dbContext;
        private readonly ILogger<PaymentsController> logger;
        private readonly SignInManager<User> signInManager;
        private readonly UserManager<User> userManager;
        private readonly IConfiguration config;

        public PaymentsController(ILogger<PaymentsController> logger,
            SignInManager<User> signInManager,
            UserManager<User> userManager,
            IConfiguration config,
            CoDbContext dbContext)
        {
            this.logger = logger;
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.config = config;
            _dbContext = dbContext;
        }

        [HttpPost]
        [Route("ApprovePayment")]
        [Authorize(Policy = "RequireAdminAccess")]
        public async Task<IActionResult> ApprovePayment(int paymentId)
        {
            ErrorHandler err = new ErrorHandler() { isSuccess = false, data = null, error = null };
            Payment payment = this._dbContext.payments.Where(w => w.id == paymentId).FirstOrDefault();
            if (payment == null)
            {
                err.failFunction("no payment found");
                return BadRequest(err);
            }
            if (payment.isApproved)
            {
                err.failFunction("payment already approved");
                return BadRequest(err);
            }
            payment.ApproveDate = DateTime.Now;
            payment.isApproved = true;
            this._dbContext.SaveChanges();
            err.successFunction(payment);
            return Ok(err);
        }

        [HttpPost]
        [Route("addPayment")]
        public async Task<IActionResult> addPayment([FromBody] PaymentDto paymentDto)
        {
            
            ErrorHandler err = new ErrorHandler() { isSuccess = false,data=null,error=null };

            
            if(paymentDto.sum<=0 || string.IsNullOrWhiteSpace(paymentDto.business))
            {
                err.error= "sum is smaller then zero or no business";
                return BadRequest(err);
            }
            Payment payment = new Payment()
            {
                business = paymentDto.business,
                comment = paymentDto.comment,
                dateAdded = DateTime.Now,
                sum = paymentDto.sum,
                isApproved=false
            };
            payment.user = await this.userManager.FindByEmailAsync(HttpContext.User.Identity.Name);
            this._dbContext.Add(payment);
            this._dbContext.SaveChanges();
            err.isSuccess = true;
            return Ok(err);



        }
    }
}
