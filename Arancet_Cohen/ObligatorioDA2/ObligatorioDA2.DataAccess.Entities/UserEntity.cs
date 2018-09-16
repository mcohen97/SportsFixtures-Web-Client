using System;

namespace ObligatorioDA2.DataAccess.Entities
{
    public class UserEntity
    {
        public int Id { get; private set; }
        public string Name { get; private set; }

        public string Surname { get; private set; }

        public string UserName { get; private set; }

        public string Password { get; private set; }

        public string Email { get; private set; }

        public bool IsAdmin { get; protected set; }
    }
}
