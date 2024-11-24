using L072NS_HSZF_2024251.Application;
using L072NS_HSZF_2024251.Application.Interfaces;
using L072NS_HSZF_2024251.Application.Services;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L072NS_HSZF_2024251.Console.UI
{
    public class RegionUI : IRegionUI
    {
        private IRegionService regionService;
        private IRouteActions routeActions;
        
        public event EventHandler? OnExiting;

        private SelectionPrompt<string> menuPrompt = new SelectionPrompt<string>().Title("NLB Busmanager Program - Viewing Regions");

        public RegionUI(IRegionService regionService, IRouteActions routeActions)
        {
            this.regionService = regionService;
            this.routeActions = routeActions;
            this.routeActions.OnExiting += (obj, args) =>
            {
                Run();
            };
        }

        private void Search()
        {
            RegionDto dto = new RegionDto()
            {
                RegionName = AnsiConsole.Prompt(new TextPrompt<string>("Region name: [[[green]optional[/]]]").AllowEmpty().DefaultValue(null)),
                RegionNumber = AnsiConsole.Prompt(new TextPrompt<int?>("Region number: [[[green]optional[/]]]").AllowEmpty().DefaultValue(null))
            };

            AnsiConsole.Clear();

            menuPrompt = new SelectionPrompt<string>().Title("NLB Busmanager Program - Search Results");
            menuPrompt.AddChoices([
                    .. regionService.SearchRegions(dto).Select(e => $"{e.RegionNumber}|{e.RegionName}: {e.Routes.Count} routes"),
                    "Back"
                ]);

            string selection = AnsiConsole.Prompt(menuPrompt);

            if (selection == "Back")
                Run();
            else
                HandleSelection(selection);
        }

        private void Add()
        {
            Model.Region region = new()
            {
                RegionName = AnsiConsole.Prompt(new TextPrompt<string>("Insert the regions name: "))
            };
            regionService.UploadRegion(region);
            Run();
        }

        public void Run()
        {
            menuPrompt = new SelectionPrompt<string>().Title("NLB Busmanager Program - Viewing Regions");
            menuPrompt.AddChoices([
                    "Add Region",
                    "Search Regions",
                    .. regionService.GetAllRegions().Select(e => $"{e.RegionNumber}|{e.RegionName}: {e.Routes.Count} routes"),
                    "Back"
            ]);
            AnsiConsole.Clear();
            string selection = AnsiConsole.Prompt(menuPrompt);
            HandleSelection(selection);
        }
        private void HandleSelection(string selection)
        {
            switch (selection)
            {
                case "Add Region":
                    Add();
                    break;
                case "Search Regions":
                    Search();
                    break;
                case "Back":
                    OnExiting?.Invoke(this, new());
                    break;
                default:
                    routeActions.Run(regionService.GetRegionById(int.Parse(selection.Split('|')[0])));
                    break;
            }
        }
    }
}
