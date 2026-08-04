using CAM.DataAttributes.Grid;
using CAM.DataTransferObjects.QueryDto.Base;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAM.DataTransferObjects.Entita.Idenitty
{
    public class HardwareConfigurationDto
    {
        [OrderGrid(Order =2)]
        [Default]
        public string Oem { get; set; } 
        [OrderGrid(Order = 3)]
        [Default]
        public string ElementName { get; set; }

        [OrderGrid(Order = 6)]
        public string ProductName { get; set; }

        [OrderGrid(Order = 7)]
        [Default]
        public string SerialNumber { get; set; }

        [OrderGrid(Order = 8)]
        [Default]
        public string UnitLocation { get; set; }

        [OrderGrid(Order = 9)]
        public string Vendor { get; set; }
        [OrderGrid(Order = 10)]
        public string ProductNumber { get; set; }
        [OrderGrid(Order = 11)]
        public string Revision { get; set; }

        [OrderGrid(Order = 12)]
        public string HardwareType { get; set; }
        [OrderGrid(Order = 13)]
        [MailTo]
        public string CreationUser { get; set; }
        [OrderGrid(Order = 14)]
        [DateRangeGrid]
        public DateTime CreationDate { get; set; }
        [OrderGrid(Order = 15)]
        [MailTo]
        public string ModificationUser { get; set; }
        [OrderGrid(Order = 16)]
        [DateRangeGrid]
        public DateTime ModificationDate { get; set; }

    }
    public class HardwareConfigurationDtoGrid : HardwareConfigurationDto
    {
        [OrderGrid(Order = 4)]
        public long HardwareConfigurationId { get; set; }

        [OrderGrid(Order = 5)]
        public long NetworkElementId { get; set; }

        [OrderGrid(Order = 1)]
        [Default]
        public string OpCo { get; set; }
        
    }
}
