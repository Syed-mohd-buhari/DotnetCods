import {
  ApiCallWithErrorHandling,
  FilterValueDto,
} from "../../../Business/Common/CommonBusiness";
import { DesignAspectApi } from "../../../Business/DesignAspectsBusiness";
import {
  GET_FILTER_DESIGN_ASPECT,
  GET_GRID_DESIGN_ASPECT,
  DesignAspectGrid,
  DesignAspectQueryObjectGrid,
  QueryResultDtoOfDesignAspectDtoGrid,
} from "../../../Model/DesignAspects";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";

export async function GetDesignAspectGrid(
  queryFilter?: DesignAspectQueryObjectGrid
) {
  let api = new DesignAspectApi();
  let result: QueryResultDtoOfDesignAspectDtoGrid | null | undefined;
  setLoader("ADD", "GetDesignAspectGrid");
  try {
    result = await ApiCallWithErrorHandling<
      Promise<QueryResultDtoOfDesignAspectDtoGrid>
    >(() => api.designAspectGetDesignAspect(queryFilter ?? {}));
    let rtn = {
      DesignAspectGridResult: result,
      filter: null,
    } as DesignAspectGrid;
    rootStore.dispatch({ type: GET_GRID_DESIGN_ASPECT, payload: rtn });
  } catch (error) {
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
    rootStore.dispatch({
      type: GET_GRID_DESIGN_ASPECT,
      payload: {
        DesignAspectGridResult: null,
        filter: null,
      } as DesignAspectGrid,
    });
  }
  setLoader("REMOVE", "GetDesignAspectGrid");
}

export async function GetFilterColumDesignAspect(
  columName: string,
  columValue: string,
  queryFilter?: DesignAspectQueryObjectGrid
) {
  let result: FilterValueDto[] | undefined;
  let api = new DesignAspectApi();

  result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
    api.designAspectGetFilterResult(
      {
        ...queryFilter,
        archived:
          sessionStorage.getItem("isArchivedMode") &&
          sessionStorage.getItem("archivedType") === "DA"
            ? true
            : false,
      } ?? {},
      columName,
      columValue
    )
  );
  let rtn = {
    filter: result,
    DesignAspectGridResult: null,
  } as DesignAspectGrid;
  rootStore.dispatch({ type: GET_FILTER_DESIGN_ASPECT, payload: rtn });
  // setLoader("REMOVE", "GetFilterColumDesignAspect");
}
