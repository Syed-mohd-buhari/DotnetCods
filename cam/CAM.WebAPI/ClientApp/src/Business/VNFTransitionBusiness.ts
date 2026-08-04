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
import { headerObj } from "./header";

import {
  VnfTransitionDtoCreate,
  VNFTransitionDtoUpdate,
  VNFTransitionDto,
  QueryResultDtoOfVNFTransitionDtoGrid,
} from "../Model/VNFTransition";
import { VNFTransitionQueryObjectGrid as VnfTransitionQueryDto } from "../Model/VNFTransition";
/**
 * VNFTransitionApi - fetch parameter creator
 * @export
 */
export const VNFTransitionApiFetchParamCreator = function (
  configuration?: Configuration
) {
  return {
    /**
     *
     * @param {VnfTransitionDtoCreate} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    vNFTransitionCreate(
      body: VnfTransitionDtoCreate,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling vNFTransitionCreate."
        );
      }
      const localVarPath = `/api/VNFTransition`;
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
        <any>"VnfTransitionDtoCreate" !== "string" ||
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
    vNFTransitionDelete(id?: number, options: any = {}): FetchArgs {
      const localVarPath = `/api/VNFTransition/Delete`;
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
    vNFTransitionDeleteDeep(id?: number, options: any = {}): FetchArgs {
      const localVarPath = `/api/VNFTransition/DeleteDeep`;
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
     * @param {VnfTransitionQueryDto} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    vNFTransitionExportReport(
      body: VnfTransitionQueryDto,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling vNFTransitionExportReport."
        );
      }
      const localVarPath = `/api/VNFTransition/ExportReport`;
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
        <any>"VnfTransitionQueryDto" !== "string" ||
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
    vNFTransitionGetCreateResourceVNFTransition(options: any = {}): FetchArgs {
      const localVarPath = `/api/VNFTransition/Create`;
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
     * @param {VnfTransitionQueryDto} body
     * @param {string} [propertyName]
     * @param {string} [propertyFilter]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    vNFTransitionGetFilterResult(
      body: VnfTransitionQueryDto,
      propertyName?: string,
      propertyFilter?: string,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling vNFTransitionGetFilterResult."
        );
      }
      const localVarPath = `/api/VNFTransition/Filter`;
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
        <any>"VnfTransitionQueryDto" !== "string" ||
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
    vNFTransitionGetRelatedRecords(id: number, options: any = {}): FetchArgs {
      // verify required parameter 'id' is not null or undefined
      if (id === null || id === undefined) {
        throw new RequiredError(
          "id",
          "Required parameter id was null or undefined when calling vNFTransitionGetRelatedRecords."
        );
      }
      const localVarPath = `/api/VNFTransition/GetRelatedRecords{id}`.replace(
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
    vNFTransitionGetUpdateResourceVNFTransition(
      id: number,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'id' is not null or undefined
      if (id === null || id === undefined) {
        throw new RequiredError(
          "id",
          "Required parameter id was null or undefined when calling vNFTransitionGetUpdateResourceVNFTransition."
        );
      }
      const localVarPath = `/api/VNFTransition/Update{id}`.replace(
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
     * @param {VnfTransitionQueryDto} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    vNFTransitionGetVNFTransition(
      body: VnfTransitionQueryDto,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling vNFTransitionGetVNFTransition."
        );
      }
      const localVarPath = `/api/VNFTransition/Get`;
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
        <any>"VnfTransitionQueryDto" !== "string" ||
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
     * @param {VNFTransitionDtoUpdate} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    vNFTransitionPut(
      body: VNFTransitionDtoUpdate,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling vNFTransitionPut."
        );
      }
      const localVarPath = `/api/VNFTransition`;
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
        <any>"VNFTransitionDtoUpdate" !== "string" ||
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
    vNFTransitionRestore(id?: number, options: any = {}): FetchArgs {
      const localVarPath = `/api/VNFTransition/Restore`;
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
 * VNFTransitionApi - functional programming interface
 * @export
 */
export const VNFTransitionApiFp = function (configuration?: Configuration) {
  return {
    /**
     *
     * @param {VnfTransitionDtoCreate} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    vNFTransitionCreate(
      body: VnfTransitionDtoCreate,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = VNFTransitionApiFetchParamCreator(
        configuration
      ).vNFTransitionCreate(body, options);
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
    vNFTransitionDelete(
      id?: number,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = VNFTransitionApiFetchParamCreator(
        configuration
      ).vNFTransitionDelete(id, options);
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
    vNFTransitionDeleteDeep(
      id?: number,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = VNFTransitionApiFetchParamCreator(
        configuration
      ).vNFTransitionDeleteDeep(id, options);
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
     * @param {VnfTransitionQueryDto} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    vNFTransitionExportReport(
      body: VnfTransitionQueryDto,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ReturnFile> {
      const localVarFetchArgs = VNFTransitionApiFetchParamCreator(
        configuration
      ).vNFTransitionExportReport(body, options);
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
    vNFTransitionGetCreateResourceVNFTransition(
      options?: any
    ): (
      fetch?: FetchAPI,
      basePath?: string
    ) => Promise<VnfTransitionDtoCreate> {
      const localVarFetchArgs =
        VNFTransitionApiFetchParamCreator(
          configuration
        ).vNFTransitionGetCreateResourceVNFTransition(options);
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
     * @param {VnfTransitionQueryDto} body
     * @param {string} [propertyName]
     * @param {string} [propertyFilter]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    vNFTransitionGetFilterResult(
      body: VnfTransitionQueryDto,
      propertyName?: string,
      propertyFilter?: string,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<Array<FilterValueDto>> {
      const localVarFetchArgs = VNFTransitionApiFetchParamCreator(
        configuration
      ).vNFTransitionGetFilterResult(
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
    vNFTransitionGetRelatedRecords(
      id: number,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = VNFTransitionApiFetchParamCreator(
        configuration
      ).vNFTransitionGetRelatedRecords(id, options);
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
    vNFTransitionGetUpdateResourceVNFTransition(
      id: number,
      options?: any
    ): (
      fetch?: FetchAPI,
      basePath?: string
    ) => Promise<VNFTransitionDtoUpdate> {
      const localVarFetchArgs = VNFTransitionApiFetchParamCreator(
        configuration
      ).vNFTransitionGetUpdateResourceVNFTransition(id, options);
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
     * @param {VnfTransitionQueryDto} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    vNFTransitionGetVNFTransition(
      body: VnfTransitionQueryDto,
      options?: any
    ): (
      fetch?: FetchAPI,
      basePath?: string
    ) => Promise<QueryResultDtoOfVNFTransitionDtoGrid> {
      const localVarFetchArgs = VNFTransitionApiFetchParamCreator(
        configuration
      ).vNFTransitionGetVNFTransition(body, options);
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
     * @param {VNFTransitionDtoUpdate} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    vNFTransitionPut(
      body: VNFTransitionDtoUpdate,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = VNFTransitionApiFetchParamCreator(
        configuration
      ).vNFTransitionPut(body, options);
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
    vNFTransitionRestore(
      id?: number,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = VNFTransitionApiFetchParamCreator(
        configuration
      ).vNFTransitionRestore(id, options);
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
 * VNFTransitionApi - factory interface
 * @export
 */
export const VNFTransitionApiFactory = function (
  configuration?: Configuration,
  fetch?: FetchAPI,
  basePath?: string
) {
  return {
    /**
     *
     * @param {VnfTransitionDtoCreate} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    vNFTransitionCreate(body: VnfTransitionDtoCreate, options?: any) {
      return VNFTransitionApiFp(configuration).vNFTransitionCreate(
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
    vNFTransitionDelete(id?: number, options?: any) {
      return VNFTransitionApiFp(configuration).vNFTransitionDelete(id, options)(
        fetch,
        basePath
      );
    },
    /**
     *
     * @param {number} [id]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    vNFTransitionDeleteDeep(id?: number, options?: any) {
      return VNFTransitionApiFp(configuration).vNFTransitionDeleteDeep(
        id,
        options
      )(fetch, basePath);
    },
    /**
     *
     * @param {VnfTransitionQueryDto} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    vNFTransitionExportReport(body: VnfTransitionQueryDto, options?: any) {
      return VNFTransitionApiFp(configuration).vNFTransitionExportReport(
        body,
        options
      )(fetch, basePath);
    },
    /**
     *
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    vNFTransitionGetCreateResourceVNFTransition(options?: any) {
      return VNFTransitionApiFp(
        configuration
      ).vNFTransitionGetCreateResourceVNFTransition(options)(fetch, basePath);
    },
    /**
     *
     * @param {VnfTransitionQueryDto} body
     * @param {string} [propertyName]
     * @param {string} [propertyFilter]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    vNFTransitionGetFilterResult(
      body: VnfTransitionQueryDto,
      propertyName?: string,
      propertyFilter?: string,
      options?: any
    ) {
      return VNFTransitionApiFp(configuration).vNFTransitionGetFilterResult(
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
    vNFTransitionGetRelatedRecords(id: number, options?: any) {
      return VNFTransitionApiFp(configuration).vNFTransitionGetRelatedRecords(
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
    vNFTransitionGetUpdateResourceVNFTransition(id: number, options?: any) {
      return VNFTransitionApiFp(
        configuration
      ).vNFTransitionGetUpdateResourceVNFTransition(id, options)(
        fetch,
        basePath
      );
    },
    /**
     *
     * @param {VnfTransitionQueryDto} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    vNFTransitionGetVNFTransition(body: VnfTransitionQueryDto, options?: any) {
      return VNFTransitionApiFp(configuration).vNFTransitionGetVNFTransition(
        body,
        options
      )(fetch, basePath);
    },
    /**
     *
     * @param {VNFTransitionDtoUpdate} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    vNFTransitionPut(body: VNFTransitionDtoUpdate, options?: any) {
      return VNFTransitionApiFp(configuration).vNFTransitionPut(body, options)(
        fetch,
        basePath
      );
    },
    /**
     *
     * @param {number} [id]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    vNFTransitionRestore(id?: number, options?: any) {
      return VNFTransitionApiFp(configuration).vNFTransitionRestore(
        id,
        options
      )(fetch, basePath);
    },
  };
};

/**
 * VNFTransitionApi - object-oriented interface
 * @export
 * @class VNFTransitionApi
 * @extends {BaseAPI}
 */
export class VNFTransitionApi extends BaseAPI {
  /**
   *
   * @param {VnfTransitionDtoCreate} body
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof VNFTransitionApi
   */
  public vNFTransitionCreate(body: VnfTransitionDtoCreate, options?: any) {
    return VNFTransitionApiFp(this.configuration).vNFTransitionCreate(
      body,
      options
    )(this.fetch, this.basePath);
  }

  /**
   *
   * @param {number} [id]
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof VNFTransitionApi
   */
  public vNFTransitionDelete(id?: number, options?: any) {
    return VNFTransitionApiFp(this.configuration).vNFTransitionDelete(
      id,
      options
    )(this.fetch, this.basePath);
  }

  /**
   *
   * @param {number} [id]
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof VNFTransitionApi
   */
  public vNFTransitionDeleteDeep(id?: number, options?: any) {
    return VNFTransitionApiFp(this.configuration).vNFTransitionDeleteDeep(
      id,
      options
    )(this.fetch, this.basePath);
  }

  /**
   *
   * @param {VnfTransitionQueryDto} body
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof VNFTransitionApi
   */
  public vNFTransitionExportReport(body: VnfTransitionQueryDto, options?: any) {
    return VNFTransitionApiFp(this.configuration).vNFTransitionExportReport(
      body,
      options
    )(this.fetch, this.basePath);
  }

  /**
   *
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof VNFTransitionApi
   */
  public vNFTransitionGetCreateResourceVNFTransition(options?: any) {
    return VNFTransitionApiFp(
      this.configuration
    ).vNFTransitionGetCreateResourceVNFTransition(options)(
      this.fetch,
      this.basePath
    );
  }

  /**
   *
   * @param {VnfTransitionQueryDto} body
   * @param {string} [propertyName]
   * @param {string} [propertyFilter]
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof VNFTransitionApi
   */
  public vNFTransitionGetFilterResult(
    body: VnfTransitionQueryDto,
    propertyName?: string,
    propertyFilter?: string,
    options?: any
  ) {
    return VNFTransitionApiFp(this.configuration).vNFTransitionGetFilterResult(
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
   * @memberof VNFTransitionApi
   */
  public vNFTransitionGetRelatedRecords(id: number, options?: any) {
    return VNFTransitionApiFp(
      this.configuration
    ).vNFTransitionGetRelatedRecords(id, options)(this.fetch, this.basePath);
  }

  /**
   *
   * @param {number} id
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof VNFTransitionApi
   */
  public vNFTransitionGetUpdateResourceVNFTransition(
    id: number,
    options?: any
  ) {
    return VNFTransitionApiFp(
      this.configuration
    ).vNFTransitionGetUpdateResourceVNFTransition(id, options)(
      this.fetch,
      this.basePath
    );
  }

  /**
   *
   * @param {VnfTransitionQueryDto} body
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof VNFTransitionApi
   */
  public vNFTransitionGetVNFTransition(
    body: VnfTransitionQueryDto,
    options?: any
  ) {
    return VNFTransitionApiFp(this.configuration).vNFTransitionGetVNFTransition(
      body,
      options
    )(this.fetch, this.basePath);
  }

  /**
   *
   * @param {VNFTransitionDtoUpdate} body
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof VNFTransitionApi
   */
  public vNFTransitionPut(body: VNFTransitionDtoUpdate, options?: any) {
    return VNFTransitionApiFp(this.configuration).vNFTransitionPut(
      body,
      options
    )(this.fetch, this.basePath);
  }

  /**
   *
   * @param {number} [id]
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof VNFTransitionApi
   */
  public vNFTransitionRestore(id?: number, options?: any) {
    return VNFTransitionApiFp(this.configuration).vNFTransitionRestore(
      id,
      options
    )(this.fetch, this.basePath);
  }
}
