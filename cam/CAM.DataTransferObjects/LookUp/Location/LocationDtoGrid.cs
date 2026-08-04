using CAM.DataAttributes.Grid;
using CAM.DataTransferObjects.QueryDto.Base;

namespace CAM.DataTransferObjects.LookUp.Location
{
    public class LocationDtoGrid : GridDtoBase
    {
        [OrderGrid(Order = 1)]
        [Default]
        public short Id { get; set; }

        [OrderGrid(Order = 2)]
        [Default]
        public string Description { get; set; }

        [OrderGrid(Order = 3)]
        [Default]
        public string Opco { get; set; }

        [OrderGrid(Order = 4)]
        [Default]
        public string LocationType { get; set; }

        [IgnoreGrid]
        public short OpcoId { get; set; }

        [IgnoreGrid]
        public short LocationTypeId { get; set; }

        [OrderGrid(Order = 5)]
        [Default]
        public bool DefaultValue { get; set; }
       
        [OrderGrid(Order = 6)]
        [Default]
        public string ShortDescription { get; set; }
    }
}