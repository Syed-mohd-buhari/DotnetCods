using System.Collections.Generic;

namespace CAM.DataTransferObjects.Entita.SoftwareConfiguration
{
    public class SWConfigurationFilterDto
    {
        public List<string> OpCoResource { get; set; }
        public List<string> OemResource { get; set; }
        public List<string> ElementResource { get; set; }
    }
}
