import * as url from "url";
import * as isomorphicFetch from "isomorphic-fetch";
import {
  BASE_PATH,
  BaseAPI,
  FetchAPI,
  FetchArgs,
  RequiredError,
} from "./Common/CommonBusiness";
import { headerObj } from "./header";
import { Configuration } from "./Common/configuration";
import { ResultDto } from "../Model/CommonModels";
import { safeNumber } from "../Hook/Common";

/**
 * ProjectPlanApi - fetch parameter creator
 * @export
 */
export const ProjectPlanApiFetchParamCreator = function (
  configuration?: Configuration
) {
  return {
    /**
     *
     * @param {string} paId
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    getProjectPlanReport(paId: string, options: any = {}): FetchArgs {
      // verify required parameter 'body' is not null or undefined

      const localVarPath = `/api/ProjectPlan/Get`;
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
      if (paId !== null) {
        localVarQueryParameter["paId"] = paId;
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
      // const needsSerialization =
      //   <any>"ProjectPlanDtoCreate" !== "string" ||
      //   localVarRequestOptions.headers["Content-Type"] === "application/json";
      // localVarRequestOptions.body = needsSerialization
      //   ? JSON.stringify({ paId: safeNumber(paId) } || {})
      //   : { paId: safeNumber(paId) } || "";

      return {
        url: url.format(localVarUrlObj),
        options: localVarRequestOptions,
      };
    },
    /**
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    projectPlanRefreshStatus(options: any = {}): FetchArgs {
      const localVarPath = `/api/ProjectPlan/BulkUpdate`;
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
    /**
     *
     * @param {any} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    editProjectPlan(body: any, options: any = {}): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling editProjectPlan."
        );
      }
      const localVarPath = `/api/ProjectPlan`;
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
        <any>"PlannedActivityTypesDtoUpdate" !== "string" ||
        localVarRequestOptions.headers["Content-Type"] === "application/json";
      localVarRequestOptions.body = needsSerialization
        ? JSON.stringify(body || {})
        : body || "";

      return {
        url: url.format(localVarUrlObj),
        options: localVarRequestOptions,
      };
    },
  };
};
/**
 * ProjectPlanApi - functional programming interface
 * @export
 */
export const ProjectPlanApiFp = function (configuration?: Configuration) {
  return {
    /**
     *
     * @param {string} paId
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    getProjectPlanReport(
      paId: string,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = ProjectPlanApiFetchParamCreator(
        configuration
      ).getProjectPlanReport(paId, options);
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
    projectPlanRefreshStatus(
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<any> {
      const localVarFetchArgs =
        ProjectPlanApiFetchParamCreator(configuration).projectPlanRefreshStatus(
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
     * @param {any} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    editProjectPlan(
      body: any,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = ProjectPlanApiFetchParamCreator(
        configuration
      ).editProjectPlan(body, options);
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
 * ProjectPlanApi - factory interface
 * @export
 */
export const ProjectPlanApiFactory = function (
  configuration?: Configuration,
  fetch?: FetchAPI,
  basePath?: string
) {
  return {
    /**
     *
     * @param {string} paId
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    getProjectPlanReport(paId: string, options?: any) {
      return ProjectPlanApiFp(configuration).getProjectPlanReport(
        paId,
        options
      )(fetch, basePath);
    },
    /**
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    projectPlanRefreshStatus(options?: any) {
      return ProjectPlanApiFp(configuration).projectPlanRefreshStatus(options)(
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
    editProjectPlan(body: any, options?: any) {
      return ProjectPlanApiFp(configuration).editProjectPlan(body, options)(
        fetch,
        basePath
      );
    },
  };
};
/**
 * ProjectPlanApi - object-oriented interface
 * @export
 * @class ProjectPlanApi
 * @extends {BaseAPI}
 */
export class ProjectPlanApi extends BaseAPI {
  /**
   *
   * @param {string} paId
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof ProjectPlanApi
   */
  public getProjectPlanReport(paId: string, options?: any) {
    return ProjectPlanApiFp(this.configuration).getProjectPlanReport(
      paId,
      options
    )(this.fetch, this.basePath);
  }

  /**
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof ProjectPlanApi
   */
  public projectPlanRefreshStatus() {
    return ProjectPlanApiFp(this.configuration).projectPlanRefreshStatus()(
      this.fetch,
      this.basePath
    );
  }

  /**
   *
   * @param {any} body
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof PlannedActivityTypesApi
   */
  public editProjectPlan(body: any, options?: any) {
    return ProjectPlanApiFp(this.configuration).editProjectPlan(body, options)(
      this.fetch,
      this.basePath
    );
  }
}
