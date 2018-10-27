using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Text;

namespace ObligatorioDA2.Data.DataAccess
{
    public class DatabaseContextFactory : IDesignTimeDbContextFactory<DatabaseConnection>
    {
        DatabaseConnection IDesignTimeDbContextFactory<DatabaseConnection>.CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<DatabaseConnection>();
            optionsBuilder.UseSqlServer("Server=.\\SQLSERVER_R14;Database=ObligatorioDA2;Trusted_Connection=True;Integrated Security=True;");

            return new DatabaseConnection(optionsBuilder.Options);
        }
    }
}
