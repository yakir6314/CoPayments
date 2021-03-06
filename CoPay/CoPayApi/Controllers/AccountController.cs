using CoPayApi.Data;
using CoPayApi.Data.Entities;
using CoPayApi.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CoPayApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly CoDbContext _dbContext;
        private readonly ILogger<AccountController> logger;
        private readonly SignInManager<User> signInManager;
        private readonly UserManager<User> userManager;
        private readonly IConfiguration config;


        public AccountController(ILogger<AccountController> logger,
            SignInManager<User> signInManager,
            UserManager<User> userManager,
            IConfiguration config,
            CoDbContext dbContext)
        {
            this.logger = logger;
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.config = config;
            this._dbContext = dbContext;
        }
        [HttpPost("register"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddEmployee([FromBody] RegisterDto model)
        {
            if (ModelState.IsValid)
            {
                var existingUser = await this.userManager.FindByEmailAsync(model.Email);
                if (existingUser == null)
                {
                    User user = new User();
                    user.UserName = model.Email;
                    user.Email = model.Email;
                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;
                    //User adminUser = await userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                    user.company = this._dbContext.companies.FirstOrDefault();
                    IdentityResult result = userManager.CreateAsync(user, model.Password).Result;

                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, "Agent");
                        return Created("", model);
                    }
                }

            }

            return BadRequest();
        }
        [HttpPost("changePassword"), Authorize]
        public async Task<IActionResult> changePassword([FromBody] ChangePasswordDto model)
        {
            if (HttpContext.User.Identity.Name != model.Email)
            {
                return Unauthorized();
            }
            if (ModelState.IsValid)
            {
                User user = await this.userManager.FindByEmailAsync(model.Email);
                
                if (user != null)
                {
                    var passwordCheck = await this.signInManager.CheckPasswordSignInAsync(user, model.OldPassword, false);
                    if (passwordCheck.Succeeded)
                    {
                        IdentityResult result = userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword).Result;
                        if (result.Succeeded)
                        {
                            LoginDto login = new LoginDto
                            {
                                Email = model.Email,
                                Password = model.NewPassword
                            };
                            return await Login(login);
                        }
                    }
                }
                else
                {
                    return Unauthorized();
                }

            }
            return BadRequest();
        }
        [HttpPost("ChangeToAdminPolicy"), Authorize(Roles = "Admin")]
        
        public async Task<IActionResult> ChangeToAdminPolicy(string emailAddress)
        {
            if (ModelState.IsValid)
            {
                User user = await this.userManager.FindByEmailAsync(emailAddress);
                User adminUser=await userManager.GetUserAsync(HttpContext.User);
                if (user != null)
                {
                    List<string> currentRole = userManager.GetRolesAsync(user).Result.ToList();
                    if(currentRole.Any(a=>a== "Admin"))
                    {
                        return BadRequest();
                    }
                    if (user.company.Id != adminUser.company.Id)
                    {
                        return Unauthorized();
                    }
                    var role=userManager.AddToRoleAsync(user, "Admin").Result;
                    if (role.Succeeded)
                    {
                        return Ok();
                    }
                    else
                    {
                        return StatusCode(500, "something went wrong");
                    }
                }
                else
                {
                    return Unauthorized();
                }
            }
            return BadRequest();
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            if (ModelState.IsValid)
            {
                User user = await this.userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    var passwordCheck = await this.signInManager.CheckPasswordSignInAsync(user, model.Password, false);
                    if (passwordCheck.Succeeded)
                    {
                        var claims = new List<Claim>
                        {
                            new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                            new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
                        };
                        var roles = await this.userManager.GetRolesAsync(user);
                        foreach (var role in roles)
                        {
                            claims.Add(new Claim("roles", role));
                        }
                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.config["Tokens:Key"]));
                        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                        var token = new JwtSecurityToken(
                            this.config["Tokens:Issuer"],
                            this.config["Tokens:Audience"],
                            claims,
                            expires: DateTime.UtcNow.AddHours(3),
                            signingCredentials: credentials
                            );

                        return Ok(new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(token),
                            expiration = token.ValidTo
                        });
                    }

                }
                else
                {
                    return Unauthorized();
                }
            }

            return BadRequest();
        }
    }
}
