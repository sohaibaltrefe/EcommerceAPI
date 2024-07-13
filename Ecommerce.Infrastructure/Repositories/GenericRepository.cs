using Ecommerce.Core.Entities;
using Ecommerce.Core.IRepositories;
using Ecommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly AppDbContext dbContext;

        public GenericRepository(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task Create(T Model)
        {
           await  dbContext.Set<T>().AddAsync(Model);
        }

        public void Delete(int id)
        {
            dbContext.Remove(id);
        }

        public async Task< IEnumerable<T>> GetAll()
        {

            if(typeof(T)==typeof(Products))
            {
                var model =await dbContext.Products.Include(x=>x.Category).ToListAsync();
                return (IEnumerable<T>)model;
            }
          return  await dbContext.Set<T>().ToListAsync();
        }

        public async Task<T> GetById(int id)
        {
            return  await dbContext.Set<T>().FindAsync(id);
        }

        public void Update(T Model)
        {
            dbContext.Set<T>().Update(Model);
        }
    }
}
