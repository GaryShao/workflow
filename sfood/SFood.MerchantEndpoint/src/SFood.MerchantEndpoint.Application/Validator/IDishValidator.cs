using System.Collections.Generic;
using System.Threading.Tasks;

namespace SFood.MerchantEndpoint.Application.Validator
{
    public interface IDishValidator
    {
        Task ValidateDishIds(List<string> dishIds, string restaurantId);

        Task ValidateDuplicateIds(List<string> dishIds, string restaurantId, string categoryId);
    }
}
