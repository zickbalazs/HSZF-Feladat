﻿using L072NS_HSZF_2024251.Application.Interfaces;
using L072NS_HSZF_2024251.Application.Services;
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
public class RouteGetAllTest
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

        routeRepo.Setup(r => r.Batch()).Returns([
                    TestData.RouteSample
                ]);
    }

    [Test]
    public void TestMocking()
    {
        Assert.That(() => routeService.GetAllRoutes().Count, Is.EqualTo(1));
    }

}