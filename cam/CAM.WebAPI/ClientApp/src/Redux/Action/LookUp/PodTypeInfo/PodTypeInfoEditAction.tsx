import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { PodTypeInfoApi } from "../../../../Business/LookUp/PodTypeInfoBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import {
  PodTypeInfoDto,
  PodTypeInfoDtoGrid,
  PodTypeInfoEdit,
} from "../../../../Model/LookUp/PodTypeInfo";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";

export async function GetPodTypeInfoEditResource(id: number) {
  setLoader("ADD", "GetPodTypeInfoEditResource");

  let api = new PodTypeInfoApi();
  let createResource = await ApiCallWithErrorHandling<Promise<PodTypeInfoDto>>(
    () => api.PodTypeInfoGetUpdatedPage(id)
  );
  let rtn = { LookUpDtoEdit: createResource } as PodTypeInfoEdit;
  rootStore.dispatch({ type: "GET_EDIT_PODTYPEINFO", payload: rtn });
  setLoader("REMOVE", "GetPodTypeInfoEditResource");

  return rtn;
}

export async function EditPodTypeInfo(data: PodTypeInfoDto) {
  setLoader("ADD", "EditPodTypeInfo");
  let api = new PodTypeInfoApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.PodTypeInfoUpdate(data)
  );
  let rtn = { ResultDtoEdit: result } as PodTypeInfoEdit;
  rootStore.dispatch(
    setNotification({
      message: result?.info ?? "",
      notifyType: result?.warning ? NotifyType.error : NotifyType.success,
    })
  );
  rootStore.dispatch({ type: "EDIT_PODTYPEINFO", payload: rtn });
  setLoader("REMOVE", "EditPodTypeInfo");
  return rtn;
}
