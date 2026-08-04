using CAM.Entities.Models.Base;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAM.Entities.Models
{
    [Table("Appsettings")]
    public class AppSetting : AuditableEntity
    {
        [Key]
        public long AppSettingId { get; set; }
        public string Description { get; set; }

    }
}
