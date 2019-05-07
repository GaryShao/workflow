using System.Collections.Generic;
using System.Threading.Tasks;

namespace SFood.ClientEndpoint.Application.Validator
{
    public interface IDishValidator
    {
        Task ValidateBadDishId(List<string> dishIds, string restaurantId);

        Task ValidateDuplicatedDishId(List<string> dishIds, string categoryId);
    }
}
