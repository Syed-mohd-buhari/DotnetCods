import * as isomorphicFetch from "isomorphic-fetch";
import * as url from "url";
import { QueryResultDtoOfFeedbackLogsDtoGrid } from "../Model/FeedbackLogs";
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
import { FeedbackLogsQueryObjectGrid as FeedbackLogsQueryDto } from "../Model/FeedbackLogs";
import { NetworkElementAsIsApiFp } from "./NetworkElementAsIsBusiness";
import { ResultDto } from "../Model/CommonModels";
import { ReturnFile } from "../Model/Common";

/**
 * FeedbackLogsApi - fetch parameter creator
 * @export
 */
export const FeedbackLogsApiFetchParamCreator = function (
  configuration?: Configuration
) {
  return {
    /**
     *
     * @param {any} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    DownloadFeedbackLogs(body: any, options: any = {}): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      const localVarPath = `/api/FeedBackLoopLog/ExportReport`;
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
     * @param {FeedbackLogsQueryDto} body
     * @param {string} [propertyName]
     * @param {string} [propertyFilter]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    FeedbackLogsGetFilterResult(
      body: FeedbackLogsQueryDto,
      propertyName?: string,
      propertyFilter?: string,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling FeedbackLogsGetFilterResult."
        );
      }
      const localVarPath = `/api/FeedBackLoopLog/Filter`;
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
        <any>"FeedbackLogsQueryDto" !== "string" ||
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
     * @param {FeedbackLogsQueryDto} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    getFeedbackLogsGrid(
      body: FeedbackLogsQueryDto,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling getFeedbackLogsGrid."
        );
      }
      const localVarPath = `/api/FeedBackLoopLog/Get`;
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
        <any>"FeedbackLogsQueryDto" !== "string" ||
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
 * FeedbackLogsApi - functional programming interface
 * @export
 */
export const FeedbackLogsApiFp = function (configuration?: Configuration) {
  return {
    /**
     *
     * @param {any} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    DownloadFeedbackLogs(
      body: any,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ReturnFile> {
      const localVarFetchArgs = FeedbackLogsApiFetchParamCreator(
        configuration
      ).DownloadFeedbackLogs(body, options);
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
     * @param {FeedbackLogsQueryDto} body
     * @param {string} [propertyName]
     * @param {string} [propertyFilter]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    FeedbackLogsGetFilterResult(
      body: FeedbackLogsQueryDto,
      propertyName?: string,
      propertyFilter?: string,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<Array<FilterValueDto>> {
      const localVarFetchArgs = FeedbackLogsApiFetchParamCreator(
        configuration
      ).FeedbackLogsGetFilterResult(
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
     * @param {FeedbackLogsQueryDto} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    getFeedbackLogsGrid(
      body: FeedbackLogsQueryDto,
      options?: any
    ): (
      fetch?: FetchAPI,
      basePath?: string
    ) => Promise<QueryResultDtoOfFeedbackLogsDtoGrid> {
      const localVarFetchArgs = FeedbackLogsApiFetchParamCreator(
        configuration
      ).getFeedbackLogsGrid(body, options);
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
 * FeedbackLogsApi - factory interface
 * @export
 */
export const FeedbackLogsApiFactory = function (
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
    DownloadFeedbackLogs(body: any, options?: any) {
      return FeedbackLogsApiFp(configuration).DownloadFeedbackLogs(
        body,
        options
      )(fetch, basePath);
    },

    /**
     *
     * @param {FeedbackLogsQueryDto} body
     * @throws {RequiredError}
     */
    getFeedbackLogsGrid(body: FeedbackLogsQueryDto) {
      return FeedbackLogsApiFp(configuration).getFeedbackLogsGrid(body)(
        fetch,
        basePath
      );
    },
  };
};

/**
 * FeedbackLogsApi - object-oriented interface
 * @export
 * @class FeedbackLogsApi
 * @extends {BaseAPI}
 */
export class FeedbackLogsApi extends BaseAPI {
  /**
   *
   * @param {any} body
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof FeedbackLogsApi
   */
  public DownloadFeedbackLogs(body: any, options?: any) {
    return FeedbackLogsApiFp(this.configuration).DownloadFeedbackLogs(
      body,
      options
    )(this.fetch, this.basePath);
  }

  /**
   *
   * @param {FeedbackLogsQueryDto} body
   * @param {string} [propertyName]
   * @param {string} [propertyFilter]
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof FeedbackLogsApi
   */
  public FeedbackLogsGetFilterResult(
    body: FeedbackLogsQueryDto,
    propertyName?: string,
    propertyFilter?: string,
    options?: any
  ) {
    return FeedbackLogsApiFp(this.configuration).FeedbackLogsGetFilterResult(
      body,
      propertyName,
      propertyFilter,
      options
    )(this.fetch, this.basePath);
  }

  /**
   *
   * @param {FeedbackLogsQueryDto} body
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof FeedbackLogsApi
   */
  public getFeedbackLogsGrid(body: FeedbackLogsQueryDto, options?: any) {
    return FeedbackLogsApiFp(this.configuration).getFeedbackLogsGrid(
      body,
      options
    )(this.fetch, this.basePath);
  }
}

export const GET_FEEDBACK_LOGS = "GET_FEEDBACK_LOGS";
