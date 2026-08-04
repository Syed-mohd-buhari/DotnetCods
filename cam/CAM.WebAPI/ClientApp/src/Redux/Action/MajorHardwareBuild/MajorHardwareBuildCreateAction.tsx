import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import { MajorHardwareBuildApi } from "../../../Business/MajorHardwareBuildBusiness";
import { ResultDto } from "../../../Model/CommonModels";
import {
  CREATE_MAJOR_HARDWARE_BUILD,
  GET_CREATE_MAJOR_HARDWARE_BUILD,
  MajorHardwareBuildCreate,
  MajorHardwareBuildDtoCreate,
} from "../../../Model/MajorHardwareBuild";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";

export async function GetMajorHardwareBuildCreateResource({
  isRefillData,
}: {
  isRefillData?: boolean;
} = {}) {
  setLoader("ADD", "GetMajorHardwareBuildCreateResource");

  let api = new MajorHardwareBuildApi();
  let createResource = await ApiCallWithErrorHandling<
    Promise<MajorHardwareBuildDtoCreate>
  >(() => api.majorHardwareBuildGetCreateResourceMajorHardwareBuild());
  let rtn = {
    ResultDtoCreate: null,
    MajorHardwareBuildDtoCreate: { ...createResource, eomStatus: 1 },
  } as MajorHardwareBuildCreate;
  if (!isRefillData || isRefillData === undefined) {
    rootStore.dispatch({ type: GET_CREATE_MAJOR_HARDWARE_BUILD, payload: rtn });
  }
  setLoader("REMOVE", "GetMajorHardwareBuildCreateResource");

  return rtn.MajorHardwareBuildDtoCreate;
}

export async function CreatMajorHardwareBuild(
  data: MajorHardwareBuildDtoCreate,
  forced?: boolean
) {
  let api = new MajorHardwareBuildApi();
  setLoader("ADD", "CreatMajorHardwareBuild");
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.majorHardwareBuildCreate(data, forced)
  );
  let rtn = {
    ResultDtoCreate: result,
    MajorHardwareBuildDtoCreate: null,
  } as MajorHardwareBuildCreate;
  rootStore.dispatch(
    setNotification({
      message: result?.info ?? "",
      notifyType: result?.warning ? NotifyType.error : NotifyType.success,
    })
  );
  rootStore.dispatch({ type: CREATE_MAJOR_HARDWARE_BUILD, payload: rtn });

  setLoader("REMOVE", "CreatMajorHardwareBuild");
  return rtn;
}
