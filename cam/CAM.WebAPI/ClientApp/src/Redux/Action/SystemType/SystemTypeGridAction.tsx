import {
  ApiCallWithErrorHandling,
  FilterValueDto,
} from "../../../Business/Common/CommonBusiness";
import { SystemTypeApi } from "../../../Business/SystemTypeBusiness";
import {
  GET_FILTER_SYSTEM_TYPE,
  GET_GRID_SYSTEM_TYPE,
  QueryResultDtoOfSystemTypeDtoGrid,
  SystemTypeGrid,
  SystemTypeQueryObjectGrid,
} from "../../../Model/SystemTypeModel";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";

export async function GetSystemTypeGrid(
  queryFilter?: SystemTypeQueryObjectGrid,
  returnValues?: boolean
) {
  setLoader("ADD", "GetSystemTypeGrid");

  let result: QueryResultDtoOfSystemTypeDtoGrid | null | undefined;
  let api = new SystemTypeApi();
  try {
    result = await ApiCallWithErrorHandling<
      Promise<QueryResultDtoOfSystemTypeDtoGrid>
    >(() => api.systemTypeGetSystemType(queryFilter ?? {}));

    if (returnValues != true) {
      rootStore.dispatch({
        type: GET_GRID_SYSTEM_TYPE,
        payload: {
          SystemTypeGridResult: result,
          filter: null,
        } as SystemTypeGrid,
      });
    } else {
      setLoader("REMOVE", "GetSystemTypeGrid");

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
      type: GET_GRID_SYSTEM_TYPE,
      payload: { SystemTypeGridResult: null, filter: null } as SystemTypeGrid,
    });
  }
  setLoader("REMOVE", "GetSystemTypeGrid");
}

export async function GetFilterColumSystemType(
  columName: string,
  columValue: string,
  queryFilter?: SystemTypeQueryObjectGrid
) {
  let api = new SystemTypeApi();
  let result: FilterValueDto[] | undefined;
  result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
    api.systemTypeGetFilterResult(queryFilter ?? {}, columName, columValue)
  );
  let rtn = { filter: result, SystemTypeGridResult: null } as SystemTypeGrid;
  rootStore.dispatch({ type: GET_FILTER_SYSTEM_TYPE, payload: rtn });

  return rtn;
}
