using System;

namespace CAM.Entities.Models.Lookup
{
    public class SWConfigSubFunctionAreas
    {
        public int Swconfigsubfunctionareaid { get; set; }
        public int? Swconfigsubfunctionid { get; set; }
        public string Subfunctionareaname { get; set; }
        public string Subfunctionareadescription { get; set; }
        public int CreationUser { get; set; }
        public DateTime CreationDate { get; set; }
        public int ModificationUser { get; set; }
        public DateTime ModificationDate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? DeletionDate { get; set; }
        public virtual SWConfigSubFunction SWConfigSubFunction { get; set; }

    }
}
