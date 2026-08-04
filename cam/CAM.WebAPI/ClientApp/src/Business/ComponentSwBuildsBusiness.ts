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
  CloneComponentSwBuildDto,
  ComponentSwBuildDtoCreate,
  ComponentSwBuildDtoUpdate,
  QueryResultDtoOfComponentSwBuildDtoGrid,
  ResultDtoOfComponentSwBuildToCloneDto,
  ComponentSwBuildQueryObjectGrid as ComponentSwBuildQueryDto,
} from "../Model/ComponentSwBuild";
import { BaseAPI } from "./Common/CommonBusiness";
import { ReturnFile } from "../Model/Common";
/**
 * ComponentSwBuildApi - fetch parameter creator
 * @export
 */
export const ComponentSwBuildApiFetchParamCreator = function (
  configuration?: Configuration
) {
  return {
    /**
     *
     * @param {DataRemediationDto} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    componentSwBuildApplyDataRemediation(
      body: DataRemediationDto,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling componentSwBuildApplyDataRemediation."
        );
      }
      const localVarPath = `/api/ComponentSoftwareBuild/ApplyDataRemediation`;
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
     * @param {CloneComponentSwBuildDto} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    componentSwBuildCloneComponentSwBuild(
      body: CloneComponentSwBuildDto,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling componentSwBuildCloneComponentSwBuild."
        );
      }
      const localVarPath = `/api/ComponentSoftwareBuild/UpgradeClonedComponentSwBuild`;
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
        <any>"CloneComponentSwBuildDto" !== "string" ||
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
     * @param {ComponentSwBuildDtoCreate} body
     * @param {boolean} [forced]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    componentSwBuildCreate(
      body: ComponentSwBuildDtoCreate,
      forced?: boolean,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling componentSwBuildCreate."
        );
      }
      const localVarPath = `/api/ComponentSoftwareBuild/CreateNewComponentSwBuild`;
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
        <any>"ComponentSwBuildDtoCreate" !== "string" ||
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
    componentSwBuildDelete(id?: number, options: any = {}): FetchArgs {
      const localVarPath = `/api/ComponentSoftwareBuild/DeleteComponentSwBuild`;
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
    componentSwBuildDeleteDeep(id?: number, options: any = {}): FetchArgs {
      const localVarPath = `/api/ComponentSoftwareBuild/DeleteDeep`;
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
    componentSwBuildDeleteOrphans(options: any = {}): FetchArgs {
      const localVarPath = `/api/ComponentSoftwareBuild/DeleteOrphans`;
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
     * @param {ComponentSwBuildQueryDto} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    componentSwBuildExportReport(
      body: ComponentSwBuildQueryDto,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling componentSwBuildExportReport."
        );
      }
      const localVarPath = `/api/ComponentSoftwareBuild/ExportReport`;
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
        <any>"ComponentSwBuildQueryDto" !== "string" ||
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
    componentSwBuildGetCreateResourceComponentSwBuild(
      options: any = {}
    ): FetchArgs {
      const localVarPath = `/api/ComponentSoftwareBuild/CreateComponentSwBuild`;
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
     * @param {ComponentSwBuildQueryDto} body
     * @param {string} [propertyName]
     * @param {string} [propertyFilter]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    componentSwBuildGetFilterResult(
      body: ComponentSwBuildQueryDto,
      propertyName?: string,
      propertyFilter?: string,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling componentSwBuildGetFilterResult."
        );
      }
      const localVarPath = `/api/ComponentSoftwareBuild/FilterComponentSwBuild`;
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
        <any>"ComponentSwBuildQueryDto" !== "string" ||
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
    componentSwBuildGetInfoComponentSwBuildToClone(
      id: number,
      flag?: boolean,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'id' is not null or undefined
      if (id === null || id === undefined) {
        throw new RequiredError(
          "id",
          "Required parameter id was null or undefined when calling componentSwBuildGetInfoComponentSwBuildToClone."
        );
      }
      const localVarPath =
        `/api/ComponentSoftwareBuild/GetInfoComponentSwBuildToClone{id}`.replace(
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
     * @param {ComponentSwBuildQueryDto} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    componentSwBuildGetComponentSwBuild(
      body: ComponentSwBuildQueryDto,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling componentSwBuildGetComponentSwBuild."
        );
      }
      const localVarPath = `/api/ComponentSoftwareBuild/GetComponentSoftwareBuilds`;
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
        <any>"ComponentSwBuildQueryDto" !== "string" ||
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
    componentSwBuildGetComponentSwBuildToClone(
      id: number,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'id' is not null or undefined
      if (id === null || id === undefined) {
        throw new RequiredError(
          "id",
          "Required parameter id was null or undefined when calling componentSwBuildGetComponentSwBuildToClone."
        );
      }
      const localVarPath =
        `/api/ComponentSoftwareBuild/GetComponentSwBuildForClone{id}`.replace(
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
    componentSwBuildGetSystemTypeForAddMajorSW(
      body: any,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling componentSwBuildGetSystemTypeForAddMajorSW."
        );
      }
      const localVarPath = `/api/ComponentSoftwareBuild/GetSystemTypeForAddMajorSW`;
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
        <any>"ComponentSwBuildQueryDto" !== "string" ||
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
    componentSwBuildGetRelatedRecords(
      id: number,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'id' is not null or undefined
      if (id === null || id === undefined) {
        throw new RequiredError(
          "id",
          "Required parameter id was null or undefined when calling componentSwBuildGetRelatedRecords."
        );
      }
      const localVarPath =
        `/api/ComponentSoftwareBuild/GetReferenceRecord{id}`.replace(
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
    componentSwBuildGetUpdateResourceComponentSwBuild(
      id: number,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'id' is not null or undefined
      if (id === null || id === undefined) {
        throw new RequiredError(
          "id",
          "Required parameter id was null or undefined when calling componentSwBuildGetUpdateResourceComponentSwBuild."
        );
      }
      const localVarPath =
        `/api/ComponentSoftwareBuild/EditComponentSwBuild{id}`.replace(
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
     * @param {ComponentSwBuildDtoUpdate} body
     * @param {boolean} [forced]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    componentSwBuildPut(
      body: ComponentSwBuildDtoUpdate,
      forced?: boolean,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling componentSwBuildPut."
        );
      }
      const localVarPath = `/api/ComponentSoftwareBuild/UpdateComponentSwBuild`;
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
        <any>"ComponentSwBuildDtoUpdate" !== "string" ||
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
    componentSwBuildRestore(id?: number, options: any = {}): FetchArgs {
      const localVarPath = `/api/ComponentSoftwareBuild/Restore`;
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
 * ComponentSwBuildApi - functional programming interface
 * @export
 */
export const ComponentSwBuildApiFp = function (configuration?: Configuration) {
  return {
    /**
     *
     * @param {DataRemediationDto} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    componentSwBuildApplyDataRemediation(
      body: DataRemediationDto,
      options?: any
    ): (
      fetch?: FetchAPI,
      basePath?: string
    ) => Promise<ResultDtoOfResultDataRemediationDto> {
      const localVarFetchArgs = ComponentSwBuildApiFetchParamCreator(
        configuration
      ).componentSwBuildApplyDataRemediation(body, options);
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
     * @param {CloneComponentSwBuildDto} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    componentSwBuildCloneComponentSwBuild(
      body: CloneComponentSwBuildDto,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = ComponentSwBuildApiFetchParamCreator(
        configuration
      ).componentSwBuildCloneComponentSwBuild(body, options);
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
     * @param {ComponentSwBuildDtoCreate} body
     * @param {boolean} [forced]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    componentSwBuildCreate(
      body: ComponentSwBuildDtoCreate,
      forced?: boolean,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = ComponentSwBuildApiFetchParamCreator(
        configuration
      ).componentSwBuildCreate(body, forced, options);
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
    componentSwBuildDelete(
      id?: number,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = ComponentSwBuildApiFetchParamCreator(
        configuration
      ).componentSwBuildDelete(id, options);
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
    componentSwBuildDeleteDeep(
      id?: number,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = ComponentSwBuildApiFetchParamCreator(
        configuration
      ).componentSwBuildDeleteDeep(id, options);
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
    componentSwBuildDeleteOrphans(
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs =
        ComponentSwBuildApiFetchParamCreator(
          configuration
        ).componentSwBuildDeleteOrphans(options);
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
     * @param {ComponentSwBuildQueryDto} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    componentSwBuildExportReport(
      body: ComponentSwBuildQueryDto,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ReturnFile> {
      const localVarFetchArgs = ComponentSwBuildApiFetchParamCreator(
        configuration
      ).componentSwBuildExportReport(body, options);
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
    componentSwBuildGetCreateResourceComponentSwBuild(
      options?: any
    ): (
      fetch?: FetchAPI,
      basePath?: string
    ) => Promise<ComponentSwBuildDtoCreate> {
      const localVarFetchArgs =
        ComponentSwBuildApiFetchParamCreator(
          configuration
        ).componentSwBuildGetCreateResourceComponentSwBuild(options);
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
     * @param {ComponentSwBuildQueryDto} body
     * @param {string} [propertyName]
     * @param {string} [propertyFilter]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    componentSwBuildGetFilterResult(
      body: ComponentSwBuildQueryDto,
      propertyName?: string,
      propertyFilter?: string,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<Array<FilterValueDto>> {
      const localVarFetchArgs = ComponentSwBuildApiFetchParamCreator(
        configuration
      ).componentSwBuildGetFilterResult(
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
    componentSwBuildGetInfoComponentSwBuildToClone(
      id: number,
      flag?: boolean,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = ComponentSwBuildApiFetchParamCreator(
        configuration
      ).componentSwBuildGetInfoComponentSwBuildToClone(id, flag, options);
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
     * @param {ComponentSwBuildQueryDto} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    componentSwBuildGetComponentSwBuild(
      body: ComponentSwBuildQueryDto,
      options?: any
    ): (
      fetch?: FetchAPI,
      basePath?: string
    ) => Promise<QueryResultDtoOfComponentSwBuildDtoGrid> {
      const localVarFetchArgs = ComponentSwBuildApiFetchParamCreator(
        configuration
      ).componentSwBuildGetComponentSwBuild(body, options);
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
    componentSwBuildGetComponentSwBuildToClone(
      id: number,
      options?: any
    ): (
      fetch?: FetchAPI,
      basePath?: string
    ) => Promise<ResultDtoOfComponentSwBuildToCloneDto> {
      const localVarFetchArgs = ComponentSwBuildApiFetchParamCreator(
        configuration
      ).componentSwBuildGetComponentSwBuildToClone(id, options);
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
    componentSwBuildGetSystemTypeForAddMajorSW(
      body: any,
      options?: any
    ): (
      fetch?: FetchAPI,
      basePath?: string
    ) => Promise<ResultDtoOfComponentSwBuildToCloneDto> {
      const localVarFetchArgs = ComponentSwBuildApiFetchParamCreator(
        configuration
      ).componentSwBuildGetSystemTypeForAddMajorSW(body, options);
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
    componentSwBuildGetRelatedRecords(
      id: number,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = ComponentSwBuildApiFetchParamCreator(
        configuration
      ).componentSwBuildGetRelatedRecords(id, options);
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
    componentSwBuildGetUpdateResourceComponentSwBuild(
      id: number,
      options?: any
    ): (
      fetch?: FetchAPI,
      basePath?: string
    ) => Promise<ComponentSwBuildDtoUpdate> {
      const localVarFetchArgs = ComponentSwBuildApiFetchParamCreator(
        configuration
      ).componentSwBuildGetUpdateResourceComponentSwBuild(id, options);
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
     * @param {ComponentSwBuildDtoUpdate} body
     * @param {boolean} [forced]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    componentSwBuildPut(
      body: ComponentSwBuildDtoUpdate,
      forced?: boolean,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = ComponentSwBuildApiFetchParamCreator(
        configuration
      ).componentSwBuildPut(body, forced, options);
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
    componentSwBuildRestore(
      id?: number,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = ComponentSwBuildApiFetchParamCreator(
        configuration
      ).componentSwBuildRestore(id, options);
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
 * ComponentSwBuildApi - factory interface
 * @export
 */
export const ComponentSwBuildApiFactory = function (
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
    componentSwBuildApplyDataRemediation(
      body: DataRemediationDto,
      options?: any
    ) {
      return ComponentSwBuildApiFp(
        configuration
      ).componentSwBuildApplyDataRemediation(body, options)(fetch, basePath);
    },
    /**
     *
     * @param {CloneComponentSwBuildDto} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    componentSwBuildCloneComponentSwBuild(
      body: CloneComponentSwBuildDto,
      options?: any
    ) {
      return ComponentSwBuildApiFp(
        configuration
      ).componentSwBuildCloneComponentSwBuild(body, options)(fetch, basePath);
    },
    /**
     *
     * @param {ComponentSwBuildDtoCreate} body
     * @param {boolean} [forced]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    componentSwBuildCreate(
      body: ComponentSwBuildDtoCreate,
      forced?: boolean,
      options?: any
    ) {
      return ComponentSwBuildApiFp(configuration).componentSwBuildCreate(
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
    componentSwBuildDelete(id?: number, options?: any) {
      return ComponentSwBuildApiFp(configuration).componentSwBuildDelete(
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
    componentSwBuildDeleteDeep(id?: number, options?: any) {
      return ComponentSwBuildApiFp(configuration).componentSwBuildDeleteDeep(
        id,
        options
      )(fetch, basePath);
    },
    /**
     *
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    componentSwBuildDeleteOrphans(options?: any) {
      return ComponentSwBuildApiFp(configuration).componentSwBuildDeleteOrphans(
        options
      )(fetch, basePath);
    },
    /**
     *
     * @param {ComponentSwBuildQueryDto} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    componentSwBuildExportReport(
      body: ComponentSwBuildQueryDto,
      options?: any
    ) {
      return ComponentSwBuildApiFp(configuration).componentSwBuildExportReport(
        body,
        options
      )(fetch, basePath);
    },
    /**
     *
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    componentSwBuildGetCreateResourceComponentSwBuild(options?: any) {
      return ComponentSwBuildApiFp(
        configuration
      ).componentSwBuildGetCreateResourceComponentSwBuild(options)(
        fetch,
        basePath
      );
    },
    /**
     *
     * @param {ComponentSwBuildQueryDto} body
     * @param {string} [propertyName]
     * @param {string} [propertyFilter]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    componentSwBuildGetFilterResult(
      body: ComponentSwBuildQueryDto,
      propertyName?: string,
      propertyFilter?: string,
      options?: any
    ) {
      return ComponentSwBuildApiFp(
        configuration
      ).componentSwBuildGetFilterResult(
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
    componentSwBuildGetInfoComponentSwBuildToClone(
      id: number,
      flag: boolean,
      options?: any
    ) {
      return ComponentSwBuildApiFp(
        configuration
      ).componentSwBuildGetInfoComponentSwBuildToClone(
        id,
        flag,
        options
      )(fetch, basePath);
    },
    /**
     *
     * @param {ComponentSwBuildQueryDto} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    componentSwBuildGetComponentSwBuild(
      body: ComponentSwBuildQueryDto,
      options?: any
    ) {
      return ComponentSwBuildApiFp(
        configuration
      ).componentSwBuildGetComponentSwBuild(body, options)(fetch, basePath);
    },
    /**
     *
     * @param {number} id
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    componentSwBuildGetComponentSwBuildToClone(id: number, options?: any) {
      return ComponentSwBuildApiFp(
        configuration
      ).componentSwBuildGetComponentSwBuildToClone(id, options)(
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
    componentSwBuildGetSystemTypeForAddMajorSW(body: any, options?: any) {
      return ComponentSwBuildApiFp(
        configuration
      ).componentSwBuildGetSystemTypeForAddMajorSW(body, options)(
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
    componentSwBuildGetRelatedRecords(id: number, options?: any) {
      return ComponentSwBuildApiFp(
        configuration
      ).componentSwBuildGetRelatedRecords(id, options)(fetch, basePath);
    },
    /**
     *
     * @param {number} id
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    componentSwBuildGetUpdateResourceComponentSwBuild(
      id: number,
      options?: any
    ) {
      return ComponentSwBuildApiFp(
        configuration
      ).componentSwBuildGetUpdateResourceComponentSwBuild(id, options)(
        fetch,
        basePath
      );
    },
    /**
     *
     * @param {ComponentSwBuildDtoUpdate} body
     * @param {boolean} [forced]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    componentSwBuildPut(
      body: ComponentSwBuildDtoUpdate,
      forced?: boolean,
      options?: any
    ) {
      return ComponentSwBuildApiFp(configuration).componentSwBuildPut(
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
    componentSwBuildRestore(id?: number, options?: any) {
      return ComponentSwBuildApiFp(configuration).componentSwBuildRestore(
        id,
        options
      )(fetch, basePath);
    },
  };
};

/**
 * ComponentSwBuildApi - object-oriented interface
 * @export
 * @class ComponentSwBuildApi
 * @extends {BaseAPI}
 */
export class ComponentSwBuildApi extends BaseAPI {
  /**
   *
   * @param {DataRemediationDto} body
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof ComponentSwBuildApi
   */
  public componentSwBuildApplyDataRemediation(
    body: DataRemediationDto,
    options?: any
  ) {
    return ComponentSwBuildApiFp(
      this.configuration
    ).componentSwBuildApplyDataRemediation(body, options)(
      this.fetch,
      this.basePath
    );
  }

  /**
   *
   * @param {CloneComponentSwBuildDto} body
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof ComponentSwBuildApi
   */
  public componentSwBuildCloneComponentSwBuild(
    body: CloneComponentSwBuildDto,
    options?: any
  ) {
    return ComponentSwBuildApiFp(
      this.configuration
    ).componentSwBuildCloneComponentSwBuild(body, options)(
      this.fetch,
      this.basePath
    );
  }

  /**
   *
   * @param {ComponentSwBuildDtoCreate} body
   * @param {boolean} [forced]
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof ComponentSwBuildApi
   */
  public componentSwBuildCreate(
    body: ComponentSwBuildDtoCreate,
    forced?: boolean,
    options?: any
  ) {
    return ComponentSwBuildApiFp(this.configuration).componentSwBuildCreate(
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
   * @memberof ComponentSwBuildApi
   */
  public componentSwBuildDelete(id?: number, options?: any) {
    return ComponentSwBuildApiFp(this.configuration).componentSwBuildDelete(
      id,
      options
    )(this.fetch, this.basePath);
  }

  /**
   *
   * @param {number} [id]
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof ComponentSwBuildApi
   */
  public componentSwBuildDeleteDeep(id?: number, options?: any) {
    return ComponentSwBuildApiFp(this.configuration).componentSwBuildDeleteDeep(
      id,
      options
    )(this.fetch, this.basePath);
  }

  /**
   *
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof ComponentSwBuildApi
   */
  public componentSwBuildDeleteOrphans(options?: any) {
    return ComponentSwBuildApiFp(
      this.configuration
    ).componentSwBuildDeleteOrphans(options)(this.fetch, this.basePath);
  }

  /**
   *
   * @param {ComponentSwBuildQueryDto} body
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof ComponentSwBuildApi
   */
  public componentSwBuildExportReport(
    body: ComponentSwBuildQueryDto,
    options?: any
  ) {
    return ComponentSwBuildApiFp(
      this.configuration
    ).componentSwBuildExportReport(body, options)(this.fetch, this.basePath);
  }

  /**
   *
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof ComponentSwBuildApi
   */
  public componentSwBuildGetCreateResourceComponentSwBuild(options?: any) {
    return ComponentSwBuildApiFp(
      this.configuration
    ).componentSwBuildGetCreateResourceComponentSwBuild(options)(
      this.fetch,
      this.basePath
    );
  }

  /**
   *
   * @param {ComponentSwBuildQueryDto} body
   * @param {string} [propertyName]
   * @param {string} [propertyFilter]
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof ComponentSwBuildApi
   */
  public componentSwBuildGetFilterResult(
    body: ComponentSwBuildQueryDto,
    propertyName?: string,
    propertyFilter?: string,
    options?: any
  ) {
    return ComponentSwBuildApiFp(
      this.configuration
    ).componentSwBuildGetFilterResult(
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
   * @memberof ComponentSwBuildApi
   */
  public componentSwBuildGetInfoComponentSwBuildToClone(
    id: number,
    flag?: boolean,
    options?: any
  ) {
    return ComponentSwBuildApiFp(
      this.configuration
    ).componentSwBuildGetInfoComponentSwBuildToClone(
      id,
      flag,
      options
    )(this.fetch, this.basePath);
  }

  /**
   *
   * @param {ComponentSwBuildQueryDto} body
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof ComponentSwBuildApi
   */
  public componentSwBuildGetComponentSwBuild(
    body: ComponentSwBuildQueryDto,
    options?: any
  ) {
    return ComponentSwBuildApiFp(
      this.configuration
    ).componentSwBuildGetComponentSwBuild(body, options)(
      this.fetch,
      this.basePath
    );
  }

  /**
   *
   * @param {number} id
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof ComponentSwBuildApi
   */
  public componentSwBuildGetComponentSwBuildToClone(id: number, options?: any) {
    return ComponentSwBuildApiFp(
      this.configuration
    ).componentSwBuildGetComponentSwBuildToClone(id, options)(
      this.fetch,
      this.basePath
    );
  }

  /**
   *
   * @param {any} body
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof ComponentSwBuildApi
   */
  public componentSwBuildGetSystemTypeForAddMajorSW(body: any, options?: any) {
    return ComponentSwBuildApiFp(
      this.configuration
    ).componentSwBuildGetSystemTypeForAddMajorSW(body, options)(
      this.fetch,
      this.basePath
    );
  }

  /**
   *
   * @param {number} id
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof ComponentSwBuildApi
   */
  public componentSwBuildGetRelatedRecords(id: number, options?: any) {
    return ComponentSwBuildApiFp(
      this.configuration
    ).componentSwBuildGetRelatedRecords(id, options)(this.fetch, this.basePath);
  }

  /**
   *
   * @param {number} id
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof ComponentSwBuildApi
   */
  public componentSwBuildGetUpdateResourceComponentSwBuild(
    id: number,
    options?: any
  ) {
    return ComponentSwBuildApiFp(
      this.configuration
    ).componentSwBuildGetUpdateResourceComponentSwBuild(id, options)(
      this.fetch,
      this.basePath
    );
  }

  /**
   *
   * @param {ComponentSwBuildDtoUpdate} body
   * @param {boolean} [forced]
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof ComponentSwBuildApi
   */
  public componentSwBuildPut(
    body: ComponentSwBuildDtoUpdate,
    forced?: boolean,
    options?: any
  ) {
    return ComponentSwBuildApiFp(this.configuration).componentSwBuildPut(
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
   * @memberof ComponentSwBuildApi
   */
  public componentSwBuildRestore(id?: number, options?: any) {
    return ComponentSwBuildApiFp(this.configuration).componentSwBuildRestore(
      id,
      options
    )(this.fetch, this.basePath);
  }
}
