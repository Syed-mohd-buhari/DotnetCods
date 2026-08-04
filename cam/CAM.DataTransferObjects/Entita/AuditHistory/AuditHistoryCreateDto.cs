using System.Collections.Generic;

namespace CAM.DataTransferObjects.Entita.AuditHistory
{
    public class AuditHistoryCreateDto : AuditHistoryDtoGrid
    {       
        public int Creationuser { get; set; }   
        public int Modificationuser { get; set; }
                
    }
    public class NetworkElementAsIsAttributes
    {
        public List<string> AsIsAttributes { get; set; } = new List<string> { "softwareproductnumber",
            "softwareproductdate",
            "softwareinstalldate",
            "dataacquisitiondate",
            "nodetype",
            "platformtype",
            "hardwaretype",
            "softwarereleaseinformation",
            "ipaddress"
        };
            
    }
}
