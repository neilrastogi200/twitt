using System;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using Twitter2.Infrastructure;
using Twitter2.Models;
using Twitter2.Repository;
using Twitter2.Services;

namespace UnitTests
{
    [TestFixture]
    public class TwitterUserFeedServiceTests
    {
        [SetUp]
        public void Reset()
        {
            _mockUserRepository.Reset();
            _mockUserRepository.Reset();
            _mockConsole.Reset();
        }

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

            _twitterUserFeedService = new TwitterUserFeedService(_mockMessageRepository.Object,
                _mockUserRepository.Object, _mockConsole.Object);
        }

        [Test]
        public void CreateUser_When_User_Does_Not_Exists_Returns_New_User()
        {
            //Arrange
            var userName = "Neil";

            //Act
            var actualResult = _twitterUserFeedService.CreateUser(userName);

            //Assert
            Assert.IsNotNull(actualResult);
            Assert.AreEqual("Neil", actualResult.UserName);
        }

        [Test]
        [ExpectedException(typeof (ArgumentException))]
        public void FollowUser_When_CurrentUser_And_The_FollowUser_Does_Not_Exist_ArgumentException()
        {
            //Arrange
            var testUser3 = new User {Id = Guid.NewGuid(), UserName = "TestUser3"};

            _mockUserRepository.Setup(x => x.CreateUser(testUser3));
            _mockUserRepository.Setup(x => x.GetUsers("TestUser3")).Returns(testUser3);

            //Act
            var actualResult = _twitterUserFeedService.FollowUser("TestUser1", "TestUser2");

            //Assert
        }

        [Test]
        [ExpectedException(typeof (ArgumentException))]
        public void FollowUser_When_CurrentUser_Does_Not_Exist_Returns_ArgumentException()
        {
            //Arrange
            var testUser2 = new User {Id = Guid.NewGuid(), UserName = "TestUser2"};

            _mockUserRepository.Setup(x => x.CreateUser(testUser2));
            _mockUserRepository.Setup(x => x.GetUsers("TestUser2")).Returns(testUser2);

            //Act
            var actualResult = _twitterUserFeedService.FollowUser("TestUser1", "TestUser2");

            //Assert
        }

        [Test]
        public void FollowUser_When_User_Is_Already_Following_The_User_Returns_True()
        {
            //Arrange
            var testUser1 = new User {Id = Guid.NewGuid(), UserName = "TestUser1"};
            var testUser2 = new User {Id = Guid.NewGuid(), UserName = "TestUser2"};

            testUser1.Following = new List<User> {testUser2};

            _mockUserRepository.Setup(x => x.CreateUser(testUser1));
            _mockUserRepository.Setup(x => x.GetUsers("TestUser1")).Returns(testUser1);

            _mockUserRepository.Setup(x => x.CreateUser(testUser2));
            _mockUserRepository.Setup(x => x.GetUsers("TestUser2")).Returns(testUser2);

            //Act
            var actualResult = _twitterUserFeedService.FollowUser("TestUser1", "TestUser2");

            //Assert
            Assert.AreEqual(true, actualResult);
            _mockUserRepository.Verify(x => x.Update(It.IsAny<User>()), Times.Never);
        }

        [Test]
        public void FollowUser_When_User_Is_Requesting_To_Follow_Another_User_Returns_True()
        {
            //Arrange
            var testUser1 = new User {Id = Guid.NewGuid(), UserName = "TestUser1"};
            var testUser2 = new User {Id = Guid.NewGuid(), UserName = "TestUser2"};

            _mockUserRepository.Setup(x => x.CreateUser(testUser1));
            _mockUserRepository.Setup(x => x.GetUsers("TestUser1")).Returns(testUser1);

            _mockUserRepository.Setup(x => x.CreateUser(testUser2));
            _mockUserRepository.Setup(x => x.GetUsers("TestUser2")).Returns(testUser2);
            _mockUserRepository.Setup(x => x.Update(testUser1)).Returns(true);

            //Act
            var actualResult = _twitterUserFeedService.FollowUser("TestUser1", "TestUser2");

            //Assert
            Assert.AreEqual(true, actualResult);
        }


        [Test]
        public void PublishMessage_When_User_Does_Not_Exists_Returns_New_User()
        {
            //Arrange
            var userName = "Neil";
            var message = "test";

            //Act
            _twitterUserFeedService.PublishMessage(userName, message);

            //Assert
            _mockUserRepository.Verify(x => x.GetUsers(userName), Times.Once);
            _mockUserRepository.Verify(x => x.CreateUser(It.IsAny<User>()), Times.Once);
        }


        [Test]
        [TestCase(null), TestCase("")]
        public void PublishMessage_WhenMessageIsNullOrEmpty_Verify_No_User_Or_Message_Created(string message)
        {
            _twitterUserFeedService.PublishMessage("Neil", message);

            //Assert
            _mockUserRepository.Verify(x => x.GetUsers(message), Times.Never);
            _mockUserRepository.Verify(x => x.CreateUser(It.IsAny<User>()), Times.Never);
        }


        [Test]
        [TestCase(null), TestCase("")]
        public void PublishMessage_WhenUserNameIsNullOrEmpty_Verify_No_User_Or_Message_Created(string userName)
        {
            _twitterUserFeedService.PublishMessage(userName, "ttttt");

            //Assert
            _mockUserRepository.Verify(x => x.GetUsers(userName), Times.Never);
            _mockUserRepository.Verify(x => x.CreateUser(It.IsAny<User>()), Times.Never);
        }

        [Test]
        [ExpectedException(typeof (ArgumentException))]
        public void ReadMessage_When_User_Does_Not_Exist_Returns_ArgumentException()
        {
            //Act
            var result = _twitterUserFeedService.ReadMessage("TestUser1");
        }


        [Test]
        [ExpectedException(typeof (ArgumentException))]
        public void ReadMessage_When_User_Exists_But_NoMessages_Returns_ArgumentException()
        {
            //Arrange
            var testUser1 = new User {Id = Guid.NewGuid(), UserName = "TestUser1"};

            _mockUserRepository.Setup(x => x.CreateUser(testUser1));
            _mockUserRepository.Setup(x => x.GetUsers("TestUser1")).Returns(testUser1);

            //Act
            var result = _twitterUserFeedService.ReadMessage("TestUser1");
        }

        [Test]
        public void ReadMessage_When_User_Exists_Returns_Messages()
        {
            //Arrange
            var testUser1 = new User {Id = Guid.NewGuid(), UserName = "TestUser1"};

            var message1 = new Message {Content = "blah", DateTime = DateTime.UtcNow.AddMinutes(-1), User = testUser1};
            var message2 = new Message {Content = "ddd", DateTime = DateTime.UtcNow.AddMinutes(-2), User = testUser1};

            testUser1.Messages = new List<Message> {message1, message2};


            _mockUserRepository.Setup(x => x.CreateUser(testUser1));
            _mockUserRepository.Setup(x => x.GetUsers("TestUser1")).Returns(testUser1);

            _mockMessageRepository.Setup(x => x.CreateMessage(message1));
            _mockMessageRepository.Setup(x => x.CreateMessage(message2));

            var writeLineBuffer = new List<string>();

            _mockConsole.Setup(x => x.WriteLine(It.IsAny<string>(), It.IsAny<object[]>()))
                .Callback((string line, object[] args) => writeLineBuffer.Add(string.Format(line, args)));

            //Act
            var result = _twitterUserFeedService.ReadMessage("TestUser1");

            //assert
            Assert.AreEqual("ddd (2 minutes ago)", writeLineBuffer[0]);
            Assert.AreEqual("blah (1 minutes ago)", writeLineBuffer[1]);
            Assert.IsNull(result);
            Assert.IsTrue(writeLineBuffer.Count == 2);
        }

        [Test]
        [ExpectedException(typeof (ArgumentException))]
        public void ShowWall_When_User_Is_Does_Not_Exist_Returns_ArgumentException()
        {
            //Act
            var result = _twitterUserFeedService.ShowWall("TestUser1");
        }


        [Test]
        public void ShowWall_When_User_Is_Following_Another_User_Returns_All_Messages()
        {
            //Arrange
            var testUser1 = new User {Id = Guid.NewGuid(), UserName = "TestUser1"};
            var testUser2 = new User {Id = Guid.NewGuid(), UserName = "TestUser2"};

            var message1 = new Message {Content = "blah", DateTime = DateTime.UtcNow.AddMinutes(-1), User = testUser1};
            var message2 = new Message {Content = "ddd", DateTime = DateTime.UtcNow.AddMinutes(-2), User = testUser2};

            testUser1.Following = new List<User> {testUser2};
            testUser1.Messages = new List<Message> {message1};
            testUser2.Messages = new List<Message> {message2};

            _mockUserRepository.Setup(x => x.CreateUser(testUser1));
            _mockUserRepository.Setup(u => u.CreateUser(testUser2));
            _mockUserRepository.Setup(x => x.GetUsers("TestUser1")).Returns(testUser1);

            _mockMessageRepository.Setup(x => x.CreateMessage(message1));
            _mockMessageRepository.Setup(x => x.CreateMessage(message2));

            var writeLineBuffer = new List<string>();

            _mockConsole.Setup(x => x.WriteLine(It.IsAny<string>(), It.IsAny<object[]>()))
                .Callback((string line, object[] args) => writeLineBuffer.Add(string.Format(line, args)));

            var result = _twitterUserFeedService.ShowWall("TestUser1");

            Assert.AreEqual("TestUser1 - blah (1 minutes ago)", writeLineBuffer[0]);
            Assert.AreEqual("TestUser2 - ddd (2 minutes ago)", writeLineBuffer[1]);
            Assert.IsTrue(writeLineBuffer.Count == 2);
        }

        [Test]
        public void ShowWall_When_User_Is_Following_NoOne_Returns_All_Messages()
        {
            //Arrange
            var testUser1 = new User {Id = Guid.NewGuid(), UserName = "TestUser1"};
            var message1 = new Message {Content = "blah", DateTime = DateTime.UtcNow.AddMinutes(-1), User = testUser1};

            testUser1.Messages = new List<Message> {message1};


            _mockUserRepository.Setup(x => x.CreateUser(testUser1));
            _mockUserRepository.Setup(x => x.GetUsers("TestUser1")).Returns(testUser1);

            _mockMessageRepository.Setup(x => x.CreateMessage(message1));

            var writeLineBuffer = new List<string>();

            _mockConsole.Setup(x => x.WriteLine(It.IsAny<string>(), It.IsAny<object[]>()))
                .Callback((string line, object[] args) => writeLineBuffer.Add(string.Format(line, args)));

            //Act
            var result = _twitterUserFeedService.ShowWall("TestUser1");

            //assert
            Assert.AreEqual("TestUser1 - blah (1 minutes ago)", writeLineBuffer[0]);
            Assert.IsTrue(writeLineBuffer.Count == 1);
        }
    }
}