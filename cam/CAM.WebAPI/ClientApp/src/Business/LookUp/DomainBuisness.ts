import * as url from "url";
import * as isomorphicFetch from "isomorphic-fetch";
import { Configuration } from "../Common/configuration";
import { headerObj } from "../header";
import {
  DataRemediationDto,
  ResultDto,
  ResultDtoOfResultDataRemediationDto,
} from "../../Model/CommonModels";

import {
  BASE_PATH,
  FetchArgs,
  FetchAPI,
  RequiredError,
  FilterValueDto,
} from "../Common/CommonBusiness";
import { BaseAPI } from "../Common/CommonBusiness";
import {
  QueryResultDtoOfSystemNamesDtoGrid,
  SystemNamesDto,
} from "../../Model/LookUp/Domain";

export const SystemNamesApiFetchParamCreator = function (
  configuration?: Configuration
) {
  return {
    /**
     *
     * @param {SystemNamesDto} body
     * @param {*} [options] Override http request option.
     */
    systemNamesCreate(body: SystemNamesDto, options: any = {}): FetchArgs {
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling systemNamesCreate."
        );
      }
      const localVarPath = `/api/SystemNames`;
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
        <any>"DeploymentStatusDto" !== "string" ||
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
     */
    systemNamesDelete(id?: number, options: any = {}): FetchArgs {
      const localVarPath = `/api/SystemNames/Delete`;
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
     */
    systemNamesDeleteDeep(id?: number, options: any = {}): FetchArgs {
      const localVarPath = `/api/SystemNames/DeleteDeep`;
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
     */
    systemNamesGetCreateResourceSystemNames(
      id: number,
      options: any = {}
    ): FetchArgs {
      const localVarPath = `/api/SystemNames/Create`;
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
     * @param {Array<number>} [systemNameId]
     * @param {Array<string>} [systemNameDescription]
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
     * @param {*} [options] Override http request option.
     */
    systemNamesGetSystemNames(
      systemNameId?: Array<number>,
      systemNameDescription?: Array<string>,
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
      options: any = {}
    ): FetchArgs {
      const localVarPath = `/api/SystemNames`;
      const localVarUrlObj = url.parse(localVarPath, true);
      const localVarRequestOptions = Object.assign({ method: "GET" }, options);
      const localVarHeaderParameter = { ...headerObj } as any;
      const localVarQueryParameter = {} as any;
      if (configuration && configuration.apiKey) {
        const localVarApiKeyValue =
          typeof configuration.apiKey === "function"
            ? configuration.apiKey("Authorization")
            : configuration.apiKey;
        localVarHeaderParameter["Authorization"] = localVarApiKeyValue;
      }

      if (systemNameId) {
        localVarQueryParameter["systemNameId"] = systemNameId;
      }

      if (systemNameDescription) {
        localVarQueryParameter["systemNameDescription"] = systemNameDescription;
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
     * @param {Array<number>} [systemNameId]
     * @param {Array<string>} [systemNameDescription]
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
     * @param {*} [options] Override http request option.
     */
    systemNamesGetFilterResult(
      propertyName?: string,
      propertyFilter?: string,
      systemNameId?: Array<number>,
      systemNameDescription?: Array<string>,
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
      options: any = {}
    ): FetchArgs {
      const localVarPath = `/api/SystemNames/FilterResult`;
      const localVarUrlObj = new URL(
        localVarPath,
        "/api/SystemNames/FilterResult"
      );
      const queryParameters = new URLSearchParams();

      if (propertyName !== undefined) {
        queryParameters.set("propertyName", propertyName);
      }
      if (propertyFilter !== undefined) {
        queryParameters.set("propertyFilter", propertyFilter);
      }
      if (systemNameId) {
        systemNameId.forEach((val) =>
          queryParameters.append("systemNameId", val.toString())
        );
      }

      if (systemNameDescription) {
        systemNameDescription.forEach((val) =>
          queryParameters.append("systemNameDescription", val)
        );
      }
      if (sortBy !== undefined) {
        queryParameters.set("sortBy", sortBy);
      }
      if (isSortAscending !== undefined) {
        queryParameters.set("isSortAscending", String(isSortAscending));
      }
      if (page !== undefined) {
        queryParameters.set("page", page.toString());
      }
      if (pageSize !== undefined) {
        queryParameters.set("pageSize", pageSize.toString());
      }
      if (lastModifiedStartDate !== undefined) {
        queryParameters.set(
          "lastModifiedStartDate",
          lastModifiedStartDate.toISOString()
        );
      }
      if (lastModifiedEndDate !== undefined) {
        queryParameters.set(
          "lastModifiedEndDate",
          lastModifiedEndDate.toISOString()
        );
      }
      if (principalId !== undefined) {
        queryParameters.set("principalId", principalId.toString());
      }
      if (deleted !== undefined) {
        queryParameters.set("deleted", String(deleted));
      }
      if (orphan !== undefined) {
        queryParameters.set("orphan", String(orphan));
      }
      if (lastModifiedBy) {
        lastModifiedBy.forEach((val) =>
          queryParameters.append("lastModifiedBy", val)
        );
      }

      localVarUrlObj.search = queryParameters.toString();

      const localVarRequestOptions = { method: "GET", ...options };
      const localVarHeaders = new Headers(options.headers);

      localVarRequestOptions.headers = localVarHeaders;

      return {
        url: localVarUrlObj.pathname + localVarUrlObj.search,
        options: localVarRequestOptions,
      };
    },

    /**
     *
     * @param {number} id
     * @param {*} [options] Override http request option.
     */
    systemNamesGetRelatedRecords(id: number, options: any = {}): FetchArgs {
      if (id === null || id === undefined) {
        throw new RequiredError(
          "id",
          "Required parameter id was null or undefined when calling systemNamesGetRelatedRecords."
        );
      }
      const localVarPath = `/api/SystemNames/RelatedRecords/${encodeURIComponent(
        String(id)
      )}`;
      const localVarUrlObj = new URL(
        localVarPath,
        "/api/SystemNames/RelatedRecords"
      );
      const localVarRequestOptions = { method: "GET", ...options };
      const localVarHeaders = new Headers(options.headers);

      localVarRequestOptions.headers = localVarHeaders;

      return {
        url: localVarUrlObj.pathname + localVarUrlObj.search,
        options: localVarRequestOptions,
      };
    },

    /**
     *
     * @param {number} id
     * @param {*} [options] Override http request option.
     */
    systemNamesGetUpdateResourceSystemNames(
      id: number,
      options: any = {}
    ): FetchArgs {
      if (id === null || id === undefined) {
        throw new RequiredError(
          "id",
          "Required parameter id was null or undefined when calling deploymentStatusGetUpdateResourceDeploymentStatus."
        );
      }
      const localVarPath = `/api/SystemNames/Update{id}`.replace(
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
     * @param {SystemNamesDto} body
     * @param {*} [options] Override http request option.
     */
    systemNamesPut(body: SystemNamesDto, options: any = {}): FetchArgs {
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling deploymentStatusPut."
        );
      }
      const localVarPath = `/api/SystemNames`;
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
        <any>"SystemNamedsDTO" !== "string" ||
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

export const SystemNamesApiFp = function (configuration?: Configuration) {
  return {
    /**
     *
     * @param {SystemNamesDto} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    systemNamesCreate(
      body: SystemNamesDto,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = SystemNamesApiFetchParamCreator(
        configuration
      ).systemNamesCreate(body, options);
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
    systemNamesDelete(
      id?: number,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = SystemNamesApiFetchParamCreator(
        configuration
      ).systemNamesDelete(id, options);
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
    systemNamesDeleteDeep(
      id?: number,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = SystemNamesApiFetchParamCreator(
        configuration
      ).systemNamesDelete(id, options);
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
    systemNamesGetCreateResourceSystemNames(
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<SystemNamesDto> {
      const localVarFetchArgs =
        SystemNamesApiFetchParamCreator(
          configuration
        ).systemNamesGetCreateResourceSystemNames(options);
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
     * @param {Array<number>} [systemNameId]
     * @param {Array<string>} [systemNameDescription]
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
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    systemNamesGetSystemNames(
      systemNameId?: Array<number>,
      systemNameDescription?: Array<string>,
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
      options?: any
    ): (
      fetch?: FetchAPI,
      basePath?: string
    ) => Promise<QueryResultDtoOfSystemNamesDtoGrid> {
      const localVarFetchArgs = SystemNamesApiFetchParamCreator(
        configuration
      ).systemNamesGetSystemNames(
        systemNameId,
        systemNameDescription,
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
     * @param {string} [propertyName]
     * @param {string} [propertyFilter]
     * @param {Array<number>} [systemNameId]
     * @param {Array<string>} [systemNameDescription]
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
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    systemNamesGetFilterResult(
      propertyName?: string,
      propertyFilter?: string,
      systemNameId?: Array<number>,
      systemNameDescription?: Array<string>,
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
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<Array<FilterValueDto>> {
      const localVarFetchArgs = SystemNamesApiFetchParamCreator(
        configuration
      ).systemNamesGetFilterResult(
        propertyName,
        propertyFilter,
        systemNameId,
        systemNameDescription,
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
    systemNamesGetRelatedRecords(
      id: number,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = SystemNamesApiFetchParamCreator(
        configuration
      ).systemNamesGetRelatedRecords(id, options);
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
    systemNamesGetUpdateResourceSystemNames(
      id: number,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<SystemNamesDto> {
      const localVarFetchArgs = SystemNamesApiFetchParamCreator(
        configuration
      ).systemNamesGetUpdateResourceSystemNames(id, options);
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
     * @param {SystemNamesDto} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    systemNamesPut(
      body: SystemNamesDto,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = SystemNamesApiFetchParamCreator(
        configuration
      ).systemNamesPut(body, options);
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

export const SystemNamesApiFactory = function (
  configuration?: Configuration,
  fetch?: FetchAPI,
  basePath?: string
) {
  return {
    /**
     *
     * @param {SystemNamesDto} body
     * @param {*} [options] Override http request option.
     */
    systemNamesCreate(body: SystemNamesDto, options?: any): Promise<ResultDto> {
      return SystemNamesApiFp(configuration).systemNamesCreate(body, options)(
        fetch,
        basePath
      );
    },

    /**
     *
     * @param {number} [id]
     * @param {*} [options] Override http request option.
     */
    systemNamesDelete(id?: number, options?: any): Promise<ResultDto> {
      return SystemNamesApiFp(configuration).systemNamesDelete(id, options)(
        fetch,
        basePath
      );
    },

    /**
     *
     * @param {number} [id]
     * @param {*} [options] Override http request option.
     */
    systemNamesDeleteDeep(id?: number, options?: any): Promise<ResultDto> {
      return SystemNamesApiFp(configuration).systemNamesDeleteDeep(id, options)(
        fetch,
        basePath
      );
    },

    /**
     *
     * @param {*} [options] Override http request option.
     */
    systemNamesGetCreateResourceSystemNames(
      options?: any
    ): Promise<SystemNamesDto> {
      return SystemNamesApiFp(
        configuration
      ).systemNamesGetCreateResourceSystemNames(options)(fetch, basePath);
    },

    /**
     *
     * @param {Array<number>} [systemNameId]
     * @param {Array<string>} [systemNameDescription]
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
     * @param {*} [options] Override http request option.
     */
    systemNamesGetSystemNames(
      systemNameId?: Array<number>,
      systemNameDescription?: Array<string>,
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
      options?: any
    ): Promise<QueryResultDtoOfSystemNamesDtoGrid> {
      return SystemNamesApiFp(configuration).systemNamesGetSystemNames(
        systemNameId,
        systemNameDescription,
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
        options
      )(fetch, basePath);
    },

    /**
     *
     * @param {string} [propertyName]
     * @param {string} [propertyFilter]
     * @param {Array<number>} [systemNameId]
     * @param {Array<string>} [systemNameDescription]
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
     * @param {*} [options] Override http request option.
     */
    systemNamesGetFilterResult(
      propertyName?: string,
      propertyFilter?: string,
      systemNameId?: Array<number>,
      systemNameDescription?: Array<string>,
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
      options?: any
    ): Promise<Array<FilterValueDto>> {
      return SystemNamesApiFp(configuration).systemNamesGetFilterResult(
        propertyName,
        propertyFilter,
        systemNameId,
        systemNameDescription,
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
        options
      )(fetch, basePath);
    },

    /**
     *
     * @param {number} id
     * @param {*} [options] Override http request option.
     */
    systemNamesGetRelatedRecords(
      id: number,
      options?: any
    ): Promise<ResultDto> {
      return SystemNamesApiFp(configuration).systemNamesGetRelatedRecords(
        id,
        options
      )(fetch, basePath);
    },

    /**
     *
     * @param {number} id
     * @param {*} [options] Override http request option.
     */
    systemNamesGetUpdateResourceSystemNames(
      id: number,
      options?: any
    ): Promise<SystemNamesDto> {
      return SystemNamesApiFp(
        configuration
      ).systemNamesGetUpdateResourceSystemNames(id, options)(fetch, basePath);
    },

    /**
     *
     * @param {SystemNamesDto} body
     * @param {*} [options] Override http request option.
     */
    systemNamesPut(body: SystemNamesDto, options?: any): Promise<ResultDto> {
      return SystemNamesApiFp(configuration).systemNamesPut(body, options)(
        fetch,
        basePath
      );
    },
  };
};

export class SystemNamesApi extends BaseAPI {
  public systemNamesCreate(body: SystemNamesDto, options?: any) {
    return SystemNamesApiFp(this.configuration).systemNamesCreate(
      body,
      options
    )(this.fetch, this.basePath);
  }

  public systemNamesDelete(id?: number, options?: any) {
    return SystemNamesApiFp(this.configuration).systemNamesDelete(id, options)(
      this.fetch,
      this.basePath
    );
  }

  public systemNamesGetCreateResourceSystemNames(options?: any) {
    return SystemNamesApiFp(
      this.configuration
    ).systemNamesGetCreateResourceSystemNames(options)(
      this.fetch,
      this.basePath
    );
  }

  public systemNamesGetSystemNames(
    systemNameId?: Array<number>,
    systemNameDescription?: Array<string>,
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
    options?: any
  ) {
    return SystemNamesApiFp(this.configuration).systemNamesGetSystemNames(
      systemNameId,
      systemNameDescription,
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
      options
    )(this.fetch, this.basePath);
  }

  public systemNamesGetRelatedRecords(id: number, options?: any) {
    return SystemNamesApiFp(this.configuration).systemNamesGetRelatedRecords(
      id,
      options
    )(this.fetch, this.basePath);
  }

  public systemNamesGetUpdateResourceSystemNames(id: number, options?: any) {
    return SystemNamesApiFp(
      this.configuration
    ).systemNamesGetUpdateResourceSystemNames(id, options)(
      this.fetch,
      this.basePath
    );
  }

  public systemNamesPut(body: SystemNamesDto, options?: any) {
    return SystemNamesApiFp(this.configuration).systemNamesPut(body, options)(
      this.fetch,
      this.basePath
    );
  }

  /**
   * Added filter method to fetch filtered values for a specific column/property
   */
  public systemNamesGetFilterResult(
    propertyName?: string,
    propertyFilter?: string,
    systemNameId?: Array<number>,
    systemNameDescription?: Array<string>,
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
    options?: any
  ) {
    return SystemNamesApiFp(this.configuration).systemNamesGetFilterResult(
      propertyName,
      propertyFilter,
      systemNameId,
      systemNameDescription,
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
      options
    )(this.fetch, this.basePath);
  }
}
