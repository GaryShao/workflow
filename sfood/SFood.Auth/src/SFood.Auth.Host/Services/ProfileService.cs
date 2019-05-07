using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using SFood.DataAccess.Models.IdentityModels;
using System.Linq;
using System.Threading.Tasks;

namespace SFood.Auth.Host.Services
{
    public class ProfileService<TUser> : IProfileService
    {
        private readonly UserManager<User> _userManager;
        private readonly IUserClaimsPrincipalFactory<User> _claimsFactory;

        public ProfileService(IUserClaimsPrincipalFactory<User> claimsFactory, UserManager<User> userManager)
        {
            _claimsFactory = claimsFactory;
            _userManager = userManager;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var user = await _userManager.GetUserAsync(context.Subject);
            
            var principal = await _claimsFactory.CreateAsync(user);            
            var claims = principal.Claims.ToList();

            context.AddRequestedClaims(claims);
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var user = await _userManager.GetUserAsync(context.Subject);

            context.IsActive = (user != null);
        }
    }
}
