using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Voltekpi
    {
        public long Voltekpiid { get; set; }
        public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public int Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }
        public short Opcoid { get; set; }
        public short Month { get; set; }
        public short Year { get; set; }
        public decimal? Kpioneeoytarget { get; set; }
        public decimal? Kpionemonthlytarget { get; set; }
        public decimal? Kpioneactualvalue { get; set; }
        public decimal? Kpi1targetcluchngproposal { get; set; }
        public decimal? Kpi2actualnoofregsubsc { get; set; }
        public decimal? Kpi2actualnoofprovisionedsubsc { get; set; }
        public decimal? Kpithreeeoytarget { get; set; }
        public decimal? Kpithreemonthlytarget { get; set; }
        public decimal? Kpithreeactualvalue { get; set; }
        public decimal? Kpi3targetvluchngproposal { get; set; }
        public decimal? Kpifourfinaltarget { get; set; }
        public decimal? Kpifourtargetmonthly { get; set; }
        public decimal? Kpifouractualmonthly { get; set; }
        public decimal? Kpi4targetvluchngproposal { get; set; }
        public short? Kpifourtargetdatemonth { get; set; }
        public short? Kpifourtargetdateyear { get; set; }
        public string Kpithreecomment { get; set; }
        public string Kpifourcomment { get; set; }
        public string Kpitwocomment { get; set; }
        public string Kpionecomment { get; set; }

        public virtual Aspnetusers CreationuserNavigation { get; set; }
        public virtual Aspnetusers ModificationuserNavigation { get; set; }
        public virtual Opcos Opco { get; set; }
    }
}
