namespace Aban.Common
{
    public class ControllerInfo
    {
        public Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary? ModelState { get; set; }
        public Microsoft.AspNetCore.Http.HttpContext? httpContext { get; set; }
    }
}
