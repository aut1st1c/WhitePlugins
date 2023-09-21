using BrokeProtocol.API;
using BrokeProtocol.Entities;

namespace WhiteChat.Commands
{
    public class Advertise : IScript
    {
        public static void Advert(ShPlayer player, string message)
        {
            string JobColor = ("&8");
            var a = (player.svPlayer.job.info.shared.jobIndex);
            if (a == 0 || a == 4 || a == 5 || a == 9 || a == 10 || a == 11 || a == 13)
            {
                JobColor = ("&7");
            }
            else
            {
                if (a == 1 || a == 14)
                {
                    JobColor = ("&6");
                }
                else
                {
                    if (a == 2 || a == 6 || a == 7 || a == 8)
                    {
                        JobColor = ("&c");
                    }
                    else
                    {
                        if (a == 3 || a == 12)
                        {
                            JobColor = ("&9");
                        }
                    }
                }
            }
            InterfaceHandler.SendGameMessageToAll($"[&6Реклама&f] {JobColor}{player.username}&f >> {message}");
        }
    }
}