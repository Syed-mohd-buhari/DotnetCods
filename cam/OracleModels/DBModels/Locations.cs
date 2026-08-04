using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Locations
    {
        public Locations()
        {
            Cnfclusterinfo = new HashSet<Cnfclusterinfo>();
            Daassetmigration = new HashSet<Daassetmigration>();
            Damigrationstatus = new HashSet<Damigrationstatus>();
            Infraclusterasplanned = new HashSet<Infraclusterasplanned>();
            Locationdeploymenttypes = new HashSet<Locationdeploymenttypes>();
            Networkelementsasis = new HashSet<Networkelementsasis>();
            Networkelementsasplanned = new HashSet<Networkelementsasplanned>();
            Sites = new HashSet<Sites>();
            Vnfclusterinfo = new HashSet<Vnfclusterinfo>();
        }

        public short Locationid { get; set; }
        public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public int Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }
        public short? Opcoid { get; set; }
        public bool Defaultvalue { get; set; }
        public string Location { get; set; }
        public string Shortdescription { get; set; }

        public virtual Aspnetusers CreationuserNavigation { get; set; }
        public virtual Aspnetusers ModificationuserNavigation { get; set; }
        public virtual Opcos Opco { get; set; }
        public virtual ICollection<Cnfclusterinfo> Cnfclusterinfo { get; set; }
        public virtual ICollection<Daassetmigration> Daassetmigration { get; set; }
        public virtual ICollection<Damigrationstatus> Damigrationstatus { get; set; }
        public virtual ICollection<Infraclusterasplanned> Infraclusterasplanned { get; set; }
        public virtual ICollection<Locationdeploymenttypes> Locationdeploymenttypes { get; set; }
        public virtual ICollection<Networkelementsasis> Networkelementsasis { get; set; }
        public virtual ICollection<Networkelementsasplanned> Networkelementsasplanned { get; set; }
        public virtual ICollection<Sites> Sites { get; set; }
        public virtual ICollection<Vnfclusterinfo> Vnfclusterinfo { get; set; }
    }
}
