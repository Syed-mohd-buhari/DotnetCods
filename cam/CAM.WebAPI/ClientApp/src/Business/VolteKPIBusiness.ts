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
import { ReturnFile } from "../Model/Common";
import { headerObj } from "./header";

import {
  VolteKPIDtoCreate,
  VolteKPIDtoUpdate,
  QueryResultDtoOfVolteKPIDtoGrid,
  VolteKPIType,
  VolteKPIDashboardDto,
} from "../Model/VolteKpi/VolteKPI";
import { VolteKPIReportDto } from "../Model/Report/ReportVolteKPIModel";
import { VolteKPIQueryObjectGrid as VolteKPIQueryDto } from "../Model/VolteKpi/VolteKPI";
/**
 * VolteKPIApi - fetch parameter creator
 * @export
 */
export const VolteKPIApiFetchParamCreator = function (
  configuration?: Configuration
) {
  return {
    /**
     *
     * @param {number} [id]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    volteKPIDelete(id?: number, options: any = {}): FetchArgs {
      const localVarPath = `/api/VolteKPI/Delete`;
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
    volteKPIDeleteDeep(id?: number, options: any = {}): FetchArgs {
      const localVarPath = `/api/VolteKPI/DeleteDeep`;
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
     * @param {Array<number>} [volteKPIId]
     * @param {Array<number>} [opCo]
     * @param {Array<VolteKPIType>} [volteKPITypes]
     * @param {number} [month]
     * @param {number} [year]
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
    volteKPIExportReport(
      volteKPIId?: Array<number>,
      opCo?: Array<number>,
      volteKPITypes?: Array<VolteKPIType>,
      month?: number,
      year?: number,
      sortBy?: string,
      isSortAscending?: boolean,
      page?: number,
      pageSize?: number,
      lastModifiedStartDate?: Date,
      lastModifiedEndDate?: Date,
      principalId?: number,
      deleted?: boolean,
      orphan?: boolean,
      lastModifiedBy?: Array<string>,
      options: any = {}
    ): FetchArgs {
      const localVarPath = `/api/VolteKPI/ExportReport`;
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

      if (volteKPIId) {
        localVarQueryParameter["VolteKPIId"] = volteKPIId;
      }

      if (opCo) {
        localVarQueryParameter["OpCo"] = opCo;
      }

      if (volteKPITypes) {
        localVarQueryParameter["VolteKPITypes"] = volteKPITypes;
      }

      if (month !== undefined) {
        localVarQueryParameter["Month"] = month;
      }

      if (year !== undefined) {
        localVarQueryParameter["Year"] = year;
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
     * @param {VolteKPIQueryDto} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    volteKPIGetDashboard(body: VolteKPIQueryDto, options: any = {}): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling volteKPIGetDashboard."
        );
      }
      const localVarPath = `/api/VolteKPI/GetDashboard`;
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
     * @param {VolteKPIQueryDto} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    volteKPIGetDashboardReports(
      body: VolteKPIQueryDto,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling volteKPIGetDashboardReports."
        );
      }
      const localVarPath = `/api/VolteKPI/GetDashboardReports`;
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
     * @param {VolteKPIQueryDto} body
     * @param {string} [propertyName]
     * @param {string} [propertyFilter]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    volteKPIGetFilterResult(
      body: VolteKPIQueryDto,
      propertyName?: string,
      propertyFilter?: string,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling volteKPIGetFilterResult."
        );
      }
      const localVarPath = `/api/VolteKPI/Filter`;
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
     * @param {VolteKPIQueryDto} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    volteKPIGetKPI(body: VolteKPIQueryDto, options: any = {}): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling volteKPIGetKPI."
        );
      }
      const localVarPath = `/api/VolteKPI/KPIGet`;
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
     * @param {number} id
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    volteKPIGetRelatedRecords(id: number, options: any = {}): FetchArgs {
      // verify required parameter 'id' is not null or undefined
      if (id === null || id === undefined) {
        throw new RequiredError(
          "id",
          "Required parameter id was null or undefined when calling volteKPIGetRelatedRecords."
        );
      }
      const localVarPath = `/api/VolteKPI/GetRelatedRecords{id}`.replace(
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
    volteKPIGetUpdateResourceVolteKPI(
      id: number,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'id' is not null or undefined
      if (id === null || id === undefined) {
        throw new RequiredError(
          "id",
          "Required parameter id was null or undefined when calling volteKPIGetUpdateResourceVolteKPI."
        );
      }
      const localVarPath = `/api/VolteKPI/Update{id}`.replace(
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
     * @param {VolteKPIQueryDto} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    volteKPIGetVolteKPI(body: VolteKPIQueryDto, options: any = {}): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling volteKPIGetVolteKPI."
        );
      }
      const localVarPath = `/api/VolteKPI/GetVolteKPI`;
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
     * @param {VolteKPIDtoUpdate} body
     * @param {boolean} [forced]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    volteKPIPut(
      body: VolteKPIDtoUpdate,
      forced?: boolean,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling volteKPIPut."
        );
      }
      const localVarPath = `/api/VolteKPI`;
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
        <any>"VolteKPIDtoUpdate" !== "string" ||
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
    volteKPIRestore(id?: number, options: any = {}): FetchArgs {
      const localVarPath = `/api/VolteKPI/Restore`;
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
    /**
     *
     * @param {VolteKPIDtoCreate} body
     * @param {boolean} [forced]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    volteKPISaveOrEdit(
      body: VolteKPIDtoCreate,
      forced?: boolean,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling volteKPISaveOrEdit."
        );
      }
      const localVarPath = `/api/VolteKPI/KPI`;
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
        <any>"VolteKPIDtoCreate" !== "string" ||
        localVarRequestOptions.headers["Content-Type"] === "application/json";
      localVarRequestOptions.body = needsSerialization
        ? JSON.stringify(body || {})
        : body || "";

      return {
        url: url.format(localVarUrlObj),
        options: localVarRequestOptions,
      };
    },
  };
};

/**
 * VolteKPIApi - functional programming interface
 * @export
 */
export const VolteKPIApiFp = function (configuration?: Configuration) {
  return {
    /**
     *
     * @param {number} [id]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    volteKPIDelete(
      id?: number,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = VolteKPIApiFetchParamCreator(
        configuration
      ).volteKPIDelete(id, options);
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
    volteKPIDeleteDeep(
      id?: number,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = VolteKPIApiFetchParamCreator(
        configuration
      ).volteKPIDeleteDeep(id, options);
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
     * @param {Array<number>} [volteKPIId]
     * @param {Array<number>} [opCo]
     * @param {Array<VolteKPIType>} [volteKPITypes]
     * @param {number} [month]
     * @param {number} [year]
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
    volteKPIExportReport(
      volteKPIId?: Array<number>,
      opCo?: Array<number>,
      volteKPITypes?: Array<VolteKPIType>,
      month?: number,
      year?: number,
      sortBy?: string,
      isSortAscending?: boolean,
      page?: number,
      pageSize?: number,
      lastModifiedStartDate?: Date,
      lastModifiedEndDate?: Date,
      principalId?: number,
      deleted?: boolean,
      orphan?: boolean,
      lastModifiedBy?: Array<string>,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ReturnFile> {
      const localVarFetchArgs = VolteKPIApiFetchParamCreator(
        configuration
      ).volteKPIExportReport(
        volteKPIId,
        opCo,
        volteKPITypes,
        month,
        year,
        sortBy,
        isSortAscending,
        page,
        pageSize,
        lastModifiedStartDate,
        lastModifiedEndDate,
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
     * @param {VolteKPIQueryDto} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    volteKPIGetDashboard(
      body: VolteKPIQueryDto,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<VolteKPIDashboardDto> {
      const localVarFetchArgs = VolteKPIApiFetchParamCreator(
        configuration
      ).volteKPIGetDashboard(body, options);
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
     * @param {VolteKPIQueryDto} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    volteKPIGetDashboardReports(
      body: VolteKPIQueryDto,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<VolteKPIReportDto> {
      const localVarFetchArgs = VolteKPIApiFetchParamCreator(
        configuration
      ).volteKPIGetDashboardReports(body, options);
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
     * @param {VolteKPIQueryDto} body
     * @param {string} [propertyName]
     * @param {string} [propertyFilter]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    volteKPIGetFilterResult(
      body: VolteKPIQueryDto,
      propertyName?: string,
      propertyFilter?: string,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<Array<FilterValueDto>> {
      const localVarFetchArgs = VolteKPIApiFetchParamCreator(
        configuration
      ).volteKPIGetFilterResult(body, propertyName, propertyFilter, options);
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
     * @param {VolteKPIQueryDto} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    volteKPIGetKPI(
      body: VolteKPIQueryDto,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<VolteKPIDtoCreate> {
      const localVarFetchArgs = VolteKPIApiFetchParamCreator(
        configuration
      ).volteKPIGetKPI(body, options);
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
    volteKPIGetRelatedRecords(
      id: number,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = VolteKPIApiFetchParamCreator(
        configuration
      ).volteKPIGetRelatedRecords(id, options);
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
    volteKPIGetUpdateResourceVolteKPI(
      id: number,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<VolteKPIDtoUpdate> {
      const localVarFetchArgs = VolteKPIApiFetchParamCreator(
        configuration
      ).volteKPIGetUpdateResourceVolteKPI(id, options);
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
     * @param {VolteKPIQueryDto} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    volteKPIGetVolteKPI(
      body: VolteKPIQueryDto,
      options?: any
    ): (
      fetch?: FetchAPI,
      basePath?: string
    ) => Promise<QueryResultDtoOfVolteKPIDtoGrid> {
      const localVarFetchArgs = VolteKPIApiFetchParamCreator(
        configuration
      ).volteKPIGetVolteKPI(body, options);
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
     * @param {VolteKPIDtoUpdate} body
     * @param {boolean} [forced]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    volteKPIPut(
      body: VolteKPIDtoUpdate,
      forced?: boolean,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = VolteKPIApiFetchParamCreator(
        configuration
      ).volteKPIPut(body, forced, options);
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
    volteKPIRestore(
      id?: number,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = VolteKPIApiFetchParamCreator(
        configuration
      ).volteKPIRestore(id, options);
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
     * @param {VolteKPIDtoCreate} body
     * @param {boolean} [forced]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    volteKPISaveOrEdit(
      body: VolteKPIDtoCreate,
      forced?: boolean,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = VolteKPIApiFetchParamCreator(
        configuration
      ).volteKPISaveOrEdit(body, forced, options);
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
 * VolteKPIApi - factory interface
 * @export
 */
export const VolteKPIApiFactory = function (
  configuration?: Configuration,
  fetch?: FetchAPI,
  basePath?: string
) {
  return {
    /**
     *
     * @param {number} [id]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    volteKPIDelete(id?: number, options?: any) {
      return VolteKPIApiFp(configuration).volteKPIDelete(id, options)(
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
    volteKPIDeleteDeep(id?: number, options?: any) {
      return VolteKPIApiFp(configuration).volteKPIDeleteDeep(id, options)(
        fetch,
        basePath
      );
    },
    /**
     *
     * @param {Array<number>} [volteKPIId]
     * @param {Array<number>} [opCo]
     * @param {Array<VolteKPIType>} [volteKPITypes]
     * @param {number} [month]
     * @param {number} [year]
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
    volteKPIExportReport(
      volteKPIId?: Array<number>,
      opCo?: Array<number>,
      volteKPITypes?: Array<VolteKPIType>,
      month?: number,
      year?: number,
      sortBy?: string,
      isSortAscending?: boolean,
      page?: number,
      pageSize?: number,
      lastModifiedStartDate?: Date,
      lastModifiedEndDate?: Date,
      principalId?: number,
      deleted?: boolean,
      orphan?: boolean,
      lastModifiedBy?: Array<string>,
      options?: any
    ) {
      return VolteKPIApiFp(configuration).volteKPIExportReport(
        volteKPIId,
        opCo,
        volteKPITypes,
        month,
        year,
        sortBy,
        isSortAscending,
        page,
        pageSize,
        lastModifiedStartDate,
        lastModifiedEndDate,
        principalId,
        deleted,
        orphan,
        lastModifiedBy,
        options
      )(fetch, basePath);
    },
    /**
     *
     * @param {VolteKPIQueryDto} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    volteKPIGetDashboard(body: VolteKPIQueryDto, options?: any) {
      return VolteKPIApiFp(configuration).volteKPIGetDashboard(body, options)(
        fetch,
        basePath
      );
    },
    /**
     *
     * @param {VolteKPIQueryDto} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    volteKPIGetDashboardReports(body: VolteKPIQueryDto, options?: any) {
      return VolteKPIApiFp(configuration).volteKPIGetDashboardReports(
        body,
        options
      )(fetch, basePath);
    },
    /**
     *
     * @param {VolteKPIQueryDto} body
     * @param {string} [propertyName]
     * @param {string} [propertyFilter]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    volteKPIGetFilterResult(
      body: VolteKPIQueryDto,
      propertyName?: string,
      propertyFilter?: string,
      options?: any
    ) {
      return VolteKPIApiFp(configuration).volteKPIGetFilterResult(
        body,
        propertyName,
        propertyFilter,
        options
      )(fetch, basePath);
    },
    /**
     *
     * @param {VolteKPIQueryDto} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    volteKPIGetKPI(body: VolteKPIQueryDto, options?: any) {
      return VolteKPIApiFp(configuration).volteKPIGetKPI(body, options)(
        fetch,
        basePath
      );
    },
    /**
     *
     * @param {number} id
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    volteKPIGetRelatedRecords(id: number, options?: any) {
      return VolteKPIApiFp(configuration).volteKPIGetRelatedRecords(
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
    volteKPIGetUpdateResourceVolteKPI(id: number, options?: any) {
      return VolteKPIApiFp(configuration).volteKPIGetUpdateResourceVolteKPI(
        id,
        options
      )(fetch, basePath);
    },
    /**
     *
     * @param {VolteKPIQueryDto} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    volteKPIGetVolteKPI(body: VolteKPIQueryDto, options?: any) {
      return VolteKPIApiFp(configuration).volteKPIGetVolteKPI(body, options)(
        fetch,
        basePath
      );
    },
    /**
     *
     * @param {VolteKPIDtoUpdate} body
     * @param {boolean} [forced]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    volteKPIPut(body: VolteKPIDtoUpdate, forced?: boolean, options?: any) {
      return VolteKPIApiFp(configuration).volteKPIPut(
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
    volteKPIRestore(id?: number, options?: any) {
      return VolteKPIApiFp(configuration).volteKPIRestore(id, options)(
        fetch,
        basePath
      );
    },
    /**
     *
     * @param {VolteKPIDtoCreate} body
     * @param {boolean} [forced]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    volteKPISaveOrEdit(
      body: VolteKPIDtoCreate,
      forced?: boolean,
      options?: any
    ) {
      return VolteKPIApiFp(configuration).volteKPISaveOrEdit(
        body,
        forced,
        options
      )(fetch, basePath);
    },
  };
};

/**
 * VolteKPIApi - object-oriented interface
 * @export
 * @class VolteKPIApi
 * @extends {BaseAPI}
 */
export class VolteKPIApi extends BaseAPI {
  /**
   *
   * @param {number} [id]
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof VolteKPIApi
   */
  public volteKPIDelete(id?: number, options?: any) {
    return VolteKPIApiFp(this.configuration).volteKPIDelete(id, options)(
      this.fetch,
      this.basePath
    );
  }

  /**
   *
   * @param {number} [id]
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof VolteKPIApi
   */
  public volteKPIDeleteDeep(id?: number, options?: any) {
    return VolteKPIApiFp(this.configuration).volteKPIDeleteDeep(id, options)(
      this.fetch,
      this.basePath
    );
  }

  /**
   *
   * @param {Array<number>} [volteKPIId]
   * @param {Array<number>} [opCo]
   * @param {Array<VolteKPIType>} [volteKPITypes]
   * @param {number} [month]
   * @param {number} [year]
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
   * @memberof VolteKPIApi
   */
  public volteKPIExportReport(
    volteKPIId?: Array<number>,
    opCo?: Array<number>,
    volteKPITypes?: Array<VolteKPIType>,
    month?: number,
    year?: number,
    sortBy?: string,
    isSortAscending?: boolean,
    page?: number,
    pageSize?: number,
    lastModifiedStartDate?: Date,
    lastModifiedEndDate?: Date,
    principalId?: number,
    deleted?: boolean,
    orphan?: boolean,
    lastModifiedBy?: Array<string>,
    options?: any
  ) {
    return VolteKPIApiFp(this.configuration).volteKPIExportReport(
      volteKPIId,
      opCo,
      volteKPITypes,
      month,
      year,
      sortBy,
      isSortAscending,
      page,
      pageSize,
      lastModifiedStartDate,
      lastModifiedEndDate,
      principalId,
      deleted,
      orphan,
      lastModifiedBy,
      options
    )(this.fetch, this.basePath);
  }

  /**
   *
   * @param {VolteKPIQueryDto} body
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof VolteKPIApi
   */
  public volteKPIGetDashboard(body: VolteKPIQueryDto, options?: any) {
    return VolteKPIApiFp(this.configuration).volteKPIGetDashboard(
      body,
      options
    )(this.fetch, this.basePath);
  }

  /**
   *
   * @param {VolteKPIQueryDto} body
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof VolteKPIApi
   */
  public volteKPIGetDashboardReports(body: VolteKPIQueryDto, options?: any) {
    return VolteKPIApiFp(this.configuration).volteKPIGetDashboardReports(
      body,
      options
    )(this.fetch, this.basePath);
  }

  /**
   *
   * @param {VolteKPIQueryDto} body
   * @param {string} [propertyName]
   * @param {string} [propertyFilter]
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof VolteKPIApi
   */
  public volteKPIGetFilterResult(
    body: VolteKPIQueryDto,
    propertyName?: string,
    propertyFilter?: string,
    options?: any
  ) {
    return VolteKPIApiFp(this.configuration).volteKPIGetFilterResult(
      body,
      propertyName,
      propertyFilter,
      options
    )(this.fetch, this.basePath);
  }

  /**
   *
   * @param {VolteKPIQueryDto} body
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof VolteKPIApi
   */
  public volteKPIGetKPI(body: VolteKPIQueryDto, options?: any) {
    return VolteKPIApiFp(this.configuration).volteKPIGetKPI(body, options)(
      this.fetch,
      this.basePath
    );
  }

  /**
   *
   * @param {number} id
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof VolteKPIApi
   */
  public volteKPIGetRelatedRecords(id: number, options?: any) {
    return VolteKPIApiFp(this.configuration).volteKPIGetRelatedRecords(
      id,
      options
    )(this.fetch, this.basePath);
  }

  /**
   *
   * @param {number} id
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof VolteKPIApi
   */
  public volteKPIGetUpdateResourceVolteKPI(id: number, options?: any) {
    return VolteKPIApiFp(this.configuration).volteKPIGetUpdateResourceVolteKPI(
      id,
      options
    )(this.fetch, this.basePath);
  }

  /**
   *
   * @param {VolteKPIQueryDto} body
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof VolteKPIApi
   */
  public volteKPIGetVolteKPI(body: VolteKPIQueryDto, options?: any) {
    return VolteKPIApiFp(this.configuration).volteKPIGetVolteKPI(body, options)(
      this.fetch,
      this.basePath
    );
  }

  /**
   *
   * @param {VolteKPIDtoUpdate} body
   * @param {boolean} [forced]
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof VolteKPIApi
   */
  public volteKPIPut(body: VolteKPIDtoUpdate, forced?: boolean, options?: any) {
    return VolteKPIApiFp(this.configuration).volteKPIPut(
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
   * @memberof VolteKPIApi
   */
  public volteKPIRestore(id?: number, options?: any) {
    return VolteKPIApiFp(this.configuration).volteKPIRestore(id, options)(
      this.fetch,
      this.basePath
    );
  }

  /**
   *
   * @param {VolteKPIDtoCreate} body
   * @param {boolean} [forced]
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof VolteKPIApi
   */
  public volteKPISaveOrEdit(
    body: VolteKPIDtoCreate,
    forced?: boolean,
    options?: any
  ) {
    return VolteKPIApiFp(this.configuration).volteKPISaveOrEdit(
      body,
      forced,
      options
    )(this.fetch, this.basePath);
  }
}
