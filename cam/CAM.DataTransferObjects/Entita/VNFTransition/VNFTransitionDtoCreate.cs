using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CAM.Infrastucture;

namespace CAM.DataTransferObjects.Entita.VNFTransition
{
    public class VnfTransitionDtoCreate : VNFTransitionDto
    {
        public IDictionary<short, string> VnfDesignComponentResource { get; set; }
        public IDictionary<short, string> EquipmentStatusResource { get; set; }
        [Required(ErrorMessage = "Design Component is required")]
        public short? VnfDesignComponentId { get; set; }
        [Required(ErrorMessage = "Equipment Status is required")]
        public short? EquipmentStatusId { get; set; }

        public IDictionary<short, string> OpCoResource { get; set; }
        [Required(ErrorMessage = "OpCo is required")]
        public short OpCoId { get; set; }

        public IDictionary<short, RelatedResource> NfviBundleIDResource { get; set; }
        [Required(ErrorMessage = "NFVI Bundle ID is required")]
        public short? NfviBundleIDId { get; set; }
        public List<string> VnfTypeResource { get; set; }
        public string VnfType { get; set; }

        public List<string> NfviSiteDesignationResource { get; set; }
        public string NfviSiteDesignation { get; set; }
    }
}
