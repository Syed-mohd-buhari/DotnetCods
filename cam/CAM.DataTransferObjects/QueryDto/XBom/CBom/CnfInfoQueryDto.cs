using CAM.DataTransferObjects.QueryDto.Base;
using System.Collections.Generic;

namespace CAM.DataTransferObjects.QueryDto.XBom.CBom
{
    public class CnfCapcityQueryDto : QueryObject
    {

        public List<long> Cnfpodinfoid { get; set; }
        public List<long> CnfCapacityId { get; set; }
        public List<long> FinancialYear { get; set; }
        public List<int> Financialversion { get; set; }


        public List<int> NoOfCnfInstancesPersite { get; set; }
        public List<int> NumberOfPodsPerPodType { get; set; }
        public List<long> VcpuRequestForPodType { get; set; }
        public List<long> VcpuLimitForPodType { get; set; }

        public List<string> MemRequestForPodType { get; set; }
        public List<string> MemLimitForPodType { get; set; }

        public List<string> NonPresistentStorageForProdType { get; set; }
        public List<string> IsPresistentVolumesRequired { get; set; }
        public List<string> PersistentVolumNeaccessMode { get; set; }

        public List<string> PersistentStorageForPodType { get; set; }
        public List<string> StorageIopsForPodType { get; set; }
        public List<string> StoragerWorkloadDistribution { get; set; }
        public List<string> NorthSouthBandWidthForPodType { get; set; }

        public List<string> EastWestBandWidthForPodType { get; set; }

        public List<string> SpecialRequirementPerPodType { get; set; }
        public List<string> ListOfCapacitySpecialRequirement { get; set; } 
    }
        public class CnfClusterInfoQueryDtos : CnfCapcityQueryDto
    {
        public List<long> CnfClusterInfoId { get; set; }
        public List<string> CnfClusterNameDescription { get; set; }
        public List<string> NodePool { get; set; }
       
        public List<string> PodTypeInfoName { get; set; }

        public List<string> OpcoName { get; set; }
        public List<string> ShortLocation { get; set; }
        public List<string> LocationName { get; set; }

        public List<string> FunctionStandardName { get; set; }
        public List<string> PriorityId { get; set; }
        public List<string> PodRoleDescription { get; set; }
        public List<string> DaemonSetPod { get; set; }
       
        public List<string> IntraPodRules { get; set; }

        public List<string> InterPodRules { get; set; }

        public List<bool> IsEnhancedHa { get; set; }
        public List<string> PodTypeQos { get; set; }

        public List<string> IsPersistanceStorageFlag { get; set; }
        public List<string> IsProdhPaEnable { get; set; } 

    }

    public class CnfInfoQueryDto : CnfClusterInfoQueryDto
    {
        public List<long> CnfInfoId { get; set; }
        public List<string> K8CnfNameDescritpion { get; set; }

        public List<string> NodePoolBreakUp { get; set; }
        public List<string> SpecialRequirements { get; set; }
        public List<string> HyperThreading { get; set; }

        public List<string> OverProvisioning { get; set; }
        public List<string> WorkerNodeConfiguration { get; set; }
        public List<string> Hardware { get; set; }
        public List<decimal> CpuKubelet { get; set; }

        public List<int> MemKubelet { get; set; }
        public List<decimal> CpuSystem { get; set; }
        public List<int> MemSystem { get; set; }
        public List<string> VerticalDomain { get; set; }
    }

    public class CnfClusterInfoQueryDto  : CnfCapcityQueryDto
    {
        #region Cnflcuster Info
        public List<long> Cnfclusterinfoid { get; set; }
        public List<long> Cnfnameid { get; set; }
        public List<long> Cnfclusterid { get; set; }
        public List<short> Opcoid { get; set; }
        public List<short> Siteid { get; set; }
        public List<long> Cnfhardwareid { get; set; }
        public List<int> Noofcnfinstancespersite { get; set; }
        public List<string> Nodepoolbreakup { get; set; }
        public List<long> nodePool { get; set; }
        public List<string> Specialrequirements { get; set; }
        public List<string> Hyperthreading { get; set; }
        public List<string> Overprovisioning { get; set; }
        public List<string> Workernodeconfiguration { get; set; }
        public List<string> Hardware { get; set; }
        public List<decimal> Cpukubelet { get; set; }
        public List<int> Memkubelet { get; set; }
        public List<decimal> Cpusystem { get; set; }
        public List<int> Memsystem { get; set; }
        public List<int> Verticalresponsibleid { get; set; }
        public List<string> Comments { get; set; }
        public List<string> Note { get; set; }
        public List<string> Filename { get; set; }
        public List<string> Revision { get; set; }
        public List<string> Aggregateimageclustersize { get; set; }

        public List<string> opcoName { get; set; }
        public List<string> Site { get; set; }
        public List<string> Location { get; set; }

        public List<string> CnfClusterName { get; set; }
        public List<string> cnfName { get; set; }
        public List<string> CnfhardwareDescription { get; set; }
        public List<string> verticalDomain { get; set; }

        #endregion

        #region Cnf Pod

        public List<long> CnfPodInfoId { get; set; }      
    public List<string> PodTypeName { get; set; }
    public List<string> FunctionStandardName { get; set; }
    public List<string> PriorityName { get; set; }
    public List<string> PodroleDescription { get; set; }
    public List<string> DaemonSetPod { get; set; }
    public List<string> IntraPodRules { get; set; }
    public List<string> InterPodRules { get; set; }
    public List<string> IsEnhancedHa { get; set; }
    public List<string> PodTypeQos { get; set; }
    public List<string> IsPersistanceStorageFlag { get; set; }
    public List<string> IsProdhPaEnable { get; set; }

    #endregion
    }
    public class CbomReportQueryDto
    {
        public List<string> OpcoName { get; set; }
        public List<long> HardwareType { get; set; }
        public List<short> FinancialYear { get; set; }
        public List<long> CnfName { get; set; }
    }
}
