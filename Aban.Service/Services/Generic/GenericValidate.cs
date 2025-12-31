using Aban.Common;
using Aban.Domain.Entities;
using static Aban.Domain.Enumerations.Enumeration;

namespace Aban.Service.Services
{
    public class GenericValidate<T> where T : class
    {

        public static Task<Tuple<T, ResultStatusOperation>> CheckValidatedModel(ControllerInfo controllerInfo, T model)
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
                    resultStatusOperation.Type = MessageTypeResult.Warning;
                    resultStatusOperation.IsSuccessed = false;
                    resultStatusOperation.Message = "مدل خالی است";

#pragma warning disable CS8619 // Nullability of reference types in value doesn't match target type.
                    return Task.FromResult(Tuple.Create(model, resultStatusOperation));
                }
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                if (!controllerInfo.ModelState.IsValid)
                {
                    resultStatusOperation.Type = MessageTypeResult.Warning;
                    resultStatusOperation.IsSuccessed = false;
                    resultStatusOperation.Message = "اطلاعات کامل نیست";
                    return Task.FromResult(Tuple.Create(model, resultStatusOperation));
                }

                resultStatusOperation.Type = MessageTypeResult.Success;
                return Task.FromResult(Tuple.Create(model, resultStatusOperation));

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


    }
}
