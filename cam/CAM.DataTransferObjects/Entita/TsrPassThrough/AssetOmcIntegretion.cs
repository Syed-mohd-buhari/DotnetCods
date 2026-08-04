using System.Collections.Generic;

namespace CAM.DataTransferObjects.Entita.TsrPassThrough
{
    public class AssetOmcIntegretion
    {
        public string AssetName { get; set; }
        public string CloudVendor { get;set; }
        public string Model { get; set; }
        public string FirmwareVersion { get; set; }
        public string DependentHW { get; set; }
        public string ManagementIpAddress { get; set; }
        public string ManagementIpAddressForCluster { get; set; }
        public string HwManufacturer { get; set; } 
        public string HwComponentName { get; set; }
        public string HwPartNumber { get; set; }
        public string SystemName { get; set; }
        public string SystemNameForCluster { get; set; }
        public string SwitchSerialNumbers { get; set; }
    }
}
