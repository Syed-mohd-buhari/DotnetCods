using System;
using System.Collections.Generic;
using System.ComponentModel;
using CAM.DataAttributes.Grid;
using CAM.DataTransferObjects.QueryDto.Base;

namespace CAM.DataTransferObjects.LookUp.Asset
{
    public class AssetClassDto
    {
        public int Id { get; set; }
        public string Description { get; set; }
        //public int AssetCategoryId { get; set; }
        public DateTime? LastModified { get; set; }
        //public Dictionary<int, string> AssetCategoryResource { get; set; }
        public string LastModifiedBy { get; set; }

    }
    public class AssetClassDtoGrid : GridDtoBase
    {
        [OrderGrid(Order = 1)]
        [Default]
        public int Id { get; set; }
        [OrderGrid(Order = 2)]
        [Default]
        public string Description { get; set; }
        //[OrderGrid(Order = 3)]
        //public string AssetCategoryId { get; set; }
        //[IgnoreGrid]
        //public int IdAssetCategory { get; set; }
        [OrderGrid(Order = 4)]
        [MailTo]
        [DisplayName("Last Modified By")]
        [Default]
        public string LastModifiedBy { get; set; }

    }
    public class AssetClassDtoQuery:QueryObject
    {
        public List<int> Id { get; set; }
        public List<string> Description { get; set; }
        //public List<int> AssetCategoryId { get; set; }
       
    }
}