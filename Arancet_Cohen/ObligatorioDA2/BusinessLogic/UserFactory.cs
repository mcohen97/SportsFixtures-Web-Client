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

        public User CreateFollower(UserId identity)
        {
            return new User(identity, false);
        }

    }
}
