using L072NS_HSZF_2024251.Application.Interfaces;
using Spectre.Console;
using Spectre.Console.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace L072NS_HSZF_2024251.Console.UI
{
    public class StatisticsUI : IStatisticsUI
    {
        public StatisticsUI(IStatisticsService service, IFileService fileService) 
        {
            statisticsService = service;
            this.fileService = fileService;
        }

        private IStatisticsService statisticsService;
        private IFileService fileService;

        public event EventHandler? OnExiting;

        public void Run()
        {
            AnsiConsole.WriteLine("Regions by low delays\n");
            LowDelaysByRegion();
            AvgDelaysByRegion();
            MostDelayedByRegion();
            AnsiConsole.Write(new Markup("Write statistics to file? [green](y/N)[/]\n"));
            if (System.Console.ReadKey(true).Key == ConsoleKey.Y)
                SaveToFiles(AnsiConsole.Prompt(new TextPrompt<string>("Give the path to save:").DefaultValue("./")));
            OnExiting?.Invoke(this, new());
        }

        private void LowDelaysByRegion()
        {
            int length = typeof(Color).GetProperties().Length;
            PropertyInfo[] properties = typeof(Color).GetProperties();
            Random r = new Random();

            IEnumerable<IBarChartItem> items = statisticsService.GetAmountOfLowDelaysByRegion()
                                                            .Where(x=>x.Count > 0)
                                                            .Select(e => new BarChartItem(e.RegionName,
                                                                                          e.Count,
                                                                                          (Color?)properties[r.Next(0, length)].GetValue(new()) ?? Color.Red));
            BarChart chart = new BarChart().AddItems(items).Width(80);
            if (items.Count()>0)
                AnsiConsole.Write(chart);
        }

        private void AvgDelaysByRegion()
        {
            Table table = new Table();
            table.Title("Statistics about delays by region");
            table.AddColumns(["Region name", "Average delay (min)", "Least delayed route", "Most delayed route"]);
            foreach (var k in statisticsService.GetAvgDelayByRegions())
            {
                table.AddRow([
                    new Markup($"{k.RegionName}"),
                    new Markup($"{k.AvgDelay}"),
                    new Markup(k.MostDelayed.Id != -1 ? $"[blue]({k.LeastDelayed.Id})[/] ({k.LeastDelayed.Amount} minutes)" : "[red]No routes :-([/]"),
                    new Markup(k.MostDelayed.Id != -1 ? $"[blue]({k.MostDelayed.Id})[/] ({k.MostDelayed.Amount} minutes)" : "[red]No routes :-([/]")
                    ]);
            }
            AnsiConsole.Write(table);
        }

        private void MostDelayedByRegion()
        {
            Table table = new Table();

            table.AddColumns(["Region", "Terminus", "Delay Total (min)"]);
            table.Title("Most delayed terminus station by region");
            foreach (var k in statisticsService.GetMostDelayedStationByRegions())
            {
                table.AddRow([
                        new Markup($"{k.RegionName}"),
                        new Markup(k.MostDelayedTerminusName == "" ? "[red]No routes :-([/]" : k.MostDelayedTerminusName),
                        k.MostDelayedTerminusName == "" ?  new Markup("") : new Markup($"[yellow]{k.Delay}[/] min")
                    ]);
            }
            AnsiConsole.Write(table);
        }

        public void SaveToFiles(string folderPath)
        {
            if (!Directory.Exists(folderPath))
            {
                bool isSure = AnsiConsole.Prompt(new TextPrompt<bool>("[red]The folder is not found![/]\nTry again?")
                                                                            .AddChoices([true, false])
                                                                            .DefaultValue(false)
                                                                            .AllowEmpty()
                                                                            .WithConverter(x => x ? "y" : "n"));

                if (isSure)
                {
                    SaveToFiles(AnsiConsole.Prompt(new TextPrompt<string>("Give the path to save:").DefaultValue("./")));
                }
                else
                {
                    OnExiting?.Invoke(this, new());
                }
            }
            else
            {
                long date = DateTime.UtcNow.ToFileTimeUtc();

                //Saving Most Delayed
                var mostStats = statisticsService.GetMostDelayedStationByRegions();
                IEnumerable<string> mostDelayedLines = [
                    "Region Name,MostDelayedTerminus,DelaySum",
                    .. mostStats.Select(x => $"{x.RegionName},{x.MostDelayedTerminusName},{x.Delay}")
                    ];
                File.WriteAllLines($"{folderPath}{(folderPath[folderPath.Length - 1] == '/' || folderPath[folderPath.Length - 1] == '\\' ? "\\" : "/")}MostDelayed{date}.csv", mostDelayedLines);
                //Saving Low Counts of delays
                var lowStats = statisticsService.GetAmountOfLowDelaysByRegion();
                IEnumerable<string> lowDelaysLines = [
                    "Region Name,Count",
                    .. lowStats.Select(x=>$"{x.RegionName},{x.Count}")
                    ];
                File.WriteAllLines($"{folderPath}{(folderPath[folderPath.Length - 1] == '/' || folderPath[folderPath.Length - 1] == '\\' ? "\\" : "/")}LeastDelayed{date}.csv", lowDelaysLines);
                //Saving Avg delays
                var avgStats = statisticsService.GetAvgDelayByRegions();
                IEnumerable<string> avgDelayedLines = [
                        "RegionName,Average Delay,MostDelayedId,MostDelayedSum,LeastDelayedId,LeastDelayedSum",
                        .. avgStats.Select(x=>$"{x.RegionName},{x.AvgDelay},{x.MostDelayed.Id},{x.MostDelayed.Amount},{x.LeastDelayed.Id},{x.LeastDelayed.Amount}")
                    ];
                File.WriteAllLines($"{folderPath}{(folderPath[folderPath.Length - 1] == '/' || folderPath[folderPath.Length - 1] == '\\' ? "" : "/")}AvgDelays{date}.csv", avgDelayedLines);

                AnsiConsole.Write(new Markup("[green]Successfully written statistics![/]\nPress any key to continue..."));
                System.Console.ReadKey();
                OnExiting?.Invoke(this, new());
            }
        }
    }
}
