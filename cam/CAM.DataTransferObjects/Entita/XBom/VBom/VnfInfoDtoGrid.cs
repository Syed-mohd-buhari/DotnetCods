using CAM.DataAttributes.Grid;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto.Base;
using System.Collections.Generic;
using System.ComponentModel;

namespace CAM.DataTransferObjects.Entita.XBom.VBom
{
    public class VnfClusterInfoDtoGrid : GridDtoBase
    {
        [OrderGrid(Order = 1)]
        [DisplayName("VNFClusterInfo Id")]
        [Default]
        public long VnfClusterInfoId { get; set; }

        [IgnoreGrid]
        public long OpCoId { get; set; }

        [OrderGrid(Order = 2)]
        [DisplayName("Opco")]
        [Default]
        public string OpCoDescritpion { get; set; }

        [OrderGrid(Order = 3)]
        [DisplayName("Location")]
        [Default]
        public string LocationName { get; set; }

        [OrderGrid(Order = 4)]
        [DisplayName("Site")]
        [Default]
        public string SiteName { get; set; }

        [IgnoreGrid]
        public long ShortLocationId { get; set; }

        [OrderGrid(Order = 5)]
        [Default]
        [DisplayName("Cluster")]
        public string ClusterDescription { get; set; }

        [IgnoreGrid]
        public long ClusterId { get; set; }

        [OrderGrid(Order = 6)]
        [Default]
        [DisplayName("Hardware Type")]
        public string HardwareType { get; set; }

        [IgnoreGrid]
        public long HardwareTypeId { get; set; }

        [OrderGrid(Order = 7)]
        [Default]
        [DisplayName("File Name")]
        public string FileName { get; set; }

        [OrderGrid(Order = 8)]
        [Default]
        [DisplayName("Revision")]
        public int? Revision { get; set; }

        [OrderGrid(Order = 9)]
        [Default]
        [DisplayName("No Of Blades")]
        public short? NoOfBlades { get; set; }
 
    }
    public class VnfVbomCapacityDtoGrid  
    {

        #region  VnfVmCapacity  


        [OrderGrid(Order = 19)]
        [DisplayName("Financial Year")]
        [Default]
        public string FinancialYear { get; set; }

        [OrderGrid(Order = 20)]
        [DisplayName("Financial Version")]
        [Default]
        public string FinancialVersion { get; set; }



        [OrderGrid(Order = 21)]
        [Default]
        [DisplayName("No Of VNF Instances")]
        public string NoOfVnfInstances { get; set; }

        [OrderGrid(Order = 22)]
        [Default]
        [DisplayName("Site")]

        public string SiteName { get; set; }
        [IgnoreGrid]
        public string LocationName { get; set; }
        [IgnoreGrid]
        public long ShortLocationId { get; set; }

        [OrderGrid(Order = 23)]
        [Default]
        [DisplayName("No Of VMs Per Type")]
        public string NoOfVmsPerType { get; set; }


        [OrderGrid(Order = 24)]
        [Default]
        [DisplayName("No Of VCPU Per VM")]
        public string VcpuPerVm { get; set; }

        [OrderGrid(Order = 25)]
        [Default]
        [DisplayName("RX/TX CPU INCLUDED")]
        public string RxTxCpuCount { get; set; }

        [OrderGrid(Order = 26)]
        [Default]
        [DisplayName("RAM (GB) Per VM - Data Disk")]
        public string RamPerVm { get; set; }

        [OrderGrid(Order = 27)]
        [Default]
        [DisplayName("Storage (GB) Per VM - Data Disk")]
        public long DataDisk { get; set; }

        [OrderGrid(Order = 28)]
        [Default]
        [DisplayName("Storage (GB) Per VM - OS Disk")]
        public long? OsDisk { get; set; }

        [OrderGrid(Order = 29)]
        [Default]
        [DisplayName("IOPS per VM Running")]
        public string IopsRunning { get; set; }

        [OrderGrid(Order = 30)]
        [Default]
        [DisplayName("IOPS per VM Loading")]
        public string IopsLoading { get; set; }

        [OrderGrid(Order = 31)]
        [Default]
        [DisplayName("Read and Write VM workload distribution")]
        public string VmWorkLoadDistribution { get; set; }

        [OrderGrid(Order = 32)]
        [Default]
        [DisplayName("North and South bandwidth per VM (Mbits)")]
        public string NorthDouthBoundBandWidth { get; set; }

        [OrderGrid(Order = 33)]
        [Default]
        [DisplayName("East and West bandwidth per VM (Mbits)")]
        public string EastWestBoundBandWidth { get; set; }

        [OrderGrid(Order = 34)]
        [Default]
        [DisplayName("Onboarding Date  or Other Requirements")]
        public string OtherRequirements { get; set; }

        [OrderGrid(Order = 35)]
        [Default]
        [DisplayName("Backup Required")]
        public string BackupRequired { get; set; }

        [OrderGrid(Order = 36)]
        [Default]
        [DisplayName("Probing Required")]
        public string ProbIngRequired { get; set; }

      
        [OrderGrid(Order = 37)]
        [Default]
        [DisplayName("VNF VmCapacity Id")]
        public long VnfVmCapacityId { get; set; }
        #endregion

    }
    
    public class VnfVbomInfoDtoGrid : GridDtoBase
    {
        #region Info
        [OrderGrid(Order = 7)]
        [DisplayName("VNFINFO Id")]
        [Default]
        public long VnfInfoId { get; set; }
 

        [OrderGrid(Order = 8)]
        [Default]
        [DisplayName("VNF Name")]
        public string VnfNameDescritpion { get; set; }

        [IgnoreGrid]
        public long vnfNameId { get; set; }


        [OrderGrid(Order = 9)]
        [Default]
        [DisplayName("VM Type Name")]
        public string VnfVmTypeNameDescription { get; set; }

        [IgnoreGrid]
        public long VnfVmtypenameid { get; set; }          

        [OrderGrid(Order = 10)]
        [DisplayName("NSX T")]
        [Default]
        public string Nsxt { get; set; }

        [OrderGrid(Order = 11)]
        [Default]
        [DisplayName("INTRA VM Type")]
        public string IntraVmType { get; set; }

        [OrderGrid(Order =    12)]
        [Default]
        [DisplayName("INTER VM Type")]
        public string InterVmType { get; set; }

        [OrderGrid(Order = 13)]
        [DisplayName("VM Workload Type")]
        [Default]
        public string VmWorkLoadType { get; set; }

        [OrderGrid(Order = 14)]
        [DisplayName("Storage Block Size")]
        [Default]
        public string VmStorageBlockSize { get; set; }        
       
        //[OrderGrid(Order = 15)]
        //[Default]
        //[DisplayName("No Of VNF Instances")]
        //public string NoOfVnfInstances { get; set; }

        //[OrderGrid(Order = 16)]
        //[Default]
        //[DisplayName("No Of VMs Per Type")]
        //public string NoOfVmsPerType { get; set; }

        [OrderGrid(Order = 17)]
        [DisplayName("Numa")]
        [Default]
        public string Numa { get; set; }

        [OrderGrid(Order = 18)]
        [DisplayName("Socket")]
        [Default]
        public string Socket { get; set; }

        #endregion
        #region  VnfVmCapacity  


        [OrderGrid(Order = 19)]
        [DisplayName("Financial Year")]
        [Default]
        public string FinancialYear { get; set; }

        [OrderGrid(Order = 20)]
        [DisplayName("Financial Version")]
        [Default]
        public string FinancialVersion { get; set; }

        [OrderGrid(Order = 21)]
        [Default]
        [DisplayName("No Of VNF Instances")]
        public string NoOfVnfInstances { get; set; }

        [OrderGrid(Order = 22)]
        [Default]
        [DisplayName("Site")]

        public string SiteName { get; set; }
        [IgnoreGrid]
        public string LocationName { get; set; }
        [IgnoreGrid]
        public long ShortLocationId { get; set; }

        [OrderGrid(Order = 23)]
        [Default]
        [DisplayName("No Of VMs Per Type")]
        public string NoOfVmsPerType { get; set; }

        [OrderGrid(Order = 24)]
        [Default]
        [DisplayName("No Of VCPU Per VM")]
        public string VcpuPerVm { get; set; }

        [OrderGrid(Order = 25)]
        [Default]
        [DisplayName("RX/TX CPU INCLUDED")]
        public string RxTxCpuCount { get; set; }

        [OrderGrid(Order = 26)]
        [Default]
        [DisplayName("RAM (GB) Per VM - Data Disk")]
        public string RamPerVm { get; set; }

        [OrderGrid(Order = 27)]
        [Default]
        [DisplayName("Storage (GB) Per VM - Data Disk")]
        public long DataDisk { get; set; }

        [OrderGrid(Order = 28)]
        [Default]
        [DisplayName("Storage (GB) Per VM - OS Disk")]
        public long? OsDisk { get; set; }

        [OrderGrid(Order = 29)]
        [Default]
        [DisplayName("IOPS per VM Running")]
        public string IopsRunning { get; set; }

        [OrderGrid(Order = 30)]
        [Default]
        [DisplayName("IOPS per VM Loading")]
        public string IopsLoading { get; set; }

        [OrderGrid(Order = 31)]
        [Default]
        [DisplayName("Read and Write VM workload distribution")]
        public string VmWorkLoadDistribution { get; set; }

        [OrderGrid(Order = 32)]
        [Default]
        [DisplayName("North and South bandwidth per VM (Mbits)")]
        public string NorthDouthBoundBandWidth { get; set; }

        [OrderGrid(Order = 33)]
        [Default]
        [DisplayName("East and West bandwidth per VM (Mbits)")]
        public string EastWestBoundBandWidth { get; set; }

        [OrderGrid(Order = 34)]
        [Default]
        [DisplayName("Onboarding Date  or Other Requirements")]
        public string OtherRequirements { get; set; }

        [OrderGrid(Order = 35)]
        [Default]
        [DisplayName("Backup Required")]
        public string BackupRequired { get; set; }

        [OrderGrid(Order = 36)]
        [Default]
        [DisplayName("Probing Required")]
        public string ProbIngRequired { get; set; }

        [OrderGrid(Order = 37)]
        [Default]
        [DisplayName("VNF VmCapacity Id")]
        public long VnfVmCapacityId { get; set; }
        #endregion
        public List<VnfVbomCapacityDtoGrid> _vnfVbomCapacityDtoGrid { get; set; }

    }

   
    public class VnfInfoDtoGrid : GridDtoBase
    {
        [OrderGrid(Order = 1)]  [DisplayName("VNF Id")] [Default]
        public long VnfInfoId { get; set; }

        [IgnoreGrid]
        public long VnfNameId { get; set; }

        [OrderGrid(Order = 2)]  [Default] [DisplayName("VNF Name")]
        public string VnfNameDescritpion { get; set; }

        [OrderGrid(Order = 3)] [Default] [DisplayName("VM Type Name")]
        public string VmTypeNameDescription { get; set; }

        [IgnoreGrid]
        public long Vmtypenameid { get; set; }

        [OrderGrid(Order = 4)]  [Default]  [DisplayName("Cluster")]
        public string ClusterDescription { get; set; }

        [IgnoreGrid]
        public long ClusterId { get; set; }

        [OrderGrid(Order = 5)]  [DisplayName("Site")] [Default]
        public string ShortLocation { get; set; }

        [OrderGrid(Order = 6)] [DisplayName("NSX T")] [Default]
        public string Nsxt { get; set; }

        [OrderGrid(Order = 7)] [Default]  [DisplayName("INTRA VM Type")]
        public string IntraVmType { get; set; }

        [OrderGrid(Order = 8)]
        [Default]
        [DisplayName("INTER VM Type")]
        public string InterVmType { get; set; }

        [OrderGrid(Order = 9)] [DisplayName("VM Workload Type")]
        [Default]
        public string VmWorkLoadType { get; set; }

        [OrderGrid(Order = 10)] [DisplayName("Storage Block Size")]
        [Default]
        public string VmStorageBlockSize { get; set; }

        /* --------------------- VnfInstance ----------------------------*/
        [OrderGrid(Order = 32)]       
        [DisplayName("Vm Instance Id")]
        public long VnfVmInstanceId { get; set; }
        [IgnoreGrid]
        public long OpCoId { get; set; }

        [OrderGrid(Order = 11)]
        [DisplayName("Opco")]
        [Default]
        public string OpCoDescritpion { get; set; }

        [OrderGrid(Order = 12)]
        [DisplayName("Location")]
        [Default]
        public string LocationName { get; set; }
        [IgnoreGrid]
        public long ShortLocationId { get; set; }

        [OrderGrid(Order = 13)]
        [Default]
        [DisplayName("No Of VNF Instances")]
        public string NoOfVnfInstances { get; set; }

        [OrderGrid(Order = 14)]
        [Default]
        [DisplayName("No Of VMs Per Type")]
        public string NoOfVmsPerType { get; set; }

        [OrderGrid(Order = 15)]
        [DisplayName("Numa")]
        [Default]
        public string Numa { get; set; }

        [OrderGrid(Order = 16)]
        [DisplayName("Socket")]
        [Default]
        public string Socket { get; set; }

        /* --------------------- VnfVmCapacity ----------------------------*/

        [OrderGrid(Order = 17)]
        [DisplayName("Financial Year")]
        [Default]
        public string FinancialYear { get; set; }

        [OrderGrid(Order = 18)]
        [Default]
        [DisplayName("No Of VCPU Per VM")]
        public string VcpuPerVm { get; set; }

        [OrderGrid(Order = 19)]
        [Default]
        [DisplayName("RX/TX CPU INCLUDED")]
        public string RxTxCpuCount { get; set; }

        [OrderGrid(Order = 20)]
        [Default]
        [DisplayName("RAM (GB) Per VM - Data Disk")]
        public string RamPerVm { get; set; }

        [OrderGrid(Order = 21)]
        [Default]
        [DisplayName("Storage (GB) Per VM - Data Disk")]
        public long DataDisk { get; set; }

        [OrderGrid(Order = 22)]
        [Default]
        [DisplayName("Storage (GB) Per VM - OS Disk")]
        public long? OsDisk { get; set; }

        [OrderGrid(Order = 23)]
        [Default]
        [DisplayName("IOPS per VM Running")]
        public string IopsRunning { get; set; }

        [OrderGrid(Order = 24)]
        [Default]
        [DisplayName("IOPS per VM Loading")]
        public string IopsLoading { get; set; }

        [OrderGrid(Order = 25)]
        [Default]
        [DisplayName("Read and Write VM workload distribution")]
        public string VmWorkLoadDistribution { get; set; }

        [OrderGrid(Order = 26)][Default]
        [DisplayName("North and South bandwidth per VM (Mbits)")]
        public string NorthDouthBoundBandWidth { get; set; }

        [OrderGrid(Order = 27)]
        [Default]
        [DisplayName("East and West bandwidth per VM (Mbits)")]
        public string EastWestBoundBandWidth { get; set; }

        [OrderGrid(Order = 28)]
        [Default]
        [DisplayName("Onboarding Date  or Other Requirements")]
        public string OtherRequirements { get; set; }

        [OrderGrid(Order = 29)]
        [Default]
        [DisplayName("Backup Required")]
        public string BackupRequired { get; set; }

        [OrderGrid(Order = 30)]
        [Default]
        [DisplayName("Probing Required")]
        public string ProbIngRequired { get; set; }

        [OrderGrid(Order = 31)]
        [Default]
        [DisplayName("VNF VmCapacity Id")]
        public long VnfVmCapacityId { get; set; }

    }


    public class VbomExportGridDto : GridDtoBase
    {
        #region Vnf Cluster Info
        [OrderGrid(Order = 1)]
        [DisplayName("VNF Cluster Id")]
        [Default]
        public long VnfClusterInfoId { get; set; }

        [IgnoreGrid]
        public short Opcoid { get; set; }
        [IgnoreGrid]
        public short SiteLocationid { get; set; }
        [IgnoreGrid]
        public long ClusterNameId { get; set; }

        [IgnoreGrid]
        public short? Noofblades { get; set; }


        [OrderGrid(Order = 2)]
        [DisplayName("Opco")]
        [Default]
        public string OpCoDescritpion { get; set; }

        [OrderGrid(Order = 3)]
        [DisplayName("Location")]
        [Default]
        public string LocationName { get; set; }

        [OrderGrid(Order = 4)]
        [DisplayName("Site")]
        [Default]
        public string SiteLocation { get; set; }

        [OrderGrid(Order = 5)]
        [DisplayName("Cluster")]
        [Default]
        public string ClusterName { get; set; }

        public string HardwareType { get; set; }
        public string FileName { get; set; }
        public int? Revision { get; set; }

        #endregion
        #region InstanceValue //---------------------- VnfInfo
        [IgnoreGrid]
        public long VnfInfoId { get; set; }

        [OrderGrid(Order = 6)]
        [Default]
        [DisplayName("VNF Name")]
        public string VnfNameDescritpion { get; set; }

        [OrderGrid(Order = 7)]
        [Default]
        [DisplayName("VM Type Name")]
        public string VmTypeNameDescription { get; set; }

        [IgnoreGrid]
        public long Vmtypenameid { get; set; }
   

        [OrderGrid(Order = 8)]
        [DisplayName("NSX T")]
        [Default]
        public string Nsxt { get; set; }

        [OrderGrid(Order = 9)]
        [Default]
        [DisplayName("INTRA VM Type")]
        public string IntraVmType { get; set; }

        [OrderGrid(Order = 10)]
        [Default]
        [DisplayName("INTER VM Type")]
        public string InterVmType { get; set; }

        [OrderGrid(Order = 11)]
        [DisplayName("VM Workload Type")]
        [Default]
        public string VmWorkLoadType { get; set; }

        [OrderGrid(Order = 12)]
        [DisplayName("Storage Block Size")]
        [Default]
        public string VmStorageBlockSize { get; set; } 

            [OrderGrid(Order = 13)]
            [DisplayName("No Of VNF Instances")]
            [Default]
        public string NoOfVnfInstances { get; set; }

            [OrderGrid(Order = 14)]
            [DisplayName("No Of VMs Per Type")]
        [Default]
        public string NoOfVmsPerType { get; set; }

            [OrderGrid(Order = 15)]
            [DisplayName("Numa")]
        [Default]
        public string Numa { get; set; }

            [OrderGrid(Order = 16)]
            [DisplayName("Socket")]
             [Default]
             public string Socket { get; set; }

        #endregion end InstanceValue
        #region Capactity /* --------------------- VnfVmCapacity ----------------------------*/

        [OrderGrid(Order = 17)]
        [DisplayName("Financial Year")]
        [Default]
        public string FinancialYear { get; set; }

        [OrderGrid(Order = 18)]
        [DisplayName("Financial Version")]
        [Default]
        public string FinancialVersion { get; set; }

        [OrderGrid(Order = 19)]
        [DisplayName("No Of VCPU Per VM")]
        [Default]
        public string VcpuPerVm { get; set; }

        [OrderGrid(Order = 20)]
        [DisplayName("RX/TX CPU INCLUDED")]
        [Default]
        public string RxTxCpuCount { get; set; }

        [OrderGrid(Order = 21)]
        [DisplayName("RAM (GB) Per VM - Data Disk")]
        [Default]
        public string RamPerVm { get; set; }

        [OrderGrid(Order = 22)]
        [DisplayName("Storage (GB) Per VM - Data Disk")]
        [Default]
        public long DataDisk { get; set; }

        [OrderGrid(Order = 23)]
        [DisplayName("Storage (GB) Per VM - OS Disk")]
        [Default]
        public long? OsDisk { get; set; }

        [OrderGrid(Order = 24)]
        [DisplayName("IOPS per VM Running")]
        [Default]
        public string IopsRunning { get; set; }

        [OrderGrid(Order = 25)]
        [DisplayName("IOPS per VM Loading")]
        [Default]
        public string IopsLoading { get; set; }

        [OrderGrid(Order = 26)]
        [DisplayName("Read and Write VM workload distribution")]
        [Default]
        public string VmWorkLoadDistribution { get; set; }

        [OrderGrid(Order = 27)]
        [DisplayName("North and South bandwidth per VM (Mbits)")]
        [Default]
        public string NorthDouthBoundBandWidth { get; set; }

        [OrderGrid(Order = 28)]
        [DisplayName("East and West bandwidth per VM (Mbits)")]
        [Default]
        public string EastWestBoundBandWidth { get; set; }

        [OrderGrid(Order = 29)]
        [DisplayName("Onboarding Date  or Other Requirements")]
        [Default]
        public string OtherRequirements { get; set; }

        [OrderGrid(Order = 30)]
        [DisplayName("Backup Required")]
        [Default]
        public string BackupRequired { get; set; }

        [OrderGrid(Order = 31)]
        [DisplayName("Probing Required")]
        [Default]
        public string ProbIngRequired { get; set; }

        [OrderGrid(Order = 32)]
        [DisplayName("VNF VmCapacity Id")]
        [Default]
        public long VnfVmCapacityId { get; set; }


        #endregion

        public List<VnfVbomCapacityDtoGrid> vnfVbomCapacityDtoGrids { get; set; }
    }


    public class VbomReportDto
    {
        [OrderGrid(Order = 1)]
        [Default]
        [DisplayName("VNF Name")]
        public string VnfName { get; set; }
        [OrderGrid(Order = 2)]
        [Default]
        [DisplayName("VM Type Name")]
        public string VmTypeName { get; set; }
        [OrderGrid(Order = 3)]
        [Default]
        [DisplayName("Number Of Vms Per Type")]
        public long NumberOfVmsPerType { get; set; }
        [OrderGrid(Order = 4)]
        [Default]
        [DisplayName("Number Of vCpu PerType")]
        public long NumberOfvCpuPerType { get; set; }
        [OrderGrid(Order = 5)]
        [Default]
        [DisplayName("RAM(Gb)")]
        public long RamPerGb { get; set; }
        [OrderGrid(Order = 5)]
        [Default]
        [DisplayName("Storage Per Vm DataDisk")]
        public long StoragePerVmDataDisk { get; set; }
        [OrderGrid(Order = 6)]
        [Default]
        [DisplayName("Hardware Type")]
        public string HardwareType { get; set; }
        public List<VbomDisaggregatedView> vbomDisaggregatedViews { get; set; }
        public List<KeyValuePairDto> vnfNames { get; set; }
 
    }
    public class VbomDisaggregatedView
    {
        public string VnfName { get; set; }
        public string VmTypeName { get; set; }
        public long vCpu { get; set; }
        public long Ram { get; set; }
        public long DataDisk { get; set; }
    }
     
}
