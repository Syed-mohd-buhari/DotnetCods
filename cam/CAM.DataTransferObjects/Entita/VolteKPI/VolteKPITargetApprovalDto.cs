using CAM.Infrastucture.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace CAM.DataTransferObjects.Entita.VolteKPI
{
    public class VolteKPITargetApprovalDto
    {
        public long VolteKPIId { get; set; }
        public short Month { get; set; }
        public short Year { get; set; }
        public VolteKPIType Type { get; set; }
        public short OpCoId { get; set; }
        public string KPIIDName { get; set; }
        public string OpCo { get; set; }
        public decimal? MonthlyTargetPrevious { get; set; }
        public decimal? MonthlyTargetRequested { get; set; }
        public decimal? MonthlyTargetNew { get; set; }
        public string Comment { get; set; }
        public bool ApproveValueChangeProposal { get; set; }
        public string ProposalDate { get; set; }
        public string ProposedBy { get; set; }
    }
}
