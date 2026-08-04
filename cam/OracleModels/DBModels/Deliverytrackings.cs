using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Deliverytrackings
    {
        public int Id { get; set; }
        public long? Plannedactivityid { get; set; }
        public string Ms1eventtype { get; set; }
        public DateTime? Ms1baselinedate { get; set; }
        public DateTime? Ms1latestplanningdate { get; set; }
        public int? Ms1status { get; set; }
        public string Ms2eventtype { get; set; }
        public DateTime? Ms2baselinedate { get; set; }
        public DateTime? Ms2latestplanningdate { get; set; }
        public int? Ms2status { get; set; }
        public string Ms3eventtype { get; set; }
        public DateTime? Ms3baselinedate { get; set; }
        public DateTime? Ms3latestplanningdate { get; set; }
        public int? Ms3status { get; set; }
        public int? Ms4eventtype { get; set; }
        public DateTime? Ms4baselinedate { get; set; }
        public DateTime? Ms4latestplanningdate { get; set; }
        public int? Ms4status { get; set; }
        public DateTime? Ppmimportdate { get; set; }
        public string Notes1 { get; set; }
        public string Notes2 { get; set; }
        public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public int Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }

        public virtual Aspnetusers CreationuserNavigation { get; set; }
        public virtual Aspnetusers ModificationuserNavigation { get; set; }
        public virtual Plannedactivities Plannedactivity { get; set; }
    }
}
