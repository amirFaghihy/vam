using Aban.Domain.Configuration;
using Aban.Domain.Entities;
using Aban.Domain.Enumerations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using static Aban.Domain.Enumerations.Enumeration;

namespace Aban.Common
{
    public class GenericController : Controller
    {
        #region constructor
        private readonly IOptions<PathsConfiguration> pathsConfiguratin;

#pragma warning disable CS8618
        public GenericController()
#pragma warning restore CS8618
        {

        }
        public GenericController(
            IOptions<PathsConfiguration> pathsConfiguratin
            )
        {
            this.pathsConfiguratin = pathsConfiguratin;
        }

        #endregion

        public ControllerInfo fillControllerInfo()
        {
            return new ControllerInfo() { ModelState = ModelState, httpContext = HttpContext };
        }
        public ControllerInfo fillControllerInfo(string removeValidation)
        {
            RemoveModelState(removeValidation);
            return new ControllerInfo() { ModelState = ModelState, httpContext = HttpContext };
        }
        public ControllerInfo fillControllerInfo(List<string> removeValidation)
        {
            removeValidation.ForEach(x => RemoveModelState(x));
            return new ControllerInfo() { ModelState = ModelState, httpContext = HttpContext };
        }

        private void RemoveModelState(string key)
        {
            ModelState.Remove(key);
        }


        public void SetResultStatusOperation(ResultStatusOperation resultStatusOperation)
        {
            TempData["ResultStatusOperation"] = resultStatusOperation;
        }

        public void CreateAddressView(
            ref string view,
            string viewAddress = "",
            string viewName = "",
            string actionName = "",
            string extention = "cshtml")
        {
            ViewBag.actionName = actionName;
            string ViewDirectoryName = pathsConfiguratin.Value.ViewDirectoryName;
            viewAddress = viewAddress.Replace("{AreaName}", ViewDirectoryName);
            view = $"{viewAddress}/{viewName}.{extention}";
        }


        public string GetUserId()
        {
            try
            {
                string userId = "";
#pragma warning disable CS8602
                if (User.Identity.IsAuthenticated)
                {
                    //return User.Claims.FirstOrDefault(c => c.Type == "userId").Value;
                    userId = Request.Cookies["userId"];
                    if (String.IsNullOrEmpty(userId))
                    {
#pragma warning disable CS8600
                        userId = User.Identity.Name;
#pragma warning restore CS8600
                    }
                }
#pragma warning restore CS8602
#pragma warning disable CS8603
                return userId;
#pragma warning restore CS8603
            }
            catch (System.Exception exception)
            {
                SetMessageException(new ResultStatusOperation("", "", Enumeration.MessageTypeResult.Danger, false, exception), Enumeration.MessageTypeActionMethod.CreatePost);
                throw exception;
            }
        }


        public void RemoveCookie(string key)
        {
            if (Request.Cookies[key] != null)
            {
                Response.Cookies.Delete(key);
            }
        }

        public void SetCookie(string key, string value, ExpireTimeType expireTimeType = ExpireTimeType.Minutes, int expireTime = 1)
        {

            CookieOptions option = new CookieOptions();
            switch (expireTimeType)
            {
                case ExpireTimeType.Minutes:
                    {
                        option.Expires = DateTime.Now.AddMinutes(expireTime);
                        break;
                    }
                case ExpireTimeType.Hours:
                    {
                        option.Expires = DateTime.Now.AddHours(expireTime);
                        break;
                    }
                case ExpireTimeType.Days:
                    {
                        option.Expires = DateTime.Now.AddDays(expireTime);
                        break;
                    }
                case ExpireTimeType.Months:
                    {
                        option.Expires = DateTime.Now.AddMonths(expireTime);
                        break;
                    }
                case ExpireTimeType.Years:
                    {
                        option.Expires = DateTime.Now.AddYears(expireTime);
                        break;
                    }
                default:
                    {
                        option.Expires = DateTime.Now.AddMinutes(expireTime);
                        break;
                    }
            }

            Response.Cookies.Append(key, value, option);
        }

        public void SetUserId(string id)
        {
            try
            {
                CookieOptions CookieOptions = new CookieOptions();
                CookieOptions.Expires = DateTime.Now.AddDays(500);
                Response.Cookies.Append("userId", id, CookieOptions);
            }
            catch (System.Exception exception)
            {
                SetMessageException(new ResultStatusOperation("", "", Enumeration.MessageTypeResult.Danger, false, exception), Enumeration.MessageTypeActionMethod.CreatePost);
                throw exception;
            }
        }

        #region Messages

        public void SetMessage(ResultStatusOperation resultStatusOperation, bool replaceMessage = true)
        {
            if (TempData["ResultStatusOperation"] == null)//اگر پیام خالی باشد
            {
                TempData["ResultStatusOperation"] = JsonConvert.SerializeObject(resultStatusOperation);
            }
            else//اگر پیام پر باشد
            {
                if (replaceMessage) // اگر نیاز باشد پیغام آخر نمایش داده شود
                {
                    TempData["ResultStatusOperation"] = JsonConvert.SerializeObject(resultStatusOperation);
                }
                //else
                //{
                //    TempData["ResultStatusOperation"].ToString().JsonDeserialize<ResultStatusOperation>();
                //}
            }
        }

        public void SetMessageEditnotFound()
        {
            TempData["ResultStatusOperation"] = JsonConvert.SerializeObject(new ResultStatusOperation()
            { IsSuccessed = false, Message = "اطلاعات کامل نیست", Type = MessageTypeResult.Warning }
                );
        }

        public void SetMessageException(ResultStatusOperation resultStatusOperation, MessageTypeActionMethod messageTypeActionMethod = MessageTypeActionMethod.Index)
        {
            TempData["ResultStatusOperation"] = JsonConvert.SerializeObject(resultStatusOperation);
        }

        #endregion
    }
}
