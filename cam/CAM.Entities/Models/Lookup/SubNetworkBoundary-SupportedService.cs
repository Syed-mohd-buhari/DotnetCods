using System;
using System.Collections.Generic;
using System.Text;

namespace CAM.Entities.Models.Lookup
{
    public class SubNetworkBoundary_SupportedService
    {
        public int Id { get; set; }
        public long SubNetworkId { get; set; }
        public int ServiceId { get; set; }
        public  SupportedService SupportedService { get; set; }
        public  SubNetworkBoundary SubNetworkBoundary { get; set; }
    }
}
