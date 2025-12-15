using Moq;
using PlayerBack.Application.Services.PlayerNs;
using PlayerBack.Application.Services.PlayerNs.DataAccess;
using PlayerBack.Domain.Dtos;
using PlayerBack.Domain.Models;

namespace PlayerBack.Application.UnitTests.Services.PlayerNs
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

        [TestMethod]
        public async Task GetStatisticsAsync_ComputesCorrectStatistics()
        {
            // Arrange
            var players = new List<Player>
            {
                new Player
                {
                    PlayerId = 1,
                    Country = new Country { Code = "USA" },
                    Data = new PlayerData { Last = new List<int> { 1, 1, 0 }, Height = 170, Weight = 60 }
                },
                new Player
                {
                    PlayerId = 2,
                    Country = new Country { Code = "USA" },
                    Data = new PlayerData { Last = new List<int> { 1, 0 }, Height = 180, Weight = 80 }
                },
                new Player
                {
                    PlayerId = 3,
                    Country = new Country { Code = "FRA" },
                    Data = new PlayerData { Last = new List<int> { 1, 1, 1 }, Height = 175, Weight = 75 }
                },
                new Player
                {
                    PlayerId = 4,
                    Country = null,
                    Data = new PlayerData { Last = new List<int>(), Height = 0, Weight = 0 }
                }
            };

            playerDataAccessMock
                .Setup(d => d.GetPlayersAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(players);

            // Act
            var result = await service.GetStatisticsAsync(CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("FRA", result.CountryCodeWithHighestWinRatio);
            Assert.AreEqual(1, result.HighestWinRatio);
            Assert.AreEqual(23.31, result.AverageBmi);
            Assert.AreEqual(175.0, result.MedianHeight);
        }

        [TestMethod]
        public void ComputeAverageBmi_Returns_AverageIgnoringInvalidEntries()
        {
            // Arrange
            var players = new List<Player>
            {
                new Player { PlayerId = 1, Data = new PlayerData { Weight = 60, Height = 170 } }, // BMI ≈ 20.761246
                new Player { PlayerId = 2, Data = new PlayerData { Weight = 80, Height = 180 } }, // BMI ≈ 24.691358
                new Player { PlayerId = 3, Data = new PlayerData { Weight = 0, Height = 180 } },
                new Player { PlayerId = 4, Data = new PlayerData { Weight = 70, Height = 0 } }
            };

            // Act
            var avgBmi = service.ComputeAverageBmi(players);

            var expected = (60.0 / (1.7 * 1.7) + 80.0 / (1.8 * 1.8)) / 2.0;

            // Assert
            Assert.AreEqual(expected, avgBmi, 0.01);
        }

        [TestMethod]
        public void ComputeCountryWithHighestWinRatio_Returns_CorrectCountryAndRatio()
        {
            // Arrange
            var players = new List<Player>
            {
                new Player { PlayerId = 1, Country = new Country { Code = "USA" }, Data = new PlayerData { Last = new List<int> { 1, 1, 0 } } },
                new Player { PlayerId = 2, Country = new Country { Code = "USA" }, Data = new PlayerData { Last = new List<int> { 1, 0 } } },
                new Player { PlayerId = 3, Country = new Country { Code = "FRA" }, Data = new PlayerData { Last = new List<int> { 1, 1, 1 } } },
                new Player { PlayerId = 4, Country = new Country { Code = "ESP" }, Data = new PlayerData { Last = new List<int>() } },
                new Player { PlayerId = 5, Country = null, Data = new PlayerData { Last = new List<int> { 1 } } }
            };

            // Act
            var result = service.ComputeCountryWithHighestWinRatio(players);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("FRA", result.CountryCode);
            Assert.AreEqual(1.0, result.Ratio);
        }

        [TestMethod]
        public void ComputeMedianHeight_Returns_CorrectMedian_ForOddEvenAndEmpty()
        {
            // Odd count
            var oddPlayers = new List<Player>
            {
                new Player { Data = new PlayerData { Height = 170 } },
                new Player { Data = new PlayerData { Height = 175 } },
                new Player { Data = new PlayerData { Height = 180 } }
            };

            var oddMedian = service.ComputeMedianHeight(oddPlayers);
            Assert.AreEqual(175, oddMedian);

            var evenPlayers = new List<Player>
            {
                new Player { Data = new PlayerData { Height = 160 } },
                new Player { Data = new PlayerData { Height = 170 } },
                new Player { Data = new PlayerData { Height = 180 } },
                new Player { Data = new PlayerData { Height = 190 } }
            };

            var evenMedian = service.ComputeMedianHeight(evenPlayers);
            Assert.AreEqual(175, evenMedian);

            var emptyPlayers = new List<Player>
            {
                new Player { Data = new PlayerData { Height = 0 } },
                new Player { Data = new PlayerData { Height = 0 } }
            };

            var emptyMedian = service.ComputeMedianHeight(emptyPlayers);
            Assert.AreEqual(0, emptyMedian);
        }
    }
}
