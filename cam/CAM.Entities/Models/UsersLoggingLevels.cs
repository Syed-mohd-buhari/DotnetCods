using CAM.Entities.Models.Base;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CAM.Entities.Models
{
    public class UsersLoggingLevels : AuditableEntity
    {
        public string LogLevel { get; set; }
    }
}
