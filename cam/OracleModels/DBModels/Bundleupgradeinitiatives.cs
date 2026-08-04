using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Bundleupgradeinitiatives
    {
        public long Bundleupgradeinitiativeid { get; set; }
        public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public int Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }
        public short Orgeqpmanufacturerid { get; set; }
        public string Spare1json { get; set; }
        public string Remarks { get; set; }
        public string Oemcertifiedrelease { get; set; }
        public string Verticalowner { get; set; }
        public string Vnftype { get; set; }

        public virtual Aspnetusers CreationuserNavigation { get; set; }
        public virtual Aspnetusers ModificationuserNavigation { get; set; }
        public virtual Originalequipmentmanufacturers Orgeqpmanufacturer { get; set; }
    }
}
