import {
  ApiCallWithErrorHandling,
  FilterValueDto,
} from "../../../Business/Common/CommonBusiness";
import { ComponentSwBuildApi } from "../../../Business/ComponentSwBuildsBusiness";
import {
  GET_FILTER_COMPONENT_SW_BUILD,
  GET_GRID_COMPONENT_SW_BUILD,
  ComponentSwBuildGrid,
  ComponentSwBuildQueryObjectGrid,
  QueryResultDtoOfComponentSwBuildDtoGrid,
} from "../../../Model/ComponentSwBuild";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";

export async function GetComponentSwBuildGrid(
  queryFilter?: ComponentSwBuildQueryObjectGrid
) {
  setLoader("ADD", "GetComponentSwBuildGrid");
  let result: QueryResultDtoOfComponentSwBuildDtoGrid | null | undefined;
  let api = new ComponentSwBuildApi();
  try {
    result = await ApiCallWithErrorHandling<
      Promise<QueryResultDtoOfComponentSwBuildDtoGrid>
    >(() => api.componentSwBuildGetComponentSwBuild(queryFilter ?? {}));

    let rtn = {
      ComponentSwBuildGridResult: result,
      filter: null,
    } as ComponentSwBuildGrid;
    rootStore.dispatch({ type: GET_GRID_COMPONENT_SW_BUILD, payload: rtn });
  } catch (error) {
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
    rootStore.dispatch({
      type: GET_GRID_COMPONENT_SW_BUILD,
      payload: {
        ComponentSwBuildGridResult: null,
        filter: null,
      } as ComponentSwBuildGrid,
    });
  }
  setLoader("REMOVE", "GetComponentSwBuildGrid");
}

export async function GetFilterColumComponentSwBuild(
  columName: string,
  columValue: string,
  queryFilter?: ComponentSwBuildQueryObjectGrid
) {
  let result: FilterValueDto[] | undefined;
  let api = new ComponentSwBuildApi();
  result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
    api.componentSwBuildGetFilterResult(
      queryFilter ?? {},
      columName,
      columValue
    )
  );
  let rtn = {
    filter: result,
    ComponentSwBuildGridResult: null,
  } as ComponentSwBuildGrid;
  rootStore.dispatch({ type: GET_FILTER_COMPONENT_SW_BUILD, payload: rtn });

  return rtn;
}
