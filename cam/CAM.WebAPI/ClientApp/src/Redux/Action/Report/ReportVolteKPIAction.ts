import {
  ApiCallWithErrorHandling,
  FilterValueDto,
} from "../../../Business/Common/CommonBusiness";
import { VolteKPIApi } from "../../../Business/VolteKPIBusiness";
import {
  VolteKPIDtoCreate,
  VolteKPIQueryObjectGrid,
  VolteKPIType,
} from "../../../Model/VolteKpi/VolteKPI";
import {
  VolteKPIReportColumn,
  ReportVolteKPIGrid,
  VolteKPIReportDto,
  VolteKPIReportRow,
  GET_GRID_REPORT_VOLTE_KPI,
} from "../../../Model/Report/ReportVolteKPIModel";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import { setNotification } from "../NotificationAction";
import setLoader from "../LoaderAction";
// import { useDispatch } from 'react-redux'

export async function GetReportVolteKPIGrid(
  queryFilter?: VolteKPIQueryObjectGrid
) {
  setLoader("ADD", "GetReportVolteKPIGrid");
  let result: VolteKPIReportDto | null | undefined;
  let api = new VolteKPIApi();

  try {
    result = await ApiCallWithErrorHandling<Promise<VolteKPIReportDto>>(() =>
      api.volteKPIGetDashboardReports(queryFilter ?? {})
    );

    let rtn = {
      ReportVolteKPIGridResult: result,
      filter: null,
    } as ReportVolteKPIGrid;
    rootStore.dispatch({ type: GET_GRID_REPORT_VOLTE_KPI, payload: rtn });
  } catch (error) {
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
    var rtn = {
      ReportVolteKPIGridResult: null,
      filter: null,
    } as ReportVolteKPIGrid;
    rootStore.dispatch({ type: GET_GRID_REPORT_VOLTE_KPI, payload: rtn });
  }

  setLoader("REMOVE", "GetReportVolteKPIGrid");
}
