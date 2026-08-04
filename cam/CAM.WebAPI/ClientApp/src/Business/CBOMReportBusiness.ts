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
import { QueryResultDtoOfCBOMReportDtoGrid } from "../Model/CBOMReport";

/**
 * CBOMReportApi - fetch parameter creator
 * @export
 */
export const CBOMReportApiFetchParamCreator = function (
  configuration?: Configuration
) {
  return {
    /**
     * @param {any} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    CBOMReportGetGrid(body: any, options: any = {}): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling any."
        );
      }
      const localVarPath = `/api/CbomReport/GetAggregatedAnddissagregatedReport`;
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
        <any>"CBOMReportQueryObjectGrid" !== "string" ||
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
    CBOMReportGetAllResource(options: any = {}): FetchArgs {
      const localVarPath = `/api/CbomReport/GetAllResources`;
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
 * CBOMReportApi - functional programming interface
 * @export
 */
export const CBOMReportApiFp = function (configuration?: Configuration) {
  return {
    /**
     * @param {any} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    CBOMReportGetGrid(
      body: any,
      options?: any
    ): (
      fetch?: FetchAPI,
      basePath?: string
    ) => Promise<QueryResultDtoOfCBOMReportDtoGrid> {
      const localVarFetchArgs = CBOMReportApiFetchParamCreator(
        configuration
      ).CBOMReportGetGrid(body, options);
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
    CBOMReportGetAllResource(
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<any> {
      const localVarFetchArgs =
        CBOMReportApiFetchParamCreator(configuration).CBOMReportGetAllResource(
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
 * CBOMReportApi - factory interface
 * @export
 */
export const CBOMReportApiFactory = function (
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
    CBOMReportGetGrid(body: any, options?: any) {
      return CBOMReportApiFp(configuration).CBOMReportGetGrid(body, options)(
        fetch,
        basePath
      );
    },
    /**
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    CBOMReportGetAllResource(options?: any) {
      return CBOMReportApiFp(configuration).CBOMReportGetAllResource(options)(
        fetch,
        basePath
      );
    },
  };
};

/**
 * CBOMReportApi - object-oriented interface
 * @export
 * @class CBOMReportApi
 * @extends {BaseAPI}
 */
export class CBOMReportApi extends BaseAPI {
  /**
   *
   * @param {any} body
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof CBOMReportApi
   */
  public GetCBOMReportGrid(body: any, options?: any) {
    return CBOMReportApiFp(this.configuration).CBOMReportGetGrid(body, options)(
      this.fetch,
      this.basePath
    );
  }
  /**
   *
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof CBOMReportApi
   */
  public CBOMReportGetAllResource(options?: any) {
    return CBOMReportApiFp(this.configuration).CBOMReportGetAllResource(
      options
    )(this.fetch, this.basePath);
  }
}
