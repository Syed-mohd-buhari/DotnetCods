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
  MajorHardwareBuildDtoCreate,
  MajorHardwareBuildDtoUpdate,
  QueryResultDtoOfMajorHardwareBuildDtoGrid,
  MajorHardwareBuildQueryObjectGrid as MajorHardwareBuildQueryDto,
} from "../Model/MajorHardwareBuild";
import { BaseAPI } from "./Common/CommonBusiness";
import { ReturnFile, FileResult } from "../Model/Common";
import { headerObj } from "./header";

/**
 * MajorHardwareBuildApi - fetch parameter creator
 * @export
 */
export const MajorHardwareBuildApiFetchParamCreator = function (
  configuration?: Configuration
) {
  return {
    /**
     *
     * @param {DataRemediationDto} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    majorHardwareBuildApplyDataRemediation(
      body: DataRemediationDto,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling majorHardwareBuildApplyDataRemediation."
        );
      }
      const localVarPath = `/api/MajorHardwareBuild/ApplyDataRemediation`;
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
        <any>"DataRemediationDto" !== "string" ||
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
     * @param {MajorHardwareBuildDtoCreate} body
     * @param {boolean} [forced]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    majorHardwareBuildCreate(
      body: MajorHardwareBuildDtoCreate,
      forced?: boolean,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling majorHardwareBuildCreate."
        );
      }
      const localVarPath = `/api/MajorHardwareBuild`;
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
        <any>"MajorHardwareBuildDtoCreate" !== "string" ||
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
    majorHardwareBuildDelete(id?: number, options: any = {}): FetchArgs {
      const localVarPath = `/api/MajorHardwareBuild/Delete`;
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
    majorHardwareBuildDeleteDeep(id?: number, options: any = {}): FetchArgs {
      const localVarPath = `/api/MajorHardwareBuild/DeleteDeep`;
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
     * @param {MajorHardwareBuildQueryDto} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    majorHardwareBuildExportReport(
      body: MajorHardwareBuildQueryDto,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling majorHardwareBuildExportReport."
        );
      }
      const localVarPath = `/api/MajorHardwareBuild/ExportReport`;
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
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    majorHardwareBuildGetCreateResourceMajorHardwareBuild(
      options: any = {}
    ): FetchArgs {
      const localVarPath = `/api/MajorHardwareBuild/Create`;
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
     * @param {MajorHardwareBuildQueryDto} body
     * @param {string} [propertyName]
     * @param {string} [propertyFilter]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    majorHardwareBuildGetFilterResult(
      body: MajorHardwareBuildQueryDto,
      propertyName?: string,
      propertyFilter?: string,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling majorHardwareBuildGetFilterResult."
        );
      }
      const localVarPath = `/api/MajorHardwareBuild/Filter`;
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
     * @param {MajorHardwareBuildQueryDto} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    majorHardwareBuildGetMajorHardwareBuild(
      body: MajorHardwareBuildQueryDto,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling majorHardwareBuildGetMajorHardwareBuild."
        );
      }
      const localVarPath = `/api/MajorHardwareBuild/Get`;
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
     * @param {number} id
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    majorHardwareBuildGetRelatedRecords(
      id: number,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'id' is not null or undefined
      if (id === null || id === undefined) {
        throw new RequiredError(
          "id",
          "Required parameter id was null or undefined when calling majorHardwareBuildGetRelatedRecords."
        );
      }
      const localVarPath =
        `/api/MajorHardwareBuild/GetRelatedRecords{id}`.replace(
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
    majorHardwareBuildGetUpdateResourceMajorHardwareBuild(
      id: number,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'id' is not null or undefined
      if (id === null || id === undefined) {
        throw new RequiredError(
          "id",
          "Required parameter id was null or undefined when calling majorHardwareBuildGetUpdateResourceMajorHardwareBuild."
        );
      }
      const localVarPath = `/api/MajorHardwareBuild/Update{id}`.replace(
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
     * @param {MajorHardwareBuildDtoUpdate} body
     * @param {boolean} [forced]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    majorHardwareBuildPut(
      body: MajorHardwareBuildDtoUpdate,
      forced?: boolean,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling majorHardwareBuildPut."
        );
      }
      const localVarPath = `/api/MajorHardwareBuild`;
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
        <any>"MajorHardwareBuildDtoUpdate" !== "string" ||
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
    majorHardwareBuildRestore(id?: number, options: any = {}): FetchArgs {
      const localVarPath = `/api/MajorHardwareBuild/Restore`;
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
 * MajorHardwareBuildApi - functional programming interface
 * @export
 */
export const MajorHardwareBuildApiFp = function (
  configuration?: Configuration
) {
  return {
    /**
     *
     * @param {DataRemediationDto} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    majorHardwareBuildApplyDataRemediation(
      body: DataRemediationDto,
      options?: any
    ): (
      fetch?: FetchAPI,
      basePath?: string
    ) => Promise<ResultDtoOfResultDataRemediationDto> {
      const localVarFetchArgs = MajorHardwareBuildApiFetchParamCreator(
        configuration
      ).majorHardwareBuildApplyDataRemediation(body, options);
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
     * @param {MajorHardwareBuildDtoCreate} body
     * @param {boolean} [forced]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    majorHardwareBuildCreate(
      body: MajorHardwareBuildDtoCreate,
      forced?: boolean,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = MajorHardwareBuildApiFetchParamCreator(
        configuration
      ).majorHardwareBuildCreate(body, forced, options);
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
    majorHardwareBuildDelete(
      id?: number,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = MajorHardwareBuildApiFetchParamCreator(
        configuration
      ).majorHardwareBuildDelete(id, options);
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
    majorHardwareBuildDeleteDeep(
      id?: number,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = MajorHardwareBuildApiFetchParamCreator(
        configuration
      ).majorHardwareBuildDeleteDeep(id, options);
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
     * @param {MajorHardwareBuildQueryDto} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    majorHardwareBuildExportReport(
      body: MajorHardwareBuildQueryDto,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ReturnFile> {
      const localVarFetchArgs = MajorHardwareBuildApiFetchParamCreator(
        configuration
      ).majorHardwareBuildExportReport(body, options);
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
    majorHardwareBuildGetCreateResourceMajorHardwareBuild(
      options?: any
    ): (
      fetch?: FetchAPI,
      basePath?: string
    ) => Promise<MajorHardwareBuildDtoCreate> {
      const localVarFetchArgs =
        MajorHardwareBuildApiFetchParamCreator(
          configuration
        ).majorHardwareBuildGetCreateResourceMajorHardwareBuild(options);
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
     * @param {MajorHardwareBuildQueryDto} body
     * @param {string} [propertyName]
     * @param {string} [propertyFilter]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    majorHardwareBuildGetFilterResult(
      body: MajorHardwareBuildQueryDto,
      propertyName?: string,
      propertyFilter?: string,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<Array<FilterValueDto>> {
      const localVarFetchArgs = MajorHardwareBuildApiFetchParamCreator(
        configuration
      ).majorHardwareBuildGetFilterResult(
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
     * @param {MajorHardwareBuildQueryDto} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    majorHardwareBuildGetMajorHardwareBuild(
      body: MajorHardwareBuildQueryDto,
      options?: any
    ): (
      fetch?: FetchAPI,
      basePath?: string
    ) => Promise<QueryResultDtoOfMajorHardwareBuildDtoGrid> {
      const localVarFetchArgs = MajorHardwareBuildApiFetchParamCreator(
        configuration
      ).majorHardwareBuildGetMajorHardwareBuild(body, options);
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
    majorHardwareBuildGetRelatedRecords(
      id: number,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = MajorHardwareBuildApiFetchParamCreator(
        configuration
      ).majorHardwareBuildGetRelatedRecords(id, options);
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
    majorHardwareBuildGetUpdateResourceMajorHardwareBuild(
      id: number,
      options?: any
    ): (
      fetch?: FetchAPI,
      basePath?: string
    ) => Promise<MajorHardwareBuildDtoUpdate> {
      const localVarFetchArgs = MajorHardwareBuildApiFetchParamCreator(
        configuration
      ).majorHardwareBuildGetUpdateResourceMajorHardwareBuild(id, options);
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
     * @param {MajorHardwareBuildDtoUpdate} body
     * @param {boolean} [forced]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    majorHardwareBuildPut(
      body: MajorHardwareBuildDtoUpdate,
      forced?: boolean,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = MajorHardwareBuildApiFetchParamCreator(
        configuration
      ).majorHardwareBuildPut(body, forced, options);
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
    majorHardwareBuildRestore(
      id?: number,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = MajorHardwareBuildApiFetchParamCreator(
        configuration
      ).majorHardwareBuildRestore(id, options);
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
 * MajorHardwareBuildApi - factory interface
 * @export
 */
export const MajorHardwareBuildApiFactory = function (
  configuration?: Configuration,
  fetch?: FetchAPI,
  basePath?: string
) {
  return {
    /**
     *
     * @param {DataRemediationDto} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    majorHardwareBuildApplyDataRemediation(
      body: DataRemediationDto,
      options?: any
    ) {
      return MajorHardwareBuildApiFp(
        configuration
      ).majorHardwareBuildApplyDataRemediation(body, options)(fetch, basePath);
    },
    /**
     *
     * @param {MajorHardwareBuildDtoCreate} body
     * @param {boolean} [forced]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    majorHardwareBuildCreate(
      body: MajorHardwareBuildDtoCreate,
      forced?: boolean,
      options?: any
    ) {
      return MajorHardwareBuildApiFp(configuration).majorHardwareBuildCreate(
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
    majorHardwareBuildDelete(id?: number, options?: any) {
      return MajorHardwareBuildApiFp(configuration).majorHardwareBuildDelete(
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
    majorHardwareBuildDeleteDeep(id?: number, options?: any) {
      return MajorHardwareBuildApiFp(
        configuration
      ).majorHardwareBuildDeleteDeep(id, options)(fetch, basePath);
    },
    /**
     *
     * @param {MajorHardwareBuildQueryDto} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    majorHardwareBuildExportReport(
      body: MajorHardwareBuildQueryDto,
      options?: any
    ) {
      return MajorHardwareBuildApiFp(
        configuration
      ).majorHardwareBuildExportReport(body, options)(fetch, basePath);
    },
    /**
     *
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    majorHardwareBuildGetCreateResourceMajorHardwareBuild(options?: any) {
      return MajorHardwareBuildApiFp(
        configuration
      ).majorHardwareBuildGetCreateResourceMajorHardwareBuild(options)(
        fetch,
        basePath
      );
    },
    /**
     *
     * @param {MajorHardwareBuildQueryDto} body
     * @param {string} [propertyName]
     * @param {string} [propertyFilter]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    majorHardwareBuildGetFilterResult(
      body: MajorHardwareBuildQueryDto,
      propertyName?: string,
      propertyFilter?: string,
      options?: any
    ) {
      return MajorHardwareBuildApiFp(
        configuration
      ).majorHardwareBuildGetFilterResult(
        body,
        propertyName,
        propertyFilter,
        options
      )(fetch, basePath);
    },
    /**
     *
     * @param {MajorHardwareBuildQueryDto} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    majorHardwareBuildGetMajorHardwareBuild(
      body: MajorHardwareBuildQueryDto,
      options?: any
    ) {
      return MajorHardwareBuildApiFp(
        configuration
      ).majorHardwareBuildGetMajorHardwareBuild(body, options)(fetch, basePath);
    },
    /**
     *
     * @param {number} id
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    majorHardwareBuildGetRelatedRecords(id: number, options?: any) {
      return MajorHardwareBuildApiFp(
        configuration
      ).majorHardwareBuildGetRelatedRecords(id, options)(fetch, basePath);
    },
    /**
     *
     * @param {number} id
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    majorHardwareBuildGetUpdateResourceMajorHardwareBuild(
      id: number,
      options?: any
    ) {
      return MajorHardwareBuildApiFp(
        configuration
      ).majorHardwareBuildGetUpdateResourceMajorHardwareBuild(id, options)(
        fetch,
        basePath
      );
    },
    /**
     *
     * @param {MajorHardwareBuildDtoUpdate} body
     * @param {boolean} [forced]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    majorHardwareBuildPut(
      body: MajorHardwareBuildDtoUpdate,
      forced?: boolean,
      options?: any
    ) {
      return MajorHardwareBuildApiFp(configuration).majorHardwareBuildPut(
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
    majorHardwareBuildRestore(id?: number, options?: any) {
      return MajorHardwareBuildApiFp(configuration).majorHardwareBuildRestore(
        id,
        options
      )(fetch, basePath);
    },
  };
};

/**
 * MajorHardwareBuildApi - object-oriented interface
 * @export
 * @class MajorHardwareBuildApi
 * @extends {BaseAPI}
 */
export class MajorHardwareBuildApi extends BaseAPI {
  /**
   *
   * @param {DataRemediationDto} body
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof MajorHardwareBuildApi
   */
  public majorHardwareBuildApplyDataRemediation(
    body: DataRemediationDto,
    options?: any
  ) {
    return MajorHardwareBuildApiFp(
      this.configuration
    ).majorHardwareBuildApplyDataRemediation(body, options)(
      this.fetch,
      this.basePath
    );
  }

  /**
   *
   * @param {MajorHardwareBuildDtoCreate} body
   * @param {boolean} [forced]
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof MajorHardwareBuildApi
   */
  public majorHardwareBuildCreate(
    body: MajorHardwareBuildDtoCreate,
    forced?: boolean,
    options?: any
  ) {
    return MajorHardwareBuildApiFp(this.configuration).majorHardwareBuildCreate(
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
   * @memberof MajorHardwareBuildApi
   */
  public majorHardwareBuildDelete(id?: number, options?: any) {
    return MajorHardwareBuildApiFp(this.configuration).majorHardwareBuildDelete(
      id,
      options
    )(this.fetch, this.basePath);
  }

  /**
   *
   * @param {number} [id]
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof MajorHardwareBuildApi
   */
  public majorHardwareBuildDeleteDeep(id?: number, options?: any) {
    return MajorHardwareBuildApiFp(
      this.configuration
    ).majorHardwareBuildDeleteDeep(id, options)(this.fetch, this.basePath);
  }

  /**
   *
   * @param {MajorHardwareBuildQueryDto} body
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof MajorHardwareBuildApi
   */
  public majorHardwareBuildExportReport(
    body: MajorHardwareBuildQueryDto,
    options?: any
  ) {
    return MajorHardwareBuildApiFp(
      this.configuration
    ).majorHardwareBuildExportReport(body, options)(this.fetch, this.basePath);
  }

  /**
   *
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof MajorHardwareBuildApi
   */
  public majorHardwareBuildGetCreateResourceMajorHardwareBuild(options?: any) {
    return MajorHardwareBuildApiFp(
      this.configuration
    ).majorHardwareBuildGetCreateResourceMajorHardwareBuild(options)(
      this.fetch,
      this.basePath
    );
  }

  /**
   *
   * @param {MajorHardwareBuildQueryDto} body
   * @param {string} [propertyName]
   * @param {string} [propertyFilter]
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof MajorHardwareBuildApi
   */
  public majorHardwareBuildGetFilterResult(
    body: MajorHardwareBuildQueryDto,
    propertyName?: string,
    propertyFilter?: string,
    options?: any
  ) {
    return MajorHardwareBuildApiFp(
      this.configuration
    ).majorHardwareBuildGetFilterResult(
      body,
      propertyName,
      propertyFilter,
      options
    )(this.fetch, this.basePath);
  }

  /**
   *
   * @param {MajorHardwareBuildQueryDto} body
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof MajorHardwareBuildApi
   */
  public majorHardwareBuildGetMajorHardwareBuild(
    body: MajorHardwareBuildQueryDto,
    options?: any
  ) {
    return MajorHardwareBuildApiFp(
      this.configuration
    ).majorHardwareBuildGetMajorHardwareBuild(body, options)(
      this.fetch,
      this.basePath
    );
  }

  /**
   *
   * @param {number} id
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof MajorHardwareBuildApi
   */
  public majorHardwareBuildGetRelatedRecords(id: number, options?: any) {
    return MajorHardwareBuildApiFp(
      this.configuration
    ).majorHardwareBuildGetRelatedRecords(id, options)(
      this.fetch,
      this.basePath
    );
  }

  /**
   *
   * @param {number} id
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof MajorHardwareBuildApi
   */
  public majorHardwareBuildGetUpdateResourceMajorHardwareBuild(
    id: number,
    options?: any
  ) {
    return MajorHardwareBuildApiFp(
      this.configuration
    ).majorHardwareBuildGetUpdateResourceMajorHardwareBuild(id, options)(
      this.fetch,
      this.basePath
    );
  }

  /**
   *
   * @param {MajorHardwareBuildDtoUpdate} body
   * @param {boolean} [forced]
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof MajorHardwareBuildApi
   */
  public majorHardwareBuildPut(
    body: MajorHardwareBuildDtoUpdate,
    forced?: boolean,
    options?: any
  ) {
    return MajorHardwareBuildApiFp(this.configuration).majorHardwareBuildPut(
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
   * @memberof MajorHardwareBuildApi
   */
  public majorHardwareBuildRestore(id?: number, options?: any) {
    return MajorHardwareBuildApiFp(
      this.configuration
    ).majorHardwareBuildRestore(id, options)(this.fetch, this.basePath);
  }
}
