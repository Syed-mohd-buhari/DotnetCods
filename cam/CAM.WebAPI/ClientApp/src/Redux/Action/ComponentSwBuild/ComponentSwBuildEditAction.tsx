import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import { ComponentSwBuildApi } from "../../../Business/ComponentSwBuildsBusiness";
import { ResultDto } from "../../../Model/CommonModels";
import {
  EDIT_COMPONENT_SW_BUILD,
  GET_EDIT_COMPONENT_SW_BUILD,
  ComponentSwBuildDtoUpdate,
  ComponentSwBuildEdit,
} from "../../../Model/ComponentSwBuild";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";

export async function GetComponentSwBuildEditResource(id: number) {
  setLoader("ADD", "GetComponentSwBuildEditResource");

  let api = new ComponentSwBuildApi();
  let createResource = await ApiCallWithErrorHandling<
    Promise<ComponentSwBuildDtoUpdate>
  >(() => api.componentSwBuildGetUpdateResourceComponentSwBuild(id));
  let rtn = { ComponentSwBuildDtoEdit: createResource } as ComponentSwBuildEdit;
  rootStore.dispatch({ type: GET_EDIT_COMPONENT_SW_BUILD, payload: rtn });
  setLoader("REMOVE", "GetComponentSwBuildEditResource");

  return rtn.ComponentSwBuildDtoEdit;
}

export async function EditComponentSwBuild(
  data: ComponentSwBuildDtoUpdate,
  forced?: boolean
) {
  setLoader("ADD", "EditComponentSwBuild");
  let api = new ComponentSwBuildApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.componentSwBuildPut(data, forced)
  );
  let rtn = { ResultDtoEdit: result } as ComponentSwBuildEdit;
  rootStore.dispatch(
    setNotification({
      message: result?.info ?? "",
      notifyType: result?.warning ? NotifyType.error : NotifyType.success,
    })
  );
  rootStore.dispatch({ type: EDIT_COMPONENT_SW_BUILD, payload: rtn });
  setLoader("REMOVE", "EditComponentSwBuild");
  return rtn;
}
