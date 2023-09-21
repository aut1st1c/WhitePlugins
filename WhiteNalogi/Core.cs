using BrokeProtocol.API;
using System;

public class Core : Plugin
{
    public Core()
    {
        Info = new PluginInfo("WhiteTax", "wt", "Plugin by .aut1st1c");
        Console.WriteLine("[WHITE] WhiteTax Succefully loaded!");
        WhiteTax.Base.JsonParser.LoadJson();
        Console.WriteLine("[WHITE] WhiteTax Config succefully loaded!");
    }
}