using CAM.DataAttributes.Grid;
using CAM.DataTransferObjects.QueryDto.Base;
using DocumentFormat.OpenXml.Drawing.Charts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CAM.DataTransferObjects.LookUp.LcmDeploymentStatus
{
    public class LcmDeploymentStatusDtoGrid : LcmDeploymentStatusDto
    {
        [DateRangeGrid]
        [OrderGrid(Order = 3)]
        [DisplayName("Last Modified Date")]
        [Default]
        public string LastModifiedValue { get; set; }

        [IgnoreGrid]
        public DateTime? LastModified { get; set; }
    }
    public class LcmDeploymentStatusDto : GridDtoBase
    {
        [OrderGrid(Order = 1)]
        [DisplayName("ID")]
        [Default]
        public short Id { get; set; }

        [OrderGrid(Order = 2)]
        [DisplayName("Description")]
        [Default]
        public string Description { get; set; }
    }
}