using CAM.DataAttributes.Grid;
using CAM.DataTransferObjects.QueryDto.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAM.DataTransferObjects.Entita.ProblemCategory
{
    public class ProblemCategoryDto : GridDtoBase
    {
        [Default]
        [OrderGrid(Order = 1)]
        [DisplayName("Problem Category Id")]
        public long ProblemCategoryId { get; set; }
        [Default]
        [OrderGrid(Order = 2)]
        [DisplayName("Problem Category Description")]
        public string ProblemCategoryDescription { get; set; }
    }
}
