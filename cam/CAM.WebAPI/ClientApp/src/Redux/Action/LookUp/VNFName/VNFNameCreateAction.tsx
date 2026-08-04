import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { VNFNameApi } from "../../../../Business/LookUp/VNFNameBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'
import { VNFNameCreate, VNFNameDto } from "../../../../Model/LookUp/VNFName";

export async function GetVNFNameCreateResource() {
  setLoader("ADD", "GetVNFNameCreateResource");

  let api = new VNFNameApi();
  let createResource = await ApiCallWithErrorHandling<Promise<VNFNameDto>>(() =>
    api.VNFNameGetCreatepage()
  );
  let rtn = {
    ResultDtoCreate: null,
    LookUpDtoCreate: createResource,
  } as VNFNameCreate;
  rootStore.dispatch({ type: "GET_CREATE_VNFNAME", payload: rtn });
  setLoader("REMOVE", "GetVNFNameCreateResource");
}

export async function CreatVNFName(data: VNFNameDto) {
  setLoader("ADD", "CreatVNFName");
  let api = new VNFNameApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.VNFNameCreate(data)
  );
  let rtn = {
    ResultDtoCreate: result,
    LookUpDtoCreate: null,
  } as VNFNameCreate;
  rootStore.dispatch(
    setNotification({
      message: result?.info ?? "",
      notifyType: result?.warning ? NotifyType.error : NotifyType.success,
    })
  );
  rootStore.dispatch({ type: "CREATE_VNFNAME", payload: rtn });
  setLoader("REMOVE", "CreatVNFname");
  return rtn;
}
