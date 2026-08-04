using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Designaspects
    {
        public Designaspects()
        {
            Designaspectsnetworkfunctions = new HashSet<Designaspectsnetworkfunctions>();
            Designaspectssupportedsvr = new HashSet<Designaspectssupportedsvr>();
            Plannedactivities = new HashSet<Plannedactivities>();
        }

        public long Id { get; set; }
        public long Designcomponentfamilyid { get; set; }
        public short? Opcoid { get; set; }
        public string Description { get; set; }
        public int? Authenicationtypeid { get; set; }
        public int? Securitymanagerid { get; set; }
        public int? Licensemodelid { get; set; }
        public int? Thirdpartyaccessid { get; set; }
        public int? Siteresilienceid { get; set; }
        public int? Swdeliverylifecycleid { get; set; }
        public int? Instanceresilienceid { get; set; }
        public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public int Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public DateTime? Deletiondate { get; set; }
        public bool? Deleted { get; set; }
        public bool Criticalnationalinfrastructure { get; set; }
        public short? Securitytirezoneid { get; set; }
        public bool? Archived { get; set; }
        public int? Businesscontinuitymethodid { get; set; }
        public string Nominalcapacitylimit { get; set; }
        public string Designedcapacitylimit { get; set; }
        public string Maxallowedloading { get; set; }
        public int? Criticalityrating { get; set; }

        public virtual Authenicationtypes Authenicationtype { get; set; }
        public virtual Businesscontinuitymethod Businesscontinuitymethod { get; set; }
        public virtual Aspnetusers CreationuserNavigation { get; set; }
        public virtual Designcomponentfamilies Designcomponentfamily { get; set; }
        public virtual Instanceresilience Instanceresilience { get; set; }
        public virtual Licensemodels Licensemodel { get; set; }
        public virtual Aspnetusers ModificationuserNavigation { get; set; }
        public virtual Opcos Opco { get; set; }
        public virtual Securitymanager Securitymanager { get; set; }
        public virtual Securitytirezone Securitytirezone { get; set; }
        public virtual Siteresilience Siteresilience { get; set; }
        public virtual Swdeliverylifecycle Swdeliverylifecycle { get; set; }
        public virtual Thirdpartyaccesstypes Thirdpartyaccess { get; set; }
        public virtual ICollection<Designaspectsnetworkfunctions> Designaspectsnetworkfunctions { get; set; }
        public virtual ICollection<Designaspectssupportedsvr> Designaspectssupportedsvr { get; set; }
        public virtual ICollection<Plannedactivities> Plannedactivities { get; set; }
    }
}
