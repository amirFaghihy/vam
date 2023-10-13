using Aban.Common;
using Aban.Common.Utility;
using Aban.Domain.Configuration;
using Aban.Domain.Entities;
using Aban.Domain.Enumerations;
using Aban.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Aban.ViewModels;
using static Aban.Domain.Enumerations.Enumeration;
using DateTimeFormat = Aban.Domain.Enumerations.Enumeration.DateTimeFormat;
using Aban.Service.Services;

namespace Aban.Dashboard.Areas.User.Controllers
{
    [Area("User")]

    //TODO: چرا فقط ادمین می تواند وارد این کنترلر شود؟ مگر منشی یا سرکاربر نباید مددکارهای خود را ثبت کنند
    [Authorize(Roles = "Admin,Foreman,Clerk")]
    public class UserIdentityController : GenericController
    {

        #region Constructor

        private readonly IUserIdentityService userIdentityService;
        private readonly ICharityUserIdentityCharityHelperService charityUserIdentityCharityHelperService;
        private readonly IOptions<PathsConfiguration> pathsConfiguratin;
        private readonly IOptions<IdentityConfiguration> identityConfiguration;

        //private readonly IStateService stateService;
        //private readonly ICityService cityService;
        //private readonly string localPath = "\\{ViewDirectoryName}\\uploads\\useridentity\\";
        //private readonly string webPath = "/{ViewDirectoryName}/uploads/useridentity/";


        public UserIdentityController(
            IUserIdentityService userIdentityService,
            ICharityUserIdentityCharityHelperService charityUserIdentityCharityHelperService,
            IOptions<PathsConfiguration> pathsConfiguratin,
            IOptions<IdentityConfiguration> identityConfiguration
            ) : base(pathsConfiguratin)
        {
            this.userIdentityService = userIdentityService;
            this.charityUserIdentityCharityHelperService = charityUserIdentityCharityHelperService;
            this.pathsConfiguratin = pathsConfiguratin;
            this.identityConfiguration = identityConfiguration;
        }

        string ViewAddress = "~/User/UserIdentity";
#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
        string View = "";
#pragma warning restore CS0108 // Member hides inherited member; missing new keyword

        #endregion

        [HttpGet]
        public async Task<IActionResult> Index(
            string id = "",
            string userName = "",
            string fatherName = "",
            string phoneNumber = "",
            string firstName = "",
            string lastName = "",
            string nationalCode = "",
            bool? ismale = null,
            bool? islocked = null,
            bool? isconfirm = null,
            string birthDateFrom = "",
            string birthDateTo = "",
            string registerDateFrom = "",
            string registerDateTo = "",
            string startActivityDateFrom = "",
            string startActivityDateTo = "",
            List<string>? roleNames = null,
            int pageNumber = 1,
            int pageSize = 10,
            string search = "",
            string sortColumn = "RegisterDate",
            string lastColumn = "",
            bool isDesc = true)
        {
            try
            {
                if (User.IsInRole("Clerk"))
                {
                    return RedirectToAction("GetClerkAllHelper", "CharityUserIdentityCharityHelper", new { area = "Charity" });
                }
                else if (User.IsInRole("Foreman"))
                {
                    return RedirectToAction("GetForemanAllClerk", "CharityUserIdentityCharityHelper", new { area = "Charity" });
                }


                #region selectedValue

                ViewBag.id = id;
                ViewBag.pageNumber = pageNumber;
                ViewBag.fatherName = fatherName;
                ViewBag.pageSize = pageSize;
                ViewBag.search = search;
                ViewBag.sortColumn = sortColumn;
                ViewBag.lastColumn = lastColumn;
                ViewBag.isDesc = isDesc;
                ViewBag.userName = userName;
                ViewBag.phoneNumber = phoneNumber;
                ViewBag.firstName = firstName;
                ViewBag.lastName = lastName;
                ViewBag.nationalCode = nationalCode;
                ViewBag.ismale = ismale;
                ViewBag.islocked = islocked;
                ViewBag.isconfirm = isconfirm;
                ViewBag.birthDateFrom = birthDateFrom;
                ViewBag.birthDateTo = birthDateTo;
                ViewBag.registerDateFrom = registerDateFrom;
                ViewBag.registerDateTo = registerDateTo;
                ViewBag.startActivityDateFrom = startActivityDateFrom;
                ViewBag.startActivityDateTo = startActivityDateTo;
                ViewBag.roleNames = userIdentityService.FindRoleNameAsString(roleNames);


                //ViewBag.BrandId = brandId;

                //ViewBag.headerSort = headerSort;  ViewBag.sortColumn 
                //ViewBag.lastHeader = lastHeader;  ViewBag.lastColumn
                //ViewBag.isDesc = isDesc;          ViewBag.isDesc 
                if (ViewBag.lastColumn == ViewBag.sortColumn)
                {
                    ViewBag.isDesc = isDesc == true ? false : true;
                    isDesc = ViewBag.isDesc;
                }
                else
                {
                    ViewBag.isDesc = isDesc;
                }
                ViewBag.lastColumn = ViewBag.sortColumn;

                FillDropDown();
                #endregion

                #region DateTime Convertor

                DateTime? _birthDateFrom = null;
                DateTime? _birthDateTo = null;
                DateTime? _registerDateFrom = null;
                DateTime? _registerDateTo = null;
                DateTime? _startActivityDateFrom = null;
                DateTime? _startActivityDateTo = null;


                if (!string.IsNullOrEmpty(birthDateFrom))
                {
                    _birthDateFrom = await birthDateFrom.ToConvertPersianDateToDateTimeAsync(DateTimeFormat
                        .yyyy_mm_dd, DateTimeSpiliter.slash);
                }
                if (!string.IsNullOrEmpty(birthDateTo))
                {
                    _birthDateTo = await birthDateTo.ToConvertPersianDateToDateTimeAsync(DateTimeFormat.yyyy_mm_dd, DateTimeSpiliter.slash);
                }
                if (!string.IsNullOrEmpty(registerDateFrom))
                {
                    _registerDateFrom = await registerDateFrom.ToConvertPersianDateToDateTimeAsync(DateTimeFormat.yyyy_mm_dd, DateTimeSpiliter.slash);
                }
                if (!string.IsNullOrEmpty(registerDateTo))
                {
                    _registerDateTo = await registerDateTo.ToConvertPersianDateToDateTimeAsync(DateTimeFormat.yyyy_mm_dd, DateTimeSpiliter.slash);
                }
                if (!string.IsNullOrEmpty(startActivityDateFrom))
                {
                    _startActivityDateFrom = await startActivityDateFrom.ToConvertPersianDateToDateTimeAsync(DateTimeFormat.yyyy_mm_dd, DateTimeSpiliter.slash);
                }
                if (!string.IsNullOrEmpty(startActivityDateTo))
                {
                    _startActivityDateTo = await startActivityDateTo.ToConvertPersianDateToDateTimeAsync(DateTimeFormat.yyyy_mm_dd, DateTimeSpiliter.slash);
                }

                #endregion

                Tuple<IQueryable<UserIdentity>, ResultStatusOperation> result = await
                    userIdentityService.SpecificationGetData(
                        id, userName, phoneNumber, firstName, lastName, nationalCode,
                        ismale, islocked, isconfirm,
                        _birthDateFrom, _birthDateTo, _registerDateFrom, _registerDateTo, fatherName,
                        _startActivityDateFrom, _startActivityDateTo, roleNames);

                SetMessage(result.Item2);

                return View(await userIdentityService.Paginationasync(result.Item1, true, pageNumber, pageSize, isDesc, sortColumn));

            }
            catch (Exception exception)
            {
                SetMessageException(new ResultStatusOperation("", "", Enumeration.MessageTypeResult.Danger, false, exception), Enumeration.MessageTypeActionMethod.Index);
                return RedirectToAction("ShowException", "Error", new { area = "" });
            }
        }

        public IActionResult Create()
        {
            FillDropDown();
            return View(new UserIdentityViewModel() { Id = Guid.NewGuid().ToString(), UserRegistrarId = Guid.NewGuid().ToString() });
        }


        [HttpPost]
        public async Task<IActionResult> Create(
            UserIdentityViewModel viewModel,
            string? birthDateString = "",
            string? birthDateTime = "")
        {
            try
            {
                //string filename1 = DateTime.Now.ToString("yyyy/MM/dd-HH:mm:ss:ffffff").Replace("/", "-").Replace(" ", "").Replace(":", "-") + "." + (file1 != null ? file1.FileName.Split(".").Last() : "");

                viewModel.UserRegistrarId = GetUserId();

                Tuple<UserIdentity, ResultStatusOperation> resultFillModel = await
                    userIdentityService.ConvertViewModelToModel(viewModel);

                #region DateTime Convertor

                if (!string.IsNullOrEmpty(birthDateString))
                {
                    //DateTime _birthDate = birthDateString.ToConvertPersianDateToDateTime(Enumeration.DateTimeFormat.yyyy_mm_dd, Enumeration.DateTimeSpiliter.slash);
                    //TimeSpan _birthDateTime = birthDateTime.ToConvertStringToTime();
                    //model.BirthDate = _birthDate.MergeDateAndTime(_birthDateTime);

                    resultFillModel.Item1.BirthDate = birthDateString.ToConvertPersianDateToDateTime(Enumeration.DateTimeFormat.yyyy_mm_dd, Enumeration.DateTimeSpiliter.slash);
                }

                #endregion

#pragma warning disable CS8604 // Possible null reference argument.
                Tuple<UserIdentity, ResultStatusOperation> resultRegister = await
                    userIdentityService.SignUpUser(fillControllerInfo("UserRegistrar"), resultFillModel.Item1, viewModel.Password);

                switch (resultRegister.Item2.Type)
                {
                    case MessageTypeResult.Success:
                        {

                            #region CharityUserIdentityCharityHelper

                            CharityUserIdentityCharityHelper charityUserIdentityCharityHelper = new CharityUserIdentityCharityHelper()
                            {
                                HelperId = resultRegister.Item1.Id,
                                UserIdentityId = GetUserId(),
                                RegisterDate = DateTime.Now,
                                IsDelete = false
                            };

                            await charityUserIdentityCharityHelperService.Insert(fillControllerInfo(new List<string> { "UserIdentity", "Helper" }), charityUserIdentityCharityHelper);

                            #endregion


                            IdentityResult removeRolesResult = await userIdentityService.RemoveAllRolesOfUser(resultRegister.Item1.Id);


                            if (removeRolesResult.Succeeded && viewModel.RoleNames != null && viewModel.RoleNames.Count() != 0)
                            {
                                List<RoleName> roleNames = userIdentityService.FindRoleName(viewModel.RoleNames.ToList());
                                await userIdentityService.AddUserRoles(fillControllerInfo(), resultRegister.Item1, roleNames);
                            }

                            SetMessage(resultRegister.Item2);

                            if (User.IsInRole("Clerk"))
                            {
                                return RedirectToAction("GetClerkAllHelper", "CharityUserIdentityCharityHelper", new { area = "Charity" });
                            }
                            else if (User.IsInRole("Foreman"))
                            {
                                return RedirectToAction("GetForemanAllClerk", "CharityUserIdentityCharityHelper", new { area = "Charity" });

                            }

                            return RedirectToAction(nameof(Index));
                        }
                    case MessageTypeResult.Danger:
                    case MessageTypeResult.Warning:
                        {
                            SetMessage(resultRegister.Item2);
                            FillDropDown();
                            return View(viewModel);
                        }

                    default:
                        return RedirectToAction(nameof(Index));
                }

            }
            catch (Exception exception)
            {
                SetMessageException(new ResultStatusOperation("", "", Enumeration.MessageTypeResult.Danger, false, exception), Enumeration.MessageTypeActionMethod.Index);
                return RedirectToAction("ShowException", "Error", new { area = "" });
            }
        }


        [HttpGet]
        public async Task<IActionResult> Edit(string id = "")
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    SetMessageEditnotFound();
                    return RedirectToAction("Index");
                }
                Tuple<IQueryable<UserIdentity>, ResultStatusOperation> resultFindModel = userIdentityService.SpecificationGetData(id, "", "").Result;

                SetMessage(resultFindModel.Item2);
                switch (resultFindModel.Item2.Type)
                {
                    case MessageTypeResult.Success:
                        {
                            //Pass Parameter like ViewBag.cityId = resultFindModel.Item1.CityId;
                            Tuple<UserIdentityViewModel, ResultStatusOperation> resultViewModel =
                                await userIdentityService.ConvertModelToViewModel(resultFindModel.Item1.FirstOrDefault());

                            ViewBag.roleNames = userIdentityService.GetAllRolesOfUser(resultViewModel.Item1.Id).Result.ToList();

                            FillDropDown();

                            if (User.IsInRole("Clerk") || User.IsInRole("Foreman"))
                            {
                                return View("EditProfile", resultViewModel.Item1);
                            }

                            return View(resultViewModel.Item1);
                        }
                    case Enumeration.MessageTypeResult.Danger:
                        {
                            SetMessageException(resultFindModel.Item2, Enumeration.MessageTypeActionMethod.EditGet);
                            return RedirectToAction("ShowException", "Error");
                        }
                    case MessageTypeResult.Warning:
                        return RedirectToAction("Index");

                }
                SetMessage(resultFindModel.Item2);
                return RedirectToAction("Index");
            }
            catch (Exception exception)
            {
                SetMessageException(new ResultStatusOperation("", "", Enumeration.MessageTypeResult.Danger, false, exception), Enumeration.MessageTypeActionMethod.Index);
                return RedirectToAction("ShowException", "Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            UserIdentityViewModel viewModel,
            string? birthDateString = "",
            string? birthDateTime = "")
        {
            try
            {

                viewModel.UserRegistrarId = GetUserId();

                if (!User.IsInRole("Admin"))
                {
                    //viewModel.RoleNames = new List<string> { "CCD753BE-CC88-4E6F-A63C-F98AEC11F72B" };
                    viewModel.RoleNames = userIdentityService.GetAllRolesIdOfUser(userIdentityService.GetAllRolesOfUser(viewModel.Id).Result);
                    fillControllerInfo("RoleNames");
                }

                Tuple<UserIdentity, ResultStatusOperation> resultViewModelToModel =
                    await userIdentityService.ConvertViewModelToModel(viewModel);

                #region DateTime Convertor

                if (!string.IsNullOrEmpty(birthDateString))
                {
                    //DateTime _birthDate = birthDateString.ToConvertPersianDateToDateTime(Enumeration.DateTimeFormat.yyyy_mm_dd, Enumeration.DateTimeSpiliter.slash);
                    //TimeSpan _birthDateTime = birthDateTime.ToConvertStringToTime();
                    //model.BirthDate = _birthDate.MergeDateAndTime(_birthDateTime);

                    resultViewModelToModel.Item1.BirthDate = birthDateString.ToConvertPersianDateToDateTime(Enumeration.DateTimeFormat.yyyy_mm_dd, Enumeration.DateTimeSpiliter.slash);
                }

                #endregion

                Tuple<UserIdentity, ResultStatusOperation> resultEdit =
                    await userIdentityService.UpdateUser(fillControllerInfo(new List<string> { "UserRegistrar", "UserRegistrarId" }), resultViewModelToModel.Item1, resultViewModelToModel.Item1.Password);

                SetMessage(resultEdit.Item2);
                switch (resultEdit.Item2.Type)
                {
                    case MessageTypeResult.Success:

                        IdentityResult removeRolesResult = await userIdentityService.RemoveAllRolesOfUser(resultEdit.Item1.Id);


                        if (removeRolesResult.Succeeded && viewModel.RoleNames != null && viewModel.RoleNames.Count() != 0)
                        {
                            List<RoleName> roleNames = userIdentityService.FindRoleName(viewModel.RoleNames.ToList());
                            await userIdentityService.AddUserRoles(fillControllerInfo(), resultEdit.Item1, roleNames);
                        }

                        //if (User.IsInRole("Clerk") || User.IsInRole("Foreman"))
                        //{
                        //    return RedirectToAction(nameof(EditProfile));
                        //}

                        return Redirect("/");

                    case MessageTypeResult.Danger:
                    case MessageTypeResult.Warning:
                        Tuple<UserIdentityViewModel, ResultStatusOperation> resultViewModel =
                                await userIdentityService.ConvertModelToViewModel(resultEdit.Item1);

                        ViewBag.roleNames = userIdentityService.GetAllRolesOfUser(resultViewModel.Item1.Id).Result.ToList();

                        FillDropDown();
                        return View(resultEdit.Item1);

                    default:
                        return Redirect("/");
                }
            }
            catch (Exception exception)
            {
                SetMessageException(new ResultStatusOperation("", "", MessageTypeResult.Danger, false, exception), Enumeration.MessageTypeActionMethod.Index);
                return RedirectToAction("ShowException", "Error");
            }
        }



        [AllowAnonymous]
        public ActionResult Logout(string backUrl)
        {
            userIdentityService.LogoutAsync();
            RemoveCookie("Username");
            RemoveCookie("Aban.Identity");

            return Redirect(identityConfiguration.Value.AfterLogOutPath);
        }


        [AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {

            TempData["returnUrl"] = returnUrl;
            CreateAddressView(ref View, ViewAddress, nameof(Login));
            if (string.IsNullOrWhiteSpace(GetUserId()))
            {
                string viewDirectoryName = pathsConfiguratin.Value.ViewDirectoryName;
                SetViewDirectoryCookie(viewDirectoryName);
                if (viewDirectoryName != "CounterOffice")
                {
                    return View();
                }
                return View(View);
            }
            else
            {
                return Redirect("/Home/Index");
                //return RedirectToAction("Index", "Home", new { area = "ISTA" });
            }
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        //[MvcApplication.HandleAntiForgeryError]
        public async Task<ActionResult> Login(LoginViewModel viewModel, string? returnUrl = "")
        {
            TempData["returnUrl"] = returnUrl;
            CreateAddressView(ref View, ViewAddress, nameof(Login));

            bool isPersistent = identityConfiguration.Value.isPersistent;

            Tuple<LoginViewModel, ResultStatusOperation> model =
                await userIdentityService.SigninUser(fillControllerInfo(), viewModel, TypeOfLoginMethod.isLoginByUserNamePasword, isPersistent);


            SetMessage(model.Item2);

            if (!model.Item2.IsSuccessed)
            {
                ViewBag.Message = model.Item2.Message;
                string viewDirectoryName = pathsConfiguratin.Value.ViewDirectoryName;
                if (viewDirectoryName != "CounterOffice")
                {
                    return View(model.Item1);
                }
                return View(View, model.Item1);
            }

#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            UserIdentity user =
                userIdentityService.SpecificationGetData(model.Item1.Id).Result.Item1.FirstOrDefault();
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

#pragma warning disable CS8602 // Dereference of a possibly null reference.
            SetCookie("Username", $"{user.FirstName} {user.LastName}", ExpireTimeType.Days, 500);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            SetUserId(model.Item1.Id);

            if (returnUrl == null)
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                returnUrl = Url.Action("Index", "Home");
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

            return Redirect("/Home/Index");

        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult GetUserRegistrarInfo(string userId)
        {
            if (!userId.IsNullOrEmpty())
            {
                UserIdentity userIdentity = userIdentityService.Find(userId).Result.Item1;
                List<string> userIdentityRoleNames = new List<string>();
                List<string> userRegistrarRoleNames = new List<string>();

                if (userIdentity != null)
                {
                    #region UserIdentity

                    string userIdentityFullname = $"{userIdentity.FirstName} {userIdentity.LastName}";
                    if (!userIdentity.FatherName.IsNullOrEmpty())
                        userIdentityFullname += $" | فرزند: {userIdentity.FatherName}";


                    userIdentityRoleNames = userIdentityService.GetAllRoles(userIdentityService.GetAllRolesOfUser(userId).Result.ToList(), RoleName.Admin)
                        .Where(x => x.Selected).Select(x => x.Text).ToList();
                    #endregion


                    #region UserRegistrar

                    UserIdentity userRegistrar = userIdentityService.Find(userIdentity.UserRegistrarId).Result.Item1;
                    userRegistrarRoleNames = userIdentityService.GetAllRoles(userIdentityService.GetAllRolesOfUser(userRegistrar.Id).Result.ToList(), RoleName.Admin)
                        .Where(x => x.Selected).Select(x => x.Text).ToList();

                    string userRegistrarFullName = $"{userRegistrar.FirstName} {userRegistrar.LastName}";
                    if (!userRegistrar.FatherName.IsNullOrEmpty())
                        userRegistrarFullName += $" | فرزند: {userRegistrar.FatherName}";
                    #endregion

                    return Json(new
                    {
                        isSuccess = true,

                        userIdentityFullname = userIdentityFullname,
                        userIdentityRoleNames = userIdentityRoleNames,

                        userRegistrarFullName = userRegistrarFullName,
                        userRegistrarRoleNames = userRegistrarRoleNames
                    });

                }
            }
            return Json(new { isSuccess = false, result = "مشخصات کاربر موردنظر یافت نشد" });
        }


        public IActionResult EditProfile()
        {
            return RedirectToAction(nameof(Edit), new { Id = GetUserId() });
        }


        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult SetUserForUser()
        {
            ViewBag.userIdentityList1 = userIdentityService.ReadAllWithFatherName("", true, false, new List<RoleName>() { RoleName.Foreman, RoleName.Clerk });
            ViewBag.userIdentityList2 = userIdentityService.ReadAllWithFatherName("", true, false, new List<RoleName>() { RoleName.Clerk, RoleName.Helper });

            return View();
        }


        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult SetUserForUser(string userId1, string userId2)
        {
            try
            {
                if (!userId1.IsNullOrEmpty() && !userId2.IsNullOrEmpty())
                {
                    UserIdentity user1 = userIdentityService.Find(userId1).Result.Item1;
                    if (user1 != null)
                    {
                        UserIdentity user2 = userIdentityService.Find(userId2).Result.Item1;
                        if (user2 != null)
                        {
                            user2.UserRegistrarId = user1.Id;

                            if (string.IsNullOrEmpty(user2.Email))
                                user2.Email = user2.UserName + "@email.com";

                            Tuple<UserIdentity, ResultStatusOperation> resultEdit =
                                userIdentityService.UpdateUser(fillControllerInfo(new List<string> { "UserRegistrar" }), user2, user2.Password).Result;


                            SetMessage(resultEdit.Item2);
                            switch (resultEdit.Item2.Type)
                            {
                                case MessageTypeResult.Success:
                                    return Redirect("/");

                                case MessageTypeResult.Danger:
                                case MessageTypeResult.Warning:
                                    return RedirectToAction(nameof(SetUserForUser));

                                default:
                                    return Redirect("/");
                            }
                        }

                    }
                }

                return RedirectToAction(nameof(SetUserForUser));

            }
            catch (Exception exception)
            {
                SetMessageException(new ResultStatusOperation("", "", MessageTypeResult.Danger, false, exception), Enumeration.MessageTypeActionMethod.Index);
                return RedirectToAction("ShowException", "Error");
            }
        }


        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(UserIdentity model)
        {
            return View();
        }


        private void FillDropDown()
        {
            try
            {
                string userId = ViewBag.id;
                List<string> roleNames = ViewBag.roleNames ?? new List<string>();

                ViewBag.userIdentityList = userIdentityService.ReadAll(userId);
                RoleName roleName = RoleName.Admin;

                //TODO: رل ها با آیدی هایشان نشان داده می شوند در صفحه ی Create
                if (User.IsInRole("Clerk"))
                {
                    roleName = RoleName.Clerk;
                }
                else if (User.IsInRole("Foreman"))
                {
                    roleName = RoleName.Foreman;
                }
                ViewBag.rolesList = userIdentityService.GetAllRoles(roleNames, roleName);

            }
            catch (Exception exception)
            {
                SetMessageException(new ResultStatusOperation("", "", Enumeration.MessageTypeResult.Danger, false, exception), Enumeration.MessageTypeActionMethod.Index);
            }
        }


        private void SetViewDirectoryCookie(string viewDirectoryName)
        {
            RemoveCookie(nameof(viewDirectoryName));
            SetCookie(nameof(viewDirectoryName), viewDirectoryName, ExpireTimeType.Days, 500);
        }
    }
}
