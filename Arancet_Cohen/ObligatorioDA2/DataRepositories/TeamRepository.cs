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

        public void Add(Team team)
        {
            if(Exists(team))
                throw new TeamAlreadyExistsException();

            TeamEntity entityTeam = mapper.ToEntity(team);
            context.Teams.Add(entityTeam);
            context.SaveChanges();
        }

        public void Clear()
        {
            foreach(TeamEntity team in context.Teams){
                context.Teams.Remove(team);
            }
            context.SaveChanges();
        }

        public void Delete(int id)
        {
            if(!Exists(id))
                throw new TeamNotFoundException();

            TeamEntity toDelete = context.Teams.First(t => t.Id == id);
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

        private bool Exists(int id){
            return context.Teams.Any(t => t.Id == id);
        }

        public Team Get(int id)
        {
            if(!Exists(id))
                throw new TeamNotFoundException();

            TeamEntity askedEntity = context.Teams.First(t => t.Id == id);
            return mapper.ToTeam(askedEntity);
        }

        public Team Get(Team team) 
        {   
            return GetTeamByName(team.Name);
        }

        public ICollection<Team> GetAll()
        {
            IQueryable<Team> query = context.Teams.Select(t => mapper.ToTeam(t));
            return query.ToList();
        }

        public Team GetTeamByName(string teamName) 
        {
            if(!Exists(teamName))
                throw new TeamNotFoundException();

            TeamEntity askedEntity = context.Teams.First(t => t.Name == teamName);
            return mapper.ToTeam(askedEntity);
        }

        public bool IsEmpty()
        {
            return !context.Teams.Any();
        }

        public void Modify(Team teamModified)
        {
            if(!Exists(teamModified))
                throw new TeamNotFoundException();
            
            TeamEntity entityModified = mapper.ToEntity(teamModified);
            TeamEntity recordInDB = context.Teams.First(t => t.Name == teamModified.Name);
            entityModified.Id = recordInDB.Id;
            context.Entry(recordInDB).CurrentValues.SetValues(entityModified);
            context.SaveChanges();
        }

        public void Delete(Team teamToDelete)
        {
            if(!Exists(teamToDelete))
                throw new TeamNotFoundException();

            TeamEntity recordToDelete = context.Teams.First(t => t.Name == teamToDelete.Name);
            context.Teams.Remove(recordToDelete);
            context.SaveChanges();
        }
    }
}