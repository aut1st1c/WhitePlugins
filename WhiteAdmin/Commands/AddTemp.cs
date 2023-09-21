using BrokeProtocol.API;
using BrokeProtocol.Entities;
using BrokeProtocol.LiteDB;
using BrokeProtocol.Managers;
using System;

namespace WhiteAdmin
{
    public class AddPlayer
    {
        public static void TempBan(ShPlayer player, ShPlayer target, int time, string reason)
        {
            var user = SvManager.Instance.database.Users.FindById(target.username);
            SvManager.Instance.database.Bans.Insert(user.IP, new Ban
            {
                Reason = reason,
                Username = player.username
            });
            WhiteAdmin.TempBan.AddTempBan(target.username, time);
            SvManager.Instance.Disconnect(target.svPlayer.connection, 6U);
            InterfaceHandler.SendGameMessageToAll($"Игрок &4{target.username}&f был заблокирован администратором &4{player.username}&f на &4{time}&f минут, по причине: &4{reason}&f");
            Console.WriteLine($"{target.username} banned for {time} min");
        }
    }
}
