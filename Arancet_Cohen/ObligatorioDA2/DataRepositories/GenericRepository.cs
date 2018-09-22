﻿using DataAccess;
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
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public void Delete(T entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public bool Exists(T record)
        {
            throw new NotImplementedException();
        }

        public T First(Expression<Func<T, bool>> predicate)
        {
            throw new NotImplementedException();
        }

    

        public ICollection<T> Get(Expression<Func<T, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public T Get(T asked)
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
