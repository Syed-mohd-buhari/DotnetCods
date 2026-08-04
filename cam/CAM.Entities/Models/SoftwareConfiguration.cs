using CAM.Entities.Models.Base;
using CAM.Entities.Models.Lookup;
using System;
using System.Collections.Generic;

namespace CAM.Entities.Models
{
    public class SoftwareConfiguration
    {
        public SoftwareConfiguration()
        {
            Function = new HashSet<Functions>();           
        }
      
        public decimal Softwareconfigurationid { get; set; }
        public decimal? Networkelementid { get; set; }
        public string Elementname { get; set; }

        public string Opco { get; set; }

        public string Oem { get; set; }
        public int creationUser { get; set; }
        public DateTime creationDate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }
        public int modificationUser { get; set; }
        public DateTime modificationDate { get; set; }
        public NetworkElement NetworkElement { get; set; }
        public virtual ICollection<Functions> Function { get; set; } 


    }
}
