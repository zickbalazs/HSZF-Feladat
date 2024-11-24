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

namespace L072NS_HSZF_2024251.Test.FileServiceTests;

[TestFixture]
public class ExportTests
{
    private Mock<IRegionRepository> regionMock;
    private Mock<IRouteRepository> routeMock;
    private IFileService fileService;

    [SetUp]
    public void Setup()
    {
        regionMock = new Mock<IRegionRepository>(MockBehavior.Strict);
        routeMock = new Mock<IRouteRepository>(MockBehavior.Strict);
        fileService = new FileService(regionMock.Object, routeMock.Object);

        regionMock.Setup(r => r.Batch()).Returns(new HashSet<Region>
        {
            new()
            {
                RegionName = "Bács-Kiskun",
                RegionNumber = 1,
                Routes = new HashSet<Route>
                {
                    new()
                    {
                        BusNumber = 1,
                        BusType = "Local",
                        DelayAmount = 120,
                        From = "Baja",
                        To = "Kalocsa",
                        Id = new(),
                        RegionId = 1
                    }
                }
            }
        });
    }

    [Test]
    public void TestBadRoute()
    {
        Assert.Throws<DirectoryNotFoundException>(() => fileService.ExportDatabaseToJSON("./xddddd", out string f));
    }
    [Test]
    public void TestCorrectRoute()
    {
        fileService.ExportDatabaseToJSON("./", out string filename);
        Assert.That(() => File.Exists($"./{filename}"));
        File.Delete("./"+filename);
    }

}
