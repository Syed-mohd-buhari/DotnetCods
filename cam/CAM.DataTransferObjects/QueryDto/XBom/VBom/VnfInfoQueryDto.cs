using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto.Base;
using System;
using System.Collections.Generic;

namespace CAM.DataTransferObjects.QueryDto.XBom.VBom
{
    public class VnfVmCapacityQueryDto : QueryObject
    {
        public List<long> Vnfinfoid { get; set; }
        public List<short> FinancialYear { get; set; }
        public List<int> Financialversion { get; set; }

        public List<string> VcpuPerVm { get; set; }

        public List<bool> RxTxCpuCount { get; set; }

        public List<long> RamPerVm { get; set; }
        public List<long> DataDisk { get; set; }
        public List<long> OsDisk { get; set; }
        public List<string> IopsRunning { get; set; }

        public List<string> IopsLoading { get; set; }
        public List<string> VmWorkLoadDistribution { get; set; }
        public List<string> NorthDouthBoundBandWidth { get; set; }
        public List<string> EastWestBoundBandWidth { get; set; }

        public List<string> OtherRequirements { get; set; }
        public List<bool> BackupRequired { get; set; }
        public List<bool> ProbIngRequired { get; set; }
        public List<long> Noofvnfinstances { get; set; }
        public List<long> Noofvmspertype { get; set; }
    }

    public class VnfClusterInfoQueryDto : VnfVmCapacityQueryDto // : VnfInfoQueryDto
    {
        public List<long> VnfClusterInfoId { get; set; }
        public List<long> OpcoId { get; set; }
        public List<long> LocationId { get; set; }

        public List<long> ClusterNameId { get; set; }
        public List<long> NoOfBlades { get; set; }

        public List<string> LocationName { get; set; }

        public List<string> SiteName { get; set; }

        public List<string> OpcoDescritpion { get; set; }
        public List<string> ClusterDescription { get; set; }

        public List<string> HardwareType { get; set; }
        public List<long> HardwareTypeId { get; set; }
        public List<string> FileName { get; set; }
        public List<int> Revision { get; set; }

        public List<string> lastModified { get; set; }
        public List<long> VnfInfoId { get; set; }
        public List<string> VnfNameDescritpion { get; set; }
        public List<string> VnfVmTypeNameDescription { get; set; }
        public List<string> IntraVmType { get; set; }
        public List<string> InterVmType { get; set; }
        public List<string> VmWorkLoadType { get; set; }
        public List<string> Nsxt { get; set; }
        public List<string> VmStorageBlockSize { get; set; }
        public List<string> Numa { get; set; }
        public List<string> Socket { get; set; }

    }

    public class VbomReportQueryDto
    {
        public List<string> OpcoName { get; set; }
        public List<string> HardwareType { get; set; }
        public List<short> FinancialYear { get; set; }
        public List<long> VnfName{get;set; }
    }
}