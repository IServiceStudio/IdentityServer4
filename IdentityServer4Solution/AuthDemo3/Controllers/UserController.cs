using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AuthDemo3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        [HttpGet("login")]
        public async Task<IActionResult> Login(string loginName, string loginPwd, string role)
        {
            if (loginName == "admin" && loginPwd == "password")
            {
                var claim = new ClaimsIdentity("UserIDentity");
                claim.AddClaim(new Claim(ClaimTypes.Name, loginName));
                claim.AddClaim(new Claim(ClaimTypes.Email, "iservice@outlook.com"));
                if (role != null)
                {
                    claim.AddClaim(new Claim(ClaimTypes.Role, role));
                }

                await HttpContext.SignInAsync(new ClaimsPrincipal(claim), new AuthenticationProperties
                {
                    ExpiresUtc = DateTime.UtcNow.AddMinutes(30)
                });
                return Ok("登陆成功");
            }
            return NotFound("用户名或密码错误");
        }

        [HttpGet("loginOut")]
        [Authorize]
        public async Task LoginOut()
        {
            await HttpContext.SignOutAsync();
        }

        [HttpGet("Authentication")]
        [Authorize]
        public async Task<IActionResult> Authentication()
        {
            var result = await HttpContext.AuthenticateAsync();
            if (result?.Principal != null)
            {
                HttpContext.User = result.Principal;
                return Ok($"{HttpContext.User.Identity.Name}:认证成功");
            }
            return NotFound("认证失败");
        }

        [HttpGet("Authorization")]
        [Authorize(AuthenticationSchemes = "Cookies")]
        public async Task<IActionResult> Authorization()
        {
            var result = await HttpContext.AuthenticateAsync();
            if (result?.Principal == null)
            {
                return NotFound("认证失败");
            }
            HttpContext.User = result.Principal;
            var user = HttpContext.User;
            if (user?.Identity.IsAuthenticated ?? false)
            {
                if (user?.Identity.Name.Equals("admin", StringComparison.OrdinalIgnoreCase) == false)
                {
                    await HttpContext.ForbidAsync();
                    return NotFound("授权失败");
                }
                else
                {
                    return Ok($"{user.Identity.Name}授权成功");
                }
            }
            else
            {
                await HttpContext.ChallengeAsync();
                return NotFound("授权失败");
            }
        }

        [Authorize(Roles = "admin")]
        [HttpGet("Admin")]
        public IActionResult Admin()
        {
            return Ok("Admin");
        }

        [Authorize(Roles = "visitor")]
        [HttpGet("Visitor")]
        public IActionResult Visitor()
        {
            return Ok("Visitor");
        }

        [Authorize(Roles = "admin,visitor")]
        [HttpGet("AdminAndVisitor")]
        public IActionResult AdminAndVisitor()
        {
            return Ok($"{HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.Role).First().Value}");
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpGet("NomalPolicy")]
        public IActionResult NomalPolicy()
        {
            return Ok("NomalPolicy");
        }
    }
}