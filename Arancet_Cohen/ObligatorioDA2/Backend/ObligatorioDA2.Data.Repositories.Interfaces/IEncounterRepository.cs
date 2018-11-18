using System;
using System.Collections.Generic;
using System.Text;
using ObligatorioDA2.BusinessLogic;

namespace ObligatorioDA2.Data.Repositories.Interfaces
{
    public interface IEncounterRepository : IRepository<Encounter, int>
    {

        Commentary CommentOnEncounter(int idMatch, Commentary aComment);
        Commentary GetComment(int id);
        ICollection<Commentary> GetComments();
    }
}
