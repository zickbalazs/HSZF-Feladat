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
        private IFileRead fileRead;

        public Main(IViewData viewData, IFileRead fileRead)
        {
            this.viewData = viewData;
            this.fileRead = fileRead;
            this.viewData.OnExiting += (e, obj) => this.Run();
            this.fileRead.OnExiting += (e, obj) => this.Run();
        }

        private SelectionPrompt<string> mainMenu = new SelectionPrompt<string>()
            .Title("NLB Busmanagement App")
            .AddChoices(new[]{"Start", "Statistics", "Import", "Export", "Quit"});

        public void Run()
        {
            AnsiConsole.Clear();
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
                case "Statistics":
                    break;
                case "Import":
                    fileRead.Run();
                    break;
                case "Export":
                    break;
                default:
                    Environment.Exit(0);
                    break;
            }
        }
    }
}
