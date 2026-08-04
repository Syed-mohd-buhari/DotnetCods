using CAM.DataAttributes.Export;
using CAM.DataAttributes.Grid;
using System;

namespace CAM.DataTransferObjects.Entita.GenericReportDto
{
    public class GenericReportDtoGrid
    {
        #region //Lcm

        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [ColorGrid(Color = "red")]
        [IgnoreGrid]
        public long? LcmEngineeringId { get; set; }
        [IgnoreGrid]
        public long? DesignComponentId { get; set; }
        [IgnoreGrid]
        public short? opCoId { get; set; }
        public string opCo { get; set; }
        public DateTime? SoftwareEndOfWarrantyDate { get; set; }
        public int? NumberOfNodes { get; set; }
        [IgnoreGrid]
        public short? ProductImportanceId { get; set; }
        public bool? Warranty { get; set; }
        public int? NumberOfNodesInLab { get; set; }
        public string HardwareSheetIndex { get; set; }
        public string SoftwareSheetIndex { get; set; }
        public bool? OnHardware { get; set; }
        public bool? OnSoftware { get; set; }
        [IgnoreGrid]
        public short? FullOrPartialSupportId { get; set; }

        public string FullOrPartialSupportIdDesc { get; set; }
        public DateTime? HardwareEndOfSupportContract { get; set; }
        [IgnoreGrid]
        public short? HardwareSupportedId { get; set; }

        public string HardwareSupportedIdDesc { get; set; }

        public bool? RenewalinProgress { get; set; }
        public DateTime? SoftwareEndOfSupportContract { get; set; }
        [IgnoreGrid]
        public short? SoftwareSupportedId { get; set; }

        public string SoftwareSupportedIdDesc { get; set; }

        public bool? SparesProvisioned { get; set; }
        public DateTime? VendorEndMntDateHw { get; set; }
        public DateTime? VendorEndMnteDateSw { get; set; }
        public bool? ElementCount { get; set; }
        public string HardwareSupportProvider { get; set; }
        public string HardwareSupportType { get; set; }
        public string LcmStatusEngHardware { get; set; }
        public string LcmStatusEngSoftware { get; set; }
        public string LcmStatusHardware { get; set; }
        public string LcmStatusOpsHardware { get; set; }
        public string LcmStatusOpsSoftware { get; set; }
        public string LcmStatusSoftware { get; set; }
        public string OutputToLcmHardware { get; set; }
        public string OutputToLcmSoftware { get; set; }
        public string SoftwareSupportProvider { get; set; }
        public string SoftwareSupportType { get; set; }

        [IgnoreGrid]
        public short? FullOrPartialSupportHwId { get; set; }
        public string FullOrPartialSupportHwIdDesc { get; set; }

        [IgnoreGrid]
        public string LcmSpreadSheetHwId { get; set; }

        [IgnoreGrid]
        public string LcmSpreadSheetSwId { get; set; }


        public bool? Archived { get; set; }
        public string LcmDeploymentStatus { get; set; }
        public string ResourceKey { get; set; }
        public string PreviousResourceKey { get; set; }
        public bool? IsExtendedSupportOfferedByVendor { get; set; }
        public bool? HwIsExtendedSupportOfferedByVendor { get; set; }
        public string EngKpi2 { get; set; }
        public string IpAddress { get; set; }
        public string ManagedByGdc { get; set; }
        public string ExpLcmStatusAtEndOfFY24 { get; set; }
        public string EngineeringContactPoint { get; set; }
        public string OperationsContactPoint { get; set; }
        public DateTime? VendorEndOfVulnerabilitySecuritySupportDateValue { get; set; }
        public string AssetServiceFunctionality { get; set; }
        public string MaintenanceSupportSupplierSw { get; set; }
        public string MaintenanceSupportSupplierHw { get; set; }
        public string EduSpoc { get; set; }
        public string SubDomainSpoc { get; set; }
        public string VerticalResponsible { get; set; }
        #endregion
        #region //Assets
        //[IgnoreGrid]
        public string NetworkElementAsPlannedId { get; set; }
        public bool AutomatedFeedback { get; set; }
        public bool PlannedAction { get; set; }
        public string Environment { get; set; }
        [IgnoreGrid]
        public short DeploymentStatusId { get; set; }
        public string AssetDeploymentStatus { get; set; }
        [IgnoreGrid]
        public short? DeploymentTypeId { get; set; }
        public string DeploymentTypeIdDesc { get; set; }
        public string LocationName { get; set; }
        [IgnoreGrid]
        public short? NfviBundleidId { get; set; }
        [IgnoreGrid]
        public short? OrgEqpManufacturerId { get; set; }
        public string OrgEqpManufacturerIdDesc { get; set; }
        public string CapacityPlanReference { get; set; }
        public string ElementName { get; set; }
        public string AdditionalInformation1 { get; set; }
        public string NetworkConstruct { get; set; }
        public string AdditionalInformation2 { get; set; }
        public string HwResourceKey { get; set; }
        public string PreviousHwResourceKey { get; set; }
        public string SwResourceKey { get; set; }
        public string PreviousSwResourceKey { get; set; }
        public string MeStatus { get; set; }
        public string MeVirtualFlg { get; set; }
        public string MeDeploymentType { get; set; }
        public string MeType { get; set; }
        public string MeSerialNumber { get; set; }
        public string MeExternalConnectionFlg { get; set; }
        public string HardwareModules { get; set; }
        public string HardwareManfacturer { get; set; }
        public string LastCheckedTime { get; set; }

        #region New TSR Attributes
        public string VodafoneUniqueIdentifier { get; set; }
        public string AssetType { get; set; }
        public string RegulatoryScope { get; set; }
        public string GeoLocation { get; set; }

        public string DateAssetMovedToLiveStatus { get; set; }
        public string DateAssetDecommissioned { get; set; }
        public string ModuleType { get; set; }
        public string ComponentVersionNumber { get; set; }
        public string DescriptionOfPlannedAction { get; set; }
        public string IdentifiedAction { get; set; }
        public string AssetLastUpgradeDate { get; set; }
        public string HardwareVendorName { get; set; }

        public string SystemNameDns { get; set; }
        public string SystemNameBios { get; set; }
        public string LocalSiteResilience { get; set; }
        public string NameOfTheProduct { get; set; }
        public string EquipmentName { get; set; }
        #endregion

        #endregion
        #region //planned
        [IgnoreGrid]
        public long? PlannedActivityId { get; set; }
        public short? PlannedImplementationYear { get; set; }
        [IgnoreGrid]
        public short? ActivityStatusId { get; set; }

        public string ActivityStatusIdDesc { get; set; }

        [IgnoreGrid]
        public short? PlanningActivityStatusId { get; set; }

        public string PlanningActivityStatusIdDesc { get; set; }

        public string PlannedActivity { get; set; }
        public string ActivityDetails { get; set; }
        public string RagStatus { get; set; }
        public string DeliveryProjectName { get; set; }
        public string LocalApproval { get; set; }
        [IgnoreGrid]
        public short? DeliveryStatusId { get; set; }

        public string DeliveryStatusIdDesc { get; set; }
        [IgnoreGrid]
        public short? ResponsibilityPhaseId { get; set; }
        public string ResponsibilityPhaseIdDesc { get; set; }

        public DateTime? PlannedCompletion { get; set; }
        public string PlannedSpareFieldsJson { get; set; }
        [IgnoreGrid]
        public short? RelatestoId { get; set; }
        public decimal? BudgetValue { get; set; }
        public string PlannedActivityResource { get; set; }
        [IgnoreGrid]
        public short? BudgetAvailabilityId { get; set; }

        public string BudgetAvailabilityIdDesc { get; set; }
        [IgnoreGrid]
        public short? EngineeringRiskId { get; set; }
        public string EngineeringRiskIdDesc { get; set; }
        [IgnoreGrid]
        public short? OperationalRiskId { get; set; }
        public string OperationalRiskIdDesc { get; set; }
        [IgnoreGrid]
        public long? LinkedToPlannedActivityId { get; set; }

        public bool? IsNewServiceArchitecture { get; set; }
        public bool? IsReplacementExistingSolution { get; set; }
        [IgnoreGrid]
        public short? BenefitId { get; set; }
        public string BenefitIdDesc { get; set; }
        [IgnoreGrid]
        public short? DriverId { get; set; }
        public string DriverIdDesc { get; set; }
        [IgnoreGrid]
        public short? PlanningRiskId { get; set; }

        public string PlanningRiskIdDesc { get; set; }
        public string Currency { get; set; }
        public string Notes { get; set; }
        public string OverAllRiskEvaluation { get; set; }
        [IgnoreGrid]
        public string DeliveryProjectId { get; set; }

        public string PlanningRisk { get; set; }
        public string ProjectStatus { get; set; }
        public string RiskEngineeringNotes { get; set; }
        public string RiskOperationalNotes { get; set; }

        public string BudgetTrackingId { get; set; }
        [IgnoreGrid]
        public long? DesignComponentFamilyId { get; set; }
        public DateTime? StartDate { get; set; }
        public bool? ForAddAsset { get; set; }
        public bool? ForEditAsset { get; set; }
        [IgnoreGrid]
        public long? OriginalLcmEngineeringId { get; set; }
        public bool? DeliveryPlanAvailable { get; set; }
        public string Program { get; set; }
        public string ProjectOwner { get; set; }
        public string BudgetEstimated { get; set; }
        public string BundleBudget { get; set; }
        [IgnoreGrid]
        public string BundleId { get; set; }

        public string PlannedDc { get; set; }
        public long? PlannedDcIndex { get; set; }
        public string IsPlannedActivity { get; set; }
        public string Implemented { get; set; }
        public string IsAnicallaryDataExists { get; set; }
        public string DeliveryStatus { get; set; }
        public string PlannedActivityStatus { get; set; }
        #endregion
        #region //System
        [IgnoreGrid]
        public long? SystemTypeId { get; set; }
        public string SystemTypeNameVodafone { get; set; }
        public string SystemTypeName3gpp { get; set; }
        public string SystemTypeNameOem { get; set; }
        [IgnoreGrid]
        public long? MajorSoftwareBuildsId { get; set; }
        public DateTime? ConstraintsCaling { get; set; }
        public DateTime? EndOfMaintenanceValue { get; set; }
        [IgnoreGrid]
        public int? VerticalNameId { get; set; }
        public string AssetVirtualized { get; set; }
        public string BuildConstruction { get; set; }

        public string SystemTypeDesignContact { get; set; }

        [IgnoreGrid]
        public int? AssetCategoryId { get; set; }
        public string AssetCategoryIdDesc { get; set; }
        [IgnoreGrid]
        public int? AssetClassId { get; set; }
        public string AssetClassIdDesc { get; set; }
        [IgnoreGrid]
        public int? AssetTypeId { get; set; }
        public string AssetTypeIdDesc { get; set; }
        public string AssetClass { get; set; }
        public string ConstraintLcm { get; set; }
        public string VodafoneName { get; set; }

        public string SoftWareDesignContactEmail { get; set; }

        public string SoftWareVertical { get; set; }

        public string SoftWareSubdomain { get; set; }

        public string HardWareDesignContactEmail { get; set; }

        public string HardWareVertical { get; set; }

        public string HardWareSubdomain { get; set; }
        #endregion
        #region //AssetCate
        public string AssetCategory { get; set; }
        public bool? TakeFromAssetTypeTable { get; set; }
        #endregion
        #region //SubnetWork
        [IgnoreGrid]
        public long? Id { get; set; }
        public string SubDescription { get; set; }
        public bool? Default { get; set; }
        public string Alias { get; set; }
        public int? Order { get; set; }
        public string SwApplicationName { get; set; }
        public bool? GdprRelevant { get; set; }
        public bool? InternetFacing { get; set; }
        public string LcmPolicy { get; set; }
        public string Criticality { get; set; }
        public bool? SecurityElement { get; set; }
        public int? GdprClassification { get; set; }
        public bool? PciSox { get; set; }
        public bool? C3C4 { get; set; }
        public bool? MissionCritical { get; set; }
        [IgnoreGrid]
        public decimal? ProductNameId { get; set; }

        public string ProductNameIdDesc { get; set; }

        [IgnoreGrid]
        public int? VodafoneNameId { get; set; }
        public string VodafoneNameIdDesc { get; set; }
        #endregion
        #region //ProductImpor
        public string ProductImportance { get; set; }
        #endregion
        #region //Majorsoftware
        public string OrgEqpmanufacturer { get; set; }
        public string SoftwareVersion { get; set; }
        public DateTime? LastTimeBuyNew { get; set; }
        public DateTime? LastTimeBuyUpgrades { get; set; }
        public DateTime? LastTimeBuyExpansions { get; set; }
        public DateTime? MajEndOfMaintenance { get; set; }
        public DateTime? EndOfSupport { get; set; }
        public DateTime? GeneraAvailableDate { get; set; }
        public string DeliveryMethod { get; set; }
        public string MajSpareFieldsJson { get; set; }
        [IgnoreGrid]
        public short? OperatingSystemId { get; set; }

        public string OperatingSystemIdDesc { get; set; }

        public string VulnerabilityStatus { get; set; }
        public short? EomStatus { get; set; }
        public string CriticalAssetType { get; set; }
        public string ProductName { get; set; }
        public string MajorDescription { get; set; }
        public string Platform { get; set; }
        public string TciBundleVersion { get; set; }

        public string TcpBundleVersion { get; set; }
        public string SoftwareDesignContact { get; set; }
        #endregion
        #region //MajorHardware
        [IgnoreGrid]
        public long? MajorHardwareId { get; set; }
        public string HardwareSolution { get; set; }
        public string OtherHardwareInfo { get; set; }
        public DateTime? HardwareLastTimeBuyNew { get; set; }
        public DateTime? HardwareLastTimeBuyUpgrades { get; set; }
        public DateTime? HardwareLastTimeBuyExpansions { get; set; }
        public DateTime? HardwareEndofmaintenance { get; set; }
        public DateTime? HardwareEndofsupport { get; set; }
        public bool? ProprietaryHardware { get; set; }
        [IgnoreGrid]
        public short? PlatformId { get; set; }

        public string PlatformIdDesc { get; set; }
        [IgnoreGrid]
        public short? Buildconstructionid { get; set; }

        public string BuildconstructionidDesc { get; set; }

        public string HardwareType { get; set; }
        public string OperatingSystem { get; set; }
        public string TypeOfProcessor { get; set; }
        public string HardwareVulnerabilityStatus { get; set; }
        public short? HardwareEomStatus { get; set; }
        public DateTime? GeneralAvailabledate { get; set; }
        public string HardwareModel { get; set; }
        public string HardwareDescription { get; set; }
        public string HardwareDesignContact { get; set; }
        #endregion
        #region //Risk
        [IgnoreGrid]
        public short? RiskId { get; set; }
        public int? Severity { get; set; }
        public string RiskDescription { get; set; }
        #endregion
        #region //LcmAncillaryData
        [IgnoreGrid]
        public long? LcmAncillaryDataId { get; set; }
        public string ProductCode { get; set; }
        public bool? HandedOverToOperation { get; set; }
        public string ContractRenewalPlan { get; set; }
        public string ReasonForNoPlan { get; set; }
        public string CommentOnProjectStatus { get; set; }
        public string ScopeOfSimplification { get; set; }
        public string DataSource { get; set; }
        public string IncidentClass { get; set; }
        public string OccurenceProbability { get; set; }
        public string SecurityRiskPotential { get; set; }
        public string SecurityRiskEffective { get; set; }
        public string SecurityMitigation { get; set; }
        public string AssetOutOfScope { get; set; }
        public bool? IncludedInSecurityScanning { get; set; }

        public string RaId { get; set; }

        public string RequestId { get; set; }
        [DateRangeGrid]
        public DateTime? LastScanDate { get; set; }
        [DateRangeGrid]
        public DateTime? LastUpgradeDate { get; set; }
        public string EomControl { get; set; }
        public string EngUpdateTracker { get; set; }
        public string OpsUpdateTracker { get; set; }
        public string KpiStatusService { get; set; }
        public string Custom { get; set; }
        public string Custom1 { get; set; }
        public string Custom2 { get; set; }
        public string IdNew { get; set; }
        public string ExNetworks { get; set; }
        public string ProductImportanceHistory2 { get; set; }
        public string CloudVersion { get; set; }
        public string CertifiedSWRealeseForNfviBundle { get; set; }
        public string LcmStatus { get; set; }
        public string OriginalHwLcmId { get; set; }
        public string OriginalSwLcmId { get; set; }

        public string NewopsRiskEvaluation { get; set; }

        public string SecurityRiskOverall { get; set; }

        public string LabSwRelease { get; set; }
        public bool? IsPecn { get; set; }
        public bool? IsPecs { get; set; }
        public bool? IsScf { get; set; }
        public bool? IsNof { get; set; }

        public string ExposedEdgeFlag { get; set; }
        public bool? ExternalFacingFlag { get; set; }
        public string InfrastructureLocation { get; set; }
        public string VulnerabilityRating { get; set; }

        public string CyberRiskRequestId { get; set; }
        #endregion
        #region //DesignAspect
        [IgnoreGrid]
        public long? DesignAspectId { get; set; }
        public string SubNetworkBoundary { get; set; }
        public string DesignAspectDescription { get; set; }
        public string SupportedServices { get; set; }
        [IgnoreGrid]
        public int? AuthenicationTypeId { get; set; }
        public string AuthenicationTypeName { get; set; }
        [IgnoreGrid]
        public int? SecurityManagerId { get; set; }
        public string SecurityManagerName { get; set; }
        [IgnoreGrid]
        public int? LicenseModelId { get; set; }
        public string LicenseModelName { get; set; }
        [IgnoreGrid]
        public int? ThirdPartyAccessId { get; set; }
        public string ThirdPartyAccessName { get; set; }
        [IgnoreGrid]
        public int? SiteResilienceId { get; set; }
        public string SiteResilienceName { get; set; }
        [IgnoreGrid]
        public int? SWDeliveryLifeCycleId { get; set; }
        public string SwDeliveryLifeCycleName { get; set; }
        [IgnoreGrid]
        public int? BusinessContinuityMethodId { get; set; }
        public string BusinessContinuityMethodName { get; set; }
        [IgnoreGrid]
        public int? InstanceResilienceId { get; set; }
        public string InstanceResilienceName { get; set; }
        public bool? CriticalNationalInfrastructureName { get; set; }
        [IgnoreGrid]
        public short? SecurityTireZoneId { get; set; }
        public string SecurityTireZoneName { get; set; }
        public bool? DaArchived { get; set; } = false;
        public string NominalCapacityLimit { get; set; }
        public string MaxAllowedLoading { get; set; }
        public string DesignedCapacityLimit { get; set; }
        public int? CriticalityRating { get; set; }
        public bool? CountrySpecificCriticality { get; set; }
        public string VerticalName { get; set; }
        [IgnoreGrid]
        public long? VerticalId { get; set; }
        public string UsedNetworkFunctions { get; set; }
        #endregion
        #region //DesignCompent
        [IgnoreGrid]
        public long? SubNetworkBoundaryId { get; set; }
        public string DesignComponentName { get; set; }
        #endregion
        #region //DesignComponentFamily
        public bool? SystemIsShared { get; set; }
        [IgnoreGrid]
        public short? SharingTypeId { get; set; }
        [IgnoreGrid]
        public short? MajorHardwareOemId { get; set; }
        public string MajorHardwareOemIdDesc { get; set; }
        [IgnoreGrid]
        public short? MajorSoftwareOemId { get; set; }
        public string MajorSoftwareOemIdDesc { get; set; }
        public string DcfDescription { get; set; }
        public string SystemTypeIdentityName { get; set; }
        public bool? Implementation { get; set; }
        public string DesignComponentFamilyName { get; set; }
        #endregion
        #region // SystemVerificationProblems
        [IgnoreGrid]
        public long? SystemVerificationProblemId { get; set; }
        [IgnoreGrid]
        public string ProblemId { get; set; }
        public DateTime? DateFound { get; set; }
        [IgnoreGrid]
        public long? ProblemCategoryId { get; set; }
        public string ProblemDescription { get; set; }
        public string MaintenanceReference { get; set; }
        public string StatusUrl { get; set; }
        public string Mitigation { get; set; }
        public string SolutionDescription { get; set; }
        public string PatchReference { get; set; }
        public string ProductUpgradeReference { get; set; }
        public string SuppleMental { get; set; }
        public string VendorCsr { get; set; }
        public string SubNetwork { get; set; }
        public string TestReport { get; set; }
        public string StandardNir { get; set; }
        public string EricssonSecReport { get; set; }
        public string SwAndStEntries { get; set; }
        public string PenTestingReport { get; set; }
        #endregion

        public string LastModifiedBy { get; set; }
        public string LastModified { get; set; }
        #region //Ticket 852 - Insert or update the details for the automation report process from the UI
        public string? ExportFilePath { get; set; }
        public string? ExportFileFormat { get; set; }
        public string? ScheduledDate { get; set; }
        public string? ExportType { get; set; }
        public string? ScheduledDayInWeek { get; set; }
        public string? ScheduledType { get; set; }
        public bool IsTestNodeRequired { get; set; }
        #endregion
    }

    public class LCMEngineering
    {
        [IgnoreGrid]
        public long LcmEngineeringId { get; set; }
        [IgnoreGrid]
        public long DesignComponentId { get; set; }
        [IgnoreGrid]
        public short? opCoId { get; set; }
        public string opCo { get; set; }
        [DateRangeGrid]
        public DateTime? SoftwareEndOfWarrantyDate { get; set; }
        public int NumberOfNodes { get; set; }
        [IgnoreGrid]
        public short? ProductImportanceId { get; set; }
        public bool Warranty { get; set; }
        public int NumberOfNodesInLab { get; set; }
        public string HardwareSheetIndex { get; set; }
        public string SoftwareSheetIndex { get; set; }
        public bool OnHardware { get; set; }
        public bool OnSoftware { get; set; }
        [IgnoreGrid]
        public short? FullOrPartialSupportId { get; set; }

        public string FullOrPartialSupportIdDesc { get; set; }

        [DateRangeGrid]
        public DateTime? HardwareEndOfSupportContract { get; set; }

        [IgnoreGrid]
        public short? HardwareSupportedId { get; set; }

        public string HardwareSupportedIdDesc { get; set; }
        public bool RenewalinProgress { get; set; }
        [DateRangeGrid]
        public DateTime? SoftwareEndOfSupportContract { get; set; }
        [IgnoreGrid]
        public short? SoftwareSupportedId { get; set; }
        public string SoftwareSupportedIdDesc { get; set; }
        public bool SparesProvisioned { get; set; }
        [DateRangeGrid]
        public DateTime? VendorEndMntDateHw { get; set; }
        [DateRangeGrid]
        public DateTime? VendorEndMnteDateSw { get; set; }
        public bool ElementCount { get; set; }
        public string HardwareSupportProvider { get; set; }
        public string HardwareSupportType { get; set; }
        public string LcmStatusEngHardware { get; set; }
        public string LcmStatusEngSoftware { get; set; }
        public string LcmStatusHardware { get; set; }
        public string LcmStatusOpsHardware { get; set; }
        public string LcmStatusOpsSoftware { get; set; }
        public string LcmStatusSoftware { get; set; }
        public string OutputToLcmHardware { get; set; }
        public string OutputToLcmSoftware { get; set; }
        public string OperationalContract { get; set; }
        public string SoftwareSupportProvider { get; set; }
        public string SoftwareSupportType { get; set; }
        [IgnoreGrid]
        public short? FullOrPartialSupportHwId { get; set; }
        public string FullOrPartialSupportHwIdDesc { get; set; }
        [IgnoreGrid]
        public string LcmSpreadSheetHwId { get; set; }
        [IgnoreGrid]
        public string LcmSpreadSheetSwId { get; set; }
        public bool? Archived { get; set; }
        public string LcmDeploymentStatus { get; set; }
        public string ResourceKey { get; set; }
        public string PreviousResourceKey { get; set; }

        public bool? IsExtendedSupportOfferedByVendor { get; set; }
        public bool? HwIsExtendedSupportOfferedByVendor { get; set; }
        public string EngKpi2 { get; set; }
        public string ManagedByGdc { get; set; }
        public string ExpLcmStatusAtEndOfFY24 { get; set; }
        public string EngineeringContactPoint { get; set; }
        public string OperationsContactPoint { get; set; }
        [DateRangeGrid]
        public DateTime? VendorEndOfVulnerabilitySecuritySupportDateValue { get; set; }
        public string AssetServiceFunctionality { get; set; }
        public string MaintenanceSupportSupplierSw { get; set; }
        public string MaintenanceSupportSupplierHw { get; set; }

        public string LastModifiedBy { get; set; }
        public string LastModified { get; set; }
        public string EduSpoc { get; set; }
        public string SubDomainSpoc { get; set; }
        public string VerticalResponsible { get; set; }
    }
    public class Assets
    {
        //[IgnoreGrid]
        public string NetworkElementAsPlannedId { get; set; }
        public bool AutomatedFeedback { get; set; }
        public bool PlannedAction { get; set; }
        public string Environment { get; set; }
        [IgnoreGrid]
        public short DeploymentStatusId { get; set; }
        public string DeploymentTypeIdDesc { get; set; }
        [IgnoreGrid]
        public short? DeploymentTypeId { get; set; }

        public string LocationName { get; set; }
        [IgnoreGrid]
        public short? NfviBundleidId { get; set; }
        [IgnoreGrid]
        public short? OrgEqpManufacturerId { get; set; }
        public string OrgEqpManufacturerIdDesc { get; set; }
        public string CapacityPlanReference { get; set; }
        public string ElementName { get; set; }
        public string AdditionalInformation1 { get; set; }
        public string NetworkConstruct { get; set; }
        public string AdditionalInformation2 { get; set; }
        public string HwResourceKey { get; set; }
        public string PreviousHwResourceKey { get; set; }
        public string SwResourceKey { get; set; }
        public string PreviousSwResourceKey { get; set; }
        public string IpAddress { get; set; }
        public string MeStatus { get; set; }
        public string MeVirtualFlg { get; set; }
        public string MeDeploymentType { get; set; }
        public string MeType { get; set; }
        public string MeSerialNumber { get; set; }
        public string MeExternalConnectionFlg { get; set; }
        public string HardwareModules { get; set; }
        public string HardwareManfacturer { get; set; }
        public string LastCheckedTime { get; set; }

        #region TSR New Attributes
        public string VodafoneUniqueIdentifier { get; set; }
        public string AssetType { get; set; }

        public string RegulatoryScope { get; set; }
        public string GeoLocation { get; set; }

        public string DateAssetMovedToLiveStatus { get; set; }
        public string DateAssetDecommissioned { get; set; }
        public string ModuleType { get; set; }
        public string ComponentVersionNumber { get; set; }
        public string DescriptionOfPlannedAction { get; set; }
        public string IdentifiedAction { get; set; }
        [DateRangeGrid]
        public DateTime? AssetLastUpgradeDate { get; set; }
        public string HardwareVendorName { get; set; }
        public string AssetDeploymentStatus { get; set; }
        public string SystemNameDns { get; set; }
        public string SystemNameBios { get; set; }
        public string LocalSiteResilience { get; set; }
        public string NameOfTheProduct { get; set; }
        public string EquipmentName { get; set; }
        #endregion
    }
    public class PlannedActivities
    {
        [IgnoreGrid]
        public long PlannedActivityId { get; set; }
        public short PlannedImplementationYear { get; set; }
        [IgnoreGrid]
        public short ActivityStatusId { get; set; }
        [IgnoreGrid]
        public short PlanningActivityStatusId { get; set; }
        public string PlannedActivity { get; set; }
        public string ActivityDetails { get; set; }
        public string DeliveryProjectName { get; set; }
        public string LocalApproval { get; set; }
        [IgnoreGrid]
        public short? DeliveryStatusId { get; set; }
        [IgnoreGrid]
        public short? ResponsibilityPhaseId { get; set; }
        [DateRangeGrid]
        public DateTime? PlannedCompletion { get; set; }

        public string RagStatus { get; set; }
        [IgnoreGrid]
        public short? RelatestoId { get; set; }
        public decimal? BudgetValue { get; set; }
        public string PlannedActivityResource { get; set; }
        [IgnoreGrid]
        public short? BudgetAvailabilityId { get; set; }
        [IgnoreGrid]
        public short? EngineeringRiskId { get; set; }
        [IgnoreGrid]
        public short? OperationalRiskId { get; set; }
        [IgnoreGrid]
        public long? LinkedToPlannedActivityId { get; set; }

        public bool IsNewServiceArchitecture { get; set; }
        public bool IsReplacementExistingSolution { get; set; }
        [IgnoreGrid]
        public short? BenefitId { get; set; }
        [IgnoreGrid]
        public short? DriverId { get; set; }
        [IgnoreGrid]
        public short? PlanningRiskId { get; set; }
        public string Currency { get; set; }
        public string Notes { get; set; }
        public string OverAllRiskEvaluation { get; set; }
        [IgnoreGrid]
        public string DeliveryProjectId { get; set; }
        public string PlanningRisk { get; set; }
        public string ProjectStatus { get; set; }
        public string RiskEngineeringNotes { get; set; }
        public string RiskOperationalNotes { get; set; }
        [IgnoreGrid]
        public string BudgetTrackingId { get; set; }
        [IgnoreGrid]
        public long? DesignComponentFamilyId { get; set; }
        [DateRangeGrid]
        public DateTime? StartDate { get; set; }
        public bool? ForAddAsset { get; set; }
        public bool? ForEditAsset { get; set; }
        [IgnoreGrid]
        public long? OriginalLcmEngineeringId { get; set; }
        public bool DeliveryPlanAvailable { get; set; }
        public string Program { get; set; }
        public string ProjectOwner { get; set; }
        public string BudgetEstimated { get; set; }
        public string BundleBudget { get; set; }
        [IgnoreGrid]
        public string BundleId { get; set; }
        public string PlannedDc { get; set; }
        public long? PlannedDcIndex { get; set; }
        public string IsPlannedActivity { get; set; }
        public string Implemented { get; set; }
        public string IsAnicallaryDataExists { get; set; }
        public string DeliveryStatus { get; set; }
        public string PlannedActivityStatus { get; set; }

        public string ActivityStatusIdDesc { get; set; }
        public string PlanningActivityStatusIdDesc { get; set; }
        public string DeliveryStatusIdDesc { get; set; }
        public string ResponsibilityPhaseIdDesc { get; set; }
        public string BudgetAvailabilityIdDesc { get; set; }
        public string EngineeringRiskIdDesc { get; set; }
        public string OperationalRiskIdDesc { get; set; }
        public string BenefitIdDesc { get; set; }
        public string DriverIdDesc { get; set; }
        public string PlanningRiskIdDesc { get; set; }
    }
    public class SystemTypes
    {
        [IgnoreGrid]
        public long SystemTypeId { get; set; }
        public string SystemTypeNameVodafone { get; set; }
        public string SystemTypeName3gpp { get; set; }
        public string SystemTypeNameOem { get; set; }
        [IgnoreGrid]
        public long? MajorSoftwareBuildsId { get; set; }
        [DateRangeGrid]
        public DateTime? ConstraintsCaling { get; set; }
        [DateRangeGrid]
        public DateTime? EndOfMaintenanceValue { get; set; }

        public string AssetVirtualized { get; set; }
        public string BuildConstruction { get; set; }

        public string SystemTypeDesignContact { get; set; }
        [IgnoreGrid]
        public int? AssetCategoryId { get; set; }
        [IgnoreGrid]
        public int? AssetClassId { get; set; }
        [IgnoreGrid]
        public int? AssetTypeId { get; set; }
        public string AssetClass { get; set; }
        public string ConstraintLcm { get; set; }
        public string VodafoneName { get; set; }


        public string SoftWareDesignContactEmail { get; set; }

        public string SoftWareVertical { get; set; }

        public string SoftWareSubdomain { get; set; }

        public string HardWareDesignContactEmail { get; set; }

        public string HardWareVertical { get; set; }

        public string HardWareSubdomain { get; set; }

        public string AssetCategoryIdDesc { get; set; }
        public string AssetClassIdDesc { get; set; }
        public string AssetTypeIdDesc { get; set; }
    }
    public class AssetCategories
    {
        public string AssetCategory { get; set; }
        public bool TakeFromAssetTypeTable { get; set; }
    }
    public class SubNetWorkBoundaries
    {
        [IgnoreGrid]
        public long Id { get; set; }
        public string SubDescription { get; set; }
        public bool? Default { get; set; }
        public string Alias { get; set; }
        public int? Order { get; set; }
        public string SwApplicationName { get; set; }
        public bool? InternetFacing { get; set; }
        public string LcmPolicy { get; set; }
        public string Criticality { get; set; }
        public bool? SecurityElement { get; set; }
        public int? GdprClassification { get; set; }
        public bool? PciSox { get; set; }
        public bool? C3C4 { get; set; }
        public bool? MissionCritical { get; set; }

        [IgnoreGrid]
        public decimal? ProductNameId { get; set; }
        [IgnoreGrid]
        public int? VodafoneNameId { get; set; }

        public string ProductNameIdDesc { get; set; }
        public string VodafoneNameIdDesc { get; set; }

    }
    public class ProductImportances
    {
        public string ProductImportance { get; set; }
    }
    public class MajorSoftWareBuilds
    {
        public string OrgEqpmanufacturer { get; set; }
        public string SoftwareVersion { get; set; }
        [DateRangeGrid]
        public DateTime? LastTimeBuyNew { get; set; }
        [DateRangeGrid]
        public DateTime? LastTimeBuyUpgrades { get; set; }
        [DateRangeGrid]
        public DateTime? LastTimeBuyExpansions { get; set; }
        [DateRangeGrid]
        public DateTime? MajEndOfMaintenance { get; set; }
        [DateRangeGrid]
        public DateTime? EndOfSupport { get; set; }
        [DateRangeGrid]
        public DateTime? GeneraAvailableDate { get; set; }
        public string DeliveryMethod { get; set; }
        [IgnoreGrid]
        public short? OperatingSystemId { get; set; }
        public String OperatingSystemIdDesc { get; set; }
        public string VulnerabilityStatus { get; set; }
        public short EomStatus { get; set; }
        public string CriticalAssetType { get; set; }
        public string ProductName { get; set; }

        public string MajorDescription { get; set; }
        public string TciBundleVersion { get; set; }
        public string TcpBundleVersion { get; set; }
        public string SoftwareDesignContact { get; set; }
    }
    public class MajorHardWareBuilds
    {
        [IgnoreGrid]
        public long MajorHardwareId { get; set; }
        public string HardwareSolution { get; set; }
        public string OtherHardwareInfo { get; set; }
        [DateRangeGrid]
        public DateTime? HardwareLastTimeBuyNew { get; set; }
        [DateRangeGrid]
        public DateTime? HardwareLastTimeBuyUpgrades { get; set; }
        [DateRangeGrid]
        public DateTime? HardwareLastTimeBuyExpansions { get; set; }
        [DateRangeGrid]
        public DateTime? HardwareEndofmaintenance { get; set; }
        [DateRangeGrid]
        public DateTime? HardwareEndofsupport { get; set; }

        public bool ProprietaryHardware { get; set; }
        [IgnoreGrid]
        public short PlatformId { get; set; }
        [IgnoreGrid]
        public short? Buildconstructionid { get; set; }

        public string PlatformIdDesc { get; set; }

        public string BuildconstructionidDesc { get; set; }

        public string HardwareType { get; set; }
        public string OperatingSystem { get; set; }
        public string TypeOfProcessor { get; set; }
        public string HardwareVulnerabilityStatus { get; set; }
        public short HardwareEomStatus { get; set; }
        public string HardwareModel { get; set; }
        [DateRangeGrid]
        public DateTime? GeneralAvailabledate { get; set; }
        public string HardwareDescription { get; set; }
        public string HardwareDesignContact { get; set; }
    }
    public class Risk
    {
        [IgnoreGrid]
        public short RiskId { get; set; }
        public int Severity { get; set; }
        public string RiskDescription { get; set; }
    }
    public class LcmAncillaryData
    {
        [IgnoreGrid]
        public string LcmAncillaryDataId { get; set; }
        public string ProductCode { get; set; }
        public bool? HandedOverToOperation { get; set; }
        public string ContractRenewalPlan { get; set; }
        public string ReasonForNoPlan { get; set; }
        public string CommentOnProjectStatus { get; set; }
        public string ScopeOfSimplification { get; set; }
        public string DataSource { get; set; }
        public string IncidentClass { get; set; }
        public string OccurenceProbability { get; set; }
        public string SecurityRiskPotential { get; set; }
        public string SecurityRiskEffective { get; set; }
        public string SecurityMitigation { get; set; }
        public string AssetOutOfScope { get; set; }
        public string IncludedInSecurityScanning { get; set; }
        [IgnoreGrid]
        public string RaId { get; set; }
        [IgnoreGrid]
        public string RequestId { get; set; }
        [DateRangeGrid]
        public DateTime? LastScanDate { get; set; }
        [DateRangeGrid]
        public DateTime? LastUpgradeDate { get; set; }
        public string EomControl { get; set; }
        public string EngUpdateTracker { get; set; }
        public string OpsUpdateTracker { get; set; }
        public string ExNetworks { get; set; }
        [IgnoreGrid]
        public string OriginalHwLcmId { get; set; }
        [IgnoreGrid]
        public string OriginalSwLcmId { get; set; }
        public string NewopsRiskEvaluation { get; set; }
        public string SecurityRiskOverall { get; set; }
        public string LabSwRelease { get; set; }
        public bool? IsPecn { get; set; }
        public bool? IsPecs { get; set; }
        public bool? IsScf { get; set; }
        public bool? IsNof { get; set; }
        public string ExposedEdgeFlag { get; set; }
        public bool? ExternalFacingFlag { get; set; }
        public string InfrastructureLocation { get; set; }
        public string VulnerabilityRating { get; set; }
        [IgnoreGrid]
        public string CyberRiskRequestId { get; set; }

    }
    public class DesignComponent
    {
        public string DesignComponentName { get; set; }
        public bool? GdprRelevant { get; set; }
        [IgnoreGrid]
        public long SubNetworkBoundaryId { get; set; }


    }

    public class DesignComponentFamily
    {
        public string DesignComponentFamilyName { get; set; }
        public bool SystemIsShared { get; set; }
        [IgnoreGrid]
        public short? SharingTypeId { get; set; }
        [IgnoreGrid]
        public short? MajorHardwareOemId { get; set; }
        public string MajorHardwareOemIdDesc { get; set; }
        [IgnoreGrid]
        public short? MajorSoftwareOemId { get; set; }
        public string MajorSoftwareOemIdDesc { get; set; }
        public string DcfDescription { get; set; }
        public string SystemtypeIdentityName { get; set; }
        public bool? Implementation { get; set; }

    }
    public class DesignAspects
    {
        [IgnoreGrid]
        public long DesignAspectId { get; set; }
        public string SubNetworkBoundary { get; set; }
        public string DesignAspectDescription { get; set; }
        public string SupportedServices { get; set; }
        [IgnoreGrid]
        public int? AuthenicationTypeId { get; set; }
        public string AuthenicationTypeName { get; set; }
        [IgnoreGrid]
        public int? SecurityManagerId { get; set; }
        public string SecurityManagerName { get; set; }
        [IgnoreGrid]
        public int? LicenseModelId { get; set; }
        public string LicenseModelName { get; set; }
        [IgnoreGrid]
        public int? ThirdPartyAccessId { get; set; }
        public string ThirdPartyAccessName { get; set; }
        [IgnoreGrid]
        public int? SiteResilienceId { get; set; }
        public string SiteResilienceName { get; set; }
        [IgnoreGrid]
        public int? SWDeliveryLifeCycleId { get; set; }
        public string SwDeliveryLifeCycleName { get; set; }
        [IgnoreGrid]
        public int? BusinessContinuityMethodId { get; set; }
        public string BusinessContinuityMethodName { get; set; }
        [IgnoreGrid]
        public int? InstanceResilienceId { get; set; }
        public string InstanceResilienceName { get; set; }
        public string CriticalNationalInfrastructureName { get; set; }
        [IgnoreGrid]
        public short? SecurityTireZoneId { get; set; }
        public string SecurityTireZoneName { get; set; }
        public bool? DaArchived { get; set; } = false;
        public string NominalCapacityLimit { get; set; }
        public string MaxAllowedLoading { get; set; }
        public string DesignedCapacityLimit { get; set; }
        public int? CriticalityRating { get; set; }
        public bool CountrySpecificCriticality { get; set; }
        public string VerticalName { get; set; }
        [IgnoreGrid]
        public long VerticalId { get; set; }
        public string UsedNetworkFunctions { get; set; }
    }

    public class SystemVerificationProblems
    {
        [IgnoreGrid]
        public long SystemVerificationProblemId { get; set; }
        [IgnoreGrid]
        public string ProblemId { get; set; }
        public DateTime? DateFound { get; set; }
        [IgnoreGrid]
        public long? ProblemCategoryId { get; set; }
        public string ProblemDescription { get; set; }
        public string MaintenanceReference { get; set; }
        public string StatusUrl { get; set; }
        public string Mitigation { get; set; }
        public string SolutionDescription { get; set; }
        public string PatchReference { get; set; }
        public string ProductUpgradeReference { get; set; }
        public string SuppleMental { get; set; }
        public string VendorCsr { get; set; }
        public string SubNetwork { get; set; }
        public string TestReport { get; set; }
        public string StandardNir { get; set; }
        public string EricssonSecReport { get; set; }
        public string SwAndStEntries { get; set; }
        public string PenTestingReport { get; set; }

    }
}