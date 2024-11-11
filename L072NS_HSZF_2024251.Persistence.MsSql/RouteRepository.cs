using L072NS_HSZF_2024251.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L072NS_HSZF_2024251.Persistence.MsSql
{
    public class RouteRepository : IRouteRepository
    {
        private readonly BusContext context;

        public RouteRepository(BusContext context)
        {
            this.context = context;
        }

        public void Add(Route route)
        {
            context.Routes.Add(route);
            context.SaveChanges();
        }

        public ICollection<Route> Batch()
        {
            return context.Routes.ToHashSet();
        }

        public void Delete(Route route)
        {
            Route dbObj = context.Routes.FirstOrDefault(e=>e.Id == route.Id)!;
            context.Routes.Remove(dbObj);
            context.SaveChanges();
        }

        public Route? Get(Guid id)
        {
            return context.Routes.FirstOrDefault(e => e.Id == id);
        }

        public void Update(Guid id, Route update)
        {
            Route dbObj = context.Routes.FirstOrDefault(e => e.Id == id)!;
            dbObj = update;
            context.SaveChanges();
        }
    }
}
