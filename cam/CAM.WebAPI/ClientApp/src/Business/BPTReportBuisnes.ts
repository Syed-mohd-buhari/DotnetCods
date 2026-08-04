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
import { QueryResultDtoOfBPTReportDtoGrid } from "../Model/BPTReport";
import { ReturnFile } from "../Model/Common";
import { BPTReportQueryObjectGrid as BPTReportQueryObjectGrid } from "../Model/BPTReport";
import {
  TipologicaGridDto,
  TipologicheQueryObjectGrid,
} from "../Model/LookUp/LookUpGenericModel";
/**
 * BPTReportApi - fetch parameter creator
 * @export
 */
export const BPTReportApiFetchParamCreator = function (
  configuration?: Configuration
) {
  return {
    /**
     * @param {BPTReportQueryObjectGrid} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    BPTReportGetGrid(
      body: BPTReportQueryObjectGrid,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling BPTReportQueryObjectGrid."
        );
      }
      const localVarPath = `/api/BPT/GetBptRecords`;
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
        <any>"BPTReportQueryObjectGrid" !== "string" ||
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
     * @param {BPTReportQueryObjectGrid} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    BPTReportExportReport(
      body: BPTReportQueryObjectGrid,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling BPTReportExportReport."
        );
      }
      const localVarPath = `/api/BPT/ExportReport`;
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
        <any>"BPTReportQueryObjectGrid" !== "string" ||
        localVarRequestOptions.headers["Content-Type"] === "application/json";
      localVarRequestOptions.body = needsSerialization
        ? JSON.stringify(body || {})
        : body || "";

      return {
        url: url.format(localVarUrlObj),
        options: localVarRequestOptions,
      };
    },

    BPTReportGetFilterResult(
      body: BPTReportQueryObjectGrid,
      propertyName?: string,
      propertyFilter?: string,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling BPTReportGetFilterResult."
        );
      }
      const localVarPath = `/api/BPT/Filter`;
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
        <any>"BPTReportQueryObjectGrid" !== "string" ||
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
     * @param {Array<number>} mode
     * @param {Array<number>} verticalId
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */

    bptReportImport(body: File, options: any = {}): FetchArgs {
      // Set up FormData
      const formData = new FormData();
      formData.append("file", body);
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling bptReportImport."
        );
      }

      const localVarPath = `/api/BPT/import`;
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
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    bptRefreshStatus(options: any = {}): FetchArgs {
      const localVarPath = `/api/BPT/BulkInsertForBpt`;
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

      return {
        url: url.format(localVarUrlObj),
        options: localVarRequestOptions,
      };
    },
  };
};

/**
 * BPTReportApi - functional programming interface
 * @export
 */
export const BPTReportApiFp = function (configuration?: Configuration) {
  return {
    /**
     *
     * @param {BPTReportQueryObjectGrid} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    BPTReportExportReport(
      body: BPTReportQueryObjectGrid,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ReturnFile> {
      const localVarFetchArgs = BPTReportApiFetchParamCreator(
        configuration
      ).BPTReportExportReport(body, options);
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
     * @param {number} id
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */

    /**
     *
     * @param {BPTReportQueryObjectGrid} body
     * @param {string} [propertyName]
     * @param {string} [propertyFilter]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    BPTReportGetFilterResult(
      body: BPTReportQueryObjectGrid,
      propertyName?: string,
      propertyFilter?: string,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<Array<FilterValueDto>> {
      const localVarFetchArgs = BPTReportApiFetchParamCreator(
        configuration
      ).BPTReportGetFilterResult(body, propertyName, propertyFilter, options);
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
     * @param {BPTReportQueryObjectGrid} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    BPTReportGetGrid(
      body: BPTReportQueryObjectGrid,
      options?: any
    ): (
      fetch?: FetchAPI,
      basePath?: string
    ) => Promise<QueryResultDtoOfBPTReportDtoGrid> {
      const localVarFetchArgs = BPTReportApiFetchParamCreator(
        configuration
      ).BPTReportGetGrid(body, options);
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
     * @param {TipologicheQueryObjectGrid} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */

    bptReportImport(
      body: File,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<boolean> {
      const localVarFetchArgs = BPTReportApiFetchParamCreator(
        configuration
      ).bptReportImport(body, options);

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
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    bptRefreshStatus(
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<any> {
      const localVarFetchArgs =
        BPTReportApiFetchParamCreator(configuration).bptRefreshStatus(options);
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
 * BPTReportApi - factory interface
 * @export
 */
export const BPTReportApiFactory = function (
  configuration?: Configuration,
  fetch?: FetchAPI,
  basePath?: string
) {
  return {
    BPTReportExportReport(body: BPTReportQueryObjectGrid, options?: any) {
      return BPTReportApiFp(configuration).BPTReportExportReport(body, options)(
        fetch,
        basePath
      );
    },

    /**
     *
     * @param {BPTReportQueryObjectGrid} body
     * @param {string} [propertyName]
     * @param {string} [propertyFilter]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    BPTReportGetFilterResult(
      body: BPTReportQueryObjectGrid,
      propertyName?: string,
      propertyFilter?: string,
      options?: any
    ) {
      return BPTReportApiFp(configuration).BPTReportGetFilterResult(
        body,
        propertyName,
        propertyFilter,
        options
      )(fetch, basePath);
    },
    /**
     * @param {BPTReportQueryObjectGrid} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    BPTReportGetGrid(body: BPTReportQueryObjectGrid, options?: any) {
      return BPTReportApiFp(configuration).BPTReportGetGrid(body, options)(
        fetch,
        basePath
      );
    },

    /**
     *
     * @param {File} body
     * @param {Array<number>} mode
     * @param {Array<number>} verticalId
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */

    bptReportImport(body: File, options?: any): Promise<boolean> {
      return BPTReportApiFp(configuration).bptReportImport(body, options)(
        fetch,
        basePath
      );
    },

    /**
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    bptRefreshStatus(options?: any) {
      return BPTReportApiFp(configuration).bptRefreshStatus(options)(
        fetch,
        basePath
      );
    },
  };
};

/**
 * BPTReportApi - object-oriented interface
 * @export
 * @class BPTReportApi
 * @extends {BaseAPI}
 */
export class BPTReportApi extends BaseAPI {
  /**
   *
   * @param {BPTReportQueryObjectGrid} body
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof BPTReportApi
   */
  public BPTReportExportReport(body: BPTReportQueryObjectGrid, options?: any) {
    return BPTReportApiFp(this.configuration).BPTReportExportReport(
      body,
      options
    )(this.fetch, this.basePath);
  }

  /**
   *
   * @param {BPTReportQueryObjectGrid} body
   * @param {string} [propertyName]
   * @param {string} [propertyFilter]
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof BPTReportApi
   */
  public BPTReportGetFilterResult(
    body: BPTReportQueryObjectGrid,
    propertyName?: string,
    propertyFilter?: string,
    options?: any
  ) {
    return BPTReportApiFp(this.configuration).BPTReportGetFilterResult(
      body,
      propertyName,
      propertyFilter,
      options
    )(this.fetch, this.basePath);
  }

  /**
   *
   * @param {BPTReportQueryObjectGrid} body
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof BPTReportApi
   */
  public BPTReportGetGrid(body: BPTReportQueryObjectGrid, options?: any) {
    return BPTReportApiFp(this.configuration).BPTReportGetGrid(body, options)(
      this.fetch,
      this.basePath
    );
  }

  /**
   *
   * @param {File} body
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof BPTReportApi
   */

  public async bptReportImport(body: File) {
    return BPTReportApiFp(this.configuration).bptReportImport(body)(
      this.fetch,
      this.basePath
    );
  }

  /**
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof TSRReportApi
   */
  public bptRefreshStatus() {
    return BPTReportApiFp(this.configuration).bptRefreshStatus()(
      this.fetch,
      this.basePath
    );
  }
}
