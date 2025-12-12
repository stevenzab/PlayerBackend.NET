using PlayerBack.Domain.Dtos;
using PlayerBack.Domain.Models;

namespace PlayerBack.Domain.Mapping
{
    public static class PlayerExtensions
    {
        public static PlayerDto MapToDto(this Player source)
        {
            if (source == null) return null;

            return new PlayerDto
            {
                PlayerId = source.PlayerId,
                Firstname = source.Firstname,
                Lastname = source.Lastname,
                Shortname = source.Shortname,
                Sex = source.Sex,
                Country = new CountryDto
                {
                    Picture = source.Country.Picture,
                    Code = source.Country.Code
                },
                Picture = source.Picture,
                Data = new PlayerDataDto
                {
                    Rank = source.Data.Rank,
                    Points = source.Data.Points,
                    Weight = source.Data.Weight,
                    Height = source.Data.Height,
                    Age = source.Data.Age,
                    Last = source.Data.Last
                }
            };
        }
    }
}