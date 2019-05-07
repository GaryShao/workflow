using Microsoft.AspNetCore.Identity;
using SFood.Auth.Common.Enums;
using SFood.DataAccess.Infrastructure.Interfaces;
using SFood.DataAccess.Models.IdentityModels;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SFood.Auth.Host.Validators
{
    public class NewUserValidator : IUserValidator<User>
    {
        private static readonly string passwordRegex = @"(?=.*[0-9])(?=.*[a-zA-Z]).{6,12}";

        private static readonly string emailRegex = @"^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";

        private readonly IReadOnlyRepository _readOnlyRepository;

        public NewUserValidator(IReadOnlyRepository readOnlyRepository)
        {
            _readOnlyRepository = readOnlyRepository;
        }

        public Task<IdentityResult> ValidateAsync(UserManager<User> manager, User user)
        {            
            var result = new IdentityResult();            

            EmailValidationResult emailValidationResult;

            var isValidEmail = IsValidEmail(user.Email, out emailValidationResult);
            var isValidPhone = IsValidPhone(user.PhoneNumber);
            var isValidPassword = IsValidPassword(user.PasswordHash);

            if (isValidEmail && isValidPhone && isValidPassword)
            {
                return Task.FromResult(IdentityResult.Success);
            }
            var errors = new List<IdentityError>();

            if (!isValidEmail)
            {
                errors.Add(new IdentityError {
                    Code = "Invalid Email",
                    Description = GetEmailValidationDescription(emailValidationResult)
                });
            }

            if (!isValidPassword)
            {
                errors.Add(new IdentityError
                {
                    Code = "Invalid Password",
                    Description = "Password must contain number, capital or lower-case letter, symbol; more than 6 and less than 12 letters"
                });
            }

            if (!isValidPhone)
            {
                errors.Add(new IdentityError
                {
                    Code = "Invalid Phone",
                    Description = "This phone number has already existed."
                });
            }

            return Task.FromResult(IdentityResult.Failed(errors.ToArray()));
        }

        private bool IsValidEmail(string email, out EmailValidationResult emailValidationResult)
        {
            var isFormatValid = Regex.IsMatch(email, emailRegex);
            if (!isFormatValid)
            {
                emailValidationResult = EmailValidationResult.InvalidFormat;
                return false;
            }
            var userWithSameName = _readOnlyRepository.GetFirst<User>(u => u.Email == email);
            if (userWithSameName != null)
            {
                emailValidationResult = EmailValidationResult.AlreadyExist;
                return false;
            }
            emailValidationResult = EmailValidationResult.Success;
            return true;
        }

        private bool IsValidPhone(string newPhone)
        {
            var userWithSamePhone = _readOnlyRepository.GetFirst<User>(u => u.PhoneNumber == newPhone);
            return userWithSamePhone == null;
        }

        private bool IsValidPassword(string password)
        {            
            return Regex.IsMatch(password, passwordRegex);
        }

        private string GetEmailValidationDescription(EmailValidationResult validationResult)
        {
            switch (validationResult)
            {
                case EmailValidationResult.InvalidFormat:
                    return "It's not a valid email format";
                case EmailValidationResult.AlreadyExist:
                    return "The email has already existed in our db";
                default:
                    return null;
            }
        }
    }
}
