using System;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

namespace NetworkScanner.Network
{
    public static class PingAble
    {
        public static async Task<bool> PingHost(string nameOrAddress)
        {
            bool pingable = false;
            Ping pinger = null;

            try
            {
                pinger = new Ping();
                pinger.PingCompleted += PingCompletedEvent;

                PingReply reply = await pinger.SendPingAsync(nameOrAddress);
                
                pingable = reply.Status == IPStatus.Success;
            }
            catch (PingException)
            {
                // Discard PingExceptions and return false;
            }
            finally
            {
                pinger?.Dispose();
            }

            return pingable;
        }

        private static void PingCompletedEvent(object sender, PingCompletedEventArgs e)
        {
            //throw new NotImplementedException();
            Console.WriteLine(e.Reply.Status);
            //e.Reply.Status
        }

    }
}
