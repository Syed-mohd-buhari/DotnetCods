import {
  ApiCallWithErrorHandling,
  FilterValueDto,
} from "../../../Business/Common/CommonBusiness";
import { MajorSoftwareBuildApi } from "../../../Business/MajorSoftwareBuildsBusiness";
import {
  GET_FILTER_MAJOR_SOFTWARE_BUILD,
  GET_GRID_MAJOR_SOFTWARE_BUILD,
  GET_GRID_MAJOR_SOFTWARE_BUILD_PRODUCT_BASED,
  MajorSoftwareBuildGrid,
  MajorSoftwareBuildProductBasedQueryObjectGrid,
  MajorSoftwareBuildQueryObjectGrid,
  QueryResultDtoOfMajorSoftwareBuildDtoGrid,
} from "../../../Model/MajorSoftwareBuild";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";

export async function GetMajorSoftwareBuildGrid(
  queryFilter?: MajorSoftwareBuildQueryObjectGrid
) {
  // setLoader("ADD", "GetMajorSoftwareBuildGrid");
  let result: QueryResultDtoOfMajorSoftwareBuildDtoGrid | null | undefined;
  let api = new MajorSoftwareBuildApi();
  try {
    result = await ApiCallWithErrorHandling<
      Promise<QueryResultDtoOfMajorSoftwareBuildDtoGrid>
    >(() => api.majorSoftwareBuildGetMajorSoftwareBuild(queryFilter ?? {}));

    let rtn = {
      MajorSoftwareBuildGridResult: result,
      filter: null,
    } as MajorSoftwareBuildGrid;
    rootStore.dispatch({ type: GET_GRID_MAJOR_SOFTWARE_BUILD, payload: rtn });
  } catch (error) {
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
    rootStore.dispatch({
      type: GET_GRID_MAJOR_SOFTWARE_BUILD,
      payload: {
        MajorSoftwareBuildGridResult: null,
        filter: null,
      } as MajorSoftwareBuildGrid,
    });
  }
  // setLoader("REMOVE", "GetMajorSoftwareBuildGrid");
}

export async function GetMajorSoftwareBuildProductBasedGrid(
  queryFilter?: MajorSoftwareBuildProductBasedQueryObjectGrid
) {
  let result: any;
  let api = new MajorSoftwareBuildApi();
  result = await ApiCallWithErrorHandling<Promise<any>>(() =>
    api.majorSoftwareBuildProductBasedGrid(queryFilter ?? {})
  );

  return result.data;
}

export async function GetMajorSoftwareBuildGridOnly(
  queryFilter?: MajorSoftwareBuildQueryObjectGrid
) {
  let result: QueryResultDtoOfMajorSoftwareBuildDtoGrid | null | undefined;
  let api = new MajorSoftwareBuildApi();

  result = await ApiCallWithErrorHandling<
    Promise<QueryResultDtoOfMajorSoftwareBuildDtoGrid>
  >(() => api.majorSoftwareBuildGetMajorSoftwareBuild(queryFilter ?? {}));
  return result;
}

export async function GetVersionMajorSoftwareBuildGrid(
  queryFilter?: MajorSoftwareBuildQueryObjectGrid
) {
  let result: QueryResultDtoOfMajorSoftwareBuildDtoGrid | null | undefined;
  let api = new MajorSoftwareBuildApi();
  result = await ApiCallWithErrorHandling<
    Promise<QueryResultDtoOfMajorSoftwareBuildDtoGrid>
  >(() => api.majorSoftwareBuildGetMajorSoftwareBuild(queryFilter ?? {}));

  return result;
}

export async function GetFilterColumMajorSoftwareBuild(
  columName: string,
  columValue: string,
  queryFilter?: MajorSoftwareBuildQueryObjectGrid
) {
  let result: FilterValueDto[] | undefined;
  let api = new MajorSoftwareBuildApi();
  result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
    api.majorSoftwareBuildGetFilterResult(
      queryFilter ?? {},
      columName,
      columValue
    )
  );
  let rtn = {
    filter: result,
    MajorSoftwareBuildGridResult: null,
  } as MajorSoftwareBuildGrid;
  rootStore.dispatch({ type: GET_FILTER_MAJOR_SOFTWARE_BUILD, payload: rtn });

  return rtn;
}
