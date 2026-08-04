using System;

namespace CAM.Entities.Models.Lookup
{
    public class SWConfigFunctionAreas
    {
        public decimal SWConfigFunctionAreaId { get; set; }
        public decimal? FunctionId { get; set; }
        public string FunctionAreaName { get; set; }
        public string FunctionAreaDescription { get; set; }
        public int CreationUser { get; set; }
        public DateTime CreationDate { get; set; }
        public int ModificationUser { get; set; }
        public DateTime ModificationDate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? DeletionDate { get; set; }
        public virtual Functions Function { get; set; }
    }
}
