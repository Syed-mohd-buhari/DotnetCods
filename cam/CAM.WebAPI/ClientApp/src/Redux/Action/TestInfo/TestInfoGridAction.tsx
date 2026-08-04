import {
  ApiCallWithErrorHandling,
  FilterValueDto,
} from "../../../Business/Common/CommonBusiness";
import { TestInfoApi } from "../../../Business/TestInfoBusiness";
import {
  GET_FILTER_TEST_INFO,
  GET_GRID_TEST_INFO,
  QueryResultDtoOfTestInfoDtoGrid,
  TestInfoGrid,
  TestInfoQueryObjectGrid,
} from "../../../Model/TestInfo";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";

export async function GetTestInfoGrid(
  queryFilter?: TestInfoQueryObjectGrid,
  returnValues?: boolean
) {
  setLoader("ADD", "GetTestInfoGrid");

  let result: QueryResultDtoOfTestInfoDtoGrid | null | undefined;
  let api = new TestInfoApi();
  try {
    result = await ApiCallWithErrorHandling<
      Promise<QueryResultDtoOfTestInfoDtoGrid>
    >(() => api.testInfoGetGrid(queryFilter ?? {}));

    if (returnValues != true) {
      rootStore.dispatch({
        type: GET_GRID_TEST_INFO,
        payload: {
          TestInfoGridResult: result,
          filter: null,
        } as TestInfoGrid,
      });
    } else {
      setLoader("REMOVE", "GetTestInfoGrid");

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
      type: GET_GRID_TEST_INFO,
      payload: { TestInfoGridResult: null, filter: null } as TestInfoGrid,
    });
  }
  setLoader("REMOVE", "GetTestInfoGrid");
}

export async function GetFilterColumTestInfo(
  columName: string,
  columValue: string,
  queryFilter?: TestInfoQueryObjectGrid
) {
  let api = new TestInfoApi();
  let result: FilterValueDto[] | undefined;
  result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
    api.testInfoGetFilterResult(queryFilter ?? {}, columName, columValue)
  );
  let rtn = { filter: result, TestInfoGridResult: null } as TestInfoGrid;
  rootStore.dispatch({ type: GET_FILTER_TEST_INFO, payload: rtn });

  return rtn;
}
