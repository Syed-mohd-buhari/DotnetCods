using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Aspnetuserclaims
    {
        public int Id { get; set; }
        public int Userid { get; set; }
        public string Claimtype { get; set; }
        public string Claimvalue { get; set; }

        public virtual Aspnetusers User { get; set; }
    }
}
