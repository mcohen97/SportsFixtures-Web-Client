using System;
using System.Collections.Generic;
using System.Text;
using DataAccess;
using RepositoryInterface;
using BusinessLogic;
using ObligatorioDA2.DataAccess.Domain.Mappers;
using ObligatorioDA2.BusinessLogic.Data.Exceptions;
using ObligatorioDA2.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataRepositories
{
    public class SportRepository : IRepository<Sport>
    {
        private DatabaseConnection context;
        private SportMapper mapper;

        public SportRepository(DatabaseConnection context)
        {
            this.context = context;
            mapper = new SportMapper();
        }

        public void Add(Sport sport)
        {
            if (Exists(sport))
                throw new SportAlreadyExistsException();

            SportEntity entity = mapper.ToEntity(sport);
            context.Entry(entity).State = EntityState.Added;
            context.SaveChanges();
        }

        public void Clear()
        {
            foreach (SportEntity sport in context.Sports)
            {
                context.Sports.Remove(sport);
            }
            context.SaveChanges();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public bool Exists(Sport record)
        {
            throw new NotImplementedException();
        }

        public Sport Get(int id)
        {
            throw new NotImplementedException();
        }

        public ICollection<Sport> GetAll()
        {
            throw new NotImplementedException();
        }

        public bool IsEmpty()
        {
            throw new NotImplementedException();
        }

        public void Modify(Sport entity)
        {
            throw new NotImplementedException();
        }
    }
}
