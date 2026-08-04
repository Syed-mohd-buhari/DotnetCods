using CAM.DataAttributes.Grid;
using CAM.DataTransferObjects.QueryDto.Base;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAM.DataTransferObjects.Entita.NetworkElement
{
    public class NetworkelementDto 
    {
        [OrderGrid(Order = 3)]
        [Default]
        public string ElementName { get; set; }
        [OrderGrid(Order = 5)]
        [Default]
        public string SoftwareProductNumber { get; set; }
        [OrderGrid(Order = 6)]
        [DateRangeGrid]
        public DateTime? SoftwareInstallDate { get; set; }
        [DateRangeGrid]
        [OrderGrid(Order = 7)]
        [Default]
        public DateTime? DataAcquisitionDate { get; set; }
        [OrderGrid(Order = 8)]
        [Default]
        public string SoftwareReleaseInformation { get; set; }
        [OrderGrid(Order = 9)]
        [DateRangeGrid]
        public DateTime? SoftwareInstallDateAP { get; set; }
        [OrderGrid(Order = 10)]
        [DateRangeGrid]
        public DateTime? SoftwareInstallDateCP { get; set; }
        [OrderGrid(Order = 11)]
        [DateRangeGrid]
        public DateTime? SoftwareProductDate { get; set; }
        [OrderGrid(Order = 12)]
        [DateRangeGrid]
        public DateTime? SoftwareProductDateAP { get; set; }
        [OrderGrid(Order = 13)]
        [DateRangeGrid]
        public DateTime? SoftwareProductDateCP { get; set; }
        [OrderGrid(Order = 14)]
        public string SoftwareProductNumberAP { get; set; }
        [OrderGrid(Order = 15)]
        public string SoftwareProductNumberCP { get; set; }
        [OrderGrid(Order = 16)]
        public string SoftwareReleaseInformationAP { get; set; }
        [OrderGrid(Order = 17)]
        public string SoftwareReleaseInformationCP { get; set; }
        [OrderGrid(Order = 18)]
        [DisplayName("Spare1ossorenm")]
        public string Spare1ossorenm { get; set; }
        [OrderGrid(Order = 19)]
        [DisplayName("Spare2XmlVersion")]
        public string Spare2XmlVersion { get; set; }

        [OrderGrid(Order = 20)]
        public string NodeType { get; set; }
        [OrderGrid(Order = 21)]
        public string NodeTypeName { get; set; }
        [OrderGrid(Order = 22)]
        public string PlatformType { get; set; }
        [OrderGrid(Order = 23)]
        public string SiteLocation { get; set; }              
        [OrderGrid(Order = 24)]
        [DateRangeGrid]
        public DateTime? XmlLastParseFileDate { get; set; }
        [OrderGrid(Order = 25)]
        [MailTo]
        public string CreationUser { get; set; }
        [OrderGrid(Order = 26)]
        [DateRangeGrid]
        public DateTime CreationDate { get; set; }
        [OrderGrid(Order = 27)]
        [MailTo]
        public string ModificationUser { get; set; }
        [OrderGrid(Order = 28)]
        [DateRangeGrid]
        public DateTime ModificationDate { get; set; }
        //[IgnoreGrid]
        //[OrderGrid(Order = 27)]
        //[DateRangeGrid]
        //public DateTime? LastModified { get; set; }

        //[DateRangeGrid]
        //[OrderGrid(Order = 26)]
        //public string LastModifiedValue { get; set; }

        //[OrderGrid(Order = 25)]
        //[MailTo]
        //public string LastModifiedBy { get; set; }
    }
    public class NetworkelementDtoGrid : NetworkelementDto
    {
        //[IgnoreGrid]
        [OrderGrid(Order =4)]
        public long NetworkElementId { get; set; }

        [OrderGrid(Order = 1)]
        [Default]
        public string OpCo { get; set; }

        [OrderGrid(Order = 2)]
        [Default]
        public string Oem { get; set; }

    }
}
