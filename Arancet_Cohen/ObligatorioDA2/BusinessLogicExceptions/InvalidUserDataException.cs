using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Exceptions
{
    public class InvalidUserDataException:Exception
    {
        public InvalidUserDataException(string message) : base(message) {

        }
    }
}
