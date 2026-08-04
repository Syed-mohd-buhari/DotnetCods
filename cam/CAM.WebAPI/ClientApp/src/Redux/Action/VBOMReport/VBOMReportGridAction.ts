import {
  ApiCallWithErrorHandling,
  FilterValueDto,
} from "../../../Business/Common/CommonBusiness";
import { VBOMReportApi } from "../../../Business/VBOMReportBusiness";
import {
  GET_GRID_VBOM_REPORT,
  QueryResultDtoOfVBOMReportDtoGrid,
  VBOMReportGrid,
} from "../../../Model/VBOMReport";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";

export async function GetVBOMReportGrid(
  queryFilter?: any,
  returnValues?: boolean
) {
  setLoader("ADD", "GetVBOMReportGrid");

  let result: QueryResultDtoOfVBOMReportDtoGrid | null | undefined;
  let api = new VBOMReportApi();
  try {
    result = await ApiCallWithErrorHandling<
      Promise<QueryResultDtoOfVBOMReportDtoGrid>
    >(() => api.GetVBOMReportGrid(queryFilter ?? {}));

    if (returnValues != true) {
      rootStore.dispatch({
        type: GET_GRID_VBOM_REPORT,
        payload: {
          VBOMReportGridResult: result,
          filter: null,
        } as VBOMReportGrid,
      });
    } else {
      setLoader("REMOVE", "GetVBOMReportGrid");

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
      type: GET_GRID_VBOM_REPORT,
      payload: {
        VBOMReportGridResult: null,
        filter: null,
      } as VBOMReportGrid,
    });
  }
  setLoader("REMOVE", "GetVBOMReportGrid");
}

export async function GetVBOMReportGetAllResource() {
  let api = new VBOMReportApi();
  let result: FilterValueDto[] | undefined;
  result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
    api.VBOMReportGetAllResource()
  );
  return result;
}
