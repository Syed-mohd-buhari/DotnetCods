
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using CAM.Entities.Models.Base;
using CAM.Entities.Models.Cross;
using OracleModels.DBModels;

namespace CAM.Entities.Models.Lookup
{
  
    public partial class SubNetworkBoundary : AuditableEntity
    {
        public SubNetworkBoundary()
        {
            SupportedServices = new List<SubNetworkBoundary_SupportedService>();
            SystemFunctions = new List<SubnetworkBoundarySystemFunction>();
            CustomerWheels = new List<SubnetworkBoundaryCustomerWheel>();
        }
        public long Id { get; set; }
        public string Description { get; set; }
        public bool Default { get; set; }
        public string Alias { get; set; }
        public int? Order { get; set; }

        public bool? GdprRelevant { get; set; }
        public bool? InternetFacing { get; set; }
        public short? LcmPolicy { get; set; }
        public string Criticality { get; set; }
        public int? GDPRClassification { get; set; }
        public bool? Pcisox { get; set; }
        public bool? C3C4 { get; set; }
        public bool? MissionCritical { get; set; }
        public string GdrpClassificationValue { get; set; }

        public bool? SecurityElement { get; set; }

        public CriticalAssetType? CriticalAssetType { get; set; }
        public List<DesignComponentFamily> DesignComponentFamilies { get; set; }
        public List<SubNetworkBoundary_SupportedService> SupportedServices { get; set; }
        public List<SubnetworkBoundarySystemFunction> SystemFunctions { get; set; }
        public List<SubnetworkBoundaryCustomerWheel> CustomerWheels { get; set; }

        public int? VodafoneNameId { get; set; }

        [ForeignKey(nameof(VodafoneNameId))]
        [InverseProperty(nameof(Lookup.VodafoneNames))]
        public virtual VodafoneNames VodafoneName { get; set; }
    }
}
