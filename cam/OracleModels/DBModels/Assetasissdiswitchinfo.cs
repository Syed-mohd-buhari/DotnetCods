using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Assetasissdiswitchinfo
    {
        public long Assetasissdiswitchinfoid { get; set; }
        public string Datasourcename { get; set; }
        public string Switchname { get; set; }
        public string Switchid { get; set; }
        public string Switchadminstate { get; set; }
        public string Switchuniqueid { get; set; }
        public string Switchrole { get; set; }
        public string Switchrack { get; set; }
        public string Switchlabel { get; set; }
        public string Switchserialnumber { get; set; }
        public string Switchopsstate { get; set; }
        public string Switchnetwork { get; set; }
        public string Switchmanufacturer { get; set; }
        public string Switchmodel { get; set; }
        public string Switchipaddress { get; set; }
        public string Switchswversion { get; set; }
        public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public int Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }

        public virtual Aspnetusers CreationuserNavigation { get; set; }
        public virtual Aspnetusers ModificationuserNavigation { get; set; }
    }
}
