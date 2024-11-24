using L072NS_HSZF_2024251.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L072NS_HSZF_2024251.Application.Interfaces
{
    public interface IRegionService
    {
        ICollection<Region> GetAllRegions();
        ICollection<Region> SearchRegions(RegionDto dto);

        Region GetRegionById(int id);

        void UploadRegion(Region region);
        void UpdateRegion(int id, Region region);
        void DeleteRegion(int id);
    }
}
