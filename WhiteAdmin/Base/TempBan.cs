using BrokeProtocol.Managers;
using System;
using System.Threading.Tasks;

namespace WhiteAdmin
{
    public class TempBan
    {
        public async static void AddTempBan(string name, int time)
        {
            await Task.Delay(time * 60000);
            WhiteAdmin.TempBan.RemoveTempBan(name);
        }
        public static void RemoveTempBan(string name)
        {
            var user = SvManager.Instance.database.Users.FindById(name);
            bool flag2 = !SvManager.Instance.database.Bans.Delete(user.IP);
            if (flag2)
            {
                Console.WriteLine($"{name} isnt banned");
            }
            else
            {
                SvManager.Instance.database.Users.Upsert(user);
                Console.WriteLine($"{name} unbanned");
            }
        }
    }
}
