using BrokeProtocol.API;
using BrokeProtocol.Entities;
using BrokeProtocol.Managers;
using BrokeProtocol.Utility;

namespace WhiteTax.Base
{
    public class InputMenu : PlayerEvents
    {
        public static void BudgetUse(ShPlayer player)
        {
            player.svPlayer.SendInputMenu("", player.ID, "BudgetUse", UnityEngine.UI.InputField.ContentType.IntegerNumber);
        }
        public static void TaxUse(ShPlayer player)
        {
            player.svPlayer.SendInputMenu("Input Menu Test", player.ID, "TaxUse", UnityEngine.UI.InputField.ContentType.IntegerNumber);
        }


        [Execution(ExecutionMode.Event)]
        public override bool SubmitInput(ShPlayer player, int targetID, string id, string input)
        {
            bool isNumber = int.TryParse(input, out int amount);
            if (isNumber == true)
            {
                if (id == "BudgetUse")
                {
                    if (amount > WhiteTax.Base.JsonParser.Config.Balance | amount < 1)
                    {
                        player.svPlayer.SendGameMessage($"Введите число в диапазоне от 1 до {WhiteTax.Base.JsonParser.Config.Balance}");
                    }
                    else
                    {
                        player.TransferItem(DeltaInv.AddToMe, SceneManager.Instance.GetEntity("Money").index, amount);
                        player.svPlayer.SendGameMessage($"Успешно выведено - {amount}");
                        WhiteTax.Base.JsonParser.Config.Balance -= amount;
                    }
                }
                else if (id == "TaxUse")
                {
                    if (amount > 9 | amount < 1)
                    {
                        player.svPlayer.SendGameMessage($"Введите число в диапазоне от 1 до 9");
                    }
                    else
                    {
                        WhiteTax.Base.JsonParser.Config.Tax = (amount);
                        player.svPlayer.SendGameMessage("Налог успешно установлен");
                        player.svPlayer.SendGameMessage($"Новый налог - {WhiteTax.Base.JsonParser.Config.Tax}");
                    }
                }
            }
            else
            {
                player.svPlayer.SendGameMessage("Ебалай, ты как это сделал блять?");
                player.svPlayer.SendGameMessage("Тут только циферки писать можно");
            }
            return true;
        }
    }
}
