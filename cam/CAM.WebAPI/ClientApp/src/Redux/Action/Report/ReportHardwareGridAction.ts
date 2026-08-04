import React from "react";
import {
  ApiCallWithErrorHandling,
  FilterValueDto,
} from "../../../Business/Common/CommonBusiness";
import { ReportHardwareApi } from "../../../Business/Report/ReportHardwareBusiness";
import {
  ReportHardwareDtoGrid,
  ReportHardwareGrid,
  ReportHardwareQueryObjectGrid,
  QueryResultDtoOfReportHardwareDtoGrid,
  GET_GRID_REPORT_HARDWARE,
  GET_FILTER_REPORT_HARDWARE,
} from "../../../Model/Report/ReportHardwareModel";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";
import {
  LookUpGrid,
  QueryResultDtoOfTipologicaGridDto,
  TipologicaGridDto,
  TipologicheQueryObjectGrid,
} from "../../../Model/LookUp/LookUpGenericModel";
import { SharedLookUpApi } from "../../../Business/LookUp/SharedLookUpBusiness";
import { ReportSoftwareApi } from "../../../Business/Report/ReportSoftwareBusiness";

export async function GetReportHardwareGrid(
  queryFilter?: ReportHardwareQueryObjectGrid
) {
  setLoader("ADD", "GetReportHardwareGrid");

  let result: QueryResultDtoOfReportHardwareDtoGrid | null | undefined;
  let api = new ReportHardwareApi();
  try {
    result = await ApiCallWithErrorHandling<
      Promise<QueryResultDtoOfReportHardwareDtoGrid>
    >(() => api.reportHardwareGetReport(queryFilter ?? {}));

    let rtn = {
      ReportHardwareGridResult: result,
      filter: null,
    } as ReportHardwareGrid;
    rootStore.dispatch({ type: GET_GRID_REPORT_HARDWARE, payload: rtn });
  } catch (error) {
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
    let rtn = {
      ReportHardwareGridResult: null,
      filter: null,
    } as ReportHardwareGrid;
    rootStore.dispatch({ type: GET_GRID_REPORT_HARDWARE, payload: rtn });
  }
  setLoader("REMOVE", "GetReportHardwareGrid");
}

export async function GetFilterColumReportHardware(
  columName: string,
  columValue: string,
  queryFilter?: ReportHardwareQueryObjectGrid
) {
  let api = new ReportHardwareApi();
  let result: FilterValueDto[] | undefined;
  result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
    api.reportHardwareGetFilterResult(queryFilter ?? {}, columName, columValue)
  );
  let rtn = {
    filter: result,
    ReportHardwareGridResult: null,
  } as ReportHardwareGrid;
  rootStore.dispatch({ type: GET_FILTER_REPORT_HARDWARE, payload: rtn });
}

export async function GetSharedLookUpGrid(
  queryFilter?: TipologicheQueryObjectGrid
) {
  let result: QueryResultDtoOfTipologicaGridDto | null | undefined;
  let api = new SharedLookUpApi();
  try {
    if (queryFilter !== null && queryFilter !== undefined) {
      result = await ApiCallWithErrorHandling<
        Promise<QueryResultDtoOfTipologicaGridDto>
      >(() =>
        api.sharedLookUpGetSharedLookUp(
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
          queryFilter?.description
        )
      );
    } else {
      result = await ApiCallWithErrorHandling<
        Promise<QueryResultDtoOfTipologicaGridDto>
      >(() => api.sharedLookUpGetSharedLookUp());
    }

    return result;
  } catch (error) {
    rootStore.dispatch({
      type: "GET_GRID_SHARED_LOOKUP",
      payload: { LookUpGridResult: result, filter: null } as LookUpGrid,
    });
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
  }
}

export async function GetHistoricalDropDown() {
  try {
    setLoader("ADD", "GetHistoricalDropDown");
    let api = new ReportSoftwareApi();
    let result = await ApiCallWithErrorHandling<
      Promise<QueryResultDtoOfTipologicaGridDto>
    >(() => api.getHistoricalDropDownList());
    setLoader("REMOVE", "GetHistoricalDropDown");
    return result;
  } catch (error) {
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
  }
}
