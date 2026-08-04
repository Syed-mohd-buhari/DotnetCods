using System;
using System.Collections.Generic;
using System.Text;

namespace CAM.DataTransferObjects.Entita.PlannedActivity
{
    public class PlannedActivityToConnect
    {
       
        public string PlannedActivityName { get; set; }
        public int NumberOfNodes { get; set; }

        public int NumberOfLabNodes { get; set; }
        public IDictionary<long, PlannedActivityToConnectData> LinkedToPlannedActivity { get; set; }
        public IDictionary<long, string> LinkedDesignComponent { get; set; }
        
    }

    public class PlannedActivityToConnectData
    {
        public string Name { get; set; }
        public int NumberOfNode { get; set; }
    }
}
