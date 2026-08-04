import * as url from "url";
import * as isomorphicFetch from "isomorphic-fetch";
import { Configuration } from "../Common/configuration";
import { ResultDto } from "../../Model/CommonModels";
import { headerObj } from "../header";

import {
  BASE_PATH,
  FetchArgs,
  FetchAPI,
  BaseAPI,
  RequiredError,
  FilterValueDto,
} from "../Common/CommonBusiness";
import {
  QueryResultDtoOfTipologicaGridDto,
  TipologicaGridDto,
} from "../../Model/LookUp/LookUpGenericModel";
/**
 * OperatingSystemApi - fetch parameter creator
 * @export
 */
export const OperatingSystemApiFetchParamCreator = function (
  configuration?: Configuration
) {
  return {
    /**
     *
     * @param {TipologicaGridDto} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    operatingSystemCreate(
      body: TipologicaGridDto,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling operatingSystemCreate."
        );
      }
      const localVarPath = `/api/OperatingSystem`;
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
    /**
     *
     * @param {number} [id]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    operatingSystemDelete(id?: number, options: any = {}): FetchArgs {
      const localVarPath = `/api/OperatingSystem/Delete`;
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
     * @param {number} [id]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    operatingSystemDeleteDeep(id?: number, options: any = {}): FetchArgs {
      const localVarPath = `/api/OperatingSystem/DeleteDeep`;
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
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    operatingSystemGetCreateResourceOperatingSystem(
      options: any = {}
    ): FetchArgs {
      const localVarPath = `/api/OperatingSystem/Create`;
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
     * @param {string} [propertyName]
     * @param {string} [propertyFilter]
     * @param {string} [sortBy]
     * @param {boolean} [isSortAscending]
     * @param {number} [page]
     * @param {number} [pageSize]
     * @param {Date} [lastModifiedStartDate]
     * @param {Date} [lastModifiedEndDate]
     * @param {number} [principalId]
     * @param {boolean} [deleted]
     * @param {boolean} [orphan]
     * @param {Array<string>} [lastModifiedBy]
     * @param {Array<number>} [id]
     * @param {Array<string>} [description]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    operatingSystemGetFilterResult(
      propertyName?: string,
      propertyFilter?: string,
      sortBy?: string,
      isSortAscending?: boolean,
      page?: number,
      pageSize?: number,
      lastModifiedStartDate?: Date,
      lastModifiedEndDate?: Date,
      principalId?: number,
      deleted?: boolean,
      orphan?: boolean,
      lastModifiedBy?: Array<string>,
      id?: Array<number>,
      description?: Array<string>,
      options: any = {}
    ): FetchArgs {
      const localVarPath = `/api/OperatingSystem/Filter`;
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

      if (propertyName !== undefined) {
        localVarQueryParameter["propertyName"] = propertyName;
      }

      if (propertyFilter !== undefined) {
        localVarQueryParameter["propertyFilter"] = propertyFilter;
      }

      if (sortBy !== undefined) {
        localVarQueryParameter["SortBy"] = sortBy;
      }

      if (isSortAscending !== undefined) {
        localVarQueryParameter["IsSortAscending"] = isSortAscending;
      }

      if (page !== undefined) {
        localVarQueryParameter["Page"] = page;
      }

      if (pageSize !== undefined) {
        localVarQueryParameter["PageSize"] = pageSize;
      }

      if (lastModifiedStartDate !== undefined) {
        localVarQueryParameter["LastModified.StartDate"] = (
          lastModifiedStartDate as any
        ).toISOString();
      }

      if (lastModifiedEndDate !== undefined) {
        localVarQueryParameter["LastModified.EndDate"] = (
          lastModifiedEndDate as any
        ).toISOString();
      }

      if (principalId !== undefined) {
        localVarQueryParameter["PrincipalId"] = principalId;
      }

      if (deleted !== undefined) {
        localVarQueryParameter["Deleted"] = deleted;
      }

      if (orphan !== undefined) {
        localVarQueryParameter["Orphan"] = orphan;
      }

      if (lastModifiedBy) {
        localVarQueryParameter["LastModifiedBy"] = lastModifiedBy;
      }

      if (id) {
        localVarQueryParameter["Id"] = id;
      }

      if (description) {
        localVarQueryParameter["Description"] = description;
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
     * @param {string} [sortBy]
     * @param {boolean} [isSortAscending]
     * @param {number} [page]
     * @param {number} [pageSize]
     * @param {Date} [lastModifiedStartDate]
     * @param {Date} [lastModifiedEndDate]
     * @param {number} [principalId]
     * @param {boolean} [deleted]
     * @param {boolean} [orphan]
     * @param {Array<string>} [lastModifiedBy]
     * @param {Array<number>} [id]
     * @param {Array<string>} [description]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    operatingSystemGetOperatingSystem(
      sortBy?: string,
      isSortAscending?: boolean,
      page?: number,
      pageSize?: number,
      lastModifiedStartDate?: Date,
      lastModifiedEndDate?: Date,
      principalId?: number,
      deleted?: boolean,
      orphan?: boolean,
      lastModifiedBy?: Array<string>,
      id?: Array<number>,
      description?: Array<string>,
      options: any = {}
    ): FetchArgs {
      const localVarPath = `/api/OperatingSystem`;
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

      if (sortBy !== undefined) {
        localVarQueryParameter["SortBy"] = sortBy;
      }

      if (isSortAscending !== undefined) {
        localVarQueryParameter["IsSortAscending"] = isSortAscending;
      }

      if (page !== undefined) {
        localVarQueryParameter["Page"] = page;
      }

      if (pageSize !== undefined) {
        localVarQueryParameter["PageSize"] = pageSize;
      }

      if (lastModifiedStartDate !== undefined) {
        localVarQueryParameter["LastModified.StartDate"] = (
          lastModifiedStartDate as any
        ).toISOString();
      }

      if (lastModifiedEndDate !== undefined) {
        localVarQueryParameter["LastModified.EndDate"] = (
          lastModifiedEndDate as any
        ).toISOString();
      }

      if (principalId !== undefined) {
        localVarQueryParameter["PrincipalId"] = principalId;
      }

      if (deleted !== undefined) {
        localVarQueryParameter["Deleted"] = deleted;
      }

      if (orphan !== undefined) {
        localVarQueryParameter["Orphan"] = orphan;
      }

      if (lastModifiedBy) {
        localVarQueryParameter["LastModifiedBy"] = lastModifiedBy;
      }

      if (id) {
        localVarQueryParameter["Id"] = id;
      }

      if (description) {
        localVarQueryParameter["Description"] = description;
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
    operatingSystemGetRelatedRecords(id: number, options: any = {}): FetchArgs {
      // verify required parameter 'id' is not null or undefined
      if (id === null || id === undefined) {
        throw new RequiredError(
          "id",
          "Required parameter id was null or undefined when calling operatingSystemGetRelatedRecords."
        );
      }
      const localVarPath = `/api/OperatingSystem/GetRelatedRecords{id}`.replace(
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
     * @param {number} id
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    operatingSystemGetUpdateResourceOperatingSystem(
      id: number,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'id' is not null or undefined
      if (id === null || id === undefined) {
        throw new RequiredError(
          "id",
          "Required parameter id was null or undefined when calling operatingSystemGetUpdateResourceOperatingSystem."
        );
      }
      const localVarPath = `/api/OperatingSystem/Update{id}`.replace(
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
     * @param {TipologicaGridDto} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    operatingSystemPut(body: TipologicaGridDto, options: any = {}): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling operatingSystemPut."
        );
      }
      const localVarPath = `/api/OperatingSystem`;
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
  };
};

/**
 * OperatingSystemApi - functional programming interface
 * @export
 */
export const OperatingSystemApiFp = function (configuration?: Configuration) {
  return {
    /**
     *
     * @param {TipologicaGridDto} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    operatingSystemCreate(
      body: TipologicaGridDto,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = OperatingSystemApiFetchParamCreator(
        configuration
      ).operatingSystemCreate(body, options);
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
    operatingSystemDelete(
      id?: number,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = OperatingSystemApiFetchParamCreator(
        configuration
      ).operatingSystemDelete(id, options);
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
    operatingSystemDeleteDeep(
      id?: number,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = OperatingSystemApiFetchParamCreator(
        configuration
      ).operatingSystemDeleteDeep(id, options);
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
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    operatingSystemGetCreateResourceOperatingSystem(
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<TipologicaGridDto> {
      const localVarFetchArgs =
        OperatingSystemApiFetchParamCreator(
          configuration
        ).operatingSystemGetCreateResourceOperatingSystem(options);
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
     * @param {string} [propertyName]
     * @param {string} [propertyFilter]
     * @param {string} [sortBy]
     * @param {boolean} [isSortAscending]
     * @param {number} [page]
     * @param {number} [pageSize]
     * @param {Date} [lastModifiedStartDate]
     * @param {Date} [lastModifiedEndDate]
     * @param {number} [principalId]
     * @param {boolean} [deleted]
     * @param {boolean} [orphan]
     * @param {Array<string>} [lastModifiedBy]
     * @param {Array<number>} [id]
     * @param {Array<string>} [description]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    operatingSystemGetFilterResult(
      propertyName?: string,
      propertyFilter?: string,
      sortBy?: string,
      isSortAscending?: boolean,
      page?: number,
      pageSize?: number,
      lastModifiedStartDate?: Date,
      lastModifiedEndDate?: Date,
      principalId?: number,
      deleted?: boolean,
      orphan?: boolean,
      lastModifiedBy?: Array<string>,
      id?: Array<number>,
      description?: Array<string>,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<Array<FilterValueDto>> {
      const localVarFetchArgs = OperatingSystemApiFetchParamCreator(
        configuration
      ).operatingSystemGetFilterResult(
        propertyName,
        propertyFilter,
        sortBy,
        isSortAscending,
        page,
        pageSize,
        lastModifiedStartDate,
        lastModifiedEndDate,
        principalId,
        deleted,
        orphan,
        lastModifiedBy,
        id,
        description,
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
     * @param {string} [sortBy]
     * @param {boolean} [isSortAscending]
     * @param {number} [page]
     * @param {number} [pageSize]
     * @param {Date} [lastModifiedStartDate]
     * @param {Date} [lastModifiedEndDate]
     * @param {number} [principalId]
     * @param {boolean} [deleted]
     * @param {boolean} [orphan]
     * @param {Array<string>} [lastModifiedBy]
     * @param {Array<number>} [id]
     * @param {Array<string>} [description]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    operatingSystemGetOperatingSystem(
      sortBy?: string,
      isSortAscending?: boolean,
      page?: number,
      pageSize?: number,
      lastModifiedStartDate?: Date,
      lastModifiedEndDate?: Date,
      principalId?: number,
      deleted?: boolean,
      orphan?: boolean,
      lastModifiedBy?: Array<string>,
      id?: Array<number>,
      description?: Array<string>,
      options?: any
    ): (
      fetch?: FetchAPI,
      basePath?: string
    ) => Promise<QueryResultDtoOfTipologicaGridDto> {
      const localVarFetchArgs = OperatingSystemApiFetchParamCreator(
        configuration
      ).operatingSystemGetOperatingSystem(
        sortBy,
        isSortAscending,
        page,
        pageSize,
        lastModifiedStartDate,
        lastModifiedEndDate,
        principalId,
        deleted,
        orphan,
        lastModifiedBy,
        id,
        description,
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
     * @param {number} id
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    operatingSystemGetRelatedRecords(
      id: number,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = OperatingSystemApiFetchParamCreator(
        configuration
      ).operatingSystemGetRelatedRecords(id, options);
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
    operatingSystemGetUpdateResourceOperatingSystem(
      id: number,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<TipologicaGridDto> {
      const localVarFetchArgs = OperatingSystemApiFetchParamCreator(
        configuration
      ).operatingSystemGetUpdateResourceOperatingSystem(id, options);
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
     * @param {TipologicaGridDto} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    operatingSystemPut(
      body: TipologicaGridDto,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = OperatingSystemApiFetchParamCreator(
        configuration
      ).operatingSystemPut(body, options);
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
 * OperatingSystemApi - factory interface
 * @export
 */
export const OperatingSystemApiFactory = function (
  configuration?: Configuration,
  fetch?: FetchAPI,
  basePath?: string
) {
  return {
    /**
     *
     * @param {TipologicaGridDto} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    operatingSystemCreate(body: TipologicaGridDto, options?: any) {
      return OperatingSystemApiFp(configuration).operatingSystemCreate(
        body,
        options
      )(fetch, basePath);
    },
    /**
     *
     * @param {number} [id]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    operatingSystemDelete(id?: number, options?: any) {
      return OperatingSystemApiFp(configuration).operatingSystemDelete(
        id,
        options
      )(fetch, basePath);
    },
    /**
     *
     * @param {number} [id]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    operatingSystemDeleteDeep(id?: number, options?: any) {
      return OperatingSystemApiFp(configuration).operatingSystemDeleteDeep(
        id,
        options
      )(fetch, basePath);
    },
    /**
     *
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    operatingSystemGetCreateResourceOperatingSystem(options?: any) {
      return OperatingSystemApiFp(
        configuration
      ).operatingSystemGetCreateResourceOperatingSystem(options)(
        fetch,
        basePath
      );
    },
    /**
     *
     * @param {string} [propertyName]
     * @param {string} [propertyFilter]
     * @param {string} [sortBy]
     * @param {boolean} [isSortAscending]
     * @param {number} [page]
     * @param {number} [pageSize]
     * @param {Date} [lastModifiedStartDate]
     * @param {Date} [lastModifiedEndDate]
     * @param {number} [principalId]
     * @param {boolean} [deleted]
     * @param {boolean} [orphan]
     * @param {Array<string>} [lastModifiedBy]
     * @param {Array<number>} [id]
     * @param {Array<string>} [description]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    operatingSystemGetFilterResult(
      propertyName?: string,
      propertyFilter?: string,
      sortBy?: string,
      isSortAscending?: boolean,
      page?: number,
      pageSize?: number,
      lastModifiedStartDate?: Date,
      lastModifiedEndDate?: Date,
      principalId?: number,
      deleted?: boolean,
      orphan?: boolean,
      lastModifiedBy?: Array<string>,
      id?: Array<number>,
      description?: Array<string>,
      options?: any
    ) {
      return OperatingSystemApiFp(configuration).operatingSystemGetFilterResult(
        propertyName,
        propertyFilter,
        sortBy,
        isSortAscending,
        page,
        pageSize,
        lastModifiedStartDate,
        lastModifiedEndDate,
        principalId,
        deleted,
        orphan,
        lastModifiedBy,
        id,
        description,
        options
      )(fetch, basePath);
    },
    /**
     *
     * @param {string} [sortBy]
     * @param {boolean} [isSortAscending]
     * @param {number} [page]
     * @param {number} [pageSize]
     * @param {Date} [lastModifiedStartDate]
     * @param {Date} [lastModifiedEndDate]
     * @param {number} [principalId]
     * @param {boolean} [deleted]
     * @param {boolean} [orphan]
     * @param {Array<string>} [lastModifiedBy]
     * @param {Array<number>} [id]
     * @param {Array<string>} [description]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    operatingSystemGetOperatingSystem(
      sortBy?: string,
      isSortAscending?: boolean,
      page?: number,
      pageSize?: number,
      lastModifiedStartDate?: Date,
      lastModifiedEndDate?: Date,
      principalId?: number,
      deleted?: boolean,
      orphan?: boolean,
      lastModifiedBy?: Array<string>,
      id?: Array<number>,
      description?: Array<string>,
      options?: any
    ) {
      return OperatingSystemApiFp(
        configuration
      ).operatingSystemGetOperatingSystem(
        sortBy,
        isSortAscending,
        page,
        pageSize,
        lastModifiedStartDate,
        lastModifiedEndDate,
        principalId,
        deleted,
        orphan,
        lastModifiedBy,
        id,
        description,
        options
      )(fetch, basePath);
    },
    /**
     *
     * @param {number} id
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    operatingSystemGetRelatedRecords(id: number, options?: any) {
      return OperatingSystemApiFp(
        configuration
      ).operatingSystemGetRelatedRecords(id, options)(fetch, basePath);
    },
    /**
     *
     * @param {number} id
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    operatingSystemGetUpdateResourceOperatingSystem(id: number, options?: any) {
      return OperatingSystemApiFp(
        configuration
      ).operatingSystemGetUpdateResourceOperatingSystem(id, options)(
        fetch,
        basePath
      );
    },
    /**
     *
     * @param {TipologicaGridDto} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    operatingSystemPut(body: TipologicaGridDto, options?: any) {
      return OperatingSystemApiFp(configuration).operatingSystemPut(
        body,
        options
      )(fetch, basePath);
    },
  };
};

/**
 * OperatingSystemApi - object-oriented interface
 * @export
 * @class OperatingSystemApi
 * @extends {BaseAPI}
 */
export class OperatingSystemApi extends BaseAPI {
  /**
   *
   * @param {TipologicaGridDto} body
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof OperatingSystemApi
   */
  public operatingSystemCreate(body: TipologicaGridDto, options?: any) {
    return OperatingSystemApiFp(this.configuration).operatingSystemCreate(
      body,
      options
    )(this.fetch, this.basePath);
  }

  /**
   *
   * @param {number} [id]
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof OperatingSystemApi
   */
  public operatingSystemDelete(id?: number, options?: any) {
    return OperatingSystemApiFp(this.configuration).operatingSystemDelete(
      id,
      options
    )(this.fetch, this.basePath);
  }

  /**
   *
   * @param {number} [id]
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof OperatingSystemApi
   */
  public operatingSystemDeleteDeep(id?: number, options?: any) {
    return OperatingSystemApiFp(this.configuration).operatingSystemDeleteDeep(
      id,
      options
    )(this.fetch, this.basePath);
  }

  /**
   *
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof OperatingSystemApi
   */
  public operatingSystemGetCreateResourceOperatingSystem(options?: any) {
    return OperatingSystemApiFp(
      this.configuration
    ).operatingSystemGetCreateResourceOperatingSystem(options)(
      this.fetch,
      this.basePath
    );
  }

  /**
   *
   * @param {string} [propertyName]
   * @param {string} [propertyFilter]
   * @param {string} [sortBy]
   * @param {boolean} [isSortAscending]
   * @param {number} [page]
   * @param {number} [pageSize]
   * @param {Date} [lastModifiedStartDate]
   * @param {Date} [lastModifiedEndDate]
   * @param {number} [principalId]
   * @param {boolean} [deleted]
   * @param {boolean} [orphan]
   * @param {Array<string>} [lastModifiedBy]
   * @param {Array<number>} [id]
   * @param {Array<string>} [description]
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof OperatingSystemApi
   */
  public operatingSystemGetFilterResult(
    propertyName?: string,
    propertyFilter?: string,
    sortBy?: string,
    isSortAscending?: boolean,
    page?: number,
    pageSize?: number,
    lastModifiedStartDate?: Date,
    lastModifiedEndDate?: Date,
    principalId?: number,
    deleted?: boolean,
    orphan?: boolean,
    lastModifiedBy?: Array<string>,
    id?: Array<number>,
    description?: Array<string>,
    options?: any
  ) {
    return OperatingSystemApiFp(
      this.configuration
    ).operatingSystemGetFilterResult(
      propertyName,
      propertyFilter,
      sortBy,
      isSortAscending,
      page,
      pageSize,
      lastModifiedStartDate,
      lastModifiedEndDate,
      principalId,
      deleted,
      orphan,
      lastModifiedBy,
      id,
      description,
      options
    )(this.fetch, this.basePath);
  }

  /**
   *
   * @param {string} [sortBy]
   * @param {boolean} [isSortAscending]
   * @param {number} [page]
   * @param {number} [pageSize]
   * @param {Date} [lastModifiedStartDate]
   * @param {Date} [lastModifiedEndDate]
   * @param {number} [principalId]
   * @param {boolean} [deleted]
   * @param {boolean} [orphan]
   * @param {Array<string>} [lastModifiedBy]
   * @param {Array<number>} [id]
   * @param {Array<string>} [description]
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof OperatingSystemApi
   */
  public operatingSystemGetOperatingSystem(
    sortBy?: string,
    isSortAscending?: boolean,
    page?: number,
    pageSize?: number,
    lastModifiedStartDate?: Date,
    lastModifiedEndDate?: Date,
    principalId?: number,
    deleted?: boolean,
    orphan?: boolean,
    lastModifiedBy?: Array<string>,
    id?: Array<number>,
    description?: Array<string>,
    options?: any
  ) {
    return OperatingSystemApiFp(
      this.configuration
    ).operatingSystemGetOperatingSystem(
      sortBy,
      isSortAscending,
      page,
      pageSize,
      lastModifiedStartDate,
      lastModifiedEndDate,
      principalId,
      deleted,
      orphan,
      lastModifiedBy,
      id,
      description,
      options
    )(this.fetch, this.basePath);
  }

  /**
   *
   * @param {number} id
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof OperatingSystemApi
   */
  public operatingSystemGetRelatedRecords(id: number, options?: any) {
    return OperatingSystemApiFp(
      this.configuration
    ).operatingSystemGetRelatedRecords(id, options)(this.fetch, this.basePath);
  }

  /**
   *
   * @param {number} id
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof OperatingSystemApi
   */
  public operatingSystemGetUpdateResourceOperatingSystem(
    id: number,
    options?: any
  ) {
    return OperatingSystemApiFp(
      this.configuration
    ).operatingSystemGetUpdateResourceOperatingSystem(id, options)(
      this.fetch,
      this.basePath
    );
  }

  /**
   *
   * @param {TipologicaGridDto} body
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof OperatingSystemApi
   */
  public operatingSystemPut(body: TipologicaGridDto, options?: any) {
    return OperatingSystemApiFp(this.configuration).operatingSystemPut(
      body,
      options
    )(this.fetch, this.basePath);
  }
}
