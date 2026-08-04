import React from "react";
import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import { ResultDto } from "../../../Model/CommonModels";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";
import { NFVISwCompatibleApi } from "../../../Business/NFVISoftwareCompatibleBusiness";
import { DELETE_NFVI_COMPATIBLE } from "../../../Model/NFVISoftwareCompatible";
// import { useDispatch } from 'react-redux'

export async function deleteNFVISwCompatible(id: number) {
  setLoader("ADD", "deleteNFVISwCompatible");
  let api = new NFVISwCompatibleApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.NFVISwCompatibleDelete(id)
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
  rootStore.dispatch({ type: DELETE_NFVI_COMPATIBLE, payload: rtn });
  setLoader("REMOVE", "deleteNFVISwCompatible");
  return rtn;
}
