using BrokeProtocol.API;
using BrokeProtocol.Entities;
using System;

namespace WhiteTax
{
    public class CommandHandle : IScript
    {
        public CommandHandle()
        {

            CommandHandler.RegisterCommand("setbudget", new Action<ShPlayer, int>(WhiteTax.Commands.SetBudget.Budget));
            CommandHandler.RegisterCommand("taxmenu", new Action<ShPlayer>(WhiteTax.Base.TaxMenu.taxMenu));
        }
    }
}
