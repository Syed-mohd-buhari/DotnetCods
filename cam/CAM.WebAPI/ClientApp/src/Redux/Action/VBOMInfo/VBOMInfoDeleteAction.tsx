import React from "react";
import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import { VBOMInfoApi } from "../../../Business/VBOMInfoBusiness";
import { ResultDto } from "../../../Model/CommonModels";
import { DELETE_VBOM_INFO, RESTORE_VBOM_INFO } from "../../../Model/VBOMInfo";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function DeleteVBOMInfo(id: number) {
  setLoader("ADD", "DeleteVBOMInfo");
  let api = new VBOMInfoApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.VBOMInfoDelete(id)
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
  rootStore.dispatch({ type: DELETE_VBOM_INFO, payload: rtn });
  setLoader("REMOVE", "DeleteVBOMInfo");
  return rtn;
}

export async function VBOMInfoDelete(id: number, apiType?: string | undefined) {
  setLoader("ADD", "DeleteVBOMInfo");
  let api = new VBOMInfoApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.VBOMInfoDelete(id, apiType)
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
  rootStore.dispatch({ type: DELETE_VBOM_INFO, payload: rtn });
  setLoader("REMOVE", "DeleteVBOMInfo");
  return rtn;
}
