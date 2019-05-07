using Microsoft.AspNetCore.Identity;
using SFood.Auth.Common.Extensions;
using SFood.DataAccess.Models.IdentityModels;

namespace SFood.Auth.Host.Validators
{
    public class MySha256PasswordHasher : IPasswordHasher<User>
    {
        public string HashPassword(User user, string password)
        {
            return password.SHA256();
        }

        public PasswordVerificationResult VerifyHashedPassword(User user, string hashedPassword, string providedPassword)
        {
            if (hashedPassword == providedPassword)
            {
                return PasswordVerificationResult.Success;
            }
            return PasswordVerificationResult.Failed;
        }
    }
}
