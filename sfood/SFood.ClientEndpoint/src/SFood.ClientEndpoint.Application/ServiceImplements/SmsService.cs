using Microsoft.Extensions.Options;
using SFood.ClientEndpoint.Application.Dtos.Parameter;
using SFood.ClientEndpoint.Application.ServiceInterfaces;
using SFood.ClientEndpoint.Common.Exceptions;
using SFood.ClientEndpoint.Common.Extensions;
using SFood.ClientEndpoint.Common.Options;
using SFood.ClientEndpoint.Common.Utilities;
using SFood.DataAccess.Infrastructure.Interfaces;
using SFood.DataAccess.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SFood.ClientEndpoint.Application.ServiceImplements
{
    public class SmsService : ISmsService
    {
        private readonly IRepository _repository;
        private readonly IReadOnlyRepository _readOnlyRepository;
        private readonly ISecurityUtility _securityUtility;
        private readonly ISmsUtility _smsUtility;
        private readonly VerificationCodeOption _vCodeOption;

        public SmsService(IRepository repository
            , IReadOnlyRepository readOnlyRepository
            , ISecurityUtility securityUtility
            , ISmsUtility smsUtility
            , IOptionsSnapshot<VerificationCodeOption> vCodeOptionsSnapshot)
        {
            _repository = repository;
            _readOnlyRepository = readOnlyRepository;
            _smsUtility = smsUtility;
            _securityUtility = securityUtility;
            _vCodeOption = vCodeOptionsSnapshot.Value;
        }

        public async Task SendVerficationCode(SendVCodeSMSParam param)
        {
            var countryCode = await _readOnlyRepository.GetFirstAsync<CountryCode>(cc =>
                cc.Id == param.CountryCodeId);

            if (countryCode == null)
            {
                throw new BadRequestException("Wrong Country Code. ");
            }
            var verificationCode = _securityUtility.Generate(6);
            var content = _vCodeOption.SMSContent["zh-CN"].Replace("{SMSCODE}", verificationCode);


            await _smsUtility.SendingAsync(new Common.Dtos.CommsHub.SendParam
            {
                Phone = param.Phone,
                CountryCode = $"+{countryCode.Code}",
                Content = content
            });

            await StoreVerificationCode(new StoreVCodeParam
            {
                Code = verificationCode,
                Phone = param.Phone
            });
            return;
        }

        public async Task<double?> GetElapsedTimeOfLatestCode(string phone)
        {
            var latestCode = await _readOnlyRepository.GetFirstAsync<VerificationCode>(vc => vc.Phone == phone,
                    codes => codes.OrderByDescending(c => c.CreatedTime));

            if (latestCode == null)
            {
                return null;
            }

            return (DateTime.UtcNow - latestCode.CreatedTime).TotalSeconds;
        }

        private async Task StoreVerificationCode(StoreVCodeParam param)
        {
            await _repository.CreateAsync(new VerificationCode
            {
                Id = Guid.NewGuid().ToUuidString(),
                Phone = param.Phone,
                Code = param.Code
            });
        }
    }
}
