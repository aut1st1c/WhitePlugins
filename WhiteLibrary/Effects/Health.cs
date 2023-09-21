using BrokeProtocol.API;
using BrokeProtocol.Entities;


namespace WhiteLibrary.Effects
{
    public class Health : IScript
    {
        public static void health(ShPlayer player)
        {
            player.svPlayer.SendGameMessage("Health setted");
            player.health = 300f;
            player.svPlayer.UpdateHealth();
        }
    }
}