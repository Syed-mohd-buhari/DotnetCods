import { fail } from "assert";
import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import { IdentityAsIsApi } from "../../../Business/IdentityAsIs";
import { ResultDto } from "../../../Model/CommonModels";
import {
  EDIT_IDENTiTYASIS,
  GET_EDIT_IDENTiTYASIS,
  IdentityAsIsDtoUpdate,
  IdentityAsIsEdit,
} from "../../../Model/LookUp/Identities";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function GetIdentityAsIsEditResource(id: number) {
  setLoader("ADD", "GetIdentitiesAsIsEditResource");

  let api = new IdentityAsIsApi();
  let createResource = await ApiCallWithErrorHandling<
    Promise<IdentityAsIsDtoUpdate>
  >(() => api.IdentityAsIsGetUpdateResourceIdentityAsIs(id));
  let rtn = { IdentityAsIsDtoEdit: createResource } as IdentityAsIsEdit;
  rootStore.dispatch({ type: GET_EDIT_IDENTiTYASIS, payload: rtn });
  setLoader("REMOVE", "GetIdentitiesAsIsEditResource");

  return rtn.IdentityAsIsDtoEdit;
}

export async function EditIdentityAsIs(
  data: IdentityAsIsDtoUpdate,
  forced?: boolean
) {
  setLoader("ADD", "EditIdentitiesAsIs");
  let api = new IdentityAsIsApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.IdentityAsIsPut(data, forced)
  );
  let rtn = { ResultDtoEdit: result } as IdentityAsIsEdit;
  rootStore.dispatch(
    setNotification({
      message: result?.info ?? "",
      notifyType: result?.warning ? NotifyType.error : NotifyType.success,
    })
  );
  rootStore.dispatch({ type: EDIT_IDENTiTYASIS, payload: rtn });
  setLoader("REMOVE", "EditIdentitiesAsIs");
  return rtn;
}
