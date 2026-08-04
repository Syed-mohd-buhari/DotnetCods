import * as url from "url";
import * as isomorphicFetch from "isomorphic-fetch";
import { Configuration } from "./Common/configuration";
import {
  DataRemediationDto,
  ResultDto,
  ResultDtoOfResultDataRemediationDto,
} from "../Model/CommonModels";
import { headerObj } from "./header";

import {
  BASE_PATH,
  FetchArgs,
  FetchAPI,
  BaseAPI,
  RequiredError,
  FilterValueDto,
} from "./Common/CommonBusiness";
import {
  ConstraintInfoDto,
  LifecycleConstraintDto,
  QueryResultDtoOfSystemTypeDtoGrid,
  QueryResultDtoOfSystemTypeDtoGrouped,
  ResultDtoOfSystemTypeReleatedMajorEntity,
  SystemTypeDto,
  SystemTypeDtoCreate,
  SystemTypeDtoUpdate,
} from "../Model/SystemTypeModel";
import { ReturnFile } from "../Model/Common";
import { SystemTypeQueryObjectGrid as SystemTypeQueryDto } from "../Model/SystemTypeModel";
/**
 * SystemTypeApi - fetch parameter creator
 * @export
 */
export const SystemTypeApiFetchParamCreator = function (
  configuration?: Configuration
) {
  return {
    /**
     *
     * @param {DataRemediationDto} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    systemTypeApplyDataRemediation(
      body: DataRemediationDto,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling systemTypeApplyDataRemediation."
        );
      }
      const localVarPath = `/api/SystemType/ApplyDataRemediation`;
      const localVarUrlObj = url.parse(localVarPath, true);
      const localVarRequestOptions = Object.assign({ method: "PUT" }, options);
      const localVarHeaderParameter = { ...headerObj } as any;
      const localVarQueryParameter = {} as any;

      // authentication JWT required
      if (configuration && configuration.apiKey) {
        const localVarApiKeyValue =
          typeof configuration.apiKey === "function"
            ? configuration.apiKey("Authorization")
            : configuration.apiKey;
        localVarHeaderParameter["Authorization"] = localVarApiKeyValue;
      }

      localVarHeaderParameter["Content-Type"] = "application/json";

      localVarUrlObj.query = Object.assign(
        {},
        localVarUrlObj.query,
        localVarQueryParameter,
        options.query
      );
      // fix override query string Detail: https://stackoverflow.com/a/7517673/1077943
      localVarUrlObj.search = null;
      localVarRequestOptions.headers = Object.assign(
        {},
        localVarHeaderParameter,
        options.headers
      );
      const needsSerialization =
        <any>"DataRemediationDto" !== "string" ||
        localVarRequestOptions.headers["Content-Type"] === "application/json";
      localVarRequestOptions.body = needsSerialization
        ? JSON.stringify(body || {})
        : body || "";

      return {
        url: url.format(localVarUrlObj),
        options: localVarRequestOptions,
      };
    },
    /**
     *
     * @param {SystemTypeDtoCreate} body
     * @param {boolean} [forced]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    systemTypeCreate(
      body: SystemTypeDtoCreate,
      forced?: boolean,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling systemTypeCreate."
        );
      }
      const localVarPath = `/api/SystemType`;
      const localVarUrlObj = url.parse(localVarPath, true);
      const localVarRequestOptions = Object.assign({ method: "POST" }, options);
      const localVarHeaderParameter = { ...headerObj } as any;
      const localVarQueryParameter = {} as any;

      // authentication JWT required
      if (configuration && configuration.apiKey) {
        const localVarApiKeyValue =
          typeof configuration.apiKey === "function"
            ? configuration.apiKey("Authorization")
            : configuration.apiKey;
        localVarHeaderParameter["Authorization"] = localVarApiKeyValue;
      }

      if (forced !== undefined) {
        localVarQueryParameter["forced"] = forced;
      }

      localVarHeaderParameter["Content-Type"] = "application/json";

      localVarUrlObj.query = Object.assign(
        {},
        localVarUrlObj.query,
        localVarQueryParameter,
        options.query
      );
      // fix override query string Detail: https://stackoverflow.com/a/7517673/1077943
      localVarUrlObj.search = null;
      localVarRequestOptions.headers = Object.assign(
        {},
        localVarHeaderParameter,
        options.headers
      );
      const needsSerialization =
        <any>"SystemTypeDtoCreate" !== "string" ||
        localVarRequestOptions.headers["Content-Type"] === "application/json";
      localVarRequestOptions.body = needsSerialization
        ? JSON.stringify(body || {})
        : body || "";

      return {
        url: url.format(localVarUrlObj),
        options: localVarRequestOptions,
      };
    },
    /**
     *
     * @param {number} [id]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    systemTypeDelete(id?: number, options: any = {}): FetchArgs {
      const localVarPath = `/api/SystemType/Delete`;
      const localVarUrlObj = url.parse(localVarPath, true);
      const localVarRequestOptions = Object.assign(
        { method: "DELETE" },
        options
      );
      const localVarHeaderParameter = { ...headerObj } as any;
      const localVarQueryParameter = {} as any;

      // authentication JWT required
      if (configuration && configuration.apiKey) {
        const localVarApiKeyValue =
          typeof configuration.apiKey === "function"
            ? configuration.apiKey("Authorization")
            : configuration.apiKey;
        localVarHeaderParameter["Authorization"] = localVarApiKeyValue;
      }

      if (id !== undefined) {
        localVarQueryParameter["id"] = id;
      }

      localVarUrlObj.query = Object.assign(
        {},
        localVarUrlObj.query,
        localVarQueryParameter,
        options.query
      );
      // fix override query string Detail: https://stackoverflow.com/a/7517673/1077943
      localVarUrlObj.search = null;
      localVarRequestOptions.headers = Object.assign(
        {},
        localVarHeaderParameter,
        options.headers
      );

      return {
        url: url.format(localVarUrlObj),
        options: localVarRequestOptions,
      };
    },
    /**
     *
     * @param {number} [id]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    systemTypeDeleteDeep(id?: number, options: any = {}): FetchArgs {
      const localVarPath = `/api/SystemType/DeleteDeep`;
      const localVarUrlObj = url.parse(localVarPath, true);
      const localVarRequestOptions = Object.assign(
        { method: "DELETE" },
        options
      );
      const localVarHeaderParameter = { ...headerObj } as any;
      const localVarQueryParameter = {} as any;

      // authentication JWT required
      if (configuration && configuration.apiKey) {
        const localVarApiKeyValue =
          typeof configuration.apiKey === "function"
            ? configuration.apiKey("Authorization")
            : configuration.apiKey;
        localVarHeaderParameter["Authorization"] = localVarApiKeyValue;
      }

      if (id !== undefined) {
        localVarQueryParameter["id"] = id;
      }

      localVarUrlObj.query = Object.assign(
        {},
        localVarUrlObj.query,
        localVarQueryParameter,
        options.query
      );
      // fix override query string Detail: https://stackoverflow.com/a/7517673/1077943
      localVarUrlObj.search = null;
      localVarRequestOptions.headers = Object.assign(
        {},
        localVarHeaderParameter,
        options.headers
      );

      return {
        url: url.format(localVarUrlObj),
        options: localVarRequestOptions,
      };
    },
    /**
     *
     * @param {SystemTypeQueryDto} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    systemTypeExportReport(
      body: SystemTypeQueryDto,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling systemTypeExportReport."
        );
      }
      const localVarPath = `/api/SystemType/ExportReport`;
      const localVarUrlObj = url.parse(localVarPath, true);
      const localVarRequestOptions = Object.assign({ method: "POST" }, options);
      const localVarHeaderParameter = { ...headerObj } as any;
      const localVarQueryParameter = {} as any;

      // authentication JWT required
      if (configuration && configuration.apiKey) {
        const localVarApiKeyValue =
          typeof configuration.apiKey === "function"
            ? configuration.apiKey("Authorization")
            : configuration.apiKey;
        localVarHeaderParameter["Authorization"] = localVarApiKeyValue;
      }

      localVarHeaderParameter["Content-Type"] = "application/json";

      localVarUrlObj.query = Object.assign(
        {},
        localVarUrlObj.query,
        localVarQueryParameter,
        options.query
      );
      // fix override query string Detail: https://stackoverflow.com/a/7517673/1077943
      localVarUrlObj.search = null;
      localVarRequestOptions.headers = Object.assign(
        {},
        localVarHeaderParameter,
        options.headers
      );
      const needsSerialization =
        <any>"SystemTypeQueryDto" !== "string" ||
        localVarRequestOptions.headers["Content-Type"] === "application/json";
      localVarRequestOptions.body = needsSerialization
        ? JSON.stringify(body || {})
        : body || "";

      return {
        url: url.format(localVarUrlObj),
        options: localVarRequestOptions,
      };
    },
    /**
     *
     * @param {number} [assetCategoryId]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    systemTypeGetAssetCategoryReleated(
      assetCategoryId?: number,
      options: any = {}
    ): FetchArgs {
      const localVarPath = `/api/SystemType/GetAssetCategoryReleated`;
      const localVarUrlObj = url.parse(localVarPath, true);
      const localVarRequestOptions = Object.assign({ method: "GET" }, options);
      const localVarHeaderParameter = { ...headerObj } as any;
      const localVarQueryParameter = {} as any;

      // authentication JWT required
      if (configuration && configuration.apiKey) {
        const localVarApiKeyValue =
          typeof configuration.apiKey === "function"
            ? configuration.apiKey("Authorization")
            : configuration.apiKey;
        localVarHeaderParameter["Authorization"] = localVarApiKeyValue;
      }

      if (assetCategoryId !== undefined) {
        localVarQueryParameter["assetCategoryId"] = assetCategoryId;
      }

      localVarUrlObj.query = Object.assign(
        {},
        localVarUrlObj.query,
        localVarQueryParameter,
        options.query
      );
      // fix override query string Detail: https://stackoverflow.com/a/7517673/1077943
      localVarUrlObj.search = null;
      localVarRequestOptions.headers = Object.assign(
        {},
        localVarHeaderParameter,
        options.headers
      );

      return {
        url: url.format(localVarUrlObj),
        options: localVarRequestOptions,
      };
    },
    /**
     *
     * @param {number} [majorSoftwareBuild]
     * @param {Array<number>} [majorHardwareBuilds]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    systemTypeGetCostraintInfo(
      majorSoftwareBuild?: number,
      majorHardwareBuilds?: Array<number>,
      options: any = {}
    ): FetchArgs {
      const localVarPath = `/api/SystemType/GetCostraintInfo`;
      const localVarUrlObj = url.parse(localVarPath, true);
      const localVarRequestOptions = Object.assign({ method: "GET" }, options);
      const localVarHeaderParameter = { ...headerObj } as any;
      const localVarQueryParameter = {} as any;

      // authentication JWT required
      if (configuration && configuration.apiKey) {
        const localVarApiKeyValue =
          typeof configuration.apiKey === "function"
            ? configuration.apiKey("Authorization")
            : configuration.apiKey;
        localVarHeaderParameter["Authorization"] = localVarApiKeyValue;
      }

      if (majorSoftwareBuild !== undefined) {
        localVarQueryParameter["MajorSoftwareBuild"] = majorSoftwareBuild;
      }

      if (majorHardwareBuilds) {
        localVarQueryParameter["MajorHardwareBuilds"] = majorHardwareBuilds;
      }

      localVarUrlObj.query = Object.assign(
        {},
        localVarUrlObj.query,
        localVarQueryParameter,
        options.query
      );
      // fix override query string Detail: https://stackoverflow.com/a/7517673/1077943
      localVarUrlObj.search = null;
      localVarRequestOptions.headers = Object.assign(
        {},
        localVarHeaderParameter,
        options.headers
      );

      return {
        url: url.format(localVarUrlObj),
        options: localVarRequestOptions,
      };
    },

    systemTypeGetVodafoneNameResource(
      id?: number,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      const localVarPath = `/api/SystemType/GetMajorSoftwareVodafoneName`;
      const localVarUrlObj = url.parse(localVarPath, true);
      const localVarRequestOptions = Object.assign({ method: "GET" }, options);
      const localVarHeaderParameter = { ...headerObj } as any;
      const localVarQueryParameter = {} as any;

      // authentication JWT required
      if (configuration && configuration.apiKey) {
        const localVarApiKeyValue =
          typeof configuration.apiKey === "function"
            ? configuration.apiKey("Authorization")
            : configuration.apiKey;
        localVarHeaderParameter["Authorization"] = localVarApiKeyValue;
      }

      if (id !== undefined) {
        localVarQueryParameter["id"] = id;
      }

      localVarUrlObj.query = Object.assign(
        {},
        localVarUrlObj.query,
        localVarQueryParameter,
        options.query
      );
      // fix override query string Detail: https://stackoverflow.com/a/7517673/1077943
      localVarUrlObj.search = null;
      localVarRequestOptions.headers = Object.assign(
        {},
        localVarHeaderParameter,
        options.headers
      );

      return {
        url: url.format(localVarUrlObj),
        options: localVarRequestOptions,
      };
    },

    systemTypeGetVodafoneNameResourceWizardMode(
      id?: number,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      const localVarPath = `/api/SystemType/GetVfNameOfSwAppType`;
      const localVarUrlObj = url.parse(localVarPath, true);
      const localVarRequestOptions = Object.assign({ method: "GET" }, options);
      const localVarHeaderParameter = { ...headerObj } as any;
      const localVarQueryParameter = {} as any;

      // authentication JWT required
      if (configuration && configuration.apiKey) {
        const localVarApiKeyValue =
          typeof configuration.apiKey === "function"
            ? configuration.apiKey("Authorization")
            : configuration.apiKey;
        localVarHeaderParameter["Authorization"] = localVarApiKeyValue;
      }

      if (id !== undefined) {
        localVarQueryParameter["swAppTypeId"] = id;
      }

      localVarUrlObj.query = Object.assign(
        {},
        localVarUrlObj.query,
        localVarQueryParameter,
        options.query
      );
      // fix override query string Detail: https://stackoverflow.com/a/7517673/1077943
      localVarUrlObj.search = null;
      localVarRequestOptions.headers = Object.assign(
        {},
        localVarHeaderParameter,
        options.headers
      );

      return {
        url: url.format(localVarUrlObj),
        options: localVarRequestOptions,
      };
    },
    /**
     *
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    systemTypeGetCreateResourceSystemType(options: any = {}): FetchArgs {
      const localVarPath = `/api/SystemType/Create`;
      const localVarUrlObj = url.parse(localVarPath, true);
      const localVarRequestOptions = Object.assign({ method: "GET" }, options);
      const localVarHeaderParameter = { ...headerObj } as any;
      const localVarQueryParameter = {} as any;

      // authentication JWT required
      if (configuration && configuration.apiKey) {
        const localVarApiKeyValue =
          typeof configuration.apiKey === "function"
            ? configuration.apiKey("Authorization")
            : configuration.apiKey;
        localVarHeaderParameter["Authorization"] = localVarApiKeyValue;
      }

      localVarUrlObj.query = Object.assign(
        {},
        localVarUrlObj.query,
        localVarQueryParameter,
        options.query
      );
      // fix override query string Detail: https://stackoverflow.com/a/7517673/1077943
      localVarUrlObj.search = null;
      localVarRequestOptions.headers = Object.assign(
        {},
        localVarHeaderParameter,
        options.headers
      );

      return {
        url: url.format(localVarUrlObj),
        options: localVarRequestOptions,
      };
    },
    /**
     *
     * @param {SystemTypeQueryDto} body
     * @param {string} [propertyName]
     * @param {string} [propertyFilter]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    systemTypeGetFilterResult(
      body: SystemTypeQueryDto,
      propertyName?: string,
      propertyFilter?: string,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling systemTypeGetFilterResult."
        );
      }
      const localVarPath = `/api/SystemType/Filter`;
      const localVarUrlObj = url.parse(localVarPath, true);
      const localVarRequestOptions = Object.assign({ method: "POST" }, options);
      const localVarHeaderParameter = { ...headerObj } as any;
      const localVarQueryParameter = {} as any;

      // authentication JWT required
      if (configuration && configuration.apiKey) {
        const localVarApiKeyValue =
          typeof configuration.apiKey === "function"
            ? configuration.apiKey("Authorization")
            : configuration.apiKey;
        localVarHeaderParameter["Authorization"] = localVarApiKeyValue;
      }

      if (propertyName !== undefined) {
        localVarQueryParameter["propertyName"] = propertyName;
      }

      if (propertyFilter !== undefined) {
        localVarQueryParameter["propertyFilter"] = propertyFilter;
      }

      localVarHeaderParameter["Content-Type"] = "application/json";

      localVarUrlObj.query = Object.assign(
        {},
        localVarUrlObj.query,
        localVarQueryParameter,
        options.query
      );
      // fix override query string Detail: https://stackoverflow.com/a/7517673/1077943
      localVarUrlObj.search = null;
      localVarRequestOptions.headers = Object.assign(
        {},
        localVarHeaderParameter,
        options.headers
      );
      const needsSerialization =
        <any>"SystemTypeQueryDto" !== "string" ||
        localVarRequestOptions.headers["Content-Type"] === "application/json";
      localVarRequestOptions.body = needsSerialization
        ? JSON.stringify(body || {})
        : body || "";

      return {
        url: url.format(localVarUrlObj),
        options: localVarRequestOptions,
      };
    },
    /**
     *
     * @param {Array<number>} [systemTypeId]
     * @param {Array<string>} [systemTypeNameVodafone]
     * @param {Array<string>} [systemTypeName3Gpp]
     * @param {Array<string>} [systemTypeNameOem]
     * @param {Array<number>} [majorSoftwareBuild]
     * @param {Array<string>} [majorHardwareBuild]
     * @param {Array<number>} [systemSolution]
     * @param {Date} [constraintScalingStartDate]
     * @param {Date} [constraintScalingEndDate]
     * @param {Array<string>} [constraintLcm]
     * @param {Date} [endOfMaintenanceStartDate]
     * @param {Date} [endOfMaintenanceEndDate]
     * @param {Date} [lastModifiedStartDate]
     * @param {Date} [lastModifiedEndDate]
     * @param {Array<number>} [productImportance]
     * @param {Array<number>} [verticalResponsible]
     * @param {Array<number>} [subDomainResponsible]
     * @param {Array<string>} [subDomainSpoc]
     * @param {Array<number>} [assetCategory]
     * @param {Array<number>} [assetClass]
     * @param {Array<string>} [assetType]
     * @param {Array<string>} [spareFieldsJson]
     * @param {Array<number>} [softwareOem]
     * @param {Array<number>} [hardwareOem]
     * @param {Array<string>} [lastModifiedBy]
     * @param {string} [sortBy]
     * @param {boolean} [isSortAscending]
     * @param {number} [page]
     * @param {number} [pageSize]
     * @param {number} [principalId]
     * @param {boolean} [deleted]
     * @param {boolean} [orphan]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    systemTypeGetGridGrouped(
      systemTypeId?: Array<number>,
      systemTypeNameVodafone?: Array<string>,
      systemTypeName3Gpp?: Array<string>,
      systemTypeNameOem?: Array<string>,
      majorSoftwareBuild?: Array<number>,
      majorHardwareBuild?: Array<string>,
      systemSolution?: Array<number>,
      constraintScalingStartDate?: Date,
      constraintScalingEndDate?: Date,
      constraintLcm?: Array<string>,
      endOfMaintenanceStartDate?: Date,
      endOfMaintenanceEndDate?: Date,
      lastModifiedStartDate?: Date,
      lastModifiedEndDate?: Date,
      productImportance?: Array<number>,
      verticalResponsible?: Array<number>,
      subDomainResponsible?: Array<number>,
      subDomainSpoc?: Array<string>,
      assetCategory?: Array<number>,
      assetClass?: Array<number>,
      assetType?: Array<string>,
      spareFieldsJson?: Array<string>,
      softwareOem?: Array<number>,
      hardwareOem?: Array<number>,
      lastModifiedBy?: Array<string>,
      sortBy?: string,
      isSortAscending?: boolean,
      page?: number,
      pageSize?: number,
      principalId?: number,
      deleted?: boolean,
      orphan?: boolean,
      options: any = {}
    ): FetchArgs {
      const localVarPath = `/api/SystemType/GetGridGrouped`;
      const localVarUrlObj = url.parse(localVarPath, true);
      const localVarRequestOptions = Object.assign({ method: "GET" }, options);
      const localVarHeaderParameter = { ...headerObj } as any;
      const localVarQueryParameter = {} as any;

      // authentication JWT required
      if (configuration && configuration.apiKey) {
        const localVarApiKeyValue =
          typeof configuration.apiKey === "function"
            ? configuration.apiKey("Authorization")
            : configuration.apiKey;
        localVarHeaderParameter["Authorization"] = localVarApiKeyValue;
      }

      if (systemTypeId) {
        localVarQueryParameter["SystemTypeId"] = systemTypeId;
      }

      if (systemTypeNameVodafone) {
        localVarQueryParameter["SystemTypeNameVodafone"] =
          systemTypeNameVodafone;
      }

      if (systemTypeName3Gpp) {
        localVarQueryParameter["SystemTypeName3Gpp"] = systemTypeName3Gpp;
      }

      if (systemTypeNameOem) {
        localVarQueryParameter["SystemTypeNameOem"] = systemTypeNameOem;
      }

      if (majorSoftwareBuild) {
        localVarQueryParameter["MajorSoftwareBuild"] = majorSoftwareBuild;
      }

      if (majorHardwareBuild) {
        localVarQueryParameter["MajorHardwareBuild"] = majorHardwareBuild;
      }

      if (systemSolution) {
        localVarQueryParameter["SystemSolution"] = systemSolution;
      }

      if (constraintScalingStartDate !== undefined) {
        localVarQueryParameter["ConstraintScaling.StartDate"] = (
          constraintScalingStartDate as any
        ).toISOString();
      }

      if (constraintScalingEndDate !== undefined) {
        localVarQueryParameter["ConstraintScaling.EndDate"] = (
          constraintScalingEndDate as any
        ).toISOString();
      }

      if (constraintLcm) {
        localVarQueryParameter["ConstraintLcm"] = constraintLcm;
      }

      if (endOfMaintenanceStartDate !== undefined) {
        localVarQueryParameter["EndOfMaintenance.StartDate"] = (
          endOfMaintenanceStartDate as any
        ).toISOString();
      }

      if (endOfMaintenanceEndDate !== undefined) {
        localVarQueryParameter["EndOfMaintenance.EndDate"] = (
          endOfMaintenanceEndDate as any
        ).toISOString();
      }

      if (lastModifiedStartDate !== undefined) {
        localVarQueryParameter["LastModified.StartDate"] = (
          lastModifiedStartDate as any
        ).toISOString();
      }

      if (lastModifiedEndDate !== undefined) {
        localVarQueryParameter["LastModified.EndDate"] = (
          lastModifiedEndDate as any
        ).toISOString();
      }

      if (productImportance) {
        localVarQueryParameter["ProductImportance"] = productImportance;
      }

      if (verticalResponsible) {
        localVarQueryParameter["VerticalResponsible"] = verticalResponsible;
      }

      if (subDomainResponsible) {
        localVarQueryParameter["SubDomainResponsible"] = subDomainResponsible;
      }

      if (subDomainSpoc) {
        localVarQueryParameter["SubDomainSpoc"] = subDomainSpoc;
      }

      if (assetCategory) {
        localVarQueryParameter["AssetCategory"] = assetCategory;
      }

      if (assetClass) {
        localVarQueryParameter["AssetClass"] = assetClass;
      }

      if (assetType) {
        localVarQueryParameter["AssetType"] = assetType;
      }

      if (spareFieldsJson) {
        localVarQueryParameter["SpareFieldsJson"] = spareFieldsJson;
      }

      if (softwareOem) {
        localVarQueryParameter["SoftwareOem"] = softwareOem;
      }

      if (hardwareOem) {
        localVarQueryParameter["HardwareOem"] = hardwareOem;
      }

      if (lastModifiedBy) {
        localVarQueryParameter["LastModifiedBy"] = lastModifiedBy;
      }

      if (sortBy !== undefined) {
        localVarQueryParameter["SortBy"] = sortBy;
      }

      if (isSortAscending !== undefined) {
        localVarQueryParameter["IsSortAscending"] = isSortAscending;
      }

      if (page !== undefined) {
        localVarQueryParameter["Page"] = page;
      }

      if (pageSize !== undefined) {
        localVarQueryParameter["PageSize"] = pageSize;
      }

      if (principalId !== undefined) {
        localVarQueryParameter["PrincipalId"] = principalId;
      }

      if (deleted !== undefined) {
        localVarQueryParameter["Deleted"] = deleted;
      }

      if (orphan !== undefined) {
        localVarQueryParameter["Orphan"] = orphan;
      }

      localVarUrlObj.query = Object.assign(
        {},
        localVarUrlObj.query,
        localVarQueryParameter,
        options.query
      );
      // fix override query string Detail: https://stackoverflow.com/a/7517673/1077943
      localVarUrlObj.search = null;
      localVarRequestOptions.headers = Object.assign(
        {},
        localVarHeaderParameter,
        options.headers
      );

      return {
        url: url.format(localVarUrlObj),
        options: localVarRequestOptions,
      };
    },
    /**
     *
     * @param {number} [majorSoftwareId]
     * @param {Array<number>} [majorHardwareIds]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    systemTypeGetMinorDateFromMajorEntity(
      majorSoftwareId?: number,
      majorHardwareIds?: Array<number>,
      options: any = {}
    ): FetchArgs {
      const localVarPath = `/api/SystemType/GetMinorDateFromMajorEntity`;
      const localVarUrlObj = url.parse(localVarPath, true);
      const localVarRequestOptions = Object.assign({ method: "GET" }, options);
      const localVarHeaderParameter = { ...headerObj } as any;
      const localVarQueryParameter = {} as any;

      // authentication JWT required
      if (configuration && configuration.apiKey) {
        const localVarApiKeyValue =
          typeof configuration.apiKey === "function"
            ? configuration.apiKey("Authorization")
            : configuration.apiKey;
        localVarHeaderParameter["Authorization"] = localVarApiKeyValue;
      }

      if (majorSoftwareId !== undefined) {
        localVarQueryParameter["majorSoftwareId"] = majorSoftwareId;
      }

      if (majorHardwareIds) {
        localVarQueryParameter["majorHardwareIds"] = majorHardwareIds;
      }

      localVarUrlObj.query = Object.assign(
        {},
        localVarUrlObj.query,
        localVarQueryParameter,
        options.query
      );
      // fix override query string Detail: https://stackoverflow.com/a/7517673/1077943
      localVarUrlObj.search = null;
      localVarRequestOptions.headers = Object.assign(
        {},
        localVarHeaderParameter,
        options.headers
      );

      return {
        url: url.format(localVarUrlObj),
        options: localVarRequestOptions,
      };
    },
    /**
     *
     * @param {number} id
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    systemTypeGetRelatedRecords(id: number, options: any = {}): FetchArgs {
      // verify required parameter 'id' is not null or undefined
      if (id === null || id === undefined) {
        throw new RequiredError(
          "id",
          "Required parameter id was null or undefined when calling systemTypeGetRelatedRecords."
        );
      }
      const localVarPath = `/api/SystemType/GetRelatedRecords{id}`.replace(
        `{${"id"}}`,
        encodeURIComponent(String(id))
      );
      const localVarUrlObj = url.parse(localVarPath, true);
      const localVarRequestOptions = Object.assign({ method: "GET" }, options);
      const localVarHeaderParameter = { ...headerObj } as any;
      const localVarQueryParameter = {} as any;

      // authentication JWT required
      if (configuration && configuration.apiKey) {
        const localVarApiKeyValue =
          typeof configuration.apiKey === "function"
            ? configuration.apiKey("Authorization")
            : configuration.apiKey;
        localVarHeaderParameter["Authorization"] = localVarApiKeyValue;
      }

      localVarUrlObj.query = Object.assign(
        {},
        localVarUrlObj.query,
        localVarQueryParameter,
        options.query
      );
      // fix override query string Detail: https://stackoverflow.com/a/7517673/1077943
      localVarUrlObj.search = null;
      localVarRequestOptions.headers = Object.assign(
        {},
        localVarHeaderParameter,
        options.headers
      );

      return {
        url: url.format(localVarUrlObj),
        options: localVarRequestOptions,
      };
    },
    /**
     *
     * @param {number} id
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    systemTypeGetSingleSystemType(id: number, options: any = {}): FetchArgs {
      // verify required parameter 'id' is not null or undefined
      if (id === null || id === undefined) {
        throw new RequiredError(
          "id",
          "Required parameter id was null or undefined when calling systemTypeGetSingleSystemType."
        );
      }
      const localVarPath = `/api/SystemType/{id}`.replace(
        `{${"id"}}`,
        encodeURIComponent(String(id))
      );
      const localVarUrlObj = url.parse(localVarPath, true);
      const localVarRequestOptions = Object.assign({ method: "GET" }, options);
      const localVarHeaderParameter = { ...headerObj } as any;
      const localVarQueryParameter = {} as any;

      // authentication JWT required
      if (configuration && configuration.apiKey) {
        const localVarApiKeyValue =
          typeof configuration.apiKey === "function"
            ? configuration.apiKey("Authorization")
            : configuration.apiKey;
        localVarHeaderParameter["Authorization"] = localVarApiKeyValue;
      }

      localVarUrlObj.query = Object.assign(
        {},
        localVarUrlObj.query,
        localVarQueryParameter,
        options.query
      );
      // fix override query string Detail: https://stackoverflow.com/a/7517673/1077943
      localVarUrlObj.search = null;
      localVarRequestOptions.headers = Object.assign(
        {},
        localVarHeaderParameter,
        options.headers
      );

      return {
        url: url.format(localVarUrlObj),
        options: localVarRequestOptions,
      };
    },
    /**
     *
     * @param {string} [nameOEM]
     * @param {number} [majorHardwareBuilds]
     * @param {number} [majorSoftwareBuild]
     * @param {Array<number>} [majorHardwareList]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    systemTypeGetSystemSolutionName(
      nameOEM?: string,
      majorHardwareBuilds?: number,
      majorSoftwareBuild?: number,
      majorHardwareList?: Array<number>,
      options: any = {}
    ): FetchArgs {
      const localVarPath = `/api/SystemType/GetSystemSolutionName`;
      const localVarUrlObj = url.parse(localVarPath, true);
      const localVarRequestOptions = Object.assign({ method: "GET" }, options);
      const localVarHeaderParameter = { ...headerObj } as any;
      const localVarQueryParameter = {} as any;

      // authentication JWT required
      if (configuration && configuration.apiKey) {
        const localVarApiKeyValue =
          typeof configuration.apiKey === "function"
            ? configuration.apiKey("Authorization")
            : configuration.apiKey;
        localVarHeaderParameter["Authorization"] = localVarApiKeyValue;
      }

      if (nameOEM !== undefined) {
        localVarQueryParameter["nameOEM"] = nameOEM;
      }

      if (majorHardwareBuilds !== undefined) {
        localVarQueryParameter["majorHardwareBuilds"] = majorHardwareBuilds;
      }

      if (majorSoftwareBuild !== undefined) {
        localVarQueryParameter["majorSoftwareBuild"] = majorSoftwareBuild;
      }

      if (majorHardwareList) {
        localVarQueryParameter["majorHardwareList"] = majorHardwareList;
      }

      localVarUrlObj.query = Object.assign(
        {},
        localVarUrlObj.query,
        localVarQueryParameter,
        options.query
      );
      // fix override query string Detail: https://stackoverflow.com/a/7517673/1077943
      localVarUrlObj.search = null;
      localVarRequestOptions.headers = Object.assign(
        {},
        localVarHeaderParameter,
        options.headers
      );

      return {
        url: url.format(localVarUrlObj),
        options: localVarRequestOptions,
      };
    },
    /**
     *
     * @param {number} [majorHardwareBuildId]
     * @param {number} [majorSoftwareBuildId]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    systemTypeGetAllSubdomainAndVerticalResponsibles(
      majorHardwareBuildId?: number,
      majorSoftwareBuildId?: number,
      options: any = {}
    ): FetchArgs {
      const localVarPath = `/api/SystemType/GetAllSubdomainAndVerticalResponsibles`;
      const localVarUrlObj = url.parse(localVarPath, true);
      const localVarRequestOptions = Object.assign({ method: "GET" }, options);
      const localVarHeaderParameter = { ...headerObj } as any;
      const localVarQueryParameter = {} as any;

      // authentication JWT required
      if (configuration && configuration.apiKey) {
        const localVarApiKeyValue =
          typeof configuration.apiKey === "function"
            ? configuration.apiKey("Authorization")
            : configuration.apiKey;
        localVarHeaderParameter["Authorization"] = localVarApiKeyValue;
      }
      if (majorHardwareBuildId !== undefined && majorHardwareBuildId !== null) {
        localVarQueryParameter["majorHwId"] = majorHardwareBuildId;
      }

      if (majorSoftwareBuildId !== undefined && majorHardwareBuildId !== null) {
        localVarQueryParameter["majorSwId"] = majorSoftwareBuildId;
      }
      localVarUrlObj.query = Object.assign(
        {},
        localVarUrlObj.query,
        localVarQueryParameter,
        options.query
      );
      // fix override query string Detail: https://stackoverflow.com/a/7517673/1077943
      localVarUrlObj.search = null;
      localVarRequestOptions.headers = Object.assign(
        {},
        localVarHeaderParameter,
        options.headers
      );

      return {
        url: url.format(localVarUrlObj),
        options: localVarRequestOptions,
      };
    },
    /**
     *
     * @param {SystemTypeQueryDto} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    systemTypeGetSystemType(
      body: SystemTypeQueryDto,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling systemTypeGetSystemType."
        );
      }
      const localVarPath = `/api/SystemType/Get`;
      const localVarUrlObj = url.parse(localVarPath, true);
      const localVarRequestOptions = Object.assign({ method: "POST" }, options);
      const localVarHeaderParameter = { ...headerObj } as any;
      const localVarQueryParameter = {} as any;

      // authentication JWT required
      if (configuration && configuration.apiKey) {
        const localVarApiKeyValue =
          typeof configuration.apiKey === "function"
            ? configuration.apiKey("Authorization")
            : configuration.apiKey;
        localVarHeaderParameter["Authorization"] = localVarApiKeyValue;
      }

      localVarHeaderParameter["Content-Type"] = "application/json";

      localVarUrlObj.query = Object.assign(
        {},
        localVarUrlObj.query,
        localVarQueryParameter,
        options.query
      );
      // fix override query string Detail: https://stackoverflow.com/a/7517673/1077943
      localVarUrlObj.search = null;
      localVarRequestOptions.headers = Object.assign(
        {},
        localVarHeaderParameter,
        options.headers
      );
      const needsSerialization =
        <any>"SystemTypeQueryDto" !== "string" ||
        localVarRequestOptions.headers["Content-Type"] === "application/json";
      localVarRequestOptions.body = needsSerialization
        ? JSON.stringify(body || {})
        : body || "";

      return {
        url: url.format(localVarUrlObj),
        options: localVarRequestOptions,
      };
    },
    /**
     *
     * @param {SystemTypeQueryDto} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    systemTypeGetSystemTypeExcel(
      body: SystemTypeQueryDto,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling systemTypeGetSystemTypeExcel."
        );
      }
      const localVarPath = `/api/SystemType/Export`;
      const localVarUrlObj = url.parse(localVarPath, true);
      const localVarRequestOptions = Object.assign({ method: "POST" }, options);
      const localVarHeaderParameter = { ...headerObj } as any;
      const localVarQueryParameter = {} as any;

      // authentication JWT required
      if (configuration && configuration.apiKey) {
        const localVarApiKeyValue =
          typeof configuration.apiKey === "function"
            ? configuration.apiKey("Authorization")
            : configuration.apiKey;
        localVarHeaderParameter["Authorization"] = localVarApiKeyValue;
      }

      localVarHeaderParameter["Content-Type"] = "application/json";

      localVarUrlObj.query = Object.assign(
        {},
        localVarUrlObj.query,
        localVarQueryParameter,
        options.query
      );
      // fix override query string Detail: https://stackoverflow.com/a/7517673/1077943
      localVarUrlObj.search = null;
      localVarRequestOptions.headers = Object.assign(
        {},
        localVarHeaderParameter,
        options.headers
      );
      const needsSerialization =
        <any>"SystemTypeQueryDto" !== "string" ||
        localVarRequestOptions.headers["Content-Type"] === "application/json";
      localVarRequestOptions.body = needsSerialization
        ? JSON.stringify(body || {})
        : body || "";

      return {
        url: url.format(localVarUrlObj),
        options: localVarRequestOptions,
      };
    },
    /**
     *
     * @param {number} id
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    systemTypeGetUpdateResourceSystemType(
      id: number,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'id' is not null or undefined
      if (id === null || id === undefined) {
        throw new RequiredError(
          "id",
          "Required parameter id was null or undefined when calling systemTypeGetUpdateResourceSystemType."
        );
      }
      const localVarPath = `/api/SystemType/Update{id}`.replace(
        `{${"id"}}`,
        encodeURIComponent(String(id))
      );
      const localVarUrlObj = url.parse(localVarPath, true);
      const localVarRequestOptions = Object.assign({ method: "GET" }, options);
      const localVarHeaderParameter = { ...headerObj } as any;
      const localVarQueryParameter = {} as any;

      // authentication JWT required
      if (configuration && configuration.apiKey) {
        const localVarApiKeyValue =
          typeof configuration.apiKey === "function"
            ? configuration.apiKey("Authorization")
            : configuration.apiKey;
        localVarHeaderParameter["Authorization"] = localVarApiKeyValue;
      }

      localVarUrlObj.query = Object.assign(
        {},
        localVarUrlObj.query,
        localVarQueryParameter,
        options.query
      );
      // fix override query string Detail: https://stackoverflow.com/a/7517673/1077943
      localVarUrlObj.search = null;
      localVarRequestOptions.headers = Object.assign(
        {},
        localVarHeaderParameter,
        options.headers
      );

      return {
        url: url.format(localVarUrlObj),
        options: localVarRequestOptions,
      };
    },
    /**
     *
     * @param {SystemTypeDtoUpdate} body
     * @param {boolean} [forced]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    systemTypePut(
      body: SystemTypeDtoUpdate,
      forced?: boolean,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling systemTypePut."
        );
      }
      const localVarPath = `/api/SystemType`;
      const localVarUrlObj = url.parse(localVarPath, true);
      const localVarRequestOptions = Object.assign({ method: "PUT" }, options);
      const localVarHeaderParameter = { ...headerObj } as any;
      const localVarQueryParameter = {} as any;

      // authentication JWT required
      if (configuration && configuration.apiKey) {
        const localVarApiKeyValue =
          typeof configuration.apiKey === "function"
            ? configuration.apiKey("Authorization")
            : configuration.apiKey;
        localVarHeaderParameter["Authorization"] = localVarApiKeyValue;
      }

      if (forced !== undefined) {
        localVarQueryParameter["forced"] = forced;
      }

      localVarHeaderParameter["Content-Type"] = "application/json";

      localVarUrlObj.query = Object.assign(
        {},
        localVarUrlObj.query,
        localVarQueryParameter,
        options.query
      );
      // fix override query string Detail: https://stackoverflow.com/a/7517673/1077943
      localVarUrlObj.search = null;
      localVarRequestOptions.headers = Object.assign(
        {},
        localVarHeaderParameter,
        options.headers
      );
      const needsSerialization =
        <any>"SystemTypeDtoUpdate" !== "string" ||
        localVarRequestOptions.headers["Content-Type"] === "application/json";
      localVarRequestOptions.body = needsSerialization
        ? JSON.stringify(body || {})
        : body || "";

      return {
        url: url.format(localVarUrlObj),
        options: localVarRequestOptions,
      };
    },
    /**
     *
     * @param {number} [id]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    systemTypeRestore(id?: number, options: any = {}): FetchArgs {
      const localVarPath = `/api/SystemType/Restore`;
      const localVarUrlObj = url.parse(localVarPath, true);
      const localVarRequestOptions = Object.assign({ method: "PUT" }, options);
      const localVarHeaderParameter = { ...headerObj } as any;
      const localVarQueryParameter = {} as any;

      // authentication JWT required
      if (configuration && configuration.apiKey) {
        const localVarApiKeyValue =
          typeof configuration.apiKey === "function"
            ? configuration.apiKey("Authorization")
            : configuration.apiKey;
        localVarHeaderParameter["Authorization"] = localVarApiKeyValue;
      }

      if (id !== undefined) {
        localVarQueryParameter["id"] = id;
      }

      localVarUrlObj.query = Object.assign(
        {},
        localVarUrlObj.query,
        localVarQueryParameter,
        options.query
      );
      // fix override query string Detail: https://stackoverflow.com/a/7517673/1077943
      localVarUrlObj.search = null;
      localVarRequestOptions.headers = Object.assign(
        {},
        localVarHeaderParameter,
        options.headers
      );

      return {
        url: url.format(localVarUrlObj),
        options: localVarRequestOptions,
      };
    },
  };
};

/**
 * SystemTypeApi - functional programming interface
 * @export
 */
export const SystemTypeApiFp = function (configuration?: Configuration) {
  return {
    /**
     *
     * @param {DataRemediationDto} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    systemTypeApplyDataRemediation(
      body: DataRemediationDto,
      options?: any
    ): (
      fetch?: FetchAPI,
      basePath?: string
    ) => Promise<ResultDtoOfResultDataRemediationDto> {
      const localVarFetchArgs = SystemTypeApiFetchParamCreator(
        configuration
      ).systemTypeApplyDataRemediation(body, options);
      return (fetch: FetchAPI = isomorphicFetch, basePath = BASE_PATH) => {
        return fetch(
          basePath + localVarFetchArgs.url,
          localVarFetchArgs.options
        ).then((response) => {
          if (response.status >= 200 && response.status < 300) {
            return response.json();
          } else {
            throw response;
          }
        });
      };
    },
    /**
     *
     * @param {SystemTypeDtoCreate} body
     * @param {boolean} [forced]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    systemTypeCreate(
      body: SystemTypeDtoCreate,
      forced?: boolean,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = SystemTypeApiFetchParamCreator(
        configuration
      ).systemTypeCreate(body, forced, options);
      return (fetch: FetchAPI = isomorphicFetch, basePath = BASE_PATH) => {
        return fetch(
          basePath + localVarFetchArgs.url,
          localVarFetchArgs.options
        ).then((response) => {
          if (response.status >= 200 && response.status < 300) {
            return response.json();
          } else {
            throw response;
          }
        });
      };
    },
    /**
     *
     * @param {number} [id]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    systemTypeDelete(
      id?: number,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = SystemTypeApiFetchParamCreator(
        configuration
      ).systemTypeDelete(id, options);
      return (fetch: FetchAPI = isomorphicFetch, basePath = BASE_PATH) => {
        return fetch(
          basePath + localVarFetchArgs.url,
          localVarFetchArgs.options
        ).then((response) => {
          if (response.status >= 200 && response.status < 300) {
            return response.json();
          } else {
            throw response;
          }
        });
      };
    },
    /**
     *
     * @param {number} [id]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    systemTypeDeleteDeep(
      id?: number,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = SystemTypeApiFetchParamCreator(
        configuration
      ).systemTypeDeleteDeep(id, options);
      return (fetch: FetchAPI = isomorphicFetch, basePath = BASE_PATH) => {
        return fetch(
          basePath + localVarFetchArgs.url,
          localVarFetchArgs.options
        ).then((response) => {
          if (response.status >= 200 && response.status < 300) {
            return response.json();
          } else {
            throw response;
          }
        });
      };
    },
    /**
     *
     * @param {SystemTypeQueryDto} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    systemTypeExportReport(
      body: SystemTypeQueryDto,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ReturnFile> {
      const localVarFetchArgs = SystemTypeApiFetchParamCreator(
        configuration
      ).systemTypeExportReport(body, options);
      return (fetch: FetchAPI = isomorphicFetch, basePath = BASE_PATH) => {
        return fetch(
          basePath + localVarFetchArgs.url,
          localVarFetchArgs.options
        ).then((response) => {
          if (response.status >= 200 && response.status < 300) {
            return {
              File: response.blob(),
              FileName: response.headers.get("Content-Disposition"),
            } as ReturnFile;
          } else {
            throw response;
          }
        });
      };
    },
    /**
     *
     * @param {number} [assetCategoryId]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    systemTypeGetAssetCategoryReleated(
      assetCategoryId?: number,
      options?: any
    ): (
      fetch?: FetchAPI,
      basePath?: string
    ) => Promise<ResultDtoOfSystemTypeReleatedMajorEntity> {
      const localVarFetchArgs = SystemTypeApiFetchParamCreator(
        configuration
      ).systemTypeGetAssetCategoryReleated(assetCategoryId, options);
      return (fetch: FetchAPI = isomorphicFetch, basePath = BASE_PATH) => {
        return fetch(
          basePath + localVarFetchArgs.url,
          localVarFetchArgs.options
        ).then((response) => {
          if (response.status >= 200 && response.status < 300) {
            return response.json();
          } else {
            throw response;
          }
        });
      };
    },
    /**
     *
     * @param {number} [majorSoftwareBuild]
     * @param {Array<number>} [majorHardwareBuilds]
     * @param {*} [options]
     * @throws {RequiredError}
     */
    systemTypeGetCostraintInfo(
      majorSoftwareBuild?: number,
      majorHardwareBuilds?: Array<number>,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ConstraintInfoDto> {
      const localVarFetchArgs = SystemTypeApiFetchParamCreator(
        configuration
      ).systemTypeGetCostraintInfo(
        majorSoftwareBuild,
        majorHardwareBuilds,
        options
      );
      return (fetch: FetchAPI = isomorphicFetch, basePath = BASE_PATH) => {
        return fetch(
          basePath + localVarFetchArgs.url,
          localVarFetchArgs.options
        ).then((response) => {
          if (response.status >= 200 && response.status < 300) {
            return response.json();
          } else {
            throw response;
          }
        });
      };
    },

    systemTypeGetVodafoneNameResource(
      id?: number,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<any> {
      const localVarFetchArgs = SystemTypeApiFetchParamCreator(
        configuration
      ).systemTypeGetVodafoneNameResource(id, options);
      return (fetch: FetchAPI = isomorphicFetch, basePath = BASE_PATH) => {
        return fetch(
          basePath + localVarFetchArgs.url,
          localVarFetchArgs.options
        ).then((response) => {
          if (response.status >= 200 && response.status < 300) {
            return response.json();
          } else {
            throw response;
          }
        });
      };
    },

    systemTypeGetVodafoneNameResourceWizardMode(
      id?: number,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<any> {
      const localVarFetchArgs = SystemTypeApiFetchParamCreator(
        configuration
      ).systemTypeGetVodafoneNameResourceWizardMode(id, options);
      return (fetch: FetchAPI = isomorphicFetch, basePath = BASE_PATH) => {
        return fetch(
          basePath + localVarFetchArgs.url,
          localVarFetchArgs.options
        ).then((response) => {
          if (response.status >= 200 && response.status < 300) {
            return response.json();
          } else {
            throw response;
          }
        });
      };
    },

    /**
     *
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    systemTypeGetCreateResourceSystemType(
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<SystemTypeDtoCreate> {
      const localVarFetchArgs =
        SystemTypeApiFetchParamCreator(
          configuration
        ).systemTypeGetCreateResourceSystemType(options);
      return (fetch: FetchAPI = isomorphicFetch, basePath = BASE_PATH) => {
        return fetch(
          basePath + localVarFetchArgs.url,
          localVarFetchArgs.options
        ).then((response) => {
          if (response.status >= 200 && response.status < 300) {
            return response.json();
          } else {
            throw response;
          }
        });
      };
    },
    /**
     *
     * @param {SystemTypeQueryDto} body
     * @param {string} [propertyName]
     * @param {string} [propertyFilter]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    systemTypeGetFilterResult(
      body: SystemTypeQueryDto,
      propertyName?: string,
      propertyFilter?: string,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<Array<FilterValueDto>> {
      const localVarFetchArgs = SystemTypeApiFetchParamCreator(
        configuration
      ).systemTypeGetFilterResult(body, propertyName, propertyFilter, options);
      return (fetch: FetchAPI = isomorphicFetch, basePath = BASE_PATH) => {
        return fetch(
          basePath + localVarFetchArgs.url,
          localVarFetchArgs.options
        ).then((response) => {
          if (response.status >= 200 && response.status < 300) {
            return response.json();
          } else {
            throw response;
          }
        });
      };
    },
    /**
     *
     * @param {Array<number>} [systemTypeId]
     * @param {Array<string>} [systemTypeNameVodafone]
     * @param {Array<string>} [systemTypeName3Gpp]
     * @param {Array<string>} [systemTypeNameOem]
     * @param {Array<number>} [majorSoftwareBuild]
     * @param {Array<string>} [majorHardwareBuild]
     * @param {Array<number>} [systemSolution]
     * @param {Date} [constraintScalingStartDate]
     * @param {Date} [constraintScalingEndDate]
     * @param {Array<string>} [constraintLcm]
     * @param {Date} [endOfMaintenanceStartDate]
     * @param {Date} [endOfMaintenanceEndDate]
     * @param {Date} [lastModifiedStartDate]
     * @param {Date} [lastModifiedEndDate]
     * @param {Array<number>} [productImportance]
     * @param {Array<number>} [verticalResponsible]
     * @param {Array<number>} [subDomainResponsible]
     * @param {Array<string>} [subDomainSpoc]
     * @param {Array<number>} [assetCategory]
     * @param {Array<number>} [assetClass]
     * @param {Array<string>} [assetType]
     * @param {Array<string>} [spareFieldsJson]
     * @param {Array<number>} [softwareOem]
     * @param {Array<number>} [hardwareOem]
     * @param {Array<string>} [lastModifiedBy]
     * @param {string} [sortBy]
     * @param {boolean} [isSortAscending]
     * @param {number} [page]
     * @param {number} [pageSize]
     * @param {number} [principalId]
     * @param {boolean} [deleted]
     * @param {boolean} [orphan]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    systemTypeGetGridGrouped(
      systemTypeId?: Array<number>,
      systemTypeNameVodafone?: Array<string>,
      systemTypeName3Gpp?: Array<string>,
      systemTypeNameOem?: Array<string>,
      majorSoftwareBuild?: Array<number>,
      majorHardwareBuild?: Array<string>,
      systemSolution?: Array<number>,
      constraintScalingStartDate?: Date,
      constraintScalingEndDate?: Date,
      constraintLcm?: Array<string>,
      endOfMaintenanceStartDate?: Date,
      endOfMaintenanceEndDate?: Date,
      lastModifiedStartDate?: Date,
      lastModifiedEndDate?: Date,
      productImportance?: Array<number>,
      verticalResponsible?: Array<number>,
      subDomainResponsible?: Array<number>,
      subDomainSpoc?: Array<string>,
      assetCategory?: Array<number>,
      assetClass?: Array<number>,
      assetType?: Array<string>,
      spareFieldsJson?: Array<string>,
      softwareOem?: Array<number>,
      hardwareOem?: Array<number>,
      lastModifiedBy?: Array<string>,
      sortBy?: string,
      isSortAscending?: boolean,
      page?: number,
      pageSize?: number,
      principalId?: number,
      deleted?: boolean,
      orphan?: boolean,
      options?: any
    ): (
      fetch?: FetchAPI,
      basePath?: string
    ) => Promise<QueryResultDtoOfSystemTypeDtoGrouped> {
      const localVarFetchArgs = SystemTypeApiFetchParamCreator(
        configuration
      ).systemTypeGetGridGrouped(
        systemTypeId,
        systemTypeNameVodafone,
        systemTypeName3Gpp,
        systemTypeNameOem,
        majorSoftwareBuild,
        majorHardwareBuild,
        systemSolution,
        constraintScalingStartDate,
        constraintScalingEndDate,
        constraintLcm,
        endOfMaintenanceStartDate,
        endOfMaintenanceEndDate,
        lastModifiedStartDate,
        lastModifiedEndDate,
        productImportance,
        verticalResponsible,
        subDomainResponsible,
        subDomainSpoc,
        assetCategory,
        assetClass,
        assetType,
        spareFieldsJson,
        softwareOem,
        hardwareOem,
        lastModifiedBy,
        sortBy,
        isSortAscending,
        page,
        pageSize,
        principalId,
        deleted,
        orphan,
        options
      );
      return (fetch: FetchAPI = isomorphicFetch, basePath = BASE_PATH) => {
        return fetch(
          basePath + localVarFetchArgs.url,
          localVarFetchArgs.options
        ).then((response) => {
          if (response.status >= 200 && response.status < 300) {
            return response.json();
          } else {
            throw response;
          }
        });
      };
    },
    /**
     *
     * @param {number} [majorSoftwareId]
     * @param {Array<number>} [majorHardwareIds]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    systemTypeGetMinorDateFromMajorEntity(
      majorSoftwareId?: number,
      majorHardwareIds?: Array<number>,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = SystemTypeApiFetchParamCreator(
        configuration
      ).systemTypeGetMinorDateFromMajorEntity(
        majorSoftwareId,
        majorHardwareIds,
        options
      );
      return (fetch: FetchAPI = isomorphicFetch, basePath = BASE_PATH) => {
        return fetch(
          basePath + localVarFetchArgs.url,
          localVarFetchArgs.options
        ).then((response) => {
          if (response.status >= 200 && response.status < 300) {
            return response.json();
          } else {
            throw response;
          }
        });
      };
    },
    /**
     *
     * @param {number} id
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    systemTypeGetRelatedRecords(
      id: number,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = SystemTypeApiFetchParamCreator(
        configuration
      ).systemTypeGetRelatedRecords(id, options);
      return (fetch: FetchAPI = isomorphicFetch, basePath = BASE_PATH) => {
        return fetch(
          basePath + localVarFetchArgs.url,
          localVarFetchArgs.options
        ).then((response) => {
          if (response.status >= 200 && response.status < 300) {
            return response.json();
          } else {
            throw response;
          }
        });
      };
    },
    /**
     *
     * @param {number} id
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    systemTypeGetSingleSystemType(
      id: number,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<SystemTypeDto> {
      const localVarFetchArgs = SystemTypeApiFetchParamCreator(
        configuration
      ).systemTypeGetSingleSystemType(id, options);
      return (fetch: FetchAPI = isomorphicFetch, basePath = BASE_PATH) => {
        return fetch(
          basePath + localVarFetchArgs.url,
          localVarFetchArgs.options
        ).then((response) => {
          if (response.status >= 200 && response.status < 300) {
            return response.json();
          } else {
            throw response;
          }
        });
      };
    },
    /**
     *
     * @param {string} [nameOEM]
     * @param {number} [majorHardwareBuilds]
     * @param {number} [majorSoftwareBuild]
     * @param {Array<number>} [majorHardwareList]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    systemTypeGetSystemSolutionName(
      nameOEM?: string,
      majorHardwareBuilds?: number,
      majorSoftwareBuild?: number,
      majorHardwareList?: Array<number>,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = SystemTypeApiFetchParamCreator(
        configuration
      ).systemTypeGetSystemSolutionName(
        nameOEM,
        majorHardwareBuilds,
        majorSoftwareBuild,
        majorHardwareList,
        options
      );
      return (fetch: FetchAPI = isomorphicFetch, basePath = BASE_PATH) => {
        return fetch(
          basePath + localVarFetchArgs.url,
          localVarFetchArgs.options
        ).then((response) => {
          if (response.status >= 200 && response.status < 300) {
            return response.json();
          } else {
            throw response;
          }
        });
      };
    },
    /**
     *
     * @param {number} [majorHardwareBuildId]
     * @param {number} [majorSoftwareBuildId]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    systemTypeGetAllSubdomainAndVerticalResponsibles(
      majorHardwareBuildId?: number,
      majorSoftwareBuildId?: number,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = SystemTypeApiFetchParamCreator(
        configuration
      ).systemTypeGetAllSubdomainAndVerticalResponsibles(
        majorHardwareBuildId,
        majorSoftwareBuildId,
        options
      );
      return (fetch: FetchAPI = isomorphicFetch, basePath = BASE_PATH) => {
        return fetch(
          basePath + localVarFetchArgs.url,
          localVarFetchArgs.options
        ).then((response) => {
          if (response.status >= 200 && response.status < 300) {
            return response.json();
          } else {
            throw response;
          }
        });
      };
    },
    /**
     *
     * @param {SystemTypeQueryDto} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    systemTypeGetSystemType(
      body: SystemTypeQueryDto,
      options?: any
    ): (
      fetch?: FetchAPI,
      basePath?: string
    ) => Promise<QueryResultDtoOfSystemTypeDtoGrid> {
      const localVarFetchArgs = SystemTypeApiFetchParamCreator(
        configuration
      ).systemTypeGetSystemType(body, options);
      return (fetch: FetchAPI = isomorphicFetch, basePath = BASE_PATH) => {
        return fetch(
          basePath + localVarFetchArgs.url,
          localVarFetchArgs.options
        ).then((response) => {
          if (response.status >= 200 && response.status < 300) {
            return response.json();
          } else {
            throw response;
          }
        });
      };
    },
    /**
     *
     * @param {SystemTypeQueryDto} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    systemTypeGetSystemTypeExcel(
      body: SystemTypeQueryDto,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ReturnFile> {
      const localVarFetchArgs = SystemTypeApiFetchParamCreator(
        configuration
      ).systemTypeGetSystemTypeExcel(body, options);
      return (fetch: FetchAPI = isomorphicFetch, basePath = BASE_PATH) => {
        return fetch(
          basePath + localVarFetchArgs.url,
          localVarFetchArgs.options
        ).then((response) => {
          if (response.status >= 200 && response.status < 300) {
            return {
              File: response.blob(),
              FileName: response.headers.get("Content-Disposition"),
            } as ReturnFile;
          } else {
            throw response;
          }
        });
      };
    },
    /**
     *
     * @param {number} id
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    systemTypeGetUpdateResourceSystemType(
      id: number,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<SystemTypeDtoUpdate> {
      const localVarFetchArgs = SystemTypeApiFetchParamCreator(
        configuration
      ).systemTypeGetUpdateResourceSystemType(id, options);
      return (fetch: FetchAPI = isomorphicFetch, basePath = BASE_PATH) => {
        return fetch(
          basePath + localVarFetchArgs.url,
          localVarFetchArgs.options
        ).then((response) => {
          if (response.status >= 200 && response.status < 300) {
            return response.json();
          } else {
            throw response;
          }
        });
      };
    },
    /**
     *
     * @param {SystemTypeDtoUpdate} body
     * @param {boolean} [forced]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    systemTypePut(
      body: SystemTypeDtoUpdate,
      forced?: boolean,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = SystemTypeApiFetchParamCreator(
        configuration
      ).systemTypePut(body, forced, options);
      return (fetch: FetchAPI = isomorphicFetch, basePath = BASE_PATH) => {
        return fetch(
          basePath + localVarFetchArgs.url,
          localVarFetchArgs.options
        ).then((response) => {
          if (response.status >= 200 && response.status < 300) {
            return response.json();
          } else {
            throw response;
          }
        });
      };
    },
    /**
     *
     * @param {number} [id]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    systemTypeRestore(
      id?: number,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = SystemTypeApiFetchParamCreator(
        configuration
      ).systemTypeRestore(id, options);
      return (fetch: FetchAPI = isomorphicFetch, basePath = BASE_PATH) => {
        return fetch(
          basePath + localVarFetchArgs.url,
          localVarFetchArgs.options
        ).then((response) => {
          if (response.status >= 200 && response.status < 300) {
            return response.json();
          } else {
            throw response;
          }
        });
      };
    },
  };
};

/**
 * SystemTypeApi - factory interface
 * @export
 */
export const SystemTypeApiFactory = function (
  configuration?: Configuration,
  fetch?: FetchAPI,
  basePath?: string
) {
  return {
    /**
     *
     * @param {DataRemediationDto} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    systemTypeApplyDataRemediation(body: DataRemediationDto, options?: any) {
      return SystemTypeApiFp(configuration).systemTypeApplyDataRemediation(
        body,
        options
      )(fetch, basePath);
    },
    /**
     *
     * @param {SystemTypeDtoCreate} body
     * @param {boolean} [forced]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    systemTypeCreate(
      body: SystemTypeDtoCreate,
      forced?: boolean,
      options?: any
    ) {
      return SystemTypeApiFp(configuration).systemTypeCreate(
        body,
        forced,
        options
      )(fetch, basePath);
    },
    /**
     *
     * @param {number} [id]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    systemTypeDelete(id?: number, options?: any) {
      return SystemTypeApiFp(configuration).systemTypeDelete(id, options)(
        fetch,
        basePath
      );
    },
    /**
     *
     * @param {number} [id]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    systemTypeDeleteDeep(id?: number, options?: any) {
      return SystemTypeApiFp(configuration).systemTypeDeleteDeep(id, options)(
        fetch,
        basePath
      );
    },
    /**
     *
     * @param {SystemTypeQueryDto} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    systemTypeExportReport(body: SystemTypeQueryDto, options?: any) {
      return SystemTypeApiFp(configuration).systemTypeExportReport(
        body,
        options
      )(fetch, basePath);
    },
    /**
     *
     * @param {number} [assetCategoryId]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    systemTypeGetAssetCategoryReleated(
      assetCategoryId?: number,
      options?: any
    ) {
      return SystemTypeApiFp(configuration).systemTypeGetAssetCategoryReleated(
        assetCategoryId,
        options
      )(fetch, basePath);
    },
    /**
     *
     * @param {number} [majorSoftwareBuild]
     * @param {Array<number>} [majorHardwareBuilds]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    systemTypeGetCostraintInfo(
      majorSoftwareBuild?: number,
      majorHardwareBuilds?: Array<number>,
      options?: any
    ) {
      return SystemTypeApiFp(configuration).systemTypeGetCostraintInfo(
        majorSoftwareBuild,
        majorHardwareBuilds,
        options
      )(fetch, basePath);
    },

    systemTypeGetVodafoneNameResource(id: number, options?: any) {
      return SystemTypeApiFp(configuration).systemTypeGetVodafoneNameResource(
        id,
        options
      )(fetch, basePath);
    },

    systemTypeGetVodafoneNameResourceWizardMode(id: number, options?: any) {
      return SystemTypeApiFp(
        configuration
      ).systemTypeGetVodafoneNameResourceWizardMode(id, options)(
        fetch,
        basePath
      );
    },

    /**
     *
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    systemTypeGetCreateResourceSystemType(options?: any) {
      return SystemTypeApiFp(
        configuration
      ).systemTypeGetCreateResourceSystemType(options)(fetch, basePath);
    },
    /**
     *
     * @param {SystemTypeQueryDto} body
     * @param {string} [propertyName]
     * @param {string} [propertyFilter]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    systemTypeGetFilterResult(
      body: SystemTypeQueryDto,
      propertyName?: string,
      propertyFilter?: string,
      options?: any
    ) {
      return SystemTypeApiFp(configuration).systemTypeGetFilterResult(
        body,
        propertyName,
        propertyFilter,
        options
      )(fetch, basePath);
    },
    /**
     *
     * @param {Array<number>} [systemTypeId]
     * @param {Array<string>} [systemTypeNameVodafone]
     * @param {Array<string>} [systemTypeName3Gpp]
     * @param {Array<string>} [systemTypeNameOem]
     * @param {Array<number>} [majorSoftwareBuild]
     * @param {Array<string>} [majorHardwareBuild]
     * @param {Array<number>} [systemSolution]
     * @param {Date} [constraintScalingStartDate]
     * @param {Date} [constraintScalingEndDate]
     * @param {Array<string>} [constraintLcm]
     * @param {Date} [endOfMaintenanceStartDate]
     * @param {Date} [endOfMaintenanceEndDate]
     * @param {Date} [lastModifiedStartDate]
     * @param {Date} [lastModifiedEndDate]
     * @param {Array<number>} [productImportance]
     * @param {Array<number>} [verticalResponsible]
     * @param {Array<number>} [subDomainResponsible]
     * @param {Array<string>} [subDomainSpoc]
     * @param {Array<number>} [assetCategory]
     * @param {Array<number>} [assetClass]
     * @param {Array<string>} [assetType]
     * @param {Array<string>} [spareFieldsJson]
     * @param {Array<number>} [softwareOem]
     * @param {Array<number>} [hardwareOem]
     * @param {Array<string>} [lastModifiedBy]
     * @param {string} [sortBy]
     * @param {boolean} [isSortAscending]
     * @param {number} [page]
     * @param {number} [pageSize]
     * @param {number} [principalId]
     * @param {boolean} [deleted]
     * @param {boolean} [orphan]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    systemTypeGetGridGrouped(
      systemTypeId?: Array<number>,
      systemTypeNameVodafone?: Array<string>,
      systemTypeName3Gpp?: Array<string>,
      systemTypeNameOem?: Array<string>,
      majorSoftwareBuild?: Array<number>,
      majorHardwareBuild?: Array<string>,
      systemSolution?: Array<number>,
      constraintScalingStartDate?: Date,
      constraintScalingEndDate?: Date,
      constraintLcm?: Array<string>,
      endOfMaintenanceStartDate?: Date,
      endOfMaintenanceEndDate?: Date,
      lastModifiedStartDate?: Date,
      lastModifiedEndDate?: Date,
      productImportance?: Array<number>,
      verticalResponsible?: Array<number>,
      subDomainResponsible?: Array<number>,
      subDomainSpoc?: Array<string>,
      assetCategory?: Array<number>,
      assetClass?: Array<number>,
      assetType?: Array<string>,
      spareFieldsJson?: Array<string>,
      softwareOem?: Array<number>,
      hardwareOem?: Array<number>,
      lastModifiedBy?: Array<string>,
      sortBy?: string,
      isSortAscending?: boolean,
      page?: number,
      pageSize?: number,
      principalId?: number,
      deleted?: boolean,
      orphan?: boolean,
      options?: any
    ) {
      return SystemTypeApiFp(configuration).systemTypeGetGridGrouped(
        systemTypeId,
        systemTypeNameVodafone,
        systemTypeName3Gpp,
        systemTypeNameOem,
        majorSoftwareBuild,
        majorHardwareBuild,
        systemSolution,
        constraintScalingStartDate,
        constraintScalingEndDate,
        constraintLcm,
        endOfMaintenanceStartDate,
        endOfMaintenanceEndDate,
        lastModifiedStartDate,
        lastModifiedEndDate,
        productImportance,
        verticalResponsible,
        subDomainResponsible,
        subDomainSpoc,
        assetCategory,
        assetClass,
        assetType,
        spareFieldsJson,
        softwareOem,
        hardwareOem,
        lastModifiedBy,
        sortBy,
        isSortAscending,
        page,
        pageSize,
        principalId,
        deleted,
        orphan,
        options
      )(fetch, basePath);
    },
    /**
     *
     * @param {number} [majorSoftwareId]
     * @param {Array<number>} [majorHardwareIds]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    systemTypeGetMinorDateFromMajorEntity(
      majorSoftwareId?: number,
      majorHardwareIds?: Array<number>,
      options?: any
    ) {
      return SystemTypeApiFp(
        configuration
      ).systemTypeGetMinorDateFromMajorEntity(
        majorSoftwareId,
        majorHardwareIds,
        options
      )(fetch, basePath);
    },
    /**
     *
     * @param {number} id
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    systemTypeGetRelatedRecords(id: number, options?: any) {
      return SystemTypeApiFp(configuration).systemTypeGetRelatedRecords(
        id,
        options
      )(fetch, basePath);
    },
    /**
     *
     * @param {number} id
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    systemTypeGetSingleSystemType(id: number, options?: any) {
      return SystemTypeApiFp(configuration).systemTypeGetSingleSystemType(
        id,
        options
      )(fetch, basePath);
    },
    /**
     *
     * @param {string} [nameOEM]
     * @param {number} [majorHardwareBuilds]
     * @param {number} [majorSoftwareBuild]
     * @param {Array<number>} [majorHardwareList]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    systemTypeGetSystemSolutionName(
      nameOEM?: string,
      majorHardwareBuilds?: number,
      majorSoftwareBuild?: number,
      majorHardwareList?: Array<number>,
      options?: any
    ) {
      return SystemTypeApiFp(configuration).systemTypeGetSystemSolutionName(
        nameOEM,
        majorHardwareBuilds,
        majorSoftwareBuild,
        majorHardwareList,
        options
      )(fetch, basePath);
    },
    /**
     *
     * @param {number} [majorHardwareBuildId]
     * @param {number} [majorSoftwareBuildId]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    systemTypeGetAllSubdomainAndVerticalResponsibles(
      majorHardwareBuildId?: number,
      majorSoftwareBuildId?: number,
      options?: any
    ) {
      return SystemTypeApiFp(
        configuration
      ).systemTypeGetAllSubdomainAndVerticalResponsibles(
        majorHardwareBuildId,
        majorSoftwareBuildId,
        options
      )(fetch, basePath);
    },
    /**
     *
     * @param {SystemTypeQueryDto} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    systemTypeGetSystemType(body: SystemTypeQueryDto, options?: any) {
      return SystemTypeApiFp(configuration).systemTypeGetSystemType(
        body,
        options
      )(fetch, basePath);
    },
    /**
     *
     * @param {SystemTypeQueryDto} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    systemTypeGetSystemTypeExcel(body: SystemTypeQueryDto, options?: any) {
      return SystemTypeApiFp(configuration).systemTypeGetSystemTypeExcel(
        body,
        options
      )(fetch, basePath);
    },
    /**
     *
     * @param {number} id
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    systemTypeGetUpdateResourceSystemType(id: number, options?: any) {
      return SystemTypeApiFp(
        configuration
      ).systemTypeGetUpdateResourceSystemType(id, options)(fetch, basePath);
    },
    /**
     *
     * @param {SystemTypeDtoUpdate} body
     * @param {boolean} [forced]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    systemTypePut(body: SystemTypeDtoUpdate, forced?: boolean, options?: any) {
      return SystemTypeApiFp(configuration).systemTypePut(
        body,
        forced,
        options
      )(fetch, basePath);
    },
    /**
     *
     * @param {number} [id]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    systemTypeRestore(id?: number, options?: any) {
      return SystemTypeApiFp(configuration).systemTypeRestore(id, options)(
        fetch,
        basePath
      );
    },
  };
};

/**
 * SystemTypeApi - object-oriented interface
 * @export
 * @class SystemTypeApi
 * @extends {BaseAPI}
 */
export class SystemTypeApi extends BaseAPI {
  /**
   *
   * @param {DataRemediationDto} body
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof SystemTypeApi
   */
  public systemTypeApplyDataRemediation(
    body: DataRemediationDto,
    options?: any
  ) {
    return SystemTypeApiFp(this.configuration).systemTypeApplyDataRemediation(
      body,
      options
    )(this.fetch, this.basePath);
  }

  /**
   *
   * @param {SystemTypeDtoCreate} body
   * @param {boolean} [forced]
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof SystemTypeApi
   */
  public systemTypeCreate(
    body: SystemTypeDtoCreate,
    forced?: boolean,
    options?: any
  ) {
    return SystemTypeApiFp(this.configuration).systemTypeCreate(
      body,
      forced,
      options
    )(this.fetch, this.basePath);
  }

  /**
   *
   * @param {number} [id]
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof SystemTypeApi
   */
  public systemTypeDelete(id?: number, options?: any) {
    return SystemTypeApiFp(this.configuration).systemTypeDelete(id, options)(
      this.fetch,
      this.basePath
    );
  }

  /**
   *
   * @param {number} [id]
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof SystemTypeApi
   */
  public systemTypeDeleteDeep(id?: number, options?: any) {
    return SystemTypeApiFp(this.configuration).systemTypeDeleteDeep(
      id,
      options
    )(this.fetch, this.basePath);
  }

  /**
   *
   * @param {SystemTypeQueryDto} body
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof SystemTypeApi
   */
  public systemTypeExportReport(body: SystemTypeQueryDto, options?: any) {
    return SystemTypeApiFp(this.configuration).systemTypeExportReport(
      body,
      options
    )(this.fetch, this.basePath);
  }

  public systemTypeGetVodafoneNameResource(id?: number, options?: any) {
    return SystemTypeApiFp(
      this.configuration
    ).systemTypeGetVodafoneNameResource(id, options)(this.fetch, this.basePath);
  }

  public systemTypeGetVodafoneNameResourceWizardMode(
    id?: number,
    options?: any
  ) {
    return SystemTypeApiFp(
      this.configuration
    ).systemTypeGetVodafoneNameResourceWizardMode(id, options)(
      this.fetch,
      this.basePath
    );
  }

  /**
   *
   * @param {number} [assetCategoryId]
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof SystemTypeApi
   */
  public systemTypeGetAssetCategoryReleated(
    assetCategoryId?: number,
    options?: any
  ) {
    return SystemTypeApiFp(
      this.configuration
    ).systemTypeGetAssetCategoryReleated(assetCategoryId, options)(
      this.fetch,
      this.basePath
    );
  }

  /**
   *
   * @param {number} [majorSoftwareBuild]
   * @param {Array<number>} [majorHardwareBuilds]
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof SystemTypeApi
   */
  public systemTypeGetCostraintInfo(
    majorSoftwareBuild?: number,
    majorHardwareBuilds?: Array<number>,
    options?: any
  ) {
    return SystemTypeApiFp(this.configuration).systemTypeGetCostraintInfo(
      majorSoftwareBuild,
      majorHardwareBuilds,
      options
    )(this.fetch, this.basePath);
  }

  /**
   *
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof SystemTypeApi
   */
  public systemTypeGetCreateResourceSystemType(options?: any) {
    return SystemTypeApiFp(
      this.configuration
    ).systemTypeGetCreateResourceSystemType(options)(this.fetch, this.basePath);
  }

  /**
   *
   * @param {SystemTypeQueryDto} body
   * @param {string} [propertyName]
   * @param {string} [propertyFilter]
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof SystemTypeApi
   */
  public systemTypeGetFilterResult(
    body: SystemTypeQueryDto,
    propertyName?: string,
    propertyFilter?: string,
    options?: any
  ) {
    return SystemTypeApiFp(this.configuration).systemTypeGetFilterResult(
      body,
      propertyName,
      propertyFilter,
      options
    )(this.fetch, this.basePath);
  }

  /**
   *
   * @param {Array<number>} [systemTypeId]
   * @param {Array<string>} [systemTypeNameVodafone]
   * @param {Array<string>} [systemTypeName3Gpp]
   * @param {Array<string>} [systemTypeNameOem]
   * @param {Array<number>} [majorSoftwareBuild]
   * @param {Array<string>} [majorHardwareBuild]
   * @param {Array<number>} [systemSolution]
   * @param {Date} [constraintScalingStartDate]
   * @param {Date} [constraintScalingEndDate]
   * @param {Array<string>} [constraintLcm]
   * @param {Date} [endOfMaintenanceStartDate]
   * @param {Date} [endOfMaintenanceEndDate]
   * @param {Date} [lastModifiedStartDate]
   * @param {Date} [lastModifiedEndDate]
   * @param {Array<number>} [productImportance]
   * @param {Array<number>} [verticalResponsible]
   * @param {Array<number>} [subDomainResponsible]
   * @param {Array<string>} [subDomainSpoc]
   * @param {Array<number>} [assetCategory]
   * @param {Array<number>} [assetClass]
   * @param {Array<string>} [assetType]
   * @param {Array<string>} [spareFieldsJson]
   * @param {Array<number>} [softwareOem]
   * @param {Array<number>} [hardwareOem]
   * @param {Array<string>} [lastModifiedBy]
   * @param {string} [sortBy]
   * @param {boolean} [isSortAscending]
   * @param {number} [page]
   * @param {number} [pageSize]
   * @param {number} [principalId]
   * @param {boolean} [deleted]
   * @param {boolean} [orphan]
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof SystemTypeApi
   */
  public systemTypeGetGridGrouped(
    systemTypeId?: Array<number>,
    systemTypeNameVodafone?: Array<string>,
    systemTypeName3Gpp?: Array<string>,
    systemTypeNameOem?: Array<string>,
    majorSoftwareBuild?: Array<number>,
    majorHardwareBuild?: Array<string>,
    systemSolution?: Array<number>,
    constraintScalingStartDate?: Date,
    constraintScalingEndDate?: Date,
    constraintLcm?: Array<string>,
    endOfMaintenanceStartDate?: Date,
    endOfMaintenanceEndDate?: Date,
    lastModifiedStartDate?: Date,
    lastModifiedEndDate?: Date,
    productImportance?: Array<number>,
    verticalResponsible?: Array<number>,
    subDomainResponsible?: Array<number>,
    subDomainSpoc?: Array<string>,
    assetCategory?: Array<number>,
    assetClass?: Array<number>,
    assetType?: Array<string>,
    spareFieldsJson?: Array<string>,
    softwareOem?: Array<number>,
    hardwareOem?: Array<number>,
    lastModifiedBy?: Array<string>,
    sortBy?: string,
    isSortAscending?: boolean,
    page?: number,
    pageSize?: number,
    principalId?: number,
    deleted?: boolean,
    orphan?: boolean,
    options?: any
  ) {
    return SystemTypeApiFp(this.configuration).systemTypeGetGridGrouped(
      systemTypeId,
      systemTypeNameVodafone,
      systemTypeName3Gpp,
      systemTypeNameOem,
      majorSoftwareBuild,
      majorHardwareBuild,
      systemSolution,
      constraintScalingStartDate,
      constraintScalingEndDate,
      constraintLcm,
      endOfMaintenanceStartDate,
      endOfMaintenanceEndDate,
      lastModifiedStartDate,
      lastModifiedEndDate,
      productImportance,
      verticalResponsible,
      subDomainResponsible,
      subDomainSpoc,
      assetCategory,
      assetClass,
      assetType,
      spareFieldsJson,
      softwareOem,
      hardwareOem,
      lastModifiedBy,
      sortBy,
      isSortAscending,
      page,
      pageSize,
      principalId,
      deleted,
      orphan,
      options
    )(this.fetch, this.basePath);
  }

  /**
   *
   * @param {number} [majorSoftwareId]
   * @param {Array<number>} [majorHardwareIds]
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof SystemTypeApi
   */
  public systemTypeGetMinorDateFromMajorEntity(
    majorSoftwareId?: number,
    majorHardwareIds?: Array<number>,
    options?: any
  ) {
    return SystemTypeApiFp(
      this.configuration
    ).systemTypeGetMinorDateFromMajorEntity(
      majorSoftwareId,
      majorHardwareIds,
      options
    )(this.fetch, this.basePath);
  }

  /**
   *
   * @param {number} id
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof SystemTypeApi
   */
  public systemTypeGetRelatedRecords(id: number, options?: any) {
    return SystemTypeApiFp(this.configuration).systemTypeGetRelatedRecords(
      id,
      options
    )(this.fetch, this.basePath);
  }

  /**
   *
   * @param {number} id
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof SystemTypeApi
   */
  public systemTypeGetSingleSystemType(id: number, options?: any) {
    return SystemTypeApiFp(this.configuration).systemTypeGetSingleSystemType(
      id,
      options
    )(this.fetch, this.basePath);
  }

  /**
   *
   * @param {string} [nameOEM]
   * @param {number} [majorHardwareBuilds]
   * @param {number} [majorSoftwareBuild]
   * @param {Array<number>} [majorHardwareList]
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof SystemTypeApi
   */
  public systemTypeGetSystemSolutionName(
    nameOEM?: string,
    majorHardwareBuilds?: number,
    majorSoftwareBuild?: number,
    majorHardwareList?: Array<number>,
    options?: any
  ) {
    return SystemTypeApiFp(this.configuration).systemTypeGetSystemSolutionName(
      nameOEM,
      majorHardwareBuilds,
      majorSoftwareBuild,
      majorHardwareList,
      options
    )(this.fetch, this.basePath);
  }

  /**
   *
   * @param {number} [majorHardwareBuildId]
   * @param {number} [majorSoftwareBuildId]
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof SystemTypeApi
   */
  public systemTypeGetAllSubdomainAndVerticalResponsibles(
    majorHardwareBuildId?: number,
    majorSoftwareBuildId?: number,
    options?: any
  ) {
    return SystemTypeApiFp(
      this.configuration
    ).systemTypeGetAllSubdomainAndVerticalResponsibles(
      majorHardwareBuildId,
      majorSoftwareBuildId,
      options
    )(this.fetch, this.basePath);
  }

  /**
   *
   * @param {SystemTypeQueryDto} body
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof SystemTypeApi
   */
  public systemTypeGetSystemType(body: SystemTypeQueryDto, options?: any) {
    return SystemTypeApiFp(this.configuration).systemTypeGetSystemType(
      body,
      options
    )(this.fetch, this.basePath);
  }

  /**
   *
   * @param {SystemTypeQueryDto} body
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof SystemTypeApi
   */
  public systemTypeGetSystemTypeExcel(body: SystemTypeQueryDto, options?: any) {
    return SystemTypeApiFp(this.configuration).systemTypeGetSystemTypeExcel(
      body,
      options
    )(this.fetch, this.basePath);
  }

  /**
   *
   * @param {number} id
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof SystemTypeApi
   */
  public systemTypeGetUpdateResourceSystemType(id: number, options?: any) {
    return SystemTypeApiFp(
      this.configuration
    ).systemTypeGetUpdateResourceSystemType(id, options)(
      this.fetch,
      this.basePath
    );
  }

  /**
   *
   * @param {SystemTypeDtoUpdate} body
   * @param {boolean} [forced]
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof SystemTypeApi
   */
  public systemTypePut(
    body: SystemTypeDtoUpdate,
    forced?: boolean,
    options?: any
  ) {
    return SystemTypeApiFp(this.configuration).systemTypePut(
      body,
      forced,
      options
    )(this.fetch, this.basePath);
  }

  /**
   *
   * @param {number} [id]
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof SystemTypeApi
   */
  public systemTypeRestore(id?: number, options?: any) {
    return SystemTypeApiFp(this.configuration).systemTypeRestore(id, options)(
      this.fetch,
      this.basePath
    );
  }
}
