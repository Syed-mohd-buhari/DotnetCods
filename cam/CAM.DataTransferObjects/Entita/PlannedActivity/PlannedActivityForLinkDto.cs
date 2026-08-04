using System;
using System.Collections.Generic;
using System.Text;
using CAM.DataTransferObjects.Entita.NetworkElementAsPlanned;

namespace CAM.DataTransferObjects.Entita.PlannedActivity
{
    public class PlannedActivityForLinkDto
    {  //info da LCM
        public IDictionary<short, string>? OpCoResource { get; set; }
        public short OpCoId { get; set; }

        public IDictionary<long, string> DesignComponentResource { get; set; }

        public long DesignComponentId { get; set; }
        public int NumberOfNodes { get; set; }

        public int NumberOfLabNodes { get; set; }

        public long PlannedActivityId { get; set; }
        //info da Planned
        public IDictionary<long, PlannedActivityToConnect>  PlannedActivityResource { get; set; }

        public bool ElementCount { get; set; }


        public IEnumerable<NetworkElementAssociated> StartNetworkElementAssociateds { get; set; }
        public IEnumerable<NetworkElementAssociated> EndNetworkElementAssociateds { get; set; }


    }
}
