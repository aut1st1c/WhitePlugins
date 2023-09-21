using BrokeProtocol.API;
using BrokeProtocol.Entities;
using System;

namespace WhiteBankSystem
{
    public class CommandHandle : IScript
    {
        public CommandHandle()
        {
            CommandHandler.RegisterCommand("FreezeBank", new Action<ShPlayer, ShPlayer>(WhiteBankSystem.Commands.AddPlayer.FreezeBank));
            CommandHandler.RegisterCommand("DeleteBank", new Action<ShPlayer, ShPlayer>(WhiteBankSystem.Commands.DeleteBank.deletebank));
        }
    }
}
