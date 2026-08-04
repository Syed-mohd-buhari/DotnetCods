using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Opcos
    {
        public Opcos()
        {
            Aspnetuseropcos = new HashSet<Aspnetuseropcos>();
            Budgetprojecttrackers = new HashSet<Budgetprojecttrackers>();
            Buildbags = new HashSet<Buildbags>();
            Cnfclusterinfo = new HashSet<Cnfclusterinfo>();
            Daassetmigration = new HashSet<Daassetmigration>();
            Damigrationstatus = new HashSet<Damigrationstatus>();
            Datacenter = new HashSet<Datacenter>();
            Designaspects = new HashSet<Designaspects>();
            Infraclusterasplanned = new HashSet<Infraclusterasplanned>();
            Lcmengineering = new HashSet<Lcmengineering>();
            Locations = new HashSet<Locations>();
            Networkelementsasis = new HashSet<Networkelementsasis>();
            Networkelementsasplanned = new HashSet<Networkelementsasplanned>();
            Nfvitransitions = new HashSet<Nfvitransitions>();
            Plannedactivities = new HashSet<Plannedactivities>();
            Resourcekeymaster = new HashSet<Resourcekeymaster>();
            Serviceplan = new HashSet<Serviceplan>();
            Systemverificationproblems = new HashSet<Systemverificationproblems>();
            Vnfclusterinfo = new HashSet<Vnfclusterinfo>();
            Vnftransitions = new HashSet<Vnftransitions>();
            Voltekpi = new HashSet<Voltekpi>();
            Voltekpiworklog = new HashSet<Voltekpiworklog>();
        }

        public short Opcoid { get; set; }
        public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public int Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }
        public string Opco { get; set; }

        public virtual Aspnetusers CreationuserNavigation { get; set; }
        public virtual Aspnetusers ModificationuserNavigation { get; set; }
        public virtual ICollection<Aspnetuseropcos> Aspnetuseropcos { get; set; }
        public virtual ICollection<Budgetprojecttrackers> Budgetprojecttrackers { get; set; }
        public virtual ICollection<Buildbags> Buildbags { get; set; }
        public virtual ICollection<Cnfclusterinfo> Cnfclusterinfo { get; set; }
        public virtual ICollection<Daassetmigration> Daassetmigration { get; set; }
        public virtual ICollection<Damigrationstatus> Damigrationstatus { get; set; }
        public virtual ICollection<Datacenter> Datacenter { get; set; }
        public virtual ICollection<Designaspects> Designaspects { get; set; }
        public virtual ICollection<Infraclusterasplanned> Infraclusterasplanned { get; set; }
        public virtual ICollection<Lcmengineering> Lcmengineering { get; set; }
        public virtual ICollection<Locations> Locations { get; set; }
        public virtual ICollection<Networkelementsasis> Networkelementsasis { get; set; }
        public virtual ICollection<Networkelementsasplanned> Networkelementsasplanned { get; set; }
        public virtual ICollection<Nfvitransitions> Nfvitransitions { get; set; }
        public virtual ICollection<Plannedactivities> Plannedactivities { get; set; }
        public virtual ICollection<Resourcekeymaster> Resourcekeymaster { get; set; }
        public virtual ICollection<Serviceplan> Serviceplan { get; set; }
        public virtual ICollection<Systemverificationproblems> Systemverificationproblems { get; set; }
        public virtual ICollection<Vnfclusterinfo> Vnfclusterinfo { get; set; }
        public virtual ICollection<Vnftransitions> Vnftransitions { get; set; }
        public virtual ICollection<Voltekpi> Voltekpi { get; set; }
        public virtual ICollection<Voltekpiworklog> Voltekpiworklog { get; set; }
    }
}
