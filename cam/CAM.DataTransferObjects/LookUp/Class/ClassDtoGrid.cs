using CAM.DataAttributes.Export;
using CAM.DataAttributes.Grid;
using CAM.DataTransferObjects.QueryDto.Base;
using CAM.Entities.Mappers.Lookup;
using CAM.Entities.Models.Lookup;
using DocumentFormat.OpenXml.Drawing.Charts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CAM.DataTransferObjects.LookUp.Class

{
    public class ClassDtoGrid : ClassDto
    {
        [DateRangeGrid]
        [OrderGrid(Order = 4)]
        [DisplayName("Last Modified Date")]
        [Default]
        public string LastModifiedValue { get; set; }

        [IgnoreGrid]
        public DateTime? LastModified { get; set; }

        [OrderGrid(Order = 3)]
        [DisplayName("Category")]
        [Default]
        public string CategoryDescription { get; set; }
    }
    public class ClassDto : GridDtoBase
    {
        [OrderGrid(Order = 1)]
        [DisplayName("ID")]
        [Default]
        public int Id { get; set; }

        [OrderGrid(Order = 2)]
        [DisplayName("Description")]
        [Default]
        public string Description { get; set; }

        [IgnoreGrid]
        public int CategoryId { get; set; }
    }
}