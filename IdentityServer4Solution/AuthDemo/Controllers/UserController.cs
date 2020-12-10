using AuthDemo.Filter;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace AuthDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        [HttpGet("login")]
        [AllowAnonymous]
        public IActionResult Login(string loginName, string loginPwd)
        {
            if (loginName == "admin" && loginPwd == "password")
            {
                HttpContext.Response.Cookies.Append("UserInfo", loginName, new CookieOptions { Expires = DateTime.Now.AddMinutes(30) });
                return Ok("登陆成功");
            }
            return NotFound("用户名或密码错误");
        }

        [CustomAuthorization]
        public IActionResult Get()
        {
            string userInfo = HttpContext.Request.Cookies["UserInfo"];
            if (userInfo == null)
                return NotFound("用户未登录");
            return Ok(userInfo);
        }
    }
}