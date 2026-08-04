using System.Collections.Generic;
using CAM.DataTransferObjects.QueryDto.Base;

namespace CAM.DataTransferObjects.LookUp.Location
{
    public class LocationDtoQuery: QueryObject
    {
        public List<short> Id { get; set; }
        public List<string> Description { get; set; }
        public List<short> Opco { get; set; }
        public List<short> LocationType { get; set; }
        public List<bool> DefaultValue { get; set; }
        public List<string> ShortDescription { get; set; }

    }
}