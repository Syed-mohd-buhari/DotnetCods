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
  VolteKPIDtoCreate,
  VolteKPIDtoUpdate,
  QueryResultDtoOfVolteKPIDtoGrid,
  VolteKPIType,
  VolteKPIDashboardDto,
} from "../Model/VolteKpi/VolteKPI";
import { VolteKPIReportDto } from "../Model/Report/ReportVolteKPIModel";
import { VolteKPIQueryObjectGrid as VolteKPIQueryDto } from "../Model/VolteKpi/VolteKPI";
import {
  PATReportDto,
  ReportPATQueryObjectGrid,
  SWOEM_MODEL,
} from "../Model/Report/PlannedActivityTrackerModel";
/**
 * VolteKPIApi - fetch parameter creator
 * @export
 */
export const PlannedActivityTracker = function (configuration?: Configuration) {
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

    PATxportReport(
      buildConstruction?: number,
      opCoId?: Array<string>,
      dcfId?: Array<string>,
      vendorIds?: Array<number>,
      isEosDateEnable?: boolean,
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
      const localVarPath = `/api/PATReport/ExportReport`;
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

      if (opCoId) {
        localVarQueryParameter["opCoId"] = opCoId;
      }

      if (vendorIds) {
        localVarQueryParameter["vendorIds"] = vendorIds;
      }

      if (dcfId) {
        localVarQueryParameter["dcfId"] = dcfId;
      }

      if (buildConstruction) {
        localVarQueryParameter["buildConstruction"] = buildConstruction;
      }

      if (sortBy !== undefined) {
        localVarQueryParameter["SortBy"] = sortBy;
      }

      if (isSortAscending !== undefined) {
        localVarQueryParameter["IsSortAscending"] = isSortAscending;
      }

      if (isEosDateEnable !== undefined) {
        localVarQueryParameter["isEosDateEnable"] = isEosDateEnable;
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
    reportPATGetFilterResult(
      body: ReportPATQueryObjectGrid,
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

      const localVarPath = `/api/PATReport/Filter`;
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
    getVendorsOfPredefinedFilter(id: number, options: any = {}): FetchArgs {
      // verify required parameter 'id' is not null or undefined
      if (id === null || id === undefined) {
        throw new RequiredError(
          "id",
          "Required parameter id was null or undefined when calling getVendorsOfPredefinedFilter."
        );
      }
      const localVarPath = `/api/PATReport/GetVendorsOfPredefinedFilter`;

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

      if (id) {
        localVarQueryParameter["filterId"] = id;
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
    patGetAll(body: ReportPATQueryObjectGrid, options: any = {}): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling volteKPIGetVolteKPI."
        );
      }
      const localVarPath = `/api/PATReport/GetAll`;
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
    patGetAllExports(
      body: ReportPATQueryObjectGrid,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling volteKPIGetVolteKPI."
        );
      }
      const localVarPath = `/api/PATReport/ExportReport`;
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
    getSWOem(options: any = {}): FetchArgs {
      // verify required parameter 'id' is not null or undefined

      const localVarPath = "/api/PATReport/GetSWOem";
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
      const localVarFetchArgs = PlannedActivityTracker(
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
      const localVarFetchArgs = PlannedActivityTracker(
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
     * @param {boolean} [isEosDateEnable]
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
    PATxportReport(
      buildConstruction?: number,
      opCoId?: Array<string>,
      dcfId?: Array<string>,
      vendorIds?: Array<number>,
      isEosDateEnable?: boolean,
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
      const localVarFetchArgs = PlannedActivityTracker(
        configuration
      ).PATxportReport(
        buildConstruction,
        opCoId,
        dcfId,
        vendorIds,
        isEosDateEnable,
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
     * @param {VolteKPIQueryDto} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    volteKPIGetDashboard(
      body: VolteKPIQueryDto,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<VolteKPIDashboardDto> {
      const localVarFetchArgs = PlannedActivityTracker(
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
      const localVarFetchArgs = PlannedActivityTracker(
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
    reportPATGetFilterResult(
      body: VolteKPIQueryDto,
      propertyName?: string,
      propertyFilter?: string,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<Array<FilterValueDto>> {
      const localVarFetchArgs = PlannedActivityTracker(
        configuration
      ).reportPATGetFilterResult(body, propertyName, propertyFilter, options);
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
      const localVarFetchArgs = PlannedActivityTracker(
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
      const localVarFetchArgs = PlannedActivityTracker(
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
    getVendorsOfPredefinedFilter(
      id: number,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = PlannedActivityTracker(
        configuration
      ).getVendorsOfPredefinedFilter(id, options);
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
    patGetAll(
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<PATReportDto> {
      const localVarFetchArgs =
        PlannedActivityTracker(configuration).patGetAll(options);
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
    patGetAllExports(
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ReturnFile> {
      const localVarFetchArgs =
        PlannedActivityTracker(configuration).patGetAllExports(options);
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
    getSWOem(
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<SWOEM_MODEL> {
      const localVarFetchArgs =
        PlannedActivityTracker(configuration).getSWOem(options);
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
      const localVarFetchArgs = PlannedActivityTracker(
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
      const localVarFetchArgs = PlannedActivityTracker(
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
      const localVarFetchArgs = PlannedActivityTracker(
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
      const localVarFetchArgs = PlannedActivityTracker(
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
     * @param {boolean} [isEosDateEnable]
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
    PATxportReport(
      buildConstruction?: number,
      opCoId?: Array<string>,
      dcfId?: Array<string>,
      vendorIds?: Array<number>,
      isEosDateEnable?: boolean,
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
      return VolteKPIApiFp(configuration).PATxportReport(
        buildConstruction,
        opCoId,
        dcfId,
        vendorIds,
        isEosDateEnable,
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
    reportPATGetFilterResult(
      body: VolteKPIQueryDto,
      propertyName?: string,
      propertyFilter?: string,
      options?: any
    ) {
      return VolteKPIApiFp(configuration).reportPATGetFilterResult(
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
    getVendorsOfPredefinedFilter(id: number, options?: any) {
      return VolteKPIApiFp(configuration).getVendorsOfPredefinedFilter(
        id,
        options
      )(fetch, basePath);
    },

    /**
     *
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    patGetAll(options?: any) {
      return VolteKPIApiFp(configuration).patGetAll(options)(fetch, basePath);
    },

    /**
     *
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    patGetAllExports(options?: any) {
      return VolteKPIApiFp(configuration).patGetAllExports(options)(
        fetch,
        basePath
      );
    },

    /**
     *
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    getSWOem(options?: any) {
      return VolteKPIApiFp(configuration).getSWOem(options)(fetch, basePath);
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
export class PATReport extends BaseAPI {
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
   * @param {boolean} [isEosDateEnable]
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
  public PATxportReport(
    buildConstruction?: number,
    opCoId?: Array<string>,
    dcfId?: Array<string>,
    vendorIds?: Array<number>,
    isEosDateEnable?: boolean,
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
    return VolteKPIApiFp(this.configuration).PATxportReport(
      buildConstruction,
      opCoId,
      dcfId,
      vendorIds,
      isEosDateEnable,
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
  public reportPATGetFilterResult(
    body: VolteKPIQueryDto,
    propertyName?: string,
    propertyFilter?: string,
    options?: any
  ) {
    return VolteKPIApiFp(this.configuration).reportPATGetFilterResult(
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
  public getVendorsOfPredefinedFilter(id: number, options?: any) {
    return VolteKPIApiFp(this.configuration).getVendorsOfPredefinedFilter(
      id,
      options
    )(this.fetch, this.basePath);
  }

  /**
   *
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof VolteKPIApi
   */
  public patGetAll(options?: any) {
    return VolteKPIApiFp(this.configuration).patGetAll(options)(
      this.fetch,
      this.basePath
    );
  }

  /**
   *
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof VolteKPIApi
   */
  public patGetAllExports(options?: any) {
    return VolteKPIApiFp(this.configuration).patGetAllExports(options)(
      this.fetch,
      this.basePath
    );
  }

  /**
   *
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof VolteKPIApi
   */
  public getSWOem(options?: any) {
    return VolteKPIApiFp(this.configuration).getSWOem(options)(
      this.fetch,
      this.basePath
    );
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
