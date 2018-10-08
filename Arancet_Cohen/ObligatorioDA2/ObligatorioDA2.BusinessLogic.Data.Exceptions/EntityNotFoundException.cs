using System;
using System.Collections.Generic;
using System.Text;

namespace ObligatorioDA2.BusinessLogic.Data.Exceptions
{
    public abstract class EntityNotFoundException:DataAccessException
    {
        public EntityNotFoundException(string aMessage) : base(aMessage) { }

    }
}
