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
public class StatisticsAvgDelaysTest
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
    public void NonEmptyListStats()
    {
        Assert.DoesNotThrow(() => statisticsService.GetAvgDelayByRegions());
        var stat = statisticsService.GetAvgDelayByRegions().First(x => x.RegionName == "Bács-Kiskun vármegye");
        Assert.That(stat.AvgDelay, Is.EqualTo(12));
        Assert.That(stat.LeastDelayed.Id, Is.EqualTo(10));
        Assert.That(stat.LeastDelayed.Amount, Is.EqualTo(6));
        Assert.That(stat.MostDelayed.Id, Is.EqualTo(1));
        Assert.That(stat.MostDelayed.Amount, Is.EqualTo(45));
    }


    [Test]
    public void EmptyListNoException()
    {
        Assert.DoesNotThrow(() => statisticsService.GetAvgDelayByRegions());
        var emptyStat = statisticsService.GetAvgDelayByRegions().First(x => x.RegionName == "Pest vármegye");
        Assert.That(emptyStat.AvgDelay, Is.EqualTo(0));
        Assert.That(emptyStat.MostDelayed.Id, Is.EqualTo(-1));
        Assert.That(emptyStat.LeastDelayed.Id, Is.EqualTo(emptyStat.MostDelayed.Id));
    }
}
