using System.Collections.Generic;

namespace CAM.DataTransferObjects.Entita.DesignComponentFamily
{
    public class DesignComponentFamilyDtoCreate : DesignComponentFamilyDto
    {
        public IDictionary<long, string> SubNetworkBoundariesResource { get; set; }
        public IDictionary<int, string>? SubNetworkSupportedServices { get; set; }
        public IDictionary<short, string>? SharingTypeResource { get; set; }
        public IDictionary<decimal, string>? ProductNameResource { get; set; }
        public decimal? ProductNameId { get; set; }

    }
}