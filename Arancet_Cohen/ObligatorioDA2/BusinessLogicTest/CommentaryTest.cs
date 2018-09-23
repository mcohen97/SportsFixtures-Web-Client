using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BusinessLogic;
using System.Collections.Generic;
using Moq;
using BusinessLogic.Exceptions;

namespace BusinessLogicTest
{
    [TestClass]
    public class CommentaryTest
    {
        private Commentary commentary;
        private int id;
        private string text;
        
        [TestInitialize]
        public void TestInitialize(){
            id = 1;
            text = "The match was so boring";
            commentary = new Commentary(id, text);
        }

        [TestMethod]
        public void ConstructorTest(){
            Assert.IsNotNull(commentary);
        }

         [TestMethod]
        public void GetIdTest(){
            Assert.AreEqual(id, commentary.Id);
        }

        [TestMethod]
        public void SetIdTest(){
            int newId = 2;
            commentary.Id = newId;
            Assert.AreEqual(newId, commentary.Text);
        }

        [TestMethod]
        public void GetTextTest(){
            Assert.AreEqual(text, commentary.Text);
        }

        [TestMethod]
        public void SetTextTest(){
            string newText = "The match was incredible";
            commentary.Text = newText;
            Assert.AreEqual(newText, commentary.Text);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCommentaryDataException))]
        public void SetNullTextTest()
        {
            commentary.Text = null;
        }
    }
}