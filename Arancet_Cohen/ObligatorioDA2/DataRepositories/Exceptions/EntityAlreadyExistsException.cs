using System;
using System.Collections.Generic;
using System.Text;

namespace DataRepositories.Exceptions
{
    public class EntityAlreadyExistsException:Exception
    {
        public EntityAlreadyExistsException(string msg) : base(msg) {
        }
    }
}
