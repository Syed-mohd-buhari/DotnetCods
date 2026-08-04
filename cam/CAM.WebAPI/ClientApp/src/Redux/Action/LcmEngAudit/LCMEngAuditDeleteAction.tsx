import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import { LcmEngAuditApi } from "../../../Business/LcmEngAuditBusiness";
import { ResultDto } from "../../../Model/CommonModels";
import {
  DELETE_LCM_ENG_AUDIT,
  RESTORE_LCM_ENG_AUDIT,
} from "../../../Model/LcmEngAudit";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function deleteLcmEngAudit(id: number) {
  setLoader("ADD", "deleteLcmEngAudit");
  let api = new LcmEngAuditApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.lcmEngAuditDelete(id)
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
  rootStore.dispatch({ type: DELETE_LCM_ENG_AUDIT, payload: rtn });

  setLoader("REMOVE", "deleteLcmEngAudit");
  return rtn;
}
