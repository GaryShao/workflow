using SFood.MerchantEndpoint.Application.Dtos.Parameters.Staff;
using SFood.MerchantEndpoint.Application.Dtos.Results.Staff;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SFood.MerchantEndpoint.Application
{
    public interface IStaffService
    {
        Task InviteStaff(InviteStaffParam param);

        Task DeleteStaff(DeleteStaffParam param);

        Task ChangeStaffStatus(ChangeStatusParam param);

        Task<List<StaffResult>> GetAll(string restaurantId);

        Task ActivateStaff(ActivateStaffParam param);
    }
}
