import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { VNFHardwareTypeApi } from "../../../../Business/LookUp/VNFHardwareTypeBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import {
  VNFHardwareTypeDto,
  VNFHardwareTypeDtoGrid,
  VNFHardwareTypeEdit,
} from "../../../../Model/LookUp/VNFHardwareType";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";

export async function GetVNFHardwareTypeEditResource(id: number) {
  setLoader("ADD", "GetVNFHardwareTypeEditResource");

  let api = new VNFHardwareTypeApi();
  let createResource = await ApiCallWithErrorHandling<
    Promise<VNFHardwareTypeDto>
  >(() => api.VNFHardwareTypeGetUpdatedPage(id));
  let rtn = { LookUpDtoEdit: createResource } as VNFHardwareTypeEdit;
  rootStore.dispatch({ type: "GET_EDIT_VNFHARDWARETYPE", payload: rtn });
  setLoader("REMOVE", "GetVNFHardwareTypeTypeEditResource");

  return rtn;
}

export async function EditVNFHardwareType(data: VNFHardwareTypeDto) {
  setLoader("ADD", "EditVNFHardwareType");
  let api = new VNFHardwareTypeApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.VNFHardwareTypeUpdate(data)
  );
  let rtn = { ResultDtoEdit: result } as VNFHardwareTypeEdit;
  rootStore.dispatch(
    setNotification({
      message: result?.info ?? "",
      notifyType: result?.warning ? NotifyType.error : NotifyType.success,
    })
  );
  rootStore.dispatch({ type: "EDIT_VNFHARDWARETYPE", payload: rtn });
  setLoader("REMOVE", "EditVNFHardwareType");
  return rtn;
}
