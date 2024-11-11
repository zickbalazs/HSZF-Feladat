using L072NS_HSZF_2024251.Application.Interfaces;
using L072NS_HSZF_2024251.Model;
using L072NS_HSZF_2024251.Persistence.MsSql;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L072NS_HSZF_2024251.Application.Services
{
    public class FileService : IFileService
    {
        IRegionRepository _regionRepo;
        IRouteRepository _routeRepo;

        public FileService(IRegionRepository regionRepo, IRouteRepository routeRepo)
        {
            _regionRepo = regionRepo;
            _routeRepo = routeRepo;
        }

        public void ExportDatabaseToJSON(string folderPath)
        {
            ICollection<Region> regions = _regionRepo.Batch();
            JObject j = new JObject();
            j["BusRegions"] = JsonConvert.SerializeObject(regions);
            j.ToString();


        }

        public void ImportJSONToDatabase(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException();

            JToken? obj = JObject.Parse(File.ReadAllText(filePath))["BusRegions"];
            if (obj == null)
                throw new FormatException();

            ICollection<Region> dbRegions = _regionRepo.Batch();

            ICollection<Region>? regions = JsonConvert.DeserializeObject<ICollection<Region>>(obj.ToString());
            if (regions == null)
                throw new FormatException();

            foreach (Region region in regions)
            {
                Region? existing = dbRegions.FirstOrDefault(e=>e.RegionName == region.RegionName);
                if (existing == null)
                {
                    int regionId = _regionRepo.Add(new Region() { RegionName = region.RegionName }).RegionNumber;
                    region.Routes = region.Routes.Select(e => new Route()
                    {
                        BusNumber = e.BusNumber,
                        BusType = e.BusType,
                        DelayAmount = e.DelayAmount,
                        From = e.From,
                        To = e.To,
                        RegionId = regionId
                    }).ToHashSet();
                }
                else
                {
                    region.Routes = region.Routes.Select(e =>new Route()
                    {
                        BusNumber = e.BusNumber,
                        BusType = e.BusType,
                        DelayAmount = e.DelayAmount,
                        From = e.From,
                        To = e.To,
                        RegionId = existing.RegionNumber
                    }).ToHashSet();
                }
                foreach (Route route in region.Routes)
                {
                    _routeRepo.Add(route);
                }
            }
        }
    }
}
