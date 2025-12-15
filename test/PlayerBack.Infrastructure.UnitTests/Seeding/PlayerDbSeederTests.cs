using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mongo2Go;
using MongoDB.Driver;
using Moq;
using PlayerBack.Domain.Models;
using PlayerBack.Domain.SeedModels;
using PlayerBack.Infrastructure.Common;
using PlayerBack.Infrastructure.Seeding;
using System.Text.Json;
using System.Threading.Tasks;

namespace PlayerBack.Infrastructure.UnitTests.Seeding
{
    [TestClass]
    public class PlayerDbSeederTests
    {
        private MongoDbRunner _runner;
        private IMongoDatabase _database;

        private const string SeedFileName = "response.json";
        private readonly string seedPath = Path.Combine(AppContext.BaseDirectory, SeedFileName);


        [TestInitialize]
        public void TestInitialize()
        {
            _runner = MongoDbRunner.Start();

            var client = new MongoClient(_runner.ConnectionString);
            _database = client.GetDatabase("PlayerBackTestDb");

            if (File.Exists(seedPath))
                File.Delete(seedPath);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _runner.Dispose();
        }

        [TestMethod]
        public async Task HasDataAsync_ReturnsTrue_WhenPlayersExist()
        {
            var collection = _database.GetCollection<Player>("Player");

            await collection.InsertOneAsync(new Player { PlayerId = 1 });

            var repository = new BaseRepository(_database);
            var seeder = new PlayerDbSeeder(repository);

            // Act
            var result = await seeder.HasDataAsync();

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task SeedAsync_WritesPlayersToRepository_WhenResponseFileExists()
        {
            // Arrange
            var seed = new SeedData
            {
                Players = new List<PlayerSeedDto>
                {
                    new PlayerSeedDto
                    {
                        Id = 1,
                        Firstname = "Alice",
                        Lastname = "A",
                        Shortname = "A.",
                        Sex = "F",
                        Picture = "pic",
                        Country = new CountrySeedDto { Code = "USA", Picture = "c" },
                        Data = new PlayerDataSeedDto { Rank = 1, Points = 100, Weight = 60, Height = 170, Age = 25, Last = new List<int> { 1, 0 } }
                    },
                    new PlayerSeedDto
                    {
                        Id = 2,
                        Firstname = "Bob",
                        Lastname = "B",
                        Shortname = "B.",
                        Sex = "M",
                        Picture = "pic2",
                        Country = new CountrySeedDto { Code = "FRA", Picture = "c2" },
                        Data = new PlayerDataSeedDto { Rank = 2, Points = 80, Weight = 75, Height = 180, Age = 28, Last = new List<int> { 1, 1 } }
                    }
                }
            };

            var json = JsonSerializer.Serialize(seed, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            File.WriteAllText(seedPath, json);

            var mockRepo = new Mock<IBaseRepository>(MockBehavior.Default);

            var addedPlayers = new List<Player>();

            mockRepo
                .Setup(r => r.AddAsync(It.Is<Player>(p => p.PlayerId == 1 && p.FirstName == "Alice" && p.Country.Code == "USA")))
                .Callback<Player>(p => addedPlayers.Add(p))
                .Returns(Task.CompletedTask)
                .Verifiable();

            mockRepo
                .Setup(r => r.AddAsync(It.Is<Player>(p => p.PlayerId == 2 && p.FirstName == "Bob" && p.Country.Code == "FRA")))
                .Callback<Player>(p => addedPlayers.Add(p))
                .Returns(Task.CompletedTask)
                .Verifiable();

            var seeder = new PlayerDbSeeder(mockRepo.Object);


            // Act
            await seeder.SeedAsync();

            // Assert
            mockRepo.Verify(r => r.AddAsync(It.IsAny<Player>()), Times.Exactly(2));
            mockRepo.VerifyAll();

            Assert.IsTrue(File.Exists(seedPath));
            Assert.AreEqual(2, addedPlayers.Count, "Expected exactly two players to be added.");

            var alice = addedPlayers.SingleOrDefault(p => p.PlayerId == 1);
            Assert.IsNotNull(alice);
            Assert.AreEqual("Alice", alice.FirstName);
            Assert.IsNotNull(alice.Country);
            Assert.AreEqual("USA", alice.Country.Code);
            Assert.IsNotNull(alice.Data);
            Assert.AreEqual(170, alice.Data.Height);
            Assert.AreEqual(60, alice.Data.Weight);

            var bob = addedPlayers.SingleOrDefault(p => p.PlayerId == 2);
            Assert.IsNotNull(bob);
            Assert.AreEqual("Bob", bob.FirstName);
            Assert.IsNotNull(bob.Country);
            Assert.AreEqual("FRA", bob.Country.Code);
            Assert.IsNotNull(bob.Data);
            Assert.AreEqual(180, bob.Data.Height);
            Assert.AreEqual(75, bob.Data.Weight);
        }
    }
}
