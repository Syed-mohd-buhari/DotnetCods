import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import { ServicePlanApi } from "../../../Business/ServicePlanBusiness";
import { ResultDto } from "../../../Model/CommonModels";
import {
  CREATE_SERVICE_PLAN,
  GET_CREATE_SERVICE_PLAN,
  ServicePlanCreate,
  ServiceLevelPlanDtoCreate,
} from "../../../Model/ServicePlan";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";

export async function GetServicePlanCreateResource({
  isRefillData,
}: {
  isRefillData?: boolean;
} = {}) {
  setLoader("ADD", "GetServicePlanCreateResource");
  let api = new ServicePlanApi();
  let createResource = await ApiCallWithErrorHandling<
    Promise<ServiceLevelPlanDtoCreate>
  >(() => api.serviceLevelPlanGetResource());
  let rtn = {
    ServicePlanDtoCreate: createResource,
  } as ServicePlanCreate;
  // if (!isRefillData || isRefillData === undefined) {
  rootStore.dispatch({ type: GET_CREATE_SERVICE_PLAN, payload: rtn });
  // }
  setLoader("REMOVE", "GetServicePlanCreateResource");
  return rtn;
}

export async function CreateServicePlan(data: ServiceLevelPlanDtoCreate) {
  setLoader("ADD", "CreateServicePlan");
  let api = new ServicePlanApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.servicePlanCreate(data)
  );

  let rtn = {
    ResultDtoCreate: result,
  } as ServicePlanCreate;

  rootStore.dispatch(
    setNotification({
      message: result?.info ?? "",
      notifyType: result?.warning ? NotifyType.error : NotifyType.success,
    })
  );
  rootStore.dispatch({ type: CREATE_SERVICE_PLAN, payload: rtn });
  setLoader("REMOVE", "CreateServicePlan");
  return rtn;
}
