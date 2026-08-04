import { ComponentSwBuildApi } from "../../../Business/ComponentSwBuildsBusiness";
import {
  CREATE_COMPONENT_SW_BUILD,
  GET_CREATE_COMPONENT_SW_BUILD,
  ComponentSwBuildCreate,
  ComponentSwBuildDtoCreate,
} from "../../../Model/ComponentSwBuild";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import { setNotification } from "../NotificationAction";
import setLoader from "../LoaderAction";
import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import { ResultDto } from "../../../Model/CommonModels";
// import { useDispatch } from 'react-redux'

export async function GetComponentSwBuildCreateResource({
  isRefillData,
}: {
  isRefillData?: boolean;
} = {}) {
  setLoader("ADD", "GetComponentSwBuildCreateResource");

  let api = new ComponentSwBuildApi();
  let createResource = await ApiCallWithErrorHandling<
    Promise<ComponentSwBuildDtoCreate>
  >(() => api.componentSwBuildGetCreateResourceComponentSwBuild());
  let rtn = {
    ResultDtoCreate: null,
    ComponentSwBuildDtoCreate: createResource,
  } as ComponentSwBuildCreate;
  if (!isRefillData || isRefillData === undefined) {
    rootStore.dispatch({ type: GET_CREATE_COMPONENT_SW_BUILD, payload: rtn });
  }
  setLoader("REMOVE", "GetComponentSwBuildCreateResource");

  return rtn.ComponentSwBuildDtoCreate;
}

export async function CreatComponentSwBuild(
  data: ComponentSwBuildDtoCreate,
  forced?: boolean
) {
  setLoader("ADD", "CreatComponentSwBuild");
  let api = new ComponentSwBuildApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.componentSwBuildCreate(data, forced)
  );
  let rtn = {
    ResultDtoCreate: result,
    ComponentSwBuildDtoCreate: null,
  } as ComponentSwBuildCreate;
  rootStore.dispatch(
    setNotification({
      message: result?.info ?? "",
      notifyType: result?.warning ? NotifyType.error : NotifyType.success,
    })
  );
  rootStore.dispatch({ type: CREATE_COMPONENT_SW_BUILD, payload: rtn });
  setLoader("REMOVE", "CreatComponentSwBuild");
  return rtn;
}
