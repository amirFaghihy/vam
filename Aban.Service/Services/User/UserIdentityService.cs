using Aban.DataLayer.Context;
using Aban.DataLayer.Interfaces;
using Aban.Domain.Entities;
using Aban.Service.Interfaces;
using Aban.Service.Services.Generic;
using Aban.DataLayer.Repositories;
using Microsoft.AspNetCore.Mvc.Rendering;
using Aban.Domain.Enumerations;
using static Aban.Domain.Enumerations.Enumeration;
using Microsoft.AspNetCore.Identity;
using Aban.Common;
using Aban.Common.Utility;
using Aban.ViewModels;
using System.Security.Claims;
using System.Data;
using Microsoft.EntityFrameworkCore;

namespace Aban.Service.Services
{
    public class UserIdentityService : GenericService<UserIdentity>, IUserIdentityService
    {
        private readonly IUserIdentityRepository userIdentityRepository;
        private readonly IRoleRepository roleRepository;
        private readonly IUserRoleRepository userRoleRepository;
        public readonly UserManager<UserIdentity> userManager;
        public readonly RoleManager<IdentityRole> roleManager;
        public readonly SignInManager<UserIdentity> signInManager;


        public UserIdentityService(
            AppDbContext dbContext,
            UserManager<UserIdentity> userManager,
            RoleManager<IdentityRole> roleManager,
            SignInManager<UserIdentity> signInManager) : base(dbContext)
        {
            userIdentityRepository = new UserIdentityRepository(dbContext);
            roleRepository = new RoleRepository(dbContext);

            this.userManager = userManager;
            this.roleManager = roleManager;
            this.signInManager = signInManager;

            userRoleRepository = new UserRoleRepository(dbContext, this.roleManager, this.userManager);
        }

        public string GetUserId(ClaimsPrincipal user)
        => userManager.GetUserId(user);

        public string GetFirstNameLastName(ClaimsPrincipal user)
        {
            string userId = userManager.GetUserId(user);

            UserIdentity userNAME = userIdentityRepository.Find(userId);
            return userNAME.FirstName + " " + userNAME.LastName;
        }

        public Task<Tuple<UserIdentity, ResultStatusOperation>> FillModel(UserIdentity userIdentity)
        {
            ResultStatusOperation resultStatusOperation = new ResultStatusOperation
            {
                IsSuccessed = true,
                Type = Enumeration.MessageTypeResult.Success,
                Message = "عملیات با موفقیت انجام شد"
            };
            try
            {
                userIdentity.RegisterDate = userIdentity.RegisterDate;

                return Task.FromResult(Tuple.Create(userIdentity, resultStatusOperation));
            }
            catch (Exception exception)
            {
                resultStatusOperation.IsSuccessed = false;
                resultStatusOperation.Message = "خطایی رخ داده است";
                resultStatusOperation.Type = Enumeration.MessageTypeResult.Danger;
                resultStatusOperation.ErrorException = exception;
                throw new Exception("", exception);
            }
        }

        public Task<Tuple<UserIdentity, ResultStatusOperation>> ConvertViewModelToModel(
            UserIdentityViewModel viewModel)
        {
            ResultStatusOperation resultStatusOperation = new ResultStatusOperation
            {
                IsSuccessed = true,
                Type = Enumeration.MessageTypeResult.Success,
                Message = "عملیات با موفقیت انجام شد"
            };
            try
            {
#pragma warning disable CS8601 // Possible null reference assignment.
                UserIdentity model = new UserIdentity()
                {
                    Id = string.IsNullOrEmpty(viewModel.Id) ? Guid.NewGuid().ToString() : viewModel.Id,
                    UserName = viewModel.UserName,
                    PhoneNumber = viewModel.PhoneNumber,
                    FirstName = viewModel.FirstName,
                    LastName = viewModel.LastName,
                    NationalCode = viewModel.NationalCode,
                    Password = viewModel.Password,
                    IsMale = viewModel.IsMale,
                    IsConfirm = viewModel.IsConfirm,
                    IsLocked = viewModel.IsLocked,
                    BirthDate = viewModel.BirthDate,
                    Email = viewModel.Email,
                    RegisterDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                    FatherName = viewModel.FatherName,
                    HomeAddress = viewModel.HomeAddress

                };
#pragma warning restore CS8601 // Possible null reference assignment.

                if (string.IsNullOrWhiteSpace(model.UserName))
                    model.NormalizedUserName = model.UserName = model.PhoneNumber;

                if (string.IsNullOrEmpty(viewModel.Email))
                    model.Email = model.UserName + "@email.com";

                return Task.FromResult(Tuple.Create(model, resultStatusOperation));
            }
            catch (Exception exception)
            {
                resultStatusOperation.IsSuccessed = false;
                resultStatusOperation.Message = "خطایی رخ داده است";
                resultStatusOperation.Type = Enumeration.MessageTypeResult.Danger;
                resultStatusOperation.ErrorException = exception;
                throw new Exception("", exception);
            }
        }

        public Task<Tuple<UserIdentityViewModel, ResultStatusOperation>> ConvertModelToViewModel(
            UserIdentity model)
        {
            ResultStatusOperation resultStatusOperation = new ResultStatusOperation
            {
                IsSuccessed = true,
                Type = Enumeration.MessageTypeResult.Success,
                Message = "عملیات با موفقیت انجام شد"
            };
            try
            {
#pragma warning disable CS8601 // Possible null reference assignment.
                UserIdentityViewModel viewModel = new UserIdentityViewModel()
                {
                    Id = model.Id,
                    UserName = model.UserName,
                    PhoneNumber = model.PhoneNumber,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    NationalCode = model.NationalCode,
                    Password = model.Password,
                    IsMale = model.IsMale,
                    IsConfirm = model.IsConfirm,
                    IsLocked = model.IsLocked,
                    RegisterDate = model.RegisterDate,
                    BirthDate = model.BirthDate,
                    FatherName = model.FatherName,
                    Email = model.Email,
                    HomeAddress = model.HomeAddress
                };
#pragma warning restore CS8601 // Possible null reference assignment.

                return Task.FromResult(Tuple.Create(viewModel, resultStatusOperation));
            }
            catch (Exception exception)
            {
                resultStatusOperation.IsSuccessed = false;
                resultStatusOperation.Message = "خطایی رخ داده است";
                resultStatusOperation.Type = Enumeration.MessageTypeResult.Danger;
                resultStatusOperation.ErrorException = exception;
                throw new Exception("", exception);
            }
        }


        public List<SelectListItem> ReadAll(
            string selectedValue = "",
            bool? isConfirm = null,
            bool? isLocked = null,
            List<RoleName>? roleNames = null,
            string userRegistrarId = "")
        {
            ResultStatusOperation resultStatusOperation = new ResultStatusOperation
            {
                IsSuccessed = true,
                Type = MessageTypeResult.Success,
                Message = "عملیات با موفقیت انجام شد"
            };

            try
            {
                IQueryable<UserIdentity> query = userIdentityRepository.GetAll();

                if (isConfirm != null)
                {
                    query = query.Where(x => x.IsConfirm != isConfirm);
                }
                if (isLocked != null)
                {
                    query = query.Where(x => x.IsLocked != isLocked);
                }
                if (roleNames != null && roleNames.Count() != 0)
                {

                    IQueryable<string> userIds = userRoleRepository.GetAll().Where(userrole =>
                    roleRepository.GetAll().Where(role =>
                    roleNames.Select(rname => rname.ToString().Replace("_", " "))
                    .Contains(role.Name)).Select(x => x.Id)
                    .Contains(userrole.RoleId)).Select(uId => uId.UserId);

                    query = query.Where(x => userIds.Contains(x.Id));
                }

                List<SelectListItem> items = query.OrderBy(x => x.LastName).ToList().ConvertAll(x =>
                {
                    return new SelectListItem()
                    {
                        Text = x.FatherName.IsNullOrEmpty() ? $"{x.LastName} {x.FirstName}" : $"{x.LastName} {x.FirstName} | فرزند: {x.FatherName}",
                        Value = x.Id,
                        Selected = !string.IsNullOrEmpty(selectedValue) ? (x.Id == selectedValue) : false
                    };
                });

                return items;
            }
            catch (Exception exception)
            {
                resultStatusOperation.IsSuccessed = false;
                resultStatusOperation.Message = "خطایی رخ داده است";
                resultStatusOperation.Type = MessageTypeResult.Danger;
                resultStatusOperation.ErrorException = exception;
                throw new Exception("", exception);
            }
        }

        public List<SelectListItem> ReadAllWithFatherName(
            string selectedValue = "",
            bool? isConfirm = null,
            bool? isLocked = null)
        {
            ResultStatusOperation resultStatusOperation = new ResultStatusOperation
            {
                IsSuccessed = true,
                Type = MessageTypeResult.Success,
                Message = "عملیات با موفقیت انجام شد"
            };

            try
            {
                IQueryable<UserIdentity> query = userIdentityRepository.GetAll();

                if (isConfirm != null)
                {
                    query = query.Where(x => x.IsConfirm != isConfirm);
                }
                if (isLocked != null)
                {
                    query = query.Where(x => x.IsLocked != isLocked);
                }
                //if (roleNames != null && roleNames.Count() != 0)
                //{
                //    List<string> userIds = userRoleRepository.GetAll().Where(userrole =>
                //    roleRepository.GetAll().Where(role =>
                //    roleNames.Select(rname => rname.ToString().Replace("_", " "))
                //    .Contains(role.Name)).Select(x => x.Id)
                //    .Contains(userrole.RoleId)).Select(uId => uId.UserId).ToList();

                //    query = SpecificationGetData(userIds).Result.Item1;
                //    //query = query.Where(x => userIds.Contains(x.Id));
                //}

                List<SelectListItem> items = query.OrderBy(x => x.LastName).ToList().ConvertAll(x =>
                {
                    return new SelectListItem()
                    {
                        Text = x.FatherName.IsNullOrEmpty() ? $"{x.LastName} {x.FirstName}" : $"{x.LastName} {x.FirstName} | فرزند: {x.FatherName}",
                        Value = x.Id,
                        Selected = !string.IsNullOrEmpty(selectedValue) ? (x.Id == selectedValue) : false
                    };
                });

                return items;
            }
            catch (Exception exception)
            {
                resultStatusOperation.IsSuccessed = false;
                resultStatusOperation.Message = "خطایی رخ داده است";
                resultStatusOperation.Type = MessageTypeResult.Danger;
                resultStatusOperation.ErrorException = exception;
                throw new Exception("", exception);
            }
        }

        public Task<Tuple<IQueryable<UserIdentity>, ResultStatusOperation>> SpecificationGetData(
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
            List<string>? roleIds = null)

        {
            ResultStatusOperation resultStatusOperation = new ResultStatusOperation
            {
                IsSuccessed = true,
                Type = Enumeration.MessageTypeResult.Success,
                Message = "عملیات با موفقیت انجام شد"
            };
            try
            {
                IQueryable<UserIdentity> query = userIdentityRepository.GetAll();

                if (!string.IsNullOrEmpty(id))
                {
                    query = query.Where(x => x.Id == id);
                }
                if (!string.IsNullOrEmpty(userName))
                {
                    query = query.Where(x => x.UserName == userName);
                }
                if (!string.IsNullOrEmpty(phoneNumber))
                {
                    query = query.Where(x => x.PhoneNumber == phoneNumber);
                }
                if (!string.IsNullOrEmpty(firstName))
                {
                    query = query.Where(x => x.FirstName == firstName);
                }
                if (!string.IsNullOrEmpty(lastName))
                {
                    query = query.Where(x => x.LastName == lastName);
                }
                if (!string.IsNullOrEmpty(nationalCode))
                {
                    query = query.Where(x => x.NationalCode == nationalCode);
                }
                if (isMale != null)
                {
                    query = query.Where(x => x.IsMale == isMale);
                }
                if (isLocked != null)
                {
                    query = query.Where(x => x.IsLocked == isLocked);
                }
                if (isConfirm != null)
                {
                    query = query.Where(x => x.IsConfirm == isConfirm);
                }
                if (birthDateFrom != null)
                {
                    query = query.Where(x => x.BirthDate >= birthDateFrom.Value);
                }
                if (birthDateTo != null)
                {
                    query = query.Where(x => x.BirthDate <= birthDateTo.Value);
                }
                if (registerDateFrom != null)
                {
                    query = query.Where(x => x.RegisterDate >= registerDateFrom.Value);
                }
                if (registerDateTo != null)
                {
                    query = query.Where(x => x.RegisterDate <= registerDateTo.Value);
                }
                if (registerDateTo != null)
                {
                    query = query.Where(x => x.RegisterDate <= registerDateTo.Value);
                }
                if (!string.IsNullOrEmpty(fatherName))
                {
                    query = query.Where(x => x.FatherName == fatherName);
                }
                if (roleIds != null && roleIds.Count() != 0)
                {

                    IQueryable<string> userIds = userRoleRepository.GetAll().Where(ur => roleIds.Contains(ur.RoleId))
                    .Select(u => u.UserId);

                    query = query.Where(x =>
                    userRoleRepository.GetAll().Where(ur => roleIds.Contains(ur.RoleId))
                    .Select(u => u.UserId).Contains(x.Id)
                    );
                }

                return Task.FromResult(Tuple.Create(query, resultStatusOperation));

            }
            catch (Exception exception)
            {
                resultStatusOperation.IsSuccessed = false;
                resultStatusOperation.Message = "خطایی رخ داده است";
                resultStatusOperation.Type = Enumeration.MessageTypeResult.Danger;
                resultStatusOperation.ErrorException = exception;
                throw new Exception("", exception);
            }
        }

        public Task<Tuple<IQueryable<UserIdentity>, ResultStatusOperation>> SpecificationGetData(
            List<string> userIds)

        {
            ResultStatusOperation resultStatusOperation = new ResultStatusOperation
            {
                IsSuccessed = true,
                Type = Enumeration.MessageTypeResult.Success,
                Message = "عملیات با موفقیت انجام شد"
            };
            try
            {
                IQueryable<UserIdentity> query = userIdentityRepository.GetAll()
                    .Where(x => userIds.Contains(x.Id));

                return Task.FromResult(Tuple.Create(query, resultStatusOperation));

            }
            catch (Exception exception)
            {
                resultStatusOperation.IsSuccessed = false;
                resultStatusOperation.Message = "خطایی رخ داده است";
                resultStatusOperation.Type = Enumeration.MessageTypeResult.Danger;
                resultStatusOperation.ErrorException = exception;
                throw new Exception("", exception);
            }
        }

        /// <summary>
        /// نام رولها را بر میگرداند
        /// </summary>
        /// <param name="userIdentityId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<IEnumerable<string>> GetAllRolesOfUser(string userIdentityId)
        {
            ResultStatusOperation resultStatusOperation = new ResultStatusOperation
            {
                IsSuccessed = true,
                Type = MessageTypeResult.Success,
                Message = "عملیات با موفقیت انجام شد"
            };

            try
            {
                UserIdentity userIdentity = await userManager.FindByIdAsync(userIdentityId);
                IEnumerable<string> userRoles = await userManager.GetRolesAsync(userIdentity);

                return userRoles;
            }
            catch (Exception exception)
            {
                resultStatusOperation.IsSuccessed = false;
                resultStatusOperation.Message = "خطایی رخ داده است";
                resultStatusOperation.Type = MessageTypeResult.Danger;
                resultStatusOperation.ErrorException = exception;
                throw new Exception("", exception);
            }
        }

        /// <summary>
        /// نام رولها را بر میگرداند
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public IEnumerable<string> GetAllRolesIdOfUser(IEnumerable<string> roleName)
        {
            ResultStatusOperation resultStatusOperation = new ResultStatusOperation
            {
                IsSuccessed = true,
                Type = MessageTypeResult.Success,
                Message = "عملیات با موفقیت انجام شد"
            };

            try
            {
                IEnumerable<string> userRolesId = roleManager.Roles.Where(x => roleName.Contains(x.Name)).Select(x => x.Id).AsEnumerable();

                return userRolesId;
            }
            catch (Exception exception)
            {
                resultStatusOperation.IsSuccessed = false;
                resultStatusOperation.Message = "خطایی رخ داده است";
                resultStatusOperation.Type = MessageTypeResult.Danger;
                resultStatusOperation.ErrorException = exception;
                throw new Exception("", exception);
            }
        }


        public async Task<IEnumerable<UserIdentity>> GetAllUserInRoleAsync(List<RoleName> roleNames)
        {
            ResultStatusOperation resultStatusOperation = new ResultStatusOperation
            {
                IsSuccessed = true,
                Type = MessageTypeResult.Success,
                Message = "عملیات با موفقیت انجام شد"
            };

            try
            {
                List<UserIdentity> users = new();
                foreach (var role in roleNames)
                {
                    users.AddRange(await userManager.GetUsersInRoleAsync(role.ToString()));
                }

                return users;
            }
            catch (Exception exception)
            {
                resultStatusOperation.IsSuccessed = false;
                resultStatusOperation.Message = "خطایی رخ داده است";
                resultStatusOperation.Type = MessageTypeResult.Danger;
                resultStatusOperation.ErrorException = exception;
                throw new Exception("", exception);
            }

        }

        public IEnumerable<UserIdentity> GetAllUserInRole(List<RoleName> roleNames)
        {
            ResultStatusOperation resultStatusOperation = new ResultStatusOperation
            {
                IsSuccessed = true,
                Type = MessageTypeResult.Success,
                Message = "عملیات با موفقیت انجام شد"
            };

            try
            {
                List<UserIdentity> users = new();
                foreach (var role in roleNames)
                {
                    users.AddRange(userManager.GetUsersInRoleAsync(role.ToString()).Result);
                }

                return users;
            }
            catch (Exception exception)
            {
                resultStatusOperation.IsSuccessed = false;
                resultStatusOperation.Message = "خطایی رخ داده است";
                resultStatusOperation.Type = MessageTypeResult.Danger;
                resultStatusOperation.ErrorException = exception;
                throw new Exception("", exception);
            }

        }

        public async Task<IEnumerable<UserIdentity>> GetAllUserInRoleAsync(IQueryable<UserIdentity> queryUser, List<RoleName> roleNames)
        {
            ResultStatusOperation resultStatusOperation = new ResultStatusOperation
            {
                IsSuccessed = true,
                Type = MessageTypeResult.Success,
                Message = "عملیات با موفقیت انجام شد"
            };

            try
            {
                List<UserIdentity> users = new();
                foreach (var role in roleNames)
                {
                    users.AddRange(await userManager.GetUsersInRoleAsync(role.ToString()));
                }


                return queryUser.Where(x => users.Select(y => y.Id).Contains(x.Id)); ;
            }
            catch (Exception exception)
            {
                resultStatusOperation.IsSuccessed = false;
                resultStatusOperation.Message = "خطایی رخ داده است";
                resultStatusOperation.Type = MessageTypeResult.Danger;
                resultStatusOperation.ErrorException = exception;
                throw new Exception("", exception);
            }
        }


        public async Task<IdentityResult> RemoveAllRolesOfUser(string userIdentityId)
        {
            ResultStatusOperation resultStatusOperation = new ResultStatusOperation
            {
                IsSuccessed = true,
                Type = MessageTypeResult.Success,
                Message = "عملیات با موفقیت انجام شد"
            };

            try
            {
                UserIdentity userIdentity = await userManager.FindByIdAsync(userIdentityId);
                IEnumerable<string> userRoles = await userManager.GetRolesAsync(userIdentity);
                IdentityResult result = await userManager.RemoveFromRolesAsync(userIdentity, userRoles);

                return result;
            }
            catch (Exception exception)
            {
                resultStatusOperation.IsSuccessed = false;
                resultStatusOperation.Message = "خطایی رخ داده است";
                resultStatusOperation.Type = MessageTypeResult.Danger;
                resultStatusOperation.ErrorException = exception;
                throw new Exception("", exception);
            }
        }

        public List<SelectListItem> GetAllRoles(
            List<string> selectedValue,
            RoleName roleName = RoleName.Admin)
        {
            ResultStatusOperation resultStatusOperation = new ResultStatusOperation
            {
                IsSuccessed = true,
                Type = MessageTypeResult.Success,
                Message = "عملیات با موفقیت انجام شد"
            };

            try
            {
                List<SelectListItem> item = roleManager.Roles.ToList().ConvertAll(x =>
                {
                    return new SelectListItem()
                    {
                        Text = x.ConcurrencyStamp,
                        Value = x.Id.ToString(),
                        Selected = (selectedValue.Contains(x.Name))
                    };
                });

#pragma warning disable CS8604 // Possible null reference argument.
                if (roleName == RoleName.Clerk)//منشی
                {
                    item.Remove(item.FirstOrDefault(x => x.Value == "09c7caca-3ac7-41ab-9a10-7d34672271cf"));
                    item.Remove(item.FirstOrDefault(x => x.Value == "71a207d6-15ef-4fba-a0d6-d49871f8aef9"));
                    item.Remove(item.FirstOrDefault(x => x.Value == "BBF03F87-2A46-41FB-ABD1-FF52FE067633"));
                    item.Remove(item.FirstOrDefault(x => x.Value == "CCD753BE-CC88-4E6F-A63C-F98AEC11F72B"));
                }
                else if (roleName == RoleName.Foreman)
                {
                    item.Remove(item.FirstOrDefault(x => x.Value == "09c7caca-3ac7-41ab-9a10-7d34672271cf"));
                    item.Remove(item.FirstOrDefault(x => x.Value == "71a207d6-15ef-4fba-a0d6-d49871f8aef9"));
                    item.Remove(item.FirstOrDefault(x => x.Value == "BBF03F87-2A46-41FB-ABD1-FF52FE067633"));
                }
#pragma warning restore CS8604 // Possible null reference argument.

                return item;

            }
            catch (Exception exception)
            {
                resultStatusOperation.IsSuccessed = false;
                resultStatusOperation.Message = "خطایی رخ داده است";
                resultStatusOperation.Type = MessageTypeResult.Danger;
                resultStatusOperation.ErrorException = exception;
                throw new Exception("", exception);
            }
        }

        public List<RoleName> FindRoleName(List<string> roleId)
        {
            ResultStatusOperation resultStatusOperation = new ResultStatusOperation
            {
                IsSuccessed = true,
                Type = MessageTypeResult.Success,
                Message = "عملیات با موفقیت انجام شد"
            };

            try
            {
                List<string> identityRole = roleManager.Roles.Where(x => roleId.Contains(x.Id)).Select(x => x.Name).ToList();

                List<RoleName> roleNames = new List<RoleName>();
                foreach (var item in identityRole)
                {
                    object resultConvert;
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                    Enum.TryParse(typeof(RoleName), item.Replace(" ", "_"), out resultConvert);
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
                    if (resultConvert != null)
                        roleNames.Add((RoleName)resultConvert);
                }

                return roleNames;

            }
            catch (Exception exception)
            {
                resultStatusOperation.IsSuccessed = false;
                resultStatusOperation.Message = "خطایی رخ داده است";
                resultStatusOperation.Type = MessageTypeResult.Danger;
                resultStatusOperation.ErrorException = exception;
                throw new Exception("", exception);
            }
        }

        public List<string> FindRoleNameAsString(List<string> roleId)
        {
            ResultStatusOperation resultStatusOperation = new ResultStatusOperation
            {
                IsSuccessed = true,
                Type = MessageTypeResult.Success,
                Message = "عملیات با موفقیت انجام شد"
            };

            try
            {
                List<string> identityRole = roleManager.Roles.Where(x => roleId.Contains(x.Id)).Select(x => x.Name).ToList();


                return identityRole;

            }
            catch (Exception exception)
            {
                resultStatusOperation.IsSuccessed = false;
                resultStatusOperation.Message = "خطایی رخ داده است";
                resultStatusOperation.Type = MessageTypeResult.Danger;
                resultStatusOperation.ErrorException = exception;
                throw new Exception("", exception);
            }
        }

        public async Task<Tuple<UserIdentity, ResultStatusOperation>> SignUpUser(
            ControllerInfo controllerInfo,
            UserIdentity model,
            string password = "")
        {
            ResultStatusOperation resultStatusOperation = new ResultStatusOperation
            {
                IsSuccessed = true,
                Type = MessageTypeResult.Success,
                Message = "عملیات با موفقیت انجام شد"
            };

            try
            {
                if (!String.IsNullOrEmpty(model.NationalCode) && userIdentityRepository.GetAll().Any(x => x.NationalCode == model.NationalCode))
                {
                    resultStatusOperation.Type = MessageTypeResult.Warning;
                    resultStatusOperation.IsSuccessed = true;
                    resultStatusOperation.Message = $"کدملی {model.NationalCode} قبلا در سامانه ثبت شده است";
                    return Tuple.Create(model, resultStatusOperation);
                }
                //                UserIdentity UserIdentity =
                if (userIdentityRepository.GetAll().Any(x =>
                               /*|| x.Email == model.Email*/ x.PhoneNumber == model.PhoneNumber))
                {
                    resultStatusOperation.Type = MessageTypeResult.Warning;
                    resultStatusOperation.IsSuccessed = true;
                    resultStatusOperation.Message = $"شماره موبایل {model.PhoneNumber} قبلا در سامانه ثبت شده است";
                    return Tuple.Create(model, resultStatusOperation);
                }
                if (userIdentityRepository.GetAll().Any(x => x.UserName == model.UserName))
                {
                    resultStatusOperation.Type = MessageTypeResult.Warning;
                    resultStatusOperation.IsSuccessed = true;
                    resultStatusOperation.Message = $"نام کاربری {model.UserName} قبلا در سامانه ثبت شده است";
                    return Tuple.Create(model, resultStatusOperation);
                }



                #region Fill Model

#pragma warning disable CS0472 // The result of the expression is always the same since a value of this type is never equal to 'null'
                model.EmailConfirmed = model.EmailConfirmed == null ? false : model.EmailConfirmed;
                model.PhoneNumberConfirmed = model.PhoneNumberConfirmed == null ? false : model.PhoneNumberConfirmed;
                model.RegisterDate = DateTime.Now;
                model.TwoFactorEnabled = model.TwoFactorEnabled == null ? false : model.TwoFactorEnabled;
                model.LockoutEnabled = model.LockoutEnabled == null ? false : model.LockoutEnabled;
                model.AccessFailedCount = 0;
                model.IsLocked = model.IsLocked == null ? false : model.IsLocked;
                model.PhoneNumber = model.PhoneNumber;
#pragma warning disable CS0472 // The result of the expression is always the same since a value of this type is never equal to 'null'


                if (string.IsNullOrWhiteSpace(model.UserName))
                    model.NormalizedUserName = model.UserName = model.PhoneNumber;

                if (string.IsNullOrWhiteSpace(model.Email))
                    model.NormalizedEmail = null;

                if (string.IsNullOrWhiteSpace(password))
                    password = RandomPassword.Generate(10, 15);

#pragma warning disable CS8602 // Dereference of a possibly null reference.
                controllerInfo.ModelState.Remove(nameof(model.UserName));
#pragma warning restore CS8602 // Dereference of a possibly null reference.

                #endregion



                #region Check ValidateModel

                Tuple<UserIdentity, ResultStatusOperation> _model =
                    await GenericValidate<UserIdentity>.CheckValidatedModel(controllerInfo, model);

                if (_model.Item2.Type != MessageTypeResult.Success)
                {
                    return _model;
                }

                #endregion

                IdentityResult result = await userManager.CreateAsync(model, password);

                if (result.Succeeded)
                {
                    resultStatusOperation.Type = MessageTypeResult.Success;
                    return Tuple.Create(model, resultStatusOperation);
                }
                else
                {
                    string errorMessage = "";
                    foreach (var error in result.Errors)
                    {
                        errorMessage += error.Description + Environment.NewLine;
                    }

                    resultStatusOperation.Type = MessageTypeResult.Warning;
                    resultStatusOperation.IsSuccessed = false;
                    resultStatusOperation.Message = errorMessage;
                    return Tuple.Create(model, resultStatusOperation);
                }

            }
            catch (Exception exception)
            {
                resultStatusOperation.IsSuccessed = false;
                resultStatusOperation.Message = "خطایی رخ داده است";
                resultStatusOperation.Type = MessageTypeResult.Danger;
                resultStatusOperation.ErrorException = exception;
                throw new Exception("", exception);
            }
        }


        public async Task<Tuple<UserIdentity, ResultStatusOperation>> UpdateUser(
            ControllerInfo controllerInfo,
            UserIdentity model,
            string password = "")
        {
            ResultStatusOperation resultStatusOperation = new ResultStatusOperation
            {
                IsSuccessed = true,
                Type = MessageTypeResult.Success,
                Message = "عملیات با موفقیت انجام شد"
            };

            try
            {

                UserIdentity userIdentity = await userManager.FindByIdAsync(model.Id);

                if (!String.IsNullOrEmpty(model.NationalCode) && userIdentityRepository.GetAll().Where(x => x.Id != userIdentity.Id).Any(x => x.NationalCode == model.NationalCode))
                {
                    resultStatusOperation.Type = MessageTypeResult.Warning;
                    resultStatusOperation.IsSuccessed = true;
                    resultStatusOperation.Message = $"کدملی {model.NationalCode} قبلا در سامانه ثبت شده است";
                    return Tuple.Create(model, resultStatusOperation);
                }
                //                UserIdentity UserIdentity =
                if (userIdentityRepository.GetAll().Where(x => x.Id != userIdentity.Id).Any(x =>
                               /*|| x.Email == model.Email*/ x.PhoneNumber == model.PhoneNumber))
                {
                    resultStatusOperation.Type = MessageTypeResult.Warning;
                    resultStatusOperation.IsSuccessed = true;
                    resultStatusOperation.Message = $"شماره موبایل {model.PhoneNumber} قبلا در سامانه ثبت شده است";
                    return Tuple.Create(model, resultStatusOperation);
                }
                if (userIdentityRepository.GetAll().Where(x => x.Id != userIdentity.Id).Any(x => x.UserName == model.UserName))
                {
                    resultStatusOperation.Type = MessageTypeResult.Warning;
                    resultStatusOperation.IsSuccessed = true;
                    resultStatusOperation.Message = $"نام کاربری {model.UserName} قبلا در سامانه ثبت شده است";
                    return Tuple.Create(model, resultStatusOperation);
                }



                #region Fill Model

#pragma warning disable CS0472 // The result of the expression is always the same since a value of this type is never equal to 'null'
                userIdentity.FirstName = model.FirstName;
                userIdentity.LastName = model.LastName;
                userIdentity.NationalCode = model.NationalCode;
                userIdentity.Email = model.Email;
                userIdentity.EmailConfirmed = model.EmailConfirmed == null ? false : model.EmailConfirmed;
                userIdentity.PhoneNumberConfirmed = model.PhoneNumberConfirmed == null ? false : model.PhoneNumberConfirmed;
                userIdentity.ModifiedDate = DateTime.Now;
                userIdentity.TwoFactorEnabled = model.TwoFactorEnabled == null ? false : model.TwoFactorEnabled;
                userIdentity.LockoutEnabled = model.LockoutEnabled == null ? false : model.LockoutEnabled;
                userIdentity.AccessFailedCount = 0;
                userIdentity.IsLocked = model.IsLocked == null ? false : model.IsLocked;
                userIdentity.IsConfirm = model.IsConfirm == null ? false : model.IsConfirm;
                userIdentity.IsMale = model.IsMale;
                userIdentity.PhoneNumber = model.PhoneNumber;
                userIdentity.UserName = model.UserName;
                userIdentity.BirthDate = model.BirthDate;
                userIdentity.FatherName = model.FatherName;
                userIdentity.HomeAddress = model.HomeAddress;
#pragma warning disable CS0472 // The result of the expression is always the same since a value of this type is never equal to 'null'


                if (string.IsNullOrWhiteSpace(model.UserName))
                    userIdentity.NormalizedUserName = userIdentity.UserName = userIdentity.PhoneNumber;

                if (string.IsNullOrWhiteSpace(model.Email))
                    userIdentity.NormalizedEmail = null;

                #endregion



                #region Check ValidateModel

                Tuple<UserIdentity, ResultStatusOperation> _model =
                    await GenericValidate<UserIdentity>.CheckValidatedModel(controllerInfo, userIdentity);

                if (_model.Item2.Type != MessageTypeResult.Success)
                {
                    return Tuple.Create(model, resultStatusOperation);
                }

                #endregion



                if (!string.IsNullOrEmpty(model.Password))
                {
                    await userManager.ChangePasswordAsync(userIdentity, userIdentity.Password, password);
                    //await userManager.AddPasswordAsync(userIdentity, model.Password);
                }
                else
                {
                    await userManager.RemovePasswordAsync(userIdentity);
                }

                userIdentity.Password = model.Password;//حتماً باید بعد از تغییر پسورد باشد


                IdentityResult result = await userManager.UpdateAsync(userIdentity);

                if (result.Succeeded)
                {
                    resultStatusOperation.Type = MessageTypeResult.Success;
                    return Tuple.Create(userIdentity, resultStatusOperation);
                }
                else
                {
                    string errorMessage = "";
                    foreach (var error in result.Errors)
                    {
                        errorMessage += error.Description + Environment.NewLine;
                    }

                    resultStatusOperation.Type = MessageTypeResult.Warning;
                    resultStatusOperation.IsSuccessed = false;
                    resultStatusOperation.Message = errorMessage;
                    return Tuple.Create(model, resultStatusOperation);
                }

            }
            catch (Exception exception)
            {
                resultStatusOperation.IsSuccessed = false;
                resultStatusOperation.Message = "خطایی رخ داده است";
                resultStatusOperation.Type = MessageTypeResult.Danger;
                resultStatusOperation.ErrorException = exception;
                throw new Exception("", exception);
            }
        }


        public async Task<Tuple<UserIdentity, ResultStatusOperation>> AddUserRoles(
            ControllerInfo controllerInfo,
            UserIdentity model,
            List<RoleName> roles)
        {
            ResultStatusOperation resultStatusOperation = new ResultStatusOperation
            {
                IsSuccessed = true,
                Type = MessageTypeResult.Success
            };

            try
            {
                #region Check ValidateModel

                var _model = await GenericValidate<UserIdentity>.CheckValidatedModel(controllerInfo, model);
                if (_model.Item2.Type != MessageTypeResult.Success)
                {
                    return _model;
                }

                #endregion

                if (model == null)
                {
                    resultStatusOperation.Type = MessageTypeResult.Warning;
                    resultStatusOperation.IsSuccessed = false;
                    resultStatusOperation.Message = "حساب کاربری یافت نشد !";

#pragma warning disable CS8619 // Nullability of reference types in value doesn't match target type.
                    return Tuple.Create(model, resultStatusOperation);
                }


                IEnumerable<string> rolesString = roles.Select(x => x.ToString().Replace("_", " ")).ToList();
                UserIdentity userIdentity = await userManager.FindByIdAsync(model.Id);
                var result = await userManager.AddToRolesAsync(userIdentity, rolesString);

                if (!result.Succeeded)
                {
                    string errorMessage = "";
                    foreach (var error in result.Errors)
                    {
                        errorMessage += error.Description + Environment.NewLine;
                    }

                    resultStatusOperation.Type = MessageTypeResult.Danger;
                    resultStatusOperation.IsSuccessed = false;
                    resultStatusOperation.Message = errorMessage;
                    return Tuple.Create(model, resultStatusOperation);
                }

                resultStatusOperation.Type = MessageTypeResult.Success;
                return Tuple.Create(model, resultStatusOperation);
            }
            catch (Exception exception)
            {
                resultStatusOperation.IsSuccessed = false;
                resultStatusOperation.Message = "خطایی رخ داده است";
                resultStatusOperation.Type = MessageTypeResult.Danger;
                resultStatusOperation.ErrorException = exception;
                throw new Exception("", exception);
            }
        }


        public async Task<Tuple<LoginViewModel, ResultStatusOperation>> SigninUser(
            ControllerInfo controllerInfo,
            LoginViewModel model,
            TypeOfLoginMethod typeOfLoginMethod,
            bool isPersistent = false)
        {
            ResultStatusOperation resultStatusOperation = new ResultStatusOperation
            {
                IsSuccessed = true,
                Type = MessageTypeResult.Success,
                Message = "ورود با موفقیت انجام شد"
            };

            #region Run Logout method for Remove all Cookei before create new login

            LogoutAsync();

            #endregion

            try
            {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                switch (typeOfLoginMethod)
                {
                    case TypeOfLoginMethod.isLoginByUserNamePasword:
                        controllerInfo.ModelState.Remove(nameof(model.Id));
                        controllerInfo.ModelState.Remove(nameof(model.PhoneNumber));
                        controllerInfo.ModelState.Remove(nameof(model.SMSCode));
                        break;
                    case TypeOfLoginMethod.isLoginOnlyUserName:
                        controllerInfo.ModelState.Remove(nameof(model.Password));
                        controllerInfo.ModelState.Remove(nameof(model.PhoneNumber));
                        controllerInfo.ModelState.Remove(nameof(model.SMSCode));
                        break;
                    case TypeOfLoginMethod.isLoginOnlyPhoneNumber:
                        controllerInfo.ModelState.Remove(nameof(model.Password));
                        controllerInfo.ModelState.Remove(nameof(model.UserName));
                        controllerInfo.ModelState.Remove(nameof(model.SMSCode));
                        break;
                    case TypeOfLoginMethod.isLoginByPhoneNumber:
                        controllerInfo.ModelState.Remove(nameof(model.Password));
                        controllerInfo.ModelState.Remove(nameof(model.UserName));
                        break;
                }
#pragma warning restore CS8602 // Dereference of a possibly null reference.

                #region Check ValidateModel

                Tuple<LoginViewModel, ResultStatusOperation> _model =
                    await GenericValidate<LoginViewModel>.CheckValidatedModel(controllerInfo, model);

                if (_model.Item2.Type != MessageTypeResult.Success)
                {
                    resultStatusOperation.IsSuccessed = false;
                    resultStatusOperation.Type = MessageTypeResult.Warning;
                    resultStatusOperation.Message = "خطایی رخ داده است، لطفاً دوباره تلاش کنید !";
                    return Tuple.Create(model, resultStatusOperation);
                }

                #endregion


                #region Add Claims

                UserIdentity UserIdentity = new UserIdentity();

#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.

                // این قسمت به این دلیل اضاف شد که پسورد از سمت کاربر بررسی نمیشد و کد بررسی پسورد به شرط اضاف شد تا این مشکل رفع شود، 
                switch (typeOfLoginMethod)
                {
                    case TypeOfLoginMethod.isLoginByUserNamePasword:
                        UserIdentity = userIdentityRepository.GetAll().FirstOrDefault(x => x.UserName == model.UserName && x.Password == model.Password && x.IsConfirm && !x.IsLocked);
                        break;
                    case TypeOfLoginMethod.isLoginOnlyUserName:
                        // کدهای مربوطه
                        break;
                    case TypeOfLoginMethod.isLoginOnlyPhoneNumber:
                        // این کد بررسی شود
                        UserIdentity = userIdentityRepository.GetAll().FirstOrDefault(x => x.PhoneNumber == model.PhoneNumber && x.Password == model.Password && x.IsConfirm && !x.IsLocked);
                        break;
                    case TypeOfLoginMethod.isLoginByPhoneNumber:
                        // کدهای مربوطه
                        break;
                }

#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

                if (UserIdentity == null)
                {
                    resultStatusOperation.IsSuccessed = false;
                    resultStatusOperation.Type = MessageTypeResult.Warning;
                    resultStatusOperation.Message = "نام کاربری یا رمز عبور اشتباه است!";
                    return Tuple.Create(model, resultStatusOperation);
                }

                //await userManager.AddClaimAsync(UserIdentity, new Claim("userId", UserIdentity.Id));

                //await userManager.AddClaimAsync(UserIdentity, new System.Security.Claims.Claim("userId", UserIdentity.Id));
                //await userManager.CreateAsync(user);

                #endregion

                if (typeOfLoginMethod == TypeOfLoginMethod.isLoginByUserNamePasword)
                {
                    SignInResult signInResult = new SignInResult();
                    signInResult = await signInManager.PasswordSignInAsync(model.UserName, model.Password, (model.RememberMe == null || model.RememberMe == true), lockoutOnFailure: false);
                    if (!signInResult.Succeeded)
                    {
                        resultStatusOperation.Message = "نام کاربری یا کلمه‌ی عبور صحیح نیست";
                        resultStatusOperation.Type = MessageTypeResult.Warning;
                        resultStatusOperation.IsSuccessed = false;
                        return Tuple.Create(model, resultStatusOperation);
                    }
                    else if (model.ResultStatusOperation == null)
                    {
                        model.ResultStatusOperation = resultStatusOperation;
                    }

                }
                else if (typeOfLoginMethod == TypeOfLoginMethod.isLoginOnlyPhoneNumber || typeOfLoginMethod == TypeOfLoginMethod.isLoginByPhoneNumber || typeOfLoginMethod == TypeOfLoginMethod.isLoginOnlyUserName)
                {


                    if (UserIdentity == null)
                    {
                        resultStatusOperation.Type = MessageTypeResult.Warning;
                        resultStatusOperation.IsSuccessed = false;
                        resultStatusOperation.Message = "رکورد یافت نشد";
                        return Tuple.Create(model, resultStatusOperation);
                    }


                    if (typeOfLoginMethod == TypeOfLoginMethod.isLoginByPhoneNumber)
                    {
                        // این قسمت بر اساس پنل اس ام اس باید تکمیل شود
                        // و به همین دلیل کامنت شده است

                        //if (UserIdentity.ForgetPasswordCode != model.SMSCode)
                        //{
                        //    resultStatusOperation.Type = MessageTypeResult.Warning;
                        //    resultStatusOperation.IsSuccessed = false;
                        //    resultStatusOperation.Message = "کد وارد شده اشتباه است";
                        //    return Tuple.Create(model, resultStatusOperation);
                        //}
                    }

                    #region Run Logout method for Remove all Cookei before create new login

                    LogoutAsync();

                    #endregion

                    await signInManager.SignInAsync(UserIdentity, isPersistent: isPersistent);
                }
                await signInManager.SignInAsync(UserIdentity, isPersistent: isPersistent);

                model.ResultStatusOperation.Type = MessageTypeResult.Success;
                model.ResultStatusOperation.IsSuccessed = true;
                model.Id = UserIdentity.Id;
                return Tuple.Create(model, resultStatusOperation);
            }
            catch (Exception exception)
            {
                resultStatusOperation.IsSuccessed = false;
                resultStatusOperation.Message = "خطایی رخ داده است";
                resultStatusOperation.Type = MessageTypeResult.Danger;
                resultStatusOperation.ErrorException = exception;
                throw new Exception("", exception);
            }
        }



        public async void LogoutAsync()
        {
            await signInManager.SignOutAsync();
        }


    }
}
