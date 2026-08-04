using CAM.DataAttributes.Grid;
using CAM.DataTransferObjects.QueryDto.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;

namespace CAM.DataTransferObjects.Entita.DaAsssetMigration
{
    public class DaAssetMigrationDtoGrid : GridDtoBase
    {
        [DisplayName("DaAssetMigrationId")]
        [OrderGrid(Order = 1)]
        [IgnoreGrid]
        public long DaAssetMigrationId { get; set; }

        [DisplayName("PA Id")]
        [OrderGrid(Order = 2)]
        public long PlannedActivityId { get; set; }


        [DisplayName("Node Index")]
        [OrderGrid(Order = 3)]
        public long? NetworkElementAsPlannedId { get; set; }

        [OrderGrid(Order = 4)]
        [Default]
        [DisplayName("Element Name")]

        public string OldAssetName { get; set; }

        [OrderGrid(Order = 5)]
        [IgnoreGrid]
        [DisplayName("Current DC Id")]
        public long? CurrentDcId { get; set; }

        [OrderGrid(Order = 6)]
        [IgnoreGrid]
        [DisplayName("Current DC")]
        public String CurrentDesignComponenet { get; set; }

        [OrderGrid(Order = 7)]
        [Default]
        [DisplayName("New Element Name")]
        public string NewelEmentName { get; set; }

        [OrderGrid(Order = 8)]
        [Default]
        [DisplayName("Target DC")]
        public string? TargetDesignComponenet { get; set; }

        [OrderGrid(Order = 9)]
        [IgnoreGrid]
        [DisplayName("Target DC Id")]
        public long? TargetDesignComponenetId { get; set; }

        [OrderGrid(Order = 10)]
        [IgnoreGrid]
        [DisplayName("EnvironmentId")]
        public short? NewEnvironmentId { get; set; }

        [OrderGrid(Order = 11)]
        [Default]
        [DisplayName("Old Deployment Status")]
        public string OldDeploymentStatus { get; set; }
        [OrderGrid(Order = 12)]
        [Default]
        [DisplayName("Environment")]
        public string? NewEnvironment { get; set; }

        [OrderGrid(Order = 13)]
        [IgnoreGrid]
        [DisplayName("DeploymentStatusId")]
        public short? NewDeploymentStatusId { get; set; }

        [OrderGrid(Order = 14)]
        [Default]
        [DisplayName("Deployment Status")]
        public string? NewDeploymentStatus { get; set; }

        [OrderGrid(Order = 15)]
        [IgnoreGrid]
        [DisplayName("OpcoId")]
        public short OpcoId { get; set; }

        [OrderGrid(Order = 16)]
        [IgnoreGrid]
        [DisplayName("Opco")]
        public string OpcoDesc { get; set; }

        [OrderGrid(Order = 17)]
        [IgnoreGrid]
        [DisplayName("LocationId")]
        public short? LocationId { get; set; }

        [OrderGrid(Order = 18)]
        [Default]
        [DisplayName("Location")]
        public string? Location { get; set; }

        [OrderGrid(Order = 19)]
        [Default]
        [DateRangeGrid]
        [DisplayName("Bom Submitted Date")]
        public DateTime? BomSubmittedDate { get; set; }

        [OrderGrid(Order = 20)]
        [Default]
        [DateRangeGrid]
        [DisplayName("Hardware Po Raised Date")]
        public DateTime? HwPoRaisedDate { get; set; }

        [OrderGrid(Order = 21)]
        [Default]
        [DateRangeGrid]
        [DisplayName("Hardware Po Arrived Date")]
        public DateTime? HwPoArrivedDate { get; set; }

        [OrderGrid(Order = 22)]
        [Default]
        [DateRangeGrid]
        [DisplayName("VEC infra ready /CNIS -Workload cluster config")]
        public DateTime? VecDate { get; set; }

        [OrderGrid(Order = 23)]
        [Default]
        [DateRangeGrid]
        [DisplayName("Rfo Date")]
        public DateTime? RfoDate { get; set; }

        [OrderGrid(Order = 24)]
        [Default]
        [DateRangeGrid]
        [DisplayName("Start of app Integration")]
        public DateTime? StartOfAppIntegration { get; set; }

        [OrderGrid(Order = 25)]
        [Default]
        [DateRangeGrid]
        [DisplayName("Rfa Date")]
        public DateTime? RfaDate { get; set; }

        [OrderGrid(Order = 26)]
        [Default]
        [DateRangeGrid]
        [DisplayName("Rfs Date")]
        public DateTime? RfsDate { get; set; }


        [OrderGrid(Order = 27)]
        [Default]
        [DateRangeGrid]
        [DisplayName("Migration Start")]
        public DateTime? MigrationStart { get; set; }


        [OrderGrid(Order = 28)]
        [Default]
        [DateRangeGrid]
        [DisplayName("Migration Completion Date")]
        public DateTime? MigrationCompletionDate { get; set; }


        [OrderGrid(Order = 29)]
        [Default]
        [DisplayName("Traffic Node Percentage")]
        public string TrafficNodePercentage { get; set; }

        [IgnoreGrid]
        public string OldEnvironment { get; set; }
        [IgnoreGrid]
        public string OldDeploymentType { get; set; }



        [IgnoreGrid] // UI purpose - uniqueValue
        public long UniqueIdForUi { get; set; }

        [OrderGrid(Order = 30)]
        [Default]
        [DisplayName("Is Decommissioned")]
        public bool? IsDecommissioned { get; set; }

    }

    public class ExodusAssetLevelGrid
    {
        public long DaAssetMigrationId { get; set; }
        public long PlannedActivityId { get; set; }
        public long? NetworkElementAsPlannedId { get; set; }
        public long? CurrentDcId { get; set; }
        public String CurrentDesignComponenet { get; set; }
        public string NewelEmentName { get; set; }
        public string? TargetDesignComponenet { get; set; }
        public long? TargetDesignComponenetId { get; set; }
        public short OpcoId { get; set; }
        public string OpcoDesc { get; set; }
        public short? LocationId { get; set; }
        public string? Location { get; set; }
        public DateTime? RfoDate { get; set; }
        public DateTime? RfsDate { get; set; }
        public DateTime? MigrationCompletionDate { get; set; }
        public DateTime? BomSubmittedDate { get; set; }
        public DateTime? HwPoRaisedDate { get; set; }
        public DateTime? HwPoArrivedDate { get; set; }
        public DateTime? RfaDate { get; set; }
        public DateTime? BomStartDate { get; set; }
        public DateTime? PlannedStartDate { get; set; }
        public DateTime? VecDate { get; set; }
        public DateTime? StartOfAppIntegration { get; set; }
        public DateTime? MigrationStart { get; set; }
        public string PLannedDcfName { get; set; }
        public long? PlannedDcfId { get; set; }
        public List<ExodusMilestoneAndActivityGrid> ActivityDetails { get; set; }
    }

    public class ExodusMilestoneAndActivityGrid
    {
        public long ExodusMilestoneAndActivityId { get; set; }
        public string ActivityDescription { get; set; }
        public DateTime? ActivityStartDate { get; set; }
        public DateTime? ActivityEndDate { get; set; }
        public int? ActivityOrder { get; set; }
        public string MileStoneDescription { get; set; }
        public string ActualColumnName { get; set; }
    }
    public class ExodusAssetLevelReportGrid
    {
        public string Location { get; set; }
        public long? SelectedOpco { get; set; }
        public string ProductName { get; set; }
        public string Platform { get; set; }
        public List<long> SelectedPlannedDcf { get; set; }
        public List<ExodusAssetLevelGrid> AssetDetails { get; set; }

    }
}
