using BrokeProtocol.API;
using BrokeProtocol.Entities;
using BrokeProtocol.Managers;
using BrokeProtocol.Utility;

namespace WhiteTax
{
    public class tax : PlayerEvents
    {


        [Execution(ExecutionMode.Additive)]

        public override bool AddItem(ShEntity entity, int itemIndex, int amount, bool dispatch)
        {
            var transferItem = SceneManager.Instance.GetEntity<ShItem>(itemIndex);
            bool IsMerchant = entity.name.Contains("Merchant");
            if (IsMerchant)
            {
                if (transferItem.name == "Money")
                {
                    var tax = amount / 10 * WhiteTax.Base.JsonParser.Config.Tax;
                    WhiteTax.Base.JsonParser.Config.Balance += tax;
                }
            }
            return true;
        }

        public override bool TransferItem(ShEntity entity, byte deltaType, int itemIndex, int amount, bool dispatch)
        {
            var transferItem = SceneManager.Instance.GetEntity<ShItem>(itemIndex);
            if (deltaType == 12 && entity.isHuman)
            {
                if (transferItem.name == "Money")
                {
                    var tax = amount / 10 * WhiteTax.Base.JsonParser.Config.Tax;
                    entity.TransferMoney(DeltaInv.RemoveFromMe, amount / 10 * WhiteTax.Base.JsonParser.Config.Tax);
                    entity.svEntity.SendMessage($"Сумма в размере {amount / 10 * WhiteTax.Base.JsonParser.Config.Tax} вычтенна в качестве налога");
                    WhiteTax.Base.JsonParser.Config.Balance += tax;
                }

            }
            else
            {
                bool IsMerchant = entity.name.Contains("Merchant");
                if (deltaType == 2 && IsMerchant)
                {
                    if (transferItem.name == "Money")
                    {
                        WhiteTax.Base.JsonParser.Config.Balance += amount;
                    }
                }
            }

            return true;
        }
    }
}