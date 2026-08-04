import * as isomorphicFetch from "isomorphic-fetch";
import * as url from "url";
import {
  CreateGenericReportBody,
  GetGenericReportResponse,
  QueryResultDtoOfGenericReportDtoGrid,
  QueryResultDtoOfPreviewGenericReportDtoGrid,
} from "../Model/GenericReport";
import {
  BaseAPI,
  BASE_PATH,
  FetchAPI,
  FetchArgs,
  RequiredError,
  FilterValueDto,
} from "./Common/CommonBusiness";
import { Configuration } from "./Common/configuration";
import { headerObj } from "./header";
import {
  GenericReportQueryObjectGrid as GenericReportQueryDto,
  GenericPreviewReportQueryObjectGrid as GenericPreviewReportQueryDto,
} from "../Model/GenericReport";
import { NetworkElementAsIsApiFp } from "./NetworkElementAsIsBusiness";
import { ResultDto } from "../Model/CommonModels";
import { ReturnFile } from "../Model/Common";

/**
 * GenericReportApi - fetch parameter creator
 * @export
 */
export const GenericReportApiFetchParamCreator = function (
  configuration?: Configuration
) {
  return {
    GetGenericReport(options: any = {}): FetchArgs {
      const localVarPath = `/api/GenericReport/Get`;
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
     * @param {number} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    genericReportUpdateStatus(body: number, options: any = {}): FetchArgs {
      const payload = {
        dynamicReportId: [body],
      };
      const localVarPath = `/api/GenericReport/Update`;
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
        <any>"NetworkElementAsIsDtoUpdate" !== "string" ||
        localVarRequestOptions.headers["Content-Type"] === "application/json";
      localVarRequestOptions.body = needsSerialization
        ? JSON.stringify(payload || {})
        : payload || "";

      return {
        url: url.format(localVarUrlObj),
        options: localVarRequestOptions,
      };
    },
    /**
     *
     * @param {any} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    genericReportClone(body: any, options: any = {}): FetchArgs {
      const localVarPath = `/api/GenericReport/CloneRecord`;
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
        <any>"NetworkElementAsIsDtoUpdate" !== "string" ||
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
     * @param {any} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    DownloadGenericReport(body: any, options: any = {}): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      const localVarPath = `/api/GenericReport/ExportReport`;
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
        <any>"NetworkElementsAsIsQueryDto" !== "string" ||
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
     * @param {any} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    DownloadGenericCSVReport(body: any, options: any = {}): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      const localVarPath = `/api/GenericReport/ExportDynamicReportCSV`;
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
        <any>"NetworkElementsAsIsQueryDto" !== "string" ||
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
     * @param {number} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    genericReportDelete(body: number, options: any = {}): FetchArgs {
      const payload = {
        dynamicReportId: [body],
      };
      const localVarPath = `/api/GenericReport/Delete`;
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
        <any>"NetworkElementAsIsDtoUpdate" !== "string" ||
        localVarRequestOptions.headers["Content-Type"] === "application/json";
      localVarRequestOptions.body = needsSerialization
        ? JSON.stringify(payload || {})
        : payload || "";

      return {
        url: url.format(localVarUrlObj),
        options: localVarRequestOptions,
      };
    },
    /**
     *
     * @param {GenericReportQueryDto} body
     * @param {string} [propertyName]
     * @param {string} [propertyFilter]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    generiocReportGetFilterResult(
      body: GenericReportQueryDto,
      propertyName?: string,
      propertyFilter?: string,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling generiocReportGetFilterResult."
        );
      }
      const localVarPath = `/api/GenericReport/Filter`;
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
        <any>"GenericReportQueryDto" !== "string" ||
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
     * @param {GenericReportQueryDto} body
     * @param {string} [propertyName]
     * @param {string} [propertyFilter]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    genericPreviewReportGetFilterResult(
      body: GenericReportQueryDto,
      propertyName?: string,
      propertyFilter?: string,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling genericPreviewReportGetFilterResult."
        );
      }
      const localVarPath = `/api/GenericReport/DynamicReportFilter`;
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
        <any>"GenericReportQueryDto" !== "string" ||
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
     * @param {number} reportId
     * @param {string} [apiType]
     * @param {GenericReportQueryDto} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    genericReportPreview(
      body: GenericReportQueryDto,
      apiType?: string,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling getGenericViewGrid."
        );
      }
      const localVarPath = `/api/GenericReport/${
        apiType == "edit" ? "GetData" : "GetUserDefinedReport"
      }`;
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
        <any>"GenericPreviewReportQueryDto" !== "string" ||
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
     * @param {CreateGenericReportBody} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    genericReportCreate(
      body: CreateGenericReportBody,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling genericReportCreate."
        );
      }
      const localVarPath = `/api/GenericReport/Save`;
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
     * @param {GenericReportQueryDto} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    getGenericGrid(body: GenericReportQueryDto, options: any = {}): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling getGenericGrid."
        );
      }
      const localVarPath = `/api/GenericReport/GetReport`;
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
        <any>"GenericReportQueryDto" !== "string" ||
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
 * GenericReportApi - functional programming interface
 * @export
 */
export const GenericReportApiFp = function (configuration?: Configuration) {
  return {
    GetGenericReport(
      options?: any
    ): (
      fetch?: FetchAPI,
      basePath?: string
    ) => Promise<GetGenericReportResponse> {
      const localVarFetchArgs =
        GenericReportApiFetchParamCreator(configuration).GetGenericReport(
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
     * @param {number} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    genericReportUpdateStatus(
      body: number,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = GenericReportApiFetchParamCreator(
        configuration
      ).genericReportUpdateStatus(body, options);
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
     * @param {any} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    genericReportClone(
      body: any,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = GenericReportApiFetchParamCreator(
        configuration
      ).genericReportClone(body, options);
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
     * @param {any} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    DownloadGenericReport(
      body: any,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ReturnFile> {
      const localVarFetchArgs = GenericReportApiFetchParamCreator(
        configuration
      ).DownloadGenericReport(body, options);
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
     * @param {any} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    DownloadGenericCSVReport(
      body: any,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ReturnFile> {
      const localVarFetchArgs = GenericReportApiFetchParamCreator(
        configuration
      ).DownloadGenericCSVReport(body, options);
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
     * @param {number} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    genericReportDelete(
      body: number,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = GenericReportApiFetchParamCreator(
        configuration
      ).genericReportDelete(body, options);
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
     * @param {GenericReportQueryDto} body
     * @param {string} [propertyName]
     * @param {string} [propertyFilter]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    generiocReportGetFilterResult(
      body: GenericReportQueryDto,
      propertyName?: string,
      propertyFilter?: string,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<Array<FilterValueDto>> {
      const localVarFetchArgs = GenericReportApiFetchParamCreator(
        configuration
      ).generiocReportGetFilterResult(
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
     * @param {GenericReportQueryDto} body
     * @param {string} [propertyName]
     * @param {string} [propertyFilter]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    genericPreviewReportGetFilterResult(
      body: GenericReportQueryDto,
      propertyName?: string,
      propertyFilter?: string,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<Array<FilterValueDto>> {
      const localVarFetchArgs = GenericReportApiFetchParamCreator(
        configuration
      ).genericPreviewReportGetFilterResult(
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
     * @param {number} reportId
     * @param {string} [apiType]
     * @param {GenericReportQueryDto} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    genericReportPreview(
      body: GenericReportQueryDto,
      apiType?: string,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<any> {
      const localVarFetchArgs = GenericReportApiFetchParamCreator(
        configuration
      ).genericReportPreview(body, apiType, options);
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
     * @param {CreateGenericReportBody} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    genericReportCreate(
      body: CreateGenericReportBody,
      options?: any
    ): (
      fetch?: FetchAPI,
      basePath?: string
    ) => Promise<{ info: string; warning: boolean }> {
      const localVarFetchArgs = GenericReportApiFetchParamCreator(
        configuration
      ).genericReportCreate(body, options);
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
     * @param {GenericReportQueryDto} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    getGenericGrid(
      body: GenericReportQueryDto,
      options?: any
    ): (
      fetch?: FetchAPI,
      basePath?: string
    ) => Promise<QueryResultDtoOfGenericReportDtoGrid> {
      const localVarFetchArgs = GenericReportApiFetchParamCreator(
        configuration
      ).getGenericGrid(body, options);
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
 * GenericReportApi - factory interface
 * @export
 */
export const GenericReportApiFactory = function (
  configuration?: Configuration,
  fetch?: FetchAPI,
  basePath?: string
) {
  return {
    GetGenericReport(options?: any) {
      return GenericReportApiFp(configuration).GetGenericReport(options)(
        fetch,
        basePath
      );
    },
    /**
     *
     * @param {CreateGenericReportBody} body
     * @throws {RequiredError}
     */
    genericReportCreate(body: CreateGenericReportBody) {
      return GenericReportApiFp(configuration).GetGenericReport(body)(
        fetch,
        basePath
      );
    },
    /**
     *
     * @param {number} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    genericReportUpdateStatus(body: number, options?: any) {
      return GenericReportApiFp(configuration).genericReportUpdateStatus(
        body,
        options
      )(fetch, basePath);
    },
    /**
     *
     * @param {any} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    genericReportClone(body: any, options?: any) {
      return GenericReportApiFp(configuration).genericReportClone(
        body,
        options
      )(fetch, basePath);
    },
    /**
     *
     * @param {any} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    DownloadGenericReport(body: any, options?: any) {
      return GenericReportApiFp(configuration).DownloadGenericReport(
        body,
        options
      )(fetch, basePath);
    },
    /**
     *
     * @param {any} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    DownloadGenericCSVReport(body: any, options?: any) {
      return GenericReportApiFp(configuration).DownloadGenericCSVReport(
        body,
        options
      )(fetch, basePath);
    },
    /**
     *
     * @param {number} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    genericReportDelete(body: number, options?: any) {
      return GenericReportApiFp(configuration).genericReportDelete(
        body,
        options
      )(fetch, basePath);
    },
    /**
     *
     * @param {GenericReportQueryDto} body
     * @param {string} [propertyName]
     * @param {string} [propertyFilter]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    genericPreviewReportGetFilterResult(
      body: GenericReportQueryDto,
      propertyName?: string,
      propertyFilter?: string,
      options?: any
    ) {
      return GenericReportApiFp(
        configuration
      ).genericPreviewReportGetFilterResult(
        body,
        propertyName,
        propertyFilter,
        options
      )(fetch, basePath);
    },
    /**
     *
     * @param {GenericReportQueryDto} body
     * @param {string} [apiType]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    genericReportPreview(
      body: GenericReportQueryDto,
      apiType?: string,
      options?: any
    ) {
      return GenericReportApiFp(configuration).genericReportPreview(
        body,
        apiType
      )(fetch, basePath);
    },
    /**
     *
     * @param {GenericReportQueryDto} body
     * @throws {RequiredError}
     */
    getGenericGrid(body: GenericReportQueryDto) {
      return GenericReportApiFp(configuration).GetGenericReport(body)(
        fetch,
        basePath
      );
    },
  };
};

/**
 * GenericReportApi - object-oriented interface
 * @export
 * @class GenericReportApi
 * @extends {BaseAPI}
 */
export class GenericReportApi extends BaseAPI {
  public GetGenericReport(options?: any) {
    return GenericReportApiFp(this.configuration).GetGenericReport(options)(
      this.fetch,
      this.basePath
    );
  }

  /**
   *
   * @param {CreateGenericReportBody} body
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   */
  public genericReportCreate(body: CreateGenericReportBody, options?: any) {
    return GenericReportApiFp(this.configuration).genericReportCreate(
      body,
      options
    )(this.fetch, this.basePath);
  }
  /**
   *
   * @param {number} body
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof GenericReportApi
   */
  public genericReportUpdateStatus(body: number, options?: any) {
    return GenericReportApiFp(this.configuration).genericReportUpdateStatus(
      body,
      options
    )(this.fetch, this.basePath);
  }
  /**
   *
   * @param {any} body
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof GenericReportApi
   */
  public genericReportClone(body: any, options?: any) {
    return GenericReportApiFp(this.configuration).genericReportClone(
      body,
      options
    )(this.fetch, this.basePath);
  }
  /**
   *
   * @param {any} body
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof GenericReportApi
   */
  public DownloadGenericReport(body: any, options?: any) {
    return GenericReportApiFp(this.configuration).DownloadGenericReport(
      body,
      options
    )(this.fetch, this.basePath);
  }
  /**
   *
   * @param {any} body
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof GenericReportApi
   */
  public DownloadGenericCSVReport(body: any, options?: any) {
    return GenericReportApiFp(this.configuration).DownloadGenericCSVReport(
      body,
      options
    )(this.fetch, this.basePath);
  }
  /**
   *
   * @param {number} body
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof GenericReportApi
   */
  public genericReportDelete(body: number, options?: any) {
    return GenericReportApiFp(this.configuration).genericReportDelete(
      body,
      options
    )(this.fetch, this.basePath);
  }
  /**
   *
   * @param {GenericReportQueryDto} body
   * @param {string} [propertyName]
   * @param {string} [propertyFilter]
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof GenericReportApi
   */
  public genericReportGetFilterResult(
    body: GenericReportQueryDto,
    propertyName?: string,
    propertyFilter?: string,
    options?: any
  ) {
    return GenericReportApiFp(this.configuration).generiocReportGetFilterResult(
      body,
      propertyName,
      propertyFilter,
      options
    )(this.fetch, this.basePath);
  }
  /**
   *
   * @param {GenericReportQueryDto} body
   * @param {string} [propertyName]
   * @param {string} [propertyFilter]
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof GenericReportApi
   */
  public genericPreviewReportGetFilterResult(
    body: GenericReportQueryDto,
    propertyName?: string,
    propertyFilter?: string,
    options?: any
  ) {
    return GenericReportApiFp(
      this.configuration
    ).genericPreviewReportGetFilterResult(
      body,
      propertyName,
      propertyFilter,
      options
    )(this.fetch, this.basePath);
  }

  /**
   *
   * @param {GenericReportQueryDto} body
   * @param {string} [apiType]
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof GenericReportApi
   */
  public genericReportPreview(
    body: GenericReportQueryDto,
    apiType?: string,
    options?: any
  ) {
    return GenericReportApiFp(this.configuration).genericReportPreview(
      body,
      apiType,
      options
    )(this.fetch, this.basePath);
  }
  /**
   *
   * @param {GenericReportQueryDto} body
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof GenericReportApi
   */
  public GetGenericGrid(body: GenericReportQueryDto, options?: any) {
    return GenericReportApiFp(this.configuration).getGenericGrid(body, options)(
      this.fetch,
      this.basePath
    );
  }
}

export const GET_GENERIC_REPORT = "GET_GENERIC_REPORT";
