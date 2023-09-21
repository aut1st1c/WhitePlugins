using BrokeProtocol.API;
using BrokeProtocol.Entities;
using BrokeProtocol.GameSource;
using BrokeProtocol.Required;
using BrokeProtocol.Utility;


public class Pizdec : PlayerEvents
{
    [Execution(ExecutionMode.Override)]
    public override bool Deposit(ShPlayer player, int entityID, int amount)
    {
        if (!player.svPlayer.CanUseApp(entityID, AppIndex.Deposit) ||
            player.LifePlayer().wantedLevel > 0 || amount <= 0 || player.MyMoneyCount < amount)
        {
            player.svPlayer.SendGameMessage("Fraudulent activity detected");
        }
        else
        {
#pragma warning disable CS0252 // Возможно, использовано непреднамеренное сравнение ссылок: для левой стороны требуется приведение
            if (player.svPlayer.CustomData["IsBankBlocked"] == "Yes")
            {
                player.svPlayer.SendGameMessage("Bank account freezed");
            }
            else
            {
                player.TransferMoney(DeltaInv.RemoveFromMe, amount, true);
                player.svPlayer.bankBalance += amount;
                player.svPlayer.AppendTransaction(amount);
                player.svPlayer.SvAppDeposit(entityID);
            }
#pragma warning restore CS0252 // Возможно, использовано непреднамеренное сравнение ссылок: для левой стороны требуется приведение
        }
        return true;
    }

    [Execution(ExecutionMode.Override)]
    public override bool Withdraw(ShPlayer player, int entityID, int amount)
    {
        if (!player.svPlayer.CanUseApp(entityID, AppIndex.Withdraw) ||
            player.LifePlayer().wantedLevel > 0 || amount <= 0 || player.svPlayer.bankBalance < amount)
        {
            player.svPlayer.SendGameMessage("Fraudulent activity detected");
        }
        else
        {
#pragma warning disable CS0252 // Возможно, использовано непреднамеренное сравнение ссылок: для левой стороны требуется приведение
            if (player.svPlayer.CustomData["IsBankBlocked"] == "Yes")
            {
                player.svPlayer.SendGameMessage("Bank account freezed");
            }
            else
            {
                player.TransferMoney(DeltaInv.AddToMe, amount, true);
                player.svPlayer.bankBalance -= amount;
                player.svPlayer.AppendTransaction(-amount);
                player.svPlayer.SvAppWithdraw(entityID);
            }
#pragma warning restore CS0252 // Возможно, использовано непреднамеренное сравнение ссылок: для левой стороны требуется приведение
        }
        return true;
    }
}