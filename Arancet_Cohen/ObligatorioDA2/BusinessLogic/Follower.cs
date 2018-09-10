using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic
{
    public class Follower : User
    {
        public override bool IsAdmin { get; set; }
        public Follower(string aName, string aSurname, string aUserName, string aPassword, string anEmail) : base(aName, aSurname, aUserName, aPassword, anEmail)
        {

        }
    }
}
