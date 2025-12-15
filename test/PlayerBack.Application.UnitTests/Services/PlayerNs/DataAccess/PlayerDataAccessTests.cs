using Mongo2Go;
using MongoDB.Bson;
using MongoDB.Driver;
using Moq;
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
        private MockRepository mockRepository;

        private Mock<IBaseRepository> mockBaseRepository;

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
                new Player { PlayerId = 1, FirstName = "A", Data = new PlayerData { Rank = 5 } },
                new Player { PlayerId = 2, FirstName = "B", Data = new PlayerData { Rank = 2 } },
                new Player { PlayerId = 3, FirstName = "C", Data = new PlayerData { Rank = 10 } }
            };

            var collection = database.GetCollection<Player>(typeof(Player).Name);
            await collection.InsertManyAsync(players);

            var playerDataAccess = CreatePlayerDataAccess();

            // Act
            var result = await playerDataAccess.GetPlayersAsync(CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Count);
            Assert.AreEqual(2, result[0].PlayerId); // rank 2
            Assert.AreEqual(1, result[1].PlayerId); // rank 5
            Assert.AreEqual(3, result[2].PlayerId); // rank 10
        }

        [TestMethod]
        public async Task GetPlayerByIdAsync_ReturnsMatchingPlayerAsync()
        {
            // Arrange
            var players = new List<Player>
            {
                new Player { PlayerId = 1, FirstName = "A", Data = new PlayerData { Rank = 1 } },
                new Player { PlayerId = 5, FirstName = "Target", Data = new PlayerData { Rank = 2 } }
            };

            var collection = database.GetCollection<Player>(typeof(Player).Name);
            await collection.InsertManyAsync(players);

            var playerDataAccess = CreatePlayerDataAccess();

            // Act
            var result = await playerDataAccess.GetPlayerByIdAsync(5, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("Target", result.First().FirstName);
        }

        [TestMethod]
        public async Task CreatePlayerAsync_AssignsIdAndPersistsPlayerAsync()
        {
            // Arrange
            // prepare counters collection with sequence_value = 41 so next id becomes 42
            var counters = database.GetCollection<BsonDocument>("counters");
            var initial = new BsonDocument { { "_id", "playerId" }, { "sequence_value", 41 } };
            await counters.InsertOneAsync(initial);

            var player = new Player { FirstName = "New", LastName = "Player", Data = new PlayerData { Rank = 99 } };

            var playerDataAccess = CreatePlayerDataAccess();

            // Act
            await playerDataAccess.CreatePlayerAsync(player);

            // Assert
            Assert.AreEqual(42, player.PlayerId);

            // Verify the player was inserted in DB
            var playersCollection = database.GetCollection<Player>(typeof(Player).Name);
            var persisted = await playersCollection.Find(p => p.PlayerId == 42).FirstOrDefaultAsync();
            Assert.IsNotNull(persisted);
            Assert.AreEqual("New", persisted.FirstName);
        }
    }
}
