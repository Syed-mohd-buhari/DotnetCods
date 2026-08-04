import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import { VBOMInfoApi } from "../../../Business/VBOMInfoBusiness";
import { ResultDto } from "../../../Model/CommonModels";
import {
  GET_EDIT_VBOM_INFO,
  GET_EDIT_VBOM_CLUSTER_INFO,
  EDIT_VBOM_INFO,
  VBOMInfoDtoUpdate,
  VBOMInfoEdit,
  VBOMClusterInfoEdit,
} from "../../../Model/VBOMInfo";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";

export async function GetVBOMInfoEditResource(id: number) {
  setLoader("ADD", "GetVBOMInfoApiEditResource");

  let api = new VBOMInfoApi();
  let createResource = await ApiCallWithErrorHandling<
    Promise<VBOMInfoDtoUpdate>
  >(() => api.VBOMInfoGetUpdateResourceVBOMInfo(id));
  let rtn = { VBOMInfoDtoEdit: createResource } as VBOMInfoEdit;
  let rtn1 = { VBOMClusterInfoDtoEdit: createResource } as VBOMClusterInfoEdit;
  rootStore.dispatch({ type: GET_EDIT_VBOM_INFO, payload: rtn });
  rootStore.dispatch({ type: GET_EDIT_VBOM_CLUSTER_INFO, payload: rtn1 });
  setLoader("REMOVE", "GetVBOMInfoApiEditResource");

  return rtn1;
}

export async function EditVBOMInfo(data: VBOMInfoDtoUpdate, forced?: boolean) {
  setLoader("ADD", "EditVBOMInfo");
  let api = new VBOMInfoApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.VBOMInfoEdit(data, forced)
  );
  let rtn = { ResultDtoEdit: result } as VBOMInfoEdit;
  rootStore.dispatch(
    setNotification({
      message: result?.info ?? "",
      notifyType: result?.warning ? NotifyType.error : NotifyType.success,
    })
  );
  rootStore.dispatch({ type: EDIT_VBOM_INFO, payload: rtn });
  setLoader("REMOVE", "EditVBOMInfo");
  return rtn;
}
