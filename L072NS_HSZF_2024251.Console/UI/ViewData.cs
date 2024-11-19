using L072NS_HSZF_2024251.Application.Interfaces;
using L072NS_HSZF_2024251.Model;
using Spectre.Console;

namespace L072NS_HSZF_2024251.Console.UI
{
    internal class ViewData : IViewData
    {

        private IRegionService regionService;
        public ViewData(IRegionService regionService)
        {
            this.regionService = regionService;
        }
        public event EventHandler OnExiting;
        public void Run()
        {
            AnsiConsole.Clear();
            SelectionPrompt<string> itemsList = new SelectionPrompt<string>()
                .Title("NLB Busmanagement App - Viewing data")
                .AddChoices(
                    [
                        "Add new region",
                        .. regionService.GetAllRegions().Select(e=>$"{e.RegionNumber}. {e.RegionName} | {e.Routes.Count} route(s)"),
                        "Go back"
                    ]);

            string selected = AnsiConsole.Prompt(itemsList);

            switch (selected)
            {
                case "Add new region":
                    break;
                case "Go back":
                    OnExiting?.Invoke(this, new EventArgs());
                    break;
                default:
                    ViewRegion(selected);
                    break;
            }

            if (selected == "Go back")
                OnExiting?.Invoke(this, new EventArgs());
            else
                ViewRegion(selected);
        }

        private void ViewRegion(string selectText)
        {
            List<string> text =
            [
                "Modify Region",
                "Delete Region",
                "Back",
                "---------------",
                .. regionService.GetRegionById(int.Parse(selectText.Split('.')[0]))
                    .Routes.Select(e => $"{e.Id}| {e.BusNumber}: [yellow]{e.From}[/] -> [yellow]{e.To}[/] {DelaySeverity(e.DelayAmount)} min delay"),
            ];
            
            SelectionPrompt<string> selection = new SelectionPrompt<string>()
                .Title($"NLB Busmanagement App - Viewing Region #{selectText.Split('.')[0]}: {selectText.Split('.')[1].Split('|')[0].Trim()}")
                .AddChoices(text);
            
            HandleSelection(AnsiConsole.Prompt(selection), selectText);
        }

        private string DelaySeverity(int delay)
        {
            if (delay < 5)
                return $"[green]{delay}[/]";
            if (delay >= 5 && delay <= 15)
                return $"[orangered1]{delay}[/]";
            return $"[red]{delay}[/]";
        }

        private void HandleSelection(string selection, string originalSelection)
        {
            switch (selection)
            {
                case "Back":
                    Run();
                    break;
                case "Modify Region":
                    break;
                case "Delete Region":
                    HandleDeletionOf(originalSelection.Split('.')[0].Trim());
                    break;
            }
        }

        private void HandleDeletionOf(string id)
        {
            Model.Region region = regionService.GetRegionById(int.Parse(id));
            
            TextPrompt<bool> ask = new TextPrompt<bool>($"Are you sure you want to delete {region.Routes.Count} routes with this region?")
                                            .DefaultValue(false)
                                            .AddChoices(new[] { true, false })
                                            .AllowEmpty()
                                            .WithConverter((e) => e ? "y" : "n");
            
            if (AnsiConsole.Prompt(ask))
            {
                try
                {
                    regionService.DeleteRegion(region.RegionNumber);
                }
                catch
                {
                    AnsiConsole.Clear();
                    AnsiConsole.Write("A database error has occured! Press anything to return to the previous menu.");
                    System.Console.ReadKey();
                }
                Run();
            }
            else
                Run();
        }


        private void ViewRoute()
        {

        }
    }
}
