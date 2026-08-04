using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CAM.Infrastucture;

namespace CAM.DataTransferObjects.Entita.NFVITransition
{
   public class NFVITransitionDtoCreate : NFVITransitionDto
    {
        public IDictionary<short, string> OpCoResource { get; set; }
        [Required(ErrorMessage = "OpCo is required")]
        public short OpCoId { get; set; }

       
        public IDictionary<short, RelatedResource> StatusLabMCResource { get; set; }
        [Required(ErrorMessage = "Status: Lab-MC is required")]
        public short? StatusLabMCId { get; set; }

        public IDictionary<short, RelatedResource> StatusLabSCResource { get; set; }
        [Required(ErrorMessage = "Status: Lab-SC is required")]
        public short? StatusLabSCId { get; set; }

        public IDictionary<short, RelatedResource> StatusLiveMCResource { get; set; }
        [Required(ErrorMessage = "Status: Live-MC is required")]
        public short? StatusLiveMCId { get; set; }


        public IDictionary<short, RelatedResource> StatusLiveSCResource { get; set; }
        [Required(ErrorMessage = "Status: Live-SC is required")]
        public short? StatusLiveSCId { get; set; }
        public IDictionary<short, RelatedResource> Status12KSwitchResource { get; set; }
        public short? Status12KSwitchId { get; set; }

        //public List<string> NfviSiteDesignationResource { get; set; }
        //public string NfviSiteDesignation { get; set; }



    }
}
