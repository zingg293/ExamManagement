using DPD.HumanResources.Utilities.Utils;

namespace DPD.HumanResources.Interface;

//this interface to define a common interface
//as a Repository design pattern we'll define a common interface and the other interface will be implement from this
//some common method like a getAll, getById, insert, update, delete ...
public interface IRepository<T> where T : class
{
    Task<TemplateApi<T>> GetAllAsync(int pageNumber, int pageSize);
    Task<TemplateApi<T>> GetById(Guid id); 
    Task<TemplateApi<T>> GetAllAvailable(int pageNumber, int pageSize);
    Task<TemplateApi<T>> Update(T model, Guid idUserCurrent, string fullName);
    Task<TemplateApi<T>> Insert(T model, Guid idUserCurrent, string fullName);
    Task<TemplateApi<T>> RemoveByList(List<Guid> ids, Guid idUserCurrent, string fullName);
    Task<TemplateApi<T>> HideByList(List<Guid> ids, bool isLock, Guid idUserCurrent, string fullName);
}