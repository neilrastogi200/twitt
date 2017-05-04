using NUnit.Framework;
using Twitter2.Infrastructure;
using Twitter2.Models;

namespace UnitTests
{
    [TestFixture]
    public class ParseCommandUnitTests
    {
        private IParseCommand _parseCommand;

        [TestFixtureSetUp]
        public void Setup()
        {
            _parseCommand = new ParseCommand();
        }

        [Test]
        public void ParsingInput_For_PostCommand()
        {
            //Arrange
            var expectedOutput = new ConsoleInput
            {
                Command = "->",
                Mesage = "hello",
                UserName = "John"
            };

            var expectedInput = "John -> hello";

            //Act
            var actualResult = _parseCommand.ParsingInput(expectedInput);

            //Assert
            Assert.AreEqual(expectedOutput.Command, actualResult.Command);
            Assert.AreEqual(expectedOutput.Mesage, actualResult.Mesage);
            Assert.AreEqual(expectedOutput.UserName, actualResult.UserName);
        }


        [Test]
        public void ParsingInput_For_ReadCommand()
        {
            //Arrange
            var expectedOutput = new ConsoleInput
            {
                Command = null,
                Mesage = null,
                UserName = "John"
            };

            var expectedInput = "John";

            //Act
            var actualResult = _parseCommand.ParsingInput(expectedInput);

            //Assert
            Assert.AreEqual(expectedOutput.Command, actualResult.Command);
            Assert.AreEqual(expectedOutput.Mesage, actualResult.Mesage);
            Assert.AreEqual(expectedOutput.UserName, actualResult.UserName);
        }
    }
}