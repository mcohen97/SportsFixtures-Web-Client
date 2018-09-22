using System;

namespace ObligatorioDA2.DataAccess.Entities
{
    public class UserEntity:BaseEntity
    {
        public UserEntity()
        { 
        }
        public string Name { get; set; }

        public string Surname { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public bool IsAdmin { get; set; }
    }
}
