using BrokeProtocol.API;
using BrokeProtocol.Entities;

namespace WhiteChat.Commands
{
    public class Broadcasting : IScript
    {
        public static void Broadcast(ShPlayer player, string message)
        {
            InterfaceHandler.SendGameMessageToAll("&f[&4Broadcast&f] >> " + message);
        }
    }
}