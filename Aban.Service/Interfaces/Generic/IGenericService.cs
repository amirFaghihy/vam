using Aban.Common;
using Aban.Domain.Entities;
using Aban.Domain.Enumerations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using X.PagedList;

namespace Aban.Service.IServices.Generic
{
    public interface IGenericService<T> where T : class
    {
        List<SelectListItem> SelectListItem(IQueryable<T> query, string text, string value, string nullText = "");
        List<SelectListItem> SelectListItem(IQueryable<T> query, string text, string value, string selectedValue, string nullText = "");
        List<SelectListItem> SelectListItem(IQueryable<T> query, string text, string value, List<string> selectedValue, string nullText = "");
        List<SelectListItem> SelectListItem<TPropertyText, TPropertyValue>(IQueryable<T> query, Expression<Func<T, TPropertyText>> text, Expression<Func<T, TPropertyValue>> value, string nullText = "");
        List<SelectListItem> SelectListItem<TPropertyText, TPropertyValue>(IQueryable<T> query, Expression<Func<T, TPropertyText>> text, Expression<Func<T, TPropertyValue>> value, string selectedValue, string nullText = "");
        List<SelectListItem> SelectListItem<TPropertyText, TPropertyValue>(IQueryable<T> query, Expression<Func<T, TPropertyText>> text, Expression<Func<T, TPropertyValue>> value, List<string> selectedValue, string nullText = "");
        IQueryable<T> GetAll();
        Task<ResultStatusOperation> Delete(Guid Id);
        Task<ResultStatusOperation> Delete(int Id);
        Task<ResultStatusOperation> DeleteLogic(int Id);
        Task<ResultStatusOperation> DeleteLogic(T model);
        //Task<ResultStatusOperation> DeleteRange(IEnumerable<Guid> Ids);
        Task<ResultStatusOperation> DeleteRange(IEnumerable<int> Ids);
        Task<Tuple<T, ResultStatusOperation>> Find(Guid Id);
        Task<Tuple<T, ResultStatusOperation>> Find(string Id);
        Task<Tuple<T, ResultStatusOperation>> Find(int Id);
        Task<Tuple<T, ResultStatusOperation>> Insert(ControllerInfo controllerInfo, T model);
        Task<Tuple<T, ResultStatusOperation>> Insert(bool modelStateIsValid, T model);
        Task<Tuple<IEnumerable<T>, ResultStatusOperation>> InsertRange(ControllerInfo controllerInfom, IEnumerable<T> entity);
        Task<Tuple<IEnumerable<T>, ResultStatusOperation>> InsertRange(bool modelStateIsValid, IEnumerable<T> entity);
        Task<Tuple<T, ResultStatusOperation>> Remove(T model);
        Task<Tuple<T, ResultStatusOperation>> RemoveRange(IEnumerable<T> model);
        void SaveChanges();
        Task<Tuple<T, ResultStatusOperation>> Update(ControllerInfo controllerInfo, T model, bool isSaveChange = true);
        Task<Tuple<T, ResultStatusOperation>> Update(bool modelStateIsValid, T model, bool isSaveChange = true);
        Task<Tuple<IEnumerable<T>, ResultStatusOperation>> UpdateRange(ControllerInfo controllerInfo, IEnumerable<T> model, bool isSaveChange = true);
        Task<Tuple<IEnumerable<T>, ResultStatusOperation>> UpdateRange(bool modelStateIsValid, IEnumerable<T> model, bool isSaveChange = true);

        Tuple<T, ResultStatusOperation> GetById(Guid id);
        Tuple<T, ResultStatusOperation> GetById(string id);
        Tuple<T, ResultStatusOperation> GetById(int id);

        Task<IPagedList<T>> Paginationasync(IQueryable<T>? specification = null, bool isExportPageList = true, int pageNumber = 1, int pageSize = 10, bool isDesc = true, string sortColumn = "");
        IPagedList<T> Pagination(IQueryable<T>? specification = null, bool isExportPageList = true, int pageNumber = 1, int pageSize = 10, bool isDesc = true, string sortColumn = "");

    }
}
