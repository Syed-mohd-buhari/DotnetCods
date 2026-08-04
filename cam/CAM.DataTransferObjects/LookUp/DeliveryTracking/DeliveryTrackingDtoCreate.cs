using System;
using System.Collections.Generic;
using System.Text;

namespace CAM.DataTransferObjects.LookUp.DeliveryTracking
{
    public class DeliveryTrackingDtoCreate : DeliveryTrackingDto
    {
        public int? Ms1status { get; set; }
        public int? Ms2status { get;set; }
        public int? Ms3status { get; set; }
        public int? Ms4status { get; set; }
    }
}
