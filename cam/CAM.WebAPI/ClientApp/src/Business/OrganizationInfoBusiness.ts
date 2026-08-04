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
  QueryResultDtoOfOrganizationInfoDtoGrid,
  OrganizationInfoDtoCreate,
  OrganizationInfoDtoUpdate,
} from "../Model/OrganizationInfo";
import { ReturnFile } from "../Model/Common";
import { OrganizationInfoQueryObjectGrid } from "../Model/OrganizationInfo";
/**
 * OrganizationInfoApi - fetch parameter creator
 * @export
 */
export const OrganizationInfoApiFetchParamCreator = function (
  configuration?: Configuration
) {
  return {
    /**
     * @param {OrganizationInfoQueryObjectGrid} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    organizationInfoGetGrid(
      body: OrganizationInfoQueryObjectGrid,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling OrganizationInfoQueryObjectGrid."
        );
      }
      const localVarPath = `/api/Organisation/Get`;
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
        <any>"OrganizationInfoQueryObjectGrid" !== "string" ||
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
    organizationInfoGetCreateResourceOrganizationInfo(
      options: any = {}
    ): FetchArgs {
      const localVarPath = `/api/Organisation/Create`;
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
    organizationInfoGetUpdateResourceOrganizationInfo(
      id: number,
      options: any = {}
    ): FetchArgs {
      if (id === null || id === undefined) {
        throw new RequiredError(
          "id",
          "Required parameter id was null or undefined when calling organizationInfoGetUpdateResourceTestinfo."
        );
      }
      const localVarPath = `/api/Organisation/Update{id}`.replace(
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
     * @param {number} id
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    GetAllOpCosAndVerticalResponsibles(
      id: number,
      options: any = {}
    ): FetchArgs {
      if (id === null || id === undefined) {
        throw new RequiredError(
          "id",
          "Required parameter id was null or undefined when calling GetAllOpCosAndVerticalResponsibles."
        );
      }
      const localVarPath = `/api/Organisation/GetAllOpCosAndVerticalResponsibles`;
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
      if (id !== null) {
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
     * @param {Array<number>} id
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    GetOpCosAndVerticalAndSubDomainResponsibles(
      id: Array<number>,
      options: any = {}
    ): FetchArgs {
      if (id === null || id === undefined) {
        throw new RequiredError(
          "id",
          "Required parameter id was null or undefined when calling GetOpCosAndVerticalAndSubDomainResponsibles."
        );
      }
      const localVarPath = `/api/Organisation/GetOpCosAndVerticalAndSubDomainResponsibles`;
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
        <any>"OrganizationInfoDtoCreate" !== "string" ||
        localVarRequestOptions.headers["Content-Type"] === "application/json";
      localVarRequestOptions.body = needsSerialization
        ? JSON.stringify(id || {})
        : id || "";

      return {
        url: url.format(localVarUrlObj),
        options: localVarRequestOptions,
      };
    },
    /**
     *
     * @param {OrganizationInfoDtoUpdate} body
     * @param {boolean} [forced]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    organizationInfoEdit(
      body: OrganizationInfoDtoUpdate,
      forced?: boolean,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling organizationInfoEdit."
        );
      }
      const localVarPath = `/api/Organisation/Update`;
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
        <any>"OrganizationInfoDtoUpdate" !== "string" ||
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
     * @param {OrganizationInfoDtoCreate} body
     * @param {boolean} [forced]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    organizationInfoCreate(
      body: OrganizationInfoDtoCreate,
      forced?: boolean,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling organizationInfoCreate."
        );
      }
      const localVarPath = `/api/Organisation`;
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
        <any>"OrganizationInfoDtoCreate" !== "string" ||
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
    organizationInfoDelete(id: number, options: any = {}): FetchArgs {
      const localVarPath = `/api/Organisation/Delete`;
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
    organizationInfoDeleteDeep(id?: number, options: any = {}): FetchArgs {
      const localVarPath = `/api/Organisation/DeleteDeep`;
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
     * @param {number} id
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    organizationInfoGetRelatedRecords(
      id: number,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'id' is not null or undefined
      if (id === null || id === undefined) {
        throw new RequiredError(
          "id",
          "Required parameter id was null or undefined when calling organizationInfoGetRelatedRecords."
        );
      }
      const localVarPath = `/api/Organisation/GetRelatedRecords{id}`.replace(
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
     * @param {OrganizationInfoQueryObjectGrid} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    organizationInfoExportReport(
      body: OrganizationInfoQueryObjectGrid,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling organizationInfoExportReport."
        );
      }
      const localVarPath = `/api/Organisation/ExportReport`;
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
        <any>"OrganizationInfoQueryObjectGrid" !== "string" ||
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
     * @param {OrganizationInfoQueryObjectGrid} body
     * @param {string} [propertyName]
     * @param {string} [propertyFilter]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    organizationInfoGetFilterResult(
      body: OrganizationInfoQueryObjectGrid,
      propertyName?: string,
      propertyFilter?: string,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling organizationInfoGetFilterResult."
        );
      }
      const localVarPath = `/api/Organisation/Filter`;
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
        <any>"OrganizationInfoQueryObjectGrid" !== "string" ||
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
    organizationInfoRestore(id?: number, options: any = {}): FetchArgs {
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
 * OrganizationInfoApi - functional programming interface
 * @export
 */
export const OrganizationInfoApiFp = function (configuration?: Configuration) {
  return {
    /**
     *
     * @param {number} [id]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    organizationInfoDelete(
      id: number,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = OrganizationInfoApiFetchParamCreator(
        configuration
      ).organizationInfoDelete(id, options);
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
    organizationInfoDeleteDeep(
      id?: number,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = OrganizationInfoApiFetchParamCreator(
        configuration
      ).organizationInfoDeleteDeep(id, options);
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
    organizationInfoGetRelatedRecords(
      id: number,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = OrganizationInfoApiFetchParamCreator(
        configuration
      ).organizationInfoGetRelatedRecords(id, options);
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
     * @param {OrganizationInfoQueryObjectGrid} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    organizationInfoExportReport(
      body: OrganizationInfoQueryObjectGrid,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ReturnFile> {
      const localVarFetchArgs = OrganizationInfoApiFetchParamCreator(
        configuration
      ).organizationInfoExportReport(body, options);
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
    organizationInfoGetCreateResourceOrganizationInfo(
      options?: any
    ): (
      fetch?: FetchAPI,
      basePath?: string
    ) => Promise<OrganizationInfoDtoCreate> {
      const localVarFetchArgs =
        OrganizationInfoApiFetchParamCreator(
          configuration
        ).organizationInfoGetCreateResourceOrganizationInfo(options);
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
    organizationInfoGetUpdateResourceOrganizationInfo(
      id: number,
      options?: any
    ): (
      fetch?: FetchAPI,
      basePath?: string
    ) => Promise<OrganizationInfoDtoUpdate> {
      const localVarFetchArgs = OrganizationInfoApiFetchParamCreator(
        configuration
      ).organizationInfoGetUpdateResourceOrganizationInfo(id, options);
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
    GetAllOpCosAndVerticalResponsibles(
      id: number,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<any> {
      const localVarFetchArgs = OrganizationInfoApiFetchParamCreator(
        configuration
      ).GetAllOpCosAndVerticalResponsibles(id, options);
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
     * @param {Array<number>} id
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    GetOpCosAndVerticalAndSubDomainResponsibles(
      id: Array<number>,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<any> {
      const localVarFetchArgs = OrganizationInfoApiFetchParamCreator(
        configuration
      ).GetOpCosAndVerticalAndSubDomainResponsibles(id, options);
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
     * @param {OrganizationInfoQueryObjectGrid} body
     * @param {string} [propertyName]
     * @param {string} [propertyFilter]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    organizationInfoGetFilterResult(
      body: OrganizationInfoQueryObjectGrid,
      propertyName?: string,
      propertyFilter?: string,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<Array<FilterValueDto>> {
      const localVarFetchArgs = OrganizationInfoApiFetchParamCreator(
        configuration
      ).organizationInfoGetFilterResult(
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
     * @param {OrganizationInfoQueryObjectGrid} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    organizationInfoGetGrid(
      body: OrganizationInfoQueryObjectGrid,
      options?: any
    ): (
      fetch?: FetchAPI,
      basePath?: string
    ) => Promise<QueryResultDtoOfOrganizationInfoDtoGrid> {
      const localVarFetchArgs = OrganizationInfoApiFetchParamCreator(
        configuration
      ).organizationInfoGetGrid(body, options);
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
     * @param {OrganizationInfoDtoUpdate} body
     * @param {boolean} [forced]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    organizationInfoEdit(
      body: OrganizationInfoDtoUpdate,
      forced?: boolean,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = OrganizationInfoApiFetchParamCreator(
        configuration
      ).organizationInfoEdit(body, forced, options);
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
     * @param {OrganizationInfoDtoCreate} body
     * @param {boolean} [forced]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    organizationInfoCreate(
      body: OrganizationInfoDtoCreate,
      forced?: boolean,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = OrganizationInfoApiFetchParamCreator(
        configuration
      ).organizationInfoCreate(body, forced, options);
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
    organizationInfoRestore(
      id?: number,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = OrganizationInfoApiFetchParamCreator(
        configuration
      ).organizationInfoRestore(id, options);
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
 * OrganizationInfoApi - factory interface
 * @export
 */
export const OrganizationInfoApiFactory = function (
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
    organizationInfoDelete(id: number, options?: any) {
      return OrganizationInfoApiFp(configuration).organizationInfoDelete(
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
    organizationInfoDeleteDeep(id?: number, options?: any) {
      return OrganizationInfoApiFp(configuration).organizationInfoDeleteDeep(
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
    organizationInfoGetRelatedRecords(id: number, options?: any) {
      return OrganizationInfoApiFp(
        configuration
      ).organizationInfoGetRelatedRecords(id, options)(fetch, basePath);
    },
    /**
     *
     * @param {OrganizationInfoQueryObjectGrid} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    organizationInfoExportReport(
      body: OrganizationInfoQueryObjectGrid,
      options?: any
    ) {
      return OrganizationInfoApiFp(configuration).organizationInfoExportReport(
        body,
        options
      )(fetch, basePath);
    },
    /**
     *
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    organizationInfoGetCreateResourceOrganizationInfo(options?: any) {
      return OrganizationInfoApiFp(
        configuration
      ).organizationInfoGetCreateResourceOrganizationInfo(options)(
        fetch,
        basePath
      );
    },
    /**
     * @param {number} id
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    organizationInfoGetUpdateResourceOrganizationInfo(
      id: number,
      options?: any
    ) {
      return OrganizationInfoApiFp(
        configuration
      ).organizationInfoGetUpdateResourceOrganizationInfo(id, options)(
        fetch,
        basePath
      );
    },
    /**
     * @param {number} id
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    GetAllOpCosAndVerticalResponsibles(id: number, options?: any) {
      return OrganizationInfoApiFp(
        configuration
      ).GetAllOpCosAndVerticalResponsibles(id, options)(fetch, basePath);
    },
    /**
     * @param {Array<number>} id
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    GetOpCosAndVerticalAndSubDomainResponsibles(
      id: Array<number>,
      options?: any
    ) {
      return OrganizationInfoApiFp(
        configuration
      ).GetOpCosAndVerticalAndSubDomainResponsibles(id, options)(
        fetch,
        basePath
      );
    },
    /**
     *
     * @param {OrganizationInfoQueryObjectGrid} body
     * @param {string} [propertyName]
     * @param {string} [propertyFilter]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    organizationInfoGetFilterResult(
      body: OrganizationInfoQueryObjectGrid,
      propertyName?: string,
      propertyFilter?: string,
      options?: any
    ) {
      return OrganizationInfoApiFp(
        configuration
      ).organizationInfoGetFilterResult(
        body,
        propertyName,
        propertyFilter,
        options
      )(fetch, basePath);
    },
    /**
     * @param {OrganizationInfoQueryObjectGrid} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    organizationInfoGetGrid(
      body: OrganizationInfoQueryObjectGrid,
      options?: any
    ) {
      return OrganizationInfoApiFp(configuration).organizationInfoGetGrid(
        body,
        options
      )(fetch, basePath);
    },
    /**
     *
     * @param {OrganizationInfoDtoUpdate} body
     * @param {boolean} [forced]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    organizationInfoEdit(
      body: OrganizationInfoDtoUpdate,
      forced?: boolean,
      options?: any
    ) {
      return OrganizationInfoApiFp(configuration).organizationInfoEdit(
        body,
        forced,
        options
      )(fetch, basePath);
    },
    /**
     *
     * @param {OrganizationInfoDtoCreate} body
     * @param {boolean} [forced]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    organizationInfoCreate(
      body: OrganizationInfoDtoCreate,
      forced?: boolean,
      options?: any
    ) {
      return OrganizationInfoApiFp(configuration).organizationInfoCreate(
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
    organizationInfoRestore(id?: number, options?: any) {
      return OrganizationInfoApiFp(configuration).organizationInfoRestore(
        id,
        options
      )(fetch, basePath);
    },
  };
};

/**
 * OrganizationInfoApi - object-oriented interface
 * @export
 * @class OrganizationInfoApi
 * @extends {BaseAPI}
 */
export class OrganizationInfoApi extends BaseAPI {
  /**
   *
   * @param {number} [id]
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof OrganizationInfoApi
   */
  public organizationInfoDelete(id: number, options?: any) {
    return OrganizationInfoApiFp(this.configuration).organizationInfoDelete(
      id,
      options
    )(this.fetch, this.basePath);
  }

  /**
   *
   * @param {number} [id]
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof OrganizationInfoApi
   */
  public organizationInfoDeleteDeep(id?: number, options?: any) {
    return OrganizationInfoApiFp(this.configuration).organizationInfoDeleteDeep(
      id,
      options
    )(this.fetch, this.basePath);
  }

  /**
   *
   * @param {OrganizationInfoQueryObjectGrid} body
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof OrganizationInfoApi
   */
  public organizationInfoExportReport(
    body: OrganizationInfoQueryObjectGrid,
    options?: any
  ) {
    return OrganizationInfoApiFp(
      this.configuration
    ).organizationInfoExportReport(body, options)(this.fetch, this.basePath);
  }

  /**
   *
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof OrganizationInfoApi
   */
  public organizationInfoGetCreateResourceOrganizationInfo(options?: any) {
    return OrganizationInfoApiFp(
      this.configuration
    ).organizationInfoGetCreateResourceOrganizationInfo(options)(
      this.fetch,
      this.basePath
    );
  }
  /**
   * @param {number} id
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof OrganizationInfoApi
   */
  public organizationInfoGetUpdateResourceOrganizationInfo(
    id: number,
    options?: any
  ) {
    return OrganizationInfoApiFp(
      this.configuration
    ).organizationInfoGetUpdateResourceOrganizationInfo(id, options)(
      this.fetch,
      this.basePath
    );
  }
  /**
   * @param {number} id
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof OrganizationInfoApi
   */
  public GetAllOpCosAndVerticalResponsibles(id: number, options?: any) {
    return OrganizationInfoApiFp(
      this.configuration
    ).GetAllOpCosAndVerticalResponsibles(id, options)(
      this.fetch,
      this.basePath
    );
  }
  /**
   * @param {Array<number>} id
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof OrganizationInfoApi
   */
  public GetOpCosAndVerticalAndSubDomainResponsibles(
    id: Array<number>,
    options?: any
  ) {
    return OrganizationInfoApiFp(
      this.configuration
    ).GetOpCosAndVerticalAndSubDomainResponsibles(id, options)(
      this.fetch,
      this.basePath
    );
  }

  /**
   *
   * @param {OrganizationInfoQueryObjectGrid} body
   * @param {string} [propertyName]
   * @param {string} [propertyFilter]
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof OrganizationInfoApi
   */
  public organizationInfoGetFilterResult(
    body: OrganizationInfoQueryObjectGrid,
    propertyName?: string,
    propertyFilter?: string,
    options?: any
  ) {
    return OrganizationInfoApiFp(
      this.configuration
    ).organizationInfoGetFilterResult(
      body,
      propertyName,
      propertyFilter,
      options
    )(this.fetch, this.basePath);
  }

  /**
   *
   * @param {OrganizationInfoQueryObjectGrid} body
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof OrganizationInfoApi
   */
  public organizationInfoGetGrid(
    body: OrganizationInfoQueryObjectGrid,
    options?: any
  ) {
    return OrganizationInfoApiFp(this.configuration).organizationInfoGetGrid(
      body,
      options
    )(this.fetch, this.basePath);
  }

  /**
   *
   * @param {number} id
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof OrganizationInfoApi
   */
  public organizationInfoGetRelatedRecords(id: number, options?: any) {
    return OrganizationInfoApiFp(
      this.configuration
    ).organizationInfoGetRelatedRecords(id, options)(this.fetch, this.basePath);
  }

  /**
   *
   * @param {OrganizationInfoDtoUpdate} body
   * @param {boolean} [forced]
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof OrganizationInfoApi
   */
  public organizationInfoEdit(
    body: OrganizationInfoDtoUpdate,
    forced?: boolean,
    options?: any
  ) {
    return OrganizationInfoApiFp(this.configuration).organizationInfoEdit(
      body,
      forced,
      options
    )(this.fetch, this.basePath);
  }

  /**
   *
   * @param {OrganizationInfoDtoCreate} body
   * @param {boolean} [forced]
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof OrganizationInfoApi
   */
  public organizationInfoCreate(
    body: OrganizationInfoDtoCreate,
    forced?: boolean,
    options?: any
  ) {
    return OrganizationInfoApiFp(this.configuration).organizationInfoCreate(
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
   * @memberof OrganizationInfoApi
   */
  public organizationInfoRestore(id?: number, options?: any) {
    return OrganizationInfoApiFp(this.configuration).organizationInfoRestore(
      id,
      options
    )(this.fetch, this.basePath);
  }
}
