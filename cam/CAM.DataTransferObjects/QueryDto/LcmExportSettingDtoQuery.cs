using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace CAM.DataTransferObjects.QueryDto
{

    public class LcmExportSettingDtoQuery : QueryObject
    {
        public List<int> Id { get; set; }
        public List<string> Description { get; set; }
        public List<string> LcmHistoricalInfo { get; set; }
        public List<bool> IsHistorical { get; set; }
        public List<bool> IsCurrent { get; set; }
        public List<int> ReportLevel { get; set; }
        public List<int> ReportType { get; set; }

        public List<bool> IsDefault { get; set; }
    }
}
