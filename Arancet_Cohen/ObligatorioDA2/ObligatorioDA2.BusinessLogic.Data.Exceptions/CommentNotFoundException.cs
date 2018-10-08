using System;
using System.Collections.Generic;
using System.Text;

namespace ObligatorioDA2.BusinessLogic.Data.Exceptions
{
    public class CommentNotFoundException:DataAccessException
    {
        public CommentNotFoundException():base("Couldn't find comment") {

        }

    }
}
