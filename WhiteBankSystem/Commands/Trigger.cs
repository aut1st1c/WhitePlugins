using BrokeProtocol.API;
using BrokeProtocol.Entities;
using BrokeProtocol.GameSource;
using BrokeProtocol.Managers;
using BrokeProtocol.Utility;
using System.Threading.Tasks;

namespace WhiteBankSystem.Base
{
    public class RobberyTrigger : IScript
    {
        bool robbery;
        [CustomTarget]
        public async void RobberyStart(Serialized trigger, ShPhysical physical)
        {
            if (physical is ShPlayer player)
            {
                var otherPlayer = player.otherEntity as ShPlayer;
#pragma warning disable CS0252 // Возможно, использовано непреднамеренное сравнение ссылок: для левой стороны требуется приведение
                if (player.svPlayer.CustomData["Bank"] != "Yes" || ((MyJobInfo)player.svPlayer.job.info).groupIndex != GroupIndex.LawEnforcement)
                {
                    robbery = true;
                    while (robbery == true)
                    {
                        if (WhiteTax.Base.JsonParser.Config.Balance < 2500) { player.svPlayer.SendGameMessage("В банке нет денег"); }
                        else
                        {
                            await Task.Delay(15000);
                            player.LifePlayer().AddCrime(CrimeIndex.Robbery, otherPlayer);
                            WhiteTax.Base.JsonParser.Config.Balance -= 1000;
                            player.TransferItem(DeltaInv.AddToMe, SceneManager.Instance.GetEntity("Dirty money").index, 1000);
                        }

                    }

                }
#pragma warning restore CS0252 // Возможно, использовано непреднамеренное сравнение ссылок: для левой стороны требуется приведение
            }
        }
        [CustomTarget]
        public void RobberyEnd(Serialized trigger, ShPhysical physical)
        {
            if (physical is ShPlayer player)
            {
                robbery = false;
            }
        }
    }
}
