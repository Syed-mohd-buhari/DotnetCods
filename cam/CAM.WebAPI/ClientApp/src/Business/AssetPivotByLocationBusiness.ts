import * as url from "url";
import * as portableFetch from "portable-fetch";
import * as isomorphicFetch from "isomorphic-fetch";
import { Configuration } from "./Common/configuration";
import { ResultDto } from "../Model/CommonModels";
import {
  BASE_PATH,
  FetchArgs,
  FetchAPI,
  RequiredError,
  FilterValueDto,
} from "./Common/CommonBusiness";
import { BaseAPI } from "./Common/CommonBusiness";
import { FileResult, ReturnFile } from "../Model/Common";
import { headerObj } from "./header";

import {
  ReportPATQueryObjectGrid,
  SWOEM_MODEL,
} from "../Model/Report/PlannedActivityTrackerModel";
import {
  AssetPivotByLocationDto,
  AssetPivotByLocationQueryObjectGrid,
} from "../Model/Report/AssetPivotByLocationModel";
/**
 * AssetPivotByLocationAPI - fetch parameter creator
 * @export
 */
export const AssetPivotByLocationCall = function (
  configuration?: Configuration
) {
  return {
    /**
     *
     * @param {Array<number>} [opCo]
     * @param {Array<number>} [dcId]
     * @param {Array<number>} [hardwareType]
     * @param {Array<number>} [deploymentStatus]
     * @param {Array<number>} [paImplementaionYear]
     * @param {string} [sortBy]
     * @param {boolean} [isSortAscending]
     * @param {number} [page]
     * @param {number} [pageSize]
     * @param {Date} [lastModifiedStartDate]
     * @param {Date} [lastModifiedEndDate]
     * @param {number} [principalId]
     * @param {boolean} [deleted]
     * @param {boolean} [orphan]
     * @param {Array<string>} [lastModifiedBy]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    AssetPivotByLocationExport(
      opCo?: Array<number>,
      dcId?: Array<number>,
      hardwareType?: Array<number>,
      deploymentStatus?: Array<number>,
      paImplementaionYear?: Array<number>,
      sortBy?: string,
      isSortAscending?: boolean,
      page?: number,
      pageSize?: number,
      principalId?: number,
      deleted?: boolean,
      orphan?: boolean,
      lastModifiedBy?: any,
      options: any = {}
    ): FetchArgs {
      const localVarPath = `/api/AssetPivotReport/ExportAssetPivotReport`;
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

      if (opCo) {
        localVarQueryParameter["opCo"] = opCo;
      }

      if (hardwareType) {
        localVarQueryParameter["hardwareType"] = hardwareType;
      }
      if (paImplementaionYear) {
        localVarQueryParameter["paImplementaionYear"] = paImplementaionYear;
      }

      if (dcId) {
        localVarQueryParameter["dcId"] = dcId;
      }

      if (deploymentStatus) {
        localVarQueryParameter["deploymentStatus"] = deploymentStatus;
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

      if (lastModifiedBy) {
        localVarQueryParameter["LastModifiedBy"] = lastModifiedBy;
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
     * @param {AssetPivotByLocationQueryObjectGrid} body
     * @param {string} [propertyName]
     * @param {string} [propertyFilter]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    AssetPivotByLocationFilterResult(
      body: AssetPivotByLocationQueryObjectGrid,
      propertyName?: string,
      propertyFilter?: string,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling reportHardwareGetFilterResult."
        );
      }

      const localVarPath = `/api/AssetPivotReport/Filter`;
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

      console.log("body => ", body);

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
        <any>"ReportHardwareQueryDto" !== "string" ||
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
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    AssetPivotByLocationGetAll(
      body: AssetPivotByLocationQueryObjectGrid,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling AssetPivotByLocationGetAll."
        );
      }
      const localVarPath = `/api/AssetPivotReport/GetAssetPivot`;
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
        <any>"VolteKPIQueryDto" !== "string" ||
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
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    getDropdownData(options: any = {}): FetchArgs {
      // verify required parameter 'id' is not null or undefined

      const localVarPath = "/api/AssetPivotReport/GetPivotFilterAllDropDown";
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
  };
};

/**
 * AssetPivotByLocationAPI - functional programming interface
 * @export
 */
export const AssetPivotByLocationAPIFp = function (
  configuration?: Configuration
) {
  return {
    /**
     *
     * @param {Array<number>} [opCo]
     * @param {Array<number>} [dcId]
     * @param {Array<number>} [hardwareType]
     * @param {Array<number>} [deploymentStatus]
     * @param {Array<number>} [paImplementaionYear]
     * @param {string} [sortBy]
     * @param {boolean} [isSortAscending]
     * @param {number} [page]
     * @param {number} [pageSize]
     * @param {Date} [lastModifiedStartDate]
     * @param {Date} [lastModifiedEndDate]
     * @param {number} [principalId]
     * @param {boolean} [deleted]
     * @param {boolean} [orphan]
     * @param {Array<string>} [lastModifiedBy]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    AssetPivotByLocationExport(
      opCo?: Array<number>,
      dcId?: Array<number>,
      hardwareType?: Array<number>,
      deploymentStatus?: Array<number>,
      paImplementaionYear?: Array<number>,
      sortBy?: string,
      isSortAscending?: boolean,
      page?: number,
      pageSize?: number,
      principalId?: number,
      deleted?: boolean,
      orphan?: boolean,
      lastModifiedBy?: any,
      options: any = {}
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ReturnFile> {
      const localVarFetchArgs = AssetPivotByLocationCall(
        configuration
      ).AssetPivotByLocationExport(
        opCo,
        dcId,
        hardwareType,
        deploymentStatus,
        paImplementaionYear,
        sortBy,
        isSortAscending,
        page,
        pageSize,
        principalId,
        deleted,
        orphan,
        lastModifiedBy,
        options
      );
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
     * @param {AssetPivotByLocationQueryObjectGrid} body
     * @param {string} [propertyName]
     * @param {string} [propertyFilter]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    AssetPivotByLocationFilterResult(
      body: AssetPivotByLocationQueryObjectGrid,
      propertyName?: string,
      propertyFilter?: string,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<Array<FilterValueDto>> {
      const localVarFetchArgs = AssetPivotByLocationCall(
        configuration
      ).AssetPivotByLocationFilterResult(
        body,
        propertyName,
        propertyFilter,
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
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    AssetPivotByLocationGetAll(
      options?: any
    ): (
      fetch?: FetchAPI,
      basePath?: string
    ) => Promise<AssetPivotByLocationDto> {
      const localVarFetchArgs =
        AssetPivotByLocationCall(configuration).AssetPivotByLocationGetAll(
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
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    getDropdownData(
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<SWOEM_MODEL> {
      const localVarFetchArgs =
        AssetPivotByLocationCall(configuration).getDropdownData(options);
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
 * AssetPivotByLocationAPI - factory interface
 * @export
 */
export const AssetPivotByLocationAPIFactory = function (
  configuration?: Configuration,
  fetch?: FetchAPI,
  basePath?: string
) {
  return {
    /**
     *
     * @param {Array<number>} [opCo]
     * @param {Array<number>} [dcId]
     * @param {Array<number>} [hardwareType]
     * @param {Array<number>} [deploymentStatus]
     * @param {Array<number>} [paImplementaionYear]
     * @param {string} [sortBy]
     * @param {boolean} [isSortAscending]
     * @param {number} [page]
     * @param {number} [pageSize]
     * @param {Date} [lastModifiedStartDate]
     * @param {Date} [lastModifiedEndDate]
     * @param {number} [principalId]
     * @param {boolean} [deleted]
     * @param {boolean} [orphan]
     * @param {Array<string>} [lastModifiedBy]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    AssetPivotByLocationExport(
      opCo?: Array<number>,
      dcId?: Array<number>,
      hardwareType?: Array<number>,
      deploymentStatus?: Array<number>,
      paImplementaionYear?: Array<number>,
      sortBy?: string,
      isSortAscending?: boolean,
      page?: number,
      pageSize?: number,
      principalId?: number,
      deleted?: boolean,
      orphan?: boolean,
      lastModifiedBy?: any,
      options: any = {}
    ) {
      return AssetPivotByLocationAPIFp(
        configuration
      ).AssetPivotByLocationExport(
        opCo,
        dcId,
        hardwareType,
        deploymentStatus,
        paImplementaionYear,
        sortBy,
        isSortAscending,
        page,
        pageSize,
        principalId,
        deleted,
        orphan,
        lastModifiedBy,
        options
      )(fetch, basePath);
    },

    /**
     *
     * @param {AssetPivotByLocationQueryObjectGrid} body
     * @param {string} [propertyName]
     * @param {string} [propertyFilter]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    AssetPivotByLocationFilterResult(
      body: AssetPivotByLocationQueryObjectGrid,
      propertyName?: string,
      propertyFilter?: string,
      options?: any
    ) {
      return AssetPivotByLocationAPIFp(
        configuration
      ).AssetPivotByLocationFilterResult(
        body,
        propertyName,
        propertyFilter,
        options
      )(fetch, basePath);
    },

    /**
     *
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    AssetPivotByLocationGetAll(options?: any) {
      return AssetPivotByLocationAPIFp(
        configuration
      ).AssetPivotByLocationGetAll(options)(fetch, basePath);
    },

    /**
     *
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    getDropdownData(options?: any) {
      return AssetPivotByLocationAPIFp(configuration).getDropdownData(options)(
        fetch,
        basePath
      );
    },
  };
};

/**
 * AssetPivotByLocationAPI - object-oriented interface
 * @export
 * @class AssetPivotByLocationAPI
 * @extends {BaseAPI}
 */
export class AssetPivotByLocation extends BaseAPI {
  /**
   *
   * @param {Array<number>} [opCo]
   * @param {Array<number>} [dcId]
   * @param {Array<number>} [hardwareType]
   * @param {Array<number>} [deploymentStatus]
   * @param {Array<number>} [paImplementaionYear]
   * @param {string} [sortBy]
   * @param {boolean} [isSortAscending]
   * @param {number} [page]
   * @param {number} [pageSize]
   * @param {Date} [lastModifiedStartDate]
   * @param {Date} [lastModifiedEndDate]
   * @param {number} [principalId]
   * @param {boolean} [deleted]
   * @param {boolean} [orphan]
   * @param {Array<string>} [lastModifiedBy]
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof AssetPivotByLocationAPI
   */
  public AssetPivotByLocationExport(
    opCo?: Array<number>,
    dcId?: Array<number>,
    hardwareType?: Array<number>,
    deploymentStatus?: Array<number>,
    paImplementaionYear?: Array<number>,
    sortBy?: string,
    isSortAscending?: boolean,
    page?: number,
    pageSize?: number,
    principalId?: number,
    deleted?: boolean,
    orphan?: boolean,
    lastModifiedBy?: any,
    options: any = {}
  ) {
    return AssetPivotByLocationAPIFp(
      this.configuration
    ).AssetPivotByLocationExport(
      opCo,
      dcId,
      hardwareType,
      deploymentStatus,
      paImplementaionYear,
      sortBy,
      isSortAscending,
      page,
      pageSize,
      principalId,
      deleted,
      orphan,
      lastModifiedBy,
      options
    )(this.fetch, this.basePath);
  }

  /**
   *
   * @param {AssetPivotByLocationQueryObjectGrid} body
   * @param {string} [propertyName]
   * @param {string} [propertyFilter]
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof AssetPivotByLocationAPI
   */
  public AssetPivotByLocationFilterResult(
    body: AssetPivotByLocationQueryObjectGrid,
    propertyName?: string,
    propertyFilter?: string,
    options?: any
  ) {
    return AssetPivotByLocationAPIFp(
      this.configuration
    ).AssetPivotByLocationFilterResult(
      body,
      propertyName,
      propertyFilter,
      options
    )(this.fetch, this.basePath);
  }

  /**
   *
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof AssetPivotByLocationAPI
   */
  public AssetPivotByLocationGetAll(options?: any) {
    return AssetPivotByLocationAPIFp(
      this.configuration
    ).AssetPivotByLocationGetAll(options)(this.fetch, this.basePath);
  }

  /**
   *
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof AssetPivotByLocationAPI
   */
  public getDropdownData(options?: any) {
    return AssetPivotByLocationAPIFp(this.configuration).getDropdownData(
      options
    )(this.fetch, this.basePath);
  }
}
