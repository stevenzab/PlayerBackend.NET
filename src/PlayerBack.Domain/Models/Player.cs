namespace PlayerBack.Domain.Models
{
    public class Player : RepositoryCollection
    {
        public int PlayerId { get; set; }

        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public string Shortname { get; set; }

        public string Sex { get; set; }

        public Country Country { get; set; }

        public string Picture { get; set; }

        public PlayerData Data { get; set; }
    }

    public class Country
    {
        public string Picture { get; set; }

        public string Code { get; set; }
    }

    public class PlayerData
    {
        public int Rank { get; set; }

        public int Points { get; set; }

        public int Weight { get; set; }

        public int Height { get; set; }

        public int Age { get; set; }

        public List<int> Last { get; set; }
    }
}