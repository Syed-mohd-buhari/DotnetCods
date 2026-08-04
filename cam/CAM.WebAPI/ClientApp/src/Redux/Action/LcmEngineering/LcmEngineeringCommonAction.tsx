import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import { LcmEngineeringApi } from "../../../Business/LcmEngineeringBusiness";
import {
  DataRemediationDto,
  ResultDataRemediationDto,
  ResultDto,
  ResultDtoOfResultDataRemediationDto,
} from "../../../Model/CommonModels";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";

export async function GetMajorSoftwareBuildEoS(Id: number) {
  setLoader("ADD", "GetMajorSoftwareBuildEoS");

  let api = new LcmEngineeringApi();

  let data = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.lcmEngineeringMajorSoftwareBuildEoS(Id)
  );
  setLoader("REMOVE", "GetMajorSoftwareBuildEoS");

  return data?.data as Date | null;
}

export async function GetMajorHardwareBuildEoS(Id: number) {
  setLoader("ADD", "GetMajorHardwareBuildEoS");

  let api = new LcmEngineeringApi();

  let data = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.lcmEngineeringMajorHardwareBuildEoS(Id)
  );
  setLoader("REMOVE", "GetMajorHardwareBuildEoS");

  return data?.data as Date | null;
}

export async function ApplyDataRimediationLcmEngineering(
  data: DataRemediationDto
) {
  let api = new LcmEngineeringApi();
  setLoader("ADD", "ApplyDataRimediationLcmEngineering");
  let result = await ApiCallWithErrorHandling<
    Promise<ResultDtoOfResultDataRemediationDto>
  >(() => api.lcmEngineeringApplyDataRemediation(data));
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
  setLoader("REMOVE", "ApplyDataRimediationLcmEngineering");
}

export async function GetCreatedDCID(Id: number) {
  setLoader("ADD", "GetCreatedDCID");

  let api = new LcmEngineeringApi();

  let data = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.getCreatedDesignComponentId(Id)
  );
  setLoader("REMOVE", "GetCreatedDCID");

  return data?.data;
}
