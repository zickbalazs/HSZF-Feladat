using L072NS_HSZF_2024251.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L072NS_HSZF_2024251.Persistence.MsSql
{
    public interface IRegionRepository
    {
        ICollection<Region> Batch();
        Region? Get(int id);
        void Delete(int id);
        void Update(int id, Region region);
        Region Add(Region region);
    }
}
