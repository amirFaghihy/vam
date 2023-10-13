using Aban.Domain.Entities;
using Aban.Service.IServices.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq.Expressions;
using static Aban.Domain.Enumerations.Enumeration;

namespace Aban.Service.Interfaces
{
    public interface ICharityUserIdentityCharityHelperService : IGenericService<CharityUserIdentityCharityHelper>
    {
        Tuple<CharityUserIdentityCharityHelper, ResultStatusOperation> FillModel(CharityUserIdentityCharityHelper charityUserIdentityCharityHelper);
        Tuple<IQueryable<CharityUserIdentityCharityHelper>, ResultStatusOperation> SpecificationGetData(string userIdentityId = "", string helperId = "", DateTime? registerDateFrom = null, DateTime? registerDateTo = null);
        List<SelectListItem> ReadAll(int selectedValue);
        /// <summary>
        /// Return All Users that saved by passed userId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<List<UserIdentity>> GetAllUser(string userId, List<RoleName>? roleNames = null);
        Task<List<UserIdentity>> GetAllUser(List<RoleName> roleNames);

        Task<bool> IsUserSavedByUserId(string userIdentityId, string userRegisterId);

        Task<List<SelectListItem>> GetUserHelper(string userId, List<string>? selectedValue = null);
        Task<List<SelectListItem>> GetUserHelper(string userId, string selectedValue = "");
        Task<List<SelectListItem>> GetAllHelper(List<string>? selectedValue = null);

        ResultStatusOperation LogicDelete(CharityUserIdentityCharityHelper model);
        List<SelectListItem> ReadAllForemanAllClerk(string foremanUserId, List<string>? selectedValue = null);

        IQueryable<UserIdentity> GetUserAllHelper(string userId);

        IQueryable<UserIdentity> GetAdminAllForeman(string adminUserId);
        IQueryable<UserIdentity> GetAdminAllForeman(IQueryable<UserIdentity> adminUserId);
        IQueryable<UserIdentity> GetAdminAllHelper(string adminUserId);
        IQueryable<UserIdentity> GetAdminAllHelper(IQueryable<UserIdentity> adminUserId);


        IQueryable<UserIdentity> GetForemanAllClerk(string foremanUserId, string selectedUserId = "");
        IQueryable<UserIdentity> GetForemanAllClerk(IQueryable<UserIdentity> foremanUserId);
        IQueryable<UserIdentity> GetForemanAllHelper(string foremanUserId);
        IQueryable<UserIdentity> GetForemanAllHelper(IQueryable<UserIdentity> foremanUserId);


        IQueryable<UserIdentity> GetClerkAllHelper(string clerkUserId, string helperId = "", DateTime? registerDateFrom = null, DateTime? registerDateTo = null);
        IQueryable<UserIdentity> GetClerkAllHelper(IQueryable<UserIdentity> clerkUserId);



        List<SelectListItem> ReadAll<TPropertyText, TPropertyValue>(List<IQueryable<UserIdentity>> query, string? currentUserId,
            Expression<Func<UserIdentity, TPropertyText>> text, Expression<Func<UserIdentity, TPropertyValue>> value, List<string> selectedValue, string nullText = "");

    }
}