using L072NS_HSZF_2024251.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace L072NS_HSZF_2024251.Test
{
    public class TestData
    {
        public static Route RouteSample = new()
        {
            Id = new Guid("24bff74a-ecc6-4698-9d8c-55f6af72ac1d"),
            BusNumber = 1,
            BusType = "Local",
            DelayAmount = 10,
            From = "Szolnok",
            To = "Budapest",
            RegionId = 1
        };
        public static ICollection<Region> RegionSample = [
                new()
                {
                    RegionName = "Bács-Kiskun vármegye",
                    RegionNumber = 1,
                    Routes = [
                            new(){
                                Id = new Guid(RandomNumberGenerator.GetBytes(16)),
                                BusNumber = 1,
                                BusType = "Local",
                                From = "Baja hűtőház",
                                To = "Baja Tesco",
                                DelayAmount = 12,
                                RegionId = 1
                            },
                            new(){
                                Id = new Guid(RandomNumberGenerator.GetBytes(16)),
                                BusNumber = 1,
                                BusType = "Local",
                                From = "Baja hűtőház",
                                To = "Baja Tesco",
                                DelayAmount = 1,
                                RegionId = 1
                            },
                            new(){
                                Id = new Guid(RandomNumberGenerator.GetBytes(16)),
                                BusNumber = 1,
                                BusType = "Local",
                                From = "Baja hűtőház",
                                To = "Baja Tesco",
                                DelayAmount = 32,
                                RegionId = 1
                            },
                            new(){
                                Id = new Guid(RandomNumberGenerator.GetBytes(16)),
                                BusNumber = 10,
                                BusType = "Local",
                                From = "Baja autóbuszállomás",
                                To = "Szeremle",
                                DelayAmount = 6,
                                RegionId = 1
                            },
                            new(){
                                Id = new Guid(RandomNumberGenerator.GetBytes(16)),
                                BusNumber = 11,
                                BusType = "Local",
                                From = "Baja autóbuszállomás",
                                To = "Baja Tesco",
                                DelayAmount = 7,
                                RegionId = 1
                            }
                        ]
                },
                new()
                {
                    RegionName = "Pest vármegye",
                    RegionNumber = 2,
                    Routes = []
                }
            ];
    }
}
