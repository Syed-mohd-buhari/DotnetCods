using System;
using System.Collections.Generic;
using System.Text;

namespace CAM.DataTransferObjects.Entita.DesignComponent
{
    public class SupportedAllServicesModel
    {
        public SupportedAllServicesModel(List<SystemAndSubNetworkBoundary> usedSubNetworkBoundaries, List<SystemAndSubNetworkBoundary> unUsedSubNetworkBoundaries, bool supportedAllServices, int? vfNameId = null, string vodafoneName = null)
        {
            UsedSubNetworkBoundaries = usedSubNetworkBoundaries;
            UnUsedSubNetworkBoundaries = unUsedSubNetworkBoundaries;
            SupportedAllServices = supportedAllServices;
            VodafoneNameId=vfNameId;
            VodafoneName = vodafoneName;
        }
        public List<SystemAndSubNetworkBoundary> UsedSubNetworkBoundaries { get; set; }
        public List<SystemAndSubNetworkBoundary> UnUsedSubNetworkBoundaries { get; set; }
        public bool SupportedAllServices { get; set; }
        public int? VodafoneNameId { get; set; }

        public string VodafoneName { get; set; }
    }
}
