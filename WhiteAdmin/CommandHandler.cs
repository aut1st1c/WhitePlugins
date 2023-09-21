using BrokeProtocol.API;
using BrokeProtocol.Entities;
using System;

namespace WhiteAdmin
{
    public class CommandHandle : IScript
    {
        public CommandHandle()
        {
            CommandHandler.RegisterCommand("TempBan", new Action<ShPlayer, ShPlayer, int, string>(WhiteAdmin.AddPlayer.TempBan));
        }
    }
}