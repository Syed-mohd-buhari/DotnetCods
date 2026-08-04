import * as url from "url";
import * as portableFetch from "portable-fetch";
import * as isomorphicFetch from "isomorphic-fetch";

import { Configuration } from "./Common/configuration";
import { ResultDto, ResultDtoOfListOfShort } from "../Model/CommonModels";
import {
  BASE_PATH,
  FetchArgs,
  FetchAPI,
  RequiredError,
  FilterValueDto,
} from "./Common/CommonBusiness";
import { BaseAPI } from "./Common/CommonBusiness";
import {
  PlannedActivityDtoCreate,
  PlannedActivityDtoUpdate,
  QueryResultDtoOfPlannedActivityDtoGrid,
  ResultDtoOfPlannedActivityForLinkDto,
  PlannedActivityMigrationsDto,
  UpdatePlannedActivityStatusDto,
  ResultDtoOfUpdatePlannedActivityStatusDto,
  ResultDtoOfDictionaryOfLongAndPlannedActivityToConnectData,
  PaWhenDaMigrationCompleteApiResponse,
} from "../Model/PlannedActivity";
import { headerObj } from "./header";

import { PlannedActivityTypeForEnum, ReturnFile } from "../Model/Common";
import {
  PlannedActivityQueryObjectGrid as PlannedActivityQueryDto,
  PlannedActivityConfirmationDto,
  PaWithDaMigrationApiResponse,
} from "../Model/PlannedActivity";
import { SettingsUpdatePlannedActivityDtoUpdate } from "../Model/SettingsUpdatePlannedActivity";

/**
 * PlannedActivityApi - fetch parameter creator
 * @export
 */
export const PlannedActivityApiFetchParamCreator = function (
  configuration?: Configuration
) {
  return {
    /**
     *
     * @param {number} [deliveryId]
     * @param {number} [budgetAvId]
     * @param {number} [responsibilityPhase]
     * @param {boolean} [localApproval]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    plannedActivityActivityStatusLogics(
      deliveryId?: number,
      budgetAvId?: number,
      responsibilityPhase?: number,
      localApproval?: boolean,
      options: any = {}
    ): FetchArgs {
      const localVarPath = `/api/PlannedActivity/ActivityStatusLogics`;
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

      if (deliveryId !== undefined) {
        localVarQueryParameter["deliveryId"] = deliveryId;
      }

      if (budgetAvId !== undefined) {
        localVarQueryParameter["budgetAvId"] = budgetAvId;
      }

      if (responsibilityPhase !== undefined) {
        localVarQueryParameter["responsibilityPhase"] = responsibilityPhase;
      }

      if (localApproval !== undefined) {
        localVarQueryParameter["localApproval"] = localApproval;
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
     * @param {number} [year]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    plannedActivityCheckFiscalYear(
      year?: number,
      options: any = {}
    ): FetchArgs {
      const localVarPath = `/api/PlannedActivity/CheckFiscalYear`;
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

      if (year !== undefined) {
        localVarQueryParameter["year"] = year;
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

    getPlannedActivityRelatedDeliveryStatus(
      plannedActivityId?: number,
      plannedActivityTypeFor?: PlannedActivityTypeForEnum,
      options: any = {}
    ): FetchArgs {
      const localVarPath = `/api/PlannedActivity/GetPlannedActivityRelatedDeliveryStatus`;
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

      if (plannedActivityId !== undefined) {
        localVarQueryParameter["id"] = plannedActivityId;
      }

      if (
        plannedActivityTypeFor !== undefined &&
        plannedActivityTypeFor !== null
      ) {
        localVarQueryParameter["plannedActivityTypeFor"] =
          plannedActivityTypeFor;
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
     * @param {any} [pagewiseId]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    GetPlannedRuleConfig(pagewiseId, options: any = {}): FetchArgs {
      let payload = {
        plannedActivityTypeDescription: "",
        plannedActivityTypeId: 0,
        pagewiseId: pagewiseId,
      };
      const localVarPath = `/api/PlannedActivityTypes/GetPlannedDropDownList`;
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
        <any>"PlannedActivityQueryDto" !== "string" ||
        localVarRequestOptions.headers["Content-Type"] === "application/json";
      localVarRequestOptions.body = needsSerialization
        ? JSON.stringify(payload || {})
        : payload || "";

      return {
        url: url.format(localVarUrlObj),
        options: localVarRequestOptions,
      };
    },

    getLcmDeploymentStatusRelatedDeliveryStatusAndPAResource(
      plannedActivityResourceId?: number,
      deliveryStatusId?: number,
      options: any = {}
    ): FetchArgs {
      const localVarPath = `/api/LcmEngineering/GetLcmDeploymentStatusRelatedDeliveryStatusAndPAResource`;
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

      if (plannedActivityResourceId !== undefined) {
        localVarQueryParameter["plannedActivityResourceId"] =
          plannedActivityResourceId;
      }

      if (deliveryStatusId !== undefined) {
        localVarQueryParameter["deliveryStatusId"] = deliveryStatusId;
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
     * @param {PlannedActivityDtoCreate} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    plannedActivityCreate(
      body: PlannedActivityDtoCreate,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling plannedActivityCreate."
        );
      }
      const localVarPath = `/api/PlannedActivity`;
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
        <any>"PlannedActivityDtoCreate" !== "string" ||
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
     * @param {any} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    saveUpdatedAssetDetails(body: any, options: any = {}): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling saveUpdatedAssetDetails."
        );
      }
      const localVarPath = `/api/LcmEngineering/CreateOrUpdateAssetOnLCM`;
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
        <any>"PlannedActivityDtoCreate" !== "string" ||
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
     * @param {number} [id]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    plannedActivityDelete(id?: number, options: any = {}): FetchArgs {
      const localVarPath = `/api/PlannedActivity/Delete`;
      const localVarUrlObj = url.parse(localVarPath, true);
      const localVarRequestOptions = Object.assign(
        { method: "DELETE" },
        options
      );
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

      if (id !== undefined) {
        localVarQueryParameter["id"] = id;
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
     * @param {number} [id]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    getLocationTypeById(locationId?: number, options: any = {}): FetchArgs {
      // verify required parameter 'opCoId' is not null or undefined
      if (locationId === null || locationId === undefined) {
        throw new RequiredError(
          "opCoId",
          "Required parameter opCoId was null or undefined when calling plannedActivityGetDesignComponentList."
        );
      }
      const localVarPath =
        `/api/location/GetLocationDeploymentTypeRelated{locationId}`.replace(
          `{${"locationId"}}`,
          encodeURIComponent(String(locationId))
        );
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
     * @param {number} [id]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    plannedActivityDeleteDeep(id?: number, options: any = {}): FetchArgs {
      const localVarPath = `/api/PlannedActivity/DeleteDeep`;
      const localVarUrlObj = url.parse(localVarPath, true);
      const localVarRequestOptions = Object.assign(
        { method: "DELETE" },
        options
      );
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

      if (id !== undefined) {
        localVarQueryParameter["id"] = id;
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
     * @param {PlannedActivityQueryDto} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    plannedActivityExportReport(
      body: PlannedActivityQueryDto,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling plannedActivityExportReport."
        );
      }
      const localVarPath = `/api/PlannedActivity/ExportReport`;
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
        <any>"PlannedActivityQueryDto" !== "string" ||
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
     * @param {PlannedActivityQueryDto} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    plannedActivityArchivedExportReport(
      body: PlannedActivityQueryDto,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling plannedActivityArchivedExportReport."
        );
      }
      const localVarPath = `/api/PlannedActivity/ExportArchivedPlannedActivitiesReport`;
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
        <any>"PlannedActivityQueryDto" !== "string" ||
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
     * @param {number} [id]
     * @param {number} [rule]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    plannedActivityGetActivityDetails(
      id?: number,
      rule?: number,
      options: any = {}
    ): FetchArgs {
      const localVarPath = `/api/PlannedActivity/GetActivityDetails`;
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

      if (id !== undefined) {
        localVarQueryParameter["id"] = id;
      }

      if (rule !== undefined) {
        localVarQueryParameter["rule"] = rule;
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
     * @param {number} [dc]
     * @param {number} [pdc]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    plannedActivityGetConfrontoHardwareTypeFromDesignComponent(
      dc?: number,
      pdc?: number,
      options: any = {}
    ): FetchArgs {
      const localVarPath = `/api/PlannedActivity/GetConfrontoHardwareType`;
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

      if (dc !== undefined) {
        localVarQueryParameter["dc"] = dc;
      }

      if (pdc !== undefined) {
        localVarQueryParameter["pdc"] = pdc;
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
     * @param {number} dcId
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    plannedActivityGetCreateResourcePlannedActivity(
      dcId?: number,
      DcfId?: number,
      options: any = {}
    ): FetchArgs {
      const localVarPath = `/api/PlannedActivity/Create${
        (dcId !== null && dcId !== undefined ? `?DcId=${dcId}` : "") +
        (DcfId !== null && DcfId !== undefined
          ? `${dcId !== null && dcId !== undefined ? "&" : "?"}DcfId=${DcfId}`
          : "")
      }`;
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
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    plannedActivityGetCreateUpdatePlannedActivityStatus(
      options: any = {}
    ): FetchArgs {
      const localVarPath = `/api/PlannedActivity/GetCreateUpdatePlannedActivityStatus`;
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
     * @param {number} opCoId
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    plannedActivityGetDesignComponentList(
      opCoId: number,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'opCoId' is not null or undefined
      if (opCoId === null || opCoId === undefined) {
        throw new RequiredError(
          "opCoId",
          "Required parameter opCoId was null or undefined when calling plannedActivityGetDesignComponentList."
        );
      }
      const localVarPath =
        `/api/PlannedActivity/GetDesignComponentList{opCoId}`.replace(
          `{${"opCoId"}}`,
          encodeURIComponent(String(opCoId))
        );
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
     * @param {PlannedActivityQueryDto} body
     * @param {string} [propertyName]
     * @param {string} [propertyFilter]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    plannedActivityGetFilterResult(
      body: PlannedActivityQueryDto,
      propertyName?: string,
      propertyFilter?: string,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling plannedActivityGetFilterResult."
        );
      }
      const localVarPath = `/api/PlannedActivity/Filter`;
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
        <any>"PlannedActivityQueryDto" !== "string" ||
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
     * @param {PlannedActivityQueryDto} body
     * @param {string} [propertyName]
     * @param {string} [propertyFilter]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    plannedActivityGetArchivedFilterResult(
      body: PlannedActivityQueryDto,
      propertyName?: string,
      propertyFilter?: string,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling plannedActivityGetArchivedFilterResult."
        );
      }
      const localVarPath = `/api/PlannedActivity/ArchivedPlannedActivityFilter`;
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
        <any>"PlannedActivityQueryDto" !== "string" ||
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
     * @param {number} [designComponentId]
     * @param {number} [opCoId]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    plannedActivityGetLcmEngineeringPlannedActivity(
      designComponentId?: number,
      opCoId?: number,
      options: any = {}
    ): FetchArgs {
      const localVarPath = `/api/PlannedActivity/GetLcmEngineeringPlannedActivity`;
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

      if (designComponentId !== undefined) {
        localVarQueryParameter["designComponentId"] = designComponentId;
      }

      if (opCoId !== undefined) {
        localVarQueryParameter["opCoId"] = opCoId;
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
     * @param {number} [id]
     * @param {number} [rule]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    plannedActivityGetLinkedDesignComponent(
      id?: number,
      rule?: number,
      options: any = {}
    ): FetchArgs {
      const localVarPath = `/api/PlannedActivity/GetLinkedDesignComponent`;
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

      if (id !== undefined) {
        localVarQueryParameter["id"] = id;
      }

      if (rule !== undefined) {
        localVarQueryParameter["rule"] = rule;
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
     * @param {number} [id]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    plannedActivityCreateUnkownDCPALevel(
      id?: number,
      options: any = {}
    ): FetchArgs {
      const localVarPath = `/api/LcmEngineering/CreateUnkownDCPALevel`;
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

      if (id !== undefined) {
        localVarQueryParameter["dcId"] = id;
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
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    plannedActivityGetOpCoList(options: any = {}): FetchArgs {
      const localVarPath = `/api/PlannedActivity/GetOpCoList`;
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
     * @param {PlannedActivityQueryDto} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    plannedActivityGetPlannedActivity(
      body: PlannedActivityQueryDto,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling plannedActivityGetPlannedActivity."
        );
      }
      const localVarPath = `/api/PlannedActivity/Get`;
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
        <any>"PlannedActivityQueryDto" !== "string" ||
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
     * @param {PlannedActivityQueryDto} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    plannedActivityGetArchivedPlannedActivity(
      body: PlannedActivityQueryDto,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling plannedActivityGetArchivedPlannedActivity."
        );
      }
      const localVarPath = `/api/PlannedActivity/GetArchivedPlannedActivity`;
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
        <any>"PlannedActivityQueryDto" !== "string" ||
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
     * @param {number} id
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    plannedActivityGetPlannedActivityForLink(
      id: number,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'id' is not null or undefined
      if (id === null || id === undefined) {
        throw new RequiredError(
          "id",
          "Required parameter id was null or undefined when calling plannedActivityGetPlannedActivityForLink."
        );
      }
      const localVarPath =
        `/api/PlannedActivity/GetPlannedActivityForLink{id}`.replace(
          `{${"id"}}`,
          encodeURIComponent(String(id))
        );
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
     * @param {number} [dcId]
     * @param {number} [opcoId]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    plannedActivityGetPlannedActivityListForMigration(
      dcId?: number,
      opcoId?: number,
      options: any = {}
    ): FetchArgs {
      const localVarPath = `/api/PlannedActivity/GetPlannedActivityListForMigration`;
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

      if (dcId !== undefined) {
        localVarQueryParameter["dcId"] = dcId;
      }

      if (opcoId !== undefined) {
        localVarQueryParameter["opcoId"] = opcoId;
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
     * @param {number} [dcId]
     * @param {number} [opcoId]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    plannedActivityGetPlannedActivityTypeswithDcIdAndOpcoId(
      dcId?: number,
      opcoId?: number,
      options: any = {}
    ): FetchArgs {
      const localVarPath = `/api/PlannedActivity/GetPlannedActivityTypeswithDcIdAndOpcoId`;
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

      if (dcId !== undefined) {
        localVarQueryParameter["dcId"] = dcId;
      }

      if (opcoId !== undefined) {
        localVarQueryParameter["opcoId"] = opcoId;
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
    GetUpdateDaMigrationRecords(id: number, options: any = {}): FetchArgs {
      // verify required parameter 'id' is not null or undefined
      if (id === null || id === undefined) {
        throw new RequiredError(
          "id",
          "Required parameter id was null or undefined when calling GetUpdateDaMigrationRecords."
        );
      }
      const localVarPath =
        `/api/DaMigrationStatus/GetUpdateDaMigrationRecords{id}`.replace(
          `{${"id"}}`,
          encodeURIComponent(String(id))
        );
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
     * @param {number} [plannedActivityTypeId]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    plannedActivityCheckPlannedActivityTypefor(
      designComponentId?: number,
      opCoId?: number,
      plannedActivityTypeId?: number,
      options: any = {}
    ): FetchArgs {
      const localVarPath = `/api/PlannedActivity/CheckPlannedActivityTypefor`;
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

      if (plannedActivityTypeId !== undefined) {
        localVarQueryParameter["plannedActivityTypeId"] = plannedActivityTypeId;
      }

      if (designComponentId !== undefined) {
        localVarQueryParameter["designComponentId"] = designComponentId;
      }

      if (opCoId !== undefined) {
        localVarQueryParameter["opCoId"] = opCoId;
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
     * @param {number} [dcId]
     * @param {number} [opcoId]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    plannedActivityGetPlannedActivityListForUpdatePlannedActivityStatus(
      dcId?: number,
      opcoId?: number,
      plannedActivityTypeId?: number,
      plannedActivityTypeFor?: number,
      options: any = {}
    ): FetchArgs {
      const localVarPath = `/api/PlannedActivity/GetPlannedActivityListForUpdatePlannedActivityStatus`;
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

      if (dcId !== undefined) {
        localVarQueryParameter["dcId"] = dcId;
      }

      if (opcoId !== undefined) {
        localVarQueryParameter["opcoId"] = opcoId;
      }

      if (plannedActivityTypeId !== undefined) {
        localVarQueryParameter["plannedActivityTypeId"] = plannedActivityTypeId;
      }

      if (
        plannedActivityTypeFor !== undefined &&
        plannedActivityTypeFor !== null
      ) {
        localVarQueryParameter["plannedActivityTypeFor"] =
          plannedActivityTypeFor;
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

    plannedActivityGetSettingUpdatePlannedActivityResource(
      plannedActivityTypeId?: number,
      plannedActivityTypeFor?: number,
      options: any = {}
    ): FetchArgs {
      const localVarPath = `/api/PlannedActivity/GetSettingUpdatePlannedActivityResource`;
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

      if (plannedActivityTypeId !== undefined) {
        localVarQueryParameter["plannedActivityResourceId"] =
          plannedActivityTypeId;
      }

      if (plannedActivityTypeFor !== undefined) {
        localVarQueryParameter["plannedActivityTypeFor"] =
          plannedActivityTypeFor;
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
     * @param {number} id
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    plannedActivityGetRelatedRecords(id: number, options: any = {}): FetchArgs {
      // verify required parameter 'id' is not null or undefined
      if (id === null || id === undefined) {
        throw new RequiredError(
          "id",
          "Required parameter id was null or undefined when calling plannedActivityGetRelatedRecords."
        );
      }
      const localVarPath = `/api/PlannedActivity/GetRelatedRecords{id}`.replace(
        `{${"id"}}`,
        encodeURIComponent(String(id))
      );
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
     * @param {number} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    plannedActivityGetUpdatePlannedActivityStatus(
      plannedId: number,
      plannedActivityTypeFor: PlannedActivityTypeForEnum,
      options: any = {}
    ): FetchArgs {
      const localVarPath = `/api/PlannedActivity/GetUpdatePlannedActivityStatus?id=${plannedId}&plannedActivityTypeFor=${plannedActivityTypeFor}`;

      const localVarUrlObj = url.parse(localVarPath, true);
      const localVarRequestOptions = Object.assign({ method: "GET" }, options);
      const localVarHeaderParameter = { ...headerObj } as any;
      const localVarQueryParameter = {} as any;

      // verify required parameter 'id' is not null or undefined
      // if (plannedId === null || plannedId === undefined) {
      //   localVarQueryParameter["plannedId"] = plannedId;
      // }

      // if (
      //   plannedActivityTypeFor === null ||
      //   plannedActivityTypeFor === undefined
      // ) {
      //   localVarQueryParameter["plannedActivityTypeFor"] =
      //     plannedActivityTypeFor;
      // }

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
     * @param {number} id
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    plannedActivityGetUpdateResourcePlannedActivity(
      id: number,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'id' is not null or undefined
      if (id === null || id === undefined) {
        throw new RequiredError(
          "id",
          "Required parameter id was null or undefined when calling plannedActivityGetUpdateResourcePlannedActivity."
        );
      }
      const localVarPath = `/api/PlannedActivity/Update{id}`.replace(
        `{${"id"}}`,
        encodeURIComponent(String(id))
      );
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
     * @param {PlannedActivityMigrationsDto} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    plannedActivityPlannedActivityMigrations(
      body: PlannedActivityMigrationsDto,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling plannedActivityPlannedActivityMigrations."
        );
      }
      const localVarPath = `/api/LcmEngineering/PlannedActivityMigrations`;
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
        <any>"PlannedActivityMigrationsDto" !== "string" ||
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
     * @param {PlannedActivityDtoUpdate} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    plannedActivityPut(
      body: PlannedActivityDtoUpdate,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling plannedActivityPut."
        );
      }
      const localVarPath = `/api/PlannedActivity`;
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
        <any>"PlannedActivityDtoUpdate" !== "string" ||
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
     * @param {UpdatePlannedActivityStatusDto} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    plannedActivitySaveUpdatePlannedActivityStatus(
      body: UpdatePlannedActivityStatusDto,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling plannedActivitySaveUpdatePlannedActivityStatus."
        );
      }
      const localVarPath = `/api/PlannedActivity/SaveUpdatePlannedActivityStatus`;
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
        <any>"UpdatePlannedActivityStatusDto" !== "string" ||
        localVarRequestOptions.headers["Content-Type"] === "application/json";
      localVarRequestOptions.body = needsSerialization
        ? JSON.stringify(body || {})
        : body || "";

      return {
        url: url.format(localVarUrlObj),
        options: localVarRequestOptions,
      };
    },

    plannedActivityConfirmation(
      body: PlannedActivityConfirmationDto,
      options: any = {}
    ): FetchArgs {
      // Check if body is null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling plannedActivityConfirmation."
        );
      }

      const localVarPath = `/api/DesignAspectPlannedActivity/GetDcfAssociatedEntities`;
      const localVarUrlObj = url.parse(localVarPath, true);
      const localVarRequestOptions = Object.assign({ method: "POST" }, options);
      const localVarHeaderParameter = { ...headerObj } as any;
      const localVarQueryParameter = {} as any;

      // Add JWT token if available
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

      // Fix override query string
      localVarUrlObj.search = null;

      localVarRequestOptions.headers = Object.assign(
        {},
        localVarHeaderParameter,
        options.headers
      );

      const needsSerialization =
        <any>"PlannedActivityConfirmationDto" !== "string" ||
        localVarRequestOptions.headers["Content-Type"] === "application/json";

      localVarRequestOptions.body = needsSerialization
        ? JSON.stringify(body || {})
        : body || "";

      return {
        url: url.format(localVarUrlObj),
        options: localVarRequestOptions,
      };
    },

    updatePaWithDaMigration(
      body: PaWithDaMigrationApiResponse,
      paId: number,
      deliveryStatusId: number,
      options: any = {}
    ): FetchArgs {
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling updatePaWithDaMigration."
        );
      }

      if (paId === null || paId === undefined) {
        throw new RequiredError(
          "paId",
          "Required parameter paId was null or undefined."
        );
      }

      if (deliveryStatusId === null || deliveryStatusId === undefined) {
        throw new RequiredError(
          "deliveryStatusId",
          "Required parameter deliveryStatusId was null or undefined."
        );
      }

      const localVarPath = `/api/PlannedActivity/ArchivePAWithDaMigration`;
      const localVarUrlObj = url.parse(localVarPath, true);

      const localVarRequestOptions = Object.assign({ method: "POST" }, options);
      const localVarHeaderParameter = { ...headerObj } as any;
      const localVarQueryParameter = {
        PaId: paId,
        DeliveyStatusId: deliveryStatusId,
      };

      // Add JWT token if available
      if (configuration && configuration.apiKey) {
        const localVarApiKeyValue =
          typeof configuration.apiKey === "function"
            ? configuration.apiKey("Authorization")
            : configuration.apiKey;
        localVarHeaderParameter["Authorization"] = localVarApiKeyValue;
      }

      localVarHeaderParameter["Content-Type"] = "application/json";

      // Attach query params to URL
      localVarUrlObj.query = Object.assign(
        {},
        localVarUrlObj.query,
        localVarQueryParameter,
        options.query
      );

      // Clear cached search string to regenerate with new query
      localVarUrlObj.search = null;

      // Attach headers
      localVarRequestOptions.headers = Object.assign(
        {},
        localVarHeaderParameter,
        options.headers
      );

      // Set body
      const needsSerialization =
        <any>"PaWithDaMigrationApiResponse" !== "string" ||
        localVarRequestOptions.headers["Content-Type"] === "application/json";

      localVarRequestOptions.body = needsSerialization
        ? JSON.stringify(body || {})
        : body || "";

      return {
        url: url.format(localVarUrlObj), // URL with query params
        options: localVarRequestOptions, // POST with body and headers
      };
    },
    updatePaWhenDaMigrationCompleted(
      body: PaWhenDaMigrationCompleteApiResponse,
      paId: number,
      deliveryStatusId: number,
      options: any = {}
    ): FetchArgs {
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling updatePaWhenDaMigrationCompleted."
        );
      }

      if (paId === null || paId === undefined) {
        throw new RequiredError(
          "paId",
          "Required parameter paId was null or undefined."
        );
      }

      if (deliveryStatusId === null || deliveryStatusId === undefined) {
        throw new RequiredError(
          "deliveryStatusId",
          "Required parameter deliveryStatusId was null or undefined."
        );
      }

      const localVarPath = `/api/PlannedActivity/ArchivePaWhenDaAssetMigrationComplete`;
      const localVarUrlObj = url.parse(localVarPath, true);

      const localVarRequestOptions = Object.assign({ method: "POST" }, options);
      const localVarHeaderParameter = { ...headerObj } as any;
      const localVarQueryParameter = {
        PaId: paId,
        DeliveyStatusId: deliveryStatusId,
      };

      // Add JWT token if available
      if (configuration && configuration.apiKey) {
        const localVarApiKeyValue =
          typeof configuration.apiKey === "function"
            ? configuration.apiKey("Authorization")
            : configuration.apiKey;
        localVarHeaderParameter["Authorization"] = localVarApiKeyValue;
      }

      localVarHeaderParameter["Content-Type"] = "application/json";

      // Attach query params to URL
      localVarUrlObj.query = Object.assign(
        {},
        localVarUrlObj.query,
        localVarQueryParameter,
        options.query
      );

      // Clear cached search string to regenerate with new query
      localVarUrlObj.search = null;

      // Attach headers
      localVarRequestOptions.headers = Object.assign(
        {},
        localVarHeaderParameter,
        options.headers
      );

      // Set body
      const needsSerialization =
        <any>"PaWhenDaMigrationCompleteApiResponse" !== "string" ||
        localVarRequestOptions.headers["Content-Type"] === "application/json";

      localVarRequestOptions.body = needsSerialization
        ? JSON.stringify(body || {})
        : body || "";

      return {
        url: url.format(localVarUrlObj), // URL with query params
        options: localVarRequestOptions, // POST with body and headers
      };
    },
  };
};

/**
 * PlannedActivityApi - functional programming interface
 * @export
 */
export const PlannedActivityApiFp = function (configuration?: Configuration) {
  return {
    /**
     *
     * @param {number} [deliveryId]
     * @param {number} [budgetAvId]
     * @param {number} [responsibilityPhase]
     * @param {boolean} [localApproval]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    plannedActivityActivityStatusLogics(
      deliveryId?: number,
      budgetAvId?: number,
      responsibilityPhase?: number,
      localApproval?: boolean,
      options?: any
    ): (
      fetch?: FetchAPI,
      basePath?: string
    ) => Promise<ResultDtoOfListOfShort> {
      const localVarFetchArgs = PlannedActivityApiFetchParamCreator(
        configuration
      ).plannedActivityActivityStatusLogics(
        deliveryId,
        budgetAvId,
        responsibilityPhase,
        localApproval,
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
     * @param {number} [year]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    plannedActivityCheckFiscalYear(
      year?: number,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<boolean> {
      const localVarFetchArgs = PlannedActivityApiFetchParamCreator(
        configuration
      ).plannedActivityCheckFiscalYear(year, options);
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

    getPlannedActivityRelatedDeliveryStatus(
      plannedActivityId?: number,
      plannedActivityTypeFor?: PlannedActivityTypeForEnum,
      options?: any
    ): (
      fetch?: FetchAPI,
      basePath?: string
    ) => Promise<{ [key: string]: string }> {
      const localVarFetchArgs = PlannedActivityApiFetchParamCreator(
        configuration
      ).getPlannedActivityRelatedDeliveryStatus(
        plannedActivityId,
        plannedActivityTypeFor,
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
     * @param {any} [pagewiseId]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    GetPlannedRuleConfig(
      pagewiseId?: any,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = PlannedActivityApiFetchParamCreator(
        configuration
      ).GetPlannedRuleConfig(pagewiseId, options);
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
    getLcmDeploymentStatusRelatedDeliveryStatusAndPAResource(
      plannedActivityResourceId?: number,
      deliveryStatusId?: number,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = PlannedActivityApiFetchParamCreator(
        configuration
      ).getLcmDeploymentStatusRelatedDeliveryStatusAndPAResource(
        plannedActivityResourceId,
        deliveryStatusId,
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
     * @param {PlannedActivityDtoCreate} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    plannedActivityCreate(
      body: PlannedActivityDtoCreate,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = PlannedActivityApiFetchParamCreator(
        configuration
      ).plannedActivityCreate(body, options);
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
    saveUpdatedAssetDetails(
      body: any,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = PlannedActivityApiFetchParamCreator(
        configuration
      ).saveUpdatedAssetDetails(body, options);
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
    plannedActivityDelete(
      id?: number,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = PlannedActivityApiFetchParamCreator(
        configuration
      ).plannedActivityDelete(id, options);
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
    getLocationTypeById(
      locationId?: number,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = PlannedActivityApiFetchParamCreator(
        configuration
      ).getLocationTypeById(locationId, options);
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
    plannedActivityDeleteDeep(
      id?: number,
      options?: any
    ): (
      fetch?: FetchAPI,
      basePath?: string
    ) => Promise<{ [key: string]: string }> {
      const localVarFetchArgs = PlannedActivityApiFetchParamCreator(
        configuration
      ).plannedActivityDeleteDeep(id, options);
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
     * @param {PlannedActivityQueryDto} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    plannedActivityExportReport(
      body: PlannedActivityQueryDto,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ReturnFile> {
      const localVarFetchArgs = PlannedActivityApiFetchParamCreator(
        configuration
      ).plannedActivityExportReport(body, options);
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
     *
     * @param {PlannedActivityQueryDto} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    plannedActivityArchivedExportReport(
      body: PlannedActivityQueryDto,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ReturnFile> {
      const localVarFetchArgs = PlannedActivityApiFetchParamCreator(
        configuration
      ).plannedActivityArchivedExportReport(body, options);
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
     *
     * @param {number} [id]
     * @param {number} [rule]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    plannedActivityGetActivityDetails(
      id?: number,
      rule?: number,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = PlannedActivityApiFetchParamCreator(
        configuration
      ).plannedActivityGetActivityDetails(id, rule, options);
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
     * @param {number} [dc]
     * @param {number} [pdc]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    plannedActivityGetConfrontoHardwareTypeFromDesignComponent(
      dc?: number,
      pdc?: number,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<boolean> {
      const localVarFetchArgs = PlannedActivityApiFetchParamCreator(
        configuration
      ).plannedActivityGetConfrontoHardwareTypeFromDesignComponent(
        dc,
        pdc,
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
     * @param {number} dcId
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    plannedActivityGetCreateResourcePlannedActivity(
      dcId?: number,
      DcfId?: number,
      options?: any
    ): (
      fetch?: FetchAPI,
      basePath?: string
    ) => Promise<PlannedActivityDtoCreate> {
      const localVarFetchArgs = PlannedActivityApiFetchParamCreator(
        configuration
      ).plannedActivityGetCreateResourcePlannedActivity(dcId, DcfId, options);
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
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    plannedActivityGetCreateUpdatePlannedActivityStatus(
      options?: any
    ): (
      fetch?: FetchAPI,
      basePath?: string
    ) => Promise<UpdatePlannedActivityStatusDto> {
      const localVarFetchArgs =
        PlannedActivityApiFetchParamCreator(
          configuration
        ).plannedActivityGetCreateUpdatePlannedActivityStatus(options);
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
     * @param {number} opCoId
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    plannedActivityGetDesignComponentList(
      opCoId: number,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = PlannedActivityApiFetchParamCreator(
        configuration
      ).plannedActivityGetDesignComponentList(opCoId, options);
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
     * @param {PlannedActivityQueryDto} body
     * @param {string} [propertyName]
     * @param {string} [propertyFilter]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    plannedActivityGetFilterResult(
      body: PlannedActivityQueryDto,
      propertyName?: string,
      propertyFilter?: string,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<Array<FilterValueDto>> {
      const localVarFetchArgs = PlannedActivityApiFetchParamCreator(
        configuration
      ).plannedActivityGetFilterResult(
        body,
        propertyName,
        propertyFilter,
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
     * @param {PlannedActivityQueryDto} body
     * @param {string} [propertyName]
     * @param {string} [propertyFilter]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    plannedActivityGetArchivedFilterResult(
      body: PlannedActivityQueryDto,
      propertyName?: string,
      propertyFilter?: string,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<Array<FilterValueDto>> {
      const localVarFetchArgs = PlannedActivityApiFetchParamCreator(
        configuration
      ).plannedActivityGetArchivedFilterResult(
        body,
        propertyName,
        propertyFilter,
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
     * @param {number} [designComponentId]
     * @param {number} [opCoId]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    plannedActivityGetLcmEngineeringPlannedActivity(
      designComponentId?: number,
      opCoId?: number,
      options?: any
    ): (
      fetch?: FetchAPI,
      basePath?: string
    ) => Promise<ResultDtoOfDictionaryOfLongAndPlannedActivityToConnectData> {
      const localVarFetchArgs = PlannedActivityApiFetchParamCreator(
        configuration
      ).plannedActivityGetLcmEngineeringPlannedActivity(
        designComponentId,
        opCoId,
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
     * @param {number} [id]
     * @param {number} [rule]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    plannedActivityGetLinkedDesignComponent(
      id?: number,
      rule?: number,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = PlannedActivityApiFetchParamCreator(
        configuration
      ).plannedActivityGetLinkedDesignComponent(id, rule, options);
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
    plannedActivityCreateUnkownDCPALevel(
      id?: number,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = PlannedActivityApiFetchParamCreator(
        configuration
      ).plannedActivityCreateUnkownDCPALevel(id, options);
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
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    plannedActivityGetOpCoList(
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs =
        PlannedActivityApiFetchParamCreator(
          configuration
        ).plannedActivityGetOpCoList(options);
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
     * @param {PlannedActivityQueryDto} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    plannedActivityGetPlannedActivity(
      body: PlannedActivityQueryDto,
      options?: any
    ): (
      fetch?: FetchAPI,
      basePath?: string
    ) => Promise<QueryResultDtoOfPlannedActivityDtoGrid> {
      const localVarFetchArgs = PlannedActivityApiFetchParamCreator(
        configuration
      ).plannedActivityGetPlannedActivity(body, options);
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
     * @param {PlannedActivityConfirmationDto} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    plannedActivityConfirmation(
      body: PlannedActivityConfirmationDto,
      options: any = {}
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = PlannedActivityApiFetchParamCreator(
        configuration
      ).plannedActivityConfirmation(body, options);
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
     * @param {PaWithDaMigrationApiResponse} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    updatePaWithDaMigration(
      body: PaWithDaMigrationApiResponse,
      paId: number,
      deliveryStatusId: number,
      options: any = {}
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = PlannedActivityApiFetchParamCreator(
        configuration
      ).updatePaWithDaMigration(body, paId, deliveryStatusId, options);

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
     * @param {PaWhenDaMigrationCompleteApiResponse} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    updatePaWhenDaMigrationCompleted(
      body: PaWhenDaMigrationCompleteApiResponse,
      paId: number,
      deliveryStatusId: number,
      options: any = {}
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = PlannedActivityApiFetchParamCreator(
        configuration
      ).updatePaWhenDaMigrationCompleted(body, paId, deliveryStatusId, options);

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
     * @param {PlannedActivityQueryDto} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    plannedActivityGetArchivedPlannedActivity(
      body: PlannedActivityQueryDto,
      options?: any
    ): (
      fetch?: FetchAPI,
      basePath?: string
    ) => Promise<QueryResultDtoOfPlannedActivityDtoGrid> {
      const localVarFetchArgs = PlannedActivityApiFetchParamCreator(
        configuration
      ).plannedActivityGetArchivedPlannedActivity(body, options);
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
     * @param {number} id
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    plannedActivityGetPlannedActivityForLink(
      id: number,
      options?: any
    ): (
      fetch?: FetchAPI,
      basePath?: string
    ) => Promise<ResultDtoOfPlannedActivityForLinkDto> {
      const localVarFetchArgs = PlannedActivityApiFetchParamCreator(
        configuration
      ).plannedActivityGetPlannedActivityForLink(id, options);
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
     * @param {number} [dcId]
     * @param {number} [opcoId]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    plannedActivityGetPlannedActivityListForMigration(
      dcId?: number,
      opcoId?: number,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = PlannedActivityApiFetchParamCreator(
        configuration
      ).plannedActivityGetPlannedActivityListForMigration(
        dcId,
        opcoId,
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
     * @param {number} [dcId]
     * @param {number} [opcoId]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    plannedActivityGetPlannedActivityTypeswithDcIdAndOpcoId(
      dcId?: number,
      opcoId?: number,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = PlannedActivityApiFetchParamCreator(
        configuration
      ).plannedActivityGetPlannedActivityTypeswithDcIdAndOpcoId(
        dcId,
        opcoId,
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
    GetUpdateDaMigrationRecords(
      id: number,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = PlannedActivityApiFetchParamCreator(
        configuration
      ).GetUpdateDaMigrationRecords(id, options);
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
     * @param {number} [plannedActivityTypeId]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    plannedActivityCheckPlannedActivityTypefor(
      designComponentId?: number,
      opCoId?: number,
      plannedActivityTypeId?: number,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = PlannedActivityApiFetchParamCreator(
        configuration
      ).plannedActivityCheckPlannedActivityTypefor(
        designComponentId,
        opCoId,
        plannedActivityTypeId,
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
     * @param {number} [dcId]
     * @param {number} [opcoId]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    plannedActivityGetPlannedActivityListForUpdatePlannedActivityStatus(
      dcId?: number,
      opcoId?: number,
      plannedActivityTypeId?: number,
      plannedActivityTypeFor?: number,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = PlannedActivityApiFetchParamCreator(
        configuration
      ).plannedActivityGetPlannedActivityListForUpdatePlannedActivityStatus(
        dcId,
        opcoId,
        plannedActivityTypeId,
        plannedActivityTypeFor,
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

    plannedActivityGetSettingUpdatePlannedActivityResource(
      plannedActivityTypeId?: number,
      plannedActivityTypeFor?: number,
      options?: any
    ): (
      fetch?: FetchAPI,
      basePath?: string
    ) => Promise<{ [key: string]: SettingsUpdatePlannedActivityDtoUpdate }> {
      const localVarFetchArgs = PlannedActivityApiFetchParamCreator(
        configuration
      ).plannedActivityGetSettingUpdatePlannedActivityResource(
        plannedActivityTypeId,
        plannedActivityTypeFor,
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
     * @param {number} id
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    plannedActivityGetRelatedRecords(
      id: number,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = PlannedActivityApiFetchParamCreator(
        configuration
      ).plannedActivityGetRelatedRecords(id, options);
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
     * @param {number} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    // plannedActivityGetUpdatePlannedActivityStatus(
    //   plannedId: number,
    //   plannedAcivityTypeFor: number,
    //   options?: any
    // ): (
    //   fetch?: FetchAPI,
    //   basePath?: string
    // ) => Promise<ResultDtoOfUpdatePlannedActivityStatusDto> {
    //   const localVarFetchArgs = PlannedActivityApiFetchParamCreator(
    //     configuration
    //   ).plannedActivityGetUpdatePlannedActivityStatus(
    //     plannedId,
    //     plannedAcivityTypeFor,
    //     options
    //   );
    //   return (
    //     fetch: FetchAPI = isomorphicFetch,
    //     basePath = BASE_PATH
    //   ) => {
    //     return fetch(
    //       basePath + localVarFetchArgs.url,
    //       localVarFetchArgs.options
    //     ).then((response) => {
    //       if (response.status >= 200 && response.status < 300) {
    //         return response.json();
    //       } else {
    //         throw response;
    //       }
    //     });
    //   };
    // },

    plannedActivityGetUpdatePlannedActivityStatus(
      plannedId: number,
      plannedActivityTypeFor: number,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = PlannedActivityApiFetchParamCreator(
        configuration
      ).plannedActivityGetUpdatePlannedActivityStatus(
        plannedId,
        plannedActivityTypeFor,
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
     * @param {number} id
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    plannedActivityGetUpdateResourcePlannedActivity(
      id: number,
      options?: any
    ): (
      fetch?: FetchAPI,
      basePath?: string
    ) => Promise<PlannedActivityDtoUpdate> {
      const localVarFetchArgs = PlannedActivityApiFetchParamCreator(
        configuration
      ).plannedActivityGetUpdateResourcePlannedActivity(id, options);
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
     * @param {PlannedActivityMigrationsDto} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    plannedActivityPlannedActivityMigrations(
      body: PlannedActivityMigrationsDto,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = PlannedActivityApiFetchParamCreator(
        configuration
      ).plannedActivityPlannedActivityMigrations(body, options);
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
     * @param {PlannedActivityDtoUpdate} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    plannedActivityPut(
      body: PlannedActivityDtoUpdate,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = PlannedActivityApiFetchParamCreator(
        configuration
      ).plannedActivityPut(body, options);
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
     * @param {UpdatePlannedActivityStatusDto} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    plannedActivitySaveUpdatePlannedActivityStatus(
      body: UpdatePlannedActivityStatusDto,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs = PlannedActivityApiFetchParamCreator(
        configuration
      ).plannedActivitySaveUpdatePlannedActivityStatus(body, options);
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
 * PlannedActivityApi - factory interface
 * @export
 */
export const PlannedActivityApiFactory = function (
  configuration?: Configuration,
  fetch?: FetchAPI,
  basePath?: string
) {
  return {
    /**
     *
     * @param {number} [deliveryId]
     * @param {number} [budgetAvId]
     * @param {number} [responsibilityPhase]
     * @param {boolean} [localApproval]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    plannedActivityActivityStatusLogics(
      deliveryId?: number,
      budgetAvId?: number,
      responsibilityPhase?: number,
      localApproval?: boolean,
      options?: any
    ) {
      return PlannedActivityApiFp(
        configuration
      ).plannedActivityActivityStatusLogics(
        deliveryId,
        budgetAvId,
        responsibilityPhase,
        localApproval,
        options
      )(fetch, basePath);
    },
    /**
     *
     * @param {number} [year]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    plannedActivityCheckFiscalYear(year?: number, options?: any) {
      return PlannedActivityApiFp(configuration).plannedActivityCheckFiscalYear(
        year,
        options
      )(fetch, basePath);
    },

    getPlannedActivityRelatedDeliveryStatus(
      plannedActivityId?: number,
      plannedActivityTypeFor?: PlannedActivityTypeForEnum,
      options?: any
    ) {
      return PlannedActivityApiFp(
        configuration
      ).getPlannedActivityRelatedDeliveryStatus(
        plannedActivityId,
        plannedActivityTypeFor,
        options
      )(fetch, basePath);
    },
    /**
     * @param {any} [pagewiseId]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    GetPlannedRuleConfig(pagewiseId?: any, options?: any) {
      return PlannedActivityApiFp(configuration).GetPlannedRuleConfig(
        pagewiseId,
        options
      )(fetch, basePath);
    },
    getLcmDeploymentStatusRelatedDeliveryStatusAndPAResource(
      plannedActivityResourceId?: number,
      deliveryStatusId?: number,
      options?: any
    ) {
      return PlannedActivityApiFp(
        configuration
      ).getLcmDeploymentStatusRelatedDeliveryStatusAndPAResource(
        plannedActivityResourceId,
        deliveryStatusId,
        options
      )(fetch, basePath);
    },
    /**
     *
     * @param {PlannedActivityDtoCreate} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    plannedActivityCreate(body: PlannedActivityDtoCreate, options?: any) {
      return PlannedActivityApiFp(configuration).plannedActivityCreate(
        body,
        options
      )(fetch, basePath);
    },
    /**
     *
     * @param {any} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    saveUpdatedAssetDetails(body: any, options?: any) {
      return PlannedActivityApiFp(configuration).saveUpdatedAssetDetails(
        body,
        options
      )(fetch, basePath);
    },
    /**
     *
     * @param {number} [id]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    plannedActivityDelete(id?: number, options?: any) {
      return PlannedActivityApiFp(configuration).plannedActivityDelete(
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
    getLocationTypeById(locationId?: number, options?: any) {
      return PlannedActivityApiFp(configuration).getLocationTypeById(
        locationId,
        options
      )(fetch, basePath);
    },
    /**
     *
     * @param {number} [id]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    plannedActivityDeleteDeep(id?: number, options?: any) {
      return PlannedActivityApiFp(configuration).plannedActivityDeleteDeep(
        id,
        options
      )(fetch, basePath);
    },
    /**
     *
     * @param {PlannedActivityQueryDto} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    plannedActivityExportReport(body: PlannedActivityQueryDto, options?: any) {
      return PlannedActivityApiFp(configuration).plannedActivityExportReport(
        body,
        options
      )(fetch, basePath);
    },

    /**
     *
     * @param {PlannedActivityQueryDto} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    plannedActivityArchivedExportReport(
      body: PlannedActivityQueryDto,
      options?: any
    ) {
      return PlannedActivityApiFp(
        configuration
      ).plannedActivityArchivedExportReport(body, options)(fetch, basePath);
    },

    /**
     *
     * @param {number} [id]
     * @param {number} [rule]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    plannedActivityGetActivityDetails(
      id?: number,
      rule?: number,
      options?: any
    ) {
      return PlannedActivityApiFp(
        configuration
      ).plannedActivityGetActivityDetails(
        id,
        rule,
        options
      )(fetch, basePath);
    },
    /**
     *
     * @param {number} [dc]
     * @param {number} [pdc]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    plannedActivityGetConfrontoHardwareTypeFromDesignComponent(
      dc?: number,
      pdc?: number,
      options?: any
    ) {
      return PlannedActivityApiFp(
        configuration
      ).plannedActivityGetConfrontoHardwareTypeFromDesignComponent(
        dc,
        pdc,
        options
      )(fetch, basePath);
    },
    /**
     *
     * @param {number} dcId
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    plannedActivityGetCreateResourcePlannedActivity(
      dcId?: number,
      DcfId?: number,
      options?: any
    ) {
      return PlannedActivityApiFp(
        configuration
      ).plannedActivityGetCreateResourcePlannedActivity(
        dcId,
        DcfId,
        options
      )(fetch, basePath);
    },
    /**
     *
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    plannedActivityGetCreateUpdatePlannedActivityStatus(options?: any) {
      return PlannedActivityApiFp(
        configuration
      ).plannedActivityGetCreateUpdatePlannedActivityStatus(options)(
        fetch,
        basePath
      );
    },
    /**
     *
     * @param {number} opCoId
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    plannedActivityGetDesignComponentList(opCoId: number, options?: any) {
      return PlannedActivityApiFp(
        configuration
      ).plannedActivityGetDesignComponentList(opCoId, options)(fetch, basePath);
    },
    /**
     *
     * @param {PlannedActivityQueryDto} body
     * @param {string} [propertyName]
     * @param {string} [propertyFilter]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    plannedActivityGetFilterResult(
      body: PlannedActivityQueryDto,
      propertyName?: string,
      propertyFilter?: string,
      options?: any
    ) {
      return PlannedActivityApiFp(configuration).plannedActivityGetFilterResult(
        body,
        propertyName,
        propertyFilter,
        options
      )(fetch, basePath);
    },

    /**
     *
     * @param {PlannedActivityQueryDto} body
     * @param {string} [propertyName]
     * @param {string} [propertyFilter]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    plannedActivityGetArchivedFilterResult(
      body: PlannedActivityQueryDto,
      propertyName?: string,
      propertyFilter?: string,
      options?: any
    ) {
      return PlannedActivityApiFp(
        configuration
      ).plannedActivityGetArchivedFilterResult(
        body,
        propertyName,
        propertyFilter,
        options
      )(fetch, basePath);
    },
    /**
     *
     * @param {number} [designComponentId]
     * @param {number} [opCoId]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    plannedActivityGetLcmEngineeringPlannedActivity(
      designComponentId?: number,
      opCoId?: number,
      options?: any
    ) {
      return PlannedActivityApiFp(
        configuration
      ).plannedActivityGetLcmEngineeringPlannedActivity(
        designComponentId,
        opCoId,
        options
      )(fetch, basePath);
    },
    /**
     *
     * @param {number} [id]
     * @param {number} [rule]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    plannedActivityGetLinkedDesignComponent(
      id?: number,
      rule?: number,
      options?: any
    ) {
      return PlannedActivityApiFp(
        configuration
      ).plannedActivityGetLinkedDesignComponent(
        id,
        rule,
        options
      )(fetch, basePath);
    },
    /**
     *
     * @param {number} [id]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    plannedActivityCreateUnkownDCPALevel(
      id?: number,
      rule?: number,
      options?: any
    ) {
      return PlannedActivityApiFp(
        configuration
      ).plannedActivityCreateUnkownDCPALevel(id, options)(fetch, basePath);
    },
    /**
     *
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    plannedActivityGetOpCoList(options?: any) {
      return PlannedActivityApiFp(configuration).plannedActivityGetOpCoList(
        options
      )(fetch, basePath);
    },
    /**
     *
     * @param {PlannedActivityQueryDto} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    plannedActivityGetPlannedActivity(
      body: PlannedActivityQueryDto,
      options?: any
    ) {
      return PlannedActivityApiFp(
        configuration
      ).plannedActivityGetPlannedActivity(body, options)(fetch, basePath);
    },
    /**
     *
     * @param {PlannedActivityConfirmationDto} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    plannedActivityConfirmation(
      body: PlannedActivityConfirmationDto,
      options: any = {}
    ) {
      return PlannedActivityApiFp(configuration).plannedActivityConfirmation(
        body,
        options
      )(fetch, basePath);
    },
    /**
     *
     * @param {PaWithDaMigrationApiResponse} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    updatePaWithDaMigration(
      body: PaWithDaMigrationApiResponse,
      paId: number,
      deliveryStatusId: number,
      options: any = {}
    ) {
      return PlannedActivityApiFp(configuration).updatePaWithDaMigration(
        body,
        paId,
        deliveryStatusId,
        options
      )(fetch, basePath);
    },
    /**
     *
     * @param {PaWhenDaMigrationCompleteApiResponse} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    updatePaWhenDaMigrationCompleted(
      body: PaWhenDaMigrationCompleteApiResponse,
      paId: number,
      deliveryStatusId: number,
      options: any = {}
    ) {
      return PlannedActivityApiFp(
        configuration
      ).updatePaWhenDaMigrationCompleted(
        body,
        paId,
        deliveryStatusId,
        options
      )(fetch, basePath);
    },
    /**
     *
     * @param {PlannedActivityQueryDto} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    plannedActivityGetArchivedPlannedActivity(
      body: PlannedActivityQueryDto,
      options?: any
    ) {
      return PlannedActivityApiFp(
        configuration
      ).plannedActivityGetArchivedPlannedActivity(body, options)(
        fetch,
        basePath
      );
    },
    /**
     *
     * @param {number} id
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    plannedActivityGetPlannedActivityForLink(id: number, options?: any) {
      return PlannedActivityApiFp(
        configuration
      ).plannedActivityGetPlannedActivityForLink(id, options)(fetch, basePath);
    },
    /**
     *
     * @param {number} [dcId]
     * @param {number} [opcoId]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    plannedActivityGetPlannedActivityListForMigration(
      dcId?: number,
      opcoId?: number,
      options?: any
    ) {
      return PlannedActivityApiFp(
        configuration
      ).plannedActivityGetPlannedActivityListForMigration(
        dcId,
        opcoId,
        options
      )(fetch, basePath);
    },

    /**
     *
     * @param {number} [dcId]
     * @param {number} [opcoId]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    plannedActivityGetPlannedActivityTypeswithDcIdAndOpcoId(
      dcId?: number,
      opcoId?: number,
      options?: any
    ) {
      return PlannedActivityApiFp(
        configuration
      ).plannedActivityGetPlannedActivityTypeswithDcIdAndOpcoId(
        dcId,
        opcoId,
        options
      )(fetch, basePath);
    },
    GetUpdateDaMigrationRecords(id: number, options?: any) {
      return PlannedActivityApiFp(configuration).GetUpdateDaMigrationRecords(
        id,
        options
      )(fetch, basePath);
    },
    /**
     *
     * @param {number} [dcId]
     * @param {number} [opcoId]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    plannedActivityCheckPlannedActivityTypefor(
      designComponentId?: number,
      opCoId?: number,
      plannedActivityTypeId?: number,
      options?: any
    ) {
      return PlannedActivityApiFp(
        configuration
      ).plannedActivityCheckPlannedActivityTypefor(
        designComponentId,
        opCoId,
        plannedActivityTypeId,
        options
      )(fetch, basePath);
    },
    /**
     *
     * @param {number} [dcId]
     * @param {number} [opcoId]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    plannedActivityGetPlannedActivityListForUpdatePlannedActivityStatus(
      dcId?: number,
      opcoId?: number,
      plannedActivityTypeId?: number,
      plannedActivityTypeFor?: number,
      options?: any
    ) {
      return PlannedActivityApiFp(
        configuration
      ).plannedActivityGetPlannedActivityListForUpdatePlannedActivityStatus(
        dcId,
        opcoId,
        plannedActivityTypeId,
        plannedActivityTypeFor,
        options
      )(fetch, basePath);
    },

    plannedActivityGetSettingUpdatePlannedActivityResource(
      plannedActivityTypeId?: number,
      plannedActivityTypeFor?: number,
      options?: any
    ) {
      return PlannedActivityApiFp(
        configuration
      ).plannedActivityGetSettingUpdatePlannedActivityResource(
        plannedActivityTypeId,
        plannedActivityTypeFor,
        options
      )(fetch, basePath);
    },
    /**
     *
     * @param {number} id
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    plannedActivityGetRelatedRecords(id: number, options?: any) {
      return PlannedActivityApiFp(
        configuration
      ).plannedActivityGetRelatedRecords(id, options)(fetch, basePath);
    },
    /**
     *
     * @param {number} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    plannedActivityGetUpdatePlannedActivityStatus(
      plannedId: number,
      plannedActivityTypeFor: number,
      options?: any
    ) {
      return PlannedActivityApiFp(
        configuration
      ).plannedActivityGetUpdatePlannedActivityStatus(
        plannedId,
        plannedActivityTypeFor,
        options
      )(fetch, basePath);
    },

    /**
     *
     * @param {number} id
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    plannedActivityGetUpdateResourcePlannedActivity(id: number, options?: any) {
      return PlannedActivityApiFp(
        configuration
      ).plannedActivityGetUpdateResourcePlannedActivity(id, options)(
        fetch,
        basePath
      );
    },
    /**
     *
     * @param {PlannedActivityMigrationsDto} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    plannedActivityPlannedActivityMigrations(
      body: PlannedActivityMigrationsDto,
      options?: any
    ) {
      return PlannedActivityApiFp(
        configuration
      ).plannedActivityPlannedActivityMigrations(body, options)(
        fetch,
        basePath
      );
    },
    /**
     *
     * @param {PlannedActivityDtoUpdate} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    plannedActivityPut(body: PlannedActivityDtoUpdate, options?: any) {
      return PlannedActivityApiFp(configuration).plannedActivityPut(
        body,
        options
      )(fetch, basePath);
    },
    /**
     *
     * @param {UpdatePlannedActivityStatusDto} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    plannedActivitySaveUpdatePlannedActivityStatus(
      body: UpdatePlannedActivityStatusDto,
      options?: any
    ) {
      return PlannedActivityApiFp(
        configuration
      ).plannedActivitySaveUpdatePlannedActivityStatus(body, options)(
        fetch,
        basePath
      );
    },
  };
};

/**
 * PlannedActivityApi - object-oriented interface
 * @export
 * @class PlannedActivityApi
 * @extends {BaseAPI}
 */
export class PlannedActivityApi extends BaseAPI {
  /**
   *
   * @param {number} [deliveryId]
   * @param {number} [budgetAvId]
   * @param {number} [responsibilityPhase]
   * @param {boolean} [localApproval]
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof PlannedActivityApi
   */
  public plannedActivityActivityStatusLogics(
    deliveryId?: number,
    budgetAvId?: number,
    responsibilityPhase?: number,
    localApproval?: boolean,
    options?: any
  ) {
    return PlannedActivityApiFp(
      this.configuration
    ).plannedActivityActivityStatusLogics(
      deliveryId,
      budgetAvId,
      responsibilityPhase,
      localApproval,
      options
    )(this.fetch, this.basePath);
  }

  /**
   *
   * @param {number} [year]
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof PlannedActivityApi
   */
  public plannedActivityCheckFiscalYear(year?: number, options?: any) {
    return PlannedActivityApiFp(
      this.configuration
    ).plannedActivityCheckFiscalYear(year, options)(this.fetch, this.basePath);
  }

  public getPlannedActivityRelatedDeliveryStatus(
    plannedActivityId?: number,
    plannedActivityTypeFor?: PlannedActivityTypeForEnum,
    options?: any
  ) {
    return PlannedActivityApiFp(
      this.configuration
    ).getPlannedActivityRelatedDeliveryStatus(
      plannedActivityId,
      plannedActivityTypeFor,
      options
    )(this.fetch, this.basePath);
  }

  /**
   * @param {any} [pagewiseId]
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof PlannedActivityApi
   */
  public GetPlannedRuleConfig(pagewiseId?: any, options?: any) {
    return PlannedActivityApiFp(this.configuration).GetPlannedRuleConfig(
      pagewiseId,
      options
    )(this.fetch, this.basePath);
  }

  public getLcmDeploymentStatusRelatedDeliveryStatusAndPAResource(
    plannedActivityResourceId?: number,
    deliveryStatusId?: number,
    options?: any
  ) {
    return PlannedActivityApiFp(
      this.configuration
    ).getLcmDeploymentStatusRelatedDeliveryStatusAndPAResource(
      plannedActivityResourceId,
      deliveryStatusId,
      options
    )(this.fetch, this.basePath);
  }

  /**
   *
   * @param {PlannedActivityDtoCreate} body
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof PlannedActivityApi
   */
  public plannedActivityCreate(body: PlannedActivityDtoCreate, options?: any) {
    return PlannedActivityApiFp(this.configuration).plannedActivityCreate(
      body,
      options
    )(this.fetch, this.basePath);
  }
  /**
   *
   * @param {any} body
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof PlannedActivityApi
   */
  public saveUpdatedAssetDetails(body: any, options?: any) {
    return PlannedActivityApiFp(this.configuration).saveUpdatedAssetDetails(
      body,
      options
    )(this.fetch, this.basePath);
  }

  /**
   *
   * @param {number} [id]
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof PlannedActivityApi
   */
  public plannedActivityDelete(id?: number, options?: any) {
    return PlannedActivityApiFp(this.configuration).plannedActivityDelete(
      id,
      options
    )(this.fetch, this.basePath);
  }

  /**
   *
   * @param {number} [id]
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof PlannedActivityApi
   */
  public getLocationTypeById(locationId?: number, options?: any) {
    return PlannedActivityApiFp(this.configuration).getLocationTypeById(
      locationId,
      options
    )(this.fetch, this.basePath);
  }

  /**
   *
   * @param {number} [id]
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof PlannedActivityApi
   */
  public plannedActivityDeleteDeep(id?: number, options?: any) {
    return PlannedActivityApiFp(this.configuration).plannedActivityDeleteDeep(
      id,
      options
    )(this.fetch, this.basePath);
  }

  /**
   *
   * @param {PlannedActivityQueryDto} body
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof PlannedActivityApi
   */
  public plannedActivityExportReport(
    body: PlannedActivityQueryDto,
    options?: any
  ) {
    return PlannedActivityApiFp(this.configuration).plannedActivityExportReport(
      body,
      options
    )(this.fetch, this.basePath);
  }

  /**
   *
   * @param {PlannedActivityQueryDto} body
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof PlannedActivityApi
   */
  public plannedActivityArchivedExportReport(
    body: PlannedActivityQueryDto,
    options?: any
  ) {
    return PlannedActivityApiFp(
      this.configuration
    ).plannedActivityArchivedExportReport(body, options)(
      this.fetch,
      this.basePath
    );
  }

  /**
   *
   * @param {number} [id]
   * @param {number} [rule]
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof PlannedActivityApi
   */
  public plannedActivityGetActivityDetails(
    id?: number,
    rule?: number,
    options?: any
  ) {
    return PlannedActivityApiFp(
      this.configuration
    ).plannedActivityGetActivityDetails(
      id,
      rule,
      options
    )(this.fetch, this.basePath);
  }

  /**
   *
   * @param {number} [dc]
   * @param {number} [pdc]
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof PlannedActivityApi
   */
  public plannedActivityGetConfrontoHardwareTypeFromDesignComponent(
    dc?: number,
    pdc?: number,
    options?: any
  ) {
    return PlannedActivityApiFp(
      this.configuration
    ).plannedActivityGetConfrontoHardwareTypeFromDesignComponent(
      dc,
      pdc,
      options
    )(this.fetch, this.basePath);
  }

  /**
   *
   * @param {number} dcId
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof PlannedActivityApi
   */
  public plannedActivityGetCreateResourcePlannedActivity(
    dcId?: number,
    DcfId?: number,
    options?: any
  ) {
    return PlannedActivityApiFp(
      this.configuration
    ).plannedActivityGetCreateResourcePlannedActivity(
      dcId,
      DcfId,
      options
    )(this.fetch, this.basePath);
  }

  /**
   *
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof PlannedActivityApi
   */
  public plannedActivityGetCreateUpdatePlannedActivityStatus(options?: any) {
    return PlannedActivityApiFp(
      this.configuration
    ).plannedActivityGetCreateUpdatePlannedActivityStatus(options)(
      this.fetch,
      this.basePath
    );
  }

  /**
   *
   * @param {number} opCoId
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof PlannedActivityApi
   */
  public plannedActivityGetDesignComponentList(opCoId: number, options?: any) {
    return PlannedActivityApiFp(
      this.configuration
    ).plannedActivityGetDesignComponentList(opCoId, options)(
      this.fetch,
      this.basePath
    );
  }

  /**
   *
   * @param {PlannedActivityQueryDto} body
   * @param {string} [propertyName]
   * @param {string} [propertyFilter]
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof PlannedActivityApi
   */
  public plannedActivityGetFilterResult(
    body: PlannedActivityQueryDto,
    propertyName?: string,
    propertyFilter?: string,
    options?: any
  ) {
    return PlannedActivityApiFp(
      this.configuration
    ).plannedActivityGetFilterResult(
      body,
      propertyName,
      propertyFilter,
      options
    )(this.fetch, this.basePath);
  }

  /**
   *
   * @param {PlannedActivityQueryDto} body
   * @param {string} [propertyName]
   * @param {string} [propertyFilter]
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof PlannedActivityApi
   */
  public plannedActivityGetArchivedFilterResult(
    body: PlannedActivityQueryDto,
    propertyName?: string,
    propertyFilter?: string,
    options?: any
  ) {
    return PlannedActivityApiFp(
      this.configuration
    ).plannedActivityGetArchivedFilterResult(
      body,
      propertyName,
      propertyFilter,
      options
    )(this.fetch, this.basePath);
  }

  /**
   *
   * @param {number} [designComponentId]
   * @param {number} [opCoId]
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof PlannedActivityApi
   */
  public plannedActivityGetLcmEngineeringPlannedActivity(
    designComponentId?: number,
    opCoId?: number,
    options?: any
  ) {
    return PlannedActivityApiFp(
      this.configuration
    ).plannedActivityGetLcmEngineeringPlannedActivity(
      designComponentId,
      opCoId,
      options
    )(this.fetch, this.basePath);
  }

  /**
   *
   * @param {number} [id]
   * @param {number} [rule]
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof PlannedActivityApi
   */
  public plannedActivityGetLinkedDesignComponent(
    id?: number,
    rule?: number,
    options?: any
  ) {
    return PlannedActivityApiFp(
      this.configuration
    ).plannedActivityGetLinkedDesignComponent(
      id,
      rule,
      options
    )(this.fetch, this.basePath);
  }

  /**
   *
   * @param {number} [id]
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof PlannedActivityApi
   */
  public plannedActivityCreateUnkownDCPALevel(id?: number, options?: any) {
    return PlannedActivityApiFp(
      this.configuration
    ).plannedActivityCreateUnkownDCPALevel(id, options)(
      this.fetch,
      this.basePath
    );
  }

  /**
   *
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof PlannedActivityApi
   */
  public plannedActivityGetOpCoList(options?: any) {
    return PlannedActivityApiFp(this.configuration).plannedActivityGetOpCoList(
      options
    )(this.fetch, this.basePath);
  }

  /**
   *
   * @param {PlannedActivityQueryDto} body
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof PlannedActivityApi
   */
  public plannedActivityGetPlannedActivity(
    body: PlannedActivityQueryDto,
    options?: any
  ) {
    return PlannedActivityApiFp(
      this.configuration
    ).plannedActivityGetPlannedActivity(body, options)(
      this.fetch,
      this.basePath
    );
  }

  public plannedActivityConfirmation(
    body: PlannedActivityConfirmationDto,
    options: any = {}
  ) {
    return PlannedActivityApiFp(this.configuration).plannedActivityConfirmation(
      body,
      options
    )(this.fetch, this.basePath);
  }

  public updatePaWithDaMigration(
    body: PaWithDaMigrationApiResponse,
    paId: number,
    deliveryStatusId: number,
    options: any = {}
  ) {
    return PlannedActivityApiFp(this.configuration).updatePaWithDaMigration(
      body,
      paId,
      deliveryStatusId,
      options
    )(this.fetch, this.basePath);
  }

  public updatePaWhenDaMigrationCompleted(
    body: PaWhenDaMigrationCompleteApiResponse,
    paId: number,
    deliveryStatusId: number,
    options: any = {}
  ) {
    return PlannedActivityApiFp(
      this.configuration
    ).updatePaWhenDaMigrationCompleted(
      body,
      paId,
      deliveryStatusId,
      options
    )(this.fetch, this.basePath);
  }

  /**
   *
   * @param {PlannedActivityQueryDto} body
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof PlannedActivityApi
   */
  public plannedActivityGetArchivedPlannedActivity(
    body: PlannedActivityQueryDto,
    options?: any
  ) {
    return PlannedActivityApiFp(
      this.configuration
    ).plannedActivityGetArchivedPlannedActivity(body, options)(
      this.fetch,
      this.basePath
    );
  }

  /**
   *
   * @param {number} id
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof PlannedActivityApi
   */
  public plannedActivityGetPlannedActivityForLink(id: number, options?: any) {
    return PlannedActivityApiFp(
      this.configuration
    ).plannedActivityGetPlannedActivityForLink(id, options)(
      this.fetch,
      this.basePath
    );
  }

  /**
   *
   * @param {number} [dcId]
   * @param {number} [opcoId]
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof PlannedActivityApi
   */
  public plannedActivityGetPlannedActivityListForMigration(
    dcId?: number,
    opcoId?: number,
    options?: any
  ) {
    return PlannedActivityApiFp(
      this.configuration
    ).plannedActivityGetPlannedActivityListForMigration(
      dcId,
      opcoId,
      options
    )(this.fetch, this.basePath);
  }

  /**
   *
   * @param {number} [dcId]
   * @param {number} [opcoId]
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof PlannedActivityApi
   */
  public plannedActivityGetPlannedActivityTypeswithDcIdAndOpcoId(
    dcId?: number,
    opcoId?: number,
    options?: any
  ) {
    return PlannedActivityApiFp(
      this.configuration
    ).plannedActivityGetPlannedActivityTypeswithDcIdAndOpcoId(
      dcId,
      opcoId,
      options
    )(this.fetch, this.basePath);
  }
  public GetUpdateDaMigrationRecords(id: number, options?: any) {
    return PlannedActivityApiFp(this.configuration).GetUpdateDaMigrationRecords(
      id,
      options
    )(this.fetch, this.basePath);
  }
  /**
   *
   * @param {number} [dcId]
   * @param {number} [opcoId]
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof PlannedActivityApi
   */
  public plannedActivityCheckPlannedActivityTypefor(
    designComponentId?: number,
    opCoId?: number,
    plannedActivityTypeId?: number,
    options?: any
  ) {
    return PlannedActivityApiFp(
      this.configuration
    ).plannedActivityCheckPlannedActivityTypefor(
      designComponentId,
      opCoId,
      plannedActivityTypeId,
      options
    )(this.fetch, this.basePath);
  }

  /**
   *
   * @param {number} [dcId]
   * @param {number} [opcoId]
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof PlannedActivityApi
   */
  public plannedActivityGetPlannedActivityListForUpdatePlannedActivityStatus(
    dcId?: number,
    opcoId?: number,
    plannedActivityTypeId?: number,
    plannedActivityTypeFor?: number,
    options?: any
  ) {
    return PlannedActivityApiFp(
      this.configuration
    ).plannedActivityGetPlannedActivityListForUpdatePlannedActivityStatus(
      dcId,
      opcoId,
      plannedActivityTypeId,
      plannedActivityTypeFor,
      options
    )(this.fetch, this.basePath);
  }

  public plannedActivityGetSettingUpdatePlannedActivityResource(
    plannedActivityTypeId?: number,
    plannedActivityTypeFor?: number,
    options?: any
  ) {
    return PlannedActivityApiFp(
      this.configuration
    ).plannedActivityGetSettingUpdatePlannedActivityResource(
      plannedActivityTypeId,
      plannedActivityTypeFor,
      options
    )(this.fetch, this.basePath);
  }

  /**
   *
   * @param {number} id
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof PlannedActivityApi
   */
  public plannedActivityGetRelatedRecords(id: number, options?: any) {
    return PlannedActivityApiFp(
      this.configuration
    ).plannedActivityGetRelatedRecords(id, options)(this.fetch, this.basePath);
  }

  /**
   *
   * @param {number} body
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof PlannedActivityApi
   */
  public plannedActivityGetUpdatePlannedActivityStatus(
    plannedId: number,
    plannedAcivityTypeFor: number,
    options?: any
  ) {
    return PlannedActivityApiFp(
      this.configuration
    ).plannedActivityGetUpdatePlannedActivityStatus(
      plannedId,
      plannedAcivityTypeFor,
      options
    )(this.fetch, this.basePath);
  }

  /**
   *
   * @param {number} id
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof PlannedActivityApi
   */
  public plannedActivityGetUpdateResourcePlannedActivity(
    id: number,
    options?: any
  ) {
    return PlannedActivityApiFp(
      this.configuration
    ).plannedActivityGetUpdateResourcePlannedActivity(id, options)(
      this.fetch,
      this.basePath
    );
  }

  /**
   *
   * @param {PlannedActivityMigrationsDto} body
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof PlannedActivityApi
   */
  public plannedActivityPlannedActivityMigrations(
    body: PlannedActivityMigrationsDto,
    options?: any
  ) {
    return PlannedActivityApiFp(
      this.configuration
    ).plannedActivityPlannedActivityMigrations(body, options)(
      this.fetch,
      this.basePath
    );
  }

  /**
   *
   * @param {PlannedActivityDtoUpdate} body
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof PlannedActivityApi
   */
  public plannedActivityPut(body: PlannedActivityDtoUpdate, options?: any) {
    return PlannedActivityApiFp(this.configuration).plannedActivityPut(
      body,
      options
    )(this.fetch, this.basePath);
  }

  /**
   *
   * @param {UpdatePlannedActivityStatusDto} body
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof PlannedActivityApi
   */
  public plannedActivitySaveUpdatePlannedActivityStatus(
    body: UpdatePlannedActivityStatusDto,
    options?: any
  ) {
    return PlannedActivityApiFp(
      this.configuration
    ).plannedActivitySaveUpdatePlannedActivityStatus(body, options)(
      this.fetch,
      this.basePath
    );
  }
}
