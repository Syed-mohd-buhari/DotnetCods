import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { CNFNameApi } from "../../../../Business/LookUp/CNFNameBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'
import { CNFNameCreate, CNFNameDto } from "../../../../Model/LookUp/CNFName";

export async function GetCNFNameCreateResource() {
  setLoader("ADD", "GetCNFNameCreateResource");

  let api = new CNFNameApi();
  let createResource = await ApiCallWithErrorHandling<Promise<CNFNameDto>>(() =>
    api.CNFNameGetCreatepage()
  );
  let rtn = {
    ResultDtoCreate: null,
    LookUpDtoCreate: createResource,
  } as CNFNameCreate;
  rootStore.dispatch({ type: "GET_CREATE_CNFNAME", payload: rtn });
  setLoader("REMOVE", "GetCNFNameCreateResource");
}

export async function CreatCNFName(data: CNFNameDto) {
  setLoader("ADD", "CreatCNFName");
  let api = new CNFNameApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.CNFNameCreate(data)
  );
  let rtn = {
    ResultDtoCreate: result,
    LookUpDtoCreate: null,
  } as CNFNameCreate;
  rootStore.dispatch(
    setNotification({
      message: result?.info ?? "",
      notifyType: result?.warning ? NotifyType.error : NotifyType.success,
    })
  );
  rootStore.dispatch({ type: "CREATE_CNFNAME", payload: rtn });
  setLoader("REMOVE", "CreatCNFName");
  return rtn;
}
