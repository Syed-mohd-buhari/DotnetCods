import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { CNFHardwareTypeApi } from "../../../../Business/LookUp/CNFHardwareTypeBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'
import {
  CNFHardwareTypeCreate,
  CNFHardwareTypeDto,
} from "../../../../Model/LookUp/CNFHardwareType";

export async function GetCNFHardwareTypeCreateResource() {
  setLoader("ADD", "GetCNFHardwareTypeCreateResource");

  let api = new CNFHardwareTypeApi();
  let createResource = await ApiCallWithErrorHandling<
    Promise<CNFHardwareTypeDto>
  >(() => api.CNFHardwareTypeGetCreatepage());
  let rtn = {
    ResultDtoCreate: null,
    LookUpDtoCreate: createResource,
  } as CNFHardwareTypeCreate;
  rootStore.dispatch({ type: "GET_CREATE_CNFHARDWARETYPE", payload: rtn });
  setLoader("REMOVE", "GetCNFHardwareTypeCreateResource");
}

export async function CreatCNFHardwareType(data: CNFHardwareTypeDto) {
  setLoader("ADD", "CreatCNFHardwareType");
  let api = new CNFHardwareTypeApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.CNFHardwareTypeCreate(data)
  );
  let rtn = {
    ResultDtoCreate: result,
    LookUpDtoCreate: null,
  } as CNFHardwareTypeCreate;
  rootStore.dispatch(
    setNotification({
      message: result?.info ?? "",
      notifyType: result?.warning ? NotifyType.error : NotifyType.success,
    })
  );
  rootStore.dispatch({ type: "CREATE_CNFHARDWARETYPE", payload: rtn });
  setLoader("REMOVE", "CreatCNFHardwareType");
  return rtn;
}
