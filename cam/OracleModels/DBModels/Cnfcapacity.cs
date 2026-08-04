using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Cnfcapacity
    {
        public long Cnfcapacityid { get; set; }
        public long Cnfpodinfoid { get; set; }
        public short Financialyear { get; set; }
        public int Financialversion { get; set; }
        public long? Vcpurequestforpodtype { get; set; }
        public long? Vcpulimitforpodtype { get; set; }
        public long? Pcpurequestforpodtype { get; set; }
        public decimal? Memrequestforpodtype { get; set; }
        public decimal? Memlimitforpodtype { get; set; }
        public string Nonpresistentstorageforprodtype { get; set; }
        public bool? Ispresistentvolumesrequired { get; set; }
        public string Persistentvolumneaccessmode { get; set; }
        public string Persistentstorageforpodtype { get; set; }
        public string Storageiopsforpodtype { get; set; }
        public string Storagerworkloaddistribution { get; set; }
        public string Northsouthbandwidthforpodtype { get; set; }
        public string Eastwestbandwidthforpodtype { get; set; }
        public string Specialrequirementperpodtype { get; set; }
        public string Capacityspecialrequirement { get; set; }
        public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public int Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }
        public int? Noofcnfinstancespersite { get; set; }
        public int? Numberofpodsperpodtype { get; set; }

        public virtual Cnfpodinfo Cnfpodinfo { get; set; }
        public virtual Aspnetusers CreationuserNavigation { get; set; }
        public virtual Aspnetusers ModificationuserNavigation { get; set; }
    }
}
