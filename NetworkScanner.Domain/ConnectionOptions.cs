namespace NetworkScanner.Domain
{
    public class ConnectionOptions
    {
        public ConnectionOptions(string ip, string user, string pwd)
        {
            IpAddress = ip;
            User = user;
            Pwd = pwd;
        }

        public string IpAddress { get; set; }
        public string User { get; set; }
        public string Pwd { get; set; }
    }
}
