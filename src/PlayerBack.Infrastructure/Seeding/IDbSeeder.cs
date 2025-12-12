namespace PlayerBack.Infrastructure.Seeding
{
    public interface IDbSeeder
    {
        Task<bool> HasDataAsync();

        Task SeedAsync();
    }
}