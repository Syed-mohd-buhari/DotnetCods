using System.Collections.Generic;
using System.ComponentModel;
using CAM.DataAttributes.Grid;

namespace CAM.DataTransferObjects.Entita.ComponentSoftware
{
    public class ComponentMappingSWBuildBagGridDto
    {
        [IgnoreGrid]
        [DisplayName("Component Software Build Bag Index")]

        public long ComponentSwBuildBagId { get; set; }

       
        [DisplayName("Build Bag Index")]
        [OrderGrid(Order = 1)]
        public long BuildBagId { get; set; }


        [OrderGrid(Order = 2)]
        [DisplayName("Build Bag Description")]

        public string BuildBagDescription { get; set; }


        [DisplayName("Component Software Build Index")]
        [IgnoreGrid]
        public string ComponentSwBuildId { get; set; }

        [OrderGrid(Order = 3)]
        [DisplayName("Component Software Build Bag Description")]

        public string ComponentSwBuildBagDescription { get; set; }


        [OrderGrid(Order = 4)]
        [DisplayName("Last Modified")]
        public string LastModifiedValue { get; set; }

        [OrderGrid(Order = 5)]
        [DateRangeGrid]
        [DisplayName("Last Modified By")]
        public new string LastModifiedBy { get; set; }
       

       

    }
}