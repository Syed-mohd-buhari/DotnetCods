using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAM.DataTransferObjects.LookUp.MajorHardwareBuildAsIs
{
    public class MajorHardwareBuildAsIsUpdateDto
    {
        public Dictionary<long, string> OrgEqpmanuFacturerResources { get; set; }
        public Dictionary<long, string> HardwareSolutionResourceAllResources { get; set; }
        public Dictionary<long, string> PlatformResources { get; set; }
        public Dictionary<long, string> BuildConstructionResources { get; set; }
    }
}
