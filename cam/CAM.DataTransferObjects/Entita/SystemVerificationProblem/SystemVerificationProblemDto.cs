using CAM.DataAttributes.Grid;
using CAM.DataTransferObjects.QueryDto.Base;
using DocumentFormat.OpenXml.Drawing.Charts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAM.DataTransferObjects.Entita.SystemVerificationProblem
{
    public class SystemVerificationProblemDto : GridDtoBase
    {

        [Default]
        [OrderGrid(Order = 8)]
        [DisplayName("Date Found")]
        [DateRangeGrid]
        public DateTime? DateFound { get; set; }


        [OrderGrid(Order = 9)]
        [DisplayName("Problem Category Id")]
        public long? ProblemCategoryId { get; set; }

        [OrderGrid(Order = 10)]
        [DisplayName("Problem Category ")]
        public string ProblemCategory { get; set; }

        [Default]
        [OrderGrid(Order = 11)]
        [DisplayName("Problem Description")]
        public string ProblemDescription { get; set; }

        [Default]
        [OrderGrid(Order = 12)]
        [DisplayName("Maintenance Reference")]
        public string MaintenanceReference { get; set; }

        [Default]
        [OrderGrid(Order = 13)]
        [DisplayName("Severity")]
        public int? Severity { get; set; }

        [Default]
        [OrderGrid(Order = 14)]
        [DisplayName("Severity Description")]
        public string SeverityDescription { get; set; }

        [Default]
        [OrderGrid(Order = 15)]
        [DisplayName("Status URL")]
        public string StatusUrl { get; set; }

        [Default]
        [OrderGrid(Order = 16)]
        [DisplayName("Mitigation")]
        public string Mitigation { get; set; }

        [Default]
        [OrderGrid(Order = 17)]
        [DisplayName("Solution Description")]
        public string SolutionDescription { get; set; }

        [Default]
        [OrderGrid(Order = 18)]
        [DisplayName("Patch Reference")]
        public string PatchReference { get; set; }

        [Default]
        [OrderGrid(Order = 19)]
        [DisplayName("Product Upgrade Reference")]
        public string ProductUpgradeReference { get; set; }

        [Default]
        [OrderGrid(Order = 20)]
        [DisplayName("Supplemental")]
        public string SuppleMental { get; set; }

        [Default]
        [OrderGrid(Order = 21)]
        [DisplayName("VendorCSR")]
        public string VendorCsr { get; set; }

        [Default]
        [OrderGrid(Order = 22)]
        [DisplayName("SubNetwork")]
        public string SubNetwork { get; set; }

        [Default]
        [OrderGrid(Order = 23)]
        [DisplayName("Test Report")]
        public string TestReport { get; set; }

        [Default]
        [OrderGrid(Order = 24)]
        [DisplayName("Standard NIR")]
        public string StandardNir { get; set; }

        [Default]
        [OrderGrid(Order = 25)]
        [DisplayName("Ericsson Sec Report")]
        public string EricssonSecReport { get; set; }

        [Default]
        [OrderGrid(Order = 26)]
        [DisplayName("SW and/or System Type Entries")]
        public string SwAndStEntries { get; set; }

        [Default]
        [OrderGrid(Order = 27)]
        [DisplayName("Pen Testing Report")]
        public string PenTestingReport { get; set; }
    }

    public class SystemVerificationProblemDtoGrid : SystemVerificationProblemDto
    {
        [Default]
        [OrderGrid(Order = 1)]
        [DisplayName("OpCo")]
        public string OpCoName { get; set; }

        [Default]
        [OrderGrid(Order = 2)]
        [DisplayName("Environment")]
        public string Environment { get; set; }

        [Default]
        [OrderGrid(Order = 3)]
        [DisplayName("Problem Id")]
        public string ProblemId { get; set; }

        [OrderGrid(Order = 4)]
        [DisplayName("System Verification Problem Id")]
        public long SystemVerificationProblemId { get; set; }

        [Default]
        [OrderGrid(Order = 5)]
        [DisplayName("OpCoId")]
        public short? OpCoId { get; set; }

        [Default]
        [OrderGrid(Order = 6)]
        [DisplayName("System Type Id")]
        public long? SystemTypeId { get; set; }

        [Default]
        [OrderGrid(Order = 7)]
        [DisplayName("Environment Id")]
        public short? EnvironmentId { get; set; }


    }
}
