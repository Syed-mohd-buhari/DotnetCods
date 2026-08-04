import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { ComponentManufacturerApi } from "../../../../Business/LookUp/ComponentManufactureBuisness";
import { ResultDto } from "../../../../Model/CommonModels";
import {
  LookUpCreate,
  TipologicaGridDto,
} from "../../../../Model/LookUp/LookUpGenericModel";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";

export async function GetComponentManufacturerCreateResource( data: TipologicaGridDto) {
  setLoader("ADD", "GetComponentManufacturerCreateResource");

  let api = new ComponentManufacturerApi();
  let createResource = await ApiCallWithErrorHandling<
    Promise<TipologicaGridDto>
  >(() =>
    api.componentManufacturerGetCreateResourceOriginalEquipmentManufacturer(data)
  );
  let rtn = {
    ResultDtoCreate: null,
    LookUpDtoCreate: createResource,
  } as LookUpCreate;
  rootStore.dispatch({
    type: "GET_CREATE_COMPONENT_MANUFACTURER",
    payload: rtn,
  });
  setLoader("REMOVE", "GetComponentManufacturerCreateResource");
}

export async function CreatComponentManufacturer(
  data: TipologicaGridDto
) {
  setLoader("ADD", "CreatComponentManufacturer");

  const api = new ComponentManufacturerApi();
  let result: ResultDto | null |undefined;

  try {
    result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
      api.componentManufacturerCreate(data)
    );

    const rtn = {
      ResultDtoCreate: result,
      LookUpDtoCreate: null,
    } as LookUpCreate;

    rootStore.dispatch(
      setNotification({
        message: result?.info ?? "Created successfully",
        notifyType: result?.warning ? NotifyType.error : NotifyType.success,
      })
    );

    rootStore.dispatch({
      type: "CREATE_COMPONENT_MANUFACTURER",
      payload: rtn,
    });

    return rtn;

  } catch (error: any) {
    console.error("❌ Error occurred while saving OEM:", error);

    // If it's a fetch Response, parse the error body
    if (error instanceof Response) {
      try {
        const errJson = await error.json();
        console.error("🔍 Error response JSON:", errJson);
      } catch {
        const errText = await error.text();
        console.error("📝 Error response text:", errText);
      }
    }

    rootStore.dispatch(
      setNotification({
        message: "Save failed. Please check required fields or console for details.",
        notifyType: NotifyType.error,
      })
    );

    return {
      ResultDtoCreate: null,
      LookUpDtoCreate: null,
    } as LookUpCreate;
  } finally {
    setLoader("REMOVE", "CreatComponentManufacturer");
  }
}

