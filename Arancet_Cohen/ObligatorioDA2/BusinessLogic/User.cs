using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLogicExceptions;

namespace BusinessLogic
{
    public abstract class User
    {
        private string name;

        public string Name { get { return name; } set { SetName(value); } }

        private string surname;

        public string Surname { get {return surname } set { SetSurname(value); } }

        public User(string aName, string aSurname, string aUserName, string aPassword, string aEmail) {
           Name = aName;

        }

        private void SetName(string aName)
        {
            if (String.IsNullOrWhiteSpace(aName)) {
                throw new InvalidUserDataException("Invalid name format");
            }
            name = aName;
        }

        private void SetSurname(string aSurname)
        {
            aSurname = "";
        }
    }
}
