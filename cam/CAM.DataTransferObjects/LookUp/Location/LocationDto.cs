using System;
using System.Collections.Generic;

namespace CAM.DataTransferObjects.LookUp.Location
{
   
    public class LocationDto
    {
        public short Id { get; set; }
        public string Description { get; set; }
        public short? OpcoId { get; set; }
        public bool DefaultValue { get; set; }
        public List<short> LocationTypeIdsList { get; set; }
        public DateTime? LastModified { get; set; }
        public string LastModifiedBy { get; set; }
        public Dictionary<short, string> OpcoResource { get; set; }
        public Dictionary<short, string> LocationTypeResource { get; set; }

    }
}