using CAM.DataAttributes.Grid;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto.Base;
using DocumentFormat.OpenXml.Drawing.Charts;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAM.DataTransferObjects.QueryDto
{
    public class HardwareConfigurationQueryDto : QueryObject
    {
        public List<long> HardwareConfigurationid { get; set; }
        public List<long> NetworkElementId { get; set; }
        public List<string> Opco { get; set; }
        public List<string>Oem { get; set; }
        public List<string> Elementname { get; set; }        
        public List<string> Hardwaretype { get; set; }
        public List<string> Productname { get; set; }
        public List<string> Serialnumber { get; set; }
        public List<string> Unitlocation { get; set; }
        public List<string> Vendor { get; set; }
        public List<string> ProductNumber { get; set; }       
        public List<string> Revision { get; set; }
        public List<string> Creationuser { get; set; }
        public DateFilter Creationdate { get; set; }      
        public List<string> Modificationuser { get; set; }
        public DateFilter Modificationdate { get; set; }

    }
}
