using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic
{
    public abstract class User
    {
        private string name;

        public string Name { get { return name; } set { SetName(value); } }

        public User(string aName, string aSurname, string aUserName, string aPassword, string aEmail) {
            
        }

        private void SetName(string aName)
        {
            if (String.IsNullOrWhiteSpace(aName)) {
               
            }
            name = aName;
        }
    }
}
