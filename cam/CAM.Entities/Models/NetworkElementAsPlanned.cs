using CAM.Entities.Models.Base;
using CAM.Entities.Models.Cross;
using CAM.Entities.Models.Lookup;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace CAM.Entities.Models
{
    [Table("NetworkElementsAsPlanned")]
    public class NetworkElementAsPlanned : AuditableEntity
    {

        public NetworkElementAsPlanned()
        {
            NetworkElementAsPlannedEduSpoc = new List<NetworkElementAsPlannedEduSpoc>();
            NetworkElementAsPlannedSubDomainSpoc = new List<NetworkElementAsPlannedSubDomainSpoc>();
            PlannedActivities = new List<PlannedActivity>();
            Networkelementsasis = new List<NetworkElementAsIs>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("NetworkElementAsPlannedId")]
        public long NetworkElementAsPlannedId { get; set; }
        public short OpCoId { get; set; }
        public long DesignComponentId { get; set; }
        public long? DesignComponentFamilyId { get; set; }
        public string? VodafoneName { get; set; }

        public short? OriginalEquipmentManufacturerId { get; set; }
        public string ElementName { get; set; }
        public string CapacityPlanReference { get; set; }
        public string AdditionalInformation1 { get; set; }
        public string AdditionalInformation2 { get; set; }
        public bool AutomatedFeedback { get; set; }
        public bool PlannedAction { get; set; }
        public string NetworkConstruct { get; set; }
        public short EnvironmentId { get; set; }
        public short DeploymentStatusId { get; set; }
        public short? DeploymentTypeId { get; set; }
        public short? LocationId { get; set; }
        public short? NfviBundleIDId { get; set; }  
        public bool? IsFinalAsset { get; set; }
        public string HwResourceKey { get; set; }
        public string PreviousHWResourceKey { get; set; }
        public string SwResourceKey { get; set; }
        public string PreviousSWResourceKey { get; set; }
        public long? LcmEngineeringId { get; set; }

        public DateTime? BomSubmittedDate { get; set; }
        public DateTime? HwPoRaisedDate { get; set; }
        public DateTime? HwPoArrivedDate { get; set; }
        public DateTime? RfaDate { get; set; }
        public List<int?> NetworkElementEduSpocIdList { get; set; }
        public List<int?> NetworkElementSubDomainSpocIdList { get; set; }

        public Dictionary<long, string> PlannedActivityDictonary { get; set; }

        public Dictionary<short,string> VerticalFilterDto { get; set; }
        public string VerticalNameResponse { get; set; }

        public List<int> VerticalNameResponseId { get; set; }

        public long Buildbagid { get; set; }
        public DateTime? AssetLiveStatusDate { get; set; }
        public DateTime? AssetDecommissionedDate { get; set; }

        public DateTime? AssetRfoDate { get; set; }
        public DateTime? AssetRfsDate { get; set; }

        public string ElementDomianName { get; set; }
        public bool? IsAssured { get; set; }

        public virtual BuildBag Buildbag { get; set; }

        public virtual SystemNames DomianName { get; set; }

        [ForeignKey(nameof(OpCoId))]
        [InverseProperty("Networkelementsasplanned")]
        public virtual OpCo OpCo { get; set; }

        [ForeignKey(nameof(DesignComponentId))]
        [InverseProperty("Networkelementsasplanned")]
        public virtual DesignComponent DesignComponent { get; set; }

        [ForeignKey(nameof(EnvironmentId))]
        [InverseProperty("Networkelementsasplanned")]
        public virtual Lookup.Environment Environment { get; set; }

        [ForeignKey(nameof(DeploymentStatusId))]
        [InverseProperty("Networkelementsasplanned")]
        public virtual DeploymentStatus DeploymentStatus { get; set; }

        [ForeignKey(nameof(DeploymentTypeId))]
        [InverseProperty("Networkelementsasplanned")]
        public virtual DeploymentType DeploymentType { get; set; }

        [ForeignKey(nameof(LocationId))]
        [InverseProperty("Networkelementsasplanned")]
        public virtual Location Location { get; set; }

        [ForeignKey(nameof(NfviBundleIDId))]
        [InverseProperty("Networkelementsasplanned")]
        public virtual NFVIBundleID NFVIBundleID { get; set; }

        [ForeignKey(nameof(LcmEngineeringId))]
        public virtual LcmEngineering LcmEngineering { get; set; }

        public List<PlannedActivity> PlannedActivities { get; set; }
        public List<NetworkElementAsPlannedEduSpoc> NetworkElementAsPlannedEduSpoc { get; set; }
        public List<NetworkElementAsPlannedSubDomainSpoc> NetworkElementAsPlannedSubDomainSpoc { get; set; }

        [InverseProperty(nameof(NetworkElementAsIs.NetworkElementAsPlanned))]
        public List<NetworkElementAsIs> Networkelementsasis { get; set; }


        public object GetDesignComponentDescriptionOrAlias(NetworkElementAsPlanned p)
        {
            var subNetworkBoundary = p.DesignComponent?.DesignComponentFamily?.SubNetworkBoundary;
            return string.IsNullOrEmpty(subNetworkBoundary?.Alias) ? subNetworkBoundary?.Description : subNetworkBoundary?.Alias;
        }


        public object GetDeploymentTypeDescription(NetworkElementAsPlanned p)
        {
            return p.DeploymentType?.DeploymentTypeDescription ?? "";
        }
 
        public object GetNFVIBundleIdDescription(NetworkElementAsPlanned p)
        {
            return p.NFVIBundleID?.NFVIBundleIdDescription ?? "";
        }
        public object GetPlannedImplementationYear(NetworkElementAsPlanned p)
        {
            return p.PlannedActivities?.FirstOrDefault()?.PlannedImplementationYear;
        }
        public object GetActivityStatusDescription(NetworkElementAsPlanned p)
        {
            return p.PlannedActivities?.FirstOrDefault()?.ActivityStatus?.ActivityStatusDescription ?? "";
        }

        public object GetPlanningActivityStatusDescription(NetworkElementAsPlanned p)
        {
            return p.PlannedActivities?.FirstOrDefault()?.PlanningActivityStatus?.PlanningActivityStatusDescription ?? "";
        }

    }
    public class FilterValueDtoKeyValueList
    {
        public int Key { get; set; }
        public string Value { get; set; }
    }
}
