using System;
using System.Collections.Generic;
using System.Text;

namespace CAM.DataTransferObjects.LookUp.DeliveryTracking
{
    public class DeliveryTrackingDtoUpdate : DeliveryTrackingDtoCreate
    {

        public IDictionary<int, string>? MS1EventTypeResource { get; set; }
        public IDictionary<int, string>? MS1StatusResource { get; set; }
        public IDictionary<int, string>? MS2EventTypeResource { get; set; }
        public IDictionary<int, string>? MS2StatusResource { get; set; }
        public IDictionary<int, string>? MS3EventTypeResource { get; set; }
        public IDictionary<int, string>? MS3StatusResource { get; set; }
        public IDictionary<int, string>? MS4EventTypeResource { get; set; }
        public IDictionary<int, string>? MS4StatusResource { get; set; }

        public IDictionary<int, string>? MsStatusResource { get; set; }

    }
}
