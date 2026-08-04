using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Voltekpiworklog
    {
        public long Voltekpiworklogid { get; set; }
        public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public int Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }
        public long Voltekpiid { get; set; }
        public short Opcoid { get; set; }
        public short Month { get; set; }
        public short Year { get; set; }
        public int Voltekpitype { get; set; }
        public decimal? Targetmonthlyvalueold { get; set; }
        public decimal? Targetmonthlyvalueproposed { get; set; }
        public decimal? Targetmonthlyvaluenew { get; set; }
        public decimal? Eoytargetold { get; set; }
        public decimal? Eoytargetnew { get; set; }
        public decimal? Actualmonthlyvalueold { get; set; }
        public decimal? Actualmonthlyvaluenew { get; set; }
        public decimal? Actualnumberofregisteredold { get; set; }
        public decimal? Actualnumberofregisterednew { get; set; }
        public decimal? Actualnumberofprovisionedold { get; set; }
        public decimal? Actualnumberofprovisionednew { get; set; }
        public bool? Approved { get; set; }
        public bool Isstored { get; set; }
        public string Comments { get; set; }

        public virtual Aspnetusers CreationuserNavigation { get; set; }
        public virtual Aspnetusers ModificationuserNavigation { get; set; }
        public virtual Opcos Opco { get; set; }
    }
}
