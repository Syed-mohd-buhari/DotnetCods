import * as isomorphicFetch from "isomorphic-fetch";
import * as url from "url";
import { QueryResultDtoOfUserDefinedReportsLogsDtoGrid } from "../Model/UserDefinedReportsLogs";
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
import { UserDefinedReportsLogsQueryObjectGrid as UserDefinedReportsLogsQueryDto } from "../Model/UserDefinedReportsLogs";
import { NetworkElementAsIsApiFp } from "./NetworkElementAsIsBusiness";
import { ResultDto } from "../Model/CommonModels";
import { ReturnFile } from "../Model/Common";

/**
 * UserDefinedReportsLogsApi - fetch parameter creator
 * @export
 */
export const UserDefinedReportsLogsApiFetchParamCreator = function (
  configuration?: Configuration
) {
  return {
    /**
     *
     * @param {any} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    DownloadUserDefinedReportsLogs(body: any, options: any = {}): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      const localVarPath = `/api/UserDefinedReportLog/ExportReport`;
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
     * @param {UserDefinedReportsLogsQueryDto} body
     * @param {string} [propertyName]
     * @param {string} [propertyFilter]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    UserDefinedReportsLogsGetFilterResult(
      body: UserDefinedReportsLogsQueryDto,
      propertyName?: string,
      propertyFilter?: string,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling UserDefinedReportsLogsGetFilterResult."
        );
      }
      const localVarPath = `/api/UserDefinedReportLog/Filter`;
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
        <any>"UserDefinedReportsLogsQueryDto" !== "string" ||
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
     * @param {UserDefinedReportsLogsQueryDto} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    GetUserDefinedReportsLogsGrid(
      body: UserDefinedReportsLogsQueryDto,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling GetUserDefinedReportsLogsGrid."
        );
      }
      const localVarPath = `/api/UserDefinedReportLog/Get`;
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
        <any>"UserDefinedReportsLogsQueryDto" !== "string" ||
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
 * UserDefinedReportsLogsApi - functional programming interface
 * @export
 */
export const UserDefinedReportsLogsApiFp = function (
  configuration?: Configuration
) {
  return {
    /**
     *
     * @param {any} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    DownloadUserDefinedReportsLogs(
      body: any,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ReturnFile> {
      const localVarFetchArgs = UserDefinedReportsLogsApiFetchParamCreator(
        configuration
      ).DownloadUserDefinedReportsLogs(body, options);
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
     * @param {UserDefinedReportsLogsQueryDto} body
     * @param {string} [propertyName]
     * @param {string} [propertyFilter]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    UserDefinedReportsLogsGetFilterResult(
      body: UserDefinedReportsLogsQueryDto,
      propertyName?: string,
      propertyFilter?: string,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<Array<FilterValueDto>> {
      const localVarFetchArgs = UserDefinedReportsLogsApiFetchParamCreator(
        configuration
      ).UserDefinedReportsLogsGetFilterResult(
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
     * @param {UserDefinedReportsLogsQueryDto} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    GetUserDefinedReportsLogsGrid(
      body: UserDefinedReportsLogsQueryDto,
      options?: any
    ): (
      fetch?: FetchAPI,
      basePath?: string
    ) => Promise<QueryResultDtoOfUserDefinedReportsLogsDtoGrid> {
      const localVarFetchArgs = UserDefinedReportsLogsApiFetchParamCreator(
        configuration
      ).GetUserDefinedReportsLogsGrid(body, options);
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
 * UserDefinedReportsLogsApi - factory interface
 * @export
 */
export const UserDefinedReportsLogsApiFactory = function (
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
    DownloadUserDefinedReportsLogs(body: any, options?: any) {
      return UserDefinedReportsLogsApiFp(
        configuration
      ).DownloadUserDefinedReportsLogs(body, options)(fetch, basePath);
    },

    /**
     *
     * @param {UserDefinedReportsLogsQueryDto} body
     * @throws {RequiredError}
     */
    GetUserDefinedReportsLogsGrid(body: UserDefinedReportsLogsQueryDto) {
      return UserDefinedReportsLogsApiFp(
        configuration
      ).GetUserDefinedReportsLogsGrid(body)(fetch, basePath);
    },
  };
};

/**
 * UserDefinedReportsLogsApi - object-oriented interface
 * @export
 * @class UserDefinedReportsLogsApi
 * @extends {BaseAPI}
 */
export class UserDefinedReportsLogsApi extends BaseAPI {
  /**
   *
   * @param {any} body
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof UserDefinedReportsLogsApi
   */
  public DownloadUserDefinedReportsLogs(body: any, options?: any) {
    return UserDefinedReportsLogsApiFp(
      this.configuration
    ).DownloadUserDefinedReportsLogs(body, options)(this.fetch, this.basePath);
  }

  /**
   *
   * @param {UserDefinedReportsLogsQueryDto} body
   * @param {string} [propertyName]
   * @param {string} [propertyFilter]
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof UserDefinedReportsLogsApi
   */
  public UserDefinedReportsLogsGetFilterResult(
    body: UserDefinedReportsLogsQueryDto,
    propertyName?: string,
    propertyFilter?: string,
    options?: any
  ) {
    return UserDefinedReportsLogsApiFp(
      this.configuration
    ).UserDefinedReportsLogsGetFilterResult(
      body,
      propertyName,
      propertyFilter,
      options
    )(this.fetch, this.basePath);
  }

  /**
   *
   * @param {UserDefinedReportsLogsQueryDto} body
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof UserDefinedReportsLogsApi
   */
  public GetUserDefinedReportsLogsGrid(
    body: UserDefinedReportsLogsQueryDto,
    options?: any
  ) {
    return UserDefinedReportsLogsApiFp(
      this.configuration
    ).GetUserDefinedReportsLogsGrid(body, options)(this.fetch, this.basePath);
  }
}

export const GET_USER_DEFINED_REPORTS_LOGS = "GET_USER_DEFINED_REPORTS_LOGS";
