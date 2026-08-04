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
  QueryResultDtoOfFNTReportDtoGrid,
  FNTReportQueryDto,
  FNTReportVerticalQueryObjectGrid,
} from "../Model/FNTReport";
import { ReturnFile } from "../Model/Common";
import { FNTReportQueryObjectGrid as FNTReportQueryObjectGrid } from "../Model/FNTReport";
import {
  TipologicaGridDto,
  TipologicheQueryObjectGrid,
} from "../Model/LookUp/LookUpGenericModel";
/**
 * FNTReportApi - fetch parameter creator
 * @export
 */
export const FNTReportApiFetchParamCreator = function (
  configuration?: Configuration
) {
  return {
    /**
     * @param {FNTReportQueryObjectGrid} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    NonTemsFNTReportGetGrid(
      body: FNTReportQueryObjectGrid,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling FNTReportQueryObjectGrid."
        );
      }
      const localVarPath = `/api/NonTemsFntReport/Get`;
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
        <any>"FNTReportQueryObjectGrid" !== "string" ||
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
     * @param {FNTReportQueryObjectGrid} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    FNTReportGetGrid(
      body: FNTReportQueryObjectGrid,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling FNTReportQueryObjectGrid."
        );
      }
      const localVarPath = `/api/TemsFntReport/Get`;
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
        <any>"FNTReportQueryObjectGrid" !== "string" ||
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
     * @param {FNTReportQueryObjectGrid} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    ReportSchedulerGet(
      body: FNTReportQueryObjectGrid,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling FNTReportQueryObjectGrid."
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
        <any>"FNTReportQueryObjectGrid" !== "string" ||
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
     * @param {TipologicheQueryObjectGrid} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    tsrReportGetVerticalGrid(options: any = {}): FetchArgs {
      const localVarPath = `/api/NonTemsFntReport/GetDomainNames`;
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
     * @param {FNTReportVerticalQueryObjectGrid} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    TSRReportVerticalCreate(
      body: FNTReportVerticalQueryObjectGrid,
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
        <any>"FNTReportVerticalQueryObjectGrid" !== "string" ||
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
     * @param {FNTReportQueryObjectGrid} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    NonTemsFNTReportExportReport(
      body: FNTReportQueryObjectGrid,
      fileType: "excel",
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling NonTemsFNTReportExportReport."
        );
      }

      const localVarPath = `/api/NonTemsFntReport/ExportReport`;
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
        <any>"FNTReportQueryObjectGrid" !== "string" ||
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
     * @param {FNTReportQueryObjectGrid} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    FNTReportExportReport(
      body: FNTReportQueryObjectGrid,
      fileType: "excel",
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling FNTReportExportReport."
        );
      }

      const localVarPath = `/api/TemsFntReport/ExportReport`;
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
        <any>"FNTReportQueryObjectGrid" !== "string" ||
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
     * @param {FNTReportQueryObjectGrid} body
     * @param {string} [propertyName]
     * @param {string} [propertyFilter]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    NonTemsFNTReportGetFilterResult(
      body: FNTReportQueryObjectGrid,
      propertyName?: string,
      propertyFilter?: string,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling NonTemsFNTReportGetFilterResult."
        );
      }
      const localVarPath = `/api/NonTemsFntReport/Filter`;
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
        <any>"FNTReportQueryObjectGrid" !== "string" ||
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
     * @param {FNTReportQueryObjectGrid} body
     * @param {string} [propertyName]
     * @param {string} [propertyFilter]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    FNTReportGetFilterResult(
      body: FNTReportQueryObjectGrid,
      propertyName?: string,
      propertyFilter?: string,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling FNTReportGetFilterResult."
        );
      }
      const localVarPath = `/api/TemsFntReport/Filter`;
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
        <any>"FNTReportQueryObjectGrid" !== "string" ||
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
    TemsFNTReportImport(
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
          "Required parameter body was null or undefined when calling TemsFNTReportImport."
        );
      }
      const localVarPath = `/api/TemsFntReport/Import`;
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
      // if (mode !== undefined) {
      //   localVarQueryParameter["temsFntReportId"] = mode?.[0] ?? 0;
      // }
      // if (verticalId !== undefined) {
      //   localVarQueryParameter["nonTemsVertical"] = verticalId?.[0] ?? 0;
      // }

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
    FNTRefresh(body?: any, options: any = {}): FetchArgs {
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling FNTRefresh."
        );
      }
      const localVarPath = `/api/TemsFntReport/TemsFNTDataLoads`;
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
        <any>"FNTReportQueryObjectGrid" !== "string" ||
        localVarRequestOptions.headers["Content-Type"] === "application/json";
      localVarRequestOptions.body = needsSerialization
        ? JSON.stringify(body || {})
        : body || "";

      return {
        url: url.format(localVarUrlObj),
        options: localVarRequestOptions,
      };
    },

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

      if (configuration && configuration.apiKey) {
        const localVarApiKeyValue =
          typeof configuration.apiKey === "function"
            ? configuration.apiKey("Authorization")
            : configuration.apiKey;
        localVarHeaderParameter["Authorization"] = localVarApiKeyValue;
      }

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
 * FNTReportApi - functional programming interface
 * @export
 */
export const FNTReportApiFp = function (configuration?: Configuration) {
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
      const localVarFetchArgs = FNTReportApiFetchParamCreator(
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
      const localVarFetchArgs = FNTReportApiFetchParamCreator(
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
     * @param {FNTReportQueryObjectGrid} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    NonTemsFNTReportExportReport(
      body: FNTReportQueryObjectGrid,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ReturnFile> {
      const localVarFetchArgs = FNTReportApiFetchParamCreator(
        configuration
      ).NonTemsFNTReportExportReport(body, options);
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
     * @param {FNTReportQueryObjectGrid} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    FNTReportExportReport(
      body: FNTReportQueryObjectGrid,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ReturnFile> {
      const localVarFetchArgs = FNTReportApiFetchParamCreator(
        configuration
      ).FNTReportExportReport(body, options);
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
     * @param {FNTReportQueryObjectGrid} body
     * @param {string} [propertyName]
     * @param {string} [propertyFilter]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    NonTemsFNTReportGetFilterResult(
      body: FNTReportQueryObjectGrid,
      propertyName?: string,
      propertyFilter?: string,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<Array<FilterValueDto>> {
      const localVarFetchArgs = FNTReportApiFetchParamCreator(
        configuration
      ).NonTemsFNTReportGetFilterResult(
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
     * @param {number} id
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */

    /**
     *
     * @param {FNTReportQueryObjectGrid} body
     * @param {string} [propertyName]
     * @param {string} [propertyFilter]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    FNTReportGetFilterResult(
      body: FNTReportQueryObjectGrid,
      propertyName?: string,
      propertyFilter?: string,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<Array<FilterValueDto>> {
      const localVarFetchArgs = FNTReportApiFetchParamCreator(
        configuration
      ).FNTReportGetFilterResult(body, propertyName, propertyFilter, options);
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
     * @param {FNTReportQueryObjectGrid} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    NonTemsFNTReportGetGrid(
      body: FNTReportQueryObjectGrid,
      options?: any
    ): (
      fetch?: FetchAPI,
      basePath?: string
    ) => Promise<QueryResultDtoOfFNTReportDtoGrid> {
      const localVarFetchArgs = FNTReportApiFetchParamCreator(
        configuration
      ).NonTemsFNTReportGetGrid(body, options);
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
     * @param {FNTReportQueryObjectGrid} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    FNTReportGetGrid(
      body: FNTReportQueryObjectGrid,
      options?: any
    ): (
      fetch?: FetchAPI,
      basePath?: string
    ) => Promise<QueryResultDtoOfFNTReportDtoGrid> {
      const localVarFetchArgs = FNTReportApiFetchParamCreator(
        configuration
      ).FNTReportGetGrid(body, options);
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
     * @param {FNTReportQueryObjectGrid} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    ReportSchedulerGet(
      body: FNTReportQueryObjectGrid,
      options?: any
    ): (
      fetch?: FetchAPI,
      basePath?: string
    ) => Promise<QueryResultDtoOfFNTReportDtoGrid> {
      const localVarFetchArgs = FNTReportApiFetchParamCreator(
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
     *
     * @param {TipologicaGridDto} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    ReportSchedulerUpdate(
      body: TipologicaGridDto,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = FNTReportApiFetchParamCreator(
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
     * @param {TipologicheQueryObjectGrid} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    tsrReportGetVerticalGrid(
      options?: any
    ): (
      fetch?: FetchAPI,
      basePath?: string
    ) => Promise<{ [key: string]: string }> {
      const localVarFetchArgs =
        FNTReportApiFetchParamCreator(configuration).tsrReportGetVerticalGrid(
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
    TSRReportVerticalGetCreateResource(
      options?: any
    ): (
      fetch?: FetchAPI,
      basePath?: string
    ) => Promise<QueryResultDtoOfFNTReportDtoGrid> {
      const localVarFetchArgs =
        FNTReportApiFetchParamCreator(
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
    TSRVerticalUpdateResource(
      body: TipologicaGridDto,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = FNTReportApiFetchParamCreator(
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
     * @param {FNTReportVerticalQueryObjectGrid} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    TSRReportVerticalCreate(
      body: FNTReportVerticalQueryObjectGrid,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = FNTReportApiFetchParamCreator(
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
      const localVarFetchArgs = FNTReportApiFetchParamCreator(
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
      const localVarFetchArgs = FNTReportApiFetchParamCreator(
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
    TemsFNTReportImport(
      body: File,
      mode: Array<number>,
      verticalId: Array<number>,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<boolean> {
      const localVarFetchArgs = FNTReportApiFetchParamCreator(
        configuration
      ).TemsFNTReportImport(body, mode, verticalId, options);
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
    FNTRefresh(
      body?: any,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<any> {
      const localVarFetchArgs = FNTReportApiFetchParamCreator(
        configuration
      ).FNTRefresh(body, options);
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
      const localVarFetchArgs = FNTReportApiFetchParamCreator(
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
 * FNTReportApi - factory interface
 * @export
 */
export const FNTReportApiFactory = function (
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
      return FNTReportApiFp(configuration).TSRReportDelete(id, options)(
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
      return FNTReportApiFp(configuration).TSRReportDeleteDeep(id, options)(
        fetch,
        basePath
      );
    },
    /**
     *
     * @param {FNTReportQueryObjectGrid} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    NonTemsFNTReportExportReport(
      body: FNTReportQueryObjectGrid,
      options?: any
    ) {
      return FNTReportApiFp(configuration).NonTemsFNTReportExportReport(
        body,
        options
      )(fetch, basePath);
    },
    /**
     *
     * @param {FNTReportQueryObjectGrid} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    FNTReportExportReport(body: FNTReportQueryObjectGrid, options?: any) {
      return FNTReportApiFp(configuration).FNTReportExportReport(body, options)(
        fetch,
        basePath
      );
    },
    /**
     *
     * @param {FNTReportQueryObjectGrid} body
     * @param {string} [propertyName]
     * @param {string} [propertyFilter]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    NonTemsFNTReportGetFilterResult(
      body: FNTReportQueryObjectGrid,
      propertyName?: string,
      propertyFilter?: string,
      options?: any
    ) {
      return FNTReportApiFp(configuration).NonTemsFNTReportGetFilterResult(
        body,
        propertyName,
        propertyFilter,
        options
      )(fetch, basePath);
    },
    /**
     *
     * @param {FNTReportQueryObjectGrid} body
     * @param {string} [propertyName]
     * @param {string} [propertyFilter]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    FNTReportGetFilterResult(
      body: FNTReportQueryObjectGrid,
      propertyName?: string,
      propertyFilter?: string,
      options?: any
    ) {
      return FNTReportApiFp(configuration).FNTReportGetFilterResult(
        body,
        propertyName,
        propertyFilter,
        options
      )(fetch, basePath);
    },
    /**
     * @param {FNTReportQueryObjectGrid} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    NonTemsFNTReportGetGrid(body: FNTReportQueryObjectGrid, options?: any) {
      return FNTReportApiFp(configuration).NonTemsFNTReportGetGrid(
        body,
        options
      )(fetch, basePath);
    },
    /**
     * @param {FNTReportQueryObjectGrid} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    FNTReportGetGrid(body: FNTReportQueryObjectGrid, options?: any) {
      return FNTReportApiFp(configuration).FNTReportGetGrid(body, options)(
        fetch,
        basePath
      );
    },
    /**
     * @param {FNTReportQueryObjectGrid} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    ReportSchedulerGet(body: FNTReportQueryObjectGrid, options?: any) {
      return FNTReportApiFp(configuration).ReportSchedulerGet(body, options)(
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
    ReportSchedulerUpdate(body: TipologicaGridDto, options?: any) {
      return FNTReportApiFp(configuration).ReportSchedulerUpdate(body, options)(
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
      return FNTReportApiFp(configuration).tsrReportGetVerticalGrid(options)(
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
      return FNTReportApiFp(configuration).TSRReportVerticalGetCreateResource(
        options
      )(fetch, basePath);
    },

    /**
     *
     * @param {FNTReportVerticalQueryObjectGrid} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    TSRReportVerticalCreate(
      body: FNTReportVerticalQueryObjectGrid,
      options?: any
    ) {
      return FNTReportApiFp(configuration).TSRReportVerticalCreate(
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
      const localVarFetchArgs = FNTReportApiFetchParamCreator(
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
    TSRVerticalUpdateResource(body: TipologicaGridDto, options?: any) {
      return FNTReportApiFp(configuration).TSRVerticalUpdateResource(
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
      return FNTReportApiFp(configuration).TSRReportRestore(id, options)(
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
    TemsFNTReportImport(
      body: File,
      mode: Array<number>,
      verticalId: Array<number>,
      options?: any
    ) {
      return FNTReportApiFp(configuration).TemsFNTReportImport(
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
    FNTRefresh(body?: any, options?: any) {
      return FNTReportApiFp(configuration).FNTRefresh(body, options)(
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
      return FNTReportApiFp(configuration).TSRRefreshStatus(filterId, options)(
        fetch,
        basePath
      );
    },
  };
};

/**
 * FNTReportApi - object-oriented interface
 * @export
 * @class FNTReportApi
 * @extends {BaseAPI}
 */
export class FNTReportApi extends BaseAPI {
  /**
   *
   * @param {number} [id]
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof FNTReportApi
   */
  public TSRReportDelete(id: number, options?: any) {
    return FNTReportApiFp(this.configuration).TSRReportDelete(id, options)(
      this.fetch,
      this.basePath
    );
  }

  /**
   *
   * @param {number} [id]
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof FNTReportApi
   */
  public TSRReportDeleteDeep(id?: number, options?: any) {
    return FNTReportApiFp(this.configuration).TSRReportDeleteDeep(id, options)(
      this.fetch,
      this.basePath
    );
  }
  /**
   *
   * @param {FNTReportQueryObjectGrid} body
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof FNTReportApi
   */
  public NonTemsFNTReportExportReport(
    body: FNTReportQueryObjectGrid,
    options?: any
  ) {
    return FNTReportApiFp(this.configuration).NonTemsFNTReportExportReport(
      body,
      options
    )(this.fetch, this.basePath);
  }

  /**
   *
   * @param {FNTReportQueryObjectGrid} body
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof FNTReportApi
   */
  public FNTReportExportReport(body: FNTReportQueryObjectGrid, options?: any) {
    return FNTReportApiFp(this.configuration).FNTReportExportReport(
      body,
      options
    )(this.fetch, this.basePath);
  }
  /**
   *
   * @param {FNTReportQueryObjectGrid} body
   * @param {string} [propertyName]
   * @param {string} [propertyFilter]
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof FNTReportApi
   */
  public NonTemsFNTReportGetFilterResult(
    body: FNTReportQueryObjectGrid,
    propertyName?: string,
    propertyFilter?: string,
    options?: any
  ) {
    return FNTReportApiFp(this.configuration).NonTemsFNTReportGetFilterResult(
      body,
      propertyName,
      propertyFilter,
      options
    )(this.fetch, this.basePath);
  }
  /**
   *
   * @param {FNTReportQueryObjectGrid} body
   * @param {string} [propertyName]
   * @param {string} [propertyFilter]
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof FNTReportApi
   */
  public FNTReportGetFilterResult(
    body: FNTReportQueryObjectGrid,
    propertyName?: string,
    propertyFilter?: string,
    options?: any
  ) {
    return FNTReportApiFp(this.configuration).FNTReportGetFilterResult(
      body,
      propertyName,
      propertyFilter,
      options
    )(this.fetch, this.basePath);
  }
  /**
   *
   * @param {FNTReportQueryObjectGrid} body
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof FNTReportApi
   */
  public NonTemsFNTReportGetGrid(
    body: FNTReportQueryObjectGrid,
    options?: any
  ) {
    return FNTReportApiFp(this.configuration).NonTemsFNTReportGetGrid(
      body,
      options
    )(this.fetch, this.basePath);
  }
  /**
   *
   * @param {FNTReportQueryObjectGrid} body
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof FNTReportApi
   */
  public FNTReportGetGrid(body: FNTReportQueryObjectGrid, options?: any) {
    return FNTReportApiFp(this.configuration).FNTReportGetGrid(body, options)(
      this.fetch,
      this.basePath
    );
  }
  /**
   *
   * @param {FNTReportQueryObjectGrid} body
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof FNTReportApi
   */
  public ReportSchedulerGet(body: FNTReportQueryObjectGrid, options?: any) {
    return FNTReportApiFp(this.configuration).ReportSchedulerGet(body, options)(
      this.fetch,
      this.basePath
    );
  }
  /**
   *
   * @param {TipologicaGridDto} body
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof ProductImportanceApi
   */
  public ReportSchedulerUpdate(body: TipologicaGridDto, options?: any) {
    return FNTReportApiFp(this.configuration).ReportSchedulerUpdate(
      body,
      options
    )(this.fetch, this.basePath);
  }
  /**
   *
   * @param {TipologicheQueryObjectGrid} body
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof FNTReportApi
   */
  public tsrReportGetVerticalGrid(options?: any) {
    return FNTReportApiFp(this.configuration).tsrReportGetVerticalGrid(options)(
      this.fetch,
      this.basePath
    );
  }

  /**
   *
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof FNTReportApi
   */
  public TSRReportVerticalGetCreateResource(options?: any) {
    return FNTReportApiFp(
      this.configuration
    ).TSRReportVerticalGetCreateResource(options)(this.fetch, this.basePath);
  }

  /**
   *
   * @param {FNTReportVerticalQueryObjectGrid} body
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof ProductImportanceApi
   */
  public TSRReportVerticalCreate(
    body: FNTReportVerticalQueryObjectGrid,
    options?: any
  ) {
    return FNTReportApiFp(this.configuration).TSRReportVerticalCreate(
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
    return FNTReportApiFp(this.configuration).TSRVerticalGetUpdateResource(
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
  public TSRVerticalUpdateResource(body: TipologicaGridDto, options?: any) {
    return FNTReportApiFp(this.configuration).TSRVerticalUpdateResource(
      body,
      options
    )(this.fetch, this.basePath);
  }

  /**
   *
   * @param {number} [id]
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof FNTReportApi
   */
  public TSRReportRestore(id?: number, options?: any) {
    return FNTReportApiFp(this.configuration).TSRReportRestore(id, options)(
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
   * @memberof FNTReportApi
   */
  public TemsFNTReportImport(
    body: File,
    mode: Array<number>,
    verticalId: Array<number>
  ) {
    return FNTReportApiFp(this.configuration).TemsFNTReportImport(
      body,
      mode,
      verticalId
    )(this.fetch, this.basePath);
  }

  /**
   * @param {any} body
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof FNTReportApi
   */
  public FNTRefresh(body?: any) {
    return FNTReportApiFp(this.configuration).FNTRefresh(body)(
      this.fetch,
      this.basePath
    );
  }

  /**
   * @param {number} filterId
   * @param {*} [options]
   * @throws {RequiredError}
   * @memberof FNTReportApi
   */
  public TSRRefreshStatus(filterId: number) {
    return FNTReportApiFp(this.configuration).TSRRefreshStatus(filterId)(
      this.fetch,
      this.basePath
    );
  }
}
