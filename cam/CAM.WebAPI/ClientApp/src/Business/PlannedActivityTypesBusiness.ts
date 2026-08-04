import * as url from "url";
import * as isomorphicFetch from "isomorphic-fetch";
import { Configuration } from "./Common/configuration";
import { ChangeGridOrderDto, ResultDto } from "../Model/CommonModels";
import {
  BASE_PATH,
  FetchArgs,
  FetchAPI,
  RequiredError,
  FilterValueDto,
} from "./Common/CommonBusiness";
import { BaseAPI } from "./Common/CommonBusiness";
import { headerObj } from "./header";

import {
  QueryResultDtoOfPlannedActivityTypesDtoGrid,
  PlannedActivityTypesDtoCreate,
  PlannedActivityTypesDtoUpdate,
  PlannedActivityTypesQueryObjectGrid,
} from "../Model/PlannedActivityTypes";
import { ReturnFile } from "../Model/Common";
/**
 * PlannedActivityTypesApi - fetch parameter creator
 * @export
 */
export const PlannedActivityTypesApiFetchParamCreator = function (
  configuration?: Configuration
) {
  return {
    /**
     *
     * @param {MajorHardwareBuildQueryDto} body
     * @param {string} [propertyName]
     * @param {string} [propertyFilter]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    PlannedActivityTypesGetFilterResult(
      body: PlannedActivityTypesQueryObjectGrid,
      propertyName?: string,
      propertyFilter?: string,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling PlannedActivityTypesGetFilterResult."
        );
      }
      const localVarPath = `/api/PlannedActivityTypes/Filter`;
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
        <any>"MajorHardwareBuildQueryDto" !== "string" ||
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
     * @param {PlannedActivityTypesDtoCreate} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    PlannedActivityTypesCreate(
      body: PlannedActivityTypesDtoCreate,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling PlannedActivityTypesCreate."
        );
      }
      const localVarPath = `/api/PlannedActivityTypes/CreatePlannedRule`;
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
        <any>"PlannedActivityTypesDtoCreate" !== "string" ||
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
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    PlannedActivityTypeEnableLinkedDC(body: any, options: any = {}): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling PlannedActivityTypesGetGrid."
        );
      }
      const localVarPath = `/api/PlannedActivityTypes/EnableDisableLinkedDcRule`;
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
        <any>"PlannedActivityTypesGetGrid" !== "string" ||
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
     * @param {number} [id]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    PlannedActivityTypesDelete(id?: number, options: any = {}): FetchArgs {
      const localVarPath = `/api/PlannedActivityTypes/DeleteRule`;
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

      if (id !== undefined) {
        localVarQueryParameter["id"] = id;
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
     * @param {PlannedActivityTypesQueryObjectGrid} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    PlannedActivityTypesGetGrid(
      body: PlannedActivityTypesQueryObjectGrid,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling PlannedActivityTypesGetGrid."
        );
      }
      const localVarPath = `/api/PlannedActivityTypes/GetPlannedActivity`;
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
        <any>"PlannedActivityTypesGetGrid" !== "string" ||
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
     * @param {PlannedActivityTypesDtoUpdate} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    PlannedActivityTypesUpdate(
      body: PlannedActivityTypesDtoUpdate,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling PlannedActivityTypesUpdate."
        );
      }
      const localVarPath = `/api/PlannedActivityTypes/UpdatePlannedRule`;
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
        <any>"PlannedActivityTypesDtoUpdate" !== "string" ||
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
 * PlannedActivityTypesApi - functional programming interface
 * @export
 */
export const PlannedActivityTypesApiFp = function (
  configuration?: Configuration
) {
  return {
    PlannedActivityTypesGetFilterResult(
      body: PlannedActivityTypesQueryObjectGrid,
      propertyName?: string,
      propertyFilter?: string,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<Array<FilterValueDto>> {
      const localVarFetchArgs = PlannedActivityTypesApiFetchParamCreator(
        configuration
      ).PlannedActivityTypesGetFilterResult(
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
     * @param {PlannedActivityTypesDtoCreate} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    PlannedActivityTypesCreate(
      body: PlannedActivityTypesDtoCreate,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = PlannedActivityTypesApiFetchParamCreator(
        configuration
      ).PlannedActivityTypesCreate(body, options);
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
     * @param {any} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    PlannedActivityTypeEnableLinkedDC(
      body: any,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = PlannedActivityTypesApiFetchParamCreator(
        configuration
      ).PlannedActivityTypeEnableLinkedDC(body, options);
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
     * @param {number} [id]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    PlannedActivityTypesDelete(
      id?: number,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = PlannedActivityTypesApiFetchParamCreator(
        configuration
      ).PlannedActivityTypesDelete(id, options);
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
     * @param {PlannedActivityTypesQueryObjectGrid} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    PlannedActivityTypesGetGrid(
      body,
      options?: any
    ): (
      fetch?: FetchAPI,
      basePath?: string
    ) => Promise<QueryResultDtoOfPlannedActivityTypesDtoGrid> {
      const localVarFetchArgs = PlannedActivityTypesApiFetchParamCreator(
        configuration
      ).PlannedActivityTypesGetGrid(body, options);
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
     * @param {PlannedActivityTypesDtoUpdate} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    PlannedActivityTypesUpdate(
      body: PlannedActivityTypesDtoUpdate,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = PlannedActivityTypesApiFetchParamCreator(
        configuration
      ).PlannedActivityTypesUpdate(body, options);
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
 * PlannedActivityTypesApi - factory interface
 * @export
 */
export const PlannedActivityTypesApiFactory = function (
  configuration?: Configuration,
  fetch?: FetchAPI,
  basePath?: string
) {
  return {
    /**
     *
     * @param {PlannedActivityTypesQueryObjectGrid} body
     * @param {string} [propertyName]
     * @param {string} [propertyFilter]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    PlannedActivityTypesGetFilterResult(
      body: PlannedActivityTypesQueryObjectGrid,
      propertyName?: string,
      propertyFilter?: string,
      options?: any
    ) {
      return PlannedActivityTypesApiFp(
        configuration
      ).PlannedActivityTypesGetFilterResult(
        body,
        propertyName,
        propertyFilter,
        options
      )(fetch, basePath);
    },
    /**
     *
     * @param {PlannedActivityTypesDtoCreate} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    PlannedActivityTypesCreate(
      body: PlannedActivityTypesDtoCreate,
      options?: any
    ) {
      return PlannedActivityTypesApiFp(
        configuration
      ).PlannedActivityTypesCreate(body, options)(fetch, basePath);
    },
    /**
     *
     * @param {any} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    PlannedActivityTypeEnableLinkedDC(body: any, options?: any) {
      return PlannedActivityTypesApiFp(
        configuration
      ).PlannedActivityTypeEnableLinkedDC(body, options)(fetch, basePath);
    },
    /**
     *
     * @param {number} [id]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    PlannedActivityTypesDelete(id?: number, options?: any) {
      return PlannedActivityTypesApiFp(
        configuration
      ).PlannedActivityTypesDelete(id, options)(fetch, basePath);
    },
    /**
     *
     * @param {PlannedActivityTypesQueryObjectGrid} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    PlannedActivityTypesGetGrid(
      body?: PlannedActivityTypesQueryObjectGrid,
      options?: any
    ) {
      return PlannedActivityTypesApiFp(
        configuration
      ).PlannedActivityTypesGetGrid(body, options)(fetch, basePath);
    },
    /**
     *
     * @param {PlannedActivityTypesDtoUpdate} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    PlannedActivityTypesUpdate(
      body: PlannedActivityTypesDtoUpdate,
      options?: any
    ) {
      return PlannedActivityTypesApiFp(
        configuration
      ).PlannedActivityTypesUpdate(body, options)(fetch, basePath);
    },
  };
};

/**
 * PlannedActivityTypesApi - object-oriented interface
 * @export
 * @class PlannedActivityTypesApi
 * @extends {BaseAPI}
 */
export class PlannedActivityTypesApi extends BaseAPI {
  /**
   *
   * @param {PlannedActivityTypesQueryObjectGrid} body
   * @param {string} [propertyName]
   * @param {string} [propertyFilter]
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof MajorHardwareBuildApi
   */
  public PlannedActivityTypesGetFilterResult(
    body: PlannedActivityTypesQueryObjectGrid,
    propertyName?: string,
    propertyFilter?: string,
    options?: any
  ) {
    return PlannedActivityTypesApiFp(
      this.configuration
    ).PlannedActivityTypesGetFilterResult(
      body,
      propertyName,
      propertyFilter,
      options
    )(this.fetch, this.basePath);
  }

  /**
   *
   * @param {PlannedActivityTypesDtoCreate} body
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof PlannedActivityTypesApi
   */
  public PlannedActivityTypesCreate(
    body: PlannedActivityTypesDtoCreate,
    options?: any
  ) {
    return PlannedActivityTypesApiFp(
      this.configuration
    ).PlannedActivityTypesCreate(body, options)(this.fetch, this.basePath);
  }

  /**
   *
   * @param {any} body
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof PlannedActivityTypesApi
   */
  public PlannedActivityTypeEnableLinkedDC(body: any, options?: any) {
    return PlannedActivityTypesApiFp(
      this.configuration
    ).PlannedActivityTypeEnableLinkedDC(body, options)(
      this.fetch,
      this.basePath
    );
  }

  /**
   *
   * @param {number} [id]
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof PlannedActivityTypesApi
   */
  public PlannedActivityTypesDelete(id?: number, options?: any) {
    return PlannedActivityTypesApiFp(
      this.configuration
    ).PlannedActivityTypesDelete(id, options)(this.fetch, this.basePath);
  }

  /**
   *
   * @param {PlannedActivityTypesQueryObjectGrid} body
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof PlannedActivityTypesApi
   */
  public PlannedActivityTypesGetGrid(body?: any, options?: any) {
    return PlannedActivityTypesApiFp(
      this.configuration
    ).PlannedActivityTypesGetGrid(body, options)(this.fetch, this.basePath);
  }

  /**
   *
   * @param {PlannedActivityTypesDtoUpdate} body
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof PlannedActivityTypesApi
   */
  public PlannedActivityTypesUpdate(
    body: PlannedActivityTypesDtoUpdate,
    options?: any
  ) {
    return PlannedActivityTypesApiFp(
      this.configuration
    ).PlannedActivityTypesUpdate(body, options)(this.fetch, this.basePath);
  }
}
