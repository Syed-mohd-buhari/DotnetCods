using System;
using System.Collections.Generic;
using CAM.DataAttributes.Grid;
using CAM.DataTransferObjects.QueryDto.Base;

namespace CAM.DataTransferObjects.LookUp.Asset
{
   
    public class AssetTypeDto
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public int? AssetCategoryId { get; set; }
        public int? IdAssetCategory { get; set; }
        public DateTime? LastModified { get; set; }
        public string LastModifiedBy { get; set; }
        public Dictionary<int, string> AssetCategoryResource { get; set; }

    }
    public class AssetTypeDtoGrid : GridDtoBase
    {
        [OrderGrid(Order = 1)]
        [Default]
        public int Id { get; set; }
        [OrderGrid(Order = 2)]
        [Default]
        public string Description { get; set; }
        [OrderGrid(Order = 3)]
        [Default]
        public string AssetCategoryId { get; set; }
        [IgnoreGrid]
        public int? IdAssetCategory { get; set; }

    }
    public class AssetTypeDtoQuery: QueryObject
    {
        public List<int> Id { get; set; }
        public List<string> Description { get; set; }
        public List<int> AssetCategoryId { get; set; }
       
    }
}