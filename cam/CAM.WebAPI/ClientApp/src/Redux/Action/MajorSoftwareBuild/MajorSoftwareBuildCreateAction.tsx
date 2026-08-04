import { MajorSoftwareBuildApi } from "../../../Business/MajorSoftwareBuildsBusiness";
import {
  CREATE_MAJOR_SOFTWARE_BUILD,
  GET_CREATE_MAJOR_SOFTWARE_BUILD,
  MajorSoftwareBuildCreate,
  MajorSoftwareBuildDtoCreate,
} from "../../../Model/MajorSoftwareBuild";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import { setNotification } from "../NotificationAction";
import setLoader from "../../Action/LoaderAction";
import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import { ResultDto } from "../../../Model/CommonModels";
// import { useDispatch } from 'react-redux'

export async function GetMajorSoftwareBuildCreateResource({
  isRefillData,
}: {
  isRefillData?: boolean;
} = {}) {
  setLoader("ADD", "GetMajorSoftwareBuildCreateResource");

  let api = new MajorSoftwareBuildApi();
  let createResource = await ApiCallWithErrorHandling<
    Promise<MajorSoftwareBuildDtoCreate>
  >(() => api.majorSoftwareBuildGetCreateResourceMajorSoftwareBuild());
  let rtn = {
    ResultDtoCreate: null,
    MajorSoftwareBuildDtoCreate: createResource,
  } as MajorSoftwareBuildCreate;
  if (!isRefillData || isRefillData === undefined) {
    rootStore.dispatch({ type: GET_CREATE_MAJOR_SOFTWARE_BUILD, payload: rtn });
  }
  setLoader("REMOVE", "GetMajorSoftwareBuildCreateResource");

  return rtn.MajorSoftwareBuildDtoCreate;
}

export async function CreatMajorSoftwareBuild(
  data: MajorSoftwareBuildDtoCreate,
  forced?: boolean
) {
  setLoader("ADD", "CreatMajorSoftwareBuild");
  let api = new MajorSoftwareBuildApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.majorSoftwareBuildCreate(data, forced)
  );
  let rtn = {
    ResultDtoCreate: result,
    MajorSoftwareBuildDtoCreate: null,
  } as MajorSoftwareBuildCreate;
  rootStore.dispatch(
    setNotification({
      message: result?.info ?? "",
      notifyType: result?.warning ? NotifyType.error : NotifyType.success,
    })
  );
  rootStore.dispatch({ type: CREATE_MAJOR_SOFTWARE_BUILD, payload: rtn });
  setLoader("REMOVE", "CreatMajorSoftwareBuild");
  return rtn;
}
