using BrokeProtocol.API;
using BrokeProtocol.Entities;
using BrokeProtocol.GameSource;
using BrokeProtocol.Managers;
using BrokeProtocol.Utility;

namespace WhiteCrimes.Event
{
    public class Transfer : PlayerEvents
    {
        [Execution(ExecutionMode.Additive)]
        public override bool TransferItem(ShEntity entity, byte deltaType, int itemIndex, int amount, bool dispatch)
        {
            var player = entity.Player;
            var transferItem = SceneManager.Instance.GetEntity<ShItem>(itemIndex);

            switch (deltaType)
            {
                case DeltaInv.OtherToMe:
                    var otherPlayer = player.otherEntity as ShPlayer;

                    if (otherPlayer)
                    {
                        if (transferItem.name == "Money")
                        {
                            if (((MyJobInfo)player.svPlayer.job.info).groupIndex == GroupIndex.LawEnforcement)
                            {
                                if (SceneManager.Instance.TryGetEntity<ShItem>(itemIndex, out var item) && item.illegal)
                                {
                                    if (otherPlayer && otherPlayer.Shop)
                                    {
                                        if (player.MyMoneyCount < amount)
                                        {
                                            int MyMoney = player.MyMoneyCount;
                                            entity.TransferMoney(DeltaInv.RemoveFromMe, player.MyMoneyCount);
                                            entity.TransferItem(DeltaInv.AddToMe, SceneManager.Instance.GetEntity("Dirty money").index, MyMoney);
                                        }
                                        else
                                        {
                                            entity.TransferMoney(DeltaInv.RemoveFromMe, amount);
                                            entity.TransferItem(DeltaInv.AddToMe, SceneManager.Instance.GetEntity("Dirty money").index, amount);
                                        }
                                    }
                                    else
                                    {
                                        if (player.MyMoneyCount < amount)
                                        {
                                            int MyMoney = player.MyMoneyCount;
                                            entity.TransferMoney(DeltaInv.RemoveFromMe, player.MyMoneyCount);
                                            entity.TransferItem(DeltaInv.AddToMe, SceneManager.Instance.GetEntity("Dirty money").index, MyMoney);
                                        }
                                        else
                                        {
                                            entity.TransferMoney(DeltaInv.RemoveFromMe, amount);
                                            entity.TransferItem(DeltaInv.AddToMe, SceneManager.Instance.GetEntity("Dirty money").index, amount);
                                        }
                                    }
                                }
                                else
                                {
                                    if (player.MyMoneyCount < amount)
                                    {
                                        int MyMoney = player.MyMoneyCount;
                                        entity.TransferMoney(DeltaInv.RemoveFromMe, player.MyMoneyCount);
                                        entity.TransferItem(DeltaInv.AddToMe, SceneManager.Instance.GetEntity("Dirty money").index, MyMoney);
                                    }
                                    else
                                    {
                                        entity.TransferMoney(DeltaInv.RemoveFromMe, amount);
                                        entity.TransferItem(DeltaInv.AddToMe, SceneManager.Instance.GetEntity("Dirty money").index, amount);
                                    }
                                }
                            }
                            else
                            {
                                if (player.MyMoneyCount < amount)
                                {
                                    int MyMoney = player.MyMoneyCount;
                                    entity.TransferMoney(DeltaInv.RemoveFromMe, player.MyMoneyCount);
                                    entity.TransferItem(DeltaInv.AddToMe, SceneManager.Instance.GetEntity("Dirty money").index, MyMoney);
                                }
                                else
                                {
                                    entity.TransferMoney(DeltaInv.RemoveFromMe, amount);
                                    entity.TransferItem(DeltaInv.AddToMe, SceneManager.Instance.GetEntity("Dirty money").index, amount);
                                }
                            }
                        }
                    }
                    break;
            }
            return true;
        }
    }
}
