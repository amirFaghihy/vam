using Aban.Domain.Entities;
using Aban.Service.Interfaces;
using Aban.DataLayer.Context;
using Microsoft.AspNetCore.Mvc.Rendering;
using Aban.Service.Services.Generic;
using Aban.DataLayer.Interfaces;
using Aban.Domain.Enumerations;
using Microsoft.EntityFrameworkCore;
using static Aban.Domain.Enumerations.Enumeration;
using System.Linq.Expressions;
using Aban.Common;
using Aban.DataLayer.Repositories;

namespace Aban.Service.Services
{
    public class CharityUserIdentityCharityHelperService : GenericService<CharityUserIdentityCharityHelper>, ICharityUserIdentityCharityHelperService
    {
        private readonly ICharityUserIdentityCharityHelperRepository charityUserIdentityCharityHelperRepository;
        //private readonly IUserIdentityRepository _userIdentityRepository;
        private readonly IUserIdentityService _userIdentityService;
        private readonly IRoleRepository _roleRepository;


        public CharityUserIdentityCharityHelperService(
            AppDbContext _dbContext,
            IUserIdentityService userIdentityService) : base(_dbContext)
        {
            charityUserIdentityCharityHelperRepository = new CharityUserIdentityCharityHelperRepository(_dbContext);
            //_userIdentityRepository = new UserIdentityRepository(_dbContext);
            _roleRepository = new RoleRepository(_dbContext);
            _userIdentityService = userIdentityService;

        }

        public IQueryable<UserIdentity> GetUserAllHelper(string userId)
        {
            ResultStatusOperation resultStatusOperation = new ResultStatusOperation();
            resultStatusOperation.IsSuccessed = true;
            resultStatusOperation.Type = Enumeration.MessageTypeResult.Success;
            try
            {
                IQueryable<CharityUserIdentityCharityHelper> query = charityUserIdentityCharityHelperRepository.GetAll();

                return charityUserIdentityCharityHelperRepository.GetAll().Where(x => !x.IsDelete && x.UserIdentityId == userId).Include(x =>
                 x.UserIdentity).Select(x => x.Helper).Where(
                     y =>
                     _userIdentityService.GetAllUserInRoleAsync(new List<RoleName>() { RoleName.Helper }).Result.Select(x => x.Id).ToList().Contains(y.Id)
                     );

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

        public IQueryable<UserIdentity> GetAdminAllForeman(string adminUserId)
        {
            ResultStatusOperation resultStatusOperation = new ResultStatusOperation();
            resultStatusOperation.IsSuccessed = true;
            resultStatusOperation.Type = Enumeration.MessageTypeResult.Success;
            try
            {

                IQueryable<UserIdentity> query =
                    charityUserIdentityCharityHelperRepository.GetAll()
                    .Where(x => !x.IsDelete && x.UserIdentityId == adminUserId)
                    .Include(x => x.UserIdentity).Select(x => x.Helper).Where(y =>
                    _userIdentityService.GetAllUserInRoleAsync(new List<RoleName>() { RoleName.Foreman }).Result.Select(x => x.Id).ToList().Contains(y.Id)
                    );

                return query;

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
        public IQueryable<UserIdentity> GetAdminAllForeman(IQueryable<UserIdentity> adminUserId)
        {
            ResultStatusOperation resultStatusOperation = new ResultStatusOperation();
            resultStatusOperation.IsSuccessed = true;
            resultStatusOperation.Type = Enumeration.MessageTypeResult.Success;
            try
            {
                List<UserIdentity> listForeman = adminUserId.ToList();
                IQueryable<CharityUserIdentityCharityHelper> query = charityUserIdentityCharityHelperRepository.GetAll();

                return charityUserIdentityCharityHelperRepository.GetAll().Where(x => !x.IsDelete && listForeman.Contains(x.UserIdentity)).Include(x =>
                 x.UserIdentity).Select(x => x.Helper).Where(
                     y =>
                     _userIdentityService.GetAllUserInRoleAsync(new List<RoleName>() { RoleName.Foreman }).Result.Select(x => x.Id).ToList().Contains(y.Id)
                     );

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


        public IQueryable<UserIdentity> GetForemanAllClerk(string foremanUserId, string selectedUserId = "")
        {
            ResultStatusOperation resultStatusOperation = new ResultStatusOperation();
            resultStatusOperation.IsSuccessed = true;
            resultStatusOperation.Type = Enumeration.MessageTypeResult.Success;
            try
            {
                IQueryable<UserIdentity> query = charityUserIdentityCharityHelperRepository.GetAll()
                    .Where(x => !x.IsDelete && x.UserIdentityId == foremanUserId).Include(x =>
                 x.UserIdentity).Select(x => x.Helper).Where(
                     y =>
                     _userIdentityService.GetAllUserInRoleAsync(new List<RoleName>() { RoleName.Clerk }).Result.Select(x => x.Id).ToList().Contains(y.Id)
                     );

                if (!selectedUserId.IsNullOrEmpty())
                {
                    query = query.Where(x => x.Id == selectedUserId);
                }

                return query;

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
        public IQueryable<UserIdentity> GetForemanAllClerk(IQueryable<UserIdentity> foremanUserId)
        {
            ResultStatusOperation resultStatusOperation = new ResultStatusOperation();
            resultStatusOperation.IsSuccessed = true;
            resultStatusOperation.Type = Enumeration.MessageTypeResult.Success;
            try
            {
                List<UserIdentity> listForeman = foremanUserId.ToList();
                IQueryable<CharityUserIdentityCharityHelper> query = charityUserIdentityCharityHelperRepository.GetAll();

                return charityUserIdentityCharityHelperRepository.GetAll().Where(x => !x.IsDelete && listForeman.Contains(x.UserIdentity)).Include(x =>
                 x.UserIdentity).Select(x => x.Helper).Where(
                     y =>
                     _userIdentityService.GetAllUserInRole(new List<RoleName>() { RoleName.Clerk }).Select(x => x.Id).ToList().Contains(y.Id)
                     );

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

        public IQueryable<UserIdentity> GetClerkAllHelper(
            string clerkUserId,
            string helperId = "",
            DateTime? registerDateFrom = null,
            DateTime? registerDateTo = null)
        {
            ResultStatusOperation resultStatusOperation = new ResultStatusOperation();
            resultStatusOperation.IsSuccessed = true;
            resultStatusOperation.Type = Enumeration.MessageTypeResult.Success;
            try
            {
                IQueryable<UserIdentity> query = charityUserIdentityCharityHelperRepository.GetAll().Where(x => x.UserIdentityId == clerkUserId).Include(x =>
                 x.UserIdentity).Select(x => x.Helper).Where(
                     y =>
                     _userIdentityService.GetAllUserInRole(new List<RoleName>() { RoleName.Helper }).Select(x => x.Id).ToList().Contains(y.Id)
                     );

                if (!helperId.IsNullOrEmpty())
                {
                    query = query.Where(x => x.Id == helperId);
                }

                if (registerDateFrom != null || registerDateTo != null)
                {
                    DateTime _registerDateFrom = registerDateFrom != null ? registerDateFrom.Value : new DateTime(1930, 1, 1, 1, 0, 0, 0);
                    DateTime _registerDateTo = registerDateTo != null ? registerDateTo.Value : new DateTime(DateTime.Now.AddYears(1).Year, 1, 1, 1, 0, 0, 0);
                    query = query.Where(x => x.RegisterDate >= _registerDateFrom && x.RegisterDate <= _registerDateTo);
                }

                return query;

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
        public IQueryable<UserIdentity> GetClerkAllHelper(IQueryable<UserIdentity> clerkUserId)
        {
            ResultStatusOperation resultStatusOperation = new ResultStatusOperation();
            resultStatusOperation.IsSuccessed = true;
            resultStatusOperation.Type = Enumeration.MessageTypeResult.Success;
            try
            {
                List<UserIdentity> listForeman = clerkUserId.ToList();
                IQueryable<CharityUserIdentityCharityHelper> query = charityUserIdentityCharityHelperRepository.GetAll();

                return charityUserIdentityCharityHelperRepository.GetAll().Where(x => !x.IsDelete && listForeman.Contains(x.UserIdentity)).Include(x =>
                 x.UserIdentity).Select(x => x.Helper).Where(
                     y =>
                     _userIdentityService.GetAllUserInRole(new List<RoleName>() { RoleName.Helper }).Select(x => x.Id).ToList().Contains(y.Id)
                     );

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


        public IQueryable<UserIdentity> GetAdminAllHelper(string adminUserId)
        {
            ResultStatusOperation resultStatusOperation = new ResultStatusOperation();
            resultStatusOperation.IsSuccessed = true;
            resultStatusOperation.Type = Enumeration.MessageTypeResult.Success;
            try
            {
                IQueryable<CharityUserIdentityCharityHelper> query = charityUserIdentityCharityHelperRepository.GetAll();

                return charityUserIdentityCharityHelperRepository.GetAll().Where(x => !x.IsDelete && x.UserIdentityId == adminUserId).Include(x =>
                 x.UserIdentity).Select(x => x.Helper).Where(

                     y =>
                     _userIdentityService.GetAllUserInRole(new List<RoleName>() { RoleName.Helper }).Select(x => x.Id).ToList().Contains(y.Id)
                     );

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
        public IQueryable<UserIdentity> GetAdminAllHelper(IQueryable<UserIdentity> adminUserId)
        {
            ResultStatusOperation resultStatusOperation = new ResultStatusOperation();
            resultStatusOperation.IsSuccessed = true;
            resultStatusOperation.Type = Enumeration.MessageTypeResult.Success;
            try
            {
                List<UserIdentity> listForeman = adminUserId.ToList();
                IQueryable<CharityUserIdentityCharityHelper> query = charityUserIdentityCharityHelperRepository.GetAll();

                return charityUserIdentityCharityHelperRepository.GetAll().Where(x => !x.IsDelete && listForeman.Contains(x.UserIdentity)).Include(x =>
                 x.UserIdentity).Select(x => x.Helper).Where(
                     y =>
                     _userIdentityService.GetAllUserInRole(new List<RoleName>() { RoleName.Helper }).Select(x => x.Id).ToList().Contains(y.Id)
                     );

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

        public IQueryable<UserIdentity> GetForemanAllHelper(string foremanUserId)
        {
            ResultStatusOperation resultStatusOperation = new ResultStatusOperation();
            resultStatusOperation.IsSuccessed = true;
            resultStatusOperation.Type = Enumeration.MessageTypeResult.Success;
            try
            {
                IQueryable<CharityUserIdentityCharityHelper> query = charityUserIdentityCharityHelperRepository.GetAll();

                return charityUserIdentityCharityHelperRepository.GetAll().Where(x => !x.IsDelete && x.UserIdentityId == foremanUserId).Include(x =>
                 x.UserIdentity).Select(x => x.Helper).Where(

                     y =>
                     _userIdentityService.GetAllUserInRole(new List<RoleName>() { RoleName.Helper }).Select(x => x.Id).ToList().Contains(y.Id)
                     );

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
        public IQueryable<UserIdentity> GetForemanAllHelper(IQueryable<UserIdentity> foremanUserId)
        {
            ResultStatusOperation resultStatusOperation = new ResultStatusOperation();
            resultStatusOperation.IsSuccessed = true;
            resultStatusOperation.Type = Enumeration.MessageTypeResult.Success;
            try
            {
                List<UserIdentity> listForeman = foremanUserId.ToList();
                IQueryable<CharityUserIdentityCharityHelper> query = charityUserIdentityCharityHelperRepository.GetAll();

                return charityUserIdentityCharityHelperRepository.GetAll().Where(x => !x.IsDelete && listForeman.Contains(x.UserIdentity)).Include(x =>
                 x.UserIdentity).Select(x => x.Helper).Where(
                     y =>
                     _userIdentityService.GetAllUserInRole(new List<RoleName>() { RoleName.Helper }).Select(x => x.Id).ToList().Contains(y.Id)
                     );

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

        public List<SelectListItem> ReadAll<TPropertyText, TPropertyValue>(
            List<IQueryable<UserIdentity>> query,
            string? currentUserId,
            Expression<Func<UserIdentity, TPropertyText>> text,
            Expression<Func<UserIdentity, TPropertyValue>> value,
            List<string> selectedValue,
            string nullText = "")
        {
            ResultStatusOperation resultStatusOperation = new ResultStatusOperation();
            resultStatusOperation.IsSuccessed = true;
            resultStatusOperation.Type = MessageTypeResult.Success;
            try
            {
                List<SelectListItem> listItem = new List<SelectListItem>();
                List<string> listTitleProperty = new();

                text.Body.ToString().Replace("(", "").Replace(")", "")
                    .Replace("{", "").Replace("}", "")
                    .Replace("[", "").Replace("]", "").Split("+").ToList().ForEach(x =>
                    {
                        listTitleProperty.Add(x.Split(".").Last().Trim());

                    });
                string titleProperty = PropertyUtility.GetFullPropertyName(text);
                string valueProeprty = PropertyUtility.GetFullPropertyName(value);
                query.ForEach(x =>
                {
                    x.ToList().ForEach(y =>
                    {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8604 // Possible null reference argument.
                        SelectListItem selectListItem = new SelectListItem()
                        {
                            Text = GetPropertyText<UserIdentity>(listTitleProperty, y),
                            Value = typeof(UserIdentity).GetProperty(valueProeprty).GetValue(y, null).ToString(),
                            Selected = selectedValue.Contains(typeof(UserIdentity).GetProperty(valueProeprty).GetValue(y, null).ToString()) ? true : false
                        };
#pragma warning restore CS8604 // Possible null reference argument.
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                        listItem.Add(selectListItem);
                    });

                });

                if (!String.IsNullOrEmpty(nullText))
                {
                    listItem.Insert(0, new SelectListItem(nullText, ""));
                }
                if (!currentUserId.IsNullOrEmpty())
                {
#pragma warning disable CS8604 // Possible null reference argument.
                    Task<Tuple<UserIdentity, ResultStatusOperation>> userIdentity = _userIdentityService.Find(currentUserId);
#pragma warning restore CS8604 // Possible null reference argument.
                    listItem.Insert(1, new SelectListItem(userIdentity.Result.Item1.FirstName + " " + userIdentity.Result.Item1.LastName, userIdentity.Result.Item1.Id, false));
                }
                return listItem;


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
        public Tuple<IQueryable<CharityUserIdentityCharityHelper>, ResultStatusOperation> SpecificationGetData(
            string userIdentityId = "",
            string helperId = "",
            DateTime? registerDateFrom = null,
            DateTime? registerDateTo = null)
        {
            ResultStatusOperation resultStatusOperation = new ResultStatusOperation();
            resultStatusOperation.IsSuccessed = true;
            resultStatusOperation.Type = Enumeration.MessageTypeResult.Success;
            try
            {
                IQueryable<CharityUserIdentityCharityHelper> query = charityUserIdentityCharityHelperRepository.GetAll()
                    .Where(x => !x.IsDelete);

                if (!string.IsNullOrEmpty(userIdentityId))
                {
                    query = query.Where(x => x.UserIdentityId == userIdentityId);
                }
                if (!string.IsNullOrEmpty(helperId))
                {
                    query = query.Where(x => x.HelperId.Contains(helperId));
                }
                if (registerDateFrom != null || registerDateTo != null)
                {
                    DateTime _registerDateFrom = registerDateFrom != null ? registerDateFrom.Value : new DateTime(1930, 1, 1, 1, 0, 0, 0);
                    DateTime _registerDateTo = registerDateTo != null ? registerDateTo.Value : new DateTime(DateTime.Now.AddYears(1).Year, 1, 1, 1, 0, 0, 0);
                    query = query.Where(x => x.RegisterDate >= _registerDateFrom && x.RegisterDate <= _registerDateTo);
                }

                return Tuple.Create(query, resultStatusOperation);
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

        public async Task<bool> IsUserSavedByUserId(string userIdentityId, string userRegisterId)
        {
            ResultStatusOperation resultStatusOperation = new ResultStatusOperation();
            resultStatusOperation.IsSuccessed = true;
            resultStatusOperation.Type = Enumeration.MessageTypeResult.Success;
            try
            {
                return await charityUserIdentityCharityHelperRepository.GetAll()
                    .AnyAsync(x => !x.IsDelete && x.UserIdentityId == userIdentityId && x.HelperId == userRegisterId);

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
        public async Task<List<UserIdentity>> GetAllUser(string userId, List<RoleName>? roleNames = null)
        {
            ResultStatusOperation resultStatusOperation = new ResultStatusOperation();
            resultStatusOperation.IsSuccessed = true;
            resultStatusOperation.Type = Enumeration.MessageTypeResult.Success;
            try
            {

                IQueryable<UserIdentity> queryUser = _userIdentityService.GetAll().Where(y =>
                    charityUserIdentityCharityHelperRepository.GetAll().Where(x => !x.IsDelete && x.UserIdentityId == userId).Select(x => x.HelperId)
                    .Contains(y.Id)
                );
                if (roleNames == null)
                {
                    return queryUser.ToList();
                }

                IEnumerable<UserIdentity> queryUser2 = await _userIdentityService.GetAllUserInRoleAsync(queryUser, roleNames);


                return queryUser2.ToList();
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
        public async Task<List<UserIdentity>> GetAllUser(List<RoleName> roleNames)
        {
            ResultStatusOperation resultStatusOperation = new ResultStatusOperation();
            resultStatusOperation.IsSuccessed = true;
            resultStatusOperation.Type = Enumeration.MessageTypeResult.Success;
            try
            {

                IQueryable<UserIdentity> queryUser = _userIdentityService.GetAll().Where(y =>

                    charityUserIdentityCharityHelperRepository.GetAll()
                    .Where(x => !x.IsDelete).Select(x => x.HelperId)
                    .Contains(y.Id)
                );





                IEnumerable<UserIdentity> queryUser2 = await _userIdentityService.GetAllUserInRoleAsync(queryUser, roleNames);


                return queryUser2.ToList();
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
        public Tuple<CharityUserIdentityCharityHelper, ResultStatusOperation> FillModel(CharityUserIdentityCharityHelper charityUserIdentityCharityHelper)
        {
            ResultStatusOperation resultStatusOperation = new ResultStatusOperation();
            resultStatusOperation.IsSuccessed = true;
            resultStatusOperation.Type = Enumeration.MessageTypeResult.Success;
            try
            {
                charityUserIdentityCharityHelper.IsDelete = false;
                charityUserIdentityCharityHelper.RegisterDate = DateTime.Now;

                return Tuple.Create(charityUserIdentityCharityHelper, resultStatusOperation);
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

        public List<SelectListItem> ReadAll(int selectedValue)
        {
            {
                ResultStatusOperation resultStatusOperation = new ResultStatusOperation();
                resultStatusOperation.IsSuccessed = true;
                resultStatusOperation.Type = Enumeration.MessageTypeResult.Success;

                try
                {
                    List<CharityUserIdentityCharityHelper> query = charityUserIdentityCharityHelperRepository.GetAll()
                        .Where(x => !x.IsDelete)
                        .Include(x => x.Helper)
                        .OrderBy(x => x.Id).ToList();

                    List<SelectListItem> item = query.ConvertAll(x =>
                    {
                        return new SelectListItem()
                        {
                            Text = $"{x.Helper.UserName} {x.Helper.LastName}",
                            Value = x.Id.ToString(),
                            Selected = (x.Id == selectedValue) ? true : false
                        };
                    });


                    return item;
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
        }

        public async Task<List<SelectListItem>> GetUserHelper(string userId, string selectedValue = "")
        {
            ResultStatusOperation resultStatusOperation = new ResultStatusOperation();
            resultStatusOperation.IsSuccessed = true;
            resultStatusOperation.Type = Enumeration.MessageTypeResult.Success;

            try
            {


                IEnumerable<UserIdentity> resultQuery =
                    await _userIdentityService.GetAllUserInRoleAsync(new List<RoleName>() { RoleName.Helper });

                List<UserIdentity> resultQueryList = resultQuery.ToList();


                List<UserIdentity> query =
                charityUserIdentityCharityHelperRepository.GetAll()
                    .Include(x => x.Helper)
                    .Where(x => x.UserIdentityId == userId && resultQuery.Select(y => y.Id).Contains(x.Helper.Id)).Include(x => x.Helper)
                   .OrderBy(x => x.RegisterDate).Select(x => x.Helper).ToList();

                List<SelectListItem> item = query.ConvertAll(x =>
                {
                    return new SelectListItem()
                    {
                        Text = $"{x.FirstName} {x.LastName}",
                        Value = x.Id.ToString(),
                        Selected = (x.Id == selectedValue) ? true : false
                    };
                });

                item.Insert(0, new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem()
                {
                    Text = "--- انتخاب ---",
                    Value = ""
                });
                return item;
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

        public async Task<List<SelectListItem>> GetUserHelper(
            string userId,
            List<string>? selectedValue = null)
        {
            ResultStatusOperation resultStatusOperation = new ResultStatusOperation();
            resultStatusOperation.IsSuccessed = true;
            resultStatusOperation.Type = Enumeration.MessageTypeResult.Success;

            try
            {
                var resultQuery = await _userIdentityService.GetAllUserInRoleAsync(new List<RoleName>() { RoleName.Helper });
                List<UserIdentity> resultQueryList = resultQuery.ToList();

                List<UserIdentity> query =
                charityUserIdentityCharityHelperRepository.GetAll()
                    //.Include(x => x.Helper)
                    .Where(x => !x.IsDelete && x.UserIdentityId == userId && resultQuery
                    .Select(y => y.Id).Contains(x.Helper.Id)).Include(x => x.Helper)
                   .OrderBy(x => x.RegisterDate).Select(x => x.Helper).ToList();

                List<SelectListItem> item = query.ConvertAll(x =>
                {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                    return new SelectListItem()
                    {
                        Text = $"{x.FirstName} {x.LastName}",
                        Value = x.Id.ToString(),
                        Selected = (selectedValue.Contains(x.Id)) ? true : false
                    };
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                });


                return item;
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

        public async Task<List<SelectListItem>> GetAllHelper(List<string>? selectedValue = null)
        {
            ResultStatusOperation resultStatusOperation = new ResultStatusOperation();
            resultStatusOperation.IsSuccessed = true;
            resultStatusOperation.Type = Enumeration.MessageTypeResult.Success;

            try
            {


                IEnumerable<UserIdentity> resultQuery =
                    await _userIdentityService.GetAllUserInRoleAsync(new List<RoleName>() { RoleName.Helper });

                List<UserIdentity> resultQueryList = resultQuery.ToList();


                List<UserIdentity> query =
                charityUserIdentityCharityHelperRepository.GetAll()
                    .Where(x => !x.IsDelete && resultQuery.Select(y => y.Id).Contains(x.Helper.Id)).Include(x => x.Helper)
                   .OrderBy(x => x.RegisterDate).Select(x => x.Helper).ToList();

                List<SelectListItem> item = query.ConvertAll(x =>
                {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                    return new SelectListItem()
                    {
                        Text = $"{x.FirstName} {x.LastName}",
                        Value = x.Id.ToString(),
                        Selected = (selectedValue.Contains(x.Id)) ? true : false
                    };
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                });


                return item;
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

        public ResultStatusOperation LogicDelete(CharityUserIdentityCharityHelper model)
        {
            ResultStatusOperation resultStatusOperation = new ResultStatusOperation
            {
                IsSuccessed = true,
                Type = MessageTypeResult.Success,
                Message = "عملیات با موفقیت انجام شد"
            };
            try
            {
                if (model == null)
                {
                    resultStatusOperation.Message = "رکورد یافت نشد";
                    resultStatusOperation.IsSuccessed = false;
                    resultStatusOperation.Type = MessageTypeResult.Warning;
#pragma warning disable CS8619 // Nullability of reference types in value doesn't match target type.
                    return resultStatusOperation;
                }

                model.IsDelete = true;

                charityUserIdentityCharityHelperRepository.Update(model);
                charityUserIdentityCharityHelperRepository.SaveChangesAsync();
                resultStatusOperation.Type = MessageTypeResult.Success;
                resultStatusOperation.Message = "اطلاعات با موفقیت بروزرسانی شد";
                return resultStatusOperation;
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


        public List<SelectListItem> ReadAllForemanAllClerk(
            string foremanUserId,
            List<string>? selectedValue = null)
        {
            ResultStatusOperation resultStatusOperation = new ResultStatusOperation();
            resultStatusOperation.IsSuccessed = true;
            resultStatusOperation.Type = Enumeration.MessageTypeResult.Success;

            try
            {
                List<UserIdentity> query = charityUserIdentityCharityHelperRepository.GetAll()
                    .Where(x => !x.IsDelete && x.UserIdentityId == foremanUserId).Include(x =>
                     x.UserIdentity).Select(x => x.Helper).Where(
                         y =>
                         _userIdentityService.GetAllUserInRoleAsync(new List<RoleName>() { RoleName.Clerk }).Result.Select(x => x.Id).ToList().Contains(y.Id)
                         ).ToList();

                List<SelectListItem> item = query.ConvertAll(x =>
                {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                    return new SelectListItem()
                    {
                        Text = $"{x.FirstName} {x.LastName}",
                        Value = x.Id.ToString(),
                        Selected = selectedValue != null ? ((selectedValue.Contains(x.Id)) ? true : false) : false
                    };
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                });


                return item;
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



    }


}
