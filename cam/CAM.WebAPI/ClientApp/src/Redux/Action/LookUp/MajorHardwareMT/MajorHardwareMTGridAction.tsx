import React from "react";
import {
  ApiCallWithErrorHandling,
  FilterValueDto,
} from "../../../../Business/Common/CommonBusiness";
import { MajorHardwareMTApi } from "../../../../Business/LookUp/MajorHardwareMTBusiness";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
import {
  MajorHardwareMTGrid,
  MajorHardwareMTQueryObjectGrid,
  QueryResultDtoOfMajorHardwareMTDtoGrid,
} from "../../../../Model/LookUp/MajorHardwareMT";

export async function GetMajorHardwareMTGrid(
  queryFilter?: MajorHardwareMTQueryObjectGrid
) {
  setLoader("ADD", "GetMajorHardwareMTGrid");

  let result: QueryResultDtoOfMajorHardwareMTDtoGrid | null | undefined;
  let api = new MajorHardwareMTApi();
  try {
    if (queryFilter !== null && queryFilter !== undefined) {
      result = await ApiCallWithErrorHandling<
        Promise<QueryResultDtoOfMajorHardwareMTDtoGrid>
      >(() => api.MajorHardwareMTGet(queryFilter));
    } else {
      result = await ApiCallWithErrorHandling<
        Promise<QueryResultDtoOfMajorHardwareMTDtoGrid>
      >(() => api.MajorHardwareMTGet());
    }
    // if (result?.items?.length === 0 || result?.totalItems === undefined) {
    //     rootStore.dispatch(setNotification({ message: "no results found", notifyType: NotifyType.warning }));
    // }
    let rtn = { LookUpGridResult: result, filter: null } as MajorHardwareMTGrid;
    rootStore.dispatch({
      type: "GET_GRID_MAJORHARDWAREMT",
      payload: rtn as MajorHardwareMTGrid,
    });
  } catch (error) {
    rootStore.dispatch({
      type: "GET_GRID_MAJORHARDWAREMT",
      payload: {
        LookUpGridResult: result,
        filter: null,
      } as MajorHardwareMTGrid,
    });
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
  }
  setLoader("REMOVE", "GetMajorHardwareMTGrid");
}

export async function GetMajorHardwareMTGridALL(queryFilter) {
  setLoader("ADD", "GetMajorHardwareMTGridALL");

  let result: QueryResultDtoOfMajorHardwareMTDtoGrid | null | undefined;
  let api = new MajorHardwareMTApi();
  try {
    result = await ApiCallWithErrorHandling<
      Promise<QueryResultDtoOfMajorHardwareMTDtoGrid>
    >(() => api.MajorHardwareMTGet(queryFilter));
    let rtn = { LookUpGridResult: result, filter: null } as MajorHardwareMTGrid;
    rootStore.dispatch({
      type: "GET_GRID_MAJORHARDWAREMT_ALL",
      payload: rtn as MajorHardwareMTGrid,
    });
  } catch (error) {
    rootStore.dispatch({
      type: "GET_GRID_MAJORHARDWAREMT_ALL",
      payload: {
        LookUpGridResult: result,
        filter: null,
      } as MajorHardwareMTGrid,
    });
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
  }
  setLoader("REMOVE", "GetMajorHardwareMTGridALL");
}

export async function GetFilterColumMajorHardwareMT(
  columName: string,
  columValue: string,
  queryFilter?: MajorHardwareMTQueryObjectGrid
) {
  // setLoader("ADD", "GetFilterColumLocation");

  let result: FilterValueDto[] | undefined;
  let api = new MajorHardwareMTApi();
  if (queryFilter !== null && queryFilter !== undefined) {
    result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
      api.MajorHardwareMTGetFilterResult(
        columName,
        columValue,
        queryFilter?.majorHardwareBuildAsisId,
        queryFilter?.orgEqpManuFacturerId?.map(String),
        queryFilter?.hardwareSolution,
        queryFilter?.hardwareSolutionReourceId?.map(String),
        queryFilter?.platformId?.map(String),
        queryFilter?.hardwareType,
        queryFilter?.buildConstructionId?.map(String),
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
      api.MajorHardwareMTGetFilterResult(columName, columValue)
    );
  }
  let rtn = { filter: result, LookUpGridResult: null } as MajorHardwareMTGrid;
  rootStore.dispatch({ type: "GET_FILTER_MAJORHARDWAREMT", payload: rtn });
  // setLoader("REMOVE", "GetFilterColumLocation");
}
