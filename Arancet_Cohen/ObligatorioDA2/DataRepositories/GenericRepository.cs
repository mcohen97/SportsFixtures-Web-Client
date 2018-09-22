using DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using ObligatorioDA2.DataAccess.Entities;
using RepositoryInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;


namespace DataRepositories
{
    public class GenericRepository<T> : IEntityRepository<T> where T : BaseEntity
    {
        private readonly DatabaseConnection context;
        public GenericRepository(DatabaseConnection aContext)
        {
            context = aContext;
        }
        public void Add(T entity)
        {
            context.Set<T>().Add(entity);
            context.SaveChanges();
        }

        public T Get(int id)
        {
            return context.Set<T>().First(u=> u.Id == id);
        }
    }
}
