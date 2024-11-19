using L072NS_HSZF_2024251.Application.Interfaces;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L072NS_HSZF_2024251.Console.UI
{
    internal class FileRead : IFileRead
    {

        private IFileService fileService;
        public event EventHandler OnExiting;
       
        public FileRead(IFileService fileService)
        {
            this.fileService = fileService;
        }

        public void Run()
        {
            TextPrompt<string> filePrompt = new TextPrompt<string>("Please enter a path to the file:")
                .ShowDefaultValue(false)
                .ShowChoices(false)
                .DefaultValue(@".\exampleImport.json");

            TextPrompt<bool> ask = new TextPrompt<bool>("Would you like to try again?")
                    .AddChoices(new[] { true, false })
                    .ShowDefaultValue(false)
                    .DefaultValue(false)
                    .WithConverter(e => e ? "y" : "n");


            string path = AnsiConsole.Prompt(filePrompt);

            try
            {
                fileService.ImportJSONToDatabase(path);
                AnsiConsole.Write(new Markup("[green]File successfully read![/]\nPress anything to continue..."));
                System.Console.ReadKey();
            }
            catch (FileNotFoundException)
            {
                AnsiConsole.Clear();
                AnsiConsole.Write(new Markup("[red]The file has not been found[/]\n"));
                if (AnsiConsole.Prompt(ask))
                {
                    Run();
                }
            }
            catch (FormatException)
            {
                AnsiConsole.Clear();
                AnsiConsole.Write(new Markup("[red]The file is not formatted correctly[/]\n"));
                if (AnsiConsole.Prompt(ask))
                {
                    Run();
                }
            }

            OnExiting?.Invoke(this, new EventArgs());
        }


    }
}
