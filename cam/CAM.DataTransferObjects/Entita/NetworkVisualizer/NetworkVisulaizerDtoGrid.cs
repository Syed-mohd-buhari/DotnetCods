using System.Collections.Generic;

namespace CAM.DataTransferObjects.Entita.NetworkVisualizer
{
    public class NetworkVisulaizerDtoGrid
    {
        public string OpCo { get; set; }
        public string ElementName { get; set; }
        public string Product { get; set; }
        public string Vendor { get; set; }
        public string Location { get; set; }
        public string VRFName { get; set; }
        public string Service { get; set; }
        public string IpAddress { get; set; }

    }
    public class NetworkVisulaizerDto
    {
        public Dictionary<short, string> OpCoResource { get; set; }
        public Dictionary<int, string> SupportServiceResource { get; set; }
    }
}
