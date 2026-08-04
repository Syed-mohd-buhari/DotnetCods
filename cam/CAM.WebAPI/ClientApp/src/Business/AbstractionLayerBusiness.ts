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
import { ReturnFile } from "../Model/Common";

/**
 * AbstractionLayerInfoApi - fetch parameter creator
 * @export
 */
export const AbstractionLayerInfoApiFetchParamCreator = function (
  configuration?: Configuration
) {
  return {
    /**
     * @param {number} id
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    PaSWUpgradeGet(id: number, options: any = {}): FetchArgs {
      if (id === null || id === undefined) {
        throw new RequiredError(
          "id",
          "Required parameter id was null or undefined when calling PlanndActivitySoftwareUpgradDetails."
        );
      }
      const localVarPath = `/api/AbstractionLayer/PlanndActivitySoftwareUpgradDetails`;
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
    EosAndEomMileStonesGet(id: number, options: any = {}): FetchArgs {
      if (id === null || id === undefined) {
        throw new RequiredError(
          "id",
          "Required parameter id was null or undefined when calling EosAndEomMileStones."
        );
      }
      const localVarPath = `/api/AbstractionLayer/GetEosAndEomMileStones`;
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
    UserPrefrenceDetailsGet(id: number, options: any = {}): FetchArgs {
      if (id === null || id === undefined) {
        throw new RequiredError(
          "id",
          "Required parameter id was null or undefined when calling EosAndEomMileStones."
        );
      }
      const localVarPath = `/api/AbstractionLayer/UserPrefrenceDetails`;
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
    DoingSectionPADetailsGet(id: number, options: any = {}): FetchArgs {
      if (id === null || id === undefined) {
        throw new RequiredError(
          "id",
          "Required parameter id was null or undefined when calling PlanndActivitySoftwareUpgradDetails."
        );
      }
      const localVarPath = `/api/AbstractionLayer/DoingSectionPADetails`;
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
    DoingSectionForEomAndEosGet(id: number, options: any = {}): FetchArgs {
      if (id === null || id === undefined) {
        throw new RequiredError(
          "id",
          "Required parameter id was null or undefined when calling PlanndActivitySoftwareUpgradDetails."
        );
      }
      const localVarPath = `/api/AbstractionLayer/DoingSectionForEomAndEos`;
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
    AchievementsDetailsGet(id: number, options: any = {}): FetchArgs {
      if (id === null || id === undefined) {
        throw new RequiredError(
          "id",
          "Required parameter id was null or undefined when calling PlanndActivitySoftwareUpgradDetails."
        );
      }
      const localVarPath = `/api/AbstractionLayer/AchievementsDetails`;
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
    SignPostPaDetailsGet(id: number, options: any = {}): FetchArgs {
      if (id === null || id === undefined) {
        throw new RequiredError(
          "id",
          "Required parameter id was null or undefined when calling PlanndActivitySoftwareUpgradDetails."
        );
      }
      const localVarPath = `/api/AbstractionLayer/SignPostPaDetails`;
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
 * AbstractionLayerInfoApi - functional programming interface
 * @export
 */
export const AbstractionLayerInfoApiFp = function (
  configuration?: Configuration
) {
  return {
    /**
     *
     * @param {number} [id]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    PaSWUpgradeGet(
      id: number,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = AbstractionLayerInfoApiFetchParamCreator(
        configuration
      ).PaSWUpgradeGet(id, options);
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
    EosAndEomMileStonesGet(
      id: number,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = AbstractionLayerInfoApiFetchParamCreator(
        configuration
      ).EosAndEomMileStonesGet(id, options);
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
    UserPrefrenceDetailsGet(
      id: number,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = AbstractionLayerInfoApiFetchParamCreator(
        configuration
      ).UserPrefrenceDetailsGet(id, options);
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
    DoingSectionPADetailsGet(
      id: number,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = AbstractionLayerInfoApiFetchParamCreator(
        configuration
      ).DoingSectionPADetailsGet(id, options);
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
    DoingSectionForEomAndEosGet(
      id: number,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = AbstractionLayerInfoApiFetchParamCreator(
        configuration
      ).DoingSectionForEomAndEosGet(id, options);
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
    AchievementsDetailsGet(
      id: number,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = AbstractionLayerInfoApiFetchParamCreator(
        configuration
      ).AchievementsDetailsGet(id, options);
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
    SignPostPaDetailsGet(
      id: number,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = AbstractionLayerInfoApiFetchParamCreator(
        configuration
      ).SignPostPaDetailsGet(id, options);
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
 * AbstractionLayerInfoApi - factory interface
 * @export
 */
export const AbstractionLayerInfoApiFactory = function (
  configuration?: Configuration,
  fetch?: FetchAPI,
  basePath?: string
) {
  return {
    /**
     *
     * @param {number} [id]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    PaSWUpgradeGet(id: number, options?: any) {
      return AbstractionLayerInfoApiFp(configuration).PaSWUpgradeGet(
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
    EosAndEomMileStonesGet(id: number, options?: any) {
      return AbstractionLayerInfoApiFp(configuration).EosAndEomMileStonesGet(
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
    UserPrefrenceDetailsGet(id: number, options?: any) {
      return AbstractionLayerInfoApiFp(configuration).UserPrefrenceDetailsGet(
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
    DoingSectionPADetailsGet(id: number, options?: any) {
      return AbstractionLayerInfoApiFp(configuration).DoingSectionPADetailsGet(
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
    DoingSectionForEomAndEosGet(id: number, options?: any) {
      return AbstractionLayerInfoApiFp(
        configuration
      ).DoingSectionForEomAndEosGet(id, options)(fetch, basePath);
    },
    /**
     *
     * @param {number} [id]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    AchievementsDetailsGet(id: number, options?: any) {
      return AbstractionLayerInfoApiFp(configuration).AchievementsDetailsGet(
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
    SignPostPaDetailsGet(id: number, options?: any) {
      return AbstractionLayerInfoApiFp(configuration).SignPostPaDetailsGet(
        id,
        options
      )(fetch, basePath);
    },
  };
};

/**
 * AbstractionLayerInfoApi - object-oriented interface
 * @export
 * @class AbstractionLayerInfoApi
 * @extends {BaseAPI}
 */
export class AbstractionLayerInfoApi extends BaseAPI {
  /**
   *
   * @param {number} [id]
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof AbstractionLayerInfoApi
   */
  public PaSWUpgradeGet(id: number, options?: any) {
    return AbstractionLayerInfoApiFp(this.configuration).PaSWUpgradeGet(
      id,
      options
    )(this.fetch, this.basePath);
  }
  /**
   *
   * @param {number} [id]
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof AbstractionLayerInfoApi
   */
  public EosAndEomMileStonesGet(id: number, options?: any) {
    return AbstractionLayerInfoApiFp(this.configuration).EosAndEomMileStonesGet(
      id,
      options
    )(this.fetch, this.basePath);
  }
  /**
   *
   * @param {number} [id]
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof AbstractionLayerInfoApi
   */
  public UserPrefrenceDetailsGet(id: number, options?: any) {
    return AbstractionLayerInfoApiFp(
      this.configuration
    ).UserPrefrenceDetailsGet(id, options)(this.fetch, this.basePath);
  }
  /**
   *
   * @param {number} [id]
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof AbstractionLayerInfoApi
   */
  public DoingSectionPADetailsGet(id: number, options?: any) {
    return AbstractionLayerInfoApiFp(
      this.configuration
    ).DoingSectionPADetailsGet(id, options)(this.fetch, this.basePath);
  }
  /**
   *
   * @param {number} [id]
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof AbstractionLayerInfoApi
   */
  public DoingSectionForEomAndEosGet(id: number, options?: any) {
    return AbstractionLayerInfoApiFp(
      this.configuration
    ).DoingSectionForEomAndEosGet(id, options)(this.fetch, this.basePath);
  }
  /**
   *
   * @param {number} [id]
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof AbstractionLayerInfoApi
   */
  public AchievementsDetailsGet(id: number, options?: any) {
    return AbstractionLayerInfoApiFp(this.configuration).AchievementsDetailsGet(
      id,
      options
    )(this.fetch, this.basePath);
  }
  /**
   *
   * @param {number} [id]
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof AbstractionLayerInfoApi
   */
  public SignPostPaDetailsGet(id: number, options?: any) {
    return AbstractionLayerInfoApiFp(this.configuration).SignPostPaDetailsGet(
      id,
      options
    )(this.fetch, this.basePath);
  }
}
