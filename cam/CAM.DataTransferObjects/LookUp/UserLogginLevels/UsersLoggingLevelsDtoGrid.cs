using CAM.DataAttributes.Grid;
using CAM.DataTransferObjects.Entita.NetworkElementAsIs;
using CAM.DataTransferObjects.LookUp.UserLogginLevels;
using CAM.DataTransferObjects.QueryDto.Base;
using CAM.Entities.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CAM.DataTransferObjects.LookUp.SoftwareApplicationTypes
{
    public class UsersLoggingLevelsDtoGrid : UserLoggingLevelDto
    {

        [DateRangeGrid]
        [OrderGrid(Order = 3)]
        [Default]
        public DateTime? LastModified { get; set; }

        [OrderGrid(Order = 4)]
        [MailTo]
        [Default]
        public string LastModifiedBy { get; set; }


    }
    public class UserLoggingLevelDto : GridDtoBase
    {
        [DisplayName("LoggingLevel")]
        public string AllLoggingLevels { get; set; }
        public string CurrentLogLevel { get; set; }

    }

}