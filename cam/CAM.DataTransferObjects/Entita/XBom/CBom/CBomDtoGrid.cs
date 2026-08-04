using CAM.DataAttributes.Grid;
using CAM.DataTransferObjects.Entita.XBom.VBom;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto.Base;
using System.Collections.Generic;
using System.ComponentModel;

namespace CAM.DataTransferObjects.Entita.XBom.CBom
{

    public class CnfClusterInfoDtoGrid : GridDtoBase
    {
        #region Cnf Cluster Info
        [OrderGrid(Order = 1)]
        [DisplayName("CnfClusterInfo Id")]
        [Default]
        public long CnfClusterInfoId { get; set; }

        [IgnoreGrid]
        public long CnfNameId { get; set; }

        [OrderGrid(Order = 2)]
        [Default]
        [DisplayName("Cnf Name")]
        public string CnfName { get; set; }

        [IgnoreGrid]
        public long CnfClusterId { get; set; }

       

        [OrderGrid(Order = 3)]
        [Default]
        [DisplayName("Cnf ClusterName")]
        public string CnfClusterName { get; set; }


        [IgnoreGrid]
        public long NodePoolId { get; set; }

        [OrderGrid(Order = 4)]
        [Default]
        [DisplayName("Node Pool")]
        public string NodePool { get; set; }

        [OrderGrid(Order = 5)]
        [Default]
        [DisplayName("Opco")]
        public string OpcoName { get; set; }

        [IgnoreGrid]
        public long OpcoId { get; set; }

        [OrderGrid(Order = 6)]
        [Default]
        [DisplayName("Location")]
        public string Location { get; set; }

        [OrderGrid(Order = 7)]
        [Default]
        [DisplayName("Site")]
        public string Site { get; set; }

        [IgnoreGrid]
        public long SiteId { get; set; }


        [OrderGrid(Order = 8)]
        [Default]
        [DisplayName("Hardware")]
        public string Hardware { get; set; }

       

        [OrderGrid(Order = 10)]
        [Default]
        [DisplayName("NodePool Breakup")]
        public string NodePoolBreakup { get; set; }


        [OrderGrid(Order = 11)]
        [Default]
        [DisplayName("Special Requirements")]
        public string SpecialRequirements { get; set; }



        [OrderGrid(Order = 12)]
        [Default]
        [DisplayName("Hyperthreading")]
        public string HyperThreading { get; set; }
        [OrderGrid(Order = 13)]
        [Default]
        [DisplayName("Over Provisioning")]
        public string OverProvisioning { get; set; }
        [OrderGrid(Order = 14)]
        [Default]
        [DisplayName("Worker Node Configuration")]
        public string? WorkerNodeConfiguration { get; set; }
         
        [OrderGrid(Order = 15)]
        [Default]
        [DisplayName("Cpu Kubelet")]
        public decimal? CpuKubelet { get; set; }

        [OrderGrid(Order = 16)]
        [Default]
        [DisplayName("Mem Kubelet")]
        public decimal? MemKubelet { get; set; }

        [OrderGrid(Order = 17)]
        [Default]
        [DisplayName("Cpu System")]
        public decimal? CpuSystem { get; set; }

        [OrderGrid(Order = 18)]
        [Default]
        [DisplayName("Mem System")]
        public decimal? MemSystem { get; set; }

        [OrderGrid(Order = 19)]
        [Default]
        [DisplayName("Vertical Domain Owner")]
        public string VerticalDomain { get; set; }

        [IgnoreGrid]
        public long VerticalDomainId { get; set; }

        [OrderGrid(Order = 20)]
        [Default]
        [DisplayName("Comments")]
        public string? Comments { get; set; }
        
        [OrderGrid(Order = 21)]
        [Default]
        [DisplayName("Note")]
        public string? Note { get; set; }

        [OrderGrid(Order = 22)]
        [DisplayName("Hardware Type")]
        [Default]
        public string HardwareType { get; set; }

        [IgnoreGrid]
        public long HardwareTypeId { get; set; }


        [OrderGrid(Order = 23)]
        [DisplayName("File Name")]
        [Default]
        public string FileName { get; set; }
        
        [OrderGrid(Order = 24)]
        [DisplayName("REVISION")]
        [Default]
        public string Revision { get; set; }

        [OrderGrid(Order = 25)]        
        [DisplayName("Aggregate Image Size")]
        public string AggregateImageClusterSize { get; set; } 
        #endregion
    }

    public class CnfCapacityDtoGrid
    {
        #region Cnf Capacity
        [OrderGrid(Order = 15)]
        [DisplayName("Cnf Capacity Id")]
        [Default]
        public long CnfCapacityId { get; set; }

        [OrderGrid(Order = 16)]
        [DisplayName("Financial Year")]
        [Default]
        public string FinancialYear { get; set; }

        [OrderGrid(Order = 17)]
        [DisplayName("Financial Version")]
        [Default]
        public string FinancialVersion { get; set; }


        [OrderGrid(Order = 18)]
        [DisplayName("No of CNF Instances Per Site")]
        [Default]
        public string NoOfCnfInstancesPersite { get; set; }

        [OrderGrid(Order = 19)]
        [DisplayName("VCPU Limit  For PodType")]
        [Default]
        public string VcpuLimitForPodType { get; set; }

        [OrderGrid(Order = 20)]
        [DisplayName("PCPU Request For PodType")]
        [Default]
        public string PcpuRequestForPodType { get; set; }

        [OrderGrid(Order = 21)]
        [DisplayName("Mem Request  For PodType")]
        [Default]
        public string MemRequestForPodType { get; set; }


        [OrderGrid(Order = 22)]
        [DisplayName("Mem Limit Pod Type")]
        [Default]
        public string MemLimitForPodType { get; set; }

        [OrderGrid(Order = 23)]
        [DisplayName("VCPU Request  For PodType")]
        [Default]
        public string VcpuRequestForPodType { get; set; }


        [OrderGrid(Order = 24)]
        [DisplayName("Non-Persistent Storage Pod Type")]
        [Default]
        public string NonPresistentStorageForProdType { get; set; }

        [OrderGrid(Order = 25)]
        [DisplayName("Persistent Volumes Required")]
        [Default]
        public string IsPresistentVolumesRequired { get; set; }

        [OrderGrid(Order = 26)]
        [DisplayName("Persistent Volume Access Mode")]
        [Default]
        public string PersistentVolumNeaccessMode { get; set; }


        [OrderGrid(Order = 27)]
        [DisplayName("Persistent Storage Pod Type")]
        [Default]
        public string PersistentStorageForPodType { get; set; }

        [OrderGrid(Order = 28)]
        [DisplayName("Storage IOPS Pod Type")]
        [Default]
        public string StorageIopsForPodType { get; set; }

        [OrderGrid(Order = 29)]
        [DisplayName("Storage RW Workload Distribution")]
        [Default]
        public string StoragerWorkloadDistribution { get; set; }
        [OrderGrid(Order = 30)]
        [DisplayName("North/South BandWidth PodType")]
        [Default]
        public string NorthSouthBandWidthForPodType { get; set; }
        [OrderGrid(Order = 31)]
        [DisplayName("East/West BandWidth PodType")]
        [Default]
        public string EastWestBandWidthForPodType { get; set; }

        [OrderGrid(Order = 32)]
        [DisplayName("Special Requirement Per PodType")]
        [Default]
        public string SpecialRequirementPerPodType { get; set; }

        [OrderGrid(Order = 33)]
        [DisplayName("SpecialRequirement")]
        [Default]
        public string ListOfCapacitySpecialRequirement { get; set; }

        [OrderGrid(Order = 34)]
        [Default]
        [DisplayName("No of Pod Per Site")]
        public string NumberOfPodsPerPodType { get; set; }


        [OrderGrid(Order = 35)]
        [Default]
        [DisplayName("No of Cnf Instances Per Site")]
        public string NoOfCnfInstancesPerSite { get; set; }
        #endregion
    }
    public class CnfInfoAndCapacityDtoGrid
    {
        #region CNF Pod Info

        [OrderGrid(Order = 1)]
        [Default]
        [DisplayName("Cnf PodInfo Id")] 
        public long CnfPodInfoId { get; set; }

        [OrderGrid(Order = 2)]
        [Default]
        [DisplayName("Cnf ClusterInfo Id")]
        public long CnfClusterInfoId { get; set; }


        [IgnoreGrid]
        public long PodTypeInfoId { get; set; }


        [OrderGrid(Order = 3)]
        [Default]
        [DisplayName("Pod Type")]
        public string PodTypeName { get; set; }

        [IgnoreGrid]
        public long FunctionStandardId { get;set; }

        [OrderGrid(Order = 4)]
        [Default]
        [DisplayName("Function Standard Name")]
        public string FunctionStandardName { get; set; }

        [IgnoreGrid]
        public long PriorityId { get; set; }

        [OrderGrid(Order = 5)]
        [Default]
        [DisplayName("Priority")]
        public string PriorityName { get; set; }

        [IgnoreGrid]
        public long PodroleDescriptionId { get; set; }

        [OrderGrid(Order = 6)]
        [Default]
        [DisplayName("Pod Role Description")]
        public string PodroleDescription { get; set; }

       

        [OrderGrid(Order = 8)]
        [DisplayName("DaemonSet Pod")]
        [Default]
        public string DaemonSetPod { get; set; }

        [OrderGrid(Order = 9)]
        [DisplayName("Intra Pod Rules")]
        [Default]
        public string IntraPodRules { get; set; }

        [OrderGrid(Order = 10)]
        [DisplayName("Inter Pod Rules")]
        [Default]
        public string InterPodRules { get; set; }

        [OrderGrid(Order = 11)]
        [DisplayName("Enhanced HA")]
        [Default]
        public string IsEnhancedHa { get; set; }

        [OrderGrid(Order = 12)]
        [DisplayName("Pod Type Qos")]
        [Default]
        public string PodTypeQos { get; set; }

        [OrderGrid(Order = 13)]
        [DisplayName("Persistance Storage Flag")]
        [Default]
        public string IsPersistanceStorageFlag { get; set; }

        [OrderGrid(Order = 14)]
        [DisplayName("Pod HPA")]
        [Default]
        public string IsProdhPaEnable { get; set; }

        #endregion

        #region Cnf Capacity
        [OrderGrid(Order = 15)]
        [DisplayName("Cnf Capacity Id")]
        [Default]
        public long CnfCapacityId { get; set; }

        [OrderGrid(Order = 16)]
        [DisplayName("Financial Year")]
        [Default]
        public string FinancialYear { get; set; }

        [OrderGrid(Order = 17)]
        [DisplayName("Financial Version")]
        [Default]
        public string FinancialVersion{ get; set; }

        [OrderGrid(Order = 18)]
        [DisplayName("No of CNF Instances Per Site")]
        [Default]
        public string NoOfCnfInstancesPersite { get; set; }


        [OrderGrid(Order = 19)]
        [Default]
        [DisplayName("No of Pod Per Site")]
        public string NumberOfPodsPerPodType { get; set; }

        [OrderGrid(Order = 20)]
        [DisplayName("VCPU Limit  For PodType")]
        [Default]
        public string VcpuLimitForPodType { get; set; }

        [OrderGrid(Order = 21)]
        [DisplayName("PCPU Request For PodType")]
        [Default]
        public string PcpuRequestForPodType { get; set; }

        [OrderGrid(Order = 22)]
        [DisplayName("Mem Request  For PodType")]
        [Default]
        public string MemRequestForPodType { get; set; }


        [OrderGrid(Order = 23)]
        [DisplayName("Mem Limit Pod Type")]
        [Default]
        public string MemLimitForPodType { get; set; }

        [OrderGrid(Order = 24)]
        [DisplayName("VCPU Request  For PodType")]
        [Default]
        public string VcpuRequestForPodType { get; set; }


        [OrderGrid(Order = 25)]
        [DisplayName("Non-Persistent Storage Pod Type")]
        [Default]
        public string NonPresistentStorageForProdType { get; set; }

        [OrderGrid(Order = 26)]
        [DisplayName("Persistent Volumes Required")]
        [Default]
        public string IsPresistentVolumesRequired { get; set; }

        [OrderGrid(Order = 27)]
        [DisplayName("Persistent Volume Access Mode")]
        [Default]
        public string PersistentVolumNeaccessMode { get; set; }


        [OrderGrid(Order = 28)]
        [DisplayName("Persistent Storage Pod Type")]
        [Default]
        public string PersistentStorageForPodType { get; set; }

        [OrderGrid(Order = 29)]
        [DisplayName("Storage IOPS Pod Type")]
        [Default]
        public string StorageIopsForPodType { get; set; }

        [OrderGrid(Order = 30)]
        [DisplayName("Storage RW Workload Distribution")]
        [Default]
        public string StoragerWorkloadDistribution { get; set; }
        [OrderGrid(Order = 31)]
        [DisplayName("North/South BandWidth PodType")]
        [Default]
        public string NorthSouthBandWidthForPodType { get; set; }
        [OrderGrid(Order = 32)]
        [DisplayName("East/West BandWidth PodType")]
        [Default]
        public string EastWestBandWidthForPodType { get; set; }

        [OrderGrid(Order = 33)]
        [DisplayName("Special Requirement Per PodType")]
        [Default]
        public string SpecialRequirementPerPodType { get; set; }

        [OrderGrid(Order = 34)]
        [DisplayName("SpecialRequirement")]
        [Default]
        public string ListOfCapacitySpecialRequirement { get; set; }
         
        

        #endregion
        public List<CnfCapacityDtoGrid> _cnfCapacityDtoGrid { get; set; }
    }

    public class CnfInfoAndCbomExportGridDto
    {
        public List<CbomExportGridDto> cbomExport { get; set; }
        public List<CnfInfoSheetExportGridDto> cnfInfoExport { get; set; }
    }

    public class CbomExportGridDto : GridDtoBase
    {
       

        #region Cnf Cluster Info
        [OrderGrid(Order = 1)]
        [DisplayName("CnfClusterInfo Id")]
        [Default]
        public long CnfClusterInfoId { get; set; }

        [IgnoreGrid]
        public long CnfNameId { get; set; }

        [OrderGrid(Order = 2)]
        [Default]
        [DisplayName("Cnf Name")]
        public string CnfName { get; set; }

        [IgnoreGrid]
        public long CnfClusterId { get; set; }

        [OrderGrid(Order = 3)]
        [Default]
        [DisplayName("Cnf ClusterName")]
        public string CnfClusterName { get; set; }


        [IgnoreGrid]
        public long NodePoolId { get; set; }

        [OrderGrid(Order = 4)]
        [Default]
        [DisplayName("Node Pool")]
        public string NodePool { get; set; }

        [OrderGrid(Order = 5)]
        [Default]
        [DisplayName("Opco")]
        public string OpcoName { get; set; }

        [IgnoreGrid]
        public long OpcoId { get; set; }

        [OrderGrid(Order = 6)]
        [Default]
        [DisplayName("Location")]
        public string Location { get; set; }

        [OrderGrid(Order = 7)]
        [Default]
        [DisplayName("Site")]
        public string Site { get; set; }

        [IgnoreGrid]
        public long SiteId { get; set; }


        [OrderGrid(Order = 8)]
        [Default]
        [DisplayName("Hardware")]
        public string Hardware { get; set; }

       
        [OrderGrid(Order = 9)]
        [Default]
        [DisplayName("NoOfCnfInstancesPerSite")]
        public string NoOfCnfInstancesPerSite { get; set; }

        [OrderGrid(Order = 10)]
        [Default]
        [DisplayName("NodePool Breakup")]
        public string NodePoolBreakup { get; set; }


        [OrderGrid(Order = 11)]
        [Default]
        [DisplayName("Special Requirements")]
        public string SpecialRequirements { get; set; }



        [OrderGrid(Order = 12)]
        [Default]
        [DisplayName("Hyperthreading")]
        public string HyperThreading { get; set; }
        [OrderGrid(Order = 13)]
        [Default]
        [DisplayName("Over Provisioning")]
        public string OverProvisioning { get; set; }
        [OrderGrid(Order = 14)]
        [Default]
        [DisplayName("Worker Node Configuration")]
        public string? WorkerNodeConfiguration { get; set; }

        [OrderGrid(Order = 15)]
        [Default]
        [DisplayName("Cpu Kubelet")]
        public decimal? CpuKubelet { get; set; }

        [OrderGrid(Order = 16)]
        [Default]
        [DisplayName("Mem Kubelet")]
        public decimal? MemKubelet { get; set; }

        [OrderGrid(Order = 17)]
        [Default]
        [DisplayName("Cpu System")]
        public decimal? CpuSystem { get; set; }

        [OrderGrid(Order = 18)]
        [Default]
        [DisplayName("Mem System")]
        public decimal? MemSystem { get; set; }

        [OrderGrid(Order = 19)]
        [Default]
        [DisplayName("Vertical Domain Owner")]
        public string VerticalDomain { get; set; }

        [IgnoreGrid]
        public long VerticalDomainId { get; set; }

        [OrderGrid(Order = 20)]
        [Default]
        [DisplayName("Comments")]
        public string? Comments { get; set; }

        [OrderGrid(Order = 21)]
        [Default]
        [DisplayName("Note")]
        public string? Note { get; set; }
       

        [OrderGrid(Order = 22)]
        [DisplayName("Hardware Type")]
        [Default]
        public string? HardwareType { get; set; }

        [IgnoreGrid]
        public long HardwareTypeId { get; set; }


        [OrderGrid(Order = 23)]
        [DisplayName("File Name")]
        [Default]
        public string? FileName { get; set; }

        [OrderGrid(Order = 24)]
        [DisplayName("REVISION")]
        [Default]
        public string Revision { get; set; }

        [OrderGrid(Order = 25)]
        [IgnoreGrid]
        [DisplayName("Aggregate Image Size")]
        public string AggregateImageClusterSize { get; set; }
        #endregion

        #region CNF Pod Info

        [OrderGrid(Order = 26)]
        [Default]
        [DisplayName("Cnf PodInfo Id")]
        public long CnfPodInfoId { get; set; }
 
        [IgnoreGrid]
        public long PodTypeInfoId { get; set; }


        [OrderGrid(Order = 27)]
        [Default]
        [DisplayName("Pod Type")]
        public string PodTypeName { get; set; }

        [IgnoreGrid]
        public long FunctionStandardId { get; set; }

        [OrderGrid(Order = 28)]
        [Default]
        [DisplayName("Function Standard Name")]
        public string FunctionStandardName { get; set; }

        [IgnoreGrid]
        public long PriorityId { get; set; }

        [OrderGrid(Order = 29)]
        [Default]
        [DisplayName("Priority")]
        public string PriorityName { get; set; }

        [IgnoreGrid]
        public long PodroleDescriptionId { get; set; }

        [OrderGrid(Order = 30)]
        [Default]
        [DisplayName("Pod Role Description")]
        public string PodroleDescription { get; set; }

        [OrderGrid(Order = 31)]
        [Default]
        [DisplayName("No of Pod Per Site")]
        public string NumberOfPodsPerPodType { get; set; }

        [OrderGrid(Order = 32)]
        [DisplayName("DaemonSet Pod")]
        [Default]
        public string DaemonSetPod { get; set; }

        [OrderGrid(Order = 33)]
        [DisplayName("Intra Pod Rules")]
        [Default]
        public string IntraPodRules { get; set; }

        [OrderGrid(Order = 34)]
        [DisplayName("Inter Pod Rules")]
        [Default]
        public string InterPodRules { get; set; }

        [OrderGrid(Order = 35)]
        [DisplayName("Enhanced HA")]
        [Default]
        public string IsEnhancedHa { get; set; }

        [OrderGrid(Order = 36)]
        [DisplayName("Pod Type Qos")]
        [Default]
        public string PodTypeQos { get; set; }

        [OrderGrid(Order = 37)]
        [DisplayName("Persistance Storage Flag")]
        [Default]
        public string IsPersistanceStorageFlag { get; set; }

        [OrderGrid(Order = 38)]
        [DisplayName("Pod HPA")]
        [Default]
        public string IsProdhPaEnable { get; set; }

        #endregion

        #region Cnf Capacity
        [OrderGrid(Order = 39)]
        [DisplayName("Cnf Capacity Id")]
        [Default]
        public long CnfCapacityId { get; set; }

        [OrderGrid(Order = 40)]
        [DisplayName("Financial Year")]
        [Default]
        public string FinancialYear { get; set; }

        [OrderGrid(Order = 41)]
        [DisplayName("Financial Version")]
        [Default]
        public string FinancialVersion { get; set; }
         
        [OrderGrid(Order = 42)]
        [DisplayName("VCPU Limit  For PodType")]
        [Default]
        public string VcpuLimitForPodType { get; set; }

        [OrderGrid(Order = 43)]
        [DisplayName("PCPU Request For PodType")]
        [Default]
        public string PcpuRequestForPodType { get; set; }

        [OrderGrid(Order = 44)]
        [DisplayName("Mem Request  For PodType")]
        [Default]
        public string MemRequestForPodType { get; set; }


        [OrderGrid(Order = 45)]
        [DisplayName("Mem Limit Pod Type")]
        [Default]
        public string MemLimitForPodType { get; set; }

        [OrderGrid(Order = 46)]
        [DisplayName("VCPU Request  For PodType")]
        [Default]
        public string VcpuRequestForPodType { get; set; }


        [OrderGrid(Order = 47)]
        [DisplayName("Non-Persistent Storage Pod Type")]
        [Default]
        public string NonPresistentStorageForProdType { get; set; }

        [OrderGrid(Order = 48)]
        [DisplayName("Persistent Volumes Required")]
        [Default]
        public string IsPresistentVolumesRequired { get; set; }

        [OrderGrid(Order = 49)]
        [DisplayName("Persistent Volume Access Mode")]
        [Default]
        public string PersistentVolumNeaccessMode { get; set; }


        [OrderGrid(Order = 50)]
        [DisplayName("Persistent Storage Pod Type")]
        [Default]
        public string PersistentStorageForPodType { get; set; }

        [OrderGrid(Order = 51)]
        [DisplayName("Storage IOPS Pod Type")]
        [Default]
        public string StorageIopsForPodType { get; set; }

        [OrderGrid(Order = 52)]
        [DisplayName("Storage RW Workload Distribution")]
        [Default]
        public string StoragerWorkloadDistribution { get; set; }
        [OrderGrid(Order = 53)]
        [DisplayName("North/South BandWidth PodType")]
        [Default]
        public string NorthSouthBandWidthForPodType { get; set; }
        [OrderGrid(Order = 54)]
        [DisplayName("East/West BandWidth PodType")]
        [Default]
        public string EastWestBandWidthForPodType { get; set; }

        [OrderGrid(Order = 55)]
        [DisplayName("Special Requirement Per PodType")]
        [Default]
        public string SpecialRequirementPerPodType { get; set; }

        [OrderGrid(Order = 56)]
        [DisplayName("SpecialRequirement")]
        [Default]
        public string ListOfCapacitySpecialRequirement { get; set; }



        #endregion

        [OrderGrid(Order = 57)]
        [DisplayName("Request Type")]
        [Default]
        public string RequestType { get; set; }

        [OrderGrid(Order = 58)]
        [DisplayName("K8ClusterName")]
        [Default]
        public string K8ClusterName { get; set; }

        public List<CnfCapacityDtoGrid> _cnfCapacityDtoGrid { get; set; }
    }

    public class CnfInfoSheetExportGridDto : GridDtoBase
    {


        #region Cnf Cluster Info

        [OrderGrid(Order = 1)]
        [DisplayName("Request Type")]
        [Default]
        public string RequestType { get; set; }

        [OrderGrid(Order = 2)]
        [DisplayName("K8ClusterName")]
        [Default]
        public string K8ClusterName { get; set; }
         
        [OrderGrid(Order =3)]
        [Default]
        [DisplayName("Node Pool")]
        public string NodePool { get; set; }

        [OrderGrid(Order =4)]
        [Default]
        [DisplayName("Comments")]
        public string Comments { get; set; }

        [OrderGrid(Order = 5)]
        [Default]
        [DisplayName("NodePool Breakup")]
        public string NodePoolBreakup { get; set; }

        [OrderGrid(Order = 6)]
        [Default]
        [DisplayName("Special Requirements")]
        public string SpecialRequirements { get; set; }


        [OrderGrid(Order =7)]
        [Default]
        [DisplayName("Hyperthreading")]
        public string HyperThreading { get; set; }

        [OrderGrid(Order = 8)]
        [Default]
        [DisplayName("Over Provisioning")]
        public string OverProvisioning { get; set; }
        [OrderGrid(Order = 9)]
        [Default]
        [DisplayName("Worker Node Configuration")]
        public string? WorkerNodeConfiguration { get; set; }

        [OrderGrid(Order = 10)]
        [Default]
        [DisplayName("Cpu Kubelet")]
        public decimal? CpuKubelet { get; set; }

        [OrderGrid(Order = 11)]
        [Default]
        [DisplayName("Mem Kubelet")]
        public decimal? MemKubelet { get; set; }

        [OrderGrid(Order = 12)]
        [Default]
        [DisplayName("Cpu System")]
        public decimal?  CpuSystem { get; set; }

        [OrderGrid(Order = 13)]
        [Default]
        [DisplayName("Mem System")]
        public decimal? MemSystem { get; set; }

        [OrderGrid(Order = 14)]
        [Default]
        [DisplayName("Notes")]
        public string? Note { get; set; }

        [OrderGrid(Order = 15)]
        [IgnoreGrid]
        [DisplayName("Aggregate Image Size")]
        public string AggregateImageClusterSize { get; set; }

        [OrderGrid(Order = 16)]
        [Default]
        [DisplayName("Hardware")]
        public string Hardware { get; set; }
        #endregion


    }

    public class CbomReportDto
    {
        [OrderGrid(Order = 1)]
        [DisplayName("Cnf Name")]
        [Default]
        public string CnfName { get; set; }
        [OrderGrid(Order = 2)]
        [DisplayName("PodType Name")]
        [Default]
        public string PodTypeName { get; set; }
        [OrderGrid(Order = 3)]
        [DisplayName("Number Of Cnf Instances PerSite")]
        [Default]
        public long NumberOfCnfInstancesPerSite { get; set; }
        [OrderGrid(Order = 4)]
        [DisplayName("Number Of Pods Per PodType")]
        [Default]
        public int? NumberOfPodsPerPodType { get; set; }
        [OrderGrid(Order = 5)]
        [DisplayName("vCPU Request For PodType")]
        [Default]
        public long VcpuRequestForPodType { get; set; }
        [OrderGrid(Order = 6)]
        [DisplayName("MEM Request For PodType")]
        [Default]
        public decimal? MemRequestForPodType { get; set; }
        [OrderGrid(Order = 7)]
        [DisplayName("Non-Persistent Storage Per PodType")]
        [Default]
        public string NonPersistentStoragePerPodType { get; set; }
        [OrderGrid(Order = 8)]
        [DisplayName("Hardware Type")]
        [Default]
        public string HardwareType { get; set; }
        public List<CbomDisaggregatedView> cbomDisaggregatedView { get; set; }
        public List<KeyValuePairDto> cnfNames { get; set; }
    }
    public class CbomDisaggregatedView
    {
        public string CnfName { get; set; }
        public string PodTypeName { get; set; }
        public int NumberOfCnfInstancesPerSite { get; set; }
        public int? NumberOfPodsPerPodType { get; set; }
        public decimal? MemRequestForPodType { get; set; }
        public string NonPersistentStoragePerPodType { get; set; }
    }
}
