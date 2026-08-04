import * as url from "url";
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
import { headerObj } from "./header";

import {
  CloneMajorSoftwareBuildDto,
  MajorSoftwareBuildDtoCreate,
  MajorSoftwareBuildDtoUpdate,
  QueryResultDtoOfMajorSoftwareBuildDtoGrid,
  ResultDtoOfMajorSoftwareBuildToCloneDto,
  MajorSoftwareBuildQueryObjectGrid as MajorSoftwareBuildQueryDto,
  MajorSoftwareBuildProductBasedQueryObjectGrid,
} from "../Model/MajorSoftwareBuild";
import { BaseAPI } from "./Common/CommonBusiness";
import { ReturnFile } from "../Model/Common";
/**
 * MajorSoftwareBuildApi - fetch parameter creator
 * @export
 */
export const MajorSoftwareBuildApiFetchParamCreator = function (
  configuration?: Configuration
) {
  return {
    /**
     *
     * @param {DataRemediationDto} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    majorSoftwareBuildApplyDataRemediation(
      body: DataRemediationDto,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling majorSoftwareBuildApplyDataRemediation."
        );
      }
      const localVarPath = `/api/MajorSoftwareBuild/ApplyDataRemediation`;
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
     * @param {CloneMajorSoftwareBuildDto} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    majorSoftwareBuildCloneMajorSoftwareBuild(
      body: CloneMajorSoftwareBuildDto,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling majorSoftwareBuildCloneMajorSoftwareBuild."
        );
      }
      const localVarPath = `/api/MajorSoftwareBuild/CloneMajorSoftwareBuild`;
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
        <any>"CloneMajorSoftwareBuildDto" !== "string" ||
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
     * @param {MajorSoftwareBuildDtoCreate} body
     * @param {boolean} [forced]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    majorSoftwareBuildCreate(
      body: MajorSoftwareBuildDtoCreate,
      forced?: boolean,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling majorSoftwareBuildCreate."
        );
      }
      const localVarPath = `/api/MajorSoftwareBuild`;
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
        <any>"MajorSoftwareBuildDtoCreate" !== "string" ||
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
    majorSoftwareBuildDelete(id?: number, options: any = {}): FetchArgs {
      const localVarPath = `/api/MajorSoftwareBuild/Delete`;
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
    majorSoftwareBuildDeleteDeep(id?: number, options: any = {}): FetchArgs {
      const localVarPath = `/api/MajorSoftwareBuild/DeleteDeep`;
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
    majorSoftwareBuildDeleteOrphans(options: any = {}): FetchArgs {
      const localVarPath = `/api/MajorSoftwareBuild/DeleteOrphans`;
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
     * @param {MajorSoftwareBuildQueryDto} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    majorSoftwareBuildExportReport(
      body: MajorSoftwareBuildQueryDto,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling majorSoftwareBuildExportReport."
        );
      }
      const localVarPath = `/api/MajorSoftwareBuild/ExportReport`;
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
        <any>"MajorSoftwareBuildQueryDto" !== "string" ||
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
    majorSoftwareBuildGetCreateResourceMajorSoftwareBuild(
      options: any = {}
    ): FetchArgs {
      const localVarPath = `/api/MajorSoftwareBuild/Create`;
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
     * @param {MajorSoftwareBuildQueryDto} body
     * @param {string} [propertyName]
     * @param {string} [propertyFilter]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    majorSoftwareBuildGetFilterResult(
      body: MajorSoftwareBuildQueryDto,
      propertyName?: string,
      propertyFilter?: string,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling majorSoftwareBuildGetFilterResult."
        );
      }
      const localVarPath = `/api/MajorSoftwareBuild/Filter`;
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
        <any>"MajorSoftwareBuildQueryDto" !== "string" ||
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
     * @param {boolean} flag
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    majorSoftwareBuildGetInfoMajorSoftwareBuildToClone(
      id: number,
      flag?: boolean,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'id' is not null or undefined
      if (id === null || id === undefined) {
        throw new RequiredError(
          "id",
          "Required parameter id was null or undefined when calling majorSoftwareBuildGetInfoMajorSoftwareBuildToClone."
        );
      }
      const localVarPath =
        `/api/MajorSoftwareBuild/GetInfoMajorSoftwareBuildToClone{id}`.replace(
          `{${"id"}}`,
          encodeURIComponent(String(id))
        ) + `/${flag}`;
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
     * @param {MajorSoftwareBuildQueryDto} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    majorSoftwareBuildGetMajorSoftwareBuild(
      body: MajorSoftwareBuildQueryDto,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling majorSoftwareBuildGetMajorSoftwareBuild."
        );
      }
      const localVarPath = `/api/MajorSoftwareBuild/Get`;
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
        <any>"MajorSoftwareBuildQueryDto" !== "string" ||
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
     * @param {MajorSoftwareBuildProductBasedQueryObjectGrid} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    majorSoftwareBuildProductBasedGrid(
      body: MajorSoftwareBuildProductBasedQueryObjectGrid,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling majorSoftwareBuildProductBasedGrid."
        );
      }
      const localVarPath = `/api/MajorSoftwareBuild/GetProductBasedSofware`;
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
        <any>"MajorSoftwareBuildProductBasedQueryObjectGrid" !== "string" ||
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
    majorSoftwareBuildGetMajorSoftwareBuildToClone(
      id: number,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'id' is not null or undefined
      if (id === null || id === undefined) {
        throw new RequiredError(
          "id",
          "Required parameter id was null or undefined when calling majorSoftwareBuildGetMajorSoftwareBuildToClone."
        );
      }
      const localVarPath =
        `/api/MajorSoftwareBuild/GetMajorSoftwareBuild{id}`.replace(
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
     * @param {any} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    majorSoftwareBuildGetSystemTypeForAddMajorSW(
      body: any,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling majorSoftwareBuildGetSystemTypeForAddMajorSW."
        );
      }
      const localVarPath = `/api/MajorSoftwareBuild/GetSystemTypeForAddMajorSW`;
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
        <any>"MajorSoftwareBuildQueryDto" !== "string" ||
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
    majorSoftwareBuildGetRelatedRecords(
      id: number,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'id' is not null or undefined
      if (id === null || id === undefined) {
        throw new RequiredError(
          "id",
          "Required parameter id was null or undefined when calling majorSoftwareBuildGetRelatedRecords."
        );
      }
      const localVarPath =
        `/api/MajorSoftwareBuild/GetRelatedRecords{id}`.replace(
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
    majorSoftwareBuildGetUpdateResourceMajorSoftwareBuild(
      id: number,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'id' is not null or undefined
      if (id === null || id === undefined) {
        throw new RequiredError(
          "id",
          "Required parameter id was null or undefined when calling majorSoftwareBuildGetUpdateResourceMajorSoftwareBuild."
        );
      }
      const localVarPath = `/api/MajorSoftwareBuild/Update{id}`.replace(
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
     * @param {MajorSoftwareBuildDtoUpdate} body
     * @param {boolean} [forced]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    majorSoftwareBuildPut(
      body: MajorSoftwareBuildDtoUpdate,
      forced?: boolean,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling majorSoftwareBuildPut."
        );
      }
      const localVarPath = `/api/MajorSoftwareBuild`;
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
        <any>"MajorSoftwareBuildDtoUpdate" !== "string" ||
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
    majorSoftwareBuildRestore(id?: number, options: any = {}): FetchArgs {
      const localVarPath = `/api/MajorSoftwareBuild/Restore`;
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
 * MajorSoftwareBuildApi - functional programming interface
 * @export
 */
export const MajorSoftwareBuildApiFp = function (
  configuration?: Configuration
) {
  return {
    /**
     *
     * @param {DataRemediationDto} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    majorSoftwareBuildApplyDataRemediation(
      body: DataRemediationDto,
      options?: any
    ): (
      fetch?: FetchAPI,
      basePath?: string
    ) => Promise<ResultDtoOfResultDataRemediationDto> {
      const localVarFetchArgs = MajorSoftwareBuildApiFetchParamCreator(
        configuration
      ).majorSoftwareBuildApplyDataRemediation(body, options);
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
     * @param {CloneMajorSoftwareBuildDto} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    majorSoftwareBuildCloneMajorSoftwareBuild(
      body: CloneMajorSoftwareBuildDto,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = MajorSoftwareBuildApiFetchParamCreator(
        configuration
      ).majorSoftwareBuildCloneMajorSoftwareBuild(body, options);
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
     * @param {MajorSoftwareBuildDtoCreate} body
     * @param {boolean} [forced]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    majorSoftwareBuildCreate(
      body: MajorSoftwareBuildDtoCreate,
      forced?: boolean,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = MajorSoftwareBuildApiFetchParamCreator(
        configuration
      ).majorSoftwareBuildCreate(body, forced, options);
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
    majorSoftwareBuildDelete(
      id?: number,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = MajorSoftwareBuildApiFetchParamCreator(
        configuration
      ).majorSoftwareBuildDelete(id, options);
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
    majorSoftwareBuildDeleteDeep(
      id?: number,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = MajorSoftwareBuildApiFetchParamCreator(
        configuration
      ).majorSoftwareBuildDeleteDeep(id, options);
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
    majorSoftwareBuildDeleteOrphans(
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs =
        MajorSoftwareBuildApiFetchParamCreator(
          configuration
        ).majorSoftwareBuildDeleteOrphans(options);
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
     * @param {MajorSoftwareBuildQueryDto} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    majorSoftwareBuildExportReport(
      body: MajorSoftwareBuildQueryDto,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ReturnFile> {
      const localVarFetchArgs = MajorSoftwareBuildApiFetchParamCreator(
        configuration
      ).majorSoftwareBuildExportReport(body, options);
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
    majorSoftwareBuildGetCreateResourceMajorSoftwareBuild(
      options?: any
    ): (
      fetch?: FetchAPI,
      basePath?: string
    ) => Promise<MajorSoftwareBuildDtoCreate> {
      const localVarFetchArgs =
        MajorSoftwareBuildApiFetchParamCreator(
          configuration
        ).majorSoftwareBuildGetCreateResourceMajorSoftwareBuild(options);
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
     * @param {MajorSoftwareBuildQueryDto} body
     * @param {string} [propertyName]
     * @param {string} [propertyFilter]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    majorSoftwareBuildGetFilterResult(
      body: MajorSoftwareBuildQueryDto,
      propertyName?: string,
      propertyFilter?: string,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<Array<FilterValueDto>> {
      const localVarFetchArgs = MajorSoftwareBuildApiFetchParamCreator(
        configuration
      ).majorSoftwareBuildGetFilterResult(
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
     * @param {boolean} flag
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    majorSoftwareBuildGetInfoMajorSoftwareBuildToClone(
      id: number,
      flag?: boolean,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = MajorSoftwareBuildApiFetchParamCreator(
        configuration
      ).majorSoftwareBuildGetInfoMajorSoftwareBuildToClone(id, flag, options);
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
     * @param {MajorSoftwareBuildQueryDto} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    majorSoftwareBuildGetMajorSoftwareBuild(
      body: MajorSoftwareBuildQueryDto,
      options?: any
    ): (
      fetch?: FetchAPI,
      basePath?: string
    ) => Promise<QueryResultDtoOfMajorSoftwareBuildDtoGrid> {
      const localVarFetchArgs = MajorSoftwareBuildApiFetchParamCreator(
        configuration
      ).majorSoftwareBuildGetMajorSoftwareBuild(body, options);
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
     * @param {MajorSoftwareBuildProductBasedQueryObjectGrid} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    majorSoftwareBuildProductBasedGrid(
      body: MajorSoftwareBuildProductBasedQueryObjectGrid,
      options?: any
    ): (
      fetch?: FetchAPI,
      basePath?: string
    ) => Promise<QueryResultDtoOfMajorSoftwareBuildDtoGrid> {
      const localVarFetchArgs = MajorSoftwareBuildApiFetchParamCreator(
        configuration
      ).majorSoftwareBuildProductBasedGrid(body, options);
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
    majorSoftwareBuildGetMajorSoftwareBuildToClone(
      id: number,
      options?: any
    ): (
      fetch?: FetchAPI,
      basePath?: string
    ) => Promise<ResultDtoOfMajorSoftwareBuildToCloneDto> {
      const localVarFetchArgs = MajorSoftwareBuildApiFetchParamCreator(
        configuration
      ).majorSoftwareBuildGetMajorSoftwareBuildToClone(id, options);
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
    majorSoftwareBuildGetSystemTypeForAddMajorSW(
      body: any,
      options?: any
    ): (
      fetch?: FetchAPI,
      basePath?: string
    ) => Promise<ResultDtoOfMajorSoftwareBuildToCloneDto> {
      const localVarFetchArgs = MajorSoftwareBuildApiFetchParamCreator(
        configuration
      ).majorSoftwareBuildGetSystemTypeForAddMajorSW(body, options);
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
    majorSoftwareBuildGetRelatedRecords(
      id: number,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = MajorSoftwareBuildApiFetchParamCreator(
        configuration
      ).majorSoftwareBuildGetRelatedRecords(id, options);
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
    majorSoftwareBuildGetUpdateResourceMajorSoftwareBuild(
      id: number,
      options?: any
    ): (
      fetch?: FetchAPI,
      basePath?: string
    ) => Promise<MajorSoftwareBuildDtoUpdate> {
      const localVarFetchArgs = MajorSoftwareBuildApiFetchParamCreator(
        configuration
      ).majorSoftwareBuildGetUpdateResourceMajorSoftwareBuild(id, options);
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
     * @param {MajorSoftwareBuildDtoUpdate} body
     * @param {boolean} [forced]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    majorSoftwareBuildPut(
      body: MajorSoftwareBuildDtoUpdate,
      forced?: boolean,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = MajorSoftwareBuildApiFetchParamCreator(
        configuration
      ).majorSoftwareBuildPut(body, forced, options);
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
    majorSoftwareBuildRestore(
      id?: number,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = MajorSoftwareBuildApiFetchParamCreator(
        configuration
      ).majorSoftwareBuildRestore(id, options);
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
 * MajorSoftwareBuildApi - factory interface
 * @export
 */
export const MajorSoftwareBuildApiFactory = function (
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
    majorSoftwareBuildApplyDataRemediation(
      body: DataRemediationDto,
      options?: any
    ) {
      return MajorSoftwareBuildApiFp(
        configuration
      ).majorSoftwareBuildApplyDataRemediation(body, options)(fetch, basePath);
    },
    /**
     *
     * @param {CloneMajorSoftwareBuildDto} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    majorSoftwareBuildCloneMajorSoftwareBuild(
      body: CloneMajorSoftwareBuildDto,
      options?: any
    ) {
      return MajorSoftwareBuildApiFp(
        configuration
      ).majorSoftwareBuildCloneMajorSoftwareBuild(body, options)(
        fetch,
        basePath
      );
    },
    /**
     *
     * @param {MajorSoftwareBuildDtoCreate} body
     * @param {boolean} [forced]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    majorSoftwareBuildCreate(
      body: MajorSoftwareBuildDtoCreate,
      forced?: boolean,
      options?: any
    ) {
      return MajorSoftwareBuildApiFp(configuration).majorSoftwareBuildCreate(
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
    majorSoftwareBuildDelete(id?: number, options?: any) {
      return MajorSoftwareBuildApiFp(configuration).majorSoftwareBuildDelete(
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
    majorSoftwareBuildDeleteDeep(id?: number, options?: any) {
      return MajorSoftwareBuildApiFp(
        configuration
      ).majorSoftwareBuildDeleteDeep(id, options)(fetch, basePath);
    },
    /**
     *
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    majorSoftwareBuildDeleteOrphans(options?: any) {
      return MajorSoftwareBuildApiFp(
        configuration
      ).majorSoftwareBuildDeleteOrphans(options)(fetch, basePath);
    },
    /**
     *
     * @param {MajorSoftwareBuildQueryDto} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    majorSoftwareBuildExportReport(
      body: MajorSoftwareBuildQueryDto,
      options?: any
    ) {
      return MajorSoftwareBuildApiFp(
        configuration
      ).majorSoftwareBuildExportReport(body, options)(fetch, basePath);
    },
    /**
     *
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    majorSoftwareBuildGetCreateResourceMajorSoftwareBuild(options?: any) {
      return MajorSoftwareBuildApiFp(
        configuration
      ).majorSoftwareBuildGetCreateResourceMajorSoftwareBuild(options)(
        fetch,
        basePath
      );
    },
    /**
     *
     * @param {MajorSoftwareBuildQueryDto} body
     * @param {string} [propertyName]
     * @param {string} [propertyFilter]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    majorSoftwareBuildGetFilterResult(
      body: MajorSoftwareBuildQueryDto,
      propertyName?: string,
      propertyFilter?: string,
      options?: any
    ) {
      return MajorSoftwareBuildApiFp(
        configuration
      ).majorSoftwareBuildGetFilterResult(
        body,
        propertyName,
        propertyFilter,
        options
      )(fetch, basePath);
    },
    /**
     *
     * @param {number} id
     * @param {boolean} flag
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    majorSoftwareBuildGetInfoMajorSoftwareBuildToClone(
      id: number,
      flag: boolean,
      options?: any
    ) {
      return MajorSoftwareBuildApiFp(
        configuration
      ).majorSoftwareBuildGetInfoMajorSoftwareBuildToClone(
        id,
        flag,
        options
      )(fetch, basePath);
    },
    /**
     *
     * @param {MajorSoftwareBuildQueryDto} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    majorSoftwareBuildGetMajorSoftwareBuild(
      body: MajorSoftwareBuildQueryDto,
      options?: any
    ) {
      return MajorSoftwareBuildApiFp(
        configuration
      ).majorSoftwareBuildGetMajorSoftwareBuild(body, options)(fetch, basePath);
    },
    /**
     *
     * @param {MajorSoftwareBuildProductBasedQueryObjectGrid} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    majorSoftwareBuildProductBasedGrid(
      body: MajorSoftwareBuildProductBasedQueryObjectGrid,
      options?: any
    ) {
      return MajorSoftwareBuildApiFp(
        configuration
      ).majorSoftwareBuildProductBasedGrid(body, options)(fetch, basePath);
    },
    /**
     *
     * @param {number} id
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    majorSoftwareBuildGetMajorSoftwareBuildToClone(id: number, options?: any) {
      return MajorSoftwareBuildApiFp(
        configuration
      ).majorSoftwareBuildGetMajorSoftwareBuildToClone(id, options)(
        fetch,
        basePath
      );
    },
    /**
     *
     * @param {any} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    majorSoftwareBuildGetSystemTypeForAddMajorSW(body: any, options?: any) {
      return MajorSoftwareBuildApiFp(
        configuration
      ).majorSoftwareBuildGetSystemTypeForAddMajorSW(body, options)(
        fetch,
        basePath
      );
    },
    /**
     *
     * @param {number} id
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    majorSoftwareBuildGetRelatedRecords(id: number, options?: any) {
      return MajorSoftwareBuildApiFp(
        configuration
      ).majorSoftwareBuildGetRelatedRecords(id, options)(fetch, basePath);
    },
    /**
     *
     * @param {number} id
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    majorSoftwareBuildGetUpdateResourceMajorSoftwareBuild(
      id: number,
      options?: any
    ) {
      return MajorSoftwareBuildApiFp(
        configuration
      ).majorSoftwareBuildGetUpdateResourceMajorSoftwareBuild(id, options)(
        fetch,
        basePath
      );
    },
    /**
     *
     * @param {MajorSoftwareBuildDtoUpdate} body
     * @param {boolean} [forced]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    majorSoftwareBuildPut(
      body: MajorSoftwareBuildDtoUpdate,
      forced?: boolean,
      options?: any
    ) {
      return MajorSoftwareBuildApiFp(configuration).majorSoftwareBuildPut(
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
    majorSoftwareBuildRestore(id?: number, options?: any) {
      return MajorSoftwareBuildApiFp(configuration).majorSoftwareBuildRestore(
        id,
        options
      )(fetch, basePath);
    },
  };
};

/**
 * MajorSoftwareBuildApi - object-oriented interface
 * @export
 * @class MajorSoftwareBuildApi
 * @extends {BaseAPI}
 */
export class MajorSoftwareBuildApi extends BaseAPI {
  /**
   *
   * @param {DataRemediationDto} body
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof MajorSoftwareBuildApi
   */
  public majorSoftwareBuildApplyDataRemediation(
    body: DataRemediationDto,
    options?: any
  ) {
    return MajorSoftwareBuildApiFp(
      this.configuration
    ).majorSoftwareBuildApplyDataRemediation(body, options)(
      this.fetch,
      this.basePath
    );
  }

  /**
   *
   * @param {CloneMajorSoftwareBuildDto} body
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof MajorSoftwareBuildApi
   */
  public majorSoftwareBuildCloneMajorSoftwareBuild(
    body: CloneMajorSoftwareBuildDto,
    options?: any
  ) {
    return MajorSoftwareBuildApiFp(
      this.configuration
    ).majorSoftwareBuildCloneMajorSoftwareBuild(body, options)(
      this.fetch,
      this.basePath
    );
  }

  /**
   *
   * @param {MajorSoftwareBuildDtoCreate} body
   * @param {boolean} [forced]
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof MajorSoftwareBuildApi
   */
  public majorSoftwareBuildCreate(
    body: MajorSoftwareBuildDtoCreate,
    forced?: boolean,
    options?: any
  ) {
    return MajorSoftwareBuildApiFp(this.configuration).majorSoftwareBuildCreate(
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
   * @memberof MajorSoftwareBuildApi
   */
  public majorSoftwareBuildDelete(id?: number, options?: any) {
    return MajorSoftwareBuildApiFp(this.configuration).majorSoftwareBuildDelete(
      id,
      options
    )(this.fetch, this.basePath);
  }

  /**
   *
   * @param {number} [id]
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof MajorSoftwareBuildApi
   */
  public majorSoftwareBuildDeleteDeep(id?: number, options?: any) {
    return MajorSoftwareBuildApiFp(
      this.configuration
    ).majorSoftwareBuildDeleteDeep(id, options)(this.fetch, this.basePath);
  }

  /**
   *
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof MajorSoftwareBuildApi
   */
  public majorSoftwareBuildDeleteOrphans(options?: any) {
    return MajorSoftwareBuildApiFp(
      this.configuration
    ).majorSoftwareBuildDeleteOrphans(options)(this.fetch, this.basePath);
  }

  /**
   *
   * @param {MajorSoftwareBuildQueryDto} body
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof MajorSoftwareBuildApi
   */
  public majorSoftwareBuildExportReport(
    body: MajorSoftwareBuildQueryDto,
    options?: any
  ) {
    return MajorSoftwareBuildApiFp(
      this.configuration
    ).majorSoftwareBuildExportReport(body, options)(this.fetch, this.basePath);
  }

  /**
   *
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof MajorSoftwareBuildApi
   */
  public majorSoftwareBuildGetCreateResourceMajorSoftwareBuild(options?: any) {
    return MajorSoftwareBuildApiFp(
      this.configuration
    ).majorSoftwareBuildGetCreateResourceMajorSoftwareBuild(options)(
      this.fetch,
      this.basePath
    );
  }

  /**
   *
   * @param {MajorSoftwareBuildQueryDto} body
   * @param {string} [propertyName]
   * @param {string} [propertyFilter]
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof MajorSoftwareBuildApi
   */
  public majorSoftwareBuildGetFilterResult(
    body: MajorSoftwareBuildQueryDto,
    propertyName?: string,
    propertyFilter?: string,
    options?: any
  ) {
    return MajorSoftwareBuildApiFp(
      this.configuration
    ).majorSoftwareBuildGetFilterResult(
      body,
      propertyName,
      propertyFilter,
      options
    )(this.fetch, this.basePath);
  }

  /**
   *
   * @param {number} id
   * @param {boolean} flag
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof MajorSoftwareBuildApi
   */
  public majorSoftwareBuildGetInfoMajorSoftwareBuildToClone(
    id: number,
    flag?: boolean,
    options?: any
  ) {
    return MajorSoftwareBuildApiFp(
      this.configuration
    ).majorSoftwareBuildGetInfoMajorSoftwareBuildToClone(
      id,
      flag,
      options
    )(this.fetch, this.basePath);
  }

  /**
   *
   * @param {MajorSoftwareBuildQueryDto} body
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof MajorSoftwareBuildApi
   */
  public majorSoftwareBuildGetMajorSoftwareBuild(
    body: MajorSoftwareBuildQueryDto,
    options?: any
  ) {
    return MajorSoftwareBuildApiFp(
      this.configuration
    ).majorSoftwareBuildGetMajorSoftwareBuild(body, options)(
      this.fetch,
      this.basePath
    );
  }
  /**
   *
   * @param {MajorSoftwareBuildProductBasedQueryObjectGrid} body
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof MajorSoftwareBuildApi
   */
  public majorSoftwareBuildProductBasedGrid(
    body: MajorSoftwareBuildProductBasedQueryObjectGrid,
    options?: any
  ) {
    return MajorSoftwareBuildApiFp(
      this.configuration
    ).majorSoftwareBuildProductBasedGrid(body, options)(
      this.fetch,
      this.basePath
    );
  }

  /**
   *
   * @param {number} id
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof MajorSoftwareBuildApi
   */
  public majorSoftwareBuildGetMajorSoftwareBuildToClone(
    id: number,
    options?: any
  ) {
    return MajorSoftwareBuildApiFp(
      this.configuration
    ).majorSoftwareBuildGetMajorSoftwareBuildToClone(id, options)(
      this.fetch,
      this.basePath
    );
  }

  /**
   *
   * @param {any} body
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof MajorSoftwareBuildApi
   */
  public majorSoftwareBuildGetSystemTypeForAddMajorSW(
    body: any,
    options?: any
  ) {
    return MajorSoftwareBuildApiFp(
      this.configuration
    ).majorSoftwareBuildGetSystemTypeForAddMajorSW(body, options)(
      this.fetch,
      this.basePath
    );
  }

  /**
   *
   * @param {number} id
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof MajorSoftwareBuildApi
   */
  public majorSoftwareBuildGetRelatedRecords(id: number, options?: any) {
    return MajorSoftwareBuildApiFp(
      this.configuration
    ).majorSoftwareBuildGetRelatedRecords(id, options)(
      this.fetch,
      this.basePath
    );
  }

  /**
   *
   * @param {number} id
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof MajorSoftwareBuildApi
   */
  public majorSoftwareBuildGetUpdateResourceMajorSoftwareBuild(
    id: number,
    options?: any
  ) {
    return MajorSoftwareBuildApiFp(
      this.configuration
    ).majorSoftwareBuildGetUpdateResourceMajorSoftwareBuild(id, options)(
      this.fetch,
      this.basePath
    );
  }

  /**
   *
   * @param {MajorSoftwareBuildDtoUpdate} body
   * @param {boolean} [forced]
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof MajorSoftwareBuildApi
   */
  public majorSoftwareBuildPut(
    body: MajorSoftwareBuildDtoUpdate,
    forced?: boolean,
    options?: any
  ) {
    return MajorSoftwareBuildApiFp(this.configuration).majorSoftwareBuildPut(
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
   * @memberof MajorSoftwareBuildApi
   */
  public majorSoftwareBuildRestore(id?: number, options?: any) {
    return MajorSoftwareBuildApiFp(
      this.configuration
    ).majorSoftwareBuildRestore(id, options)(this.fetch, this.basePath);
  }
}
