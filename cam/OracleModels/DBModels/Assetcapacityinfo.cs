using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Assetcapacityinfo
    {
        public long Assetcapacityinfoid { get; set; }
        public long Assethardwareancillaryid { get; set; }
        public string Physicalserverhostname { get; set; }
        public string Physicalserveripaddress { get; set; }
        public string Physicalserverserialnumber { get; set; }
        public long? Noofinstances { get; set; }
        public long? Vcpu { get; set; }
        public decimal? Memory { get; set; }
        public decimal? Storage { get; set; }
        public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public int Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }

        public virtual Assethardwareancillary Assethardwareancillary { get; set; }
        public virtual Aspnetusers CreationuserNavigation { get; set; }
        public virtual Aspnetusers ModificationuserNavigation { get; set; }
    }
}
