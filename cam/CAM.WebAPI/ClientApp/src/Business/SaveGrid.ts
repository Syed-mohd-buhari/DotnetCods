import { BaseAPI, RequiredError } from "./Common/CommonBusiness";
import * as url from "url";
import * as portableFetch from "portable-fetch";
import { Configuration } from "./Common/configuration";
import { BASE_PATH, FetchArgs, FetchAPI } from "./Common/CommonBusiness";
import { ResultDto, SaveGrid } from "../Model/CommonModels";
import { headerObj } from "./header";

/**
 * GridApi - fetch parameter creator
 * @export
 */
export const GridApiFetchParamCreator = function (
  configuration?: Configuration
) {
  return {
    /**
     *
     * @param {string} [className]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    gridDelete(className?: string, options: any = {}): FetchArgs {
      const localVarPath = `/api/Grid`;
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

      if (className !== undefined) {
        localVarQueryParameter["className"] = className;
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
     * @param {SaveGrid} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    gridSave(body: SaveGrid, options: any = {}): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling gridSave."
        );
      }
      const localVarPath = `/api/Grid`;
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
  };
};

/**
 * GridApi - functional programming interface
 * @export
 */
export const GridApiFp = function (configuration?: Configuration) {
  return {
    /**
     *
     * @param {string} [className]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    gridDelete(
      className?: string,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = GridApiFetchParamCreator(
        configuration
      ).gridDelete(className, options);
      return (fetch: FetchAPI = portableFetch, basePath = BASE_PATH) => {
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
     * @param {SaveGrid} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    gridSave(
      body: SaveGrid,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = GridApiFetchParamCreator(
        configuration
      ).gridSave(body, options);
      return (fetch: FetchAPI = portableFetch, basePath = BASE_PATH) => {
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
 * GridApi - factory interface
 * @export
 */
export const GridApiFactory = function (
  configuration?: Configuration,
  fetch?: FetchAPI,
  basePath?: string
) {
  return {
    /**
     *
     * @param {string} [className]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    gridDelete(className?: string, options?: any) {
      return GridApiFp(configuration).gridDelete(className, options)(
        fetch,
        basePath
      );
    },
    /**
     *
     * @param {SaveGrid} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    gridSave(body: SaveGrid, options?: any) {
      return GridApiFp(configuration).gridSave(body, options)(fetch, basePath);
    },
  };
};

/**
 * GridApi - object-oriented interface
 * @export
 * @class GridApi
 * @extends {BaseAPI}
 */
export class GridApi extends BaseAPI {
  /**
   *
   * @param {string} [className]
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof GridApi
   */
  public gridDelete(className?: string, options?: any) {
    return GridApiFp(this.configuration).gridDelete(className, options)(
      this.fetch,
      this.basePath
    );
  }

  /**
   *
   * @param {SaveGrid} body
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof GridApi
   */
  public gridSave(body: SaveGrid, options?: any) {
    return GridApiFp(this.configuration).gridSave(body, options)(
      this.fetch,
      this.basePath
    );
  }
}
