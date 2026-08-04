using CAM.Entities.Models.Base;
using System.Collections.Generic;

namespace CAM.Entities.Models.Lookup
{
    public partial class PlannedActivityCategory : AuditableEntity
    {
        public PlannedActivityCategory()
        {
            Budgetprojecttrackers = new HashSet<BudgetProjectTrackers>();
            Plannedactivities = new HashSet<PlannedActivity>();
        }

        public short Plannedactivitycategoryid { get; set; }
        public string Categorydescription { get; set; }       
       
        public virtual ICollection<BudgetProjectTrackers> Budgetprojecttrackers { get; set; }
        public virtual ICollection<PlannedActivity> Plannedactivities { get; set; }
    }
}

