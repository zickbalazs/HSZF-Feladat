using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L072NS_HSZF_2024251.Console.UI
{
    internal class Main
    {
        private IViewData viewData;

        public Main(IViewData viewData)
        {
            this.viewData = viewData;
            this.viewData.OnExiting += (e, obj) => this.Run();
        }



        private SelectionPrompt<string> mainMenu = new SelectionPrompt<string>()
            .Title("NLB Busmanagement App")
            .AddChoices(new[]{"Start", "Statistics", "Import", "Export", "Quit"});

        public void Run()
        {
            string selected = AnsiConsole.Prompt(mainMenu);
            HandleSelection(selected);
        }
        private void HandleSelection(string selected)
        {
            switch (selected)
            {
                case "Start":
                    viewData.Run();
                    break;
            }
        }
    }
}
