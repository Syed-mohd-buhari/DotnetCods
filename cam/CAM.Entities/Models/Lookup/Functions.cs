using System;
using System.Collections.Generic;

namespace CAM.Entities.Models.Lookup
{
    public class Functions
    {
        public Functions()
        {
            Functionareas = new HashSet<FunctionAreas>();
            SWConfigFunctionAreas =new HashSet<SWConfigFunctionAreas>();
        }
        public decimal Functionid { get; set; }
        public decimal? Softwareconfigurationid { get; set; }
        public string Functionname { get; set; }
        public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public int Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }        
        public virtual SoftwareConfiguration Softwareconfiguration { get; set; }
        public virtual ICollection<FunctionAreas> Functionareas { get; set; }
        public virtual ICollection<SWConfigFunctionAreas> SWConfigFunctionAreas { get; set; }

    }
}
