using L072NS_HSZF_2024251.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L072NS_HSZF_2024251.Persistence.MsSql
{
    public interface IRouteRepository
    {
        ICollection<Route> Batch();
        Route? Get(Guid id);
        void Add(Route route);
        void Delete(Route id);
        void Update(Guid id, Route update);
    }
}
