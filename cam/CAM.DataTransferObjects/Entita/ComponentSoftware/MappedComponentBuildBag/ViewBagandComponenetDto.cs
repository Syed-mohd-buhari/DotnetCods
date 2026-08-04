using CAM.DataTransferObjects.FunctionalityDto;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CAM.DataTransferObjects.Entita.ComponentSoftware
{
    public class ViewBagandComponenetDto
    {
        [StringLength(2000)]
        public string ComponentBagDescription { get; set; }
        public long BuildBagId { get; set; }
        #region used for Dropdown filter
        public string Text { get; set; }
        public long Key { get; set; }

        #endregion
        public List<FilterValueDto> MappedComponentDetails { get; set; }        
        public bool isColour { get;set; }
    }
 
}