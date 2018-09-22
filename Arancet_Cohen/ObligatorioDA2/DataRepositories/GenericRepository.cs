using DataAccess;
using DataRepositories.Exceptions;
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
            if (context.Set<T>().Contains(entity))
            {
                throw new EntityAlreadyExistsException();
            }
            else {
                context.Set<T>().Add(entity);
                context.SaveChanges();
            }
        }

        public T Get(int id)
        {
            T toReturn;
            if (context.Set<T>().Any(u => u.Id == id))
            {
                toReturn = context.Set<T>().First(u => u.Id == id);
            }
            else {
                throw new EntityNotFoundException();
            }
            return toReturn;
        }

        public bool IsEmpty()
        {
            return !context.Set<T>().Any();
        }

        public bool Any(Expression<Func<T, bool>> predicate)
        {
            return context.Set<T>().Any(predicate);
        }

        public void Clear()
        {
            DbSet<T> set = context.Set<T>();
            foreach (T entity in set) {
                set.Remove(entity);
            }
            context.SaveChanges();
        }


        public void Delete(int id)
        {
            if (AnyWithId(id))
            {
                T toDelete = context.Set<T>().First(e => e.Id == id);
                context.Set<T>().Remove(toDelete);
                context.SaveChanges();
            }
            else {
                throw new EntityNotFoundException();
            }
        }

        public bool Exists(T record)
        {
            return AnyWithId(record.Id);
        }

        private bool AnyWithId(int anId) {
            return context.Set<T>().Any(e => e.Id == anId);
        }

        public T First(Expression<Func<T, bool>> predicate)
        {
            T firstToComply;
            if (Any(predicate))
            {
                firstToComply = context.Set<T>().First(predicate);
            }
            else {
                throw new EntityNotFoundException();
            }
            return firstToComply;
        }


        public ICollection<T> Get(Expression<Func<T, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public ICollection<T> GetAll()
        {
            throw new NotImplementedException();
        }

        public void Modify(T entity)
        {
            throw new NotImplementedException();
        }
    }
}
