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

[TestFixture]
public class StatisticsMostDelayedTest
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
    public void Check()
    {
        Assert.DoesNotThrow(() => statisticsService.GetMostDelayedStationByRegions());
        var stats = statisticsService.GetMostDelayedStationByRegions();

        var kiskunStat = stats.First(e => e.RegionName == "Bács-Kiskun vármegye");
        var pestStat = stats.First(e => e.RegionName == "Pest vármegye");

        Assert.That(kiskunStat.MostDelayedTerminusName, Is.EqualTo("Baja Tesco"));
        Assert.That(kiskunStat.Delay, Is.EqualTo(52));
        Assert.That(pestStat.MostDelayedTerminusName, Is.EqualTo(""));
        Assert.That(pestStat.Delay, Is.EqualTo(0));
    }
}
