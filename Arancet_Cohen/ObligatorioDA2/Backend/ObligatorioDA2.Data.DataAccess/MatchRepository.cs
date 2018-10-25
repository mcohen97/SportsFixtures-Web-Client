using ObligatorioDA2.BusinessLogic;
using ObligatorioDA2.Data.DataAccess;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using ObligatorioDA2.Data.DomainMappers;
using ObligatorioDA2.Data.Entities;
using System.Collections.Generic;
using ObligatorioDA2.BusinessLogic.Data.Exceptions;
using ObligatorioDA2.Data.Repositories.Interfaces;
using System.Data.Common;
using System;

namespace ObligatorioDA2.Data.Repositories
{
    public class MatchRepository : IMatchRepository
    {
        private DatabaseConnection context;
        private MatchMapper matchConverter;
        private TeamMapper teamConverter;
        private CommentMapper commentConverter;
        public MatchRepository(DatabaseConnection aContext)
        {
            context = aContext;
            matchConverter = new MatchMapper();
            commentConverter = new CommentMapper();
            teamConverter = new TeamMapper();
        }


        public Encounter Add(Encounter aMatch)
        {
            Match added;
            try
            {
                added = TryAdd(aMatch);
            }
            catch (DbException)
            {
                throw new DataInaccessibleException();
            }
            return added;
        }

        private Match TryAdd(Encounter aMatch)
        {
            Match added;
            if (!Exists(aMatch.Id))
            {
                added = AddNew(aMatch);
            }
            else
            {
                throw new MatchAlreadyExistsException();
            }
            return added;
        }

        private Match AddNew(Encounter aMatch)
        {
            MatchEntity toAdd = matchConverter.ToEntity(aMatch);
            context.Entry(toAdd).State = EntityState.Added;
            Match added = new Match(toAdd.Id, aMatch.GetParticipants(), aMatch.Date, aMatch.Sport);

            ICollection<MatchTeam> playingTeams = matchConverter.ConvertParticipants(added);
            context.MatchTeams.AddRange(playingTeams);

            //We also need to ask if it is an Sql database, so that we can execute the sql scripts.
            if (aMatch.Id > 0 && context.Database.IsSqlServer())
            {
                SaveWithIdentityInsert();
            }
            else
            {
                context.SaveChanges();
            }
            return added;
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
            try
            {
                TryClear();
            }
            catch (DbException)
            {
                throw new DataInaccessibleException();
            }
        }

        private void TryClear()
        {
            foreach (MatchEntity match in context.Matches)
            {
                context.Matches.Remove(match);
            }
            context.SaveChanges();
        }

        public void Delete(int anId)
        {
            try
            {
                TryDelete(anId);
            }
            catch (DbException)
            {
                throw new DataInaccessibleException();
            }
        }

        private void TryDelete(int anId)
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

        public Encounter Get(int anId)
        {
            Encounter toReturn;
            try
            {
                toReturn = TryGet(anId);
            }
            catch (DbException)
            {
                throw new DataInaccessibleException();
            }
            return toReturn;
        }

        private Encounter TryGet(int anId)
        {
            Encounter toReturn;
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

        private Encounter GetExistentMatch(int anId)
        {
            MatchEntity entity = context.Matches
                .Include(m => m.SportEntity)
                .Include(m => m.Commentaries).ThenInclude(c => c.Maker)
                .First(me => me.Id == anId);

            ICollection<MatchTeam> match_teams = context.MatchTeams.Include(mt=> mt.Team)
                                                                   .ThenInclude(t => t.Sport)                                                                             
                                                                   .Where(mt => mt.MatchId == anId)
                                                                   .ToList();
            Encounter conversion = matchConverter.ToMatch(entity,match_teams);
            context.Entry(entity).State = EntityState.Detached;
            return conversion;
        }

        public ICollection<Encounter> GetAll()
        {
            ICollection<Encounter> allOfThem;
            try
            {
                allOfThem = TryGetAll();
            }
            catch (DbException)
            {
                throw new DataInaccessibleException();
            }
            return allOfThem;
        }

        private ICollection<Encounter> TryGetAll()
        {
            ICollection<Encounter> allOfThem = new List<Encounter>();
            IQueryable<MatchEntity> entities = context.Matches
                                            .Include(m => m.SportEntity)
                                            .Include(m => m.Commentaries).ThenInclude(c => c.Maker);

            foreach (MatchEntity match in entities) {
                IQueryable<MatchTeam> matchPlayers = context.MatchTeams
                    .Include(mt => mt.Team)
                    .Where(mt => mt.MatchId == match.Id);
                Encounter built = matchConverter.ToMatch(match, matchPlayers.ToList());
                allOfThem.Add(built);
            }
            return allOfThem;
        }

        public bool IsEmpty()
        {
            bool isEmpty;
            try
            {
                isEmpty = AskIfEmpty();
            }
            catch (DbException)
            {
                throw new DataInaccessibleException();
            }
            return isEmpty;
        }

        private bool AskIfEmpty()
        {
            return !context.Matches.Any();
        }

        public void Modify(Encounter aMatch)
        {
            try
            {
                TryModify(aMatch);
            }
            catch (DbException)
            {
                throw new DataInaccessibleException();
            }
        }

        private void TryModify(Encounter aMatch)
        {
            if (Exists(aMatch.Id))
            {
                ModifyExistent(aMatch);
            }
            else
            {
                throw new MatchNotFoundException();
            }
        }

        public void ModifyExistent(Encounter aMatch)
        {
            MatchEntity converted = matchConverter.ToEntity(aMatch);
            if (context.Matches.Any(m => m.Id == aMatch.Id))
            {
                MatchEntity old = context.Matches.First(m => m.Id == aMatch.Id);
                context.Entry(old).State = EntityState.Detached;
            }

            context.Entry(converted).State = EntityState.Modified;
            context.SaveChanges();
        }

        private bool AnyWithId(int anId)
        {
            return context.Matches.Any(m => m.Id == anId);
        }

        public Commentary CommentOnMatch(int idMatch, Commentary aComment)
        {
            Commentary made;
            try
            {
                made = TryComment(idMatch, aComment);
            }
            catch (DbException)
            {
                throw new DataInaccessibleException();
            }
            return made;
        }

        private Commentary TryComment(int idMatch, Commentary aComment)
        {
            if (!context.Matches.Any(m => m.Id == idMatch))
            {
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
            bool exists;
            try
            {
                exists = AskIfExists(id);
            }
            catch (DbException)
            {
                throw new DataInaccessibleException();
            }
            return exists;
        }

        private bool AskIfExists(int id)
        {
            return context.Matches.Any(m => m.Id == id);
        }

        public ICollection<Commentary> GetComments()
        {
            ICollection<Commentary> allComments;
            try
            {
                allComments = TryGetComments();
            }
            catch (DbException)
            {
                throw new DataInaccessibleException();
            }
            return allComments;
        }

        private ICollection<Commentary> TryGetComments()
        {
            IQueryable<CommentEntity> allComments = context.Comments.Include(c => c.Maker);
            ICollection<Commentary> conversion = allComments.Select(c => commentConverter.ToComment(c)).ToList();
            return conversion;
        }

        public Commentary GetComment(int id)
        {
            Commentary toReturn;
            try
            {
                toReturn = TryGetComment(id);
            }
            catch (DbException)
            {
                throw new DataInaccessibleException();
            }
            return toReturn;
        }

        private Commentary TryGetComment(int id)
        {
            if (!context.Comments.Any(c => c.Id == id))
            {
                throw new CommentNotFoundException();
            }
            CommentEntity retrieved = context.Comments.Include(c => c.Maker).First(c => c.Id == id);
            return commentConverter.ToComment(retrieved);
        }
    }
}