import { type } from "os";
import React from "react";
import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import {
  PlannedActivityTypesApiFetchParamCreator,
  PlannedActivityTypesApi,
} from "../../../Business/PlannedActivityTypesBusiness";
import { ResultDto } from "../../../Model/CommonModels";
import {
  CREATE_PLANNED_ACTIVITY_TYPES,
  PlannedActivityTypesCreate,
  PlannedActivityTypesDtoCreate,
  GET_CREATE_PLANNED_ACTIVITY_TYPES,
  PLANNED_ACTIVITY_TYPES_ENABLE_LINKED_DC,
} from "../../../Model/PlannedActivityTypes";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function CreatPlannedActivityTypes(
  data: PlannedActivityTypesDtoCreate,
  forced?: boolean
) {
  setLoader("ADD", "CreatPlannedActivityTypes");
  let api = new PlannedActivityTypesApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.PlannedActivityTypesCreate(data, forced)
  );
  let rtn = {
    ResultDtoCreate: result,
    PlannedActivityTypesDtoCreate: null,
  } as PlannedActivityTypesCreate;
  rootStore.dispatch(
    setNotification({
      message: result?.info ?? "",
      notifyType: result?.warning ? NotifyType.error : NotifyType.success,
    })
  );
  rootStore.dispatch({
    type: CREATE_PLANNED_ACTIVITY_TYPES,
    payload: rtn,
  });
  setLoader("REMOVE", "CreatPlannedActivityTypes");
  return rtn;
}

export async function PlannedActivityTypeEnableLinkedDC(
  data: any,
  forced?: boolean
) {
  setLoader("ADD", "PlannedActivityTypeEnableLinkedDC");
  let api = new PlannedActivityTypesApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.PlannedActivityTypeEnableLinkedDC(data, forced)
  );
  rootStore.dispatch(
    setNotification({
      message: result?.info ?? "",
      notifyType: result?.warning ? NotifyType.error : NotifyType.success,
    })
  );
  setLoader("REMOVE", "PlannedActivityTypeEnableLinkedDC");
  return result;
}
