using BrokeProtocol.API;
using BrokeProtocol.Entities;

namespace WhiteTax.Commands
{
    public class SetBudget : IScript
    {
        public static void Budget(ShPlayer player, int balance)
        {
            WhiteTax.Base.JsonParser.Config.Balance = (balance);
            player.svPlayer.SendGameMessage("Баланс успешно установлен");
            player.svPlayer.SendGameMessage($"Новый баланс - {WhiteTax.Base.JsonParser.Config.Balance}");
        }
    }
}