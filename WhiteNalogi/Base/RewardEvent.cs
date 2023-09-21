using BrokeProtocol.API;
using BrokeProtocol.Entities;
using BrokeProtocol.Utility;
using UnityEngine;

namespace WhiteTax.Base
{
    public class Rewards : PlayerEvents
    {
        [Execution(ExecutionMode.Override)]
        public override bool Reward(ShPlayer player, int experienceDelta, int moneyDelta)
        {
            if (!player.isHuman) return true;
            string jobname = player.svPlayer.job.info.shared.jobName;
            string jobclass = player.svPlayer.job.info.jobClassName;
            // Player rank affects money rewards (can adjust for modding)
            moneyDelta *= player.rank + 1;

            if (moneyDelta > 0)
            {
                if (jobname == "Медик" || jobname == "Доставщик" || jobname == "Ищейка" || jobname == "Таксист" || jobname == "Наемник" || jobname == "Пожарный")
                {
                    player.TransferMoney(DeltaInv.AddToMe, moneyDelta);
                }
            }
            else if (moneyDelta < 0)
            {
                if (jobclass != "Prisoner" || jobclass != "DrugDealer")
                    player.TransferMoney(DeltaInv.RemoveFromMe, -moneyDelta);
            }

            if (player.svPlayer.job.info.shared.upgrades.Length <= 1) return true;

            while (experienceDelta != 0)
            {
                var previousMaxExperience = player.GetMaxExperience();

                var newExperience = Mathf.Clamp(player.experience + experienceDelta, -1, previousMaxExperience);

                experienceDelta -= newExperience - player.experience;

                if (newExperience >= previousMaxExperience)
                {
                    if (player.rank + 1 >= player.svPlayer.job.info.shared.upgrades.Length)
                    {
                        if (player.experience != previousMaxExperience)
                        {
                            player.svPlayer.SetExperience(previousMaxExperience, true);
                        }
                        return true;
                    }
                    else
                    {
                        var newRank = player.rank + 1;
                        player.svPlayer.AddJobItems(player.svPlayer.job.info, newRank, false);
                        player.svPlayer.SetRank(newRank);
                        player.svPlayer.SetExperience(newExperience - previousMaxExperience, false);
                    }
                }
                else if (newExperience < 0)
                {
                    if (player.rank <= 0)
                    {
                        player.svPlayer.SendGameMessage("You lost your job");
                        player.svPlayer.SvResetJob();
                        return true;
                    }
                    else
                    {
                        player.svPlayer.SetRank(player.rank - 1);
                        player.svPlayer.SetExperience(newExperience + player.GetMaxExperience(), false);
                    }
                }
                else
                {
                    player.svPlayer.SetExperience(newExperience, true);
                }
            }

            return true;
        }
    }
}
