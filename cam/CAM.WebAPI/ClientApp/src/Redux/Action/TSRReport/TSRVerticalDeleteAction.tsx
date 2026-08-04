import React from "react";
import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import { ResultDto } from "../../../Model/CommonModels";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";
import { TSRReportApi } from "../../../Business/TSRReportBusiness";

export async function DeleteDeepTSRVertical(id: number) {
  let api = new TSRReportApi();
  setLoader("ADD", "DeleteDeepTSRVertical");
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.TSRReportDeleteDeep(id)
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
  rootStore.dispatch({ type: "DELETE_TSR_VERTICAL", payload: rtn });
  setLoader("REMOVE", "DeleteDeepTSRVertical");
  return rtn;
}
