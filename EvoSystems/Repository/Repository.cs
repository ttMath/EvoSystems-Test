using EvoSystems.Dto;
using EvoSystems.Models;
using EvoSystems.Repository;
using EvoSystems.Service.EmployeeService;
using global::EvoSystems.DataContext;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EvoSystems.Persistence.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly AppDBContext _context;
        public Repository(AppDBContext context)
        {
            _context = context;
        }
        public async Task<ServiceResponse<T>> Add(T entity)
        {
            ServiceResponse<T> serviceResponse = new ServiceResponse<T>();

            try
            {
                if (entity == null)
                {
                    serviceResponse.Data = null;
                    serviceResponse.Message = "Data is missing.";
                    serviceResponse.Success = false;
                    return serviceResponse;
                }

                _context.Add(entity);
                await _context.SaveChangesAsync();

                serviceResponse.Data = entity;
                serviceResponse.Message = $"{typeof(T).Name} successfully created.";

            }
            catch (Exception ex)
            {
                serviceResponse.Message = ex.Message;
                serviceResponse.Success = false;
            }

            return serviceResponse;
        }
        public async Task<ServiceResponse<List<T>>> GetAll()
        {
            ServiceResponse<List<T>> serviceResponse = new ServiceResponse<List<T>>();

            try
            {
                serviceResponse.Data = _context.Set<T>().ToList();

                if (serviceResponse.Data.Count == 0)
                {
                    serviceResponse.Message = "No data was found.";
                }
            }
            catch (Exception ex)
            {
                serviceResponse.Message = ex.Message;
                serviceResponse.Success = false;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<T>> GetById(int id)
        {
            ServiceResponse<T> serviceResponse = new ServiceResponse<T>();

            try
            {
                Type entityType = typeof(T);
                var idProperty = entityType.GetProperty("Id");

                if (idProperty != null)
                {
                    var entity = await _context.Set<T>().FindAsync(id);

                    if (entity != null)
                    {
                        serviceResponse.Data = entity;
                        serviceResponse.Success = true;
                        serviceResponse.Message = $"{entityType.Name} with ID {id} retrieved successfully.";
                    }
                    else
                    {
                        serviceResponse.Message = $"{entityType.Name} with ID {id} not found.";
                        serviceResponse.Success = false;
                    }
                }
            }
            catch (Exception ex)
            {
                serviceResponse.Message = ex.Message;
                serviceResponse.Success = false;
                serviceResponse.Data = null;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<T>> Inactive(int id)
        {
            ServiceResponse<T> serviceResponse = new ServiceResponse<T>();

            try
            {
                Type entityType = typeof(T);

                var idProperty = entityType.GetProperty("Id");
                var isActive = entityType.GetProperty("IsActive");

                if (idProperty != null)
                {
                    var entity = await _context.Set<T>().FindAsync(id);

                    if (entity == null)
                    {
                        serviceResponse.Message = $"{typeof(T).Name} with ID {id} not found.";
                        serviceResponse.Success = false;
                        return serviceResponse;
                    }

                    if (isActive != null && isActive.PropertyType == typeof(bool))
                    {
                        isActive.SetValue(entity, false);
                    }

                    _context.Set<T>().Update(entity);
                    await _context.SaveChangesAsync();

                    serviceResponse.Data = entity;
                    serviceResponse.Message = $"{typeof(T).Name} successfully inactivated.";
                }
            }
            catch (Exception ex)
            {
                serviceResponse.Message = ex.Message;
                serviceResponse.Success = false;
                serviceResponse.Data = null;
            }

            return serviceResponse;
        }
        public async Task<ServiceResponse<T>> Update(T entity)
        {
            ServiceResponse<T> serviceResponse = new ServiceResponse<T>();

            try
            {
                Type entityType = typeof(T);

                var idProperty = entityType.GetProperty("Id");
                var lastUpdate = entityType.GetProperty("LastUpdatedAt");

                if (idProperty != null)
                {
                    int entityId = (int)idProperty.GetValue(entity);

                    if (lastUpdate != null && lastUpdate.PropertyType == typeof(DateTime))
                    {
                        lastUpdate.SetValue(entity, DateTime.Now.ToLocalTime());
                    }
                }

                _context.Set<T>().Update(entity);
                await _context.SaveChangesAsync();

                serviceResponse.Data = entity;
                serviceResponse.Message = $"{typeof(T).Name} successfully updated.";
            }
            catch (Exception ex)
            {
                serviceResponse.Message = ex.Message;
                serviceResponse.Success = false;
                serviceResponse.Data = null;
            }

            return serviceResponse;
        }
        public async Task<ServiceResponse<T>> Delete(int id)
        {
            ServiceResponse<T> serviceResponse = new ServiceResponse<T>();

            try
            {
                Type entityType = typeof(T);
                var idProperty = entityType.GetProperty("Id");

                if (idProperty != null)
                {
                    var entity = await _context.Set<T>().FindAsync(id);

                    if (entity != null)
                    {
                        _context.Set<T>().Remove(entity);
                        await _context.SaveChangesAsync();

                        serviceResponse.Data = entity;
                        serviceResponse.Message = $"{entityType.Name} successfully deleted.";
                    }
                    else
                    {
                        serviceResponse.Message = $"{entityType.Name} with ID {id} not found.";
                        serviceResponse.Success = false;
                    }
                }
            }
            catch (Exception ex)
            {
                serviceResponse.Message = ex.Message;
                serviceResponse.Success = false;
                serviceResponse.Data = null;
            }
            return serviceResponse;
        }

    }
}