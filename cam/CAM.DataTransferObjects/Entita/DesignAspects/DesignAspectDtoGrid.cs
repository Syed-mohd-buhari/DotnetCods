using CAM.DataAttributes.Grid;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CAM.DataTransferObjects.Entita.DesignAspects
{
    public class DesignAspectDtoGrid :DesignAspectDto
    {
       

        [DisplayName("Used Functional Entities")]
        [OrderGrid(Order = 21)]
        [Default]
        public string UsedNetworkFunctions { get; set; }

        [DisplayName("Vodafone Name")]
        [OrderGrid(Order = 22)]
        [Default]
        public string VodafoneName { get; set; }

        [OrderGrid(Order = 18)]
        [DisplayName("Planned Action")]
        [Default]
        public IDictionary<short, string> PlannedActivity { get; set; }
    }
}
