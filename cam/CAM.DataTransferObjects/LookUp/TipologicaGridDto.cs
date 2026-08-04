using System;
using System.ComponentModel;
using CAM.DataAttributes.Grid;
using CAM.DataTransferObjects.QueryDto.Base;

namespace CAM.DataTransferObjects.LookUp
{
    public class TipologicaGridDto : GridDtoBase
    {
        [OrderGrid(Order = 1)]
        [Default]
        public short Id { get; set; }

        [OrderGrid(Order = 2)]
        [Default]
        public string Description { get; set; }

        [OrderGrid(Order = 3)]
        [DateRangeGrid]
        [Default]

        public DateTime? LastModified { get; set; }

        [OrderGrid(Order = 4)]
        [MailTo]
        [DisplayName("Last Modified By")]
        [Default]
        public string LastModifiedBy { get; set; }       


    }

    public class OperatingSystemDto : TipologicaGridDto
    {
        [OrderGrid(Order = 8)]
        [Default]
        public string OperatingSystemVersion { get; set; }
    }

    public class DriverDto : TipologicaGridDto
    {
        [OrderGrid(Order = 10)]
        [DisplayName("BPT Driver Details")]
        [Default]
        public string BptDriverDetails { get; set; }
    }
}
