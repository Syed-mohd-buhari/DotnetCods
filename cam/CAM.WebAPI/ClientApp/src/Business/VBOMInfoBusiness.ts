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
  QueryResultDtoOfVBOMClusterInfoDtoGrid,
  QueryResultDtoOfVBOMInfoDtoGrid,
  VBOMClusterInfoQueryObjectGrid,
  VBOMInfoDtoCreate,
  VBOMInfoDtoUpdate,
} from "../Model/VBOMInfo";
import { ReturnFile } from "../Model/Common";
import { VBOMInfoQueryObjectGrid } from "../Model/VBOMInfo";
/**
 * VBOMInfoApi - fetch parameter creator
 * @export
 */
export const VBOMInfoApiFetchParamCreator = function (
  configuration?: Configuration
) {
  return {
    /**
     * @param {VBOMInfoQueryObjectGrid} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    VBOMInfoGetGrid(
      body: VBOMInfoQueryObjectGrid,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling VBOMInfoQueryObjectGrid."
        );
      }
      const localVarPath = `/api/VBOM/GetAllVnfinfoDetails`;
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
        <any>"VBOMInfoQueryObjectGrid" !== "string" ||
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
     * @param {VBOMClusterInfoQueryObjectGrid} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    VBOMClusterInfoGetGrid(
      body: VBOMClusterInfoQueryObjectGrid,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling VBOMClusterInfoQueryObjectGrid."
        );
      }
      const localVarPath = `/api/VBOM/GetVnfClusterinfoDetails`;
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
        <any>"VBOMClusterInfoQueryObjectGrid" !== "string" ||
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
     * @param {VBOMClusterInfoQueryObjectGrid} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    VBOMVnfInfoAndCapacityGetGrid(
      body: VBOMClusterInfoQueryObjectGrid,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling VBOMVnfInfoAndCapacityGetGrid."
        );
      }
      const localVarPath = `/api/VBOM/GetVnfInfoAndCapacityDetails`;
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
        <any>"VBOMClusterInfoQueryObjectGrid" !== "string" ||
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
    VBOMInfoGetCreateResourceVBOMInfo(options: any = {}): FetchArgs {
      const localVarPath = `/api/VBOM/CreateVnfVBomPageResource`;
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
    VBOMInfoGetUpdateResourceVBOMInfo(
      id: number,
      options: any = {}
    ): FetchArgs {
      if (id === null || id === undefined) {
        throw new RequiredError(
          "id",
          "Required parameter id was null or undefined when calling VBOMInfoGetUpdateResourceTestinfo."
        );
      }
      const localVarPath = `/api/VBOM/EditVnfVBomPageResource{id}`.replace(
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
        <any>"VBOMInfoDtoCreate" !== "string" ||
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
     * @param {VBOMInfoDtoUpdate} body
     * @param {boolean} [forced]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    VBOMInfoEdit(
      body: VBOMInfoDtoUpdate,
      forced?: boolean,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling VBOMInfoEdit."
        );
      }
      const localVarPath = `/api/VBOM/UpdateExistingVBomResource`;
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
        <any>"VBOMInfoDtoUpdate" !== "string" ||
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
     * @param {VBOMInfoDtoCreate} body
     * @param {boolean} [forced]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    VBOMInfoCreate(
      body: VBOMInfoDtoCreate,
      forced?: boolean,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling VBOMInfoCreate."
        );
      }
      const localVarPath = `/api/VBOM/AddNewVBomResource`;
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
        <any>"VBOMInfoDtoCreate" !== "string" ||
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
    VBOMInfoDelete(id: number, apiType?: string, options: any = {}): FetchArgs {
      const localVarPath = `/api/VBOM/DeleteVbomEntity`;
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
      localVarQueryParameter["instanceId"] = apiType === "instanceId" ? id : 0;
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
    VBOMInfoDeleteDeep(id?: number, options: any = {}): FetchArgs {
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
    VBOMInfoGetRelatedRecords(id: number, options: any = {}): FetchArgs {
      // verify required parameter 'id' is not null or undefined
      if (id === null || id === undefined) {
        throw new RequiredError(
          "id",
          "Required parameter id was null or undefined when calling VBOMInfoGetRelatedRecords."
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
     * @param {VBOMInfoQueryObjectGrid} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    VBOMInfoExportReport(
      body: VBOMInfoQueryObjectGrid,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling VBOMInfoExportReport."
        );
      }
      const localVarPath = `/api/VBOM/ExportReport`;
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
        <any>"VBOMInfoQueryObjectGrid" !== "string" ||
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
     * @param {VBOMInfoQueryObjectGrid} body
     * @param {boolean} [forced]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    VBOMInfoImportExcel(file: File, options: any = {}): FetchArgs {
      if (!file) {
        throw new RequiredError(
          "file",
          "Required parameter file was null or undefined when calling VBOMInfoImportExcel."
        );
      }

      const localVarPath = `/api/VBOM/import`;
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
     * @param {VBOMInfoQueryObjectGrid} body
     * @param {string} [propertyName]
     * @param {string} [propertyFilter]
     * @param {boolean} [isInstance]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    VBOMInfoGetFilterResult(
      body: VBOMInfoQueryObjectGrid,
      propertyName?: string,
      propertyFilter?: string,
      isInstance?: boolean,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling VBOMInfoGetFilterResult."
        );
      }
      const localVarPath = `/api/VBOM/Filter`;
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
        <any>"VBOMInfoQueryObjectGrid" !== "string" ||
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
     * @param {VBOMInfoQueryObjectGrid} body
     * @param {string} [propertyName]
     * @param {string} [propertyFilter]
     * @param {boolean} [isInstance]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    VBOMVnfCapacityFilter(
      body: VBOMInfoQueryObjectGrid,
      propertyName?: string,
      propertyFilter?: string,
      isInstance?: boolean,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling VBOMVnfCapacityFilter."
        );
      }
      const localVarPath = `/api/VBOM/VnfCapacityFilter`;
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
        <any>"VBOMInfoQueryObjectGrid" !== "string" ||
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
    VBOMInfoRestore(id?: number, options: any = {}): FetchArgs {
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
 * VBOMInfoApi - functional programming interface
 * @export
 */
export const VBOMInfoApiFp = function (configuration?: Configuration) {
  return {
    /**
     *
     * @param {string} [apiType]
     * @param {number} [id]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    VBOMInfoDelete(
      id: number,
      apiType?: string,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = VBOMInfoApiFetchParamCreator(
        configuration
      ).VBOMInfoDelete(id, apiType, options);
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
    VBOMInfoDeleteDeep(
      id?: number,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = VBOMInfoApiFetchParamCreator(
        configuration
      ).VBOMInfoDeleteDeep(id, options);
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
    VBOMInfoGetRelatedRecords(
      id: number,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = VBOMInfoApiFetchParamCreator(
        configuration
      ).VBOMInfoGetRelatedRecords(id, options);
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
     * @param {VBOMInfoQueryObjectGrid} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    VBOMInfoExportReport(
      body: VBOMInfoQueryObjectGrid,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ReturnFile> {
      const localVarFetchArgs = VBOMInfoApiFetchParamCreator(
        configuration
      ).VBOMInfoExportReport(body, options);
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

    VBOMInfoImportExcel(
      file: File,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = VBOMInfoApiFetchParamCreator(
        configuration
      ).VBOMInfoImportExcel(file, options);

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
    VBOMInfoGetCreateResourceVBOMInfo(
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<VBOMInfoDtoCreate> {
      const localVarFetchArgs =
        VBOMInfoApiFetchParamCreator(
          configuration
        ).VBOMInfoGetCreateResourceVBOMInfo(options);
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
    VBOMInfoGetUpdateResourceVBOMInfo(
      id: number,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<VBOMInfoDtoUpdate> {
      const localVarFetchArgs = VBOMInfoApiFetchParamCreator(
        configuration
      ).VBOMInfoGetUpdateResourceVBOMInfo(id, options);
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
      const localVarFetchArgs = VBOMInfoApiFetchParamCreator(
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
      const localVarFetchArgs = VBOMInfoApiFetchParamCreator(
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
     * @param {VBOMInfoQueryObjectGrid} body
     * @param {string} [propertyName]
     * @param {string} [propertyFilter]
     * @param {boolean} [isInstance]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    VBOMInfoGetFilterResult(
      body: VBOMInfoQueryObjectGrid,
      propertyName?: string,
      propertyFilter?: string,
      isInstance?: boolean,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<Array<FilterValueDto>> {
      const localVarFetchArgs = VBOMInfoApiFetchParamCreator(
        configuration
      ).VBOMInfoGetFilterResult(
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
     * @param {VBOMInfoQueryObjectGrid} body
     * @param {string} [propertyName]
     * @param {string} [propertyFilter]
     * @param {boolean} [isInstance]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    VBOMVnfCapacityFilter(
      body: VBOMInfoQueryObjectGrid,
      propertyName?: string,
      propertyFilter?: string,
      isInstance?: boolean,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<Array<FilterValueDto>> {
      const localVarFetchArgs = VBOMInfoApiFetchParamCreator(
        configuration
      ).VBOMVnfCapacityFilter(
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
     * @param {VBOMInfoQueryObjectGrid} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    VBOMInfoGetGrid(
      body: VBOMInfoQueryObjectGrid,
      options?: any
    ): (
      fetch?: FetchAPI,
      basePath?: string
    ) => Promise<QueryResultDtoOfVBOMInfoDtoGrid> {
      const localVarFetchArgs = VBOMInfoApiFetchParamCreator(
        configuration
      ).VBOMInfoGetGrid(body, options);
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
     * @param {VBOMClusterInfoQueryObjectGrid} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    VBOMClusterInfoGetGrid(
      body: VBOMClusterInfoQueryObjectGrid,
      options?: any
    ): (
      fetch?: FetchAPI,
      basePath?: string
    ) => Promise<QueryResultDtoOfVBOMClusterInfoDtoGrid> {
      const localVarFetchArgs = VBOMInfoApiFetchParamCreator(
        configuration
      ).VBOMClusterInfoGetGrid(body, options);
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
     * @param {VBOMClusterInfoQueryObjectGrid} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    VBOMVnfInfoAndCapacityGetGrid(
      body: VBOMClusterInfoQueryObjectGrid,
      options?: any
    ): (
      fetch?: FetchAPI,
      basePath?: string
    ) => Promise<QueryResultDtoOfVBOMClusterInfoDtoGrid> {
      const localVarFetchArgs = VBOMInfoApiFetchParamCreator(
        configuration
      ).VBOMVnfInfoAndCapacityGetGrid(body, options);
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
     * @param {VBOMInfoDtoUpdate} body
     * @param {boolean} [forced]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    VBOMInfoEdit(
      body: VBOMInfoDtoUpdate,
      forced?: boolean,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = VBOMInfoApiFetchParamCreator(
        configuration
      ).VBOMInfoEdit(body, forced, options);
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
     * @param {VBOMInfoDtoCreate} body
     * @param {boolean} [forced]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    VBOMInfoCreate(
      body: VBOMInfoDtoCreate,
      forced?: boolean,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = VBOMInfoApiFetchParamCreator(
        configuration
      ).VBOMInfoCreate(body, forced, options);
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
    VBOMInfoRestore(
      id?: number,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = VBOMInfoApiFetchParamCreator(
        configuration
      ).VBOMInfoRestore(id, options);
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
 * VBOMInfoApi - factory interface
 * @export
 */
export const VBOMInfoApiFactory = function (
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
    VBOMInfoDelete(id: number, apiType?: string, options?: any) {
      return VBOMInfoApiFp(configuration).VBOMInfoDelete(
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
    VBOMInfoDeleteDeep(id?: number, options?: any) {
      return VBOMInfoApiFp(configuration).VBOMInfoDeleteDeep(id, options)(
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
    VBOMInfoGetRelatedRecords(id: number, options?: any) {
      return VBOMInfoApiFp(configuration).VBOMInfoGetRelatedRecords(
        id,
        options
      )(fetch, basePath);
    },
    /**
     *
     * @param {VBOMInfoQueryObjectGrid} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    VBOMInfoExportReport(body: VBOMInfoQueryObjectGrid, options?: any) {
      return VBOMInfoApiFp(configuration).VBOMInfoExportReport(body, options)(
        fetch,
        basePath
      );
    },
    /**
     *
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    VBOMInfoImportExcel(file: File, options?: any): Promise<ResultDto> {
      return VBOMInfoApiFp(configuration).VBOMInfoImportExcel(file, options)(
        fetch,
        basePath
      );
    },
    /**
     *
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    VBOMInfoGetCreateResourceVBOMInfo(options?: any) {
      return VBOMInfoApiFp(configuration).VBOMInfoGetCreateResourceVBOMInfo(
        options
      )(fetch, basePath);
    },
    /**
     * @param {number} id
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    VBOMInfoGetUpdateResourceVBOMInfo(id: number, options?: any) {
      return VBOMInfoApiFp(configuration).VBOMInfoGetUpdateResourceVBOMInfo(
        id,
        options
      )(fetch, basePath);
    },
    /**
     * @param {number} id
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    GetAllOpCosAndVerticalResponsibles(id: number, options?: any) {
      return VBOMInfoApiFp(configuration).GetAllOpCosAndVerticalResponsibles(
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
      return VBOMInfoApiFp(
        configuration
      ).GetOpCosAndVerticalAndSubDomainResponsibles(id, options)(
        fetch,
        basePath
      );
    },
    /**
     *
     * @param {VBOMInfoQueryObjectGrid} body
     * @param {string} [propertyName]
     * @param {string} [propertyFilter]
     * @param {boolean} [isInstance]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    VBOMInfoGetFilterResult(
      body: VBOMInfoQueryObjectGrid,
      propertyName?: string,
      propertyFilter?: string,
      isInstance?: boolean,
      options?: any
    ) {
      return VBOMInfoApiFp(configuration).VBOMInfoGetFilterResult(
        body,
        propertyName,
        propertyFilter,
        isInstance,
        options
      )(fetch, basePath);
    },
    /**
     *
     * @param {VBOMInfoQueryObjectGrid} body
     * @param {string} [propertyName]
     * @param {string} [propertyFilter]
     * @param {boolean} [isInstance]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    VBOMVnfCapacityFilter(
      body: VBOMInfoQueryObjectGrid,
      propertyName?: string,
      propertyFilter?: string,
      isInstance?: boolean,
      options?: any
    ) {
      return VBOMInfoApiFp(configuration).VBOMVnfCapacityFilter(
        body,
        propertyName,
        propertyFilter,
        isInstance,
        options
      )(fetch, basePath);
    },
    /**
     * @param {VBOMInfoQueryObjectGrid} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    VBOMInfoGetGrid(body: VBOMInfoQueryObjectGrid, options?: any) {
      return VBOMInfoApiFp(configuration).VBOMInfoGetGrid(body, options)(
        fetch,
        basePath
      );
    },
    /**
     * @param {VBOMClusterInfoQueryObjectGrid} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    VBOMClusterInfoGetGrid(
      body: VBOMClusterInfoQueryObjectGrid,
      options?: any
    ) {
      return VBOMInfoApiFp(configuration).VBOMClusterInfoGetGrid(body, options)(
        fetch,
        basePath
      );
    },
    /**
     * @param {VBOMClusterInfoQueryObjectGrid} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    VBOMVnfInfoAndCapacityGetGrid(
      body: VBOMClusterInfoQueryObjectGrid,
      options?: any
    ) {
      return VBOMInfoApiFp(configuration).VBOMVnfInfoAndCapacityGetGrid(
        body,
        options
      )(fetch, basePath);
    },
    /**
     *
     * @param {VBOMInfoDtoUpdate} body
     * @param {boolean} [forced]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    VBOMInfoEdit(body: VBOMInfoDtoUpdate, forced?: boolean, options?: any) {
      return VBOMInfoApiFp(configuration).VBOMInfoEdit(
        body,
        forced,
        options
      )(fetch, basePath);
    },
    /**
     *
     * @param {VBOMInfoDtoCreate} body
     * @param {boolean} [forced]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    VBOMInfoCreate(body: VBOMInfoDtoCreate, forced?: boolean, options?: any) {
      return VBOMInfoApiFp(configuration).VBOMInfoCreate(
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
    VBOMInfoRestore(id?: number, options?: any) {
      return VBOMInfoApiFp(configuration).VBOMInfoRestore(id, options)(
        fetch,
        basePath
      );
    },
  };
};

/**
 * VBOMInfoApi - object-oriented interface
 * @export
 * @class VBOMInfoApi
 * @extends {BaseAPI}
 */
export class VBOMInfoApi extends BaseAPI {
  /**
   *
   * @param {string} [apiType]
   * @param {number} [id]
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof VBOMInfoApi
   */
  public VBOMInfoDelete(id: number, apiType?: string, options?: any) {
    return VBOMInfoApiFp(this.configuration).VBOMInfoDelete(
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
   * @memberof VBOMInfoApi
   */
  public VBOMInfoDeleteDeep(id?: number, options?: any) {
    return VBOMInfoApiFp(this.configuration).VBOMInfoDeleteDeep(id, options)(
      this.fetch,
      this.basePath
    );
  }

  /**
   *
   * @param {VBOMInfoQueryObjectGrid} body
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof VBOMInfoApi
   */
  public VBOMInfoExportReport(body: VBOMInfoQueryObjectGrid, options?: any) {
    return VBOMInfoApiFp(this.configuration).VBOMInfoExportReport(
      body,
      options
    )(this.fetch, this.basePath);
  }
  public async VBOMInfoImportStatus(file: File) {
    const fetchArgs = VBOMInfoApiFetchParamCreator(
      this.configuration
    ).VBOMInfoImportExcel(file);

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
   * @memberof VBOMInfoApi
   */
  public VBOMInfoGetCreateResourceVBOMInfo(options?: any) {
    return VBOMInfoApiFp(this.configuration).VBOMInfoGetCreateResourceVBOMInfo(
      options
    )(this.fetch, this.basePath);
  }
  /**
   * @param {number} id
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof VBOMInfoApi
   */
  public VBOMInfoGetUpdateResourceVBOMInfo(id: number, options?: any) {
    return VBOMInfoApiFp(this.configuration).VBOMInfoGetUpdateResourceVBOMInfo(
      id,
      options
    )(this.fetch, this.basePath);
  }
  /**
   * @param {number} id
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof VBOMInfoApi
   */
  public GetAllOpCosAndVerticalResponsibles(id: number, options?: any) {
    return VBOMInfoApiFp(this.configuration).GetAllOpCosAndVerticalResponsibles(
      id,
      options
    )(this.fetch, this.basePath);
  }
  /**
   * @param {Array<number>} id
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof VBOMInfoApi
   */
  public GetOpCosAndVerticalAndSubDomainResponsibles(
    id: Array<number>,
    options?: any
  ) {
    return VBOMInfoApiFp(
      this.configuration
    ).GetOpCosAndVerticalAndSubDomainResponsibles(id, options)(
      this.fetch,
      this.basePath
    );
  }

  /**
   *
   * @param {VBOMInfoQueryObjectGrid} body
   * @param {string} [propertyName]
   * @param {string} [propertyFilter]
   * @param {boolean} [isInstance]
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof VBOMInfoApi
   */
  public VBOMInfoGetFilterResult(
    body: VBOMInfoQueryObjectGrid,
    propertyName?: string,
    propertyFilter?: string,
    isInstance?: boolean,
    options?: any
  ) {
    return VBOMInfoApiFp(this.configuration).VBOMInfoGetFilterResult(
      body,
      propertyName,
      propertyFilter,
      isInstance,
      options
    )(this.fetch, this.basePath);
  }
  /**
   *
   * @param {VBOMInfoQueryObjectGrid} body
   * @param {string} [propertyName]
   * @param {string} [propertyFilter]
   * @param {boolean} [isInstance]
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof VBOMInfoApi
   */
  public VBOMVnfCapacityFilter(
    body: VBOMInfoQueryObjectGrid,
    propertyName?: string,
    propertyFilter?: string,
    isInstance?: boolean,
    options?: any
  ) {
    return VBOMInfoApiFp(this.configuration).VBOMVnfCapacityFilter(
      body,
      propertyName,
      propertyFilter,
      isInstance,
      options
    )(this.fetch, this.basePath);
  }

  /**
   *
   * @param {VBOMInfoQueryObjectGrid} body
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof VBOMInfoApi
   */
  public VBOMInfoGetGrid(body: VBOMInfoQueryObjectGrid, options?: any) {
    return VBOMInfoApiFp(this.configuration).VBOMInfoGetGrid(body, options)(
      this.fetch,
      this.basePath
    );
  }

  /**
   *
   * @param {VBOMClusterInfoQueryObjectGrid} body
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof VBOMInfoApi
   */
  public VBOMClusterInfoGetGrid(
    body: VBOMClusterInfoQueryObjectGrid,
    options?: any
  ) {
    return VBOMInfoApiFp(this.configuration).VBOMClusterInfoGetGrid(
      body,
      options
    )(this.fetch, this.basePath);
  }

  /**
   *
   * @param {VBOMClusterInfoQueryObjectGrid} body
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof VBOMInfoApi
   */
  public VBOMVnfInfoAndCapacityGetGrid(
    body: VBOMClusterInfoQueryObjectGrid,
    options?: any
  ) {
    return VBOMInfoApiFp(this.configuration).VBOMVnfInfoAndCapacityGetGrid(
      body,
      options
    )(this.fetch, this.basePath);
  }

  /**
   *
   * @param {number} id
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof VBOMInfoApi
   */
  public VBOMInfoGetRelatedRecords(id: number, options?: any) {
    return VBOMInfoApiFp(this.configuration).VBOMInfoGetRelatedRecords(
      id,
      options
    )(this.fetch, this.basePath);
  }

  /**
   *
   * @param {VBOMInfoDtoUpdate} body
   * @param {boolean} [forced]
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof VBOMInfoApi
   */
  public VBOMInfoEdit(
    body: VBOMInfoDtoUpdate,
    forced?: boolean,
    options?: any
  ) {
    return VBOMInfoApiFp(this.configuration).VBOMInfoEdit(
      body,
      forced,
      options
    )(this.fetch, this.basePath);
  }

  /**
   *
   * @param {VBOMInfoDtoCreate} body
   * @param {boolean} [forced]
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof VBOMInfoApi
   */
  public VBOMInfoCreate(
    body: VBOMInfoDtoCreate,
    forced?: boolean,
    options?: any
  ) {
    return VBOMInfoApiFp(this.configuration).VBOMInfoCreate(
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
   * @memberof VBOMInfoApi
   */
  public VBOMInfoRestore(id?: number, options?: any) {
    return VBOMInfoApiFp(this.configuration).VBOMInfoRestore(id, options)(
      this.fetch,
      this.basePath
    );
  }
}
