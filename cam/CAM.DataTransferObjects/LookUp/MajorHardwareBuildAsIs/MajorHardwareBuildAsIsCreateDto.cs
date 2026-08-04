using CAM.DataTransferObjects.FunctionalityDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAM.DataTransferObjects.LookUp.MajorHardwareBuildAsIs
{
    public class MajorHardwareBuildAsIsCreateDto:MajorHardwareBuildAsIsDtoGrid
    {
        public List<KeyValuePairDto> OrgEqpmanuFacturerResources { get; set; }
        public List<KeyValuePairDto> HardwareSolutionResourceAllResources { get; set; }
        public List<KeyValuePairDto> PlatformResources { get; set; }
        public List<KeyValuePairDto> BuildConstructionResources { get; set; }
    }
}
