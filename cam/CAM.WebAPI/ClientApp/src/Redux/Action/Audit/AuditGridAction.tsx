import { AuditApi } from "../../../Business/AuditBusiness";
import {
  ApiCallWithErrorHandling,
  FilterValueDto,
} from "../../../Business/Common/CommonBusiness";
import {
  GET_FILTER_NETWORK_ELEMENT_AS_IS,
  GET_GRID_AUDIT,
  NetworkElementAsIsGrid,
  AuditGrid,
  AuditQueryObjectGrid,
  QueryResultDtoOfAuditDtoGrid,
} from "../../../Model/Audit";
import { ResultDto } from "../../../Model/CommonModels";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";

export async function GetAuditGrid(queryFilter?: AuditQueryObjectGrid) {
  setLoader("ADD", "GetAuditGrid");
  let api = new AuditApi();

  let result: QueryResultDtoOfAuditDtoGrid | null | undefined;
  try {
    result = await ApiCallWithErrorHandling<
      Promise<QueryResultDtoOfAuditDtoGrid>
    >(() => api.auditGetAudit(queryFilter ?? {}));

    let rtn = { AuditGridResult: result, filter: null } as AuditGrid;

    rootStore.dispatch({ type: GET_GRID_AUDIT, payload: rtn });
  } catch (error) {
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
    rootStore.dispatch({
      type: GET_GRID_AUDIT,
      payload: { AuditGridResult: null, filter: null } as AuditGrid,
    });
  }
  setLoader("REMOVE", "GetAuditGrid");
}

export async function GetAuditApproveStatus(data) {
  setLoader("ADD", "GetAuditApproveStatus");
  let api = new AuditApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.networkElementAsIsGetApproveStatus(data)
  );
  let rtn = {
    data: result?.data,
    info: result?.info,
    warning: result?.warning,
  } as ResultDto;
  rootStore.dispatch(
    setNotification({
      message: result?.info ?? "Failed to Submit",
      notifyType:
        result !== undefined && result.warning === true
          ? NotifyType.success
          : NotifyType.error,
    })
  );
  setLoader("REMOVE", "GetAuditApproveStatus");
  return rtn;
}

export async function GetAuditRejectStatus(data) {
  setLoader("ADD", "GetAuditRejectStatus");
  let api = new AuditApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.networkElementAsIsGetRejectStatus(data)
  );
  let rtn = {
    data: result?.data,
    info: result?.info,
    warning: result?.warning,
  } as ResultDto;
  rootStore.dispatch(
    setNotification({
      message: result?.info ?? "Failed to Submit",
      notifyType:
        result !== undefined && result.warning === true
          ? NotifyType.success
          : NotifyType.error,
    })
  );
  setLoader("REMOVE", "GetAuditRejectStatus");
  return rtn;
}
export async function GetAuditOverrideStatus(data) {
  setLoader("ADD", "GetAuditOverrideStatus");
  let api = new AuditApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.networkElementAsIsGetOverrideStatus(data)
  );
  let rtn = {
    data: result?.data,
    info: result?.info,
    warning: result?.warning,
  } as ResultDto;
  rootStore.dispatch(
    setNotification({
      message: result?.info ?? "Failed to Submit",
      notifyType:
        result !== undefined && result.warning === true
          ? NotifyType.success
          : NotifyType.error,
    })
  );
  setLoader("REMOVE", "GetAuditOverrideStatus");
  return rtn;
}

export async function GetFilterColumAudit(
  columName: string,
  columValue: string,
  queryFilter?: AuditQueryObjectGrid
) {
  let result: FilterValueDto[] | undefined;
  let api = new AuditApi();
  result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
    api.auditGetFilterResult(queryFilter ?? {}, columName, columValue)
  );
  let rtn = {
    filter: result,
    NetworkElementAsIsGridResult: null,
  } as NetworkElementAsIsGrid;
  rootStore.dispatch({ type: GET_FILTER_NETWORK_ELEMENT_AS_IS, payload: rtn });
}
