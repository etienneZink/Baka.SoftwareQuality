using System.Linq;
using Baka.ContactSplitter.Services.Implementations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Baka.ContactSplitter.Test
{
    [TestClass]
    public class TitleServiceTests
    {
        [TestMethod]
        [DataRow("Professor")]
        [DataRow("Professorin")]
        [DataRow("Prof.")]
        [DataRow("Dr.")]
        [DataRow("Dr. rer. nat.")]
        [DataRow("Dipl.-Ing.")]
        [DataRow("M.Sc.")]
        [DataRow("B.Sc.")]
        public void TitleService_GetTitles_ContainsTitle(string title)
        {
            // Arrange
            var titleService = new TitleService();
            
            // Act
            var titles = titleService.GetTitles();

            // Assert
            Assert.IsTrue(titles.Contains(title));
        }

        [TestMethod]
        [DataRow("B.A.")]
        [DataRow("M.A.")]
        public void TitleService_GetTitles_NotContainsTitle(string title)
        {
            // Arrange
            var titleService = new TitleService();

            // Act
            var titles = titleService.GetTitles();

            // Assert
            Assert.IsFalse(titles.Contains(title));
        }

        [TestMethod]
        [DataRow("Professor", "Prof.")]
        [DataRow("Professorin", "Prof.")]
        [DataRow("Prof.", "Prof.")]
        [DataRow("Dr.", "Dr.")]
        [DataRow("Dr. rer. nat.", "Dr.")]
        [DataRow("Dipl.-Ing.", "Dipl.-Ing.")]
        [DataRow("M.Sc.", "")]
        [DataRow("B.Sc.", "")]
        public void TitleService_GetTitleSalutation_Success(string title, string expectedTitleSalutation)
        {
            // Arrange
            var titleService = new TitleService();

            // Act
            var titleSalutation = titleService.GetTitleSalutation(title);

            // Assert
            Assert.AreEqual(expectedTitleSalutation, titleSalutation);
        }

        [TestMethod]
        [DataRow("B.A.")]
        [DataRow("M.A.")]
        public void TitleService_GetTitleSalutation_FalseTitle(string title)
        {
            // Arrange
            var titleService = new TitleService();

            // Act
            var titleSalutation = titleService.GetTitleSalutation(title);

            // Assert
            Assert.AreEqual(string.Empty, titleSalutation);
        }

        // Test needs to test add and delete because otherwise it could be influencing other tests!
        [TestMethod]
        [DataRow("B.A.", "B.A.")]
        [DataRow("M.A.", "M.A.")]
        public void TitleService_SaveAndDeleteTitle_Success(string title, string titleSalutation)
        {
            // Arrange
            var titleService = new TitleService();

            // Act
            // Save
            var saved = titleService.SaveOrUpdateTitle(title, titleSalutation);
            var savedTitles = titleService.GetTitles().ToArray();
            var savedTitleSalutation = titleService.GetTitleSalutation(title);
            //Delete
            var deleted = titleService.DeleteTitle(title);
            var deletedTitles = titleService.GetTitles().ToArray();
            var deletedTitleSalutation = titleService.GetTitleSalutation(title);

            // Assert
            Assert.IsTrue(saved);
            Assert.IsTrue(savedTitles.Contains(title));
            Assert.AreEqual(titleSalutation, savedTitleSalutation);

            Assert.IsTrue(deleted);
            Assert.IsFalse(deletedTitles.Contains(title));
            Assert.AreEqual(string.Empty, deletedTitleSalutation);
        }
    }
}