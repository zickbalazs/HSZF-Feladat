using L072NS_HSZF_2024251.Application.Interfaces;
using L072NS_HSZF_2024251.Model;
using L072NS_HSZF_2024251.Persistence.MsSql;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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

        public void ExportDatabaseToJSON(string folderPath, out string generatedFileName)
        {
            ICollection<Region> regions = _regionRepo.Batch();
            JObject j = new JObject();
            j["BusRegions"] = JArray.FromObject(regions);
            string _out = j.ToString(Formatting.Indented);
            generatedFileName = $"NLB-export-{DateTime.Now.ToFileTimeUtc()}.json";
            if (Directory.Exists(folderPath))
                File.WriteAllText($@"{folderPath}\{generatedFileName}", _out);
            else throw new DirectoryNotFoundException();

        }

        public void ImportJSONToDatabase(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException();

            JToken obj = JToken.Parse(File.ReadAllText(filePath));
            
            try
            {
                JObject i = (JObject)obj;
            }
            catch
            {
                throw new FormatException("The file doesn't contain the BusRegions Root element");
            }

            JToken token = obj["BusRegions"] ?? throw new FormatException("Couldn't find the root BusRegions element!");

            

            ICollection<Region> dbRegions = _regionRepo.Batch();

            ICollection<Region> regions;
            try
            {
                regions = JsonConvert.DeserializeObject<ICollection<Region>>(token.ToString(), new JsonSerializerSettings()
                {
                    MissingMemberHandling = MissingMemberHandling.Error
                })!;
            }
            catch
            {
                throw new FormatException("The BusRegions field is not correctly formatted!");
            }

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
