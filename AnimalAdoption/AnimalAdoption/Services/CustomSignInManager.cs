using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using AnimalAdoption.Models;
using Microsoft.AspNetCore.Authentication;

namespace AnimalAdoption.Services
{
    public class CustomSignInManager : SignInManager<User>
    {
        public CustomSignInManager(UserManager<User> userManager, IHttpContextAccessor contextAccessor, IUserClaimsPrincipalFactory<User> claimsFactory, IOptions<IdentityOptions> optionsAccessor, ILogger<SignInManager<User>> logger, IAuthenticationSchemeProvider schemes, IUserConfirmation<User> confirmation)
            : base(userManager, contextAccessor, claimsFactory, optionsAccessor, logger, schemes, confirmation)
        {
        }

        public override async Task<ClaimsPrincipal> CreateUserPrincipalAsync(User user)
        {
            var principal = await base.CreateUserPrincipalAsync(user);
            var identity = (ClaimsIdentity)principal.Identity!;

            if (user.IsAdmin)
            {
                identity.AddClaim(new Claim("IsAdmin", "true"));
            }
            return principal;
        }
    }
}