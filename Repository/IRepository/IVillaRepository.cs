﻿using System.Linq.Expressions;
using VillaApp_WebAPI.Models;

namespace VillaApp_WebAPI.Repository.IRepository
{
    public interface IVillaRepository
    {
        Task<List<Villa>> GetAll(Expression<Func<Villa>> filter = null );
        //async tracking
        Task<Villa> GetAll(Expression<Func<Villa>> filter = null, bool trackeed= true );
        Task Create(Villa entity);
        Task Remove(Villa entity);
        Task Save();




    }
}