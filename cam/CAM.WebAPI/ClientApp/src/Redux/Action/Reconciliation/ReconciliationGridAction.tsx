import {
  ApiCallWithErrorHandling,
  FilterValueDto,
} from "../../../Business/Common/CommonBusiness";
import { ReconciliationApi } from "../../../Business/ReconciliationBusiness";
import { ResultDto } from "../../../Model/CommonModels";
import {
  GET_FILTER_RECONCILIATION,
  GET_GRID_RECONCILIATION,
  ReconciliationGrid,
  ReconciliationQueryObjectGrid,
  QueryResultDtoOfReconciliationDtoGrid,
} from "../../../Model/Reconciliation";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";

export async function GetReconciliationGrid(
  queryFilter?: ReconciliationQueryObjectGrid
) {
  setLoader("ADD", "GetReconciliationGrid");

  let api = new ReconciliationApi();

  let result: QueryResultDtoOfReconciliationDtoGrid | null | undefined;
  try {
    result = await ApiCallWithErrorHandling<
      Promise<QueryResultDtoOfReconciliationDtoGrid>
    >(() => api.ReconciliationGetReconciliation(queryFilter ?? {}));

    let rtn = {
      ReconciliationGridResult: result,
      filter: null,
    } as ReconciliationGrid;

    rootStore.dispatch({ type: GET_GRID_RECONCILIATION, payload: rtn });
  } catch (error) {
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
    rootStore.dispatch({
      type: GET_GRID_RECONCILIATION,
      payload: {
        ReconciliationGridResult: null,
        filter: null,
      } as ReconciliationGrid,
    });
  }
  setLoader("REMOVE", "GetReconciliationGrid");
}

export async function GetFilterColumReconciliation(
  columName: string,
  columValue: string,
  queryFilter?: ReconciliationQueryObjectGrid
) {
  let result: FilterValueDto[] | undefined;
  let api = new ReconciliationApi();
  result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
    api.ReconciliationGetFilterResult(queryFilter ?? {}, columName, columValue)
  );
  let rtn = {
    filter: result,
    ReconciliationGridResult: null,
  } as ReconciliationGrid;
  rootStore.dispatch({ type: GET_FILTER_RECONCILIATION, payload: rtn });
}

export async function UpdateReconciliationStatus(reconciliationId: number) {
  let result: ResultDto | undefined;
  let api = new ReconciliationApi();
  result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.UpdateReconciliationStatus(reconciliationId)
  );
  let rtn = {
    filter: result,
    ReconciliationGridResult: null,
  } as ReconciliationGrid;
  rootStore.dispatch({ type: GET_FILTER_RECONCILIATION, payload: rtn });
}
