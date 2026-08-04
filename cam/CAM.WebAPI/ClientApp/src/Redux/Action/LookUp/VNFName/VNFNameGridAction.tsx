import React from "react";
import {
  ApiCallWithErrorHandling,
  FilterValueDto,
} from "../../../../Business/Common/CommonBusiness";
import { VNFNameApi } from "../../../../Business/LookUp/VNFNameBusiness";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
import {
  VNFNameGrid,
  VNFNameQueryObjectGrid,
  QueryResultDtoOfVNFNameDtoGrid,
} from "../../../../Model/LookUp/VNFName";

export async function GetVNFNameGrid(queryFilter?: VNFNameQueryObjectGrid) {
  setLoader("ADD", "GetLocationGrid");

  let result: QueryResultDtoOfVNFNameDtoGrid | null | undefined;
  let api = new VNFNameApi();
  try {
    if (queryFilter !== null && queryFilter !== undefined) {
      result = await ApiCallWithErrorHandling<
        Promise<QueryResultDtoOfVNFNameDtoGrid>
      >(() => api.VNFNameGet(queryFilter));
    } else {
      result = await ApiCallWithErrorHandling<
        Promise<QueryResultDtoOfVNFNameDtoGrid>
      >(() => api.VNFNameGet());
    }
    // if (result?.items?.length === 0 || result?.totalItems === undefined) {
    //     rootStore.dispatch(setNotification({ message: "no results found", notifyType: NotifyType.warning }));
    // }
    let rtn = { LookUpGridResult: result, filter: null } as VNFNameGrid;
    rootStore.dispatch({
      type: "GET_GRID_VNFNAME",
      payload: rtn as VNFNameGrid,
    });
  } catch (error) {
    rootStore.dispatch({
      type: "GET_GRID_VNFNAME",
      payload: { LookUpGridResult: result, filter: null } as VNFNameGrid,
    });
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
  }
  setLoader("REMOVE", "GetVNFNameGrid");
}

export async function GetVNFNameGridALL(queryFilter) {
  setLoader("ADD", "GetVNFNameGridALL");

  let result: QueryResultDtoOfVNFNameDtoGrid | null | undefined;
  let api = new VNFNameApi();
  try {
    result = await ApiCallWithErrorHandling<
      Promise<QueryResultDtoOfVNFNameDtoGrid>
    >(() => api.VNFNameGet(queryFilter));
    let rtn = { LookUpGridResult: result, filter: null } as VNFNameGrid;
    rootStore.dispatch({
      type: "GET_GRID_VNFNAME_ALL",
      payload: rtn as VNFNameGrid,
    });
  } catch (error) {
    rootStore.dispatch({
      type: "GET_GRID_VNFNAME_ALL",
      payload: { LookUpGridResult: result, filter: null } as VNFNameGrid,
    });
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
  }
  setLoader("REMOVE", "GetVNFNameGridALL");
}

export async function GetFilterColumVNFName(
  columName: string,
  columValue: string,
  queryFilter?: VNFNameQueryObjectGrid
) {
  // setLoader("ADD", "GetFilterColumLocation");

  let result: FilterValueDto[] | undefined;
  let api = new VNFNameApi();
  if (queryFilter !== null && queryFilter !== undefined) {
    result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
      api.VNFNameGetFilterResult(
        columName,
        columValue,
        queryFilter?.vnfDescription,
        queryFilter?.vnfNameId,
        queryFilter?.productId,
        queryFilter?.product,
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
      api.VNFNameGetFilterResult(columName, columValue)
    );
  }
  let rtn = { filter: result, LookUpGridResult: null } as VNFNameGrid;
  rootStore.dispatch({ type: "GET_FILTER_VNFNAME", payload: rtn });
  // setLoader("REMOVE", "GetFilterColumLocation");
}
