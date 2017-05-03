using System;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using Twitter2.Commands;
using Twitter2.Infrastructure;
using Twitter2.Models;
using Twitter2.Repository;
using Twitter2.Services;

namespace UnitTests
{
    [TestFixture]
    public class TwitterUserFeedServiceTests
    {
        private Mock<IMessageRepository> _mockMessageRepository;
        private Mock<IUserRepository> _mockUserRepository;
        private Mock<IConsole> _mockConsole; 
        private ITwitterUserFeedService _twitterUserFeedService;

        [TestFixtureSetUp]
        public void SetUp()
        {
            _mockMessageRepository = new Mock<IMessageRepository>();
            _mockUserRepository = new Mock<IUserRepository>();
            _mockConsole = new Mock<IConsole>();

            _twitterUserFeedService = new TwitterUserFeedService(_mockMessageRepository.Object, _mockUserRepository.Object, _mockConsole.Object);
        }


        [Test]
        public void ShowWall_When_User_Is_Following_Another_User_Returns_All_Messages()
        {
            //Arrange
            var testUser1 = new User() { Id = Guid.NewGuid(), UserName = "TestUser1" };
            var testUser2 = new User() { Id = Guid.NewGuid(), UserName = "TestUser2" };

            var message1 = new Message() { Content = "blah", DateTime = DateTime.UtcNow.AddMinutes(-1), User = testUser1 };
            var message2 = new Message() { Content = "ddd", DateTime = DateTime.UtcNow.AddMinutes(-2), User = testUser2 };

            testUser1.Following = new List<User>() { testUser2 };
            testUser1.Messages = new List<Message>() { message1 };
            testUser2.Messages = new List<Message>() { message2 };

            _mockUserRepository.Setup(x => x.CreateUser(testUser1));
            _mockUserRepository.Setup(u => u.CreateUser(testUser2));
            _mockUserRepository.Setup(x => x.GetUsers("TestUser1")).Returns(testUser1);

            _mockMessageRepository.Setup(x => x.CreateMessage(message1));
            _mockMessageRepository.Setup(x => x.CreateMessage(message2));

           var writeLineBuffer = new List<string>();

            //_mockConsole.Setup(x => x.Write(It.IsAny<string>())).Callback((string line) => writeLineBuffer.Add(line));
            //_mockConsole.Setup(x => x.WriteLine(It.IsAny<string>())).Callback((string line) => writeLineBuffer.Add(line));
            //_mockConsole.Setup(x => x.WriteLine(It.IsAny<string>())).Callback((string line) => writeLineBuffer.Add(line));
            _mockConsole.Setup(x => x.WriteLine(It.IsAny<string>(), It.IsAny<object[]>())).Callback((string line, object[] args) => writeLineBuffer.Add(string.Format(line, args)));



            var result = _twitterUserFeedService.ShowWall("TestUser1");

            Assert.AreEqual("TestUser1 - blah (1 minutes ago)", writeLineBuffer[0]);
            Assert.AreEqual("TestUser2 - ddd (2 minutes ago)", writeLineBuffer[1]);

        }
    }
}
