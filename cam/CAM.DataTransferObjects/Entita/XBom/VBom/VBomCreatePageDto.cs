using CAM.DataTransferObjects.FunctionalityDto;
using OracleModels.DBModels;
using System.Collections.Generic;
 
namespace CAM.DataTransferObjects.Entita.XBom.VBom
{
    public class VBomCreatePageEntityDto
    {
        public Vnfclusterinfo _VnfClusterInfoDto { get; set; }
       
        public List<KeyValuePairDto> VnfNameResources { get; set; }
        public List<KeyValuePairDto> VnClusterNameResource { get; set; }
        public List<Vmtypename> VnVmTypeNameResource { get; set; }
        public List<OpcoBasedLocation> OpcoBasedLocationResource { get; set; }
        public List<FilterValueDto> IntraVmTypeResource { get; set; }
        public List<FilterValueDto> InterTypeResource { get; set; }
        public List<FilterValueDto> VmWorkLoadTypeDetail { get; set; }
        public List<FilterValueDto> FinancialVersion { get; set; }
        public List<KeyValuePairDto> HardwareTypeResource { get; set; }


    }

    public class VnfInfoEntitiesDto
    {
        public long VnfInfoId { get; set; }
        public long VnfNameId { get; set; }
        public long ClusterId { get; set; }
        public long VmTypeNameId { get; set; }
        public string Nsxt { get; set; }
        public string IntraVmType { get; set; }
        public string InterVmType { get; set; }
        public string VmWorkLoadType { get; set; }
        public string VmStorageBlockSize { get; set; }

        public List<VnfInstancesEntitiesDto> VnfInstancesDtoEntity { get; set; }
    }
    public class VnfInstancesEntitiesDto
    {
        public long VnfVmInstanceId { get; set; }
        public long VnfInfoId { get; set; }
        public short OpcoId { get; set; }
        public short LocationId { get; set; } 
        public decimal NoOfVnfInstances { get; set; }
        public decimal NoOfVmsPerType { get; set; }
        public string  Numa { get; set; }
        public string Socket { get; set; }

        public List<VnfVmCapacityEntitiesDto> VnfVmCapacityDtoEntity { get; set; }

    }
    public class VnfVmCapacityEntitiesDto
    {
        public long VnfVmCapacityId { get; set; }
        public long VnfVmInstanceId { get; set; }
        public short FinancialYear { get; set; }
        public decimal VcpuPerVm { get; set; }
        public string RxTxCpuCount { get; set; }
        public string RamPerVm { get; set; }
        public string DataDisk { get; set; }
        public long? OsDisk { get; set; }
        public decimal? IopsRunning { get; set; }
        public string IopsLoading { get; set; }
        public string VmWorkLoadDistribution { get; set; }
        public string NorthDouthBoundBandWidth { get; set; }
        public string EastWestBoundBandWidth { get; set; }
        public string OtherRequirements { get; set; }
        public string BackupRequired { get; set; }
        public string ProbIngRequired { get; set; }

    }

    public class OpcoBasedLocation
    {

        public short OpcoId { get; set; }
        public string OpcoDescription { get; set; }
        public List<LocationValueDto> LocationDetails { get; set; }

    }
    public class LocationValueDto 
    {
         public short Value { get; set; }
          public string Text { get; set; }
         public string ShortDescription { get; set; }
    }



}
