import * as url from "url";
import * as isomorphicFetch from "isomorphic-fetch";
import { Configuration } from "./Common/configuration";
import { headerObj } from "./header";

import {
  BASE_PATH,
  FetchArgs,
  FetchAPI,
  BaseAPI,
  RequiredError,
} from "./Common/CommonBusiness";
import {
  InfraClusterDtoCreate,
  InfraClusterDtoUpdate,
  InfraClusterQueryDto,
} from "../Model/InfraCluster";
import { ResultDto } from "../Model/CommonModels";

/**
 * InfraClusterApi - fetch parameter creator
 */
export const InfraClusterApiFetchParamCreator = function (
  configuration?: Configuration
) {
  return {
    GetInfraClusterPaLevel(
      body: InfraClusterDtoUpdate,
      options: any = {}
    ): FetchArgs {
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling GetInfraClusterPaLevel."
        );
      }
      const localVarPath = `/api/ClusterLevelPA/GetInfraClusterPaLevel`;
      const localVarUrlObj = url.parse(localVarPath, true);
      const localVarRequestOptions = Object.assign({ method: "POST" }, options);
      const localVarHeaderParameter = { ...headerObj } as any;
      const localVarQueryParameter = {} as any;

      if (configuration && configuration.apiKey) {
        const apiKeyValue =
          typeof configuration.apiKey === "function"
            ? configuration.apiKey("Authorization")
            : configuration.apiKey;
        localVarHeaderParameter["Authorization"] = apiKeyValue;
      }

      localVarHeaderParameter["Content-Type"] = "application/json";

      localVarUrlObj.query = Object.assign(
        {},
        localVarUrlObj.query,
        localVarQueryParameter,
        options.query
      );
      localVarUrlObj.search = null;

      localVarRequestOptions.headers = Object.assign(
        {},
        localVarHeaderParameter,
        options.headers
      );

      const needsSerialization =
        <any>"InfraClusterDtoUpdate" !== "string" ||
        localVarRequestOptions.headers["Content-Type"] === "application/json";

      localVarRequestOptions.body = needsSerialization
        ? JSON.stringify(body || {})
        : body || "";

      return {
        url: url.format(localVarUrlObj),
        options: localVarRequestOptions,
      };
    },

    GetInfraHardwareClusterPaLevel(
      body: InfraClusterDtoUpdate,
      options: any = {}
    ): FetchArgs {
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling GetInfraHardwareClusterPaLevel."
        );
      }
      const localVarPath = `/api/ClusterLevelPA/GetSitelevelClusters`;
      const localVarUrlObj = url.parse(localVarPath, true);
      const localVarRequestOptions = Object.assign({ method: "POST" }, options);
      const localVarHeaderParameter = { ...headerObj } as any;
      const localVarQueryParameter = {} as any;

      if (configuration && configuration.apiKey) {
        const apiKeyValue =
          typeof configuration.apiKey === "function"
            ? configuration.apiKey("Authorization")
            : configuration.apiKey;
        localVarHeaderParameter["Authorization"] = apiKeyValue;
      }

      localVarHeaderParameter["Content-Type"] = "application/json";

      localVarUrlObj.query = Object.assign(
        {},
        localVarUrlObj.query,
        localVarQueryParameter,
        options.query
      );
      localVarUrlObj.search = null;

      localVarRequestOptions.headers = Object.assign(
        {},
        localVarHeaderParameter,
        options.headers
      );

      const needsSerialization =
        <any>"InfraClusterDtoUpdate" !== "string" ||
        localVarRequestOptions.headers["Content-Type"] === "application/json";

      localVarRequestOptions.body = needsSerialization
        ? JSON.stringify(body || {})
        : body || "";

      return {
        url: url.format(localVarUrlObj),
        options: localVarRequestOptions,
      };
    },
    GetInfraProgramClusterPaLevel(
      body: InfraClusterDtoUpdate,
      options: any = {}
    ): FetchArgs {
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling GetInfraProgramClusterPaLevel."
        );
      }
      const localVarPath = `/api/ClusterLevelPA/GetInfraClusterListBasedonOpco`;
      const localVarUrlObj = url.parse(localVarPath, true);
      const localVarRequestOptions = Object.assign({ method: "POST" }, options);
      const localVarHeaderParameter = { ...headerObj } as any;
      const localVarQueryParameter = {} as any;

      if (configuration && configuration.apiKey) {
        const apiKeyValue =
          typeof configuration.apiKey === "function"
            ? configuration.apiKey("Authorization")
            : configuration.apiKey;
        localVarHeaderParameter["Authorization"] = apiKeyValue;
      }

      localVarHeaderParameter["Content-Type"] = "application/json";

      localVarUrlObj.query = Object.assign(
        {},
        localVarUrlObj.query,
        localVarQueryParameter,
        options.query
      );
      localVarUrlObj.search = null;

      localVarRequestOptions.headers = Object.assign(
        {},
        localVarHeaderParameter,
        options.headers
      );

      const needsSerialization =
        <any>"InfraClusterDtoUpdate" !== "string" ||
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
     * @param {boolean} [forced]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    InfraClusterCreateResource(
      body: any,
      forced?: boolean,
      options: any = {}
    ): FetchArgs {
      const { opCoId, DcId } = body;
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling InfraClusterCreateResource."
        );
      }
      const localVarPath =
        `/api/ClusterLevelPA/CreateInfraCluster/{opCoId}/{DcId}`
          .replace(`{${"opCoId"}}`, encodeURIComponent(String(opCoId)))
          .replace(`{${"DcId"}}`, encodeURIComponent(String(DcId)));
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
      // console.log("body", body);
      // if (body?.opCoId !== undefined) {
      //   localVarQueryParameter["opCoId"] = body?.opCoId;
      // }
      // if (body?.DcId !== undefined) {
      //   localVarQueryParameter["DcId "] = body?.DcId;
      // }

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
    InfraClusterHardwareCreateResource(
      body: any,
      forced?: boolean,
      options: any = {}
    ): FetchArgs {
      const { opCoId, DcId } = body;
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling InfraClusterHardwareCreateResource."
        );
      }
      const localVarPath =
        `/api/ClusterLevelPA/GetPlannedHardwareTypes/{opCoId}/{DcId}`
          .replace(`{${"opCoId"}}`, encodeURIComponent(String(opCoId)))
          .replace(`{${"DcId"}}`, encodeURIComponent(String(DcId)));
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
      // console.log("body", body);
      // if (body?.opCoId !== undefined) {
      //   localVarQueryParameter["opCoId"] = body?.opCoId;
      // }
      // if (body?.DcId !== undefined) {
      //   localVarQueryParameter["DcId "] = body?.DcId;
      // }

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
    InfraProgramClusterCreateResource(
      body: any,
      forced?: boolean,
      options: any = {}
    ): FetchArgs {
      const { opCoId, DcId } = body;
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling InfraProgramClusterCreateResource."
        );
      }
      const localVarPath =
        `/api/NetworkElementClusterAsPlanned/CreateNetworkElementCluster`
          .replace(`{${"opCoId"}}`, encodeURIComponent(String(opCoId)))
          .replace(`{${"DcId"}}`, encodeURIComponent(String(DcId)));
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
      // console.log("body", body);
      // if (body?.opCoId !== undefined) {
      //   localVarQueryParameter["opCoId"] = body?.opCoId;
      // }
      // if (body?.DcId !== undefined) {
      //   localVarQueryParameter["DcId "] = body?.DcId;
      // }

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
 * InfraClusterApi - functional interface
 */
export const InfraClusterApiFp = function (configuration?: Configuration) {
  return {
    GetInfraClusterPaLevel(body: InfraClusterDtoUpdate, options?: any) {
      const fetchArgs = InfraClusterApiFetchParamCreator(
        configuration
      ).GetInfraClusterPaLevel(body, options);

      return (fetch: FetchAPI = isomorphicFetch, basePath = BASE_PATH) => {
        return fetch(basePath + fetchArgs.url, fetchArgs.options).then(
          async (response) => {
            if (response.status >= 200 && response.status < 300) {
              return response.json();
            } else {
              throw response;
            }
          }
        );
      };
    },
    GetInfraHardwareClusterPaLevel(body: InfraClusterDtoUpdate, options?: any) {
      const fetchArgs = InfraClusterApiFetchParamCreator(
        configuration
      ).GetInfraHardwareClusterPaLevel(body, options);

      return (fetch: FetchAPI = isomorphicFetch, basePath = BASE_PATH) => {
        return fetch(basePath + fetchArgs.url, fetchArgs.options).then(
          async (response) => {
            if (response.status >= 200 && response.status < 300) {
              return response.json();
            } else {
              throw response;
            }
          }
        );
      };
    },
    GetInfraProgramClusterPaLevel(body: InfraClusterDtoUpdate, options?: any) {
      const fetchArgs = InfraClusterApiFetchParamCreator(
        configuration
      ).GetInfraProgramClusterPaLevel(body, options);

      return (fetch: FetchAPI = isomorphicFetch, basePath = BASE_PATH) => {
        return fetch(basePath + fetchArgs.url, fetchArgs.options).then(
          async (response) => {
            if (response.status >= 200 && response.status < 300) {
              return response.json();
            } else {
              throw response;
            }
          }
        );
      };
    },
    /**
     *
     * @param {any} body
     * @param {boolean} [forced]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    InfraClusterCreateResource(
      body: any,
      forced?: boolean,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = InfraClusterApiFetchParamCreator(
        configuration
      ).InfraClusterCreateResource(body, forced, options);
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
    InfraClusterHardwareCreateResource(
      body: any,
      forced?: boolean,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = InfraClusterApiFetchParamCreator(
        configuration
      ).InfraClusterHardwareCreateResource(body, forced, options);
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
    InfraProgramClusterCreateResource(
      body: any,
      forced?: boolean,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = InfraClusterApiFetchParamCreator(
        configuration
      ).InfraProgramClusterCreateResource(body, forced, options);
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
 * InfraCluster - factory interface
 * @export
 */
export const InfraClusterFactory = function (
  configuration?: Configuration,
  fetch?: FetchAPI,
  basePath?: string
) {
  return {
    /**
     *
     * @param {InfraClusterDtoUpdate} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    GetInfraClusterPaLevel(body: InfraClusterDtoUpdate, options?: any) {
      return InfraClusterApiFp(configuration).GetInfraClusterPaLevel(
        body,
        options
      )(fetch, basePath);
    },

    GetInfraHardwareClusterPaLevel(body: InfraClusterDtoUpdate, options?: any) {
      return InfraClusterApiFp(configuration).GetInfraHardwareClusterPaLevel(
        body,
        options
      )(fetch, basePath);
    },
    GetInfraProgramClusterPaLevel(body: InfraClusterDtoUpdate, options?: any) {
      return InfraClusterApiFp(configuration).GetInfraProgramClusterPaLevel(
        body,
        options
      )(fetch, basePath);
    },

    /**
     *
     * @param {any} body
     * @param {boolean} [forced]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    InfraClusterCreateResource(body: any, forced?: boolean, options?: any) {
      return InfraClusterApiFp(configuration).InfraClusterCreateResource(
        body,
        forced,
        options
      )(fetch, basePath);
    },
    InfraClusterHardwareCreateResource(
      body: any,
      forced?: boolean,
      options?: any
    ) {
      return InfraClusterApiFp(
        configuration
      ).InfraClusterHardwareCreateResource(
        body,
        forced,
        options
      )(fetch, basePath);
    },
    InfraProgramClusterCreateResource(
      body: any,
      forced?: boolean,
      options?: any
    ) {
      return InfraClusterApiFp(configuration).InfraProgramClusterCreateResource(
        body,
        forced,
        options
      )(fetch, basePath);
    },
  };
};

/**
 * InfraClusterApi - object-oriented class
 */
export class InfraClusterApi extends BaseAPI {
  public GetInfraClusterPaLevel(body: InfraClusterDtoUpdate, options?: any) {
    return InfraClusterApiFp(this.configuration).GetInfraClusterPaLevel(
      body,
      options
    )(this.fetch, this.basePath);
  }

  public GetInfraHardwareClusterPaLevel(
    body: InfraClusterDtoUpdate,
    options?: any
  ) {
    return InfraClusterApiFp(this.configuration).GetInfraHardwareClusterPaLevel(
      body,
      options
    )(this.fetch, this.basePath);
  }
  public GetInfraProgramClusterPaLevel(
    body: InfraClusterDtoUpdate,
    options?: any
  ) {
    return InfraClusterApiFp(this.configuration).GetInfraProgramClusterPaLevel(
      body,
      options
    )(this.fetch, this.basePath);
  }

  /**
   *
   * @param {any} body
   * @param {boolean} [forced]
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof InfraCluster
   */
  public InfraClusterCreateResource(
    body: any,
    forced?: boolean,
    options?: any
  ) {
    return InfraClusterApiFp(this.configuration).InfraClusterCreateResource(
      body,
      forced,
      options
    )(this.fetch, this.basePath);
  }
  public InfraClusterHardwareCreateResource(
    body: any,
    forced?: boolean,
    options?: any
  ) {
    return InfraClusterApiFp(
      this.configuration
    ).InfraClusterHardwareCreateResource(
      body,
      forced,
      options
    )(this.fetch, this.basePath);
  }
  public InfraProgramClusterCreateResource(
    body: any,
    forced?: boolean,
    options?: any
  ) {
    return InfraClusterApiFp(
      this.configuration
    ).InfraProgramClusterCreateResource(
      body,
      forced,
      options
    )(this.fetch, this.basePath);
  }
}
