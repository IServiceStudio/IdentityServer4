using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AuthDemo2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        [HttpGet("login")]
        public async Task<IActionResult> Login(string loginName, string loginPwd)
        {
            if (loginName == "admin" && loginPwd == "password")
            {
                var claim = new ClaimsIdentity("UserIDentity");
                claim.AddClaim(new Claim(ClaimTypes.Name, loginName));
                claim.AddClaim(new Claim(ClaimTypes.Email, "iservice@outlook.com"));
                await HttpContext.SignInAsync("AuthScheme", new ClaimsPrincipal(claim), new AuthenticationProperties
                {
                    ExpiresUtc = DateTime.UtcNow.AddMinutes(30)
                });
                return Ok("登陆成功");
            }
            return NotFound("用户名或密码错误");
        }

        [HttpGet("loginOut")]
        public async Task LoginOut()
        {
            await HttpContext.SignOutAsync("AuthScheme");
        }

        [HttpGet("Authentication")]
        public async Task<IActionResult> Authentication()
        {
            var result = await HttpContext.AuthenticateAsync("AuthScheme");
            if (result?.Principal != null)
            {
                HttpContext.User = result.Principal;
                return Ok($"{HttpContext.User.Identity.Name}:认证成功");
            }
            return NotFound("认证失败");
        }

        [HttpGet("Authorization")]
        public async Task<IActionResult> Authorization()
        {
            var result = await HttpContext.AuthenticateAsync("AuthScheme");
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
                    await HttpContext.ForbidAsync("AuthScheme");
                    return NotFound("授权失败");
                }
                else
                {
                    return Ok($"{user.Identity.Name}授权成功");
                }
            }
            else
            {
                await HttpContext.ChallengeAsync("AuthScheme");
                return NotFound("授权失败");
            }
        }
    }
}