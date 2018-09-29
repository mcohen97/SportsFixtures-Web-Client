﻿using BusinessLogic;
using DataAccess;
using DataRepositoryInterfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using ObligatorioDA2.DataAccess.Domain.Mappers;
using ObligatorioDA2.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using ObligatorioDA2.BusinessLogic.Data.Exceptions;

namespace DataRepositories
{
    public class MatchRepository : IMatchRepository
    {
        private DatabaseConnection context;
        private MatchMapper matchConverter;
        private CommentMapper commentConverter;
        public MatchRepository(DatabaseConnection aContext)
        {
            context = aContext;
            matchConverter = new MatchMapper();
            commentConverter = new CommentMapper();
        }


        public Match Add(string sportName, Match aMatch)
        {
            Match created;
            if (!Exists(aMatch.Id))
            {
                created = TryAdd(sportName, aMatch);
            }
            else {
                throw new MatchAlreadyExistsException();
            }
            return created;
        }

        private Match TryAdd(string sportName, Match aMatch)
        {
            MatchEntity toAdd = matchConverter.ToEntity(aMatch);
            toAdd.HomeTeam.SportEntityName = sportName;
            toAdd.AwayTeam.SportEntityName = sportName;
            context.Entry(toAdd).State = EntityState.Added;
            context.SaveChanges();
            return new Match(toAdd.Id, aMatch.HomeTeam, aMatch.AwayTeam, aMatch.Date, aMatch.Sport);
        }

        public void Clear()
        {
            foreach (MatchEntity match in context.Matches)
            {
                context.Matches.Remove(match);
            }
            context.SaveChanges();
        }

        public void Delete(int anId)
        {
            if (AnyWithId(anId))
            {
                DeleteExistent(anId);
            }
            else
            {
                throw new MatchNotFoundException();
            }
        }

        private void DeleteExistent(int anId)
        {
            MatchEntity retrieved = context.Matches.Include(m => m.Commentaries).First(m => m.Id == anId);
            context.Matches.Remove(retrieved);
            context.Comments.RemoveRange(retrieved.Commentaries);
            context.SaveChanges();
        }

        public bool Exists(Match record)
        {
            return AnyWithId(record.Id);
        }

        public Match Get(int anId)
        {
            Match toReturn;
            if (AnyWithId(anId))
            {
                toReturn = GetExistentMatch(anId);
            }
            else
            {
                throw new MatchNotFoundException();
            }
            return toReturn;
        }

        private Match GetExistentMatch(int anId)
        {
            MatchEntity entity = context.Matches.First(me => me.Id == anId);
            Match conversion = matchConverter.ToMatch(entity);
            return conversion;
        }

        public ICollection<Match> GetAll()
        {
            IQueryable<MatchEntity> entities = context.Matches;
            ICollection<Match> translation = entities.Select(m => matchConverter.ToMatch(m)).ToList();
            return translation;
        }

        public bool IsEmpty()
        {
            return !context.Matches.Any();
        }

        public void Modify(Match aMatch)
        {
            if (Exists(aMatch))
            {
                ModifyExistent(aMatch);
            }
            else
            {
                throw new MatchNotFoundException();
            }
        }

        private void ModifyExistent(Match aMatch)
        {
            MatchEntity toAdd = matchConverter.ToEntity(aMatch);
            context.Entry(toAdd).State = EntityState.Modified;
            context.SaveChanges();
        }

        private bool AnyWithId(int anId)
        {
            return context.Matches.Any(m => m.Id == anId);
        }

        public Commentary CommentOnMatch(int idMatch, Commentary aComment)
        {
            CommentEntity comment = commentConverter.ToEntity(aComment);
            MatchEntity commented = context.Matches.Include(m => m.Commentaries).First(m => m.Id == idMatch);
            commented.Commentaries.Add(comment);
            context.Attach(comment).State = EntityState.Added;
            var x = context.Entry(comment.Maker).State;
            context.SaveChanges();
            return commentConverter.ToComment(comment);
        }

        public bool Exists(int id)
        {
            return context.Matches.Any(m => m.Id == id);
        }

        public void Modify(string sportName, Match aMatch)
        {
            if (Exists(aMatch.Id))
            {
                MatchEntity converted = matchConverter.ToEntity(aMatch);
                converted.SportEntity = new SportEntity { Name = sportName };
                context.Matches.Attach(converted).State = EntityState.Modified;
                context.SaveChanges();
            }
            else {
                throw new MatchNotFoundException();
            }
        }
    }
}