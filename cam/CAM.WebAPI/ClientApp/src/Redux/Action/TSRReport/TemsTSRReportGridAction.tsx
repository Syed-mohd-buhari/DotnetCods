import {
  ApiCallWithErrorHandling,
  FilterValueDto,
} from "../../../Business/Common/CommonBusiness";
import { TSRReportApi } from "../../../Business/TSRReportBusiness";
import {
  LookUpGrid,
  QueryResultDtoOfTipologicaGridDto,
  TipologicaGridDto,
  TipologicheQueryObjectGrid,
} from "../../../Model/LookUp/LookUpGenericModel";
import {
  GET_FILTER_TEMS_TSR_REPORT,
  GET_GRID_TEMS_TSR_REPORT,
  GET_FILTER_NON_TEMS_TSR_REPORT,
  GET_GRID_NON_TEMS_TSR_REPORT,
  QueryResultDtoOfTSRReportDtoGrid,
  TemsTSRReportGrid,
  NonTemsTSRReportGrid,
  TSRReportQueryObjectGrid,
  TSRReportVerticalQueryObjectGrid,
  GET_GRID_VERTICAL_TEMS_TSR_REPORT,
} from "../../../Model/TSRReport";

import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";

export async function GetTemsTSRReportGrid(
  queryFilter?: TSRReportQueryObjectGrid,
  returnValues?: boolean
) {
  setLoader("ADD", "GetTemsTSRReportGrid");

  let result: QueryResultDtoOfTSRReportDtoGrid | null | undefined;
  let api = new TSRReportApi();
  try {
    result = await ApiCallWithErrorHandling<
      Promise<QueryResultDtoOfTSRReportDtoGrid>
    >(() => api.tsrReportGetGrid(queryFilter ?? {}));

    if (returnValues != true) {
      rootStore.dispatch({
        type: GET_GRID_TEMS_TSR_REPORT,
        payload: {
          TemsTSRReportGridResult: result,
          filter: null,
        } as TemsTSRReportGrid,
      });
    } else {
      setLoader("REMOVE", "GetTemsTSRReportGrid");

      return result?.items;
    }
  } catch (error) {
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
    rootStore.dispatch({
      type: GET_GRID_TEMS_TSR_REPORT,
      payload: {
        TemsTSRReportGridResult: null,
        filter: null,
      } as TemsTSRReportGrid,
    });
  }
  setLoader("REMOVE", "GetTemsTSRReportGrid");
}

export async function GetNonTemsTSRReportGrid(
  queryFilter?: TSRReportQueryObjectGrid,
  returnValues?: boolean
) {
  setLoader("ADD", "GetNonTemsTSRReportGrid");

  let result: QueryResultDtoOfTSRReportDtoGrid | null | undefined;
  let api = new TSRReportApi();
  try {
    result = await ApiCallWithErrorHandling<
      Promise<QueryResultDtoOfTSRReportDtoGrid>
    >(() => api.tsrnontemsReportGetGrid(queryFilter ?? {}));

    if (returnValues != true) {
      rootStore.dispatch({
        type: GET_GRID_NON_TEMS_TSR_REPORT,
        payload: {
          NonTemsTSRReportGridResult: result,
          filter: null,
        } as NonTemsTSRReportGrid,
      });
    } else {
      setLoader("REMOVE", "GetNonTemsTSRReportGrid");

      return result?.items;
    }
  } catch (error) {
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
    rootStore.dispatch({
      type: GET_GRID_NON_TEMS_TSR_REPORT,
      payload: {
        NonTemsTSRReportGridResult: null,
        filter: null,
      } as NonTemsTSRReportGrid,
    });
  }
  setLoader("REMOVE", "GetNonTemsTSRReportGrid");
}

export async function GetTSRReportVerticalGrid(
  queryFilter?: TipologicheQueryObjectGrid,
  returnValues?: boolean
) {
  setLoader("ADD", "GetTSRReportVerticalGrid");

  let result: QueryResultDtoOfTipologicaGridDto | null | undefined;
  let api = new TSRReportApi();
  try {
    if (queryFilter !== null && queryFilter !== undefined) {
      result = await ApiCallWithErrorHandling<
        Promise<QueryResultDtoOfTipologicaGridDto>
      >(() => api.tsrReportGetVerticalGrid(queryFilter));
    } else {
      result = await ApiCallWithErrorHandling<
        Promise<QueryResultDtoOfTipologicaGridDto>
      >(() => api.tsrReportGetVerticalGrid({}));
    }
    // if (result?.items?.length === 0 || result?.totalItems === undefined) {
    //     rootStore.dispatch(setNotification({ message: "no results found", notifyType: NotifyType.warning }));
    // }
    let rtn = { LookUpGridResult: result, filter: null } as TipologicaGridDto;
    rootStore.dispatch({
      type: "GET_GRID_TSR_VERTICAL",
      payload: rtn as TipologicaGridDto,
    });
  } catch (error) {
    rootStore.dispatch({
      type: "GET_GRID_TSR_VERTICAL",
      payload: { LookUpGridResult: result, filter: null } as LookUpGrid,
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

export async function GetTSRReportVerticalDropdown(
  queryFilter?: TipologicheQueryObjectGrid,
  returnValues?: boolean
) {
  setLoader("ADD", "GetTSRReportVerticalDropdown");

  let result: { [key: string]: string } | null | undefined;
  let api = new TSRReportApi();
  try {
    result = await ApiCallWithErrorHandling<Promise<{ [key: string]: string }>>(
      () => api.tsrReportGetVerticalGrid(queryFilter)
    );

    let rtn = { LookUpGridResult: result, filter: null } as TipologicaGridDto;
    rootStore.dispatch({
      type: "GET_GRID_TSR_VERTICAL",
      payload: rtn as TipologicaGridDto,
    });
    setLoader("REMOVE", "GetTSRReportVerticalDropdown");

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
  setLoader("REMOVE", "GetTSRReportVerticalDropdown");
}

export async function GetFilterColumTSRReport(
  columName: string,
  columValue: string,
  queryFilter?: TSRReportQueryObjectGrid
) {
  let api = new TSRReportApi();
  let result: FilterValueDto[] | undefined;
  result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
    api.TSRReportGetFilterResult(queryFilter ?? {}, columName, columValue)
  );
  let rtn = {
    filter: result,
    TemsTSRReportGridResult: null,
  } as TemsTSRReportGrid;
  rootStore.dispatch({ type: GET_FILTER_TEMS_TSR_REPORT, payload: rtn });

  return rtn;
}

export async function GetFilterColumNonTemsTSRReport(
  columName: string,
  columValue: string,
  queryFilter?: TSRReportQueryObjectGrid
) {
  let api = new TSRReportApi();
  let result: FilterValueDto[] | undefined;
  result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
    api.TSRNonTemsReportGetFilterResult(
      queryFilter ?? {},
      columName,
      columValue
    )
  );
  let rtn = {
    filter: result,
    NonTemsTSRReportGridResult: null,
  } as NonTemsTSRReportGrid;
  rootStore.dispatch({ type: GET_FILTER_NON_TEMS_TSR_REPORT, payload: rtn });

  return rtn;
}
