import {
  ApiCallWithErrorHandling,
  FilterValueDto,
} from "../../../Business/Common/CommonBusiness";
import { FNTReportApi } from "../../../Business/FNTReportBusiness";
import {
  LookUpGrid,
  QueryResultDtoOfTipologicaGridDto,
  TipologicaGridDto,
  TipologicheQueryObjectGrid,
} from "../../../Model/LookUp/LookUpGenericModel";
import {
  GET_FILTER_TEMS_FNT_REPORT,
  GET_GRID_TEMS_FNT_REPORT,
  GET_FILTER_NON_TEMS_FNT_REPORT,
  GET_GRID_NON_TEMS_FNT_REPORT,
  QueryResultDtoOfFNTReportDtoGrid,
  TemsFNTReportGrid,
  NonTemsFNTReportGrid,
  FNTReportQueryObjectGrid,
  FNTReportVerticalQueryObjectGrid,
  GET_GRID_VERTICAL_TEMS_FNT_REPORT,
} from "../../../Model/FNTReport";

import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";

export async function GetTemsFNTReportGrid(
  queryFilter?: FNTReportQueryObjectGrid,
  returnValues?: boolean
) {
  setLoader("ADD", "GetTemsFNTReportGrid");

  let result: QueryResultDtoOfFNTReportDtoGrid | null | undefined;
  let api = new FNTReportApi();
  try {
    result = await ApiCallWithErrorHandling<
      Promise<QueryResultDtoOfFNTReportDtoGrid>
    >(() => api.FNTReportGetGrid(queryFilter ?? {}));

    if (returnValues != true) {
      rootStore.dispatch({
        type: GET_GRID_TEMS_FNT_REPORT,
        payload: {
          TemsFNTReportGridResult: result,
          filter: null,
        } as TemsFNTReportGrid,
      });
    } else {
      setLoader("REMOVE", "GetTemsFNTReportGrid");

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
      type: GET_GRID_TEMS_FNT_REPORT,
      payload: {
        TemsFNTReportGridResult: null,
        filter: null,
      } as TemsFNTReportGrid,
    });
  }
  setLoader("REMOVE", "GetTemsFNTReportGrid");
}

export async function GetTSRReportVerticalDropdown(
  queryFilter?: TipologicheQueryObjectGrid,
  returnValues?: boolean
) {
  setLoader("ADD", "GetTSRReportVerticalDropdown");

  let result: { [key: string]: string } | null | undefined;
  let api = new FNTReportApi();
  try {
    result = await ApiCallWithErrorHandling<Promise<{ [key: string]: string }>>(
      () => api.tsrReportGetVerticalGrid(queryFilter ?? {})
    );

    let rtn = { LookUpGridResult: result, filter: null } as TipologicaGridDto;
    rootStore.dispatch({
      type: GET_GRID_VERTICAL_TEMS_FNT_REPORT,
      payload: rtn as TipologicaGridDto,
    });
    setLoader("REMOVE", "GetTSRReportVerticalDropdown");

    return rtn;
  } catch (error) {
    rootStore.dispatch({
      type: GET_GRID_VERTICAL_TEMS_FNT_REPORT,
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

export async function GetFilterColumFNTReport(
  columName: string,
  columValue: string,
  queryFilter?: FNTReportQueryObjectGrid
) {
  let api = new FNTReportApi();
  let result: FilterValueDto[] | undefined;
  result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
    api.FNTReportGetFilterResult(queryFilter ?? {}, columName, columValue)
  );
  let rtn = {
    filter: result,
    TemsFNTReportGridResult: null,
  } as TemsFNTReportGrid;
  rootStore.dispatch({ type: GET_FILTER_TEMS_FNT_REPORT, payload: rtn });

  return rtn;
}
