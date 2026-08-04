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

namespace CAM.DataTransferObjects.Entita.IdentityAsIs

{
    public class IdentityAsIsDtoGrid : IdentityAsIsDto
    {
        [DateRangeGrid]
        [OrderGrid(Order = 15)]
        [DisplayName("Last Modified")]
        [Default]
        public string LastModifiedValue { get; set; }

        [IgnoreGrid]
        public DateTime? LastModified { get; set; }

        [OrderGrid(Order = 8)]
        [DisplayName("Class")]
        [Default]
        public string ClassDescription { get; set; }

        [OrderGrid(Order = 7)]
        [DisplayName("Category")]
        [Default]
        public string CategoryDescription { get; set; }

        [DisplayName("Type")]
        [OrderGrid(Order = 9)]
        [Default]
        public string TypeDescription { get; set; }

    }
    public class IdentityAsIsDto : GridDtoBase
    {
        [Default]
        [OrderGrid(Order = 1)]
        public string OpCo { get; set; }

        [Default]
        [DisplayName("Design Component Family")]
        [OrderGrid(Order = 2)]
        public string DesignComponentFamily { get; set; }

        [Default]
        [DisplayName("Resource Key")]
        [OrderGrid(Order = 3)]
        public string ResourceKey { get; set; }

        [Default]
        [DisplayName("Asset Name")]
        [OrderGrid(Order = 4)]
        public string AssetName { get; set; }


        [OrderGrid(Order = 5)]
        [DisplayName("Identity Index")]
        [Default]
        public int Id { get; set; }

        [OrderGrid(Order = 6)]
        [DisplayName("Value")]
        [Default]
        public string Value { get; set; }

        [IgnoreGrid]
        public int? ClassId { get; set; }

        [IgnoreGrid]
        public int? CategoryId { get; set; }

        [IgnoreGrid]
        public int? TypeId { get; set; }

        [DisplayName("Node Index")]
        [OrderGrid(Order = 10)]
        [Default]
        public long AssetId { get; set; }



        [Default]
        [DisplayName("Previous Resource Key")]
        [OrderGrid(Order = 11)]
        public string PreviousResourceKey { get; set; }

        [OrderGrid(Order = 12)]
        public string InterfaceType { get; set; }

        [OrderGrid(Order = 13)]
        public string InterfaceName { get; set; }

        [IgnoreGrid]
        public int OpCoId { get; set; }


        [DisplayName("Vertical Name")]
        [OrderGrid(Order = 14)]
        public string VerticalName { get; set; }

        [IgnoreGrid]
        public int VerticalId { get; set; }
    }
}