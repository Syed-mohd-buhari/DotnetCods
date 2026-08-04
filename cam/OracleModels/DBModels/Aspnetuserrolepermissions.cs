using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Aspnetuserrolepermissions
    {
        public int Aspnetuserrolepermissionid { get; set; }
        public int? Roleid { get; set; }
        public int? Moduleid { get; set; }
        public short? Permissionlevel { get; set; }
        public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public int Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }

        public virtual Aspnetusers CreationuserNavigation { get; set; }
        public virtual Aspnetusers ModificationuserNavigation { get; set; }
        public virtual Aspnetmodules Module { get; set; }
        public virtual Aspnetroles Role { get; set; }
    }
}
