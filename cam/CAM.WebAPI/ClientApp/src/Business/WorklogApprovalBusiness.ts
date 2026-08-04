import * as isomorphicFetch from "isomorphic-fetch";
import * as url from "url";
import { ResultDto } from "../Model/CommonModels";
import {
  QueryResultDtoOfVolteKPIWorklogDto,
  VolteKPIWorklogDto,
} from "../Model/VolteKpi/WorklogApproval";
import {
  BaseAPI,
  BASE_PATH,
  FetchAPI,
  FetchArgs,
  FilterValueDto,
  RequiredError,
} from "./Common/CommonBusiness";
import { headerObj } from "./header";

import { Configuration } from "./Common/configuration";
import { VolteKPIQueryObjectGrid as VolteKPIWorklogQueryDto } from "../Model/VolteKpi/VolteKPI";

/**
 * VolteKPIWorklogApi - fetch parameter creator
 * @export
 */
export const VolteKPIWorklogApiFetchParamCreator = function (
  configuration?: Configuration
) {
  return {
    /**
     *
     * @param {VolteKPIWorklogQueryDto} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    volteKPIWorklogGet(
      body: VolteKPIWorklogQueryDto,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling volteKPIWorklogGet."
        );
      }
      const localVarPath = `/api/VolteKPIWorklog/Get`;
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
        <any>"VolteKPIWorklogQueryDto" !== "string" ||
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
     * @param {number} [volteKPIId]
     * @param {number} [volteKPIType]
     * @param {number} [volteKPIWorklogId]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    volteKPIWorklogGetDuplicates(
      volteKPIId?: number,
      volteKPIType?: number,
      volteKPIWorklogId?: number,
      options: any = {}
    ): FetchArgs {
      const localVarPath = `/api/VolteKPIWorklog/GetDuplicates`;
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

      if (volteKPIId !== undefined) {
        localVarQueryParameter["VolteKPIId"] = volteKPIId;
      }

      if (volteKPIType !== undefined) {
        localVarQueryParameter["VolteKPIType"] = volteKPIType;
      }

      if (volteKPIWorklogId !== undefined) {
        localVarQueryParameter["VolteKPIWorklogId"] = volteKPIWorklogId;
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
     * @param {VolteKPIWorklogQueryDto} body
     * @param {string} [propertyName]
     * @param {string} [propertyFilter]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    volteKPIWorklogGetFilterResult(
      body: VolteKPIWorklogQueryDto,
      propertyName?: string,
      propertyFilter?: string,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling volteKPIWorklogGetFilterResult."
        );
      }
      const localVarPath = `/api/VolteKPIWorklog/Filter`;
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
        <any>"VolteKPIWorklogQueryDto" !== "string" ||
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
     * @param {VolteKPIWorklogDto} body
     * @param {boolean} [forced]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    volteKPIWorklogPut(
      body: VolteKPIWorklogDto,
      forced?: boolean,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling volteKPIWorklogPut."
        );
      }
      const localVarPath = `/api/VolteKPIWorklog`;
      const localVarUrlObj = url.parse(localVarPath, true);
      const localVarRequestOptions = Object.assign({ method: "PUT" }, options);
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

      if (forced !== undefined) {
        localVarQueryParameter["forced"] = forced;
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
        <any>"VolteKPIWorklogDto" !== "string" ||
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
 * VolteKPIWorklogApi - functional programming interface
 * @export
 */
export const VolteKPIWorklogApiFp = function (configuration?: Configuration) {
  return {
    /**
     *
     * @param {VolteKPIWorklogQueryDto} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    volteKPIWorklogGet(
      body: VolteKPIWorklogQueryDto,
      options?: any
    ): (
      fetch?: FetchAPI,
      basePath?: string
    ) => Promise<QueryResultDtoOfVolteKPIWorklogDto> {
      const localVarFetchArgs = VolteKPIWorklogApiFetchParamCreator(
        configuration
      ).volteKPIWorklogGet(body, options);
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
     * @param {number} [volteKPIId]
     * @param {number} [volteKPIType]
     * @param {number} [volteKPIWorklogId]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    volteKPIWorklogGetDuplicates(
      volteKPIId?: number,
      volteKPIType?: number,
      volteKPIWorklogId?: number,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = VolteKPIWorklogApiFetchParamCreator(
        configuration
      ).volteKPIWorklogGetDuplicates(
        volteKPIId,
        volteKPIType,
        volteKPIWorklogId,
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
     * @param {VolteKPIWorklogQueryDto} body
     * @param {string} [propertyName]
     * @param {string} [propertyFilter]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    volteKPIWorklogGetFilterResult(
      body: VolteKPIWorklogQueryDto,
      propertyName?: string,
      propertyFilter?: string,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<Array<FilterValueDto>> {
      const localVarFetchArgs = VolteKPIWorklogApiFetchParamCreator(
        configuration
      ).volteKPIWorklogGetFilterResult(
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
     * @param {VolteKPIWorklogDto} body
     * @param {boolean} [forced]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    volteKPIWorklogPut(
      body: VolteKPIWorklogDto,
      forced?: boolean,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = VolteKPIWorklogApiFetchParamCreator(
        configuration
      ).volteKPIWorklogPut(body, forced, options);
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
 * VolteKPIWorklogApi - factory interface
 * @export
 */
export const VolteKPIWorklogApiFactory = function (
  configuration?: Configuration,
  fetch?: FetchAPI,
  basePath?: string
) {
  return {
    /**
     *
     * @param {VolteKPIWorklogQueryDto} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    volteKPIWorklogGet(body: VolteKPIWorklogQueryDto, options?: any) {
      return VolteKPIWorklogApiFp(configuration).volteKPIWorklogGet(
        body,
        options
      )(fetch, basePath);
    },
    /**
     *
     * @param {number} [volteKPIId]
     * @param {number} [volteKPIType]
     * @param {number} [volteKPIWorklogId]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    volteKPIWorklogGetDuplicates(
      volteKPIId?: number,
      volteKPIType?: number,
      volteKPIWorklogId?: number,
      options?: any
    ) {
      return VolteKPIWorklogApiFp(configuration).volteKPIWorklogGetDuplicates(
        volteKPIId,
        volteKPIType,
        volteKPIWorklogId,
        options
      )(fetch, basePath);
    },
    /**
     *
     * @param {VolteKPIWorklogQueryDto} body
     * @param {string} [propertyName]
     * @param {string} [propertyFilter]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    volteKPIWorklogGetFilterResult(
      body: VolteKPIWorklogQueryDto,
      propertyName?: string,
      propertyFilter?: string,
      options?: any
    ) {
      return VolteKPIWorklogApiFp(configuration).volteKPIWorklogGetFilterResult(
        body,
        propertyName,
        propertyFilter,
        options
      )(fetch, basePath);
    },
    /**
     *
     * @param {VolteKPIWorklogDto} body
     * @param {boolean} [forced]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    volteKPIWorklogPut(
      body: VolteKPIWorklogDto,
      forced?: boolean,
      options?: any
    ) {
      return VolteKPIWorklogApiFp(configuration).volteKPIWorklogPut(
        body,
        forced,
        options
      )(fetch, basePath);
    },
  };
};

/**
 * VolteKPIWorklogApi - object-oriented interface
 * @export
 * @class VolteKPIWorklogApi
 * @extends {BaseAPI}
 */
export class VolteKPIWorklogApi extends BaseAPI {
  /**
   *
   * @param {VolteKPIWorklogQueryDto} body
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof VolteKPIWorklogApi
   */
  public volteKPIWorklogGet(body: VolteKPIWorklogQueryDto, options?: any) {
    return VolteKPIWorklogApiFp(this.configuration).volteKPIWorklogGet(
      body,
      options
    )(this.fetch, this.basePath);
  }

  /**
   *
   * @param {number} [volteKPIId]
   * @param {number} [volteKPIType]
   * @param {number} [volteKPIWorklogId]
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof VolteKPIWorklogApi
   */
  public volteKPIWorklogGetDuplicates(
    volteKPIId?: number,
    volteKPIType?: number,
    volteKPIWorklogId?: number,
    options?: any
  ) {
    return VolteKPIWorklogApiFp(
      this.configuration
    ).volteKPIWorklogGetDuplicates(
      volteKPIId,
      volteKPIType,
      volteKPIWorklogId,
      options
    )(this.fetch, this.basePath);
  }

  /**
   *
   * @param {VolteKPIWorklogQueryDto} body
   * @param {string} [propertyName]
   * @param {string} [propertyFilter]
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof VolteKPIWorklogApi
   */
  public volteKPIWorklogGetFilterResult(
    body: VolteKPIWorklogQueryDto,
    propertyName?: string,
    propertyFilter?: string,
    options?: any
  ) {
    return VolteKPIWorklogApiFp(
      this.configuration
    ).volteKPIWorklogGetFilterResult(
      body,
      propertyName,
      propertyFilter,
      options
    )(this.fetch, this.basePath);
  }

  /**
   *
   * @param {VolteKPIWorklogDto} body
   * @param {boolean} [forced]
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof VolteKPIWorklogApi
   */
  public volteKPIWorklogPut(
    body: VolteKPIWorklogDto,
    forced?: boolean,
    options?: any
  ) {
    return VolteKPIWorklogApiFp(this.configuration).volteKPIWorklogPut(
      body,
      forced,
      options
    )(this.fetch, this.basePath);
  }
}
