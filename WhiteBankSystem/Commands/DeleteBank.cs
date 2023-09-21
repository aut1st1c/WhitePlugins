using BrokeProtocol.Entities;
using System;

namespace WhiteBankSystem.Commands
{
    public class DeleteBank
    {
        public static void deletebank(ShPlayer player, ShPlayer target)
        {
            Console.WriteLine($"[WHITE] WhiteBankSystem: {target.username} deleted bank with {target.svPlayer.bankBalance} on deposit");
            player.svPlayer.SendGameMessage("Банковский аккаунт обнулен");
            WhiteTax.Base.JsonParser.Config.Balance += target.svPlayer.bankBalance;
            target.svPlayer.bankBalance = 0;
        }
    }
}
