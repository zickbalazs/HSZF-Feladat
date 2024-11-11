using L072NS_HSZF_2024251.Application.Interfaces;
using L072NS_HSZF_2024251.Model;
using L072NS_HSZF_2024251.Persistence.MsSql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L072NS_HSZF_2024251.Application.Services
{
    public class RegionService : IRegionService
    {

        private readonly IRegionRepository _regionRepository;

        public RegionService(IRegionRepository regionRepository)
        {
            _regionRepository = regionRepository;
        }

        public void DeleteRegion(int id)
        {
            Region? region = _regionRepository.Get(id);
            if (region == null)
                throw new KeyNotFoundException();
            _regionRepository.Delete(id);
        }

        public ICollection<Region> GetAllRegions()
        {
            return _regionRepository.Batch();
        }

        public Region GetRegionById(int id)
        {
            Region? search = _regionRepository.Get(id);
            if (search == null)
                throw new KeyNotFoundException();
            return search;
        }

        public ICollection<Region> GetRegionsByFilter(Func<Region, bool> filter)
        {
            return _regionRepository.Batch().Where(filter).ToHashSet();
        }

        public void UpdateRegion(int id, Region region)
        {
            if (region.RegionNumber != id)
                throw new ArgumentException();
            _regionRepository.Add(region);
        }

        public void UploadRegion(Region region)
        {
            _regionRepository.Add(region);
        }
    }
}
