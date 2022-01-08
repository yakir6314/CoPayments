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
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CoPayApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> logger;
        private readonly SignInManager<User> signInManager;
        private readonly UserManager<User> userManager;
        private readonly IConfiguration config;

        public AccountController(ILogger<AccountController> logger,
            SignInManager<User> signInManager,
            UserManager<User> userManager,
            IConfiguration config)
        {
            this.logger = logger;
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.config = config;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto model)
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
        [HttpPost("changePassword")]
        [Authorize]
        public async Task<IActionResult> changePassword([FromBody] ChangePasswordDto model)
        {
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
                            await userManager.AddToRoleAsync(user, "Agent");
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
        [HttpPost("ChangeToAdminPolicy")]
        [Authorize(Policy = "RequireAdminAccess")]
        public async Task<IActionResult> ChangeToAdminPolicy([FromBody] string emailAddress)
        {
            if (ModelState.IsValid)
            {
                User user = await this.userManager.FindByEmailAsync(emailAddress);
                if (user != null)
                {
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
                            new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName)
                        };
                        var roles = await this.userManager.GetRolesAsync(user);
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
