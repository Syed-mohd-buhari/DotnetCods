using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Reportscheduler
    {
        public decimal Reportschedulerid { get; set; }
        public string Reportname { get; set; }
        public bool? Isscheduled { get; set; }
        public string Opcoid { get; set; }
        public string Reportvertical { get; set; }
        public string Exportfilepath { get; set; }
        public string Exportfileformat { get; set; }
        public int? Scheduleddate { get; set; }
        public string Scheduleddayinweek { get; set; }
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
