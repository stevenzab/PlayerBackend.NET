namespace PlayerBack.Domain.SeedModels
{
    public class PlayerSeedDto
    {
        public int Id { get; set; }
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public string? Shortname { get; set; }
        public string? Sex { get; set; }
        public string? Picture { get; set; }
        public CountrySeedDto? Country { get; set; }
        public PlayerDataSeedDto? Data { get; set; }
    }
}
