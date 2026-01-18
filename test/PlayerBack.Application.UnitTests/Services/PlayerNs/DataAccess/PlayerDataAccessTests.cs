using Mongo2Go;
using MongoDB.Bson;
using MongoDB.Driver;
using PlayerBack.Application.Services.PlayerNs.DataAccess;
using PlayerBack.Domain.Models;
using PlayerBack.Infrastructure.Common;


namespace PlayerBack.Application.UnitTests.Services.PlayerNs.DataAccess
{
    [TestClass]
    public class PlayerDataAccessTests
    {
        private MongoDbRunner runner;
        private IMongoClient client;
        private IMongoDatabase database;
        private IBaseRepository baseRepository;


        [TestInitialize]
        public void Setup()
        {
            runner = MongoDbRunner.Start();
            client = new MongoClient(runner.ConnectionString);
            database = client.GetDatabase("PlayerBackTestDb");
            baseRepository = new BaseRepository(database);
        }

        [TestCleanup]
        public void Cleanup()
        {
            if (database != null)
            {
                client.DropDatabase(database.DatabaseNamespace.DatabaseName);
            }

            runner?.Dispose();
        }

        private PlayerDataAccess CreatePlayerDataAccess()
        {
            return new PlayerDataAccess(baseRepository);
        }

        [TestMethod]
        public async Task GetPlayersAsync_ReturnsOrderedPlayersAsync()
        {
            // Arrange
            var players = new List<Player>
            {
                new Player { FirstName = "A", Data = new PlayerData { Rank = 5 } },
                new Player { FirstName = "B", Data = new PlayerData { Rank = 2 } },
                new Player { FirstName = "C", Data = new PlayerData { Rank = 10 } }
            };

            var collection = database.GetCollection<Player>(typeof(Player).Name);
            await collection.InsertManyAsync(players);

            var playerDataAccess = CreatePlayerDataAccess();

            // Act
            var result = await playerDataAccess.GetPlayersAsync(CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Count);
            Assert.AreEqual(2, result[0].Data.Rank);
            Assert.AreEqual(5, result[1].Data.Rank);
            Assert.AreEqual(10, result[2].Data.Rank);
        }

        [TestMethod]
        public async Task GetPlayerByIdAsync_ReturnsMatchingPlayerAsync()
        {
            // Arrange
            var players = new List<Player>
            {
                new Player { FirstName = "A", Data = new PlayerData { Rank = 1 } },
                new Player { FirstName = "Target", Data = new PlayerData { Rank = 2 } }
            };

            var collection = database.GetCollection<Player>(typeof(Player).Name);
            await collection.InsertManyAsync(players);

            // find the inserted target id
            var insertedTarget = await collection.Find(p => p.FirstName == "Target").FirstOrDefaultAsync();
            Assert.IsNotNull(insertedTarget);

            var playerDataAccess = CreatePlayerDataAccess();

            // Act
            var result = await playerDataAccess.GetPlayerByIdAsync(insertedTarget.Id, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Target", result.FirstName);
        }

        [TestMethod]
        public async Task CreatePlayerAsync_AssignsIdAndPersistsPlayerAsync()
        {
            // Arrange
            var player = new Player { FirstName = "New", LastName = "Player", Data = new PlayerData { Rank = 99 } };

            var playerDataAccess = CreatePlayerDataAccess();

            // Act
            await playerDataAccess.CreatePlayerAsync(player);

            // Assert
            Assert.IsFalse(string.IsNullOrWhiteSpace(player.Id));

            var playersCollection = database.GetCollection<Player>(typeof(Player).Name);
            var persisted = await playersCollection.Find(p => p.Id == player.Id).FirstOrDefaultAsync();
            Assert.IsNotNull(persisted);
            Assert.AreEqual("New", persisted.FirstName);
        }
    }
}
