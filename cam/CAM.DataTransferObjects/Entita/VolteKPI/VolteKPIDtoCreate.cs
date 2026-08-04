using CAM.Infrastucture.Enums;
using System.Collections.Generic;

namespace CAM.DataTransferObjects.Entita.VolteKPI
{
    public class VolteKPIDtoCreate : VolteKPIDto
    {
        public VolteKPIDtoCreate()
        {
            KPIOneActualValue = 0;
            KPIOneComment = "";
            KPIOneEoYTarget = 0;
            KPIOneMonthlyTarget = 0;
            KPIOneTargetValueChangeProposal = null;
            //KPITwoActualValue = 0;
            KPITwoComment = "";
            KPITwoActualNumberOfRegisteredSubscribers = 0;
            KPITwoActualNumberOfProvisionedSubscriber = 0;
            //KPITwoTargetValueChangeProposal = null;
            KPIThreeActualValue = 0;
            KPIThreeComment = "";
            KPIThreeEoYTarget = 0;
            KPIThreeMonthlyTarget = 0;
            KPIThreeTargetValueChangeProposal = null;
            KPIFourTargetDateMonth = 0;
            KPIFourTargetDateYear = 0;
            KPIFourActualMonthly = 0;
            KPIFourComment = "";
            KPIFourFinalTarget = 0;
            KPIFourTargetMonthly = 0;
            KPIFourTargetValueChangeProposal = null;
            OpCoResource = new Dictionary<short, string>();
            VolteKPITypeResource = new Dictionary<int, string>();
        }
        public long VolteKPIId { get; set; }
        public short OpCoId { get; set; }
        public VolteKPIType VolteKPIType { get; set; }
        public IDictionary<short, string> OpCoResource { get; set; }
        public IDictionary<int, string> VolteKPITypeResource { get; set; }
    }
}
