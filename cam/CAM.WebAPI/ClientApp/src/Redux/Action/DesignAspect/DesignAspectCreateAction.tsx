import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import { DesignAspectApi } from "../../../Business/DesignAspectsBusiness";
import { ResultDto } from "../../../Model/CommonModels";
import {
  CREATE_DESIGN_ASPECT,
  GET_CREATE_DESIGN_ASPECT,
  DesignAspectCreate,
  DesignAspectDtoCreate,
} from "../../../Model/DesignAspects";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function GetDesignAspectCreateResource() {
  setLoader("ADD", "GetDesignAspectCreateResource");

  let api = new DesignAspectApi();
  let createResource = await ApiCallWithErrorHandling<
    Promise<DesignAspectDtoCreate>
  >(() => api.designAspectGetCreateResourceDesignAspect(sessionStorage.dcfId));
  let rtn = {
    ResultDtoCreate: null,
    DesignAspectDtoCreate: createResource,
  } as DesignAspectCreate;
  rootStore.dispatch({ type: GET_CREATE_DESIGN_ASPECT, payload: rtn });
  setLoader("REMOVE", "GetDesignAspectCreateResource");
  sessionStorage.clear();
}

export async function CreatDesignAspect(
  data: DesignAspectDtoCreate,
  forced?: boolean
) {
  setLoader("ADD", "CreatDesignAspect");
  let api = new DesignAspectApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.designAspectCreate(data, forced)
  );
  let rtn = {
    ResultDtoCreate: result,
    DesignAspectDtoCreate: null,
  } as DesignAspectCreate;
  rootStore.dispatch(
    setNotification({
      message: result?.info ?? "",
      notifyType: result?.warning ? NotifyType.error : NotifyType.success,
    })
  );
  rootStore.dispatch({ type: CREATE_DESIGN_ASPECT, payload: rtn });
  setLoader("REMOVE", "CreatDesignAspect");
  return rtn;
}
