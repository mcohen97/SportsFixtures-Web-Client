using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic
{
    public class Admin:User
    {
        public override bool IsAdmin { get; set; }

        public Admin(string aName, string aSurname, string aUserName, string aPassword, string aEmail) : base(aName,aSurname, aUserName,aPassword, aEmail) {
            IsAdmin = true;
        }

        
    }
}
