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

namespace L072NS_HSZF_2024251.Test.RouteServiceTests;

[TestFixture]
public class RouteByIdTests
{
    private Mock<IRouteRepository> routeRepo;
    private Mock<IRegionRepository> regionRepo;
    private IRouteService routeService;
    
    [SetUp]
    public void Init()
    {
        routeRepo = new Mock<IRouteRepository>(MockBehavior.Strict);
        regionRepo = new Mock<IRegionRepository>(MockBehavior.Strict);
        routeService = new RouteService(routeRepo.Object, regionRepo.Object);

        routeRepo.Setup(r => r.Get(It.IsAny<Guid>())).Returns((Guid id) => id.ToString() == "24bff74a-ecc6-4698-9d8c-55f6af72ac1d" ? TestData.RouteSample : null);
    }

    [Test]
    public void FoundItem()
    {
        Route? route = routeService.GetRouteById(new("24bff74a-ecc6-4698-9d8c-55f6af72ac1d"));

        Assert.IsNotNull(route);

        routeRepo.Verify(r => r.Get(new("24bff74a-ecc6-4698-9d8c-55f6af72ac1d")));
    }

    [Test]
    public void NotFound()
    {

        Assert.Throws<KeyNotFoundException>(() => routeService.GetRouteById(new()));

        routeRepo.Verify(r => r.Get(new()));
    }

}
