using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L072NS_HSZF_2024251.Application.Interfaces
{
    public interface IStatisticsService
    {
        ICollection<(string RegionName, int Count)> GetAmountOfLowDelaysByRegion();
        ICollection<(string RegionName, int AvgDelay, (int Id, int Amount) MostDelayed, (int Id, int Amount) LeastDelayed)> GetAvgDelayByRegions();
        ICollection<(string RegionName, string MostDelayedTerminusName, int Delay)> GetMostDelayedStationByRegions();
    }
}
