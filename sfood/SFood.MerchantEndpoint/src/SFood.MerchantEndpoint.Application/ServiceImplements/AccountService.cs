using Microsoft.Extensions.Options;
using Microsoft.International.Converters.PinYinConverter;
using SFood.DataAccess.Common.Enums;
using SFood.DataAccess.Infrastructure.Interfaces;
using SFood.DataAccess.Models;
using SFood.DataAccess.Models.IdentityModels;
using SFood.MerchantEndpoint.Application.Dtos.Internal;
using SFood.MerchantEndpoint.Application.Dtos.Parameters;
using SFood.MerchantEndpoint.Application.Dtos.Parameters.Account;
using SFood.MerchantEndpoint.Application.Dtos.Results.Account;
using SFood.MerchantEndpoint.Common.Exceptions;
using SFood.MerchantEndpoint.Common.Options;
using SFood.MerchantEndpoint.Common.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFood.MerchantEndpoint.Application.ServiceImplements
{
    public class AccountService : IAccountService
    {
        private readonly IRepository _repository;
        private readonly IReadOnlyRepository _readOnlyRepository;
        //private readonly ILocalizationUtility _localizationUtility;
        private readonly QiNiuOption _qiNiuOptions;        

        public AccountService(IRepository repository,
            IReadOnlyRepository readOnlyRepository,
            //ILocalizationUtility localizationUtility,
            IOptionsSnapshot<QiNiuOption> qiNiuOptions)
        {
            _repository = repository;
            _readOnlyRepository = readOnlyRepository;
            _qiNiuOptions = qiNiuOptions.Value;
        }

        /// <summary>
        /// 更新密码
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task UpdatePasswordByUserId(ResetPasswordParam param)
        {
            var user = await _readOnlyRepository.GetFirstAsync<User>(u => u.Id == param.UserId);
            if (user == null)
            {
                throw new BadRequestException("The user does not exist");
            }
            user.PasswordHash = param.NewPassword;

            _repository.Update(user);
        }

        /// <summary>
        /// 更新密码
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task UpdatePasswordByPhone(RetrievePasswordParam param)
        {
            var user = await _readOnlyRepository.GetFirstAsync<User>(u => 
                u.PhoneNumber == param.Phone);
            
            if (user == null)
            {
                throw new BadRequestException("The user does not exist");
            }

            var userExtension = await _readOnlyRepository.GetFirstAsync<UserExtension>(ue =>
                ue.UserId == user.Id && ue.CountryCodeId == param.CountryCodeId);

            if (userExtension == null)
            {
                throw new BadRequestException("Wrong country code .");
            }

            user.PasswordHash = param.Password;

            _repository.Update(user);
        }

        /// <summary>
        /// 更新手机号码
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task UpdatePhone(UpdatePhoneNumberParam param)
        {
            var user = await _readOnlyRepository.GetOneAsync<User>(u => u.Id == param.UserId);
            if (user == null)
            {
                throw new Exception("No such data exit in our db. ");
            }

            if (user.PhoneNumber == param.Phone)
            {
                throw new BadRequestException("The new phone could not be just as same as the old one. ");
            }

            user.PhoneNumber = param.Phone;
            user.PhoneNumberConfirmed = true;
            _repository.Update(user);
        }

        /// <summary>
        /// 当前的密码是否正确
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<bool> IsPassworRight(VerificationPasswordParam param)
        {
            var user = await _readOnlyRepository.GetOneAsync<User>(u => u.Id == param.UserId);
            if (user == null)
            {
                throw new Exception("No such data exit in our db. ");
            }

            return user.PasswordHash == param.Password;
        }

        /// <summary>
        /// 验证码是否正确
        /// </summary>
        /// <returns></returns>
        public async Task<bool> IsVerificationCodeValid(VerificationCodeValidationParam param)
        {
            var countryCode = await _readOnlyRepository.GetFirstAsync<CountryCode>(cc => 
                cc.Id == param.CountryCodeId);
            if (countryCode == null)
            {
                throw new BadRequestException($"Wrong country code.");
            }

            var phoneNumber = param.Phone;
            var record = await _readOnlyRepository.GetFirstAsync<VerificationCode>(c => c.Phone == phoneNumber && 
                            (DateTime.UtcNow - c.CreatedTime).TotalMinutes < 5,
                            codes => codes.OrderByDescending(c => c.CreatedTime));
            if (record == null)
            {
                throw new Exception($"No verification code found for this phone {param.Phone} in db");
            }

            return record.Code == param.Code;
        }

        public async Task Logout(string userId)
        {
            var user = await _readOnlyRepository.GetFirstAsync<User>(u => u.Id == userId);
            if (user == null)
            {
                throw new Exception($"user not exist. ");
            }
            var userExtension = await _readOnlyRepository.GetFirstAsync<UserExtension>(u => 
                u.UserId == userId);
            if (userExtension != null)
            {
                var restaurantId = userExtension.RestaurantId;
                _repository.Delete(userExtension);
                var restaurant = await _readOnlyRepository.GetFirstAsync<Restaurant>(r => 
                    r.Id == restaurantId);
                restaurant.Status = RestaurantStatus.Logout;
                _repository.Update(restaurant);
            }

            _repository.Delete(user);
        }

        public async Task<List<GroupedCountryCodesResult>> GetDialingCodes()
        {
            var codes = (await _readOnlyRepository.GetAllAsync<CountryCode>()).Select(c => 
                new CountryCodeDto {
                    Id = c.Id,
                    Code = $"+{c.Code.Trim()}",
                    Name = c.Name,
                    EnglishName = c.EnglishName,
                    FlagUrl = $"{_qiNiuOptions.Domain}{c.FlagUrl}",
                    PinYin = GetPinYin(c.Name),
                    Initial = GetPinYinInitial(c.Name)
                }).ToList();
            var result = codes.OrderBy(c => c.Initial).GroupBy(c => c.Initial).Select(g => new GroupedCountryCodesResult {
                Initial = g.Key,
                Countries = g.OrderBy(c => c.PinYin).ToList()
            }).ToList();

            return result;
        }

        /// <summary>
        /// 获取拼音缩写
        /// </summary>
        /// <param name="countryName"></param>
        private string GetPinYin(string chineseStr)
        {
            var builder = new StringBuilder();
            for (int i = 0; i < chineseStr.Length; i++)
             {
                 if (ChineseChar.IsValidChar(chineseStr[i]))
                 {
                     var cc = new ChineseChar(chineseStr[i]);           
                     builder.Append(cc.Pinyins[0].TrimEnd('1', '2', '3', '4', '5').ToLower());
                 }
                 else
                 {
                    builder.Append(chineseStr[i]);
                 }
             }
            return builder.ToString();
        }

        /// <summary>
        /// 获取拼音首字母
        /// </summary>
        /// <param name="countryName"></param>
        private string GetPinYinInitial(string chineseStr)
        {
            var builder = new StringBuilder();
            var initialChinese = chineseStr[0];
            if (ChineseChar.IsValidChar(initialChinese))
            {
                var initial = new ChineseChar(chineseStr[0]);
                var pinyin = initial.Pinyins[0];
                return pinyin.ElementAt(0).ToString();
            }
            else
            {
                return initialChinese.ToString();
            }               
        }
    }
}
