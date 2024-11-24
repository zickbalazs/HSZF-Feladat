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

        public event EventHandler? OnLowestDelayAdded;

        public RouteService(IRouteRepository routeRepo, IRegionRepository regionRepo)
        {
            _routeRepository = routeRepo;
            _regionRepository = regionRepo;
        }

        public ICollection<Route> GetAllRoutes()
        {
            return _routeRepository.Batch();
        }
        public Route GetRouteById(Guid id)
        {
            Route? route = _routeRepository.Get(id);

            if (route == null)
                throw new KeyNotFoundException();

            return route;
        }

        public void UploadRoute(Route route)
        {
            Region? region = _regionRepository.Get(route.RegionId);

            if (region == null)
                throw new KeyNotFoundException();


            if (region.Routes.Count > 0)
            {
                if (region.Routes.Min(x => x.DelayAmount) > route.DelayAmount)
                    OnLowestDelayAdded?.Invoke(this, new());
            }
            else
            {
                OnLowestDelayAdded?.Invoke(this, new());
            }
            _routeRepository.Add(route);
        }
    }
}
