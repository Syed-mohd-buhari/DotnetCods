import * as url from "url";
import * as isomorphicFetch from "isomorphic-fetch";
import { Configuration } from "./Common/configuration";
import { ResultDto } from "../Model/CommonModels";
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
  QueryResultDtoOfTSRReportDtoGrid,
  TSRReportQueryDto,
  TSRReportVerticalQueryObjectGrid,
} from "../Model/TSRReport";
import { ReturnFile } from "../Model/Common";
import { TSRReportQueryObjectGrid as TSRReportQueryObjectGrid } from "../Model/TSRReport";
import {
  TipologicaGridDto,
  TipologicheQueryObjectGrid,
} from "../Model/LookUp/LookUpGenericModel";
/**
 * TSRReportApi - fetch parameter creator
 * @export
 */
export const TSRReportApiFetchParamCreator = function (
  configuration?: Configuration
) {
  return {
    /**
     * @param {TSRReportQueryObjectGrid} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    tsrnontemsReportGetGrid(
      body: TSRReportQueryObjectGrid,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling TSRReportQueryObjectGrid."
        );
      }
      const localVarPath = `/api/TsrNonTem/Get`;
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
        <any>"TSRReportQueryObjectGrid" !== "string" ||
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
     * @param {TSRReportQueryObjectGrid} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    ReportSchedulerGet(
      body: TSRReportQueryObjectGrid,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling TSRReportQueryObjectGrid."
        );
      }
      const localVarPath = `/api/ReportScheduler/Get`;
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
        <any>"TSRReportQueryObjectGrid" !== "string" ||
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
     * @param {TSRReportQueryObjectGrid} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    tsrReportGetGrid(
      body: TSRReportQueryObjectGrid,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling TSRReportQueryObjectGrid."
        );
      }
      const localVarPath = `/api/TsrPassThrough/Get`;
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
        <any>"TSRReportQueryObjectGrid" !== "string" ||
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
     * @param {TipologicheQueryObjectGrid} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    tsrReportGetVerticalGrid(options: any = {}): FetchArgs {
      const localVarPath = `/api/TsrNonTem/GetDomainNames`;
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
    TSRReportVerticalGetCreateResource(options: any = {}): FetchArgs {
      const localVarPath = `/api/AppSettingsConfiguration/Create`;
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
     * @param {TSRReportVerticalQueryObjectGrid} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    TSRReportVerticalCreate(
      body: TSRReportVerticalQueryObjectGrid,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling TSRReportVerticalCreate."
        );
      }
      const localVarPath = `/api/AppSettingsConfiguration`;
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
        <any>"TSRReportVerticalQueryObjectGrid" !== "string" ||
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
    TSRVerticalGetUpdateResource(id: number, options: any = {}): FetchArgs {
      // verify required parameter 'id' is not null or undefined
      if (id === null || id === undefined) {
        throw new RequiredError(
          "id",
          "Required parameter id was null or undefined when calling TSRVerticalGetUpdateResource."
        );
      }
      const localVarPath = `/api/AppSettingsConfiguration/Update{id}`.replace(
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
     * @param {TipologicaGridDto} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    ReportSchedulerUpdate(
      body: TipologicaGridDto,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling ReportSchedulerUpdate."
        );
      }
      const localVarPath = `/api/ReportScheduler`;
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
        <any>"TipologicaGridDto" !== "string" ||
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
     * @param {TipologicaGridDto} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    TSRVerticalUpdateResource(
      body: TipologicaGridDto,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling TSRVerticalUpdateResource."
        );
      }
      const localVarPath = `/api/AppSettingsConfiguration/Update`;
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
        <any>"TipologicaGridDto" !== "string" ||
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
    TSRReportGetCreateResourceTSRReport(options: any = {}): FetchArgs {
      const localVarPath = `/api/SystemVerificationProblem/Create`;
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
     * @param {number} [id]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    TSRReportDelete(id: number, options: any = {}): FetchArgs {
      const localVarPath = `/api/SystemVerificationProblem/Delete`;
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
    TSRReportDeleteDeep(id?: number, options: any = {}): FetchArgs {
      const localVarPath = `/api/AppSettingsConfiguration/DeleteDeep`;
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
     * @param {TSRReportQueryObjectGrid} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    TSRNonTemsReportExportReport(
      body: TSRReportQueryObjectGrid,
      fileType: "excel" | "csv" = "excel",
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling TSRNonTemsReportExportReport."
        );
      }

      const localVarPath =
        fileType === "csv"
          ? `/api/TsrNonTem/ExportDynamicReportCSV`
          : `/api/TsrNonTem/ExportReport`;
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
        <any>"TSRReportQueryObjectGrid" !== "string" ||
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
     * @param {TSRReportQueryObjectGrid} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    TSRReportExportReport(
      body: TSRReportQueryObjectGrid,
      fileType: "excel" | "csv" = "excel",
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling TSRReportExportReport."
        );
      }

      const localVarPath =
        fileType === "csv"
          ? `/api/TsrPassThrough/ExportDynamicReportCSV`
          : `/api/TsrPassThrough/ExportReport`;
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
        <any>"TSRReportQueryObjectGrid" !== "string" ||
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
     * @param {TSRReportQueryObjectGrid} body
     * @param {string} [propertyName]
     * @param {string} [propertyFilter]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    TSRNonTemsReportGetFilterResult(
      body: TSRReportQueryObjectGrid,
      propertyName?: string,
      propertyFilter?: string,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling TSRNonTemsReportGetFilterResult."
        );
      }
      const localVarPath = `/api/TsrNonTem/Filter`;
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
        <any>"TSRReportQueryObjectGrid" !== "string" ||
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
     * @param {TSRReportQueryObjectGrid} body
     * @param {string} [propertyName]
     * @param {string} [propertyFilter]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    TSRReportGetFilterResult(
      body: TSRReportQueryObjectGrid,
      propertyName?: string,
      propertyFilter?: string,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling TSRReportGetFilterResult."
        );
      }
      const localVarPath = `/api/TsrPassThrough/Filter`;
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
        <any>"TSRReportQueryObjectGrid" !== "string" ||
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
    TSRReportRestore(id?: number, options: any = {}): FetchArgs {
      const localVarPath = `/api/SystemVerificationProblem/Restore`;
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
     * @param {File} body
     * @param {Array<number>} mode
     * @param {Array<number>} verticalId
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    TSRReportImport(
      body: File,
      mode: Array<number>,
      verticalId: Array<number>,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      // const blob = new Blob([body], { type: body.type });
      const formData = new FormData();
      formData.append("file", body);
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling TSRReportImport."
        );
      }
      const localVarPath = `/api/TsrPassThrough/ImportExcel`;
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
      if (mode !== undefined) {
        localVarQueryParameter["recordClassifier"] = mode?.[0] ?? 0;
      }
      if (verticalId !== undefined) {
        localVarQueryParameter["nonTemsVertical"] = verticalId?.[0] ?? 0;
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

      localVarRequestOptions.body = formData;
      return {
        url: url.format(localVarUrlObj),
        options: localVarRequestOptions,
      };
    },
    /**
     * @param {any} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    TSRRefresh(body?: any, options: any = {}): FetchArgs {
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling TSRReportQueryObjectGrid."
        );
      }
      const localVarPath = `/api/TsrPassThrough/TSRDataLoads`;
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
        <any>"TSRReportQueryObjectGrid" !== "string" ||
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
     * @param {number} filterId
     * @param {*} [options]
     * @throws {RequiredError}
     */
    TSRRefreshStatus(filterId: number, options: any = {}): FetchArgs {
      if (filterId === null || filterId === undefined) {
        throw new RequiredError(
          "filterId",
          "Required parameter filterId was null or undefined when calling TSRRefreshStatus."
        );
      }

      const localVarPath = `/api/TsrLog/GetLatestRefreshStatus`;
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

      // ✅ Add filterId as query parameter
      if (filterId !== undefined) {
        localVarQueryParameter["filterId"] = filterId;
      }

      localVarUrlObj.query = Object.assign(
        {},
        localVarUrlObj.query,
        localVarQueryParameter,
        options.query
      );

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
 * TSRReportApi - functional programming interface
 * @export
 */
export const TSRReportApiFp = function (configuration?: Configuration) {
  return {
    /**
     *
     * @param {number} [id]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    TSRReportDelete(
      id: number,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = TSRReportApiFetchParamCreator(
        configuration
      ).TSRReportDelete(id, options);
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
    TSRReportDeleteDeep(
      id?: number,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = TSRReportApiFetchParamCreator(
        configuration
      ).TSRReportDeleteDeep(id, options);
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
     * @param {TSRReportQueryObjectGrid} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    TSRNonTemsReportExportReport(
      body: TSRReportQueryObjectGrid,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ReturnFile> {
      const localVarFetchArgs = TSRReportApiFetchParamCreator(
        configuration
      ).TSRNonTemsReportExportReport(body, options);
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
     * @param {TSRReportQueryObjectGrid} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    TSRReportExportReport(
      body: TSRReportQueryObjectGrid,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ReturnFile> {
      const localVarFetchArgs = TSRReportApiFetchParamCreator(
        configuration
      ).TSRReportExportReport(body, options);
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
     * @param {number} id
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */

    /**
     *
     * @param {TSRReportQueryObjectGrid} body
     * @param {string} [propertyName]
     * @param {string} [propertyFilter]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    TSRReportGetFilterResult(
      body: TSRReportQueryObjectGrid,
      propertyName?: string,
      propertyFilter?: string,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<Array<FilterValueDto>> {
      const localVarFetchArgs = TSRReportApiFetchParamCreator(
        configuration
      ).TSRReportGetFilterResult(body, propertyName, propertyFilter, options);
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
     * @param {number} id
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */

    /**
     *
     * @param {TSRReportQueryObjectGrid} body
     * @param {string} [propertyName]
     * @param {string} [propertyFilter]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    TSRNonTemsReportGetFilterResult(
      body: TSRReportQueryObjectGrid,
      propertyName?: string,
      propertyFilter?: string,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<Array<FilterValueDto>> {
      const localVarFetchArgs = TSRReportApiFetchParamCreator(
        configuration
      ).TSRNonTemsReportGetFilterResult(
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
     * @param {TSRReportQueryObjectGrid} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    tsrnontemsReportGetGrid(
      body: TSRReportQueryObjectGrid,
      options?: any
    ): (
      fetch?: FetchAPI,
      basePath?: string
    ) => Promise<QueryResultDtoOfTSRReportDtoGrid> {
      const localVarFetchArgs = TSRReportApiFetchParamCreator(
        configuration
      ).tsrnontemsReportGetGrid(body, options);
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
     * @param {TSRReportQueryObjectGrid} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    ReportSchedulerGet(
      body: TSRReportQueryObjectGrid,
      options?: any
    ): (
      fetch?: FetchAPI,
      basePath?: string
    ) => Promise<QueryResultDtoOfTSRReportDtoGrid> {
      const localVarFetchArgs = TSRReportApiFetchParamCreator(
        configuration
      ).ReportSchedulerGet(body, options);
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
     * @param {TSRReportQueryObjectGrid} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    tsrReportGetGrid(
      body: TSRReportQueryObjectGrid,
      options?: any
    ): (
      fetch?: FetchAPI,
      basePath?: string
    ) => Promise<QueryResultDtoOfTSRReportDtoGrid> {
      const localVarFetchArgs = TSRReportApiFetchParamCreator(
        configuration
      ).tsrReportGetGrid(body, options);
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
     * @param {TipologicheQueryObjectGrid} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    tsrReportGetVerticalGrid(
      options?: any // Remove body parameter
    ): (
      fetch?: FetchAPI,
      basePath?: string
    ) => Promise<{ [key: string]: string }> {
      // Change return type to match response
      const localVarFetchArgs =
        TSRReportApiFetchParamCreator(configuration).tsrReportGetVerticalGrid(
          options
        ); // Remove body argument
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
    TSRReportVerticalGetCreateResource(
      options?: any
    ): (
      fetch?: FetchAPI,
      basePath?: string
    ) => Promise<QueryResultDtoOfTSRReportDtoGrid> {
      const localVarFetchArgs =
        TSRReportApiFetchParamCreator(
          configuration
        ).TSRReportVerticalGetCreateResource(options);
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
     * @param {TipologicaGridDto} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    ReportSchedulerUpdate(
      body: TipologicaGridDto,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = TSRReportApiFetchParamCreator(
        configuration
      ).ReportSchedulerUpdate(body, options);
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
     * @param {TipologicaGridDto} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    TSRVerticalUpdateResource(
      body: TipologicaGridDto,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = TSRReportApiFetchParamCreator(
        configuration
      ).TSRVerticalUpdateResource(body, options);
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
     * @param {TSRReportVerticalQueryObjectGrid} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    TSRReportVerticalCreate(
      body: TSRReportVerticalQueryObjectGrid,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = TSRReportApiFetchParamCreator(
        configuration
      ).TSRReportVerticalCreate(body, options);
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
    TSRVerticalGetUpdateResource(
      id: number,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<TipologicaGridDto> {
      const localVarFetchArgs = TSRReportApiFetchParamCreator(
        configuration
      ).TSRVerticalGetUpdateResource(id, options);
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
    TSRReportRestore(
      id?: number,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = TSRReportApiFetchParamCreator(
        configuration
      ).TSRReportRestore(id, options);
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
     * @param {File} body
     * @param {Array<number>} mode
     * @param {Array<number>} verticalId
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    TSRReportImport(
      body: File,
      mode: Array<number>,
      verticalId: Array<number>,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<boolean> {
      const localVarFetchArgs = TSRReportApiFetchParamCreator(
        configuration
      ).TSRReportImport(body, mode, verticalId, options);
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
     * @param {any} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    TSRRefresh(
      body?: any,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<any> {
      const localVarFetchArgs = TSRReportApiFetchParamCreator(
        configuration
      ).TSRRefresh(body, options);
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
     * @param {number} filterId
     * @param {*} [options]
     * @throws {RequiredError}
     */
    TSRRefreshStatus(
      filterId: number,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<any> {
      const localVarFetchArgs = TSRReportApiFetchParamCreator(
        configuration
      ).TSRRefreshStatus(filterId, options);
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
 * TSRReportApi - factory interface
 * @export
 */
export const TSRReportApiFactory = function (
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
    TSRReportDelete(id: number, options?: any) {
      return TSRReportApiFp(configuration).TSRReportDelete(id, options)(
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
    TSRReportDeleteDeep(id?: number, options?: any) {
      return TSRReportApiFp(configuration).TSRReportDeleteDeep(id, options)(
        fetch,
        basePath
      );
    },
    /**
     *
     * @param {TSRReportQueryObjectGrid} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    TSRNonTemsReportExportReport(
      body: TSRReportQueryObjectGrid,
      options?: any
    ) {
      return TSRReportApiFp(configuration).TSRNonTemsReportExportReport(
        body,
        options
      )(fetch, basePath);
    },
    /**
     *
     * @param {TSRReportQueryObjectGrid} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    TSRReportExportReport(body: TSRReportQueryObjectGrid, options?: any) {
      return TSRReportApiFp(configuration).TSRReportExportReport(body, options)(
        fetch,
        basePath
      );
    },

    /**
     *
     * @param {TSRReportQueryObjectGrid} body
     * @param {string} [propertyName]
     * @param {string} [propertyFilter]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    TSRReportGetFilterResult(
      body: TSRReportQueryObjectGrid,
      propertyName?: string,
      propertyFilter?: string,
      options?: any
    ) {
      return TSRReportApiFp(configuration).TSRReportGetFilterResult(
        body,
        propertyName,
        propertyFilter,
        options
      )(fetch, basePath);
    },
    /**
     *
     * @param {TSRReportQueryObjectGrid} body
     * @param {string} [propertyName]
     * @param {string} [propertyFilter]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    TSRNonTemsReportGetFilterResult(
      body: TSRReportQueryObjectGrid,
      propertyName?: string,
      propertyFilter?: string,
      options?: any
    ) {
      return TSRReportApiFp(configuration).TSRNonTemsReportGetFilterResult(
        body,
        propertyName,
        propertyFilter,
        options
      )(fetch, basePath);
    },
    /**
     * @param {TSRReportQueryObjectGrid} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    tsrnontemsReportGetGrid(body: TSRReportQueryObjectGrid, options?: any) {
      return TSRReportApiFp(configuration).tsrnontemsReportGetGrid(
        body,
        options
      )(fetch, basePath);
    },
    /**
     * @param {TSRReportQueryObjectGrid} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    ReportSchedulerGet(body: TSRReportQueryObjectGrid, options?: any) {
      return TSRReportApiFp(configuration).ReportSchedulerGet(body, options)(
        fetch,
        basePath
      );
    },
    /**
     * @param {TSRReportQueryObjectGrid} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    tsrReportGetGrid(body: TSRReportQueryObjectGrid, options?: any) {
      return TSRReportApiFp(configuration).tsrReportGetGrid(body, options)(
        fetch,
        basePath
      );
    },
    /**
     * @param {TipologicheQueryObjectGrid} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    tsrReportGetVerticalGrid(options?: any) {
      return TSRReportApiFp(configuration).tsrReportGetVerticalGrid(options)(
        fetch,
        basePath
      );
    },

    /**
     *
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    TSRReportVerticalGetCreateResource(options?: any) {
      return TSRReportApiFp(configuration).TSRReportVerticalGetCreateResource(
        options
      )(fetch, basePath);
    },

    /**
     *
     * @param {TSRReportVerticalQueryObjectGrid} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    TSRReportVerticalCreate(
      body: TSRReportVerticalQueryObjectGrid,
      options?: any
    ) {
      return TSRReportApiFp(configuration).TSRReportVerticalCreate(
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
    TSRVerticalGetUpdateResource(
      id: number,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<TipologicaGridDto> {
      const localVarFetchArgs = TSRReportApiFetchParamCreator(
        configuration
      ).TSRVerticalGetUpdateResource(id, options);
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
     * @param {TipologicaGridDto} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    ReportSchedulerUpdate(body: TipologicaGridDto, options?: any) {
      return TSRReportApiFp(configuration).ReportSchedulerUpdate(body, options)(
        fetch,
        basePath
      );
    },

    /**
     *
     * @param {TipologicaGridDto} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    TSRVerticalUpdateResource(body: TipologicaGridDto, options?: any) {
      return TSRReportApiFp(configuration).TSRVerticalUpdateResource(
        body,
        options
      )(fetch, basePath);
    },

    /**
     *
     * @param {number} [id]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    TSRReportRestore(id?: number, options?: any) {
      return TSRReportApiFp(configuration).TSRReportRestore(id, options)(
        fetch,
        basePath
      );
    },

    /**
     *
     * @param {File} body
     * @param {Array<number>} mode
     * @param {Array<number>} verticalId
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    TSRReportImport(
      body: File,
      mode: Array<number>,
      verticalId: Array<number>,
      options?: any
    ) {
      return TSRReportApiFp(configuration).TSRReportImport(
        body,
        mode,
        verticalId,
        options
      )(fetch, basePath);
    },

    /**
     * @param {any} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    TSRRefresh(body?: any, options?: any) {
      return TSRReportApiFp(configuration).TSRRefresh(body, options)(
        fetch,
        basePath
      );
    },

    /**
     * @param {number} filterId
     * @param {*} [options]
     * @throws {RequiredError}
     */
    TSRRefreshStatus(filterId: number, options?: any) {
      return TSRReportApiFp(configuration).TSRRefreshStatus(filterId, options)(
        fetch,
        basePath
      );
    },
  };
};

/**
 * TSRReportApi - object-oriented interface
 * @export
 * @class TSRReportApi
 * @extends {BaseAPI}
 */
export class TSRReportApi extends BaseAPI {
  /**
   *
   * @param {number} [id]
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof TSRReportApi
   */
  public TSRReportDelete(id: number, options?: any) {
    return TSRReportApiFp(this.configuration).TSRReportDelete(id, options)(
      this.fetch,
      this.basePath
    );
  }

  /**
   *
   * @param {number} [id]
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof TSRReportApi
   */
  public TSRReportDeleteDeep(id?: number, options?: any) {
    return TSRReportApiFp(this.configuration).TSRReportDeleteDeep(id, options)(
      this.fetch,
      this.basePath
    );
  }
  /**
   *
   * @param {TSRReportQueryObjectGrid} body
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof TSRReportApi
   */
  public TSRNonTemsReportExportReport(
    body: TSRReportQueryObjectGrid,
    options?: any
  ) {
    return TSRReportApiFp(this.configuration).TSRNonTemsReportExportReport(
      body,
      options
    )(this.fetch, this.basePath);
  }
  /**
   *
   * @param {TSRReportQueryObjectGrid} body
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof TSRReportApi
   */
  public TSRReportExportReport(body: TSRReportQueryObjectGrid, options?: any) {
    return TSRReportApiFp(this.configuration).TSRReportExportReport(
      body,
      options
    )(this.fetch, this.basePath);
  }

  /**
   *
   * @param {TSRReportQueryObjectGrid} body
   * @param {string} [propertyName]
   * @param {string} [propertyFilter]
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof TSRReportApi
   */
  public TSRReportGetFilterResult(
    body: TSRReportQueryObjectGrid,
    propertyName?: string,
    propertyFilter?: string,
    options?: any
  ) {
    return TSRReportApiFp(this.configuration).TSRReportGetFilterResult(
      body,
      propertyName,
      propertyFilter,
      options
    )(this.fetch, this.basePath);
  }
  /**
   *
   * @param {TSRReportQueryObjectGrid} body
   * @param {string} [propertyName]
   * @param {string} [propertyFilter]
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof TSRReportApi
   */
  public TSRNonTemsReportGetFilterResult(
    body: TSRReportQueryObjectGrid,
    propertyName?: string,
    propertyFilter?: string,
    options?: any
  ) {
    return TSRReportApiFp(this.configuration).TSRNonTemsReportGetFilterResult(
      body,
      propertyName,
      propertyFilter,
      options
    )(this.fetch, this.basePath);
  }
  /**
   *
   * @param {TSRReportQueryObjectGrid} body
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof TSRReportApi
   */
  public tsrnontemsReportGetGrid(
    body: TSRReportQueryObjectGrid,
    options?: any
  ) {
    return TSRReportApiFp(this.configuration).tsrnontemsReportGetGrid(
      body,
      options
    )(this.fetch, this.basePath);
  }
  /**
   *
   * @param {TSRReportQueryObjectGrid} body
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof TSRReportApi
   */
  public ReportSchedulerGet(body: TSRReportQueryObjectGrid, options?: any) {
    return TSRReportApiFp(this.configuration).ReportSchedulerGet(body, options)(
      this.fetch,
      this.basePath
    );
  }
  /**
   *
   * @param {TSRReportQueryObjectGrid} body
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof TSRReportApi
   */
  public tsrReportGetGrid(body: TSRReportQueryObjectGrid, options?: any) {
    return TSRReportApiFp(this.configuration).tsrReportGetGrid(body, options)(
      this.fetch,
      this.basePath
    );
  }
  /**
   *
   * @param {TipologicheQueryObjectGrid} body
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof TSRReportApi
   */
  public tsrReportGetVerticalGrid(options?: any) {
    return TSRReportApiFp(this.configuration).tsrReportGetVerticalGrid(options)(
      this.fetch,
      this.basePath
    );
  }
  /**
   *
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof TSRReportApi
   */
  public TSRReportVerticalGetCreateResource(options?: any) {
    return TSRReportApiFp(
      this.configuration
    ).TSRReportVerticalGetCreateResource(options)(this.fetch, this.basePath);
  }

  /**
   *
   * @param {TSRReportVerticalQueryObjectGrid} body
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof ProductImportanceApi
   */
  public TSRReportVerticalCreate(
    body: TSRReportVerticalQueryObjectGrid,
    options?: any
  ) {
    return TSRReportApiFp(this.configuration).TSRReportVerticalCreate(
      body,
      options
    )(this.fetch, this.basePath);
  }

  /**
   *
   * @param {number} id
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof ProductImportanceApi
   */
  public TSRVerticalGetUpdateResource(id: number, options?: any) {
    return TSRReportApiFp(this.configuration).TSRVerticalGetUpdateResource(
      id,
      options
    )(this.fetch, this.basePath);
  }

  /**
   *
   * @param {TipologicaGridDto} body
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof ProductImportanceApi
   */
  public ReportSchedulerUpdate(body: TipologicaGridDto, options?: any) {
    return TSRReportApiFp(this.configuration).ReportSchedulerUpdate(
      body,
      options
    )(this.fetch, this.basePath);
  }

  /**
   *
   * @param {TipologicaGridDto} body
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof ProductImportanceApi
   */
  public TSRVerticalUpdateResource(body: TipologicaGridDto, options?: any) {
    return TSRReportApiFp(this.configuration).TSRVerticalUpdateResource(
      body,
      options
    )(this.fetch, this.basePath);
  }

  /**
   *
   * @param {number} [id]
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof TSRReportApi
   */
  public TSRReportRestore(id?: number, options?: any) {
    return TSRReportApiFp(this.configuration).TSRReportRestore(id, options)(
      this.fetch,
      this.basePath
    );
  }

  /**
   *
   * @param {File} body
   * @param {Array<number>} mode
   * @param {Array<number>}verticalId
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof TSRReportApi
   */
  public TSRReportImport(
    body: File,
    mode: Array<number>,
    verticalId: Array<number>
  ) {
    return TSRReportApiFp(this.configuration).TSRReportImport(
      body,
      mode,
      verticalId
    )(this.fetch, this.basePath);
  }

  /**
   * @param {any} body
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof TSRReportApi
   */
  public TSRRefresh(body?: any) {
    return TSRReportApiFp(this.configuration).TSRRefresh(body)(
      this.fetch,
      this.basePath
    );
  }

  /**
   * @param {number} filterId
   * @param {*} [options]
   * @throws {RequiredError}
   * @memberof TSRReportApi
   */
  public TSRRefreshStatus(filterId: number) {
    return TSRReportApiFp(this.configuration).TSRRefreshStatus(filterId)(
      this.fetch,
      this.basePath
    );
  }
}
