import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import { CBOMApi } from "../../../Business/CBOMBusiness";
import { ResultDto } from "../../../Model/CommonModels";
import {
  GET_EDIT_CBOM,
  EDIT_CBOM,
  CBOMDtoUpdate,
  CBOMEdit,
} from "../../../Model/CBOM";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";

export async function GetCBOMEditResource(id: number) {
  setLoader("ADD", "GetCBOMApiEditResource");

  let api = new CBOMApi();
  let createResource = await ApiCallWithErrorHandling<Promise<CBOMDtoUpdate>>(
    () => api.CBOMGetUpdateResourceCBOM(id)
  );
  let rtn = { CBOMDtoEdit: createResource } as CBOMEdit;
  rootStore.dispatch({ type: GET_EDIT_CBOM, payload: rtn });
  setLoader("REMOVE", "GetCBOMApiEditResource");

  return rtn;
}

export async function EditCBOM(data: CBOMDtoUpdate, forced?: boolean) {
  setLoader("ADD", "EditCBOM");
  let api = new CBOMApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.CBOMEdit(data, forced)
  );
  let rtn = { ResultDtoEdit: result } as CBOMEdit;
  rootStore.dispatch(
    setNotification({
      message: result?.info ?? "",
      notifyType: result?.warning ? NotifyType.error : NotifyType.success,
    })
  );
  rootStore.dispatch({ type: EDIT_CBOM, payload: rtn });
  setLoader("REMOVE", "EditCBOM");
  return rtn;
}
