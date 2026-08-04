using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Vnftransitions
    {
        public long Vnftransitionid { get; set; }
        public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public int Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }
        public short Opcoid { get; set; }
        public short? Vnfdesigncomponentid { get; set; }
        public string Spare1json { get; set; }
        public short? Equipmentstatusid { get; set; }
        public short? Nfvibundleidid { get; set; }
        public string Currentrelease { get; set; }
        public string Elementname { get; set; }
        public string Location { get; set; }
        public string Plannedrelease { get; set; }
        public string Vnftype { get; set; }
        public string Nfvisitedesignation { get; set; }

        public virtual Aspnetusers CreationuserNavigation { get; set; }
        public virtual Equipmentstatuses Equipmentstatus { get; set; }
        public virtual Aspnetusers ModificationuserNavigation { get; set; }
        public virtual Nfvibundleids Nfvibundleid { get; set; }
        public virtual Opcos Opco { get; set; }
        public virtual Vfndesigncomponents Vnfdesigncomponent { get; set; }
    }
}
