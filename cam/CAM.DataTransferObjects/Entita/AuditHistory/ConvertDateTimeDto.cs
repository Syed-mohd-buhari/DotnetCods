using System;

namespace CAM.DataTransferObjects.Entita.AuditHistory
{
    public class ConvertDateTimeDto
    {
        public DateTime? OldVlaueforDateRange { get; set; } = null;
        public string OldValue { get; set; }
        public bool Warning { get; set; } = false;
    }
}
