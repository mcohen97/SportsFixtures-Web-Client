using System;
using System.Collections.Generic;
using System.Text;

namespace ObligatorioDA2.BusinessLogic.Data.Exceptions
{
    public class CommentAlreadyExistsException: EntityAlreadyExistsException
    {
        public CommentAlreadyExistsException():base("Comment with the same Id already exists") {
        }
    }
}
