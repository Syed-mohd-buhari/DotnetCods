import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import { DesignComponentFamilyApi } from "../../../Business/DesignComponentFamilyBusiness";
import { ResultDto } from "../../../Model/CommonModels";
import {
  CREATE_DESIGN_COMPONENT_FAMILY,
  DesignComponentFamilyCreate,
  DesignComponentFamilyDtoCreate,
  GET_CREATE_DESIGN_COMPONENT_FAMILY,
} from "../../../Model/DesignComponentFamily";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function GetDesignComponentFamilyCreateResource() {
  setLoader("ADD", "GetDesignComponentFamilyCreateResource");

  // const dispach = useDispatch();
  // ActionCenter<>
  // let test = await  ActionCenter<Promise<DesignComponentFamilyDtoCreate>>(() => api.designComponentFamilyGetCreateResourceDesignComponentFamily());
  let api = new DesignComponentFamilyApi();

  let createResource = await ApiCallWithErrorHandling<
    Promise<DesignComponentFamilyDtoCreate>
  >(() => api.designComponentFamilyGetCreateResourceDesignComponentFamily());
  let rtn = {
    ResultDtoCreate: null,
    DesignComponentFamilyDtoCreate: createResource,
  } as DesignComponentFamilyCreate;
  rootStore.dispatch({
    type: GET_CREATE_DESIGN_COMPONENT_FAMILY,
    payload: rtn,
  });
  setLoader("REMOVE", "GetDesignComponentFamilyCreateResource");
  return rtn.DesignComponentFamilyDtoCreate;
}

export async function CreatDesignComponentFamily(
  data: DesignComponentFamilyDtoCreate,
  forced?: boolean
) {
  setLoader("ADD", "CreatDesignComponentFamily");
  let api = new DesignComponentFamilyApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.designComponentFamilyCreate(data, forced)
  );
  let rtn = {
    ResultDtoCreate: result,
    DesignComponentFamilyDtoCreate: null,
  } as DesignComponentFamilyCreate;
  rootStore.dispatch(
    setNotification({
      message: result?.info ?? "",
      notifyType: result?.warning ? NotifyType.error : NotifyType.success,
    })
  );
  rootStore.dispatch({ type: CREATE_DESIGN_COMPONENT_FAMILY, payload: rtn });
  setLoader("REMOVE", "CreatDesignComponentFamily");
  return rtn;
}
