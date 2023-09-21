using BrokeProtocol.API;
using BrokeProtocol.Entities;


namespace WhiteLibrary.Commands
{
    public class Version : IScript
    {
        public static void About(ShPlayer player)
        {
            player.svPlayer.SendGameMessage("[WhiteLibrary] Plugin by aut1st1c. Version 1.0.1");
        }
    }
}