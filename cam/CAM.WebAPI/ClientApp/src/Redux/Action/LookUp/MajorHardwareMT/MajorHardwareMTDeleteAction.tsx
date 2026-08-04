import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { MajorHardwareMTApi } from "../../../../Business/LookUp/MajorHardwareMTBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function DeleteMajorHardwareMT(id: number) {
  setLoader("ADD", "deleteMajorHardwareMT");
  let api = new MajorHardwareMTApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.MajorHardwareMTDelete(id)
  );
  let rtn = {
    data: result?.data,
    info: result?.info,
    warning: result?.warning,
  } as ResultDto;
  rootStore.dispatch(
    setNotification({
      message: result?.info ?? "",
      notifyType: result?.warning ? NotifyType.error : NotifyType.success,
    })
  );
  rootStore.dispatch({ type: "DELETE_MAJORHARDWAREMT", payload: rtn });
  setLoader("REMOVE", "deleteMajorHardwareMT");
  return rtn;
}

export async function DeleteDeepMajorHardwareMT(id: number) {
  let api = new MajorHardwareMTApi();
  setLoader("ADD", "DeleteDeepMajorHardwareMT");
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.MajorHardwareMTDeleteDeep(id)
  );
  let rtn = {
    data: result?.data,
    info: result?.info,
    warning: result?.warning,
  } as ResultDto;
  rootStore.dispatch(
    setNotification({
      message: result?.info ?? "",
      notifyType: result?.warning ? NotifyType.error : NotifyType.success,
    })
  );
  rootStore.dispatch({ type: "DELETE_MAJORHARDWAREMT", payload: rtn });
  setLoader("REMOVE", "DeleteDeepMajorHardwareMT");
  return rtn;
}

export async function GetRelatedRecordsMajorHardwareMT(id: number) {
  let api = new MajorHardwareMTApi();
  setLoader("ADD", "GetRelatedRecordsMajorHardwareMT");
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.MajorHardwareMTGetRelatedRecords(id)
  );
  let rtn = {
    data: result?.data,
    info: result?.info,
    warning: result?.warning,
  } as ResultDto;
  if (result?.warning)
    rootStore.dispatch(
      setNotification({
        message: result?.info ?? "",
        notifyType: NotifyType.error,
      })
    );
  setLoader("REMOVE", "GetRelatedRecordsMajorHardwareMT");
  return rtn;
}
