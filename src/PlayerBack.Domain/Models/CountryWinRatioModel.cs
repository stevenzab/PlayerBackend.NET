using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayerBack.Domain.Models
{
    public class CountryWinRatioModel
    {
        public string? CountryCode { get; set; }
        public double Ratio { get; set; }
    }
}
