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
    public class SubFunctionAreaQueryDto:QueryObject
    {
        public List<long> SoftwareConfigurationId { get; set; }
        public List<long> NetworkElementId { get; set; }
        public List<long> FunctionId { get; set; }
        public List<long> FunctionAreaId { get; set; }
        public List<long> SubFunctionId { get; set; }
        public List<long> SubFunctionAreaId { get; set; }
        public List<string> Opco { get; set; }
        public List<string> Oem { get; set; }
        public List<string> Elementname { get; set; }
        public List<string> FunctionName { get; set; }
        public List<string> FunctionAreaName { get; set; }
        public List<string> SubFunctionName { get; set; }
        public List<string> SubFunctionAreaName { get; set; }
        public List<string> Creationuser { get; set; }
        public DateFilter Creationdate { get; set; }
        public List<string> Modificationuser { get; set; }
        public DateFilter Modificationdate { get; set; }
   
    }
}
