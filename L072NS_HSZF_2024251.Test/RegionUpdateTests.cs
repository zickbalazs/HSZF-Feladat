using L072NS_HSZF_2024251.Application.Interfaces;
using L072NS_HSZF_2024251.Application.Services;
using L072NS_HSZF_2024251.Model;
using L072NS_HSZF_2024251.Persistence.MsSql;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L072NS_HSZF_2024251.Test.RegionServiceTests;

[TestFixture]
public class RegionUpdateTests
{
    private Mock<IRegionRepository> regionRepo;
    IRegionService regionService;

    [SetUp]
    public void Init()
    {
        regionRepo = new Mock<IRegionRepository>();
        regionService = new RegionService(regionRepo.Object);

        regionRepo.Setup(r => r.Update(It.IsAny<int>(), It.IsAny<Region>())).Verifiable();
        regionRepo.Setup(r => r.Get(It.IsAny<int>())).Returns((int i) => TestData.RegionSample.FirstOrDefault(e=>e.RegionNumber == i));

    }


    [Test]
    public void SuccessfulUpdate()
    {
        Region _new = new()
        {
            RegionNumber = 1,
            RegionName = "Bács-Kiskun megye",
            Routes = []
        };
        regionService.UpdateRegion(1, _new);
        regionRepo.Verify(e=>e.Get(1));
        regionRepo.Verify(e => e.Update(1, _new));
    }

    [Test]
    public void BadUpdateId()
    {
        Assert.Throws<ArgumentException>(() => regionService.UpdateRegion(1, new()
        {
            RegionName = "Changed",
            RegionNumber = 2,
            Routes = []
        }));
        regionRepo.Verify(r => r.Get(It.IsAny<int>()), Times.Never());
    }

    [Test]
    public void NotFound()
    {
        Assert.Throws<KeyNotFoundException>(() => regionService.UpdateRegion(4, new()
        {
            RegionNumber = 4,
            RegionName = "Changed",
            Routes = []
        }));
        regionRepo.Verify(r => r.Get(4));
    }

}
