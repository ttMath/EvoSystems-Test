using EvoSystems.Dto;
using EvoSystems.Models;

namespace EvoSystems.Repository

{
    public interface IRepository<T> where T : class
    {
        Task<ServiceResponse<List<T>>> GetAll();
        Task<ServiceResponse<T>> GetById(int id);
        Task<ServiceResponse<T>> Add(T entity);
        Task<ServiceResponse<T>> Update(T entity);
        Task<ServiceResponse<T>> Delete(int id);
        Task<ServiceResponse<T>> Inactive(int id);
    }
}