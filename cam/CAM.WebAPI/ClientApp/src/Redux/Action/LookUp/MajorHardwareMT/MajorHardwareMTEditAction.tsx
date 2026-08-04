import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { MajorHardwareMTApi } from "../../../../Business/LookUp/MajorHardwareMTBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import {
  MajorHardwareMTDto,
  MajorHardwareMTEdit,
} from "../../../../Model/LookUp/MajorHardwareMT";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";

export async function GetMajorHardwareMTEditResource(id: number) {
  setLoader("ADD", "GetMajorHardwareMTEditResource");

  let api = new MajorHardwareMTApi();
  let createResource = await ApiCallWithErrorHandling<
    Promise<MajorHardwareMTDto>
  >(() => api.MajorHardwareMTGetUpdatedPage(id));
  let rtn = { LookUpDtoEdit: createResource } as MajorHardwareMTEdit;
  rootStore.dispatch({ type: "GET_EDIT_MAJORHARDWAREMT", payload: rtn });
  setLoader("REMOVE", "GetMajorHardwareMTEditResource");

  return rtn;
}

export async function EditMajorHardwareMT(data: MajorHardwareMTDto) {
  setLoader("ADD", "EditMajorHardwareMT");
  let api = new MajorHardwareMTApi();

  const payload = {
    majorHardwareBuildAsisId: data.majorHardwareBuildAsisId,
    hardwareSolution: data.hardwareSolution,
    hardwareType: data.hardwareType,
    buildConstructionId: data.buildConstructionId,
    orgEqpManuFacturerId: data.orgEqpManuFacturerId,
    platformId: data.platformId,
  };

  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.MajorHardwareMTUpdate(payload)
  );
  let rtn = { ResultDtoEdit: result } as MajorHardwareMTEdit;
  rootStore.dispatch(
    setNotification({
      message: result?.info ?? "",
      notifyType: result?.warning ? NotifyType.error : NotifyType.success,
    })
  );
  rootStore.dispatch({ type: "EDIT_MAJORHARDWAREMT", payload: rtn });
  setLoader("REMOVE", "EditMajorHardwareMT");
  return rtn;
}
