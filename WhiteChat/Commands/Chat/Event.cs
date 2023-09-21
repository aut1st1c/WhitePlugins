using BrokeProtocol.API;
using BrokeProtocol.Entities;
using BrokeProtocol.Utility;
using System.Linq;
using UnityEngine;

public class ChatGlobalOverride : PlayerEvents
{
    private bool ChatBoilerplate(ShPlayer player, string prefix, string message, out string cleanMessage)
    {
        cleanMessage = message.CleanMessage();
        Debug.Log($"{prefix} {player.username}: {cleanMessage}");
        return !string.IsNullOrWhiteSpace(cleanMessage) &&
            !CommandHandler.OnEvent(player, cleanMessage);
    }

    [Execution(ExecutionMode.Override)]
    public override bool ChatGlobal(ShPlayer player, string message)
    {
        string JobColor = ("&8");
        var a = (player.svPlayer.job.info.shared.jobIndex);
        if (a == 0 || a == 4 || a == 5 || a == 9 || a == 10 || a == 11 || a == 13)
        {
            JobColor = ("&7");
        }
        else
        {
            if (a == 1 || a == 14)
            {
                JobColor = ("&6");
            }
            else
            {
                if (a == 2 || a == 6 || a == 7 || a == 8)
                {
                    JobColor = ("&c");
                }
                else
                {
                    if (a == 3 || a == 12)
                    {
                        JobColor = ("&9");
                    }
                }
            }
        }
        if (ChatBoilerplate(player, "[GLOBAL]", message, out var cleanMessage))
        {
            player.svPlayer.SendGameMessage($"[&aГоворит&f] {JobColor}{player.username}&f >> {message}");
            foreach (ShPlayer p in player.svPlayer.GetLocalInRange<ShPlayer>(50f).Where(x => x != player))
            {
                p.svPlayer.SendGameMessage($"[&aГоворит&f] {JobColor}{player.username}&f >> {message}");
            }
        }
        return true;
    }

    [Execution(ExecutionMode.Event)]
    public override bool Ready(ShPlayer player)
    {
        player.svPlayer.SendGameMessage("&2〔WhiteRP〕 » &eЕсли у вас карта отображается неправильно, то выключите <Occlusion> в настройках.");
        return true;
    }
}