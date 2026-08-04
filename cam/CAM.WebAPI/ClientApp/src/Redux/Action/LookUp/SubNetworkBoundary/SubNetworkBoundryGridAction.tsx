import {
  ApiCallWithErrorHandling,
  FilterValueDto,
} from "../../../../Business/Common/CommonBusiness";
import { SubNetworkBoundaryApi } from "../../../../Business/LookUp/SubnetworkBoundryBusiness";
import {
  QueryResultDtoOfSubNetworkBoundaryGridDto,
  SubNetworkBoundaryQueryDto,
  LookUpSubNetworkBoundaryGrid,
} from "../../../../Model/LookUp/SubnetworkBoundry";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";

export async function GetSubNetworkBoundaryGrid(
  queryFilter?: SubNetworkBoundaryQueryDto
) {
  setLoader("ADD", "GetSubNetworkBoundaryGrid");
  let result: QueryResultDtoOfSubNetworkBoundaryGridDto | null | undefined;
  let api = new SubNetworkBoundaryApi();
  try {
    if (queryFilter !== null && queryFilter !== undefined) {
      result = await ApiCallWithErrorHandling<
        Promise<QueryResultDtoOfSubNetworkBoundaryGridDto>
      >(() =>
        api.subNetworkBoundaryGetSubNetworkBoundary(
          queryFilter?.subNetworkBoundaryId,
          queryFilter?.vodafoneName,
          queryFilter?.subNetworkBoundaryDescription,
          queryFilter?.alias,
          queryFilter?.allSupportedServices,
          queryFilter?.c3C4,
          queryFilter?.criticalAssetType,
          queryFilter?.criticality,
          queryFilter?.customerWheel,
          queryFilter?.gdprRelevant,
          queryFilter?.gdrpClassificationValue,
          queryFilter?.internetFacing,
          queryFilter?.lcmPolicy,
          queryFilter?.missionCritical,
          queryFilter?.securityElement,
          queryFilter?.pcisox,
          queryFilter?.systemFunction,
          queryFilter?.sortBy,
          queryFilter?.isSortAscending,
          queryFilter?.page,
          queryFilter?.pageSize,
          queryFilter?.lastModifiedValueStartDate,
          queryFilter?.lastModifiedValueEndDate,
          queryFilter?.principalId,
          queryFilter?.deleted,
          queryFilter?.orphan,
          queryFilter?.lastModifiedBy
        )
      );
    } else {
      result = await ApiCallWithErrorHandling<
        Promise<QueryResultDtoOfSubNetworkBoundaryGridDto>
      >(() => api.subNetworkBoundaryGetSubNetworkBoundary());
    }

    let rtn = {
      LookUpGridResult: result,
      filter: null,
    } as LookUpSubNetworkBoundaryGrid;
    rootStore.dispatch({
      type: "GET_GRID_SUBNETWORK_BOUNDARY",
      payload: rtn as LookUpSubNetworkBoundaryGrid,
    });
  } catch (error) {
    rootStore.dispatch({
      type: "GET_GRID_SUBNETWORK_BOUNDARY",
      payload: {
        LookUpGridResult: result,
        filter: null,
      } as LookUpSubNetworkBoundaryGrid,
    });
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
  }
  setLoader("REMOVE", "GetSubNetworkBoundaryGrid");
}

export async function GetSubNetworkBoundaryGridALL() {
  setLoader("ADD", "GetSubNetworkBoundaryGridALL");

  let result: QueryResultDtoOfSubNetworkBoundaryGridDto | null | undefined;
  let api = new SubNetworkBoundaryApi();
  try {
    result = await ApiCallWithErrorHandling<
      Promise<QueryResultDtoOfSubNetworkBoundaryGridDto>
    >(() => api.subNetworkBoundaryGetSubNetworkBoundary());

    let rtn = {
      LookUpGridResult: result,
      filter: null,
    } as LookUpSubNetworkBoundaryGrid;
    rootStore.dispatch({
      type: "GET_GRID_SUBNETWORK_BOUNDARY_ALL",
      payload: rtn as LookUpSubNetworkBoundaryGrid,
    });
  } catch (error) {
    rootStore.dispatch({
      type: "GET_GRID_SUBNETWORK_BOUNDARY_ALL",
      payload: {
        LookUpGridResult: result,
        filter: null,
      } as LookUpSubNetworkBoundaryGrid,
    });
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
  }
  setLoader("REMOVE", "GetSubNetworkBoundaryGridALL");
}

export async function GetFilterColumSubNetworkBoundary(
  columName: string,
  columValue: string,
  queryFilter?: SubNetworkBoundaryQueryDto
) {
  // setLoader("ADD", "GetFilterColumSubNetworkBoundary");

  let result: FilterValueDto[] | undefined;
  let api = new SubNetworkBoundaryApi();

  if (queryFilter !== null && queryFilter !== undefined) {
    result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
      api.subNetworkBoundaryGetFilterResult(
        columName,
        columValue,
        queryFilter?.subNetworkBoundaryId,
        queryFilter?.vodafoneName,
        queryFilter?.subNetworkBoundaryDescription,
        queryFilter?.alias,
        queryFilter?.AllSupportedServices,
        queryFilter?.c3C4,
        queryFilter?.criticalAssetType,
        queryFilter?.criticality,
        queryFilter?.customerWheel,
        queryFilter?.gdprRelevant,
        queryFilter?.gdrpClassificationValue,
        queryFilter?.internetFacing,
        queryFilter?.lcmPolicy,
        queryFilter?.missionCritical,
        queryFilter?.securityElement,
        queryFilter?.pcisox,
        queryFilter?.systemFunction,
        queryFilter?.sortBy,
        queryFilter?.isSortAscending,
        queryFilter?.page,
        queryFilter?.pageSize,
        queryFilter?.lastModifiedValueStartDate,
        queryFilter?.lastModifiedValueEndDate,
        queryFilter?.principalId,
        queryFilter?.deleted,
        queryFilter?.orphan,
        queryFilter?.lastModifiedBy
      )
    );
  } else {
    result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
      api.subNetworkBoundaryGetFilterResult(columName, columValue)
    );
  }
  let rtn = {
    filter: result,
    LookUpGridResult: null,
  } as LookUpSubNetworkBoundaryGrid;
  rootStore.dispatch({ type: "GET_FILTER_SUBNETWORK_BOUNDARY", payload: rtn });
  // setLoader("REMOVE", "GetFilterColumSubNetworkBoundary");
}
