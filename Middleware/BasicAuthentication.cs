using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;

namespace StudentCRUD.Middleware
{
    public class BasicAuthentication : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public BasicAuthentication(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
        {
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if(!Request.Headers.ContainsKey("Authorization"))
            {
                return AuthenticateResult.Fail("Authentication Header Not Found");
            }

            try
            {
                var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
                var credentialbytes = Convert.FromBase64String(authHeader.Parameter);

                var credentials = Encoding.UTF8.GetString(credentialbytes).Split(":");

                var username = credentials[0];
                var password = credentials[1];

                if(username == "Admin" && password == "123")
                {
                    var claims = new[] { new Claim(ClaimTypes.Name, username) };
                    var identity = new ClaimsIdentity(claims, Scheme.Name);
                    var principle = new ClaimsPrincipal(identity);
                    var ticket = new AuthenticationTicket(principle, Scheme.Name);

                    return AuthenticateResult.Success(ticket);
                }
                else
                {
                    return AuthenticateResult.Fail("Invalid UserName  Or Password ! Please Try Again");
                }
            }
            catch (Exception ex)
            {
                return AuthenticateResult.Fail($"Authentication Fails {ex.Message}");
            }
            throw new NotImplementedException();
        }
    }
}
