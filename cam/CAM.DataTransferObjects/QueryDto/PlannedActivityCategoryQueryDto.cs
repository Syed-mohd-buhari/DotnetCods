using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto.Base;
using System.Collections.Generic;

namespace CAM.DataTransferObjects.QueryDto
{
    public class PlannedActivityCategoryQueryDto : QueryObject
    {
        public List<short> Plannedactivitycategoryid { get; set; }
        public List<string> Categorydescription { get; set; }
        public List<string> CreationUser { get; set; }
        public DateFilter CreationDate { get; set; }
        public List<string> ModificationUser { get; set; }     
        public DateFilter ModificationDate { get; set; }
    }
}
