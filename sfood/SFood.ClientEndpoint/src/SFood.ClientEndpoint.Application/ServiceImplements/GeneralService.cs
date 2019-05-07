using Microsoft.Extensions.Options;
using Microsoft.International.Converters.PinYinConverter;
using SFood.ClientEndpoint.Application.Dtos.Internal;
using SFood.ClientEndpoint.Application.Dtos.Result;
using SFood.ClientEndpoint.Application.ServiceInterfaces;
using SFood.ClientEndpoint.Common.Options;
using SFood.DataAccess.Infrastructure.Interfaces;
using SFood.DataAccess.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFood.ClientEndpoint.Application.ServiceImplements
{
    public class GeneralService : IGeneralService
    {
        private readonly IReadOnlyRepository _readOnlyRepository;
        private readonly QiNiuOption _options;

        public GeneralService(IReadOnlyRepository readOnlyRepository,
            IOptionsSnapshot<QiNiuOption> options)
        {
            _readOnlyRepository = readOnlyRepository;
            _options = options.Value;
        }

        /// <summary>
        /// 获取所有的餐厅分类
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<RestaurantCategoryResult>> GetRestaurantCategories()
        {
            var categories = (await _readOnlyRepository.GetAllAsync<RestaurantCategory>()).
                Select(category => new RestaurantCategoryResult {
                    Id = category.Id,
                    Name = category.Name,
                    Icon = category.Icon,
                    SelectedIcon = category.SelectedIcon
                });
            return categories;
        }

        public async Task<List<GroupedCountryCodesResult>> GetCountryCodes()
        {
            var codes = (await _readOnlyRepository.GetAllAsync<CountryCode>()).Select(c =>
                new CountryCodeDto
                {
                    Id = c.Id,
                    Code = $"+{c.Code.Trim()}",
                    Name = c.Name,
                    EnglishName = c.EnglishName,
                    FlagUrl = $"{_options.Domain}{c.FlagUrl}",
                    PinYin = GetPinYin(c.Name),
                    Initial = GetPinYinInitial(c.Name)
                }).ToList();
            var result = codes.OrderBy(c => c.Initial).GroupBy(c => c.Initial).Select(g => new GroupedCountryCodesResult
            {
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
