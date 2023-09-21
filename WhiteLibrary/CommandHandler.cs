using BrokeProtocol.API;
using BrokeProtocol.Entities;
using System;

namespace WhiteLibrary
{
    public class CommandHandle : IScript
    {
        public CommandHandle()
        {
            CommandHandler.RegisterCommand("CheckSpeed", new Action<ShPlayer, ShPlayer>(WhiteLibrary.Commands.CheckPlayer.Check));
            CommandHandler.RegisterCommand("Regen", new Action<ShPlayer>(WhiteLibrary.Effects.Regen.Regeneration));
            CommandHandler.RegisterCommand("Speed", new Action<ShPlayer>(WhiteLibrary.Effects.Speed.speed));
            CommandHandler.RegisterCommand("Health", new Action<ShPlayer>(WhiteLibrary.Effects.Health.health));
            CommandHandler.RegisterCommand("HP", new Action<ShPlayer, ShPlayer>(WhiteLibrary.Commands.HP.Health));
            CommandHandler.RegisterCommand("About", new Action<ShPlayer>(WhiteLibrary.Commands.Version.About));
        }
    }
}
