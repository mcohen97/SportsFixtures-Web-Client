﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ObligatorioDA2.Data.DataAccess;

namespace ObligatorioDA2.WebAPI.Test
{
    [Route("api/[controller]")]
    public class TestController : Controller
    {
        private DatabaseConnection context;
        public TestController(DatabaseConnection connection) {
            context = connection;
        }
  
        [HttpPost]
        public void ResetDatabase()
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            context.Database.ExecuteSqlCommand("INSERT INTO Users VALUES('admin','admin','admin','admin','admin@admin.com',1);");
            context.Database.ExecuteSqlCommand("INSERT INTO Users VALUES('name','surname','username','password','mail@domain.com',0);");
        }

    }
}
