import {
  ApiCallWithErrorHandling,
  FilterValueDto,
} from "../../../Business/Common/CommonBusiness";

import { BPTReportApi } from "../../../Business/BPTReportBuisnes";
import {
  LookUpGrid,
  QueryResultDtoOfTipologicaGridDto,
  TipologicaGridDto,
  TipologicheQueryObjectGrid,
} from "../../../Model/LookUp/LookUpGenericModel";
import {
  GET_FILTER_BPT_REPORT,
  GET_GRID_BPT_REPORT,
  QueryResultDtoOfBPTReportDtoGrid,
  BPTReportGrid,
  BPTReportQueryObjectGrid,
} from "../../../Model/BPTReport";

import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";

export async function GetBPTReportGrid(queryFilter?: BPTReportQueryObjectGrid) {
  setLoader("ADD", "GetBPTReportGrid");
  let result: QueryResultDtoOfBPTReportDtoGrid | null | undefined;
  let api = new BPTReportApi();
  try {
    result = await ApiCallWithErrorHandling<
      Promise<QueryResultDtoOfBPTReportDtoGrid>
    >(() => api.BPTReportGetGrid(queryFilter ?? {}));

    let rtn = {
      BPTReportGridResult: result,
      filter: null,
    } as BPTReportGrid;
    rootStore.dispatch({
      type: GET_GRID_BPT_REPORT,
      payload: rtn,
    });

    return result;
  } catch (error) {
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
    rootStore.dispatch({
      type: GET_GRID_BPT_REPORT,
      payload: {
        BPTReportGridResult: null,
        filter: null,
      } as BPTReportGrid,
    });
  }
  setLoader("REMOVE", "GetBPTReportGrid");
}

export async function GetFilterColumBPTReport(
  columName: string,
  columValue: string,
  queryFilter?: BPTReportQueryObjectGrid
) {
  let api = new BPTReportApi();
  let result: FilterValueDto[] | undefined;
  result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
    api.BPTReportGetFilterResult(queryFilter ?? {}, columName, columValue)
  );
  let rtn = {
    filter: result,
    BPTReportGridResult: null,
  } as BPTReportGrid;
  rootStore.dispatch({ type: GET_FILTER_BPT_REPORT, payload: rtn });

  return rtn;
}
