using CAM.DataAttributes.Grid;
using CAM.DataTransferObjects.QueryDto.Base;
using System.ComponentModel;

namespace CAM.DataTransferObjects.Entita.ComponentSoftware
{
    public class BuildBagDtoGrid :GridDtoBase
    {
        [OrderGrid(Order = 1)]
        [DisplayName("Build Id")]
        [Default]
        public string BuildBagId { get; set; }

        [OrderGrid(Order = 2)]
        [DisplayName("OpCo")]
        [Default]
        public string OpCo { get; set; }

        [OrderGrid(Order = 3)]
        [DisplayName("DCF Name")]
        [Default]
        public string DesignComponentFamilyName { get; set; }


        [OrderGrid(Order = 4)]
        [DisplayName("Bag Name")]
        [Default]
        public string BuildBagDescription { get; set; }

        [IgnoreGrid]
        [DisplayName("Bag Version")]
        [Default]
        public long BagVersion { get; set; }

        //[OrderGrid(Order = 3)]
        //[DateRangeGrid]
        //[DisplayName("Last Modified")]
        //public string LastModifiedValue { get; set; }

        //[OrderGrid(Order = 4)]        
        //[DisplayName("Last Modified By")]
        //public string LastModifiedBy { get; set; }

        [OrderGrid(Order = 3)]
        [Default]
        [DisplayName("Associated With LCM")]
        public string AssociatedWithLcm { get; set; }

        [OrderGrid(Order = 4)]        
        [DisplayName("Component Sofware")]
        [Default]
        public  string MappedComponentSoftwareBuild { get; set; }

        [OrderGrid(Order = 5)]
        [Default]
        [DisplayName("Visible Flag")]
        public string VisibleFlag { get; set; }



    }
}