using Microsoft.EntityFrameworkCore;
using SFood.DataAccess.Common.Enums;
using SFood.DataAccess.EFCore;
using SFood.DataAccess.Infrastructure.Interfaces;
using SFood.DataAccess.Models;
using SFood.DataAccess.Models.IdentityModels;
using SFood.MerchantEndpoint.Application.Dtos.Parameters.Staff;
using SFood.MerchantEndpoint.Application.Dtos.Results.Staff;
using SFood.MerchantEndpoint.Common.Consts;
using SFood.MerchantEndpoint.Common.Exceptions;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace SFood.MerchantEndpoint.Application.ServiceImplements
{
    public class StaffService : IStaffService
    {
        private readonly IRepository _repository;
        private readonly IReadOnlyRepository _readOnlyRepository;
        private readonly SFoodDbContext _dbContext;       

        public StaffService(IRepository repository,
            IReadOnlyRepository readOnlyRepository,
            SFoodDbContext dbContext)
        {
            _repository = repository;
            _readOnlyRepository = readOnlyRepository;
            _dbContext = dbContext;
        }

        /// <summary>
        /// 邀请员工
        /// </summary>
        /// <returns></returns>
        public async Task InviteStaff(InviteStaffParam param)
        {
            var user = await _readOnlyRepository.GetFirstAsync<User>(u => 
                u.PhoneNumber == param.Phone);

            if (user != null)
            {
                throw new BadRequestException($"The user with that phone has already existed in system. ");
            }

            var sqlParams = new SqlParameter[4]
            {
                new SqlParameter("@Phone", param.Phone),
                new SqlParameter("@NickName", param.Name),
                new SqlParameter("@RestaurantId", param.RestaurantId),
                new SqlParameter("@StaffStatus", StaffStatus.NotActivated.ToString())
            };

            var sql = @"
                BEGIN TRANSACTION myTran;  
                USE [SFood-Beta];  

                declare @userId nvarchar(32);
                declare @employeeRoleId nvarchar(32);

                set @userId = REPLACE(NEWID(), '-', '');

                insert into IdentitySchema.Users (Id, PhoneNumber, EmailConfirmed, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnabled, AccessFailedCount)
                values(@userId, @Phone, 0, 0, 0, 0, 0); 

                select @employeeRoleId = Id
                from IdentitySchema.Roles
                where Name = 'restaurant_employee';

                insert into IdentitySchema.UserRoles(UserId, RoleId)
                values(@userId, @employeeRoleId);

                insert into IdentitySchema.UserExtensions(Id, UserId, NickName, RestaurantId, StaffStatus)
                values(REPLACE(NEWID(), '-', ''), @userId, @NickName, @RestaurantId, @StaffStatus);

                COMMIT TRANSACTION myTran;                                  
            ";

            await _dbContext.Database.ExecuteSqlCommandAsync(sql, sqlParams);            
        }       

        /// <summary>
        /// 删除员工
        /// </summary>
        /// <returns></returns>
        public async Task DeleteStaff(DeleteStaffParam param)
        {
            var sqlParams = new SqlParameter[1]
            {
                new SqlParameter("@Id", param.Id)
            };

            var sql = @"
                BEGIN TRANSACTION myTran  
                USE [SFood-Beta];  

                declare @employeeRoleId nvarchar(32);

                select @employeeRoleId = Id
                from IdentitySchema.Roles
                where Name = 'restaurant_employee';

                delete from IdentitySchema.UserExtensions
                where UserId = @Id;

                delete from IdentitySchema.UserRoles
                where UserId = @Id and RoleId = @employeeRoleId;

                delete from IdentitySchema.Users
                where Id = @Id;

                COMMIT TRANSACTION myTran    
            ";

            await _dbContext.Database.ExecuteSqlCommandAsync(sql, sqlParams);
        }

        /// <summary>
        /// 改变员工状态
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task ChangeStaffStatus(ChangeStatusParam param)
        {           
            var userExtension = await _readOnlyRepository.GetFirstAsync<UserExtension>(ue =>
                ue.UserId == param.Id);

            userExtension.StaffStatus = param.Status;
            _repository.Update(userExtension);
        }

        /// <summary>
        /// 获取员工列表
        /// </summary>
        /// <returns></returns>
        public async Task<List<StaffResult>> GetAll(string restaurantId)
        {
            var countryCodes = (await _readOnlyRepository.GetAllAsync<CountryCode>()).ToList();

            var users = (await _readOnlyRepository.GetAllAsync<UserExtension>(ue =>
                ue.RestaurantId == restaurantId &&
                ue.StaffStatus.HasValue, null, "User")).ToList();

            var staffs = users.Select(ue => 
                new StaffResult {
                    Id = ue.UserId,
                    Name = ue.NickName,
                    Phone = ue.User.PhoneNumber,
                    Status = ue.StaffStatus.Value,
                    CountryCode = countryCodes.FirstOrDefault(cc => cc.Id == ue.CountryCodeId) == null ? null : $"+{countryCodes.FirstOrDefault(cc => cc.Id == ue.CountryCodeId).Code.TrimEnd()}"
                }).ToList();

            return staffs;
        }

        public async Task ActivateStaff(ActivateStaffParam param)
        {
            var user = await _readOnlyRepository.GetFirstAsync<User>(u =>
                u.PhoneNumber == param.Phone);

            user.PasswordHash = param.Password;
            _repository.Update(user);

            var extension = await _readOnlyRepository.GetFirstAsync<UserExtension>(ue =>
                ue.UserId == user.Id);
            extension.StaffStatus = StaffStatus.Normal;
            _repository.Update(extension);
        }
    }
}
