using L072NS_HSZF_2024251.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L072NS_HSZF_2024251.Persistence.MsSql
{
    public class RegionRepository : IRegionRepository
    {
        private readonly BusContext context;

        public RegionRepository(BusContext context)
        {
            this.context = context;
        }

        public Region Add(Region region)
        {
            Region add = context.Regions.Add(region).Entity;
            context.SaveChanges();
            return add;
        }

        public ICollection<Region> Batch()
        {
            return context.Regions.Include(e => e.Routes).ToHashSet();
        }

        public void Delete(int id)
        {
            Region search = context.Regions.First(e => e.RegionNumber == id);
            context.Remove(search);
            context.SaveChanges();
        }

        public Region? Get(int id)
        {
            return context.Regions.FirstOrDefault(e => e.RegionNumber == id);
        }

        public void Update(int id, Region region)
        {
            Region dbObj = context.Regions.First(e => e.RegionNumber == id)!;
            dbObj.RegionName = region.RegionName;
            context.SaveChanges();
        }
    }
}
