using CAM.DataAttributes.Grid;
using CAM.DataTransferObjects.QueryDto.Base;
using System.Collections.Generic;
using System.ComponentModel;

namespace CAM.DataTransferObjects.Entita.AssetHardwareConfig
{
    public class AssetCapacityInfoGridDto
    {

        public long AssetCapacityInfoId { get; set; }

        public string? PhysicalServerHostName { get; set; }
        public string? PhysicalServerIpAddress { get; set; }
        public string PhysicalServerSerialNumber { get; set; }
        public long? NoOfInstances { get; set; }
        public long? Vcpu { get; set; }
        public decimal? Memory { get; set; }
        public decimal? Storage { get; set; }

        public long? PhysicalServerHwModelId { get; set; }
        public long? PhysicalServerVendorId { get; set; }
        public string? PhysicalServerHwModel { get; set; }
        public string? PhysicalServerVendor { get; set; }


    }
    public class AssetHardwareAncillaryGridDto : GridDtoBase
    {

        [OrderGrid(Order = 1)]
        [DisplayName("Asset Ancillary Id")]
        [Default]
        public long AssetHardwareAncillaryId { get; set; }

        [OrderGrid(Order = 2)]
        [Default]
        public long NetworkElementAsPlannedId { get; set; }

        public string ElementName { get; set; }

        [IgnoreGrid]
        public long? MajorHardwareId { get; set; }

        public string? MajorHardwareName { get; set; }

        [IgnoreGrid]
        public long? DataCenterId { get; set; }

        public string DataCeterName { get; set; }
        [IgnoreGrid]
        public long? ClusterNameId { get; set; }

        public string ClusterNameDesc { get; set; }
        [IgnoreGrid]
        public long? AssetClusterTypeId { get; set; }

        public string AssetClusterTypeDesc { get; set; }
        [IgnoreGrid]
        public long? AssetClusterId { get; set; }

        public string AssetClusterDesc { get; set; }

        public List<AssetCapacityInfoGridDto> assetCapacityInfoGridDtos { get; set; }

        #region Capacity


        public long AssetCapacityInfoId { get; set; }

        public string? PhysicalServerHostName { get; set; }
        public string? PhysicalServerIpAddress { get; set; }
        public string? PhysicalServerSerialNumber { get; set; }
        public long? NoOfInstances { get; set; }
        public long? Vcpu { get; set; }
        public decimal? Memory { get; set; }
        public decimal? Storage { get; set; }

        public long? PhysicalServerHwModelId { get; set; }
        public long? PhysicalServerVendorId { get; set; }
        public string PhysicalServerHwModel { get; set; }
        public string PhysicalServerVendor { get; set; }
        #endregion
    }


}
