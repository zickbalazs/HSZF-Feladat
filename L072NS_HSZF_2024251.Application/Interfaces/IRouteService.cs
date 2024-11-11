using L072NS_HSZF_2024251.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L072NS_HSZF_2024251.Application.Interfaces
{
    public interface IRouteService
    {
        ICollection<Route> GetAllRoutes();
        ICollection<Route> GetRoutesByRegion(int regionNumber);
        Route GetRouteById(Guid id);

        public void UploadRoute(Route route);
        public void DeleteRoute(Guid id);
        public void UpdateRoute(Guid id, Route modifiedRoute);
    }
}
