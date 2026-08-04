using CAM.DataTransferObjects.QueryDto.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace CAM.DataTransferObjects.QueryDto
{
    public class UsersLoggingLevelsDtoQuery : QueryObject
    {
        public int UsersId { get; set; }
        public List<string> LoggingLevels { get; set; }
    }
}
