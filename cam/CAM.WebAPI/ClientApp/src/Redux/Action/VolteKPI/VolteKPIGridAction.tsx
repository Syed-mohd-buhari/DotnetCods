import React from "react";
import {
  ApiCallWithErrorHandling,
  FilterValueDto,
} from "../../../Business/Common/CommonBusiness";
import { VolteKPIApi } from "../../../Business/VolteKPIBusiness";
import {
  VolteKPIGrid,
  VolteKPIQueryObjectGrid,
  GET_FILTER_VOLTE_KPI,
  GET_GRID_VOLTE_KPI,
  QueryResultDtoOfVolteKPIDtoGrid,
} from "../../../Model/VolteKpi/VolteKPI";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function GetVolteKPIGrid(queryFilter?: VolteKPIQueryObjectGrid) {
  setLoader("ADD", "GetVolteKPIGrid");

  let result: QueryResultDtoOfVolteKPIDtoGrid | null | undefined;
  let api = new VolteKPIApi();

  try {
    result = await ApiCallWithErrorHandling<
      Promise<QueryResultDtoOfVolteKPIDtoGrid>
    >(() => api.volteKPIGetDashboard(queryFilter ?? {}));
    let rtn = { VolteKPIGridResult: result, filter: null } as VolteKPIGrid;
    rootStore.dispatch({
      type: GET_GRID_VOLTE_KPI,
      payload: rtn as VolteKPIGrid,
    });
  } catch (error) {
    rootStore.dispatch({
      type: GET_GRID_VOLTE_KPI,
      payload: { VolteKPIGridResult: result, filter: null } as VolteKPIGrid,
    });
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
  }
  setLoader("REMOVE", "GetVolteKPIGrid");
}

export async function GetFilterColumVolteKPI(
  columName: string,
  columValue: string,
  queryFilter?: VolteKPIQueryObjectGrid
) {
  // setLoader("ADD", "GetFilterColumVolteKPI");

  let result: FilterValueDto[] | undefined;
  let api = new VolteKPIApi();

  result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
    api.volteKPIGetFilterResult(queryFilter ?? {}, columName, columValue)
  );
  let rtn = { filter: result, VolteKPIGridResult: null } as VolteKPIGrid;
  rootStore.dispatch({ type: GET_FILTER_VOLTE_KPI, payload: rtn });
  // setLoader("REMOVE", "GetFilterColumVolteKPI");
}
