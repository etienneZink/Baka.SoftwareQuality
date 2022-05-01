using System.Collections.Generic;
using System.Linq;
using Baka.ContactSplitter.model;
using Baka.ContactSplitter.services.implementations;
using Baka.ContactSplitter.services.interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

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
        [DataRow("Reinhilde Zufall", "", "Reinhilde", "Zufall")]
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
        [DataRow("Herr Professor Reiner   Zufall", "Herr", "Reiner", "Zufall", "Professor")]
        [DataRow("Frau Professorin Reinhilde Zufall", "Frau", "Reinhilde", "Zufall", "Professorin")]
        [DataRow("Frau Prof. Reinhilde Zufall", "Frau", "Reinhilde", "Zufall", "Prof.")]
        [DataRow("Frau Dr. Reinhilde Zufall", "Frau", "Reinhilde", "Zufall", "Dr.")]
        [DataRow("Frau Dr. rer. nat. Reinhilde Zufall", "Frau", "Reinhilde", "Zufall", "Dr. rer. nat.")]
        [DataRow("Frau Dipl.-Ing. Reinhilde Zufall", "Frau", "Reinhilde", "Zufall", "Dipl.-Ing.")]
        [DataRow("Frau M.Sc. Reinhilde Zufall", "Frau", "Reinhilde", "Zufall", "M.Sc.")]
        [DataRow("Frau B.Sc. Reinhilde Zufall", "Frau", "Reinhilde", "Zufall", "B.Sc.")]
        [DataRow("Frau    Prof.   Dr.    Reinhilde    Zufall", "Frau", "Reinhilde", "Zufall", "Prof.", "Dr.")]
        [DataRow("Frau    Professorin   Dr.  Dipl.-Ing.  Reinhilde    Zufall", "Frau", "Reinhilde", "Zufall", "Professorin", "Dr.", "Dipl.-Ing.")]
        public void ParserService_ParseContact_Success(string contactString,
            string salutation, string firstName, string lastName, params string[] titles)
        {
            // Arrange
            var titleService = Substitute.For<ITitleService>();
            titleService.GetTitles().Returns(titles);

            var salutationService = Substitute.For<ISalutationService>();
            salutationService.GetSalutations().Returns(new [] { salutation });

            var parser = new ParserService(titleService, salutationService);

            // Act
            var parseResult = parser.ParseContact(contactString);

            // Assert
            Assert.IsNotNull(parseResult);
            Assert.IsTrue(parseResult.Successful);
            Assert.AreEqual(salutation, parseResult.Model.Salutation);
            Assert.AreEqual(firstName, parseResult.Model.FirstName);
            Assert.AreEqual(lastName, parseResult.Model.LastName);
            Assert.IsTrue(titles.SequenceEqual(parseResult.Model.Titles));

            titleService.Received(2).GetTitles();
            salutationService.Received(1).GetSalutations();
        }

        [TestMethod]
        public void ParserService_ParseContact_contactStringIsNull_Fail()
        {
            // Arrange
            var titleService = Substitute.For<ITitleService>();

            var salutationService = Substitute.For<ISalutationService>();

            var parser = new ParserService(titleService, salutationService);

            string contactString = null;

            // Act
            var parseResult = parser.ParseContact(contactString);

            // Assert
            Assert.IsNotNull(parseResult);
            Assert.IsFalse(parseResult.Successful);
            Assert.IsNull(parseResult.Model);
            Assert.AreEqual(1, parseResult.ErrorMessages.Count);
            Assert.AreEqual("contactString ist null!", parseResult.ErrorMessages.FirstOrDefault());

            titleService.DidNotReceive().GetTitles();
            salutationService.DidNotReceive().GetSalutations();
        }

        [TestMethod]
        [DataRow("Frau Dr. rer. nat. Reinhilde Zufall-Experiment1")]
        [DataRow("Frau Dr. rer. nat. Reinhilde Zufall-Experiment-Versuch")]
        [DataRow("Frau Dr. rer. nat. Reinhilde1 Zufall-Experiment")]
        [DataRow("Frau Dr. rer. nat. Dipl.-Ing. Reinhilde Zufall-Experiment")]
        [DataRow("Herr Dr. rer. nat. Reinhilde Zufall-Experiment")]
        public void ParserService_ParseContact_contactStringWithIncorrectFormat_Fail(string contactString)
        {
            // Arrange
            var titleService = Substitute.For<ITitleService>();
            titleService.GetTitles().Returns(new[] { "Dr. rer. nat." });

            var salutationService = Substitute.For<ISalutationService>();
            salutationService.GetSalutations().Returns(new[] { "Frau" });

            var parser = new ParserService(titleService, salutationService);

            // Act
            var parseResult = parser.ParseContact(contactString);

            // Assert
            Assert.IsNotNull(parseResult);
            Assert.IsFalse(parseResult.Successful);
            Assert.IsNull(parseResult.Model);
            Assert.AreEqual(1, parseResult.ErrorMessages.Count);
            Assert.AreEqual("Die Eingabe konnte nicht erfolgreich eingelesen werden!", parseResult.ErrorMessages.FirstOrDefault());

            titleService.Received(1).GetTitles();
            salutationService.Received(1).GetSalutations();
        }
    }
}
