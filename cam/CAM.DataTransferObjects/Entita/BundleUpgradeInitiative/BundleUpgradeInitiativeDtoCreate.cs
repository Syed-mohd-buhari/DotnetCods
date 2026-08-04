using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CAM.DataTransferObjects.Entita.BundleUpgradeInitiative
{
  public  class BundleUpgradeInitiativeDtoCreate : BundleUpgradeInitiativeDto
    {
        public IDictionary<short, string>? OriginalEquipmentManufacturerResource { get; set; }
        [Required(ErrorMessage = "Equipment Manufacturer is required")]
        public short OriginalEquipmentManufacturerId { get; set; } //Id del selezionato      

    }

}
