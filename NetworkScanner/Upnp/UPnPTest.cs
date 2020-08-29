using System;
using OpenSource.UPnP;
using System.Threading;

namespace NetworkScanner.Upnp
{
    public class UPnPTest
    {
        public void StartHere()
        {
            var controlPoint = new UpnpSmartControlPoint();
            controlPoint.OnAddedDevice += ControPoint_OnAddedDevice;
            controlPoint.OnRemovedDevice += ControPoint_OnRemovedDevice;
            controlPoint.OnDeviceExpired += ControPoint_OnDeviceExpired;

            //StartControlPoint(controlPoint);

            Console.WriteLine("Ready");
            while (true)
            {
                Thread.Sleep(1000);

                var s = Console.ReadLine();
                if (s == "-" && controlPoint != null)
                {
                    StopControlPoint(controlPoint);
                }
                if (s == "+")
                {
                    StartControlPoint(controlPoint);
                }
            }
        }

        private static void StartControlPoint(UpnpSmartControlPoint controlPoint)
        {
            StopControlPoint(controlPoint);
            controlPoint = new UpnpSmartControlPoint();
            controlPoint.OnUpdatedDevice += controlPoint_OnUpodateDevice;
            controlPoint.OnAddedDevice += ControPoint_OnAddedDevice;
            controlPoint.OnRemovedDevice += ControPoint_OnRemovedDevice;
            controlPoint.OnDeviceExpired += ControPoint_OnDeviceExpired;
        }

        private static void controlPoint_OnUpodateDevice(UpnpSmartControlPoint sender, UPnPDevice Device)
        {
            throw new NotImplementedException();
        }

        private static void StopControlPoint(UpnpSmartControlPoint controlPoint)
        {
            controlPoint.ShutDown();
            controlPoint.OnAddedDevice -= ControPoint_OnAddedDevice;
            controlPoint.OnRemovedDevice -= ControPoint_OnRemovedDevice;
            controlPoint.OnDeviceExpired -= ControPoint_OnDeviceExpired;
            controlPoint = null;
        }

        [Obsolete]
        private static void AddWeMoSwitch()
        {
            var localDevice = UPnPDevice.CreateRootDevice( /* expiration */ 3600, /* version*/ 1, /* web dir */ null);
            localDevice.StandardDeviceType = "urn:Belkin:device:controllee";
            localDevice.UniqueDeviceName = "Lightswitch-32f9a52c-79d2-4ae2-8957-1f5a0f044e36";
            localDevice.FriendlyName = "Test Lamp";
            //localDevice.Icon = null;
            //localDevice.HasPresentation = true;
            //localDevice.PresentationURL = presentationUrl;
            localDevice.Major = 1; localDevice.Minor = 0;
            localDevice.SerialNumber = "1234567890";
            localDevice.ModelNumber = "3.1234";
            localDevice.Manufacturer = "Belkin International Inc.";
            localDevice.ManufacturerURL = "http://www.belkin.com";
            localDevice.ModelName = "Socket";
            localDevice.ModelDescription = "Belkin Plugin Socket 1.0";
            /*if (Uri.IsWellFormedUriString(manufacturerUrl, UriKind.Absolute))
            {
                localDevice.ModelURL = new Uri(manufacturerUrl);
            }
            */
            localDevice.UserAgentTag = "redsonic";

            // Create an instance of the BasicEvent service
            dynamic instance = new { };

            // Declare the "BasicEvent1" service
            var service = new UPnPService(
                // Version
                1.0,
                // Service ID
                "urn:Belkin:serviceId:basicevent1",
                // Service Type
                "urn:Belkin:service:basicevent:1",
                // Standard Service?
                true,
                // Service Object Instance
                instance
            );
            service.ControlURL = "/upnp/control/basicevent1";
            service.EventURL = "/upnp/event/basicevent1";
            service.SCPDURL = "/eventservice.xml";

            string stateVarName = "BinaryState";
            var stateVariable = new UPnPStateVariable(stateVarName, typeof(bool), true);
            stateVariable.AddAssociation("GetBinaryState", stateVarName);
            stateVariable.AddAssociation("SetBinaryState", stateVarName);
            stateVariable.Value = false;
            service.AddStateVariable(stateVariable);

            instance.GetBinaryState = new Func<bool>(() => (bool)service.GetStateVariable(stateVarName));
            instance.SetBinaryState = new Action<int>((BinaryState) =>
            {
                Console.WriteLine("SetBinaryState({0})", BinaryState);
                service.SetStateVariable(stateVarName, BinaryState != 0);
            });

            // Add the methods
            service.AddMethod("GetBinaryState", stateVarName);
            service.AddMethod("SetBinaryState", stateVarName);

            // Add the service
            localDevice.AddService(service);
            // Start the WeMo switch device UPnP simulator
            localDevice.StartDevice();
        }

        private static void ControPoint_OnAddedDevice(UpnpSmartControlPoint sender, UPnPDevice device)
        {
            Console.WriteLine("Added " + device.FriendlyName);
        }

        private static void ControPoint_OnRemovedDevice(UpnpSmartControlPoint sender, UPnPDevice device)
        {
            Console.WriteLine("Removed " + device.FriendlyName);
        }

        private static void ControPoint_OnDeviceExpired(UpnpSmartControlPoint sender, UPnPDevice device)
        {
            Console.WriteLine("Expired " + device.FriendlyName);
        }
    }
}