#nullable enable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CAM.DataTransferObjects.LookUp.Asset;
using CAM.Entities.Models.Lookup;
using CAM.Infrastucture;

namespace CAM.DataTransferObjects.Entita.SystemType
{
    public class SystemTypeDtoCreate : SystemTypeDto
    {
        [Required]
        public long MajorSoftwareBuildsId { get; set; }
        public IDictionary<int, string>? VerticalResponsibleResource { get; set; }
        //[Required]
        //[Range(1, int.MaxValue, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public int? VerticalResponsibleId { get; set; }
        public IDictionary<int, string>? SubDomainResponsibleResource { get; set; }
        //[Required]
        //[Range(1, int.MaxValue, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public int? SubDomainResponsibleId { get; set; }
        public IDictionary<int, AssetCategoryDto>? AssetCategoryResource { get; set; }
        [Required]
        public int? AssetCategoryId { get; set; }

        public DateTime? ConstraintScalings { get; set; }
        public IDictionary<int, RelatedResource>? AssetClassResource { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public int? AssetClassId { get; set; }
        public IDictionary<int, RelatedResource>? AssetTypeResource { get; set; }
        [Required]
        public IDictionary<short, string>? ProductImportanceResource { get; set; }

        //[Required]
        //[Range(1, int.MaxValue, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public int? AssetTypeId { get; set; }
        [Required]
        public IEnumerable<MajorHardwareBuildMainSystemTypeDto> MajorHardwareBuildId { get; set; }
       // [Required]
       // public IEnumerable<int> SubDomainSpocIds { get; set; }
        public IDictionary<int, string>? SubDomainSpocResource { get; set; }
        public string? AssetClassDescription { get; set; }

        public IDictionary<int, string>? VodafoneNameResource { get; set; }
        public int? vodafoneNameId { get; set; }

    }




}