using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayerBack.Domain.Models
{
    public class StatisticsModel
    {
        public string? CountryCodeWithHighestWinRatio { get; set; }
        public double HighestWinRatio { get; set; }
        public double AverageBmi { get; set; }
        public double MedianHeight { get; set; }
    }
}
