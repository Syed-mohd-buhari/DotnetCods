using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Vnfvmcapacity
    {
        public long Vnfvmcapacityid { get; set; }
        public long Vnfinfoid { get; set; }
        public short Financialyear { get; set; }
        public long Vnfcpupervm { get; set; }
        public bool? Rxtxcpucount { get; set; }
        public long Rampervm { get; set; }
        public long Datadisk { get; set; }
        public long? Osdisk { get; set; }
        public string Iopsrunning { get; set; }
        public string Iopsloading { get; set; }
        public string Vmworkloaddistribution { get; set; }
        public string Northsouthboundbandwidth { get; set; }
        public string Eastwestboundbandwidth { get; set; }
        public string Otherrequirements { get; set; }
        public bool? Backuprequired { get; set; }
        public bool? Probingrequired { get; set; }
        public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public int Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }
        public int Financialversion { get; set; }
        public long? Noofvnfinstances { get; set; }
        public long? Noofvmspertype { get; set; }

        public virtual Aspnetusers CreationuserNavigation { get; set; }
        public virtual Aspnetusers ModificationuserNavigation { get; set; }
        public virtual Vnfinfo Vnfinfo { get; set; }
    }
}
