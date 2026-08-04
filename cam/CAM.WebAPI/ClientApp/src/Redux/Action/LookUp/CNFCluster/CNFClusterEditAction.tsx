import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { CNFClusterApi } from "../../../../Business/LookUp/CNFClusterBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import {
  CNFClusterDto,
  CNFClusterDtoGrid,
  CNFClusterEdit,
} from "../../../../Model/LookUp/CNFCluster";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";

export async function GetCNFClusterEditResource(id: number) {
  setLoader("ADD", "GetCNFClusterEditResource");

  let api = new CNFClusterApi();
  let createResource = await ApiCallWithErrorHandling<Promise<CNFClusterDto>>(
    () => api.CNFClusterGetUpdatedPage(id)
  );
  let rtn = { LookUpDtoEdit: createResource } as CNFClusterEdit;
  rootStore.dispatch({ type: "GET_EDIT_CNFCLUSTER", payload: rtn });
  setLoader("REMOVE", "GetCNFClusterEditResource");

  return rtn;
}

export async function EditCNFCluster(data: CNFClusterDto) {
  setLoader("ADD", "EditCNFCluster");
  let api = new CNFClusterApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.CNFClusterUpdate(data)
  );
  let rtn = { ResultDtoEdit: result } as CNFClusterEdit;
  rootStore.dispatch(
    setNotification({
      message: result?.info ?? "",
      notifyType: result?.warning ? NotifyType.error : NotifyType.success,
    })
  );
  rootStore.dispatch({ type: "EDIT_CNFCLUSTER", payload: rtn });
  setLoader("REMOVE", "EditCNFCluster");
  return rtn;
}
