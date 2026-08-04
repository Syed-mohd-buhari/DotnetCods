using CAM.Entities.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CAM.Entities.Models.Cross;
using CAM.Entities.Models.Lookup;
using OracleModels.DBModels;

namespace CAM.Entities.Models
{
    [Table("SystemTypes")]
    public partial class SystemType : AuditableEntity
    {
        public SystemType()
        {
            DesignComponents = new List<DesignComponent>();
           
            SystemTypesMajorHardwareBuilds = new HashSet<SystemTypesMajorHardwareBuild>();
            SystemTypesSubDomainSpocs = new HashSet<SystemTypesSubDomainSpoc>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long SystemTypeId { get; set; }

        [Column("SystemTypeName3GPP")]
        [StringLength(255)]
        public string SystemTypeName3Gpp { get; set; }
        [Column("SystemTypeNameOEM")]
        [StringLength(255)]
        public string SystemTypeNameOem { get; set; }
        public long? MajorSoftwareBuildsId { get; set; }
       
        public DateTime? ConstraintScaling { get; set; }
        [Column("ConstraintLCM")]
        public string ConstraintLcm { get; set; }
        [Column(TypeName = "date")]
        public string EndOfMaintenance { get; set; }
        public short? ProductImportanceId { get; set; }
        public int? VerticalResponsibleId { get; set; }
        public int? SubDomainResponsibleId { get; set; }
        [Column("SubDomainSPOC")]
        [StringLength(2000)]
        public string SubDomainSpoc { get; set; }
        public int? AssetCategoryId { get; set; }
        public int? AssetClassId { get; set; }
        public int? AssetTypeId { get; set; }
        [Column("SpareFieldsJSON")]
        public string SpareFieldsJson { get; set; }

        [ForeignKey(nameof(AssetCategoryId))]
        [InverseProperty("SystemTypes")]
        public virtual AssetCategory AssetCategory { get; set; }

        public int? VodafoneNameId { get; set; }

        [ForeignKey(nameof(VodafoneNameId))]
        //[InverseProperty("MajorSoftwareBuilds")]
        [InverseProperty(nameof(Lookup.VodafoneNames.SystemTypes))]
        public virtual VodafoneNames VodafoneName { get; set; }
        public DateTime? EndOfMaintenanceValue { get; set; }

        [ForeignKey(nameof(AssetClassId))]
        [InverseProperty(nameof(AssetClass.SystemTypes))]
        public virtual AssetClass AssetClassIdNavigation { get; set; }
        [ForeignKey(nameof(AssetTypeId))]
        [InverseProperty(nameof(AssetType.SystemTypes))]
        public virtual AssetType AssetTypeIdNavigation { get; set; }

        [ForeignKey(nameof(MajorSoftwareBuildsId))]
        [InverseProperty(nameof(MajorSoftwareBuild.SystemTypes))]
        public virtual MajorSoftwareBuild MajorSoftwareBuilds { get; set; }

        [ForeignKey(nameof(ProductImportanceId))]
        [InverseProperty(nameof(ProductImportance.SystemTypes))]
        public virtual ProductImportance ProductImportanceRel { get; set; }


        [ForeignKey(nameof(SubDomainResponsibleId))]
        [InverseProperty("SystemTypes")]
        public virtual SubDomainResponsible SubDomainResponsible { get; set; }
        
        [ForeignKey(nameof(VerticalResponsibleId))]
        [InverseProperty("SystemTypes")]
        public virtual VerticalResponsible VerticalResponsible { get; set; }
        [InverseProperty(nameof(DesignComponent.SystemType))]
        public List<DesignComponent> DesignComponents { get; set; }
        
        [InverseProperty(nameof(SystemTypesMajorHardwareBuild.SystemType))]
        public virtual ICollection<SystemTypesMajorHardwareBuild> SystemTypesMajorHardwareBuilds { get; set; }
        public virtual ICollection<SystemTypesSubDomainSpoc> SystemTypesSubDomainSpocs { get; set; }
        [InverseProperty(nameof(NetworkElementAsIs.SystemType))]
        public virtual ICollection<NetworkElementAsIs> Networkelementsasis { get; set; }

        #region

        public int? SoftWareDesignContactId { get; set; }
        public string SoftWareDesignContactEmail { get; set; }

        public int? HardWareDesignContactId { get; set; }
        public string HardWareDesignContactEmail { get; set; }

        public string HardWareVertical { get; set; }
        public string HardWareSubdomain { get; set; }
        public string SoftWareVertical { get; set; }
        public string SoftWareSubdomain { get; set; }

        #endregion
    }
}