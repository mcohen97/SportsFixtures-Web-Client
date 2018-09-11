using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic
{
    public class Admin:User
    {
        public Admin(string aName, string aSurname, string aUserName, string aPassword, string aEmail) : base(aName,aSurname, aUserName,aPassword, aEmail) {
        }
        public override bool IsAdmin() {
            return true;
        }

    }
}
