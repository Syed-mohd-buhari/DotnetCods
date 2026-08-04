using CAM.Entities.Models.Base;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CAM.Entities.Models.ComponentSoftware
{
    [Table("Componentmanufacturers")]
    public class ComponentManufacturers : AuditableEntity
    {
        public long Componentmanufacturerid { get; set; }
        public string Componentmanufacturer { get; set; }
        public string Componentname { get; set; }

        public virtual string LastModifiedBy { get; set; }
        public virtual DateTime? LastModified { get; set; }


    }
}
