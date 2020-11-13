using NetworkScanner.Domain.Entities;
using Newtonsoft.Json;
using PipleLineExample.LightBulb;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace NetworkScanner.Domain.LightBulb
{
    #nullable enable

    public class WizLightBulb
    {
        private readonly UdpClient sendClient;
        private readonly int localPort = 38900;
        private readonly int remotePort = 38899;
        private readonly IPEndPoint localEP;
        private IPEndPoint? remoteEP;

        public WizLightBulb()
        {
            remoteEP = null;
            sendClient = new UdpClient();
            var timeOut = new TimeSpan(0, 5, 0);
            sendClient.Client.ReceiveTimeout = Convert.ToInt32(timeOut.TotalMilliseconds); //10 seconds.

            localEP = new IPEndPoint(IPAddress.Any, localPort);
            InitStuff();
        }

        public string MacFinder(string ip)
        {
            #region LighBulb

            try
            {
                remoteEP = new IPEndPoint(IPAddress.Parse(ip), remotePort);

                const string payload = "{\"method\":\"getPilot\",\"params\":{}}";

                //var payload = PayloadBuilder.BuildPayload("getPilot");
                byte[] send_buffer = Encoding.ASCII.GetBytes(payload);
                sendClient.Send(send_buffer, send_buffer.Length, remoteEP);

                var results = sendClient.Receive(ref remoteEP);

                var json = Encoding.Default.GetString(results);
                var result = JsonConvert.DeserializeObject<WizMsg>(json);
                var mac = Utils.Common;

                if(result.method.Equals("firstBeat"))
                {
                    var firstBeat = JsonConvert.DeserializeObject<FirstBeat>(json);
                    return firstBeat._params.mac;
                }

                if (result == null)
                {
                    var firstBeat = JsonConvert.DeserializeObject<FirstBeat>(json);
                    return firstBeat._params.mac;
                }
                else if (result == null)
                {
                    result = JsonConvert.DeserializeObject<WizConfig>(json);
                    return result.result.mac;
                }
                else if (result == null)
                {
                    return Utils.Common;
                }
                else
                {
                    return result.result.mac;
                }
            }
            catch (JsonSerializationException)
            {
                return Utils.Common;
            }
            catch (Exception)
            {
                return Utils.Common;
            }

            #endregion LighBulb
        }

        public WizConfig? GetLighBulbConfig(string ip)
        {
            #region LighBulb

            try
            {
                remoteEP = new IPEndPoint(IPAddress.Parse(ip), remotePort);

                const string method = "getSystemConfig";
                const string payload = "{\"method\":\"" + method + "\",\"params\":{}}";

                byte[] send_buffer = Encoding.ASCII.GetBytes(payload);
                sendClient.Send(send_buffer, send_buffer.Length, remoteEP);

                var results = sendClient.Receive(ref remoteEP);

                string json = Encoding.Default.GetString(results);
                var result = JsonConvert.DeserializeObject<WizConfig>(json);

                return result;
            }
            catch (Exception)
            {
                return new WizConfig();
            }

            #endregion LighBulb
        }

        public WizMsg? GetLightBulbMsg(string ip)
        {
            #region LighBulb

            try
            {
                remoteEP = new IPEndPoint(IPAddress.Parse(ip), remotePort);
                const string method = "getPilot";
                const string payload = "{\"method\":\"" + method + "\",\"params\":{}}";

                byte[] send_buffer = Encoding.ASCII.GetBytes(payload);

                sendClient.Send(send_buffer, send_buffer.Length, remoteEP);
                var results = sendClient.Receive(ref remoteEP);

                string json = Encoding.Default.GetString(results);
                return JsonConvert.DeserializeObject<WizMsg>(json);
            }
            catch (Exception)
            {
                return null;
            }

            #endregion LighBulb
        }

        private void InitStuff()
        {
            sendClient.ExclusiveAddressUse = false;
            sendClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            sendClient.Client.Bind(localEP);
        }
    }
}