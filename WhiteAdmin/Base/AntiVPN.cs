using BrokeProtocol.API;
using BrokeProtocol.Entities;
using BrokeProtocol.Utility.Networking;
using System.Net;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace WhiteAdmin
{
    internal class AntiVPN : PlayerEvents
    {
        [Target(GameSourceEvent.PlayerInitialize, ExecutionMode.Event)]
        public async void OnEvent(ShPlayer player)
        {
            if (!player.isHuman)
                return;

            if (!IsLocalIpAddress(player.svPlayer.connection.IP))
            {
                player.svPlayer.SendGameMessage("〔WhiteProject〕 | Проверяем соединение...");

                IpInfo ipInfo = await IpInfoFetcher.CheckIfProxy(player.svPlayer.connection.IP);

                if (ipInfo != null)
                {
                    if (ipInfo.status == "success")
                    {
                        try
                        {
                            if (ipInfo.proxy == true || ipInfo.hosting == true)
                            {
                                player.svPlayer.SendGameMessage("〔WhiteProject〕 | Вы исключены за vpn/proxy!");
                                await Task.Delay(3000);
                                player.svPlayer.svManager.Disconnect(player.svPlayer.connection, DisconnectTypes.Kicked);
                            }
                            else
                            {
                                player.svPlayer.SendGameMessage("〔WhiteProject〕 | Вы прошли проверку!");
                            }
                        }
                        catch { }
                    }
                    else if (ipInfo.status == "fail")
                    {
                        player.svPlayer.SendGameMessage("〔WhiteProject〕 | Произошла ошибка!");
                        player.svPlayer.svManager.Disconnect(player.svPlayer.connection, DisconnectTypes.Normal);
                    }
                }
            }
            else if (IsLocalIpAddress(player.svPlayer.connection.IP) || player.svPlayer.HasPermission("wa.bypass"))
            {
                player.svPlayer.SendGameMessage("〔WhiteProject〕 | Bypassed!");
            }
        }

        public class IpInfo
        {
            public string status;
            public bool proxy;
            public bool hosting;
        }

        public static class IpInfoFetcher
        {
            public static async Task<IpInfo> CheckIfProxy(string ip)
            {
                string apiUrl = $"https://thingproxy.freeboard.io/fetch/http://ip-api.com/json/{ip}?fields=status,proxy,hosting";
                using (UnityWebRequest request = UnityWebRequest.Get(apiUrl))
                {
                    var tcs = new TaskCompletionSource<IpInfo>();

                    request.SendWebRequest().completed += operation =>
                    {
                        if (request.result != UnityWebRequest.Result.Success)
                        {
                            Debug.LogError($"Request failed: {request.error}");
                            tcs.SetResult(null);
                            return;
                        }

                        string responseBody = request.downloadHandler.text;
                        IpInfo ipInfo = JsonUtility.FromJson<IpInfo>(responseBody);
                        tcs.SetResult(ipInfo);
                    };

                    return await tcs.Task;
                }
            }
        }

        public static bool IsLocalIpAddress(string host)
        {
            try
            {
                IPAddress[] hostIPs = Dns.GetHostAddresses(host);
                IPAddress[] localIPs = Dns.GetHostAddresses(Dns.GetHostName());

                foreach (IPAddress hostIP in hostIPs)
                {
                    if (IPAddress.IsLoopback(hostIP))
                        return true;

                    foreach (IPAddress localIP in localIPs)
                    {
                        if (hostIP.Equals(localIP))
                            return true;
                    }
                }
            }
            catch { }

            return false;
        }
    }
}