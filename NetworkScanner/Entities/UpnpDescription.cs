using System;

namespace NetworkScanner.Entities
{
    [Serializable]
    [System.ComponentModel.DesignerCategory("code")]
    [System.Xml.Serialization.XmlType(AnonymousType = true, Namespace = "urn:schemas-upnp-org:device-1-0")]
    [System.Xml.Serialization.XmlRoot(Namespace = "urn:schemas-upnp-org:device-1-0", IsNullable = false)]

    public class UpnpDescription
    {
        // NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
        /// <remarks/>

        /// <remarks/>
        public rootSpecVersion specVersion { get; set; }

        /// <remarks/>
        public string URLBase { get; set; }
        
        /// <remarks/>
        public rootDevice device { get; set; }


        /// <remarks/>
        [Serializable]
        [System.ComponentModel.DesignerCategory("code")]
        [System.Xml.Serialization.XmlType(AnonymousType = true, Namespace = "urn:schemas-upnp-org:device-1-0")]
        public partial class rootSpecVersion
        {

            /// <remarks/>
            public byte major { get; set; }

            /// <remarks/>
            public byte minor { get; set; }
        }

        /// <remarks/>
        [Serializable]
        [System.ComponentModel.DesignerCategory("code")]
        [System.Xml.Serialization.XmlType(AnonymousType = true, Namespace = "urn:schemas-upnp-org:device-1-0")]
        public partial class rootDevice
        {

            /// <remarks/>
            [System.Xml.Serialization.XmlElement(Namespace = "http://schemas.microsoft.com/windows/pnpx/2005/11")]
            public string X_hardwareId { get; set; }

            /// <remarks/>
            [System.Xml.Serialization.XmlElement(Namespace = "http://schemas.microsoft.com/windows/pnpx/2005/11")]
            public string X_compatibleId { get; set; }

            /// <remarks/>
            [System.Xml.Serialization.XmlElement(Namespace = "http://schemas.microsoft.com/windows/pnpx/2005/11")]
            public string X_deviceCategory { get; set; }

            /// <remarks/>
            [System.Xml.Serialization.XmlElement("X_deviceCategory", Namespace = "http://schemas.microsoft.com/windows/2008/09/devicefoundation")]
            public string X_deviceCategory1 { get; set; }

            /// <remarks/>
            public string deviceType { get; set; }

            /// <remarks/>
            public string friendlyName { get; set; }

            /// <remarks/>
            public string manufacturer { get; set; }

            /// <remarks/>
            public string manufacturerURL { get; set; }

            /// <remarks/>
            public string modelDescription { get; set; }

            /// <remarks/>
            public string modelName { get; set; }

            /// <remarks/>
            public string modelNumber { get; set; }

            /// <remarks/>
            public string modelURL { get; set; }

            /// <remarks/>
            public string serialNumber { get; set; }

            /// <remarks/>
            public string UDN { get; set; }

            /// <remarks/>
            public string UPC { get; set; }

            /// <remarks/>
            public rootDeviceServiceList serviceList { get; set; }

            /// <remarks/>
            public rootDeviceDeviceList deviceList { get; set; }

            /// <remarks/>
            public string presentationURL { get; set; }
        }

        /// <remarks/>
        [Serializable]
        [System.ComponentModel.DesignerCategory("code")]
        [System.Xml.Serialization.XmlType(AnonymousType = true, Namespace = "urn:schemas-upnp-org:device-1-0")]
        public partial class rootDeviceServiceList
        {
            /// <remarks/>
            public rootDeviceServiceListService service { get; set; }
        }

        /// <remarks/>
        [Serializable]
        [System.ComponentModel.DesignerCategory("code")]
        [System.Xml.Serialization.XmlType(AnonymousType = true, Namespace = "urn:schemas-upnp-org:device-1-0")]
        public partial class rootDeviceServiceListService
        {

            /// <remarks/>
            public string serviceType { get; set; }

            /// <remarks/>
            public string serviceId { get; set; }

            /// <remarks/>
            public string controlURL { get; set; }

            /// <remarks/>
            public string eventSubURL { get; set; }

            /// <remarks/>
            public string SCPDURL { get; set; }
        }

        /// <remarks/>
        [Serializable]
        [System.ComponentModel.DesignerCategory("code")]
        [System.Xml.Serialization.XmlType(AnonymousType = true, Namespace = "urn:schemas-upnp-org:device-1-0")]
        public partial class rootDeviceDeviceList
        {
            /// <remarks/>
            public rootDeviceDeviceListDevice device { get; set; }
        }

        /// <remarks/>
        [Serializable]
        [System.ComponentModel.DesignerCategory("code")]
        [System.Xml.Serialization.XmlType(AnonymousType = true, Namespace = "urn:schemas-upnp-org:device-1-0")]
        public partial class rootDeviceDeviceListDevice
        {

            /// <remarks/>
            public string deviceType { get; set; }

            /// <remarks/>
            public string friendlyName { get; set; }

            /// <remarks/>
            public string manufacturer { get; set; }

            /// <remarks/>
            public string manufacturerURL { get; set; }

            /// <remarks/>
            public string modelDescription { get; set; }

            /// <remarks/>
            public string modelName { get; set; }

            /// <remarks/>
            public string modelNumber { get; set; }

            /// <remarks/>
            public string modelURL { get; set; }

            /// <remarks/>
            public string serialNumber { get; set; }

            /// <remarks/>
            public string UDN { get; set; }

            /// <remarks/>
            public string UPC { get; set; }

            /// <remarks/>
            public rootDeviceDeviceListDeviceServiceList serviceList { get; set; }

            /// <remarks/>
            public rootDeviceDeviceListDeviceDeviceList deviceList { get; set; }
        }

        /// <remarks/>
        [Serializable]
        [System.ComponentModel.DesignerCategory("code")]
        [System.Xml.Serialization.XmlType(AnonymousType = true, Namespace = "urn:schemas-upnp-org:device-1-0")]
        public partial class rootDeviceDeviceListDeviceServiceList
        {
            /// <remarks/>
            public rootDeviceDeviceListDeviceServiceListService service { get; set; }
        }

        /// <remarks/>
        [Serializable]
        [System.ComponentModel.DesignerCategory("code")]
        [System.Xml.Serialization.XmlType(AnonymousType = true, Namespace = "urn:schemas-upnp-org:device-1-0")]
        public partial class rootDeviceDeviceListDeviceServiceListService
        {

            /// <remarks/>
            public string serviceType { get; set; }

            /// <remarks/>
            public string serviceId { get; set; }

            /// <remarks/>
            public string controlURL { get; set; }

            /// <remarks/>
            public string eventSubURL { get; set; }

            /// <remarks/>
            public string SCPDURL { get; set; }
        }

        /// <remarks/>
        [Serializable]
        [System.ComponentModel.DesignerCategory("code")]
        [System.Xml.Serialization.XmlType(AnonymousType = true, Namespace = "urn:schemas-upnp-org:device-1-0")]
        public partial class rootDeviceDeviceListDeviceDeviceList
        {

            /// <remarks/>
            public rootDeviceDeviceListDeviceDeviceListDevice device { get; set; }
        }

        /// <remarks/>
        [Serializable]
        [System.ComponentModel.DesignerCategory("code")]
        [System.Xml.Serialization.XmlType(AnonymousType = true, Namespace = "urn:schemas-upnp-org:device-1-0")]
        public partial class rootDeviceDeviceListDeviceDeviceListDevice
        {

            /// <remarks/>
            public string deviceType { get; set; }

            /// <remarks/>
            public string friendlyName { get; set; }

            /// <remarks/>
            public string manufacturer { get; set; }

            /// <remarks/>
            public string manufacturerURL { get; set; }

            /// <remarks/>
            public string modelDescription { get; set; }

            /// <remarks/>
            public string modelName { get; set; }

            /// <remarks/>
            public string modelNumber { get; set; }

            /// <remarks/>
            public string modelURL { get; set; }

            /// <remarks/>
            public string serialNumber { get; set; }

            /// <remarks/>
            public string UDN { get; set; }

            /// <remarks/>
            public string UPC { get; set; }

            /// <remarks/>
            [System.Xml.Serialization.XmlArrayItem("service", IsNullable = false)]
            public rootDeviceDeviceListDeviceDeviceListDeviceService[] serviceList { get; set; }
        }

        /// <remarks/>
        [Serializable]
        [System.ComponentModel.DesignerCategory("code")]
        [System.Xml.Serialization.XmlType(AnonymousType = true, Namespace = "urn:schemas-upnp-org:device-1-0")]
        public partial class rootDeviceDeviceListDeviceDeviceListDeviceService
        {

            /// <remarks/>
            public string serviceType { get; set; }

            /// <remarks/>
            public string serviceId { get; set; }

            /// <remarks/>
            public string controlURL { get; set; }

            /// <remarks/>
            public string eventSubURL { get; set; }

            /// <remarks/>
            public string SCPDURL { get; set; }
        }

    }
}
