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
    [Table("Appsettingsconfiguration")]
    public class AppSettingsConfiguration : AuditableEntity
    {
        [Key]
        public long AppSettingsConfigurationId { get; set; }
        public long? AppSettingsId { get; set; }
        public string SettingsValue { get; set; }

        public virtual AppSetting AppConfiguration { get; set; }

    }
}
