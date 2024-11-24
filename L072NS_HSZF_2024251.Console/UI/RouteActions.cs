using L072NS_HSZF_2024251.Application.Interfaces;
using L072NS_HSZF_2024251.Model;
using Spectre.Console;
using System.Reflection;

namespace L072NS_HSZF_2024251.Console.UI
{
    public class RouteActions : IRouteActions
    {
        private IRouteService routeService;
        private IRegionService regionService;
        private Model.Region? current;

        private SelectionPrompt<string> prompt = new SelectionPrompt<string>();
        
        public RouteActions(IRouteService routeService, IRegionService regionService)
        {
            this.routeService = routeService;
            this.regionService = regionService;
            routeService.OnLowestDelayAdded += (obj, args) =>
            {
                AnsiConsole.Write(new Markup($"[green]This is the lowest delay so far in {current!.RegionName}![/]\nPress any key to continue..."));
                System.Console.ReadKey(true);
            };
        }

        public event EventHandler? OnExiting;

        public void Run(Model.Region region)
        {
            current = region;
            AnsiConsole.Clear();
            prompt = new();

            IEnumerable<string> routes = current.Routes.Select(e => $"([blue]{e.BusNumber}[/]) [yellow]{e.From}[/] -> [yellow]{e.To}[/] {DelaySeverityColorizer(e.DelayAmount)}");
            prompt.AddChoices([
                    "Add Route",
                    "Rename Region",
                    "Delete Region",
                    "Back"
                ])
                .Title($"NLB Busmanager Program | Viewing {region.RegionName}");

            prompt.AddChoices(
                    routes.Count() > 0 ?
                    ["[green]-----Routes-----[/]" , .. routes ] : [ "[red]No Routes yet! :([/]" ]
                );


            switch (AnsiConsole.Prompt(prompt))
            {
                case "Add Route":
                    AddRoute();
                    break;
                case "Rename Region":
                    ModifyRegion();
                    break;
                case "Delete Region":
                    Delete();
                    break;
                case "Back":
                    OnExiting?.Invoke(this,new());
                    break;
                default:
                    prompt = new SelectionPrompt<string>();
                    Run(current);
                    break;
            }
        }

        private string DelaySeverityColorizer(int amount)
        {
            switch (amount)
            {
                case int n when n <= 5:
                    return $"[green]{amount}[/] min. delay";
                case int n when n > 5 && n <= 15:
                    return $"[orange3]{amount}[/] min. delay";
                default:
                    return $"[red]{amount}[/] min. delay";
            }
        }

        private void Delete()
        {
            AnsiConsole.Clear();
            bool isSure = AnsiConsole.Prompt(new SelectionPrompt<string>()
                                        .Title($"Are you sure you want to delete this region?\n[red]{current!.Routes.Count} routes will be deleted[/]")
                                        .AddChoices(["Yes", "No"])) == "Yes";
            if (isSure)
            {
                regionService.DeleteRegion(current.RegionNumber);
                OnExiting?.Invoke(this, new());
            }
            else
            {
                prompt = new();
                Run(current);
            }
        }

        private void ModifyRegion()
        {
            Model.Region region = new()
            {
                RegionNumber = current!.RegionNumber,
                RegionName = AnsiConsole.Prompt(new TextPrompt<string>("Input the new region name: "))
            };
            regionService.UpdateRegion(current.RegionNumber, region);
            prompt = new();
            Run(current);
        }

        private void AddRoute()
        {
            routeService.UploadRoute(new()
            {
                BusNumber = AnsiConsole.Prompt(new TextPrompt<int>("Line number: ")),
                BusType = AnsiConsole.Prompt(new TextPrompt<string>("Line type: ").DefaultValue("Local").AllowEmpty()),
                DelayAmount = AnsiConsole.Prompt(new TextPrompt<int>("Delay Amount (in minutes): ").DefaultValue(0).AllowEmpty()),
                From = AnsiConsole.Prompt(new TextPrompt<string>("Start station: ")),
                To = AnsiConsole.Prompt(new TextPrompt<string>("Terminus station: ")),
                RegionId = current!.RegionNumber
            });
            AnsiConsole.Clear();
            AnsiConsole.Write(new Markup("[green]Successful upload![/]\nPress any key to return to the previous menu..."));
            System.Console.ReadKey();
            prompt = new();
            Run(current!);
        }


    }
}
