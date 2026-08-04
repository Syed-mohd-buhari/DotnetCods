using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Aspnetroleclaims
    {
        public int Id { get; set; }
        public int Roleid { get; set; }
        public string Claimvalue { get; set; }
        public string Claimtype { get; set; }

        public virtual Aspnetroles Role { get; set; }
    }
}
