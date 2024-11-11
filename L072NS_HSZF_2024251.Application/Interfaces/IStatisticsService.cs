using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L072NS_HSZF_2024251.Application.Interfaces
{
    public interface IStatisticsService
    {
        public ICollection<(string RegionName, int Count)> GetAmountOfLowDelaysByRegion();
        public ICollection<(string RegionName, string StationName, int DelayCount)> GetMostDelayedDestinationByRegion();
        public ICollection<(string RegionName, int LowDelayAvg, int HighDelayAvg)> GetAvgDelayByRegion();
    }
}
