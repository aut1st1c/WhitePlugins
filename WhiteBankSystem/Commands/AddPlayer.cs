using BrokeProtocol.Entities;
using System;

namespace WhiteBankSystem.Commands
{
    public class AddPlayer
    {
        public static void FreezeBank(ShPlayer player, ShPlayer target)
        {
#pragma warning disable CS0252 // Возможно, использовано непреднамеренное сравнение ссылок: для левой стороны требуется приведение
            if (target.svPlayer.CustomData["IsBankBlocked"] == "Yes")
            {
                Console.WriteLine($"[WHITE] WhiteBankSystem: {target.username} bank unfreezed");
                player.svPlayer.SendGameMessage($"Банковский аккаунт {target.displayName} разморожен");
                target.svPlayer.CustomData.AddOrUpdate("IsBankBlocked", "No");
            }
            else
            {
                Console.WriteLine($"[WHITE] WhiteBankSystem: {target.username} bank freezed");
                player.svPlayer.SendGameMessage($"Банковский аккаунт {target.displayName} заморожен");
                target.svPlayer.CustomData.AddOrUpdate("IsBankBlocked", "Yes");
            }
#pragma warning restore CS0252 // Возможно, использовано непреднамеренное сравнение ссылок: для левой стороны требуется приведение
        }
    }
}
