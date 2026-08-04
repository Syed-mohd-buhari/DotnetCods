using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using CAM.Entities.Models.Base;
using CAM.Entities.Models.Lookup;

namespace CAM.Entities.Models
{
    [Table("NetworkElementsAsIs")]
    public class NetworkElementAsIs : AuditableEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("NetworkElementAsIsId")]


        public long NetworkElementAsIsId { get; set; }

        public short OpCoId { get; set; }
        public long SystemTypeId { get; set; }

        public short OriginalEquipmentManufacturerId { get; set; }
        public string ElementDeploymentName { get; set; }
        public long NetworkElementAsPlannedId { get; set; }

        public short LocationId { get; set; }
        public string SoftwareProductNumber { get; set; }
        public string ElementManager { get; set; }
        public string PatchDetails { get; set; }

        public DateTime? SoftwareProductionDate { get; set; }
        public DateTime? SoftwareInstallDate { get; set; }
        public DateTime? DataAcquisitionDate { get; set; }

        public string DataAcquisitionMethod { get; set; }
        public string HardwareAcquisition { get; set; }
        public bool ManualOverride { get; set; }
        public string NodeType { get; set; }
        public string ElementManagerExportFileFormat { get; set; }
        public DateTime? HardwareInstallDate { get; set; }
        public string PlatformType { get; set; }
        public string HardwareType { get; set; }
        public string SoftwareReleaseInformation { get; set; }


        [ForeignKey(nameof(OpCoId))]
        [InverseProperty("Networkelementsasis")]
        public virtual OpCo OpCo { get; set; }

        [ForeignKey(nameof(SystemTypeId))]
        [InverseProperty("Networkelementsasis")]
        public virtual SystemType SystemType { get; set; }

        [ForeignKey(nameof(OriginalEquipmentManufacturerId))]
        [InverseProperty("Networkelementsasis")]
        public virtual OriginalEquipmentManufacturer OriginalEquipmentManufacturer { get; set; }

        [ForeignKey(nameof(LocationId))]
        [InverseProperty("Networkelementsasis")]
        public virtual Location Location { get; set; }

        [ForeignKey(nameof(NetworkElementAsPlannedId))]
        [InverseProperty("Networkelementsasis")]
        public virtual NetworkElementAsPlanned NetworkElementAsPlanned { get; set; }

        public List<int?> NetworkElementAsPlannedSubdomainSpoc { get; set; }

        public Dictionary<short, string> VerticalFilterDto { get; set; }

    }
}
