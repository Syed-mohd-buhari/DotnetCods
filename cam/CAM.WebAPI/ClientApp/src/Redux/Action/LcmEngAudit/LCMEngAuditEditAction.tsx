import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import { LcmEngAuditApi } from "../../../Business/LcmEngAuditBusiness";
import { ResultDto } from "../../../Model/CommonModels";
import { LcmEngAuditQueryDto } from "../../../Model/LcmEngAudit";
import {
  EDIT_LCM_ENG_AUDIT,
  GET_EDIT_LCM_ENG_AUDIT,
  LcmEngAuditDtoUpdate,
  LcmEngAuditEdit,
} from "../../../Model/LcmEngAudit";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function EditLcmEngAudit(
  data: LcmEngAuditDtoUpdate,
  forced?: boolean
) {
  let api = new LcmEngAuditApi();
  setLoader("ADD", "EditLcmEngAudit");
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.lcmEngAuditUpdate(data, forced)
  );
  let rtn = { ResultDtoEdit: result } as LcmEngAuditEdit;
  rootStore.dispatch(
    setNotification({
      message: result?.info ?? "",
      notifyType: result?.warning ? NotifyType.error : NotifyType.success,
    })
  );
  rootStore.dispatch({ type: EDIT_LCM_ENG_AUDIT, payload: rtn });
  setLoader("REMOVE", "EditLcmEngAudit");
  return rtn;
}
