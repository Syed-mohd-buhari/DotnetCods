import {
  ApiCallWithErrorHandling,
  FilterValueDto,
} from "../../../Business/Common/CommonBusiness";
import { MajorHardwareBuildApi } from "../../../Business/MajorHardwareBuildBusiness";
import {
  GET_FILTER_MAJOR_HARDWARE_BUILD,
  GET_GRID_MAJOR_HARDWARE_BUILD,
  MajorHardwareBuildDtoGrid,
  MajorHardwareBuildGrid,
  MajorHardwareBuildQueryObjectGrid,
  QueryResultDtoOfMajorHardwareBuildDtoGrid,
} from "../../../Model/MajorHardwareBuild";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";

export async function GetMajorHardwareBuildGrid(
  queryFilter?: MajorHardwareBuildQueryObjectGrid,
  returnValues?: boolean,
  hideLoader?: boolean
) {
  if (hideLoader !== true) {
    setLoader("ADD", "GetMajorHardwareBuildGrid");
  }
  let api = new MajorHardwareBuildApi();
  let result: QueryResultDtoOfMajorHardwareBuildDtoGrid | null | undefined;
  try {
    result = await ApiCallWithErrorHandling<
      Promise<QueryResultDtoOfMajorHardwareBuildDtoGrid>
    >(() => api.majorHardwareBuildGetMajorHardwareBuild(queryFilter ?? {}));

    let rtn = {
      MajorHardwareBuildGridResult: result,
      filter: null,
    } as MajorHardwareBuildGrid;
    if (returnValues !== true) {
      rootStore.dispatch({ type: GET_GRID_MAJOR_HARDWARE_BUILD, payload: rtn });
    } else {
      setLoader("REMOVE", "GetMajorHardwareBuildGrid");
      return rtn.MajorHardwareBuildGridResult
        ?.items as MajorHardwareBuildDtoGrid[];
    }
  } catch (error) {
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
    rootStore.dispatch({
      type: GET_GRID_MAJOR_HARDWARE_BUILD,
      payload: {
        MajorHardwareBuildGridResult: null,
        filter: null,
      } as MajorHardwareBuildGrid,
    });
  }
  setLoader("REMOVE", "GetMajorHardwareBuildGrid");
}

export async function GetMajorHardwareBuildGridOnly(
  queryFilter?: MajorHardwareBuildQueryObjectGrid,
  returnValues?: boolean,
  hideLoader?: boolean
) {
  if (hideLoader !== true) {
    setLoader("ADD", "GetMajorHardwareBuildGridOnly");
  }
  let api = new MajorHardwareBuildApi();
  let result: QueryResultDtoOfMajorHardwareBuildDtoGrid | null | undefined;

  result = await ApiCallWithErrorHandling<
    Promise<QueryResultDtoOfMajorHardwareBuildDtoGrid>
  >(() => api.majorHardwareBuildGetMajorHardwareBuild(queryFilter ?? {}));

  setLoader("REMOVE", "GetMajorHardwareBuildGridOnly");
  return result;
}

export async function GetFilterColumMajorHardwareBuild(
  columName: string,
  columValue: string,
  queryFilter?: MajorHardwareBuildQueryObjectGrid
) {
  let result: FilterValueDto[] | undefined;
  let api = new MajorHardwareBuildApi();
  result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
    api.majorHardwareBuildGetFilterResult(
      queryFilter ?? {},
      columName,
      columValue
    )
  );
  let rtn = {
    filter: result,
    MajorHardwareBuildGridResult: null,
  } as MajorHardwareBuildGrid;
  rootStore.dispatch({ type: GET_FILTER_MAJOR_HARDWARE_BUILD, payload: rtn });
}
