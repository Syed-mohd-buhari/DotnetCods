import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import { LcmEngAuditApi } from "../../../Business/LcmEngAuditBusiness";
import { getResourceObject } from "../../../Model/Common";
import { ResultDto } from "../../../Model/CommonModels";
import { LcmEngAuditDtoUpdate, LcmEngAuditQueryDto } from "../../../Model/LcmEngAudit";
import {
  CREATE_LCM_ENG_AUDIT,
  GET_CREATE_LCM_ENG_AUDIT,
  LcmEngAuditCreate,
  LcmEngAuditDtoCreate,
} from "../../../Model/LcmEngAudit";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function CreatLcmEngAudit(
  data: LcmEngAuditDtoUpdate,
  forced?: boolean
) {
  setLoader("ADD", "CreatLcmEngAudit");
  let api = new LcmEngAuditApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.lcmEngAuditCreate(data, forced)
  );
  let rtn = {
    ResultDtoCreate: result,
    LcmEngAuditDtoCreate: null,
  } as LcmEngAuditCreate;
  rootStore.dispatch(
    setNotification({
      message: result?.info ?? "",
      notifyType: result?.warning ? NotifyType.error : NotifyType.success,
    })
  );
  rootStore.dispatch({ type: CREATE_LCM_ENG_AUDIT, payload: rtn });
  setLoader("REMOVE", "CreatLcmEngAudit");
  return rtn;
}
