using L072NS_HSZF_2024251.Application.Interfaces;
using L072NS_HSZF_2024251.Application.Services;
using L072NS_HSZF_2024251.Persistence.MsSql;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L072NS_HSZF_2024251.Test.StatisticsServiceTests;

public class StatisticsLowDelaysTest
{
    IStatisticsService statisticsService;
    IRegionService regionService;
    Mock<IRegionRepository> regionRepo;


    [SetUp]
    public void Init()
    {
        regionRepo = new Mock<IRegionRepository>(MockBehavior.Strict);
        regionService = new RegionService(regionRepo.Object);
        statisticsService = new StatisticsService(regionService);

        regionRepo.Setup(r => r.Batch()).Returns(TestData.RegionSample);

    }


    [Test]
    public void CorrectValues()
    {
        var stats = statisticsService.GetAmountOfLowDelaysByRegion();
        Assert.That(stats.Count, Is.EqualTo(2));
        Assert.That(stats.First(x => x.RegionName == "Bács-Kiskun vármegye").Count, Is.EqualTo(1));
        Assert.That(stats.First(x => x.RegionName == "Pest vármegye").Count, Is.EqualTo(0));
    }
}
