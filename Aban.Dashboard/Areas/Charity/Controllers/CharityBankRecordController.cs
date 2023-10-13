using Aban.Common;
using Aban.Common.Utility;
using Aban.Domain.Entities;
using Aban.Service.Interfaces;
using ExcelDataReader;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Text;
using X.PagedList;
using static Aban.Domain.Enumerations.Enumeration;

namespace Aban.Dashboard.Areas.Charity.Controllers
{
    [Area("Charity")]
    [Authorize(Roles = "Admin")]
    public class CharityBankRecordController : GenericController
    {
        IConfiguration _configuration;
        IWebHostEnvironment _hostEnvironment;
        //ServicingContext context;
        IExcelDataReader reader;
        string fileAddress = @"aban.charity\upload\";
        private readonly ICharityBankRecordService _charityBankRecordService;

        private readonly ICharityAccountService _charityAccountService;
        private readonly ICharityDepositService _charityDepositService;

#pragma warning disable CS8618
        public CharityBankRecordController(
#pragma warning restore CS8618
            IConfiguration configuration,
            IWebHostEnvironment hostEnvironment,
            ICharityBankRecordService charityBankRecordService,
            ICharityAccountService charityAccountService,
            ICharityDepositService charityDepositService
            )
        {
            _charityBankRecordService = charityBankRecordService;
            _configuration = configuration;
            _hostEnvironment = hostEnvironment;
            _charityAccountService = charityAccountService;
            _charityDepositService = charityDepositService;
        }


        public async Task<IActionResult> Index(
            string userIdentityId = "",
            string paperNumber = "",
            string documentNumber = "",
            double creditor = 0,
            double debtor = 0,
            double inventory = 0,
            string? documentRegisterDateTimeFrom = null,
            string? documentRegisterDateTimeTo = null,
            string? registerDateFrom = null,
            string? registerDateTo = null,
            int? charityAccountId = 0,
            string description = "",


            bool isDesc = true,
            int pageNumber = 1,
            int pageSize = 10,
            string sortColumn = "RegisterDate",
            string lastColumn = "",
            int? id = null
            )
        {

            #region selectedValue

            ViewBag.pageNumber = pageNumber;
            ViewBag.pageSize = pageSize;
            ViewBag.sortColumn = sortColumn;
            ViewBag.lastColumn = lastColumn;
            ViewBag.isDesc = isDesc;

            ViewBag.userIdentityId = userIdentityId;
            ViewBag.charityAccountId = charityAccountId;
            ViewBag.paperNumber = paperNumber;
            ViewBag.documentNumber = documentNumber;
            ViewBag.creditor = creditor;
            ViewBag.debtor = debtor;
            ViewBag.inventory = inventory;
            ViewBag.documentRegisterDateTimeFrom = documentRegisterDateTimeFrom;
            ViewBag.documentRegisterDateTimeTo = documentRegisterDateTimeTo;
            ViewBag.registerDateFrom = registerDateFrom;
            ViewBag.registerDateTo = registerDateTo;
            ViewBag.description = description;

            //ViewBag.BrandId = brandId;

            //ViewBag.headerSort = headerSort;  ViewBag.sortColumn 
            //ViewBag.lastHeader = lastHeader;  ViewBag.lastColumn
            //ViewBag.isDesc = isDesc;          ViewBag.isDesc 
            //if (ViewBag.lastColumn == ViewBag.sortColumn)
            //{
            //    ViewBag.isDesc = isDesc == true ? false : true;
            //    isDesc = ViewBag.isDesc;
            //}
            //else
            //{
            //    ViewBag.isDesc = isDesc;
            //}
            ViewBag.lastColumn = ViewBag.sortColumn;

            #endregion


            #region DateTime Convertor

            DateTime? _documentRegisterDateTimeFrom = null;
            DateTime? _documentRegisterDateTimeTo = null;
            DateTime? _registerDateFrom = null;
            DateTime? _registerDateTo = null;


            if (!string.IsNullOrEmpty(documentRegisterDateTimeFrom))
            {
                _documentRegisterDateTimeFrom = await documentRegisterDateTimeFrom.ToConvertPersianDateToDateTimeAsync(DateTimeFormat.yyyy_mm_dd, DateTimeSpiliter.slash);
            }
            if (!string.IsNullOrEmpty(documentRegisterDateTimeTo))
            {
                _documentRegisterDateTimeTo = await documentRegisterDateTimeTo.ToConvertPersianDateToDateTimeAsync(DateTimeFormat.yyyy_mm_dd, DateTimeSpiliter.slash);
            }
            if (!string.IsNullOrEmpty(registerDateFrom))
            {
                _registerDateFrom = await registerDateFrom.ToConvertPersianDateToDateTimeAsync(DateTimeFormat.yyyy_mm_dd, DateTimeSpiliter.slash);
            }
            if (!string.IsNullOrEmpty(registerDateTo))
            {
                _registerDateTo = await registerDateTo.ToConvertPersianDateToDateTimeAsync(DateTimeFormat.yyyy_mm_dd, DateTimeSpiliter.slash);
            }
            FillDropDown(charityAccountId);

            #endregion

            return View(await _charityBankRecordService.Paginationasync(
             _charityBankRecordService.SpecificationGetData(userIdentityId, paperNumber, documentNumber, creditor,
                debtor, inventory, charityAccountId, description, _documentRegisterDateTimeFrom, _documentRegisterDateTimeTo, _registerDateFrom, _registerDateTo
                , id),
            false, pageNumber, pageSize, isDesc, sortColumn));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            ResultStatusOperation _resultStatusOperation = new()
            {
                IsSuccessed = false,
                Title = "خطا",
                Message = "خطا در حذف",
                Type = MessageTypeResult.Danger,
                ErrorException = null
            };

            if (id is null || await _charityDepositService.GetAll().AnyAsync(x => x.CharityBankRecordId == id))
            {
                SetMessage(_resultStatusOperation);
                return RedirectToAction("Index");
            }


            _resultStatusOperation = await _charityBankRecordService.Delete(id.Value);
            SetMessage(_resultStatusOperation);
            return RedirectToAction("Index");
        }

        [HttpGet("/GetDetail")]
        public async Task<IActionResult> GetDetail(int bankRecordId)
        {
            var resultDetail = await _charityDepositService.GetDetailByBankRecordId(bankRecordId);

            return Json(new { clreckName = resultDetail.clreckName, foremanName = resultDetail.foremanName, depositId = resultDetail.depositId });
        }

        public IActionResult Create()
        {
            FillDropDown();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(int charityAccountId, IFormFile file, bool containIssueTrackNumber = false)
        {
            int count = 0;
            var resultAccount = await _charityAccountService.Find(charityAccountId);
            switch (resultAccount.Item2.Type)
            {
                case MessageTypeResult.Success:
                    break;
                case MessageTypeResult.Danger:
                case MessageTypeResult.Warning:
                    FillDropDown(charityAccountId);
                    return View(resultAccount.Item1);
                default:
                    SetMessage(new ResultStatusOperation()
                    {
                        IsSuccessed = false,
                        Title = "حساب یافت نشد",
                        Message = "حساب یافت نشد",
                        Type = MessageTypeResult.Warning
                    });
                    return RedirectToAction(nameof(Index));
            }


            #region Uplaod And Save

            if (file == null)
            {
                SetMessage(new ResultStatusOperation()
                {
                    Message = "فایلی انتخاب نشده است",
                    IsSuccessed = false,
                    Type = MessageTypeResult.Danger,
                    Title = "خطا"
                });
                FillDropDown(charityAccountId);
                return View();
            }

            string dirPath = Path.Combine(_hostEnvironment.WebRootPath, fileAddress);
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }
            string dataFileName = DateTime.Now.ToConvertDateTimeToPersianDateTime(inputDateTimeSpiliter: Domain.Enumerations.Enumeration.DateTimeSpiliter.underline).Replace(":", "_") + "-" + resultAccount.Item1.BankName.ToString() + "-" + Path.GetFileName(file.FileName);
            string dataFileNameError = DateTime.Now.ToConvertDateTimeToPersianDateTime(inputDateTimeSpiliter: Domain.Enumerations.Enumeration.DateTimeSpiliter.underline).Replace(":", "_") + "-" + resultAccount.Item1.BankName.ToString() + "-" + "ERROR" + ".txt";
            string extension = Path.GetExtension(dataFileName);

            string[] allowedExtsnions = new string[] { ".xls", ".xlsx" };

            if (!allowedExtsnions.Contains(extension))
            {
                FillDropDown(charityAccountId);
                SetMessage(new ResultStatusOperation()
                {
                    Message = "فقط فایل اکسل اجازه ی آپلود دارد",
                    IsSuccessed = false,
                    Type = MessageTypeResult.Danger,
                    Title = "خطا"
                });
                return View();
            }

            // Make a Copy of the Posted File from the Received HTTP Request
            string saveToPath = Path.Combine(dirPath, dataFileName);

            using (FileStream stream = new FileStream(saveToPath, FileMode.CreateNew))
            {
                file.CopyTo(stream);
            }


            #endregion


            #region Read File

            using (var stream = new FileStream(saveToPath, FileMode.Open))
            {
                if (extension == ".xls")
                    reader = ExcelReaderFactory.CreateBinaryReader(stream);
                else
                    reader = ExcelReaderFactory.CreateOpenXmlReader(stream);

                DataSet ds = new DataSet();
                ds = reader.AsDataSet();
                reader.Close();
                List<int> rowError = new();
                if (ds != null && ds.Tables.Count > 0)
                {
                    // Read the the Table
                    switch (resultAccount.Item1.BankName)
                    {
                        case BankName.بانک_سپه:
                            count = await LogErrorInExcelFile(dirPath, dataFileNameError, await ConvertDataToSepah(ds, charityAccountId, resultAccount.Item1.BankName));
                            break;
                        case BankName.بانک_ملت:
                            count = await LogErrorInExcelFile(dirPath, dataFileNameError, await ConvertDataToMellat(ds, charityAccountId, resultAccount.Item1.BankName));
                            break;
                        case BankName.بانک_ملی_ایران:
                            count = await LogErrorInExcelFile(dirPath, dataFileNameError, await ConvertDataToMeli(ds, charityAccountId, resultAccount.Item1.BankName));
                            break;


                        case BankName.بانک_صنعت_و_معدن:
                        case BankName.بانک_کشاورزی:
                        case BankName.بانک_مسکن:
                        case BankName.بانک_توسعه_صادرات_ایران:
                        case BankName.بانک_توسعه_تعاون:
                        case BankName.پست_بانک_ایران:
                        case BankName.بانک_اقتصاد_نوین:
                        case BankName.بانک_پارسیان:
                        case BankName.بانک_کارآفرین:
                        case BankName.بانک_سامان:
                        case BankName.بانک_سینا:
                        case BankName.بانک_خاورمیانه:
                        case BankName.بانک_شهر:
                        case BankName.بانک_دی:
                        case BankName.بانک_صادرات:
                        case BankName.بانک_تجارت:
                        case BankName.بانک_رفاه:
                        case BankName.بانک_حکمت_ایرانیان:
                        case BankName.بانک_گردشگردی:
                        case BankName.بانک_ایران_زمین:
                        case BankName.بانک_قوامین:
                        case BankName.بانک_انصار:
                        case BankName.بانک_سرمایه:
                        case BankName.بانک_پاسارگاد:
                        case BankName.بانک_قرض_الحسنه_مهر_ایران:
                        case BankName.بانک_قرض_الحسنه_مهر_رسالت:
                            SetMessage(new ResultStatusOperation()
                            {
                                IsSuccessed = false,
                                Title = "حساب یافت نشد",
                                Message = "حساب یافت نشد",
                                Type = MessageTypeResult.Warning
                            });
                            return RedirectToAction(nameof(Index));

                    }
                }
            }
            #endregion


            string ErrorFileAddress = Path.Combine(fileAddress, dataFileNameError);
            return RedirectToAction(nameof(ShowResult), new
            {
                filePath = ErrorFileAddress,
                ErrorCount = count
            });

        }

        public IActionResult ShowResult(string filePath, int ErrorCount)
        {
            ViewBag.ErrorCount = ErrorCount;
            return View(model: filePath);
        }


        private void FillDropDown(int? accountId = null)
        {
            ViewBag.listCharityAccount = _charityAccountService.SelectListItem(_charityAccountService.GetAll().Where(x => x.IsVisible && !x.IsDelete),
                x => x.BankName + x.Title, x => x.Id, accountId != null ? accountId.Value.ToString() : 0.ToString(), "---انتخاب---");
        }

        //فقط واریز ها رو باید چک کنه برای تکراری نبودن ولی کل رکورد ها باید تو سیستم ذخیره بشه



        private async Task<List<int>> ConvertDataToSepah(DataSet ds, int charityAccountId, BankName bankName)
        {
            int counter = 0;
            List<int> rowError = new();
            DataTable serviceDetails = ds.Tables[0];
            for (int i = 1; i < serviceDetails.Rows.Count; i++)
            {
                try
                {

#pragma warning disable CS8604 // Possible null reference argument.
#pragma warning disable CS8601 // Possible null reference assignment.
                    CharityBankRecord charityBankRecord = new()
                    {
                        DocumentRegisterDateTime = convertDateAndTime2(serviceDetails.Rows[i][0].ToString(),
                    serviceDetails.Rows[i][1].ToString()),

                        Description = i + " -- " + serviceDetails.Rows[i][2].ToString(),
                        Debtor = Convert.ToDouble(serviceDetails.Rows[i][3].ToString()),
                        Creditor = Convert.ToDouble(serviceDetails.Rows[i][4].ToString()),
                        Inventory = Convert.ToDouble(serviceDetails.Rows[i][5].ToString()),

                        PaperNumber = "",
                        DocumentNumber = "",

                        RegisterDate = DateTime.Now,
                        CharityAccountId = charityAccountId
                    };
#pragma warning restore CS8601 // Possible null reference assignment.
#pragma warning restore CS8604 // Possible null reference argument.


                    //await _charityBankRecordService.Insert(true, charityBankRecord);

                    //واریزی باشد
                    if (charityBankRecord.Debtor == 0)
                    {
                        string issueNumberNew = SepahGetIssueNumber(charityBankRecord.Description);
                        string cardNumber = this.GetSepahCardNumber(charityBankRecord.Description);
                        charityBankRecord.DocumentNumber = issueNumberNew;
                        charityBankRecord.Accountant = cardNumber.ReplacePersianNumberToEnglishNumber();

                        //رکورد تکراری نباشد
                        if (!await this.CheckRecordIsExist(issueNumberNew, charityBankRecord.Creditor, charityBankRecord.DocumentRegisterDateTime, charityAccountId) || !await this.CheckRecordIsExistBy_Date_Amount_Card(charityBankRecord.Creditor, charityBankRecord.Accountant, charityBankRecord.DocumentRegisterDateTime, charityAccountId))
                        {


                            var resultAdd = await _charityBankRecordService.Insert(true, charityBankRecord);
                            if (!await this.UpdateConfirmRecordByDateAmountCardNumber(
                                    resultAdd.Item1.Creditor, resultAdd.Item1.Accountant, 
                                    resultAdd.Item1.DocumentRegisterDateTime, resultAdd.Item1.Id, charityAccountId, bankName))
                            {
                                await this.UpdateConfirmRecord(resultAdd.Item1.DocumentNumber, resultAdd.Item1.Creditor, resultAdd.Item1.DocumentRegisterDateTime, resultAdd.Item1.Id, charityAccountId);
                            }


                        }
                        else
                        {
                            counter++;
                            rowError.Add(i);
                        }
                    }
                    //else
                    //{
                    //    await _charityBankRecordService.Insert(true, charityBankRecord);
                    //}

                }
                catch
                {
                    counter++;
                    rowError.Add(i);

                }

            }

            if (counter == 0)
            {
                SetMessage(new ResultStatusOperation()
                {
                    IsSuccessed = false,
                    Title = "اطلاعات با موفقیت ذخیره شد",
                    Message = $" اطلاعات با موفقیت ذخیره شد",
                    Type = MessageTypeResult.Warning
                });

            }
            else
            {
                SetMessage(new ResultStatusOperation()
                {
                    IsSuccessed = false,
                    Title = $" {counter} رکورد ذخیره نشد",
                    Message = $" {counter} رکورد ذخیره نشد",
                    Type = MessageTypeResult.Warning
                });
            }
            return rowError;
        }

        /// <summary>
        /// OK
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="charityAccountId"></param>
        /// <returns></returns>
        private async Task<List<int>> ConvertDataToMeli(DataSet ds, int charityAccountId , BankName bankName)
        {
            int counter = 0;
            List<int> rowError = new();
            DataTable serviceDetails = ds.Tables[0];
            for (int i = 1; i < serviceDetails.Rows.Count; i++)
            {
                try
                {


#pragma warning disable CS8601 // Possible null reference assignment.
#pragma warning disable CS8604 // Possible null reference argument.
                    CharityBankRecord charityBankRecord = new()
                    {
                        PaperNumber = serviceDetails.Rows[i][0].ToString(),//شماره برگه
                        DocumentNumber = serviceDetails.Rows[i][1].ToString(),//شماره سند
                        Creditor = Convert.ToDouble(serviceDetails.Rows[i][2].ToString()),//بستانکار
                        Debtor = Convert.ToDouble(serviceDetails.Rows[i][3].ToString()),//بدهکار
                        Description = i + " -- " + serviceDetails.Rows[i][4].ToString(), //شرح
                        DocumentRegisterDateTime = convertDateAndTime(serviceDetails.Rows[i][5].ToString(),
                    serviceDetails.Rows[i][6].ToString()),
                        Inventory = Convert.ToDouble(serviceDetails.Rows[i][7].ToString()),
                        RegisterDate = DateTime.Now,
                        CharityAccountId = charityAccountId
                    };
#pragma warning restore CS8604 // Possible null reference argument.
#pragma warning restore CS8601 // Possible null reference assignment.


                    string cardNumber = this.GetMelliCardNumber(charityBankRecord.Description);
                    charityBankRecord.Accountant = cardNumber.ReplacePersianNumberToEnglishNumber();

                    //واریزی باشد
                    if (charityBankRecord.Debtor == 0)
                    {
                        if (!await this.CheckRecordIsExistBy_Date_Amount_Card(charityBankRecord.Creditor, charityBankRecord.Accountant, charityBankRecord.DocumentRegisterDateTime, charityAccountId))
                        {

                            var resultAdd = await _charityBankRecordService.Insert(true, charityBankRecord);

                            if (!await this.UpdateConfirmRecordByDateAmountCardNumber(resultAdd.Item1.Creditor,
                                    resultAdd.Item1.Accountant!, resultAdd.Item1.DocumentRegisterDateTime, 
                                    resultAdd.Item1.Id, charityAccountId, bankName))
                            {
                                await this.UpdateConfirmRecord(resultAdd.Item1.DocumentNumber, resultAdd.Item1.Creditor, resultAdd.Item1.DocumentRegisterDateTime, resultAdd.Item1.Id, charityAccountId);


                            }
                        }
                        else
                        {
                            counter++;
                            rowError.Add(i);
                        }
                    }
                    //else
                    //{
                    //    await _charityBankRecordService.Insert(true, charityBankRecord);
                    //}


                }
                catch (Exception exception)
                {

                    counter++;
                    rowError.Add(i);
                }

            }

            if (counter == 0)
            {
                SetMessage(new ResultStatusOperation()
                {
                    IsSuccessed = false,
                    Title = "اطلاعات با موفقیت ذخیره شد",
                    Message = $"اطلاعات با موفقیت ذخیره شد",
                    Type = MessageTypeResult.Warning
                });

            }
            else
            {
                SetMessage(new ResultStatusOperation()
                {
                    IsSuccessed = false,
                    Title = $" {counter} رکورد ذخیره نشد",
                    Message = $" {counter} رکورد ذخیره نشد",
                    Type = MessageTypeResult.Warning
                });
            }
            return rowError;
        }


        private async Task<List<int>> ConvertDataToMellat(DataSet ds, int charityAccountId,BankName bankName)
        {
            int counter = 0;
            //int successCounter = 0; جایی استفاده نمیشد، کامنت شد
            List<int> rowError = new();
            DataTable serviceDetails = ds.Tables[0];
            for (int i = 1; i < serviceDetails.Rows.Count; i++)
            {
                try
                {

                    CharityBankRecord charityBankRecord = new()
                    {
                        RegisterDate = DateTime.Now,
                        CharityAccountId = charityAccountId
                    };
#pragma warning disable CS8601
#pragma warning disable CS8604
                    charityBankRecord.Inventory = Convert.ToDouble(serviceDetails.Rows[i][0].ToString());//مانده
                    charityBankRecord.Creditor = Convert.ToDouble(serviceDetails.Rows[i][1].ToString());//مبلغ گردش بستانکار
                    charityBankRecord.Debtor = Convert.ToDouble(serviceDetails.Rows[i][2].ToString());//مبلغ گردش بدهکار 
                    charityBankRecord.Description = i + " -- " + serviceDetails.Rows[i][3].ToString();//شرح
                    charityBankRecord.Accountant = GetMellatCardNumber(serviceDetails.Rows[i][4].ToString()); // واریز کننده و ذینفع
                    charityBankRecord.DocumentNumber = serviceDetails.Rows[i][5].ToString();//شماره سریال
                    charityBankRecord.BankDepositId = serviceDetails.Rows[i][6].ToString();//شناسه واریز
                    charityBankRecord.PaperNumber = serviceDetails.Rows[i][7].ToString(); // کد حسابگری
                    charityBankRecord.Branch = serviceDetails.Rows[i][8].ToString();// شعبه

                    charityBankRecord.DocumentRegisterDateTime = convertDateAndTime2(
                        serviceDetails.Rows[i][10].ToString(),
                        serviceDetails.Rows[i][9].ToString());
                    //.-_*#
                    //واریزی باشد
                    if (charityBankRecord.Debtor == 0)
                    {
                        //رکورد تکراری نباشد
                        if (!await this.CheckRecordIsExist(charityBankRecord.DocumentNumber, charityBankRecord.Creditor, charityBankRecord.DocumentRegisterDateTime, charityAccountId) || 
                            !await this.CheckRecordIsExistBy_Date_Amount_Card(charityBankRecord.Creditor, charityBankRecord.Accountant, charityBankRecord.DocumentRegisterDateTime, charityAccountId))
                        {
                            var resultAdd = await _charityBankRecordService.Insert(true, charityBankRecord);

                            if (!await this.UpdateConfirmRecordByDateAmountCardNumber(resultAdd.Item1.Creditor,
                                    resultAdd.Item1.Accountant, resultAdd.Item1.DocumentRegisterDateTime,
                                    resultAdd.Item1.Id, charityAccountId, bankName))
                            {
                                //بانک ملت به دلیل ندادن کد پیگیری و تغییر آن ، از بررسی آن صرف نظر شده است 
                                // await this.UpdateConfirmRecord(resultAdd.Item1.DocumentNumber, resultAdd.Item1.Id);
                            }
                        }
                        else
                        {
                            counter++;
                            rowError.Add(i);
                        }
                    }
                    //else
                    //{
                    //    await _charityBankRecordService.Insert(true, charityBankRecord);
                    //}


                }
                catch
                {
                    counter++;
                    rowError.Add(i);
                }

            }

            if (counter == 0)
            {
                SetMessage(new ResultStatusOperation()
                {
                    IsSuccessed = false,
                    Title = "اطلاعات با موفقیت ذخیره شد",
                    Message = $" اطلاعات با موفقیت ذخیره شد",
                    Type = MessageTypeResult.Warning
                });
            }
            else
            {
                SetMessage(new ResultStatusOperation()
                {
                    IsSuccessed = false,
                    Title = $" {counter} رکورد ذخیره نشد",
                    Message = $" {counter} رکورد ذخیره نشد",
                    Type = MessageTypeResult.Warning
                });
            }
            return rowError;
        }



        /// <summary>
        /// Mellat  And Sepah
        /// </summary>
        /// <param name="documentNumber"></param>
        /// <returns></returns>
        private async Task<bool> CheckRecordIsExist(string documentNumber, double amount, DateTime documentDate, int charityAccountId)
            => await _charityBankRecordService.CheckRecordIsExistByIssueNumberAsync(documentNumber, amount, documentDate, charityAccountId);

        private async Task<bool> CheckRecordIsExistBy_Date_Amount_Card(double amount, string cardNumber, DateTime date, int charityAccountId)
            => await _charityBankRecordService.CheckRecordIsExistByDateAmountCardAsync(amount, cardNumber, date, charityAccountId);

        private async Task<bool> UpdateConfirmRecord(string documentNumber, double amount, DateTime documentDate, int bankRecordId, int charityAccountId)
            => await _charityBankRecordService.UpdateConfirmByIssueNumberAsync(documentNumber, amount, documentDate, bankRecordId, charityAccountId);

        private async Task<bool> UpdateConfirmRecordByDateAmountCardNumber(double amount, string cardNumber,
            DateTime date, int bankRecordId, int charityAccountId,BankName bankName)
            => await _charityBankRecordService.UpdateConfirmByDateAmountCardNumberAsync(amount, cardNumber, date,
                bankRecordId, charityAccountId,bankName);




        /// <summary>
        /// Seperate from decription
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        private string GetSepahCardNumber(string description)
        {
            string cardNumber = "";
            try
            {
                string last = "";
                var resultSplit = description.Split("از کارت ");

                foreach (var item in resultSplit.Last())
                {
                    if (char.IsDigit(item))
                    {
                        cardNumber += item;
                    }
                    else
                    {
                        break;
                    }
                }

                return cardNumber;
            }
            catch (Exception e)
            {
                return cardNumber;
            }
        }


        /// <summary>
        /// Seperate from decription
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        private string GetMelliCardNumber(string description)
        {
            string cardNumber = "";
            try
            {
                string last = "";
                var resultSplit = description.Split(" ش ک ");

                foreach (var item in resultSplit.Last())
                {
                    if (char.IsDigit(item))
                    {
                        cardNumber += item;
                    }
                    else
                    {
                        break;
                    }
                }

                return cardNumber;
            }
            catch (Exception e)
            {
                return cardNumber;

            }
        }



        private string GetMellatCardNumber(string cardNumber)
        => cardNumber.Replace(".", "").Replace("-", "").Replace("_", "").Replace("*", "")
            .Replace("#", "").ReplacePersianNumberToEnglishNumber();




        private string SepahGetIssueNumber(string description)
        {
            string issueString = "";
            try
            {
                string last = "";
                var resultSplit = description.Split("پيگيري ");
                if (resultSplit.Length == 1)
                    last = description.Split("پیگیری ").Last();
                else
                    last = resultSplit.Last();


                foreach (var item in last)
                {
                    if (char.IsDigit(item))
                    {
                        issueString += item;
                    }
                    else
                    {
                        break;
                    }
                }

                return issueString;
            }
            catch (Exception e)
            {
                return issueString;
            }

        }



        private string MelliGetIssueNumber(string description)
        {
            string issueString = "";
            try
            {
                string last = "";
                var resultSplit = description.Split("ش پ ");
                if (resultSplit.Length == 1)
                    last = description.Split("پیگیری ").Last();
                else
                    last = resultSplit.Last();


                foreach (var item in last)
                {
                    if (char.IsDigit(item))
                    {
                        issueString += item;
                    }
                    else
                    {
                        break;
                    }
                }

                return issueString;
            }
            catch (Exception e)
            {
                return issueString;

            }

        }



        /// <summary>
        /// BANK MELI DATETIME
        /// </summary>
        /// <param name="date"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        private DateTime convertDateAndTime(string date, string time)
        {
            try
            {
                DateTime oldDate = date.ToConvertPersianDateToDateTime();
                TimeSpan oldTime = time.ToConvertStringToTime();
                return new DateTime(oldDate.Year, oldDate.Month, oldDate.Day, oldTime.Hours, oldTime.Minutes, oldTime.Seconds);
            }
            catch
            {
                throw;
            }

        }

        /// <summary>
        /// BANK SEPAH, MELLAT DATETIME
        /// </summary>
        /// <param name="date"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        private DateTime convertDateAndTime2(string Date, string Time)
        {
            try
            {
                DateTime oldDate = Date.ToConvertPersianDateToDateTime();
                DateTime oldTime = DateTime.Parse(Time);

                return new DateTime(oldDate.Year, oldDate.Month, oldDate.Day, oldTime.TimeOfDay.Hours, oldTime.TimeOfDay.Minutes, oldTime.TimeOfDay.Seconds);
            }
            catch
            {
                throw;
            }

        }



        private async Task<int> LogErrorInExcelFile(string filePath, string fileName, List<int> rowNumber)
        {
            string combine = Path.Combine(filePath, fileName);
            if (System.IO.File.Exists(combine))
            {
                System.IO.File.Delete(combine);
            }

            // Create a new file     
            using (FileStream fs = System.IO.File.Create(combine))
            {
                // Add some text to file    
                //Byte[] title = new UTF8Encoding(true).GetBytes(fileName);
                //fs.Write(title, 0, title.Length);
                string text = "";
                if (rowNumber.Count == 0)
                {
                    text = " تمام اطلاعات ذخیره شد ";
                }
                else
                {
                    text = " ردیف رکوردهایی که ذخیره نشدند در سیستم ";
                    foreach (var item in rowNumber)
                    {
                        text += $"\n {item + 1}";
                    }
                }

                byte[] author = new UTF8Encoding(true).GetBytes(text);
                //fs.Write(author, 0, author.Length);
                await fs.WriteAsync(author, 0, author.Length);
                fs.Close();
                //return File(fs, "text/plain", fileName);
            }

            return rowNumber.Count;
            //HttpContext.Response.ContentType = "text/plain";
            //FileContentResult result = new FileContentResult(System.IO.File.ReadAllBytes(combine), "text/plain")
            //{
            //    FileDownloadName = $"{fileName}"
            //};

            //return result;
        }


        /// <summary>
        /// اگر بین تعداد رکورد های تایید شده ی بانکی و واریزی ها اختلافی باشد، ان ها را برمیگرداند 
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> HiddenFeature()
        {
            var listConfirmmDeposit = _charityDepositService.GetAll().Where(x => x.IsConfirm)
                .Select(x => x.CharityBankRecordId).ToList();
            var listIsInUseBank = _charityBankRecordService.GetAll().Include(x=>x.CharityAccount).Where(x => x.IsInUse).ToList();

            List<CharityBankRecord> emptyList = new();
            foreach (var item in listIsInUseBank)
            {
                if (!listConfirmmDeposit.Contains(item.Id)) emptyList.Add(item);
            }

            PagedList<CharityBankRecord> pageList = new(emptyList, 1, 1000000);


            return View("Index", pageList);
        }
    }
}
