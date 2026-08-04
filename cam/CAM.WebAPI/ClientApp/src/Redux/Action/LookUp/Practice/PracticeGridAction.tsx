import React from "react";
import {
  ApiCallWithErrorHandling,
  FilterValueDto,
} from "../../../../Business/Common/CommonBusiness";
import { PracticeApi } from "../../../../Business/LookUp/PracticeBusiness";
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

export async function GetPracticeGrid(
  queryFilter?: TipologicheQueryObjectGrid
) {
  setLoader("ADD", "GetPracticeGrid");

  let result: QueryResultDtoOfTipologicaGridDto | null | undefined;
  let api = new PracticeApi();
  try {
    if (queryFilter !== null && queryFilter !== undefined) {
      result = await ApiCallWithErrorHandling<
        Promise<QueryResultDtoOfTipologicaGridDto>
      >(() =>
        api.practiceGetPractice(
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
          queryFilter?.practiceId,
          queryFilter?.practiceDescription
        )
      );
    } else {
      result = await ApiCallWithErrorHandling<
        Promise<QueryResultDtoOfTipologicaGridDto>
      >(() => api.practiceGetPractice());
    }
    // if (result?.items?.length === 0 || result?.totalItems === undefined) {
    //     rootStore.dispatch(setNotification({ message: "no results found", notifyType: NotifyType.warning }));
    // }
    let rtn = { LookUpGridResult: result, filter: null } as TipologicaGridDto;
    rootStore.dispatch({
      type: "GET_GRID_PRACTICE",
      payload: rtn as TipologicaGridDto,
    });
  } catch (error) {
    rootStore.dispatch({
      type: "GET_GRID_PRACTICE",
      payload: { LookUpGridResult: result, filter: null } as LookUpGrid,
    });
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
  }
  setLoader("REMOVE", "GetPracticeGrid");
}

export async function GetPracticeGridALL() {
  setLoader("ADD", "GetPracticeGridALL");

  let result: QueryResultDtoOfTipologicaGridDto | null | undefined;
  let api = new PracticeApi();
  try {
    result = await ApiCallWithErrorHandling<
      Promise<QueryResultDtoOfTipologicaGridDto>
    >(() => api.practiceGetPractice());
    let rtn = { LookUpGridResult: result, filter: null } as TipologicaGridDto;
    rootStore.dispatch({
      type: "GET_GRID_PRACTICE_ALL",
      payload: rtn as TipologicaGridDto,
    });
  } catch (error) {
    rootStore.dispatch({
      type: "GET_GRID_PRACTICE_ALL",
      payload: { LookUpGridResult: result, filter: null } as LookUpGrid,
    });
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
  }
  setLoader("REMOVE", "GetPracticeGridALL");
}

export async function GetFilterColumPractice(
  columName: string,
  columValue: string,
  queryFilter?: TipologicheQueryObjectGrid
) {
  // setLoader("ADD", "GetFilterColumPractice");

  let result: FilterValueDto[] | undefined;
  let api = new PracticeApi();
  if (queryFilter !== null && queryFilter !== undefined) {
    result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
      api.practiceGetFilterResult(
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
        queryFilter?.practiceId,
        queryFilter?.practiceDescription
      )
    );
  } else {
    result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
      api.practiceGetFilterResult(columName, columValue)
    );
  }
  let rtn = { filter: result, LookUpGridResult: null } as LookUpGrid;
  rootStore.dispatch({ type: "GET_FILTER_PRACTICE", payload: rtn });
  // setLoader("REMOVE", "GetFilterColumPractice");
}
