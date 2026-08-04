using System.ComponentModel;
using CAM.DataAttributes.Grid;

namespace CAM.DataTransferObjects.Entita.BundleUpgradeInitiative
{
   public class BundleUpgradeInitiativeDtoGrid : BundleUpgradeInitiativeDto
    {
        [IgnoreGrid]
        public long BundleUpgradeInitiativeId { get; set; }
        [OrderGrid(Order = 2)]
        [DisplayName("Equipment Manufacturer")]
        [Default]
        public string OriginalEquipmentManufacturer { get; set; }
       
    }
}
