using Aban.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aban.Dashboard.Areas.Charity.Controllers
{
    [Area("Charity")]
    [Authorize]
    public class HomeController : GenericController
    {
        public IActionResult Index()
        {
            return View();
        }

        
       
        //بانک ملی == شماره سند کد پیگیری _ طبق این ولیدیت بکن هم اگر نبود شماره کارت
        
        //
        //دکمه بررسی مجدد واریزی ها برای تایید
       
        //      
        //واریزی های سرگردان تخصیص بدن به یه واریزی
        //
    }
}
