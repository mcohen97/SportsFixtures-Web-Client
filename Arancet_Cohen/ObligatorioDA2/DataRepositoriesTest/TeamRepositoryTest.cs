using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using Moq;
using DataAccess;
using DataRepositoryInterfaces;
using DataRepositories;
using System.Collections.Generic;
using BusinessLogic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;
using RepositoryInterface;
using ObligatorioDA2.DataAccess.Entities;
using ObligatorioDA2.BusinessLogic.Data.Exceptions;

namespace DataRepositoriesTest
{
    [TestClass]
    public class TeamRepositoryTest
    {
        private IRepository<Team> teamRepo;

        [TestInitialize]
        public void TestInitialize(){
            DbContextOptions<DatabaseConnection> options = new DbContextOptionsBuilder<DatabaseConnection>()
                .UseInMemoryDatabase(databaseName: "TeamRepository")
                .Options;
            DatabaseConnection context = new DatabaseConnection(options);
            teamRepo = new TeamRepository(context);
            ClearDataBase(context);        
        }
    }
}