using System;
using System.Collections.Generic;
using System.Text;
using CAM.DataTransferObjects.Entita.DesignComponent;
using CAM.DataTransferObjects.Entita.MajorHardwareBuild;
using CAM.DataTransferObjects.Entita.MajorSoftwareBuild;
using CAM.DataTransferObjects.Entita.SystemType;

namespace CAM.DataTransferObjects.Entita.InizializeNewProduct
{
   public class InizializeNewProductCreateDto
    {
        public DesignComponentDtoCreate DesignComponentDto { get; set; }
        public SystemTypeDtoCreate SystemTypeDto { get; set; }
        public MajorSoftwareBuildDtoCreate MajorSoftwareBuildDto { get; set; }
        public MajorHardwareBuildDtoUpdate MajorHardwareBuildDto { get; set; }
    }
}
