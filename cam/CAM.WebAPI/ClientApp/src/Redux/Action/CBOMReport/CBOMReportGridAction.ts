import {
  ApiCallWithErrorHandling,
  FilterValueDto,
} from "../../../Business/Common/CommonBusiness";
import { CBOMReportApi } from "../../../Business/CBOMReportBusiness";
import {
  GET_GRID_CBOM_REPORT,
  QueryResultDtoOfCBOMReportDtoGrid,
  CBOMReportGrid,
} from "../../../Model/CBOMReport";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";

export async function GetCBOMReportGrid(
  queryFilter?: any,
  returnValues?: boolean
) {
  setLoader("ADD", "GetCBOMReportGrid");

  let result: QueryResultDtoOfCBOMReportDtoGrid | null | undefined;
  let api = new CBOMReportApi();
  try {
    result = await ApiCallWithErrorHandling<
      Promise<QueryResultDtoOfCBOMReportDtoGrid>
    >(() => api.GetCBOMReportGrid(queryFilter ?? {}));

    if (returnValues != true) {
      rootStore.dispatch({
        type: GET_GRID_CBOM_REPORT,
        payload: {
          CBOMReportGridResult: result,
          filter: null,
        } as CBOMReportGrid,
      });
    } else {
      setLoader("REMOVE", "GetCBOMReportGrid");

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
      type: GET_GRID_CBOM_REPORT,
      payload: {
        CBOMReportGridResult: null,
        filter: null,
      } as CBOMReportGrid,
    });
  }
  setLoader("REMOVE", "GetCBOMReportGrid");
}

export async function GetCBOMReportGetAllResource() {
  let api = new CBOMReportApi();
  let result: FilterValueDto[] | undefined;
  result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
    api.CBOMReportGetAllResource()
  );
  return result;
}
