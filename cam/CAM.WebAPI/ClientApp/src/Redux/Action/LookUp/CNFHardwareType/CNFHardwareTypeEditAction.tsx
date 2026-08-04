import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { CNFHardwareTypeApi } from "../../../../Business/LookUp/CNFHardwareTypeBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import {
  CNFHardwareTypeDto,
  CNFHardwareTypeDtoGrid,
  CNFHardwareTypeEdit,
} from "../../../../Model/LookUp/CNFHardwareType";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";

export async function GetCNFHardwareTypeEditResource(id: number) {
  setLoader("ADD", "GetCNFHardwareTypeEditResource");

  let api = new CNFHardwareTypeApi();
  let createResource = await ApiCallWithErrorHandling<
    Promise<CNFHardwareTypeDto>
  >(() => api.CNFHardwareTypeGetUpdatedPage(id));
  let rtn = { LookUpDtoEdit: createResource } as CNFHardwareTypeEdit;
  rootStore.dispatch({ type: "GET_EDIT_CNFHARDWARETYPE", payload: rtn });
  setLoader("REMOVE", "GetCnfHardwareTypeEditResource");

  return rtn;
}

export async function EditCNFHardwareType(data: CNFHardwareTypeDto) {
  setLoader("ADD", "EditCNFHardwareType");
  let api = new CNFHardwareTypeApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.CNFHardwareTypeUpdate(data)
  );
  let rtn = { ResultDtoEdit: result } as CNFHardwareTypeEdit;
  rootStore.dispatch(
    setNotification({
      message: result?.info ?? "",
      notifyType: result?.warning ? NotifyType.error : NotifyType.success,
    })
  );
  rootStore.dispatch({ type: "EDIT_CNFHARDWARETYPE", payload: rtn });
  setLoader("REMOVE", "EditCNFHardwareType");
  return rtn;
}
