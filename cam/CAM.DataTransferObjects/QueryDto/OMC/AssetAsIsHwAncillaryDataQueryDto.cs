using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto.Base;
using System;
using System.Collections.Generic;

namespace CAM.DataTransferObjects.QueryDto.OMC
{
    public class AssetAsIsHwAncillaryDataQueryDto:QueryObject
    {
        public List<long> Assetasishwancillarydataid { get; set; }
        public List<long?> Networkelementasisid { get; set; }
        public List<string> Site { get; set; }
        public List<string> Host { get; set; }
        public List<string> Datasourcename { get; set; }
        public List<string> Provider { get; set; }
        public List<string> Providertype { get; set; }
        public List<string> Consumer { get; set; }
        public List<string> Consumertype { get; set; }
        public List<string> Consumerrole { get; set; }
        public List<string> Clustername { get; set; }
        public List<string> Partnumber { get; set; }
        public List<string> Systemtype { get; set; }
        public List<string> Manufacturer { get; set; }
        public List<string> Biosversion { get; set; }
        public List<string> Model { get; set; }
        public List<string> Sku { get; set; }
        public List<int?> Cpucapacity { get; set; }
        public List<int?> Ephemeralstoragecapacity { get; set; }
        public List<int?> Memorycapacity { get; set; }
        public List<string> Processorsummarymodel { get; set; }
        public List<string> Kubernetesnodetype { get; set; }
        public List<string> Managementip { get; set; }
        public List<string> Kubernetesnodename { get; set; }
        public List<string> Kubeletversion { get; set; }
        public List<string> Kubernetesnodeos { get; set; }
        public List<string> Kubernetesnoderesourcetype { get; set; }
        public List<string> Kubernetesnodestate { get; set; }
        public DateFilter? Kubernetesnodestatusupdatetime { get; set; }
        public List<string> Chassisdetails { get; set; }
    }
}
