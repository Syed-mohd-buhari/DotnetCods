import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import { NFVISwCompatibleApi } from "../../../Business/NFVISoftwareCompatibleBusiness";
import { ResultDto } from "../../../Model/CommonModels";
import {
  EDIT_NFVI_SW_COMPATIBLE,
  GET_EDIT_NFVI_SW_COMPATIBLE,
  NFVISwCompatibleDtoUpdate,
  NFVISwCompatibleEdit,
} from "../../../Model/NFVISoftwareCompatible";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";

export async function GetNFVISwCompatibleEditResource(id: number) {
  setLoader("ADD", "GetNFVISwCompatibleEditResource");

  let api = new NFVISwCompatibleApi();
  let createResource = await ApiCallWithErrorHandling<
    Promise<NFVISwCompatibleDtoUpdate>
  >(() => api.nfviSwCompatibleGetUpdateResourceNFVISwCompatible(id));
  let rtn = { NFVISwCompatibleDtoEdit: createResource } as NFVISwCompatibleEdit;
  rootStore.dispatch({ type: GET_EDIT_NFVI_SW_COMPATIBLE, payload: rtn });
  setLoader("REMOVE", "GetNFVISwCompatibleEditResource");

  return rtn;
}

export async function EditNFVISwCompatible(
  data: NFVISwCompatibleDtoUpdate,
  forced?: boolean
) {
  setLoader("ADD", "EditNFVISwCompatible");
  let api = new NFVISwCompatibleApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.NFVISwCompatibleEdit(data, forced)
  );
  let rtn = { ResultDtoEdit: result } as NFVISwCompatibleEdit;
  rootStore.dispatch(
    setNotification({
      message: result?.info ?? "",
      notifyType: result?.warning ? NotifyType.error : NotifyType.success,
    })
  );
  rootStore.dispatch({ type: EDIT_NFVI_SW_COMPATIBLE, payload: rtn });
  setLoader("REMOVE", "EditNFVISwCompatible");
  return rtn;
}
