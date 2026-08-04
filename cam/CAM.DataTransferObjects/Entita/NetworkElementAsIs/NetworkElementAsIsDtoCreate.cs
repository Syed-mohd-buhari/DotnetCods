using System;
using System.Collections.Generic;
using System.Text;

namespace CAM.DataTransferObjects.Entita.NetworkElementAsIs
{
   public  class NetworkElementAsIsDtoCreate : NetworkElementAsIsDto
    {
        public short OriginalEquipmentManufacturerId { get; set; }
        public IDictionary<short, string> OriginalEquipmentManufacturerResource { get; set; } 

        public long NetworkElementAsPlannedId { get; set; }
        public IDictionary<long, string> NetworkElementAsPlannedResource { get; set; }
        public short LocationId { get; set; }
        public IDictionary<short, string> LocationResource { get; set; }
        public short OpCoId { get; set; }
        public IDictionary<short, string> OpCoResource { get; set; }

        public long SystemTypeId { get; set; }
        public IDictionary<long, string> SystemTypeResource { get; set; }




    }
}
