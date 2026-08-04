using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto.Base;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAM.DataTransferObjects.QueryDto
{
    public class SoftwareConfigurationQueryDto : QueryObject
    {
        public List<decimal> SoftwareConfigurationId { get; set; }
        public List<decimal?> NetworkElementId { get; set; }
        public List<decimal> FunctionId { get; set; }
        public List<decimal> FunctionAreaId { get; set; }
        public List<decimal> SubFunctionId { get; set; }
        public List<decimal> SubFunctionAreaId { get; set; }
        public List<string> Opco { get; set; }
        public List<string> Oem { get; set; }
        public List<string> Elementname { get; set; }
        public List<string> FunctionName { get; set; }
        public List<string> FunctionAreaName { get; set; }
        public List<string> SubFunctionName { get; set; }
        public List<string> SubFunctionAreaName { get; set; }
        public List<int> Creationuser { get; set; }
        public DateFilter Creationdate { get; set; }
        public List<int?> Modificationuser { get; set; }
        public DateFilter Modificationdate { get; set; }
    }
}
