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
    public class RouteService : IRouteService
    {
        private readonly IRouteRepository _routeRepository;
        private readonly IRegionRepository _regionRepository;
        
        public RouteService(IRouteRepository routeRepo, IRegionRepository regionRepo)
        {
            _routeRepository = routeRepo;
            _regionRepository = regionRepo;
        }

        public void DeleteRoute(Guid id)
        {
            Route? route = _routeRepository.Get(id);

            if (route == null)
                throw new KeyNotFoundException();

            _routeRepository.Delete(route);
        }

        public ICollection<Route> GetAllRoutes()
        {
            return _routeRepository.Batch();
        }

        public ICollection<Route> GetRoutesByRegion(int regionNumber)
        {
            Region? search = _regionRepository.Batch().FirstOrDefault(e=>e.RegionNumber == regionNumber);

            if (search == null)
                throw new KeyNotFoundException();

            return search.Routes;
        }

        public Route GetRouteById(Guid id)
        {
            Route? route = _routeRepository.Get(id);

            if (route == null)
                throw new KeyNotFoundException();

            return route;
        }

        public void UpdateRoute(Guid id, Route modifiedRoute)
        {
            if (id != modifiedRoute.Id)
                throw new ArgumentException();

            if (_regionRepository.Get(modifiedRoute.RegionId) == null)
                throw new KeyNotFoundException();

            _routeRepository.Update(id, modifiedRoute);
        }

        public void UploadRoute(Route route)
        {
            Region? region = _regionRepository.Get(route.RegionId);

            if (region == null)
                throw new KeyNotFoundException();

            _routeRepository.Add(route);
        }
    }
}
