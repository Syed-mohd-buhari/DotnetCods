using System.ComponentModel;
using CAM.DataAttributes.Grid;

namespace CAM.DataTransferObjects.Entita.VNFTransition
{
   public class VNFTransitionDtoGrid : VNFTransitionDto
    {
        [IgnoreGrid]
        public long VNFTransitionId { get; set; }
        [OrderGrid(Order = 5)]
        [DisplayName("VNF Design Component")]

        public string vnfDesignComponent { get; set; }
       
        [OrderGrid(Order = 1)]
        [Default]
        public string OpCo { get; set; }

        [OrderGrid(Order = 4)]
        [DisplayName("Equipment Status")]
        [Default]
        public string EquipmentStatus { get; set; }

        [OrderGrid(Order = 10)]
        [DisplayName("NFVI Bundle ID")]
        [Default]

        public string NfviBundleID { get; set; }


    }
}
