using System.Linq;
using Baka.ContactSplitter.Model;
using Baka.ContactSplitter.Services.Implementations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Baka.ContactSplitter.Test
{
    [TestClass]
    public class SalutationServiceTest
    {
        [TestMethod]
        [DataRow("Frau")]
        [DataRow("Mrs.")]
        [DataRow("Ms.")]
        [DataRow("Mmd.")]
        [DataRow("Herr")]
        [DataRow("Mr.")]
        [DataRow("Sir")]
        public void SalutationService_GetSalutations_ContainsSalutation(string salutation)
        {
            // Arrange
            var salutationService = new SalutationService();

            // Act
            var salutations = salutationService.GetSalutations();

            // Assert
            Assert.IsTrue(salutations.Contains(salutation));
        }

        [TestMethod]
        [DataRow("Test Anrede")]
        [DataRow("Frau Madame")]
        public void SalutationService_GetSalutations_NotContainsSalutation(string salutation)
        {
            // Arrange
            var salutationService = new SalutationService();

            // Act
            var salutations = salutationService.GetSalutations();

            // Assert
            Assert.IsFalse(salutations.Contains(salutation));
        }

        [TestMethod]
        [DataRow("Frau", Gender.Female)]
        [DataRow("Mrs.", Gender.Female)]
        [DataRow("Ms.", Gender.Female)]
        [DataRow("Mmd.", Gender.Female)]
        [DataRow("Herr", Gender.Male)]
        [DataRow("Mr.", Gender.Male)]
        [DataRow("Sir", Gender.Male)]
        public void SalutationService_GetGender_Success(string salutation, Gender expectedGender)
        {
            // Arrange
            var salutationService = new SalutationService();

            // Act
            var gender = salutationService.GetGender(salutation);

            // Assert
            Assert.AreEqual(expectedGender, gender);
        }

        [TestMethod]
        [DataRow("Test Anrede")]
        [DataRow("Frau Madame")]
        public void SalutationService_GetGender_FalseTitle(string salutation)
        {
            // Arrange
            var salutationService = new SalutationService();

            // Act
            var gender = salutationService.GetGender(salutation);

            // Assert
            Assert.AreEqual(Gender.Neutral, gender);
        }

        // Test needs to test add and delete because otherwise it could be influencing other tests!
        [TestMethod]
        [DataRow("Test Anrede", Gender.Neutral)]
        [DataRow("Frau Madame", Gender.Female)]
        public void SalutationService_SaveAndDeleteSalutation_Success(string salutation, Gender gender)
        {
            // Arrange
            var salutationService = new SalutationService();

            // Act
            // Save
            var saved = salutationService.SaveOrUpdateSalutation(salutation, gender);
            var savedSalutations = salutationService.GetSalutations().ToArray();
            var savedGender = salutationService.GetGender(salutation);
            //Delete
            var deleted = salutationService.DeleteSalutation(salutation);
            var deletedSalutation = salutationService.GetSalutations().ToArray();
            var deletedGender = salutationService.GetGender(salutation);

            // Assert
            Assert.IsTrue(saved);
            Assert.IsTrue(savedSalutations.Contains(salutation));
            Assert.AreEqual(gender, savedGender);

            Assert.IsTrue(deleted);
            Assert.IsFalse(deletedSalutation.Contains(salutation));
            Assert.AreEqual(Gender.Neutral, deletedGender);
        }
    }
}