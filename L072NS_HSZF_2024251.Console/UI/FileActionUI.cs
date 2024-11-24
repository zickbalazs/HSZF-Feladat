using L072NS_HSZF_2024251.Application.Interfaces;
using L072NS_HSZF_2024251.Console.UI.Interfaces;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L072NS_HSZF_2024251.Console.UI
{
    public class FileActionUI : IFileActionUI
    {
        private IFileService fileService;

        public FileActionUI(IFileService fileService)
        {
            this.fileService = fileService;
        }

        public event EventHandler? OnExiting;

        public void ReadRun()
        {
            AnsiConsole.Clear();

            TextPrompt<string> filePath = new TextPrompt<string>("Enter the path to the file:").AllowEmpty().DefaultValue("./exampleImport.json");
            bool triesAgain;

            try
            {
                fileService.ImportJSONToDatabase(AnsiConsole.Prompt(filePath));
                AnsiConsole.Write(new Markup("[green]File Successfully read![/]\n"));
            }
            catch (FileNotFoundException)
            {
                AnsiConsole.Write(new Markup("[red]The file is not found[/]\n"));
            }
            catch (Exception exception)
            {
                AnsiConsole.Write(new Markup($"[red]{exception.Message}[/]\n)"));
            }
            finally
            {
                triesAgain = TriesAgain();
            }
            if (triesAgain)
                ReadRun();
            else
                OnExiting?.Invoke(this, new());
        }

        public void WriteRun()
        {
            AnsiConsole.Clear();
            TextPrompt<string> folderPath = new TextPrompt<string>("Input which folder to save the file: ").AllowEmpty().DefaultValue("./");
            bool triesAgain;
            try
            {
                string folder = AnsiConsole.Prompt(folderPath);
                fileService.ExportDatabaseToJSON(folder, out string exportName);
                AnsiConsole.Write(new Markup($"[green]Successful export![/]\n[yellow]The file is saved as [blue]{exportName}[/] at [blue]{folder}[/][/]\n"));

            }
            catch 
            {
                AnsiConsole.Write(new Markup("[red]The folder doesn't exist![/]\n"));
            }
            finally
            {
                triesAgain = TriesAgain();
            }
            if (triesAgain)
                WriteRun();
            else
                OnExiting?.Invoke(this, new());
        }

        private bool TriesAgain()
        {
            return AnsiConsole.Prompt(new TextPrompt<bool>("Try Again?").AddChoices([true, false]).DefaultValue(false).WithConverter(x => x ? "y" : "n"));
        }
    }
}
