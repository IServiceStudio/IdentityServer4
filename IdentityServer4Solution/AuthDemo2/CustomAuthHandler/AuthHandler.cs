using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AuthDemo2.CustomAuthHandler
{
    public class AuthHandler : IAuthenticationHandler, IAuthenticationSignInHandler, IAuthenticationSignOutHandler
    {
        #region 成员变量及其属性

        public AuthenticationScheme AuthenticationScheme { get; set; }
        protected HttpContext HttpContext { get; set; }

        #endregion

        #region IAuthenticationHandler

        public Task InitializeAsync(AuthenticationScheme scheme, HttpContext context)
        {
            AuthenticationScheme = scheme;
            HttpContext = context;
            return Task.CompletedTask;
        }

        public Task<AuthenticateResult> AuthenticateAsync()
        {
            var cookie = HttpContext.Request.Cookies["UserInfo"];
            if (string.IsNullOrEmpty(cookie))
                return Task.Run(() => AuthenticateResult.NoResult());

            return Task.Run(() => AuthenticateResult.Success(Deserialize(cookie)));
        }

        public Task ChallengeAsync(AuthenticationProperties properties)
        {
            return Task.CompletedTask;
        }

        public Task ForbidAsync(AuthenticationProperties properties)
        {
            HttpContext.Response.StatusCode = 403;
            return Task.CompletedTask;
        }

        #endregion

        #region SignIn&SignOut

        public Task SignInAsync(ClaimsPrincipal user, AuthenticationProperties properties)
        {
            var ticket = new AuthenticationTicket(user, properties, AuthenticationScheme.Name);
            HttpContext.Response.Cookies.Append("UserInfo", Serialize(ticket));
            return Task.CompletedTask;
        }

        public Task SignOutAsync(AuthenticationProperties properties)
        {
            HttpContext.Response.Cookies.Delete("UserInfo");
            return Task.CompletedTask;
        }

        #endregion

        #region private func

        private AuthenticationTicket Deserialize(string content)
        {
            byte[] byteTicket = System.Text.Encoding.Default.GetBytes(content);
            return TicketSerializer.Default.Deserialize(byteTicket);
        }
        private string Serialize(AuthenticationTicket ticket)
        {
            byte[] byteTicket = TicketSerializer.Default.Serialize(ticket);
            return Encoding.Default.GetString(byteTicket);
        }

        #endregion
    }
}
