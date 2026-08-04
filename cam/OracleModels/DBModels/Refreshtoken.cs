using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Refreshtoken
    {
        public int Id { get; set; }
        public int Userid { get; set; }
        public string Token { get; set; }
        public string Jwtid { get; set; }
        public bool? Isused { get; set; }
        public bool? Isrevoked { get; set; }
        public DateTime? Addeddate { get; set; }
        public DateTime? Expirydate { get; set; }

        public virtual Aspnetusers User { get; set; }
    }
}
