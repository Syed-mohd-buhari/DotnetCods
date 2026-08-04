using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto.Base;
using System.Collections.Generic;

namespace CAM.DataTransferObjects.QueryDto
{
    public class DaAssetMigrationQueryDto : QueryObject
    {
        public List<long> DaAssetMigrationId { get; set; }
        public List<long> PlannedActivityId { get; set; }
        public List<long> NetworkElementAsPlannedId { get; set; }
        public List<string> NewelEmentName { get; set; }
        public List<long>? TargetDesignComponenetId { get; set; }
        public List<short>? EnvironmentDesc { get; set; }
        public List<short>? DeploymentStatusDesc { get; set; }       
        public List<short> OpcoDesc { get; set; }
        public List<short> LocationDesc { get; set; }
        public DateFilter? RfoDate { get; set; }
        public DateFilter? RfsDate { get; set; }
        public DateFilter? MigrationCompletionDate { get; set; }
        public List<string> TrafficNodePercentage { get; set; } 
        public List<string> OldAssetName { get; set; }        
        public List<string> CurrentDesignComponenet { get; set; } 
        public List<string>? TargetDesignComponenet { get; set; } 
        public List<string>? NewEnvironment { get; set; }
        public List<string>? NewDeploymentStatus { get; set; } 
        public List<string>? Location { get; set; }
         
        public List<string> OldEnvironment { get; set; }
       
        public List<string> OldDeploymentType { get; set; }
      
        public List<string> OldDeploymentStatus { get; set; }
        public List<long>? CurrentDcfId { get; set; }
        #region // UI request for remove auto filter 
        public List<short> OpcoId { get; set; }
        public bool IsPagination { get; set; } = true;

        #endregion
        public DateFilter? HwPoRaisedDate { get; set; }
        public DateFilter? HwPoArrivedDate { get; set; }
        public DateFilter? BomSubmittedDate { get; set; }
        public DateFilter? RfaDate { get; set; }
        public List<string> VerticalName { get; set; }
        public List<string>? isDecommissioned { get; set; }
        public DateFilter? VecDate { get; set; }
        public DateFilter? StartOfAppIntegration { get; set; }
        public DateFilter? MigrationStart { get; set; }


    }

    public class ExodusQueryLevel3ReportQueryDto 
    {
        public List<long> DaAssetMigrationId { get; set; }
        public List<long> PlannedActivityId { get; set; }
        public List<long> NetworkElementAsPlannedId { get; set; }
        public List<string> NewelEmentName { get; set; }
        public List<long> TargetDesignComponenetId { get; set; }
        public List<short> OpcoDesc { get; set; }
        public List<short> LocationDesc { get; set; }
        public DateFilter RfoDate { get; set; }
        public DateFilter RfsDate { get; set; }
        public DateFilter MigrationCompletionDate { get; set; }
        public List<string> TrafficNodePercentage { get; set; }
        public List<string> CurrentDesignComponenet { get; set; }
        public List<string> TargetDesignComponenet { get; set; }
        public List<string> Location { get; set; }
        public List<long> CurrentDcfId { get; set; }
        public List<long> PlannedDcfId { get; set; }
        public List<string> PlannedDcfName { get; set; }
        public List<long> OpcoId { get; set; }
        public DateFilter HwPoRaisedDate { get; set; }
        public DateFilter HwPoArrivedDate { get; set; }
        public DateFilter BomSubmittedDate { get; set; }
        public DateFilter BomStartDate { get; set; }
        public DateFilter RfaDate { get; set; }
        public List<string> VerticalName { get; set; }
        public List<long> EnvironmentId { get; set; }
        public List<long> PlatformId { get; set; }
        public List<long> ProductId { get; set; }
    }
}
