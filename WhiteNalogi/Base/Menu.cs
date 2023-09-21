using BrokeProtocol.API;
using BrokeProtocol.Entities;
using BrokeProtocol.Required;
using System.Collections.Generic;

namespace WhiteTax.Base
{
    public class TaxMenu : PlayerEvents
    {
        public static void taxMenu(ShPlayer player)
        {
            List<LabelID> options = new List<LabelID>
            {
                new LabelID("Бюджет", "budget"),
                new LabelID("Налог", "tax"),
            };
            List<LabelID> actions = new List<LabelID>
            {
                new LabelID("Взаимодействие", "use"),
                new LabelID("Информация", "get"),
            };
            player.svPlayer.SendOptionMenu("Меню бюджета", player.ID, "taxmenu", options.ToArray(), actions.ToArray());
        }

        [Execution(ExecutionMode.Event)]
        public override bool OptionAction(ShPlayer player, int targetID, string id, string optionID, string actionID)
        {
            if (id == "taxmenu")
            {
                switch (optionID)
                {
                    case "budget":
                        if (actionID == "use")
                        {
                            WhiteTax.Base.InputMenu.BudgetUse(player);
                        }
                        else if (actionID == "get")
                        {
                            player.svPlayer.SendGameMessage("Бюджет = " + WhiteTax.Base.JsonParser.Config.Balance);
                        }
                        break;
                    case "tax":
                        if (actionID == "use")
                        {
                            WhiteTax.Base.InputMenu.TaxUse(player);
                        }
                        else if (actionID == "get")
                        {
                            player.svPlayer.SendGameMessage("Налог = " + WhiteTax.Base.JsonParser.Config.Tax);
                        }
                        break;
                }
            }
            return true;
        }
    }
}
