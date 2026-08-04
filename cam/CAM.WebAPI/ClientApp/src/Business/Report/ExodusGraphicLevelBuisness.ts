import * as url from "url";
import * as isomorphicFetch from "isomorphic-fetch";
import { Configuration } from "../Common/configuration";
import {
  BASE_PATH,
  FetchArgs,
  FetchAPI,
  RequiredError,
} from "../Common/CommonBusiness";
import { headerObj } from "../header";
import { BaseAPI } from "../Common/CommonBusiness";
import {
  GetAssetLevelReportForExodusDTO,
  AssetLevelReportForExodusResponse,
  OpcoDcfDropdownResponse,
} from "../../Model/Report/AssetLevelReportForExodus";

export const AssetLevelReportForExodusApiFetchParamCreator = function (
  configuration?: Configuration
) {
  return {
    GetAssetLevelReportForExodus(
      body: GetAssetLevelReportForExodusDTO,
      options: any = {}
    ): FetchArgs {
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling GetAssetLevelReportForExodus."
        );
      }
      const localVarPath = `/api/ExodusGraphicalLevel3Report/GetAssetLevlReportForExodus`;
      const localVarUrlObj = url.parse(localVarPath, true);
      const localVarRequestOptions = Object.assign({ method: "POST" }, options);
      const localVarHeaderParameter = { ...headerObj } as any;
      const localVarQueryParameter = {} as any;

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

      localVarUrlObj.search = null;
      localVarRequestOptions.headers = Object.assign(
        {},
        localVarHeaderParameter,
        options.headers
      );
      const needsSerialization =
        <any>"GetAssetLevelReportForExodusDTO" !== "string" ||
        localVarRequestOptions.headers["Content-Type"] === "application/json";
      localVarRequestOptions.body = needsSerialization
        ? JSON.stringify(body || {})
        : body || "";

      return {
        url: url.format(localVarUrlObj),
        options: localVarRequestOptions,
      };
    },
    GetOpcoAndPlannnedDcfDropdown(
      body: any = {},
      options: any = {}
    ): FetchArgs {
      const localVarPath = `/api/ExodusGraphicalLevel3Report/GetOpcoAndPlannnedDcfDropdowns`;
      const localVarUrlObj = url.parse(localVarPath, true);
      const localVarRequestOptions = Object.assign({ method: "POST" }, options);
      const localVarHeaderParameter = { ...headerObj } as any;
      if (configuration && configuration.apiKey) {
        const v =
          typeof configuration.apiKey === "function"
            ? configuration.apiKey("Authorization")
            : configuration.apiKey;
        localVarHeaderParameter["Authorization"] = v;
      }
      localVarHeaderParameter["Content-Type"] = "application/json";
      localVarUrlObj.search = null;
      localVarRequestOptions.headers = Object.assign(
        {},
        localVarHeaderParameter,
        options.headers
      );
      localVarRequestOptions.body = JSON.stringify(body || {});
      return {
        url: url.format(localVarUrlObj),
        options: localVarRequestOptions,
      };
    },
  };
};

export const AssetLevelReportForExodusApiFp = function (
  configuration?: Configuration
) {
  return {
    GetAssetLevelReportForExodus(
      body: GetAssetLevelReportForExodusDTO,
      options?: any
    ): (
      fetch?: FetchAPI,
      basePath?: string
    ) => Promise<AssetLevelReportForExodusResponse> {
      const localVarFetchArgs = AssetLevelReportForExodusApiFetchParamCreator(
        configuration
      ).GetAssetLevelReportForExodus(body, options);
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
    GetOpcoAndPlannnedDcfDropdown(
      body?: any,
      options?: any
    ): (
      fetch?: FetchAPI,
      basePath?: string
    ) => Promise<OpcoDcfDropdownResponse> {
      const args = AssetLevelReportForExodusApiFetchParamCreator(
        configuration
      ).GetOpcoAndPlannnedDcfDropdown(body ?? {}, options);
      return (fetch: FetchAPI = isomorphicFetch, basePath = BASE_PATH) =>
        fetch(basePath + args.url, args.options).then((response) => {
          if (response.status >= 200 && response.status < 300)
            return response.json();
          throw response;
        });
    },
  };
};

export const AssetLevelReportForExodusApiFactory = function (
  configuration?: Configuration,
  fetch?: FetchAPI,
  basePath?: string
) {
  return {
    GetAssetLevelReportForExodus(
      body: GetAssetLevelReportForExodusDTO,
      options?: any
    ) {
      return AssetLevelReportForExodusApiFp(
        configuration
      ).GetAssetLevelReportForExodus(body, options)(fetch, basePath);
    },
    GetOpcoAndPlannnedDcfDropdown(body?: any, options?: any) {
      return AssetLevelReportForExodusApiFp(
        configuration
      ).GetOpcoAndPlannnedDcfDropdown(body ?? {}, options)(fetch, basePath);
    },
  };
};

export class AssetLevelReportForExodusApi extends BaseAPI {
  public GetAssetLevelReportForExodus(
    body: GetAssetLevelReportForExodusDTO,
    options?: any
  ) {
    return AssetLevelReportForExodusApiFp(
      this.configuration
    ).GetAssetLevelReportForExodus(body, options)(this.fetch, this.basePath);
  }
  GetOpcoAndPlannnedDcfDropdown(body?: any, options?: any) {
    return AssetLevelReportForExodusApiFp(
      this.configuration
    ).GetOpcoAndPlannnedDcfDropdown(body ?? {}, options)(
      this.fetch,
      this.basePath
    );
  }
}
