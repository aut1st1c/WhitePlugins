using BrokeProtocol.API;
using BrokeProtocol.Entities;
using System.Threading.Tasks;


namespace WhiteLibrary.Effects
{
    public class Regen : IScript
    {
        public static async void Regeneration(ShPlayer player)
        {
            player.svPlayer.SendGameMessage("Regeneration started");
            for (int i = 0; i < 30; i++)
            {
                await Task.Delay(400);
                player.svPlayer.Heal(2);
                await Task.Delay(400);
                player.svPlayer.Heal(2);
                await Task.Delay(400);
                player.svPlayer.Heal(2);
                await Task.Delay(400);
                player.svPlayer.Heal(2);
                await Task.Delay(400);
                player.svPlayer.Heal(2);
            }
        }
    }
}