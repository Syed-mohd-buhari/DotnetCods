using System.Collections.Generic;

namespace CAM.DataTransferObjects.Entita.MajorSoftwareBuild
{
    public class MajorSoftwareBuildToCloneDto
    {
        public long MajorSoftwareBuildsId { get; set; }
        public string OriginalEquipmentManufacturer { get; set; }
        public string ProductName { get; set; }
        public string SoftwareVersion { get; set; }
        public decimal? ProductNameId { get; set; }
        public IDictionary<long, string> DesignComponentResource { get; set; }


        //Ticket 767 - Bundle - CRUD operation for MajorsoftwareBuildBundle Table and implement Configuration File        
        public List<long> TCPSoftwareCompatibilityIdList { get; set; }
        public List<long> TCISoftwareCompatibilityIdList { get; set; }

        public IDictionary<long, string> TCPBundleVersion { get; set; }

        public IDictionary<long, string> TCIBundleVersion { get; set; }

        public IEnumerable<int> DesignContactIds { get; set; }

        public IDictionary<int, string> DesignContacts { get; set; }
        public bool? Isvmware { get; set; }

   }
}
