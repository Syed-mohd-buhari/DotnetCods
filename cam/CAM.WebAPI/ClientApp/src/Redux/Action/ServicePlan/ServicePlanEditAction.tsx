import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import { ServicePlanApi } from "../../../Business/ServicePlanBusiness";
import { ResultDto } from "../../../Model/CommonModels";
import {
  EDIT_SERVICE_PLAN,
  GET_EDIT_SERVICE_PLAN,
  ServiceLevelPlanDtoUpdate,
  ServicePlanEdit,
} from "../../../Model/ServicePlan";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";

export async function GetServicePlanEditResource(id) {
  setLoader("ADD", "GetServicePlanEditResource");
  let api = new ServicePlanApi();
  let response = await ApiCallWithErrorHandling<
    Promise<ServiceLevelPlanDtoUpdate>
  >(() => api.ServicePlanGetUpdateResource(id));
  let rtn = {
    ServicePlanDtoEdit: response,
  } as ServicePlanEdit;
  rootStore.dispatch({ type: GET_EDIT_SERVICE_PLAN, payload: rtn });

  setLoader("REMOVE", "GetServicePlanEditResource");
  return rtn;
}

export async function EditServicePlan(
  data: ServiceLevelPlanDtoUpdate,
  forced?: boolean
) {
  let api = new ServicePlanApi();
  setLoader("ADD", "EditServicePlan");
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.servicePlanUpdate(data, forced)
  );
  let rtn = { ResultDtoEdit: result } as ServicePlanEdit;
  rootStore.dispatch(
    setNotification({
      message: result?.info ?? "",
      notifyType: result?.warning ? NotifyType.error : NotifyType.success,
    })
  );
  rootStore.dispatch({ type: EDIT_SERVICE_PLAN, payload: rtn });
  setLoader("REMOVE", "EditServicePlan");
  return rtn;
}
