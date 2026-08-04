using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Identitiesasis
    {
        public int Id { get; set; }
        public string Value { get; set; }
        public string Resourcekey { get; set; }
        public string Previousresourcekey { get; set; }
        public long Assetid { get; set; }
        public int? Categoryid { get; set; }
        public int? Classid { get; set; }
        public int? Typeid { get; set; }
        public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public int Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }
        public string Interfacetype { get; set; }
        public string Interfacename { get; set; }

        public virtual Networkelementsasplanned Asset { get; set; }
        public virtual Categories Category { get; set; }
        public virtual Classes Class { get; set; }
        public virtual Aspnetusers CreationuserNavigation { get; set; }
        public virtual Aspnetusers ModificationuserNavigation { get; set; }
        public virtual Types Type { get; set; }
    }
}
