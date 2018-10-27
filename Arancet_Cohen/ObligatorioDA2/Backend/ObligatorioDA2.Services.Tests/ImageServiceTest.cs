using Microsoft.VisualStudio.TestTools.UnitTesting;
using ObligatorioDA2.Services.Interfaces;
using System;
using System.IO;
using System.Text;

namespace ObligatorioDA2.Services.Tests
{
    [TestClass]
    public class ImageServiceTest
    {
        private IImageService service;
        private string directoryPath;
        private string testImagePath;
        private string testImage;

        [TestInitialize]
        public void SetUp() {
            directoryPath = "TestDirectory";
            service = new ImageService(directoryPath);
            testImagePath = directoryPath + "/" + "testImage.jpg";
            testImage = "asdasdasdadad"; 
        }

        [TestMethod]
        public void TestSaveImage() {
            SaveTestImage();
            Assert.IsTrue(File.Exists(testImagePath));
        }

        private string Base64Encode(string plainText)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        [TestMethod]
        public void TestReadImage() {
            SaveTestImage();
            byte[] bytes= service.ReadImage("testImage");
            string actual = Convert.ToBase64String(bytes);
            string expected = Base64Encode(testImage);
        }

        private void SaveTestImage()
        {
            service.SaveImage("testImage", Base64Encode(testImage));
        }
    }
}
