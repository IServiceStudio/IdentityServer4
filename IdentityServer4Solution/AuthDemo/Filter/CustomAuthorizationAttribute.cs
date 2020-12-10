using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;

namespace AuthDemo.Filter
{
    public class CustomAuthorizationAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (context.ActionDescriptor.EndpointMetadata.Any(c => c is AllowAnonymousAttribute))
                return;

            string userInfo = context.HttpContext.Request.Cookies["UserInfo"];
            if (userInfo == null)
            {
                var result = new NotFoundResult();
                result.ExecuteResult(context);
                context.Result = result;
            }
        }
    }
}
