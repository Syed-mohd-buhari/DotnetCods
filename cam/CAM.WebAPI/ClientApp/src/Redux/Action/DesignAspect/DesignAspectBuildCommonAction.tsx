import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import { DesignAspectApi } from "../../../Business/DesignAspectsBusiness";
import {
  DataRemediationDto,
  ResultDataRemediationDto,
  ResultDtoOfResultDataRemediationDto,
} from "../../../Model/CommonModels";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";

export async function GetServicesAndNetworks(Id: number) {
  setLoader("ADD", "GetServicesAndNetworks");

  let api = new DesignAspectApi();

  let data = await ApiCallWithErrorHandling<Promise<any>>(() =>
    api.designAspectServicesAndNetworks(Id)
  );
  setLoader("REMOVE", "GetServicesAndNetworks");
  return data;
}

export async function GetDesignComponentFamily(Id: number) {
  setLoader("ADD", "GetDesignComponentFamily");

  let api = new DesignAspectApi();

  let data = await ApiCallWithErrorHandling<Promise<any>>(() =>
    api.designAspectGetDesginComponentFamily(Id)
  );
  setLoader("REMOVE", "GetDesignComponentFamily");
  return data;
}

export async function ApplyDataRimediationDesignAspect(
  data: DataRemediationDto
) {
  let api = new DesignAspectApi();
  setLoader("ADD", "ApplyDataRimediationDesignAspect");
  let result = await ApiCallWithErrorHandling<
    Promise<ResultDtoOfResultDataRemediationDto>
  >(() => api.designAspectApplyDataRemediation(data));
  if (result && !result.warning) {
    rootStore.dispatch(
      setNotification({
        message: result?.info ?? "",
        notifyType: result?.warning ? NotifyType.error : NotifyType.success,
      })
    );
    return result.data as ResultDataRemediationDto;
  } else {
    rootStore.dispatch(
      setNotification({
        message: result?.info ?? "",
        notifyType: result?.warning ? NotifyType.error : NotifyType.success,
      })
    );
  }
  setLoader("REMOVE", "ApplyDataRimediationDesignAspect");
}
