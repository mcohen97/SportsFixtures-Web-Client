using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogic.Factories
{
    public class UserFactory
    {
       
        public User CreateAdmin(UserId identity)
        {
            return new User(identity, true);
        }

        public User CreateAdmin(UserId identity, int id)
        {
            return new User(identity, true,id);
        }

        public User CreateFollower(UserId identity)
        {
            return new User(identity, false);
        }

        public User CreateFollower(UserId identity, int id)
        {
            return new User(identity, false,id);
        }
    }
}
