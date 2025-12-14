using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PlayerBack.Application.Services.PlayerNs.DataAccess;
using PlayerBack.Infrastructure.Common;
using System;
using System.Threading.Tasks;

namespace PlayerBack.Application.UnitTests.Services.PlayerNs.DataAccess
{
    [TestClass]
    public class PlayerDataAccessTests
    {
        private MockRepository mockRepository;

        private Mock<IBaseRepository> mockBaseRepository;

        [TestInitialize]
        public void TestInitialize()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);

            this.mockBaseRepository = this.mockRepository.Create<IBaseRepository>();
        }

        private PlayerDataAccess CreatePlayerDataAccess()
        {
            return new PlayerDataAccess(
                this.mockBaseRepository.Object);
        }

        [TestMethod]
        public async Task GetPlayersAsync_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var playerDataAccess = this.CreatePlayerDataAccess();
            CancellationToken cancellationToken = default(global::System.Threading.CancellationToken);

            // Act
            var result = await playerDataAccess.GetPlayersAsync(
                cancellationToken);

            // Assert
            Assert.Fail();
            this.mockRepository.VerifyAll();
        }

        [TestMethod]
        public async Task CreatePlayerAsync_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var playerDataAccess = this.CreatePlayerDataAccess();
            Player player = null;

            // Act
            await playerDataAccess.CreatePlayerAsync(
                player);

            // Assert
            Assert.Fail();
            this.mockRepository.VerifyAll();
        }

        [TestMethod]
        public async Task GetPlayerByIdAsync_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var playerDataAccess = this.CreatePlayerDataAccess();
            int id = 0;
            CancellationToken cancellationToken = default(global::System.Threading.CancellationToken);

            // Act
            var result = await playerDataAccess.GetPlayerByIdAsync(
                id,
                cancellationToken);

            // Assert
            Assert.Fail();
            this.mockRepository.VerifyAll();
        }
    }
}
