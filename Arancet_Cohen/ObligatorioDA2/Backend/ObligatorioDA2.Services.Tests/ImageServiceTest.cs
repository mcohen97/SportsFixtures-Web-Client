using Microsoft.VisualStudio.TestTools.UnitTesting;
using ObligatorioDA2.Services.Interfaces;
using System.IO;

namespace ObligatorioDA2.Services.Tests
{
    [TestClass]
    public class ImageServiceTest
    {
        private IImageService service;
        string directoryPath;

        [TestInitialize]
        public void SetUp() {
            directoryPath = "TestDirectory";
            service = new ImageService(directoryPath);
        }

        [TestMethod]
        public void TestSaveImage() {
            string img = "asdasdasdadad";
            service.SaveImage("testImage", Base64Encode(img));
            string imagePath= directoryPath +"/"+ "testImage.jpg";
            Assert.IsTrue(File.Exists(imagePath));
        }

        private string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
    }
}
