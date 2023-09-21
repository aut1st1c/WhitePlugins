using BrokeProtocol.API;
using BrokeProtocol.Entities;
using System;

namespace WhiteChat
{
    public class CommandHandle : IScript
    {
        public CommandHandle()
        {
            CommandHandler.RegisterCommand("advert", new Action<ShPlayer, string>(WhiteChat.Commands.Advertise.Advert));
            CommandHandler.RegisterCommand("broadcast", new Action<ShPlayer, string>(WhiteChat.Commands.Broadcasting.Broadcast));
            CommandHandler.RegisterCommand("ooc", new Action<ShPlayer, string>(WhiteChat.Commands.ooc.OOC));
            CommandHandler.RegisterCommand("/", new Action<ShPlayer, string>(WhiteChat.Commands.ooc.OOC));
            CommandHandler.RegisterCommand("w", new Action<ShPlayer, string>(WhiteChat.Commands.Whispers.Whishper));
            CommandHandler.RegisterCommand("y", new Action<ShPlayer, string>(WhiteChat.Commands.Yells.Yell));
        }
    }
}
