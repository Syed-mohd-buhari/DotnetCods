import React from "react";
import setLoader from "../LoaderAction";
import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import { TSRReportApi } from "../../../Business/TSRReportBusiness";
import { rootStore } from "../../Store/rootStore";
import {
  LookUpEdit,
  TipologicaGridDto,
} from "../../../Model/LookUp/LookUpGenericModel";
import { ResultDto } from "../../../Model/CommonModels";
import { setNotification } from "../NotificationAction";
import { NotifyType } from "../../Reducer/NotificationReducer";

export async function GetTSRVerticalEditResource(id: number) {
  setLoader("ADD", "GetTSRVerticalEditResource");

  let api = new TSRReportApi();
  let createResource = await ApiCallWithErrorHandling<
    Promise<TipologicaGridDto>
  >(() => api.TSRVerticalGetUpdateResource(id));
  let rtn = { LookUpDtoEdit: createResource } as LookUpEdit;
  rootStore.dispatch({ type: "GET_EDIT_TSR_VERTICAL", payload: rtn });
  setLoader("REMOVE", "GetTSRVerticalEditResource");

  return rtn;
}

export async function EditTSRVertical(data: TipologicaGridDto) {
  setLoader("ADD", "EditTSRVertical");
  let api = new TSRReportApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.TSRVerticalUpdateResource(data)
  );
  let rtn = { ResultDtoEdit: result } as TipologicaGridDto;
  rootStore.dispatch(
    setNotification({
      message: result?.info ?? "",
      notifyType: result?.warning ? NotifyType.error : NotifyType.success,
    })
  );
  rootStore.dispatch({ type: "EDIT_TSR_VERTICAL", payload: rtn });
  setLoader("REMOVE", "EditTSRVertical");
  return rtn;
}
