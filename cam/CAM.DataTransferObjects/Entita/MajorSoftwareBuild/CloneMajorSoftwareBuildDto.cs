using CAM.Enum;
using System;
using System.Collections.Generic;

namespace CAM.DataTransferObjects.Entita.MajorSoftwareBuild
{
    public class CloneMajorSoftwareBuildDto
    {
        public long MajorSoftwareBuildsId { get; set; }
        public string SoftwareVersion { get; set; }
        public DateTime? EndOfMaintenance { get; set; }
        public EOMEnum EomStatus { get; set; }
        public DateTime? EndOfsupport { get; set; }
        ////Ticket 642 - Add - Software Upgrade Utility to be enhanced to cover all required design components, System Types and Sub network boundary is created 
        public MajorSoftwareBuildDtoCreate MSWCreateDto {get;set;}

        //Ticket 767 - Bundle - CRUD operation for MajorsoftwareBuildBundle Table and implement Configuration File        
        public List<long> TCPSoftwareCompatibilityIdList { get; set; }
        public List<long> TCISoftwareCompatibilityIdList { get; set; }

        public IEnumerable<int> DesignContactIds { get; set; }
        public bool? Isvmware { get; set; }


    }
}
