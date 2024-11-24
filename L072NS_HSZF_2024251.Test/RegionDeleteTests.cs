﻿using L072NS_HSZF_2024251.Application.Interfaces;
using L072NS_HSZF_2024251.Application.Services;
using L072NS_HSZF_2024251.Persistence.MsSql;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L072NS_HSZF_2024251.Test.RegionServiceTests;

[TestFixture]
public class RegionDeleteTests
{
    Mock<IRegionRepository> regionRepo;
    IRegionService regionService;

    [SetUp]
    public void Init()
    {
        regionRepo = new Mock<IRegionRepository>(MockBehavior.Strict);
        regionService = new RegionService(regionRepo.Object);
        regionRepo.Setup(r => r.Get(It.IsAny<int>())).Returns((int i) => TestData.RegionSample.FirstOrDefault(e => e.RegionNumber == i));
        regionRepo.Setup(r => r.Delete(It.IsAny<int>())).Verifiable();
    }


    [Test]
    public void NotFound()
    {
        Assert.Throws<KeyNotFoundException>(() => regionService.DeleteRegion(-1));
        regionRepo.Verify(r => r.Delete(-1), Times.Never());
    }

    [Test]
    public void DeleteRan()
    {
        regionService.DeleteRegion(1);
        regionRepo.Verify(r => r.Delete(1), Times.Once());
    }
}