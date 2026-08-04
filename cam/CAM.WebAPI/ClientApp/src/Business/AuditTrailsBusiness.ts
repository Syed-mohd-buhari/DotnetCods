import * as isomorphicFetch from "isomorphic-fetch";
import * as url from "url";
import { QueryResultDtoOfAuditTrailsDtoGrid } from "../Model/AuditTrails";
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
import { AuditTrailsQueryObjectGrid as AuditTrailsQueryDto } from "../Model/AuditTrails";
import { NetworkElementAsIsApiFp } from "./NetworkElementAsIsBusiness";
import { ResultDto } from "../Model/CommonModels";
import { ReturnFile } from "../Model/Common";

/**
 * AuditTrailsApi - fetch parameter creator
 * @export
 */
export const AuditTrailsApiFetchParamCreator = function (
  configuration?: Configuration
) {
  return {
    /**
     *
     * @param {any} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    DownloadAuditTrails(body: any, options: any = {}): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      const localVarPath = `/api/AuditLog/ExportReport`;
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
     * @param {AuditTrailsQueryDto} body
     * @param {string} [propertyName]
     * @param {string} [propertyFilter]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    generiocReportGetFilterResult(
      body: AuditTrailsQueryDto,
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
      const localVarPath = `/api/AuditLog/Filter`;
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
        <any>"AuditTrailsQueryDto" !== "string" ||
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
     * @param {AuditTrailsQueryDto} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    getAuditTrailsGrid(
      body: AuditTrailsQueryDto,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling getAuditTrailsGrid."
        );
      }
      const localVarPath = `/api/AuditLog/Get`;
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
        <any>"AuditTrailsQueryDto" !== "string" ||
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
 * AuditTrailsApi - functional programming interface
 * @export
 */
export const AuditTrailsApiFp = function (configuration?: Configuration) {
  return {
    /**
     *
     * @param {any} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    DownloadAuditTrails(
      body: any,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ReturnFile> {
      const localVarFetchArgs = AuditTrailsApiFetchParamCreator(
        configuration
      ).DownloadAuditTrails(body, options);
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
     * @param {AuditTrailsQueryDto} body
     * @param {string} [propertyName]
     * @param {string} [propertyFilter]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    generiocReportGetFilterResult(
      body: AuditTrailsQueryDto,
      propertyName?: string,
      propertyFilter?: string,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<Array<FilterValueDto>> {
      const localVarFetchArgs = AuditTrailsApiFetchParamCreator(
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
     * @param {AuditTrailsQueryDto} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    getAuditTrailsGrid(
      body: AuditTrailsQueryDto,
      options?: any
    ): (
      fetch?: FetchAPI,
      basePath?: string
    ) => Promise<QueryResultDtoOfAuditTrailsDtoGrid> {
      const localVarFetchArgs = AuditTrailsApiFetchParamCreator(
        configuration
      ).getAuditTrailsGrid(body, options);
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
 * AuditTrailsApi - factory interface
 * @export
 */
export const AuditTrailsApiFactory = function (
  configuration?: Configuration,
  fetch?: FetchAPI,
  basePath?: string
) {
  return {
    /**
     *
     * @param {any} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    DownloadAuditTrails(body: any, options?: any) {
      return AuditTrailsApiFp(configuration).DownloadAuditTrails(body, options)(
        fetch,
        basePath
      );
    },

    /**
     *
     * @param {AuditTrailsQueryDto} body
     * @throws {RequiredError}
     */
    getAuditTrailsGrid(body: AuditTrailsQueryDto) {
      return AuditTrailsApiFp(configuration).getAuditTrailsGrid(body)(
        fetch,
        basePath
      );
    },
  };
};

/**
 * AuditTrailsApi - object-oriented interface
 * @export
 * @class AuditTrailsApi
 * @extends {BaseAPI}
 */
export class AuditTrailsApi extends BaseAPI {
  /**
   *
   * @param {any} body
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof AuditTrailsApi
   */
  public DownloadAuditTrails(body: any, options?: any) {
    return AuditTrailsApiFp(this.configuration).DownloadAuditTrails(
      body,
      options
    )(this.fetch, this.basePath);
  }

  /**
   *
   * @param {AuditTrailsQueryDto} body
   * @param {string} [propertyName]
   * @param {string} [propertyFilter]
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof AuditTrailsApi
   */
  public AuditTrailsGetFilterResult(
    body: AuditTrailsQueryDto,
    propertyName?: string,
    propertyFilter?: string,
    options?: any
  ) {
    return AuditTrailsApiFp(this.configuration).generiocReportGetFilterResult(
      body,
      propertyName,
      propertyFilter,
      options
    )(this.fetch, this.basePath);
  }

  /**
   *
   * @param {AuditTrailsQueryDto} body
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof AuditTrailsApi
   */
  public getAuditTrailsGrid(body: AuditTrailsQueryDto, options?: any) {
    return AuditTrailsApiFp(this.configuration).getAuditTrailsGrid(
      body,
      options
    )(this.fetch, this.basePath);
  }
}

export const GET_AUDIT_TRAILS = "GET_AUDIT_TRAILS";
