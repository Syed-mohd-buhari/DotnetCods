import {
  ApiCallWithErrorHandling,
  FilterValueDto,
} from "../../../Business/Common/CommonBusiness";
import { ResultDto } from "../../../Model/CommonModels";
import {
  DesignComponentFamilyGrid,
  DesignComponentFamilyQueryObjectGrid,
  GET_FILTER_DESIGN_COMPONENT_FAMILY,
  GET_GRID_DESIGN_COMPONENT_FAMILY,
  QueryResultDtoOfDesignComponentFamilyDtoGrid,
} from "../../../Model/DesignComponentFamily";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";
import { DesignComponentFamilyApi } from "../../../Business/DesignComponentFamilyBusiness";
// import { useDispatch } from 'react-redux'

export async function GetDesignComponentFamilyGrid(
  queryFilter?: DesignComponentFamilyQueryObjectGrid
) {
  let result: QueryResultDtoOfDesignComponentFamilyDtoGrid | null | undefined;
  let api = new DesignComponentFamilyApi();
  setLoader("ADD", "GetDesignComponentFamilyGrid");
  try {
    result = await ApiCallWithErrorHandling<
      Promise<QueryResultDtoOfDesignComponentFamilyDtoGrid>
    >(() =>
      api.designComponentFamilyGetDesignComponentFamily(queryFilter || {})
    );

    let rtn = {
      DesignComponentFamilyGridResult: result,
      filter: null,
    } as DesignComponentFamilyGrid;
    rootStore.dispatch({
      type: GET_GRID_DESIGN_COMPONENT_FAMILY,
      payload: rtn as DesignComponentFamilyGrid,
    });
  } catch (error) {
    rootStore.dispatch({
      type: GET_GRID_DESIGN_COMPONENT_FAMILY,
      payload: {
        DesignComponentFamilyGridResult: result,
        filter: null,
      } as DesignComponentFamilyGrid,
    });
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
  }
  setLoader("REMOVE", "GetDesignComponentFamilyGrid");
}

export async function GetFilterColumDesignComponentFamily(
  columName: string,
  columValue: string,
  queryFilter?: DesignComponentFamilyQueryObjectGrid
) {
  let result: FilterValueDto[] | undefined;
  let api = new DesignComponentFamilyApi();
  // setLoader("ADD", "GetFilterColumDesignComponentFamily");

  result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
    api.designComponentFamilyGetFilterResult(
      queryFilter || {},
      columName,
      columValue
    )
  );

  let rtn = {
    filter: result,
    DesignComponentFamilyGridResult: null,
  } as DesignComponentFamilyGrid;
  rootStore.dispatch({
    type: GET_FILTER_DESIGN_COMPONENT_FAMILY,
    payload: rtn,
  });
  // setLoader("REMOVE", "GetFilterColumDesignComponentFamily");
}
export async function GetOpenImplementation(id: number) {
  let api = new DesignComponentFamilyApi();
  setLoader("ADD", "GetOpenImplementation");
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.openImplementation(id)
  );
  let rtn = {
    data: result?.data,
    info: result?.info,
    warning: result?.warning,
  } as ResultDto;
  if (result?.warning)
    rootStore.dispatch(
      setNotification({
        message: result?.info ?? "",
        notifyType: NotifyType.error,
      })
    );
  setLoader("REMOVE", "GetOpenImplementation");
  return rtn;
}

export async function GetOpenImplementationStatus(id: number) {
  let api = new DesignComponentFamilyApi();
  setLoader("ADD", "GetOpenImplementation");
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.openImplementationStatus(id)
  );
  let rtn = {
    data: result?.data,
    info: result?.info,
    warning: result?.warning,
  } as ResultDto;
  if (result?.warning)
    rootStore.dispatch(
      setNotification({
        message: result?.info ?? "",
        notifyType: NotifyType.error,
      })
    );
  setLoader("REMOVE", "GetOpenImplementation");
  return rtn;
}

export async function GetOpenDCS(id: number) {
  let api = new DesignComponentFamilyApi();
  setLoader("ADD", "GetOpenDCS");
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.openDCS(id)
  );
  let rtn = {
    data: result?.data,
    info: result?.info,
    warning: result?.warning,
  } as ResultDto;
  if (result?.warning)
    rootStore.dispatch(
      setNotification({
        message: result?.info ?? "",
        notifyType: NotifyType.error,
      })
    );
  setLoader("REMOVE", "GetOpenDCS");
  return rtn;
}
