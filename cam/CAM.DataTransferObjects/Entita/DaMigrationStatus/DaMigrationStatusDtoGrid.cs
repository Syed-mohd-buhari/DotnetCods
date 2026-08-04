using CAM.DataAttributes.Grid;
using CAM.DataTransferObjects.QueryDto.Base;
using System.Collections.Generic;
using System.ComponentModel;

namespace CAM.DataTransferObjects.Entita.DaMigrationStatus
{
    public   class DaMigrationStatusDtoGrid : GridDtoBase
    {
        [DisplayName("DaMigrationStatusId")]
        [OrderGrid(Order = 1)]
        [Default]
        public long DaMigrationStatusId { get; set; }


        [DisplayName("Opco")]
        [OrderGrid(Order = 2)]
        [Default]
        public string Opco { get; set; }
        [IgnoreGrid]
        public short OpcoId { get; set; }

        [DisplayName("Location")]
        [OrderGrid(Order = 3)]
        [Default]
        public string Location { get; set; }
        [IgnoreGrid]
        public short LocationId { get; set; }

        [DisplayName("Status")]
        [OrderGrid(Order = 4)]
        [Default]
        public string Status { get; set; }

        [IgnoreGrid]
        public short StatusId { get; set; }

        [IgnoreGrid]
        public Dictionary<int, string> MigratonStatus { get; set; }

        [IgnoreGrid]
        public long PlannedActivityId { get; set; }
    }
}
