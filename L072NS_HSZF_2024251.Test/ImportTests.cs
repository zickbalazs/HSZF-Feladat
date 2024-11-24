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

namespace L072NS_HSZF_2024251.Test.FileServiceTests;

[TestFixture]
public class ImportTests
{
    private Mock<IRouteRepository> routeRepo;
    private Mock<IRegionRepository> regionRepo;
    private IFileService fileService;



    [SetUp]
    public void Init()
    {
        routeRepo = new Mock<IRouteRepository>(MockBehavior.Strict);
        regionRepo = new Mock<IRegionRepository>(MockBehavior.Strict);
        fileService = new FileService(regionRepo.Object, routeRepo.Object);

        regionRepo.Setup(e => e.Batch()).Returns(TestData.RegionSample);

        routeRepo.Setup(e => e.Add(It.IsAny<Route>())).Verifiable();
        regionRepo.Setup(e => e.Add(It.IsAny<Region>())).Returns((Region region) => new Region()
        {
            RegionNumber = region.RegionNumber,
            RegionName = region.RegionName,
            Routes = new HashSet<Route>()
        });
    }
    [Test]
    public void SuccessfulWithDuplicates()
    {
        fileService.ImportJSONToDatabase("./TestFiles/correctFormatWithDuplicates.json");

        regionRepo.Verify(r => r.Batch());
        routeRepo.Verify(r => r.Add(It.IsAny<Route>()));
    }
    [Test]
    public void SuccessfulWithoutDuplicates()
    {
        fileService.ImportJSONToDatabase("./TestFiles/correctFormatWithoutDuplicates.json");

        regionRepo.Verify(r => r.Batch());
        routeRepo.Verify(r => r.Add(It.IsAny<Route>()));
    }

    [Test]
    public void BadPath()
    {
        Assert.Throws<FileNotFoundException>(() => fileService.ImportJSONToDatabase("./file.json"));
    }
    [Test]
    public void BadFormat()
    {
        FormatException exception = Assert.Throws<FormatException>(()=>fileService.ImportJSONToDatabase("./TestFiles/badFormat.json"));
        Assert.That(exception.Message, Is.EqualTo("The BusRegions field is not correctly formatted!"));
    }
    [Test]
    public void NoRootField()
    {
        FormatException exception = Assert.Throws<FormatException>(() => fileService.ImportJSONToDatabase("./TestFiles/noBusRegions.json"));
        Assert.That(exception.Message, Is.EqualTo("The file doesn't contain the BusRegions Root element"));
    }
    [Test]
    public void BadRootField()
    {
        FormatException exception = Assert.Throws<FormatException>(() => fileService.ImportJSONToDatabase("./TestFiles/badRoot.json"));
        Assert.That(exception.Message, Is.EqualTo("Couldn't find the root BusRegions element!"));
    }
}
