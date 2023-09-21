using BrokeProtocol.API;
using BrokeProtocol.Entities;


namespace WhiteLibrary.Commands
{
    public class HP : IScript
    {
        public static void Health(ShPlayer player, ShPlayer target = null)
        {
            if (!target) target = player;
            string str1 = target.health.ToString();
            player.svPlayer.SendGameMessage(target.username + " has " + str1 + " Health Points");
        }
    }
}