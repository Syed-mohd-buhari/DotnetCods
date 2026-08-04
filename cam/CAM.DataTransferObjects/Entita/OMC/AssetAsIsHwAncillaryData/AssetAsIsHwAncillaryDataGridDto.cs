using CAM.DataAttributes.Grid;
using CAM.DataTransferObjects.QueryDto.Base;
using System;
using System.ComponentModel;

namespace CAM.DataTransferObjects.Entita.OMC.AssetAsIsHwAncillaryData
{
    public class AssetAsIsHwAncillaryDataGridDto:GridDtoBase
    {
       
        [Default]
        [DisplayName("Site")]
        [OrderGrid(Order =1)]
        public string Site { get; set; }
        [Default]
        [DisplayName("Host")]
        [OrderGrid(Order = 2)]
        public string Host { get; set; }
        [Default]
        [DisplayName("Data Source Name")]
        [OrderGrid(Order = 3)]
        public string Datasourcename { get; set; }
        [Default]
        [DisplayName("Provider")]
        [OrderGrid(Order = 4)]
        public string Provider { get; set; }
        [Default]
        [DisplayName("Provider Type")]
        [OrderGrid(Order = 5)]
        public string Providertype { get; set; }
        [Default]
        [DisplayName("Consumer")]
        [OrderGrid(Order = 6)]
        public string Consumer { get; set; }
        [Default]
        [DisplayName("Consumer Type")]
        [OrderGrid(Order = 7)]
        public string Consumertype { get; set; }
        [Default]
        [DisplayName("Consumer Role")]
        [OrderGrid(Order = 8)]
        public string Consumerrole { get; set; }
        [Default]
        [DisplayName("Cluster Name")]
        [OrderGrid(Order = 9)]
        public string Clustername { get; set; }
        [Default]
        [DisplayName("Part Number")]
        [OrderGrid(Order = 10)]
        public string Partnumber { get; set; }
        [Default]
        [DisplayName("System Type")]
        [OrderGrid(Order = 11)]
        public string Systemtype { get; set; }
        [Default]
        [DisplayName("Manufacturer")]
        [OrderGrid(Order = 12)]
        public string Manufacturer { get; set; }
        [Default]
        [DisplayName("Bios Version")]
        [OrderGrid(Order = 13)]
        public string Biosversion { get; set; }
        [Default]
        [DisplayName("Model")]
        [OrderGrid(Order = 14)]
        public string Model { get; set; }
        [Default]
        [DisplayName("Sku")]
        [OrderGrid(Order = 15)]
        public string Sku { get; set; }
        [Default]
        [DisplayName("Cpu Capacity")]
        [OrderGrid(Order = 16)]
        public int? Cpucapacity { get; set; }
        [Default]
        [DisplayName("Ephemeral Storage Capacity")]
        [OrderGrid(Order = 17)]
        public int? Ephemeralstoragecapacity { get; set; }
        [Default]
        [DisplayName("Memory Capacity")]
        [OrderGrid(Order = 18)]
        public int? Memorycapacity { get; set; }
        [Default]
        [DisplayName("Processor Summary Model")]
        [OrderGrid(Order = 19)]
        public string Processorsummarymodel { get; set; }
        [Default]
        [DisplayName("Kubernetes Node Type")]
        [OrderGrid(Order = 20)]
        public string Kubernetesnodetype { get; set; }
        [Default]
        [DisplayName("Management Ip")]
        [OrderGrid(Order = 21)]
        public string Managementip { get; set; }
        [Default]
        [DisplayName("Kubernetes Node Name")]
        [OrderGrid(Order = 22)]
        public string Kubernetesnodename { get; set; }
        [Default]
        [DisplayName("Kubelet Version")]
        [OrderGrid(Order = 23)]
        public string Kubeletversion { get; set; }
        [Default]
        [DisplayName("Kubernetes Nodeos")]
        [OrderGrid(Order = 24)]
        public string Kubernetesnodeos { get; set; }
        [Default]
        [DisplayName("Kubernetes Node Resource Type")]
        [OrderGrid(Order = 25)]
        public string Kubernetesnoderesourcetype { get; set; }
        [Default]
        [DisplayName("Kubernetes Node State")]
        [OrderGrid(Order = 26)]
        public string Kubernetesnodestate { get; set; }
        [Default]
        [DateRangeGrid]
        [DisplayName("Kubernetes Node Status Update Time")]
        [OrderGrid(Order = 27)]
        public DateTime? Kubernetesnodestatusupdatetime { get; set; }
        [Default]
        [DisplayName("Chassis Details")]
        [OrderGrid(Order = 28)]
        public string Chassisdetails { get; set; }

        [DisplayName("Id Index")]
        [OrderGrid(Order = 29)]
        public long Assetasishwancillarydataid { get; set; }
        [DisplayName("Network Element Asis Id Index")]
        [OrderGrid(Order = 30)]
        public long? Networkelementasisid { get; set; }
    }
}
