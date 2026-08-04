using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Tsrlogs
    {
        public decimal Tsrlogid { get; set; }
        public string Typeofoperation { get; set; }
        public string Filename { get; set; }
        public long? Totalrecord { get; set; }
        public long? Processedrecord { get; set; }
        public DateTime? Starttime { get; set; }
        public DateTime? Endtime { get; set; }
        public string Status { get; set; }
        public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public int Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }
        public string Domain { get; set; }
        public string Batchidentifier { get; set; }

        public virtual Aspnetusers CreationuserNavigation { get; set; }
        public virtual Aspnetusers ModificationuserNavigation { get; set; }
    }
}
