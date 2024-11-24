using L072NS_HSZF_2024251.Application.Interfaces;
using L072NS_HSZF_2024251.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L072NS_HSZF_2024251.Application.Services
{
    public class StatisticsService(IRegionService regionService) : IStatisticsService
    {
        private readonly IRegionService _regionService = regionService;
        public ICollection<(string RegionName, int Count)> GetAmountOfLowDelaysByRegion() =>
            _regionService.GetAllRegions()
                .Select(e => (e.RegionName,
                              e.Routes.Count(e => e.DelayAmount <= 5)))
                .ToHashSet();

        public ICollection<(string RegionName, int AvgDelay, (int Id, int Amount) MostDelayed, (int Id, int Amount) LeastDelayed)> GetAvgDelayByRegions()
        {
            return _regionService.GetAllRegions().Select(
                e =>
                {
                    string regionName = e.RegionName;
                    int averageDelay = e.Routes.Count > 0 ? Convert.ToInt32(e.Routes.Average(e => e.DelayAmount)) : 0;
                    (int id, int amount) mostDelayed = (GetMostDelayedId(e), e.Routes.Where(h=>h.BusNumber == GetMostDelayedId(e)).Sum(x=>x.DelayAmount));
                    (int id, int amount) leastDelayed = (GetLeastDelayedId(e), e.Routes.Where(h => h.BusNumber == GetLeastDelayedId(e)).Sum(x => x.DelayAmount));
                    return (regionName, averageDelay, mostDelayed, leastDelayed);
                }).ToHashSet();
        }

        private int GetLeastDelayedId(Region region)
        {
            var grouped = region.Routes.GroupBy(e => e.BusNumber).Select(h => new
            {
                h.Key,
                DelaySum = h.Sum(v=>v.DelayAmount)
            });
            return grouped.FirstOrDefault(e => e.DelaySum == grouped.Min(x => x.DelaySum))?.Key ?? -1;
        }

        private int GetMostDelayedId(Region region)
        {
            var grouped = region.Routes.GroupBy(e => e.BusNumber).Select(h => new
            {
                h.Key,
                DelaySum = h.Sum(v => v.DelayAmount)
            });
            return grouped.FirstOrDefault(e => e.DelaySum == grouped.Max(x => x.DelaySum))?.Key ?? -1;
        }

        public ICollection<(string RegionName, string MostDelayedTerminusName, int Delay)> GetMostDelayedStationByRegions()
        {
            return _regionService.GetAllRegions().Select(GetMostDelayedStationForRegion).ToHashSet();
        }

        private (string RegionName, string MostDelayedTerminusName, int Delay) GetMostDelayedStationForRegion(Region region)
        {
            string regionName = region.RegionName;

            var grouping = region.Routes.GroupBy(x => x.To).Select(h => new
            {
                h.Key,
                DelaySum = h.Sum(v => v.DelayAmount)
            });

            string terminus = grouping.FirstOrDefault(e => e.DelaySum == grouping.Max(x => x.DelaySum))?.Key ?? "";
            int amount = terminus == "" ? 0 : grouping.Max(x => x.DelaySum);
            return (regionName, terminus, amount);
        }
    }
}
