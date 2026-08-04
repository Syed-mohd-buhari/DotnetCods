using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto.Base;
using System;
using System.Collections.Generic;

namespace CAM.DataTransferObjects.QueryDto
{
    public class NetworkElementQueryDto : QueryObject
    {
        public List<long> Networkelementid { get; set; }
        public List<string> Opco { get; set; }
        public List<string> Oem { get; set; }
        public List<string> Elementname { get; set; }
        public DateFilter Dataacquisitiondate { get; set; }
        public List<string> Nodetype { get; set; }
        public List<string> Platformtype { get; set; }
        public List<string> Sitelocation { get; set; }
        public DateFilter Softwareinstalldate { get; set; }
        public DateFilter Softwareinstalldateap { get; set; }
        public DateFilter Softwareinstalldatecp { get; set; }
        public DateFilter Softwareproductdate { get; set; }
        public DateFilter Softwareproductdateap { get; set; }
        public DateFilter Softwareproductdatecp { get; set; }
        public List<string> Softwareproductnumber { get; set; }
        public List<string> Softwareproductnumberap { get; set; }
        public List<string> Softwareproductnumbercp { get; set; }
        public List<string> Softwarereleaseinformation { get; set; }
        public List<string> Softwarereleaseinformationap { get; set; }
        public List<string> Softwarereleaseinformationcp { get; set; }
        public List<string> Spare1ossorenm { get; set; }
        public List<string> Spare2xmlversion { get; set; }
        public List<string> NodeTypeName { get; set; }
        public DateFilter Xmllastparsefiledate { get; set; }
        public List<string> Creationuser { get; set; }
        public DateFilter Creationdate { get; set; }
        public List<string> Modificationuser { get; set; }
        public DateFilter Modificationdate { get; set; }
    }
}