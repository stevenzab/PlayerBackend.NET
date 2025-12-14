using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PlayerBack.Api.Controllers;
using PlayerBack.Application.Services.PlayerNs;
using PlayerBack.Domain.Dtos;
using PlayerBack.Domain.Models;
using System;
using System.Threading.Tasks;

namespace PlayerBack.Api.UnitTests.Controllers
{
    [TestClass]
    public class PlayerControllerTest
    {
        private Mock<IPlayerService> playerServiceMock;
        private PlayerController controller;

        [TestInitialize]
        public void Setup()
        {
            playerServiceMock = new Mock<IPlayerService>();
            controller = new PlayerController(playerServiceMock.Object);
        }

        [TestMethod]
        public async Task GetPlayerListAsync_ReturnOk()
        {
            //Arrange
            var players = new List<PlayerDto>
            {
                new PlayerDto { PlayerId = 1, FirstName = "Player1" },
                new PlayerDto { PlayerId = 2, FirstName = "Player2" }
            };

            playerServiceMock
                    .Setup(service => service.GetPlayersAsync(It.IsAny<CancellationToken>()))
                    .ReturnsAsync(players);


            //Act
            var result = await controller.GetPlayersAsync(CancellationToken.None);

            //Assert
            Assert.IsNotNull(result);
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);

            var returnedPlayers = okResult.Value as IList<PlayerDto>;
            Assert.IsNotNull(returnedPlayers);
            Assert.AreEqual(2, returnedPlayers.Count);
        }
        [TestMethod]
        public async Task GetPlayerByIdAsync_ReturnsOk_WhenPlayerExist()
        {
            // Arrange
            var expected = new PlayerDto { PlayerId = 52, FirstName = "Existing" };

            playerServiceMock
                .Setup(s => s.GetPlayerByIdAsync(52, It.IsAny<CancellationToken>()))
                .ReturnsAsync(expected);

            // Act
            var result = await controller.GetPlayerByIdAsync(52, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult, "Expected an OkObjectResult when the player exists.");
            Assert.AreEqual(200, okResult.StatusCode);

            var returnedPlayer = okResult.Value as PlayerDto;
            Assert.IsNotNull(returnedPlayer);
            Assert.AreEqual(expected.PlayerId, returnedPlayer.PlayerId);
            Assert.AreEqual(expected.FirstName, returnedPlayer.FirstName);
        }

        [TestMethod]
        public async Task CreatePlayerAsync_ReturnsCreatedAtRoute()
        {
            // Arrange
            var player = new PlayerDto
            {
                PlayerId = 42,
                FirstName = "New",
                LastName = "Player",
                Country = new CountryDto { Code = "ESP", Picture = string.Empty },
                Data = new PlayerDataDto { Height = 170, Weight = 68, Last = new List<int>() }
            };

            playerServiceMock
                .Setup(s => s.CreatePlayerAsync(player))
                .ReturnsAsync(player);

            // Act
            var result = await controller.CreatePlayerAsync(player);

            // Assert
            Assert.IsInstanceOfType(result, typeof(CreatedAtRouteResult));
        }
        [TestMethod]
        public async Task GetPlayerStatisticsAsync_ReturnsOk_WhenStatisticsExist()
        {
            // Arrange
            var stats = new StatisticsModel
            {
                CountryCodeWithHighestWinRatio = "FRA",
                HighestWinRatio = 0.75,
                AverageBmi = 23.5,
                MedianHeight = 180.0
            };

            playerServiceMock
                .Setup(s => s.GetStatisticsAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(stats);

            // Act
            var actionResult = await controller.GetPlayerStatisticsAsync(CancellationToken.None);

            // Assert
            Assert.IsNotNull(actionResult);
            var okResult = actionResult.Result as OkObjectResult;
            Assert.AreEqual(200, okResult.StatusCode);

            var returnedStats = okResult.Value as StatisticsModel;
            Assert.IsNotNull(returnedStats);
            Assert.AreEqual(stats.CountryCodeWithHighestWinRatio, returnedStats.CountryCodeWithHighestWinRatio);
            Assert.AreEqual(stats.HighestWinRatio, returnedStats.HighestWinRatio);
            Assert.AreEqual(stats.AverageBmi, returnedStats.AverageBmi);
            Assert.AreEqual(stats.MedianHeight, returnedStats.MedianHeight);
        }
    }
}
