using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAM.DataTransferObjects.QueryDto
{
    public class NetworkElementAsPlannedGetLCMIDQueryDto
    {
        public List<short> OpCo { get; set; }
        public List<long> DesignComponent { get; set; }
        public List<short> DeploymentStatus { get; set; }
    }
}
