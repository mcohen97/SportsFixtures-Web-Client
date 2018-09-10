using System;
using BusinessLogicExceptions;
using System.Net.Mail;

namespace BusinessLogic
{
    public abstract class User
    {
        private string name;

        public string Name { get { return name; } set { SetName(value); } }

        private string surname;

        public string Surname { get { return surname; } set { SetSurname(value); } }

        private string userName;

        public string UserName { get { return userName; } set { SetUserName(value); } }

        private string password;
        public string Password { get { return password; } set { SetPassword(value); } }

        private string email;
        //public string Email { get { return email; } set { SetEmail(value); } }

        public string Email { get; set; }

        public User(string aName, string aSurname, string aUserName, string aPassword, string aEmail)
        {
            Name = aName;
            Surname = aSurname;
            UserName = aUserName;
            Password = aPassword;

        }

        private void SetName(string aName)
        {
            if (String.IsNullOrWhiteSpace(aName))
            {
                throw new InvalidUserDataException("Invalid name format");
            }
            name = aName;
        }

        private void SetSurname(string aSurname)
        {
            if (String.IsNullOrWhiteSpace(aSurname))
            {
                throw new InvalidUserDataException("Invalid surname format");
            }
            surname = aSurname;
        }

        private void SetUserName(string aUserName)
        {
            if (string.IsNullOrWhiteSpace(aUserName))
            {
                throw new InvalidUserDataException("Invalid user name format");
            }
            userName = aUserName;
        }


        private void SetPassword(string aPassword)
        {
            if (string.IsNullOrWhiteSpace(aPassword))
            {
                throw new InvalidUserDataException("Invalid password format");
            }
            password = aPassword;
        }

        
    }
}
