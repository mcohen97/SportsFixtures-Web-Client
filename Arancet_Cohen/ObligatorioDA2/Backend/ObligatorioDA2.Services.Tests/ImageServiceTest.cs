using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ObligatorioDA2.Services.Tests
{
    [TestClass]
    public class ImageServiceTest
    {
        private IImageService service;

        [TestInitialize]
        public void SetUp() {
            service = new ImageService()
        }
    }
}
