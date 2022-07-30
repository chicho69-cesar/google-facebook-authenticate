using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GoogleAndFacebookAuthentication.Controllers {
    [AllowAnonymous, Route("account")]
    public class AccountController : Controller {
        private readonly ILogger<AccountController> _logger;
        private readonly HttpContext httpContext;

        public AccountController(
            ILogger<AccountController> logger,
            IHttpContextAccessor httpContextAccesor
        ) {
            _logger = logger;
            httpContext = httpContextAccesor.HttpContext;
        }

        [Route("google-login")]
        public IActionResult GoogleLogin() {
            var properties = new AuthenticationProperties { RedirectUri = Url.Action("GoogleResponse") };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        [Route("google-response")]
        public async Task<IActionResult> GoogleResponse() {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            var claims = result.Principal.Identities
                .FirstOrDefault()
                .Claims.Select(claim => new {
                    claim.Issuer,
                    claim.OriginalIssuer,
                    claim.Type,
                    claim.Value
                });

            _logger.LogWarning($"{GetUserId()}");

            return Json(claims);
        }

        [Route("facebook-login")]
        public IActionResult FacebookLogin() {
            var properties = new AuthenticationProperties { RedirectUri = Url.Action("FacebookResponse") };
            return Challenge(properties, FacebookDefaults.AuthenticationScheme);
        }

        [Route("facebook-response")]
        public async Task<IActionResult> FacebookResponse() {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            var claims = result.Principal.Identities
                .FirstOrDefault()
                .Claims.Select(claim => new {
                    claim.Issuer,
                    claim.OriginalIssuer,
                    claim.Type,
                    claim.Value
                });
            
            _logger.LogWarning($"{GetUserId()}");

            return Json(claims);
        }

        private string GetUserId() {
            if (httpContext.User.Identity.IsAuthenticated) {
                var idClaim = httpContext.User.Claims
                    .Where(x => x.Type == ClaimTypes.NameIdentifier)
                    .FirstOrDefault();

                _logger.LogWarning($"{idClaim}");

                var id = (idClaim.Value);

                return id;
            } else {
                //throw new ApplicationException("El usuario no esta autenticado");
                return "-1";
            }
        }
    }
}