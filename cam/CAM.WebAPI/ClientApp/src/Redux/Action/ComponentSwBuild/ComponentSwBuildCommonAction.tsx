import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import { ComponentSwBuildApi } from "../../../Business/ComponentSwBuildsBusiness";
import {
  DataRemediationDto,
  ResultDto,
  ResultDataRemediationDto,
  ResultDtoOfResultDataRemediationDto,
} from "../../../Model/CommonModels";
import {
  ComponentSwBuildToCloneDto,
  ResultDtoOfComponentSwBuildToCloneDto,
  CloneComponentSwBuildDto,
} from "../../../Model/ComponentSwBuild";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";

export async function ApplyDataRimediationComponentSw(
  data: DataRemediationDto
) {
  let api = new ComponentSwBuildApi();
  setLoader("ADD", "ApplyDataRimediationComponentSw");
  let result = await ApiCallWithErrorHandling<
    Promise<ResultDtoOfResultDataRemediationDto>
  >(() => api.componentSwBuildApplyDataRemediation(data));
  if (result && !result.warning) {
    rootStore.dispatch(
      setNotification({
        message: result?.info ?? "",
        notifyType: result?.warning ? NotifyType.error : NotifyType.success,
      })
    );
    setLoader("REMOVE", "ApplyDataRimediationComponentSw");
    return result.data as ResultDataRemediationDto;
  } else {
    rootStore.dispatch(
      setNotification({
        message: result?.info ?? "",
        notifyType: result?.warning ? NotifyType.error : NotifyType.success,
      })
    );
    setLoader("REMOVE", "ApplyDataRimediationComponentSw");
  }
}

export async function GetComponentSwToClone(data: number) {
  let api = new ComponentSwBuildApi();
  setLoader("ADD", "GetComponentSwToClone");
  let result = await ApiCallWithErrorHandling<
    Promise<ResultDtoOfComponentSwBuildToCloneDto>
  >(() => api.componentSwBuildGetComponentSwBuildToClone(data));
  if (result && !result.warning) {
    // rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
    setLoader("REMOVE", "GetComponentSwToClone");
    return result as ResultDtoOfComponentSwBuildToCloneDto;
  } else {
    rootStore.dispatch(
      setNotification({
        message: result?.info ?? "",
        notifyType: result?.warning ? NotifyType.error : NotifyType.success,
      })
    );
    setLoader("REMOVE", "GetComponentSwToClone");
  }
}

export async function GetSystemTypeForAddMajorSW(data: any) {
  let api = new ComponentSwBuildApi();
  setLoader("ADD", "GetSystemTypeForAddMajorSW");
  let result = await ApiCallWithErrorHandling<
    Promise<ResultDtoOfComponentSwBuildToCloneDto>
  >(() => api.componentSwBuildGetSystemTypeForAddMajorSW(data));
  if (result && !result.warning) {
    // rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
    setLoader("REMOVE", "GetSystemTypeForAddMajorSW");
    return result as ResultDtoOfComponentSwBuildToCloneDto;
  } else {
    rootStore.dispatch(
      setNotification({
        message: result?.info ?? "",
        notifyType: result?.warning ? NotifyType.error : NotifyType.success,
      })
    );
    setLoader("REMOVE", "GetSystemTypeForAddMajorSW");
  }
}

export async function GetComponentSwClonePreSubmit(
  data: number,
  flag?: boolean
) {
  let api = new ComponentSwBuildApi();
  setLoader("ADD", "GetComponentSwClonePreSubmit");
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.componentSwBuildGetInfoComponentSwBuildToClone(data, flag)
  );
  if (result && !result.warning) {
    setLoader("REMOVE", "GetComponentSwClonePreSubmit");
    return result as ResultDto;
  } else {
    if (result?.info != "systemtype" && result?.info != "designcomponent") {
      rootStore.dispatch(
        setNotification({
          message: result?.info ?? "",
          notifyType: result?.warning ? NotifyType.error : NotifyType.success,
        })
      );
    }
    setLoader("REMOVE", "GetComponentSwClonePreSubmit");
    return result as ResultDto;
  }
}
export async function CloneComponentSw(data: CloneComponentSwBuildDto) {
  let api = new ComponentSwBuildApi();
  setLoader("ADD", "CloneComponentSw");
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.componentSwBuildCloneComponentSwBuild(data)
  );
  if (result && !result.warning) {
    setLoader("REMOVE", "CloneComponentSw");
    rootStore.dispatch(
      setNotification({
        message: result?.info ?? "",
        notifyType: result?.warning ? NotifyType.error : NotifyType.success,
      })
    );
    return result as ResultDto;
  } else {
    rootStore.dispatch(
      setNotification({
        message: result?.info ?? "",
        notifyType: result?.warning ? NotifyType.error : NotifyType.success,
      })
    );
    setLoader("REMOVE", "CloneComponentSw");
  }
}
