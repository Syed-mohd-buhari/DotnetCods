import { BaseAPI, RequiredError } from "./Common/CommonBusiness";
import * as url from "url";
import * as portableFetch from "portable-fetch";
import { Configuration } from "./Common/configuration";
import { BASE_PATH, FetchArgs, FetchAPI } from "./Common/CommonBusiness";
import { ResultDto, ExodusAtGlanceGraph } from "../Model/CommonModels";
import { headerObj } from "./header";
import * as isomorphicFetch from "isomorphic-fetch";

/**
 * GridApi - fetch parameter creator
 * @export
 */
export const ExodusAtGlanceApiFetchParamCreator = function (
  configuration?: Configuration
) {
  return {
    /**
     *
     * @param {ExodusAtGlanceGraph} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */

    ExodusAtGlanceGraphGet(
      body: ExodusAtGlanceGraph,
      isAssetLevel?: boolean,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling ExodusAtGlanceGraph."
        );
      }
      const localVarPath = isAssetLevel
        ? `/api/ExodusGraphicalReport/Get`
        : `/api/ExodusGraphicalAssetLevelReport/Get`;
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
    ExodusAtGlanceGraphGetResource(
      isPageLoad?: boolean,
      filterObj: any = {},
      isAssetLevel?: boolean,
      options: any = {}
    ): FetchArgs {
      const localVarPath = isAssetLevel
        ? `/api/ExodusGraphicalReport/GetAllDropdownRecords`
        : `/api/ExodusGraphicalAssetLevelReport/GetAllDropdownRecords`;
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

      localVarQueryParameter["isPageLoad"] = isPageLoad;
      localVarHeaderParameter["Content-Type"] = "application/json";
      localVarRequestOptions.body = JSON.stringify(filterObj);

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

    GetOpcoWisePercentage(
      isPageLoad: boolean,
      body: ExodusAtGlanceGraph,
      isEOS?: boolean,
      isAssetLevel?: boolean,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling ExodusAtGlanceGraph."
        );
      }
      const localVarPath = isAssetLevel
        ? `/api/ExodusGraphicalReport/GetOpcoWisePercentage`
        : `/api/ExodusGraphicalAssetLevelReport/GetOpcoWisePercentage`;
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

      localVarQueryParameter["isPageLoad"] = isPageLoad;

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

    ExodusAtGlancePAGraph(
      body: ExodusAtGlanceGraph,
      isToggleOn?: boolean,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling ExodusAtGlanceGraph."
        );
      }
      const localVarPath = isToggleOn
        ? `/api/ExodusGraphicalReport/GetPlannedActivity`
        : `/api/ExodusGraphicalReport/GetPlannedActivityForAsset`;
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
 * ExodusAtGlanceGraphApi - functional programming interface
 * @export
 */
export const ExodusAtGlanceGraphApiFp = function (
  configuration?: Configuration
) {
  return {
    /**
     *
     * @param {ExodusAtGlanceGraph} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    ExodusAtGlanceGraphGet(
      body: ExodusAtGlanceGraph,
      isAssetLevel?: boolean,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = ExodusAtGlanceApiFetchParamCreator(
        configuration
      ).ExodusAtGlanceGraphGet(body, isAssetLevel, options);
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
    GetOpcoWisePercentage(
      isPageLoad: boolean,
      body: ExodusAtGlanceGraph,
      isEOS?: boolean,
      isAssetLevel?: boolean,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = ExodusAtGlanceApiFetchParamCreator(
        configuration
      ).GetOpcoWisePercentage(isPageLoad, body, isEOS, isAssetLevel, options);
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

    ExodusAtGlanceGraphGetResource(
      isPageLoad?: boolean,
      filterObj: any = {},
      isAssetLevel?: boolean,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = ExodusAtGlanceApiFetchParamCreator(
        configuration
      ).ExodusAtGlanceGraphGetResource(
        isPageLoad,
        filterObj,
        isAssetLevel,
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

    ExodusAtGlancePAGraph(
      body: ExodusAtGlanceGraph,
      isToggleOn?: boolean,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = ExodusAtGlanceApiFetchParamCreator(
        configuration
      ).ExodusAtGlancePAGraph(body, isToggleOn, options);
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
 * ExodusAtGlanceGraphApi - factory interface
 * @export
 */
export const ExodusAtGlanceGraphApiFactory = function (
  configuration?: Configuration,
  fetch?: FetchAPI,
  basePath?: string
) {
  return {
    /**
     *
     * @param {ExodusAtGlanceGraph} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    ExodusAtGlanceGraphGet(
      body: ExodusAtGlanceGraph,
      isAssetLevel?: boolean,
      options?: any
    ) {
      return ExodusAtGlanceGraphApiFp(configuration).ExodusAtGlanceGraphGet(
        body,
        isAssetLevel,
        options
      )(fetch, basePath);
    },

    ExodusAtGlanceGraphGetResource(
      isPageLoad?: boolean,
      filterObj: any = {},
      isAssetLevel?: boolean,
      options?: any
    ) {
      return ExodusAtGlanceGraphApiFp(
        configuration
      ).ExodusAtGlanceGraphGetResource(
        isPageLoad,
        filterObj,
        isAssetLevel,
        options
      )(fetch, basePath);
    },

    ExodusAtGlancePAGraph(
      body: ExodusAtGlanceGraph,
      isToggleOn?: boolean,
      options?: any
    ) {
      return ExodusAtGlanceGraphApiFp(configuration).ExodusAtGlancePAGraph(
        body,
        isToggleOn,
        options
      )(fetch, basePath);
    },
  };
};

/**
 * ExodusAtGlanceGraphApi - object-oriented interface
 * @export
 * @class ExodusAtGlanceGraphApi
 * @extends {BaseAPI}
 */
export class ExodusAtGlanceGraphApi extends BaseAPI {
  /**
   *
   * @param {ExodusAtGlanceGraph} body
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof ExodusAtGlanceGraphApi
   */
  public ExodusAtGlanceGraphGet(
    body: ExodusAtGlanceGraph,
    isAssetLevel?: boolean,
    options?: any
  ) {
    return ExodusAtGlanceGraphApiFp(this.configuration).ExodusAtGlanceGraphGet(
      body,
      isAssetLevel,
      options
    )(this.fetch, this.basePath);
  }

  public GetOpcoWisePercentage(
    isPageLoad: boolean,
    body: ExodusAtGlanceGraph,
    isEOS?: boolean,
    isAssetLevel?: boolean,
    options?: any
  ) {
    return ExodusAtGlanceGraphApiFp(this.configuration).GetOpcoWisePercentage(
      isPageLoad,
      body,
      isEOS,
      isAssetLevel,
      options
    )(this.fetch, this.basePath);
  }

  public ExodusAtGlanceGraphGetResource(
    isPageLoad?: boolean,
    filterObj: any = {},
    isAssetLevel?: boolean,
    options?: any
  ) {
    return ExodusAtGlanceGraphApiFp(
      this.configuration
    ).ExodusAtGlanceGraphGetResource(
      isPageLoad,
      filterObj,
      isAssetLevel,
      options
    )(this.fetch, this.basePath);
  }

  public ExodusAtGlancePAGraph(
    body: ExodusAtGlanceGraph,
    isToggleOn?: boolean,
    options?: any
  ) {
    return ExodusAtGlanceGraphApiFp(this.configuration).ExodusAtGlancePAGraph(
      body,
      isToggleOn,
      options
    )(this.fetch, this.basePath);
  }
}
