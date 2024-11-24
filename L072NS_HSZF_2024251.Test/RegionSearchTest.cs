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
public class RegionSearchTest
{
    Mock<IRegionRepository> regionRepo;
    IRegionService regionService;
    [SetUp]
    public void Init()
    {
        regionRepo = new Mock<IRegionRepository>(MockBehavior.Strict);
        
        regionService = new RegionService(regionRepo.Object);
        
        regionRepo.Setup(r => r.Batch()).Returns(TestData.RegionSample);
    }
    [Test]
    public void NothingFound()
    {
        ICollection<Region> regions = regionService.SearchRegions(new() { RegionName = "xd", RegionNumber = -32 });
        Assert.That(regions.Count, Is.EqualTo(0));
        regionRepo.Verify(x => x.Batch(), Times.Once());
    }
    [Test]
    public void EverythingFound()
    {
        ICollection<Region> regions = regionService.SearchRegions(new());
        Assert.That(regions.Count, Is.EqualTo(TestData.RegionSample.Count));
    }
    [Test]
    public void ExactFind()
    {
        ICollection<Region> regions = regionService.SearchRegions(new() { RegionNumber = 1 });
        Assert.That(regions.Count, Is.EqualTo(1));
        regionRepo.Verify(x => x.Batch(), Times.Once());
    }
}
