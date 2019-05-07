using SFood.MerchantEndpoint.Application.Dtos.Parameters;
using SFood.MerchantEndpoint.Application.Dtos.Parameters.Restaurant;
using SFood.MerchantEndpoint.Application.Dtos.Results;
using SFood.MerchantEndpoint.Application.Dtos.Results.Restaurant;
using System.Threading.Tasks;

namespace SFood.MerchantEndpoint.Application
{
    public interface IRestaurantService
    {
        Task PostRestaurantBasicInfo(RestaurantBasicInfoParam param);

        Task SwitchToOpen(SwitchOpenOrCloseParam param);

        Task PostRestaurantQualificationInfo(QualificationInfoParam param);

        Task PostRestaurantConfig(RestaurantConfigurationParam param);

        Task<RestaurantConfigurationResult> GetRestaurantConfig(string restaurantId);

        Task<RestaurantProfileResult> GetRestaurantProfile(string restaurantId);

        Task PostRestaurantProfile(RestaurantProfileParam param);

        Task PostRestaurantRoughProfile(RestaurantRoughProfileParam param);

        Task PostRestaurantAnnouncement(RestaurantAnnouncementParam param);

        Task PostRestaurantIntroduction(RestaurantIntroductionParam param);

        Task PostRestaurantImages(RestaurantImagesParam param);

        Task<RestaurantIndexResult> GetRestaurantIndexInfo(string restaurantId);

        Task<bool> GetOpenStatus(string restaurantId);

        Task<QualificationInfoResult> GetQualification(string restaurantId);

        Task UpdateQualification(UpdateQualificationParam param);
    }
}
