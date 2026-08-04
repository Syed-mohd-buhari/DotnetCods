import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { CNFClusterApi } from "../../../../Business/LookUp/CNFClusterBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'
import {
  CNFClusterCreate,
  CNFClusterDto,
} from "../../../../Model/LookUp/CNFCluster";

export async function GetCNFClusterCreateResource() {
  setLoader("ADD", "GetCNFClusterCreateResource");

  let api = new CNFClusterApi();
  let createResource = await ApiCallWithErrorHandling<Promise<CNFClusterDto>>(
    () => api.CNFClusterGetCreatepage()
  );
  let rtn = {
    ResultDtoCreate: null,
    LookUpDtoCreate: createResource,
  } as CNFClusterCreate;
  rootStore.dispatch({ type: "GET_CREATE_CNFCLUSTER", payload: rtn });
  setLoader("REMOVE", "GetCNFClusterCreateResource");
}

export async function CreatCNFCluster(data: CNFClusterDto) {
  setLoader("ADD", "CreatCNFCluster");
  let api = new CNFClusterApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.CNFClusterCreate(data)
  );
  let rtn = {
    ResultDtoCreate: result,
    LookUpDtoCreate: null,
  } as CNFClusterCreate;
  rootStore.dispatch(
    setNotification({
      message: result?.info ?? "",
      notifyType: result?.warning ? NotifyType.error : NotifyType.success,
    })
  );
  rootStore.dispatch({ type: "CREATE_CNFCLUSTER", payload: rtn });
  setLoader("REMOVE", "CreatCNFCluster");
  return rtn;
}
