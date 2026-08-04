using CAM.DataTransferObjects.Entita.AssetHardwareConfig;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.LookUp;
using OracleModels.DBModels;
using System.Collections.Generic;

namespace CAM.DataTransferObjects.Entita.DaAsssetMigration
{
    public class AssetHardwareAncillaryAddUpdateDto
    {
        public AssetHardwareAncillaryAddUpdateDto()
        {
            
        }

        public AssetHardwareAncillaryGridDto assetHardwareAncillaryGridDto { get; set; }
        public List<AssetAncillaryMajorHardwareResourceDto> MajorHardwareResource { get; set; }

        public List<KeyValuePairDto> ClusterNameResource { get; set; }

        public List<Datacenter> DataCenterResource { get; set; }
        public List<KeyValuePairDto> AssetClusterResource { get; set; }

        public List<KeyValuePairDto> AssetClusterTypeResource { get; set; }

        public short OpCoId { get; set; }   
        public long AssetId { get; set; }
    }

    public class AssetAncillaryCreateEditDto
    {
        public AssetAncillaryCreateEditDto()
        {

        } 
        public short OpCoId { get; set; }
        public long AssetId { get; set; }
    }

    public class AssetAncillaryMajorHardwareResourceDto
    {
        public long Key { get; set; }
        public string Text { get; set; }
        public long PhysicalServerHwModelId { get; set; }
        public long PhysicalServerVendorId { get; set; }
        public string PhysicalServerHwModel { get; set; }
        public string PhysicalServerVendor { get; set; }
    }
}
