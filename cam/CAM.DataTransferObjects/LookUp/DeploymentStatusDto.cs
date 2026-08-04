using System;
using System.Collections.Generic;
using System.ComponentModel;
using CAM.DataAttributes.Grid;
using CAM.DataTransferObjects.QueryDto.Base;

namespace CAM.DataTransferObjects.LookUp
{
    public class DeploymentStatusDto
    {
        public short DeploymentStatusId { get; set; }
        public string DeploymentStatusDescription { get; set; }
        public int? Rule { get; set; }

      //  public List<string> PlannedActivityResourceAllowed { get; set; }
        public List<short> PlannedActivityResourceAllowedId { get; set; }
        public bool ReadOnlyPlannedActivity { get; set; }
        public bool CheckPlannedActivity { get; set; }
        public bool DefaultValue { get; set; }


        [DateRangeGrid]
        public DateTime? LastModified { get; set; }
        public string LastModifiedBy { get; set; }

        public IDictionary<short, string> PlannedActivityResources { get; set; }

    }
    public class DeploymentStatusDtoGrid : GridDtoBase
    {
        [OrderGrid(Order = 1)]
        [Default]
        public short DeploymentStatusId { get; set; }
        [OrderGrid(Order = 2)]
        [Default]
        public string DeploymentStatusDescription { get; set; }

        [OrderGrid(Order = 3)]
        [Default]
        public int? Rule { get; set; }

        [OrderGrid(Order = 4)]
        [Default]
        public string PlannedActivityResourceAllowed { get; set; }
        [IgnoreGrid]
        public List<short> PlannedActivityResourceAllowedId { get; set; }

        [OrderGrid(Order = 5)]
        [Default]
        public bool ReadOnlyPlannedActivity { get; set; }
        [OrderGrid(Order = 6)]
        [Default]
        public bool CheckPlannedActivity { get; set; }
        [OrderGrid(Order = 7)]
        [Default]
        public bool DefaultValue { get; set; }
        [DateRangeGrid]
        [OrderGrid(Order = 8)]
        [Default]
        public DateTime? LastModified { get; set; }
        [OrderGrid(Order = 9)]
        [MailTo]
        [DisplayName("Last Modified By")]
        [Default]
        public string LastModifiedBy { get; set; }
    }
    public class DeploymentStatusQuery : QueryObject
    {
        public List<short> DeploymentStatusId { get; set; }
        public List<string> DeploymentStatusDescription { get; set; }
        public List<int> Rule { get; set; }
        public List<string> PlannedActivityResourceAllowed { get; set; }
        public List<bool> ReadOnlyPlannedActivity { get; set; }
        public List<bool> CheckPlannedActivity { get; set; }
        public List<bool> DefaultValue { get; set; }
    }
}
