import { FilterValueDto } from "../Business/Common/CommonBusiness";
import {
  GridDtoBase,
  RenderDetail,
  QueryObject,
  DateFilter,
  CustomGridRender,
  QueryObjectGrid,
} from "./Common";
import { ResultDto } from "./CommonModels";

/**
 *
 * @export
 * @interface CBOMQueryDto
 */
export interface CBOMQueryDto extends QueryObject {
  /**
   *
   * @type {Array<number>}
   * @memberof CBOMQueryDto
   */
  vnfVmCapacityId: Array<number>;
  /**
   *
   * @type {Array<number>}
   * @memberof CBOMQueryDto
   */
  vnfInfoId: Array<number>;
  /**
   *
   * @type {Array<string>}
   * @memberof CBOMQueryDto
   */
  vnfNameDescritpion: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof CBOMQueryDto
   */
  vmTypeNameDescription: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof CBOMQueryDto
   */
  shortLocation: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof CBOMQueryDto
   */
  nsxt: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof CBOMQueryDto
   */
  intraVmType: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof CBOMQueryDto
   */
  interVmType: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof CBOMQueryDto
   */
  vmWorkLoadType: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof CBOMQueryDto
   */
  vmStorageBlockSize: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof CBOMQueryDto
   */
  opCoDescritpion: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof CBOMQueryDto
   */
  noOfVnfInstances: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof CBOMQueryDto
   */
  noOfVmsPerType: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof CBOMQueryDto
   */
  numa: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof CBOMQueryDto
   */
  socket: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof CBOMQueryDto
   */
  financialYear: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof CBOMQueryDto
   */
  vcpuPerVm: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof CBOMQueryDto
   */
  rxTxCpuCount: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof CBOMQueryDto
   */
  ramPerVm: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof CBOMQueryDto
   */
  dataDisk: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof CBOMQueryDto
   */
  osDisk: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof CBOMQueryDto
   */
  iopsRunning: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof CBOMQueryDto
   */
  iopsLoading: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof CBOMQueryDto
   */
  vmWorkLoadDistribution: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof CBOMQueryDto
   */
  northDouthBoundBandWidth: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof CBOMQueryDto
   */
  eastWestBoundBandWidth: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof CBOMQueryDto
   */
  otherRequirements: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof CBOMQueryDto
   */
  backupRequired: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof CBOMQueryDto
   */
  probIngRequired: Array<string>;
}

/**
 *
 * @export
 * @interface CBOMDtoGrid
 */
export interface CBOMDtoGrid extends GridDtoBase {
  /**
   *
   * @type {number}
   * @memberof CBOMDtoGrid
   */
  cnfCapacityId: number;
  /**
   *
   * @type {number}
   * @memberof CBOMDtoGrid
   */
  cnfClusterInfoId: number;
  /**
   *
   * @type {string}
   * @memberof CBOMDtoGrid
   */
  cnfClusterNameDescription: string;
  /**
   *
   * @type {number}
   * @memberof CBOMDtoGrid
   */
  cnfInfoId: number;
  /**
   *
   * @type {number}
   * @memberof CBOMDtoGrid
   */
  cnfClusterInfoInstanceId?: number;
  /**
   *
   * @type {string}
   * @memberof CBOMDtoGrid
   */
  cnfNameDescritpion: string;
  /**
   *
   * @type {number}
   * @memberof CBOMDtoGrid
   */
  cnfNameId: number;
  /**
   *
   * @type {number}
   * @memberof CBOMDtoGrid
   */
  cpuKubelet: number;
  /**
   *
   * @type {number}
   * @memberof CBOMDtoGrid
   */
  cpuSystem: number;
  /**
   *
   * @type {string}
   * @memberof CBOMDtoGrid
   */
  daemonSetPod: string;
  /**
   *
   * @type {string}
   * @memberof CBOMDtoGrid
   */
  eastWestBandWidthForPodType: string;
  /**
   *
   * @type {string}
   * @memberof CBOMDtoGrid
   */
  financialYear: string;
  /**
   *
   * @type {number}
   * @memberof CBOMDtoGrid
   */
  functionStandardId: number;
  /**
   *
   * @type {string}
   * @memberof CBOMDtoGrid
   */
  functionStandardName: string;
  /**
   *
   * @type {string}
   * @memberof CBOMDtoGrid
   */
  hardware: string;
  /**
   *
   * @type {string}
   * @memberof CBOMDtoGrid
   */
  hyperThreading: string;
  /**
   *
   * @type {string}
   * @memberof CBOMDtoGrid
   */
  interPodRules: string;
  /**
   *
   * @type {string}
   * @memberof CBOMDtoGrid
   */
  intraPodRules: string;
  /**
   *
   * @type {string}
   * @memberof CBOMDtoGrid
   */
  isEnhancedHa: string;
  /**
   *
   * @type {string}
   * @memberof CBOMDtoGrid
   */
  isPersistanceStorageFlag: string;
  /**
   *
   * @type {string}
   * @memberof CBOMDtoGrid
   */
  isPresistentVolumesRequired: string;
  /**
   *
   * @type {string}
   * @memberof CBOMDtoGrid
   */
  isProdhPaEnable: string;
  /**
   *
   * @type {string}
   * @memberof CBOMDtoGrid
   */
  listOfCapacitySpecialRequirement: string;
  /**
   *
   * @type {string}
   * @memberof CBOMDtoGrid
   */
  location: string;
  /**
   *
   * @type {number}
   * @memberof CBOMDtoGrid
   */
  memKubelet: number;
  /**
   *
   * @type {string}
   * @memberof CBOMDtoGrid
   */
  memRequestForPodType: string;
  /**
   *
   * @type {number}
   * @memberof CBOMDtoGrid
   */
  memSystem: number;
  /**
   *
   * @type {string}
   * @memberof CBOMDtoGrid
   */
  noOfCnfInstancesPersite: string;
  /**
   *
   * @type {string}
   * @memberof CBOMDtoGrid
   */
  nodePoolBreakUp: string;
  /**
   *
   * @type {string}
   * @memberof CBOMDtoGrid
   */
  nodePoolName: string;
  /**
   *
   * @type {string}
   * @memberof CBOMDtoGrid
   */
  nonPresistentStorageForProdType: string;
  /**
   *
   * @type {string}
   * @memberof CBOMDtoGrid
   */
  northSouthBandWidthForPodType: string;
  /**
   *
   * @type {string}
   * @memberof CBOMDtoGrid
   */
  numberOfPodsPerPodType: string;
  /**
   *
   * @type {number}
   * @memberof CBOMDtoGrid
   */
  opcoId: number;
  /**
   *
   * @type {string}
   * @memberof CBOMDtoGrid
   */
  opcoName: string;
  /**
   *
   * @type {string}
   * @memberof CBOMDtoGrid
   */
  overProvisioning: string;
  /**
   *
   * @type {string}
   * @memberof CBOMDtoGrid
   */
  persistentStorageForPodType: string;
  /**
   *
   * @type {string}
   * @memberof CBOMDtoGrid
   */
  persistentVolumNeaccessMode: string;
  /**
   *
   * @type {string}
   * @memberof CBOMDtoGrid
   */
  podRoleDescription: string;
  /**
   *
   * @type {number}
   * @memberof CBOMDtoGrid
   */
  podRoleDescriptionId: number;
  /**
   *
   * @type {string}
   * @memberof CBOMDtoGrid
   */
  podTypeInfoName: string;
  /**
   *
   * @type {string}
   * @memberof CBOMDtoGrid
   */
  podTypeQos: string;
  /**
   *
   * @type {string}
   * @memberof CBOMDtoGrid
   */
  priority: string;
  /**
   *
   * @type {string}
   * @memberof CBOMDtoGrid
   */
  site: string;
  /**
   *
   * @type {number}
   * @memberof CBOMDtoGrid
   */
  siteId: number;
  /**
   *
   * @type {string}
   * @memberof CBOMDtoGrid
   */
  specialRequirementPerPodType: string;
  /**
   *
   * @type {string}
   * @memberof CBOMDtoGrid
   */
  specialRequirements: string;
  /**
   *
   * @type {string}
   * @memberof CBOMDtoGrid
   */
  storageIopsForPodType: string;
  /**
   *
   * @type {string}
   * @memberof CBOMDtoGrid
   */
  storagerWorkloadDistribution: string;
  /**
   *
   * @type {string}
   * @memberof CBOMDtoGrid
   */
  vcpuRequestForPodType: string;
  /**
   *
   * @type {string}
   * @memberof CBOMDtoGrid
   */
  verticalDomain: string;
  /**
   *
   * @type {string}
   * @memberof CBOMDtoGrid
   */
  workerNodeConfiguration: string;
}

export interface CBOMInstanceDtoGrid extends GridDtoBase {
  /**
   *
   * @type {number}
   * @memberof CBOMDtoGrid
   */
  cnfCapacityId: number;
  /**
   *
   * @type {number}
   * @memberof CBOMDtoGrid
   */
  cnfPodInfoId: number;
  /**
   *
   * @type {number}
   * @memberof CBOMDtoGrid
   */
  cnfClusterInfoId: number;
  /**
   *
   * @type {string}
   * @memberof CBOMDtoGrid
   */
  daemonSetPod: string;
  /**
   *
   * @type {number}
   * @memberof CBOMDtoGrid
   */
  functionStandardId: number;
  /**
   *
   * @type {string}
   * @memberof CBOMDtoGrid
   */
  functionStandardName: string;
  /**
   *
   * @type {string}
   * @memberof CBOMDtoGrid
   */
  interPodRules: string;
  /**
   *
   * @type {string}
   * @memberof CBOMDtoGrid
   */
  intraPodRules: string;
  /**
   *
   * @type {string}
   * @memberof CBOMDtoGrid
   */
  isEnhancedHa: string;
  /**
   *
   * @type {string}
   * @memberof CBOMDtoGrid
   */
  isPersistanceStorageFlag: string;
  /**
   *
   * @type {string}
   * @memberof CBOMDtoGrid
   */
  isProdhPaEnable: string;
  /**
   *
   * @type {string}
   * @memberof CBOMDtoGrid
   */
  numberOfPodsPerPodType: string;
  /**
   *
   * @type {string}
   * @memberof CBOMDtoGrid
   */
  podroleDescription: string;
  /**
   *
   * @type {number}
   * @memberof CBOMDtoGrid
   */
  podroleDescriptionId: number;
  /**
   *
   * @type {number}
   * @memberof CBOMDtoGrid
   */
  podTypeInfoId: number;
  /**
   *
   * @type {string}
   * @memberof CBOMDtoGrid
   */
  podTypeInfoName: string;
  /**
   *
   * @type {string}
   * @memberof CBOMDtoGrid
   */
  podTypeQos: string;
  /**
   *
   * @type {string}
   * @memberof CBOMDtoGrid
   */
  priorityName: string;
  /**
   *
   * @type {number}
   * @memberof CBOMDtoGrid
   */
  priorityId: number;
}

export interface CBOMCapacityDtoGrid extends GridDtoBase {
  /**
   *
   * @type {number}
   * @memberof CBOMDtoGrid
   */
  cnfCapacityId: number;
  /**
   *
   * @type {string}
   * @memberof CBOMDtoGrid
   */
  eastWestBandWidthForPodType: string;
  /**
   *
   * @type {string}
   * @memberof CBOMDtoGrid
   */
  financialYear: string;
  /**
   *
   * @type {string}
   * @memberof CBOMDtoGrid
   */
  financialVersion: string;
  /**
   *
   * @type {string}
   * @memberof CBOMDtoGrid
   */
  isPresistentVolumesRequired: string;
  /**
   *
   * @type {string}
   * @memberof CBOMDtoGrid
   */
  listOfCapacitySpecialRequirement: string;
  /**
   *
   * @type {string}
   * @memberof CBOMDtoGrid
   */
  memRequestForPodType: string;
  /**
   *
   * @type {number}
   * @memberof CBOMDtoGrid
   */
  memLimitForPodType: number;
  /**
   *
   * @type {string}
   * @memberof CBOMDtoGrid
   */
  noOfCnfInstancesPersite: string;
  /**
   *
   * @type {string}
   * @memberof CBOMDtoGrid
   */
  nonPresistentStorageForProdType: string;
  /**
   *
   * @type {string}
   * @memberof CBOMDtoGrid
   */
  northSouthBandWidthForPodType: string;
  /**
   *
   * @type {string}
   * @memberof CBOMDtoGrid
   */
  pcpuRequestForPodType: string;
  /**
   *
   * @type {string}
   * @memberof CBOMDtoGrid
   */
  persistentStorageForPodType: string;
  /**
   *
   * @type {string}
   * @memberof CBOMDtoGrid
   */
  persistentVolumNeaccessMode: string;
  /**
   *
   * @type {string}
   * @memberof CBOMDtoGrid
   */
  specialRequirementPerPodType: string;
  /**
   *
   * @type {string}
   * @memberof CBOMDtoGrid
   */
  storageIopsForPodType: string;
  /**
   *
   * @type {string}
   * @memberof CBOMDtoGrid
   */
  storagerWorkloadDistribution: string;
  /**
   *
   * @type {string}
   * @memberof CBOMDtoGrid
   */
  vcpuRequestForPodType: string;
  /**
   *
   * @type {string}
   * @memberof CBOMDtoGrid
   */
  vcpuLimitForPodType: string;
}

/**
 *
 * @export
 * @interface CBOMClusterInfoDtoGrid
 */
export interface CBOMClusterInfoDtoGrid extends GridDtoBase {
  /**
   *
   * @type {number}
   * @memberof CBOMClusterInfoDtoGrid
   */
  cnfCapacityId: number;
  /**
   *
   * @type {number}
   * @memberof CBOMClusterInfoDtoGrid
   */
  cnfClusterInfoId: number;
  /**
   *
   * @type {string}
   * @memberof CBOMClusterInfoDtoGrid
   */
  cnfClusterNameDescription: string;
  /**
   *
   * @type {number}
   * @memberof CBOMClusterInfoDtoGrid
   */
  cnfInfoId: number;
  /**
   *
   * @type {number}
   * @memberof CBOMClusterInfoDtoGrid
   */
  cnfClusterInfoInstanceId?: number;
  /**
   *
   * @type {string}
   * @memberof CBOMClusterInfoDtoGrid
   */
  cnfNameDescritpion: string;
  /**
   *
   * @type {number}
   * @memberof CBOMClusterInfoDtoGrid
   */
  cnfNameId: number;
  /**
   *
   * @type {number}
   * @memberof CBOMClusterInfoDtoGrid
   */
  cpuKubelet: number;
  /**
   *
   * @type {number}
   * @memberof CBOMClusterInfoDtoGrid
   */
  cpuSystem: number;
  /**
   *
   * @type {string}
   * @memberof CBOMClusterInfoDtoGrid
   */
  daemonSetPod: string;
  /**
   *
   * @type {string}
   * @memberof CBOMClusterInfoDtoGrid
   */
  eastWestBandWidthForPodType: string;
  /**
   *
   * @type {string}
   * @memberof CBOMClusterInfoDtoGrid
   */
  financialYear: string;
  /**
   *
   * @type {number}
   * @memberof CBOMClusterInfoDtoGrid
   */
  functionStandardId: number;
  /**
   *
   * @type {string}
   * @memberof CBOMClusterInfoDtoGrid
   */
  functionStandardName: string;
  /**
   *
   * @type {string}
   * @memberof CBOMClusterInfoDtoGrid
   */
  hardware: string;
  /**
   *
   * @type {string}
   * @memberof CBOMClusterInfoDtoGrid
   */
  hyperThreading: string;
  /**
   *
   * @type {string}
   * @memberof CBOMClusterInfoDtoGrid
   */
  interPodRules: string;
  /**
   *
   * @type {string}
   * @memberof CBOMClusterInfoDtoGrid
   */
  intraPodRules: string;
  /**
   *
   * @type {string}
   * @memberof CBOMClusterInfoDtoGrid
   */
  isEnhancedHa: string;
  /**
   *
   * @type {string}
   * @memberof CBOMClusterInfoDtoGrid
   */
  isPersistanceStorageFlag: string;
  /**
   *
   * @type {string}
   * @memberof CBOMClusterInfoDtoGrid
   */
  isPresistentVolumesRequired: string;
  /**
   *
   * @type {string}
   * @memberof CBOMClusterInfoDtoGrid
   */
  isProdhPaEnable: string;
  /**
   *
   * @type {string}
   * @memberof CBOMClusterInfoDtoGrid
   */
  listOfCapacitySpecialRequirement: string;
  /**
   *
   * @type {string}
   * @memberof CBOMClusterInfoDtoGrid
   */
  location: string;
  /**
   *
   * @type {number}
   * @memberof CBOMClusterInfoDtoGrid
   */
  memKubelet: number;
  /**
   *
   * @type {string}
   * @memberof CBOMClusterInfoDtoGrid
   */
  memRequestForPodType: string;
  /**
   *
   * @type {number}
   * @memberof CBOMClusterInfoDtoGrid
   */
  memSystem: number;
  /**
   *
   * @type {string}
   * @memberof CBOMClusterInfoDtoGrid
   */
  noOfCnfInstancesPersite: string;
  /**
   *
   * @type {string}
   * @memberof CBOMClusterInfoDtoGrid
   */
  nodePoolBreakUp: string;
  /**
   *
   * @type {string}
   * @memberof CBOMClusterInfoDtoGrid
   */
  nodePoolName: string;
  /**
   *
   * @type {string}
   * @memberof CBOMClusterInfoDtoGrid
   */
  nonPresistentStorageForProdType: string;
  /**
   *
   * @type {string}
   * @memberof CBOMClusterInfoDtoGrid
   */
  northSouthBandWidthForPodType: string;
  /**
   *
   * @type {string}
   * @memberof CBOMClusterInfoDtoGrid
   */
  numberOfPodsPerPodType: string;
  /**
   *
   * @type {number}
   * @memberof CBOMClusterInfoDtoGrid
   */
  opcoId: number;
  /**
   *
   * @type {string}
   * @memberof CBOMClusterInfoDtoGrid
   */
  opcoName: string;
  /**
   *
   * @type {string}
   * @memberof CBOMClusterInfoDtoGrid
   */
  overProvisioning: string;
  /**
   *
   * @type {string}
   * @memberof CBOMClusterInfoDtoGrid
   */
  persistentStorageForPodType: string;
  /**
   *
   * @type {string}
   * @memberof CBOMClusterInfoDtoGrid
   */
  persistentVolumNeaccessMode: string;
  /**
   *
   * @type {string}
   * @memberof CBOMClusterInfoDtoGrid
   */
  podRoleDescription: string;
  /**
   *
   * @type {number}
   * @memberof CBOMClusterInfoDtoGrid
   */
  podRoleDescriptionId: number;
  /**
   *
   * @type {string}
   * @memberof CBOMClusterInfoDtoGrid
   */
  podTypeInfoName: string;
  /**
   *
   * @type {string}
   * @memberof CBOMClusterInfoDtoGrid
   */
  podTypeQos: string;
  /**
   *
   * @type {string}
   * @memberof CBOMClusterInfoDtoGrid
   */
  priority: string;
  /**
   *
   * @type {string}
   * @memberof CBOMClusterInfoDtoGrid
   */
  site: string;
  /**
   *
   * @type {number}
   * @memberof CBOMClusterInfoDtoGrid
   */
  siteId: number;
  /**
   *
   * @type {string}
   * @memberof CBOMClusterInfoDtoGrid
   */
  specialRequirementPerPodType: string;
  /**
   *
   * @type {string}
   * @memberof CBOMClusterInfoDtoGrid
   */
  specialRequirements: string;
  /**
   *
   * @type {string}
   * @memberof CBOMClusterInfoDtoGrid
   */
  storageIopsForPodType: string;
  /**
   *
   * @type {string}
   * @memberof CBOMClusterInfoDtoGrid
   */
  storagerWorkloadDistribution: string;
  /**
   *
   * @type {string}
   * @memberof CBOMClusterInfoDtoGrid
   */
  vcpuRequestForPodType: string;
  /**
   *
   * @type {string}
   * @memberof CBOMClusterInfoDtoGrid
   */
  verticalDomain: string;
  /**
   *
   * @type {string}
   * @memberof CBOMClusterInfoDtoGrid
   */
  workerNodeConfiguration: string;
}
/**
 *
 * @export
 * @interface CnfClusterInfoDto
 */
export interface CnfClusterInfoDto {
  /**
   *
   * @type {number}
   * @memberof CnfClusterInfoDto
   */
  cnfpodinfoid: number;
  /**
   *
   * @type {number}
   * @memberof CnfClusterInfoDto
   */
  podtypeinfoid: number;
  /**
   *
   * @type {string}
   * @memberof CnfClusterInfoDto
   */
  podtypeinfo?: string;
  /**
   *
   * @type {number}
   * @memberof CnfClusterInfoDto
   */
  podroledescriptionid: number;
  /**
   *
   * @type {string}
   * @memberof CnfClusterInfoDto
   */
  podroledescription?: string;
  /**
   *
   * @type {number}
   * @memberof CnfClusterInfoDto
   */
  functionstandardid: number;
  /**
   *
   * @type {string}
   * @memberof CnfClusterInfoDto
   */
  functionstandard?: string;
  /**
   *
   * @type {number}
   * @memberof CnfClusterInfoDto
   */
  priorityid: number;
  /**
   *
   * @type {string}
   * @memberof CnfClusterInfoDto
   */
  priority?: string;
  /**
   *
   * @type {boolean}
   * @memberof CnfClusterInfoDto
   */
  daemonsetpod: boolean;

  /**
   *
   * @type {string}
   * @memberof CnfClusterInfoDto
   */
  intrapodrules: string;

  /**
   *
   * @type {string}
   * @memberof CnfClusterInfoDto
   */
  interpodrules: string;
  /**
   *
   * @type {boolean}
   * @memberof CnfClusterInfoDto
   */
  isenhancedha: boolean;

  /**
   *
   * @type {string}
   * @memberof CnfClusterInfoDto
   */
  podtypeqos: string;

  /**
   *
   * @type {string}
   * @memberof CnfClusterInfoDto
   */
  ispersistancestorageflag: string;
  /**
   *
   * @type {boolean}
   * @memberof CnfClusterInfoDto
   */
  isprodhpaenable: boolean;
  cnfcapacity: Array<CnfCapacityDto>;
}

/**
 *
 * @export
 * @interface CnfCapacityDto
 */
export interface CnfCapacityDto {
  /**
   *
   * @type {number}
   * @memberof VNFVMCapacity
   */
  cnfcapacityid: number;
  /**
   *
   * @type {number | null}
   * @memberof VNFVMCapacity
   */
  cnfclusterinfoid?: number | null;
  /**
   *
   * @type {number}
   * @memberof VNFVMCapacity
   */
  financialyear: number;
  /**
   *
   * @type {string}
   * @memberof VNFVMClusterCapacity
   */
  financialversion: string;
  /**
   *
   * @type {number}
   * @memberof VNFVMCapacity
   */
  noofcnfinstancespersite: number;
  /**
   *
   * @type {number}
   * @memberof VNFVMCapacity
   */
  numberofpodsperpodtype: number;
  /**
   *
   * @type {number}
   * @memberof VNFVMCapacity
   */
  vcpurequestforpodtype: number;
  /**
   *
   * @type {number}
   * @memberof VNFVMCapacity
   */
  vcpulimitforpodtype: number;
  /**
   *
   * @type {number}
   * @memberof VNFVMCapacity
   */
  pcpurequestforpodtype: number;
  /**
   *
   * @type {string}
   * @memberof VNFVMCapacity
   */
  memrequestforpodtype: string;
  /**
   *
   * @type {string}
   * @memberof VNFVMCapacity
   */
  memlimitforpodtype: string;
  /**
   *
   * @type {string}
   * @memberof VNFVMCapacity
   */
  nonpresistentstorageforprodtype: string;
  /**
   *
   * @type {boolean}
   * @memberof VNFVMCapacity
   */
  ispresistentvolumesrequired: boolean;
  /**
   *
   * @type {string}
   * @memberof VNFVMCapacity
   */
  persistentvolumneaccessmode: string;
  /**
   *
   * @type {string}
   * @memberof VNFVMCapacity
   */
  persistentstorageforpodtype: string;
  /**
   *
   * @type {string}
   * @memberof VNFVMCapacity
   */
  storageiopsforpodtype: string;
  /**
   *
   * @type {string}
   * @memberof VNFVMCapacity
   */
  storagerworkloaddistribution: string;
  /**
   *
   * @type {string}
   * @memberof VNFVMCapacity
   */
  northsouthbandwidthforpodtype: string;
  /**
   *
   * @type {string}
   * @memberof VNFVMCapacity
   */
  eastwestbandwidthforpodtype: string;
  /**
   *
   * @type {string}
   * @memberof VNFVMCapacity
   */
  specialrequirementperpodtype: string;
  /**
   *
   * @type {string}
   * @memberof VNFVMCapacity
   */
  specialrequirement?: string;
  /**
   *
   * @type {string}
   * @memberof VNFVMCapacity
   */
  capacityspecialrequirement: string;
}

/**
 *
 * @export
 * @interface CBOMDtoCreate
 */
export interface CBOMDtoCreate extends GridDtoBase {
  /**
   *
   * @type {number}
   * @memberof CBOMDtoCreate
   */
  cnfnameid: number;
  /**
   *
   * @type {number}
   * @memberof CnfClusterInfoDto
   */
  cnfclusterid: number;
  /**
   *
   * @type {number}
   * @memberof CnfClusterInfoDto
   */
  opcoid: number;
  /**
   *
   * @type {string}
   * @memberof VNFVMInstances
   */
  opco?: string;
  /**
   *
   * @type {number | null}
   * @memberof CnfClusterInfoDto
   */
  siteid: number | null;
  /**
   *
   * @type {number}
   * @memberof CnfClusterInfoDto
   */
  cnfclusternodepoolid: number;
  /**
   *
   * @type {boolean}
   * @memberof CBOMDtoCreate
   */
  nodepoolbreakup: boolean;
  /**
   *
   * @type {string}
   * @memberof CBOMDtoCreate
   */
  specialrequirements: string;
  /**
   *
   * @type {string}
   * @memberof CBOMDtoCreate
   */
  hyperthreading: string;
  /**
   *
   * @type {string}
   * @memberof CBOMDtoCreate
   */
  overprovisioning: string;
  /**
   *
   * @type {string}
   * @memberof CBOMDtoCreate
   */
  workernodeconfiguration: string;
  /**
   *
   * @type {string}
   * @memberof CBOMDtoCreate
   */
  hardware: string;
  /**
   *
   * @type {string}
   * @memberof CBOMDtoCreate
   */
  aggregateimageclustersize: string;
  /**
   *
   * @type {string}
   * @memberof CBOMDtoCreate
   */
  comments: string;
  /**
   *
   * @type {string}
   * @memberof CBOMDtoCreate
   */
  filename: string;
  /**
   *
   * @type {string}
   * @memberof CBOMDtoCreate
   */
  notes: string;
  /**
   *
   * @type {string}
   * @memberof CBOMDtoCreate
   */
  revision: string;
  /**
   *
   * @type {number}
   * @memberof CBOMDtoCreate
   */
  cpukubelet: number;
  /**
   *
   * @type {number}
   * @memberof CBOMDtoCreate
   */
  memkubelet: number;
  /**
   *
   * @type {number}
   * @memberof CBOMDtoCreate
   */
  cpusystem: number;
  /**
   *
   * @type {number}
   * @memberof CBOMDtoCreate
   */
  memsystem: number;
  /**
   *
   * @type {number}
   * @memberof CBOMDtoCreate
   */
  cnfhardwareid: number;
  /**
   *
   * @type {number}
   * @memberof CBOMDtoCreate
   */
  verticalresponsibleid: number;
  cnfclusterinfo?: Array<CnfClusterInfoDto>;
  cnfpodinfo?: Array<CnfClusterInfoDto>;
}

/**
 *
 * @export
 * @interface CBOMDtoUpdate
 */
export interface CBOMDtoUpdate extends CBOMDtoCreate {
  /**
   *
   * @type {number}
   * @memberof CBOMDtoUpdate
   */
  cnfclusterinfoid?: number;
}

/**
 *
 * @export
 * @interface CBOMDto
 */
export interface CBOMDto extends CBOMDtoCreate {
  /**
   *
   * @type {any}
   * @memberof CBOMDto
   */
  cnfClusterNameResource?: any;
  /**
   *
   * @type {any}
   * @memberof CBOMDto
   */
  cnfClusterNodePoolResource?: any;
  /**
   *
   * @type {{key: number, text: string}[]}
   * @memberof CBOMDto
   */
  cnfNameResources?: { key: number; text: string }[];
  /**
   *
   * @type {{key: number, text: string}[]}
   * @memberof CBOMDto
   */
  priorityResource?: { key: number; text: string }[];
  /**
   *
   * @type {{key: number, text: string}[]}
   * @memberof CBOMDto
   */
  hardwareResource?: { key: number; text: string }[];
  /**
   *
   * @type {{key: number, text: string}[]}
   * @memberof CBOMDto
   */
  verticalResource?: { key: number; text: string }[];
  /**
   *
   * @type {CBOMDtoUpdate}
   * @memberof CBOMDto
   */
  cnfinfoDetail?: CBOMDtoUpdate;
  /**
   *
   * @type {{value: string, text: string}[]}
   * @memberof CBOMDto
   */
  financialVersion?: { value: string; text: string }[];
  /**
   *
   * @type {CBOMDtoUpdate}
   * @memberof CBOMDto
   */
  _cnfClusterInfoEntity?: CBOMDtoUpdate;
  /**
   *
   * @type {any}
   * @memberof CBOMDto
   */
  functionStandardedNameResource?: any;
  /**
   *
   * @type {any}
   * @memberof CBOMDto
   */
  opcoBasedLocationResource?: any;
  /**
   *
   * @type {any}
   * @memberof CBOMDto
   */
  podTypeInfoResource?: any;
  /**
   *
   * @type {any}
   * @memberof CBOMDto
   */
  podTypeDescriptionInfoResource?: any;
}

/**
 *
 * @export
 * @interface CBOMCnfInstanceAndCapacityDtoGrid
 */
export interface CBOMCnfInstanceAndCapacityDtoGrid extends GridDtoBase {
  /**
   *
   * @type {number}
   * @memberof CBOMCnfInstanceAndCapacityDtoGrid
   */
  cnfclusterinfoid?: number;
  /**
   *
   * @type {number}
   * @memberof CBOMCnfInstanceAndCapacityDtoGrid
   */
  cnfnameid?: number;
  /**
   *
   * @type {number}
   * @memberof CBOMCnfInstanceAndCapacityDtoGrid
   */
  cnfPodInfoId?: number;
  /**
   *
   * @type {number}
   * @memberof CBOMCnfInstanceAndCapacityDtoGrid
   */
  opcoid?: number;
  /**
   *
   * @type {number}
   * @memberof CBOMCnfInstanceAndCapacityDtoGrid
   */
  siteid?: number;
  /**
   *
   * @type {number}
   * @memberof CBOMCnfInstanceAndCapacityDtoGrid
   */
  cnfhardwareid?: number;
  /**
   *
   * @type {number}
   * @memberof CBOMCnfInstanceAndCapacityDtoGrid
   */
  noofcnfinstancespersite?: number;
  /**
   *
   * @type {boolean}
   * @memberof CBOMCnfInstanceAndCapacityDtoGrid
   */
  nodepoolbreakup?: boolean;
  /**
   *
   * @type {string}
   * @memberof CBOMCnfInstanceAndCapacityDtoGrid
   */
  specialrequirements?: string;
  /**
   *
   * @type {string}
   * @memberof CBOMCnfInstanceAndCapacityDtoGrid
   */
  hyperthreading?: string;
  /**
   *
   * @type {string}
   * @memberof CBOMCnfInstanceAndCapacityDtoGrid
   */
  overprovisioning?: string;
  /**
   *
   * @type {string}
   * @memberof CBOMCnfInstanceAndCapacityDtoGrid
   */
  workernodeconfiguration?: string;
  /**
   *
   * @type {string}
   * @memberof CBOMCnfInstanceAndCapacityDtoGrid
   */
  hardware?: string;
  /**
   *
   * @type {number}
   * @memberof CBOMCnfInstanceAndCapacityDtoGrid
   */
  cpukubelet?: number;
  /**
   *
   * @type {number}
   * @memberof CBOMCnfInstanceAndCapacityDtoGrid
   */
  memkubelet?: number;
  /**
   *
   * @type {number}
   * @memberof CBOMCnfInstanceAndCapacityDtoGrid
   */
  cpusystem?: number;
  /**
   *
   * @type {number}
   * @memberof CBOMCnfInstanceAndCapacityDtoGrid
   */
  memsystem?: number;
  /**
   *
   * @type {number}
   * @memberof CBOMCnfInstanceAndCapacityDtoGrid
   */
  verticalresponsibleid?: number;
  /**
   *
   * @type {string}
   * @memberof CBOMCnfInstanceAndCapacityDtoGrid
   */
  comments?: string;
  /**
   *
   * @type {string}
   * @memberof CBOMCnfInstanceAndCapacityDtoGrid
   */
  notes?: string;
  /**
   *
   * @type {string}
   * @memberof CBOMCnfInstanceAndCapacityDtoGrid
   */
  filename?: string;
  /**
   *
   * @type {string}
   * @memberof CBOMCnfInstanceAndCapacityDtoGrid
   */
  revision?: string;
  /**
   *
   * @type {string}
   * @memberof CBOMCnfInstanceAndCapacityDtoGrid
   */
  aggregateimageclustersize?: string;
  /**
   *
   * @type {string}
   * @memberof CBOMCnfInstanceAndCapacityDtoGrid
   */
  opcoDescritpion?: string;
  /**
   *
   * @type {string}
   * @memberof CBOMCnfInstanceAndCapacityDtoGrid
   */
  siteName?: string;
  /**
   *
   * @type {string}
   * @memberof CBOMCnfInstanceAndCapacityDtoGrid
   */
  locationName?: string;
  /**
   *
   * @type {string}
   * @memberof CBOMCnfInstanceAndCapacityDtoGrid
   */
  clusterDescription?: string;
  /**
   *
   * @type {string}
   * @memberof CBOMCnfInstanceAndCapacityDtoGrid
   */
  cnfNameDescription?: string;
  /**
   *
   * @type {string}
   * @memberof CBOMCnfInstanceAndCapacityDtoGrid
   */
  cnfhardwareDescription?: string;
  /**
   *
   * @type {string}
   * @memberof CBOMCnfInstanceAndCapacityDtoGrid
   */
  verticalDescription?: string;
  /**
   *
   * @type {CbomCapacityQueryDto}
   * @memberof CBOMCnfInstanceAndCapacityDtoGrid
   */
  _cnfCapacityDtoGrid?: CbomCapacityQueryDto;
}
/**
 *
 * @export
 * @interface CbomCapacityQueryDto
 */
export interface CbomCapacityQueryDto {
  /**
   *
   * @type {Array<number>}
   * @memberof VbomCapacityQueryDto
   */
  cnfCapacityId: Array<number>;
  /**
   *
   * @type {Array<string>}
   * @memberof VbomCapacityQueryDto
   */
  eastWestBandWidthForPodType: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof VbomCapacityQueryDto
   */
  financialVersion: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof VbomCapacityQueryDto
   */
  financialYear: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof VbomCapacityQueryDto
   */
  isPresistentVolumesRequired: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof VbomCapacityQueryDto
   */
  listOfCapacitySpecialRequirement: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof VbomCapacityQueryDto
   */
  memLimitForPodType: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof VbomCapacityQueryDto
   */
  memRequestForPodType: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof VbomCapacityQueryDto
   */
  noOfCnfInstancesPersite: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof VbomCapacityQueryDto
   */
  nonPresistentStorageForProdType: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof VbomCapacityQueryDto
   */
  northSouthBandWidthForPodTyp: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof VbomCapacityQueryDto
   */
  pcpuRequestForPodType: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof VbomCapacityQueryDto
   */
  persistentStorageForPodType: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof VbomCapacityQueryDto
   */
  persistentVolumNeaccessMode: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof VbomCapacityQueryDto
   */
  specialRequirementPerPodType: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof VbomCapacityQueryDto
   */
  storageIopsForPodType: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof VbomCapacityQueryDto
   */
  storagerWorkloadDistribution: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof VbomCapacityQueryDto
   */
  vcpuLimitForPodType: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof VbomCapacityQueryDto
   */
  vcpuRequestForPodType: Array<string>;
}

/**
 *
 * @export
 * @interface QueryResultDtoOfCBOMDtoGrid
 */
export interface QueryResultDtoOfCBOMDtoGrid {
  /**
   *
   * @type {number}
   * @memberof QueryResultDtoOfCBOMDtoGrid
   */
  totalItems?: number;
  /**
   *
   * @type {Array<CBOMDtoGrid>}
   * @memberof QueryResultDtoOfCBOMDtoGrid
   */
  items?: Array<CBOMDtoGrid>;
  /**
   *
   * @type {CustomGridRender}
   * @memberof CustomGridRender
   */
  gridRender?: CustomGridRender;
}

/**
 *
 * @export
 * @interface QueryResultDtoOfCBOMCnfInstanceAndCapacityDtoGrid
 */
export interface QueryResultDtoOfCBOMCnfInstanceAndCapacityDtoGrid {
  /**
   *
   * @type {number}
   * @memberof QueryResultDtoOfCBOMCnfInstanceAndCapacityDtoGrid
   */
  totalItems?: number;
  /**
   *
   * @type {Array<CBOMInstanceDtoGrid>}
   * @memberof QueryResultDtoOfCBOMCnfInstanceAndCapacityDtoGrid
   */
  items?: Array<CBOMInstanceDtoGrid>;
  /**
   *
   * @type {CustomGridRender}
   * @memberof CustomGridRender
   */
  gridRender?: CustomGridRender;
}

/**
 *
 * @export
 * @interface QueryResultDtoOfCBOMClusterInfoDtoGrid
 */
export interface QueryResultDtoOfCBOMClusterInfoDtoGrid {
  /**
   *
   * @type {number}
   * @memberof QueryResultDtoOfCBOMClusterInfoDtoGrid
   */
  totalItems?: number;
  /**
   *
   * @type {Array<CBOMClusterInfoDtoGrid>}
   * @memberof QueryResultDtoOfCBOMClusterInfoDtoGrid
   */
  items?: Array<CBOMClusterInfoDtoGrid>;
  /**
   *
   * @type {CustomGridRender}
   * @memberof CustomGridRender
   */
  gridRender?: CustomGridRender;
}

export interface CBOMQueryObjectGrid extends QueryObjectGrid {
  /**
   *
   * @type {Array<number>}
   * @memberof CBOMQueryDto
   */
  cnfCapacityId?: Array<number>;
  /**
   *
   * @type {Array<number>}
   * @memberof CBOMQueryDto
   */
  cnfPodInfoId?: Array<number>;
  /**
   *
   * @type {Array<number>}
   * @memberof CBOMQueryDto
   */
  financialYear?: Array<number>;
  /**
   *
   * @type {Array<string>}
   * @memberof CBOMQueryDto
   */
  financialVersion?: Array<string>;
  /**
   *
   * @type {Array<number>}
   * @memberof CBOMQueryDto
   */
  noOfCnfInstancesPersite?: Array<number>;
  /**
   *
   * @type {Array<number>}
   * @memberof CBOMQueryDto
   */
  numberOfPodsPerPodType?: Array<number>;
  /**
   *
   * @type {Array<number>}
   * @memberof CBOMQueryDto
   */
  vcpuRequestForPodType?: Array<number>;
  /**
   *
   * @type {Array<string>}
   * @memberof CBOMQueryDto
   */
  memRequestForPodType?: Array<string>;
  /**
   * @type {Array<string>}
   * @memberof CBOMQueryDto
   */
  nonPresistentStorageForProdType?: Array<string>;
  /**
   * @type {Array<string>}
   * @memberof CBOMQueryDto
   */
  isPresistentVolumesRequired?: Array<string>;
  /**
   * @type {Array<string>}
   * @memberof CBOMQueryDto
   */
  persistentVolumNeaccessMode?: Array<string>;
  /**
   * @type {Array<string>}
   * @memberof CBOMQueryDto
   */
  persistentStorageForPodType?: Array<string>;
  /**
   * @type {Array<string>}
   * @memberof CBOMQueryDto
   */
  storageIopsForPodType?: Array<string>;
  /**
   * @type {Array<string>}
   * @memberof CBOMQueryDto
   */
  storagerWorkloadDistribution?: Array<string>;
  /**
   * @type {Array<string>}
   * @memberof CBOMQueryDto
   */
  northSouthBandWidthForPodType?: Array<string>;
  /**
   * @type {Array<string>}
   * @memberof CBOMQueryDto
   */
  eastWestBandWidthForPodType?: Array<string>;
  /**
   * @type {Array<string>}
   * @memberof CBOMQueryDto
   */
  specialRequirementPerPodType?: Array<string>;
  /**
   * @type {Array<string>}
   * @memberof CBOMQueryDto
   */
  listOfCapacitySpecialRequirement?: Array<string>;
  /**
   * @type {Array<number>}
   * @memberof CBOMQueryDto
   */
  cnfClusterInfoId?: Array<number>;
  /**
   *
   * @type {Array<string>}
   * @memberof CBOMQueryDto
   */
  cnfClusterName?: Array<string>;
  /**
   * @type {Array<string>}
   * @memberof CBOMQueryDto
   */
  podTypeInfoName?: Array<string>;
  /**
   * @type {Array<string>}
   * @memberof CBOMQueryDto
   */
  opcoName?: Array<string>;
  /**
   * @type {Array<string>}
   * @memberof CBOMQueryDto
   */
  shortLocation?: Array<string>;
  /**
   * @type {Array<string>}
   * @memberof CBOMQueryDto
   */
  locationName?: Array<string>;
  /**
   * @type {Array<string>}
   * @memberof CBOMQueryDto
   */
  functionStandardName?: Array<string>;
  /**
   * @type {Array<string>}
   * @memberof CBOMQueryDto
   */
  priorityId?: Array<string>;
  /**
   * @type {Array<string>}
   * @memberof CBOMQueryDto
   */
  podRoleDescription?: Array<string>;
  /**
   * @type {Array<string>}
   * @memberof CBOMQueryDto
   */
  daemonSetPod?: Array<string>;
  /**
   * @type {Array<string>}
   * @memberof CBOMQueryDto
   */
  intraPodRules?: Array<string>;
  /**
   * @type {Array<string>}
   * @memberof CBOMQueryDto
   */
  interPodRules?: Array<string>;
  /**
   * @type {Array<boolean>}
   * @memberof CBOMQueryDto
   */
  isEnhancedHa?: Array<boolean>;
  /**
   * @type {Array<string>}
   * @memberof CBOMQueryDto
   */
  podTypeQos?: Array<string>;
  /**
   * @type {Array<string>}
   * @memberof CBOMQueryDto
   */
  isPersistanceStorageFlag?: Array<string>;
  /**
   * @type {Array<string>}
   * @memberof CBOMQueryDto
   */
  isProdhPaEnable?: Array<string>;
  /**
   * @type {Array<number>}
   * @memberof CBOMQueryDto
   */
  cnfInfoId?: Array<number>;
  /**
   *
   * @type {Array<string>}
   * @memberof CBOMQueryDto
   */
  cnfNameDescription?: Array<string>;
  /**
   * @type {Array<string>}
   * @memberof CBOMQueryDto
   */
  nodePoolBreakUp?: Array<string>;
  /**
   * @type {Array<string>}
   * @memberof CBOMQueryDto
   */
  specialRequirements?: Array<string>;
  /**
   * @type {Array<string>}
   * @memberof CBOMQueryDto
   */
  hyperThreading?: Array<string>;
  /**
   * @type {Array<string>}
   * @memberof CBOMQueryDto
   */
  overProvisioning?: Array<string>;
  /**
   * @type {Array<string>}
   * @memberof CBOMQueryDto
   */
  workerNodeConfiguration?: Array<string>;
  /**
   * @type {Array<string>}
   * @memberof CBOMQueryDto
   */
  hardware?: Array<string>;
  /**
   * @type {Array<string>}
   * @memberof CBOMQueryDto
   */
  cpuKubelet?: Array<string>;
  /**
   * @type {Array<number>}
   * @memberof CBOMQueryDto
   */
  memKubelet?: Array<number>;
  /**
   *
   * @type {Array<string>}
   * @memberof CBOMQueryDto
   */
  cpuSystem?: Array<string>;
  /**
   * @type {Array<number>}
   * @memberof CBOMQueryDto
   */
  memSystem?: Array<number>;
  /**
   *
   * @type {Array<string>}
   * @memberof CBOMQueryDto
   */
  verticalDomain?: Array<string>;
  /**
   *
   * @type {DateFilter}
   * @memberof SystemTypeQueryDto
   */
  lastModified?: DateFilter;
  /**
   *
   * @type {Array<string>}
   * @memberof SystemTypeQueryDto
   */
  lastModifiedBy?: Array<string>;
}

export interface CBOMClusterInfoQueryObjectGrid extends QueryObjectGrid {
  /**
   *
   * @type {Array<number>}
   * @memberof CBOMQueryDto
   */
  cnfCapacityId?: Array<number>;
  /**
   *
   * @type {Array<number>}
   * @memberof CBOMQueryDto
   */
  financialYear?: Array<number>;
  /**
   *
   * @type {Array<number>}
   * @memberof CBOMQueryDto
   */
  noOfCnfInstancesPersite?: Array<number>;
  /**
   *
   * @type {Array<number>}
   * @memberof CBOMQueryDto
   */
  numberOfPodsPerPodType?: Array<number>;
  /**
   *
   * @type {Array<number>}
   * @memberof CBOMQueryDto
   */
  vcpuRequestForPodType?: Array<number>;
  /**
   *
   * @type {Array<string>}
   * @memberof CBOMQueryDto
   */
  memRequestForPodType?: Array<string>;
  /**
   * @type {Array<string>}
   * @memberof CBOMQueryDto
   */
  nonPresistentStorageForProdType?: Array<string>;
  /**
   * @type {Array<string>}
   * @memberof CBOMQueryDto
   */
  isPresistentVolumesRequired?: Array<string>;
  /**
   * @type {Array<string>}
   * @memberof CBOMQueryDto
   */
  persistentVolumNeaccessMode?: Array<string>;
  /**
   * @type {Array<string>}
   * @memberof CBOMQueryDto
   */
  persistentStorageForPodType?: Array<string>;
  /**
   * @type {Array<string>}
   * @memberof CBOMQueryDto
   */
  storageIopsForPodType?: Array<string>;
  /**
   * @type {Array<string>}
   * @memberof CBOMQueryDto
   */
  storagerWorkloadDistribution?: Array<string>;
  /**
   * @type {Array<string>}
   * @memberof CBOMQueryDto
   */
  northSouthBandWidthForPodType?: Array<string>;
  /**
   * @type {Array<string>}
   * @memberof CBOMQueryDto
   */
  eastWestBandWidthForPodType?: Array<string>;
  /**
   * @type {Array<string>}
   * @memberof CBOMQueryDto
   */
  specialRequirementPerPodType?: Array<string>;
  /**
   * @type {Array<string>}
   * @memberof CBOMQueryDto
   */
  listOfCapacitySpecialRequirement?: Array<string>;
  /**
   * @type {Array<number>}
   * @memberof CBOMQueryDto
   */
  cnfClusterInfoId?: Array<number>;
  /**
   *
   * @type {Array<string>}
   * @memberof CBOMQueryDto
   */
  cnfClusterName?: Array<string>;
  /**
   * @type {Array<string>}
   * @memberof CBOMQueryDto
   */
  podTypeInfoName?: Array<string>;
  /**
   * @type {Array<string>}
   * @memberof CBOMQueryDto
   */
  opcoName?: Array<string>;
  /**
   * @type {Array<string>}
   * @memberof CBOMQueryDto
   */
  shortLocation?: Array<string>;
  /**
   * @type {Array<string>}
   * @memberof CBOMQueryDto
   */
  locationName?: Array<string>;
  /**
   * @type {Array<string>}
   * @memberof CBOMQueryDto
   */
  functionStandardName?: Array<string>;
  /**
   * @type {Array<string>}
   * @memberof CBOMQueryDto
   */
  priorityId?: Array<string>;
  /**
   * @type {Array<string>}
   * @memberof CBOMQueryDto
   */
  podRoleDescription?: Array<string>;
  /**
   * @type {Array<string>}
   * @memberof CBOMQueryDto
   */
  daemonSetPod?: Array<string>;
  /**
   * @type {Array<string>}
   * @memberof CBOMQueryDto
   */
  intraPodRules?: Array<string>;
  /**
   * @type {Array<string>}
   * @memberof CBOMQueryDto
   */
  interPodRules?: Array<string>;
  /**
   * @type {Array<boolean>}
   * @memberof CBOMQueryDto
   */
  isEnhancedHa?: Array<boolean>;
  /**
   * @type {Array<string>}
   * @memberof CBOMQueryDto
   */
  podTypeQos?: Array<string>;
  /**
   * @type {Array<string>}
   * @memberof CBOMQueryDto
   */
  isPersistanceStorageFlag?: Array<string>;
  /**
   * @type {Array<string>}
   * @memberof CBOMQueryDto
   */
  isProdhPaEnable?: Array<string>;
  /**
   * @type {Array<number>}
   * @memberof CBOMQueryDto
   */
  cnfInfoId?: Array<number>;
  /**
   *
   * @type {Array<string>}
   * @memberof CBOMQueryDto
   */
  cnfNameDescription?: Array<string>;
  /**
   * @type {Array<string>}
   * @memberof CBOMQueryDto
   */
  nodePoolBreakUp?: Array<string>;
  /**
   * @type {Array<string>}
   * @memberof CBOMQueryDto
   */
  specialRequirements?: Array<string>;
  /**
   * @type {Array<string>}
   * @memberof CBOMQueryDto
   */
  hyperThreading?: Array<string>;
  /**
   * @type {Array<string>}
   * @memberof CBOMQueryDto
   */
  overProvisioning?: Array<string>;
  /**
   * @type {Array<string>}
   * @memberof CBOMQueryDto
   */
  workerNodeConfiguration?: Array<string>;
  /**
   * @type {Array<string>}
   * @memberof CBOMQueryDto
   */
  hardware?: Array<string>;
  /**
   * @type {Array<string>}
   * @memberof CBOMQueryDto
   */
  cpuKubelet?: Array<string>;
  /**
   * @type {Array<number>}
   * @memberof CBOMQueryDto
   */
  memKubelet?: Array<number>;
  /**
   *
   * @type {Array<string>}
   * @memberof CBOMQueryDto
   */
  cpuSystem?: Array<string>;
  /**
   * @type {Array<number>}
   * @memberof CBOMQueryDto
   */
  memSystem?: Array<number>;
  /**
   *
   * @type {Array<string>}
   * @memberof CBOMQueryDto
   */
  verticalDomain?: Array<string>;
  /**
   *
   * @type {DateFilter}
   * @memberof SystemTypeQueryDto
   */
  lastModified?: DateFilter;
  /**
   *
   * @type {Array<string>}
   * @memberof SystemTypeQueryDto
   */
  lastModifiedBy?: Array<string>;
  /**
   * @type {Array<string>}
   * @memberof CBOMQueryDto
   */
}

export interface CBOMClusterInstanceCapacityQueryObjectGrid
  extends QueryObjectGrid {}

export interface CBOMEdit {
  CBOMDtoEdit: CBOMDtoUpdate | null;
  ResultDtoEdit: ResultDto | null;
}

export interface CBOMCreate {
  CBOMDtoCreate: CBOMDtoCreate | null;
  ResultDtoCreate: ResultDto | null;
}

export interface CBOMGrid {
  CBOMGridResult: QueryResultDtoOfCBOMDtoGrid | null;
  filter: FilterValueDto[] | null;
}
export interface CBOMClusterInfoGrid {
  CBOMClusterInfoGridResult: QueryResultDtoOfCBOMClusterInfoDtoGrid | null;
  filter: FilterValueDto[] | null;
}

export interface CBOMCnfInstanceAndCapacityGrid {
  CBOMCnfInstanceAndCapacityGridResult: QueryResultDtoOfCBOMCnfInstanceAndCapacityDtoGrid | null;
  filter: FilterValueDto[] | null;
}
export interface CBOMVnfCapacityGrid {
  CBOMCnfCapacityGridResult: QueryResultDtoOfCBOMCnfInstanceAndCapacityDtoGrid | null;
  filter: FilterValueDto[] | null;
}

export const GET_CREATE_CBOM = "GET_CREATE_CBOM";
export const GET_EDIT_CBOM = "GET_EDIT_CBOM";
export const GET_GRID_CBOM = "GET_GRID_CBOM";
export const GET_FILTER_CBOM = "GET_FILTER_CBOM";
export const CREATE_CBOM = "CREATE_CBOM";
export const EDIT_CBOM = "EDIT_CBOM";
export const DELETE_CBOM = "DELETE_CBOM";
export const RESTORE_CBOM = "RESTORE_CBOM";
export const GET_CREATE_CBOM_CLUSTER_INFO = "GET_CREATE_CBOM_CLUSTER_INFO";
export const GET_EDIT_CBOM_CLUSTER_INFO = "GET_EDIT_CBOM_CLUSTER_INFO";
export const GET_GRID_CBOM_CLUSTER_INFO = "GET_GRID_CBOM_CLUSTER_INFO";
export const GET_FILTER_CBOM_CLUSTER_INFO = "GET_FILTER_CBOM_CLUSTER_INFO";
export const CREATE_CBOM_CLUSTER_INFO = "CREATE_CBOM_CLUSTER_INFO";
export const EDIT_CBOM_CLUSTER_INFO = "EDIT_CBOM_CLUSTER_INFO";
export const GET_GRID_CBOM_CNF_INSTANCE_AND_CAPACITY =
  "GET_GRID_CBOM_CNF_INSTANCE_AND_CAPACITY";
export const GET_FILTER_CBOM_CNF_INSTANCE_AND_CAPACITY =
  "GET_FILTER_CBOM_CNF_INSTANCE_AND_CAPACITY";
export const GET_FILTER_CBOM_CNF_CAPACITY = "GET_FILTER_CBOM_CNF_CAPACITY";
export const GET_GRID_CBOM_CNF_CAPACITY = "GET_GRID_CBOM_CNF_CAPACITY";
