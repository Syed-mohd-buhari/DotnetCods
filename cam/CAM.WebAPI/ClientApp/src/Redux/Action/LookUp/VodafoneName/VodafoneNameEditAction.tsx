import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { VodafoneNameApi } from "../../../../Business/LookUp/VodafoneNameBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import {
  LookUpEdit,
  TipologicaGridDto,
} from "../../../../Model/LookUp/LookUpGenericModel";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";

export async function GetVodafoneNameEditResource(id: number) {
  setLoader("ADD", "GetVodafoneNameEditResource");

  let api = new VodafoneNameApi();
  let createResource = await ApiCallWithErrorHandling<
    Promise<TipologicaGridDto>
  >(() => api.vodafoneNameGetUpdateResourceVodafoneName(id));
  let rtn = { LookUpDtoEdit: createResource } as LookUpEdit;
  rootStore.dispatch({ type: "GET_EDIT_VODAFONE_NAME", payload: rtn });
  setLoader("REMOVE", "GetVodafoneNameEditResource");

  return rtn;
}

export async function EditVodafoneName(data: TipologicaGridDto) {
  setLoader("ADD", "EditVodafoneName");
  let api = new VodafoneNameApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.vodafoneNamePut(data)
  );
  let rtn = { ResultDtoEdit: result } as TipologicaGridDto;
  rootStore.dispatch(
    setNotification({
      message: result?.info ?? "",
      notifyType: result?.warning ? NotifyType.error : NotifyType.success,
    })
  );
  rootStore.dispatch({ type: "EDIT_VODAFONE_NAME", payload: rtn });
  setLoader("REMOVE", "EditVodafoneName");
  return rtn;
}
