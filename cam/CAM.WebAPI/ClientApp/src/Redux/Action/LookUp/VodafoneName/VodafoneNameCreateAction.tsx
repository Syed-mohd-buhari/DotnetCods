import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { VodafoneNameApi } from "../../../../Business/LookUp/VodafoneNameBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import {
  LookUpCreate,
  TipologicaGridDto,
} from "../../../../Model/LookUp/LookUpGenericModel";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function GetVodafoneNameCreateResource() {
  setLoader("ADD", "GetVodafoneNameCreateResource");

  let api = new VodafoneNameApi();
  let createResource = await ApiCallWithErrorHandling<
    Promise<TipologicaGridDto>
  >(() => api.vodafoneNameGetCreateResourceVodafoneName());
  let rtn = {
    ResultDtoCreate: null,
    LookUpDtoCreate: createResource,
  } as LookUpCreate;
  rootStore.dispatch({ type: "GET_CREATE_VODAFONE_NAME", payload: rtn });
  setLoader("REMOVE", "GetVodafoneNameCreateResource");
}

export async function CreatVodafoneName(data: TipologicaGridDto) {
  setLoader("ADD", "CreatVodafoneName");
  let api = new VodafoneNameApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.vodafoneNameCreate(data)
  );
  let rtn = { ResultDtoCreate: result, LookUpDtoCreate: null } as LookUpCreate;
  rootStore.dispatch(
    setNotification({
      message: result?.info ?? "",
      notifyType: result?.warning ? NotifyType.error : NotifyType.success,
    })
  );
  rootStore.dispatch({ type: "CREATE_VODAFONE_NAME", payload: rtn });
  setLoader("REMOVE", "CreatVodafoneName");
  return rtn;
}
