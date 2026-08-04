using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Reasoncheckboxresources
    {
        public Reasoncheckboxresources()
        {
            Reasoncheckboxresourcelcmengineeringhardware = new HashSet<Reasoncheckboxresourcelcmengineeringhardware>();
            Reasoncheckboxresourcelcmengineeringsoftware = new HashSet<Reasoncheckboxresourcelcmengineeringsoftware>();
        }

        public short Id { get; set; }
        public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public int Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }
        public bool Ishardware { get; set; }
        public bool Issoftware { get; set; }
        public string Description { get; set; }

        public virtual Aspnetusers CreationuserNavigation { get; set; }
        public virtual Aspnetusers ModificationuserNavigation { get; set; }
        public virtual ICollection<Reasoncheckboxresourcelcmengineeringhardware> Reasoncheckboxresourcelcmengineeringhardware { get; set; }
        public virtual ICollection<Reasoncheckboxresourcelcmengineeringsoftware> Reasoncheckboxresourcelcmengineeringsoftware { get; set; }
    }
}
