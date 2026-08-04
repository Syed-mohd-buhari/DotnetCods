import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { MajorHardwareMTApi } from "../../../../Business/LookUp/MajorHardwareMTBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'
import {
  MajorHardwareMTCreate,
  MajorHardwareMTDto,
} from "../../../../Model/LookUp/MajorHardwareMT";

export async function GetMajorHardwareMTCreateResource() {
  setLoader("ADD", "GetMajorHardwareMTCreateResource");

  let api = new MajorHardwareMTApi();
  let createResource = await ApiCallWithErrorHandling<
    Promise<MajorHardwareMTDto>
  >(() => api.MajorHardwareMTGetCreatepage());
  let rtn = {
    ResultDtoCreate: null,
    LookUpDtoCreate: createResource,
  } as MajorHardwareMTCreate;
  rootStore.dispatch({ type: "GET_CREATE_MAJORHARDWAREMT", payload: rtn });
  setLoader("REMOVE", "GetMajorHardwareMTCreateResource");
}

export async function CreatMajorHardwareMT(data: MajorHardwareMTDto) {
  setLoader("ADD", "CreatMajorHardwareMT");
  let api = new MajorHardwareMTApi();

  const payload = {
    hardwareSolution: data.hardwareSolution,
    hardwareType: data.hardwareType,
    buildConstructionId: data.buildConstructionId,
    orgEqpManuFacturerId: data.orgEqpManuFacturerId,
    platformId: data.platformId,
  };

  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.MajorHardwareMTCreate(payload)
  );
  let rtn = {
    ResultDtoCreate: result,
    LookUpDtoCreate: null,
  } as MajorHardwareMTCreate;
  rootStore.dispatch(
    setNotification({
      message: result?.info ?? "",
      notifyType: result?.warning ? NotifyType.error : NotifyType.success,
    })
  );
  rootStore.dispatch({ type: "CREATE_MAJORHARDWAREMT", payload: rtn });
  setLoader("REMOVE", "CreatMajorHardwareMT");
  return rtn;
}
