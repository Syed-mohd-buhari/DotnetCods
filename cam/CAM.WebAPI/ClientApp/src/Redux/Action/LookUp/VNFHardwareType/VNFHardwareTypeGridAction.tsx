import React from "react";
import {
  ApiCallWithErrorHandling,
  FilterValueDto,
} from "../../../../Business/Common/CommonBusiness";
import { VNFHardwareTypeApi } from "../../../../Business/LookUp/VNFHardwareTypeBusiness";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
import {
  VNFHardwareTypeGrid,
  VNFHardwareTypeQueryObjectGrid,
  QueryResultDtoOfVNFHardwareTypeDtoGrid,
} from "../../../../Model/LookUp/VNFHardwareType";

export async function GetVNFHardwareTypeGrid(
  queryFilter?: VNFHardwareTypeQueryObjectGrid
) {
  setLoader("ADD", "GetVNFHardwareTypeGrid");

  let result: QueryResultDtoOfVNFHardwareTypeDtoGrid | null | undefined;
  let api = new VNFHardwareTypeApi();
  try {
    if (queryFilter !== null && queryFilter !== undefined) {
      result = await ApiCallWithErrorHandling<
        Promise<QueryResultDtoOfVNFHardwareTypeDtoGrid>
      >(() => api.VNFHardwareTypeGet(queryFilter));
    } else {
      result = await ApiCallWithErrorHandling<
        Promise<QueryResultDtoOfVNFHardwareTypeDtoGrid>
      >(() => api.VNFHardwareTypeGet());
    }
    // if (result?.items?.length === 0 || result?.totalItems === undefined) {
    //     rootStore.dispatch(setNotification({ message: "no results found", notifyType: NotifyType.warning }));
    // }
    let rtn = { LookUpGridResult: result, filter: null } as VNFHardwareTypeGrid;
    rootStore.dispatch({
      type: "GET_GRID_VNFHARDWARETYPE",
      payload: rtn as VNFHardwareTypeGrid,
    });
  } catch (error) {
    rootStore.dispatch({
      type: "GET_GRID_VNFHARDWARETYPE",
      payload: {
        LookUpGridResult: result,
        filter: null,
      } as VNFHardwareTypeGrid,
    });
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
  }
  setLoader("REMOVE", "GetVNFHardwareTypeGrid");
}

export async function GetVNFHardwareTypeGridALL(queryFilter) {
  setLoader("ADD", "GetVNFHardwareTypeGridALL");

  let result: QueryResultDtoOfVNFHardwareTypeDtoGrid | null | undefined;
  let api = new VNFHardwareTypeApi();
  try {
    result = await ApiCallWithErrorHandling<
      Promise<QueryResultDtoOfVNFHardwareTypeDtoGrid>
    >(() => api.VNFHardwareTypeGet(queryFilter));
    let rtn = { LookUpGridResult: result, filter: null } as VNFHardwareTypeGrid;
    rootStore.dispatch({
      type: "GET_GRID_VNFHARDWARETYPE_ALL",
      payload: rtn as VNFHardwareTypeGrid,
    });
  } catch (error) {
    rootStore.dispatch({
      type: "GET_GRID_VNFHARDWARETYPE_ALL",
      payload: {
        LookUpGridResult: result,
        filter: null,
      } as VNFHardwareTypeGrid,
    });
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
  }
  setLoader("REMOVE", "GetVNFHardwareTypeGridALL");
}

export async function GetFilterColumVNFHardwareType(
  columName: string,
  columValue: string,
  queryFilter?: VNFHardwareTypeQueryObjectGrid
) {
  // setLoader("ADD", "GetFilterColumLocation");

  let result: FilterValueDto[] | undefined;
  let api = new VNFHardwareTypeApi();
  if (queryFilter !== null && queryFilter !== undefined) {
    result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
      api.VNFHardwareTypeGetFilterResult(
        columName,
        columValue,
        queryFilter?.description,
        queryFilter?.vnfHardwareId,
        queryFilter?.sortBy,
        queryFilter?.isSortAscending,
        queryFilter?.page,
        queryFilter?.pageSize,
        queryFilter?.lastModified?.startDate,
        queryFilter?.lastModified?.endDate,
        queryFilter?.principalId,
        queryFilter?.deleted,
        queryFilter?.orphan,
        queryFilter?.lastModifiedBy
      )
    );
  } else {
    result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
      api.VNFHardwareTypeGetFilterResult(columName, columValue)
    );
  }
  let rtn = { filter: result, LookUpGridResult: null } as VNFHardwareTypeGrid;
  rootStore.dispatch({ type: "GET_FILTER_VNFHARDWARETYPE", payload: rtn });
  // setLoader("REMOVE", "GetFilterColumLocation");
}
