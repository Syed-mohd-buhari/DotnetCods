import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import { IdentityAsIsApi } from "../../../Business/IdentityAsIs";
import { ResultDto } from "../../../Model/CommonModels";
import {
  DELETE_IDENTiTYASIS,
  RESTORE_IDENTiTYASIS,
} from "../../../Model/LookUp/Identities";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";

export async function deleteIdentityAsIs(id: number) {
  let api = new IdentityAsIsApi();
  setLoader("ADD", "deleteIdentityAsIs");
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.IdentityAsIsDelete(id)
  );
  let rtn = {
    data: result?.data,
    info: result?.info,
    warning: result?.warning,
  } as ResultDto;
  rootStore.dispatch(
    setNotification({
      message: result?.info ?? "",
      notifyType: result?.warning ? NotifyType.error : NotifyType.success,
    })
  );
  rootStore.dispatch({ type: DELETE_IDENTiTYASIS, payload: rtn });
  setLoader("REMOVE", "deleteIdentityAsIs");
  return rtn;
}

export async function RestoreIdentityAsIs(id: number) {
  setLoader("ADD", "RestoreIdentityAsIs");
  let api = new IdentityAsIsApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.IdentityAsIsRestore(id)
  );
  let rtn = {
    data: result?.data,
    info: result?.info,
    warning: result?.warning,
  } as ResultDto;
  rootStore.dispatch(
    setNotification({
      message: result?.info ?? "",
      notifyType: result?.warning ? NotifyType.error : NotifyType.success,
    })
  );
  rootStore.dispatch({ type: RESTORE_IDENTiTYASIS, payload: rtn });
  setLoader("REMOVE", "RestoreIdentityAsIs");
  return rtn;
}

export async function DeleteDeepIdentityAsIs(id: number) {
  let api = new IdentityAsIsApi();
  setLoader("ADD", "DeleteDeepIdentityAsIs");
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.IdentityAsIsDeleteDeep(id)
  );
  let rtn = {
    data: result?.data,
    info: result?.info,
    warning: result?.warning,
  } as ResultDto;
  rootStore.dispatch(
    setNotification({
      message: result?.info ?? "",
      notifyType: result?.warning ? NotifyType.error : NotifyType.success,
    })
  );
  rootStore.dispatch({ type: DELETE_IDENTiTYASIS, payload: rtn });
  setLoader("REMOVE", "DeleteDeepIdentityAsIs");
  return rtn;
}
