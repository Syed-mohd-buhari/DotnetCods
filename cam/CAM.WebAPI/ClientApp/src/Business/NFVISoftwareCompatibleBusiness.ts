import * as url from "url";
import * as isomorphicFetch from "isomorphic-fetch";
import { Configuration } from "./Common/configuration";
import { ResultDto } from "../Model/CommonModels";
import { headerObj } from "./header";

import {
  BASE_PATH,
  FetchArgs,
  FetchAPI,
  BaseAPI,
  RequiredError,
  FilterValueDto,
} from "./Common/CommonBusiness";
import {
  QueryResultDtoOfNFVISwCompatibleDtoGrid,
  NFVISwCompatibleDtoCreate,
  NFVISwCompatibleDtoUpdate,
  NFVISwCompatibleQueryDto,
  NFVISwCompatibleQueryObjectGrid,
} from "../Model/NFVISoftwareCompatible";
import { ReturnFile } from "../Model/Common";
/**
 * NFVISwCompatibleApi - fetch parameter creator
 * @export
 */
export const NFVISwCompatibleApiFetchParamCreator = function (
  configuration?: Configuration
) {
  return {
    /**
     * @param {NFVISwCompatibleQueryObjectGrid} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    NFVISoftwareCompatibleGetGrid(
      body: NFVISwCompatibleQueryObjectGrid,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling NFVISwCompatibleQueryObjectGrid."
        );
      }
      const localVarPath = `/api/NfviSoftwareCompatibility/GetNfviSoftwareCompatibility`;
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
        <any>"NFVISwCompatibleQueryObjectGrid" !== "string" ||
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
    nfviSwCompatibleGetCreateResourceNFVISwCompatible(
      options: any = {}
    ): FetchArgs {
      const localVarPath = `/api/NfviSoftwareCompatibility/CreateNfviSoftwareCompatibility`;
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
     * @param {number} id
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    nfviSwCompatibleGetUpdateResourceNFVISwCompatible(
      id: number,
      options: any = {}
    ): FetchArgs {
      if (id === null || id === undefined) {
        throw new RequiredError(
          "id",
          "Required parameter id was null or undefined when calling nfviSwCompatibleGetUpdateResourceNFVISwCompatible."
        );
      }
      const localVarPath =
        `/api/NfviSoftwareCompatibility/EditNfviSoftwareCompatibility{id}`.replace(
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
     * @param {NFVISwCompatibleDtoUpdate} body
     * @param {boolean} [forced]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    NFVISwCompatibleEdit(
      body: NFVISwCompatibleDtoUpdate,
      forced?: boolean,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling NFVISwCompatibleEdit."
        );
      }
      const localVarPath = `/api/NfviSoftwareCompatibility/CreateOrUpdateNfviSoftwareCompat`;
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
        <any>"NFVISwCompatibleDtoUpdate" !== "string" ||
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
     * @param {NFVISwCompatibleDtoCreate} body
     * @param {boolean} [forced]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    NFVISwCompatibleImportExcel(file: File, options: any = {}): FetchArgs {
      if (!file) {
        throw new RequiredError(
          "file",
          "Required parameter file was null or undefined when calling NFVISwCompatibleImportExcel."
        );
      }

      const localVarPath = `/api/NfviSoftwareCompatibility/ImportReport`;
      const localVarUrlObj = url.parse(localVarPath, true);
      const localVarRequestOptions = Object.assign({ method: "POST" }, options);
      const localVarHeaderParameter = { ...headerObj } as any;

      // Authentication
      if (configuration && configuration.apiKey) {
        const localVarApiKeyValue =
          typeof configuration.apiKey === "function"
            ? configuration.apiKey("Authorization")
            : configuration.apiKey;
        localVarHeaderParameter["Authorization"] = localVarApiKeyValue;
      }

      // Set up FormData
      const formData = new FormData();
      formData.append("file", file);

      localVarUrlObj.query = Object.assign(
        {},
        localVarUrlObj.query,
        options.query
      );
      localVarUrlObj.search = null;

      localVarRequestOptions.headers = Object.assign(
        {},
        localVarHeaderParameter,
        options.headers
      );
      localVarRequestOptions.body = formData;

      return {
        url: url.format(localVarUrlObj),
        options: localVarRequestOptions,
      };
    },

    NFVISwCompatibleCreate(
      body: NFVISwCompatibleDtoCreate,
      forced?: boolean,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling NFVISwCompatibleCreate."
        );
      }
      const localVarPath = `/api/NfviSoftwareCompatibility/CreateOrUpdateNfviSoftwareCompat`;
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
        <any>"NFVISwCompatibleDtoCreate" !== "string" ||
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
    NFVISwCompatibleDelete(id: number, options: any = {}): FetchArgs {
      const localVarPath = `/api/NfviSoftwareCompatibility/DeleteNfviSoftware`;
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
     * @param {NFVISwCompatibleQueryObjectGrid} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    NFVISwCompatibleExportReport(
      body: NFVISwCompatibleQueryObjectGrid,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling NFVISwCompatibleExportReport."
        );
      }
      const localVarPath = `/api/NfviSoftwareCompatibility/ExportReport`;
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
        <any>"NFVISwCompatibleQueryObjectGrid" !== "string" ||
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
     * @param {NFVISwCompatibleQueryObjectGrid} body
     * @param {string} [propertyName]
     * @param {string} [propertyFilter]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    NFVISwCompatibleGetFilterResult(
      body: NFVISwCompatibleQueryObjectGrid,
      propertyName?: string,
      propertyFilter?: string,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling NFVISwCompatibleGetFilterResult."
        );
      }
      const localVarPath = `/api/NfviSoftwareCompatibility/Filter`;
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
        <any>"NFVISwCompatibleQueryObjectGrid" !== "string" ||
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
    testInfoRestore(id?: number, options: any = {}): FetchArgs {
      const localVarPath = `/api/SystemVerificationProblem/Restore`;
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
 * NFVISwCompatibleApi - functional programming interface
 * @export
 */
export const nfviSwCompatibleApiFp = function (configuration?: Configuration) {
  return {
    /**
     *
     * @param {number} [id]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    NFVISwCompatibleDelete(
      id: number,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = NFVISwCompatibleApiFetchParamCreator(
        configuration
      ).NFVISwCompatibleDelete(id, options);
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

    NFVISwCompatibleImportExcel(
      file: File,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = NFVISwCompatibleApiFetchParamCreator(
        configuration
      ).NFVISwCompatibleImportExcel(file, options);

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
     * @param {NFVISwCompatibleQueryObjectGrid} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    NFVISwCompatibleExportReport(
      body: NFVISwCompatibleQueryObjectGrid,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ReturnFile> {
      const localVarFetchArgs = NFVISwCompatibleApiFetchParamCreator(
        configuration
      ).NFVISwCompatibleExportReport(body, options);
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
    nfviSwCompatibleGetCreateResourceNFVISwCompatible(
      options?: any
    ): (
      fetch?: FetchAPI,
      basePath?: string
    ) => Promise<NFVISwCompatibleDtoCreate> {
      const localVarFetchArgs =
        NFVISwCompatibleApiFetchParamCreator(
          configuration
        ).nfviSwCompatibleGetCreateResourceNFVISwCompatible(options);
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
     * @param {number} id
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    nfviSwCompatibleGetUpdateResourceNFVISwCompatible(
      id: number,
      options?: any
    ): (
      fetch?: FetchAPI,
      basePath?: string
    ) => Promise<NFVISwCompatibleDtoUpdate> {
      const localVarFetchArgs = NFVISwCompatibleApiFetchParamCreator(
        configuration
      ).nfviSwCompatibleGetUpdateResourceNFVISwCompatible(id, options);
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
     * @param {NFVISwCompatibleQueryObjectGrid} body
     * @param {string} [propertyName]
     * @param {string} [propertyFilter]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    NFVISwCompatibleGetFilterResult(
      body: NFVISwCompatibleQueryObjectGrid,
      propertyName?: string,
      propertyFilter?: string,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<Array<FilterValueDto>> {
      const localVarFetchArgs = NFVISwCompatibleApiFetchParamCreator(
        configuration
      ).NFVISwCompatibleGetFilterResult(
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
     * @param {NFVISwCompatibleQueryObjectGrid} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    NFVISoftwareCompatibleGetGrid(
      body: NFVISwCompatibleQueryObjectGrid,
      options?: any
    ): (
      fetch?: FetchAPI,
      basePath?: string
    ) => Promise<QueryResultDtoOfNFVISwCompatibleDtoGrid> {
      const localVarFetchArgs = NFVISwCompatibleApiFetchParamCreator(
        configuration
      ).NFVISoftwareCompatibleGetGrid(body, options);
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
     * @param {NFVISwCompatibleDtoUpdate} body
     * @param {boolean} [forced]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    NFVISwCompatibleEdit(
      body: NFVISwCompatibleDtoUpdate,
      forced?: boolean,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = NFVISwCompatibleApiFetchParamCreator(
        configuration
      ).NFVISwCompatibleEdit(body, forced, options);
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
     * @param {NFVISwCompatibleDtoCreate} body
     * @param {boolean} [forced]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    NFVISwCompatibleCreate(
      body: NFVISwCompatibleDtoCreate,
      forced?: boolean,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = NFVISwCompatibleApiFetchParamCreator(
        configuration
      ).NFVISwCompatibleCreate(body, forced, options);
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
    testInfoRestore(
      id?: number,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = NFVISwCompatibleApiFetchParamCreator(
        configuration
      ).testInfoRestore(id, options);
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
 * NFVISwCompatibleApi - factory interface
 * @export
 */
export const NFVISwCompatibleApiFactory = function (
  configuration?: Configuration,
  fetch?: FetchAPI,
  basePath?: string
) {
  return {
    /**
     *
     * @param {number} [id]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    NFVISwCompatibleDelete(id: number, options?: any) {
      return nfviSwCompatibleApiFp(configuration).NFVISwCompatibleDelete(
        id,
        options
      )(fetch, basePath);
    },
    /**
     *
     * @param {NFVISwCompatibleQueryObjectGrid} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    NFVISwCompatibleExportReport(
      body: NFVISwCompatibleQueryObjectGrid,
      options?: any
    ) {
      return nfviSwCompatibleApiFp(configuration).NFVISwCompatibleExportReport(
        body,
        options
      )(fetch, basePath);
    },
    NFVISwCompatibleImportExcel(file: File, options?: any): Promise<ResultDto> {
      return nfviSwCompatibleApiFp(configuration).NFVISwCompatibleImportExcel(
        file,
        options
      )(fetch, basePath);
    },

    /**
     *
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    nfviSwCompatibleGetCreateResourceNFVISwCompatible(options?: any) {
      return nfviSwCompatibleApiFp(
        configuration
      ).nfviSwCompatibleGetCreateResourceNFVISwCompatible(options)(
        fetch,
        basePath
      );
    },
    /**
     * @param {number} id
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    nfviSwCompatibleGetUpdateResourceNFVISwCompatible(
      id: number,
      options?: any
    ) {
      return nfviSwCompatibleApiFp(
        configuration
      ).nfviSwCompatibleGetUpdateResourceNFVISwCompatible(id, options)(
        fetch,
        basePath
      );
    },
    /**
     *
     * @param {NFVISwCompatibleQueryObjectGrid} body
     * @param {string} [propertyName]
     * @param {string} [propertyFilter]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    NFVISwCompatibleGetFilterResult(
      body: NFVISwCompatibleQueryObjectGrid,
      propertyName?: string,
      propertyFilter?: string,
      options?: any
    ) {
      return nfviSwCompatibleApiFp(
        configuration
      ).NFVISwCompatibleGetFilterResult(
        body,
        propertyName,
        propertyFilter,
        options
      )(fetch, basePath);
    },
    /**
     * @param {NFVISwCompatibleQueryObjectGrid} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    NFVISoftwareCompatibleGetGrid(
      body: NFVISwCompatibleQueryObjectGrid,
      options?: any
    ) {
      return nfviSwCompatibleApiFp(configuration).NFVISoftwareCompatibleGetGrid(
        body,
        options
      )(fetch, basePath);
    },
    /**
     *
     * @param {NFVISwCompatibleDtoUpdate} body
     * @param {boolean} [forced]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    NFVISwCompatibleEdit(
      body: NFVISwCompatibleDtoUpdate,
      forced?: boolean,
      options?: any
    ) {
      return nfviSwCompatibleApiFp(configuration).NFVISwCompatibleEdit(
        body,
        forced,
        options
      )(fetch, basePath);
    },
    /**
     *
     * @param {NFVISwCompatibleDtoCreate} body
     * @param {boolean} [forced]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    NFVISwCompatibleCreate(
      body: NFVISwCompatibleDtoCreate,
      forced?: boolean,
      options?: any
    ) {
      return nfviSwCompatibleApiFp(configuration).NFVISwCompatibleCreate(
        body,
        forced,
        options
      )(fetch, basePath);
    },
    /**
     *
     * @param {number} [id]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    testInfoRestore(id?: number, options?: any) {
      return nfviSwCompatibleApiFp(configuration).testInfoRestore(id, options)(
        fetch,
        basePath
      );
    },
  };
};

/**
 * NFVISwCompatibleApi - object-oriented interface
 * @export
 * @class NFVISwCompatibleApi
 * @extends {BaseAPI}
 */
export class NFVISwCompatibleApi extends BaseAPI {
  /**
   *
   * @param {number} [id]
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof NFVISwCompatibleApi
   */
  public NFVISwCompatibleDelete(id: number, options?: any) {
    return nfviSwCompatibleApiFp(this.configuration).NFVISwCompatibleDelete(
      id,
      options
    )(this.fetch, this.basePath);
  }

  /**
   *
   * @param {NFVISwCompatibleQueryObjectGrid} body
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof NFVISwCompatibleApi
   */
  public NFVISwCompatibleExportReport(
    body: NFVISwCompatibleQueryObjectGrid,
    options?: any
  ) {
    return nfviSwCompatibleApiFp(
      this.configuration
    ).NFVISwCompatibleExportReport(body, options)(this.fetch, this.basePath);
  }

  public async nfvicImportStatus(file: File) {
    const fetchArgs = NFVISwCompatibleApiFetchParamCreator(
      this.configuration
    ).NFVISwCompatibleImportExcel(file);

    return this.fetch(this.basePath + fetchArgs.url, fetchArgs.options).then(
      async (response) => {
        if (response.ok) return response.json(); // adjust to .text() if response is plain text
        throw response;
      }
    );
  }

  /**
   *
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof NFVISwCompatibleApi
   */
  public nfviSwCompatibleGetCreateResourceNFVISwCompatible(options?: any) {
    return nfviSwCompatibleApiFp(
      this.configuration
    ).nfviSwCompatibleGetCreateResourceNFVISwCompatible(options)(
      this.fetch,
      this.basePath
    );
  }
  /**
   * @param {number} id
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof NFVISwCompatibleApi
   */
  public nfviSwCompatibleGetUpdateResourceNFVISwCompatible(
    id: number,
    options?: any
  ) {
    return nfviSwCompatibleApiFp(
      this.configuration
    ).nfviSwCompatibleGetUpdateResourceNFVISwCompatible(id, options)(
      this.fetch,
      this.basePath
    );
  }

  /**
   *
   * @param {NFVISwCompatibleQueryObjectGrid} body
   * @param {string} [propertyName]
   * @param {string} [propertyFilter]
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof NFVISwCompatibleApi
   */
  public NFVISwCompatibleGetFilterResult(
    body: NFVISwCompatibleQueryObjectGrid,
    propertyName?: string,
    propertyFilter?: string,
    options?: any
  ) {
    return nfviSwCompatibleApiFp(
      this.configuration
    ).NFVISwCompatibleGetFilterResult(
      body,
      propertyName,
      propertyFilter,
      options
    )(this.fetch, this.basePath);
  }

  /**
   *
   * @param {NFVISwCompatibleQueryObjectGrid} body
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof NFVISwCompatibleApi
   */
  public NFVISoftwareCompatibleGetGrid(
    body: NFVISwCompatibleQueryObjectGrid,
    options?: any
  ) {
    return nfviSwCompatibleApiFp(
      this.configuration
    ).NFVISoftwareCompatibleGetGrid(body, options)(this.fetch, this.basePath);
  }

  /**
   *
   * @param {NFVISwCompatibleDtoUpdate} body
   * @param {boolean} [forced]
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof NFVISwCompatibleApi
   */
  public NFVISwCompatibleEdit(
    body: NFVISwCompatibleDtoUpdate,
    forced?: boolean,
    options?: any
  ) {
    return nfviSwCompatibleApiFp(this.configuration).NFVISwCompatibleEdit(
      body,
      forced,
      options
    )(this.fetch, this.basePath);
  }

  /**
   *
   * @param {NFVISwCompatibleDtoCreate} body
   * @param {boolean} [forced]
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof NFVISwCompatibleApi
   */
  public NFVISwCompatibleCreate(
    body: NFVISwCompatibleDtoCreate,
    forced?: boolean,
    options?: any
  ) {
    return nfviSwCompatibleApiFp(this.configuration).NFVISwCompatibleCreate(
      body,
      forced,
      options
    )(this.fetch, this.basePath);
  }

  /**
   *
   * @param {number} [id]
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof NFVISwCompatibleApi
   */
  public testInfoRestore(id?: number, options?: any) {
    return nfviSwCompatibleApiFp(this.configuration).testInfoRestore(
      id,
      options
    )(this.fetch, this.basePath);
  }
}
