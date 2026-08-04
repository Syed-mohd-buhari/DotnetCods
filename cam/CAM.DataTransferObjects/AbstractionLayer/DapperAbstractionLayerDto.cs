using System;

namespace CAM.DataTransferObjects.AbstractionLayer
{
    public class DapperAbstractionLayerDto
    {
    }
   
public class LcmPlannedActivityDapperDto
    {
        /*=========================================================
            LCM ENGINEERING
        =========================================================*/
        public long LcmEngineeringId { get; set; }

        public int LcmCount { get; set; }

        public bool LcmArchived { get; set; }

        public bool LcmDeleted { get; set; }

        /*=========================================================
            OPCO
        =========================================================*/
        public short OpcoId { get; set; }

        public string Opco { get; set; }

        /*=========================================================
            LCM DESIGN COMPONENT
        =========================================================*/
        public long? LcmDcId { get; set; }

        public long? LcmSystemTypeId { get; set; }

        /*=========================================================
            LCM SOFTWARE DETAILS
        =========================================================*/
        public string LcmOem { get; set; }

        public string LcmProductName { get; set; }

        /*=========================================================
            PLANNED ACTIVITY
        =========================================================*/
        public long? PlannedActivityId { get; set; }

        public bool? PaDeleted { get; set; }

        public long? PaLcmId { get; set; }

        public DateTime? PlannedCompletion { get; set; }

        public DateTime? StartDate { get; set; }

        public short? PlannedActivityResourceId { get; set; }

        public int? PaDeliveryStatusId { get; set; }

        public bool? PaArchived { get; set; }

        public short? PaOpcoId { get; set; }

        public long? PaDcfId { get; set; }

        public long? PaDcId { get; set; }

        public string PaDeliveryStatus { get; set; }

        public string PlannedActivityResource { get; set; }

        public long? PaSystemTypeId { get; set; }

        /*=========================================================
            PA SOFTWARE DETAILS
        =========================================================*/
        public long? PaProductId { get; set; }

        public long? PaOemId { get; set; }

        public DateTime? PaEom { get; set; }

        public DateTime? PaEos { get; set; }

        public string PaVersion { get; set; }

        public string PaOem { get; set; }

        public string PaProductName { get; set; }

        /*=========================================================
            DC NAMES
        =========================================================*/
        public string LcmDcName { get; set; }

        public string PaDcName { get; set; }
    }

    public class LcmBasedEomEosDapperDto
    {
        public long LcmEngineeringId { get; set; }
        public string Opco { get; set; }
        public string Product { get; set; }
        public string OriginalEquipmentManufacturer { get; set; }
        public string SoftwareVersion { get; set; }
        public DateTime? EndOfMaintenance { get; set; }
        public DateTime? EndOfSupport { get; set; }
        public long MajorSoftwareBuildId { get; set; }
        public int ProductImportanceId { get; set; }

        public bool LcmArchived {get; set; }
    }

}
