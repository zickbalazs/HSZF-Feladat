using L072NS_HSZF_2024251.Console.UI.Interfaces;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L072NS_HSZF_2024251.Console.UI;

public class Main : IMainUI
{
    private readonly IRegionUI regionUI;
    private readonly IStatisticsUI statisticsUI;
    private readonly IFileActionUI fileactionUI;
    
    private readonly SelectionPrompt<string> mainPrompt = new SelectionPrompt<string>()
                                                                        .Title("NLB Busmanagement Program - Main Menu")
                                                                        .AddChoices(new[]
                                                                        {
                                                                            "Start",
                                                                            "Statistics",
                                                                            "Import",
                                                                            "Export",
                                                                            "Exit"
                                                                        });


    public Main(IRegionUI regionUI, IStatisticsUI statisticsUI, IFileActionUI fileactionUI)
    {
        this.regionUI = regionUI;
        this.statisticsUI = statisticsUI;
        this.fileactionUI = fileactionUI;
        regionUI.OnExiting += (obj, args) => RunMain();
        statisticsUI.OnExiting += (obj, args) => RunMain();
        fileactionUI.OnExiting += (obj, args) => RunMain();
    }
    
    
    public void RunMain()
    {
        AnsiConsole.Clear();
        switch (AnsiConsole.Prompt(mainPrompt))
        {
            case "Start":
                regionUI.Run();
                break;
            case "Statistics":
                statisticsUI.Run();
                break;
            case "Import":
                fileactionUI.ReadRun();
                break;
            case "Export":
                fileactionUI.WriteRun();
                break;
            default:
                Environment.Exit(0);
                break;
        }
    }



}
