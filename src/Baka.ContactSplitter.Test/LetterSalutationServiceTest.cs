using Baka.ContactSplitter.Model;
using Baka.ContactSplitter.Services.Implementations;
using Baka.ContactSplitter.Services.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace Baka.ContactSplitter.Test
{
    [TestClass]
    public class LetterSalutationServiceTest
    {
        private ITitleService _titleService;
        private ILetterSalutationService _letterSalutationService;

        [TestInitialize]
        public void TestInitialization()
        {
            // Arrange
            _titleService = Substitute.For<ITitleService>();
            _titleService.GetTitles().Returns(new[]
            {
                "Professor",
                "Professorin",
                "Prof.",
                "Dr.",
                "Dr. rer. nat.",
                "Dipl.-Ing.",
                "M.Sc.",
                "B.Sc."
            });
            _titleService.GetTitleSalutation("Professor").Returns("Prof.");
            _titleService.GetTitleSalutation("Professorin").Returns("Prof.");
            _titleService.GetTitleSalutation("Prof.").Returns("Prof.");
            _titleService.GetTitleSalutation("Dr.").Returns("Dr.");
            _titleService.GetTitleSalutation("Dr. rer. nat").Returns("Dr.");
            _titleService.GetTitleSalutation("Dipl.-Ing.").Returns("Dipl.-Ing.");
            _titleService.GetTitleSalutation("M.Sc.").Returns("");
            _titleService.GetTitleSalutation("B.Sc.").Returns("");

            _letterSalutationService = new LetterSalutationService(_titleService);
        }

        [TestMethod]
        public void LetterSalutationService_NullContact_Null()
        {
            // Arranged in TestInitialization()

            // Act
            var letterSalutation = _letterSalutationService.GenerateLetterSalutation(null);

            // Assert
            Assert.IsNull(letterSalutation);
        }

        [TestMethod]
        [DataRow(null)]
        [DataRow("")]
        public void LetterSalutationService_NoSalutation_Default(string salutation)
        {
            // Arranged in TestInitialization()

            // Act
            var letterSalutation = _letterSalutationService.GenerateLetterSalutation(new Contact
            {
                Salutation = salutation
            });

            // Assert
            Assert.IsNotNull(letterSalutation);
            Assert.AreEqual("Dear Sir or Madam", letterSalutation);
        }

        [TestMethod]
        [DataRow("Sehr geehrte Frau Reinhilde Zufall", "Frau", "Reinhilde", "Zufall")]
        [DataRow("Sehr geehrter Herr Reiner Zufall", "Herr", "Reiner", "Zufall")]
        [DataRow("Sehr geehrte Frau Dr. Reinhilde Zufall", "Frau", "Reinhilde", "Zufall", "Dr.")]
        [DataRow("Sehr geehrte Frau Dr. Prof. Reinhilde Zufall", "Frau", "Reinhilde", "Zufall", "Dr.", "Prof.")]
        [DataRow("Sehr geehrte Frau Dr. Prof. Reinhilde Zufall", "Frau", "Reinhilde", "Zufall", "Dr.", "Professorin")]
        [DataRow("Dear Miss Reinhilde Zufall", "Miss", "Reinhilde", "Zufall")]
        [DataRow("Dear Sir Reiner Zufall", "Sir", "Reiner", "Zufall")]
        [DataRow("Dear Dr. Reinhilde Zufall", "Miss", "Reinhilde", "Zufall", "Dr.")]
        [DataRow("Dear Dr. Prof. Reinhilde Zufall", "Miss", "Reinhilde", "Zufall", "Dr.", "Professor")]
        public void LetterSalutationService_Contact_Correct(string expectedLetterSalutation,
            string salutation, string firstName, string lastName, params string[] titles)
        {
            // Arranged in TestInitialization()

            // Act
            var letterSalutation = _letterSalutationService.GenerateLetterSalutation(new Contact(titles)
            {
                Salutation = salutation,
                FirstName = firstName,
                LastName = lastName
            });

            // Assert
            Assert.IsNotNull(letterSalutation);
            Assert.AreEqual(expectedLetterSalutation, letterSalutation);
        }
    }
}