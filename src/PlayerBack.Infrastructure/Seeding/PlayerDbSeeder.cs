using MongoDB.Driver.Linq;
using PlayerBack.Domain.Models;
using PlayerBack.Domain.SeedModels;
using PlayerBack.Infrastructure.Common;
using System.Text.Json;

namespace PlayerBack.Infrastructure.Seeding
{
    public class PlayerDbSeeder : IDbSeeder
    {
        private readonly IBaseRepository repository;
        private readonly string seedFilePath;

        public PlayerDbSeeder(IBaseRepository repository)
        {
            this.repository = repository;
            seedFilePath = Path.Combine(AppContext.BaseDirectory, "response.json");
        }

        public async Task<bool> HasDataAsync()
        {
            var hasPlayers = await repository.AsQueryable<Player>().AnyAsync();
            return hasPlayers;
        }

        public async Task SeedAsync()
        {
            if (!File.Exists(seedFilePath))
                return;

            var jsonContent = await File.ReadAllTextAsync(seedFilePath);
            var seedData = JsonSerializer.Deserialize<SeedData>(jsonContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (seedData?.Players == null || seedData.Players.Count == 0)
                return;

            foreach (var playerData in seedData.Players)
            {
                var player = new Player
                {
                    PlayerId = playerData.Id,
                    Firstname = playerData.Firstname ?? string.Empty,
                    Lastname = playerData.Lastname ?? string.Empty,
                    Shortname = playerData.Shortname ?? string.Empty,
                    Sex = playerData.Sex ?? string.Empty,
                    Picture = playerData.Picture ?? string.Empty,
                    Country = new Country
                    {
                        Picture = playerData.Country?.Picture ?? string.Empty,
                        Code = playerData.Country?.Code ?? string.Empty
                    },
                    Data = new PlayerData
                    {
                        Rank = playerData.Data?.Rank ?? 0,
                        Points = playerData.Data?.Points ?? 0,
                        Weight = playerData.Data?.Weight ?? 0,
                        Height = playerData.Data?.Height ?? 0,
                        Age = playerData.Data?.Age ?? 0,
                        Last = playerData.Data?.Last ?? new List<int>()
                    }
                };

                await repository.AddAsync(player);
            }
        }
    }
}