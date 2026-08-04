import {
  ApiCallWithErrorHandling,
  FilterValueDto,
} from "../../../Business/Common/CommonBusiness";
import { TSRReportApi } from "../../../Business/TSRReportBusiness";
import {
  GET_FILTER_NON_TEMS_TSR_REPORT,
  GET_GRID_NON_TEMS_TSR_REPORT,
  QueryResultDtoOfTSRReportDtoGrid,
  NonTemsTSRReportGrid,
  TSRReportQueryObjectGrid,
} from "../../../Model/TSRReport";

import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";

export async function GetNonTEMSTSRReportGrid(
  queryFilter?: TSRReportQueryObjectGrid,
  returnValues?: boolean
) {
  setLoader("ADD", "GetNonTEMSTSRReportGrid");

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
      setLoader("REMOVE", "GetNonTEMSTSRReportGrid");

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
  setLoader("REMOVE", "GetNonTEMSTSRReportGrid");
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
    NonTemsTSRReportGridResult: null,
  } as NonTemsTSRReportGrid;
  rootStore.dispatch({ type: GET_FILTER_NON_TEMS_TSR_REPORT, payload: rtn });

  return rtn;
}
