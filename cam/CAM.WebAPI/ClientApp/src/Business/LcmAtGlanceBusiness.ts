import { BaseAPI, RequiredError } from "./Common/CommonBusiness";
import * as url from "url";
import * as portableFetch from "portable-fetch";
import { Configuration } from "./Common/configuration";
import { BASE_PATH, FetchArgs, FetchAPI } from "./Common/CommonBusiness";
import { ResultDto, LCMAtGlanceGraph } from "../Model/CommonModels";
import { headerObj } from "./header";
import * as isomorphicFetch from "isomorphic-fetch";

/**
 * GridApi - fetch parameter creator
 * @export
 */
export const LCMAtGlanceApiFetchParamCreator = function (
  configuration?: Configuration
) {
  return {
    /**
     *
     * @param {LCMAtGlanceGraph} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */

    LCMAtGlanceGraphGet(body: LCMAtGlanceGraph, options: any = {}): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling LCMAtGlanceGraph."
        );
      }
      const localVarPath = `/api/LcmAtGlance/Get`;
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

    LCMAtGlanceGraphGetResource(
      isPageLoad?: boolean,
      options: any = {}
    ): FetchArgs {
      const localVarPath = `/api/LcmAtGlance/GetAllDropdownRecords`;
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

    LCMAtGlancePAGraph(
      body: LCMAtGlanceGraph,
      isEOS: boolean,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling LCMAtGlanceGraph."
        );
      }
      const localVarPath = `/api/LcmAtGlance/GetPlannedActivity${
        isEOS ? "ForEofs" : ""
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

    GetOpcoWisePercentage(
      isPageLoad: boolean,
      body: LCMAtGlanceGraph,
      isEOS: boolean,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling LCMAtGlanceGraph."
        );
      }
      const localVarPath = `/api/LcmAtGlance/GetOpcoWisePercentage${
        isEOS ? "ForEofs" : ""
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

    GetProductWisePercentage(
      isPageLoad: boolean,
      body: LCMAtGlanceGraph,
      isEOS: boolean,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling LCMAtGlanceGraph."
        );
      }
      const localVarPath = `/api/LcmAtGlance/GetProductWisePercentage${
        isEOS ? "ForEofs" : ""
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

    GetSupportedServiceWisePercentage(
      isPageLoad: boolean,
      body: LCMAtGlanceGraph,
      isEOS: boolean,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling LCMAtGlanceGraph."
        );
      }
      const localVarPath = `/api/LcmAtGlance/GetSupportedServiceWisePercentage${
        isEOS ? "ForEofs" : ""
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

    GetOverAllPercentageBasedOnFilters(
      isPageLoad: boolean,
      body: LCMAtGlanceGraph,
      isEOS: boolean,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling LCMAtGlanceGraph."
        );
      }
      const localVarPath = `/api/LcmAtGlance/GetOverAllPercentageBasedOnFilters${
        isEOS ? "ForEofs" : ""
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

    LCMAtGlanceGraphOverAll(options: any = {}): FetchArgs {
      const localVarPath = `/api/LcmAtGlance/GetOverAllOpcoAndAssetWisePercentageForEofs`;
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
 * LCMAtGlanceGraphApi - functional programming interface
 * @export
 */
export const LCMAtGlanceGraphApiFp = function (configuration?: Configuration) {
  return {
    /**
     *
     * @param {LCMAtGlanceGraph} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    LCMAtGlanceGraphGet(
      body: LCMAtGlanceGraph,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = LCMAtGlanceApiFetchParamCreator(
        configuration
      ).LCMAtGlanceGraphGet(body, options);
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
      body: LCMAtGlanceGraph,
      isEOS: boolean,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = LCMAtGlanceApiFetchParamCreator(
        configuration
      ).GetOpcoWisePercentage(isPageLoad, body, isEOS, options);
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

    GetProductWisePercentage(
      isPageLoad: boolean,
      body: LCMAtGlanceGraph,
      isEOS: boolean,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = LCMAtGlanceApiFetchParamCreator(
        configuration
      ).GetProductWisePercentage(isPageLoad, body, isEOS, options);
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

    GetSupportedServiceWisePercentage(
      isPageLoad: boolean,
      body: LCMAtGlanceGraph,
      isEOS: boolean,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = LCMAtGlanceApiFetchParamCreator(
        configuration
      ).GetSupportedServiceWisePercentage(isPageLoad, body, isEOS, options);
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

    GetOverAllPercentageBasedOnFilters(
      isPageLoad: boolean,
      body: LCMAtGlanceGraph,
      isEOS: boolean,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = LCMAtGlanceApiFetchParamCreator(
        configuration
      ).GetOverAllPercentageBasedOnFilters(isPageLoad, body, isEOS, options);
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

    LCMAtGlanceGraphGetResource(
      isPageLoad?: boolean,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = LCMAtGlanceApiFetchParamCreator(
        configuration
      ).LCMAtGlanceGraphGetResource(isPageLoad, options);
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

    LCMAtGlancePAGraph(
      body: LCMAtGlanceGraph,
      isEOS: boolean,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = LCMAtGlanceApiFetchParamCreator(
        configuration
      ).LCMAtGlancePAGraph(body, isEOS, options);
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

    LCMAtGlanceGraphOverAll(
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs =
        LCMAtGlanceApiFetchParamCreator(configuration).LCMAtGlanceGraphOverAll(
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
  };
};

/**
 * LCMAtGlanceGraphApi - factory interface
 * @export
 */
export const LCMAtGlanceGraphApiFactory = function (
  configuration?: Configuration,
  fetch?: FetchAPI,
  basePath?: string
) {
  return {
    /**
     *
     * @param {LCMAtGlanceGraph} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    LCMAtGlanceGraphGet(body: LCMAtGlanceGraph, options?: any) {
      return LCMAtGlanceGraphApiFp(configuration).LCMAtGlanceGraphGet(
        body,
        options
      )(fetch, basePath);
    },

    GetOpcoWisePercentage(
      isPageLoad: boolean,
      body: LCMAtGlanceGraph,
      isEOS: boolean,
      options?: any
    ) {
      return LCMAtGlanceGraphApiFp(configuration).GetOpcoWisePercentage(
        isPageLoad,
        body,
        isEOS,
        options
      )(fetch, basePath);
    },
    GetProductWisePercentage(
      isPageLoad: boolean,
      body: LCMAtGlanceGraph,
      isEOS: boolean,
      options?: any
    ) {
      return LCMAtGlanceGraphApiFp(configuration).GetProductWisePercentage(
        isPageLoad,
        body,
        isEOS,
        options
      )(fetch, basePath);
    },
    GetSupportedServiceWisePercentage(
      isPageLoad: boolean,
      body: LCMAtGlanceGraph,
      isEOS: boolean,
      options?: any
    ) {
      return LCMAtGlanceGraphApiFp(
        configuration
      ).GetSupportedServiceWisePercentage(
        isPageLoad,
        body,
        isEOS,
        options
      )(fetch, basePath);
    },
    GetOverAllPercentageBasedOnFilters(
      isPageLoad: boolean,
      body: LCMAtGlanceGraph,
      isEOS: boolean,
      options?: any
    ) {
      return LCMAtGlanceGraphApiFp(
        configuration
      ).GetOverAllPercentageBasedOnFilters(
        isPageLoad,
        body,
        isEOS,
        options
      )(fetch, basePath);
    },
    LCMAtGlancePAGraph(body: LCMAtGlanceGraph, isEOS: boolean, options?: any) {
      return LCMAtGlanceGraphApiFp(configuration).LCMAtGlancePAGraph(
        body,
        isEOS,
        options
      )(fetch, basePath);
    },

    LCMAtGlanceGraphGetResource(isPageLoad?: boolean, options?: any) {
      return LCMAtGlanceGraphApiFp(configuration).LCMAtGlanceGraphGetResource(
        isPageLoad,
        options
      )(fetch, basePath);
    },

    LCMAtGlanceGraphOverAll(options?: any) {
      return LCMAtGlanceGraphApiFp(configuration).LCMAtGlanceGraphOverAll(
        options
      )(fetch, basePath);
    },
  };
};

/**
 * LCMAtGlanceGraphApi - object-oriented interface
 * @export
 * @class LCMAtGlanceGraphApi
 * @extends {BaseAPI}
 */
export class LCMAtGlanceGraphApi extends BaseAPI {
  /**
   *
   * @param {LCMAtGlanceGraph} body
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof LCMAtGlanceGraphApi
   */

  public LCMAtGlanceGraphGet(body: LCMAtGlanceGraph, options?: any) {
    return LCMAtGlanceGraphApiFp(this.configuration).LCMAtGlanceGraphGet(
      body,
      options
    )(this.fetch, this.basePath);
  }

  public GetOpcoWisePercentage(
    isPageLoad: boolean,
    body: LCMAtGlanceGraph,
    isEOS: boolean,
    options?: any
  ) {
    return LCMAtGlanceGraphApiFp(this.configuration).GetOpcoWisePercentage(
      isPageLoad,
      body,
      isEOS,
      options
    )(this.fetch, this.basePath);
  }

  public GetProductWisePercentage(
    isPageLoad: boolean,
    body: LCMAtGlanceGraph,
    isEOS: boolean,
    options?: any
  ) {
    return LCMAtGlanceGraphApiFp(this.configuration).GetProductWisePercentage(
      isPageLoad,
      body,
      isEOS,
      options
    )(this.fetch, this.basePath);
  }

  public GetSupportedServiceWisePercentage(
    isPageLoad: boolean,
    body: LCMAtGlanceGraph,
    isEOS: boolean,
    options?: any
  ) {
    return LCMAtGlanceGraphApiFp(
      this.configuration
    ).GetSupportedServiceWisePercentage(
      isPageLoad,
      body,
      isEOS,
      options
    )(this.fetch, this.basePath);
  }

  public GetOverAllPercentageBasedOnFilters(
    isPageLoad: boolean,
    body: LCMAtGlanceGraph,
    isEOS: boolean,
    options?: any
  ) {
    return LCMAtGlanceGraphApiFp(
      this.configuration
    ).GetOverAllPercentageBasedOnFilters(
      isPageLoad,
      body,
      isEOS,
      options
    )(this.fetch, this.basePath);
  }

  public LCMAtGlancePAGraph(
    body: LCMAtGlanceGraph,
    isEOS: boolean,
    options?: any
  ) {
    return LCMAtGlanceGraphApiFp(this.configuration).LCMAtGlancePAGraph(
      body,
      isEOS,
      options
    )(this.fetch, this.basePath);
  }

  public LCMAtGlanceGraphGetResource(isPageLoad?: boolean, options?: any) {
    return LCMAtGlanceGraphApiFp(
      this.configuration
    ).LCMAtGlanceGraphGetResource(isPageLoad, options)(
      this.fetch,
      this.basePath
    );
  }

  public LCMAtGlanceGraphOverAll(options?: any) {
    return LCMAtGlanceGraphApiFp(this.configuration).LCMAtGlanceGraphOverAll(
      options
    )(this.fetch, this.basePath);
  }
}
