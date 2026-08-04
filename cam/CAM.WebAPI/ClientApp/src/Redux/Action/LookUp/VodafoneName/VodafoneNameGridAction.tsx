import React from "react";
import {
  ApiCallWithErrorHandling,
  FilterValueDto,
} from "../../../../Business/Common/CommonBusiness";
import { VodafoneNameApi } from "../../../../Business/LookUp/VodafoneNameBusiness";
import {
  TipologicaGridDto,
  QueryResultDtoOfTipologicaGridDto,
  TipologicheQueryObjectGrid,
  LookUpGrid,
} from "../../../../Model/LookUp/LookUpGenericModel";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";

export async function GetVodafoneNameGrid(
  queryFilter?: TipologicheQueryObjectGrid
) {
  setLoader("ADD", "GetVodafoneNameGrid");

  let result: QueryResultDtoOfTipologicaGridDto | null | undefined;
  let api = new VodafoneNameApi();
  try {
    if (queryFilter !== null && queryFilter !== undefined) {
      result = await ApiCallWithErrorHandling<
        Promise<QueryResultDtoOfTipologicaGridDto>
      >(() =>
        api.vodafoneNameGetVodafoneName(
          queryFilter?.sortBy,
          queryFilter?.isSortAscending,
          queryFilter?.page,
          queryFilter?.pageSize,
          queryFilter?.lastModifiedStartDate,
          queryFilter?.lastModifiedEndDate,
          queryFilter?.principalId,
          queryFilter?.deleted,
          queryFilter?.orphan,
          queryFilter?.lastModifiedBy,
          queryFilter?.id,
          queryFilter?.description,
          queryFilter?.productName,
          queryFilter?.riskCluster,
          queryFilter?.riskLevel
        )
      );
    } else {
      result = await ApiCallWithErrorHandling<
        Promise<QueryResultDtoOfTipologicaGridDto>
      >(() => api.vodafoneNameGetVodafoneName());
    }
    let rtn = { LookUpGridResult: result, filter: null } as TipologicaGridDto;
    rootStore.dispatch({
      type: "GET_GRID_VODAFONE_NAME",
      payload: rtn as TipologicaGridDto,
    });
  } catch (error) {
    rootStore.dispatch({
      type: "GET_GRID_VODAFONE_NAME",
      payload: { LookUpGridResult: result, filter: null } as LookUpGrid,
    });
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
  }
  setLoader("REMOVE", "GetVodafoneNameGrid");
}

export async function GetVodafoneNameGridALL() {
  setLoader("ADD", "GetVodafoneNameGridALL");

  let result: QueryResultDtoOfTipologicaGridDto | null | undefined;
  let api = new VodafoneNameApi();
  try {
    result = await ApiCallWithErrorHandling<
      Promise<QueryResultDtoOfTipologicaGridDto>
    >(() => api.vodafoneNameGetVodafoneName());
    let rtn = { LookUpGridResult: result, filter: null } as TipologicaGridDto;
    rootStore.dispatch({
      type: "GET_GRID_VODAFONE_NAME_ALL",
      payload: rtn as TipologicaGridDto,
    });
    setLoader("REMOVE", "GetVodafoneNameGridALL");
    return result;
  } catch (error) {
    rootStore.dispatch({
      type: "GET_GRID_VODAFONE_NAME_ALL",
      payload: { LookUpGridResult: result, filter: null } as LookUpGrid,
    });
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
  }
  setLoader("REMOVE", "GetVodafoneNameGridALL");
}

export async function GetFilterColumVodafoneName(
  columName: string,
  columValue: string,
  queryFilter?: TipologicheQueryObjectGrid
) {
  let result: FilterValueDto[] | undefined;
  let api = new VodafoneNameApi();
  if (queryFilter !== null && queryFilter !== undefined) {
    result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
      api.vodafoneNameGetFilterResult(
        columName,
        columValue,
        queryFilter?.sortBy,
        queryFilter?.isSortAscending,
        queryFilter?.page,
        queryFilter?.pageSize,
        queryFilter?.lastModifiedStartDate,
        queryFilter?.lastModifiedEndDate,
        queryFilter?.principalId,
        queryFilter?.deleted,
        queryFilter?.orphan,
        queryFilter?.lastModifiedBy,
        queryFilter?.id,
        queryFilter?.description,
        queryFilter?.productName
      )
    );
  } else {
    result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
      api.vodafoneNameGetFilterResult(columName, columValue)
    );
  }
  let rtn = { filter: result, LookUpGridResult: null } as LookUpGrid;
  rootStore.dispatch({ type: "GET_FILTER_VODAFONE_NAME", payload: rtn });
  // setLoader("REMOVE", "GetFilterColumVodafoneName");
}
