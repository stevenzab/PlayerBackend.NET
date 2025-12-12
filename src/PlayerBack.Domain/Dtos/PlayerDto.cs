namespace PlayerBack.Domain.Dtos
{
    using System.Collections.Generic;

    public class PlayerDto
    {
        public int PlayerId { get; set; }

        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public string Shortname { get; set; }

        public string Sex { get; set; }

        public CountryDto Country { get; set; }

        public string Picture { get; set; }

        public PlayerDataDto Data { get; set; }
    }

    public class CountryDto
    {
        public string Picture { get; set; }

        public string Code { get; set; }
    }

    public class PlayerDataDto
    {
        public int Rank { get; set; }

        public int Points { get; set; }

        public int Weight { get; set; }

        public int Height { get; set; }

        public int Age { get; set; }

        public List<int> Last { get; set; }
    }
}