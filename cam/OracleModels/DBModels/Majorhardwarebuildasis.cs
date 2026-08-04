using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Majorhardwarebuildasis
    {
        public Majorhardwarebuildasis()
        {
            Assethardwareancillary = new HashSet<Assethardwareancillary>();
        }

        public long Majorhardwarebuildasisid { get; set; }
        public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public int Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }
        public short Orgeqpmanufacturerid { get; set; }
        public string Hardwaresolution { get; set; }
        public short? Hardwaresolutionreourceid { get; set; }
        public short Platformid { get; set; }
        public short Buildconstructionid { get; set; }
        public string Hardwaretype { get; set; }

        public virtual Buildconstructions Buildconstruction { get; set; }
        public virtual Aspnetusers CreationuserNavigation { get; set; }
        public virtual Hardwaresolutionresource Hardwaresolutionreource { get; set; }
        public virtual Aspnetusers ModificationuserNavigation { get; set; }
        public virtual Originalequipmentmanufacturers Orgeqpmanufacturer { get; set; }
        public virtual Platforms Platform { get; set; }
        public virtual ICollection<Assethardwareancillary> Assethardwareancillary { get; set; }
    }
}
