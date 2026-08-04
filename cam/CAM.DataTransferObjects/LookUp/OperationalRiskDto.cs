using System;
using System.ComponentModel;
using CAM.DataAttributes.Grid;
using CAM.DataTransferObjects.QueryDto.Base;

namespace CAM.DataTransferObjects.LookUp
{
    public class OperationalRiskDto :GridDtoBase
    {
        [OrderGrid(Order = 1)]
        [Default]
        public short Id { get; set; }

        [OrderGrid(Order = 2)]
        [Default]
        public string Description { get; set; }


        [OrderGrid(Order = 3)]
        [Default]
        public int Severity { get; set; }

        [OrderGrid(Order = 4)]
        [DateRangeGrid]
        [Default]
        public DateTime? LastModified { get; set; }

        [OrderGrid(Order = 5)]
        [MailTo]
        [DisplayName("Last Modified By")]
        [Default]
        public string LastModifiedBy { get; set; }
    }
}