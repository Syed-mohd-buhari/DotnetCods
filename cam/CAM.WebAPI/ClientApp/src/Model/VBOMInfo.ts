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
 * @interface VBOMInfoQueryDto
 */
export interface VBOMInfoQueryDto extends QueryObject {
  /**
   *
   * @type {Array<number>}
   * @memberof VBOMInfoQueryDto
   */
  vnfVmCapacityId: Array<number>;
  /**
   *
   * @type {Array<number>}
   * @memberof VBOMInfoQueryDto
   */
  vnfInfoId: Array<number>;
  /**
   *
   * @type {Array<string>}
   * @memberof VBOMInfoQueryDto
   */
  vnfNameDescritpion: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof VBOMInfoQueryDto
   */
  vmTypeNameDescription: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof VBOMInfoQueryDto
   */
  shortLocation: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof VBOMInfoQueryDto
   */
  nsxt: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof VBOMInfoQueryDto
   */
  intraVmType: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof VBOMInfoQueryDto
   */
  interVmType: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof VBOMInfoQueryDto
   */
  vmWorkLoadType: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof VBOMInfoQueryDto
   */
  vmStorageBlockSize: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof VBOMInfoQueryDto
   */
  opCoDescritpion: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof VBOMInfoQueryDto
   */
  noOfVnfInstances: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof VBOMInfoQueryDto
   */
  noOfVmsPerType: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof VBOMInfoQueryDto
   */
  numa: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof VBOMInfoQueryDto
   */
  socket: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof VBOMInfoQueryDto
   */
  financialYear: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof VBOMInfoQueryDto
   */
  vcpuPerVm: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof VBOMInfoQueryDto
   */
  rxTxCpuCount: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof VBOMInfoQueryDto
   */
  ramPerVm: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof VBOMInfoQueryDto
   */
  dataDisk: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof VBOMInfoQueryDto
   */
  osDisk: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof VBOMInfoQueryDto
   */
  iopsRunning: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof VBOMInfoQueryDto
   */
  iopsLoading: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof VBOMInfoQueryDto
   */
  vmWorkLoadDistribution: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof VBOMInfoQueryDto
   */
  northDouthBoundBandWidth: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof VBOMInfoQueryDto
   */
  eastWestBoundBandWidth: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof VBOMInfoQueryDto
   */
  otherRequirements: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof VBOMInfoQueryDto
   */
  backupRequired: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof VBOMInfoQueryDto
   */
  probIngRequired: Array<string>;
}

/**
 *
 * @export
 * @interface VbomCapacityQueryDto
 */
export interface VbomCapacityQueryDto extends QueryObject {
  /**
   *
   * @type {Array<number>}
   * @memberof VbomCapacityQueryDto
   */
  vnfVmCapacityId: Array<number>;
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
  vcpuPerVm: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof VbomCapacityQueryDto
   */
  rxTxCpuCount: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof VbomCapacityQueryDto
   */
  ramPerVm: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof VbomCapacityQueryDto
   */
  dataDisk: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof VbomCapacityQueryDto
   */
  osDisk: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof VbomCapacityQueryDto
   */
  iopsRunning: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof VbomCapacityQueryDto
   */
  iopsLoading: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof VbomCapacityQueryDto
   */
  vmWorkLoadDistribution: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof VbomCapacityQueryDto
   */
  northDouthBoundBandWidth: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof VbomCapacityQueryDto
   */
  eastWestBoundBandWidth: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof VbomCapacityQueryDto
   */
  otherRequirements: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof VbomCapacityQueryDto
   */
  backupRequired: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof VbomCapacityQueryDto
   */
  probIngRequired: Array<string>;
}

/**
 *
 * @export
 * @interface VBOMInfoDtoGrid
 */
export interface VBOMInfoDtoGrid extends GridDtoBase {
  /**
   *
   * @type {number}
   * @memberof VBOMInfoDtoGrid
   */
  vnfVmCapacityId: number;
  /**
   *
   * @type {number}
   * @memberof VBOMInfoDtoGrid
   */
  vnfInfoId: number;
  /**
   *
   * @type {number}
   * @memberof VBOMInfoDtoGrid
   */
  vnfVmInstanceId: number;
  /**
   *
   * @type {string}
   * @memberof VBOMInfoDtoGrid
   */
  vnfNameDescritpion: string;
  /**
   *
   * @type {string}
   * @memberof VBOMInfoDtoGrid
   */
  vmTypeNameDescription: string;
  /**
   *
   * @type {string}
   * @memberof VBOMInfoDtoGrid
   */
  shortLocation: string;
  /**
   *
   * @type {string}
   * @memberof VBOMInfoDtoGrid
   */
  nsxt: string;
  /**
   *
   * @type {string}
   * @memberof VBOMInfoDtoGrid
   */
  intraVmType: string;
  /**
   *
   * @type {string}
   * @memberof VBOMInfoDtoGrid
   */
  interVmType: string;
  /**
   *
   * @type {string}
   * @memberof VBOMInfoDtoGrid
   */
  vmWorkLoadType: string;
  /**
   *
   * @type {string}
   * @memberof VBOMInfoDtoGrid
   */
  vmStorageBlockSize: string;
  /**
   *
   * @type {string}
   * @memberof VBOMInfoDtoGrid
   */
  opCoDescritpion: string;
  /**
   *
   * @type {string}
   * @memberof VBOMInfoDtoGrid
   */
  noOfVnfInstances: string;
  /**
   *
   * @type {string}
   * @memberof VBOMInfoDtoGrid
   */
  noOfVmsPerType: string;
  /**
   *
   * @type {string}
   * @memberof VBOMInfoDtoGrid
   */
  numa: string;
  /**
   *
   * @type {string}
   * @memberof VBOMInfoDtoGrid
   */
  socket: string;
  /**
   *
   * @type {string}
   * @memberof VBOMInfoDtoGrid
   */
  financialYear: string;
  /**
   *
   * @type {string}
   * @memberof VBOMInfoDtoGrid
   */
  vcpuPerVm: string;
  /**
   *
   * @type {string}
   * @memberof VBOMInfoDtoGrid
   */
  rxTxCpuCount: string;
  /**
   *
   * @type {string}
   * @memberof VBOMInfoDtoGrid
   */
  ramPerVm: string;
  /**
   *
   * @type {string}
   * @memberof VBOMInfoDtoGrid
   */
  dataDisk: string;
  /**
   *
   * @type {string}
   * @memberof VBOMInfoDtoGrid
   */
  osDisk: string;
  /**
   *
   * @type {string}
   * @memberof VBOMInfoDtoGrid
   */
  iopsRunning: string;
  /**
   *
   * @type {string}
   * @memberof VBOMInfoDtoGrid
   */
  iopsLoading: string;
  /**
   *
   * @type {string}
   * @memberof VBOMInfoDtoGrid
   */
  vmWorkLoadDistribution: string;
  /**
   *
   * @type {string}
   * @memberof VBOMInfoDtoGrid
   */
  northDouthBoundBandWidth: string;
  /**
   *
   * @type {string}
   * @memberof VBOMInfoDtoGrid
   */
  eastWestBoundBandWidth: string;
  /**
   *
   * @type {string}
   * @memberof VBOMInfoDtoGrid
   */
  otherRequirements: string;
  /**
   *
   * @type {string}
   * @memberof VBOMInfoDtoGrid
   */
  backupRequired: string;
  /**
   *
   * @type {string}
   * @memberof VBOMInfoDtoGrid
   */
  probIngRequired: string;
}

/**
 *
 * @export
 * @interface VBOMClusterInfoDtoGrid
 */
export interface VBOMClusterInfoDtoGrid extends GridDtoBase {
  /**
   *
   * @type {number}
   * @memberof VBOMClusterInfoDtoGrid
   */
  vnfClusterInfoId: number;
  /**
   *
   * @type {number}
   * @memberof VBOMClusterInfoDtoGrid
   */
  vnfInfoId: number;
  /**
   *
   * @type {number}
   * @memberof VBOMClusterInfoDtoGrid
   */
  noOfBlades: number;
  /**
   *
   * @type {number}
   * @memberof VBOMClusterInfoDtoGrid
   */
  opCoId: number;
  /**
   *
   * @type {number}
   * @memberof VBOMClusterInfoDtoGrid
   */
  clusterId: number;
  /**
   *
   * @type {number}
   * @memberof VBOMClusterInfoDtoGrid
   */
  shortLocationId: number;
  /**
   *
   * @type {string}
   * @memberof VBOMClusterInfoDtoGrid
   */
  opCoDescritpion: string;
  /**
   *
   * @type {string}
   * @memberof VBOMClusterInfoDtoGrid
   */
  locationName: string;
  /**
   *
   * @type {string}
   * @memberof VBOMClusterInfoDtoGrid
   */
  siteName: string;
  /**
   *
   * @type {string}
   * @memberof VBOMClusterInfoDtoGrid
   */
  clusterDescription: string;
}

/**
 *
 * @export
 * @interface VBOMVnfInfoAndCapacityDtoGrid
 */
export interface VBOMVnfInfoAndCapacityDtoGrid extends GridDtoBase {
  /**
   *
   * @type {number}
   * @memberof VBOMVnfInfoAndCapacityDtoGrid
   */
  vnfInfoId?: number;
  /**
   *
   * @type {string}
   * @memberof VBOMVnfInfoAndCapacityDtoGrid
   */
  vnfNameDescritpion?: string;
  /**
   *
   * @type {string}
   * @memberof VBOMVnfInfoAndCapacityDtoGrid
   */
  vmTypeNameDescription?: string;
  /**
   *
   * @type {number}
   * @memberof VBOMVnfInfoAndCapacityDtoGrid
   */
  vnfVmtypenameid?: number;
  /**
   *
   * @type {string}
   * @memberof VBOMVnfInfoAndCapacityDtoGrid
   */
  nsxt?: string;
  /**
   *
   * @type {string}
   * @memberof VBOMVnfInfoAndCapacityDtoGrid
   */
  intraVmType?: string;
  /**
   *
   * @type {string}
   * @memberof VBOMVnfInfoAndCapacityDtoGrid
   */
  interVmType?: string;
  /**
   *
   * @type {string}
   * @memberof VBOMVnfInfoAndCapacityDtoGrid
   */
  vmWorkLoadType?: string;
  /**
   *
   * @type {string}
   * @memberof VBOMVnfInfoAndCapacityDtoGrid
   */
  vmStorageBlockSize?: string;
  /**
   *
   * @type {string}
   * @memberof VBOMVnfInfoAndCapacityDtoGrid
   */
  noOfVnfInstances?: string;
  /**
   *
   * @type {string}
   * @memberof VBOMVnfInfoAndCapacityDtoGrid
   */
  noOfVmsPerType?: string;
  /**
   *
   * @type {string}
   * @memberof VBOMVnfInfoAndCapacityDtoGrid
   */
  numa?: string;
  /**
   *
   * @type {string}
   * @memberof VBOMVnfInfoAndCapacityDtoGrid
   */
  socket?: string;
  /**
   *
   * @type {VbomCapacityQueryDto}
   * @memberof VBOMVnfInfoAndCapacityDtoGrid
   */
  _vnfVbomCapacityDtoGrid?: VbomCapacityQueryDto;
}

/**
 *
 * @export
 * @interface VBOMInfoDtoCreate
 */
export interface VBOMInfoDtoCreate extends GridDtoBase {
  /**
   *
   * @type {string}
   * @memberof VBOMInfoDtoCreate
   */
  vnfNameDescritpion?: string;
  /**
   *
   * @type {string}
   * @memberof VBOMInfoDtoCreate
   */
  vmTypeNameDescription?: string;
  /**
   *
   * @type {string}
   * @memberof VBOMInfoDtoCreate
   */
  shortLocation?: string;
  /**
   *
   * @type {string}
   * @memberof VBOMInfoDtoCreate
   */
  nsxt?: string;
  /**
   *
   * @type {string}
   * @memberof VBOMInfoDtoCreate
   */
  intraVmType?: string;
  /**
   *
   * @type {string}
   * @memberof VBOMInfoDtoCreate
   */
  interVmType?: string;
  /**
   *
   * @type {string}
   * @memberof VBOMInfoDtoCreate
   */
  vmWorkLoadType?: string;
  /**
   *
   * @type {string}
   * @memberof VBOMInfoDtoCreate
   */
  vmStorageBlockSize?: string;
  /**
   *
   * @type {string}
   * @memberof VBOMInfoDtoCreate
   */
  opCoDescritpion?: string;
  /**
   *
   * @type {string}
   * @memberof VBOMInfoDtoCreate
   */
  noOfVnfInstances?: string;
  /**
   *
   * @type {string}
   * @memberof VBOMInfoDtoCreate
   */
  noOfVmsPerType?: string;
  /**
   *
   * @type {string}
   * @memberof VBOMInfoDtoCreate
   */
  numa?: string;
  /**
   *
   * @type {string}
   * @memberof VBOMInfoDtoCreate
   */
  socket?: string;
  /**
   *
   * @type {string}
   * @memberof VBOMInfoDtoCreate
   */
  financialYear?: string;
  /**
   *
   * @type {string}
   * @memberof VBOMInfoDtoCreate
   */
  vcpuPerVm?: string;
  /**
   *
   * @type {string}
   * @memberof VBOMInfoDtoCreate
   */
  rxTxCpuCount?: string;
  /**
   *
   * @type {string}
   * @memberof VBOMInfoDtoCreate
   */
  ramPerVm?: string;
  /**
   *
   * @type {string}
   * @memberof VBOMInfoDtoCreate
   */
  dataDisk?: string;
  /**
   *
   * @type {string}
   * @memberof VBOMInfoDtoCreate
   */
  osDisk?: string;
  /**
   *
   * @type {string}
   * @memberof VBOMInfoDtoCreate
   */
  iopsRunning?: string;
  /**
   *
   * @type {string}
   * @memberof VBOMInfoDtoCreate
   */
  iopsLoading?: string;
  /**
   *
   * @type {string}
   * @memberof VBOMInfoDtoCreate
   */
  vmWorkLoadDistribution?: string;
  /**
   *
   * @type {string}
   * @memberof VBOMInfoDtoCreate
   */
  northDouthBoundBandWidth?: string;
  /**
   *
   * @type {string}
   * @memberof VBOMInfoDtoCreate
   */
  eastWestBoundBandWidth?: string;
  /**
   *
   * @type {string}
   * @memberof VBOMInfoDtoCreate
   */
  otherRequirements?: string;
  /**
   *
   * @type {string}
   * @memberof VBOMInfoDtoCreate
   */
  backupRequired?: string;
  /**
   *
   * @type {string}
   * @memberof VBOMInfoDtoCreate
   */
  probIngRequired?: string;

  /**
   *
   * @type {{key: number, text: string}[]}
   * @memberof VBOMInfoDtoCreate
   */
  vnClusterNameResource?: { key: number; text: string }[];
  /**
   *
   * @type {any}
   * @memberof VBOMInfoDtoCreate
   */
  opcoBasedLocationResource?: any;

  /**
   *
   * @type {{key: number, text: string}[]}
   * @memberof VBOMInfoDtoCreate
   */
  vnVmTypeNameResource?: { key: number; text: string }[];
  /**
   *
   * @type {{key: number, text: string}[]}
   * @memberof VBOMInfoDtoCreate
   */
  vnfNameResources?: { key: number; text: string }[];
  /**
   *
   * @type {{value: string, text: string}[]}
   * @memberof VBOMInfoDtoCreate
   */
  intraVmTypeResource?: { value: string; text: string }[];
  /**
   *
   * @type {{value: string, text: string}[]}
   * @memberof VBOMInfoDtoCreate
   */
  interTypeResource?: { value: string; text: string }[];
  /**
   *
   * @type {{value: string, text: string}[]}
   * @memberof VBOMInfoDtoCreate
   */
  vmWorkLoadTypeDetail?: { value: string; text: string }[];

  vnfinfoDetail?: VNFInfoDetail;
}

/**
 *
 * @export
 * @interface VBOMClusterInfoDtoCreate
 */
export interface VBOMClusterInfoDtoCreate extends GridDtoBase {
  /**
   *
   * @type {{key: number, text: string}[]}
   * @memberof VBOMClusterInfoDtoCreate
   */
  vnClusterNameResource?: { key: number; text: string }[];
  /**
   *
   * @type {{key: number, text: string}[]}
   * @memberof VBOMClusterInfoDtoCreate
   */
  hardwareTypeResource?: { key: number; text: string }[];
  /**
   *
   * @type {any}
   * @memberof VBOMClusterInfoDtoCreate
   */
  opcoBasedLocationResource?: any;

  /**
   *
   * @type {{key: number, text: string}[]}
   * @memberof VBOMClusterInfoDtoCreate
   */
  vnVmTypeNameResource?: { key: number; text: string }[];
  /**
   *
   * @type {{key: number, text: string}[]}
   * @memberof VBOMClusterInfoDtoCreate
   */
  vnfNameResources?: { key: number; text: string }[];
  /**
   *
   * @type {{value: string, text: string}[]}
   * @memberof VBOMClusterInfoDtoCreate
   */
  financialVersion?: { value: string; text: string }[];
  /**
   *
   * @type {{value: string, text: string}[]}
   * @memberof VBOMClusterInfoDtoCreate
   */
  interTypeResource?: { value: string; text: string }[];
  /**
   *
   * @type {{value: string, text: string}[]}
   * @memberof VBOMClusterInfoDtoCreate
   */
  intraVmTypeResource?: { value: string; text: string }[];
  /**
   *
   * @type {{value: string, text: string}[]}
   * @memberof VBOMClusterInfoDtoCreate
   */
  vmWorkLoadTypeDetail?: { value: string; text: string }[];
  /**
   *
   * @type {VNFClusterInfoDetail}
   * @memberof VBOMClusterInfoDtoCreate
   */
  _VnfClusterInfoDto?: VNFClusterInfoDetail;
}

/**
 *
 * @export
 * @interface VBOMClusterInfo
 */
export interface VBOMClusterInfo extends GridDtoBase {
  /**
   *
   * @type {number | null}
   * @memberof VBOMClusterInfo
   */
  opcoid: number | null;
  /**
   *
   * @type {number | null}
   * @memberof VBOMClusterInfo
   */
  clusternameid: number | null;
  /**
   *
   * @type {number | null}
   * @memberof VBOMClusterInfo
   */
  locationid: number | null;
  /**
   *
   * @type {string}
   * @memberof VBOMClusterInfo
   */
  clustername?: string;
  /**
   *
   * @type {string}
   * @memberof VBOMClusterInfo
   */
  opco?: string;
  /**
   *
   * @type {string}
   * @memberof VBOMClusterInfo
   */
  location?: string;
  /**
   *
   * @type {number | null}
   * @memberof VBOMClusterInfo
   */
  noofblades: number | null;
  /**
   *
   * @type {Array<VNFVMClusterInfo>}
   * @memberof VBOMClusterInfo
   */
  vnfinfo: Array<VNFVMClusterInfo>;
}
/**
 *
 * @export
 * @interface VNFVMClusterInfo
 */
export interface VNFVMClusterInfo {
  /**
   *
   * @type {number | null}
   * @memberof VNFVMClusterInfo
   */
  vnfinfoid: number | null;
  /**
   *
   * @type {number | null}
   * @memberof VNFVMClusterInfo
   */
  vnfvmtypenameid: number | null;
  /**
   *
   * @type {number | null}
   * @memberof VNFVMClusterInfo
   */
  vnfnameid: number | null;
  /**
   *
   * @type {number | null}
   * @memberof VNFVMClusterInfo
   */
  vnfclusterinfoid: number | null;
  /**
   *
   * @type {boolean | string | null}
   * @memberof VNFVMClusterInfo
   */
  numa: boolean | string | null;
  /**
   *
   * @type {boolean | string}
   * @memberof VNFVMClusterInfo
   */
  nsxt?: boolean | string;
  /**
   *
   * @type {string}
   * @memberof VNFVMClusterInfo
   */
  socket: string;
  /**
   *
   * @type {string}
   * @memberof VNFVMClusterInfo
   */
  vmworkloadtype?: string;
  /**
   *
   * @type {number | null}
   * @memberof VNFVMClusterInfo
   */
  vmworkloadtypeid?: number | null;
  /**
   *
   * @type {string}
   * @memberof VNFVMClusterInfo
   */
  vmstorageblocksize?: string;
  /**
   *
   * @type {number | null}
   * @memberof VNFVMClusterInfo
   */
  intervmtypeid?: number | null;
  /**
   *
   * @type {number | null}
   * @memberof VNFVMClusterInfo
   */
  intravmtypeid?: number | null;
  /**
   *
   * @type {string}
   * @memberof VNFVMClusterInfo
   */
  intervmtype?: string;
  /**
   *
   * @type {string}
   * @memberof VNFVMClusterInfo
   */
  intravmtype?: string;
  /**
   *
   * @type {string}
   * @memberof VNFVMClusterInfo
   */
  vnfclusterinfo?: string;
  /**
   *
   * @type {string}
   * @memberof VNFVMClusterInfo
   */
  vnfname?: string;
  /**
   *
   * @type {string}
   * @memberof VNFVMClusterInfo
   */
  vnfvmtypename?: string;
  /**
   *
   * @type {Array<VNFVMClusterCapacity>}
   * @memberof VNFInfoDetail
   */
  vnfvmcapacity: Array<VNFVMClusterCapacity>;
}

/**
 *
 * @export
 * @interface VNFClusterInfoDetail
 */
export interface VNFClusterInfoDetail {
  /**
   *
   * @type {number}
   * @memberof VNFClusterInfoDetail
   */
  vnfclusterinfoid?: number;
  /**
   *
   * @type {number}
   * @memberof VNFClusterInfoDetail
   */
  clusternameid?: number;
  /**
   *
   * @type {string}
   * @memberof VNFClusterInfoDetail
   */
  clustername?: string;
  /**
   *
   * @type {string}
   * @memberof VNFClusterInfoDetail
   */
  hardwaretype: string;
  /**
   *
   * @type {number | null}
   * @memberof VNFClusterInfoDetail
   */
  opcoid: number | null;
  /**
   *
   * @type {number | null}
   * @memberof VNFClusterInfoDetail
   */
  hardwaretypeid: number | null;
  /**
   *
   * @type {string}
   * @memberof VNFClusterInfoDetail
   */
  opco?: string;
  /**
   *
   * @type {number | null}
   * @memberof VNFClusterInfoDetail
   */
  locationid: number | null;
  /**
   *
   * @type {string}
   * @memberof VNFClusterInfoDetail
   */
  location?: string;
  /**
   *
   * @type {number | string}
   * @memberof VNFClusterInfoDetail
   */
  noofblades: number | string;
  /**
   *
   * @type {number | string}
   * @memberof VNFClusterInfoDetail
   */
  revision: number | string;
  /**
   *
   * @type {string}
   * @memberof VNFClusterInfoDetail
   */
  filename?: string;
  /**
   *
   * @type {Array<VNFVMClusterInfo>}
   * @memberof VNFInfoDetail
   */
  vnfinfo: Array<VNFVMClusterInfo>;
}

/**
 *
 * @export
 * @interface VNFInfoDetail
 */
export interface VNFInfoDetail {
  /**
   *
   * @type {number}
   * @memberof VNFInfoDetail
   */
  vnfnameid?: number;
  /**
   *
   * @type {number}
   * @memberof VNFInfoDetail
   */
  clusterid?: number;
  /**
   *
   * @type {number}
   * @memberof VNFInfoDetail
   */
  vmtypenameid?: number;
  /**
   *
   * @type {boolean | string}
   * @memberof VNFInfoDetail
   */
  nsxt?: boolean | string;
  /**
   *
   * @type {string}
   * @memberof VNFInfoDetail
   */
  vmworkloadtype?: string;
  /**
   *
   * @type {string}
   * @memberof VNFInfoDetail
   */
  vmstorageblocksize?: string;
  /**
   *
   * @type {string}
   * @memberof VNFInfoDetail
   */
  intervmtype?: string;
  /**
   *
   * @type {string}
   * @memberof VNFInfoDetail
   */
  intravmtype?: string;
  /**
   *
   * @type {Array<VNFVMInstances>}
   * @memberof VNFInfoDetail
   */
  vnfvminstances: Array<VNFVMInstances>;
}

/**
 *
 * @export
 * @interface VNFVMInstances
 */
export interface VNFVMInstances {
  /**
   *
   * @type {number | null}
   * @memberof VNFVMInstances
   */
  opcoid: number | null;
  /**
   *
   * @type {number | null}
   * @memberof VNFVMInstances
   */
  hardwaretypeid: number | null;
  /**
   *
   * @type {number | null}
   * @memberof VNFVMInstances
   */
  locationid: number | null;
  /**
   *
   * @type {string}
   * @memberof VNFVMInstances
   */
  opco?: string;
  /**
   *
   * @type {string}
   * @memberof VNFVMInstances
   */
  location?: string;
  /**
   *
   * @type {number | null}
   * @memberof VNFVMInstances
   */
  noofvnfinstances: number | null;
  /**
   *
   * @type {number | null}
   * @memberof VNFVMInstances
   */
  noofvmspertype: number | null;
  /**
   *
   * @type {boolean | string | null}
   * @memberof VNFVMInstances
   */
  numa: boolean | string | null;
  /**
   *
   * @type {string}
   * @memberof VNFVMInstances
   */
  socket: string;
  /**
   *
   * @type {Array<VNFVMCapacity>}
   * @memberof VNFVMInstances
   */
  vnfvmcapacity: Array<VNFVMCapacity>;
  /**
   *
   * @type {number | null}
   * @memberof VNFVMInstances
   */
  vnfvminstanceid?: number | null;
}

/**
 *
 * @export
 * @interface VNFVMCapacity
 */
export interface VNFVMCapacity {
  /**
   *
   * @type {number}
   * @memberof VNFVMCapacity
   */
  financialyear: number;
  /**
   *
   * @type {number}
   * @memberof VNFVMCapacity
   */
  vcpupervm: number;
  /**
   *
   * @type {boolean}
   * @memberof VNFVMCapacity
   */
  rxtxcpucount: boolean;
  /**
   *
   * @type {number}
   * @memberof VNFVMCapacity
   */
  rampervm: number;
  /**
   *
   * @type {string}
   * @memberof VNFVMCapacity
   */
  datadisk: string;
  /**
   *
   * @type {number}
   * @memberof VNFVMCapacity
   */
  osdisk: number;
  /**
   *
   * @type {string}
   * @memberof VNFVMCapacity
   */
  iopsrunning: string;
  /**
   *
   * @type {string}
   * @memberof VNFVMCapacity
   */
  iopsloading: string;
  /**
   *
   * @type {string}
   * @memberof VNFVMCapacity
   */
  vmworkloaddistribution: string;
  /**
   *
   * @type {string}
   * @memberof VNFVMCapacity
   */
  northsouthboundbandwidth: string;
  /**
   *
   * @type {string}
   * @memberof VNFVMCapacity
   */
  eastwestboundbandwidth: string;
  /**
   *
   * @type {string}
   * @memberof VNFVMCapacity
   */
  otherrequirements: string;
  /**
   *
   * @type {boolean}
   * @memberof VNFVMCapacity
   */
  backuprequired: boolean;
  /**
   *
   * @type {boolean}
   * @memberof VNFVMCapacity
   */
  probingrequired: boolean;
  /**
   *
   * @type {nummber}
   * @memberof VNFVMCapacity
   */
  vnfvmcapacityid?: number | null;
  /**
   *
   * @type {nummber}
   * @memberof VNFVMCapacity
   */
  vnfvminstanceid?: number | null;
}

/**
 *
 * @export
 * @interface VNFVMClusterCapacity
 */
export interface VNFVMClusterCapacity {
  /**
   *
   * @type {number}
   * @memberof VNFVMClusterCapacity
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
   * @memberof VNFVMClusterCapacity
   */
  vnfcpupervm: number;
  /**
   *
   * @type {boolean}
   * @memberof VNFVMClusterCapacity
   */
  rxtxcpucount: boolean;
  /**
   *
   * @type {number}
   * @memberof VNFVMClusterCapacity
   */
  rampervm: number;
  /**
   *
   * @type {number | string | null}
   * @memberof VNFVMClusterCapacity
   */
  noofvnfinstances: number | string | null;
  /**
   *
   * @type {number | string | null}
   * @memberof VNFVMClusterCapacity
   */
  noofvmspertype: number | string | null;
  /**
   *
   * @type {string}
   * @memberof VNFVMClusterCapacity
   */
  datadisk: string;
  /**
   *
   * @type {number}
   * @memberof VNFVMClusterCapacity
   */
  osdisk: number;
  /**
   *
   * @type {string}
   * @memberof VNFVMClusterCapacity
   */
  iopsrunning: string;
  /**
   *
   * @type {string}
   * @memberof VNFVMClusterCapacity
   */
  iopsloading: string;
  /**
   *
   * @type {string}
   * @memberof VNFVMClusterCapacity
   */
  vmworkloaddistribution: string;
  /**
   *
   * @type {string}
   * @memberof VNFVMClusterCapacity
   */
  northsouthboundbandwidth: string;
  /**
   *
   * @type {string}
   * @memberof VNFVMClusterCapacity
   */
  eastwestboundbandwidth: string;
  /**
   *
   * @type {string}
   * @memberof VNFVMClusterCapacity
   */
  otherrequirements: string;
  /**
   *
   * @type {boolean}
   * @memberof VNFVMClusterCapacity
   */
  backuprequired: boolean;
  /**
   *
   * @type {boolean}
   * @memberof VNFVMClusterCapacity
   */
  probingrequired: boolean;
  /**
   *
   * @type {nummber}
   * @memberof VNFVMClusterCapacity
   */
  vnfvmcapacityid?: number | null;
  /**
   *
   * @type {nummber}
   * @memberof VNFVMClusterCapacity
   */
  vnfinfoid?: number | null;
}

/**
 *
 * @export
 * @interface VBOMInfoDtoUpdate
 */
export interface VBOMInfoDtoUpdate extends VBOMInfoDtoCreate {
  /**
   *
   * @type {number}
   * @memberof VBOMInfoDtoUpdate
   */
  vnfVmCapacityId?: number;
}

/**
 *
 * @export
 * @interface VBOMClusterInfoDtoUpdate
 */
export interface VBOMClusterInfoDtoUpdate extends VBOMClusterInfoDtoCreate {
  /**
   *
   * @type {number}
   * @memberof VBOMClusterInfoDtoUpdate
   */
  vnfclusterinfoid?: number;
}

/**
 *
 * @export
 * @interface QueryResultDtoOfVBOMInfoDtoGrid
 */
export interface QueryResultDtoOfVBOMInfoDtoGrid {
  /**
   *
   * @type {number}
   * @memberof QueryResultDtoOfVBOMInfoDtoGrid
   */
  totalItems?: number;
  /**
   *
   * @type {Array<VBOMInfoDtoGrid>}
   * @memberof QueryResultDtoOfVBOMInfoDtoGrid
   */
  items?: Array<VBOMInfoDtoGrid>;
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
 * @interface QueryResultDtoOfVBOMClusterInfoDtoGrid
 */
export interface QueryResultDtoOfVBOMClusterInfoDtoGrid {
  /**
   *
   * @type {number}
   * @memberof QueryResultDtoOfVBOMClusterInfoDtoGrid
   */
  totalItems?: number;
  /**
   *
   * @type {Array<VBOMClusterInfoDtoGrid>}
   * @memberof QueryResultDtoOfVBOMClusterInfoDtoGrid
   */
  items?: Array<VBOMClusterInfoDtoGrid>;
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
 * @interface QueryResultDtoOfVBOMVnfInfoAndCapacityDtoGrid
 */
export interface QueryResultDtoOfVBOMVnfInfoAndCapacityDtoGrid {
  /**
   *
   * @type {number}
   * @memberof QueryResultDtoOfVBOMVnfInfoAndCapacityDtoGrid
   */
  totalItems?: number;
  /**
   *
   * @type {Array<VBOMVnfInfoAndCapacityDtoGrid>}
   * @memberof QueryResultDtoOfVBOMVnfInfoAndCapacityDtoGrid
   */
  items?: Array<VBOMVnfInfoAndCapacityDtoGrid>;
  /**
   *
   * @type {CustomGridRender}
   * @memberof CustomGridRender
   */
  gridRender?: CustomGridRender;
}

export interface VBOMInfoQueryObjectGrid extends QueryObjectGrid {
  /**
   *
   * @type {Array<number>}
   * @memberof VBOMInfoQueryDto
   */
  vnfVmCapacityId?: Array<number>;
  /**
   *
   * @type {Array<number>}
   * @memberof VBOMInfoQueryDto
   */
  vnfInfoId?: Array<number>;
  vnfNameDescritpion?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof VBOMInfoQueryDto
   */

  /**
   *
   * @type {Array<string>}
   * @memberof VBOMInfoQueryDto
   */
  vmTypeNameDescription?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof VBOMInfoQueryDto
   */
  shortLocation?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof VBOMInfoQueryDto
   */
  nsxt?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof VBOMInfoQueryDto
   */
  intraVmType?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof VBOMInfoQueryDto
   */
  interVmType?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof VBOMInfoQueryDto
   */
  vmWorkLoadType?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof VBOMInfoQueryDto
   */
  vmStorageBlockSize?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof VBOMInfoQueryDto
   */
  opCoDescritpion?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof VBOMInfoQueryDto
   */
  hardwareType?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof VBOMInfoQueryDto
   */
  noOfVnfInstances?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof VBOMInfoQueryDto
   */
  noOfVmsPerType?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof VBOMInfoQueryDto
   */
  numa?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof VBOMInfoQueryDto
   */
  socket?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof VBOMInfoQueryDto
   */
  financialYear?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof VBOMInfoQueryDto
   */
  vcpuPerVm?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof VBOMInfoQueryDto
   */
  rxTxCpuCount?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof VBOMInfoQueryDto
   */
  ramPerVm?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof VBOMInfoQueryDto
   */
  dataDisk?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof VBOMInfoQueryDto
   */
  osDisk?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof VBOMInfoQueryDto
   */
  iopsRunning?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof VBOMInfoQueryDto
   */
  iopsLoading?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof VBOMInfoQueryDto
   */
  vmWorkLoadDistribution?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof VBOMInfoQueryDto
   */
  northDouthBoundBandWidth?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof VBOMInfoQueryDto
   */
  eastWestBoundBandWidth?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof VBOMInfoQueryDto
   */
  otherRequirements?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof VBOMInfoQueryDto
   */
  backupRequired?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof VBOMInfoQueryDto
   */
  probIngRequired?: Array<string>;
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

export interface VBOMClusterInfoQueryObjectGrid extends QueryObjectGrid {
  /**
   *
   * @type {Array<number>}
   * @memberof VBOMInfoQueryDto
   */
  vnfClusterInfoId?: Array<number>;
  /**
   *
   * @type {Array<number>}
   * @memberof VBOMInfoQueryDto
   */
  vnfInfoId?: Array<number>;
  /**
   *
   * @type {Array<number>}
   * @memberof VBOMInfoQueryDto
   */
  noOfBlades?: Array<number>;
  /**
   *
   * @type {Array<number>}
   * @memberof VBOMInfoQueryDto
   */
  opCoId?: Array<number>;
  /**
   *
   * @type {Array<number>}
   * @memberof VBOMInfoQueryDto
   */
  vnfNameDescritpion?: Array<string>;
  clusterId?: Array<number>;
  /**
   *
   * @type {Array<number>}
   * @memberof VBOMInfoQueryDto
   */
  shortLocationId?: Array<number>;
  /**
   *
   * @type {Array<string>}
   * @memberof VBOMInfoQueryDto
   */
  opCoDescritpion?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof VBOMInfoQueryDto
   */
  hardwareType?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof VBOMInfoQueryDto
   */
  locationName?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof VBOMInfoQueryDto
   */
  siteName?: Array<string>;
  /**
   *
   * @type {Array<string>}
   * @memberof VBOMInfoQueryDto
   */
  clusterDescription?: Array<string>;
  /**
   *
   * @type {DateFilter}
   * @memberof VBOMInfoQueryDto
   */
  lastModified?: DateFilter;
  /**
   *
   * @type {Array<string>}
   * @memberof VBOMInfoQueryDto
   */
  lastModifiedBy?: Array<string>;
}

export interface VBOMClusterInfoEdit {
  VBOMClusterInfoDtoEdit: VBOMClusterInfoDtoUpdate | null;
  ResultDtoEdit: ResultDto | null;
}

export interface VBOMClusterInfoCreate {
  VBOMClusterInfoDtoCreate: VBOMClusterInfoDtoCreate | null;
  ResultDtoCreate: ResultDto | null;
}

export interface VBOMInfoEdit {
  VBOMInfoDtoEdit: VBOMInfoDtoUpdate | null;
  ResultDtoEdit: ResultDto | null;
}

export interface VBOMInfoCreate {
  VBOMInfoDtoCreate: VBOMInfoDtoCreate | null;
  ResultDtoCreate: ResultDto | null;
}

export interface VBOMInfoGrid {
  VBOMInfoGridResult: QueryResultDtoOfVBOMInfoDtoGrid | null;
  filter: FilterValueDto[] | null;
}

export interface VBOMClusterInfoGrid {
  VBOMClusterInfoGridResult: QueryResultDtoOfVBOMClusterInfoDtoGrid | null;
  filter: FilterValueDto[] | null;
}

export interface VBOMVnfInfoAndCapacityGrid {
  VBOMVnfInfoAndCapacityGridResult: QueryResultDtoOfVBOMVnfInfoAndCapacityDtoGrid | null;
  filter: FilterValueDto[] | null;
}

export interface VBOMVnfInstanceAndCapacityGrid {
  VBOMVnfInstanceAndCapacityGridResult: QueryResultDtoOfVBOMVnfInfoAndCapacityDtoGrid | null;
  filter: FilterValueDto[] | null;
}
export interface VBOMVnfCapacityGrid {
  VBOMVnfCapacityGridResult: QueryResultDtoOfVBOMVnfInfoAndCapacityDtoGrid | null;
  filter: FilterValueDto[] | null;
}

export const GET_CREATE_VBOM_INFO = "GET_CREATE_VBOM_INFO";
export const GET_EDIT_VBOM_INFO = "GET_EDIT_VBOM_INFO";
export const GET_GRID_VBOM_INFO = "GET_GRID_VBOM_INFO";
export const GET_CREATE_VBOM_CLUSTER_INFO = "GET_CREATE_VBOM_CLUSTER_INFO";
export const GET_EDIT_VBOM_CLUSTER_INFO = "GET_EDIT_VBOM_INFO";
export const GET_GRID_VBOM_CLUSTER_INFO = "GET_GRID_VBOM_CLUSTER_INFO";
export const GET_FILTER_VBOM_CLUSTER_INFO = "GET_FILTER_VBOM_CLUSTER_INFO";
export const GET_GRID_VBOM_VNF_INFO_AND_CAPACITY =
  "GET_GRID_VBOM_VNF_INFO_AND_CAPACITY";
export const GET_FILTER_VBOM_VNF_INFO_AND_CAPACITY =
  "GET_FILTER_VBOM_VNF_INFO_AND_CAPACITY";
export const GET_FILTER_VBOM_INFO = "GET_FILTER_VBOM_INFO";
export const CREATE_VBOM_INFO = "CREATE_VBOM_INFO";
export const EDIT_VBOM_CLUSTER_INFO = "EDIT_VBOM_CLUSTER_INFO";
export const CREATE_VBOM_CLUSTER_INFO = "CREATE_VBOM_CLUSTER_INFO";
export const EDIT_VBOM_INFO = "EDIT_VBOM_INFO";
export const DELETE_VBOM_INFO = "DELETE_VBOM_INFO";
export const RESTORE_VBOM_INFO = "RESTORE_VBOM_INFO";
export const GET_GRID_VBOM_VNF_INSTANCE_AND_CAPACITY =
  "GET_GRID_VBOM_VNF_INSTANCE_AND_CAPACITY";
export const GET_FILTER_VBOM_VNF_INSTANCE_AND_CAPACITY =
  "GET_FILTER_VBOM_VNF_INSTANCE_AND_CAPACITY";
export const GET_FILTER_VBOM_VNF_CAPACITY = "GET_FILTER_VBOM_VNF_CAPACITY";
export const GET_GRID_VBOM_VNF_CAPACITY = "GET_GRID_VBOM_VNF_CAPACITY";
