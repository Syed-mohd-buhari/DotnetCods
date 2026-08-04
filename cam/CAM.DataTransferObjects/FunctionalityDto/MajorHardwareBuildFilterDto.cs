using CAM.Infrastucture;
using System;

namespace CAM.DataTransferObjects.FunctionalityDto
{
    public class MajorHardwareBuildFilterDto
    {
        //public short OriginalEquipmentManufacturerId { get; set; }
        public FilterBaseDto<string> HardwareSolution { get; set; }
        public FilterBaseDto<string> Platform { get; set; }
        public FilterBaseDto<string> HardwareType { get; set; }
        public FilterBaseDto<string> OtherHardwareInfo { get; set; }
        public FilterBaseDto<DateTime?> LastTimeBuyNew { get; set; }
        public FilterBaseDto<DateTime?> LastTimeBuyUpgrades { get; set; }
        public FilterBaseDto<DateTime?> LastTimeBuyExpansions { get; set; }
        public FilterBaseDto<DateTime?> EndOfMaintenance { get; set; }
        public FilterBaseDto<DateTime?> EndOfsupport { get; set; }
        public FilterBaseDto<string> VulnerabilityStatus { get; set; }
        public FilterBaseDto<string> SpareFieldsJson { get; set; }
        public FilterBaseDto<short> OriginalEquipmentManufacturer { get; set; }

    }


}
