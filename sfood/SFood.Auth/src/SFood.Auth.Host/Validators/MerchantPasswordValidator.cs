using IdentityModel;
using IdentityServer4.AspNetIdentity;
using IdentityServer4.Events;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace SFood.Auth.Host.Validators
{
    public class MerchantPasswordValidator<TUser> : ResourceOwnerPasswordValidator<TUser>
        where TUser : class
    {
        private readonly IEventService _events;
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<TUser> _userManager;
        private readonly ILogger<ResourceOwnerPasswordValidator<TUser>> _logger;
        private readonly SignInManager<TUser> _signInManager;

        public MerchantPasswordValidator(UserManager<TUser> userManager, SignInManager<TUser> signInManager,
            IEventService events, ILogger<ResourceOwnerPasswordValidator<TUser>> logger,
            ApplicationDbContext dbContext) : base(userManager,
            signInManager, events, logger)
        {
            _events = events;
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _dbContext = dbContext;
        }

        public override async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.PhoneNumber == context.UserName);

            if (user == null)
            {
                _logger.LogInformation("No user found matching username: {username}", context.UserName);
                await _events.RaiseAsync(new UserLoginFailureEvent(context.UserName, "invalid username",
                    interactive: false));
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant);

                return;
            }

            if (user.PasswordHash == context.Password)
            {
                context.Result = new GrantValidationResult(user.Id, OidcConstants.AuthenticationMethods.Password);
            }
            else
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant);
            }
        }
    }
}
