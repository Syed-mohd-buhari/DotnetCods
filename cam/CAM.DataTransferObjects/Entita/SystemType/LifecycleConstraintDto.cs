using System;
using System.Collections.Generic;
using System.Text;
using CAM.DataTransferObjects.Entita.MajorHardwareBuild;
using CAM.DataTransferObjects.Entita.MajorSoftwareBuild;

namespace CAM.DataTransferObjects.Entita.SystemType
{
    public class LifecycleConstraintDto
    {
        public SystemTypeDtoUpdate SystemTypeDto { get; set; }
        public MajorHardwareBuildDtoUpdate MajorHardwareBuildDto { get; set; }
        public MajorSoftwareBuildDtoUpdate MajorSoftwareBuildDto { get; set; }
    }
}
