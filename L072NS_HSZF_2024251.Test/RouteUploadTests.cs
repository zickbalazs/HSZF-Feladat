using L072NS_HSZF_2024251.Application.Interfaces;
using L072NS_HSZF_2024251.Application.Services;
using L072NS_HSZF_2024251.Model;
using L072NS_HSZF_2024251.Persistence.MsSql;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace L072NS_HSZF_2024251.Test.RouteServiceTests;
[TestFixture]
public class RouteUploadTests
{
    private Mock<IRouteRepository> routeRepo;
    private Mock<IRegionRepository> regionRepo;
    private IRouteService routeService;
    bool eventCalledChecker;

    [SetUp]
    public void Init()
    {
        routeRepo = new Mock<IRouteRepository>(MockBehavior.Strict);
        regionRepo = new Mock<IRegionRepository>(MockBehavior.Strict);
        routeService = new RouteService(routeRepo.Object, regionRepo.Object);
        
        eventCalledChecker = false;

        regionRepo.Setup(r => r.Get(It.IsAny<int>())).Returns((int obj) => TestData.RegionSample.FirstOrDefault(e=>e.RegionNumber == obj));

        routeRepo.Setup(r => r.Add(It.IsAny<Route>())).Verifiable();

        routeService.OnLowestDelayAdded += (obj, args) => eventCalledChecker = true;
    }

    [Test]
    public void SuccessfulUploadToEmptyRoutes()
    {
        Route newRoute = new()
        {
            BusNumber = 10,
            BusType = "Local",
            DelayAmount = 90,
            RegionId = 2,
            From = "Igen",
            To = "Nem"
        };
        routeService.UploadRoute(newRoute);

        regionRepo.Verify(r => r.Get(2));
        routeRepo.Verify(r => r.Add(newRoute));

        Assert.That(eventCalledChecker, Is.True);
    }


    [Test]
    public void SuccessfulUploadWithLateBus()
    {
        Route newRoute = new()
        {
            BusNumber = 10,
            BusType = "Local",
            DelayAmount = 90,
            RegionId = 1,
            From = "Igen",
            To = "Nem"
        };
        routeService.UploadRoute(newRoute);

        regionRepo.Verify(r => r.Get(1));
        routeRepo.Verify(r => r.Add(newRoute));

        Assert.That(eventCalledChecker, Is.False);
    }

    [Test]
    public void SuccessfulUploadWithMinDelay()
    {
        Route newRoute = new()
        {
            BusNumber = 11,
            BusType = "Local",
            DelayAmount = -10,
            RegionId = 1,
            From = "Igen",
            To = "Nem"
        };
        routeService.UploadRoute(newRoute);

        regionRepo.Verify(r => r.Get(1));
        routeRepo.Verify(r => r.Add(newRoute));

        Assert.That(eventCalledChecker, Is.True);
    }

    [Test]
    public void NoRegionError()
    {
        Assert.Throws<KeyNotFoundException>(() => routeService.UploadRoute(new()
        {
            BusNumber = 10,
            BusType = "Local",
            DelayAmount = 8,
            RegionId = 3,
            From = "Igen",
            To = "Nem"
        }));
    }
}
