using BrokeProtocol.API;
using BrokeProtocol.Entities;
using BrokeProtocol.Required;
using System.Threading.Tasks;

namespace WhiteLibrary.Effects
{
    public class Speed : IScript
    {
        public static async void speed(ShPlayer player)
        {
            var maxspeed = player.maxSpeed;
            if (maxspeed == 20f)
            {
                player.svPlayer.SvForceStance(StanceIndex.KnockedOut);
                player.svPlayer.SendGameMessage("You got an overdose");
            }
            else
            {
                player.svPlayer.CustomData.AddOrUpdate("PodSpidami???", "Yes");
                player.svPlayer.SendGameMessage("Speed given");
                player.svPlayer.SetMaxSpeed(20f);
                await Task.Delay(60000);
                player.svPlayer.SetMaxSpeed(maxspeed);
                player.svPlayer.CustomData.AddOrUpdate("PodSpidami???", "No");
            }
        }
    }
}