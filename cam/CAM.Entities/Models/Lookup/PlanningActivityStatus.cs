using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CAM.Entities.Models.Base;
using CAM.Entities.Models.Settings;
using OracleModels.DBModels;

namespace CAM.Entities.Models.Lookup
{
    [Table("PlanningActivityStatuses")]
    public partial class PlanningActivityStatus : AuditableEntity
    {
        public PlanningActivityStatus()
        {
            PlannedActivities = new HashSet<PlannedActivity>();
            
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public short PlanningActivityStatusId { get; set; }

        [Required]
        [Column("PlanningActivityStatus")]
        [StringLength(50)]
        public string PlanningActivityStatusDescription { get; set; }
        
        public int ProjectStatusCombinationRule { get; set; }

        public bool Default { get; set; }

        [InverseProperty(nameof(PlannedActivity.PlanningActivityStatus))]
        public virtual ICollection<PlannedActivity> PlannedActivities { get; set; }
        
        public virtual ICollection<SettingsUpdatePlannedActivity> SettingsUpdatePlannedActivities { get; set; }


        //public Planningactivitystatuses()
        //{
        //    Plannedactivities = new HashSet<Plannedactivities>();
        //    Settingsupdateplannedactivity = new HashSet<Settingsupdateplannedactivity>();
        //}

        //public short Planningactivitystatusid { get; set; }
        //public int Creationuser { get; set; }
        //public DateTime Creationdate { get; set; }
        //public int Modificationuser { get; set; }
        //public DateTime Modificationdate { get; set; }
        //public bool? Deleted { get; set; }
        //public DateTime? Deletiondate { get; set; }
        //public string Planningactivitystatus { get; set; }
        //public int Projectstatuscombinationrule { get; set; }
        //public bool Default { get; set; }
        //public virtual Aspnetusers CreationuserNavigation { get; set; }
        //public virtual Aspnetusers ModificationuserNavigation { get; set; }
        //public virtual ICollection<Plannedactivities> Plannedactivities { get; set; }
        //public virtual ICollection<Settingsupdateplannedactivity> Settingsupdateplannedactivity { get; set; }


    }
}