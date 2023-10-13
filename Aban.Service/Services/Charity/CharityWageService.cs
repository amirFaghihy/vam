using Aban.Domain.Entities;
using Aban.Service.Interfaces;
using Aban.DataLayer.Context;
using Microsoft.AspNetCore.Mvc.Rendering;
using Aban.Service.Services.Generic;
using Aban.DataLayer.Interfaces;
using Aban.Domain.Enumerations;
using Microsoft.EntityFrameworkCore;
using Aban.ViewModels;
using static Aban.Domain.Enumerations.Enumeration;
using Aban.Common;
using Aban.DataLayer.Repositories;

namespace Aban.Service.Services
{
    public class CharityWageService : GenericService<CharityWage>, ICharityWageService
    {
        private readonly ICharityWageRepository charityWageRepository;
        private readonly ICharityAdditionRepository charityAdditionRepository;
        private readonly ICharityDeducationRepository charityDeducationRepository;
        private readonly ICharityLoanRepository charityLoanRepository;
        private readonly ICharityLoanInstallmentsRepository charityLoanInstallmentsRepository;
        private readonly ICharityDepositRepository charityDepositRepository;
        private readonly ICharityWageCharityAdditionRepository charityWageCharityAdditionRepository;
        private readonly ICharityWageCharityDeductionRepository charityWageCharityDeductionRepository;
        private readonly ICharityWageCharityDepositRepository charityWageCharityDepositRepository;
        private readonly IUserIdentityRepository userIdentityRepository;
        private readonly ICharityWageCharityLoanInstallmentRepository charityWageCharityLoanInstallmentRepository;

        public CharityWageService(AppDbContext _dbContext) : base(_dbContext)
        {
            charityWageRepository = new CharityWageRepository(_dbContext);
            charityAdditionRepository = new CharityAdditionRepository(_dbContext);
            charityDeducationRepository = new CharityDeducationRepository(_dbContext);
            charityLoanRepository = new CharityLoanRepository(_dbContext);
            charityLoanInstallmentsRepository = new CharityLoanInstallmentsRepository(_dbContext);
            charityDepositRepository = new CharityDepositRepository(_dbContext);
            charityWageCharityAdditionRepository = new CharityWageCharityAdditionRepository(_dbContext);
            charityWageCharityDeductionRepository = new CharityWageCharityDeductionRepository(_dbContext);
            charityWageCharityDepositRepository = new CharityWageCharityDepositRepository(_dbContext);
            userIdentityRepository = new UserIdentityRepository(_dbContext);
            charityWageCharityLoanInstallmentRepository = new CharityWageCharityLoanInstallmentRepository(_dbContext);
        }


        public Tuple<IQueryable<CharityWage>, ResultStatusOperation> SpecificationGetData(
            string userIdentityId = "",
            string wageReceiverId = "",
            string accountNumber = "",
            string description = "",
            float? fixedSalary = null,
            byte? percentSalary = null,
            DateTime? registerDateFrom = null,
            DateTime? registerDateTo = null,
            DateTime? wageDateFrom = null,
            DateTime? wageDateTo = null)
        {
            ResultStatusOperation resultStatusOperation = new ResultStatusOperation();
            resultStatusOperation.IsSuccessed = true;
            resultStatusOperation.Type = Enumeration.MessageTypeResult.Success;
            try
            {
                IQueryable<CharityWage> query = charityWageRepository.GetAll();

                if (!string.IsNullOrEmpty(userIdentityId))
                {
                    query = query.Where(x => x.UserIdentityId == userIdentityId);
                }
                if (!string.IsNullOrEmpty(wageReceiverId))
                {
                    query = query.Where(x => x.WageReceiverId == wageReceiverId);
                }
                if (!string.IsNullOrEmpty(accountNumber))
                {
                    query = query.Where(x => x.AccountNumber.Contains(accountNumber));
                }
                if (!string.IsNullOrEmpty(description))
                {
                    query = query.Where(x => x.Description.Contains(description));
                }
                if (fixedSalary != null)
                {
                    query = query.Where(x => x.FixedSalary == fixedSalary);
                }
                if (percentSalary != null)
                {
                    query = query.Where(x => x.PercentSalary == percentSalary);
                }
                if (registerDateFrom != null || registerDateTo != null)
                {
                    DateTime _registerDateFrom = registerDateFrom != null ? registerDateFrom.Value : new DateTime(1930, 1, 1, 1, 0, 0, 0);
                    DateTime _registerDateTo = registerDateTo != null ? registerDateTo.Value : new DateTime(DateTime.Now.AddYears(1).Year, 1, 1, 1, 0, 0, 0);
                    query = query.Where(x => x.RegisterDate >= _registerDateFrom && x.RegisterDate <= _registerDateTo);
                }
                if (wageDateFrom != null || wageDateTo != null)
                {
                    DateTime _wageDateFrom = wageDateFrom != null ? wageDateFrom.Value : new DateTime(1930, 1, 1, 1, 0, 0, 0);
                    DateTime _wageDateTo = wageDateTo != null ? wageDateTo.Value : new DateTime(DateTime.Now.AddYears(1).Year, 1, 1, 1, 0, 0, 0);
                    query = query.Where(x => x.WageDateFrom >= _wageDateFrom && x.WageDateTo <= _wageDateTo);
                }

                query = query
                    .Include(x => x.UserIdentity)
                    .Include(x => x.WageReceiver);

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

        /// <summary>
        /// در این قسمت فقط جهت نمایش اطلاعات ویومدل پُر میشود
        /// </summary>
        /// <param name="users"></param>
        /// <param name="deposits"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public Tuple<List<CharityUserIdentityDepositViewModel>, ResultStatusOperation> ConvertModelToViewModel(
            List<UserIdentity> users,
            List<RoleName> roleName,
            List<CharityDeposit>? deposits = null,
            List<CharityWage>? wages = null,
            DateTime? startDate = null,
            DateTime? endDate = null,
            string roleId = "")
        {
            ResultStatusOperation resultStatusOperation = new ResultStatusOperation();
            resultStatusOperation.IsSuccessed = true;
            resultStatusOperation.Type = Enumeration.MessageTypeResult.Success;
            try
            {


#pragma warning disable CS8629 // Nullable value type may be null.
                List<CharityUserIdentityDepositViewModel> charityUserIdentityDepositViewModel =
                    users.Select(x => new CharityUserIdentityDepositViewModel
                    {
                        UserIdentityId = x.Id,
                        FirstName = x.FirstName,
                        LastName = x.LastName,
                        FixedSalary = x.FixedSalary,
                        PercentSalary = x.PercentSalary,
                        FatherName = x.FatherName,
                        WageDateFrom = startDate.Value,
                        WageDateTo = endDate.Value,
                        RoleId = roleId
                    }).ToList();
#pragma warning restore CS8629 // Nullable value type may be null.


                if (roleName.FirstOrDefault() == RoleName.Clerk)
                {
                    // جمع واریزیهای هر منشی
                    foreach (var item in charityUserIdentityDepositViewModel)
                    {
#pragma warning disable CS8604
                        item.TotalOfDeposits = deposits.Where(x => x.UserIdentityId == item.UserIdentityId)
                            .Select(x => x.Amount).ToList().Sum();
#pragma warning restore CS8604
                    }
                }
                else if (roleName.FirstOrDefault() == RoleName.Foreman)
                {
                    // جمع واریزیهای منشی های هر سرکاربر
                    foreach (var item in charityUserIdentityDepositViewModel)
                    {
                        IQueryable<string> clerkIds = userIdentityRepository.GetAll().Where(x => x.UserRegistrarId == item.UserIdentityId).Select(x => x.Id);

#pragma warning disable CS8604
                        item.TotalOfDeposits = wages.Where(x => clerkIds.Contains(x.WageReceiverId))
                            .Select(x => x.SumOfDeposits).ToList().Sum();
#pragma warning restore CS8604
                    }
                }


                return Tuple.Create(charityUserIdentityDepositViewModel, resultStatusOperation);
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
        /// در این قسمت جمع پرداختیهای کاربر، افزودن اضافات، کسر کسورات، کسر قسط وام برای هر کاربر محاسبه
        /// و در ویومدل ریخته میشود
        /// </summary>
        /// <param name="users"></param>
        /// <param name="deposits"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public Tuple<List<CharityUserIdentityDepositViewModel>, ResultStatusOperation> ConvertModelToViewModelForSet(
            string userIdentityId,
            List<UserIdentity> users,
            List<RoleName> roleName,
            List<CharityDeposit>? deposits = null,
            List<CharityWage>? wages = null,
            DateTime? startDate = null,
            DateTime? endDate = null)
        {
            ResultStatusOperation resultStatusOperation = new ResultStatusOperation();
            resultStatusOperation.IsSuccessed = true;
            resultStatusOperation.Message = "عملیات با موفقیت انجام شد.";
            resultStatusOperation.Type = Enumeration.MessageTypeResult.Success;
            try
            {

                List<CharityAddition> allCharityAdditions = new List<CharityAddition>();
                List<CharityDeducation> allCharityDeducations = new List<CharityDeducation>();
                List<CharityLoanInstallments> allCharityLoanInstallments = new List<CharityLoanInstallments>();
                List<CharityLoan> allCharityLoans = new List<CharityLoan>();
                List<CharityWageCharityAddition> allCharityWageCharityAdditions = new List<CharityWageCharityAddition>();
                List<CharityWageCharityDeduction> allCharityWageCharityDeduction = new List<CharityWageCharityDeduction>();
                List<CharityWageCharityDeposit> allCharityWageCharityDeposit = new List<CharityWageCharityDeposit>();
                List<CharityWageCharityLoanInstallment> allCharityWageCharityLoanInstallment = new List<CharityWageCharityLoanInstallment>();

#pragma warning disable CS8629
                List<CharityUserIdentityDepositViewModel> charityUserIdentityDepositViewModel =
                    users.Select(x => new CharityUserIdentityDepositViewModel
                    {
                        UserIdentityId = x.Id,
                        FirstName = x.FirstName,
                        LastName = x.LastName,
                        FixedSalary = x.FixedSalary,
                        PercentSalary = x.PercentSalary,
                        FatherName = x.FatherName,
                        WageDateFrom = startDate.Value,
                        WageDateTo = endDate.Value
                    }).ToList();
#pragma warning restore CS8629


                // لیست تمامی اضافات کاربران
                List<CharityAddition> additions = ListOfAdditions(charityUserIdentityDepositViewModel);

                // لیست تمامی کسورات سررسید شده کاربران
                List<CharityDeducation> deducations = ListOfDeducations(charityUserIdentityDepositViewModel);

                // لیست تمامی اقساط سررسید شده پرداخت نشده کاربران
                List<CharityLoanInstallments> loanInstallments = ListOfLoanInstallments(charityUserIdentityDepositViewModel);

                // جمع واریزیها، افزودن اضافات، کسر کسورات، کسر قسط وام سررسید شده هر کاربر
                foreach (var item in charityUserIdentityDepositViewModel)
                {
                    if (roleName.FirstOrDefault() == RoleName.Clerk)
                    {
#pragma warning disable CS8604
                        item.TotalOfDeposits = TotalOfDepositsClerk(deposits, item);
#pragma warning restore CS8604
                    }
                    else if (roleName.FirstOrDefault() == RoleName.Foreman)
                    {
#pragma warning disable CS8604
                        item.TotalOfDeposits = TotalOfDepositsForeman(wages, item);
#pragma warning disable CS8604
                    }

                    List<CharityAddition>? charityAddition = additions.Where(x => x.UserIdentityReciverId == item.UserIdentityId).ToList();
                    List<CharityDeducation>? deducation = deducations.Where(x => x.UserIdentityReciverId == item.UserIdentityId).ToList();
                    List<CharityLoanInstallments>? loanInstallment = loanInstallments.Where(x => x.CharityLoan?.LoanReceiverId == item.UserIdentityId).ToList();

                    if (charityAddition != null && charityAddition.Count() != 0)
                    {
                        //item.TotalOfDeposits += charityAddition.Select(x => x.Amount).Sum();
                        item.CharityAdditions = charityAddition;
                        allCharityAdditions.AddRange(charityAddition);
                    }

                    if (deducation != null && deducation.Count() != 0)
                    {
                        //item.TotalOfDeposits -= deducation.Select(x => x.Amount).Sum();
                        item.CharityDeducations = deducation;
                        allCharityDeducations.AddRange(deducation);
                    }

                    if (loanInstallment != null && loanInstallment.Count() != 0)
                    {
                        //item.TotalOfDeposits -= loanInstallment.Select(x => x.InstallmentAmount).Sum();
                        item.CharityLoanInstallments = loanInstallment;
                        allCharityLoanInstallments.AddRange(loanInstallment);

                        if (loanInstallment.Count() == 1)
                        {
                            item.CharityLoan = loanInstallment?.FirstOrDefault()?.CharityLoan;
#pragma warning disable CS8604
                            allCharityLoans.Add(item.CharityLoan);
#pragma warning restore CS8604
                        }
                    }
                }

                List<CharityWage> charityWages = ConvertViewModelToModel(userIdentityId, charityUserIdentityDepositViewModel);
                charityWages = this.UpdateRange(true, charityWages, true).Result.Item1.ToList();

                foreach (var item in charityWages)
                {

                    List<CharityWageCharityAddition> charityWageCharityAdditions = new List<CharityWageCharityAddition>();
                    List<CharityAddition> charityAdditions = allCharityAdditions.Where(x => x.UserIdentityReciverId == item.WageReceiverId).ToList();
                    if (charityAdditions != null && charityAdditions.Count() != 0)
                    {
                        foreach (var itemCharityAdditions in charityAdditions)
                        {
                            charityWageCharityAdditions.Add(new CharityWageCharityAddition() { CharityAdditionId = itemCharityAdditions.Id, CharityWageId = item.Id, IsDelete = false });
                        }
                        allCharityWageCharityAdditions.AddRange(charityWageCharityAdditions);
                    }

                    List<CharityWageCharityDeduction> charityWageCharityDeductions = new List<CharityWageCharityDeduction>();
                    List<CharityDeducation> charityDeducations = allCharityDeducations.Where(x => x.UserIdentityReciverId == item.WageReceiverId).ToList();
                    if (charityDeducations != null && charityDeducations.Count() != 0)
                    {
                        foreach (var itemCharityDeducations in charityDeducations)
                        {
                            charityWageCharityDeductions.Add(new CharityWageCharityDeduction() { CharityDeducationId = itemCharityDeducations.Id, CharityWageId = item.Id, IsDelete = false });
                        }
                        allCharityWageCharityDeduction.AddRange(charityWageCharityDeductions);
                    }

                    List<CharityWageCharityLoanInstallment> charityWageCharityLoanInstallments = new List<CharityWageCharityLoanInstallment>();
                    List<CharityLoanInstallments> charityLoanInstallments = allCharityLoanInstallments.Where(x => x.CharityLoan.LoanReceiverId == item.WageReceiverId).ToList();
                    if (charityLoanInstallments != null && charityLoanInstallments.Count() != 0)
                    {
                        foreach (var itemCharityLoanInstallments in charityLoanInstallments)
                        {
                            charityWageCharityLoanInstallments.Add(new CharityWageCharityLoanInstallment() { CharityLoanInstallmentId = itemCharityLoanInstallments.Id, CharityWageId = item.Id, IsDelete = false });
                        }
                        allCharityWageCharityLoanInstallment.AddRange(charityWageCharityLoanInstallments);
                    }

                    if (roleName.FirstOrDefault() == RoleName.Clerk)
                    {
#pragma warning disable CS8604
                        foreach (var itemDeposit in deposits.Where(x => x.UserIdentityId == item.WageReceiverId))
                        {
                            allCharityWageCharityDeposit.Add(new CharityWageCharityDeposit()
                            {
                                CharityDepositId = itemDeposit.Id,
                                CharityWageId = item.Id,
                                IsDelete = false
                            });
                        }
#pragma warning restore CS8604
                    }

                }


                if (allCharityWageCharityAdditions != null && allCharityWageCharityAdditions.Count() != 0)
                    charityWageCharityAdditionRepository.UpdateRange(allCharityWageCharityAdditions, false);


                if (allCharityWageCharityDeduction != null && allCharityWageCharityDeduction.Count() != 0)
                    charityWageCharityDeductionRepository.UpdateRange(allCharityWageCharityDeduction, false);


                if (allCharityWageCharityLoanInstallment != null && allCharityWageCharityLoanInstallment.Count() != 0)
                    charityWageCharityLoanInstallmentRepository.UpdateRange(allCharityWageCharityLoanInstallment, false);


                if (allCharityWageCharityDeposit != null && allCharityWageCharityDeposit.Count() != 0)
                    charityWageCharityDepositRepository.UpdateRange(allCharityWageCharityDeposit, false);




#pragma warning disable CS8604
                if (deposits.Count() != 0)
                {
                    foreach (var item in deposits)
                        item.IsDone = true;
                    charityDepositRepository.UpdateRange(deposits, false);
                }


                if (allCharityAdditions != null && allCharityAdditions.Count() != 0)
                {
                    allCharityAdditions.ForEach(x => x.IsDone = true);
                    charityAdditionRepository.UpdateRange(allCharityAdditions, false);
                }


                if (allCharityDeducations != null && allCharityDeducations.Count() != 0)
                {
                    allCharityDeducations.ForEach(x => x.IsDone = true);
                    charityDeducationRepository.UpdateRange(allCharityDeducations, false);
                }


                if (allCharityLoanInstallments != null && allCharityLoanInstallments.Count() != 0)
                {
                    allCharityLoanInstallments.ForEach(x => x.IsDone = true);
                    allCharityLoanInstallments.ForEach(x => x.PaymentDate = DateTime.Now);
                    charityLoanInstallmentsRepository.UpdateRange(allCharityLoanInstallments, false);
                }


                if (allCharityLoans != null && allCharityLoans.Count() != 0)
                {
                    allCharityLoans.ForEach(x => x.IsDone = true);
                    charityLoanRepository.UpdateRange(allCharityLoans, false);
                }

                this.SaveChanges();

                return Tuple.Create(charityUserIdentityDepositViewModel, resultStatusOperation);
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
        /// لیست تمامی اضافات کاربران
        /// </summary>
        /// <param name="charityUserIdentityDepositViewModel"></param>
        /// <returns></returns>
        private List<CharityAddition> ListOfAdditions(List<CharityUserIdentityDepositViewModel> charityUserIdentityDepositViewModel)
        {
            return
            charityAdditionRepository.GetAll().Where(x =>
                    charityUserIdentityDepositViewModel.Select(u => u.UserIdentityId).Contains(x.UserIdentityReciverId))
                    .Where(x => !x.IsDelete && x.IsDone != null && !x.IsDone.Value).ToList();
        }

        /// <summary>
        /// لیست تمامی کسورات سررسید شده کاربران
        /// </summary>
        /// <param name="charityUserIdentityDepositViewModel"></param>
        /// <returns></returns>
        private List<CharityDeducation> ListOfDeducations(List<CharityUserIdentityDepositViewModel> charityUserIdentityDepositViewModel)
        {
            return
            charityDeducationRepository.GetAll().Where(x =>
                    charityUserIdentityDepositViewModel.Select(u => u.UserIdentityId).Contains(x.UserIdentityReciverId))
                    .Where(x => !x.IsDelete && x.IsDone != null && !x.IsDone.Value && x.TimeForAction <= DateTime.Now).ToList();
        }

        /// <summary>
        /// لیست تمامی اقساط سررسید شده پرداخت نشده کاربران
        /// </summary>
        /// <param name="charityUserIdentityDepositViewModel"></param>
        /// <returns></returns>
        private List<CharityLoanInstallments> ListOfLoanInstallments(List<CharityUserIdentityDepositViewModel> charityUserIdentityDepositViewModel)
        {
            return
            charityLoanInstallmentsRepository.GetAll()
                    .Where(x =>
                        (charityLoanRepository.GetAll().Where(lo => charityUserIdentityDepositViewModel.Select(u => u.UserIdentityId).Contains(lo.LoanReceiverId))
                        .Where(lo => !lo.IsDelete && !lo.IsDone && lo.PaymentStartDate <= DateTime.Now)).Select(lo => lo.Id).Contains(x.CharityLoanId))
                    .Where(x => !x.IsDelete && !x.IsDone && x.PaymentDue <= DateTime.Now)
                    .Include(x => x.CharityLoan).ToList();
        }

        /// <summary>
        /// جمع واریزیهای تأیید شده منشی
        /// </summary>
        /// <param name="deposits"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        private double TotalOfDepositsClerk(List<CharityDeposit> deposits, CharityUserIdentityDepositViewModel item)
        {
            return
            deposits.Where(x => x.UserIdentityId == item.UserIdentityId)
                            .Select(x => x.Amount).ToList().Sum();
        }

        /// <summary>
        /// جمع واریزیهای منشیهای هر سرکاربر
        /// </summary>
        /// <param name="wages"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        private double TotalOfDepositsForeman(List<CharityWage> wages, CharityUserIdentityDepositViewModel item)
        {
            IQueryable<string> clerkIds = userIdentityRepository.GetAll().Where(x => x.UserRegistrarId == item.UserIdentityId).Select(x => x.Id);

            return
            wages.Where(x => clerkIds.Contains(x.WageReceiverId))
                            .Select(x => x.SumOfDeposits).ToList().Sum();
        }


        public List<CharityWage> ConvertViewModelToModel(
            string userIdentityId,
            List<CharityUserIdentityDepositViewModel> viewModel)
        {
            // مقادیری که از ویومدل آمده است تبدیل به مدل وِیج می شود
            List<CharityWage> model = viewModel.Select(x => new CharityWage
            {
                UserIdentityId = userIdentityId,
                WageReceiverId = x.UserIdentityId,
                AccountNumber = "---",
                Description = "واریز حقوق و سود ماهانه",
                WageDateFrom = x.WageDateFrom,
                WageDateTo = x.WageDateTo,
                ModifiedDate = null,
                RegisterDate = DateTime.Now,
                FixedSalary = x.FixedSalary,
                PercentSalary = x.PercentSalary,
                SumOfDeposits = x.TotalOfDeposits
            }).ToList();

            return model;
        }


        /// <summary>
        /// ساخت فیش حقوقی با اضافاات و کسورات و قسط پرداختی
        /// </summary>
        /// <param name="charityWages"></param>
        /// <returns></returns>
        public List<PaySlipViewModel> ConvertModelToViewModelPaySlip(
            List<CharityWage> charityWages)
        {

#pragma warning disable CS8602
            List<PaySlipViewModel> model = charityWages.Select(x => new PaySlipViewModel
            {
                CharityWageId = x.Id,
                FirstName = x.WageReceiver.FirstName,
                LastName = x.WageReceiver.LastName,
                FatherName = x.WageReceiver.FatherName.IsNullOrEmpty() ? "" : x.WageReceiver.FatherName,
                AccountNumber = x.WageReceiver.CardNumber.IsNullOrEmpty() ? "" : x.WageReceiver.CardNumber,
                Description = "واریز حقوق و سود ماهانه",
                WageDateFrom = x.WageDateFrom,
                WageDateTo = x.WageDateTo,
                FixedSalary = x.FixedSalary,
                PercentSalary = x.PercentSalary,
                TotalOfDeposits = x.SumOfDeposits,
                NationalCode = x.WageReceiver.NationalCode,
                BirthDate = x.WageReceiver.BirthDate,
                UserRegistrar = x.WageReceiver.UserRegistrar
            }).ToList();
#pragma warning restore CS8602

            #region Addition

            IEnumerable<IGrouping<int, CharityWageCharityAddition>> charityWageAdittions = charityWageCharityAdditionRepository.GetAll()
                .Where(x => !x.IsDelete && charityWages.Select(cw => cw.Id).Contains(x.CharityWageId))
                .Include(x => x.CharityAddition)
                .ToList().GroupBy(x => x.CharityWageId);

            foreach (var group in charityWageAdittions)
            {
                List<CharityAddition> charityAdditions = new List<CharityAddition>();

                foreach (var item in group)
                {
                    if (item.CharityAddition != null)
                    {
                        charityAdditions.Add(item.CharityAddition);
                    }
                }

                model.Where(x => x.CharityWageId == group.Key).First().CharityAdditions = charityAdditions;
            }

            #endregion


            #region Deduction

            IEnumerable<IGrouping<int, CharityWageCharityDeduction>> charityDeductions = charityWageCharityDeductionRepository.GetAll()
                .Where(x => !x.IsDelete && charityWages.Select(cw => cw.Id).Contains(x.CharityWageId))
                .Include(x => x.CharityDeducation)
                .ToList().GroupBy(x => x.CharityWageId);

            foreach (var group in charityDeductions)
            {
                List<CharityDeducation> charityDeducations = new List<CharityDeducation>();
                foreach (var item in group)
                {
                    if (item.CharityDeducation != null)
                    {
                        charityDeducations.Add(item.CharityDeducation);
                    }
                }


                // این خط با اینکه مقدار لیست پُر میشود، باز هم خطا میدهد
                model.Where(x => x.CharityWageId == group.Key).First().CharityDeducations = charityDeducations;
            }

            #endregion


            #region Loan

            IEnumerable<IGrouping<int, CharityWageCharityLoanInstallment>> charityLoanInstallments = charityWageCharityLoanInstallmentRepository.GetAll()
                .Where(x => !x.IsDelete && charityWages.Select(cw => cw.Id).Contains(x.CharityWageId))
                .Include(x=>x.CharityLoanInstallments)
                .ToList().GroupBy(x => x.CharityWageId);

            foreach (var group in charityLoanInstallments)
            {
                List<CharityLoanInstallments> installments = new List<CharityLoanInstallments>();
                foreach (var item in group)
                {
                    if (item.CharityLoanInstallments != null)
                    {
                        installments.Add(item.CharityLoanInstallments);
                    }
                }
                model.Where(x => x.CharityWageId == group.Key).First().CharityLoanInstallments = installments;
            }


            #endregion


            return model;
        }


        public Tuple<CharityWage, ResultStatusOperation> FillModel(CharityWage charityWage)
        {
            ResultStatusOperation resultStatusOperation = new ResultStatusOperation();
            resultStatusOperation.IsSuccessed = true;
            resultStatusOperation.Type = Enumeration.MessageTypeResult.Success;
            try
            {
                charityWage.IsDelete = false;
                charityWage.RegisterDate = DateTime.Now;

                return Tuple.Create(charityWage, resultStatusOperation);
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
                    List<CharityWage> query = charityWageRepository.GetAll()
                        .Where(x => !x.IsDelete)
                        .OrderBy(x => x.Id).ToList();

                    List<SelectListItem> item = query.ConvertAll(x =>
                    {
                        return new SelectListItem()
                        {
                            Text = x.ToString(),// تصمیم گرفته شود که چه فیلدی را نمایش دهد
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

    }
}
