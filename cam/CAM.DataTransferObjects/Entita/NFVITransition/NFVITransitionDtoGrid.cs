using System.ComponentModel;
using CAM.DataAttributes.Grid;

namespace CAM.DataTransferObjects.Entita.NFVITransition
{
    public class NFVITransitionDtoGrid : NFVITransitionDto
    {
        [IgnoreGrid]
        public long NfviTransitionId { get; set; }


        [IgnoreGrid]
        public string StatusLabMCColor { get; set; }
        [IgnoreGrid]
        public string StatusLabSCColor { get; set; }
        [IgnoreGrid]
        public string StatusLiveMCColor { get; set; }
        [IgnoreGrid]
        public string StatusLiveSCColor { get; set; }
        [IgnoreGrid]
        public string Status12KSwitchColor { get; set; }

        [OrderGrid(Order = 1)]
        [DisplayName("OpCo")]
        [Default]
        public string OpCo { get; set; }

        [OrderGrid(Order = 4)]
        [DisplayName("Status: Lab-SC")]
        [Default]
        public string StatusLabSC { get; set; }
        [OrderGrid(Order = 3)]
        [DisplayName("Status: Lab-MC")]
        [Default]
        public string StatusLabMC { get; set; }
        [DisplayName("Status: Live-MC")]
        [Default]
        [OrderGrid(Order = 5)]
        public string StatusLiveMC { get; set; }

        [DisplayName("Status: Live-SC")]
        [Default]
        [OrderGrid(Order = 6)]
        public string StatusLiveSC { get; set; }

        [OrderGrid(Order = 7)]
        [DisplayName("Status: 12K Switch")]
        [Default]
        public string Status12KSwitch { get; set; }





    }
}
