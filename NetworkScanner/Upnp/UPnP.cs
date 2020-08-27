using MIG;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace NetworkScanner.Network
{
    public class UPnP
    {

        public UPnP()
        {
            string webPort = "8088";

            //Console.WriteLine("MigService test APP");
            //Console.WriteLine("URL: http://localhost:{0}", webPort);

            var migService = new MigService();

            // Add and configure the Web gateway
            var web = migService.AddGateway("WebServiceGateway");
            web.SetOption("HomePath", "html");
            web.SetOption("BaseUrl", "/pages/");
            web.SetOption("Host", "*");
            web.SetOption("Port", webPort);
            web.SetOption("Password", "");
            web.SetOption("EnableFileCaching", "False");
            
            // Add and configure the Web Socket gateway
            var ws = migService.AddGateway("WebSocketGateway");
            ws.SetOption("Port", "8181");

            ws.PostProcessRequest += Ws_PostProcessRequest;

            migService.StartService();

            // Enable UPnP interface          
            var upnp = migService.AddInterface("Protocols.UPnP", "MIG.Protocols.dll");
            migService.EnableInterface("Protocols.UPnP");
            migService.EnableInterface("upnp:rootdevice");

            migService.RegisterApi("http://192.168.1.1:49152/GetDeviceInfo", test);

            migService.InterfacePropertyChanged += interfacePropertyChangedEventHandler;
            migService.InterfaceModulesChanged += InterfaceModeulesChangedEventHandler;
            migService.GatewayRequestPostProcess += MigService_GatewayRequestPostProcess;

            while (true)
            {
                Thread.Sleep(10000);
            }
        }

        private object test(MigClientRequest arg)
        {
            throw new NotImplementedException();
        }

        private void Ws_PostProcessRequest(object sender, ProcessRequestEventArgs args)
        {
            throw new NotImplementedException();
        }

        private void MigService_GatewayRequestPostProcess(object sender, ProcessRequestEventArgs args)
        {
            throw new NotImplementedException();
        }

        private void InterfaceModeulesChangedEventHandler(object sender, InterfaceModulesChangedEventArgs args)
        {
            //Console.WriteLine(args.Domain);
            //throw new NotImplementedException();
        }

        private void interfacePropertyChangedEventHandler(object sender, InterfacePropertyChangedEventArgs args)
        {
            var results = sender as MIG.Interfaces.Protocols.UPnP;
            /*
            Console.Clear();
            Console.SetCursorPosition(0, 0);
            for (int i = 0; i < results.UpnpControlPoint.DeviceTable.Count; i++)
            {
                var device = results.UpnpControlPoint.DeviceTable[i] as OpenSource.UPnP.UPnPDevice;

                Console.WriteLine(device.FriendlyName);
                Console.WriteLine(device.ModelDescription);
                Console.WriteLine(device.BaseURL.DnsSafeHost);

                foreach(var service in device.Services.ToList())
                {
                    Console.WriteLine($"\t{service.EventURL}");
                }
                Console.WriteLine();               
            }
            */
           
        }
    }
}
