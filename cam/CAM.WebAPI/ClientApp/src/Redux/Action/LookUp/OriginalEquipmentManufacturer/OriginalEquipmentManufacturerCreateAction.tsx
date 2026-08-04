import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { OriginalEquipmentManufacturerApi } from "../../../../Business/LookUp/OriginalEquipmentManufacturerBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import {
  LookUpCreate,
  TipologicaGridDto,
} from "../../../../Model/LookUp/LookUpGenericModel";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";

export async function GetOriginalEquipmentManufacturerCreateResource() {
  setLoader("ADD", "GetOriginalEquipmentManufacturerCreateResource");

  let api = new OriginalEquipmentManufacturerApi();
  let createResource = await ApiCallWithErrorHandling<
    Promise<TipologicaGridDto>
  >(() =>
    api.originalEquipmentManufacturerGetCreateResourceOriginalEquipmentManufacturer()
  );
  let rtn = {
    ResultDtoCreate: null,
    LookUpDtoCreate: createResource,
  } as LookUpCreate;
  rootStore.dispatch({
    type: "GET_CREATE_ORIGINAL_EQUIPMENT_MANUFACTURER",
    payload: rtn,
  });
  setLoader("REMOVE", "GetOriginalEquipmentManufacturerCreateResource");
}

export async function CreatOriginalEquipmentManufacturer(
  data: TipologicaGridDto
) {
  setLoader("ADD", "CreatOriginalEquipmentManufacturer");
  let api = new OriginalEquipmentManufacturerApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.originalEquipmentManufacturerCreate(data)
  );
  let rtn = { ResultDtoCreate: result, LookUpDtoCreate: null } as LookUpCreate;
  rootStore.dispatch(
    setNotification({
      message: result?.info ?? "",
      notifyType: result?.warning ? NotifyType.error : NotifyType.success,
    })
  );
  rootStore.dispatch({
    type: "CREATE_ORIGINAL_EQUIPMENT_MANUFACTURER",
    payload: rtn,
  });
  setLoader("REMOVE", "CreatOriginalEquipmentManufacturer");
  return rtn;
}
