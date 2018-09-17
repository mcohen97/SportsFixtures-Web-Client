using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using DataRepositoryInterfaces;
using DataAccess;
using BusinessLogic;
using RepositoryInterface;
using Microsoft.EntityFrameworkCore;
using ObligatorioDA2.DataAccess.Entities;
using ObligatorioDA2.DataAccess.Domain.Mappers;
using ObligatorioDA2.BusinessLogic.Data.Exceptions;

namespace DataRepositories
{
    public class TeamRepository : ITeamRepository, IRepository<Team>
    {
        private DatabaseConnection context;
        private readonly TeamMapper mapper;

        public TeamRepository(DatabaseConnection context)
        {
            this.context = context;
            this.mapper = new TeamMapper();
        }

        public void Add(Team entity)
        {
            if(Exists(entity))
                throw new TeamAlreadyExistsException();

            TeamEntity convertedTeam = mapper.ToEntity(entity);
            context.Teams.Add(convertedTeam);
            context.SaveChanges();
        }

        public void Clear()
        {
            foreach(TeamEntity team in context.Teams){
                context.Teams.Remove(team);
            }
            context.SaveChanges();
        }

        public void Delete(Team entity)
        {
            if(!Exists(entity))
                throw new TeamNotFoundException();

            TeamEntity toDelete = context.Teams.First(t => t.Name == entity.Name);
            context.Teams.Remove(toDelete);
            context.SaveChanges();
        }

        public bool Exists(Team record)
        {
            return Exists(record.Name);
        }

         private bool Exists(string name){
             return context.Teams.Any(t => t.Name == name);
        }

        public Team Get(Guid id)
        {
            throw new NotImplementedException();
        }

        public Team Get(Team asked) 
        {   
            return GetTeamByName(asked.Name);
        }

        public ICollection<Team> GetAll()
        {
            IQueryable<Team> query = context.Teams.Select(t => mapper.ToTeam(t));
            return query.ToList();
        }

        public Team GetTeamByName(string name) 
        {
            if(!Exists(name))
                throw new TeamNotFoundException();

            TeamEntity askedEntity = context.Teams.First(t => t.Name == name);
            return mapper.ToTeam(askedEntity);
        }

        public bool IsEmpty()
        {
            return !context.Teams.Any();
        }

        public void Modify(Team entity)
        {
            if(!Exists(entity))
                throw new TeamNotFoundException();
            
            TeamEntity modified = mapper.ToEntity(entity);
            context.Teams.Attach(modified);
            context.Entry(modified).State = EntityState.Modified;
            context.SaveChanges();
        }
    }
}