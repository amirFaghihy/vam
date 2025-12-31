using Aban.Common;
using Aban.DataLayer.Context;
using Aban.DataLayer.Repositories.Generics;
using Aban.Domain.Entities;
using Aban.Service.IServices.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq.Expressions;
using X.PagedList;
using static Aban.Domain.Enumerations.Enumeration;

namespace Aban.Service.Services.Generic
{
    public class GenericService<T> : IGenericService<T> where T : class
    {
        private readonly GenericRepository<T> _genericRepository;
        public GenericService(AppDbContext appDbContext)
        {
            _genericRepository = new GenericRepository<T>(appDbContext);
        }

        public virtual List<SelectListItem> SelectListItem(IQueryable<T> query, string text, string value, string nullText = "")
        {

            var enumerable = query.AsEnumerable();
            List<SelectListItem> listItem = new List<SelectListItem>();

            listItem = enumerable.ToList().ConvertAll(x =>
            {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                return new SelectListItem()
                {
                    Text = typeof(T).GetProperty(text).GetValue(x, null).ToString(),
                    //this.GetPropertyValue(x, text).ToString(),
                    Value = typeof(T).GetProperty(value).GetValue(x, null).ToString(),

                };
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            });
            if (!String.IsNullOrEmpty(nullText))
            {

                listItem.Insert(0, new SelectListItem(nullText, ""));
            }
            return listItem;


        }

        public virtual List<SelectListItem> SelectListItem(IQueryable<T> query, string text, string value, string selectedValue, string nullText = "")
        {

            var enumerable = query.AsEnumerable();
            List<SelectListItem> listItem = new List<SelectListItem>();

            listItem = enumerable.ToList().ConvertAll(x =>
            {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                return new SelectListItem()
                {
                    Text = typeof(T).GetProperty(text).GetValue(x, null).ToString(),
                    //this.GetPropertyValue(x, text).ToString(),
                    Value = typeof(T).GetProperty(value).GetValue(x, null).ToString(),
                    Selected = selectedValue == typeof(T).GetProperty(value).GetValue(x, null).ToString() ? true : false
                };
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            });
            if (!String.IsNullOrEmpty(nullText))
            {

                listItem.Insert(0, new SelectListItem(nullText, ""));
            }
            return listItem;


        }
        public virtual List<SelectListItem> SelectListItem(IQueryable<T> query, string text, string value, List<string> selectedValue, string nullText = "")
        {

            var enumerable = query.AsEnumerable();
            List<SelectListItem> listItem = new List<SelectListItem>();

            listItem = enumerable.ToList().ConvertAll(x =>
            {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8604 // Possible null reference argument.
                return new SelectListItem()
                {
                    Text = typeof(T).GetProperty(text).GetValue(x, null).ToString(),
                    //this.GetPropertyValue(x, text).ToString(),
                    Value = typeof(T).GetProperty(value).GetValue(x, null).ToString(),
                    Selected = selectedValue.Contains(typeof(T).GetProperty(value).GetValue(x, null).ToString()) ? true : false
                };
#pragma warning restore CS8604 // Possible null reference argument.
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            });
            if (!String.IsNullOrEmpty(nullText))
            {

                listItem.Insert(0, new SelectListItem(nullText, ""));
            }
            return listItem;


        }


        public virtual List<SelectListItem> SelectListItem<TPropertyText, TPropertyValue>(IQueryable<T> query, Expression<Func<T, TPropertyText>> text, Expression<Func<T, TPropertyValue>> value, string nullText = "")
        {
            var enumerable = query.AsEnumerable();
            List<SelectListItem> listItem = new List<SelectListItem>();
            List<string> listTitleProperty = new();



            listTitleProperty = GetPropertyTextList<TPropertyText, TPropertyValue>(text);
            //.ForEach(x=>x.Split(".").Last()

            var titleProperty = PropertyUtility.GetFullPropertyName(text);
            var valueProeprty = PropertyUtility.GetFullPropertyName(value);
            listItem = enumerable.ToList().ConvertAll(x =>
            {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                return new SelectListItem()
                {
                    Text = GetPropertyText<T>(listTitleProperty, x),
                    Value = typeof(T).GetProperty(valueProeprty).GetValue(x, null).ToString(),

                };
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            });
            if (!String.IsNullOrEmpty(nullText))
            {

                listItem.Insert(0, new SelectListItem(nullText, ""));
            }
            return listItem;


        }
        private List<string> GetPropertyTextList<TPropertyText, TPropertyValue>(Expression<Func<T, TPropertyText>> text)
        {
            List<string> listTitleProperty = new();
            text.Body.ToString().Replace("(", "").Replace(")", "")
                .Replace("{", "").Replace("}", "").Replace("Object", "").Replace(",", "")
                .Replace("[", "").Replace("]", "").Split("+").ToList().ForEach(x =>
                {
                    listTitleProperty.Add(x.Split(".").Last().Trim());

                });
            return listTitleProperty;
        }
#pragma warning disable CS0693 // Type parameter has the same name as the type parameter from outer type
        public string GetPropertyText<T>(List<string> textProperty, object? x)
#pragma warning restore CS0693 // Type parameter has the same name as the type parameter from outer type
        {
            string returnValue = "";
            foreach (var item in textProperty)
            {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                returnValue += typeof(T).GetProperty(item).GetValue(x) + " ";
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            }
            return returnValue.TrimStart().TrimEnd();
        }

        //Now it can convert multi lambda property to text but no space and splitter ! so it's required one 
        public virtual List<SelectListItem> SelectListItem<TPropertyText, TPropertyValue>(IQueryable<T> query, Expression<Func<T, TPropertyText>> text, Expression<Func<T, TPropertyValue>> value, string selectedValue, string nullText = "")
        {
            var enumerable = query.AsEnumerable();
            List<SelectListItem> listItem = new List<SelectListItem>();
            List<string> listTitleProperty = new();

            //.ForEach(x=>x.Split(".").Last()
            listTitleProperty = GetPropertyTextList<TPropertyText, TPropertyValue>(text);
            var titleProperty = PropertyUtility.GetFullPropertyName(text);
            var valueProeprty = PropertyUtility.GetFullPropertyName(value);

            listItem = enumerable.ToList().ConvertAll(x =>
            {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                return new SelectListItem()
                {
                    Text = GetPropertyText<T>(listTitleProperty, x),//typeof(T).GetProperty(titleProperty).GetValue(x, null).ToString(),
                    Value = typeof(T).GetProperty(valueProeprty).GetValue(x, null).ToString(),
                    Selected = selectedValue == typeof(T).GetProperty(valueProeprty).GetValue(x, null).ToString() ? true : false
                };
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            });
            if (!String.IsNullOrEmpty(nullText))
            {

                listItem.Insert(0, new SelectListItem(nullText, ""));
            }
            return listItem;


        }

        public virtual List<SelectListItem> SelectListItem<TPropertyText, TPropertyValue>(IQueryable<T> query, Expression<Func<T, TPropertyText>> text, Expression<Func<T, TPropertyValue>> value, List<string> selectedValue, string nullText = "")
        {

            var enumerable = query.AsEnumerable();
            List<SelectListItem> listItem = new List<SelectListItem>();
            List<string> listTitleProperty = new();

            text.Body.ToString().Replace("(", "").Replace(")", "")
                .Replace("{", "").Replace("}", "")
                .Replace("[", "").Replace("]", "").Split("+").ToList().ForEach(x =>
                {
                    listTitleProperty.Add(x.Split(".").Last().Trim());

                });

            //.ForEach(x=>x.Split(".").Last()

            var titleProperty = PropertyUtility.GetFullPropertyName(text);
            var valueProeprty = PropertyUtility.GetFullPropertyName(value);
            listItem = enumerable.ToList().ConvertAll(x =>
            {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8604 // Possible null reference argument.
                return new SelectListItem()
                {
                    Text = GetPropertyText<T>(listTitleProperty, x),
                    Value = typeof(T).GetProperty(valueProeprty).GetValue(x, null).ToString(),
                    Selected = selectedValue.Contains(typeof(T).GetProperty(valueProeprty).GetValue(x, null).ToString()) ? true : false
                };
#pragma warning restore CS8604 // Possible null reference argument.
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            });
            if (!String.IsNullOrEmpty(nullText))
            {

                listItem.Insert(0, new SelectListItem(nullText, ""));
            }
            return listItem;

        }

        public IQueryable<T> GetAll()
        {
            try
            {
                IQueryable<T> obj = _genericRepository.GetAll();
                return obj;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// with SaveChanges()
        /// </summary>
        public async Task<ResultStatusOperation> Delete(Guid Id)
        {
            ResultStatusOperation resultStatusOperation = new ResultStatusOperation
            {
                IsSuccessed = true,
                Type = MessageTypeResult.Success,
                Message = "عملیات با موفقیت انجام شد"
            };
            try
            {
                T model = _genericRepository.Find(Id);
                if (model != null)
                {
                    _genericRepository.Delete(model);
                    await _genericRepository.SaveChangesAsync();
                }
                else
                {
                    resultStatusOperation.IsSuccessed = false;
                    resultStatusOperation.Message = "رکورد یافت نشد";
                    resultStatusOperation.Type = MessageTypeResult.Warning;
                }

                return resultStatusOperation;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ResultStatusOperation> DeleteLogic(int Id)
        {
            ResultStatusOperation resultStatusOperation = new ResultStatusOperation
            {
                IsSuccessed = true,
                Type = MessageTypeResult.Success,
                Message = "عملیات با موفقیت انجام شد"
            };
            try
            {
                T model = _genericRepository.Find(Id);
                if (model != null)
                {
                    var propertyInfo = typeof(T).GetProperty("IsDelete");
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                    propertyInfo.SetValue(model, true);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                    await _genericRepository.SaveChangesAsync();
                }
                else
                {
                    resultStatusOperation.IsSuccessed = false;
                    resultStatusOperation.Message = "خطایی رخ داده است";
                    resultStatusOperation.Type = MessageTypeResult.Warning;
                }

                return resultStatusOperation;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ResultStatusOperation> DeleteLogic(T model)
        {
            ResultStatusOperation resultStatusOperation = new ResultStatusOperation
            {
                IsSuccessed = true,
                Type = MessageTypeResult.Success,
                Message = "عملیات با موفقیت انجام شد"
            };
            try
            {
                if (model != null)
                {
                    var propertyInfo = typeof(T).GetProperty("IsDelete");
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                    propertyInfo.SetValue(model, true);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                    await _genericRepository.SaveChangesAsync();
                }
                else
                {
                    resultStatusOperation.IsSuccessed = false;
                    resultStatusOperation.Message = "خطایی رخ داده است";
                    resultStatusOperation.Type = MessageTypeResult.Warning;
                }

                return resultStatusOperation;
            }
            catch (Exception exception)
            {
                resultStatusOperation.ErrorException = exception;
                resultStatusOperation.IsSuccessed = false;
                resultStatusOperation.Message = "خطایی رخ داده است";
                resultStatusOperation.Type = MessageTypeResult.Warning;
                return resultStatusOperation;
            }
        }

        /// <summary>
        /// with SaveChanges()
        /// </summary>
        public async Task<ResultStatusOperation> Delete(int Id)
        {
            ResultStatusOperation resultStatusOperation = new ResultStatusOperation
            {
                IsSuccessed = true,
                Type = MessageTypeResult.Success,
                Message = "عملیات با موفقیت انجام شد"
            };
            try
            {
                T model = _genericRepository.Find(Id);
                if (model != null)
                {
                    _genericRepository.Delete(model);
                    await _genericRepository.SaveChangesAsync();
                }
                else
                {
                    resultStatusOperation.IsSuccessed = false;
                    resultStatusOperation.Message = "خطایی رخ داده است";
                    resultStatusOperation.Type = MessageTypeResult.Warning;
                }

                return resultStatusOperation;
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// with SaveChanges()
        /// </summary>
        public async Task<ResultStatusOperation> DeleteRange(IEnumerable<int> Id)
        {
            ResultStatusOperation resultStatusOperation = new ResultStatusOperation
            {
                IsSuccessed = true,
                Type = MessageTypeResult.Success,
                Message = "عملیات با موفقیت انجام شد"
            };
            try
            {
                IEnumerable<T> model = _genericRepository.GetAll();
                if (model != null)
                {
                    _genericRepository.DeleteRange(model); //just updates models
                    await _genericRepository.SaveChangesAsync();
                }
                else
                {
                    resultStatusOperation.IsSuccessed = false;
                    resultStatusOperation.Message = "رکورد یافت نشد";
                    resultStatusOperation.Type = MessageTypeResult.Warning;
                }

                return resultStatusOperation;
            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// with SaveChanges()
        /// </summary>
        /// <param name="entity"></param>
        public async void DeleteRange(IEnumerable<T> models)
        {
            ResultStatusOperation resultStatusOperation = new ResultStatusOperation
            {
                IsSuccessed = true,
                Type = MessageTypeResult.Success,
                Message = "عملیات با موفقیت انجام شد"
            };
            try
            {
                if (models != null)
                {
                    _genericRepository.DeleteRange(models);//just updates models
                    await _genericRepository.SaveChangesAsync();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public virtual Task<Tuple<T, ResultStatusOperation>> Find(Guid Id)
        {
            ResultStatusOperation resultStatusOperation = new ResultStatusOperation
            {
                IsSuccessed = true,
                Type = MessageTypeResult.Success,
                Message = "عملیات با موفقیت انجام شد"
            };

            try
            {
                T model = _genericRepository.Find(Id);

                if (model != null)
                {
                    resultStatusOperation.Message = "رکورد باموفقیت بازیابی شد";
                    resultStatusOperation.IsSuccessed = true;
                    resultStatusOperation.Type = MessageTypeResult.Success;
                    return Task.FromResult(Tuple.Create(model, resultStatusOperation));
                }
                else
                {
                    resultStatusOperation.Message = "رکورد یافت نشد";
                    resultStatusOperation.IsSuccessed = false;
                    resultStatusOperation.Type = MessageTypeResult.Warning;
#pragma warning disable CS8619 // Nullability of reference types in value doesn't match target type.
                    return Task.FromResult(Tuple.Create(model, resultStatusOperation));
#pragma warning disable CS8619 // Nullability of reference types in value doesn't match target type.
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

        public Task<Tuple<T, ResultStatusOperation>> Find(string Id)
        {
            ResultStatusOperation resultStatusOperation = new ResultStatusOperation
            {
                IsSuccessed = true,
                Type = MessageTypeResult.Success,
                Message = "عملیات با موفقیت انجام شد"
            };
            try
            {
                T obj = _genericRepository.Find(Id);

                return Task.FromResult(Tuple.Create(obj, resultStatusOperation));
            }
            catch (Exception)
            {
                throw;
            }
        }


        public Task<Tuple<T, ResultStatusOperation>> Find(int Id)
        {
            ResultStatusOperation resultStatusOperation = new ResultStatusOperation
            {
                IsSuccessed = true,
                Type = MessageTypeResult.Success,
                Message = "عملیات با موفقیت انجام شد"
            };
            try
            {
                T obj = _genericRepository.Find(Id);
                if (obj == null)
                {
                    resultStatusOperation = new ResultStatusOperation
                    {
                        IsSuccessed = false,
                        Type = MessageTypeResult.Warning,
                        Message = "اطلاعات یافت نشد"
                    };
                }

                return Task.FromResult(Tuple.Create(obj, resultStatusOperation));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public virtual async Task<Tuple<T, ResultStatusOperation>> Insert(ControllerInfo controllerInfo, T model)
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
                    return Tuple.Create(model, resultStatusOperation);
                }

#pragma warning disable CS8602 // Dereference of a possibly null reference.
                if (!controllerInfo.ModelState.IsValid)
                {
                    resultStatusOperation.Type = MessageTypeResult.Warning;
                    resultStatusOperation.IsSuccessed = false;
                    resultStatusOperation.Message = "اطلاعات کامل نیست";
                    return Tuple.Create(model, resultStatusOperation);
                }

                _genericRepository.Insert(model);
                await _genericRepository.SaveChangesAsync();
                resultStatusOperation.Type = MessageTypeResult.Success;
                resultStatusOperation.Message = "اطلاعات با موفقیت ثبت شد";
                return Tuple.Create(model, resultStatusOperation);
            }
            catch (Exception exception)
            {
                resultStatusOperation.IsSuccessed = false;
                resultStatusOperation.Message = "خطایی رخ داده است";
                resultStatusOperation.Type = MessageTypeResult.Danger;
                resultStatusOperation.ErrorException = exception;
                return Tuple.Create(model, resultStatusOperation);
            }
        }

        public virtual async Task<Tuple<T, ResultStatusOperation>> Insert(bool modelStateIsValid, T model)
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
                    return Tuple.Create(model, resultStatusOperation);
                }

                if (!modelStateIsValid)
                {
                    resultStatusOperation.Type = MessageTypeResult.Warning;
                    resultStatusOperation.IsSuccessed = false;
                    resultStatusOperation.Message = "اطلاعات کامل نیست";
                    return Tuple.Create(model, resultStatusOperation);
                }

                _genericRepository.Insert(model);
                await _genericRepository.SaveChangesAsync();
                resultStatusOperation.Type = MessageTypeResult.Success;
                resultStatusOperation.Message = "اطلاعات با موفقیت ثبت شد";
                return Tuple.Create(model, resultStatusOperation);
            }
            catch (Exception exception)
            {
                resultStatusOperation.IsSuccessed = false;
                resultStatusOperation.Message = "خطایی رخ داده است";
                resultStatusOperation.Type = MessageTypeResult.Danger;
                resultStatusOperation.ErrorException = exception;
                return Tuple.Create(model, resultStatusOperation);
            }
        }


        public async Task<Tuple<IEnumerable<T>, ResultStatusOperation>> InsertRange(ControllerInfo controllerInfom, IEnumerable<T> entity)
        {
            ResultStatusOperation resultStatusOperation = new ResultStatusOperation
            {
                IsSuccessed = true,
                Type = MessageTypeResult.Success,
                Message = "عملیات با موفقیت انجام شد"
            };
            try
            {

                if (entity == null)
                {
                    resultStatusOperation.Message = "اطلاعات صحیح نیست";
                    resultStatusOperation.IsSuccessed = false;
                    resultStatusOperation.Type = MessageTypeResult.Warning;
                    return Tuple.Create(entity, resultStatusOperation);
                }
                if (!controllerInfom.ModelState.IsValid)
                {
                    resultStatusOperation.Message = "اطلاعات صحیح نیست";
                    resultStatusOperation.IsSuccessed = false;
                    resultStatusOperation.Type = MessageTypeResult.Warning;
                    return Tuple.Create(entity, resultStatusOperation);
                }

                if (entity != null)
                {
                    _genericRepository.InsertRange(entity);
                    await _genericRepository.SaveChangesAsync();

                    resultStatusOperation.Type = MessageTypeResult.Success;
                    resultStatusOperation.Message = "اطلاعات با موفقیت بروزرسانی شد";
                }

                return Tuple.Create(entity, resultStatusOperation);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Tuple<IEnumerable<T>, ResultStatusOperation>> InsertRange(bool modelStateIsValid, IEnumerable<T> entity)
        {
            ResultStatusOperation resultStatusOperation = new ResultStatusOperation
            {
                IsSuccessed = true,
                Type = MessageTypeResult.Success,
                Message = "عملیات با موفقیت انجام شد"
            };
            try
            {

                if (entity == null)
                {
                    resultStatusOperation.Type = MessageTypeResult.Warning;
                    resultStatusOperation.IsSuccessed = false;
                    resultStatusOperation.Message = "مدل خالی است";
                    return Tuple.Create(entity, resultStatusOperation);
                }

                if (!modelStateIsValid)
                {
                    resultStatusOperation.Type = MessageTypeResult.Warning;
                    resultStatusOperation.IsSuccessed = false;
                    resultStatusOperation.Message = "اطلاعات کامل نیست";
                    return Tuple.Create(entity, resultStatusOperation);
                }

                _genericRepository.InsertRange(entity);
                await _genericRepository.SaveChangesAsync();

                resultStatusOperation.Type = MessageTypeResult.Success;
                resultStatusOperation.Message = "اطلاعات با موفقیت بروزرسانی شد";

                return Tuple.Create(entity, resultStatusOperation);
            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// without SaveChanges()
        /// </summary>
        /// <param name="entity"></param>
        public void Remove(T entity)
        {
            try
            {
                if (entity != null)
                {
                    _genericRepository.Remove(entity);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// without SaveChanges()
        /// </summary>
        /// <param name="entity"></param>
        public void RemoveRange(IEnumerable<T> entity)
        {
            try
            {
                if (entity != null)
                {
                    _genericRepository.RemoveRange(entity);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        public void SaveChanges() => _genericRepository.SaveChangesAsync();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

        public virtual async Task<Tuple<T, ResultStatusOperation>> Update(ControllerInfo controllerInfo, T model, bool isSaveChange = true)
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
                    return Tuple.Create(model, resultStatusOperation);
                }
                if (!controllerInfo.ModelState.IsValid)
                {
                    resultStatusOperation.Message = "اطلاعات کامل نیست";
                    resultStatusOperation.IsSuccessed = false;
                    resultStatusOperation.Type = MessageTypeResult.Warning;
                    return Tuple.Create(model, resultStatusOperation);
                }

                _genericRepository.Update(model);

                if (isSaveChange)
                    await _genericRepository.SaveChangesAsync();

                resultStatusOperation.Type = MessageTypeResult.Success;
                resultStatusOperation.Message = "اطلاعات با موفقیت بروزرسانی شد";
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

        public virtual async Task<Tuple<T, ResultStatusOperation>> Update(bool modelStateIsValid, T model, bool isSaveChange = true)
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
                    return Tuple.Create(model, resultStatusOperation);
                }
                if (!modelStateIsValid)
                {
                    resultStatusOperation.Message = "اطلاعات کامل نیست";
                    resultStatusOperation.IsSuccessed = false;
                    resultStatusOperation.Type = MessageTypeResult.Warning;
                    return Tuple.Create(model, resultStatusOperation);
                }

                _genericRepository.Update(model);

                if (isSaveChange)
                    await _genericRepository.SaveChangesAsync();


                resultStatusOperation.Type = MessageTypeResult.Success;
                resultStatusOperation.Message = "اطلاعات با موفقیت بروزرسانی شد";
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

        public virtual async Task<Tuple<IEnumerable<T>, ResultStatusOperation>> UpdateRange(ControllerInfo controllerInfo, IEnumerable<T> model, bool isSaveChange = true)
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
                    resultStatusOperation.Message = "اطلاعات صحیح نیست";
                    resultStatusOperation.IsSuccessed = false;
                    resultStatusOperation.Type = MessageTypeResult.Warning;
                    return Tuple.Create(model, resultStatusOperation);
                }
                if (!controllerInfo.ModelState.IsValid)
                {
                    resultStatusOperation.Message = "اطلاعات صحیح نیست";
                    resultStatusOperation.IsSuccessed = false;
                    resultStatusOperation.Type = MessageTypeResult.Warning;
                    return Tuple.Create(model, resultStatusOperation);
                }

                if (model != null)
                {
                    _genericRepository.UpdateRange(model);

                    if (isSaveChange)
                        await _genericRepository.SaveChangesAsync();

                    resultStatusOperation.Type = MessageTypeResult.Success;
                    resultStatusOperation.Message = "اطلاعات با موفقیت بروزرسانی شد";
                }

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

        public virtual async Task<Tuple<IEnumerable<T>, ResultStatusOperation>> UpdateRange(bool modelStateIsValid, IEnumerable<T> model, bool isSaveChange = true)
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
                    resultStatusOperation.Message = "اطلاعات صحیح نیست";
                    resultStatusOperation.IsSuccessed = false;
                    resultStatusOperation.Type = MessageTypeResult.Warning;
                    return Tuple.Create(model, resultStatusOperation);
                }
                if (!modelStateIsValid)
                {
                    resultStatusOperation.Message = "اطلاعات صحیح نیست";
                    resultStatusOperation.IsSuccessed = false;
                    resultStatusOperation.Type = MessageTypeResult.Warning;
                    return Tuple.Create(model, resultStatusOperation);
                }

                if (model != null)
                {
                    _genericRepository.UpdateRange(model);

                    if (isSaveChange)
                        await _genericRepository.SaveChangesAsync();

                    resultStatusOperation.Type = MessageTypeResult.Success;
                    resultStatusOperation.Message = "اطلاعات با موفقیت بروزرسانی شد";
                }

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

        public virtual async Task<IPagedList<T>> Paginationasync(
            IQueryable<T>? specification = null,
            bool isExportPageList = true,
            int pageNumber = 1,
            int pageSize = 10,
            bool isDesc = true,
            string sortColumn = "")
        {
            IPagedList<T> resultList;
            var sortColumnParam = "Id";
            if (!string.IsNullOrWhiteSpace(sortColumn))
            {
                sortColumnParam = sortColumn;
            }

            var propertyInfo = typeof(T).GetProperty(sortColumnParam);

            IQueryable<T> query = specification == null ? _genericRepository.GetAll() : specification;

            if (isDesc)
                resultList = await query.AsEnumerable().OrderByDescending(x => propertyInfo.GetValue(x, null)).ToPagedListAsync<T>(pageNumber, pageSize);
            else
                resultList = await query.AsEnumerable().OrderBy(x => propertyInfo.GetValue(x, null)).ToPagedListAsync<T>(pageNumber, pageSize);

            return resultList;
        }

        public virtual IPagedList<T> Pagination(
            IQueryable<T>? specification = null,
            bool isExportPageList = true,
            int pageNumber = 1,
            int pageSize = 10,
            bool isDesc = true,
            string sortColumn = "")
        {
            IPagedList<T> resultList;
            var sortColumnParam = "Id";
            if (!string.IsNullOrWhiteSpace(sortColumn))
            {
                sortColumnParam = sortColumn;
            }

            var propertyInfo = typeof(T).GetProperty(sortColumnParam);

            IQueryable<T> query = specification == null ? _genericRepository.GetAll() : specification;

            if (isDesc)
                resultList = query.AsEnumerable().OrderByDescending(x => propertyInfo.GetValue(x, null)).ToPagedListAsync<T>(pageNumber, pageSize).Result;
            else
                resultList = query.AsEnumerable().OrderBy(x => propertyInfo.GetValue(x, null)).ToPagedListAsync<T>(pageNumber, pageSize).Result;

            return resultList;
        }

        public virtual Tuple<T, ResultStatusOperation> GetById(Guid id)
        {
            ResultStatusOperation resultStatusOperation = new ResultStatusOperation
            {
                IsSuccessed = true,
                Type = MessageTypeResult.Success
            };

            try
            {
                return Tuple.Create(_genericRepository.Find(id), resultStatusOperation);
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

        public virtual Tuple<T, ResultStatusOperation> GetById(string id)
        {
            ResultStatusOperation resultStatusOperation = new ResultStatusOperation
            {
                IsSuccessed = true,
                Type = MessageTypeResult.Success
            };

            try
            {
                return Tuple.Create(_genericRepository.Find(id), resultStatusOperation);
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

        public virtual Tuple<T, ResultStatusOperation> GetById(int id)
        {
            ResultStatusOperation resultStatusOperation = new ResultStatusOperation
            {
                IsSuccessed = true,
                Type = MessageTypeResult.Success
            };

            try
            {
                return Tuple.Create(_genericRepository.Find(id), resultStatusOperation);
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

        public virtual async Task<Tuple<T, ResultStatusOperation>> FindByStringId(string Id)
        {
            ResultStatusOperation resultStatusOperation = new ResultStatusOperation
            {
                IsSuccessed = true,
                Type = MessageTypeResult.Success
            };

            try
            {
                T model = await _genericRepository.GetByIdStringAsync(Id);
                if (model != null)
                {
                    resultStatusOperation.Message = "رکورد باموفقیت بازیابی شد";
                    resultStatusOperation.IsSuccessed = true;
                    resultStatusOperation.Type = MessageTypeResult.Success;
                    return Tuple.Create(model, resultStatusOperation);
                }
                else
                {
                    resultStatusOperation.Message = "رکورد یافت نشد";
                    resultStatusOperation.IsSuccessed = false;
                    resultStatusOperation.Type = MessageTypeResult.Warning;
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


        Task<Tuple<T, ResultStatusOperation>> IGenericService<T>.Remove(T model)
        {
            throw new NotImplementedException();
        }

        Task<Tuple<T, ResultStatusOperation>> IGenericService<T>.RemoveRange(IEnumerable<T> model)
        {
            throw new NotImplementedException();
        }


    }
}
