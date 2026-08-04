import React from "react";
import {
  ApiCallWithErrorHandling,
  FilterValueDto,
} from "../../../Business/Common/CommonBusiness";
import { PassThroughReportApi } from "../../../Business/Report/PassThroughReportBusiness";
import {
  PassThroughReportDtoGrid,
  PassThroughReportGrid,
  PassThroughReportQueryObjectGrid,
  QueryResultDtoOfPassThroughReportDtoGrid,
  GET_GRID_PASSTHROUGH_REPORT,
  GET_FILTER_PASSTHROUGH_REPORT,
} from "../../../Model/Report/PassThroughReportExport";
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
import { TSRReportApi } from "../../../Business/TSRReportBusiness";
import { ReportSoftwareApi } from "../../../Business/Report/ReportSoftwareBusiness";
import { RiTentLine } from "react-icons/ri";

export async function GetPassThroughReportGrid(
  queryFilter?: PassThroughReportQueryObjectGrid
) {
  setLoader("ADD", "GetPassThroughReportGrid");

  let result: QueryResultDtoOfPassThroughReportDtoGrid | null | undefined;
  let api = new PassThroughReportApi();
  try {
    result = await ApiCallWithErrorHandling<
      Promise<QueryResultDtoOfPassThroughReportDtoGrid>
    >(() => api.PassThroughReportGetReport(queryFilter ?? {}));

    let rtn = {
      PassThroughReportGridResult: result,
      filter: null,
    } as PassThroughReportGrid;
    rootStore.dispatch({ type: GET_GRID_PASSTHROUGH_REPORT, payload: rtn });
    // return rtn;
  } catch (error) {
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
    let rtn = {
      PassThroughReportGridResult: null,
      filter: null,
    } as PassThroughReportGrid;
    rootStore.dispatch({ type: GET_GRID_PASSTHROUGH_REPORT, payload: rtn });
  }
  setLoader("REMOVE", "GetPassThroughReportGrid");
}

export async function GetFilterColumPassThroughReport(
  columName: string,
  columValue: string,
  queryFilter?: PassThroughReportQueryObjectGrid
) {
  let api = new PassThroughReportApi();
  let result: FilterValueDto[] | undefined;
  result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
    api.PassThroughReportGetFilterResult(
      queryFilter ?? {},
      columName,
      columValue
    )
  );
  let rtn = {
    filter: result,
    PassThroughReportGridResult: null,
  } as PassThroughReportGrid;
  rootStore.dispatch({ type: GET_FILTER_PASSTHROUGH_REPORT, payload: rtn });
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

export async function GetTSRReportVerticalGrid(
  queryFilter?: TipologicheQueryObjectGrid,
  returnValues?: boolean
) {
  setLoader("ADD", "GetTSRReportVerticalGrid");

  let result: { [key: string]: string } | null | undefined;
  let api = new PassThroughReportApi();
  try {
    result = await ApiCallWithErrorHandling<Promise<{ [key: string]: string }>>(
      () => api.tsrReportGetVerticalGrid(queryFilter ?? {})
    );

    let rtn = { LookUpGridResult: result, filter: null } as TipologicaGridDto;
    rootStore.dispatch({
      type: "GET_GRID_TSR_VERTICAL",
      payload: rtn as TipologicaGridDto,
    });
    setLoader("REMOVE", "GetTSRReportVerticalGrid");

    return rtn;
  } catch (error) {
    rootStore.dispatch({
      type: "GET_GRID_TSR_VERTICAL",
      payload: { LookUpGridResult: null, filter: null } as LookUpGrid,
    });
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
  }
  setLoader("REMOVE", "GetTSRReportVerticalGrid");
}
