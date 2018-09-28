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
using System.Linq;
using DataRepositoryInterfaces;

namespace DataRepositories
{
    public class SportRepository : IRepository<Sport>, ISportRepository
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
            if (!Exists(id))
                throw new SportNotFoundException();

            SportEntity sportInDb = context.Sports.First(s => s.Id == id);
            context.Sports.Remove(sportInDb);
            context.SaveChanges();
        }

        private bool Exists(int id)
        {
            return context.Sports.Any(s => s.Id == id);
        }

        public bool Exists(Sport record)
        {
            return Exists(record.Id);
        }

        public Sport Get(int id)
        {
            if (!Exists(id))
                throw new SportNotFoundException();

            SportEntity sportInDb = context.Sports.First(s => s.Id == id);
            Sport domainSport = mapper.ToSport(sportInDb);
            return domainSport;
        }

        public ICollection<Sport> GetAll()
        {
            IQueryable<Sport> query = context.Sports.Select(s => mapper.ToSport(s));
            return query.ToList();
        }

        public bool IsEmpty()
        {
            return !context.Sports.Any();
        }

        public void Modify(Sport entity)
        {
            if (!Exists(entity))
                throw new SportNotFoundException();

            SportEntity modified = mapper.ToEntity(entity);
            context.Entry(modified).State = EntityState.Modified;
            context.SaveChanges();
        }

        public Sport GetSportByName(string name)
        {
            if (!Exists(name))
                throw new SportNotFoundException();

            SportEntity sportInDb = context.Sports.First(s => s.Name == name);
            return mapper.ToSport(sportInDb);
        }

        private bool Exists(string name)
        {
            return context.Sports.Any(s => s.Name == name);
        }
    }
}
