using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PlayerBack.Application.Services.PlayerNs;
using PlayerBack.Application.Services.PlayerNs.DataAccess;
using PlayerBack.Domain.Dtos;
using PlayerBack.Domain.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace PlayerBack.Application.UnitTests
{
    [TestClass]
    public sealed class PlayerServiceTest
    {
        private Mock<IPlayerDataAccess> playerDataAccessMock = null!;
        private PlayerService service = null!;

        [TestInitialize]
        public void Setup()
        {
            playerDataAccessMock = new Mock<IPlayerDataAccess>();
            service = new PlayerService(playerDataAccessMock.Object);
        }

        [TestMethod]
        public async Task GetPlayersAsync_ReturnsMappedDtos()
        {
            // Arrange
            var players = new List<Player>
            {
                new Player
                {
                    PlayerId = 1,
                    FirstName = "Alice",
                    LastName = "A",
                    Country = new Country { Code = "USA", Picture = string.Empty },
                    Data = new PlayerData { Rank = 1, Points = 100, Height = 170, Weight = 60, Age = 25, Last = new List<int>() }
                },
                new Player
                {
                    PlayerId = 2,
                    FirstName = "Bob",
                    LastName = "B",
                    Country = new Country { Code = "FRA", Picture = string.Empty },
                    Data = new PlayerData { Rank = 2, Points = 80, Height = 180, Weight = 75, Age = 28, Last = new List<int>() }
                }
            };

            playerDataAccessMock
                .Setup(d => d.GetPlayersAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(players);

            // Act
            var result = await service.GetPlayersAsync(CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);

            Assert.AreEqual(players[0].PlayerId, result[0].PlayerId);
            Assert.AreEqual(players[0].FirstName, result[0].FirstName);
            Assert.IsNotNull(result[0].Country);
            Assert.AreEqual(players[0].Country.Code, result[0].Country.Code);
            Assert.IsNotNull(result[0].Data);
            Assert.AreEqual(players[0].Data.Height, result[0].Data.Height);

            Assert.AreEqual(players[1].PlayerId, result[1].PlayerId);
            Assert.AreEqual(players[1].FirstName, result[1].FirstName);
            Assert.IsNotNull(result[1].Country);
            Assert.AreEqual(players[1].Country.Code, result[1].Country.Code);
            Assert.IsNotNull(result[1].Data);
            Assert.AreEqual(players[1].Data.Height, result[1].Data.Height);
        }

        [TestMethod]
        public async Task GetPlayerByIdAsync_ReturnsDto_WhenFound()
        {
            // Arrange
            var player = new Player
            {
                PlayerId = 1,
                FirstName = "Existing",
                LastName = "Player",
                Country = new Country { Code = "ESP", Picture = string.Empty },
                Data = new PlayerData { Rank = 1, Points = 100, Height = 180, Weight = 75, Age = 25, Last = new List<int>() }
            };

            playerDataAccessMock
                .Setup(d => d.GetPlayerByIdAsync(player.PlayerId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Player> { player });

            // Act
            var result = await service.GetPlayerByIdAsync(player.PlayerId, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(player.PlayerId, result.PlayerId);
            Assert.AreEqual("Existing", result.FirstName);
        }

        [TestMethod]
        public async Task GetPlayerByIdAsync_ReturnsNull_WhenNotFound()
        {
            // Arrange
            playerDataAccessMock
                .Setup(d => d.GetPlayerByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Player>());

            // Act
            var result = await service.GetPlayerByIdAsync(1, CancellationToken.None);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task CreatePlayerAsync_AssignsId_AndCallsDataAccess()
        {
            // Arrange
            var dto = new PlayerDto
            {
                PlayerId = 1,
                FirstName = "New",
                LastName = "Player",
                Country = null,
                Data = null
            };

            playerDataAccessMock
                .Setup(d => d.CreatePlayerAsync(It.IsAny<Player>()))
                .Callback<Player>(p => p.PlayerId = 1)
                .Returns(Task.CompletedTask);

            // Act
            var result = await service.CreatePlayerAsync(dto);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(dto.PlayerId, result.PlayerId);
            playerDataAccessMock.Verify(d => d.CreatePlayerAsync(It.Is<Player>(p => p.FirstName == dto.FirstName && p.LastName == dto.LastName)), Times.Once);
        }
    }
}