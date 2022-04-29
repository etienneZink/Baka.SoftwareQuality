using Baka.ContactSplitter.model;
using Baka.ContactSplitter.services.implementations;
using Baka.ContactSplitter.services.interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Baka.ContactSplitter.Test
{
    [TestClass]
    public class ParserServiceTests
    {
        //Tests without titles
        [TestMethod]
        [DataRow("Frau Reinhilde Zufall", "Frau", "Reinhilde", "Zufall")]
        [DataRow("Mrs. Reinhilde Zufall", "Mrs.", "Reinhilde", "Zufall")]
        [DataRow("Ms. Reinhilde Zufall", "Ms.", "Reinhilde", "Zufall")]
        [DataRow("Mmd. Reinhilde Zufall", "Mmd.", "Reinhilde", "Zufall")]
        [DataRow("Herr Reiner   Zufall", "Herr", "Reiner", "Zufall")]
        [DataRow("Mr.   Reiner Zufall", "Mr.", "Reiner", "Zufall")]
        [DataRow("Sir Reiner Zufall", "Sir", "Reiner", "Zufall")]
        [DataRow("Frau   Zufall  ,   Reinhilde", "Frau", "Reinhilde", "Zufall")]
        [DataRow("Frau Reinhilde  Gertrut   Zufall", "Frau", "Reinhilde Gertrut", "Zufall")]
        [DataRow("Frau Reinhilde Zufall-Experiment", "Frau", "Reinhilde", "Zufall-Experiment")]
        [DataRow("Frau    Reinhilde    von   Zufall", "Frau", "Reinhilde", "von Zufall")]
        [DataRow("Frau Reinhilde   von   dem   Zufall", "Frau", "Reinhilde", "von dem Zufall")]
        public void ParserService_ParseContact_Success(string contactString,
            string salutation, string firstName, string lastName)
        {
            // Arrange
            var parser = InitializeParser();

            // Act
            var parseResult = parser.ParseContact(contactString);

            // Assert
            Assert.IsNotNull(parseResult);
            Assert.IsTrue(parseResult.Successful);
            Assert.AreEqual(salutation, parseResult.Model.Salutation);
            Assert.AreEqual(firstName, parseResult.Model.FirstName);
            Assert.AreEqual(lastName, parseResult.Model.LastName);
            Assert.IsTrue(parseResult.Model.Titles.Count == 0);
        }

        private IParserService InitializeParser()
        {
            var titleService = new TitleService();
            var salutationService = new SalutationService();

            return new ParserService(titleService, salutationService);
        }
    }
}
