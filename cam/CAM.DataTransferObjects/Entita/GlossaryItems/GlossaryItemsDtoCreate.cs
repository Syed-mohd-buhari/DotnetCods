using CAM.DataAttributes.Grid;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CAM.DataTransferObjects.Entita
{
    public class GlossaryItemsDtoCreate : GlossaryItemsDto
    {
        [OrderGrid(Order = 1)]
        [DisplayName("Header")]
        [StringLength(50)]
        public string Header { get; set; }


        [OrderGrid(Order = 2)]
        [DisplayName("Description")]
        [StringLength(2000)]
        public string Description { get; set; }

    }
}