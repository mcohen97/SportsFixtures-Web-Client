using ObligatorioDA2.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Text;

namespace ObligatorioDA2.Services
{
    public interface IInnerMatchService
    {
        Encounter AddMatch(Encounter anEncounter);
        ICollection<Encounter> GetAllMatches(string sportName);

    }
}
