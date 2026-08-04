import React from "react";
import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import { ResultDto } from "../../../Model/CommonModels";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";
import { ProjectPlanApi } from "../../../Business/ProjectPlanBusiness";
// import { useDispatch } from 'react-redux'

export async function EditProjectPlan(data: any, forced?: boolean) {
  setLoader("ADD", "EditProjectPlan");
  let api = new ProjectPlanApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.editProjectPlan(data, forced)
  );
  // let rtn = { ResultDtoEdit: result } as PlannedActivityTypesEdit;
  rootStore.dispatch(
    setNotification({
      message: result?.info ?? "",
      notifyType: result?.warning ? NotifyType.error : NotifyType.success,
    })
  );
  // rootStore.dispatch({ type: EDIT_PLANNED_ACTIVITY_TYPES, payload: rtn });
  setLoader("REMOVE", "EditProjectPlan");
  return result;
}
