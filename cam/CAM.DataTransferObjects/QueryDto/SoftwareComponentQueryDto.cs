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
    public class SoftwareComponentQueryDto : QueryObject
    {
        public List<long> Softwarecomponentid { get; set; }
        public List<long> Networkelementid { get; set; }
        public List<string> Oem { get; set; }
        public List<string> Elementname { get; set; }
        public List<string> Componentname { get; set; }
        public DateFilter Productiondate { get; set; }
        public List<string> Productionnumber { get; set; }
        public List<string> Productionrevision { get; set; }
        public List<string> Componentcreationuser { get; set; }
        public DateFilter Componentcreationdate { get; set; }
        public List<string> Componentmodificationuser { get; set; }
        public DateFilter Componentmodificationdate { get; set; }
        public List<long> ComponentId { get; set; }
        public List<string> Opco { get; set; }
        public List<string> Mainsoftwareversion { get; set; }
        public DateFilter Creationdate { get; set; }
        public List<string> Creationuser { get; set; }
        public List<string> Modificationuser { get; set; }
        public DateFilter Modificationdate { get; set; }
    }
}
