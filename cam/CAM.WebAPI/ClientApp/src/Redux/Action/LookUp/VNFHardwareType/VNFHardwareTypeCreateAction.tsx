import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { VNFHardwareTypeApi } from "../../../../Business/LookUp/VNFHardwareTypeBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'
import {
  VNFHardwareTypeCreate,
  VNFHardwareTypeDto,
} from "../../../../Model/LookUp/VNFHardwareType";

export async function GetVNFHardwareTypeCreateResource() {
  setLoader("ADD", "GetVNFHardwareTypeCreateResource");

  let api = new VNFHardwareTypeApi();
  let createResource = await ApiCallWithErrorHandling<
    Promise<VNFHardwareTypeDto>
  >(() => api.VNFHardwareTypeGetCreatepage());
  let rtn = {
    ResultDtoCreate: null,
    LookUpDtoCreate: createResource,
  } as VNFHardwareTypeCreate;
  rootStore.dispatch({ type: "GET_CREATE_VNFHARDWARETYPE", payload: rtn });
  setLoader("REMOVE", "GetVNFHardwareTypeCreateResource");
}

export async function CreatVNFHardwareType(data: VNFHardwareTypeDto) {
  setLoader("ADD", "CreatVNFHardwareVMType");
  let api = new VNFHardwareTypeApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.VNFHardwareTypeCreate(data)
  );
  let rtn = {
    ResultDtoCreate: result,
    LookUpDtoCreate: null,
  } as VNFHardwareTypeCreate;
  rootStore.dispatch(
    setNotification({
      message: result?.info ?? "",
      notifyType: result?.warning ? NotifyType.error : NotifyType.success,
    })
  );
  rootStore.dispatch({ type: "CREATE_VNFHARDWARETYPE", payload: rtn });
  setLoader("REMOVE", "CreatVNFHardwareType");
  return rtn;
}
