using Aban.Common;
using Aban.Domain.Entities;
using Aban.Service.IServices.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Aban.ViewModels;
using static Aban.Domain.Enumerations.Enumeration;
using System.Security.Claims;

namespace Aban.Service.Interfaces
{
    public interface IUserIdentityService : IGenericService<UserIdentity>
    {
        string GetUserId(ClaimsPrincipal user);
        string GetFirstNameLastName(ClaimsPrincipal user);
        Task<Tuple<UserIdentity, ResultStatusOperation>> FillModel(UserIdentity userIdentity);

        Task<Tuple<UserIdentity, ResultStatusOperation>> ConvertViewModelToModel(
            UserIdentityViewModel viewModel);

        Task<Tuple<UserIdentityViewModel, ResultStatusOperation>> ConvertModelToViewModel(
            UserIdentity model);

        Task<Tuple<IQueryable<UserIdentity>, ResultStatusOperation>> SpecificationGetData(
            string id = "",
            string userName = "",
            string phoneNumber = "",
            string firstName = "",
            string lastName = "",
            string nationalCode = "",
            bool? isMale = null,
            bool? isLocked = null,
            bool? isConfirm = null,
            DateTime? birthDateFrom = null,
            DateTime? birthDateTo = null,
            DateTime? registerDateFrom = null,
            DateTime? registerDateTo = null,
            string fatherName = "",
            DateTime? startActivityDateFrom = null,
            DateTime? startActivityDateTo = null,
            List<string>? roleIds = null);


        Task<Tuple<IQueryable<UserIdentity>, ResultStatusOperation>> SpecificationGetData(List<string> userIds);

        Task<IEnumerable<string>> GetAllRolesOfUser(string userIdentityId);
        IEnumerable<string> GetAllRolesIdOfUser(IEnumerable<string> roleName);
        Task<IEnumerable<UserIdentity>> GetAllUserInRoleAsync(List<RoleName> roleNames);
        IEnumerable<UserIdentity> GetAllUserInRole(List<RoleName> roleNames);
        //Task<IEnumerable<UserIdentity>> GetAllUserInRole(IQueryable<UserIdentity> queryUser, List<RoleName> roleNames);
        Task<IEnumerable<UserIdentity>> GetAllUserInRoleAsync(IQueryable<UserIdentity> queryUser, List<RoleName> roleNames);
        Task<IdentityResult> RemoveAllRolesOfUser(string userIdentityId);

        List<SelectListItem> GetAllRoles(List<string> selectedValue, RoleName roleName = RoleName.Admin);

        List<RoleName> FindRoleName(List<string> roleId);
        List<string> FindRoleNameAsString(List<string> roleId);
        Task<Tuple<UserIdentity, ResultStatusOperation>> SignUpUser(
            ControllerInfo controllerInfo,
            UserIdentity model,
            string password = "");

        Task<Tuple<UserIdentity, ResultStatusOperation>> UpdateUser(
            ControllerInfo controllerInfo,
            UserIdentity model,
            string password = "");

        Task<Tuple<UserIdentity, ResultStatusOperation>> AddUserRoles(
            ControllerInfo controllerInfo,
            UserIdentity model,
            List<RoleName> roles);

        Task<Tuple<LoginViewModel, ResultStatusOperation>> SigninUser(
            ControllerInfo controllerInfo,
            LoginViewModel model,
            TypeOfLoginMethod typeOfLoginMethod,
            bool isPersistent = false);

        void LogoutAsync();

        List<SelectListItem> ReadAll(string selectedValue = "", bool? isConfirm = null, bool? isLocked = null, List<RoleName>? roleNames = null, string userRegistrarId = "");
        List<SelectListItem> ReadAllWithFatherName(string selectedValue = "", bool? isConfirm = null, bool? isLocked = null, List<RoleName>? roleNames = null);
    }
}
