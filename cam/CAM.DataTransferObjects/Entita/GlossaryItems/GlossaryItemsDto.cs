using CAM.DataAttributes.Grid;
using CAM.DataTransferObjects.QueryDto.Base;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CAM.DataTransferObjects.Entita
{
    public abstract class GlossaryItemsDto : GridDtoBase
    {
        public int GlossaryItemsId { get; set; }

        [OrderGrid(Order = 1)]
        [DisplayName("Header")]
        [StringLength(50)]
        public string Header { get; set; }


        [OrderGrid(Order = 2)]
        [DisplayName("Description")]
        [StringLength(2000)]
        public string Description { get; set; }

        [IgnoreGrid]
        public bool? IsTsrField { get; set; }

    }

    public class GlossaryItemsGridDto : GlossaryItemsDto
    {


    }
}