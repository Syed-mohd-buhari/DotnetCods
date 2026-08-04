using CAM.Entities.Models.Base;
using System.Collections.Generic;

namespace CAM.Entities.Models.Lookup
{
    public class SWConfigSubFunction : AuditableEntity
    {
        public SWConfigSubFunction()
        {
            SWConfigSubFunctionAreas = new HashSet<SWConfigSubFunctionAreas>();
        }

        public int SWConfigSubUunctionId { get; set; }
        public decimal? SWConfigFunctionAreaId { get; set; }
        public string SubFunctionName { get; set; }
        public virtual SWConfigFunctionAreas SWConfigFunctionArea { get; set; }
        public virtual ICollection<SWConfigSubFunctionAreas> SWConfigSubFunctionAreas { get; set; }

    }
}
