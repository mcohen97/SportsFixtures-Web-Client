using ObligatorioDA2.BusinessLogic;
using DataAccess;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using ObligatorioDA2.DataAccess.Domain.Mappers;
using ObligatorioDA2.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using ObligatorioDA2.BusinessLogic.Data.Exceptions;
using ObligatorioDA2.Data.Repositories.Interfaces;

namespace ObligatorioDA2.Data.Repositories
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


        public Match Add(Match aMatch)
        {
            Match added;
            if (!Exists(aMatch.Id))
            {
                added = TryAdd(aMatch);
            }
            else {
                throw new MatchAlreadyExistsException();
            }
            return added;
        }

        private Match TryAdd(Match aMatch)
        {
            MatchEntity toAdd = matchConverter.ToEntity(aMatch);
            context.Entry(toAdd).State = EntityState.Added;

            //We also need to ask if it is an Sql database, so that we can execute the sql scripts.
            if (aMatch.Id > 0 && context.Database.IsSqlServer())
            {
                SaveWithIdentityInsert();
            }
            else
            {
                context.SaveChanges();
            }
            return matchConverter.ToMatch(toAdd);
        }

        private void SaveWithIdentityInsert()
        {
            context.Database.OpenConnection();
            try
            {
                context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT dbo.Matches ON");
                context.SaveChanges();
                context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT dbo.Matches OFF");
            }
            finally
            {
                context.Database.CloseConnection();
            }
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
            MatchEntity entity = context.Matches
                .Include(m=>m.HomeTeam)
                .Include(m=>m.AwayTeam)
                .Include(m=>m.SportEntity)
                .Include(m => m.Commentaries).ThenInclude(c => c.Maker)
                .First(me => me.Id == anId);

            Match conversion = matchConverter.ToMatch(entity);
            context.Entry(entity).State = EntityState.Detached;
            return conversion;
        }

        public ICollection<Match> GetAll()
        {
            IQueryable<MatchEntity> entities = context.Matches
                .Include(m => m.HomeTeam)
                .Include(m => m.AwayTeam)
                .Include(m => m.SportEntity)
                .Include(m => m.Commentaries).ThenInclude(c => c.Maker);

            ICollection<Match> translation = entities.Select(m => matchConverter.ToMatch(m)).ToList();

            return translation;
        }

        public bool IsEmpty()
        {
            return !context.Matches.Any();
        }

        public void Modify(Match aMatch)
        {
            if (Exists(aMatch.Id))
            {
                MatchEntity converted = matchConverter.ToEntity(aMatch);
                if (context.Matches.Any(m => m.Id == aMatch.Id)) {
                    MatchEntity old = context.Matches.First(m => m.Id == aMatch.Id);
                    context.Entry(old).State = EntityState.Detached;
                }

                context.Entry(converted).State = EntityState.Modified;
                context.SaveChanges();
            }
            else
            {
                throw new MatchNotFoundException();
            }
        }

        private bool AnyWithId(int anId)
        {
            return context.Matches.Any(m => m.Id == anId);
        }

        public Commentary CommentOnMatch(int idMatch, Commentary aComment)
        {
            if (!context.Matches.Any(m => m.Id == idMatch)) {
                throw new MatchNotFoundException();
            }
            return CommentOnExistingMatch(idMatch, aComment);
        }

        private Commentary CommentOnExistingMatch(int idMatch, Commentary aComment)
        {
            CommentEntity comment = commentConverter.ToEntity(aComment);
            MatchEntity commented = context.Matches.Include(m => m.Commentaries).First(m => m.Id == idMatch);
            commented.Commentaries.Add(comment);
            context.Attach(comment).State = EntityState.Added;
            context.SaveChanges();
            return commentConverter.ToComment(comment);
        }

        public bool Exists(int id)
        {
            return context.Matches.Any(m => m.Id == id);
        }

        public ICollection<Commentary> GetComments()
        {
            IQueryable<CommentEntity> allComments = context.Comments.Include(c => c.Maker);
            ICollection<Commentary> conversion = allComments.Select(c => commentConverter.ToComment(c)).ToList();
            return conversion;
        }

        public Commentary GetComment(int id)
        {
            if (!context.Comments.Any(c => c.Id == id)) {
                throw new CommentNotFoundException();
            }
            CommentEntity retrieved = context.Comments.Include(c =>c.Maker).First(c => c.Id == id);
            return commentConverter.ToComment(retrieved);
        }
    }
}