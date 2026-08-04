import { BaseAPI, RequiredError } from "./Common/CommonBusiness";
import * as url from "url";
import * as portableFetch from "portable-fetch";
import { Configuration } from "./Common/configuration";
import { BASE_PATH, FetchArgs, FetchAPI } from "./Common/CommonBusiness";
import { ResultDto, NetworkElementGraph } from "../Model/CommonModels";
import { headerObj } from "./header";
import * as isomorphicFetch from "isomorphic-fetch";
import { AssestOverviewByMarketQueryObjectGrid } from "../Model/NetworkElementAsPlanned";
import { ReturnFile } from "../Model/Common";

/**
 * GridApi - fetch parameter creator
 * @export
 */
export const NetworkElementGraphApiFetchParamCreator = function (
  configuration?: Configuration
) {
  return {
    /**
     *
     * @param {AssestOverviewByMarketQueryObjectGrid} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */

    networkElementGraph(
      body: AssestOverviewByMarketQueryObjectGrid,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling networkElementGraph."
        );
      }
      const localVarPath = `/api/AssetOverviewbyMarket/GetAssetOverviewbyMarket`;
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
        <any>"SaveGrid" !== "string" ||
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
     * @param {AssestOverviewByMarketQueryObjectGrid} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    AssetOverviewByMarketExport(
      body: AssestOverviewByMarketQueryObjectGrid,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling AssetOverviewByMarketExport."
        );
      }
      const localVarPath = `/api/AssetOverviewbyMarket/ExportReport`;
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
        <any>"AssestOverviewByMarketQueryObjectGrid" !== "string" ||
        localVarRequestOptions.headers["Content-Type"] === "application/json";
      localVarRequestOptions.body = needsSerialization
        ? JSON.stringify(body || {})
        : body || "";

      return {
        url: url.format(localVarUrlObj),
        options: localVarRequestOptions,
      };
    },

    networkElementGraphGetAllOpcos(options: any = {}): FetchArgs {
      const localVarPath = `/api/NetworkElementGraph/GetAllOpcos`;
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
 * NetworkElementGraphApi - functional programming interface
 * @export
 */
export const NetworkElementGraphApiFp = function (
  configuration?: Configuration
) {
  return {
    /**
     *
     * @param {AssestOverviewByMarketQueryObjectGrid} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    networkElementGraph(
      body: AssestOverviewByMarketQueryObjectGrid,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = NetworkElementGraphApiFetchParamCreator(
        configuration
      ).networkElementGraph(body, options);
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
     * @param {AssestOverviewByMarketQueryObjectGrid} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    AssetOverviewByMarketExport(
      body: AssestOverviewByMarketQueryObjectGrid,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ReturnFile> {
      const localVarFetchArgs = NetworkElementGraphApiFetchParamCreator(
        configuration
      ).AssetOverviewByMarketExport(body, options);
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

    networkElementGraphGetAllOpcos(
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs =
        NetworkElementGraphApiFetchParamCreator(
          configuration
        ).networkElementGraphGetAllOpcos(options);
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
 * NetworkElementGraphApi - factory interface
 * @export
 */
export const NetworkElementGraphApiFactory = function (
  configuration?: Configuration,
  fetch?: FetchAPI,
  basePath?: string
) {
  return {
    /**
     *
     * @param {AssestOverviewByMarketQueryObjectGrid} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    networkElementGraph(
      body: AssestOverviewByMarketQueryObjectGrid,
      options?: any
    ) {
      return NetworkElementGraphApiFp(configuration).networkElementGraph(
        body,
        options
      )(fetch, basePath);
    },

    /**
     *
     * @param {AssestOverviewByMarketQueryObjectGrid} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    AssetOverviewByMarketExport(
      body: AssestOverviewByMarketQueryObjectGrid,
      options?: any
    ) {
      return NetworkElementGraphApiFp(
        configuration
      ).AssetOverviewByMarketExport(body, options)(fetch, basePath);
    },

    networkElementGraphGetAllOpcos(options?: any) {
      return NetworkElementGraphApiFp(
        configuration
      ).networkElementGraphGetAllOpcos(options)(fetch, basePath);
    },
  };
};

/**
 * NetworkElementGraphApi - object-oriented interface
 * @export
 * @class NetworkElementGraphApi
 * @extends {BaseAPI}
 */
export class NetworkElementGraphApi extends BaseAPI {
  /**
   *
   * @param {AssestOverviewByMarketQueryObjectGrid} body
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof NetworkElementGraphApi
   */

  public networkElementGraph(
    body: AssestOverviewByMarketQueryObjectGrid,
    options?: any
  ) {
    return NetworkElementGraphApiFp(this.configuration).networkElementGraph(
      body,
      options
    )(this.fetch, this.basePath);
  }

  /**
   *
   * @param {AssestOverviewByMarketQueryObjectGrid} body
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof NetworkElementGraphApi
   */
  public AssetOverviewByMarketExport(
    body: AssestOverviewByMarketQueryObjectGrid,
    options?: any
  ) {
    return NetworkElementGraphApiFp(
      this.configuration
    ).AssetOverviewByMarketExport(body, options)(this.fetch, this.basePath);
  }

  public networkElementGraphGetAllOpcos(options?: any) {
    return NetworkElementGraphApiFp(
      this.configuration
    ).networkElementGraphGetAllOpcos(options)(this.fetch, this.basePath);
  }
}
