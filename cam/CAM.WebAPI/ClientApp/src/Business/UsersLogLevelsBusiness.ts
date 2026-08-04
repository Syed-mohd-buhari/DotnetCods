import * as isomorphicFetch from "isomorphic-fetch";
import * as url from "url";
import { ReturnFile } from "../Model/Common";

import {
  GetUsersLoggingLevels,
  UserLogLevelsBody,
} from "../Model/UsersLoggingLevels";
import {
  BaseAPI,
  BASE_PATH,
  FetchAPI,
  FetchArgs,
  RequiredError,
} from "./Common/CommonBusiness";
import { Configuration } from "./Common/configuration";

import { headerObj } from "./header";

/**
 * UserLoggingLevelApi - fetch parameter creator


 * @export
 */
export const UsersLoggingLevelsApiFetchParamCreator = function (
  configuration?: Configuration
) {
  return {
    GetCreatePageForUsersLoggingLevels(options: any = {}): FetchArgs {
      const localVarPath = `/api/UserLoggingLevel/GetCreatePageForLoggingLevels`;
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
    getLogLevelById(id: number, options: any = {}): FetchArgs {
      // verify required parameter 'id' is not null or undefined
      if (id === null || id === undefined) {
        throw new RequiredError(
          "id",
          "Required parameter id was null or undefined when calling getLogLevelById."
        );
      }
      const localVarPath = `/api/UserLoggingLevel/GetLogLevelsById{id}`.replace(
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
     * @param {UserLogLevelsBody} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    userLoggingLevelCreate(
      body: UserLogLevelsBody,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling userLoggingLevelCreate."
        );
      }
      const localVarPath = `/api/UserLoggingLevel`;
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

    userDwnloadLogs(options: any = {}) {
      const localVarPath = `/api/UserLoggingLevel/DownloadLogs`;
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
 * UserLoggingLevelApi - functional programming interface
 * @export
 */
export const UserLoggingLevelApiFp = function (configuration?: Configuration) {
  return {
    GetCreatePageForUsersLoggingLevels(
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<GetUsersLoggingLevels> {
      const localVarFetchArgs =
        UsersLoggingLevelsApiFetchParamCreator(
          configuration
        ).GetCreatePageForUsersLoggingLevels(options);
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
    getLogLevelById(
      id: number,
      options?: any
    ): (
      fetch?: FetchAPI,
      basePath?: string
    ) => Promise<{ data: string; info: string; warning: boolean }> {
      const localVarFetchArgs = UsersLoggingLevelsApiFetchParamCreator(
        configuration
      ).getLogLevelById(id, options);
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
     * @param {UserLogLevelsBody} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    userLoggingLevelCreate(
      body: UserLogLevelsBody,
      options?: any
    ): (
      fetch?: FetchAPI,
      basePath?: string
    ) => Promise<{ info: string; warning: boolean }> {
      const localVarFetchArgs = UsersLoggingLevelsApiFetchParamCreator(
        configuration
      ).userLoggingLevelCreate(body, options);
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

    userDwnloadLogs(
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ReturnFile> {
      const localVarFetchArgs =
        UsersLoggingLevelsApiFetchParamCreator(configuration).userDwnloadLogs(
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
  };
};

/**
 * UserLoggingLevelApi - factory interface
 * @export
 */
export const UserLoggingLevelApiFactory = function (
  configuration?: Configuration,
  fetch?: FetchAPI,
  basePath?: string
) {
  return {
    GetCreatePageForUsersLoggingLevels(options?: any) {
      return UserLoggingLevelApiFp(
        configuration
      ).GetCreatePageForUsersLoggingLevels(options)(fetch, basePath);
    },

    /**
     *
     * @param {number} id
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    getLogLevelById(id: number, options?: any) {
      return UserLoggingLevelApiFp(configuration).getLogLevelById(id, options)(
        fetch,
        basePath
      );
    },

    /**
     *
     * @param {UserLogLevelsBody} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    userLoggingLevelCreate(body: UserLogLevelsBody, options?: any) {
      return UserLoggingLevelApiFp(configuration).userLoggingLevelCreate(
        body,
        options
      )(fetch, basePath);
    },

    userDwnloadLogs(options?: any) {
      return UserLoggingLevelApiFp(configuration).userDwnloadLogs(options)(
        fetch,
        basePath
      );
    },
  };
};

/**
 * UserLoggingLevelApi - object-oriented interface
 * @export
 * @class UserLoggingLevelApi
 * @extends {BaseAPI}
 */
export class UserLoggingLevelApi extends BaseAPI {
  public GetCreatePageForUsersLoggingLevels(options?: any) {
    return UserLoggingLevelApiFp(
      this.configuration
    ).GetCreatePageForUsersLoggingLevels(options)(this.fetch, this.basePath);
  }

  /**
   *
   * @param {number} id
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof VodafoneNameApi
   */
  public getLogLevelById(id: number, options?: any) {
    return UserLoggingLevelApiFp(this.configuration).getLogLevelById(
      id,
      options
    )(this.fetch, this.basePath);
  }

  /**
   *
   * @param {UserLogLevelsBody} body
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof VodafoneNameApi
   */
  public userLoggingLevelCreate(body: UserLogLevelsBody, options?: any) {
    return UserLoggingLevelApiFp(this.configuration).userLoggingLevelCreate(
      body,
      options
    )(this.fetch, this.basePath);
  }

  public userDwnloadLogs(options?: any) {
    return UserLoggingLevelApiFp(this.configuration).userDwnloadLogs(options)(
      this.fetch,
      this.basePath
    );
  }
}

export const GEt_USERS_LOGGING_LEVEL = "GEt_USERS_LOGGING_LEVEL";
export const GET_USERS_ROLLES_BY_ID = "GET_USERS_ROLLES_BY_ID";
