import { BaseAPI, RequiredError } from "./Common/CommonBusiness";
import * as url from "url";
import * as portableFetch from "portable-fetch";
import { Configuration } from "./Common/configuration";
import { BASE_PATH, FetchArgs, FetchAPI } from "./Common/CommonBusiness";
import { ResultDto, NetworkVisualizerGraph } from "../Model/CommonModels";
import { headerObj } from "./header";
import * as isomorphicFetch from "isomorphic-fetch";

/**
 * GridApi - fetch parameter creator
 * @export
 */
export const NetworkVisualizerApiFetchParamCreator = function (
  configuration?: Configuration
) {
  return {
    /**
     *
     * @param {NetworkVisualizerGraph} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */

    networkElementGraph(
      body: NetworkVisualizerGraph,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling networkVisualizerGraph."
        );
      }
      const localVarPath = `/api/NetworkVisualizer/Get`;
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

    networkVisualizerGraphGetResource(options: any = {}): FetchArgs {
      const localVarPath = `/api/NetworkVisualizer/GetResource`;
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
 * NetworkVisualizerGraphApi - functional programming interface
 * @export
 */
export const NetworkVisualizerGraphApiFp = function (
  configuration?: Configuration
) {
  return {
    /**
     *
     * @param {NetworkVisualizerGraph} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    networkElementGraph(
      body: NetworkVisualizerGraph,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = NetworkVisualizerApiFetchParamCreator(
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

    networkVisualizerGraphGetResource(
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs =
        NetworkVisualizerApiFetchParamCreator(
          configuration
        ).networkVisualizerGraphGetResource(options);
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
 * NetworkVisualizerGraphApi - factory interface
 * @export
 */
export const NetworkVisualizerGraphApiFactory = function (
  configuration?: Configuration,
  fetch?: FetchAPI,
  basePath?: string
) {
  return {
    /**
     *
     * @param {NetworkVisualizerGraph} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    networkElementGraph(body: NetworkVisualizerGraph, options?: any) {
      return NetworkVisualizerGraphApiFp(configuration).networkElementGraph(
        body,
        options
      )(fetch, basePath);
    },

    networkVisualizerGraphGetResource(options?: any) {
      return NetworkVisualizerGraphApiFp(
        configuration
      ).networkVisualizerGraphGetResource(options)(fetch, basePath);
    },
  };
};

/**
 * NetworkVisualizerGraphApi - object-oriented interface
 * @export
 * @class NetworkVisualizerGraphApi
 * @extends {BaseAPI}
 */
export class NetworkVisualizerGraphApi extends BaseAPI {
  /**
   *
   * @param {NetworkVisualizerGraph} body
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof NetworkVisualizerGraphApi
   */

  public networkElementGraph(body: NetworkVisualizerGraph, options?: any) {
    return NetworkVisualizerGraphApiFp(this.configuration).networkElementGraph(
      body,
      options
    )(this.fetch, this.basePath);
  }

  public networkVisualizerGraphGetResource(options?: any) {
    return NetworkVisualizerGraphApiFp(
      this.configuration
    ).networkVisualizerGraphGetResource(options)(this.fetch, this.basePath);
  }
}
