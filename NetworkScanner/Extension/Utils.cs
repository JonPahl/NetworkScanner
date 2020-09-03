using System.Collections.Generic;

namespace NetworkScanner.Extension
{
    public static class Utils
    {
        public static List<string> LoadCommunities()
        {
            var communities = new List<string>();
            communities.Add("Public");
            communities.Add("public");
            communities.Add("community");

            return communities;
        }
    }
}
