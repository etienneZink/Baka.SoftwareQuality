using Baka.ContactSplitter.services.implementations;
using Baka.ContactSplitter.services.interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Baka.ContactSplitter.Test
{
    [TestClass]
    public class ParserServiceTests
    {
        [TestMethod]
        [DataRow("")]
        public void ParserService_ParseContact_Success(string contactString)
        {
            // Arrange
            var parser = InitializeParser();

            // Act
            var parseResult = parser.ParseContact(contactString);

            // Assert
            Assert.IsNotNull(parseResult);
            Assert.IsTrue(parseResult.Successful);
            Assert.AreEqual("", parseResult.Model.FirstName);
        }

        private IParserService InitializeParser()
        {
            var titleService = new TitleService();
            var salutationService = new SalutationService();

            return new ParserService(titleService, salutationService);
        }
    }
}
