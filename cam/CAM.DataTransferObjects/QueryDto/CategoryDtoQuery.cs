using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace CAM.DataTransferObjects.QueryDto
{

    public class CategoryDtoQuery : QueryObject
    {
        public List<int> Id { get; set; }
        public List<string> Description { get; set; }
        public DateFilter LastModifiedValue { get; set; }
    }
}
