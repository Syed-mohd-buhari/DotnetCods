import * as url from "url";
import * as isomorphicFetch from "isomorphic-fetch";
import { Configuration } from "./Common/configuration";
import { ResultDto } from "../Model/CommonModels";
import { headerObj } from "./header";
import { ReturnFile } from "../Model/Common";
import {
  BASE_PATH,
  FetchArgs,
  FetchAPI,
  BaseAPI,
  RequiredError,
  FilterValueDto,
} from "./Common/CommonBusiness";
import { QueryResultDtoOfVBOMReportDtoGrid } from "../Model/VBOMReport";

/**
 * VBOMReportApi - fetch parameter creator
 * @export
 */
export const VBOMReportApiFetchParamCreator = function (
  configuration?: Configuration
) {
  return {
    /**
     * @param {any} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    VBOMReportGetGrid(body: any, options: any = {}): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling any."
        );
      }
      const localVarPath = `/api/VbomReport/GetAggregatedAnddissagregatedReport`;
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
        <any>"VBOMReportQueryObjectGrid" !== "string" ||
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
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    VBOMReportGetAllResource(options: any = {}): FetchArgs {
      const localVarPath = `/api/VbomReport/GetAllResources`;
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

      return {
        url: url.format(localVarUrlObj),
        options: localVarRequestOptions,
      };
    },
  };
};
/**
 * VBOMReportApi - functional programming interface
 * @export
 */
export const VBOMReportApiFp = function (configuration?: Configuration) {
  return {
    /**
     * @param {any} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    VBOMReportGetGrid(
      body: any,
      options?: any
    ): (
      fetch?: FetchAPI,
      basePath?: string
    ) => Promise<QueryResultDtoOfVBOMReportDtoGrid> {
      const localVarFetchArgs = VBOMReportApiFetchParamCreator(
        configuration
      ).VBOMReportGetGrid(body, options);
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
    VBOMReportGetAllResource(
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<any> {
      const localVarFetchArgs =
        VBOMReportApiFetchParamCreator(configuration).VBOMReportGetAllResource(
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
  };
};
/**
 * VBOMReportApi - factory interface
 * @export
 */
export const VBOMReportApiFactory = function (
  configuration?: Configuration,
  fetch?: FetchAPI,
  basePath?: string
) {
  return {
    /**
     * @param {any} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    VBOMReportGetGrid(body: any, options?: any) {
      return VBOMReportApiFp(configuration).VBOMReportGetGrid(body, options)(
        fetch,
        basePath
      );
    },
    /**
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    VBOMReportGetAllResource(options?: any) {
      return VBOMReportApiFp(configuration).VBOMReportGetAllResource(options)(
        fetch,
        basePath
      );
    },
  };
};

/**
 * VBOMReportApi - object-oriented interface
 * @export
 * @class VBOMReportApi
 * @extends {BaseAPI}
 */
export class VBOMReportApi extends BaseAPI {
  /**
   *
   * @param {any} body
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof VBOMReportApi
   */
  public GetVBOMReportGrid(body: any, options?: any) {
    return VBOMReportApiFp(this.configuration).VBOMReportGetGrid(body, options)(
      this.fetch,
      this.basePath
    );
  }
  /**
   *
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof VBOMReportApi
   */
  public VBOMReportGetAllResource(options?: any) {
    return VBOMReportApiFp(this.configuration).VBOMReportGetAllResource(
      options
    )(this.fetch, this.basePath);
  }
}
