using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAM.DataTransferObjects.QueryDto
{
    public class SystemVerificationProblemQueryDto : QueryObject
    {
        public List<long> SystemVerificationProblemId { get; set; }
        public List<string> ProblemId { get; set; }
        public List<short> OpCoId { get; set; }
        public List<long> SystemTypeId { get; set; }
        public List<short> EnvironmentId { get; set; }
        public DateFilter DateFound { get; set; }
        public List<long> ProblemCategoryId { get; set; }
        public List<string> ProblemDescription { get; set; }
        public List<string> MaintenanceReference { get; set; }
        public List<int> Severity { get; set; }
        public List<string> StatusUrl { get; set; }
        public List<string> Mitigation { get; set; }
        public List<string> SolutionDescription { get; set; }
        public List<string> PatchReference { get; set; }
        public List<string> ProductUpgradeReference { get; set; }
        public List<string> SuppleMental { get; set; }
        public List<string> VendorCsr { get; set; }
        public List<string> SubNetwork { get; set; }

        public List<short> OpCoName { get; set; }

        public List<short> Environment { get; set; }

        public List<int> SeverityDescription { get; set; }

        public List<long> ProblemCategory { get; set; }

        public List<string> TestReport { get; set; }

        public List<string> StandardNir { get; set; }

        public List<string> EricssonSecReport { get; set; }

        public List<string> SwAndStEntries { get; set; }

        public List<string> PenTestingReport { get; set; }

    }
}
