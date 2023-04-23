﻿using System.Linq.Expressions;
using VillaApp_WebAPI.Models;

namespace VillaApp_WebAPI.Repository.IRepository
{
    public interface IRepository<T> where T : class //generics of class T 
    {
        Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null); //all record GET call 
        //async tracking
        Task<T> GetAsync(Expression<Func<T, bool>> filter = null, bool trackeed = true); //GEt by id returns one record
        Task CreateAsync(T entity);
        Task RemoveAsync(T entity);
        Task SaveAsync();
    }
}
