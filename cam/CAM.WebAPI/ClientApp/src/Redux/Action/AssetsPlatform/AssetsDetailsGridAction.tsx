// AssetsDetailsGridAction.ts
import { ResultDto } from "../../../Model/CommonModels";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";

import {
  GET_GRID_ASSETS_DETAILS,
  GET_FILTER_ASSETS_GRID_RESULT,
  AssetsDetailsGrid,
  AssetMigrationApiResponse,
  DaAssetsMigrationGridContainer,
  DaAssetMigrationGrid,
  GET_GRID_ASSETS_RESULT,
  QueryResultDtoOfDAAssetMigrationDtoGrid,
} from "../../../Model/LookUp/AssetMigrationModels";

import {
  ApiCallWithErrorHandling,
  FilterValueDto,
} from "../../../Business/Common/CommonBusiness";
import { NetworkElementAsPlannedApi } from "../../../Business/NetworkElementAsPlannedBusiness";
import {
  CREATE_ASSET_MIGRATION,
  DaAssetMigrationDtoCreate,
  QueryResultDtoOfNetworkElementAsPlannedDtoGrid,
} from "../../../Model/NetworkElementAsPlanned";

/**
 * Fetch Asset Migration Grid Data for Lookup
 * @param opcoId - Optional Opco ID
 * @param dcfId - Optional DCF ID
 * @param paId - Optional Planned Activity ID
 */
export async function GetAssetsDetailsGrid(
  opcoId?: number,
  dcfId?: number,
  plannedDcfId?: number,
  paId?: number,
  daMigrationId?: number
) {
  setLoader("ADD", "GetAssetsDetailsGrid");

  let api = new NetworkElementAsPlannedApi();

  try {
    const result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
      api.getAssetsForPlatfromMigration(
        opcoId,
        dcfId,
        plannedDcfId,
        paId,
        daMigrationId
      )
    );

    if (result?.warning) {
      rootStore.dispatch(
        setNotification({
          message: result?.info ?? "Warning while loading Assets",
          notifyType: NotifyType.error,
        })
      );
      rootStore.dispatch({
        type: GET_GRID_ASSETS_DETAILS,
        payload: {
          AssetsDetailsGridResult: null,
          filter: null,
        } as AssetsDetailsGrid,
      });
    } else {
      const data = result as AssetMigrationApiResponse;
      rootStore.dispatch({
        type: GET_GRID_ASSETS_DETAILS,
        payload: {
          AssetsDetailsGridResult: data ?? null,
          filter: null,
        } as AssetsDetailsGrid,
      });
    }
    setLoader("REMOVE", "GetAssetsDetailsGrid");
    return result;
  } catch (error) {
    rootStore.dispatch(
      setNotification({
        message: "Failed to fetch Assets Details",
        notifyType: NotifyType.error,
      })
    );

    rootStore.dispatch({
      type: GET_GRID_ASSETS_DETAILS,
      payload: {
        AssetsDetailsGridResult: null,
        filter: null,
      } as AssetsDetailsGrid,
    });
  }

  setLoader("REMOVE", "GetAssetsDetailsGrid");
}

export async function createAssetMigration(
  paId: number,
  data: DaAssetMigrationDtoCreate
) {
  setLoader("ADD", "createAssetMigration");
  const api = new NetworkElementAsPlannedApi();

  try {
    const result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
      api.createAssetMigration(paId, data)
    );

    const payload = {
      ResultDtoCreate: result,
      daAssetMigrationDtoCreate: data,
    };

    rootStore.dispatch(
      setNotification({
        message: result?.info ?? "",
        notifyType: result?.warning ? NotifyType.error : NotifyType.success,
      })
    );

    rootStore.dispatch({
      type: CREATE_ASSET_MIGRATION,
      payload,
    });

    return payload;
  } catch (error) {
    rootStore.dispatch(
      setNotification({
        message: "Asset migration failed",
        notifyType: NotifyType.error,
      })
    );
    return {
      ResultDtoCreate: { success: false, message: "Error", errors: [] }, // or whatever minimal
      daAssetMigrationDtoCreate: data,
    };
  } finally {
    setLoader("REMOVE", "createAssetMigration");
  }
}

export async function GetAssetsPlatformMigrationGrid(
  queryFilter?: DaAssetMigrationGrid
) {
  if (
    !queryFilter ||
    !queryFilter.opcoId?.length ||
    !queryFilter.currentDcfId?.length
  ) {
    return Promise.resolve(null);
  }
  setLoader("ADD", "GetNetworkElementAsPlannedGrid");

  let api = new NetworkElementAsPlannedApi();
  let result: QueryResultDtoOfDAAssetMigrationDtoGrid | null | undefined;
  try {
    result = await ApiCallWithErrorHandling<
      Promise<QueryResultDtoOfDAAssetMigrationDtoGrid>
    >(() => api.getAssetsForDAGridPlatfromMigration(queryFilter ?? {}));

    let rtn = {
      DAAssetsMigrationGridResult: result,
      filter: null,
    } as DaAssetsMigrationGridContainer;
    rootStore.dispatch({
      type: GET_GRID_ASSETS_RESULT,
      payload: rtn,
    });
  } catch (error) {
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
    rootStore.dispatch({
      type: GET_GRID_ASSETS_RESULT,
      payload: {
        DAAssetsMigrationGridResult: result,
        filter: null,
      } as DaAssetsMigrationGridContainer,
    });
  }
  setLoader("REMOVE", "GetNetworkElementAsPlannedGrid");
}

export async function GetFilterColumAssetsPlatformMigrationGrid(
  columName: string,
  columValue: string,
  queryFilter?: DaAssetMigrationGrid
) {
  // setLoader("ADD", "GetFilterColumNetworkElementAsPlanned");

  let result: FilterValueDto[] | undefined;
  let api = new NetworkElementAsPlannedApi();
  result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
    api.getAssetsForDAGridPlatfromMigrationFilterResult(
      queryFilter ?? {},
      columName,
      columValue
    )
  );

  let rtn = {
    DAAssetsMigrationGridResult: null,
    filter: result,
  } as DaAssetsMigrationGridContainer;
  rootStore.dispatch({
    type: GET_FILTER_ASSETS_GRID_RESULT,
    payload: rtn,
  });
  // setLoader("REMOVE", "GetFilterColumNetworkElementAsPlanned");
}

export async function fetchNewElementNamesDirectly(
  paId?: number,
  opCoId?: number,
  dcfId?: number
): Promise<string[]> {
  try {
    const api = new NetworkElementAsPlannedApi();

    const filter: DaAssetMigrationGrid = {
      plannedActivityId: paId ? [paId] : [],
      page: 1,
      pageSize: 10,
      daAssetMigrationId: [],
      networkElementAsPlannedId: [],
      newelEmentName: [],
      targetDesignComponenetId: [],
      environmentDesc: [],
      deploymentStatusDesc: [],
      opcoDesc: [],
      opcoId: opCoId ? [opCoId] : [],
      locationDesc: [],
      rfoDate: undefined,
      rfsDate: undefined,
      migrationCompletionDate: undefined,
      trafficNodePercentage: [],
      oldAssetName: [],
      currentDesignComponenet: [],
      targetDesignComponenet: [],
      newEnvironment: [],
      newDeploymentStatus: [],
      location: [],
      oldEnvironment: [],
      oldDeploymentType: [],
      oldDeploymentStatus: [],
      currentDcfId: dcfId ? [dcfId] : [],
      sortBy: "",
      isSortAscending: false,
      principalId: undefined,
    };

    const result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(
      () =>
        api.getAssetsForDAGridPlatfromMigrationFilterResult(
          filter,
          "newelEmentName",
          ""
        )
    );

    if (result && Array.isArray(result)) {
      const names = result
        .filter(
          (item) =>
            item &&
            item.value !== null &&
            item.value !== undefined &&
            item.value !== ""
        )
        .map((item) => item.value?.toString().trim())
        .filter((name) => name && name !== "");

      return names;
    }

    return [];
  } catch (error) {
    console.error("Error fetching new element names:", error);
    return [];
  }
}
