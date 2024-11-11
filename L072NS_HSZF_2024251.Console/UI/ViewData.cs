using L072NS_HSZF_2024251.Application.Interfaces;
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
            SelectionPrompt<string> itemsList = new SelectionPrompt<string>()
                .Title("NLB Busmanagement App - Viewing data")
                .AddChoices(regionService.GetAllRegions().Select(e=>$"{e.RegionNumber}. {e.RegionName} | {e.Routes.Count} route(s)"));

            string selected = AnsiConsole.Prompt(itemsList);
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
            
            AnsiConsole.Prompt(selection);
        }

        private string DelaySeverity(int delay)
        {
            if (delay < 5)
                return $"[green]{delay}[/]";
            if (delay >= 5 && delay <= 15)
                return $"[orangered1]{delay}[/]";
            return $"[red]{delay}[/]";
        }


        private void ViewRoute()
        {

        }
    }
}
