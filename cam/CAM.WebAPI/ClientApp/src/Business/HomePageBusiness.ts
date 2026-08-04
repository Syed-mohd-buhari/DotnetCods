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
  BaseAPI,
} from "./Common/CommonBusiness";
import { headerObj } from "./header";
import { achievementDto } from "../NewLandingScreens/layouts/MainLayout/MainContent";

/**
 * HomePageApi - fetch parameter creator
 * @export
 */
export const HomePageApiFetchParamCreator = function (
  configuration?: Configuration
) {
  return {
    /**
     * @param {achievementDto} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    achivementAndSignPostRecordGet(
      body: achievementDto,
      options: any = {}
    ): FetchArgs {
      const localVarPath = `/api/HomePage/achievementRecords`;
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
        <any>"NetworkElementAsIsDtoUpdate" !== "string" ||
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
     * @param {number} id
     * @param {number} [roleId]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    ExpiredEomEosActionsGet(
      id: number,
      roleId: number,
      options: any = {}
    ): FetchArgs {
      if (id === null || id === undefined) {
        throw new RequiredError(
          "id",
          "Required parameter id was null or undefined when calling PlanndActivitySoftwareUpgradDetails."
        );
      }
      const localVarPath = `/api/HomePage/ExpiredEomEosActionsForDesignContactAsync`;
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
        localVarQueryParameter["userId"] = id;
      }
      if (roleId !== null) {
        localVarQueryParameter["PortalRoleId"] = roleId;
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
     * @param {number} [roleId]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    OpenLinkRecordCountGet(
      id: number,
      roleId: number,
      options: any = {}
    ): FetchArgs {
      if (id === null || id === undefined) {
        throw new RequiredError(
          "id",
          "Required parameter id was null or undefined when calling PlanndActivitySoftwareUpgradDetails."
        );
      }
      const localVarPath = `/api/HomePage/GetOpenLinkRecordCount`;
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
        localVarQueryParameter["userId"] = id;
      }
      if (roleId !== null) {
        localVarQueryParameter["PortalRoleId"] = roleId;
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
     * @param {number} [roleId]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    UpcomingEomEosActionsGet(
      id: number,
      roleId: number,
      options: any = {}
    ): FetchArgs {
      if (id === null || id === undefined) {
        throw new RequiredError(
          "id",
          "Required parameter id was null or undefined when calling PlanndActivitySoftwareUpgradDetails."
        );
      }
      const localVarPath = `/api/HomePage/UpcomingEomEosActionsForDesignContactAsync`;
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
        localVarQueryParameter["userId"] = id;
      }
      if (roleId !== null) {
        localVarQueryParameter["PortalRoleId"] = roleId;
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
     * @param {number} [roleId]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    UpcomingLinkRecordCountGet(
      id: number,
      roleId: number,
      options: any = {}
    ): FetchArgs {
      if (id === null || id === undefined) {
        throw new RequiredError(
          "id",
          "Required parameter id was null or undefined when calling PlanndActivitySoftwareUpgradDetails."
        );
      }
      const localVarPath = `/api/HomePage/GetUpcomingLinkRecordCount`;
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
        localVarQueryParameter["userId"] = id;
      }
      if (roleId !== null) {
        localVarQueryParameter["PortalRoleId"] = roleId;
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
     * @param {number} [roleId]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    ComplainceGraphDataGet(
      id: number,
      roleId: number,
      options: any = {}
    ): FetchArgs {
      if (id === null || id === undefined) {
        throw new RequiredError(
          "id",
          "Required parameter id was null or undefined when calling ComplainceGraphDataGet."
        );
      }
      const localVarPath = `/api/HomePage/ProductComplaince`;
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
      if (roleId !== null) {
        localVarQueryParameter["PortalRoleId"] = roleId;
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
 * HomePageApi - functional programming interface
 * @export
 */
export const HomePageApiFp = function (configuration?: Configuration) {
  return {
    /**
     *
     * @param {achievementDto} [body]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    achivementAndSignPostRecordGet(
      body: achievementDto
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs =
        HomePageApiFetchParamCreator(
          configuration
        ).achivementAndSignPostRecordGet(body);
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
     * @param {number} [roleId]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    ExpiredEomEosActionsGet(
      id: number,
      roleId: number,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = HomePageApiFetchParamCreator(
        configuration
      ).ExpiredEomEosActionsGet(id, roleId, options);
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
     * @param {number} [roleId]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    OpenLinkRecordCountGet(
      id: number,
      roleId: number,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = HomePageApiFetchParamCreator(
        configuration
      ).OpenLinkRecordCountGet(id, roleId, options);
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
     * @param {number} [roleId]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    UpcomingEomEosActionsGet(
      id: number,
      roleId: number,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = HomePageApiFetchParamCreator(
        configuration
      ).UpcomingEomEosActionsGet(id, roleId, options);
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
     * @param {number} [roleId]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    UpcomingLinkRecordCountGet(
      id: number,
      roleId: number,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = HomePageApiFetchParamCreator(
        configuration
      ).UpcomingLinkRecordCountGet(id, roleId, options);
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
     * @param {number} [roleId]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    ComplainceGraphDataGet(
      id: number,
      roleId: number,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = HomePageApiFetchParamCreator(
        configuration
      ).ComplainceGraphDataGet(id, roleId, options);
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
 * HomePageApi - factory interface
 * @export
 */
export const HomePageApiFactory = function (
  configuration?: Configuration,
  fetch?: FetchAPI,
  basePath?: string
) {
  return {
    /**
     *
     * @param {achievementDto} [body]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    achivementAndSignPostRecordGet(body: achievementDto) {
      return HomePageApiFp(configuration).achivementAndSignPostRecordGet(body)(
        fetch,
        basePath
      );
    },
    /**
     *
     * @param {number} [id]
     * @param {number} [roleId]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    ExpiredEomEosActionsGet(id: number, roleId: number, options?: any) {
      return HomePageApiFp(configuration).ExpiredEomEosActionsGet(
        id,
        roleId,
        options
      )(fetch, basePath);
    },
    /**
     *
     * @param {number} [id]
     * @param {number} [roleId]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    OpenLinkRecordCountGet(id: number, roleId: number, options?: any) {
      return HomePageApiFp(configuration).OpenLinkRecordCountGet(
        id,
        roleId,
        options
      )(fetch, basePath);
    },
    /**
     *
     * @param {number} [id]
     * @param {number} [roleId]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    UpcomingEomEosActionsGet(id: number, roleId: number, options?: any) {
      return HomePageApiFp(configuration).UpcomingEomEosActionsGet(
        id,
        roleId,
        options
      )(fetch, basePath);
    },
    /**
     *
     * @param {number} [id]
     * @param {number} [roleId]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    UpcomingLinkRecordCountGet(id: number, roleId: number, options?: any) {
      return HomePageApiFp(configuration).UpcomingLinkRecordCountGet(
        id,
        roleId,
        options
      )(fetch, basePath);
    },
    /**
     *
     * @param {number} [id]
     * @param {number} [roleId]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    ComplainceGraphDataGet(id: number, roleId: number, options?: any) {
      return HomePageApiFp(configuration).ComplainceGraphDataGet(
        id,
        roleId,
        options
      )(fetch, basePath);
    },
  };
};

/**
 * HomePageApi - object-oriented interface
 * @export
 * @class HomePageApi
 * @extends {BaseAPI}
 */
export class HomePageApi extends BaseAPI {
  /**
   *
   * @param {achievementDto} [body]
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof HomePageApi
   */
  public achivementAndSignPostRecordGet(body: achievementDto) {
    return HomePageApiFp(this.configuration).achivementAndSignPostRecordGet(
      body
    )(this.fetch, this.basePath);
  }
  /**
   *
   * @param {number} [id]
   * @param {number} [roleId]
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof HomePageApi
   */
  public ExpiredEomEosActionsGet(id: number, roleId: number, options?: any) {
    return HomePageApiFp(this.configuration).ExpiredEomEosActionsGet(
      id,
      roleId,
      options
    )(this.fetch, this.basePath);
  }
  /**
   *
   * @param {number} [id]
   * @param {number} [roleId]
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof HomePageApi
   */
  public OpenLinkRecordCountGet(id: number, roleId: number, options?: any) {
    return HomePageApiFp(this.configuration).OpenLinkRecordCountGet(
      id,
      roleId,
      options
    )(this.fetch, this.basePath);
  }
  /**
   *
   * @param {number} [id]
   * @param {number} [roleId]
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof HomePageApi
   */
  public UpcomingEomEosActionsGet(id: number, roleId: number, options?: any) {
    return HomePageApiFp(this.configuration).UpcomingEomEosActionsGet(
      id,
      roleId,
      options
    )(this.fetch, this.basePath);
  }
  /**
   *
   * @param {number} [id]
   * @param {number} [roleId]
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof HomePageApi
   */
  public UpcomingLinkRecordCountGet(id: number, roleId: number, options?: any) {
    return HomePageApiFp(this.configuration).UpcomingLinkRecordCountGet(
      id,
      roleId,
      options
    )(this.fetch, this.basePath);
  }
  /**
   *
   * @param {number} [id]
   * @param {number} [roleId]
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof HomePageApi
   */
  public ComplainceGraphDataGet(id: number, roleId: number, options?: any) {
    return HomePageApiFp(this.configuration).ComplainceGraphDataGet(
      id,
      roleId,
      options
    )(this.fetch, this.basePath);
  }
}
