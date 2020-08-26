using MIG;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace NetworkScanner.Network
{
    public class UPnP
    {

        public UPnP()
        {


            string webPort = "8088";

            Console.WriteLine("MigService test APP");
            Console.WriteLine("URL: http://localhost:{0}", webPort);

            var migService = new MigService();

            // Add and configure the Web gateway
            var web = migService.AddGateway("WebServiceGateway");
            web.SetOption("HomePath", "html");
            web.SetOption("BaseUrl", "/pages/");
            web.SetOption("Host", "*");
            web.SetOption("Port", webPort);
            web.SetOption("Password", "");
            web.SetOption("EnableFileCaching", "False");

            /*
            // Add and configure the Web Socket gateway
            var ws = migService.AddGateway("WebSocketGateway");
            ws.SetOption("Port", "8181");
            */

            migService.StartService();

            // Enable UPnP interface

            var upnp = migService.AddInterface("Protocols.UPnP", "MIG.Protocols.dll");
            var x =  upnp.GetDomain();
            migService.EnableInterface("Protocols.UPnP");

            while (true)
            {
                Thread.Sleep(10000);
            }



        }


    }
}
