using BrokeProtocol.Entities;
using System;

namespace WhiteLibrary.Commands
{
    public class CheckPlayer
    {
        public static void Check (ShPlayer player,ShPlayer target)
        {
#pragma warning disable CS0252 // Возможно, использовано непреднамеренное сравнение ссылок: для левой стороны требуется приведение
            if (target.svPlayer.CustomData["PodSpidami???"] == "Yes")
            {
                player.svPlayer.SendGameMessage($"Этот ебан под спидами");
            }
            else
            {
                player.svPlayer.SendGameMessage($"Этот ебан не под спидами");
            }
#pragma warning restore CS0252 // Возможно, использовано непреднамеренное сравнение ссылок: для левой стороны требуется приведение
        }
    }
}
