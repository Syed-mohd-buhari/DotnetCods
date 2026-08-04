using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using CAM.DataAttributes.Export;
using CAM.DataAttributes.Grid;
using CAM.DataTransferObjects.QueryDto.Base;
using ClosedXML.Excel;

namespace CAM.DataTransferObjects.Entita.NetworkElementAsIs
{
    public class NetworkElementAsIsDto : GridDtoBase

    {
        [OrderGrid(Order = 2)]
        [Default]
        public string ElementDeploymentName { get; set; }

        [OrderGrid(Order = 3)]
        [Default]
        public string NodeType { get; set; }

        [OrderGrid(Order = 14)]
        public string SoftwareProductNumber { get; set; }

        [OrderGrid(Order = 20)]
        public string ElementManager { get; set; }

        [IgnoreGrid]
        public string PatchDetails { get; set; }

       [IgnoreGrid]
        public DateTime? SoftwareProductionDate { get; set; }

        [DateRangeGrid]
        [OrderGrid(Order = 15)]
        [Default]
        public string SoftwareProductionDateValue { get; set; }
        
        [IgnoreGrid]
        public DateTime? SoftwareInstallDate { get; set; }

        [DateRangeGrid]
        [OrderGrid(Order = 16)]
        [Default]
        public string SoftwareInstallDateValue { get; set; }

        [IgnoreGrid]
        public DateTime? DataAcquisitionDate { get; set; }

        [DateRangeGrid]
        [OrderGrid(Order = 18)]
        public string DataAcquisitionDateValue { get; set; }


        [OrderGrid(Order = 19)]
        public string DataAcquisitionMethod { get; set; }

        [OrderGrid(Order = 17)]
        public string HardwareAcquisition { get; set; }

        [OrderGrid(Order = 9)]
        public bool ManualOverride { get; set; }

        [OrderGrid(Order = 20)]
        public string ElementManagerExportFileFormat { get; set; }

        [OrderGrid(Order = 6)]
        public string NetworkFunction { get; set; }

        [OrderGrid(Order = 7)]
        [Default]
        public string HardwareSolution { get; set; }

        [OrderGrid(Order = 10)]
        public string Platform { get; set; }

        [OrderGrid(Order = 11)]
        public string HardwareType { get; set; }

        [OrderGrid(Order = 13)]
        [FormatClosetXml(Type = XLDataType.Text)]
        [Format(FormatType = "Text")]
        [Default]
        public string SoftwareReleaseInformation { get; set; }

        [OrderGrid(Order = 12)]
        public string OtherHardwareInfo { get; set; }

        [IgnoreGrid]
        public DateTime? LastModified { get; set; }

        [DateRangeGrid]
        [OrderGrid(Order = 22)]
        public string LastModifiedValue { get; set; }

        [OrderGrid(Order = 23)]
        [MailTo]
        public string LastModifiedBy { get; set; }

        [DateRangeGrid]
        [OrderGrid(Order = 8)]
        public DateTime? HardwareInstallDate { get; set; }

        [OrderGrid(Order = 24)]
        [DisplayName("Vertical Name")]
        public string VerticalName { get; set; }
    }

    public class NetworkElementAsIsDtoGrid : NetworkElementAsIsDto
    {
        [OrderGrid(Order = 4)]
        public long NetworkElementAsIsId { get; set; }

        [OrderGrid(Order = 1)]
        [Default]
        public string OpCo { get; set; }

        [IgnoreGrid]
        public string SystemType { get; set; }

        [OrderGrid(Order = 5)]
        [Default]
        public string Location { get; set; }
    }
}
