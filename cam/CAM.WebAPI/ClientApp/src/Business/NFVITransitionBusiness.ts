import * as url from "url";
import * as isomorphicFetch from "isomorphic-fetch";
import { Configuration } from "./Common/configuration";
import { ResultDto } from "../Model/CommonModels";
import {
  BASE_PATH,
  FetchArgs,
  FetchAPI,
  RequiredError,
  FilterValueDto,
} from "./Common/CommonBusiness";
import { BaseAPI } from "./Common/CommonBusiness";
import { ReturnFile } from "../Model/Common";
import {
  NFVITransitionDto,
  NFVITransitionDtoCreate,
  NFVITransitionDtoUpdate,
  QueryResultDtoOfNFVITransitionDtoGrid,
} from "../Model/NFVITransition";
import { NFVITransitionQueryObjectGrid as NFVITransitionQueryDto } from "../Model/NFVITransition";
import { headerObj } from "./header";

/**
 * NFVITransitionApi - fetch parameter creator
 * @export
 */
export const NFVITransitionApiFetchParamCreator = function (
  configuration?: Configuration
) {
  return {
    /**
     *
     * @param {NFVITransitionDtoCreate} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    nFVITransitionCreate(
      body: NFVITransitionDtoCreate,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling nFVITransitionCreate."
        );
      }
      const localVarPath = `/api/NFVITransition`;
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
        <any>"NFVITransitionDtoCreate" !== "string" ||
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
    nFVITransitionDelete(id?: number, options: any = {}): FetchArgs {
      const localVarPath = `/api/NFVITransition`;
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
    nFVITransitionDeleteDeep(id?: number, options: any = {}): FetchArgs {
      const localVarPath = `/api/NFVITransition/DeleteDeep`;
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
     * @param {NFVITransitionQueryDto} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    nFVITransitionExportReport(
      body: NFVITransitionQueryDto,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling nFVITransitionExportReport."
        );
      }
      const localVarPath = `/api/NFVITransition/ExportReport`;
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
        <any>"NFVITransitionQueryDto" !== "string" ||
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
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    nFVITransitionGetCreateResourceVNFTransition(options: any = {}): FetchArgs {
      const localVarPath = `/api/NFVITransition/Create`;
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
     * @param {NFVITransitionQueryDto} body
     * @param {string} [propertyName]
     * @param {string} [propertyFilter]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    nFVITransitionGetFilterResult(
      body: NFVITransitionQueryDto,
      propertyName?: string,
      propertyFilter?: string,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling nFVITransitionGetFilterResult."
        );
      }
      const localVarPath = `/api/NFVITransition/Filter`;
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
        <any>"NFVITransitionQueryDto" !== "string" ||
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
     * @param {number} id
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    nFVITransitionGetRelatedRecords(id: number, options: any = {}): FetchArgs {
      // verify required parameter 'id' is not null or undefined
      if (id === null || id === undefined) {
        throw new RequiredError(
          "id",
          "Required parameter id was null or undefined when calling nFVITransitionGetRelatedRecords."
        );
      }
      const localVarPath = `/api/NFVITransition/GetRelatedRecords{id}`.replace(
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
    nFVITransitionGetUpdateResourceVNFTransition(
      id: number,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'id' is not null or undefined
      if (id === null || id === undefined) {
        throw new RequiredError(
          "id",
          "Required parameter id was null or undefined when calling nFVITransitionGetUpdateResourceVNFTransition."
        );
      }
      const localVarPath = `/api/NFVITransition/Update{id}`.replace(
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
     * @param {NFVITransitionQueryDto} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    nFVITransitionGetVNFTransition(
      body: NFVITransitionQueryDto,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling nFVITransitionGetVNFTransition."
        );
      }
      const localVarPath = `/api/NFVITransition/Get`;
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
        <any>"NFVITransitionQueryDto" !== "string" ||
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
     * @param {NFVITransitionDtoUpdate} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    nFVITransitionPut(
      body: NFVITransitionDtoUpdate,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling nFVITransitionPut."
        );
      }
      const localVarPath = `/api/NFVITransition`;
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
        <any>"NFVITransitionDtoUpdate" !== "string" ||
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
    nFVITransitionRestore(id?: number, options: any = {}): FetchArgs {
      const localVarPath = `/api/NFVITransition/Restore`;
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
  };
};

/**
 * NFVITransitionApi - functional programming interface
 * @export
 */
export const NFVITransitionApiFp = function (configuration?: Configuration) {
  return {
    /**
     *
     * @param {NFVITransitionDtoCreate} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    nFVITransitionCreate(
      body: NFVITransitionDtoCreate,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = NFVITransitionApiFetchParamCreator(
        configuration
      ).nFVITransitionCreate(body, options);
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
    nFVITransitionDelete(
      id?: number,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = NFVITransitionApiFetchParamCreator(
        configuration
      ).nFVITransitionDelete(id, options);
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
    nFVITransitionDeleteDeep(
      id?: number,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = NFVITransitionApiFetchParamCreator(
        configuration
      ).nFVITransitionDeleteDeep(id, options);
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
     * @param {NFVITransitionQueryDto} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    nFVITransitionExportReport(
      body: NFVITransitionQueryDto,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ReturnFile> {
      const localVarFetchArgs = NFVITransitionApiFetchParamCreator(
        configuration
      ).nFVITransitionExportReport(body, options);
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
    /**
     *
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    nFVITransitionGetCreateResourceVNFTransition(
      options?: any
    ): (
      fetch?: FetchAPI,
      basePath?: string
    ) => Promise<NFVITransitionDtoCreate> {
      const localVarFetchArgs =
        NFVITransitionApiFetchParamCreator(
          configuration
        ).nFVITransitionGetCreateResourceVNFTransition(options);
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
     * @param {NFVITransitionQueryDto} body
     * @param {string} [propertyName]
     * @param {string} [propertyFilter]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    nFVITransitionGetFilterResult(
      body: NFVITransitionQueryDto,
      propertyName?: string,
      propertyFilter?: string,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<Array<FilterValueDto>> {
      const localVarFetchArgs = NFVITransitionApiFetchParamCreator(
        configuration
      ).nFVITransitionGetFilterResult(
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
     * @param {number} id
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    nFVITransitionGetRelatedRecords(
      id: number,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = NFVITransitionApiFetchParamCreator(
        configuration
      ).nFVITransitionGetRelatedRecords(id, options);
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
    nFVITransitionGetUpdateResourceVNFTransition(
      id: number,
      options?: any
    ): (
      fetch?: FetchAPI,
      basePath?: string
    ) => Promise<NFVITransitionDtoUpdate> {
      const localVarFetchArgs = NFVITransitionApiFetchParamCreator(
        configuration
      ).nFVITransitionGetUpdateResourceVNFTransition(id, options);
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
     * @param {NFVITransitionQueryDto} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    nFVITransitionGetVNFTransition(
      body: NFVITransitionQueryDto,
      options?: any
    ): (
      fetch?: FetchAPI,
      basePath?: string
    ) => Promise<QueryResultDtoOfNFVITransitionDtoGrid> {
      const localVarFetchArgs = NFVITransitionApiFetchParamCreator(
        configuration
      ).nFVITransitionGetVNFTransition(body, options);
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
     * @param {NFVITransitionDtoUpdate} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    nFVITransitionPut(
      body: NFVITransitionDtoUpdate,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = NFVITransitionApiFetchParamCreator(
        configuration
      ).nFVITransitionPut(body, options);
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
    nFVITransitionRestore(
      id?: number,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = NFVITransitionApiFetchParamCreator(
        configuration
      ).nFVITransitionRestore(id, options);
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
 * NFVITransitionApi - factory interface
 * @export
 */
export const NFVITransitionApiFactory = function (
  configuration?: Configuration,
  fetch?: FetchAPI,
  basePath?: string
) {
  return {
    /**
     *
     * @param {NFVITransitionDtoCreate} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    nFVITransitionCreate(body: NFVITransitionDtoCreate, options?: any) {
      return NFVITransitionApiFp(configuration).nFVITransitionCreate(
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
    nFVITransitionDelete(id?: number, options?: any) {
      return NFVITransitionApiFp(configuration).nFVITransitionDelete(
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
    nFVITransitionDeleteDeep(id?: number, options?: any) {
      return NFVITransitionApiFp(configuration).nFVITransitionDeleteDeep(
        id,
        options
      )(fetch, basePath);
    },
    /**
     *
     * @param {NFVITransitionQueryDto} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    nFVITransitionExportReport(body: NFVITransitionQueryDto, options?: any) {
      return NFVITransitionApiFp(configuration).nFVITransitionExportReport(
        body,
        options
      )(fetch, basePath);
    },
    /**
     *
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    nFVITransitionGetCreateResourceVNFTransition(options?: any) {
      return NFVITransitionApiFp(
        configuration
      ).nFVITransitionGetCreateResourceVNFTransition(options)(fetch, basePath);
    },
    /**
     *
     * @param {NFVITransitionQueryDto} body
     * @param {string} [propertyName]
     * @param {string} [propertyFilter]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    nFVITransitionGetFilterResult(
      body: NFVITransitionQueryDto,
      propertyName?: string,
      propertyFilter?: string,
      options?: any
    ) {
      return NFVITransitionApiFp(configuration).nFVITransitionGetFilterResult(
        body,
        propertyName,
        propertyFilter,
        options
      )(fetch, basePath);
    },
    /**
     *
     * @param {number} id
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    nFVITransitionGetRelatedRecords(id: number, options?: any) {
      return NFVITransitionApiFp(configuration).nFVITransitionGetRelatedRecords(
        id,
        options
      )(fetch, basePath);
    },
    /**
     *
     * @param {number} id
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    nFVITransitionGetUpdateResourceVNFTransition(id: number, options?: any) {
      return NFVITransitionApiFp(
        configuration
      ).nFVITransitionGetUpdateResourceVNFTransition(id, options)(
        fetch,
        basePath
      );
    },
    /**
     *
     * @param {NFVITransitionQueryDto} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    nFVITransitionGetVNFTransition(
      body: NFVITransitionQueryDto,
      options?: any
    ) {
      return NFVITransitionApiFp(configuration).nFVITransitionGetVNFTransition(
        body,
        options
      )(fetch, basePath);
    },
    /**
     *
     * @param {NFVITransitionDtoUpdate} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    nFVITransitionPut(body: NFVITransitionDtoUpdate, options?: any) {
      return NFVITransitionApiFp(configuration).nFVITransitionPut(
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
    nFVITransitionRestore(id?: number, options?: any) {
      return NFVITransitionApiFp(configuration).nFVITransitionRestore(
        id,
        options
      )(fetch, basePath);
    },
  };
};

/**
 * NFVITransitionApi - object-oriented interface
 * @export
 * @class NFVITransitionApi
 * @extends {BaseAPI}
 */
export class NFVITransitionApi extends BaseAPI {
  /**
   *
   * @param {NFVITransitionDtoCreate} body
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof NFVITransitionApi
   */
  public nFVITransitionCreate(body: NFVITransitionDtoCreate, options?: any) {
    return NFVITransitionApiFp(this.configuration).nFVITransitionCreate(
      body,
      options
    )(this.fetch, this.basePath);
  }

  /**
   *
   * @param {number} [id]
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof NFVITransitionApi
   */
  public nFVITransitionDelete(id?: number, options?: any) {
    return NFVITransitionApiFp(this.configuration).nFVITransitionDelete(
      id,
      options
    )(this.fetch, this.basePath);
  }

  /**
   *
   * @param {number} [id]
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof NFVITransitionApi
   */
  public nFVITransitionDeleteDeep(id?: number, options?: any) {
    return NFVITransitionApiFp(this.configuration).nFVITransitionDeleteDeep(
      id,
      options
    )(this.fetch, this.basePath);
  }

  /**
   *
   * @param {NFVITransitionQueryDto} body
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof NFVITransitionApi
   */
  public nFVITransitionExportReport(
    body: NFVITransitionQueryDto,
    options?: any
  ) {
    return NFVITransitionApiFp(this.configuration).nFVITransitionExportReport(
      body,
      options
    )(this.fetch, this.basePath);
  }

  /**
   *
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof NFVITransitionApi
   */
  public nFVITransitionGetCreateResourceVNFTransition(options?: any) {
    return NFVITransitionApiFp(
      this.configuration
    ).nFVITransitionGetCreateResourceVNFTransition(options)(
      this.fetch,
      this.basePath
    );
  }

  /**
   *
   * @param {NFVITransitionQueryDto} body
   * @param {string} [propertyName]
   * @param {string} [propertyFilter]
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof NFVITransitionApi
   */
  public nFVITransitionGetFilterResult(
    body: NFVITransitionQueryDto,
    propertyName?: string,
    propertyFilter?: string,
    options?: any
  ) {
    return NFVITransitionApiFp(
      this.configuration
    ).nFVITransitionGetFilterResult(
      body,
      propertyName,
      propertyFilter,
      options
    )(this.fetch, this.basePath);
  }

  /**
   *
   * @param {number} id
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof NFVITransitionApi
   */
  public nFVITransitionGetRelatedRecords(id: number, options?: any) {
    return NFVITransitionApiFp(
      this.configuration
    ).nFVITransitionGetRelatedRecords(id, options)(this.fetch, this.basePath);
  }

  /**
   *
   * @param {number} id
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof NFVITransitionApi
   */
  public nFVITransitionGetUpdateResourceVNFTransition(
    id: number,
    options?: any
  ) {
    return NFVITransitionApiFp(
      this.configuration
    ).nFVITransitionGetUpdateResourceVNFTransition(id, options)(
      this.fetch,
      this.basePath
    );
  }

  /**
   *
   * @param {NFVITransitionQueryDto} body
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof NFVITransitionApi
   */
  public nFVITransitionGetVNFTransition(
    body: NFVITransitionQueryDto,
    options?: any
  ) {
    return NFVITransitionApiFp(
      this.configuration
    ).nFVITransitionGetVNFTransition(body, options)(this.fetch, this.basePath);
  }

  /**
   *
   * @param {NFVITransitionDtoUpdate} body
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof NFVITransitionApi
   */
  public nFVITransitionPut(body: NFVITransitionDtoUpdate, options?: any) {
    return NFVITransitionApiFp(this.configuration).nFVITransitionPut(
      body,
      options
    )(this.fetch, this.basePath);
  }

  /**
   *
   * @param {number} [id]
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof NFVITransitionApi
   */
  public nFVITransitionRestore(id?: number, options?: any) {
    return NFVITransitionApiFp(this.configuration).nFVITransitionRestore(
      id,
      options
    )(this.fetch, this.basePath);
  }
}
