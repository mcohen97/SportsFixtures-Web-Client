using System;
using System.Collections.Generic;
using System.Text;
using BusinessLogic;
using DataRepositoryInterfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace ObligatorioDA2.Services.Tests
{
    [TestClass]
    public class SportsServiceTest
    {
        private ISportService sportService;
        private Mock<ISportRepository> sportRepository;
        private Sport testSport;

        [TestInitialize]
        public void SetUp() {
            sportRepository = new Mock<ISportRepository>();
            sportService = new SportService(sportRepository.Object);
            testSport = new Sport("Soccer");
        }
        

    }
}
