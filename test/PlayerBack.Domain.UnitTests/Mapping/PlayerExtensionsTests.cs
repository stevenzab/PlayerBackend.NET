using PlayerBack.Domain.Mapping;
using PlayerBack.Domain.Models;
using System;

namespace PlayerBack.Domain.UnitTests.Mapping
{
    [TestClass]
    public class PlayerExtensionsTests
    {
        [TestMethod]
        public void MapToDto_FullPlayer_MapsProperties()
        {
            var source = new Player
            {
                Id = "1",
                FirstName = "Alice",
                LastName = "A",
                ShortName = "A.A.",
                Sex = "F",
                Picture = "picture_url",
                Country = new Country { Code = "ESP", Picture = "pic" },
                Data = new PlayerData { Height = 180, Weight = 70, Rank = 1, Points = 100, Age = 25, Last = new List<int> { 1, 2 } }
            };

            var dto = source.MapToDto();

            Assert.IsNotNull(dto);
            Assert.AreEqual("1", dto.PlayerId);
            Assert.AreEqual("Alice", dto.FirstName);
            Assert.AreEqual("A", dto.LastName);
            Assert.AreEqual("A.A.", dto.ShortName);
            Assert.AreEqual("F", dto.Sex);
            Assert.AreEqual("picture_url", dto.Picture);
            Assert.IsNotNull(dto.Country);
            Assert.AreEqual("ESP", dto.Country.Code);
            Assert.IsNotNull(dto.Data);
            CollectionAssert.AreEqual(new List<int> { 1, 2 }, dto.Data.Last);
        }
    }
}
