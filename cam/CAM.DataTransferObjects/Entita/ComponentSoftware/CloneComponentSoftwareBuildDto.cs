using CAM.DataAttributes.Grid;
using CAM.Enum;
using DocumentFormat.OpenXml.Drawing.Charts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CAM.DataTransferObjects.Entita.ComponentSoftware
{
    public class CloneComponentSoftwareBuildDto
    {
        public long ComponentSoftwareBuildId { get; set; }
        public string SoftwareVersion { get; set; }
        public DateTime? EndOfMaintenance { get; set; }
        public EOMEnum EomStatus { get; set; }
        public DateTime? EndOfsupport { get; set; }       
     //   public ComponentSoftwareBuildDtoCreate MSWCreateDto { get; set; }
         public IEnumerable<int> DesignContactIds { get; set; }

    }
}
