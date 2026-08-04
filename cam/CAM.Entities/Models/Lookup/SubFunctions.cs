using CAM.Entities.Models.Base;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAM.Entities.Models.Lookup
{
    [Table("Subfunction")]
    public class SubFunctions
    {
        
        public SubFunctions()
        {
            Subfunctionareas = new HashSet<Subfunctionarea>();
        }
        [Key]
        public long Subfunctionid { get; set; }
        public long? Functionareaid { get; set; }
        public string Subfunctionname { get; set; }
        public string Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public string Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }

        public virtual Functionarea Functionarea { get; set; }
        public virtual ICollection<Subfunctionarea> Subfunctionareas { get; set; }

    }
}
