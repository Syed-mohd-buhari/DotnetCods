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
  QueryResultDtoOfCBOMDtoGrid,
  CBOMDtoCreate,
  CBOMDtoUpdate,
  CBOMClusterInfoQueryObjectGrid,
  QueryResultDtoOfCBOMClusterInfoDtoGrid,
} from "../Model/CBOM";
import { ReturnFile } from "../Model/Common";
import { CBOMQueryObjectGrid } from "../Model/CBOM";
/**
 * CBOMApi - fetch parameter creator
 * @export
 */
export const CBOMApiFetchParamCreator = function (
  configuration?: Configuration
) {
  return {
    /**
     * @param {CBOMQueryObjectGrid} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    CBOMGetGrid(body: CBOMQueryObjectGrid, options: any = {}): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling CBOMQueryObjectGrid."
        );
      }
      const localVarPath = `/api/Cbom/GetCbomClusterinfoDetails`;
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
        <any>"CBOMQueryObjectGrid" !== "string" ||
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
     * @param {CBOMClusterInfoQueryObjectGrid} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    CBOMCnfInstanceAndCapacityGetGrid(
      body: CBOMClusterInfoQueryObjectGrid,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling CBOMCnfInstanceAndCapacityGetGrid."
        );
      }
      const localVarPath = `/api/cBOM/GetCbomPodInfoAndCapacityDetails`;
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
        <any>"CBOMClusterInfoQueryObjectGrid" !== "string" ||
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
    CBOMGetCreateResourceCBOM(options: any = {}): FetchArgs {
      const localVarPath = `/api/CBOM/CreateCBomPageResource`;
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
    CBOMGetUpdateResourceCBOM(id: number, options: any = {}): FetchArgs {
      if (id === null || id === undefined) {
        throw new RequiredError(
          "id",
          "Required parameter id was null or undefined when calling CBOMGetUpdateResourceTestinfo."
        );
      }
      const localVarPath = `/api/CBOM/EditCBomPageResource{id}`.replace(
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
        <any>"CBOMDtoCreate" !== "string" ||
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
     * @param {CBOMDtoUpdate} body
     * @param {boolean} [forced]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    CBOMEdit(
      body: CBOMDtoUpdate,
      forced?: boolean,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling CBOMEdit."
        );
      }
      const localVarPath = `/api/CBOM/UpdateExistingCBomResource`;
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
        <any>"CBOMDtoUpdate" !== "string" ||
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
     * @param {CBOMDtoCreate} body
     * @param {boolean} [forced]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    CBOMCreate(
      body: CBOMDtoCreate,
      forced?: boolean,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling CBOMCreate."
        );
      }
      const localVarPath = `/api/CBOM/AddNewCBomResource`;
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
        <any>"CBOMDtoCreate" !== "string" ||
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
     * @param {string} [apiType]
     * @param {number} [id]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    CBOMDelete(id: number, apiType?: string, options: any = {}): FetchArgs {
      const localVarPath = `/api/CBOM/DeleteCbomEntity`;
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
      localVarQueryParameter["infoId"] = apiType === "infoId" ? id : 0;
      localVarQueryParameter["clusterInfoId"] =
        apiType === "instanceId" ? id : 0;
      localVarQueryParameter["capacityId"] = apiType === "capacityId" ? id : 0;

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
    CBOMDeleteDeep(id?: number, options: any = {}): FetchArgs {
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
    CBOMGetRelatedRecords(id: number, options: any = {}): FetchArgs {
      // verify required parameter 'id' is not null or undefined
      if (id === null || id === undefined) {
        throw new RequiredError(
          "id",
          "Required parameter id was null or undefined when calling CBOMGetRelatedRecords."
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
     * @param {CBOMQueryObjectGrid} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    CBOMExportReport(body: CBOMQueryObjectGrid, options: any = {}): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling CBOMExportReport."
        );
      }
      const localVarPath = `/api/CBOM/ExportReport`;
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
        <any>"CBOMQueryObjectGrid" !== "string" ||
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
     * @param {CBOMQueryObjectGrid} body
     * @param {boolean} [forced]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    CBOMImportExcel(file: File, options: any = {}): FetchArgs {
      if (!file) {
        throw new RequiredError(
          "file",
          "Required parameter file was null or undefined when calling CBOMImportExcel."
        );
      }

      const localVarPath = `/api/CBOM/import`;
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
    /**
     *
     * @param {CBOMQueryObjectGrid} body
     * @param {string} [propertyName]
     * @param {string} [propertyFilter]
     * @param {boolean} [isInstance]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    CBOMGetFilterResult(
      body: CBOMQueryObjectGrid,
      propertyName?: string,
      propertyFilter?: string,
      isInstance?: boolean,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling CBOMGetFilterResult."
        );
      }
      const localVarPath = `/api/CBOM/Filter`;
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
      if (isInstance !== undefined) {
        localVarQueryParameter["isInstance"] = isInstance;
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
        <any>"CBOMQueryObjectGrid" !== "string" ||
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
     * @param {CBOMQueryObjectGrid} body
     * @param {string} [propertyName]
     * @param {string} [propertyFilter]
     * @param {boolean} [isInstance]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    CBOMCnfCapacityFilter(
      body: CBOMQueryObjectGrid,
      propertyName?: string,
      propertyFilter?: string,
      isInstance?: boolean,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling CBOMCnfCapacityFilterResult."
        );
      }
      const localVarPath = `/api/CBOM/CnfCapacityFilter`;
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
      if (isInstance !== undefined) {
        localVarQueryParameter["isInstance"] = isInstance;
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
        <any>"CBOMQueryObjectGrid" !== "string" ||
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
    CBOMRestore(id?: number, options: any = {}): FetchArgs {
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
 * CBOMApi - functional programming interface
 * @export
 */
export const CBOMApiFp = function (configuration?: Configuration) {
  return {
    /**
     *
     * @param {string} [apiType]
     * @param {number} [id]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    CBOMDelete(
      id: number,
      apiType?: string,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = CBOMApiFetchParamCreator(
        configuration
      ).CBOMDelete(id, apiType, options);
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
    CBOMDeleteDeep(
      id?: number,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = CBOMApiFetchParamCreator(
        configuration
      ).CBOMDeleteDeep(id, options);
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
    CBOMGetRelatedRecords(
      id: number,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = CBOMApiFetchParamCreator(
        configuration
      ).CBOMGetRelatedRecords(id, options);
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
     * @param {CBOMQueryObjectGrid} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    CBOMExportReport(
      body: CBOMQueryObjectGrid,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ReturnFile> {
      const localVarFetchArgs = CBOMApiFetchParamCreator(
        configuration
      ).CBOMExportReport(body, options);
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

    CBOMImportExcel(
      file: File,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = CBOMApiFetchParamCreator(
        configuration
      ).CBOMImportExcel(file, options);

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
    CBOMGetCreateResourceCBOM(
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<CBOMDtoCreate> {
      const localVarFetchArgs =
        CBOMApiFetchParamCreator(configuration).CBOMGetCreateResourceCBOM(
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
     * @param {number} id
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    CBOMGetUpdateResourceCBOM(
      id: number,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<CBOMDtoUpdate> {
      const localVarFetchArgs = CBOMApiFetchParamCreator(
        configuration
      ).CBOMGetUpdateResourceCBOM(id, options);
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
      const localVarFetchArgs = CBOMApiFetchParamCreator(
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
      const localVarFetchArgs = CBOMApiFetchParamCreator(
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
     * @param {CBOMQueryObjectGrid} body
     * @param {string} [propertyName]
     * @param {string} [propertyFilter]
     * @param {boolean} [isInstance]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    CBOMGetFilterResult(
      body: CBOMQueryObjectGrid,
      propertyName?: string,
      propertyFilter?: string,
      isInstance?: boolean,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<Array<FilterValueDto>> {
      const localVarFetchArgs = CBOMApiFetchParamCreator(
        configuration
      ).CBOMGetFilterResult(
        body,
        propertyName,
        propertyFilter,
        isInstance,
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
     * @param {CBOMQueryObjectGrid} body
     * @param {string} [propertyName]
     * @param {string} [propertyFilter]
     * @param {boolean} [isInstance]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    CBOMCnfCapacityFilter(
      body: CBOMQueryObjectGrid,
      propertyName?: string,
      propertyFilter?: string,
      isInstance?: boolean,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<Array<FilterValueDto>> {
      const localVarFetchArgs = CBOMApiFetchParamCreator(
        configuration
      ).CBOMCnfCapacityFilter(
        body,
        propertyName,
        propertyFilter,
        isInstance,
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
     * @param {CBOMQueryObjectGrid} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    CBOMGetGrid(
      body: CBOMQueryObjectGrid,
      options?: any
    ): (
      fetch?: FetchAPI,
      basePath?: string
    ) => Promise<QueryResultDtoOfCBOMDtoGrid> {
      const localVarFetchArgs = CBOMApiFetchParamCreator(
        configuration
      ).CBOMGetGrid(body, options);
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
     * @param {CBOMClusterInfoQueryObjectGrid} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    CBOMCnfInstanceAndCapacityGetGrid(
      body: CBOMClusterInfoQueryObjectGrid,
      options?: any
    ): (
      fetch?: FetchAPI,
      basePath?: string
    ) => Promise<QueryResultDtoOfCBOMClusterInfoDtoGrid> {
      const localVarFetchArgs = CBOMApiFetchParamCreator(
        configuration
      ).CBOMCnfInstanceAndCapacityGetGrid(body, options);
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
     * @param {CBOMDtoUpdate} body
     * @param {boolean} [forced]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    CBOMEdit(
      body: CBOMDtoUpdate,
      forced?: boolean,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = CBOMApiFetchParamCreator(
        configuration
      ).CBOMEdit(body, forced, options);
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
     * @param {CBOMDtoCreate} body
     * @param {boolean} [forced]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    CBOMCreate(
      body: CBOMDtoCreate,
      forced?: boolean,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = CBOMApiFetchParamCreator(
        configuration
      ).CBOMCreate(body, forced, options);
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
    CBOMRestore(
      id?: number,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = CBOMApiFetchParamCreator(
        configuration
      ).CBOMRestore(id, options);
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
 * CBOMApi - factory interface
 * @export
 */
export const CBOMApiFactory = function (
  configuration?: Configuration,
  fetch?: FetchAPI,
  basePath?: string
) {
  return {
    /**
     *
     * @param {string} [apiType]
     * @param {number} [id]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    CBOMDelete(id: number, apiType?: string, options?: any) {
      return CBOMApiFp(configuration).CBOMDelete(
        id,
        apiType,
        options
      )(fetch, basePath);
    },
    /**
     *
     * @param {number} [id]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    CBOMDeleteDeep(id?: number, options?: any) {
      return CBOMApiFp(configuration).CBOMDeleteDeep(id, options)(
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
    CBOMGetRelatedRecords(id: number, options?: any) {
      return CBOMApiFp(configuration).CBOMGetRelatedRecords(id, options)(
        fetch,
        basePath
      );
    },
    /**
     *
     * @param {CBOMQueryObjectGrid} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    CBOMExportReport(body: CBOMQueryObjectGrid, options?: any) {
      return CBOMApiFp(configuration).CBOMExportReport(body, options)(
        fetch,
        basePath
      );
    },
    /**
     *
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    CBOMImportExcel(file: File, options?: any): Promise<ResultDto> {
      return CBOMApiFp(configuration).CBOMImportExcel(file, options)(
        fetch,
        basePath
      );
    },
    /**
     *
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    CBOMGetCreateResourceCBOM(options?: any) {
      return CBOMApiFp(configuration).CBOMGetCreateResourceCBOM(options)(
        fetch,
        basePath
      );
    },
    /**
     * @param {number} id
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    CBOMGetUpdateResourceCBOM(id: number, options?: any) {
      return CBOMApiFp(configuration).CBOMGetUpdateResourceCBOM(id, options)(
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
      return CBOMApiFp(configuration).GetAllOpCosAndVerticalResponsibles(
        id,
        options
      )(fetch, basePath);
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
      return CBOMApiFp(
        configuration
      ).GetOpCosAndVerticalAndSubDomainResponsibles(id, options)(
        fetch,
        basePath
      );
    },
    /**
     *
     * @param {CBOMQueryObjectGrid} body
     * @param {string} [propertyName]
     * @param {string} [propertyFilter]
     * @param {boolean} [isInstance]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    CBOMGetFilterResult(
      body: CBOMQueryObjectGrid,
      propertyName?: string,
      propertyFilter?: string,
      isInstance?: boolean,
      options?: any
    ) {
      return CBOMApiFp(configuration).CBOMGetFilterResult(
        body,
        propertyName,
        propertyFilter,
        isInstance,
        options
      )(fetch, basePath);
    },
    /**
     *
     * @param {CBOMQueryObjectGrid} body
     * @param {string} [propertyName]
     * @param {string} [propertyFilter]
     * @param {boolean} [isInstance]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    CBOMCnfCapacityFilter(
      body: CBOMQueryObjectGrid,
      propertyName?: string,
      propertyFilter?: string,
      isInstance?: boolean,
      options?: any
    ) {
      return CBOMApiFp(configuration).CBOMCnfCapacityFilter(
        body,
        propertyName,
        propertyFilter,
        isInstance,
        options
      )(fetch, basePath);
    },
    /**
     * @param {CBOMQueryObjectGrid} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    CBOMGetGrid(body: CBOMQueryObjectGrid, options?: any) {
      return CBOMApiFp(configuration).CBOMGetGrid(body, options)(
        fetch,
        basePath
      );
    },
    /**
     * @param {CBOMClusterInfoQueryObjectGrid} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    CBOMCnfInstanceAndCapacityGetGrid(
      body: CBOMClusterInfoQueryObjectGrid,
      options?: any
    ) {
      return CBOMApiFp(configuration).CBOMCnfInstanceAndCapacityGetGrid(
        body,
        options
      )(fetch, basePath);
    },
    /**
     *
     * @param {CBOMDtoUpdate} body
     * @param {boolean} [forced]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    CBOMEdit(body: CBOMDtoUpdate, forced?: boolean, options?: any) {
      return CBOMApiFp(configuration).CBOMEdit(
        body,
        forced,
        options
      )(fetch, basePath);
    },
    /**
     *
     * @param {CBOMDtoCreate} body
     * @param {boolean} [forced]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    CBOMCreate(body: CBOMDtoCreate, forced?: boolean, options?: any) {
      return CBOMApiFp(configuration).CBOMCreate(
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
    CBOMRestore(id?: number, options?: any) {
      return CBOMApiFp(configuration).CBOMRestore(id, options)(fetch, basePath);
    },
  };
};

/**
 * CBOMApi - object-oriented interface
 * @export
 * @class CBOMApi
 * @extends {BaseAPI}
 */
export class CBOMApi extends BaseAPI {
  /**
   *
   * @param {string} [apiType]
   * @param {number} [id]
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof CBOMApi
   */
  public CBOMDelete(id: number, apiType?: string, options?: any) {
    return CBOMApiFp(this.configuration).CBOMDelete(
      id,
      apiType,
      options
    )(this.fetch, this.basePath);
  }

  /**
   *
   * @param {number} [id]
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof CBOMApi
   */
  public CBOMDeleteDeep(id?: number, options?: any) {
    return CBOMApiFp(this.configuration).CBOMDeleteDeep(id, options)(
      this.fetch,
      this.basePath
    );
  }

  /**
   *
   * @param {CBOMQueryObjectGrid} body
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof CBOMApi
   */
  public CBOMExportReport(body: CBOMQueryObjectGrid, options?: any) {
    return CBOMApiFp(this.configuration).CBOMExportReport(body, options)(
      this.fetch,
      this.basePath
    );
  }
  public async CBOMImportStatus(file: File) {
    const fetchArgs = CBOMApiFetchParamCreator(
      this.configuration
    ).CBOMImportExcel(file);

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
   * @memberof CBOMApi
   */
  public CBOMGetCreateResourceCBOM(options?: any) {
    return CBOMApiFp(this.configuration).CBOMGetCreateResourceCBOM(options)(
      this.fetch,
      this.basePath
    );
  }
  /**
   * @param {number} id
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof CBOMApi
   */
  public CBOMGetUpdateResourceCBOM(id: number, options?: any) {
    return CBOMApiFp(this.configuration).CBOMGetUpdateResourceCBOM(id, options)(
      this.fetch,
      this.basePath
    );
  }
  /**
   * @param {number} id
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof CBOMApi
   */
  public GetAllOpCosAndVerticalResponsibles(id: number, options?: any) {
    return CBOMApiFp(this.configuration).GetAllOpCosAndVerticalResponsibles(
      id,
      options
    )(this.fetch, this.basePath);
  }
  /**
   * @param {Array<number>} id
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof CBOMApi
   */
  public GetOpCosAndVerticalAndSubDomainResponsibles(
    id: Array<number>,
    options?: any
  ) {
    return CBOMApiFp(
      this.configuration
    ).GetOpCosAndVerticalAndSubDomainResponsibles(id, options)(
      this.fetch,
      this.basePath
    );
  }

  /**
   *
   * @param {CBOMQueryObjectGrid} body
   * @param {string} [propertyName]
   * @param {string} [propertyFilter]
   * @param {boolean} [isInstance]
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof CBOMApi
   */
  public CBOMGetFilterResult(
    body: CBOMQueryObjectGrid,
    propertyName?: string,
    propertyFilter?: string,
    isInstance?: boolean,
    options?: any
  ) {
    return CBOMApiFp(this.configuration).CBOMGetFilterResult(
      body,
      propertyName,
      propertyFilter,
      isInstance,
      options
    )(this.fetch, this.basePath);
  }
  /**
   *
   * @param {CBOMQueryObjectGrid} body
   * @param {string} [propertyName]
   * @param {string} [propertyFilter]
   * @param {boolean} [isInstance]
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof CBOMApi
   */
  public CBOMCnfCapacityFilter(
    body: CBOMQueryObjectGrid,
    propertyName?: string,
    propertyFilter?: string,
    isInstance?: boolean,
    options?: any
  ) {
    return CBOMApiFp(this.configuration).CBOMCnfCapacityFilter(
      body,
      propertyName,
      propertyFilter,
      isInstance,
      options
    )(this.fetch, this.basePath);
  }

  /**
   *
   * @param {CBOMQueryObjectGrid} body
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof CBOMApi
   */
  public CBOMGetGrid(body: CBOMQueryObjectGrid, options?: any) {
    return CBOMApiFp(this.configuration).CBOMGetGrid(body, options)(
      this.fetch,
      this.basePath
    );
  }
  /**
   *
   * @param {CBOMClusterInfoQueryObjectGrid} body
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof VBOMInfoApi
   */
  public CBOMCnfInstanceAndCapacityGetGrid(
    body: CBOMClusterInfoQueryObjectGrid,
    options?: any
  ) {
    return CBOMApiFp(this.configuration).CBOMCnfInstanceAndCapacityGetGrid(
      body,
      options
    )(this.fetch, this.basePath);
  }

  /**
   *
   * @param {number} id
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof CBOMApi
   */
  public CBOMGetRelatedRecords(id: number, options?: any) {
    return CBOMApiFp(this.configuration).CBOMGetRelatedRecords(id, options)(
      this.fetch,
      this.basePath
    );
  }

  /**
   *
   * @param {CBOMDtoUpdate} body
   * @param {boolean} [forced]
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof CBOMApi
   */
  public CBOMEdit(body: CBOMDtoUpdate, forced?: boolean, options?: any) {
    return CBOMApiFp(this.configuration).CBOMEdit(
      body,
      forced,
      options
    )(this.fetch, this.basePath);
  }

  /**
   *
   * @param {CBOMDtoCreate} body
   * @param {boolean} [forced]
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof CBOMApi
   */
  public CBOMCreate(body: CBOMDtoCreate, forced?: boolean, options?: any) {
    return CBOMApiFp(this.configuration).CBOMCreate(
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
   * @memberof CBOMApi
   */
  public CBOMRestore(id?: number, options?: any) {
    return CBOMApiFp(this.configuration).CBOMRestore(id, options)(
      this.fetch,
      this.basePath
    );
  }
}
