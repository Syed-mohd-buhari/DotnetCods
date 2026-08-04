using CAM.DataAttributes.Grid;
using CAM.DataTransferObjects.QueryDto.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAM.DataTransferObjects.Entita.TsrLog
{
    public class TsrLogDtoGrid : GridDtoBase
    {
        [IgnoreGrid]
        [OrderGrid(Order = 1)]
        [DisplayName("TsrLog Id")]
        public decimal TsrLogId { get; set; }

        [OrderGrid(Order = 2)]
        [DisplayName("Type Of Operation")]
        public string TypeOfOperation { get; set; }

        [OrderGrid(Order = 3)]
        [DisplayName("Domain")]
        public string Domain { get; set; }

        [OrderGrid(Order = 4)]
        [DisplayName("Batch Identifier")]
        public string BatchIdentifier { get; set; }

        [OrderGrid(Order = 5)]
        [DisplayName("File Name")]
        public string FileName { get; set; }

        [OrderGrid(Order = 6)]
        [DisplayName("Total Record")]
        public long TotalRecord { get; set; }

        [OrderGrid(Order = 7)]
        [DisplayName("Processed Record")]
        public long? ProcessedRecord { get; set; }

        [OrderGrid(Order = 8)]
        [DisplayName("Start Time")]
        [DateRangeGrid]
        public DateTime StartTime { get; set; }

        [OrderGrid(Order = 9)]
        [DisplayName("End Time")]
        [DateRangeGrid]
        public DateTime? EndTime { get; set; }

        [OrderGrid(Order = 10)]
        [DisplayName("Status")]
        public string Status { get; set; }

        [IgnoreGrid]
        public bool IsRefreshCompleted { get; set; }

        [IgnoreGrid]
        public Dictionary<short, string> opCoList { get;set;}
        [IgnoreGrid]
        public Dictionary<short, DateTime> opcoWiseLastModifiedDate { get; set; }

    }

    public class TsrLogFntDto
    {
        public long OpcoId { get; set; }
        public string Opco {  get; set; }
        public DateTime? EndTime { get; set; }
        public string Status { get; set; }

    }
}
