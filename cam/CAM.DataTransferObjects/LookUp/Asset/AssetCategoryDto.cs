using System;
using System.Collections.Generic;
using CAM.DataAttributes.Grid;
using CAM.DataTransferObjects.QueryDto.Base;

namespace CAM.DataTransferObjects.LookUp.Asset
{
   
    public class AssetCategoryDto
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string AssetClassId { get; set; }
        public int? IdAssetClass { get; set; }
        public bool TakeFromAssetTypeTable { get; set; }
        public DateTime? LastModified { get; set; }
        public string LastModifiedBy { get; set; }
        public Dictionary<int, string> AssetClassResource { get; set; }

    }
    public class AssetCategoryDtoGrid : GridDtoBase
    {
        [OrderGrid(Order = 1)]
        [Default]
        public int Id { get; set; }
        [OrderGrid(Order = 2)]
        [Default]
        public string Description { get; set; }
        [OrderGrid(Order = 3)]
        [Default]
        public string AssetClassId { get; set; }
        [IgnoreGrid]
        public int IdAssetClass { get; set; }
        [OrderGrid(Order = 4)]
        [Default]
        public bool TakeFromAssetTypeTable { get; set; }
    }
    public class AssetCategoryDtoQuery: QueryObject
    {
        public List<int> Id { get; set; }
        public List<string> Description { get; set; }
        public List<int> AssetClassId { get; set; }
        public List<bool> TakeFromAssetTypeTable { get; set; }

    }
}