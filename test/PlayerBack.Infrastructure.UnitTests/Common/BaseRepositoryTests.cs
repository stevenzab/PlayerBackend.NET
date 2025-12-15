using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Driver;
using Moq;
using PlayerBack.Domain.Models;
using PlayerBack.Infrastructure.Common;
using System;
using System.Threading.Tasks;

namespace PlayerBack.Infrastructure.UnitTests.Common
{
    [TestClass]
    public class BaseRepositoryTests
    {
        private MockRepository mockRepository;

        private Mock<IMongoDatabase> mockMongoDatabase;

        [TestInitialize]
        public void TestInitialize()
        {
            this.mockRepository = new MockRepository(MockBehavior.Default);

            this.mockMongoDatabase = this.mockRepository.Create<IMongoDatabase>();
        }

        private BaseRepository CreateBaseRepository()
        {
            return new BaseRepository(
                this.mockMongoDatabase.Object);
        }

        [TestMethod]
        public void AsQueryable_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var mockCollection = this.mockRepository.Create<IMongoCollection<Player>>();

            this.mockMongoDatabase
                .Setup(m => m.GetCollection<Player>(It.Is<string>(s => s == typeof(Player).Name), It.IsAny<MongoCollectionSettings>()))
                .Returns(mockCollection.Object);

            var baseRepository = this.CreateBaseRepository();

            // Act
            var queryPlayer = baseRepository.AsQueryable<Player>();

            // Assert
            this.mockRepository.VerifyAll();
            Assert.IsNotNull(queryPlayer);
        }

        [TestMethod]
        public async Task AddAsync_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var mockCollection = this.mockRepository.Create<IMongoCollection<Player>>();

            this.mockMongoDatabase
                .Setup(m => m.GetCollection<Player>(It.Is<string>(s => s == typeof(Player).Name), It.IsAny<MongoCollectionSettings>()))
                .Returns(mockCollection.Object);

            mockCollection
                .Setup(c => c.InsertOneAsync(It.IsAny<Player>(), It.IsAny<InsertOneOptions>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask)
                .Verifiable();

            var baseRepository = this.CreateBaseRepository();

            var entity = new Player
            {
                FirstName = "Test",
                LastName = "Player"
            };

            var before = DateTime.Now;

            // Act
            await baseRepository.AddAsync(entity);

            // Assert
            Assert.IsTrue(entity.Created >= before);
            Assert.IsTrue(entity.Updated >= before);
        }

        [TestMethod]
        public void GetCollection_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var mockCollection = this.mockRepository.Create<IMongoCollection<Player>>();
            const string name = "CustomCollectionName";

            this.mockMongoDatabase
                .Setup(m => m.GetCollection<Player>(It.Is<string>(s => s == name), It.IsAny<MongoCollectionSettings>()))
                .Returns(mockCollection.Object);

            var baseRepository = this.CreateBaseRepository();

            // Act
            var result = baseRepository.GetCollection<Player>(name);

            // Assert
            Assert.AreSame(mockCollection.Object, result);
        }
    }
}
