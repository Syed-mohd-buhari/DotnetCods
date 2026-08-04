import * as url from "url";
import * as isomorphicFetch from "isomorphic-fetch";
import { Configuration } from "./Common/configuration";
import { headerObj } from "./header";

import {
  BASE_PATH,
  FetchArgs,
  FetchAPI,
  BaseAPI,
  RequiredError,
} from "./Common/CommonBusiness";

import {
  GeneralSettingsQueryObjectGrid,
  QueryResultDtoOfGeneralSettingsDtoGrid,
  GeneralSettingsDtoUpdate,
} from "../Model/GeneralSettingsModal";

/**
 * GeneralSettingsApi - fetch parameter creator
 */
export const GeneralSettingsApiFetchParamCreator = function (
  configuration?: Configuration
) {
  return {
    generalSettingsGetGrid(
      body: GeneralSettingsQueryObjectGrid,
      options: any = {}
    ): FetchArgs {
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling generalSettingsGetGrid."
        );
      }
      if (!body.appSettingsId || body.appSettingsId.length === 0) {
        body.appSettingsId = [1];
      }

      const localVarPath = `/api/AppSettingsConfiguration/Get`;
      const localVarUrlObj = url.parse(localVarPath, true);
      const localVarRequestOptions = Object.assign({ method: "POST" }, options);
      const localVarHeaderParameter = { ...headerObj } as any;
      const localVarQueryParameter = {} as any;

      if (configuration && configuration.apiKey) {
        const apiKeyValue =
          typeof configuration.apiKey === "function"
            ? configuration.apiKey("Authorization")
            : configuration.apiKey;
        localVarHeaderParameter["Authorization"] = apiKeyValue;
      }

      localVarHeaderParameter["Content-Type"] = "application/json";

      localVarUrlObj.query = Object.assign(
        {},
        localVarUrlObj.query,
        localVarQueryParameter,
        options.query
      );
      localVarUrlObj.search = null;

      localVarRequestOptions.headers = Object.assign(
        {},
        localVarHeaderParameter,
        options.headers
      );

      const needsSerialization =
        <any>"GeneralSettingsQueryObjectGrid" !== "string" ||
        localVarRequestOptions.headers["Content-Type"] === "application/json";

      localVarRequestOptions.body = needsSerialization
        ? JSON.stringify(body || {})
        : body || "";

      return {
        url: url.format(localVarUrlObj),
        options: localVarRequestOptions,
      };
    },

    generalSettingsGetSingle(id: number, options: any = {}): FetchArgs {
      if (id === null || id === undefined) {
        throw new RequiredError("id", "Parameter 'id' is required.");
      }

      const localVarPath = `/api/AppSettingsConfiguration/GetSingle/${encodeURIComponent(
        String(id)
      )}`;
      const localVarUrlObj = url.parse(localVarPath, true);
      const localVarRequestOptions = Object.assign({ method: "GET" }, options);
      const localVarHeaderParameter = { ...headerObj } as any;

      if (configuration?.apiKey) {
        const apiKeyValue =
          typeof configuration.apiKey === "function"
            ? configuration.apiKey("Authorization")
            : configuration.apiKey;
        localVarHeaderParameter["Authorization"] = apiKeyValue;
      }

      localVarRequestOptions.headers = {
        ...localVarHeaderParameter,
        ...options.headers,
      };

      return {
        url: url.format(localVarUrlObj),
        options: localVarRequestOptions,
      };
    },

    generalSettingsUpdate(
      body: GeneralSettingsDtoUpdate,
      options: any = {}
    ): FetchArgs {
      if (!body) {
        throw new RequiredError("body", "Update data is required.");
      }

      const localVarPath = `/api/AppSettingsConfiguration/Update`;
      const localVarUrlObj = url.parse(localVarPath, true);
      const localVarRequestOptions = Object.assign({ method: "PUT" }, options);
      const localVarHeaderParameter = { ...headerObj } as any;

      if (configuration?.apiKey) {
        const apiKeyValue =
          typeof configuration.apiKey === "function"
            ? configuration.apiKey("Authorization")
            : configuration.apiKey;
        localVarHeaderParameter["Authorization"] = apiKeyValue;
      }

      localVarHeaderParameter["Content-Type"] = "application/json";

      localVarRequestOptions.headers = {
        ...localVarHeaderParameter,
        ...options.headers,
      };
      localVarRequestOptions.body = JSON.stringify(body);

      return {
        url: url.format(localVarUrlObj),
        options: localVarRequestOptions,
      };
    },
  };
};

/**
 * GeneralSettingsApi - functional interface
 */
export const GeneralSettingsApiFp = function (configuration?: Configuration) {
  return {
    generalSettingsGetGrid(
      body: GeneralSettingsQueryObjectGrid,
      options?: any
    ) {
      const fetchArgs = GeneralSettingsApiFetchParamCreator(
        configuration
      ).generalSettingsGetGrid(body, options);

      return (fetch: FetchAPI = isomorphicFetch, basePath = BASE_PATH) => {
        return fetch(basePath + fetchArgs.url, fetchArgs.options).then(
          async (response) => {
            if (response.status >= 200 && response.status < 300) {
              return response.json();
            } else {
              throw response;
            }
          }
        );
      };
    },

    generalSettingsGetSingle(id: number, options?: any) {
      const fetchArgs = GeneralSettingsApiFetchParamCreator(
        configuration
      ).generalSettingsGetSingle(id, options);
      return (fetch: FetchAPI = isomorphicFetch, basePath = BASE_PATH) => {
        return fetch(basePath + fetchArgs.url, fetchArgs.options).then(
          async (response) => {
            if (response.status >= 200 && response.status < 300) {
              return response.json();
            } else {
              throw response;
            }
          }
        );
      };
    },

    generalSettingsUpdate(data: GeneralSettingsDtoUpdate, options?: any) {
      const fetchArgs = GeneralSettingsApiFetchParamCreator(
        configuration
      ).generalSettingsUpdate(data, options);
      return (fetch: FetchAPI = isomorphicFetch, basePath = BASE_PATH) => {
        return fetch(basePath + fetchArgs.url, fetchArgs.options).then(
          async (response) => {
            if (response.status >= 200 && response.status < 300) {
              return response.json();
            } else {
              throw response;
            }
          }
        );
      };
    },
  };
};

/**
 * GeneralSettingsApi - factory interface
 */
export const GeneralSettingsApiFactory = function (
  configuration?: Configuration,
  fetch?: FetchAPI,
  basePath?: string
) {
  return {
    generalSettingsGetGrid(
      body: GeneralSettingsQueryObjectGrid,
      options?: any
    ) {
      return GeneralSettingsApiFp(configuration).generalSettingsGetGrid(
        body,
        options
      )(fetch, basePath);
    },

    generalSettingsGetSingle(id: number, options?: any) {
      return GeneralSettingsApiFp(configuration).generalSettingsGetSingle(
        id,
        options
      )(fetch, basePath);
    },

    generalSettingsUpdate(data: GeneralSettingsDtoUpdate, options?: any) {
      return GeneralSettingsApiFp(configuration).generalSettingsUpdate(
        data,
        options
      )(fetch, basePath);
    },
  };
};

/**
 * GeneralSettingsApi - object-oriented class
 */
export class GeneralSettingsApi extends BaseAPI {
  public generalSettingsGetGrid(
    body: GeneralSettingsQueryObjectGrid,
    options?: any
  ) {
    return GeneralSettingsApiFp(this.configuration).generalSettingsGetGrid(
      body,
      options
    )(this.fetch, this.basePath);
  }

  public generalSettingsGetSingle(id: number, options?: any) {
    return GeneralSettingsApiFp(this.configuration).generalSettingsGetSingle(
      id,
      options
    )(this.fetch, this.basePath);
  }

  public generalSettingsUpdate(data: GeneralSettingsDtoUpdate, options?: any) {
    return GeneralSettingsApiFp(this.configuration).generalSettingsUpdate(
      data,
      options
    )(this.fetch, this.basePath);
  }
}
