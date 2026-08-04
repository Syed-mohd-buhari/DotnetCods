using CAM.DataTransferObjects.QueryDto.Base;
using System.Collections.Generic;

namespace CAM.DataTransferObjects.QueryDto
{
    public class AppSettingsConfiguartionQueryDto : QueryObject
    {
        public List<long> AppSettingsConfiguartionId { get; set; }
        public List<long> AppSettingsId { get; set; }
        public List<string> SettingsValue { get; set; }
    }

}
