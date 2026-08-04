using System;
using System.Collections.Generic;
using System.ComponentModel;
using CAM.DataAttributes.Grid;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto.Base;

namespace CAM.DataTransferObjects.LookUp.PlannedActivityResourceDto
{
    public class PlannedActivityResourceDto
    {
        public short PlannedActivityResourceId { get; set; }
        
        public string LcmLabelSoftware { get; set; }
        public string LcmLabelHardware { get; set; }

        public string AddAssetLabelSoftware { get; set; }
        public string AddAssetLabelHardware { get; set; }

        public string EditAssetLabelSoftware { get; set; }
        public string EditAssetLabelHardware { get; set; }

        public bool LcmHardware { get; set; }
        public bool LcmSoftware { get; set; }

        public bool? AddAssetHardware { get; set; }
        public bool? AddAssetSoftware { get; set; }

        public bool? EditAssetHardware { get; set; }
        public bool? EditAssetSoftware { get; set; }

        public bool Exportable { get; set; }
        public bool DesignAspectExportable { get; set; }
        public bool ForLcm { get; set; }

        public bool? PlannedDesignComponentRequiredAddAsset { get; set; }

        public bool? PlannedDesignComponentRequiredEditAsset { get; set; }
        public int RuleActicvityDetails { get; set; }
        public int RuleLinkedDc { get; set; }
        public string PlannedActivityResourceDescription { get; set; }

        public int? RuleAddAsset { get; set; }


        public int? RuleEditAsset { get; set; }

        //
        public List<short> DriverTextLcm { get; set; }
        public List<short> BenefitTextLcm { get; set; }
        public List<short> PlanningRisksLcm { get; set; }
        //
        public List<short> DriverTextAddAsset { get; set; }
        public List<short> BenefitTextAddAsset { get; set; }
        public List<short> PlanningRisksAddAsset { get; set; }

        public List<short> DriverTextEditAsset { get; set; }
        public List<short> BenefitTextEditAsset { get; set; }
        public List<short> PlanningRisksAEditAsset { get; set; }
        //
        public List<short> DriverTextDesignAspect { get; set; }
        public List<short> BenefitTextDesignAspect { get; set; }
        public List<short> PlanningRisksDesignAspect { get; set; }

        // Service Plan
        public List<short> DriverTextServicePlan { get; set; }
        public List<short> BenefitTextServicePlan { get; set; }
        public List<short> PlanningRiskServicePlan { get; set; }

        public bool? OnBareMetalAddAsset { get; set; }
        public bool? OnVirtualizedAddAsset { get; set; }


        public bool? ForCreateAddAsset { get; set; }
        public bool? ForEditAddAsset { get; set; }
        public int? RuleActicvityDetailsAddAsset { get; set; }

        public string ActivityDetailsAddAsset { get; set; }
        public string ActivityDetailsForVirtualizedAddAsset { get; set; }

        public bool? OnBareMetalEditAsset  { get; set; }
        public bool?  OnVirtualizedEditAsset { get; set; }


        public bool? ForCreateEditAsset { get; set; }
        public bool? ForEditEditAsset { get; set; }
        public int? RuleActicvityDetailsEditAsset { get; set; }

        public string ActivityDetailsEditAsset { get; set; }
        public string ActivityDetailsForVirtualizedEditAsset { get; set; }

        public string ActivityDetailsLcm { get; set; }

        public string DesignAspectLabelSoftware { get; set; }
        public string DesignAspectLabelHardware { get; set; }
        public bool? ForDesignAspect { get; set; }
        public string ActivityDetailsDesignAspect { get; set; }

        public bool? DesignAspectSoftware { get; set; }
        public bool? DesignAspectHardware { get; set; }
        public bool? ForAddAsset { get; set; }
        public bool? ForEditAsset { get; set; }
        public bool? Forserviceplan {  get; set; }

        public int? RuleDesignAspect { get; set; }

        [IgnoreGrid]
        public IDictionary<short, TipologicaGridDtoForVirtualized> ActivityDetailsResource { get; set; }
        [IgnoreGrid]
        public IDictionary<short, string> BenefitResource { get; set; }
        [IgnoreGrid]
        public IDictionary<short, string> DriverResource { get; set; }
        [IgnoreGrid]
        public IDictionary<short, string> PlanningRiskResource { get; set; }

        [IgnoreGrid]
        public string JsonForm { get; set; }
        [DateRangeGrid]
        public DateTime? LastModified { get; set; }
        public string LastModifiedBy { get; set; }

    }
    public class PlannedActivityResourceDtoGrid : GridDtoBase
    {
        [IgnoreGrid]
        public short PlannedActivityResourceId { get; set; }

        [OrderGrid(Order = 1)]
        [Default]
        public string PlannedActivityResourceDescription { get; set; }

        [OrderGrid(Order = 2)]
        [Default]
        public bool ForLcm { get; set; }

        [OrderGrid(Order = 3)]
        [Default]
        public bool? ForAddAsset { get; set; }

        [OrderGrid(Order = 4)]
        [Default]
        public bool ForDesignAspect { get; set; }

        [OrderGrid(Order = 5)]
        [Default]
        public bool Exportable { get; set; }

        //[OrderGrid(Order = 6)]
        [IgnoreGrid]
        public int RuleLinkedDc { get; set; }


        [OrderGrid(Order = 6)]
        [Default]
        public string RuleLinkedDcPlannedActivityTypeDescription { get; set; }

        [OrderGrid(Order = 7)]
        [Default]
        public int RuleActicvityDetails { get; set; }

        [OrderGrid(Order = 8)]
        [Default]
        public string ActivityDetailsLcm { get; set; }

  
        [OrderGrid(Order = 9)]
        [Default]
        public bool LcmHardware { get; set; }

        [OrderGrid(Order = 10)]
        [Default]
        public string LcmLabelHardware { get; set; }

        [OrderGrid(Order = 11)]
        [Default]
        public bool LcmSoftware { get; set; }

        [OrderGrid(Order = 12)]
        [Default]
        public string LcmLabelSoftware { get; set; }

        [OrderGrid(Order = 13)]
        [Default]
        public string DriverTextLcm { get; set; }

        [OrderGrid(Order = 14)]
        [Default]
        public string BenefitTextLcm { get; set; }

        [OrderGrid(Order = 15)]
        [Default]
        public string PlanningRisksLcm { get; set; }

        [OrderGrid(Order = 16)]
        [Default]
        public int? RuleActicvityDetailsAddAsset { get; set; }

        [OrderGrid(Order = 17)]
        [Default]
        public string ActivityDetailsAddAsset { get; set; }

        [OrderGrid(Order = 18)]
        [Default]
        public string ActivityDetailsForVirtualizedAddAsset { get; set; }

        [OrderGrid(Order = 19)]
        [Default]
        public int? RuleAddAsset { get; set; }

        [OrderGrid(Order = 20)]
        public bool? PlannedDesignComponentRequiredAddAsset { get; set; }

        [OrderGrid(Order = 21)]
        [Default]
        public bool? AddAssetHardware { get; set; }

        [OrderGrid(Order = 22)]
        [Default]
        public string AddAssetLabelHardware { get; set; }

        [OrderGrid(Order = 23)]
        public bool? AddAssetSoftware { get; set; }

        [OrderGrid(Order = 24)]
        [Default]
        public string AddAssetLabelSoftware { get; set; }

        [OrderGrid(Order = 25)]
        [Default]
        public string DriverTextAddAsset { get; set; }

        [OrderGrid(Order = 26)]
        [Default]
        public string BenefitTextAddAsset { get; set; }

        [OrderGrid(Order = 27)]
        [Default]
        public string PlanningRisksAddAsset { get; set; }

        [OrderGrid(Order = 28)]
        [IgnoreGrid]
        public bool ForCreateAddAsset { get; set; }

        [OrderGrid(Order = 29)]
        [IgnoreGrid]
        public bool? ForEditAddAsset { get; set; }

        [OrderGrid(Order = 30)]
        [Default]
        public bool? OnBareMetalAddAsset { get; set; }

        [OrderGrid(Order = 31)]
        [Default]
        public bool? OnVirtualizedAddAsset { get; set; }

        [OrderGrid(Order = 32)]
        [Default]
        public bool? ForEditAsset { get; set; }

        [OrderGrid(Order = 33)]
        [Default]
        public int? RuleActicvityDetailsEditAsset { get; set; }

        [OrderGrid(Order = 34)]
        [Default]
        public string ActivityDetailsEditAsset { get; set; }

        [OrderGrid(Order = 35)]
        [Default]
        public string ActivityDetailsForVirtualizedEditAsset { get; set; }

        [OrderGrid(Order = 36)]
        [Default]
        public int? RuleEditAsset { get; set; }

        [OrderGrid(Order = 37)]
        public bool? PlannedDesignComponentRequiredEditAsset { get; set; }

        [OrderGrid(Order = 38)]
        [Default]
        public bool? EditAssetHardware { get; set; }

        [OrderGrid(Order = 39)]
        [Default]
        public string EditAssetLabelHardware { get; set; }

        [OrderGrid(Order = 40)]
        public bool? EditAssetSoftware { get; set; }

        [OrderGrid(Order = 41)]
        [Default]
        public string EditAssetLabelSoftware { get; set; }

        [OrderGrid(Order = 42)]
        [Default]
        public string DriverTextEditAsset { get; set; }

        [OrderGrid(Order = 43)]
        [Default]
        public string BenefitTextEditAsset { get; set; }

        [OrderGrid(Order = 44)]
        [Default]
        public string PlanningRisksEditAsset { get; set; }

        [OrderGrid(Order = 45)]
        [IgnoreGrid]
        public bool ForCreateEditAsset { get; set; }

        [OrderGrid(Order = 46)]
        [IgnoreGrid]
        public bool? ForEditEditAsset { get; set; }

        [OrderGrid(Order = 47)]
        [Default]
        public bool? OnBareMetalEditAsset { get; set; }

        [OrderGrid(Order = 48)]
        [Default]
        public bool? OnVirtualizedEditAsset { get; set; }

        [OrderGrid(Order = 49)]
        [Default]
        public bool DesignAspectHardware { get; set; }

        [OrderGrid(Order = 50)]
        [Default]
        public bool DesignAspectSoftware { get; set; }

        [OrderGrid(Order = 51)]
        [Default]
        public string ActivityDetailsDesignAspect { get; set; }
       
        [OrderGrid(Order = 52)]
        [Default]
        public int? RuleDesignAspect { get; set; }

        [OrderGrid(Order = 53)]
        [Default]
        public string DesignAspectLabelSoftware { get; set; }
        [OrderGrid(Order = 54)]
        [Default]
        public string DesignAspectLabelHardware { get; set; }

        [OrderGrid(Order = 55)]
        [Default]
        public string DriverTextDesignAspect { get; set; }

        [OrderGrid(Order = 56)]
        [Default]
        public string BenefitTextDesignAspect { get; set; }

        [OrderGrid(Order = 57)]
        [Default]
        public string PlanningRisksDesignAspect { get; set; }
        [OrderGrid(Order = 58)]
        [Default]
        public bool DesignAspectExportable { get; set; }
        [OrderGrid(Order = 59)]
        [Default]
        public bool? ForServicePlan {  get; set; }

        [OrderGrid(Order = 60)]
        [DateRangeGrid]
        [Default]
        public DateTime? LastModified { get; set; }

        [OrderGrid(Order = 61)]
        [Default]
        [MailTo]
        [DisplayName("Last Modified By")]
        public string LastModifiedBy { get; set; }

        [IgnoreGrid]
        public string JsonFormResource { get; set; }
    }
    public class PlannedActivityResourceQuery : QueryObject
    {
        public List<short> PlannedActivityResourceId { get; set; }
        public List<string> PlannedActivityResourceDescription { get; set; }
        public List<string> JsonFormResource { get; set; }

        public List<bool> Exportable { get; set; }

        public List<bool> DesignAspectExportable { get; set; }

        public List<bool> PlannedDesignComponentRequiredAddAsset { get; set; }
        public List<bool> PlannedDesignComponentRequiredEditAsset { get; set; }

        public List<bool> ForLcm { get; set; }

        public List<bool> ForDesignAspect { get; set; }

        public List<bool> DesignAspectHardware { get; set; }


        public List<bool> DesignAspectSoftware { get; set; }


        public List<string> ActivityDetailsDesignAspect { get; set; }


        public List<int?> RuleDesignAspect { get; set; }
       
        public List<string> DesignAspectLabelSoftware { get; set; }
     
        public List<string> DesignAspectLabelHardware { get; set; }

        public List<bool> ForAddAsset { get; set; }

        public List<bool> ForEditAsset { get; set; }

        public List<bool> LcmHardware { get; set; }
        public List<bool> LcmSoftware { get; set; }

        public List<bool> AddAssetHardware { get; set; }
        public List<bool> AddAssetSoftware { get; set; }

        public List<bool> EditAssetHardware { get; set; }
        public List<bool> EditAssetSoftware { get; set; }

        public List<int> RuleActicvityDetails { get; set; }
        public List<int> RuleLinkedDc { get; set; }

        public List<string> LcmLabelSoftware { get; set; }
        public List<string> LcmLabelHardware { get; set; }

        public List<string> AddAssetLabelSoftware { get; set; }
        public List<string> AddAssetLabelHardware { get; set; }

        public List<string> EditAssetLabelSoftware { get; set; }
        public List<string> EditAssetLabelHardware { get; set; }

        public List<string> ActivityDetailsAddAsset { get; set; }

        public List<string> ActivityDetailsEditAsset { get; set; }

        public List<string> ActivityDetailsLcm { get; set; }
        
        public List<string> ActivityDetailsForVirtualizedAddAsset { get; set; }
        public List<int> RuleAddAsset { get; set; }
        public List<int> RuleActicvityDetailsAddAsset { get; set; }

        public List<short> DriverTextAddAsset { get; set; }
        public List<short> BenefitTextAddAsset { get; set; }
        public List<short> PlanningRisksAddAsset { get; set; }

        public List<string> ActivityDetailsForVirtualizedEditAsset { get; set; }
        public List<int> RuleEditAsset { get; set; }
        public List<int> RuleActicvityDetailsEditAsset { get; set; }

        public List<short> DriverTextEditAsset { get; set; }
        public List<short> BenefitTextEditAsset { get; set; }
        public List<short> PlanningRisksEditAsset { get; set; }

        public List<short> DriverTextLcm { get; set; }
        public List<short> BenefitTextLcm { get; set; }
        public List<short> PlanningRisksLcm { get; set; }

  
        public List<short> DriverTextDesignAspect { get; set; }
        public List<short> BenefitTextDesignAspect { get; set; }
        public List<short> PlanningRisksDesignAspect { get; set; }

        public List<bool> OnBareMetalAddAsset { get; set; }
        public List<bool> OnVirtualizedAddAsset { get; set; }

        public List<bool> ForCreateAddAsset { get; set; }
        public List<bool> ForEditAddAsset { get; set; }

        public List<bool> OnBareMetalEditAsset { get; set; }
        public List<bool> OnVirtualizedEditAsset { get; set; }

        public List<bool> ForCreateEditAsset { get; set; }
        public List<bool> ForEditEditAsset { get; set; }

        public List<string> RuleLinkedDcPlannedActivityTypeDescription { get; set; }
        public List<string> ForServiePlan { get; set; }

        
    }
}
