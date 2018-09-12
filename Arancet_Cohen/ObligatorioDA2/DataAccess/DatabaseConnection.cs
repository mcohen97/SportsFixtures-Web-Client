using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using BusinessLogic;

namespace DataAccess
{
    public class DatabaseConnection:DbContext
    {
        public virtual DbSet<User> Users { get; set; }
        public DatabaseConnection(DbContextOptions<DatabaseConnection> options) : base(options)
        {
        }
    }
}
