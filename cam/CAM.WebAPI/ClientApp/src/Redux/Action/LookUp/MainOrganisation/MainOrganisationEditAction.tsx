import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { MainOrganisationApi } from "../../../../Business/LookUp/MainOrganisationBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import {
  LookUpEdit,
  TipologicaGridDto,
} from "../../../../Model/LookUp/LookUpGenericModel";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";

export async function GetMainOrganisationEditResource(id: number) {
  setLoader("ADD", "GetMainOrganisationEditResource");

  let api = new MainOrganisationApi();
  let createResource = await ApiCallWithErrorHandling<
    Promise<TipologicaGridDto>
  >(() => api.mainOrganisationGetUpdateResourceMainOrganisation(id));
  let rtn = { LookUpDtoEdit: createResource } as LookUpEdit;
  rootStore.dispatch({ type: "GET_EDIT_MAIN_ORGANISATION", payload: rtn });
  setLoader("REMOVE", "GetMainOrganisationEditResource");

  return rtn;
}

export async function EditMainOrganisation(data: TipologicaGridDto) {
  setLoader("ADD", "EditMainOrganisation");
  let api = new MainOrganisationApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.mainOrganisationPut(data)
  );
  let rtn = { ResultDtoEdit: result } as TipologicaGridDto;
  rootStore.dispatch(
    setNotification({
      message: result?.info ?? "",
      notifyType: result?.warning ? NotifyType.error : NotifyType.success,
    })
  );
  rootStore.dispatch({ type: "EDIT_MAIN_ORGANISATION", payload: rtn });
  setLoader("REMOVE", "EditMainOrganisation");
  return rtn;
}
