using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Driver;
using Moq;
using PlayerBack.Domain.Settings;
using PlayerBack.Infrastructure;
using PlayerBack.Infrastructure.Common;
using PlayerBack.Infrastructure.Seeding;
using System;

namespace PlayerBack.Infrastructure.UnitTests
{
    [TestClass]
    public class DependencyInjectionTests
    {
        private IConfiguration configuration;

        [TestInitialize]
        public void Setup()
        {
            var initialData = new Dictionary<string, string?>
            {
                ["MongoDB:ConnectionString"] = "mongodb://localhost:27017",
                ["MongoDB:DatabaseName"] = "TestDb"
            };

            configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(initialData!)
                .Build();
        }

        [TestMethod]
        public void AddInfrastructure_RegistersServices_CanResolveTypesAndLifetimes()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            services.AddInfrastructure(configuration);
            var provider = services.BuildServiceProvider();

            // Assert
            var database = provider.GetService<IMongoDatabase>();
            var baseRepository = provider.GetService<IBaseRepository>();
            var seeder = provider.GetService<IDbSeeder>();

            Assert.IsNotNull(database);
            Assert.IsNotNull(baseRepository);
            Assert.IsNotNull(seeder);

            var database2 = provider.GetService<IMongoDatabase>();
            Assert.AreSame(database, database2);

            Assert.IsInstanceOfType(baseRepository, typeof(BaseRepository));
            Assert.IsInstanceOfType(seeder, typeof(PlayerDbSeeder));
        }

        [TestMethod]
        public async Task SeedDatabaseAsync_DoesNotCallSeedAsync_WhenHasDataIsTrue()
        {
            // Arrange
            var mockSeeder = new Mock<IDbSeeder>(MockBehavior.Strict);
            mockSeeder.Setup(s => s.HasDataAsync()).ReturnsAsync(true);

            var services = new ServiceCollection();
            services.AddScoped<IDbSeeder>(_ => mockSeeder.Object);
            var provider = services.BuildServiceProvider();

            // Act
            await provider.SeedDatabaseAsync();

            // Assert
            mockSeeder.Verify(s => s.HasDataAsync(), Times.Once);
            mockSeeder.Verify(s => s.SeedAsync(), Times.Never);
        }

        [TestMethod]
        public async Task SeedDatabaseAsync_CallsSeedAsync_WhenHasDataIsFalse()
        {
            // Arrange
            var mockSeeder = new Mock<IDbSeeder>(MockBehavior.Strict);
            mockSeeder.Setup(s => s.HasDataAsync()).ReturnsAsync(false);
            mockSeeder.Setup(s => s.SeedAsync()).Returns(Task.CompletedTask).Verifiable();

            var services = new ServiceCollection();
            services.AddScoped<IDbSeeder>(_ => mockSeeder.Object);
            var provider = services.BuildServiceProvider();

            // Act
            await provider.SeedDatabaseAsync();

            // Assert
            mockSeeder.Verify(s => s.HasDataAsync(), Times.Once);
            mockSeeder.Verify(s => s.SeedAsync(), Times.Once);
        }
    }
}
