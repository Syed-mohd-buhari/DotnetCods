using CAM.DataAttributes.Grid;
using CAM.DataTransferObjects.QueryDto.Base;
using System.ComponentModel;

namespace CAM.DataTransferObjects.Entita.UserDefinedReportLog
{
    public class UserDefinedReportLogDto : GridDtoBase
    {
        //[OrderGrid(Order = 1)]
        //[Default]
        //public long UserDefinedReportsLogId { get; set; }

        [OrderGrid(Order = 2)]
        [Default]
        public string ReportName { get; set; }

        [OrderGrid(Order = 3)]
        [Default]
        public string ReportFormat { get; set; }

        [OrderGrid(Order = 4)]
        public string ReportDownloadedPath { get; set; }

        [OrderGrid(Order = 6)]

        public string ReportStatus { get; set; }

        [OrderGrid(Order = 7)]
        [DisplayName("Last Modified By")]
        public  string LastModifiedBy { get; set; }

        //[OrderGrid(Order = 8)]
        //[DateRangeGrid]
        //[DisplayName("Last Modified")]
        //public virtual string LastModifiedValue { get; set; }



    }

}
