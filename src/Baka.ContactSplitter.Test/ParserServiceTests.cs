using System;
using System.Collections.Generic;
using System.Linq;
using Baka.ContactSplitter.model;
using Baka.ContactSplitter.services.implementations;
using Baka.ContactSplitter.services.interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Baka.ContactSplitter.Test
{
    [TestClass]
    public class ParserServiceTests
    {
        /// <summary>
        /// These tests are successful tests.
        /// Each test is considered as additive to one another.
        /// If two modifications work with the base test case (first case), then these two are considered
        /// to work with each other.
        /// Each test case itself represents a equality class.
        /// </summary>
        /// <param name="contactString"></param>
        /// <param name="salutation"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="titles"></param>
        [TestMethod]
        [DataRow("Frau Reinhilde Zufall", "Frau", "Reinhilde", "Zufall", "")]
        [DataRow("Mrs. Reinhilde Zufall", "Mrs.", "Reinhilde", "Zufall", "")]
        [DataRow("Ms. Reinhilde Zufall", "Ms.", "Reinhilde", "Zufall", "")]
        [DataRow("Mmd. Reinhilde Zufall", "Mmd.", "Reinhilde", "Zufall", "")]
        [DataRow("Herr Reiner   Zufall", "Herr", "Reiner", "Zufall", "")]
        [DataRow("Mr.   Reiner Zufall", "Mr.", "Reiner", "Zufall", "")]
        [DataRow("Sir Reiner Zufall", "Sir", "Reiner", "Zufall", "")]
        [DataRow("Frau   Zufall  ,   Reinhilde", "Frau", "Reinhilde", "Zufall", "")]
        [DataRow("Frau Reinhilde  Gertrut   Zufall", "Frau", "Reinhilde Gertrut", "Zufall", "")]
        [DataRow("Frau Reinhilde Zufall-Experiment", "Frau", "Reinhilde", "Zufall-Experiment", "")]
        [DataRow("Frau    Reinhilde    von   Zufall", "Frau", "Reinhilde", "von Zufall", "")]
        [DataRow("Frau Reinhilde   von   dem   Zufall", "Frau", "Reinhilde", "von dem Zufall", "")]
        [DataRow("Herr Professor Reiner   Zufall", "Herr", "Reiner", "Zufall", "Professor")]
        [DataRow("Frau Professorin Reinhilde Zufall", "Frau", "Reinhilde", "Zufall", "Professorin")]
        [DataRow("Frau Prof. Reinhilde Zufall", "Frau", "Reinhilde", "Zufall", "Prof.")]
        [DataRow("Frau Dr. Reinhilde Zufall", "Frau", "Reinhilde", "Zufall", "Dr.")]
        [DataRow("Frau Dipl.-Ing. Reinhilde Zufall", "Frau", "Reinhilde", "Zufall", "Dipl.-Ing.")]
        [DataRow("Frau M.Sc. Reinhilde Zufall", "Frau", "Reinhilde", "Zufall", "M.Sc.")]
        [DataRow("Frau B.Sc. Reinhilde Zufall", "Frau", "Reinhilde", "Zufall", "B.Sc.")]
        [DataRow("Frau    Prof.   Dr.    Reinhilde    Zufall", "Frau", "Reinhilde", "Zufall", "Prof. Dr.")]
        [DataRow("Frau    Professorin   Dr.  Dipl.-Ing.  Reinhilde    Zufall", "Frau", "Reinhilde", "Zufall", "Professorin Dr. Dipl.-Ing.")]
        public void ParserService_ParseContact_Success(string contactString,
            string salutation, string firstName, string lastName, string titles)
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
            Assert.AreEqual(titles, TitlesToString(parseResult.Model.Titles));
        }

        private IParserService InitializeParser()
        {
            var titleService = new TitleService();
            var salutationService = new SalutationService();

            return new ParserService(titleService, salutationService);
        }

        private string TitlesToString(IList<string> titles)
        {
            if (titles.Count == 0) return string.Empty;
            return titles.Aggregate((current, title) => $"{current} {title}");
        }
    }
}
