import {
  configureStore,
  combineReducers,
  applyMiddleware,
} from "@reduxjs/toolkit";
import thunk from "redux-thunk";
// import { composeWithDevTools } from 'redux-devtools-extension';
import { userReducer } from "../Reducer/Autenticazione";
import { errorReducer } from "../Reducer/NotificationReducer";
import { SystemTypeCreateReducer } from "../Reducer/SystemType/SystemTypeCreateReducer";
import { SystemTypeGridReducer } from "../Reducer/SystemType/SystemTypeGridReducer";
import { SystemTypeEditReducer } from "../Reducer/SystemType/SystemTypeEditReducer";
import { SystemTypeDeleteReducer } from "../Reducer/SystemType/SystemTypeDeleteReducer";

import { TestInfoCreateReducer } from "../Reducer/TestInfo/TestInfoCreateReducer";
import { TestInfoGridReducer } from "../Reducer/TestInfo/TestInfoGridReducer";
import { TestInfoEditReducer } from "../Reducer/TestInfo/TestInfoEditReducer";
import { TestInfoDeleteReducer } from "../Reducer/TestInfo/TestInfoDeleteReducer";

import { MajorHardwareBuildCreateReducer } from "../Reducer/MajorHardwareBuild/MajorHardwareBuildCreateReducer";
import { MajorHardwareBuildGridReducer } from "../Reducer/MajorHardwareBuild/MajorHardwareBuildGridReducer";
import { MajorHardwareBuildEditReducer } from "../Reducer/MajorHardwareBuild/MajorHardwareBuildEditReducer";
import { MajorHardwareBuildDeleteReducer } from "../Reducer/MajorHardwareBuild/MajorHardwareBuildDeleteReducer";

import { DeliveryTrackingCreateReducer } from "../Reducer/DeliveryTrackingTable/DeliveryTrackingCreateReducer";
import { DeliveryTrackingGridReducer } from "../Reducer/DeliveryTrackingTable/DeliveryTrackingGridReducer";
import { DeliveryTrackingEditReducer } from "../Reducer/DeliveryTrackingTable/DeliveryTrackingEditReducer";
import { DeliveryTrackingDeleteReducer } from "../Reducer/DeliveryTrackingTable/DeliveryTrackingDeleteReducer";

import { MajorSoftwareBuildCreateReducer } from "../Reducer/MajorSoftwareBuild/MajorSoftwareBuildCreateReducer";
import { MajorSoftwareBuildGridReducer } from "../Reducer/MajorSoftwareBuild/MajorSoftwareBuildGridReducer";
import { MajorSoftwareBuildEditReducer } from "../Reducer/MajorSoftwareBuild/MajorSoftwareBuildEditReducer";
import { MajorSoftwareBuildDeleteReducer } from "../Reducer/MajorSoftwareBuild/MajorSoftwareBuildDeleteReducer";
import { DesignComponentCreateReducer } from "../Reducer/DesignComponent/DesignComponentCreateReducer";
import { DesignComponentGridReducer } from "../Reducer/DesignComponent/DesignComponentGridReducer";
import { DesignComponentEditReducer } from "../Reducer/DesignComponent/DesignComponentEditReducer";
import { DesignComponentDeleteReducer } from "../Reducer/DesignComponent/DesignComponentDeleteReducer";

import { DesignComponentFamilyCreateReducer } from "../Reducer/DesignComponentFamily/DesignComponentFamilyCreateReducer";
import { DesignComponentFamilyGridReducer } from "../Reducer/DesignComponentFamily/DesignComponentFamilyGridReducer";
import { DesignComponentFamilyEditReducer } from "../Reducer/DesignComponentFamily/DesignComponentFamilyEditReducer";
import { DesignComponentFamilyDeleteReducer } from "../Reducer/DesignComponentFamily/DesignComponentFamilyDeleteReducer";

import { SettingsUpdatePlannedActivityCreateReducer } from "../Reducer/SettingsUpdatePlannedActivity/SettingsUpdatePlannedActivityCreateReducer";
import { SettingsUpdatePlannedActivityGridReducer } from "../Reducer/SettingsUpdatePlannedActivity/SettingsUpdatePlannedActivityGridReducer";
import { SettingsUpdatePlannedActivityEditReducer } from "../Reducer/SettingsUpdatePlannedActivity/SettingsUpdatePlannedActivityEditReducer";
import { SettingsUpdatePlannedActivityDeleteReducer } from "../Reducer/SettingsUpdatePlannedActivity/SettingsUpdatePlannedActivityDeleteReducer";

import { PlannedActivityTypesCreateReducer } from "../Reducer/PlannedActivityTypes/PlannedActivityTypesCreateReducer";
import { PlannedActivityTypesGridReducer } from "../Reducer/PlannedActivityTypes/PlannedActivityTypesGridReducer";
import { PlannedActivityTypesEditReducer } from "../Reducer/PlannedActivityTypes/PlannedActivityTypesEditReducer";
import { PlannedActivityTypesDeleteReducer } from "../Reducer/PlannedActivityTypes/PlannedActivityTypesDeleteReducer";

import { ThirdPartyHardwareComponentCreateReducer } from "../Reducer/ThirdPartyHardwareComponent/ThirdPartyHardwareComponentCreateReducer";
import { ThirdPartyHardwareComponentGridReducer } from "../Reducer/ThirdPartyHardwareComponent/ThirdPartyHardwareComponentGridReducer";
import { ThirdPartyHardwareComponentEditReducer } from "../Reducer/ThirdPartyHardwareComponent/ThirdPartyHardwareComponentEditReducer";
import { ThirdPartyHardwareComponentDeleteReducer } from "../Reducer/ThirdPartyHardwareComponent/ThirdPartyHardwareComponentDeleteReducer";
import { LcmEngineeringCreateReducer } from "../Reducer/LcmEngineering/LcmEngineeringCreateReducer";
import { LcmEngineeringGridReducer } from "../Reducer/LcmEngineering/LcmEngineeringGridReducer";
import { LcmEngineeringEditReducer } from "../Reducer/LcmEngineering/LcmEngineeringEditReducer";
import { LcmEngineeringDeleteReducer } from "../Reducer/LcmEngineering/LcmEngineeringDeleteReducer";
import { ReportHardwareGridReducer } from "../Reducer/Report/ReportHardwareGridReducer";
import { PassThroughHardwareReportGridReducer } from "../Reducer/Report/PassThroughHardwareGridReducer";
import { PassThroughSoftwareReportGridReducer } from "../Reducer/Report/PassThroughSoftwareGridReducer";
import { PassThroughReportGridReducer } from "../Reducer/Report/PassThroughReportGridReducer";
import { AssetAsisSdGridReducer } from "../Reducer/Report/AssetAsisSdGridReducer";
import { AssetAsisSdDownloadReducer } from "../Reducer/Report/AssetAsisSdDownloadReducer";

import { AssetAsisSdSwitchGridReducer } from "../Reducer/Report/AssetAsisSdSwitchGridReducer";
import { AssetAsisSdSwitchDownloadReducer } from "../Reducer/Report/AssetAsisSdSwitchDownloadReducer";

import { AssetAsisHwAncillaryGridReducer } from "../Reducer/Report/AssetAsisHwAncillaryGridReducer";
import { AssetAsisHwAncillaryDownloadReducer } from "../Reducer/Report/AssetAsisHwAncillaryDownloadReducer";

import { ReportSoftwareGridReducer } from "../Reducer/Report/ReportSoftwareGridReducer";
import { ReportSubBoundHardwareGridReducer } from "../Reducer/Report/ReportSubBoundHardwareGridReducer";
import { ReportSubBoundSoftwareGridReducer } from "../Reducer/Report/ReportSubBoundSoftwareGridReducer";
import { ViaExportHardwareGridReducer } from "../Reducer/ViaExport/ViaExportHardwareGridReducer";
import { ViaExportSoftwareGridReducer } from "../Reducer/ViaExport/ViaExportSoftwareGridReducer";
import { ExportDownloadReducer } from "../Reducer/Report/ExportDownloadReducer";
import { PassThroughReportDownloadReducer } from "../Reducer/Report/PassThroughReportDownloadReducer";
import { PassThroughHardwareReportDownloadReducer } from "../Reducer/Report/PassThroughHardwareReportDownloadReducer";
import { PassThroughSoftwareReportDownloadReducer } from "../Reducer/Report/PassThroughSoftwareReportDownloadReducer";
import { LoaderReducer } from "../Reducer/Loader";
import { ModalReducer } from "../Reducer/ModalReducer";
import { PlannedActivityGridReducer } from "../Reducer/PlannedActivity/PlannedActivityGridReducer";
import { PlannedActivityCreateReducer } from "../Reducer/PlannedActivity/PlannedActivityCreateReducer";
import { PlannedActivityEditReducer } from "../Reducer/PlannedActivity/PlannedActivityEditReducer";
import { PlannedActivityDeleteReducer } from "../Reducer/PlannedActivity/PlannedActivityDeleteReducer";

import { ServicePlanCreateReducer } from "../Reducer/ServicePlan/ServicePlanCreateReducer";
import { ServicePlanGridReducer } from "../Reducer/ServicePlan/ServicePlanGridReducer";
import { ServicePlanDeleteReducer } from "../Reducer/ServicePlan/ServicePlanDeleteReducer";

// import { composeWithDevTools } from 'remote-redux-devtools';
//#region   LookUp
import { ActivityStatusCreateReducer } from "../Reducer/LookUp/ActivityStatus/ActivityStatusCreateReducer";
import { ActivityStatusEditReducer } from "../Reducer/LookUp/ActivityStatus/ActivityStatusEditReducer";
import { ActivityStatusDeleteReducer } from "../Reducer/LookUp/ActivityStatus/ActivityStatusDeleteReducer";
import { ActivityStatusGridReducer } from "../Reducer/LookUp/ActivityStatus/ActivityStatusGridReducer";

import { NetworkElementAsPlannedCreateReducer } from "../Reducer/NetworkElementAsPlanned/NetworkElementAsPlannedCreateReducer";
import { NetworkElementAsPlannedEditReducer } from "../Reducer/NetworkElementAsPlanned/NetworkElementAsPlannedEditReducer";
import { NetworkElementAsPlannedDeleteReducer } from "../Reducer/NetworkElementAsPlanned/NetworkElementAsPlannedDeleteReducer";
import { NetworkElementAsPlannedGridReducer } from "../Reducer/NetworkElementAsPlanned/NetworkElementAsPlannedGridReducer";
import { AssetHardwareAncillariesReducer } from "../Reducer/NetworkElementAsPlanned/AssetAncillariesReducer";

import { NetworkElementAsIsCreateReducer } from "../Reducer/NetworkElementAsIs/NetworkElementAsIsCreateReducer";
import { NetworkElementAsIsEditReducer } from "../Reducer/NetworkElementAsIs/NetworkElementAsIsEditReducer";
import { NetworkElementAsIsDeleteReducer } from "../Reducer/NetworkElementAsIs/NetworkElementAsIsDeleteReducer";
import { NetworkElementAsIsGridReducer } from "../Reducer/NetworkElementAsIs/NetworkElementAsIsGridReducer";
import { NewNetworkElementAsIsGridReducer } from "../Reducer/NetworkElementAsIs/NetworkElementAsIsGridReducer";
import { IdentityGridReducer } from "../Reducer/Identity/IdentityGridReducer";
import { HardwareConfigurationGridReducer } from "../Reducer/HardwareConfiguration/HardwareConfigurationGridReducer";
import {
  SoftwareConfigurationGridReducer,
  SoftwareConfigurationOpcoReducer,
  SoftwareConfigurationOemReducer,
  SoftwareConfigurationEleReducer,
} from "../Reducer/SoftwareConfiguration/SoftwareConfigurationGridReducer";
import { SoftwareComponentGridReducer } from "../Reducer/SoftwareComponent/SoftwareComponentGridReducer";
import { ResourceKeyMasterGridReducer } from "../Reducer/ResourceKeyMaster/ResourceKeyMasterGridReducer";
import { DCFLifeCycleGridReducer } from "../Reducer/DCFLifeCycle/DCFLifeCycleGridReducer";

import { AssetCategoryCreateReducer } from "../Reducer/LookUp/AssetCategory/AssetCategoryCreateReducer";
import { AssetCategoryEditReducer } from "../Reducer/LookUp/AssetCategory/AssetCategoryEditReducer";
import { AssetCategoryDeleteReducer } from "../Reducer/LookUp/AssetCategory/AssetCategoryDeleteReducer";
import { AssetCategoryGridReducer } from "../Reducer/LookUp/AssetCategory/AssetCategoryGridReducer";

import { AssetTypeCreateReducer } from "../Reducer/LookUp/AssetType/AssetTypeCreateReducer";
import { AssetTypeEditReducer } from "../Reducer/LookUp/AssetType/AssetTypeEditReducer";
import { AssetTypeDeleteReducer } from "../Reducer/LookUp/AssetType/AssetTypeDeleteReducer";
import { AssetTypeGridReducer } from "../Reducer/LookUp/AssetType/AssetTypeGridReducer";

import { DeliveryStatusCreateReducer } from "../Reducer/LookUp/DeliveryStatus/DeliveryStatusCreateReducer";
import { DeliveryStatusEditReducer } from "../Reducer/LookUp/DeliveryStatus/DeliveryStatusEditReducer";
import { DeliveryStatusDeleteReducer } from "../Reducer/LookUp/DeliveryStatus/DeliveryStatusDeleteReducer";
import { DeliveryStatusGridReducer } from "../Reducer/LookUp/DeliveryStatus/DeliveryStatusGridReducer";

import { LCMHardwareSupportTypeCreateReducer } from "../Reducer/LookUp/LCMHardwareSupportType/LCMHardwareSupportTypeCreateReducer";
import { LCMHardwareSupportTypeEditReducer } from "../Reducer/LookUp/LCMHardwareSupportType/LCMHardwareSupportTypeEditReducer";
import { LCMHardwareSupportTypeDeleteReducer } from "../Reducer/LookUp/LCMHardwareSupportType/LCMHardwareSupportTypeDeleteReducer";
import { LCMHardwareSupportTypeGridReducer } from "../Reducer/LookUp/LCMHardwareSupportType/LCMHardwareSupportTypeGridReducer";

import { OpCoCreateReducer } from "../Reducer/LookUp/OpCo/OpCoCreateReducer";
import { OpCoEditReducer } from "../Reducer/LookUp/OpCo/OpCoEditReducer";
import { OpCoDeleteReducer } from "../Reducer/LookUp/OpCo/OpCoDeleteReducer";
import { OpCoGridReducer } from "../Reducer/LookUp/OpCo/OpCoGridReducer";

import { SharingTypeCreateReducer } from "../Reducer/LookUp/SharingType/SharingTypeCreateReducer";
import { SharingTypeEditReducer } from "../Reducer/LookUp/SharingType/SharingTypeEditReducer";
import { SharingTypeDeleteReducer } from "../Reducer/LookUp/SharingType/SharingTypeDeleteReducer";
import { SharingTypeGridReducer } from "../Reducer/LookUp/SharingType/SharingTypeGridReducer";

import { EnvironmentCreateReducer } from "../Reducer/LookUp/Environment/EnvironmentCreateReducer";
import { EnvironmentEditReducer } from "../Reducer/LookUp/Environment/EnvironmentEditReducer";
import { EnvironmentDeleteReducer } from "../Reducer/LookUp/Environment/EnvironmentDeleteReducer";
import { EnvironmentGridReducer } from "../Reducer/LookUp/Environment/EnvironmentGridReducer";

import { DeploymentStatusCreateReducer } from "../Reducer/LookUp/DeploymentStatus/DeploymentStatusCreateReducer";
import { DeploymentStatusEditReducer } from "../Reducer/LookUp/DeploymentStatus/DeploymentStatusEditReducer";
import { DeploymentStatusDeleteReducer } from "../Reducer/LookUp/DeploymentStatus/DeploymentStatusDeleteReducer";
import { DeploymentStatusGridReducer } from "../Reducer/LookUp/DeploymentStatus/DeploymentStatusGridReducer";

import { DeploymentTypeCreateReducer } from "../Reducer/LookUp/DeploymentType/DeploymentTypeCreateReducer";
import { DeploymentTypeEditReducer } from "../Reducer/LookUp/DeploymentType/DeploymentTypeEditReducer";
import { DeploymentTypeDeleteReducer } from "../Reducer/LookUp/DeploymentType/DeploymentTypeDeleteReducer";
import { DeploymentTypeGridReducer } from "../Reducer/LookUp/DeploymentType/DeploymentTypeGridReducer";

import { SecurityTireZoneCreateReducer } from "../Reducer/LookUp/SecurityTireZone/SecurityTireZoneCreateReducer";
import { SecurityTireZoneEditReducer } from "../Reducer/LookUp/SecurityTireZone/SecurityTireZoneEditReducer";
import { SecurityTireZoneDeleteReducer } from "../Reducer/LookUp/SecurityTireZone/SecurityTireZoneDeleteReducer";
import { SecurityTireZoneGridReducer } from "../Reducer/LookUp/SecurityTireZone/SecurityTireZoneGridReducer";

import { NetworkConstructCreateReducer } from "../Reducer/LookUp/NetworkConstruct/NetworkConstructCreateReducer";
import { NetworkConstructEditReducer } from "../Reducer/LookUp/NetworkConstruct/NetworkConstructEditReducer";
import { NetworkConstructDeleteReducer } from "../Reducer/LookUp/NetworkConstruct/NetworkConstructDeleteReducer";
import { NetworkConstructGridReducer } from "../Reducer/LookUp/NetworkConstruct/NetworkConstructGridReducer";

import { DriverCreateReducer } from "../Reducer/LookUp/Driver/DriverCreateReducer";
import { DriverEditReducer } from "../Reducer/LookUp/Driver/DriverEditReducer";
import { DriverDeleteReducer } from "../Reducer/LookUp/Driver/DriverDeleteReducer";
import { DriverGridReducer } from "../Reducer/LookUp/Driver/DriverGridReducer";

import { PlanningRiskCreateReducer } from "../Reducer/LookUp/PlanningRisk/PlanningRiskCreateReducer";
import { PlanningRiskEditReducer } from "../Reducer/LookUp/PlanningRisk/PlanningRiskEditReducer";
import { PlanningRiskDeleteReducer } from "../Reducer/LookUp/PlanningRisk/PlanningRiskDeleteReducer";
import { PlanningRiskGridReducer } from "../Reducer/LookUp/PlanningRisk/PlanningRiskGridReducer";

import { BenefitsCreateReducer } from "../Reducer/LookUp/Benefits/BenefitsCreateReducer";
import { BenefitsEditReducer } from "../Reducer/LookUp/Benefits/BenefitsEditReducer";
import { BenefitsDeleteReducer } from "../Reducer/LookUp/Benefits/BenefitsDeleteReducer";
import { BenefitsGridReducer } from "../Reducer/LookUp/Benefits/BenefitsGridReducer";

import { ActivityDetailsCreateReducer } from "../Reducer/LookUp/ActivityDetails/ActivityDetailsCreateReducer";
import { ActivityDetailsEditReducer } from "../Reducer/LookUp/ActivityDetails/ActivityDetailsEditReducer";
import { ActivityDetailsDeleteReducer } from "../Reducer/LookUp/ActivityDetails/ActivityDetailsDeleteReducer";
import { ActivityDetailsGridReducer } from "../Reducer/LookUp/ActivityDetails/ActivityDetailsGridReducer";

import { ServiceBoundaryCreateReducer } from "../Reducer/LookUp/ServiceBoundary/ServiceBoundaryCreateReducer";
import { ServiceBoundaryEditReducer } from "../Reducer/LookUp/ServiceBoundary/ServiceBoundaryEditReducer";
import { ServiceBoundaryDeleteReducer } from "../Reducer/LookUp/ServiceBoundary/ServiceBoundaryDeleteReducer";
import { ServiceBoundaryGridReducer } from "../Reducer/LookUp/ServiceBoundary/ServiceBoundaryGridReducer";

import { PlannedActivityNetworkElementCreateReducer } from "../Reducer/LookUp/PlannedActivityNetworkElement/PlannedActivityNetworkElementCreateReducer";
import { PlannedActivityNetworkElementEditReducer } from "../Reducer/LookUp/PlannedActivityNetworkElement/PlannedActivityNetworkElementEditReducer";
import { PlannedActivityNetworkElementDeleteReducer } from "../Reducer/LookUp/PlannedActivityNetworkElement/PlannedActivityNetworkElementDeleteReducer";
import { PlannedActivityNetworkElementGridReducer } from "../Reducer/LookUp/PlannedActivityNetworkElement/PlannedActivityNetworkElementGridReducer";

import { LocationCreateReducer } from "../Reducer/LookUp/Location/LocationCreateReducer";
import { LocationEditReducer } from "../Reducer/LookUp/Location/LocationEditReducer";
import { LocationDeleteReducer } from "../Reducer/LookUp/Location/LocationDeleteReducer";
import { LocationGridReducer } from "../Reducer/LookUp/Location/LocationGridReducer";

import { InterVMTypeGridReducer } from "../Reducer/LookUp/InterVMType/InterVMTypeGridReducer";
import { InterVMTypeDeleteReducer } from "../Reducer/LookUp/InterVMType/InterVMTypeDeleteReducer";
import { InterVMTypeEditReducer } from "../Reducer/LookUp/InterVMType/InterVMTypeEditReducer";
import { InterVMTypeCreateReducer } from "../Reducer/LookUp/InterVMType/InterVMTypeCreateReducer";

import { VNFHardwareTypeGridReducer } from "../Reducer/LookUp/VNFHardwareType/VNFHardwareTypeGridReducer";
import { VNFHardwareTypeDeleteReducer } from "../Reducer/LookUp/VNFHardwareType/VNFHardwareTypeDeleteReducer";
import { VNFHardwareTypeEditReducer } from "../Reducer/LookUp/VNFHardwareType/VNFHardwareTypeEditReducer";
import { VNFHardwareTypeCreateReducer } from "../Reducer/LookUp/VNFHardwareType/VNFHardwareTypeCreateReducer";

import { IntraVMTypeGridReducer } from "../Reducer/LookUp/IntraVMType/IntraVMTypeGridReducer";
import { IntraVMTypeDeleteReducer } from "../Reducer/LookUp/IntraVMType/IntraVMTypeDeleteReducer";
import { IntraVMTypeEditReducer } from "../Reducer/LookUp/IntraVMType/IntraVMTypeEditReducer";
import { IntraVMTypeCreateReducer } from "../Reducer/LookUp/IntraVMType/IntraVMTypeCreateReducer";

import { ServiceMasterGridReducer } from "../Reducer/LookUp/ServiceMaster/ServiceMasterGridReducer";
import { ServiceMasterEditReducer } from "../Reducer/LookUp/ServiceMaster/ServiceMasterEditReducer";
import { ServiceMasterCreateReducer } from "../Reducer/LookUp/ServiceMaster/ServiceMasterCreateReducer";
import { ServiceMasterDeleteReducer } from "../Reducer/LookUp/ServiceMaster/ServiceMasterDeleteReducer";

import { AssetMapInfoGridReducer } from "../Reducer/LookUp/AssetMapInfo/AssetMapInfoGridReducer";
import { AssetMapInfoCreateReducer } from "../Reducer/LookUp/AssetMapInfo/AssetMapInfoCreateReducer";
import { AssetMapInfoEditReducer } from "../Reducer/LookUp/AssetMapInfo/AssetMapInfoEditReducer";
import { AssetMapInfoDeleteReducer } from "../Reducer/LookUp/AssetMapInfo/AssetMapInfoDeleteReducer";

import { MajorHardwareMTGridReducer } from "../Reducer/LookUp/MajorHardwareMT/MajorHardwareMTGridReducer";
import { MajorHardwareMTCreateReducer } from "../Reducer/LookUp/MajorHardwareMT/MajorHardwareMTCreateReducer";
import { MajorHardwareMTEditReducer } from "../Reducer/LookUp/MajorHardwareMT/MajorHardwareMTEditReducer";
import { MajorHardwareMTDeleteReducer } from "../Reducer/LookUp/MajorHardwareMT/MajorHardwareMTDeleteReducer";

import { VMWorkloadTypeGridReducer } from "../Reducer/LookUp/VMWorkloadType/VMWorkloadTypeGridReducer";
import { VMWorkloadTypeDeleteReducer } from "../Reducer/LookUp/VMWorkloadType/VMWorkloadTypeDeleteReducer";
import { VMWorkloadTypeEditReducer } from "../Reducer/LookUp/VMWorkloadType/VMWorkloadTypeEditReducer";
import { VMWorkloadTypeCreateReducer } from "../Reducer/LookUp/VMWorkloadType/VMWorkloadTypeCreateReducer";

import { VMTypeNameGridReducer } from "../Reducer/LookUp/VMTypeName/VMTypeNameGridReducer";
import { VMTypeNameDeleteReducer } from "../Reducer/LookUp/VMTypeName/VMTypeNameDeleteReducer";
import { VMTypeNameEditReducer } from "../Reducer/LookUp/VMTypeName/VMTypeNameEditReducer";
import { VMTypeNameCreateReducer } from "../Reducer/LookUp/VMTypeName/VMTypeNameCreateReducer";

import { VNFClusterNameGridReducer } from "../Reducer/LookUp/VNFClusterName/VNFClusterNameGridReducer";
import { VNFClusterNameDeleteReducer } from "../Reducer/LookUp/VNFClusterName/VNFClusterNameDeleteReducer";
import { VNFClusterNameEditReducer } from "../Reducer/LookUp/VNFClusterName/VNFClusterNameEditReducer";
import { VNFClusterNameCreateReducer } from "../Reducer/LookUp/VNFClusterName/VNFClusterNameCreateReducer";

import { VNFNameGridReducer } from "../Reducer/LookUp/VNFName/VNFNameGridReducer";
import { VNFNameDeleteReducer } from "../Reducer/LookUp/VNFName/VNFNameDeleteReducer";
import { VNFNameEditReducer } from "../Reducer/LookUp/VNFName/VNFNameEditReducer";
import { VNFNameCreateReducer } from "../Reducer/LookUp/VNFName/VNFNameCreateReducer";

import { CNFNameGridReducer } from "../Reducer/LookUp/CNFName/CNFNameGridReducer";
import { CNFNameDeleteReducer } from "../Reducer/LookUp/CNFName/CNFNameDeleteReducer";
import { CNFNameEditReducer } from "../Reducer/LookUp/CNFName/CNFNameEditReducer";
import { CNFNameCreateReducer } from "../Reducer/LookUp/CNFName/CNFNameCreateReducer";

import { CNFClusterGridReducer } from "../Reducer/LookUp/CNFCluster/CNFClusterGridReducer";
import { CNFClusterDeleteReducer } from "../Reducer/LookUp/CNFCluster/CNFClusterDeleteReducer";
import { CNFClusterEditReducer } from "../Reducer/LookUp/CNFCluster/CNFClusterEditReducer";
import { CNFClusterCreateReducer } from "../Reducer/LookUp/CNFCluster/CNFClusterCreateReducer";

import { CNFPriorityGridReducer } from "../Reducer/LookUp/CNFPriority/CNFPriorityGridReducer";
import { CNFPriorityDeleteReducer } from "../Reducer/LookUp/CNFPriority/CNFPriorityDeleteReducer";
import { CNFPriorityEditReducer } from "../Reducer/LookUp/CNFPriority/CNFPriorityEditReducer";
import { CNFPriorityCreateReducer } from "../Reducer/LookUp/CNFPriority/CNFPriorityCreateReducer";

import { CNFHardwareTypeGridReducer } from "../Reducer/LookUp/CNFHardwareType/CNFHardwareTypeGridReducer";
import { CNFHardwareTypeDeleteReducer } from "../Reducer/LookUp/CNFHardwareType/CNFHardwareTypeDeleteReducer";
import { CNFHardwareTypeEditReducer } from "../Reducer/LookUp/CNFHardwareType/CNFHardwareTypeEditReducer";
import { CNFHardwareTypeCreateReducer } from "../Reducer/LookUp/CNFHardwareType/CNFHardwareTypeCreateReducer";

import { CNFFunctionStandardNameGridReducer } from "../Reducer/LookUp/CNFFunctionStandardName/CNFFunctionStandardNameGridReducer";
import { CNFFunctionStandardNameDeleteReducer } from "../Reducer/LookUp/CNFFunctionStandardName/CNFFunctionStandardNameDeleteReducer";
import { CNFFunctionStandardNameEditReducer } from "../Reducer/LookUp/CNFFunctionStandardName/CNFFunctionStandardNameEditReducer";
import { CNFFunctionStandardNameCreateReducer } from "../Reducer/LookUp/CNFFunctionStandardName/CNFFunctionStandardNameCreateReducer";

import { PodTypeInfoGridReducer } from "../Reducer/LookUp/PodTypeInfo/PodTypeInfoGridReducer";
import { PodTypeInfoDeleteReducer } from "../Reducer/LookUp/PodTypeInfo/PodTypeInfoDeleteReducer";
import { PodTypeInfoEditReducer } from "../Reducer/LookUp/PodTypeInfo/PodTypeInfoEditReducer";
import { PodTypeInfoCreateReducer } from "../Reducer/LookUp/PodTypeInfo/PodTypeInfoCreateReducer";

import { FullOrPartialResourceCreateReducer } from "../Reducer/LookUp/FullOrPartialResource/FullOrPartialResourceCreateReducer";
import { FullOrPartialResourceEditReducer } from "../Reducer/LookUp/FullOrPartialResource/FullOrPartialResourceEditReducer";
import { FullOrPartialResourceDeleteReducer } from "../Reducer/LookUp/FullOrPartialResource/FullOrPartialResourceDeleteReducer";
import { FullOrPartialResourceGridReducer } from "../Reducer/LookUp/FullOrPartialResource/FullOrPartialResourceGridReducer";

import { SupportedResourceCreateReducer } from "../Reducer/LookUp/SupportedResource/SupportedResourceCreateReducer";
import { SupportedResourceEditReducer } from "../Reducer/LookUp/SupportedResource/SupportedResourceEditReducer";
import { SupportedResourceDeleteReducer } from "../Reducer/LookUp/SupportedResource/SupportedResourceDeleteReducer";
import { SupportedResourceGridReducer } from "../Reducer/LookUp/SupportedResource/SupportedResourceGridReducer";

import { ReasonCheckboxCreateReducer } from "../Reducer/LookUp/ReasonCheckbox/ReasonCheckboxCreateReducer";
import { ReasonCheckboxEditReducer } from "../Reducer/LookUp/ReasonCheckbox/ReasonCheckboxEditReducer";
import { ReasonCheckboxDeleteReducer } from "../Reducer/LookUp/ReasonCheckbox/ReasonCheckboxDeleteReducer";
import { ReasonCheckboxGridReducer } from "../Reducer/LookUp/ReasonCheckbox/ReasonCheckboxGridReducer";

import { OperationalRiskCreateReducer } from "../Reducer/LookUp/OperationalRisk/OperationalRiskCreateReducer";
import { OperationalRiskEditReducer } from "../Reducer/LookUp/OperationalRisk/OperationalRiskEditReducer";
import { OperationalRiskDeleteReducer } from "../Reducer/LookUp/OperationalRisk/OperationalRiskDeleteReducer";
import { OperationalRiskGridReducer } from "../Reducer/LookUp/OperationalRisk/OperationalRiskGridReducer";

import { BudgetAvailabilityCreateReducer } from "../Reducer/LookUp/BudgetAvailability/BudgetAvailabilityCreateReducer";
import { BudgetAvailabilityEditReducer } from "../Reducer/LookUp/BudgetAvailability/BudgetAvailabilityEditReducer";
import { BudgetAvailabilityDeleteReducer } from "../Reducer/LookUp/BudgetAvailability/BudgetAvailabilityDeleteReducer";
import { BudgetAvailabilityGridReducer } from "../Reducer/LookUp/BudgetAvailability/BudgetAvailabilityGridReducer";

import { SupportProviderCreateReducer } from "../Reducer/LookUp/SupportProvider/SupportProviderCreateReducer";
import { SupportProviderEditReducer } from "../Reducer/LookUp/SupportProvider/SupportProviderEditReducer";
import { SupportProviderDeleteReducer } from "../Reducer/LookUp/SupportProvider/SupportProviderDeleteReducer";
import { SupportProviderGridReducer } from "../Reducer/LookUp/SupportProvider/SupportProviderGridReducer";

import { EndOfSupportContractCreateReducer } from "../Reducer/LookUp/EndOfSupportContract/EndOfSupportContractCreateReducer";
import { EndOfSupportContractEditReducer } from "../Reducer/LookUp/EndOfSupportContract/EndOfSupportContractEditReducer";
import { EndOfSupportContractDeleteReducer } from "../Reducer/LookUp/EndOfSupportContract/EndOfSupportContractDeleteReducer";
import { EndOfSupportContractGridReducer } from "../Reducer/LookUp/EndOfSupportContract/EndOfSupportContractGridReducer";

import { BuildConstructionCreateReducer } from "../Reducer/LookUp/BuildConstruction/BuildConstructionCreateReducer";
import { BuildConstructionEditReducer } from "../Reducer/LookUp/BuildConstruction/BuildConstructionEditReducer";
import { BuildConstructionDeleteReducer } from "../Reducer/LookUp/BuildConstruction/BuildConstructionDeleteReducer";
import { BuildConstructionGridReducer } from "../Reducer/LookUp/BuildConstruction/BuildConstructionGridReducer";

import { NFVIBundleIDCreateReducer } from "../Reducer/LookUp/NFVIBundleID/NFVIBundleIDCreateReducer";
import { NFVIBundleIDEditReducer } from "../Reducer/LookUp/NFVIBundleID/NFVIBundleIDEditReducer";
import { NFVIBundleIDDeleteReducer } from "../Reducer/LookUp/NFVIBundleID/NFVIBundleIDDeleteReducer";
import { NFVIBundleIDGridReducer } from "../Reducer/LookUp/NFVIBundleID/NFVIBundleIDGridReducer";

import { EquipmentStatusCreateReducer } from "../Reducer/LookUp/EquipmentStatus/EquipmentStatusCreateReducer";
import { EquipmentStatusEditReducer } from "../Reducer/LookUp/EquipmentStatus/EquipmentStatusEditReducer";
import { EquipmentStatusDeleteReducer } from "../Reducer/LookUp/EquipmentStatus/EquipmentStatusDeleteReducer";
import { EquipmentStatusGridReducer } from "../Reducer/LookUp/EquipmentStatus/EquipmentStatusGridReducer";

import { NFVIStatusCreateReducer } from "../Reducer/LookUp/NFVIStatus/NFVIStatusCreateReducer";
import { NFVIStatusEditReducer } from "../Reducer/LookUp/NFVIStatus/NFVIStatusEditReducer";
import { NFVIStatusDeleteReducer } from "../Reducer/LookUp/NFVIStatus/NFVIStatusDeleteReducer";
import { NFVIStatusGridReducer } from "../Reducer/LookUp/NFVIStatus/NFVIStatusGridReducer";

import { SubDomainSpocCreateReducer } from "../Reducer/LookUp/SubDomainSpoc/SubDomainSpocCreateReducer";
import { SubDomainSpocEditReducer } from "../Reducer/LookUp/SubDomainSpoc/SubDomainSpocEditReducer";
import { SubDomainSpocDeleteReducer } from "../Reducer/LookUp/SubDomainSpoc/SubDomainSpocDeleteReducer";
import { SubDomainSpocGridReducer } from "../Reducer/LookUp/SubDomainSpoc/SubDomainSpocGridReducer";

import { VNFDesignComponentCreateReducer } from "../Reducer/LookUp/VNFDesignComponent/VNFDesignComponentCreateReducer";
import { VNFDesignComponentEditReducer } from "../Reducer/LookUp/VNFDesignComponent/VNFDesignComponentEditReducer";
import { VNFDesignComponentDeleteReducer } from "../Reducer/LookUp/VNFDesignComponent/VNFDesignComponentDeleteReducer";
import { VNFDesignComponentGridReducer } from "../Reducer/LookUp/VNFDesignComponent/VNFDesignComponentGridReducer";

import { HardwareSolutionResourceCreateReducer } from "../Reducer/LookUp/HardwareSolutionResource/HardwareSolutionResourceCreateReducer";
import { HardwareSolutionResourceEditReducer } from "../Reducer/LookUp/HardwareSolutionResource/HardwareSolutionResourceEditReducer";
import { HardwareSolutionResourceDeleteReducer } from "../Reducer/LookUp/HardwareSolutionResource/HardwareSolutionResourceDeleteReducer";
import { HardwareSolutionResourceGridReducer } from "../Reducer/LookUp/HardwareSolutionResource/HardwareSolutionResourceGridReducer";

import { OperatingSystemCreateReducer } from "../Reducer/LookUp/OperatingSystem/OperatingSystemCreateReducer";
import { OperatingSystemEditReducer } from "../Reducer/LookUp/OperatingSystem/OperatingSystemEditReducer";
import { OperatingSystemDeleteReducer } from "../Reducer/LookUp/OperatingSystem/OperatingSystemDeleteReducer";
import { OperatingSystemGridReducer } from "../Reducer/LookUp/OperatingSystem/OperatingSystemGridReducer";

import { AssetClassCreateReducer } from "../Reducer/LookUp/AssetClass/AssetClassCreateReducer";
import { AssetClassEditReducer } from "../Reducer/LookUp/AssetClass/AssetClassEditReducer";
import { AssetClassDeleteReducer } from "../Reducer/LookUp/AssetClass/AssetClassDeleteReducer";
import { AssetClassGridReducer } from "../Reducer/LookUp/AssetClass/AssetClassGridReducer";

import { NetworkFunctionCreateReducer } from "../Reducer/LookUp/NetworkFunction/NetworkFunctionCreateReucer";

import { LCMPlannedActionResourceCreateReducer } from "../Reducer/LookUp/LCMPlannedActionResource/LCMPlannedActionResourceCreateReducer";
import { LCMPlannedActionResourceEditReducer } from "../Reducer/LookUp/LCMPlannedActionResource/LCMPlannedActionResourceEditReducer";
import { LCMPlannedActionResourceDeleteReducer } from "../Reducer/LookUp/LCMPlannedActionResource/LCMPlannedActionResourceDeleteReducer";
import { LCMPlannedActionResourceGridReducer } from "../Reducer/LookUp/LCMPlannedActionResource/LCMPlannedActionResourceGridReducer";

import { LCMSoftwareSupportTypeCreateReducer } from "../Reducer/LookUp/LCMSoftwareSupportType/LCMSoftwareSupportTypeCreateReducer";
import { LCMSoftwareSupportTypeEditReducer } from "../Reducer/LookUp/LCMSoftwareSupportType/LCMSoftwareSupportTypeEditReducer";
import { LCMSoftwareSupportTypeDeleteReducer } from "../Reducer/LookUp/LCMSoftwareSupportType/LCMSoftwareSupportTypeDeleteReducer";
import { LCMSoftwareSupportTypeGridReducer } from "../Reducer/LookUp/LCMSoftwareSupportType/LCMSoftwareSupportTypeGridReducer";

import { VulnerabilityStatusCreateReducer } from "../Reducer/LookUp/VulnerabilityStatus/VulnerabilityStatusCreateReducer";
import { VulnerabilityStatusEditReducer } from "../Reducer/LookUp/VulnerabilityStatus/VulnerabilityStatusEditReducer";
import { VulnerabilityStatusDeleteReducer } from "../Reducer/LookUp/VulnerabilityStatus/VulnerabilityStatusDeleteReducer";
import { VulnerabilityStatusGridReducer } from "../Reducer/LookUp/VulnerabilityStatus/VulnerabilityStatusGridReducer";

import { VerticalResponsibleCreateReducer } from "../Reducer/LookUp/VerticalResponsible/VerticalResponsibleCreateReducer";
import { VerticalResponsibleEditReducer } from "../Reducer/LookUp/VerticalResponsible/VerticalResponsibleEditReducer";
import { VerticalResponsibleDeleteReducer } from "../Reducer/LookUp/VerticalResponsible/VerticalResponsibleDeleteReducer";
import { VerticalResponsibleGridReducer } from "../Reducer/LookUp/VerticalResponsible/VerticalResponsibleGridReducer";

import { SubDomainResponsibleCreateReducer } from "../Reducer/LookUp/SubDomainResponsible/SubDomainResponsibleCreateReducer";
import { SubDomainResponsibleEditReducer } from "../Reducer/LookUp/SubDomainResponsible/SubDomainResponsibleEditReducer";
import { SubDomainResponsibleDeleteReducer } from "../Reducer/LookUp/SubDomainResponsible/SubDomainResponsibleDeleteReducer";
import { SubDomainResponsibleGridReducer } from "../Reducer/LookUp/SubDomainResponsible/SubDomainResponsibleGridReducer";

import { ResponsibilityPhaseCreateReducer } from "../Reducer/LookUp/ResponsibilityPhase/ResponsibilityPhaseCreateReducer";
import { ResponsibilityPhaseEditReducer } from "../Reducer/LookUp/ResponsibilityPhase/ResponsibilityPhaseEditReducer";
import { ResponsibilityPhaseDeleteReducer } from "../Reducer/LookUp/ResponsibilityPhase/ResponsibilityPhaseDeleteReducer";
import { ResponsibilityPhaseGridReducer } from "../Reducer/LookUp/ResponsibilityPhase/ResponsibilityPhaseGridReducer";

import { RelatesToResourceCreateReducer } from "../Reducer/LookUp/RelatesToResource/RelatesToResourceCreateReducer";
import { RelatesToResourceEditReducer } from "../Reducer/LookUp/RelatesToResource/RelatesToResourceEditReducer";
import { RelatesToResourceDeleteReducer } from "../Reducer/LookUp/RelatesToResource/RelatesToResourceDeleteReducer";
import { RelatesToResourceGridReducer } from "../Reducer/LookUp/RelatesToResource/RelatesToResourceGridReducer";

import { ProductImportanceCreateReducer } from "../Reducer/LookUp/ProductImportance/ProductImportanceCreateReducer";
import { ProductImportanceEditReducer } from "../Reducer/LookUp/ProductImportance/ProductImportanceEditReducer";
import { ProductImportanceDeleteReducer } from "../Reducer/LookUp/ProductImportance/ProductImportanceDeleteReducer";
import { ProductImportanceGridReducer } from "../Reducer/LookUp/ProductImportance/ProductImportanceGridReducer";

import { PlanningActivityStatusCreateReducer } from "../Reducer/LookUp/PlanningActivityStatus/PlanningActivityStatusCreateReducer";
import { PlanningActivityStatusEditReducer } from "../Reducer/LookUp/PlanningActivityStatus/PlanningActivityStatusEditReducer";
import { PlanningActivityStatusDeleteReducer } from "../Reducer/LookUp/PlanningActivityStatus/PlanningActivityStatusDeleteReducer";
import { PlanningActivityStatusGridReducer } from "../Reducer/LookUp/PlanningActivityStatus/PlanningActivityStatusGridReducer";

import { PlannedActivityResourceCreateReducer } from "../Reducer/LookUp/PlannedActivityResource/PlannedActivityResourceCreateReducer";
import { PlannedActivityResourceEditReducer } from "../Reducer/LookUp/PlannedActivityResource/PlannedActivityResourceEditReducer";
import { PlannedActivityResourceDeleteReducer } from "../Reducer/LookUp/PlannedActivityResource/PlannedActivityResourceDeleteReducer";
import { PlannedActivityResourceGridReducer } from "../Reducer/LookUp/PlannedActivityResource/PlannedActivityResourceGridReducer";

import { OriginalEquipmentManufacturerCreateReducer } from "../Reducer/LookUp/OriginalEquipmentManufacturer/OriginalEquipmentManufacturerCreateReducer";
import { OriginalEquipmentManufacturerEditReducer } from "../Reducer/LookUp/OriginalEquipmentManufacturer/OriginalEquipmentManufacturerEditReducer";
import { OriginalEquipmentManufacturerDeleteReducer } from "../Reducer/LookUp/OriginalEquipmentManufacturer/OriginalEquipmentManufacturerDeleteReducer";
import { OriginalEquipmentManufacturerGridReducer } from "../Reducer/LookUp/OriginalEquipmentManufacturer/OriginalEquipmentManufacturerGridReducer";

import { ComponentManufacturerEditReducer } from "../Reducer/LookUp/ComponentManufacturer/ComponentManufacturerEditReducer";
import { ComponentManufacturerCreateReducer } from "../Reducer/LookUp/ComponentManufacturer/ComponentManufacturerCreateReducer";
import { ComponentManufacturerDeleteReducer } from "../Reducer/LookUp/ComponentManufacturer/ComponentManufacturerDeleteReducer";
import { ComponentManufacturerGridReducer } from "../Reducer/LookUp/ComponentManufacturer/ComponentManufacturerGridReducer";

import { BundleUpgradeInitiativeCreateReducer } from "../Reducer/BundleUpgradeInitiative/BundleUpgradeInitiativeCreateReducer";
import { BundleUpgradeInitiativeEditReducer } from "../Reducer/BundleUpgradeInitiative/BundleUpgradeInitiativeEditReducer";
import { BundleUpgradeInitiativeDeleteReducer } from "../Reducer/BundleUpgradeInitiative/BundleUpgradeInitiativeDeleteReducer";
import { BundleUpgradeInitiativeGridReducer } from "../Reducer/BundleUpgradeInitiative/BundleUpgradeInitiativeGridReducer";

import { VNFTransitionCreateReducer } from "../Reducer/VNFTransition/VNFTransitionCreateReducer";
import { VNFTransitionEditReducer } from "../Reducer/VNFTransition/VNFTransitionEditReducer";
import { VNFTransitionDeleteReducer } from "../Reducer/VNFTransition/VNFTransitionDeleteReducer";
import { VNFTransitionGridReducer } from "../Reducer/VNFTransition/VNFTransitionGridReducer";

import { VolteKPICreateReducer } from "../Reducer/VolteKPI/VolteKPICreateReducer";
import { VolteKPIEditReducer } from "../Reducer/VolteKPI/VolteKPIEditReducer";
import { VolteKPIDeleteReducer } from "../Reducer/VolteKPI/VolteKPIDeleteReducer";
import { VolteKPIGridReducer } from "../Reducer/VolteKPI/VolteKPIGridReducer";

import { WorklogApprovalsGridReducer } from "../Reducer/WorklogApproval/WorklogApprovalGridReducer";
import { WorklogApprovalsEditReducer } from "../Reducer/WorklogApproval/WorklogApprovalEditReducer";

import { ReportVolteKPIGridReducer } from "../Reducer/Report/ReportVolteKPIGridReducer";
import { ReportPATGridReducer } from "../Reducer/Report/PlannedActivityTrackerReducer";

import { ExportVolteKPIReducer } from "../Reducer/Report/ExportVolteKPIReducer";

import { NFVITransitionCreateReducer } from "../Reducer/NFVITransition/NFVITransitionCreateReducer";
import { NFVITransitionEditReducer } from "../Reducer/NFVITransition/NFVITransitionEditReducer";
import { NFVITransitionDeleteReducer } from "../Reducer/NFVITransition/NFVITransitionDeleteReducer";
import { NFVITransitionGridReducer } from "../Reducer/NFVITransition/NFVITransitionGridReducer";
import { PlatformCreateReducer } from "../Reducer/LookUp/Platform/PlatformCreateReducer";
import { PlatformEditReducer } from "../Reducer/LookUp/Platform/PlatformEditReducer";
import { PlatformDeleteReducer } from "../Reducer/LookUp/Platform/PlatformDeleteReducer";
import { PlatformGridReducer } from "../Reducer/LookUp/Platform/PlatformGridReducer";
import { SystemFunctionCreateReducer } from "../Reducer/LookUp/SystemFunction/SystemFunctionCreateReducer";
import { SystemFunctionEditReducer } from "../Reducer/LookUp/SystemFunction/SystemFunctionEditReducer";
import { SystemFunctionDeleteReducer } from "../Reducer/LookUp/SystemFunction/SystemFunctionDeleteReducer";
import { SystemFunctionGridReducer } from "../Reducer/LookUp/SystemFunction/SystemFunctionGridReducer";
import { HardwareTypeCreateReducer } from "../Reducer/LookUp/HardwareType/HardwareTypeCreateReducer";
import { HardwareTypeEditReducer } from "../Reducer/LookUp/HardwareType/HardwareTypeEditReducer";
import { HardwareTypeDeleteReducer } from "../Reducer/LookUp/HardwareType/HardwareTypeDeleteReducer";
import { HardwareTypeGridReducer } from "../Reducer/LookUp/HardwareType/HardwareTypeGridReducer";
import { ExternalRefreshReducer } from "../Reducer/ExternalRefreshReducer";
import { ForeignIndexReducer } from "../Reducer/ForeignIndex/ForeignIndexReducer";
import { NetworkFunctionGridReducer } from "../Reducer/LookUp/NetworkFunction/NetworkFunctionGridReducer";
import { NetworkFunctionDeleteReducer } from "../Reducer/LookUp/NetworkFunction/NetworkFunctionDeleteReducer";
import { NetworkFunctionEditReducer } from "../Reducer/LookUp/NetworkFunction/NetworkFunctionEditReducer";
import { CustomerWheelCreateReducer } from "../Reducer/LookUp/CustomerWheel/CustomerWheelCreateReucer";
import { CustomerWheelGridReducer } from "../Reducer/LookUp/CustomerWheel/CustomerWheelGridReducer";
import { CustomerWheelDeleteReducer } from "../Reducer/LookUp/CustomerWheel/CustomerWheelDeleteReducer";
import { CustomerWheelEditReducer } from "../Reducer/LookUp/CustomerWheel/CustomerWheelEditReducer";
import { SupportedServiceCreateReducer } from "../Reducer/LookUp/SupportedService/SupportedServiceCreateReducer";
import { SupportedServiceEditReducer } from "../Reducer/LookUp/SupportedService/SupportedServiceEditReducer";
import { SupportedServiceDeleteReducer } from "../Reducer/LookUp/SupportedService/SupportedServiceDeleteReducer";
import { SupportedServiceGridReducer } from "../Reducer/LookUp/SupportedService/SupportedServiceGridReducer";
import { SubNetworkBoundaryCreateReducer } from "../Reducer/LookUp/SubNetworkBoundary/SubNetworkBoundaryCreateReducer";
import { SubNetworkBoundaryEditReducer } from "../Reducer/LookUp/SubNetworkBoundary/SubNetworkBoundaryEditReducer";
import { SubNetworkBoundaryDeleteReducer } from "../Reducer/LookUp/SubNetworkBoundary/SubNetworkBoundaryDeleteReducer";
import { SubNetworkBoundaryGridReducer } from "../Reducer/LookUp/SubNetworkBoundary/SubNetworkBoundaryGridReducer";
import { CriticalAssetTypeCreateReducer } from "../Reducer/LookUp/CriticalAssetType/CriticalAssetTypeCreateReucer";
import { CriticalAssetTypeEditReducer } from "../Reducer/LookUp/CriticalAssetType/CriticalAssetTypeEditReducer";
import { CriticalAssetTypeDeleteReducer } from "../Reducer/LookUp/CriticalAssetType/CriticalAssetTypeDeleteReducer";
import { CriticalAssetTypeGridReducer } from "../Reducer/LookUp/CriticalAssetType/CriticalAssetTypeGridReducer";

import { ClassCreateReducer } from "../Reducer/LookUp/Class/ClassCreateReducer";
import { ClassEditReducer } from "../Reducer/LookUp/Class/ClassEditReducer";
import { ClassDeleteReducer } from "../Reducer/LookUp/Class/ClassDeleteReducer";
import { ClassGridReducer } from "../Reducer/LookUp/Class/ClassGridReducer";

import { TypeCreateReducer } from "../Reducer/LookUp/Type/TypeCreateReducer";
import { TypeEditReducer } from "../Reducer/LookUp/Type/TypeEditReducer";
import { TypeDeleteReducer } from "../Reducer/LookUp/Type/TypeDeleteReducer";
import { TypeGridReducer } from "../Reducer/LookUp/Type/TypeGridReducer";

import { SharedLookUpEditReducer } from "../Reducer/LookUp/SharedLookUp/SharedLookUpEditReducer";
import { SharedLookUpCreateReducer } from "../Reducer/LookUp/SharedLookUp/SharedLookUpCreateReucer";
import { SharedLookUpDeleteReducer } from "../Reducer/LookUp/SharedLookUp/SharedLookUpDeleteReducer";
import { SharedLookUpGridReducer } from "../Reducer/LookUp/SharedLookUp/SharedLookUpGridReducer";
import { DesignAspectCreateReducer } from "../Reducer/DesignAspect/DesignAspectCreateReducer";
import { DesignAspectGridReducer } from "../Reducer/DesignAspect/DesignAspectGridReducer";
import { DesignAspectEditReducer } from "../Reducer/DesignAspect/DesignAspectEditReducer";
import { DesignAspectDeleteReducer } from "../Reducer/DesignAspect/DesignAspectDeleteReducer";
import { VodafoneNameCreateReducer } from "../Reducer/LookUp/VodafoneName/VodafoneNameCreateReducer";
import { VodafoneNameEditReducer } from "../Reducer/LookUp/VodafoneName/VodafoneNameEditReducer";
import { VodafoneNameDeleteReducer } from "../Reducer/LookUp/VodafoneName/VodafoneNameDeleteReducer";
import { VodafoneNameGridReducer } from "../Reducer/LookUp/VodafoneName/VodafoneNameGridReducer";
import { BusinessContinuityMethodCreateReducer } from "../Reducer/LookUp/GeoResilience/GeoResilienceCreateReucer";
import { BusinessContinuityMethodEditReducer } from "../Reducer/LookUp/GeoResilience/GeoResilienceEditReducer";
import { BusinessContinuityMethodDeleteReducer } from "../Reducer/LookUp/GeoResilience/GeoResilienceDeleteReducer";
import { BusinessContinuityMethodGridReducer } from "../Reducer/LookUp/GeoResilience/GeoResilienceGridReducer";
import { IdentityAsIsGridReducer } from "../Reducer/IdentityAsIs/IdentityAsIsGridReducer";
import { IdentityAsIsCreateReducer } from "../Reducer/IdentityAsIs/IdentityAsIsCreateReducer";
import { IdentityAsIsEditReducer } from "../Reducer/IdentityAsIs/IdentityAsIsEditReducer";
import { AuditGridReducer } from "../Reducer/Audit/AuditGridReducer";
import {
  GenericReportGridReducer,
  GenericPreviewReportGridReducer,
  GenericAggregatedReportGridReducer,
  GenericDisAggregatedReportGridReducer,
} from "../Reducer/GenericReport/GenericGridReducer";

import { AuditTrailsGridReducer } from "../Reducer/AuditTrails/AuditTrailsGridReducer";
import { FeedbackLogsGridReducer } from "../Reducer/FeedbackLogs/FeedbackLogsGridReducer";
import { UserManagementGridReducer } from "../Reducer/UserManagement/UserManagementGridReducer";
import { UserManagementRoleGridReducer } from "../Reducer/UserManagement/UserManagementRoleGridReducer";
import { UserManagementRoleCreateReducer } from "../Reducer/UserManagement/UserManagementRoleCreateReducer";
import { ReconciliationGridReducer } from "../Reducer/Reconciliation/ReconciliationGridReducer";
import { LcmEngAuditGridReducer } from "../Reducer/LcmEngAudit/LcmEngAuditGridReducer";
import { TypedUseSelectorHook, useDispatch, useSelector } from "react-redux";
import { ProblemCategoryCreateReducer } from "../Reducer/LookUp/ProblemCategory/ProblemCategoryCreateReducer";
import { ProblemCategoryDeleteReducer } from "../Reducer/LookUp/ProblemCategory/ProblemCategoryDeleteReducer";
import { ProblemCategoryEditReducer } from "../Reducer/LookUp/ProblemCategory/ProblemCategoryEditReducer";
import { ProblemCategoryGridReducer } from "../Reducer/LookUp/ProblemCategory/ProblemCategoryGridReducer";
import { RiskClusterGridReducer } from "../Reducer/LookUp/RiskCluster/RiskClusterGridReducer";
import { RiskClusterCreateReducer } from "../Reducer/LookUp/RiskCluster/RiskClusterCreateReducer";
import { RiskClusterDeleteReducer } from "../Reducer/LookUp/RiskCluster/RiskClusterDeletedReducer";
import { RiskClusterEditReducer } from "../Reducer/LookUp/RiskCluster/RiskClusterEditReducer";
import { SubFunctionFilterGridResult } from "../Reducer/SubFuntion/SubFuntionGridReducer";
import { OrganizationInfoCreateReducer } from "../Reducer/OrganizationInfo/OrganizationInfoCreateReducer";
import { OrganizationInfoGridReducer } from "../Reducer/OrganizationInfo/OrganizationInfoGridReducer";
import { OrganizationInfoEditReducer } from "../Reducer/OrganizationInfo/OrganizationInfoEditReducer";
import { OrganizationInfoDeleteReducer } from "../Reducer/OrganizationInfo/OrganizationInfoDeleteReducer";
import { UserDefinedReportsLogsGridReducer } from "../Reducer/UserDefinedReportsLogs/UserDefinedReportsLogsGridReducer";
import { MainOrganisationCreateReducer } from "../Reducer/LookUp/MainOrganisation/MainOrganisationCreateReducer";
import { MainOrganisationDeleteReducer } from "../Reducer/LookUp/MainOrganisation/MainOrganisationDeleteReducer";
import { MainOrganisationEditReducer } from "../Reducer/LookUp/MainOrganisation/MainOrganisationEditReducer";
import { MainOrganisationGridReducer } from "../Reducer/LookUp/MainOrganisation/MainOrganisationGridReducer";
import { PracticeCreateReducer } from "../Reducer/LookUp/Practice/PracticeCreateReducer";
import { PracticeDeleteReducer } from "../Reducer/LookUp/Practice/PracticeDeleteReducer";
import { PracticeEditReducer } from "../Reducer/LookUp/Practice/PracticeEditReducer";
import { PracticeGridReducer } from "../Reducer/LookUp/Practice/PracticeGridReducer";
import { GraphFilterObject } from "../Reducer/FilterObject";
import { ReportNetworkLevel2GridReducer } from "../Reducer/Report/ReportNetworkLevel2GridReducer";
import { ReportHardwareConfigGridReducer } from "../Reducer/Report/ReportHardwareConfigGridReducer";
import { AssetPivotByLocationGridReducer } from "../Reducer/Report/AssetPivotByLocationReducer";
import { ComponentSwBuildCreateReducer } from "../Reducer/ComponentSwBuild/ComponentSwBuildCreateReducer";
import { ComponentSwBuildDeleteReducer } from "../Reducer/ComponentSwBuild/ComponentSwBuildDeleteReducer";
import { ComponentSwBuildEditReducer } from "../Reducer/ComponentSwBuild/ComponentSwBuildEditReducer";
import { ComponentSwBuildGridReducer } from "../Reducer/ComponentSwBuild/ComponentSwBuildGridReducer";
import { BuildBagCreateReducer } from "../Reducer/BuildBag/BuildBagCreateReducer";
import { BuildBagDeleteReducer } from "../Reducer/BuildBag/BuildBagDeleteReducer";
import { BuildBagEditReducer } from "../Reducer/BuildBag/BuildBagEditReducer";
import { BuildBagGridReducer } from "../Reducer/BuildBag/BuildBagGridReducer";
import { NonTemsTSRReportGridReducer } from "../Reducer/TSRReport/NonTemsTSRReportGridReducer";
import { NonTemsFNTReportGridReducer } from "../Reducer/FNTReport/NonTemsFNTReportGridReducer";
import { TemsTSRReportGridReducer } from "../Reducer/TSRReport/TemsTSRReportGridReducer";
import { TemsFNTReportGridReducer } from "../Reducer/FNTReport/TemsFNTReportGridReducer";
import { NFVISwCompatibleGridReducer } from "../Reducer/NFVISoftwareCompatible/NFVISwCompatibleGridReducer";
import { NFVISwCompatibleCreateReducer } from "../Reducer/NFVISoftwareCompatible/NFVISwCompatibleCreateReducer";
import { NFVISwCompatibleEditReducer } from "../Reducer/NFVISoftwareCompatible/NFVISwCompatibleEditReducer";
import { TSRReportVerticalGridReducer } from "../Reducer/TSRReport/TemsTSRReportVerticalGridReducer";
import { TemsTSRReportVerticalCreateReducer } from "../Reducer/TSRReport/TemsTSRReportVerticalCreateReducer";
import { TSRReportVerticalEditReducer } from "../Reducer/TSRReport/TSRReportVerticalEditReducer";
import { GeneralSettingsReducer } from "../Reducer/GeneralSettings/GeneralSettingsReducer";
import { GeneralSettingsEditReducer } from "../Reducer/GeneralSettings/GeneralSettingsEditReducer";
import { VBOMInfoGridReducer } from "../Reducer/VBOMInfo/VBOMInfoGridReducer";
import { VBOMInfoCreateReducer } from "../Reducer/VBOMInfo/VBOMInfoCreateReducer";
import { VBOMInfoEditReducer } from "../Reducer/VBOMInfo/VBOMInfoEditReducer";
import { VBOMClusterInfoCreateReducer } from "../Reducer/VBOMInfo/VBOMClusterInfoCreateReducer";
import { VBOMClusterInfoEditReducer } from "../Reducer/VBOMInfo/VBOMClusterInfoEditReducer";
import { NFVICCompatibleReducer } from "../Reducer/Nfviccompatible/NfvicompatibleReducer";
import { NFVICCompatibilityReportReducer } from "../Reducer/Nfviccompatible/NfvicCompatibilityReportReducer";
import tourReducer, { tourGuideReducer } from "../Reducer/tourReducer";
import tourStepReducer from "../Reducer/TourGuide/tourStepReducer";
import { SystemGridReducer } from "../Reducer/LookUp/Domain/DomainGridReducer";
import { SystemNameCreateReducer } from "../Reducer/LookUp/Domain/DomainCreateReducer";
import { SystemNamesEditReducer } from "../Reducer/LookUp/Domain/DomainEditReducer";
import { SystemNamesDeleteReducer } from "../Reducer/LookUp/Domain/DomainDeleteReducer";
import { CBOMGridReducer } from "../Reducer/CBOM/CBOMGridReducer";
import { CBOMCreateReducer } from "../Reducer/CBOM/CBOMCreateReducer";
import { CBOMEditReducer } from "../Reducer/CBOM/CBOMEditReducer";
import { BPTReportGridReducer } from "../Reducer/BPTReport/BPTReportGridReducer";
import { ClusterInfoGridReducer } from "../Reducer/ClusterInfo/ClusterInfoGridReducer";

// import { GetUSersLoggingLevelsReducer } from "../Reducer/UserLoggingLevels/UserLoggingLevelResucer";

import { ProgramCreateReducer } from "../Reducer/LookUp/Program/ProgramCreateReducer";
import { ProgramEditReducer } from "../Reducer/LookUp/Program/ProgramEditReducer";
import { ProgramDeleteReducer } from "../Reducer/LookUp/Program/ProgramDeleteReducer";
import { ProgramGridReducer } from "../Reducer/LookUp/Program/ProgramGridReducer";
import { VBOMClusterInfoGridReducer } from "../Reducer/VBOMInfo/VBOMClusterInfoGridReducer";
import { CBOMClusterInfoGridReducer } from "../Reducer/CBOM/CBOMClusterInfoGridReducer";
import { CBOMClusterInfoCreateReducer } from "../Reducer/CBOM/CBOMClusterInfoCreateReducer";
import { CBOMClusterInfoEditReducer } from "../Reducer/CBOM/CBOMClusterInfoEditReducer";
import { CBOMClusterInstanceCapacityGridReducer } from "../Reducer/CBOM/CBOMClusterInstanceCapacityGridReducer";
import { CBOMClusterCapacityGridReducer } from "../Reducer/CBOM/CBOMClusterCapacityGridReducer";
import { VBOMClusterInstanceCapacityGridReducer } from "../Action/VBOMInfo/VBOMClusterInstanceCapacityGridReducer";
import { VBOMClusterCapacityGridReducer } from "../Action/VBOMInfo/VBOMClusterCapacityGridReducer";
import {
  AssetsDetailsReducer,
  DAAssetsMigrationGridReducer,
} from "../Reducer/AssetsPlatform/AssetsDetailsReducer";
import { VBOMReportGridReducer } from "../Reducer/VBOMReport/VBOMReportGridReducer";
import { CBOMReportGridReducer } from "../Reducer/CBOMReport/CBOMReportGridReducer";
import { ExodusReportGridReducer } from "../Reducer/Report/ExodusReportGridReducer";
import { InfraClusterEditReducer } from "../Reducer/LcmEngineering/InfraClusterEditReducer";
import { InfraClusterCreateReducer } from "../Reducer/LcmEngineering/InfraClusterCreateReducer";
import { ServicePlanEditReducer } from "../Reducer/ServicePlan/ServicePlanEditReducer";
import { InfraClusterUpgradeReducer } from "../Reducer/LcmEngineering/InfraClusterUpgradeReducer";
import { InfraProgramClusterResource } from "../Reducer/LcmEngineering/InfraClusterProgramCreateReducer";
import { AssetLevelReportForExodusReducer } from "../Reducer/Report/AssetLevelReportForExodusReducer";
import { TeamManagementGridReducer } from "../Reducer/TeamManagement/TeamManagementGridReducer";
import { TeamManagementCreateReducer } from "../Reducer/TeamManagement/TeamManagementCreateReducer";
import { TeamManagementEditReducer } from "../Reducer/TeamManagement/TeamManagementEditReducer";
import { TeamManagementDeleteReducer } from "../Reducer/TeamManagement/TeamManagementDeleteReducer";

//#endregion

// const logger = (store) => (next) => (action) => {
//   let result = next(action);
//   return result;
// };

// const composeEnhancers =
//   (window as any).__REDUX_DEVTOOLS_EXTENSION_COMPOSE__ || compose;

export const rootReducer = combineReducers({
  autenticazione: userReducer,
  loaderReducer: LoaderReducer,
  errorReducer: errorReducer,
  graphFilterObject: GraphFilterObject,
  systemTypeCreateReducer: SystemTypeCreateReducer,
  systemTypeGridReducer: SystemTypeGridReducer,
  systemTypeEditReducer: SystemTypeEditReducer,
  systemTypeDeleteReducer: SystemTypeDeleteReducer,

  testInfoCreateReducer: TestInfoCreateReducer,
  testInfoGridReducer: TestInfoGridReducer,
  testInfoEditReducer: TestInfoEditReducer,
  testInfoDeleteReducer: TestInfoDeleteReducer,

  tour: tourReducer,
  tourGuide: tourGuideReducer,
  tourStep: tourStepReducer,

  NFVISwCompatibleGridReducer: NFVISwCompatibleGridReducer,
  NFVISwCompatibleCreateReducer: NFVISwCompatibleCreateReducer,
  NFVISwCompatibleEditReducer: NFVISwCompatibleEditReducer,

  temsTsrReportGridReducer: TemsTSRReportGridReducer,
  nonTemsTsrReportGridReducer: NonTemsTSRReportGridReducer,
  tSRReportVerticalGridReducer: TSRReportVerticalGridReducer,
  temsTSRReportVerticalCreateReducer: TemsTSRReportVerticalCreateReducer,
  tSRReportVerticalEditReducer: TSRReportVerticalEditReducer,

  temsFntReportGridReducer: TemsFNTReportGridReducer,
  nonTemsFntReportGridReducer: NonTemsFNTReportGridReducer,

  organizationInfoCreateReducer: OrganizationInfoCreateReducer,
  organizationInfoGridReducer: OrganizationInfoGridReducer,
  organizationInfoEditReducer: OrganizationInfoEditReducer,
  organizationInfoDeleteReducer: OrganizationInfoDeleteReducer,

  VBOMInfoGridReducer: VBOMInfoGridReducer,
  VBOMClusterInfoGridReducer: VBOMClusterInfoGridReducer,
  VBOMInfoCreateReducer: VBOMInfoCreateReducer,
  VBOMInfoEditReducer: VBOMInfoEditReducer,
  VBOMClusterInfoCreateReducer: VBOMClusterInfoCreateReducer,
  VBOMClusterInfoEditReducer: VBOMClusterInfoEditReducer,
  VBOMClusterInstanceCapacityGridReducer:
    VBOMClusterInstanceCapacityGridReducer,
  VBOMClusterCapacityGridReducer: VBOMClusterCapacityGridReducer,

  VBOMReportGridReducer: VBOMReportGridReducer,
  CBOMReportGridReducer: CBOMReportGridReducer,

  CBOMGridReducer: CBOMGridReducer,
  CBOMCreateReducer: CBOMCreateReducer,
  CBOMEditReducer: CBOMEditReducer,
  CBOMClusterInfoGridReducer: CBOMClusterInfoGridReducer,
  CBOMClusterInstanceCapacityGridReducer:
    CBOMClusterInstanceCapacityGridReducer,
  CBOMClusterCapacityGridReducer: CBOMClusterCapacityGridReducer,
  CBOMClusterInfoCreateReducer: CBOMClusterInfoCreateReducer,
  CBOMClusterInfoEditReducer: CBOMClusterInfoEditReducer,

  ClassGridReducer: ClassGridReducer,
  ClassEditReducer: ClassEditReducer,
  ClassDeleteReducer: ClassDeleteReducer,
  ClassCreateReducer: ClassCreateReducer,

  TypeGridReducer: TypeGridReducer,
  TypeEditReducer: TypeEditReducer,
  TypeDeleteReducer: TypeDeleteReducer,
  TypeCreateReducer: TypeCreateReducer,

  majorHardwareBuildCreateReducer: MajorHardwareBuildCreateReducer,
  majorHardwareBuildGridReducer: MajorHardwareBuildGridReducer,
  majorHardwareBuildEditReducer: MajorHardwareBuildEditReducer,
  majorHardwareBuildDeleteReducer: MajorHardwareBuildDeleteReducer,
  IdentityAsIsGridReducer: IdentityAsIsGridReducer,
  IdentityAsIsCreateReducer: IdentityAsIsCreateReducer,
  IdentityAsIsEditReducer: IdentityAsIsEditReducer,

  deliveryTrackingCreateReducer: DeliveryTrackingCreateReducer,
  deliveryTrackingGridReducer: DeliveryTrackingGridReducer,
  deliveryTrackingEditReducer: DeliveryTrackingEditReducer,
  deliveryTrackingDeleteReducer: DeliveryTrackingDeleteReducer,

  majorSoftwareBuildCreateReducer: MajorSoftwareBuildCreateReducer,
  majorSoftwareBuildGridReducer: MajorSoftwareBuildGridReducer,
  majorSoftwareBuildEditReducer: MajorSoftwareBuildEditReducer,
  majorSoftwareBuildDeleteReducer: MajorSoftwareBuildDeleteReducer,

  componentSwBuildCreateReducer: ComponentSwBuildCreateReducer,
  componentSwBuildGridReducer: ComponentSwBuildGridReducer,
  componentSwBuildEditReducer: ComponentSwBuildEditReducer,
  componentSwBuildDeleteReducer: ComponentSwBuildDeleteReducer,

  buildBagCreateReducer: BuildBagCreateReducer,
  buildBagGridReducer: BuildBagGridReducer,
  buildBagEditReducer: BuildBagEditReducer,
  buildBagDeleteReducer: BuildBagDeleteReducer,
  exodusReportGridReducer: ExodusReportGridReducer,

  designComponentCreateReducer: DesignComponentCreateReducer,
  designComponentGridReducer: DesignComponentGridReducer,
  designComponentEditReducer: DesignComponentEditReducer,
  designComponentDeleteReducer: DesignComponentDeleteReducer,

  foreignIndexReducer: ForeignIndexReducer,

  designComponentFamilyCreateReducer: DesignComponentFamilyCreateReducer,
  designComponentFamilyGridReducer: DesignComponentFamilyGridReducer,
  designComponentFamilyEditReducer: DesignComponentFamilyEditReducer,
  designComponentFamilyDeleteReducer: DesignComponentFamilyDeleteReducer,

  SettingsUpdatePlannedActivityCreateReducer:
    SettingsUpdatePlannedActivityCreateReducer,
  SettingsUpdatePlannedActivityGridReducer:
    SettingsUpdatePlannedActivityGridReducer,
  SettingsUpdatePlannedActivityEditReducer:
    SettingsUpdatePlannedActivityEditReducer,
  SettingsUpdatePlannedActivityDeleteReducer:
    SettingsUpdatePlannedActivityDeleteReducer,

  PlannedActivityTypesCreateReducer: PlannedActivityTypesCreateReducer,
  PlannedActivityTypesGridReducer: PlannedActivityTypesGridReducer,
  PlannedActivityTypesEditReducer: PlannedActivityTypesEditReducer,
  PlannedActivityTypesDeleteReducer: PlannedActivityTypesDeleteReducer,

  thirdPartyHardwareComponentCreateReducer:
    ThirdPartyHardwareComponentCreateReducer,
  thirdPartyHardwareComponentGridReducer:
    ThirdPartyHardwareComponentGridReducer,
  thirdPartyHardwareComponentEditReducer:
    ThirdPartyHardwareComponentEditReducer,
  thirdPartyHardwareComponentDeleteReducer:
    ThirdPartyHardwareComponentDeleteReducer,

  lcmEngineeringCreateReducer: LcmEngineeringCreateReducer,
  lcmEngineeringGridReducer: LcmEngineeringGridReducer,
  lcmEngineeringEditReducer: LcmEngineeringEditReducer,
  lcmEngineeringDeleteReducer: LcmEngineeringDeleteReducer,

  lcmEngAuditGridReducer: LcmEngAuditGridReducer,
  infraClusterCreateReducer: InfraClusterCreateReducer,
  infraClusterEditReducer: InfraClusterEditReducer,
  infraClusterUpgradeReducer: InfraClusterUpgradeReducer,
  infraProgramClusterResource: InfraProgramClusterResource,

  designAspectCreateReducer: DesignAspectCreateReducer,
  designAspectGridReducer: DesignAspectGridReducer,
  designAspectEditReducer: DesignAspectEditReducer,
  designAspectDeleteReducer: DesignAspectDeleteReducer,

  reportHardwareGridReducer: ReportHardwareGridReducer,
  reportSoftwareGridReducer: ReportSoftwareGridReducer,

  passThroughReportGridReducer: PassThroughReportGridReducer,
  passThroughHardwareReportGridReducer: PassThroughHardwareReportGridReducer,
  passThroughSoftwareReportGridReducer: PassThroughSoftwareReportGridReducer,

  assetAsisSdGridReducer: AssetAsisSdGridReducer,
  assetAsisSdSwitchGridReducer: AssetAsisSdSwitchGridReducer,
  assetAsisHwAncillaryGridReducer: AssetAsisHwAncillaryGridReducer,

  reportNetworkLevel2GridReducer: ReportNetworkLevel2GridReducer,
  reportHardwareConfigGridReducer: ReportHardwareConfigGridReducer,
  reportSubBoundHardwareGridReducer: ReportSubBoundHardwareGridReducer,
  reportSubBoundSoftwareGridReducer: ReportSubBoundSoftwareGridReducer,

  viaExportHardwareGridReducer: ViaExportHardwareGridReducer,
  viaExportSoftwareGridReducer: ViaExportSoftwareGridReducer,

  exportDownloadReducer: ExportDownloadReducer,

  assetAsisSdDownloadReducer: AssetAsisSdDownloadReducer,
  assetAsisSdSwitchDownloadReducer: AssetAsisSdSwitchDownloadReducer,
  assetAsisHwAncillaryDownloadReducer: AssetAsisHwAncillaryDownloadReducer,

  passThroughReportDownloadReducer: PassThroughReportDownloadReducer,
  passThroughHardwareReportDownloadReducer:
    PassThroughHardwareReportDownloadReducer,
  passThroughSoftwareReportDownloadReducer:
    PassThroughSoftwareReportDownloadReducer,

  modalReducer: ModalReducer,

  plannedActivityGridReducer: PlannedActivityGridReducer,
  plannedActivityCreateReducer: PlannedActivityCreateReducer,
  plannedActivityEditReducer: PlannedActivityEditReducer,
  plannedActivityDeleteReducer: PlannedActivityDeleteReducer,

  servicePlanCreateReducer: ServicePlanCreateReducer,
  servicePlanEditReducer: ServicePlanEditReducer,
  servicePlanGridReducer: ServicePlanGridReducer,
  servicePlanDeleteReducer: ServicePlanDeleteReducer,

  activityStatusCreateReducer: ActivityStatusCreateReducer,
  activityStatusEditReducer: ActivityStatusEditReducer,
  activityStatusDeleteReducer: ActivityStatusDeleteReducer,
  activityStatusGridReducer: ActivityStatusGridReducer,

  networkElementAsPlannedCreateReducer: NetworkElementAsPlannedCreateReducer,
  networkElementAsPlannedEditReducer: NetworkElementAsPlannedEditReducer,
  networkElementAsPlannedDeleteReducer: NetworkElementAsPlannedDeleteReducer,
  networkElementAsPlannedGridReducer: NetworkElementAsPlannedGridReducer,
  assetHardwareAncillariesReducer: AssetHardwareAncillariesReducer,

  networkElementAsIsCreateReducer: NetworkElementAsIsCreateReducer,
  networkElementAsIsEditReducer: NetworkElementAsIsEditReducer,
  networkElementAsIsDeleteReducer: NetworkElementAsIsDeleteReducer,
  networkElementAsIsGridReducer: NetworkElementAsIsGridReducer,
  newnetworkElementAsIsGridReducer: NewNetworkElementAsIsGridReducer,
  identityGridReducer: IdentityGridReducer,
  hardwareConfigurationGridReducer: HardwareConfigurationGridReducer,
  softwareConfigurationGridReducer: SoftwareConfigurationGridReducer,
  subFunctionFilterGridReducer: SubFunctionFilterGridResult,
  softwareConfigurationOpcoReducer: SoftwareConfigurationOpcoReducer,
  softwareConfigurationOemReducer: SoftwareConfigurationOemReducer,
  softwareConfigurationEleReducer: SoftwareConfigurationEleReducer,
  softwareComponentGridReducer: SoftwareComponentGridReducer,
  resourceKeyMasterGridReducer: ResourceKeyMasterGridReducer,
  dcfLifeCycleGridReducer: DCFLifeCycleGridReducer,
  auditGridReducer: AuditGridReducer,
  userManagementGridReducer: UserManagementGridReducer,
  userManagementRoleGridReducer: UserManagementRoleGridReducer,
  userManagementRoleCreateReducer: UserManagementRoleCreateReducer,

  teamManagementGridReducer: TeamManagementGridReducer,
  teamManagementCreateReducer: TeamManagementCreateReducer,
  teamManagementEditReducer: TeamManagementEditReducer,
  teamManagementDeletetReducer: TeamManagementDeleteReducer,

  genericReportGridReducer: GenericReportGridReducer,
  AuditTrailsGridReducer: AuditTrailsGridReducer,
  UserDefinedReportsLogsGridReducer: UserDefinedReportsLogsGridReducer,
  FeedbackLogsGridReducer: FeedbackLogsGridReducer,
  genericAggregatedReportGridReducer: GenericAggregatedReportGridReducer,
  genericDisAggregatedReportGridReducer: GenericDisAggregatedReportGridReducer,
  genericPreviewReportGridReducer: GenericPreviewReportGridReducer,

  assetCategoryCreateReducer: AssetCategoryCreateReducer,
  assetCategoryEditReducer: AssetCategoryEditReducer,
  assetCategoryDeleteReducer: AssetCategoryDeleteReducer,
  assetCategoryGridReducer: AssetCategoryGridReducer,

  reconciliationGridReducer: ReconciliationGridReducer,

  assetTypeCreateReducer: AssetTypeCreateReducer,
  assetTypeEditReducer: AssetTypeEditReducer,
  assetTypeDeleteReducer: AssetTypeDeleteReducer,
  assetTypeGridReducer: AssetTypeGridReducer,

  criticalAssetTypeCreateReducer: CriticalAssetTypeCreateReducer,
  criticalAssetTypeEditReducer: CriticalAssetTypeEditReducer,
  criticalAssetTypeDeleteReducer: CriticalAssetTypeDeleteReducer,
  criticalAssetTypeGridReducer: CriticalAssetTypeGridReducer,

  supportedServiceCreateReducer: SupportedServiceCreateReducer,
  supportedServiceEditReducer: SupportedServiceEditReducer,
  supportedServiceDeleteReducer: SupportedServiceDeleteReducer,
  supportedServiceGridReducer: SupportedServiceGridReducer,

  deliveryStatusCreateReducer: DeliveryStatusCreateReducer,
  deliveryStatusEditReducer: DeliveryStatusEditReducer,
  deliveryStatusDeleteReducer: DeliveryStatusDeleteReducer,
  deliveryStatusGridReducer: DeliveryStatusGridReducer,

  lCMHardwareSupportTypeCreateReducer: LCMHardwareSupportTypeCreateReducer,
  lCMHardwareSupportTypeEditReducer: LCMHardwareSupportTypeEditReducer,
  lCMHardwareSupportTypeDeleteReducer: LCMHardwareSupportTypeDeleteReducer,
  lCMHardwareSupportTypeGridReducer: LCMHardwareSupportTypeGridReducer,

  opCoCreateReducer: OpCoCreateReducer,
  opCoEditReducer: OpCoEditReducer,
  opCoDeleteReducer: OpCoDeleteReducer,
  opCoGridReducer: OpCoGridReducer,

  problemCategoryCreateReducer: ProblemCategoryCreateReducer,
  problemCategoryEditReducer: ProblemCategoryEditReducer,
  problemCategoryDeleteReducer: ProblemCategoryDeleteReducer,
  problemCategoryGridReducer: ProblemCategoryGridReducer,

  mainOrganisationCreateReducer: MainOrganisationCreateReducer,
  mainOrganisationEditReducer: MainOrganisationEditReducer,
  mainOrganisationDeleteReducer: MainOrganisationDeleteReducer,
  mainOrganisationGridReducer: MainOrganisationGridReducer,

  practiceCreateReducer: PracticeCreateReducer,
  practiceEditReducer: PracticeEditReducer,
  practiceDeleteReducer: PracticeDeleteReducer,
  practiceGridReducer: PracticeGridReducer,

  riskClusterGridReducer: RiskClusterGridReducer,
  riskClusterCreateReducer: RiskClusterCreateReducer,
  riskClusterDeleteReducer: RiskClusterDeleteReducer,
  riskClusterEditReducer: RiskClusterEditReducer,

  sharingTypeCreateReducer: SharingTypeCreateReducer,
  sharingTypeEditReducer: SharingTypeEditReducer,
  sharingTypeDeleteReducer: SharingTypeDeleteReducer,
  sharingTypeGridReducer: SharingTypeGridReducer,

  securityTireZoneCreateReducer: SecurityTireZoneCreateReducer,
  securityTireZoneEditReducer: SecurityTireZoneEditReducer,
  securityTireZoneDeleteReducer: SecurityTireZoneDeleteReducer,
  securityTireZoneGridReducer: SecurityTireZoneGridReducer,

  environmentCreateReducer: EnvironmentCreateReducer,
  environmentEditReducer: EnvironmentEditReducer,
  environmentDeleteReducer: EnvironmentDeleteReducer,
  environmentGridReducer: EnvironmentGridReducer,

  deploymentStatusCreateReducer: DeploymentStatusCreateReducer,
  deploymentStatusEditReducer: DeploymentStatusEditReducer,
  deploymentStatusDeleteReducer: DeploymentStatusDeleteReducer,
  deploymentStatusGridReducer: DeploymentStatusGridReducer,
  systemGridReducer: SystemGridReducer,
  systemNameCreateReducer: SystemNameCreateReducer,
  systemNamesEditReducer: SystemNamesEditReducer,
  systemNamesDeleteReducer: SystemNamesDeleteReducer,

  deploymentTypeCreateReducer: DeploymentTypeCreateReducer,
  deploymentTypeEditReducer: DeploymentTypeEditReducer,
  deploymentTypeDeleteReducer: DeploymentTypeDeleteReducer,
  deploymentTypeGridReducer: DeploymentTypeGridReducer,

  networkConstructCreateReducer: NetworkConstructCreateReducer,
  networkConstructEditReducer: NetworkConstructEditReducer,
  networkConstructDeleteReducer: NetworkConstructDeleteReducer,
  networkConstructGridReducer: NetworkConstructGridReducer,

  driverCreateReducer: DriverCreateReducer,
  driverEditReducer: DriverEditReducer,
  driverDeleteReducer: DriverDeleteReducer,
  driverGridReducer: DriverGridReducer,

  planningRiskCreateReducer: PlanningRiskCreateReducer,
  planningRiskEditReducer: PlanningRiskEditReducer,
  planningRiskDeleteReducer: PlanningRiskDeleteReducer,
  planningRiskGridReducer: PlanningRiskGridReducer,

  benefitsCreateReducer: BenefitsCreateReducer,
  benefitsEditReducer: BenefitsEditReducer,
  benefitsDeleteReducer: BenefitsDeleteReducer,
  benefitsGridReducer: BenefitsGridReducer,

  activityDetailsCreateReducer: ActivityDetailsCreateReducer,
  activityDetailsEditReducer: ActivityDetailsEditReducer,
  activityDetailsDeleteReducer: ActivityDetailsDeleteReducer,
  activityDetailsGridReducer: ActivityDetailsGridReducer,

  serviceBoundaryCreateReducer: ServiceBoundaryCreateReducer,
  serviceBoundaryEditReducer: ServiceBoundaryEditReducer,
  serviceBoundaryDeleteReducer: ServiceBoundaryDeleteReducer,
  serviceBoundaryGridReducer: ServiceBoundaryGridReducer,

  subNetworkBoundaryCreateReducer: SubNetworkBoundaryCreateReducer,
  subNetworkBoundaryEditReducer: SubNetworkBoundaryEditReducer,
  subNetworkBoundaryDeleteReducer: SubNetworkBoundaryDeleteReducer,
  subNetworkBoundaryGridReducer: SubNetworkBoundaryGridReducer,

  plannedActivityNetworkElementCreateReducer:
    PlannedActivityNetworkElementCreateReducer,
  plannedActivityNetworkElementEditReducer:
    PlannedActivityNetworkElementEditReducer,
  plannedActivityNetworkElementDeleteReducer:
    PlannedActivityNetworkElementDeleteReducer,
  plannedActivityNetworkElementGridReducer:
    PlannedActivityNetworkElementGridReducer,

  locationCreateReducer: LocationCreateReducer,
  locationEditReducer: LocationEditReducer,
  locationDeleteReducer: LocationDeleteReducer,
  locationGridReducer: LocationGridReducer,

  interVMTypeGridReducer: InterVMTypeGridReducer,
  interVMTypeEditReducer: InterVMTypeEditReducer,
  interVMTypeDeleteReducer: InterVMTypeDeleteReducer,
  interVMTypeCreateReducer: InterVMTypeCreateReducer,

  vNFHardwareTypeGridReducer: VNFHardwareTypeGridReducer,
  vNFHardwareTypeEditReducer: VNFHardwareTypeEditReducer,
  vNFHardwareTypeDeleteReducer: VNFHardwareTypeDeleteReducer,
  vNFHardwareTypeCreateReducer: VNFHardwareTypeCreateReducer,

  intraVMTypeGridReducer: IntraVMTypeGridReducer,
  intraVMTypeEditReducer: IntraVMTypeEditReducer,
  intraVMTypeDeleteReducer: IntraVMTypeDeleteReducer,
  intraVMTypeCreateReducer: IntraVMTypeCreateReducer,

  serviceMasterGridReducer: ServiceMasterGridReducer,
  serviceMasterEditReducer: ServiceMasterEditReducer,
  serviceMasterCreateReducer: ServiceMasterCreateReducer,
  serviceMasterDeleteReducer: ServiceMasterDeleteReducer,

  assetMapInfoGridReducer: AssetMapInfoGridReducer,
  assetMapInfoCreateReducer: AssetMapInfoCreateReducer,
  assetMapInfoEditReducer: AssetMapInfoEditReducer,
  assetMapInfoDeleteReducer: AssetMapInfoDeleteReducer,

  majorHardwareMTGridReducer: MajorHardwareMTGridReducer,
  majorHardwareMTCreateReducer: MajorHardwareMTCreateReducer,
  majorHardwareMTEditReducer: MajorHardwareMTEditReducer,
  majorHardwareMTDeleteReducer: MajorHardwareMTDeleteReducer,

  vMWorkloadTypeGridReducer: VMWorkloadTypeGridReducer,
  vMWorkloadTypeEditReducer: VMWorkloadTypeEditReducer,
  vMWorkloadTypeDeleteReducer: VMWorkloadTypeDeleteReducer,
  vMWorkloadTypeCreateReducer: VMWorkloadTypeCreateReducer,

  vMTypeNameGridReducer: VMTypeNameGridReducer,
  vMTypeNameEditReducer: VMTypeNameEditReducer,
  vMTypeNameDeleteReducer: VMTypeNameDeleteReducer,
  vMTypeNameCreateReducer: VMTypeNameCreateReducer,

  vNFClusterNameGridReducer: VNFClusterNameGridReducer,
  vNFClusterNameEditReducer: VNFClusterNameEditReducer,
  vNFClusterNameDeleteReducer: VNFClusterNameDeleteReducer,
  vNFClusterNameCreateReducer: VNFClusterNameCreateReducer,

  vNFNameGridReducer: VNFNameGridReducer,
  vNFNameEditReducer: VNFNameEditReducer,
  vNFNameDeleteReducer: VNFNameDeleteReducer,
  vNFNameCreateReducer: VNFNameCreateReducer,

  cNFNameGridReducer: CNFNameGridReducer,
  cNFNameEditReducer: CNFNameEditReducer,
  cNFNameDeleteReducer: CNFNameDeleteReducer,
  cNFNameCreateReducer: CNFNameCreateReducer,

  cNFClusterGridReducer: CNFClusterGridReducer,
  cNFClusterEditReducer: CNFClusterEditReducer,
  cNFClusterDeleteReducer: CNFClusterDeleteReducer,
  cNFClusterCreateReducer: CNFClusterCreateReducer,

  cNFPriorityGridReducer: CNFPriorityGridReducer,
  cNFPriorityEditReducer: CNFPriorityEditReducer,
  cNFPriorityDeleteReducer: CNFPriorityDeleteReducer,
  cNFPriorityCreateReducer: CNFPriorityCreateReducer,

  cNFHardwareTypeDeleteReducer: CNFHardwareTypeDeleteReducer,
  cNFHardwareTypeGridReducer: CNFHardwareTypeGridReducer,
  cNFHardwareTypeEditReducer: CNFHardwareTypeEditReducer,
  cNFHardwareTypeCreateReducer: CNFHardwareTypeCreateReducer,

  cNFFunctionStandardNameDeleteReducer: CNFFunctionStandardNameDeleteReducer,
  cNFFunctionStandardNameGridReducer: CNFFunctionStandardNameGridReducer,
  cNFFunctionStandardNameEditReducer: CNFFunctionStandardNameEditReducer,
  cNFFunctionStandardNameCreateReducer: CNFFunctionStandardNameCreateReducer,

  podTypeInfoDeleteReducer: PodTypeInfoDeleteReducer,
  podTypeInfoGridReducer: PodTypeInfoGridReducer,
  podTypeInfoEditReducer: PodTypeInfoEditReducer,
  podTypeInfoCreateReducer: PodTypeInfoCreateReducer,

  fullOrPartialResourceCreateReducer: FullOrPartialResourceCreateReducer,
  fullOrPartialResourceEditReducer: FullOrPartialResourceEditReducer,
  fullOrPartialResourceDeleteReducer: FullOrPartialResourceDeleteReducer,
  fullOrPartialResourceGridReducer: FullOrPartialResourceGridReducer,

  supportedResourceCreateReducer: SupportedResourceCreateReducer,
  supportedResourceEditReducer: SupportedResourceEditReducer,
  supportedResourceDeleteReducer: SupportedResourceDeleteReducer,
  supportedResourceGridReducer: SupportedResourceGridReducer,

  reasonCheckboxCreateReducer: ReasonCheckboxCreateReducer,
  reasonCheckboxEditReducer: ReasonCheckboxEditReducer,
  reasonCheckboxDeleteReducer: ReasonCheckboxDeleteReducer,
  reasonCheckboxGridReducer: ReasonCheckboxGridReducer,

  operationalRiskCreateReducer: OperationalRiskCreateReducer,
  operationalRiskEditReducer: OperationalRiskEditReducer,
  operationalRiskDeleteReducer: OperationalRiskDeleteReducer,
  operationalRiskGridReducer: OperationalRiskGridReducer,

  budgetAvailabilityCreateReducer: BudgetAvailabilityCreateReducer,
  budgetAvailabilityEditReducer: BudgetAvailabilityEditReducer,
  budgetAvailabilityDeleteReducer: BudgetAvailabilityDeleteReducer,
  budgetAvailabilityGridReducer: BudgetAvailabilityGridReducer,

  supportProviderCreateReducer: SupportProviderCreateReducer,
  supportProviderEditReducer: SupportProviderEditReducer,
  supportProviderDeleteReducer: SupportProviderDeleteReducer,
  supportProviderGridReducer: SupportProviderGridReducer,

  endOfSupportContractCreateReducer: EndOfSupportContractCreateReducer,
  endOfSupportContractEditReducer: EndOfSupportContractEditReducer,
  endOfSupportContractDeleteReducer: EndOfSupportContractDeleteReducer,
  endOfSupportContractGridReducer: EndOfSupportContractGridReducer,

  buildConstructionCreateReducer: BuildConstructionCreateReducer,
  buildConstructionEditReducer: BuildConstructionEditReducer,
  buildConstructionDeleteReducer: BuildConstructionDeleteReducer,
  buildConstructionGridReducer: BuildConstructionGridReducer,

  nFVIBundleIDCreateReducer: NFVIBundleIDCreateReducer,
  nFVIBundleIDEditReducer: NFVIBundleIDEditReducer,
  nFVIBundleIDDeleteReducer: NFVIBundleIDDeleteReducer,
  nFVIBundleIDGridReducer: NFVIBundleIDGridReducer,

  equipmentStatusCreateReducer: EquipmentStatusCreateReducer,
  equipmentStatusEditReducer: EquipmentStatusEditReducer,
  equipmentStatusDeleteReducer: EquipmentStatusDeleteReducer,
  equipmentStatusGridReducer: EquipmentStatusGridReducer,

  vNFDesignComponentCreateReducer: VNFDesignComponentCreateReducer,
  vNFDesignComponentEditReducer: VNFDesignComponentEditReducer,
  vNFDesignComponentDeleteReducer: VNFDesignComponentDeleteReducer,
  vNFDesignComponentGridReducer: VNFDesignComponentGridReducer,

  hardwareSolutionResourceCreateReducer: HardwareSolutionResourceCreateReducer,
  hardwareSolutionResourceEditReducer: HardwareSolutionResourceEditReducer,
  hardwareSolutionResourceDeleteReducer: HardwareSolutionResourceDeleteReducer,
  hardwareSolutionResourceGridReducer: HardwareSolutionResourceGridReducer,

  operatingSystemCreateReducer: OperatingSystemCreateReducer,
  operatingSystemEditReducer: OperatingSystemEditReducer,
  operatingSystemDeleteReducer: OperatingSystemDeleteReducer,
  operatingSystemGridReducer: OperatingSystemGridReducer,

  assetClassCreateReducer: AssetClassCreateReducer,
  assetClassEditReducer: AssetClassEditReducer,
  assetClassDeleteReducer: AssetClassDeleteReducer,
  assetClassGridReducer: AssetClassGridReducer,

  networkFunctionCreateReducer: NetworkFunctionCreateReducer,
  networkFunctionGridReducer: NetworkFunctionGridReducer,
  networkFunctionDeleteReducer: NetworkFunctionDeleteReducer,
  networkFunctionEditReducer: NetworkFunctionEditReducer,

  customerWheelCreateReducer: CustomerWheelCreateReducer,
  customerWheelGridReducer: CustomerWheelGridReducer,
  customerWheelDeleteReducer: CustomerWheelDeleteReducer,
  customerWheelEditReducer: CustomerWheelEditReducer,

  lCMPlannedActionResourceCreateReducer: LCMPlannedActionResourceCreateReducer,
  lCMPlannedActionResourceEditReducer: LCMPlannedActionResourceEditReducer,
  lCMPlannedActionResourceDeleteReducer: LCMPlannedActionResourceDeleteReducer,
  lCMPlannedActionResourceGridReducer: LCMPlannedActionResourceGridReducer,

  lCMSoftwareSupportTypeCreateReducer: LCMSoftwareSupportTypeCreateReducer,
  lCMSoftwareSupportTypeEditReducer: LCMSoftwareSupportTypeEditReducer,
  lCMSoftwareSupportTypeDeleteReducer: LCMSoftwareSupportTypeDeleteReducer,
  lCMSoftwareSupportTypeGridReducer: LCMSoftwareSupportTypeGridReducer,

  vulnerabilityStatusCreateReducer: VulnerabilityStatusCreateReducer,
  vulnerabilityStatusEditReducer: VulnerabilityStatusEditReducer,
  vulnerabilityStatusDeleteReducer: VulnerabilityStatusDeleteReducer,
  vulnerabilityStatusGridReducer: VulnerabilityStatusGridReducer,

  vodafoneNameCreateReducer: VodafoneNameCreateReducer,
  vodafoneNameEditReducer: VodafoneNameEditReducer,
  vodafoneNameDeleteReducer: VodafoneNameDeleteReducer,
  vodafoneNameGridReducer: VodafoneNameGridReducer,

  verticalResponsibleCreateReducer: VerticalResponsibleCreateReducer,
  verticalResponsibleEditReducer: VerticalResponsibleEditReducer,
  verticalResponsibleDeleteReducer: VerticalResponsibleDeleteReducer,
  verticalResponsibleGridReducer: VerticalResponsibleGridReducer,

  subDomainResponsibleCreateReducer: SubDomainResponsibleCreateReducer,
  subDomainResponsibleEditReducer: SubDomainResponsibleEditReducer,
  subDomainResponsibleDeleteReducer: SubDomainResponsibleDeleteReducer,
  subDomainResponsibleGridReducer: SubDomainResponsibleGridReducer,

  responsibilityPhaseCreateReducer: ResponsibilityPhaseCreateReducer,
  responsibilityPhaseEditReducer: ResponsibilityPhaseEditReducer,
  responsibilityPhaseDeleteReducer: ResponsibilityPhaseDeleteReducer,
  responsibilityPhaseGridReducer: ResponsibilityPhaseGridReducer,

  relatesToResourceCreateReducer: RelatesToResourceCreateReducer,
  relatesToResourceEditReducer: RelatesToResourceEditReducer,
  relatesToResourceDeleteReducer: RelatesToResourceDeleteReducer,
  relatesToResourceGridReducer: RelatesToResourceGridReducer,

  productImportanceCreateReducer: ProductImportanceCreateReducer,
  productImportanceEditReducer: ProductImportanceEditReducer,
  productImportanceDeleteReducer: ProductImportanceDeleteReducer,
  productImportanceGridReducer: ProductImportanceGridReducer,

  businessContinuityMethodCreateReducer: BusinessContinuityMethodCreateReducer,
  businessContinuityMethodEditReducer: BusinessContinuityMethodEditReducer,
  businessContinuityMethodDeleteReducer: BusinessContinuityMethodDeleteReducer,
  businessContinuityMethodGridReducer: BusinessContinuityMethodGridReducer,

  sharedLookUpCreateReducer: SharedLookUpCreateReducer,
  sharedLookUpEditReducer: SharedLookUpEditReducer,
  sharedLookUpDeleteReducer: SharedLookUpDeleteReducer,
  sharedLookUpGridReducer: SharedLookUpGridReducer,

  planningActivityStatusCreateReducer: PlanningActivityStatusCreateReducer,
  planningActivityStatusEditReducer: PlanningActivityStatusEditReducer,
  planningActivityStatusDeleteReducer: PlanningActivityStatusDeleteReducer,
  planningActivityStatusGridReducer: PlanningActivityStatusGridReducer,

  plannedActivityResourceCreateReducer: PlannedActivityResourceCreateReducer,
  plannedActivityResourceEditReducer: PlannedActivityResourceEditReducer,
  plannedActivityResourceDeleteReducer: PlannedActivityResourceDeleteReducer,
  plannedActivityResourceGridReducer: PlannedActivityResourceGridReducer,

  platformCreateReducer: PlatformCreateReducer,
  platformEditReducer: PlatformEditReducer,
  platformDeleteReducer: PlatformDeleteReducer,
  platformGridReducer: PlatformGridReducer,

  systemFunctionCreateReducer: SystemFunctionCreateReducer,
  systemFunctionEditReducer: SystemFunctionEditReducer,
  systemFunctionDeleteReducer: SystemFunctionDeleteReducer,
  systemFunctionGridReducer: SystemFunctionGridReducer,

  hardwareTypeCreateReducer: HardwareTypeCreateReducer,
  hardwareTypeEditReducer: HardwareTypeEditReducer,
  hardwareTypeDeleteReducer: HardwareTypeDeleteReducer,
  hardwareTypeGridReducer: HardwareTypeGridReducer,

  originalEquipmentManufacturerCreateReducer:
    OriginalEquipmentManufacturerCreateReducer,
  originalEquipmentManufacturerEditReducer:
    OriginalEquipmentManufacturerEditReducer,
  originalEquipmentManufacturerDeleteReducer:
    OriginalEquipmentManufacturerDeleteReducer,
  originalEquipmentManufacturerGridReducer:
    OriginalEquipmentManufacturerGridReducer,
  componentManufacturerEditReducer: ComponentManufacturerEditReducer,
  componentManufacturerCreateReducer: ComponentManufacturerCreateReducer,
  componentManufacturerDeleteReducer: ComponentManufacturerDeleteReducer,
  componentManufacturerGridReducer: ComponentManufacturerGridReducer,

  bundleUpgradeInitiativeCreateReducer: BundleUpgradeInitiativeCreateReducer,
  bundleUpgradeInitiativeEditReducer: BundleUpgradeInitiativeEditReducer,
  bundleUpgradeInitiativeDeleteReducer: BundleUpgradeInitiativeDeleteReducer,
  bundleUpgradeInitiativeGridReducer: BundleUpgradeInitiativeGridReducer,

  externalRefreshReducer: ExternalRefreshReducer,

  vNFTransitionCreateReducer: VNFTransitionCreateReducer,
  vNFTransitionEditReducer: VNFTransitionEditReducer,
  vNFTransitionDeleteReducer: VNFTransitionDeleteReducer,
  vNFTransitionGridReducer: VNFTransitionGridReducer,

  volteKPICreateReducer: VolteKPICreateReducer,
  volteKPIEditReducer: VolteKPIEditReducer,
  volteKPIDeleteReducer: VolteKPIDeleteReducer,
  volteKPIGridReducer: VolteKPIGridReducer,

  worklogApprovalsEditReducer: WorklogApprovalsEditReducer,
  worklogApprovalsGridReducer: WorklogApprovalsGridReducer,

  reportVolteKPIReducer: ReportVolteKPIGridReducer,
  exportVolteKPIReducer: ExportVolteKPIReducer,

  reportPATGridReducer: ReportPATGridReducer,
  AssetPivotByLocationGridReducer: AssetPivotByLocationGridReducer,

  nFVITransitionCreateReducer: NFVITransitionCreateReducer,
  nFVITransitionEditReducer: NFVITransitionEditReducer,
  nFVITransitionDeleteReducer: NFVITransitionDeleteReducer,
  nFVITransitionGridReducer: NFVITransitionGridReducer,

  nFVIStatusCreateReducer: NFVIStatusCreateReducer,
  nFVIStatusEditReducer: NFVIStatusEditReducer,
  nFVIStatusDeleteReducer: NFVIStatusDeleteReducer,
  nFVIStatusGridReducer: NFVIStatusGridReducer,

  subDomainSpocCreateReducer: SubDomainSpocCreateReducer,
  subDomainSpocEditReducer: SubDomainSpocEditReducer,
  subDomainSpocDeleteReducer: SubDomainSpocDeleteReducer,
  subDomainSpocGridReducer: SubDomainSpocGridReducer,

  generalSettingsReducer: GeneralSettingsReducer,
  generalSettingsEditReducer: GeneralSettingsEditReducer,
  nFVICCompatibleReducer: NFVICCompatibleReducer,
  nFVICCompatibilityReportReducer: NFVICCompatibilityReportReducer,

  bptReportGridReducer: BPTReportGridReducer,

  clusterInfoGridReducer: ClusterInfoGridReducer,

  programCreateReducer: ProgramCreateReducer,
  programEditReducer: ProgramEditReducer,
  programDeleteReducer: ProgramDeleteReducer,
  programGridReducer: ProgramGridReducer,

  assetsDetailsReducer: AssetsDetailsReducer,
  daassetsMigrationGridReducer: DAAssetsMigrationGridReducer,
  assetLevelReportForExodusReducer: AssetLevelReportForExodusReducer,

  // getUSersLoggingLevelsReducer: GetUSersLoggingLevelsReducer,
});

// Add logger middleware if needed
const logger = (store) => (next) => (action) => {
  let result = next(action);
  return result;
};

// Use configureStore from Redux Toolkit
export const rootStore = configureStore({
  reducer: rootReducer,
  middleware: (getDefaultMiddleware) =>
    getDefaultMiddleware({
      thunk: true, // Enable thunk middleware
      immutableCheck: false, // Disable ImmutableStateInvariantMiddleware
      serializableCheck: false, // Disable SerializableStateInvariantMiddleware
    }).concat(logger),
  // devTools: import.meta.env.NODE_ENV !== 'production', // Enable Redux DevTools only in development
});

export type RootState = ReturnType<typeof rootReducer>;
// export default rootStore;

export type AppDispatch = typeof rootStore.dispatch;
export const useAppDispatch = () => useDispatch<AppDispatch>();
export const useAppSelector: TypedUseSelectorHook<RootState> = useSelector;
