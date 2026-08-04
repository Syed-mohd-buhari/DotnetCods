import {
  ApiCallWithErrorHandling,
  FilterValueDto,
} from "../../../Business/Common/CommonBusiness";
import {
  GET_AUDIT_TRAILS,
  AuditTrailsApi,
} from "../../../Business/AuditTrailsBusiness";
import { ReturnFile, FileResult } from "../../../Model/Common";
import { ResultDto } from "../../../Model/CommonModels";
import {
  GET_GRID_AUDIT_TRAILS,
  AuditTrailsGrid,
  AuditTrailsQueryObjectGrid,
  QueryResultDtoOfAuditTrailsDtoGrid,
  GET_FILTER_AUDIT_TRAILS,
} from "../../../Model/AuditTrails";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";

export async function GetAuditTrailsGrid(
  queryFilter?: AuditTrailsQueryObjectGrid
) {
  setLoader("ADD", "GetAuditTrailsGrid");
  let api = new AuditTrailsApi();

  let result: QueryResultDtoOfAuditTrailsDtoGrid | null | undefined;
  try {
    result = await ApiCallWithErrorHandling<
      Promise<QueryResultDtoOfAuditTrailsDtoGrid>
    >(() => api.getAuditTrailsGrid(queryFilter ?? {}));

    let rtn = {
      AuditTrailsGridResult: result,
      filter: null,
    } as unknown as AuditTrailsGrid;

    rootStore.dispatch({ type: GET_GRID_AUDIT_TRAILS, payload: rtn });
  } catch (error) {
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
    rootStore.dispatch({
      type: GET_GRID_AUDIT_TRAILS,
      payload: {
        AuditTrailsGridResult: null,
        filter: null,
      } as unknown as AuditTrailsGrid,
    });
  }
  setLoader("REMOVE", "GetAuditTrailsGrid");
}

export async function GetFilterColumAuditTrails(
  columName: string,
  columValue: string,
  queryFilter?: AuditTrailsQueryObjectGrid
) {
  let result: FilterValueDto[] | undefined;
  let api = new AuditTrailsApi();
  result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
    api.AuditTrailsGetFilterResult(queryFilter ?? {}, columName, columValue)
  );
  let rtn = {
    filter: result,
    AuditTrailsGridResult: null,
  } as unknown as AuditTrailsGrid;
  rootStore.dispatch({ type: GET_FILTER_AUDIT_TRAILS, payload: rtn });
}

export async function DownloadAuditTrails(data: any) {
  setLoader("ADD", "DownloadAuditTrails");
  let api = new AuditTrailsApi();
  let res: ReturnFile | undefined;
  try {
    res = await ApiCallWithErrorHandling<Promise<ReturnFile>>(() =>
      api.DownloadAuditTrails(data)
    );
    let fileNameBase = res?.FileName.split(";")[1] ?? "";
    var index = fileNameBase?.indexOf('"') + 1;
    var lastIndex = fileNameBase?.indexOf('"', index);
    let fileName = fileNameBase?.substring(index, lastIndex);
    let result = res?.File.then((x) => {
      return { file: x, fileName: fileName } as FileResult;
    });
    setLoader("REMOVE", "DownloadAuditTrails");

    return result;
  } catch (error) {
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
  }
  setLoader("REMOVE", "DownloadAuditTrails");
}
