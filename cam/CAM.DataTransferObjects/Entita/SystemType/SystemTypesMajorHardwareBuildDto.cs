using CAM.DataTransferObjects.Entita.MajorHardwareBuild;

namespace CAM.DataTransferObjects.Entita.SystemType
{
    public class SystemTypesMajorHardwareBuildDto
    {
        public long SystemTypeId { get; set; }
        public long MajorHardwareId { get; set; }
        public MajorHardwareBuildDto MajorHardware { get; set; }
        public SystemTypeDto SystemType { get; set; }
    }
}