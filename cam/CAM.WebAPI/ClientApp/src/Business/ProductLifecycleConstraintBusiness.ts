import * as url from "url";
import * as portableFetch from "portable-fetch";
import { Configuration } from "./Common/configuration";
import {
  DataRemediationDto,
  RelatedResource,
  ResultDto,
  ResultDtoOfResultDataRemediationDto,
} from "../Model/CommonModels";
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
  DesignComponentDtoCreate,
  DesignComponentDtoUpdate,
  QueryResultDtoOfDesignComponentDtoGrid,
} from "../Model/DesignComponent";
import { ReturnFile } from "../Model/Common";
import { InizializeNewProductCreateDto } from "../Model/InizializeNewProduct";
import {
  ConstraintInfoDto,
  LifecycleConstraintDto,
  MajorHardwareBuildMainSystemTypeDto,
  LifecycleConstraintQueryDto,
} from "../Model/SystemTypeModel";

/**
 * ProductLifecycleConstraintsApi - fetch parameter creator
 * @export
 */
export const ProductLifecycleConstraintsApiFetchParamCreator = function (
  configuration?: Configuration
) {
  return {
    /**
     *
     * @param {LifecycleConstraintQueryDto} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    productLifecycleConstraintsGetLifecycleCostraintInfo(
      body: LifecycleConstraintQueryDto,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling productLifecycleConstraintsGetLifecycleCostraintInfo."
        );
      }
      const localVarPath = `/api/ProductLifecycleConstraints`;
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
        <any>"LifecycleConstraintQueryDto" !== "string" ||
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
     * @param {LifecycleConstraintDto} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    productLifecycleConstraintsSaveLifecycleConstraint(
      body: LifecycleConstraintDto,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling productLifecycleConstraintsSaveLifecycleConstraint."
        );
      }
      const localVarPath = `/api/ProductLifecycleConstraints/SaveLifecycleConstraint`;
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
        <any>"LifecycleConstraintDto" !== "string" ||
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
 * ProductLifecycleConstraintsApi - functional programming interface
 * @export
 */
export const ProductLifecycleConstraintsApiFp = function (
  configuration?: Configuration
) {
  return {
    /**
     *
     * @param {LifecycleConstraintQueryDto} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    productLifecycleConstraintsGetLifecycleCostraintInfo(
      body: LifecycleConstraintQueryDto,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ConstraintInfoDto> {
      const localVarFetchArgs = ProductLifecycleConstraintsApiFetchParamCreator(
        configuration
      ).productLifecycleConstraintsGetLifecycleCostraintInfo(body, options);
      return (fetch: FetchAPI = portableFetch, basePath = BASE_PATH) => {
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
     * @param {LifecycleConstraintDto} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    productLifecycleConstraintsSaveLifecycleConstraint(
      body: LifecycleConstraintDto,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = ProductLifecycleConstraintsApiFetchParamCreator(
        configuration
      ).productLifecycleConstraintsSaveLifecycleConstraint(body, options);
      return (fetch: FetchAPI = portableFetch, basePath = BASE_PATH) => {
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
 * ProductLifecycleConstraintsApi - factory interface
 * @export
 */
export const ProductLifecycleConstraintsApiFactory = function (
  configuration?: Configuration,
  fetch?: FetchAPI,
  basePath?: string
) {
  return {
    /**
     *
     * @param {LifecycleConstraintQueryDto} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    productLifecycleConstraintsGetLifecycleCostraintInfo(
      body: LifecycleConstraintQueryDto,
      options?: any
    ) {
      return ProductLifecycleConstraintsApiFp(
        configuration
      ).productLifecycleConstraintsGetLifecycleCostraintInfo(body, options)(
        fetch,
        basePath
      );
    },
    /**
     *
     * @param {LifecycleConstraintDto} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    productLifecycleConstraintsSaveLifecycleConstraint(
      body: LifecycleConstraintDto,
      options?: any
    ) {
      return ProductLifecycleConstraintsApiFp(
        configuration
      ).productLifecycleConstraintsSaveLifecycleConstraint(body, options)(
        fetch,
        basePath
      );
    },
  };
};

/**
 * ProductLifecycleConstraintsApi - object-oriented interface
 * @export
 * @class ProductLifecycleConstraintsApi
 * @extends {BaseAPI}
 */
export class ProductLifecycleConstraintsApi extends BaseAPI {
  /**
   *
   * @param {LifecycleConstraintQueryDto} body
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof ProductLifecycleConstraintsApi
   */
  public productLifecycleConstraintsGetLifecycleCostraintInfo(
    body: LifecycleConstraintQueryDto,
    options?: any
  ) {
    return ProductLifecycleConstraintsApiFp(
      this.configuration
    ).productLifecycleConstraintsGetLifecycleCostraintInfo(body, options)(
      this.fetch,
      this.basePath
    );
  }

  /**
   *
   * @param {LifecycleConstraintDto} body
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof ProductLifecycleConstraintsApi
   */
  public productLifecycleConstraintsSaveLifecycleConstraint(
    body: LifecycleConstraintDto,
    options?: any
  ) {
    return ProductLifecycleConstraintsApiFp(
      this.configuration
    ).productLifecycleConstraintsSaveLifecycleConstraint(body, options)(
      this.fetch,
      this.basePath
    );
  }
}
