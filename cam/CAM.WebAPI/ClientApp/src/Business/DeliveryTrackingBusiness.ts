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

import {
  DeliveryTrackingDtoCreate,
  DeliveryTrackingDtoUpdate,
  QueryResultDtoOfDeliveryTrackingDtoGrid,
  DeliveryTrackingDto,
  DeliveryTrackingQueryObjectGrid,
} from "../Model/DeliveryTracking";
import { BaseAPI } from "./Common/CommonBusiness";
import { ReturnFile, FileResult } from "../Model/Common";
import { headerObj } from "./header";

/**
 * DeliveryTrackingApi - fetch parameter creator
 * @export
 */
export const DeliveryTrackingApiFetchParamCreator = function (
  configuration?: Configuration
) {
  return {
    /**
     *
     * @param {DeliveryTrackingDtoCreate} body
     * @param {boolean} [forced]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    DeliveryTrackingCreate(
      body: DeliveryTrackingDtoCreate,
      forced?: boolean,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling DeliveryTrackingCreate."
        );
      }
      const localVarPath = `/api/DeliveryTracking`;
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
        <any>"DeliveryTrackingDtoCreate" !== "string" ||
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
    deliveryTrackingImportStatus(body: File, options: any = {}): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      // const blob = new Blob([body], { type: body.type });
      const formData = new FormData();
      formData.append("file", body);
      console.log(formData);
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling deliveryTrackingImportStatus."
        );
      }
      const localVarPath = `/api/DeliveryTracking/ImportReport`;
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
    DeliveryTrackingDelete(id?: number, options: any = {}): FetchArgs {
      const localVarPath = `/api/DeliveryTracking/Delete`;
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
    DeliveryTrackingDeleteDeep(id?: number, options: any = {}): FetchArgs {
      const localVarPath = `/api/DeliveryTracking/DeleteDeep`;
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
     * @param {DeliveryTrackingQueryObjectGrid} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    DeliveryTrackingExportReport(
      body: DeliveryTrackingQueryObjectGrid,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling DeliveryTrackingExportReport."
        );
      }
      const localVarPath = `/api/DeliveryTracking/ExportReport`;
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
        <any>"DeliveryTrackingDto" !== "string" ||
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
    DeliveryTrackingGetCreateResourceDeliveryTracking(
      options: any = {}
    ): FetchArgs {
      const localVarPath = `/api/DeliveryTracking/Create`;
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
     * @param {DeliveryTrackingQueryObjectGrid} body
     * @param {string} [propertyName]
     * @param {string} [propertyFilter]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    DeliveryTrackingGetFilterResult(
      body: DeliveryTrackingQueryObjectGrid,
      propertyName?: string,
      propertyFilter?: string,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling DeliveryTrackingGetFilterResult."
        );
      }
      const localVarPath = `/api/DeliveryTracking/Filter`;
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
        <any>"DeliveryTrackingDto" !== "string" ||
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
     * @param {DeliveryTrackingQueryObjectGrid} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    DeliveryTrackingGetDeliveryTracking(
      body: DeliveryTrackingQueryObjectGrid,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling DeliveryTrackingQueryObjectGrid."
        );
      }
      const localVarPath = `/api/DeliveryTracking/Get`;
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
        <any>"DeliveryTrackingDto" !== "string" ||
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
    DeliveryTrackingGetRelatedRecords(
      id: number,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'id' is not null or undefined
      if (id === null || id === undefined) {
        throw new RequiredError(
          "id",
          "Required parameter id was null or undefined when calling DeliveryTrackingGetRelatedRecords."
        );
      }
      const localVarPath =
        `/api/DeliveryTracking/GetRelatedRecords{id}`.replace(
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
    DeliveryTrackingGetUpdateResourceDeliveryTracking(
      id: number,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'id' is not null or undefined
      if (id === null || id === undefined) {
        throw new RequiredError(
          "id",
          "Required parameter id was null or undefined when calling DeliveryTrackingGetUpdateResourceDeliveryTracking."
        );
      }
      const localVarPath = `/api/DeliveryTracking/Update{id}`.replace(
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
     * @param {DeliveryTrackingDtoUpdate} body
     * @param {boolean} [forced]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    DeliveryTrackingPut(
      body: DeliveryTrackingDtoUpdate,
      forced?: boolean,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling DeliveryTrackingPut."
        );
      }
      const localVarPath = `/api/DeliveryTracking`;
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
        <any>"DeliveryTrackingDtoUpdate" !== "string" ||
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
    DeliveryTrackingRestore(id?: number, options: any = {}): FetchArgs {
      const localVarPath = `/api/DeliveryTracking/Restore`;
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
 * DeliveryTrackingApi - functional programming interface
 * @export
 */
export const DeliveryTrackingApiFp = function (configuration?: Configuration) {
  return {
    /**
     *
     * @param {DeliveryTrackingDtoCreate} body
     * @param {boolean} [forced]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    DeliveryTrackingCreate(
      body: DeliveryTrackingDtoCreate,
      forced?: boolean,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = DeliveryTrackingApiFetchParamCreator(
        configuration
      ).DeliveryTrackingCreate(body, forced, options);
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
    deliveryTrackingImportStatus(
      body: File,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<boolean> {
      const localVarFetchArgs = DeliveryTrackingApiFetchParamCreator(
        configuration
      ).deliveryTrackingImportStatus(body, options);
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
    DeliveryTrackingDelete(
      id?: number,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = DeliveryTrackingApiFetchParamCreator(
        configuration
      ).DeliveryTrackingDelete(id, options);
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
    DeliveryTrackingDeleteDeep(
      id?: number,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = DeliveryTrackingApiFetchParamCreator(
        configuration
      ).DeliveryTrackingDeleteDeep(id, options);
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
     * @param {DeliveryTrackingQueryObjectGrid} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    DeliveryTrackingExportReport(
      body: DeliveryTrackingQueryObjectGrid,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ReturnFile> {
      const localVarFetchArgs = DeliveryTrackingApiFetchParamCreator(
        configuration
      ).DeliveryTrackingExportReport(body, options);
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
    DeliveryTrackingGetCreateResourceDeliveryTracking(
      options?: any
    ): (
      fetch?: FetchAPI,
      basePath?: string
    ) => Promise<DeliveryTrackingDtoCreate> {
      const localVarFetchArgs =
        DeliveryTrackingApiFetchParamCreator(
          configuration
        ).DeliveryTrackingGetCreateResourceDeliveryTracking(options);
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
     * @param {DeliveryTrackingQueryObjectGrid} body
     * @param {string} [propertyName]
     * @param {string} [propertyFilter]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    DeliveryTrackingGetFilterResult(
      body: DeliveryTrackingQueryObjectGrid,
      propertyName?: string,
      propertyFilter?: string,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<Array<FilterValueDto>> {
      const localVarFetchArgs = DeliveryTrackingApiFetchParamCreator(
        configuration
      ).DeliveryTrackingGetFilterResult(
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
     * @param {DeliveryTrackingQueryObjectGrid} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    DeliveryTrackingGetDeliveryTracking(
      body: DeliveryTrackingQueryObjectGrid,
      options?: any
    ): (
      fetch?: FetchAPI,
      basePath?: string
    ) => Promise<QueryResultDtoOfDeliveryTrackingDtoGrid> {
      const localVarFetchArgs = DeliveryTrackingApiFetchParamCreator(
        configuration
      ).DeliveryTrackingGetDeliveryTracking(body, options);
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
    DeliveryTrackingGetRelatedRecords(
      id: number,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = DeliveryTrackingApiFetchParamCreator(
        configuration
      ).DeliveryTrackingGetRelatedRecords(id, options);
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
    DeliveryTrackingGetUpdateResourceDeliveryTracking(
      id: number,
      options?: any
    ): (
      fetch?: FetchAPI,
      basePath?: string
    ) => Promise<DeliveryTrackingDtoUpdate> {
      const localVarFetchArgs = DeliveryTrackingApiFetchParamCreator(
        configuration
      ).DeliveryTrackingGetUpdateResourceDeliveryTracking(id, options);
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
     * @param {DeliveryTrackingDtoUpdate} body
     * @param {boolean} [forced]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    DeliveryTrackingPut(
      body: DeliveryTrackingDtoUpdate,
      forced?: boolean,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = DeliveryTrackingApiFetchParamCreator(
        configuration
      ).DeliveryTrackingPut(body, forced, options);
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
    DeliveryTrackingRestore(
      id?: number,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = DeliveryTrackingApiFetchParamCreator(
        configuration
      ).DeliveryTrackingRestore(id, options);
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
 * DeliveryTrackingApi - factory interface
 * @export
 */
export const DeliveryTrackingApiFactory = function (
  configuration?: Configuration,
  fetch?: FetchAPI,
  basePath?: string
) {
  return {
    /**
     *
     * @param {DeliveryTrackingDtoCreate} body
     * @param {boolean} [forced]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    DeliveryTrackingCreate(
      body: DeliveryTrackingDtoCreate,
      forced?: boolean,
      options?: any
    ) {
      return DeliveryTrackingApiFp(configuration).DeliveryTrackingCreate(
        body,
        forced,
        options
      )(fetch, basePath);
    },
    /**
     *
     * @param {File} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    deliveryTrackingImportStatus(body: File, options?: any) {
      return DeliveryTrackingApiFp(configuration).deliveryTrackingImportStatus(
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
    DeliveryTrackingDelete(id?: number, options?: any) {
      return DeliveryTrackingApiFp(configuration).DeliveryTrackingDelete(
        id,
        options
      )(fetch, basePath);
    },
    /**
     *
     * @param {File} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    DeliveryTrackingImportStatus(body: File, options?: any) {
      return DeliveryTrackingApiFp(configuration).deliveryTrackingImportStatus(
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
    DeliveryTrackingDeleteDeep(id?: number, options?: any) {
      return DeliveryTrackingApiFp(configuration).DeliveryTrackingDeleteDeep(
        id,
        options
      )(fetch, basePath);
    },
    /**
     *
     * @param {DeliveryTrackingQueryObjectGrid} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    DeliveryTrackingExportReport(
      body: DeliveryTrackingQueryObjectGrid,
      options?: any
    ) {
      return DeliveryTrackingApiFp(configuration).DeliveryTrackingExportReport(
        body,
        options
      )(fetch, basePath);
    },
    /**
     *
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    DeliveryTrackingGetCreateResourceDeliveryTracking(options?: any) {
      return DeliveryTrackingApiFp(
        configuration
      ).DeliveryTrackingGetCreateResourceDeliveryTracking(options)(
        fetch,
        basePath
      );
    },
    /**
     *
     * @param {DeliveryTrackingQueryObjectGrid} body
     * @param {string} [propertyName]
     * @param {string} [propertyFilter]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    DeliveryTrackingGetFilterResult(
      body: DeliveryTrackingQueryObjectGrid,
      propertyName?: string,
      propertyFilter?: string,
      options?: any
    ) {
      return DeliveryTrackingApiFp(
        configuration
      ).DeliveryTrackingGetFilterResult(
        body,
        propertyName,
        propertyFilter,
        options
      )(fetch, basePath);
    },
    /**
     *
     * @param {DeliveryTrackingQueryObjectGrid} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    DeliveryTrackingGetDeliveryTracking(
      body: DeliveryTrackingQueryObjectGrid,
      options?: any
    ) {
      return DeliveryTrackingApiFp(
        configuration
      ).DeliveryTrackingGetDeliveryTracking(body, options)(fetch, basePath);
    },
    /**
     *
     * @param {number} id
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    DeliveryTrackingGetRelatedRecords(id: number, options?: any) {
      return DeliveryTrackingApiFp(
        configuration
      ).DeliveryTrackingGetRelatedRecords(id, options)(fetch, basePath);
    },
    /**
     *
     * @param {number} id
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    DeliveryTrackingGetUpdateResourceDeliveryTracking(
      id: number,
      options?: any
    ) {
      return DeliveryTrackingApiFp(
        configuration
      ).DeliveryTrackingGetUpdateResourceDeliveryTracking(id, options)(
        fetch,
        basePath
      );
    },
    /**
     *
     * @param {DeliveryTrackingDtoUpdate} body
     * @param {boolean} [forced]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    DeliveryTrackingPut(
      body: DeliveryTrackingDtoUpdate,
      forced?: boolean,
      options?: any
    ) {
      return DeliveryTrackingApiFp(configuration).DeliveryTrackingPut(
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
    DeliveryTrackingRestore(id?: number, options?: any) {
      return DeliveryTrackingApiFp(configuration).DeliveryTrackingRestore(
        id,
        options
      )(fetch, basePath);
    },
  };
};

/**
 * DeliveryTrackingApi - object-oriented interface
 * @export
 * @class DeliveryTrackingApi
 * @extends {BaseAPI}
 */
export class DeliveryTrackingApi extends BaseAPI {
  /**
   *
   * @param {DeliveryTrackingDtoCreate} body
   * @param {boolean} [forced]
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof DeliveryTrackingApi
   */
  public DeliveryTrackingCreate(
    body: DeliveryTrackingDtoCreate,
    forced?: boolean,
    options?: any
  ) {
    return DeliveryTrackingApiFp(this.configuration).DeliveryTrackingCreate(
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
  public deliveryTrackingImportStatus(body: File) {
    return DeliveryTrackingApiFp(
      this.configuration
    ).deliveryTrackingImportStatus(body)(this.fetch, this.basePath);
  }
  /**
   *
   * @param {number} [id]
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof DeliveryTrackingApi
   */
  public DeliveryTrackingDelete(id?: number, options?: any) {
    return DeliveryTrackingApiFp(this.configuration).DeliveryTrackingDelete(
      id,
      options
    )(this.fetch, this.basePath);
  }

  /**
   *
   * @param {number} [id]
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof DeliveryTrackingApi
   */
  public DeliveryTrackingDeleteDeep(id?: number, options?: any) {
    return DeliveryTrackingApiFp(this.configuration).DeliveryTrackingDeleteDeep(
      id,
      options
    )(this.fetch, this.basePath);
  }

  /**
   *
   * @param {DeliveryTrackingQueryObjectGrid} body
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof DeliveryTrackingApi
   */
  public DeliveryTrackingExportReport(
    body: DeliveryTrackingQueryObjectGrid,
    options?: any
  ) {
    return DeliveryTrackingApiFp(
      this.configuration
    ).DeliveryTrackingExportReport(body, options)(this.fetch, this.basePath);
  }

  /**
   *
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof DeliveryTrackingApi
   */
  public DeliveryTrackingGetCreateResourceDeliveryTracking(options?: any) {
    return DeliveryTrackingApiFp(
      this.configuration
    ).DeliveryTrackingGetCreateResourceDeliveryTracking(options)(
      this.fetch,
      this.basePath
    );
  }

  /**
   *
   * @param {DeliveryTrackingQueryObjectGrid} body
   * @param {string} [propertyName]
   * @param {string} [propertyFilter]
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof DeliveryTrackingApi
   */
  public DeliveryTrackingGetFilterResult(
    body: DeliveryTrackingQueryObjectGrid,
    propertyName?: string,
    propertyFilter?: string,
    options?: any
  ) {
    return DeliveryTrackingApiFp(
      this.configuration
    ).DeliveryTrackingGetFilterResult(
      body,
      propertyName,
      propertyFilter,
      options
    )(this.fetch, this.basePath);
  }

  /**
   *
   * @param {DeliveryTrackingQueryObjectGrid} body
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof DeliveryTrackingApi
   */
  public DeliveryTrackingGetDeliveryTracking(
    body: DeliveryTrackingQueryObjectGrid,
    options?: any
  ) {
    return DeliveryTrackingApiFp(
      this.configuration
    ).DeliveryTrackingGetDeliveryTracking(body, options)(
      this.fetch,
      this.basePath
    );
  }

  /**
   *
   * @param {number} id
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof DeliveryTrackingApi
   */
  public DeliveryTrackingGetRelatedRecords(id: number, options?: any) {
    return DeliveryTrackingApiFp(
      this.configuration
    ).DeliveryTrackingGetRelatedRecords(id, options)(this.fetch, this.basePath);
  }

  /**
   *
   * @param {number} id
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof DeliveryTrackingApi
   */
  public DeliveryTrackingGetUpdateResourceDeliveryTracking(
    id: number,
    options?: any
  ) {
    return DeliveryTrackingApiFp(
      this.configuration
    ).DeliveryTrackingGetUpdateResourceDeliveryTracking(id, options)(
      this.fetch,
      this.basePath
    );
  }

  /**
   *
   * @param {DeliveryTrackingDtoUpdate} body
   * @param {boolean} [forced]
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof DeliveryTrackingApi
   */
  public DeliveryTrackingPut(
    body: DeliveryTrackingDtoUpdate,
    forced?: boolean,
    options?: any
  ) {
    return DeliveryTrackingApiFp(this.configuration).DeliveryTrackingPut(
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
   * @memberof DeliveryTrackingApi
   */
  public DeliveryTrackingRestore(id?: number, options?: any) {
    return DeliveryTrackingApiFp(this.configuration).DeliveryTrackingRestore(
      id,
      options
    )(this.fetch, this.basePath);
  }
}
