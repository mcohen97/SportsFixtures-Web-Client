using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess
{
    class DatabaseConnection:DbContext
    {
        public virtual DbContext Users { get; set; }
        public DatabaseConnection(DbContextOptions<DatabaseConnection> options) : base(options)
        {
        }

    }
}
