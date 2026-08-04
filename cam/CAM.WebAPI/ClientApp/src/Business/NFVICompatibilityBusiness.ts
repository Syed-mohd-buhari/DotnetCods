// NFVICompatibilityBusiness.ts

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
  FilterValueDto,
} from "./Common/CommonBusiness";

import {
  NFVICCompatibilityQueryDto,
  NFVICompatibilityReportRequest,
  NFVICompatibilityReportResponse,
  QueryResultDtoOfNFVICCompatibilityDtoGrid,
  VmWareDropdownResponse,
} from "../Model/NfvicCompatible";
import { ReturnFile } from "../Model/Common";

export const NFVICCompatibleApiFetchParamCreator = function (
  configuration?: Configuration
) {
  return {
    nfvicCompatibleGetGrid(
      vmVarId: number,
      body: NFVICCompatibilityQueryDto,
      options: any = {}
    ): FetchArgs {
      if (!body) {
        throw new RequiredError(
          "body",
          "Missing body for nfvicCompatibleGetGrid."
        );
      }

      // const localVarPath = `/api/test/Get?vmVarId=${vmVarId}`;
      const localVarPath = `/api/NFVICompatibilityAtGlance/Get?vmVarId=${vmVarId}`;
      const localVarUrlObj = url.parse(localVarPath, true);
      const localVarRequestOptions = Object.assign({ method: "POST" }, options);
      const localVarHeaderParameter = { ...headerObj };

      // ✅ Add Authorization header if configuration has API key
      if (configuration?.apiKey) {
        localVarHeaderParameter["Authorization"] =
          typeof configuration.apiKey === "function"
            ? configuration.apiKey("Authorization")
            : configuration.apiKey;
      }

      localVarHeaderParameter["Content-Type"] = "application/json";

      localVarUrlObj.query = Object.assign(
        {},
        localVarUrlObj.query,
        options.query
      );
      localVarUrlObj.search = null;

      localVarRequestOptions.headers = localVarHeaderParameter;
      localVarRequestOptions.body = JSON.stringify(body);

      return {
        url: url.format(localVarUrlObj),
        options: localVarRequestOptions,
      };
    },

    nfvicCompatibleGetAllVmware(options: any = {}): FetchArgs {
      const localVarPath = `/api/NFVICompatibilityAtGlance/GetAllDropDown`;
      const localVarUrlObj = url.parse(localVarPath, true);
      const localVarRequestOptions = Object.assign({ method: "POST" }, options);
      const localVarHeaderParameter = { ...headerObj };

      // ✅ Add Authorization header here too
      if (configuration?.apiKey) {
        localVarHeaderParameter["Authorization"] =
          typeof configuration.apiKey === "function"
            ? configuration.apiKey("Authorization")
            : configuration.apiKey;
      }

      localVarHeaderParameter["Content-Type"] = "application/json";

      localVarUrlObj.query = Object.assign(
        {},
        localVarUrlObj.query,
        options.query
      );
      localVarUrlObj.search = null;

      localVarRequestOptions.headers = localVarHeaderParameter;
      localVarRequestOptions.body = JSON.stringify({}); // empty body for POST

      return {
        url: url.format(localVarUrlObj),
        options: localVarRequestOptions,
      };
    },
    nfvicCompatibleGetReport(
      vmVarId: number,
      body: NFVICompatibilityReportRequest,
      options: any = {}
    ): FetchArgs {
      if (!body) {
        throw new RequiredError(
          "body",
          "Missing body for nfvicCompatibleGetReport."
        );
      }

      const localVarPath = `/api/NFVICompatibilityAtGlance/GetGraphicalReport?vmVarId=${vmVarId}`;
      const localVarUrlObj = url.parse(localVarPath, true);
      const localVarRequestOptions = Object.assign({ method: "POST" }, options);
      const localVarHeaderParameter = { ...headerObj };

      if (configuration?.apiKey) {
        localVarHeaderParameter["Authorization"] =
          typeof configuration.apiKey === "function"
            ? configuration.apiKey("Authorization")
            : configuration.apiKey;
      }

      localVarHeaderParameter["Content-Type"] = "application/json";

      localVarUrlObj.query = Object.assign(
        {},
        localVarUrlObj.query,
        options.query
      );
      localVarUrlObj.search = null;

      localVarRequestOptions.headers = localVarHeaderParameter;
      localVarRequestOptions.body = JSON.stringify(body);

      return {
        url: url.format(localVarUrlObj),
        options: localVarRequestOptions,
      };
    },
    NFVICompatibleExportReport(
      vmVarId: number,
      body: NFVICompatibilityReportRequest,
      options: any = {}
    ): FetchArgs {
      if (!body) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling NFVICompatibleExportReport."
        );
      }

      const localVarPath = `/api/NFVICompatibilityAtGlance/ExportReport?vmVarId=${vmVarId}`;
      const localVarUrlObj = url.parse(localVarPath, true);
      const localVarRequestOptions = Object.assign({ method: "POST" }, options);
      const localVarHeaderParameter = { ...headerObj };

      if (configuration?.apiKey) {
        localVarHeaderParameter["Authorization"] =
          typeof configuration.apiKey === "function"
            ? configuration.apiKey("Authorization")
            : configuration.apiKey;
      }

      localVarHeaderParameter["Content-Type"] = "application/json";

      localVarUrlObj.query = Object.assign(
        {},
        localVarUrlObj.query,
        options.query
      );
      localVarUrlObj.search = null;

      localVarRequestOptions.headers = localVarHeaderParameter;
      localVarRequestOptions.body = JSON.stringify(body || {});

      return {
        url: url.format(localVarUrlObj),
        options: localVarRequestOptions,
      };
    },
  };
};

export class NFVICCompatibleApi extends BaseAPI {
  public async nfvicCompatibleGetGrid(
    vmVarId: number,
    body: NFVICCompatibilityQueryDto
  ) {
    const fetchArgs = NFVICCompatibleApiFetchParamCreator(
      this.configuration
    ).nfvicCompatibleGetGrid(vmVarId, body);

    return this.fetch(this.basePath + fetchArgs.url, fetchArgs.options).then(
      async (response) => {
        if (response.ok) return response.json();
        throw response;
      }
    );
  }

  public async nfvicCompatibleGetAllVmware() {
    const fetchArgs = NFVICCompatibleApiFetchParamCreator(
      this.configuration
    ).nfvicCompatibleGetAllVmware();

    return this.fetch(this.basePath + fetchArgs.url, fetchArgs.options).then(
      async (response) => {
        if (response.ok)
          return response.json() as Promise<VmWareDropdownResponse>;
        throw response;
      }
    );
  }

  public async nfvicCompatibleGetReport(
    vmVarId: number,
    body: NFVICompatibilityReportRequest
  ) {
    const fetchArgs = NFVICCompatibleApiFetchParamCreator(
      this.configuration
    ).nfvicCompatibleGetReport(vmVarId, body);

    return this.fetch(this.basePath + fetchArgs.url, fetchArgs.options).then(
      async (response) => {
        if (response.ok) {
          return response.json() as Promise<NFVICompatibilityReportResponse>;
        }
        throw response;
      }
    );
  }

  public async NFVICompatibleExportReport(
    vmVarId: number,
    body: NFVICompatibilityReportRequest
  ): Promise<ReturnFile> {
    const fetchArgs = NFVICCompatibleApiFetchParamCreator(
      this.configuration
    ).NFVICompatibleExportReport(vmVarId, body);

    return this.fetch(this.basePath + fetchArgs.url, fetchArgs.options).then(
      async (response) => {
        if (response.ok) {
          return {
            File: response.blob(), // blob for file download
            FileName: response.headers.get("Content-Disposition") || "",
          };
        }
        throw response;
      }
    );
  }
}
