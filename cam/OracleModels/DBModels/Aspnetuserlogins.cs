using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Aspnetuserlogins
    {
        public string Loginprovider { get; set; }
        public string Providerkey { get; set; }
        public int Userid { get; set; }
        public string Providerdisplayname { get; set; }

        public virtual Aspnetusers User { get; set; }
    }
}
