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
    public class ComponentQueryDto :QueryObject
    {
        public List<long> Componentid { get; set; }
        public List<long> softwarecomponentid { get; set; }
        public List<string> Opco { get; set; }
        public List<string> Oem { get; set; }
        public List<string> Elementname { get; set; }
        public List<long> NetworkElementId { get; set; }
        public List<string> Mainsoftwareversion { get; set; }
        public List<string> Componentname { get; set; }
        public List<string> Creationuser { get; set; }
        public DateFilter Creationdate { get; set; }
        public List<string> Modificationuser { get; set; }
        public DateFilter Modificationdate { get; set; }
        public DateFilter Productiondate { get; set; }
        public List<string> Productionnumber { get; set; }
        public List<string> ProductionRevision { get; set; }
        public List<string> ComponentCreationuser { get; set; }
        public DateFilter ComponentCreationdate { get; set; }
        public List<string> ComponentModificationuser { get; set; }
        public DateFilter ComponentModificationdate { get; set; }
    }
}
