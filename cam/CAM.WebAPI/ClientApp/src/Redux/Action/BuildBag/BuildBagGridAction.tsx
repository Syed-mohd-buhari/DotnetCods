import {
  ApiCallWithErrorHandling,
  FilterValueDto,
} from "../../../Business/Common/CommonBusiness";
import { BuildBagApi } from "../../../Business/BuildBagsBusiness";
import {
  GET_FILTER_BUILD_BAG,
  GET_GRID_BUILD_BAG,
  BuildBagGrid,
  BuildBagQueryObjectGrid,
  QueryResultDtoOfBuildBagDtoGrid,
} from "../../../Model/BuildBag";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";

export async function GetBuildBagGrid(queryFilter?: BuildBagQueryObjectGrid) {
  setLoader("ADD", "GetBuildBagGrid");
  let result: QueryResultDtoOfBuildBagDtoGrid | null | undefined;
  let api = new BuildBagApi();
  try {
    result = await ApiCallWithErrorHandling<
      Promise<QueryResultDtoOfBuildBagDtoGrid>
    >(() => api.buildBagGetBuildBag(queryFilter ?? {}));

    let rtn = {
      BuildBagGridResult: result,
      filter: null,
    } as BuildBagGrid;
    rootStore.dispatch({ type: GET_GRID_BUILD_BAG, payload: rtn });
  } catch (error) {
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
    rootStore.dispatch({
      type: GET_GRID_BUILD_BAG,
      payload: {
        BuildBagGridResult: null,
        filter: null,
      } as BuildBagGrid,
    });
  }
  setLoader("REMOVE", "GetBuildBagGrid");
}

export async function GetFilterColumBuildBag(
  columName: string,
  columValue: string,
  queryFilter?: BuildBagQueryObjectGrid
) {
  let result: FilterValueDto[] | undefined;
  let api = new BuildBagApi();
  result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
    api.buildBagGetFilterResult(queryFilter ?? {}, columName, columValue)
  );
  let rtn = {
    filter: result,
    BuildBagGridResult: null,
  } as BuildBagGrid;
  rootStore.dispatch({ type: GET_FILTER_BUILD_BAG, payload: rtn });

  return rtn;
}
