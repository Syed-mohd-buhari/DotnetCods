using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Aspnetusertokens
    {
        public int Userid { get; set; }
        public string Loginprovider { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }

        public virtual Aspnetusers User { get; set; }
    }
}
