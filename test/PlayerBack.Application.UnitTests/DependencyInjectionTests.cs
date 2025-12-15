using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PlayerBack.Application;
using PlayerBack.Application.Services.PlayerNs;
using PlayerBack.Application.Services.PlayerNs.DataAccess;
using PlayerBack.Infrastructure.Common;
using System;

namespace PlayerBack.Application.UnitTests
{
    [TestClass]
    public class DependencyInjectionTests
    {
        //[TestMethod]
        //public void AddApplication_RegistersServices_ReturnsResolvableTypes()
        //{
        //    // Arrange
        //    var services = new ServiceCollection();

        //    // Register a mock for IBaseRepository so PlayerDataAccess can be constructed by the container.
        //    var mockBaseRepo = new Mock<IBaseRepository>();
        //    services.AddSingleton<IBaseRepository>(mockBaseRepo.Object);

        //    // Act
        //    services.AddApplication();
        //    using var provider = services.BuildServiceProvider();

        //    var playerService = provider.GetService<IPlayerService>();
        //    var playerDataAccess = provider.GetService<IPlayerDataAccess>();

        //    // Assert
        //    Assert.IsNotNull(playerService, "IPlayerService should be registered and resolvable.");
        //    Assert.IsNotNull(playerDataAccess, "IPlayerDataAccess should be registered and resolvable.");

        //    Assert.IsInstanceOfType(playerService, typeof(PlayerService));
        //    Assert.IsInstanceOfType(playerDataAccess, typeof(PlayerDataAccess));
        //}
    }
}
