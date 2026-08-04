import * as url from "url";
import * as isomorphicFetch from "isomorphic-fetch";
import { Configuration } from "./Common/configuration";
import { ChangeGridOrderDto, ResultDto } from "../Model/CommonModels";
import {
  BASE_PATH,
  FetchArgs,
  FetchAPI,
  RequiredError,
  FilterValueDto,
} from "./Common/CommonBusiness";
import { BaseAPI } from "./Common/CommonBusiness";
import { headerObj } from "./header";

import {
  QueryResultDtoOfSettingsUpdatePlannedActivityDtoGrid,
  SettingsUpdatePlannedActivityDtoCreate,
  SettingsUpdatePlannedActivityDtoUpdate,
  SettingsUpdatePlannedActivityQueryObjectGrid,
} from "../Model/SettingsUpdatePlannedActivity";
import { ReturnFile } from "../Model/Common";
/**
 * SettingsUpdatePlannedActivityApi - fetch parameter creator
 * @export
 */
export const SettingsUpdatePlannedActivityApiFetchParamCreator = function (
  configuration?: Configuration
) {
  return {
    /**
     *
     * @param {Array<ChangeGridOrderDto>} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    settingsUpdatePlannedActivityChangeGridOrderSettingsUpdatePlannedActivity(
      body: Array<ChangeGridOrderDto>,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling settingsUpdatePlannedActivityChangeGridOrderSettingsUpdatePlannedActivity."
        );
      }
      const localVarPath = `/api/SettingsUpdatePlannedActivity/ChangeGridOrderSettingsUpdatePlannedActivity`;
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
        <any>"Array&lt;ChangeGridOrderDto&gt;" !== "string" ||
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
     * @param {MajorHardwareBuildQueryDto} body
     * @param {string} [propertyName]
     * @param {string} [propertyFilter]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    settingsUpdatePlannedActivityGetFilterResult(
      body: SettingsUpdatePlannedActivityQueryObjectGrid,
      propertyName?: string,
      propertyFilter?: string,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling settingsUpdatePlannedActivityGetFilterResult."
        );
      }
      const localVarPath = `/api/SettingsUpdatePlannedActivity/Filter`;
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
        <any>"MajorHardwareBuildQueryDto" !== "string" ||
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
     * @param {SettingsUpdatePlannedActivityDtoCreate} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    settingsUpdatePlannedActivityCreate(
      body: SettingsUpdatePlannedActivityDtoCreate,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling settingsUpdatePlannedActivityCreate."
        );
      }
      const localVarPath = `/api/SettingsUpdatePlannedActivity`;
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
        <any>"SettingsUpdatePlannedActivityDtoCreate" !== "string" ||
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
    settingsUpdatePlannedActivityDelete(
      id?: number,
      options: any = {}
    ): FetchArgs {
      const localVarPath = `/api/SettingsUpdatePlannedActivity/Delete`;
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
    settingsUpdatePlannedActivityDeleteDeep(
      id?: number,
      options: any = {}
    ): FetchArgs {
      const localVarPath = `/api/SettingsUpdatePlannedActivity/DeleteDeep`;
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
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    settingsUpdatePlannedActivityGetCreateResourceSettingsUpdatePlannedActivity(
      options: any = {}
    ): FetchArgs {
      const localVarPath = `/api/SettingsUpdatePlannedActivity/Create`;
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
    settingsUpdatePlannedActivityGetPATypeAndDeploymentStatus(
      plannedActivityTypeFor?: number,
      options: any = {}
    ): FetchArgs {
      const localVarPath = `/api/SettingsUpdatePlannedActivity/GetPATypeAndDeploymentStatus`;
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
     *
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    settingsUpdatePlannedActivityGetCrossSettings(
      plannedActivityTypeFor?: number,
      plannedActivityTypeId?: number,
      currentDeliveryStatus?: number,
      options: any = {}
    ): FetchArgs {
      const localVarPath = `/api/SettingsUpdatePlannedActivity/GetCrossSettings`;
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

      if (
        plannedActivityTypeFor !== undefined &&
        plannedActivityTypeFor !== null
      ) {
        localVarQueryParameter["plannedActivityTypeFor"] =
          plannedActivityTypeFor;
      }

      if (
        plannedActivityTypeId !== undefined &&
        plannedActivityTypeId !== null
      ) {
        localVarQueryParameter["plannedActivityTypeId"] = plannedActivityTypeId;
      }

      if (
        currentDeliveryStatus !== undefined &&
        currentDeliveryStatus !== null
      ) {
        localVarQueryParameter["currentDeliveryStatus"] = currentDeliveryStatus;
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
    settingsUpdatePlannedActivityGetRelatedRecords(
      id: number,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'id' is not null or undefined
      if (id === null || id === undefined) {
        throw new RequiredError(
          "id",
          "Required parameter id was null or undefined when calling settingsUpdatePlannedActivityGetRelatedRecords."
        );
      }
      const localVarPath =
        `/api/SettingsUpdatePlannedActivity/GetRelatedRecords{id}`.replace(
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
     * @param {Array<number>} [planningActivityStatus]
     * @param {Array<string>} [budgetAvailability]
     * @param {Array<string>} [localApproval]
     * @param {Array<number>} [deliveryStatusId]
     * @param {Array<string>} [lastModifiedBy]
     * @param {Array<string>} [crossSetting]
     * @param {string} [sortBy]
     * @param {boolean} [isSortAscending]
     * @param {number} [page]
     * @param {number} [pageSize]
     * @param {Date} [lastModifiedStartDate]
     * @param {Date} [lastModifiedEndDate]
     * @param {number} [principalId]
     * @param {boolean} [deleted]
     * @param {boolean} [orphan]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    settingsUpdatePlannedActivityGetSettingsUpdatePlannedActivity(
      ruleElementCount?: Array<string>,
      plannedActivityTypeFor?: Array<number>,
      lcmDeploymentStatus?: Array<string>,
      planningActivityResource?: Array<string>,
      plannedActivityTypeDescription?: Array<string>,
      successorPlannedActivityTypeResource?: Array<string>,
      ruleforSuccessorPlannedActivityCreation?: Array<boolean>,
      rule?: Array<number>,
      maxOrder?: Array<number>,
      order?: Array<number>,
      deliveryStatus?: Array<string>,
      msStatus?: Array<string>,
      msStatusDuration?: Array<number>,
      settingsUpdatePlannedActivityDescription?: Array<string>,
      planningActivityStatus?: Array<number>,
      budgetAvailability?: Array<string>,
      localApproval?: Array<string>,
      deliveryStatusId?: Array<number>,
      lastModifiedBy?: Array<string>,
      crossSetting?: Array<string>,
      sortBy?: string,
      isSortAscending?: boolean,
      page?: number,
      pageSize?: number,
      lastModifiedStartDate?: Date,
      lastModifiedEndDate?: Date,
      principalId?: number,
      deleted?: boolean,
      orphan?: boolean,
      options: any = {}
    ): FetchArgs {
      const localVarPath = `/api/SettingsUpdatePlannedActivity`;
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

      if (planningActivityStatus) {
        localVarQueryParameter["PlanningActivityStatus"] =
          planningActivityStatus;
      }

      if (lcmDeploymentStatus) {
        localVarQueryParameter["LcmDeploymentStatus"] = lcmDeploymentStatus;
      }

      if (ruleElementCount) {
        localVarQueryParameter["RuleElementCount"] = ruleElementCount;
      }
      if (budgetAvailability) {
        localVarQueryParameter["BudgetAvailability"] = budgetAvailability;
      }

      if (plannedActivityTypeFor) {
        localVarQueryParameter["plannedActivityTypeFor"] =
          plannedActivityTypeFor;
      }

      if (planningActivityResource) {
        localVarQueryParameter["PlanningActivityResource"] =
          planningActivityResource;
      }
      if (plannedActivityTypeDescription) {
        localVarQueryParameter["plannedActivityTypeDescription"] =
          plannedActivityTypeDescription;
      }
      if (successorPlannedActivityTypeResource) {
        localVarQueryParameter["SuccessorPlannedActivityTypeResource"] =
          successorPlannedActivityTypeResource;
      }

      if (order) {
        localVarQueryParameter["Order"] = order;
      }

      if (rule) {
        localVarQueryParameter["Rule"] = rule;
      }
      if (ruleforSuccessorPlannedActivityCreation) {
        localVarQueryParameter["RuleforSuccessorPlannedActivityCreation"] =
          ruleforSuccessorPlannedActivityCreation;
      }

      if (maxOrder) {
        localVarQueryParameter["MaxOrder"] = maxOrder;
      }

      if (localApproval) {
        localVarQueryParameter["LocalApproval"] = localApproval;
      }

      if (deliveryStatus) {
        localVarQueryParameter["DeliveryStatus"] = deliveryStatus;
      }
      if (msStatus) {
        localVarQueryParameter["MsStatus"] = msStatus;
      }
      if (msStatusDuration) {
        localVarQueryParameter["MsStatusDuration"] = msStatusDuration;
      }

      if (deliveryStatusId) {
        localVarQueryParameter["DeliveryStatusId"] = deliveryStatusId;
      }

      if (lastModifiedBy) {
        localVarQueryParameter["LastModifiedBy"] = lastModifiedBy;
      }

      if (settingsUpdatePlannedActivityDescription) {
        localVarQueryParameter["settingsUpdatePlannedActivityDescription"] =
          settingsUpdatePlannedActivityDescription;
      }

      if (crossSetting) {
        localVarQueryParameter["CrossSetting"] = crossSetting;
      }

      if (sortBy !== undefined) {
        localVarQueryParameter["SortBy"] = sortBy;
      }

      if (isSortAscending !== undefined) {
        localVarQueryParameter["IsSortAscending"] = isSortAscending;
      }

      if (page !== undefined) {
        localVarQueryParameter["Page"] = page;
      }

      if (pageSize !== undefined) {
        localVarQueryParameter["PageSize"] = pageSize;
      }

      if (lastModifiedStartDate !== undefined) {
        localVarQueryParameter["LastModified.StartDate"] = (
          lastModifiedStartDate as any
        ).toISOString();
      }

      if (lastModifiedEndDate !== undefined) {
        localVarQueryParameter["LastModified.EndDate"] = (
          lastModifiedEndDate as any
        ).toISOString();
      }

      if (principalId !== undefined) {
        localVarQueryParameter["PrincipalId"] = principalId;
      }

      if (deleted !== undefined) {
        localVarQueryParameter["Deleted"] = deleted;
      }

      if (orphan !== undefined) {
        localVarQueryParameter["Orphan"] = orphan;
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
    settingsUpdatePlannedActivityGetUpdateResourceSettingsUpdatePlannedActivity(
      id: number,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'id' is not null or undefined
      if (id === null || id === undefined) {
        throw new RequiredError(
          "id",
          "Required parameter id was null or undefined when calling settingsUpdatePlannedActivityGetUpdateResourceSettingsUpdatePlannedActivity."
        );
      }
      const localVarPath =
        `/api/SettingsUpdatePlannedActivity/Update{id}`.replace(
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
     * @param {SettingsUpdatePlannedActivityDtoUpdate} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    settingsUpdatePlannedActivityPut(
      body: SettingsUpdatePlannedActivityDtoUpdate,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling settingsUpdatePlannedActivityPut."
        );
      }
      const localVarPath = `/api/SettingsUpdatePlannedActivity`;
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
        <any>"SettingsUpdatePlannedActivityDtoUpdate" !== "string" ||
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
     * @param {SettingsUpdatePlannedActivityQueryObjectGrid} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    settingUpdatePlannedActivityBuildExportReport(
      body: SettingsUpdatePlannedActivityQueryObjectGrid,
      options: any = {}
    ): FetchArgs {
      // verify required parameter 'body' is not null or undefined
      if (body === null || body === undefined) {
        throw new RequiredError(
          "body",
          "Required parameter body was null or undefined when calling majorSoftwareBuildExportReport."
        );
      }
      const localVarPath = `/api/SettingsUpdatePlannedActivity/ExportReport`;
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
        <any>"SettingsUpdatePlannedActivityQueryObjectGrid" !== "string" ||
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
 * SettingsUpdatePlannedActivityApi - functional programming interface
 * @export
 */
export const SettingsUpdatePlannedActivityApiFp = function (
  configuration?: Configuration
) {
  return {
    /**
     *
     * @param {Array<ChangeGridOrderDto>} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    settingsUpdatePlannedActivityChangeGridOrderSettingsUpdatePlannedActivity(
      body: Array<ChangeGridOrderDto>,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs =
        SettingsUpdatePlannedActivityApiFetchParamCreator(
          configuration
        ).settingsUpdatePlannedActivityChangeGridOrderSettingsUpdatePlannedActivity(
          body,
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

    settingsUpdatePlannedActivityGetFilterResult(
      body: SettingsUpdatePlannedActivityQueryObjectGrid,
      propertyName?: string,
      propertyFilter?: string,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<Array<FilterValueDto>> {
      const localVarFetchArgs =
        SettingsUpdatePlannedActivityApiFetchParamCreator(
          configuration
        ).settingsUpdatePlannedActivityGetFilterResult(
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
     * @param {SettingsUpdatePlannedActivityDtoCreate} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    settingsUpdatePlannedActivityCreate(
      body: SettingsUpdatePlannedActivityDtoCreate,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs =
        SettingsUpdatePlannedActivityApiFetchParamCreator(
          configuration
        ).settingsUpdatePlannedActivityCreate(body, options);
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
    settingsUpdatePlannedActivityDelete(
      id?: number,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs =
        SettingsUpdatePlannedActivityApiFetchParamCreator(
          configuration
        ).settingsUpdatePlannedActivityDelete(id, options);
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
    settingsUpdatePlannedActivityDeleteDeep(
      id?: number,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs =
        SettingsUpdatePlannedActivityApiFetchParamCreator(
          configuration
        ).settingsUpdatePlannedActivityDeleteDeep(id, options);
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
    settingsUpdatePlannedActivityGetCreateResourceSettingsUpdatePlannedActivity(
      options?: any
    ): (
      fetch?: FetchAPI,
      basePath?: string
    ) => Promise<SettingsUpdatePlannedActivityDtoCreate> {
      const localVarFetchArgs =
        SettingsUpdatePlannedActivityApiFetchParamCreator(
          configuration
        ).settingsUpdatePlannedActivityGetCreateResourceSettingsUpdatePlannedActivity(
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

    settingsUpdatePlannedActivityGetPATypeAndDeploymentStatus(
      plannedActivityTypeFor?: number,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs =
        SettingsUpdatePlannedActivityApiFetchParamCreator(
          configuration
        ).settingsUpdatePlannedActivityGetPATypeAndDeploymentStatus(
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

    settingsUpdatePlannedActivityGetCrossSettings(
      plannedActivityTypeFor?: number,
      plannedActivityTypeId?: number,
      currentDeliveryStatus?: number,
      options?: any
    ): (
      fetch?: FetchAPI,
      basePath?: string
    ) => Promise<{ [key: string]: string }> {
      const localVarFetchArgs =
        SettingsUpdatePlannedActivityApiFetchParamCreator(
          configuration
        ).settingsUpdatePlannedActivityGetCrossSettings(
          plannedActivityTypeFor,
          plannedActivityTypeId,
          currentDeliveryStatus,
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
    settingsUpdatePlannedActivityGetRelatedRecords(
      id: number,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs =
        SettingsUpdatePlannedActivityApiFetchParamCreator(
          configuration
        ).settingsUpdatePlannedActivityGetRelatedRecords(id, options);
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
     * @param {Array<number>} [planningActivityStatus]
     * @param {Array<string>} [budgetAvailability]
     * @param {Array<string>} [localApproval]
     * @param {Array<number>} [deliveryStatusId]
     * @param {Array<string>} [lastModifiedBy]
     * @param {Array<string>} [crossSetting]
     * @param {string} [sortBy]
     * @param {boolean} [isSortAscending]
     * @param {number} [page]
     * @param {number} [pageSize]
     * @param {Date} [lastModifiedStartDate]
     * @param {Date} [lastModifiedEndDate]
     * @param {number} [principalId]
     * @param {boolean} [deleted]
     * @param {boolean} [orphan]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    settingsUpdatePlannedActivityGetSettingsUpdatePlannedActivity(
      ruleElementCount?: Array<string>,
      plannedActivityTypeFor?: Array<number>,
      lcmDeploymentStatus?: Array<string>,
      planningActivityResource?: Array<string>,
      plannedActivityTypeDescription?: Array<string>,
      successorPlannedActivityTypeResource?: Array<string>,
      ruleforSuccessorPlannedActivityCreation?: Array<boolean>,
      rule?: Array<number>,
      maxOrder?: Array<number>,
      order?: Array<number>,
      deliveryStatus?: Array<string>,
      msStatus?: Array<string>,
      msStatusDuration?: Array<number>,
      settingsUpdatePlannedActivityDescription?: Array<string>,
      planningActivityStatus?: Array<number>,
      budgetAvailability?: Array<string>,
      localApproval?: Array<string>,
      deliveryStatusId?: Array<number>,
      lastModifiedBy?: Array<string>,
      crossSetting?: Array<string>,
      sortBy?: string,
      isSortAscending?: boolean,
      page?: number,
      pageSize?: number,
      lastModifiedStartDate?: Date,
      lastModifiedEndDate?: Date,
      principalId?: number,
      deleted?: boolean,
      orphan?: boolean,
      options?: any
    ): (
      fetch?: FetchAPI,
      basePath?: string
    ) => Promise<QueryResultDtoOfSettingsUpdatePlannedActivityDtoGrid> {
      const localVarFetchArgs =
        SettingsUpdatePlannedActivityApiFetchParamCreator(
          configuration
        ).settingsUpdatePlannedActivityGetSettingsUpdatePlannedActivity(
          ruleElementCount,
          plannedActivityTypeFor,
          lcmDeploymentStatus,
          planningActivityResource,
          plannedActivityTypeDescription,
          successorPlannedActivityTypeResource,
          ruleforSuccessorPlannedActivityCreation,
          rule,
          maxOrder,
          order,
          deliveryStatus,
          msStatus,
          msStatusDuration,
          settingsUpdatePlannedActivityDescription,
          planningActivityStatus,
          budgetAvailability,
          localApproval,
          deliveryStatusId,
          lastModifiedBy,
          crossSetting,
          sortBy,
          isSortAscending,
          page,
          pageSize,
          lastModifiedStartDate,
          lastModifiedEndDate,
          principalId,
          deleted,
          orphan,
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
    settingsUpdatePlannedActivityGetUpdateResourceSettingsUpdatePlannedActivity(
      id: number,
      options?: any
    ): (
      fetch?: FetchAPI,
      basePath?: string
    ) => Promise<SettingsUpdatePlannedActivityDtoUpdate> {
      const localVarFetchArgs =
        SettingsUpdatePlannedActivityApiFetchParamCreator(
          configuration
        ).settingsUpdatePlannedActivityGetUpdateResourceSettingsUpdatePlannedActivity(
          id,
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
     * @param {SettingsUpdatePlannedActivityDtoUpdate} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    settingsUpdatePlannedActivityPut(
      body: SettingsUpdatePlannedActivityDtoUpdate,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ResultDto> {
      const localVarFetchArgs =
        SettingsUpdatePlannedActivityApiFetchParamCreator(
          configuration
        ).settingsUpdatePlannedActivityPut(body, options);
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
     * @param {SettingsUpdatePlannedActivityQueryObjectGrid} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    settingUpdatePlannedActivityBuildExportReport(
      body: SettingsUpdatePlannedActivityQueryObjectGrid,
      options?: any
    ): (fetch?: FetchAPI, basePath?: string) => Promise<ReturnFile> {
      const localVarFetchArgs =
        SettingsUpdatePlannedActivityApiFetchParamCreator(
          configuration
        ).settingUpdatePlannedActivityBuildExportReport(body, options);
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
  };
};

/**
 * SettingsUpdatePlannedActivityApi - factory interface
 * @export
 */
export const SettingsUpdatePlannedActivityApiFactory = function (
  configuration?: Configuration,
  fetch?: FetchAPI,
  basePath?: string
) {
  return {
    /**
     *
     * @param {Array<ChangeGridOrderDto>} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    settingsUpdatePlannedActivityChangeGridOrderSettingsUpdatePlannedActivity(
      body: Array<ChangeGridOrderDto>,
      options?: any
    ) {
      return SettingsUpdatePlannedActivityApiFp(
        configuration
      ).settingsUpdatePlannedActivityChangeGridOrderSettingsUpdatePlannedActivity(
        body,
        options
      )(fetch, basePath);
    },

    /**
     *
     * @param {MajorHardwareBuildQueryDto} body
     * @param {string} [propertyName]
     * @param {string} [propertyFilter]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    settingsUpdatePlannedActivityGetFilterResult(
      body: SettingsUpdatePlannedActivityQueryObjectGrid,
      propertyName?: string,
      propertyFilter?: string,
      options?: any
    ) {
      return SettingsUpdatePlannedActivityApiFp(
        configuration
      ).settingsUpdatePlannedActivityGetFilterResult(
        body,
        propertyName,
        propertyFilter,
        options
      )(fetch, basePath);
    },
    /**
     *
     * @param {SettingsUpdatePlannedActivityDtoCreate} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    settingsUpdatePlannedActivityCreate(
      body: SettingsUpdatePlannedActivityDtoCreate,
      options?: any
    ) {
      return SettingsUpdatePlannedActivityApiFp(
        configuration
      ).settingsUpdatePlannedActivityCreate(body, options)(fetch, basePath);
    },
    /**
     *
     * @param {number} [id]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    settingsUpdatePlannedActivityDelete(id?: number, options?: any) {
      return SettingsUpdatePlannedActivityApiFp(
        configuration
      ).settingsUpdatePlannedActivityDelete(id, options)(fetch, basePath);
    },
    /**
     *
     * @param {number} [id]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    settingsUpdatePlannedActivityDeleteDeep(id?: number, options?: any) {
      return SettingsUpdatePlannedActivityApiFp(
        configuration
      ).settingsUpdatePlannedActivityDeleteDeep(id, options)(fetch, basePath);
    },
    /**
     *
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    settingsUpdatePlannedActivityGetCreateResourceSettingsUpdatePlannedActivity(
      options?: any
    ) {
      return SettingsUpdatePlannedActivityApiFp(
        configuration
      ).settingsUpdatePlannedActivityGetCreateResourceSettingsUpdatePlannedActivity(
        options
      )(fetch, basePath);
    },

    settingsUpdatePlannedActivityGetPATypeAndDeploymentStatus(
      plannedActivityTypeFor?: number,
      options?: any
    ) {
      return SettingsUpdatePlannedActivityApiFp(
        configuration
      ).settingsUpdatePlannedActivityGetPATypeAndDeploymentStatus(
        plannedActivityTypeFor,
        options
      )(fetch, basePath);
    },

    settingsUpdatePlannedActivityGetCrossSettings(
      plannedActivityTypeFor?: number,
      plannedActivityTypeId?: number,
      currentDeliveryStatus?: number,
      options?: any
    ) {
      return SettingsUpdatePlannedActivityApiFp(
        configuration
      ).settingsUpdatePlannedActivityGetCrossSettings(
        plannedActivityTypeFor,
        plannedActivityTypeId,
        currentDeliveryStatus,
        options
      )(fetch, basePath);
    },
    /**
     *
     * @param {number} id
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    settingsUpdatePlannedActivityGetRelatedRecords(id: number, options?: any) {
      return SettingsUpdatePlannedActivityApiFp(
        configuration
      ).settingsUpdatePlannedActivityGetRelatedRecords(id, options)(
        fetch,
        basePath
      );
    },
    /**
     *
     * @param {Array<number>} [planningActivityStatus]
     * @param {Array<string>} [budgetAvailability]
     * @param {Array<string>} [localApproval]
     * @param {Array<number>} [deliveryStatusId]
     * @param {Array<string>} [lastModifiedBy]
     * @param {Array<string>} [crossSetting]
     * @param {string} [sortBy]
     * @param {boolean} [isSortAscending]
     * @param {number} [page]
     * @param {number} [pageSize]
     * @param {Date} [lastModifiedStartDate]
     * @param {Date} [lastModifiedEndDate]
     * @param {number} [principalId]
     * @param {boolean} [deleted]
     * @param {boolean} [orphan]
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    settingsUpdatePlannedActivityGetSettingsUpdatePlannedActivity(
      ruleElementCount?: Array<string>,
      plannedActivityTypeFor?: Array<number>,
      lcmDeploymentStatus?: Array<string>,
      planningActivityResource?: Array<string>,
      plannedActivityTypeDescription?: Array<string>,
      successorPlannedActivityTypeResource?: Array<string>,
      ruleforSuccessorPlannedActivityCreation?: Array<boolean>,
      rule?: Array<number>,
      maxOrder?: Array<number>,
      order?: Array<number>,
      deliveryStatus?: Array<string>,
      msStatus?: Array<string>,
      msStatusDuration?: Array<number>,
      settingsUpdatePlannedActivityDescription?: Array<string>,
      planningActivityStatus?: Array<number>,
      budgetAvailability?: Array<string>,
      localApproval?: Array<string>,
      deliveryStatusId?: Array<number>,
      lastModifiedBy?: Array<string>,
      crossSetting?: Array<string>,
      sortBy?: string,
      isSortAscending?: boolean,
      page?: number,
      pageSize?: number,
      lastModifiedStartDate?: Date,
      lastModifiedEndDate?: Date,
      principalId?: number,
      deleted?: boolean,
      orphan?: boolean,
      options?: any
    ) {
      return SettingsUpdatePlannedActivityApiFp(
        configuration
      ).settingsUpdatePlannedActivityGetSettingsUpdatePlannedActivity(
        ruleElementCount,
        plannedActivityTypeFor,
        lcmDeploymentStatus,
        planningActivityResource,
        plannedActivityTypeDescription,
        successorPlannedActivityTypeResource,
        ruleforSuccessorPlannedActivityCreation,
        rule,
        maxOrder,
        order,
        deliveryStatus,
        msStatus,
        msStatusDuration,
        settingsUpdatePlannedActivityDescription,
        planningActivityStatus,
        budgetAvailability,
        localApproval,
        deliveryStatusId,
        lastModifiedBy,
        crossSetting,
        sortBy,
        isSortAscending,
        page,
        pageSize,
        lastModifiedStartDate,
        lastModifiedEndDate,
        principalId,
        deleted,
        orphan,
        options
      )(fetch, basePath);
    },
    /**
     *
     * @param {number} id
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    settingsUpdatePlannedActivityGetUpdateResourceSettingsUpdatePlannedActivity(
      id: number,
      options?: any
    ) {
      return SettingsUpdatePlannedActivityApiFp(
        configuration
      ).settingsUpdatePlannedActivityGetUpdateResourceSettingsUpdatePlannedActivity(
        id,
        options
      )(fetch, basePath);
    },
    /**
     *
     * @param {SettingsUpdatePlannedActivityDtoUpdate} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    settingsUpdatePlannedActivityPut(
      body: SettingsUpdatePlannedActivityDtoUpdate,
      options?: any
    ) {
      return SettingsUpdatePlannedActivityApiFp(
        configuration
      ).settingsUpdatePlannedActivityPut(body, options)(fetch, basePath);
    },
    /**
     *
     * @param {SettingsUpdatePlannedActivityQueryObjectGrid} body
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    settingUpdatePlannedActivityBuildExportReport(
      body: SettingsUpdatePlannedActivityQueryObjectGrid,
      options?: any
    ) {
      return SettingsUpdatePlannedActivityApiFp(
        configuration
      ).settingUpdatePlannedActivityBuildExportReport(body, options)(
        fetch,
        basePath
      );
    },
  };
};

/**
 * SettingsUpdatePlannedActivityApi - object-oriented interface
 * @export
 * @class SettingsUpdatePlannedActivityApi
 * @extends {BaseAPI}
 */
export class SettingsUpdatePlannedActivityApi extends BaseAPI {
  /**
   *
   * @param {Array<ChangeGridOrderDto>} body
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof SettingsUpdatePlannedActivityApi
   */
  public settingsUpdatePlannedActivityChangeGridOrderSettingsUpdatePlannedActivity(
    body: Array<ChangeGridOrderDto>,
    options?: any
  ) {
    return SettingsUpdatePlannedActivityApiFp(
      this.configuration
    ).settingsUpdatePlannedActivityChangeGridOrderSettingsUpdatePlannedActivity(
      body,
      options
    )(this.fetch, this.basePath);
  }

  /**
   *
   * @param {MajorHardwareBuildQueryDto} body
   * @param {string} [propertyName]
   * @param {string} [propertyFilter]
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof MajorHardwareBuildApi
   */
  public settingsUpdatePlannedActivityGetFilterResult(
    body: SettingsUpdatePlannedActivityQueryObjectGrid,
    propertyName?: string,
    propertyFilter?: string,
    options?: any
  ) {
    return SettingsUpdatePlannedActivityApiFp(
      this.configuration
    ).settingsUpdatePlannedActivityGetFilterResult(
      body,
      propertyName,
      propertyFilter,
      options
    )(this.fetch, this.basePath);
  }

  /**
   *
   * @param {SettingsUpdatePlannedActivityDtoCreate} body
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof SettingsUpdatePlannedActivityApi
   */
  public settingsUpdatePlannedActivityCreate(
    body: SettingsUpdatePlannedActivityDtoCreate,
    options?: any
  ) {
    return SettingsUpdatePlannedActivityApiFp(
      this.configuration
    ).settingsUpdatePlannedActivityCreate(body, options)(
      this.fetch,
      this.basePath
    );
  }

  /**
   *
   * @param {number} [id]
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof SettingsUpdatePlannedActivityApi
   */
  public settingsUpdatePlannedActivityDelete(id?: number, options?: any) {
    return SettingsUpdatePlannedActivityApiFp(
      this.configuration
    ).settingsUpdatePlannedActivityDelete(id, options)(
      this.fetch,
      this.basePath
    );
  }

  /**
   *
   * @param {number} [id]
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof SettingsUpdatePlannedActivityApi
   */
  public settingsUpdatePlannedActivityDeleteDeep(id?: number, options?: any) {
    return SettingsUpdatePlannedActivityApiFp(
      this.configuration
    ).settingsUpdatePlannedActivityDeleteDeep(id, options)(
      this.fetch,
      this.basePath
    );
  }

  /**
   *
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof SettingsUpdatePlannedActivityApi
   */
  public settingsUpdatePlannedActivityGetCreateResourceSettingsUpdatePlannedActivity(
    options?: any
  ) {
    return SettingsUpdatePlannedActivityApiFp(
      this.configuration
    ).settingsUpdatePlannedActivityGetCreateResourceSettingsUpdatePlannedActivity(
      options
    )(this.fetch, this.basePath);
  }

  /**
   *
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof SettingsUpdatePlannedActivityApi
   */
  public settingsUpdatePlannedActivityGetPATypeAndDeploymentStatus(
    plannedActivityTypeFor?: number,
    options?: any
  ) {
    return SettingsUpdatePlannedActivityApiFp(
      this.configuration
    ).settingsUpdatePlannedActivityGetPATypeAndDeploymentStatus(
      plannedActivityTypeFor,
      options
    )(this.fetch, this.basePath);
  }

  /**
   *
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof SettingsUpdatePlannedActivityApi
   */
  public settingsUpdatePlannedActivityGetCrossSettings(
    plannedActivityTypeFor?: number,
    plannedActivityTypeId?: number,
    currentDeliveryStatus?: number,
    options?: any
  ) {
    return SettingsUpdatePlannedActivityApiFp(
      this.configuration
    ).settingsUpdatePlannedActivityGetCrossSettings(
      plannedActivityTypeFor,
      plannedActivityTypeId,
      currentDeliveryStatus,
      options
    )(this.fetch, this.basePath);
  }

  /**
   *
   * @param {number} id
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof SettingsUpdatePlannedActivityApi
   */
  public settingsUpdatePlannedActivityGetRelatedRecords(
    id: number,
    options?: any
  ) {
    return SettingsUpdatePlannedActivityApiFp(
      this.configuration
    ).settingsUpdatePlannedActivityGetRelatedRecords(id, options)(
      this.fetch,
      this.basePath
    );
  }

  /**
   *
   * @param {Array<number>} [planningActivityStatus]
   * @param {Array<string>} [budgetAvailability]
   * @param {Array<string>} [localApproval]
   * @param {Array<number>} [deliveryStatusId]
   * @param {Array<string>} [lastModifiedBy]
   * @param {Array<string>} [crossSetting]
   * @param {string} [sortBy]
   * @param {boolean} [isSortAscending]
   * @param {number} [page]
   * @param {number} [pageSize]
   * @param {Date} [lastModifiedStartDate]
   * @param {Date} [lastModifiedEndDate]
   * @param {number} [principalId]
   * @param {boolean} [deleted]
   * @param {boolean} [orphan]
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof SettingsUpdatePlannedActivityApi
   */
  public settingsUpdatePlannedActivityGetSettingsUpdatePlannedActivity(
    ruleElementCount?: Array<string>,
    plannedActivityTypeFor?: Array<number>,
    lcmDeploymentStatus?: Array<string>,
    planningActivityResource?: Array<string>,
    plannedActivityTypeDescription?: Array<string>,
    successorPlannedActivityTypeResource?: Array<string>,
    ruleforSuccessorPlannedActivityCreation?: Array<boolean>,
    rule?: Array<number>,
    maxOrder?: Array<number>,
    order?: Array<number>,
    deliveryStatus?: Array<string>,
    msStatus?: Array<string>,
    msStatusDuration?: Array<number>,
    settingsUpdatePlannedActivityDescription?: Array<string>,
    planningActivityStatus?: Array<number>,
    budgetAvailability?: Array<string>,
    localApproval?: Array<string>,
    deliveryStatusId?: Array<number>,
    lastModifiedBy?: Array<string>,
    crossSetting?: Array<string>,
    sortBy?: string,
    isSortAscending?: boolean,
    page?: number,
    pageSize?: number,
    lastModifiedStartDate?: Date,
    lastModifiedEndDate?: Date,
    principalId?: number,
    deleted?: boolean,
    orphan?: boolean,
    options?: any
  ) {
    return SettingsUpdatePlannedActivityApiFp(
      this.configuration
    ).settingsUpdatePlannedActivityGetSettingsUpdatePlannedActivity(
      ruleElementCount,
      plannedActivityTypeFor,
      lcmDeploymentStatus,
      planningActivityResource,
      plannedActivityTypeDescription,
      successorPlannedActivityTypeResource,
      ruleforSuccessorPlannedActivityCreation,
      rule,
      maxOrder,
      order,
      deliveryStatus,
      msStatus,
      msStatusDuration,
      settingsUpdatePlannedActivityDescription,
      planningActivityStatus,
      budgetAvailability,
      localApproval,
      deliveryStatusId,
      lastModifiedBy,
      crossSetting,
      sortBy,
      isSortAscending,
      page,
      pageSize,
      lastModifiedStartDate,
      lastModifiedEndDate,
      principalId,
      deleted,
      orphan,
      options
    )(this.fetch, this.basePath);
  }

  /**
   *
   * @param {number} id
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof SettingsUpdatePlannedActivityApi
   */
  public settingsUpdatePlannedActivityGetUpdateResourceSettingsUpdatePlannedActivity(
    id: number,
    options?: any
  ) {
    return SettingsUpdatePlannedActivityApiFp(
      this.configuration
    ).settingsUpdatePlannedActivityGetUpdateResourceSettingsUpdatePlannedActivity(
      id,
      options
    )(this.fetch, this.basePath);
  }

  /**
   *
   * @param {SettingsUpdatePlannedActivityDtoUpdate} body
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof SettingsUpdatePlannedActivityApi
   */
  public settingsUpdatePlannedActivityPut(
    body: SettingsUpdatePlannedActivityDtoUpdate,
    options?: any
  ) {
    return SettingsUpdatePlannedActivityApiFp(
      this.configuration
    ).settingsUpdatePlannedActivityPut(body, options)(
      this.fetch,
      this.basePath
    );
  }

  /**
   *
   * @param {SettingsUpdatePlannedActivityQueryObjectGrid} body
   * @param {*} [options] Override http request option.
   * @throws {RequiredError}
   * @memberof MajorSoftwareBuildApi
   */
  public settingUpdatePlannedActivityBuildExportReport(
    body: SettingsUpdatePlannedActivityQueryObjectGrid,
    options?: any
  ) {
    return SettingsUpdatePlannedActivityApiFp(
      this.configuration
    ).settingUpdatePlannedActivityBuildExportReport(body, options)(
      this.fetch,
      this.basePath
    );
  }
}
