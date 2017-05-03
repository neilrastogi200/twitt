using System;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using Twitter2.Commands;
using Twitter2.Infrastructure;
using Twitter2.Models;
using Twitter2.Services;

namespace UnitTests
{
    [TestFixture]
    public class WallCommandTests
    {
        private Mock<IParseCommand> _mockParseCommand;
        private Mock<ITwitterUserFeedService> _mockTwitterUserFeedService;
        private ICommandFactory _commandFactory;
        private ICollection<ICommand> _command;

        [TestFixtureSetUp]
        public void Setup()
        {
            _mockParseCommand = new Mock<IParseCommand>();
            _mockTwitterUserFeedService = new Mock<ITwitterUserFeedService>();
            _command = new List<ICommand>() { new WallCommand(_mockTwitterUserFeedService.Object, _mockParseCommand.Object) };
            _commandFactory = new CommandFactory(_command);
        }


        [Test]
        [ExpectedException(typeof (ArgumentException))]
        public void ExecuteWallCommand_When_Data_IsNull()
        {
            //Arrange
            _mockParseCommand.Setup(x => x.ParsingInput(It.IsAny<string>())).Returns(It.IsAny<ConsoleInput>());

            _mockTwitterUserFeedService.Setup(x => x.ShowWall("TestUser1")).Returns(true);

            //Act
            var actualResult = _commandFactory.Handle(null);

            //Assert
            Assert.AreEqual(false, actualResult);
        }

        [Test]
        public void ExecuteWallCommand_When_Passing_InValid_Wall_Command_Returns_False()
        {
            //Arrange
            var consoleInput = new ConsoleInput
            {
                Command = null,
                Mesage = null,
                UserName = "TestUser1"
            };

            _mockParseCommand.Setup(x => x.ParsingInput(It.IsAny<string>())).Returns(consoleInput);

            _mockTwitterUserFeedService.Setup(x => x.ShowWall("TestUser1")).Returns(true);

            //Act
            var actualResult = _commandFactory.Handle("TestUser1");

            //Assert
            Assert.AreEqual(false, actualResult);
        }

        [Test]
        public void ExecuteWallCommand_When_Passing_Valid_Wall_Command_Returns_True()
        {
            //Arrange
            var consoleInput = new ConsoleInput
            {
                Command = "wall",
                Mesage = null,
                UserName = "TestUser1"
            };

            _mockParseCommand.Setup(x => x.ParsingInput(It.IsAny<string>())).Returns(consoleInput);

            _mockTwitterUserFeedService.Setup(x => x.ShowWall("TestUser1")).Returns(true);

            //Act
            var actualResult = _commandFactory.Handle("TestUser1 wall");

            //Assert
            Assert.AreEqual(true, actualResult);
        }
    }
}