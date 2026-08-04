import * as url from "url";
import * as portableFetch from "portable-fetch";
import * as isomorphicFetch from "isomorphic-fetch";

import { Configuration } from "./Common/configuration";
import {
  DataRemediationDto,
  ResultDto,
  ResultDtoOfResultDataRemediationDto,
} from "../Model/CommonModels";
import {
  BASE_PATH,
  FetchArgs,
  FetchAPI,
  RequiredError,
  FilterValueDto,
} from "./Common/CommonBusiness";

import { BaseAPI } from "./Common/CommonBusiness";
import { ReturnFile, FileResult, getResourceObject } from "../Model/Common";
import { headerObj } from "./header";
import {
  IdentityAsIsDtoCreate,
  IdentityAsIsDtoUpdate,
  IdentityAsIsQueryDto,
  QueryResultDtoOfIdentityAsIsDtoGrid,
} from "../Model/LookUp/Identities";

/**
 * IdentityAsIsApi - fetch parameter creator
 * @export
 */
export const IdentityAsIsApiFetchParamCreator = function (
  configuration?: Configuration
) {
  return {
    /**
     *
     * @param {IdentityAsIsDtoCreate} body
     * @param {boolean} [forced]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    IdentityAsIsCreate(
      body: IdentityAsIsDtoCreate,
      forced?: boolean,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling IdentityAsIsCreate."
        );
      }
      const localVarPath = `/api/IdentityAsIs`;
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
        <any>"IdentityAsIsDtoCreate" !== "string" ||
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
     * @param {File} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    identityAsIsImportStatus(body: File, options: any = {}): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      // const blob = new Blob([body], { type: body.type });
      const formData = new FormData();
      formData.append("file", body);
      console.log(formData);
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling identityAsIsImportStatus."
        );
      }
      const localVarPath = `/api/IdentityAsIs/ImportReport`;
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
      localVarRequestOptions.body = formData;
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
    IdentityAsIsDelete(id?: number, options: any = {}): FetchArgs {
      const localVarPath = `/api/IdentityAsIs/Delete`;
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

    GetAssetsByOpcoIdAndDcfId(
      opcoId?: number,
      dcfId?: number,

      options: any = {}
    ): FetchArgs {
      const localVarPath = `/api/NetworkElementAsPlanned/GetAssetsByOpcoIdAndDcfId`;
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

      if (opcoId !== undefined) {
        localVarQueryParameter["opcoId"] = opcoId;
      }
      if (dcfId !== undefined) {
        localVarQueryParameter["dcfId"] = dcfId;
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
    IdentityAsIsDeleteDeep(id?: number, options: any = {}): FetchArgs {
      const localVarPath = `/api/IdentityAsIs/DeleteDeep`;
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
     * @param {IdentityAsIsQueryDto} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    IdentityAsIsExportReport(
      body: IdentityAsIsQueryDto,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling IdentityAsIsExportReport."
        );
      }
      const localVarPath = `/api/IdentityAsIs/ExportReport`;
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
        <any>"IdentityAsIsQueryDto" !== "string" ||
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
    IdentityAsIsGetCreateResourceIdentityAsIs(options: any = {}): FetchArgs {
      const localVarPath = `/api/IdentityAsIs/Create`;
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
     * @param {IdentityAsIsQueryDto} body
     * @param {string} [propertyName]
     * @param {string} [propertyFilter]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    IdentityAsIsGetFilterResult(
      body: IdentityAsIsQueryDto,
      propertyName?: string,
      propertyFilter?: string,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling IdentityAsIsGetFilterResult."
        );
      }
      const localVarPath = `/api/IdentityAsIs/Filter`;
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
        <any>"IdentityAsIsQueryDto" !== "string" ||
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
     * @param {IdentityAsIsQueryDto} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    IdentityAsIsGetIdentityAsIs(
      body: IdentityAsIsQueryDto,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling IdentityAsIsGetIdentityAsIs."
        );
      }
      const localVarPath = `/api/IdentityAsIs/Get`;
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
        <any>"IdentityAsIsQueryDto" !== "string" ||
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
    IdentityAsIsGetUpdateResourceIdentityAsIs(
      id: number,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'id' is not null or undefined
      if (id === null || id === undefined) {
        throw new RequiredError(
          "id",
          "Required parameter id was null or undefined when calling IdentityAsIsGetUpdateResourceIdentityAsIs."
        );
      }
      const localVarPath = `/api/IdentityAsIs/Update{id}`.replace(
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
     * @param {IdentityAsIsDtoUpdate} body
     * @param {boolean} [forced]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    IdentityAsIsPut(
      body: IdentityAsIsDtoUpdate,
      forced?: boolean,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling IdentityAsIsPut."
        );
      }
      const localVarPath = `/api/IdentityAsIs`;
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
        <any>"IdentityAsIsDtoUpdate" !== "string" ||
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
    IdentityAsIsRestore(id?: number, options: any = {}): FetchArgs {
      const localVarPath = `/api/IdentityAsIs/Restore`;
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
    /**
     *@param {getResourceObject} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    getResourceKeyForIdentityAsIs(
      body: getResourceObject,
      options: any = {}
    ): FetchArgs {
      const localVarPath = `/api/IdentityAsIs/GenerateNewIdentityRandomResourceKey`;
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
      const needsSerialization =
        <any>"LcmEngineeringQueryDto" !== "string" ||
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
 * IdentityAsIsApi - functional programming interface
 * @export
 */
export const IdentityAsIsApiFp = function (configuration?: Configuration) {
  return {
    /**
     *
     * @param {IdentityAsIsDtoCreate} body
     * @param {boolean} [forced]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    IdentityAsIsCreate(
      body: IdentityAsIsDtoCreate,
      forced?: boolean,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = IdentityAsIsApiFetchParamCreator(
        configuration
      ).IdentityAsIsCreate(body, forced, options);
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

    GetAssetsByOpcoIdAndDcfId(
      opcoId?: number,
      dcfId?: number,
      options?: any
    ): (
      fetch?: FetchAPI,
      basePath?: string
    ) => Promise<{ [key: string]: string }> {
      const localVarFetchArgs = IdentityAsIsApiFetchParamCreator(
        configuration
      ).GetAssetsByOpcoIdAndDcfId(opcoId, dcfId, options);
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
     * @param {File} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    identityAsIsImportStatus(
      body: File,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<boolean> {
      const localVarFetchArgs = IdentityAsIsApiFetchParamCreator(
        configuration
      ).identityAsIsImportStatus(body, options);
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
    IdentityAsIsDelete(
      id?: number,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = IdentityAsIsApiFetchParamCreator(
        configuration
      ).IdentityAsIsDelete(id, options);
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
    IdentityAsIsDeleteDeep(
      id?: number,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = IdentityAsIsApiFetchParamCreator(
        configuration
      ).IdentityAsIsDeleteDeep(id, options);
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
     * @param {IdentityAsIsQueryDto} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    IdentityAsIsExportReport(
      body: IdentityAsIsQueryDto,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ReturnFile> {
      const localVarFetchArgs = IdentityAsIsApiFetchParamCreator(
        configuration
      ).IdentityAsIsExportReport(body, options);
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
    IdentityAsIsGetCreateResourceIdentityAsIs(
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<IdentityAsIsDtoCreate> {
      const localVarFetchArgs =
        IdentityAsIsApiFetchParamCreator(
          configuration
        ).IdentityAsIsGetCreateResourceIdentityAsIs(options);
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
     * @param {IdentityAsIsQueryDto} body
     * @param {string} [propertyName]
     * @param {string} [propertyFilter]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    IdentityAsIsGetFilterResult(
      body: IdentityAsIsQueryDto,
      propertyName?: string,
      propertyFilter?: string,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<Array<FilterValueDto>> {
      const localVarFetchArgs = IdentityAsIsApiFetchParamCreator(
        configuration
      ).IdentityAsIsGetFilterResult(
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
     * @param {IdentityAsIsQueryDto} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    IdentityAsIsGetIdentityAsIs(
      body: IdentityAsIsQueryDto,
      options?: any
    ): (
      fetch?: FetchAPI,
      basePath?: string
    ) => Promise<QueryResultDtoOfIdentityAsIsDtoGrid> {
      const localVarFetchArgs = IdentityAsIsApiFetchParamCreator(
        configuration
      ).IdentityAsIsGetIdentityAsIs(body, options);
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
    IdentityAsIsGetUpdateResourceIdentityAsIs(
      id: number,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<IdentityAsIsDtoUpdate> {
      const localVarFetchArgs = IdentityAsIsApiFetchParamCreator(
        configuration
      ).IdentityAsIsGetUpdateResourceIdentityAsIs(id, options);
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
     * @param {IdentityAsIsDtoUpdate} body
     * @param {boolean} [forced]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    IdentityAsIsPut(
      body: IdentityAsIsDtoUpdate,
      forced?: boolean,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = IdentityAsIsApiFetchParamCreator(
        configuration
      ).IdentityAsIsPut(body, forced, options);
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
    IdentityAsIsRestore(
      id?: number,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = IdentityAsIsApiFetchParamCreator(
        configuration
      ).IdentityAsIsRestore(id, options);
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
     * @param {getResourceObject} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    getResourceKeyForIdentityAsIs(
      body: getResourceObject,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = IdentityAsIsApiFetchParamCreator(
        configuration
      ).getResourceKeyForIdentityAsIs(body, options);
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
 *IdentityAsIsApi - factory interface
 * @export
 */
export const IdentityAsIsApiFactory = function (
  configuration?: Configuration,
  fetch?: FetchAPI,
  basePath?: string
) {
  return {
    /**
     *
     * @param {IdentityAsIsDtoCreate} body
     * @param {boolean} [forced]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    IdentityAsIsCreate(
      body: IdentityAsIsDtoCreate,
      forced?: boolean,
      options?: any
    ) {
      return IdentityAsIsApiFp(configuration).IdentityAsIsCreate(
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

    GetAssetsByOpcoIdAndDcfId(
      opcoId?: number,
      dcfId?: number,

      options?: any
    ) {
      return IdentityAsIsApiFp(configuration).GetAssetsByOpcoIdAndDcfId(
        opcoId,
        dcfId,
        options
      )(fetch, basePath);
    },

    /**
     *
     * @param {File} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    identityAsIsImportStatus(body: File, options?: any) {
      return IdentityAsIsApiFp(configuration).identityAsIsImportStatus(
        body,
        options
      )(fetch, basePath);
    },
    IdentityAsIsDelete(id?: number, options?: any) {
      return IdentityAsIsApiFp(configuration).IdentityAsIsDelete(id, options)(
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
    IdentityAsIsDeleteDeep(id?: number, options?: any) {
      return IdentityAsIsApiFp(configuration).IdentityAsIsDeleteDeep(
        id,
        options
      )(fetch, basePath);
    },
    /**
     *
     * @param {IdentityAsIsQueryDto} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    IdentityAsIsExportReport(body: IdentityAsIsQueryDto, options?: any) {
      return IdentityAsIsApiFp(configuration).IdentityAsIsExportReport(
        body,
        options
      )(fetch, basePath);
    },
    /**
     *
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    IdentityAsIsGetCreateResourceIdentityAsIs(options?: any) {
      return IdentityAsIsApiFp(
        configuration
      ).IdentityAsIsGetCreateResourceIdentityAsIs(options)(fetch, basePath);
    },
    /**
     *
     * @param {IdentityAsIsQueryDto} body
     * @param {string} [propertyName]
     * @param {string} [propertyFilter]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    IdentityAsIsGetFilterResult(
      body: IdentityAsIsQueryDto,
      propertyName?: string,
      propertyFilter?: string,
      options?: any
    ) {
      return IdentityAsIsApiFp(configuration).IdentityAsIsGetFilterResult(
        body,
        propertyName,
        propertyFilter,
        options
      )(fetch, basePath);
    },
    /**
     *
     * @param {IdentityAsIsQueryDto} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    IdentityAsIsGetIdentityAsIs(body: IdentityAsIsQueryDto, options?: any) {
      return IdentityAsIsApiFp(configuration).IdentityAsIsGetIdentityAsIs(
        body,
        options
      )(fetch, basePath);
    },

    /**
     *
     * @param {number} id
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    IdentityAsIsGetUpdateResourceIdentityAsIs(id: number, options?: any) {
      return IdentityAsIsApiFp(
        configuration
      ).IdentityAsIsGetUpdateResourceIdentityAsIs(id, options)(fetch, basePath);
    },
    /**
     *
     * @param {IdentityAsIsDtoUpdate} body
     * @param {boolean} [forced]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    IdentityAsIsPut(
      body: IdentityAsIsDtoUpdate,
      forced?: boolean,
      options?: any
    ) {
      return IdentityAsIsApiFp(configuration).IdentityAsIsPut(
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
    IdentityAsIsRestore(id?: number, options?: any) {
      return IdentityAsIsApiFp(configuration).IdentityAsIsRestore(id, options)(
        fetch,
        basePath
      );
    },
    /**
     *
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    getResourceKeyForIdentityAsIs(options?: any) {
      return IdentityAsIsApiFp(configuration).getResourceKeyForIdentityAsIs(
        options
      )(fetch, basePath);
    },
  };
};

/**
 *IdentityAsIsApi - object-oriented interface
 * @export
 * @classIdentityAsIsApi
 * @extends {BaseAPI}
 */
export class IdentityAsIsApi extends BaseAPI {
  /**
   *
   * @param {IdentityAsIsDtoCreate} body
   * @param {boolean} [forced]
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof IdentityAsIsApi
   */
  public IdentityAsIsCreate(
    body: IdentityAsIsDtoCreate,
    forced?: boolean,
    options?: any
  ) {
    return IdentityAsIsApiFp(this.configuration).IdentityAsIsCreate(
      body,
      forced,
      options
    )(this.fetch, this.basePath);
  }
  /**
   *
   * @param {File} body
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof DeliveryTrackingApi
   */
  public identityAsIsImportStatus(body: File) {
    return IdentityAsIsApiFp(this.configuration).identityAsIsImportStatus(body)(
      this.fetch,
      this.basePath
    );
  }

  /**
   *
   * @param {number} [id]
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof IdentityAsIsApi
   */
  public GetAssetsByOpcoIdAndDcfId(
    opcoId?: number,
    dcfId?: number,

    options?: any
  ) {
    return IdentityAsIsApiFp(this.configuration).GetAssetsByOpcoIdAndDcfId(
      opcoId,
      dcfId,
      options
    )(this.fetch, this.basePath);
  }
  public IdentityAsIsDelete(id?: number, options?: any) {
    return IdentityAsIsApiFp(this.configuration).IdentityAsIsDelete(
      id,
      options
    )(this.fetch, this.basePath);
  }

  /**
   *
   * @param {number} [id]
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof IdentityAsIsApi
   */
  public IdentityAsIsDeleteDeep(id?: number, options?: any) {
    return IdentityAsIsApiFp(this.configuration).IdentityAsIsDeleteDeep(
      id,
      options
    )(this.fetch, this.basePath);
  }

  /**
   *
   * @param {IdentityAsIsQueryDto} body
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof IdentityAsIsApi
   */
  public IdentityAsIsExportReport(body: IdentityAsIsQueryDto, options?: any) {
    return IdentityAsIsApiFp(this.configuration).IdentityAsIsExportReport(
      body,
      options
    )(this.fetch, this.basePath);
  }

  /**
   *
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof IdentityAsIsApi
   */
  public IdentityAsIsGetCreateResourceIdentityAsIs(options?: any) {
    return IdentityAsIsApiFp(
      this.configuration
    ).IdentityAsIsGetCreateResourceIdentityAsIs(options)(
      this.fetch,
      this.basePath
    );
  }

  /**
   *
   * @param {IdentityAsIsQueryDto} body
   * @param {string} [propertyName]
   * @param {string} [propertyFilter]
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof IdentityAsIsApi
   */
  public IdentityAsIsGetFilterResult(
    body: IdentityAsIsQueryDto,
    propertyName?: string,
    propertyFilter?: string,
    options?: any
  ) {
    return IdentityAsIsApiFp(this.configuration).IdentityAsIsGetFilterResult(
      body,
      propertyName,
      propertyFilter,
      options
    )(this.fetch, this.basePath);
  }

  /**
   *
   * @param {IdentityAsIsQueryDto} body
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof IdentityAsIsApi
   */
  public IdentityAsIsGetIdentityAsIs(
    body: IdentityAsIsQueryDto,
    options?: any
  ) {
    return IdentityAsIsApiFp(this.configuration).IdentityAsIsGetIdentityAsIs(
      body,
      options
    )(this.fetch, this.basePath);
  }

  /**
   *
   * @param {number} id
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof IdentityAsIsApi
   */
  public IdentityAsIsGetUpdateResourceIdentityAsIs(id: number, options?: any) {
    return IdentityAsIsApiFp(
      this.configuration
    ).IdentityAsIsGetUpdateResourceIdentityAsIs(id, options)(
      this.fetch,
      this.basePath
    );
  }

  /**
   *
   * @param {IdentityAsIsDtoUpdate} body
   * @param {boolean} [forced]
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof IdentityAsIsApi
   */
  public IdentityAsIsPut(
    body: IdentityAsIsDtoUpdate,
    forced?: boolean,
    options?: any
  ) {
    return IdentityAsIsApiFp(this.configuration).IdentityAsIsPut(
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
   * @memberof IdentityAsIsApi
   */
  public IdentityAsIsRestore(id?: number, options?: any) {
    return IdentityAsIsApiFp(this.configuration).IdentityAsIsRestore(
      id,
      options
    )(this.fetch, this.basePath);
  }
  /**
   *@param {getResourceObject} body
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof NetworkElementAsPlannedApi
   */
  public getResourceKeyForIdentityAsIs(body: getResourceObject, options?: any) {
    return IdentityAsIsApiFp(this.configuration).getResourceKeyForIdentityAsIs(
      body,
      options
    )(this.fetch, this.basePath);
  }
}
